using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace hoTools.Utils.Diagram
{
    public class DiagramStyle : DiagramGeneralStyle
    {
        private readonly EA.Diagram _dia;
        private readonly string[] _pdatas;
        public DiagramStyle(EA.Repository rep, EA.Diagram dia, string type, string style, string pdata, string property)
            : base(rep, type, style, property)
        {
            _dia = dia;
            _pdatas = pdata.Trim().Replace(",", ";").Replace(";;", ";").TrimEnd(';').Split(';'); 
        }

        /// <summary>
        /// Set all properties, a property might be empty to not change the value
        /// </summary>
        public void SetProperties(bool withSql = false)
        {

            foreach (var p in Property)
            {
                string property = p.Trim().ToLower();
                // Check if Property has something to change
                if (!GetNameValueFromString(property, out string name, out string value)) continue;

                if (!withSql)
                { 

                    switch (name)
                    {
                        case "orientation":
                            _dia.Orientation = value.Trim();
                            break;
                        case "scale":
                            int scale;
                            if (!ConvertInteger(name, value, out scale)) continue;
                            _dia.Scale = scale;
                            break;
                        case @"cx":
                            int cx;
                            if (!ConvertInteger(name, value, out cx)) continue;
                                _dia.cx = cx;
                            break;
                        case @"cy":
                            int cy;
                            if (!ConvertInteger(name, value, out cy)) continue;
                            _dia.cy = cy;
                            break;
                        case @"showdetails":
                            int showDetails;
                            if (!ConvertInteger(name, value, out showDetails)) continue;
                            _dia.ShowDetails = showDetails;
                            
                            break;
                        case @"showpublic":
                            bool showPublic;
                            if (!ConvertBool(name, value, out showPublic)) continue;
                            _dia.ShowPublic = showPublic;
                            break;
                        case @"showprivate":
                            bool showPrivate;
                            if (!ConvertBool(name, value, out showPrivate)) continue;
                            _dia.ShowPrivate = showPrivate;
                            break;
                        case @"showprotected":
                            bool showProtected;
                            if (!ConvertBool(name, value, out showProtected)) continue;
                            _dia.ShowProtected = showProtected;
                            break;
                        case @"showpackagecontents":
                            bool showPackageContents;
                            if (!ConvertBool(name, value, out showPackageContents)) continue;
                            _dia.ShowPackageContents = showPackageContents;
                            break;
                        case @"highlightimports":
                            bool highLightImports;
                            if (!ConvertBool(name, value, out highLightImports)) continue;
                            _dia.HighlightImports = highLightImports;
                            break;

                    }
                } else
                {
                    switch (name) {
                        case @"showforeign":
                            bool showForeign;
                            if (!ConvertBool(name, value, out showForeign))
                            {
                                string updateStr = $@"update t_diagram set ShowForeign = {value.Trim()} 
                                                                                    where Diagram_ID = {_dia.DiagramID}";
                                SQL.UtilSql.SqlUpdate(Rep, updateStr);
                            }
                            break;

                        case @"showborder":
                            bool showBorder;
                            if (!ConvertBool(name, value, out showBorder))
                            {
                                string updateStr = $@"update t_diagram set ShowBorder = {value.Trim()} 
                                                                              where Diagram_ID = {_dia.DiagramID}";
                                SQL.UtilSql.SqlUpdate(Rep, updateStr);
                            }
                            break;
                    }

                }
            }
            Update();
        }
        /// <summary>
        /// Update styles
        /// - StyleEx=Style in database
        /// - ExtendedStyle=PDATA in database
        /// </summary>
        public void UpdateStyles()
        {
            // StyleEx
            string styleEx = _dia.StyleEx;
            styleEx = base.UpdateStyles(styleEx);
            _dia.StyleEx = styleEx;

            // ExtendedStyle / PDATA
            string extendedStyle = _dia.ExtendedStyle;
            extendedStyle = base.UpdateStyles(extendedStyle, _pdatas);
            _dia.ExtendedStyle = extendedStyle;
            Update();


        }
        /// <summary>
        /// Update Diagram
        /// </summary>
        private void Update()
        {
            try
            {
                _dia.Update();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Style is possibly to long for DB field\r\n\r\n{e}", "Can't write Diagram Styles!");
            }
        }

        public bool IsToProcess()
        {
            if (Type.Length == 0) return true;
            bool isToProcessType = true;
            bool isToProcessCust = true;
            bool isToProcessStereotype = true;

            foreach (var type in Type)
            {
                if (String.IsNullOrWhiteSpace(type)) continue;
                if (!GetNameValueFromString(type, out string name, out string value)) continue;
                switch (name.Substring(0,4).ToLower())
                {
                    // Standard Diagram Types
                    case @"type":
                        string nameTypes;
                        string valueTypes;
                        if (!GetNameValueFromString(type, out nameTypes, out valueTypes)) continue;
                        if (valueTypes.Trim() == "") continue;

                        // must be a supported Diagram Type value
                        isToProcessType = false;
                        foreach (var t in valueTypes.Split(','))
                        {
                            string diaType = t.Trim();
                            if (diaType == "") continue;
                            // Match only part
                            if (_dia.Type.Contains(diaType))
                            {
                                isToProcessType = true;
                                break;
                            }
                        }
                        break;

                    // Customer Diagram
                    case @"cust":
                        string nameCust;
                        string valueCust;
                        if (!GetNameValueFromString(type, out nameCust, out valueCust)) continue;
                        if (valueCust.Trim() == "") continue;

                        // must be a supported Type value
                        isToProcessCust = false;
                        string diaCustomType = GetCustomDiagramType();
                        foreach (var t in valueCust.Split(','))
                        {
                            string customType = t.Trim();
                            if (customType == "") continue;
                            if (diaCustomType.Contains(customType))
                            {
                                isToProcessCust = true;
                                break;
                            }
                        }
                        break;
                    // Stereotype
                    case @"ster":
                        string nameStereotype;
                        string valueStereotypes;
                        if (!GetNameValueFromString(type, out nameStereotype, out valueStereotypes)) continue;
                        if (valueStereotypes.Trim() == "") continue;

                        // must be a supported Stereotype value
                        isToProcessStereotype = false;
                        foreach (var objStereo in valueStereotypes.Split(','))
                        {
                            if (objStereo == "") continue;

                            // check if stereotype exists
                            if ((Array.IndexOf(_dia.StereotypeEx.Split(','), objStereo) > -1) || (_dia.Stereotype == objStereo))
                            {
                                isToProcessStereotype = true;
                                break;
                            }
                        }


                        break;
                    default:
                        MessageBox.Show($"Type '{name}' invalid.\r\n First 4 or more characters of (Type, Custom, Stereotype)",
                                        "Invalid Diagram Type (Type, Custom, Stereotype)");
                        break;
                }

            }
            return isToProcessType && isToProcessCust && isToProcessStereotype;

        }
        /// <summary>
        /// Get the Custom DiagramType
        /// </summary>
        /// <returns></returns>
        private string GetCustomDiagramType()
        {

            Match m = Regex.Match(_dia.StyleEx, @"MDGDgm=([^;]+);");
            if (m.Success)
            {
                if (m.Groups.Count > 0) return m.Groups[1].Value;
                else return "";
            }
            else return "";
        }
    }
}
