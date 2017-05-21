using System;

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
            foreach (var link in Property)
            {
                string name;
                string value;
                if (! GetNameValueFromStyle(link, out name, out value)) continue;


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
            _link.Update();

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
                if (GetNameValueFromStyle(type, out name, out value)) continue;
                switch (name)
                {
                    case "Types":
                        string nameType;
                        string valueTypes;
                        if (GetNameValueFromStyle(type, out nameType, out valueTypes)) continue;
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
                    case "Stereotypes":
                        string nameStereotype;
                        string valueStereotypes;
                        if (GetNameValueFromStyle(type, out nameStereotype, out valueStereotypes)) continue;
                        if (valueStereotypes.Trim() == "") continue;

                        // must be a supported Stereotype value
                        isToProcessStereotype = false;
                        foreach (var conStereo in valueStereotypes.Split(','))
                        {
                            if (conStereo == "") continue;

                            // check if stereotype exists
                            var a = con.StereotypeEx.Split(','); //.ToArray().
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
