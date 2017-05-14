using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace hoTools.Utils.Diagram
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
        public string Advanced { get; set; }

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
        public string Style { get; set; }
        public string Property { get; set; }

        public string Type { get; set; }
       [JsonConstructor]
        public DiagramObjectStyleItem(string name, string description, string type, string style, string property)
        {
            Name = name;
            Description = description;
            Type = type;
            Style = style;
            Property = property;
        }
    }

    public class DiagramStyle
    {
        // Diagram Styles
        public List<DiagramStyleItem> DiagramStyleItems { get;  }
        // Diagram Object Styles
        public List<DiagramObjectStyleItem> DiagramObjectStyleItems { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jasonFilePath"></param>

        public DiagramStyle(string jasonFilePath)
        {
            // Set Object Diagram Property for a bool
            
            // use 'Deserializing Partial JSON Fragments'
            try
            {
                // Read JSON
                string text = File.ReadAllText(jasonFilePath);
                JObject search = JObject.Parse(text);

                //----------------------------------------------------------------------
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

                //----------------------------------------------------------------------
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
            foreach (DiagramObjectStyleItem style in DiagramObjectStyleItems)
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
        /// Set Ea Diagram Object Style according to properties passed in the form 'properyName=propertyValue'
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="diaObj"></param>
        /// <param name="style"></param>
        private static void SetDiagramObjectStyle(Repository rep, EA.DiagramObject diaObj, string style)
        {
            // extract name and value
            Match match = Regex.Match(style, $@"(\w+)=(\w+)");
            if (!match.Success)
            {
                MessageBox.Show($"PropertyStyle='{style}', should be 'Property=xxxx;'",
                    "Invalid Property DiagramObject Style");
                return;
            }
            if (match.Groups.Count != 3)
            {
                MessageBox.Show($"PropertyStyle='{style}', should be 'Property=xxxx;'",
                    "Invalid Property DiagramObject Style");
                return;
            }


            string propertyName = match.Groups[1].Value;
            string propertyValue = match.Groups[2].Value;

            var p = diaObj.GetType().GetProperty(propertyName);
            if (p == null)
            {
                MessageBox.Show($"PropertyStyle='{style}'",
                    $@"No valid DiagramObject Property '{propertyName}'");
                return;
            }
            string propertyTyp = p.PropertyType.Name;
            if (! (propertyTyp == "Boolean" ||
                propertyTyp == "String"  ||
                propertyTyp == "Int32"))        
            {
                //MessageBox.Show($"PropertyType '{propertyTyp}' not supported.\r\nOnly: Int32=long, Boolean, String\r\nPropertyStyle='{style}'",
                //    $@"No valid DiagramObject Property Type '{propertyName}:{propertyTyp}'");
                // Most things works well with Int32
                propertyTyp = "Int32";
                //return;
            }


 
            try
            {
                switch (propertyTyp)
                {
                        
                        case "Boolean":
                            bool boolProperty;
                            if (!bool.TryParse(propertyValue, out boolProperty))
                            {
                                MessageBox.Show($"Property='{propertyValue}', should be Bool",
                                    "Property DiagramObject Style isn't Bool");
                                return;
                            }
                            diaObj.ShowConstraints = true;

                        // set property of type Boolean by PropertyName. The Property Name is case insensitive (ignore case)
                        //object[] o = new object[] { boolProperty };
                        //diaObj.GetType().InvokeMember("ShowConstraints", BindingFlags.PutRefDispProperty | BindingFlags.IgnoreCase, null, diaObj, o);

                            switch (propertyName)
                            {
                                case "FontBold":
                                    diaObj.FontBold = boolProperty;
                                    break;
                                case "FontItalic":
                                    diaObj.FontItalic = boolProperty;
                                    break;
                                case "FontUnderline":
                                    diaObj.FontUnderline = boolProperty;
                                    break;
                                case "IsSelectable":
                                    diaObj.IsSelectable = boolProperty;
                                    break;
                                case "ShowConstraints":
                                    diaObj.ShowConstraints = boolProperty;
                                    break;
                                case "ShowFormattedNotes":
                                    diaObj.ShowFormattedNotes = boolProperty;
                                    break;
                                case "ShowFullyQualifiedTags":
                                    diaObj.ShowFullyQualifiedTags = boolProperty;
                                    break;
                                case "ShowInheritedAttributes":
                                    diaObj.ShowInheritedAttributes = boolProperty;
                                    break;
                                case "ShowInheritedConstraints":
                                    diaObj.ShowInheritedConstraints = boolProperty;
                                    break;
                                case "ShowInheritedOperations":
                                    diaObj.ShowInheritedOperations = boolProperty;
                                    break;
                                case "ShowInheritedResponsibilities":
                                    diaObj.ShowInheritedResponsibilities = boolProperty;
                                    break;
                                case "ShowInheritedTags":
                                    diaObj.ShowInheritedTags = boolProperty;
                                    break;
                                case "ShowPortType":
                                    diaObj.ShowPortType = boolProperty;
                                    break;
                                case "ShowPrivateAttributes":
                                    diaObj.ShowPrivateAttributes = boolProperty;
                                    break;
                                case "ShowPublicAttributes":
                                    diaObj.ShowPublicAttributes = boolProperty;
                                    break;
                                case "ShowProtectedAttributes":
                                    diaObj.ShowProtectedAttributes = boolProperty;
                                    break;
                                case "ShowPrivateOperations":
                                    diaObj.ShowPrivateOperations = boolProperty;
                                    break;
                                case "ShowPublicOperations":
                                    diaObj.ShowPublicOperations = boolProperty;
                                    break;
                                case "ShowProtectedOperations":
                                    diaObj.ShowProtectedOperations = boolProperty;
                                    break;
                                case "ShowPackageOperations":
                                    diaObj.ShowPackageOperations = boolProperty;
                                    break;
                                case "ShowPackageAttributes":
                                    diaObj.ShowPackageAttributes = boolProperty;
                                    break;
                                case "ShowResponsibilities":
                                    diaObj.ShowResponsibilities = boolProperty;
                                    break;
                                case "ShowRunstates":
                                    diaObj.ShowRunstates = boolProperty;
                                    break;
                                case "ShowStructuredCompartments":
                                    diaObj.ShowStructuredCompartments = boolProperty;
                                    break;
                                case "ShowTags":
                                    diaObj.ShowStructuredCompartments = boolProperty;
                                    break;
                            default:
                                MessageBox.Show($@"Style={style}'\r\nProperty:'{propertyName}'", $@"Invalid DiagramObjectStyle '{propertyName}'");
                                break;
                        }

                        break;

                        case "Int32":
                            int int32Property;
                            if (!Int32.TryParse(propertyValue, out int32Property))
                            {
                                MessageBox.Show($"Property='{propertyValue}', should be Integer (int32, long)",
                                    "Property DiagramObject Style isn't Integer");
                                return;
                            }
                        diaObj.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.SetProperty, null, diaObj, new object[] { int32Property });
                        break;

                        case "String":
                            diaObj.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.SetProperty, null, diaObj, new object[] {propertyValue});
                        break;
                }
               
                
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Property: '{style}'\r\n{e}", $@"Error from EA DiagramObject API '{propertyName}:{propertyTyp}");
            }

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
        public static void SetDiagramStyle(Repository rep, EA.Diagram dia, string[] par)
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

            // find: Name=value
            Regex rx = new Regex(@"([^=]*)=(.*)");
            rep.SaveDiagram(dia.DiagramID);
            foreach (string style in styleEx)
            {
                if (style.Trim() == "") continue;
                Match match = rx.Match(style.Trim());
                if (!match.Success) continue;
                string patternFind = $@"{match.Groups[1].Value}=[^;]*;";
                diaStyle = Regex.Replace(diaStyle, patternFind, $@"{style};");
                diaExtendedStyle = Regex.Replace(diaExtendedStyle, patternFind, $@"{style};");
                // advanced styles
                SetAdvancedStyle(rep, dia, match.Groups[1].Value, match.Groups[2].Value);
            }
            // delete spaces to avoid sql exception (string to long) 
            dia.ExtendedStyle = diaExtendedStyle.Replace(";   ",";").Replace(";  ", ";").Replace("; ", ";").Trim();
            dia.StyleEx = diaStyle.Replace(";   ", ";").Replace(";  ", ";").Replace("; ", ";").Trim(); 
            dia.Update();
            rep.ReloadDiagram(dia.DiagramID);

        }
        /// <summary>
        /// Handle styles like: Orientation=L/P, Scale=100 (100%)
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetAdvancedStyle(Repository rep, EA.Diagram dia, string name, string value)
        {
            switch (name.ToLower().Trim())
            {
                case "orientation":
                    dia.Orientation = value.Trim();
                    break;
                case "scale":
                    int scale;
                    if (Int32.TryParse(value.Trim(), out scale))
                    {
                        dia.Scale = scale;
                    }
                    else
                    {
                        MessageBox.Show( $"Invalid Diagram Style 'Scale={value};' in Settings.json");
                    }
                    break;
                case @"cx":
                    int cx;
                    if (Int32.TryParse(value.Trim(), out cx))
                    {
                        dia.cx = cx;
                    }
                    else
                    {
                        MessageBox.Show("Should be Integer", $"Invalid Diagram Style 'cx={value};' in Settings.json");
                    }
                    break;
                case @"cy":
                    int cy;
                    if (Int32.TryParse(value.Trim(), out cy))
                    {
                        dia.cy = cy;
                    }
                    else
                    {
                        MessageBox.Show("Should be Integer", $"Invalid Diagram Style 'cy={value};' in Settings.json");
                    }
                    break;
                case @"showdetails":
                    int showDetails;
                    if (Int32.TryParse(value.Trim(), out showDetails))
                    {
                        dia.ShowDetails = showDetails;
                    }
                    else
                    {
                        MessageBox.Show("Should be 0=Hide/1=Show", $"Invalid Diagram Style 'ShowDetails={value};' in Settings.json");
                    }
                    break;
                case @"showpublic":
                    bool showPublic;
                    if (Boolean.TryParse(value.Trim(), out showPublic))
                    {
                        dia.ShowPublic = showPublic;
                    }
                    else
                    {
                        MessageBox.Show("Should be 'true' or 'false'", $"Invalid Diagram Style 'ShowPublic={value};' in Settings.json");
                    }
                    break;
                case @"showprivate":
                    bool showPrivate;
                    if (Boolean.TryParse(value.Trim(), out showPrivate))
                    {
                        dia.ShowPrivate = showPrivate;
                    }
                    else
                    {
                        MessageBox.Show("Should be 'true' or 'false'", $"Invalid Diagram Style 'ShowPrivate={value};' in Settings.json");
                    }
                    break;
                case @"showprotected":
                    bool showProtected;
                    if (Boolean.TryParse(value.Trim(), out showProtected))
                    {
                        dia.ShowProtected = showProtected;
                    }
                    else
                    {
                        MessageBox.Show("Should be 'true' or 'false'", $"Invalid Diagram Style 'ShowProtected={value};' in Settings.json");
                    }
                    break;
                case @"showpackagecontents":
                    bool showPackageContents;
                    if (Boolean.TryParse(value.Trim(), out showPackageContents))
                    {
                        dia.ShowPackageContents = showPackageContents;
                    }
                    else
                    {
                        MessageBox.Show("Should be 'true' or 'false'", $"Invalid Diagram Style 'ShowPackageContents={value};' in Settings.json");
                    }
                    break;
                case @"highlightimports":
                    bool highLightImports;
                    if (Boolean.TryParse(value.Trim(), out highLightImports))
                    {
                        dia.HighlightImports = highLightImports;
                    }
                    else
                    {
                        MessageBox.Show("Should be 'true' or 'false'", $"Invalid Diagram Style 'HighLightImports={value};' in Settings.json");
                    }
                    break;
            }
        }

        /// <summary>
        /// Set DiagramObject properties. You can use 'style' and/or 'properties'. 'style' can be 'none' to not use the style.
        /// First set style (overwrite all styles or none) than update styles by properties.
        /// style:      set according to style (overwrite everything)
        /// properties: set the EA properties (only the chosen properties)
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="diaObject"></param>
        /// <param name="style">Either style or properties</param>
        /// <param name="properties"></param>
        public static void SetDiagramObjectStyle(Repository rep, EA.DiagramObject diaObject, string style, string properties)
        {
            
            if (!String.IsNullOrEmpty(style))
            {
                // only use style if not 'none'
                if (style.ToLower() != "none")
                {
                    // preserve DUID Diagram Unit Identifier
                    string s = (string) diaObject.Style;
                    Match match = Regex.Match(s, @"DUID=[A-Z0-9a-z]+;");
                    string duid = "";
                    if (match.Success) duid = match.Groups[0].Value;

                    diaObject.Style = duid + style.Replace(",", ";").Replace("   ", "").Replace("  ", "")
                                          .Replace(" ", "")
                                          .Trim();
                    try
                    {
                        diaObject.Update();
                    }
                    catch (Exception e)
                    {
                        // Probably style is to long to contain all features
                        MessageBox.Show($@"EA has a restriction of the length of the Database field.
{e}
", @"Style is to long, make it shorter!");
                    }
                }
            }

            // always use properties
            if (!String.IsNullOrEmpty(properties))
            {
                // Check if there is a DiagramObject property to use
                properties = properties.Replace(",", ";").Replace("   ", "").Replace("  ", "").Replace(" ", "").Trim();
                foreach (var property in properties.Split(';'))
                {
                    SetDiagramObjectStyle(rep, diaObject, property.Trim());
                }
                diaObject.Update();

            }

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


        private enum PropertyType
        {
            PropertyBool,
            PropertyInteger,
            PropertyString
        }
    }
}

