using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace hoTools.Utils.Json
{
    public class JasonHelper
    {
        
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="search"></param>
    /// <param name="jsonChapter">"DiagramStyle", "DiagramObjectStyle","DiagramLinkStyle", "AutoIncrement"</param>
    public static  IList<T> GetConfigurationStyleItems<T>(JObject search, string jsonChapter)
        {
            try
            {

                IList<JToken> results = search[jsonChapter].Children().ToList();

                // serialize JSON results into .NET objects
                IList<T> searchResults = new List<T>();
                foreach (JToken result in results)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    T searchResult = result.ToObject<T>();
                    if (searchResult == null) continue;
                    searchResults.Add(searchResult);
                }
                return searchResults.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Cant import '{jsonChapter}' from 'Settings.json'

{e}
A chapter in Settings.JSON is missing!
Consider resetting to Factory Settings. File, .....
",
                    $@"Can't import JSON Chapter '{jsonChapter}'");
                return null;
            }
        }
    }
}
