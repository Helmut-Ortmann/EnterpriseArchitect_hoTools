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
        public string[] Type;
        public EA.Repository Rep;



        public DiagramGeneralStyle(EA.Repository rep, string type, string style, string property)
        {
            Style = style.Replace(",", ";").Replace(";;", ";").Split(';');
            Property = property.Replace(",", ";").Replace(";;", ";").Split(';');
            Type = type.Replace(",", ";").Replace(";;", ";").Split(';');
            Rep = rep;
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
            if (Int32.TryParse(value.Trim(), out intValue)) return true;
            MessageBox.Show($"Value '{value}' of Style/Property '{name}' must be Integer",
                $"Invalid Integer Style/Property in Settings.json");
            return false;
        }
        protected static bool ConvertBool(string name, string value, out bool boolValue)
        {
            boolValue = false;
            if (Boolean.TryParse(value.Trim(), out boolValue)) return true;
            MessageBox.Show($"Value '{value}' of Style/Property '{name}' must be Boolean",
                $"Invalid Boolean Style/Property in Settings.json");
            return false;
        }
        protected string UpdateStyles(string oldStyle)
        {
            string newStyle = oldStyle;
            foreach (var s in Style)
            {
                string style = s.Trim();
                string name;
                string value;
                if (!GetNameValueFromStyle(style, out name, out value)) continue;
                if (newStyle.Contains($"{name}="))
                {
                    // update style 
                    newStyle = Regex.Replace(newStyle, $@"{name}=[^;]*;", $"{style};");
                }
                else
                {
                    // insert style
                    newStyle = $"{newStyle};{style};";
                }
            }
            return newStyle;
        }

        

    }
}
