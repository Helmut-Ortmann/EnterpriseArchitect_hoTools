using System;
using System.Text.RegularExpressions;
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
            Style = style.Trim().Replace(",", ";").Replace(";;", ";").TrimEnd(';').Split(';');
            Property = property.Trim().Replace(",", ";").Replace(";;", ";").TrimEnd(';').Split(';');

            type = type.Trim().TrimEnd(';').Replace(";;", ";");
            Type = type.Split(';');
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
        /// <summary>
        /// Converts a string to integer. It also allows hexadecimal values (0xaaafffff), or the web format #ffffff.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="intValue"></param>
        /// <returns></returns>
        protected static bool ConvertInteger(string name, string value, out int intValue)
        {
            // ReSharper disable once RedundantAssignment
            intValue = 0;
            value = value.Trim().ToLower();
            if (Int32.TryParse(value, out intValue)) return true;
            if (value.Substring(0,2)=="0x") 
                if (Int32.TryParse(value.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out intValue)) return true;
            if (value.Substring(0, 1) == "#")
                if (Int32.TryParse(value.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out intValue)) return true;
            else
                if (Int32.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out intValue)) return true;


            MessageBox.Show($"Value '{value}' of Style/Property '{name}' must be Integer or Hexadecimal (e.g. 0xFA or #FA)",
                $"Invalid Integer/Hexa Style/Property in Settings.json");
            return false;
        }
        protected static bool ConvertBool(string name, string value, out bool boolValue)
        {
            // ReSharper disable once RedundantAssignment
            boolValue = false;
            if (Boolean.TryParse(value.Trim(), out boolValue)) return true;
            MessageBox.Show($"Value '{value}' of Style/Property '{name}' must be Boolean",
                $"Invalid Boolean Style/Property in Settings.json");
            return false;
        }
        protected string UpdateStyles(string oldStyle, bool withInsert=true)
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
                    if (withInsert) newStyle = $"{newStyle};{style};";
                }
            }
            return newStyle;
        }

        

    }
}
