using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EA;
using Newtonsoft.Json.Linq;
using hoTools.Utils.SQL;
using hoTools.Utils.COM;
using hoTools.Utils.Json;


// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace hoTools.Utils.Diagram
{



    public class DiagramFormat
    {
        // Diagram Styles
        public List<DiagramStyleItem> DiagramStyleItems { get; }
        // Diagram Object Styles
        public List<DiagramObjectStyleItem> DiagramObjectStyleItems { get; }
        // Diagram Link Styles
        public List<DiagramLinkStyleItem> DiagramLinkStyleItems { get; }



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jasonFilePath"></param>

        public DiagramFormat(string jasonFilePath)
        {

            // use 'Deserializing Partial JSON Fragments'
            JObject search;
            try
            {
                // Read JSON
                string text = System.IO.File.ReadAllText(jasonFilePath);
                search = JObject.Parse(text);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Can't read '{jasonFilePath}'

{e}", "Can't import Diagram Styles from Settings.json");
                return;
            }

            //----------------------------------------------------------------------
            // Deserialize "DiagramStyle", "DiagramObjectStyle",""DiagramLinkStyle"
            // get JSON result objects into a list
            DiagramStyleItems = (List < DiagramStyleItem > )JasonHelper.GetConfigurationStyleItems<DiagramStyleItem>(search, "DiagramStyle");
            DiagramObjectStyleItems = (List<DiagramObjectStyleItem>)JasonHelper.GetConfigurationStyleItems<DiagramObjectStyleItem>(search, "DiagramObjectStyle");
            DiagramLinkStyleItems = (List<DiagramLinkStyleItem>)JasonHelper.GetConfigurationStyleItems<DiagramLinkStyleItem>(search, "DiagramLinkStyle");


        }

        

        /// <summary>
        /// Create a ToolStripItem with DropDownitems for each Style (Subtypes of DiagramGeneralStyleItem) .
        /// The Tag property contains the style.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="nameRoot"></param>
        /// <param name="toolTipRoot"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public ToolStripMenuItem ConstructStyleToolStripMenuDiagram<T>(List<T>  items, string nameRoot, string toolTipRoot, EventHandler eventHandler)
        {
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem
            {
                Text = nameRoot,
                ToolTipText = toolTipRoot
            };
            // Add item of possible style as items in drop down
            foreach (T style in items)
            {
                DiagramGeneralStyleItem style1 = style as DiagramGeneralStyleItem;
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Text = style1.Name,
                    ToolTipText = style1.Description,
                    Tag = style1
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
            string propertyTyp;

            var propertyInfo = diaObj.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                // don't know why it doesn't work
                propertyTyp = "Boolean";
                //MessageBox.Show($"PropertyStyle='{style}'",
                //    $@"No valid DiagramObject Property '{propertyName}'");
                //return;
            }
            else
            {
                propertyTyp = propertyInfo.PropertyType.Name;
                if (!(propertyTyp == "Boolean" ||
                      propertyTyp == "String" ||
                      propertyTyp == "Int32"))
                {
                    // Most things works well with Int32
                    propertyTyp = "Int32";
                    //return;
                }
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
                        // Com.SetProperty(diaObj, propertyName, boolProperty);
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
                            case "ShowNotes":
                                diaObj.ShowNotes = boolProperty;
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
                                diaObj.ShowTags = boolProperty;
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
                        Com.SetProperty(diaObj, propertyName, int32Property);
                        //diaObj.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.SetProperty, null, diaObj, new object[] { int32Property });
                        break;

                    case "String":
                        Com.SetProperty(diaObj, propertyName, propertyValue);
                        //diaObj.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.SetProperty, null, diaObj, new object[] {propertyValue});
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
        /// <param name="par">par[1] contains the values as a semicolon/comma separated properties</param>
        /// <param name="par">par[2] contains the possible diagram types</param>

        public static void SetDiagramStyleDeleteMe(Repository rep, EA.Diagram dia, string[] par)
        {
            // Make '; as delimiter for types
            string styles = par[0].Replace(",", ";");
            string properties = par[1].Replace(",", ";");
            string dStyles = par[2].Replace(",", ";");


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

            // update the ExtendedStyle, StyleEX with PDATA1, StyleEX
            // Steps of update:
            // 1. StyleEx, ExtendedStyleEx
            // 2. Properties which represents EA Properties
            // 3. Properties which are set by SQL
            // Don't change order, Update()
            foreach (string style in styleEx)
            {
                if (style.Trim() == "") continue;
                Match match = rx.Match(style.Trim());
                if (!match.Success) continue;
                string patternFind = $@"{match.Groups[1].Value}=[^;]*;";
                diaStyle = Regex.Replace(diaStyle, patternFind, $@"{style};");
                diaExtendedStyle = Regex.Replace(diaExtendedStyle, patternFind, $@"{style};");
                // advanced styles
                //SetAdvancedStyle(rep, dia, match.Groups[1].Value, match.Groups[2].Value);
                //var schowForeign0 = rep.GetStringsBySql($@"select ShowForeign from t_diagram where Diagram_ID = {dia.DiagramID}");
            }
            // delete spaces to avoid sql exception (string to long) 
            dia.ExtendedStyle = diaExtendedStyle.Replace(";   ", ";").Replace(";  ", ";").Replace("; ", ";").Trim();
            dia.StyleEx = diaStyle.Replace(";   ", ";").Replace(";  ", ";").Replace("; ", ";").Trim();
            dia.Update();

            // update non SQL Diagram properties
            foreach (string property in properties.Split(';'))
            {
                if (property.Trim() == "") continue;
                Match match = rx.Match(property.Trim());
                if (!match.Success) continue;
                // advanced styles
                SetDiagramProperty(rep, dia, match.Groups[1].Value, match.Groups[2].Value, withSql: false);
            }
            dia.Update();

            // update SQL Diagram properties
            foreach (string property in properties.Split(';'))
            {
                if (property.Trim() == "") continue;
                Match match = rx.Match(property.Trim());
                if (!match.Success) continue;
                // advanced styles
                SetDiagramProperty(rep, dia, match.Groups[1].Value, match.Groups[2].Value, withSql: true);
            }
            rep.ReloadDiagram(dia.DiagramID);
        }

        /// <summary>
        /// Handle Diagram Properties like: Orientation=L/P, Scale=100 (100%) which are independent of the general styles in PDATA, StyleEX.
        /// Sequence:!!
        /// 1 Non SQL Diagram Properties
        /// 2.SQL Diagram Properties
        /// 
        /// Rational: It looks as if the EA Update process overwrites SQL changes if not separated!
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="withSql">false non SQL, true with SQL</param>
        private static void SetDiagramProperty(Repository rep, EA.Diagram dia, string name, string value, bool withSql = false)
        {
            if (!withSql)
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
                            MessageBox.Show($"Invalid Diagram Style 'Scale={value};' in Settings.json");
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
                            MessageBox.Show("Should be Integer",
                                $"Invalid Diagram Style 'cx={value};' in Settings.json");
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
                            MessageBox.Show("Should be Integer",
                                $"Invalid Diagram Style 'cy={value};' in Settings.json");
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
                            MessageBox.Show("Should be 0=Hide/1=Show",
                                $"Invalid Diagram Style 'ShowDetails={value};' in Settings.json");
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
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'ShowPublic={value};' in Settings.json");
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
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'ShowPrivate={value};' in Settings.json");
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
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'ShowProtected={value};' in Settings.json");
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
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'ShowPackageContents={value};' in Settings.json");
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
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'HighLightImports={value};' in Settings.json");
                        }
                        break;
                }

            }
            else
            {   // with SQL
                switch (name.ToLower().Trim())
                {
                    case @"showforeign":
                        //dia.Update();
                        //rep.ReloadDiagram(dia.DiagramID);
                        bool showForeign;
                        if (Boolean.TryParse(value.Trim(), out showForeign))
                        {
                            string updateStr = $@"update t_diagram set ShowForeign = {value.Trim()} 
                                                                          where Diagram_ID = {dia.DiagramID}";
                            UtilSql.SqlUpdate(rep, updateStr);
                            //rep.ReloadDiagram(dia.DiagramID);
                            //var schowForeign = rep.GetStringsBySql($@"select ShowForeign from t_diagram where Diagram_ID = {dia.DiagramID}");
                        }
                        else
                        {
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'ShowForeign={value};' in Settings.json");
                        }
                        break;
                    case @"showborder":
                        //dia.Update();
                        //rep.ReloadDiagram(dia.DiagramID);
                        bool showBorder;
                        if (Boolean.TryParse(value.Trim(), out showBorder))
                        {
                            string updateStr = $@"update t_diagram set ShowBorder = {value.Trim()} 
                                                                          where Diagram_ID = {dia.DiagramID}";
                            UtilSql.SqlUpdate(rep, updateStr);
                            //rep.ReloadDiagram(dia.DiagramID);
                            //var schowForeign = rep.GetStringsBySql($@"select ShowForeign from t_diagram where Diagram_ID = {dia.DiagramID}");
                        }
                        else
                        {
                            MessageBox.Show("Should be 'true' or 'false'",
                                $"Invalid Diagram Style 'ShowBorder={value};' in Settings.json");
                        }
                        break;
                }

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
                    string s = (string)diaObject.Style;
                    Match match = Regex.Match(s, @"DUID=[A-Z0-9a-z]+;");
                    string duid = "";
                    if (match.Success) duid = match.Groups[0].Value;

                    s = duid + style.Replace(",", ";").Replace("   ", "").Replace("  ", "")
                                          .Replace(" ", "")
                                          .Trim();
                    // ensure string ends with ";"
                    if (s.Trim() != "" & (! s.EndsWith(";")) ) {s = s + ";";}
                    diaObject.Style = s;
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

    }
}

