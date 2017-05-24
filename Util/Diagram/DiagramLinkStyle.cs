using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
            foreach (var link in Property)
            {
                string name;
                string value;
                if (! GetNameValueFromString(link, out name, out value)) continue;


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
                        foreach (var tag in tags)
                        {
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
                                    string regex111 = $@"{name}=[^;]*?({tagName}=([0-9ABCDEFabcdef#Xx]*))[^;]*;";
                                    string findProperty = $@"{name}=[^;]*;";
                                    Match matchFoundProperty = Regex.Match(linkGeometry, findProperty);
                                    if (matchFoundProperty.Success)
                                    {
                                        string oldProperty = matchFoundProperty.Groups[0].Value;
                                        string newProperty;
                                        if (oldProperty.Contains($"{tagName}="))
                                        {
                                            // update style 
                                            newProperty = Regex.Replace(oldProperty, $@"{tagName}=[0-9A-Fa-f#xX]*", $"{tagName}={intValue}");
                                        }
                                        else
                                        {
                                            // insert style
                                            // Check if there are already tags: append tag with delimiter 
                                            string delimiter = ":";
                                            if (oldProperty.Substring(oldProperty.Length - 2) == "=;") delimiter = "";
                                            newProperty = $"{oldProperty.Substring(0,oldProperty.Length-1)}{delimiter}{tagName}={intValue};";
                                        }
                                        linkGeometry = linkGeometry.Replace(oldProperty, newProperty);
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
            EA.Connector con = Rep.GetConnectorByID(_link.ConnectorID); 
            foreach (var type in Type)
            {
                
                string name;
                string value;
                if (! GetNameValueFromString(type, out name, out value)) continue;
                switch (name.Substring(1, 4).ToLower())
                {
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
                }

            }
            return isToProcessType && isToProcessStereotype;

        }

        
    }
}
