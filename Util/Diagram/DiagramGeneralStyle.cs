using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hoTools.Utils.Diagram
{
    public class DiagramGeneralStyle
    {
        public string[] Style;
        public string[] Property;

        public DiagramGeneralStyle(string style, string property)
        {
            Style = style.Replace(",", ";").Replace(";;", ";").Split(';');
            Property = property.Replace(",", ";").Replace(";;", ";").Split(';');
        }

        protected static bool GetNameValueFromStyle(string link, out string name, out string value)
        {
            Regex rx = new Regex(@"([^=]*)=(.*)");
            Match match = rx.Match(link.Trim());
            name = "";
            value = "";
            if (!match.Success) return false;
            name = match.Groups[1].Value;
            value = match.Groups[2].Value;
            return true;
        }

        protected static bool ConvertInteger(string name, string value, out int intValue)
        {
            intValue = 0;
            if (!Int32.TryParse(value.Trim(), out intValue)) return false;
            MessageBox.Show($"Value '{value}' of Style/Property '{name}' must be Integer",
                $"Invalid Integer Style/Property in Settings.json");
            return true;
        }
        protected static bool ConvertBool(string name, string value, out bool boolValue)
        {
            boolValue = false;
            if (!Boolean.TryParse(value.Trim(), out boolValue)) return false;
            MessageBox.Show($"Value '{value}' of Style/Property '{name}' must be Boolean",
                $"Invalid Boolean Style/Property in Settings.json");
            return true;
        }
        protected string UpdateStyles(string oldStyle)
        {
            foreach (var style in Style)
            {
                string name;
                string value;
                if (!GetNameValueFromStyle(style, out name, out value)) continue;
                if (oldStyle.Contains($"{name}="))
                {
                    // update style 
                    Regex.Replace(oldStyle, $@"{name}=[^;]*;", style);
                }
                else
                {
                    // insert style
                    oldStyle = $"{oldStyle};{style};";
                }
            }
            return oldStyle;
        }

    }
}
