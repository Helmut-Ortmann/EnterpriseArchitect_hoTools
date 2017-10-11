using System.Text.RegularExpressions;

namespace hoTools.Utils.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Split a string according to CamelCase
        /// - "IBMMakeStuffAndSellIt" to "IBM Make Stuff And Sell It"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}
