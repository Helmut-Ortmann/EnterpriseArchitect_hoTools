using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Windows.Forms;
using System.Xml.Linq;
using EAAddinFramework.Utils;
using hoTools.Utils.Configuration;
using hoTools.Utils.SQL;
using Microsoft.Win32;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace AddinFramework.Util
{
    public class Search
    {
        static List<SearchItem> _staticSearches;
        static AutoCompleteStringCollection _staticSearchesSuggestions;
        public static EA.Repository Rep;
        // configuration as singleton
        static readonly HoToolsGlobalCfg _globalCfg = HoToolsGlobalCfg.Instance;

        /// <summary>
        /// Loads all findable Searches. These Searches are stored outside the model and cannot be changed by the user.
        /// Dynamic added Searches aren't currently detectable (ok, by brute force). Possible search sources are:
        /// <para /> Build in: No chance.
        /// <para />
        /// All Technologies:
        /// <para />
        /// Technology folder inside installation
        /// <para />
        /// EA_Search.xml
        /// </summary>


        // ReSharper disable once EmptyConstructor
        static Search()
        {
            //LoadStaticSearches();
        }
        /// <summary>
        /// Get the list of searches (EA + SQL)
        /// </summary>
        public static List<SearchItem> Searches
        {
            get
            {
                if (_staticSearches == null)
                {
                    LoadStaticSearches();
                }
                return _staticSearches;
            }
            set
            {
                _staticSearches = value;
            }
        }

        public static AutoCompleteStringCollection SearchesSuggestions
        {
            get
            {
                if (_staticSearches == null)
                {
                    LoadStaticSearches();
                    LoadStaticSearchesSuggestions();
                }
                return _staticSearchesSuggestions;
            }
            
        }
        /// <summary>
        /// Load the possible Searches
        /// </summary>
        static void LoadStaticSearches()
        {
            _staticSearches = new List<SearchItem>();
            LoadEaStandardSearchesFromJason();

            LoadSqlSearches();

            //local scripts
            LoadLocalSearches();
            //MDG scripts in the program folder
            LoadLocalMdgSearches();
            // MDG scripts in other locations
            LoadOtherMdgSearches();
            // order
            _staticSearches = _staticSearches.OrderBy(a => a.Name)
                .ToList();
            LoadStaticSearchesSuggestions();

        }
        /// <summary>
        /// Load the suggestions for the search Combo Box
        /// </summary>
        static void LoadStaticSearchesSuggestions()
        {
            _staticSearchesSuggestions = new AutoCompleteStringCollection();
            var result = _staticSearches.Select(e => e.Name).ToArray();
            _staticSearchesSuggestions.AddRange(result);
            


        } 

        /// <summary>
        /// The local Searches are located in the "ea program files"\scripts (so usually C:\Program Files (x86)\Sparx Systems\EA\Scripts or C:\Program Files\Sparx Systems\EA\Scripts)
        /// The contents of the local scripts is loaded into the Searches.
        /// </summary>
        static void LoadLocalSearches()
        {
            
            string searchFolder = SqlError.GetEaSqlErrorPath() + @"\Search Data"; 
            LoadSearchFromFolder(searchFolder);

        }
        /// <summary>
        /// Load Searches from MDG Technology folder
        /// </summary>
        static void LoadLocalMdgSearches()
        {
            string searchFolder = Path.GetDirectoryName(Model.ApplicationFullPath) + "\\MDGTechnologies";
            LoadSearchFromFolder(searchFolder);
        }

        static void LoadSearchFromFolder(string folder)
        {
            string[] searchFiles = Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories);
            foreach (string searchFile in searchFiles)
            {
                LoadMdgSearches(File.ReadAllText(searchFile));
            }

        }
        /// <summary>
        /// loads the MDG scripts from the locations added from MDG Technologies|Advanced. 
        /// these locations are stored as a comma separated string in the registry
        /// a location can either be a directory, or an URL
        /// </summary>
        static void LoadOtherMdgSearches()
        {

                //read the registry key to find the locations
                var pathList = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Sparx Systems\EA400\EA\OPTIONS", "MDGTechnology PathList", null) as string;
                if (pathList != null)
                {
                    string[] mdgPaths = pathList.Split(',');
                    foreach (string mdgPath in mdgPaths)
                    {
                        if (mdgPath.Trim() == "") continue;
                        //figure out it we have a folder path or an URL
                        if (mdgPath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //URL
                            LoadMdgSearchFromUrl(mdgPath);
                        }
                        else
                        {
                            //directory
                            LoadSearchFromFolder(mdgPath);
                        }
                    }
                }

        }


        /// <summary>
        /// Loads the Searches described in the MDG file into the includable scripts
        /// </summary>
        /// <param name="mdgXmlContent">the string content of the MDG file</param>
        static void LoadMdgSearches(string mdgXmlContent)
        {
            try
            {
                var mdg  = XElement.Parse(mdgXmlContent);


                // get MDG data (ID, Name) 

                string id = "";
                //string name = "";
                //string notes = "";
                XElement documentation = mdg.Element("Documentation");
                // Not part of a MDG
                if (documentation != null)
                {
                    id = documentation.Attribute("id").Value;
                    //name = documentation.Attribute("name").Value;
                    //notes = documentation.Attribute("notes").Value;
                    // check if Technology is enabled
                    if (Rep.IsTechnologyEnabled(id) == false && Rep.IsTechnologyLoaded(id)) return;

                }

                //----------------------------------------
                // Get all searches
                var searches = from search in mdg.Descendants("Search")
                    select search;
                foreach (XElement search in searches)
                {
                    string searchName = search.Attribute("Name").Value;
                    _staticSearches.Add(new EaSearchItem(id, searchName));
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(@"", @"Error in loadMDGScripts: " + e.Message);
            }
        }
        /// <summary>
        /// load the MDG Search from the MDG file located at the given URL
        /// </summary>
        /// <param name="url">the URL pointing to the MDG file</param>
        static void LoadMdgSearchFromUrl(string url)
        {
            try
            {
                LoadMdgSearches(new WebClient().DownloadString(url));
            }
            catch (Exception e)
            {
                MessageBox.Show($"URL='{url}' skipped (see: Extensions, MDGTechnology,Advanced).\r\n{e.Message}",
                    @"Error in load *.xml MDGSearches from url! ");
            }
        }
        
        /// <summary>
        /// Get file names of SQL path.
        /// </summary>
        /// <returns></returns>
        public static void LoadSqlSearches()
        {
            foreach (string file in _globalCfg.getListFileCompleteName())
            {
                string name = Path.GetFileName(file);
                _staticSearches.Add( new SqlSearchItem(name, file)); 
            }
        }
        /// <summary>
        /// Load EA Standard Searches from JSON
        /// </summary>
        static void LoadEaStandardSearchesFromJason()
        {

            string jasonPath =
                @"D:\hoData\Development\GitHub\EnterpriseArchitect_hoTools\AddinFramework\Util\Search\EaStandardSearches.json";

            using (StreamReader sr = new StreamReader(path: jasonPath) )
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();
                _staticSearches.AddRange(serializer.Deserialize<List<SearchItem>>(reader));

            }
            


        }

    }

    public class Test
    {
        public Double Score { get; set; }
        public bool Favorite { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

        public Test(string name, string description)
        {
            Name = name;
            Description = description;
        }
        [JsonConstructor]
        public Test(string name)
        {
            Name = name;
        }

    }
}
