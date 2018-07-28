using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EA;
using hoLinqToSql.LinqUtils;

namespace hoTools.EaServices.AddInSearch
{
    public class AddInSearches
    {
        // Dictionary of all TaggedValues of a search
        private static Dictionary<string, string> _tv = new Dictionary<string, string>();

        ///  <summary>
        ///  Add-In Search to find nested Elements for:
        ///  -  Selected
        ///  -- Package
        ///  -- Element
        ///  - Comma separated GUID list of Packages or Elements in 'Search Term'  
        /// 
        ///   It outputs:
        ///  - All elements in it's hierarchical structure
        ///  - Tagged Values
        ///  
        ///  How it's works:
        ///  1. Create a Table and fill it with your code
        ///  2. Adapt LINQ to output the table (powerful)
        ///     -- Where to select only certain rows
        ///     -- Order By to order the result set
        ///     -- Grouping
        ///     -- Filter
        ///     -- JOIN
        ///     -- etc.
        ///  3. Deploy and test 
        ///  </summary>
        ///  <param name="repository"></param>
        ///  <param name="searchText"></param>
        /// <returns></returns>
        public static string SearchObjectsNested(EA.Repository repository, string searchText)
        {
            // 1. Collect data into a data table
            DataTable dt = AddinSearchObjectsNestedInitTable(repository, searchText);
            // 2. Order, Filter, Join, Format to XML
            return AddinSearchObjectsNestedMakeXml(dt);
        }


        /// <summary>
        /// Query to make EA xml from a Data Table by using MakeXml. 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string AddinSearchObjectsNestedMakeXml(DataTable dt)
        {
            try
            {
                // Make a LINQ query (WHERE, JOIN, ORDER,)
                EnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
                                                        select row;

                return Xml.MakeXml(dt, rows);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}", @"Error LINQ query Test query to show Table to EA xml format");
                return "";

            }
        }
        /// <summary>
        /// InitializeTable for search results
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="searchText">Optional: List of packages, EA elements as a comma separated list</param>
        /// <returns></returns>
        private static DataTable AddinSearchObjectsNestedInitTable(EA.Repository rep, string searchText)
        {
            _tv = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            dt.Columns.Add("CLASSGUID", typeof(string));
            dt.Columns.Add("CLASSTYPE", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Alias", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            int level = 0;
            if (searchText.Trim().StartsWith("{"))
            {
                // handle string
                string[] earchTextArr = GetEaFromCommaSeparatedList(searchText);
                foreach (var txt in earchTextArr)
                {
                    EA.Element el = rep.GetElementByGuid(txt);
                    if (el == null) continue;
                    if (el.Type != "Package") NestedElementsRecursive(dt, el, level);
                    if (el.Type == "Package") NestedPackageElementsRecursive(dt, rep.GetPackageByGuid(txt), level);
                }

                EA.Package pkg = rep.GetPackageByGuid(searchText);
                if (pkg != null)
                {
                    NestedPackageElementsRecursive(dt, pkg, level);
                }
            }
            else
            {
                // handle context element
                rep.GetContextItem(out var item);
                if (item is Element element) NestedElementsRecursive(dt, element, level);
                if (item is Package package) NestedPackageElementsRecursive(dt, package, level);
            }
            return dt;
        }

        /// <summary>
        /// Make a Data Table from Package Elements recursive
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pkg"></param>
        /// <param name="level"></param>
        private static void NestedPackageElementsRecursive(DataTable dt, EA.Package pkg, int level)
        {
            foreach (EA.Element el in pkg.Elements)
            {
                NestedElementsRecursive(dt, el, level);
            }

        }
        /// <summary>
        /// Make a Data Table from nested Elements recursive
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="el"></param>
        /// <param name="level"></param>
        private static void NestedElementsRecursive(DataTable dt, EA.Element el, int level)
        {

            var row = dt.NewRow();
            row["CLASSGUID"] = el.ElementGUID;
            row["CLASSTYPE"] = el.Type;
            string t = $"{new string('.', 2 * level)}{el.Name}";
            row["Name"] = t;
            row["Alias"] = el.Alias;
            row["Note"] = el.Notes;
            AddTaggedValues(dt, el, row);
            dt.Rows.Add(row);
            level += 1;
            foreach (EA.Element elChild in el.Elements)
            {

                NestedElementsRecursive(dt, elChild, level);
            }

        }
        /// <summary>
        /// Add Tagged Values of element to current data table row
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="el"></param>
        /// <param name="dataRow"></param>
        private static void AddTaggedValues(DataTable dt, EA.Element el, DataRow dataRow)
        {
            foreach (EA.TaggedValue tv in el.TaggedValues)
            {
                if (!_tv.ContainsKey(tv.Name))
                {
                    _tv.Add(tv.Name, null);
                    dt.Columns.Add(tv.Name, typeof(string));
                }

                string value = tv.Value;
                if (value.StartsWith("<memo>")) value = tv.Notes;
                dataRow[$"{tv.Name}"] = value;
            }
        }
        /// <summary>
        /// Get list of strings from a comma separated string.
        /// </summary>
        /// <param name="commaSeparated"></param>
        /// <returns></returns>
        private static string[] GetEaFromCommaSeparatedList(string commaSeparated)
        {
            if (String.IsNullOrWhiteSpace(commaSeparated)) return new string[0];
            // delete special characters like blank, linefeed, ', ""
            commaSeparated = Regex.Replace(commaSeparated, @"\r\n?|\n|'|""", "");

            // allow different delimiters
            commaSeparated = Regex.Replace(commaSeparated, @"  ", " ");
            commaSeparated = Regex.Replace(commaSeparated, @";|:| ", ",");

            return commaSeparated.Split(',');
        }
    }
}
