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

        /// <summary>
        /// Get value from string: Name=Value; extracts name and value. You can change the default delimiter "=" to whatever character you like
        /// </summary>
        /// <param name="myString"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        protected static bool GetNameValueFromStyle(string myString, out string name, out string value, char delimiter='=')
        {
            name = "";
            value = "";
            string[] s = myString.Split(delimiter);
            if (s.Length != 2) return false;
            name = s[0];
            value = s[1];
            return true;


            //Regex rx = new Regex(@"([^=]*)=(.*)");
            //Match match = rx.Match(link.Trim());
            //name = "";
            //value = "";
            //if (!match.Success) return false;
            //name = match.Groups[1].Value;
            //value = match.Groups[2].Value;
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

        /// <summary>
        /// Update local Styles (standard)
        /// </summary>
        /// <param name="oldStyle"></param>
        /// <param name="withInsert"></param>
        /// <returns></returns>
        protected string UpdateStyles(string oldStyle, bool withInsert=true)
        {
            return UpdateStyles(oldStyle, Style, withInsert);
           
        }

        /// <summary>
        /// Update arbitrary Style (used for diagram style which has an additional PDATA style
        /// </summary>
        /// <param name="oldStyle"></param>
        /// <param name="newStyles"></param>
        /// <param name="withInsert"></param>
        /// <returns></returns>
        protected string UpdateStyles(string oldStyle, string[] newStyles, bool withInsert = true)
        {
            string newStyle = oldStyle;
            foreach (var s in newStyles)
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
            return newStyle.Replace(";;;", ";").Replace(";;;", ";").Replace(";;", ";");
        }



    }
}
