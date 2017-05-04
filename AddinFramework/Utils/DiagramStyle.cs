using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable once CheckNamespace
namespace EAAddinFramework.Utils
{
    /// <summary>
    /// Item to specify the style of an EA Diagram
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiagramStyleItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Pdata { get; set; }
        public string StyleEx { get; set; }

        [JsonConstructor]
        public DiagramStyleItem(string name, string description, string type, string pdata, string styleEx)
        {
            Name = name;
            Description = description;
            Type = type;
            Pdata = pdata;
            StyleEx = styleEx;
        }
    }
    /// <summary>
    /// Item to specify the style of an EA DiagramObject
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiagramObjectStyleItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }

        [JsonConstructor]
        public DiagramObjectStyleItem(string name, string description, string type, string style)
        {
            Name = name;
            Description = description;
            Type = type;
            Style = style;
        }
    }

    public class DiagramStyle
    {
        public List<DiagramStyleItem> DiagramStyleItems { get;  }
        public List<DiagramObjectStyleItem> DiagramObjectStyleItems { get; }

        public DiagramStyle(string jasonFilePath)
        {
            // use 'Deserializing Partial JSON Fragments'
            try
            {
                // Read JSON
                string text = System.IO.File.ReadAllText(jasonFilePath);
                JObject search = JObject.Parse(text);

                // Deserialize "DiagramStyle"
                // get JSON result objects into a list
                IList<JToken> results = search["DiagramStyle"].Children().ToList();
                // serialize JSON results into .NET objects
                IList<DiagramStyleItem> searchResults = new List<DiagramStyleItem>();
                foreach (JToken result in results)
                    {
                        // JToken.ToObject is a helper method that uses JsonSerializer internally
                        DiagramStyleItem searchResult = result.ToObject<DiagramStyleItem>();
                        if (searchResult == null) continue;
                        searchResults.Add(searchResult);
                    }
                DiagramStyleItems = searchResults.ToList<DiagramStyleItem>();

                // Deserialize "DiagramObjectStyle"
                // get JSON result objects into a list
                IList<JToken> diaObjects = search["DiagramObjectStyle"].Children().ToList();
                // serialize JSON results into .NET objects
                IList<DiagramObjectStyleItem> resultsDiaObject = new List<DiagramObjectStyleItem>();

                foreach (JToken diaObject in diaObjects)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    DiagramObjectStyleItem searchResult = diaObject.ToObject<DiagramObjectStyleItem>();
                    if (searchResult == null) continue;
                    resultsDiaObject.Add(searchResult);
                }
                DiagramObjectStyleItems = resultsDiaObject.ToList<DiagramObjectStyleItem>();



            }
            catch (Exception e)
            {
                MessageBox.Show($@"Try importing from '{jasonFilePath}'
{e}", "Can't import Diagram Styles");
            }

        }

        /// <summary>
        /// Create a ToolStripItem with DropDownitems for each DiagramStyle.
        /// The Tag property contains the style.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toolTip"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public ToolStripMenuItem GetToolStripMenuDiagramStyle(string name, string toolTip, EventHandler eventHandler)
        {
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem
            {
                Text = name,
                ToolTipText = toolTip
            };
            // Add item of possible style as items in drop down
            foreach (var style in DiagramStyleItems)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Text = style.Name,
                    ToolTipText = style.Description,
                    Tag = style
                };
                item.Click += eventHandler;
                insertTemplateMenuItem.DropDownItems.Add(item);
            }
            return insertTemplateMenuItem;

        }
        /// <summary>
        /// Create a ToolStripItem with DropDownitems for each DiagramStyle.
        /// The Tag property contains the style.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toolTip"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public ToolStripMenuItem GetToolStripMenuDiagramObjectStyle(string name, string toolTip, EventHandler eventHandler)
        {
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem
            {
                Text = name,
                ToolTipText = toolTip
            };
            // Add item of possible style as items in drop down
            foreach (var style in DiagramObjectStyleItems)
            {
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Text = style.Name,
                    ToolTipText = style.Description,
                    Tag = style
                };
                item.Click += eventHandler;
                insertTemplateMenuItem.DropDownItems.Add(item);
            }
            return insertTemplateMenuItem;

        }




        /// <summary>
        /// Set Diagram styles in PDATA and StyleEx. It simply updates the parameters in both field.
        /// 
        /// HideQuals=1 HideQualifiers: 
        /// OpParams=2  Show full Operation Parameter
        /// ScalePI=1   Scale to fit page
        /// Theme=:119  Set the diagram theme and the used features of the theme (here 119, see StyleEx of t_diagram)
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="par">par[0] contains the values as a semicolon/comma separated types</param>
        /// <param name="par">par[1] contains the possible diagram types</param>
        public static void SetDiagramStyle(EA.Repository rep, EA.Diagram dia, string[] par)
        {
            // Make '; as delimiter for types
            string styles = par[0].Replace(",", ";");
            string dStyles = par[1].Replace(",", ";");

            string[] styleEx = styles.Split(';');
            string diaStyle = dia.StyleEx;
            string diaExtendedStyle = dia.ExtendedStyle.Trim();
            if (!DiagramIsToChange(dia, dStyles)) return;

            // no distinguishing between StyleEx and ExtendedStayle, may cause of trouble
            if (dia.StyleEx == "") diaStyle = dStyles + ";";
            if (dia.ExtendedStyle == "") diaExtendedStyle = dStyles + ";";

            Regex rx = new Regex(@"([^=]*)=.*");
            rep.SaveDiagram(dia.DiagramID);
            foreach (string style in styleEx)
            {
                if (style.Trim() == "") continue;
                Match match = rx.Match(style.Trim());
                if (!match.Success) continue;
                string patternFind = $@"{match.Groups[1].Value}=[^;]*;";
                diaStyle = Regex.Replace(diaStyle, patternFind, $@"{style};");
                diaExtendedStyle = Regex.Replace(diaExtendedStyle, patternFind, $@"{style};");
            }
            // delete spaces to avoid sql exception (string to long) 
            dia.ExtendedStyle = diaExtendedStyle.Replace(";   ",";").Replace(";  ", ";").Replace("; ", ";").Trim();
            dia.StyleEx = diaStyle.Replace(";   ", ";").Replace(";  ", ";").Replace("; ", ";").Trim(); 
            dia.Update();
            rep.ReloadDiagram(dia.DiagramID);

        }

        /// <summary>
        /// Set DiagramObject style. 
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="diaObject"></param>
        /// <param name="style"></param>
        public static void SetDiagramObjectStyle(EA.Repository rep, EA.DiagramObject diaObject, string style)
        {
            diaObject.Style = style.Replace(",", ";").Replace("   ","").Replace("  ", "").Replace(" ", "").Trim();
            diaObject.Update();

        }
        /// <summary>
        /// Returns true if current diagramtype is to support.
        /// diagramTypes is a comma, semicolon separated list which contains the diagramtypes:
        /// 
        /// Type=Subtype
        /// Type=Diagram types according to Diagram_Type (only part of the name is required)
        /// Subtype=Only Custom Diagrams, Use Diagramtype in StyleEx like MDGDgm=Extended::Requirements
        ///   (only part of the name is required)
        /// Examples:
        /// 'Class'                Class diagram (no other diagram type contains 'class')
        /// 'Cl'                   Class diagram (no other diagram type contains 'cl')
        /// 'Custom=Requirements'  Custom Diagram of type Requirements
        /// '=Requirements'        Custom Diagram of type Requirements
        /// 
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="diagramTypes"></param>
        /// <returns></returns>
        private static bool DiagramIsToChange(EA.Diagram dia, string diagramTypes)
        {
            diagramTypes = diagramTypes.ToLower().Trim();
            // all diagrams are valid
            if (diagramTypes == "" || diagramTypes == "*") return true;
            string customDiagramType = GetCustomDiagramType(dia.StyleEx).ToLower();
            foreach (var t in diagramTypes.Split(';'))
            {
                string diagramType = t.Trim();
                if (diagramType == "") continue;
                string[] types = diagramType.Split('=');
                if (types.Length == 1)
                {
                    // Standard EA Diagram 
                    if (dia.Type.ToLower().Contains(types[0])) return true;
                    return false;
                }
                else
                {
                    // Custom Diagram
                    if (customDiagramType.Contains(types[1])) return true;
                    if (types[1] == "" || types[1] == "*") return true;
                    return false;
                }
            }

            return false;
        }
        /// <summary>
        /// Get the Custom DiagramType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetCustomDiagramType(string type)
        {

            Match m = Regex.Match(type, @"MDGDgm=([^;]+);");
            if (m.Success)
            {
                if (m.Groups.Count > 0) return m.Groups[1].Value;
                else return "";
            }
            else return "";
        }




    }
}

