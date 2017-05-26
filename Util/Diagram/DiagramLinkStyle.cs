using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using hoTools.Utils.SQL;

namespace hoTools.Utils.Diagram
{
    public class DiagramLinkStyle : DiagramGeneralStyle
    {
        private readonly EA.DiagramLink _link;
        public DiagramLinkStyle(EA.Repository rep, EA.DiagramLink link,  string type, string style, string property)
            : base(rep, type, style, property)
        {
            _link = link;
        }

        /// <summary>
        /// Set all properties
        /// </summary>
        public void SetProperties()
        {
            string linkGeometry = _link.Geometry;
            foreach (var l in Property)
            {
                string link = l.Trim();
                if (link == "") continue;

                string name;
                string value;
                if (! GetNameValueFromString(link, out name, out value)) continue;

                EA.Connector con;
                switch (name)
                {
                    case "HiddenLabels":
                        bool hiddenLabels;
                        if (!ConvertBool(name, value, out hiddenLabels)) continue;
                        _link.HiddenLabels = hiddenLabels;
                        break;
                    case "IsHidden":
                        bool isHidden;
                        if (!ConvertBool(name, value, out isHidden)) continue;
                        _link.IsHidden = isHidden;
                        break;
                    case "LineStyle":
                        int lineStyle;
                        if (! ConvertInteger(name, value, out lineStyle)) continue;
                        _link.LineStyle = (EA.LinkLineStyle)lineStyle;
                        break;
                    case "LineColor":
                        int lineColor;
                        if (!ConvertInteger(name, value, out lineColor)) continue;
                        _link.LineColor = lineColor;
                        break;

                    case "LineWidth":
                        int lineWidth;
                        if (!ConvertInteger(name, value, out lineWidth)) continue;
                        _link.LineWidth = lineWidth;
                        break;
                    case "SuppressSegment":
                        int suppressSegment;
                        if (!ConvertInteger(name, value, out suppressSegment)) continue;
                        _link.SuppressSegment = suppressSegment;
                        break;

                    // change the default style for connector, link is set to -1
                    case "ConnectorWidth":
                        _link.LineWidth = -1;
                        int connectorLineWidth;
                        if (!ConvertInteger(name, value, out connectorLineWidth)) continue;
                        con = Rep.GetConnectorByID(_link.ConnectorID);
                        con.Width = connectorLineWidth;
                        con.Update();
                        break;

                    // change the default style for connector, link is set to -1
                    case "ConnectorColor":
                        _link.LineColor = -1;

                        int connectorLineColor;
                        if (!ConvertInteger(name, value, out connectorLineColor)) continue;
                        con = Rep.GetConnectorByID(_link.ConnectorID);
                        con.Color = connectorLineColor;
                        con.Update();
                        break;

                    case "EaStyle":
                        string sql = $@"select Notes as [STYLE] from t_trxtypes  where TRX='{value.Trim()}'";
                        var eaStyles = UtilSql.GetListOfStringFromSql(Rep, sql, "STYLE");
                        if (eaStyles.Count == 0)
                        {
                            MessageBox.Show($"Style '{value}' not available in EA.", $"Can't read EA Style '{value}'.");
                            continue;
                        }
                        if (eaStyles.Count > 1)
                        {
                            MessageBox.Show($"Style '{value}'\r\nCount: {eaStyles.Count}. EA is a little tricky with names! Check list box of styles in EA!", $"More than one srtyle with name '{value}'.");
                            continue;
                        }
                        // map general EA style to LINK styles:
                        // Color, Width
                        foreach (var s in eaStyles[0].Split(';'))
                        {
                            string style = s.Trim();
                            if (style == "") continue;
                            if (!GetNameValueFromString(style, out name, out value)) continue;
                            switch (name)
                            {
                                case "Border":
                                    _link.LineColor = Int32.Parse(value);
                                    break;
                                case @"Line":
                                    _link.LineWidth = Int32.Parse(value);
                                    break;
                            }
                        }
                        break;

                    // handle labels, geometry label definition

                    case "LLB":
                    case "LLT":
                    case "LMB":
                    case "LMT":
                    case "LRT":
                    case "LRB":
                    case "IRHS":
                    case "ILHS":
                        // extract tags
                        string[] tags = value.Split(':');
                        foreach (var t in tags)
                        {
                            string tag = t.Trim();
                            if (tag == "") return;

                            string tagName;
                            string tagValue;
                            if (!GetNameValueFromString(tag, out tagName, out tagValue, delimiter:'=')) continue;

                            // possible tags inside geometry label definition
                            switch (tagName)
                            {
                                case "BLD":
                                case "HDN":
                                case "ITA":
                                case "CLR":
                                case "ALN":
                                case "ROT":
                                case "DIR":
                                    int intValue;
                                    if (!ConvertInteger(tagName, tagValue, out intValue)) continue;
                                    // replace geometry, find LabelName with labelTag (tagName=tagValue
                                    // possible tag values: integer, hexadecimal like #FE or 0xFE
                                    string findProperty = $@"{name}=[^;]*;";
                                    Match matchFoundProperty = Regex.Match(linkGeometry, findProperty);
                                    if (matchFoundProperty.Success)
                                    {
                                        string oldProperty = matchFoundProperty.Groups[0].Value;
                                        string newProperty;
                                        // ensure default settings
                                        string defaultPropertyLabel =
                                            "CX=25:CY=13:OX=0:OY=0:HDN=1:BLD=0:ITA=0:UND=0:CLR=-1:ALN=1:DIR=0:ROT=0;";
                                        if (oldProperty.Substring(oldProperty.Length - 2) == "=;")
                                        {
                                            newProperty = $@"{name}={defaultPropertyLabel}";
                                            linkGeometry = linkGeometry.Replace(oldProperty, newProperty);
                                            oldProperty = newProperty;
                                        }

                                        // update style 
                                        newProperty = Regex.Replace(oldProperty, $@"{tagName}=[0-9A-Fa-f#xX]*", $"{tagName}={intValue}");
                                        
                                       
                                        //// insert style
                                        //// Check if there are already tags: append tag with delimiter 
                                        //string delimiter = ":";
                                        //if (oldProperty.Substring(oldProperty.Length - 2) == "=;") delimiter = "";
                                        //newProperty = $"{oldProperty.Substring(0,oldProperty.Length-1)}{delimiter}{tagName}={intValue};";
                                        
                                        linkGeometry = linkGeometry.Replace(oldProperty, newProperty);
                                    }
                                    break;
                                    // set to an arbitrary property value, be careful
                                    // LLT=SET=; makes the default 'LLT=;'
                                case "SET":
                                    string findSetProperty = $@"{name}=[^;]*;";
                                    Match matchFoundSetProperty = Regex.Match(linkGeometry, findSetProperty);

                                    if (matchFoundSetProperty.Success)
                                    {
                                        string oldSetProperty = matchFoundSetProperty.Groups[0].Value;
                                        // update style 
                                        string newSetProperty = $@"{name}={tagValue};";


                                        //// insert style
                                        //// Check if there are already tags: append tag with delimiter 
                                        //string delimiter = ":";
                                        //if (oldProperty.Substring(oldProperty.Length - 2) == "=;") delimiter = "";
                                        //newProperty = $"{oldProperty.Substring(0,oldProperty.Length-1)}{delimiter}{tagName}={intValue};";

                                        linkGeometry = linkGeometry.Replace(oldSetProperty, newSetProperty);
                                    }
                                    break;

                            }
                        }
                        break;
                      
                }
            }
            _link.Geometry = linkGeometry;
            Update();
        }
        /// <summary>
        /// Set according ti EA Text Styles
        /// </summary>
        public void SetEaTextStyles()
        {
            foreach (var s in EaTextStyle)
            {
                string style = s.Trim();
                if (style == "") continue;
                string name;
                string value;
                if (!GetNameValueFromString(style, out name, out value)) continue;
                switch (name)
                {
                    case "Border":
                        _link.LineColor = Int32.Parse(value);
                        break;
                    case @"Line":
                        _link.LineWidth = Int32.Parse(value);
                        break;
                }
            }
            _link.Update();
        }
        /// <summary>
        /// Update styles
        /// </summary>
        public void UpdateStyles()
        {
            string oldStyle = _link.Style;
            oldStyle = base.UpdateStyles(oldStyle);
            _link.Style = oldStyle;
            Update();

        }

        /// <summary>
        /// Update Diagram Link
        /// </summary>
        private void Update()
        {
            try
            {
                _link.Update();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Style is possibly to long for DB field\r\n\r\e{e}", "Cant write Diagram Styles!");
            }
        }
        public bool IsToProcess()
        {
            if (Type.Length == 0) return true;
            bool isToProcessType = true;
            bool isToProcessStereotype = true;
            bool isToProcessHidden = false;
            bool isToProcessColor = true;


            EA.Connector con = Rep.GetConnectorByID(_link.ConnectorID); 
            foreach (var type in Type)
            {
                if (String.IsNullOrWhiteSpace(type)) continue;
                string name;
                string value;
                if (! GetNameValueFromString(type, out name, out value)) continue;
                switch (name.Substring(0, 4).ToLower())
                {
                    // Type/LinkType: first 4 characters lower case
                    case "type":
                        string nameType;
                        string valueTypes;
                        if (! GetNameValueFromString(type, out nameType, out valueTypes)) continue;
                        if (valueTypes.Trim() == "") continue;

                        // must be a supported Type value
                        isToProcessType = false;
                        foreach (var conType in valueTypes.Split(','))
                        {
                            if (conType == "") continue;
                            if (conType == con.Type)
                            {
                                isToProcessType = true;
                                break;
                            }
                        }

                        break;
                    // Stereotype: first 4 characters lower case
                    case @"ster":
                        string nameStereotype;
                        string valueStereotypes;
                        if (! GetNameValueFromString(type, out nameStereotype, out valueStereotypes)) continue;
                        if (valueStereotypes.Trim() == "") continue;

                        // must be a supported Stereotype value
                        isToProcessStereotype = false;
                        foreach (var conStereo in valueStereotypes.Split(','))
                        {
                            if (conStereo == "") continue;

                            // check if stereotype exists
                            if (Array.IndexOf(con.StereotypeEx.Split(','),conStereo) > -1)
                            {
                                isToProcessStereotype = true;
                                break;
                            }
                        }
                        break;
                    // Hidden: first 4 characters lower case
                    case @"hidd":
                        if ( value.ToLower() == "true") isToProcessHidden = true;
                        break;
                    // Color: first 4 characters lower case
                    case @"colo":
                        isToProcessColor = false;
                        int color;
                        if (Int32.TryParse(ColorIntegerFromName(value), out color))
                        {
                            if (color == _link.LineColor) isToProcessColor = true;
                        }
                        else
                        {
                            MessageBox.Show($@"Type: '{type}'", "Type for color isn't correct");
                        }
                        break;

                }

            }
            return isToProcessType && isToProcessStereotype && (isToProcessHidden == _link.IsHidden) && isToProcessColor;

        }

        
    }
}
