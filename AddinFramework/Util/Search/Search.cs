using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using EAAddinFramework.Utils;
using hoTools.Utils.Configuration;
using hoTools.Utils.SQL;
using Microsoft.Win32;
using Newtonsoft.Json;

using DuoVia.FuzzyStrings;

// ReSharper disable once CheckNamespace
namespace AddinFramework.Util
{
    public class Search
    {
        static List<SearchItem> _staticSearches;
        private static List<string> _staticSearchesOrdered;
        static AutoCompleteStringCollection _staticSearchesSuggestions;

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
        public static List<SearchItem> LoadSearches(EA.Repository rep)
        {
            LoadStaticSearches(rep);
            return _staticSearches;

        }
        /// <summary>
        /// Get the list of searches (EA + SQL)
        /// </summary>
        public static List<SearchItem> GetSearches(EA.Repository rep)
        {
                if (_staticSearches == null)
                {
                    LoadStaticSearches(rep);
                }
                return _staticSearches;
            
        }
        /// <summary>
        /// Get the SearchItem for the index
        /// </summary>
        public static SearchItem GetSearche(int index)
        {
            
            return _staticSearches[index];

        }


        public static AutoCompleteStringCollection GetSearchesSuggestions(EA.Repository rep)
        {
                if (_staticSearches == null)
                {
                    LoadStaticSearches(rep);
                    LoadStaticSearchesSuggestions();
                }
                return _staticSearchesSuggestions;
            
        }
        /// <summary>
        /// Calculate score, sort and visualize rtf field. Cases are not considered.
        /// </summary>
        /// <param name="pattern"></param>
        public static void CalulateAndSort(string pattern)
        {
            pattern = pattern.ToLower();
            var l = new List<SearchItem>();

            foreach (var search in _staticSearches)
            {
                // Search for length of subsequence
                var score = pattern.LongestCommonSubsequence(search.Name.ToLower());
                l.Add(new SearchItem(score.Item1.Length, search.Name, search.Description, search.Category, search.Favorite));
                //var score = pattern.LevenshteinDistance(search.Name);
                //l.Add(new SearchItem(score, search.Name, search.Description, search.Category, search.Favorite));

            }
            // sort list
            _staticSearches = l.OrderByDescending(a => a.Score).ToList();

        }
        /// <summary>
        /// Reset sort in rtf by default order (Name field of SearchItem).
        /// </summary>
        public static void ResetSort()
        {
            // sort list
            _staticSearches = _staticSearches.OrderBy(a => a.Name).ToList();

        }


        /// <summary>
        /// Load the possible Searches
        /// </summary>
        static void LoadStaticSearches(EA.Repository rep)
        {
            _staticSearches = new List<SearchItem>();
            LoadEaStandardSearchesFromJason();

            LoadSqlSearches();

            //local scripts
            LoadLocalSearches(rep);
            //MDG scripts in the program folder
            LoadLocalMdgSearches(rep);
            // MDG scripts in other locations
            LoadOtherMdgSearches(rep);
            // order
            _staticSearches = _staticSearches.OrderBy(a => a.Name)
                .ToList();
            LoadStaticSearchesSuggestions();

        }
        /// <summary>
        /// Get the suggestions for the rtf box
        /// </summary>
        static public string GetRtf()
        {
            //var result = _staticSearches.Select(e => e.Name).ToArray();
            var s = _staticSearches.Select(e => $"{e.Score,2} {e.Category,-15} {e.Name}" ).ToList();
            return string.Join("\r\n",s);
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
        static void LoadLocalSearches(EA.Repository rep)
        {
            
            string searchFolder = SqlError.GetEaSqlErrorPath() + @"\Search Data"; 
            LoadSearchFromFolder(rep, searchFolder);

        }
        /// <summary>
        /// Load Searches from MDG Technology folder
        /// </summary>
        static void LoadLocalMdgSearches(EA.Repository rep)
        {
            string searchFolder = Path.GetDirectoryName(Model.ApplicationFullPath) + "\\MDGTechnologies";
            LoadSearchFromFolder(rep, searchFolder);
        }

        static void LoadSearchFromFolder(EA.Repository rep, string folder)
        {
            string[] searchFiles = Directory.GetFiles(folder, "*.xml", SearchOption.AllDirectories);
            foreach (string searchFile in searchFiles)
            {
                LoadMdgSearches(rep, File.ReadAllText(searchFile));
            }

        }
        /// <summary>
        /// loads the MDG scripts from the locations added from MDG Technologies|Advanced. 
        /// these locations are stored as a comma separated string in the registry
        /// a location can either be a directory, or an URL
        /// </summary>
        static void LoadOtherMdgSearches(EA.Repository rep)
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
                            LoadMdgSearchFromUrl(rep, mdgPath);
                        }
                        else
                        {
                            //directory
                            LoadSearchFromFolder(rep, mdgPath);
                        }
                    }
                }

        }


        /// <summary>
        /// Loads the Searches described in the MDG file into the includable scripts
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="mdgXmlContent">the string content of the MDG file</param>
        static void LoadMdgSearches(EA.Repository rep, string mdgXmlContent)
        {
            try
            {
                var mdg  = XElement.Parse(mdgXmlContent);


                // get MDG data (ID, Name) 

                string id = "My EA Searches";
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
                    if (rep.IsTechnologyEnabled(id) == false && rep.IsTechnologyLoaded(id)) return;

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
        /// <param name="rep"></param>
        /// <param name="url">the URL pointing to the MDG file</param>
        static void LoadMdgSearchFromUrl(EA.Repository rep, string url)
        {
            try
            {
                LoadMdgSearches(rep, new WebClient().DownloadString(url));
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

            string jasonPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"EaStandardSearches.json");

            using (StreamReader sr = new StreamReader(path: jasonPath) )
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();
                _staticSearches.AddRange(serializer.Deserialize<List<SearchItem>>(reader));

            }
            


        }

    }
    
}
