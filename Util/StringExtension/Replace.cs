using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hoTools.Utils.StringExtension
{
    /// <summary>
    /// A class to hold the replacement configuration
    /// - Regex pattern
    /// - Replacement
    /// </summary>
    public class ReplacementConfig
    {
        // The regular expression pattern to search for
        public string Pattern { get; set; }
        // The string to replace the matched pattern with
        public string Replacement { get; set; }
    }
    /// <summary>
    /// Replacements
    /// </summary>
    public class Replacement {
        /// <summary>
        /// Replace with Regex
        /// </summary>
        /// <param name="input"></param>
        /// <param name="configList"></param>
        /// <returns></returns>
        public static string ReplaceRegexWithConfig(string input, List<ReplacementConfig> configList)
        {
            // Iterate over each configuration in the list
            foreach (var config in configList)
            {
                // Use Regex.Replace to replace the pattern in the input string
                // The lambda function is used to replace only the captured group in the matched string
                // m.Groups[1].Value represents the first capturing group in the matched string
                input = Regex.Replace(input, config.Pattern, m => m.Value.Replace(m.Groups[1].Value, config.Replacement));
            }
            // Return the modified string
            return input;
        }
    }
}
