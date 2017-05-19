using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace hoTools.Utils.Diagram
{
    public class DiagramLinkStyle : DiagramGeneralStyle
    {
        private readonly EA.DiagramLink _link;

        public DiagramLinkStyle(EA.DiagramLink link, string style, string property)
            : base(style, property)
        {
            
            _link = link;
        }

        public void SetProperty()
        {
            foreach (var link in Style)
            {
                string name;
                string value;
                if (GetNameValueFromStyle(link, out name, out value)) continue;


                switch (link)
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
        /// Update style
        /// </summary>
        public void UpdateStyle()
        {
            string oldStyle = _link.Style;
            oldStyle = UpdateStyles(oldStyle);
            _link.Style = oldStyle;

        }

        
    }
}
