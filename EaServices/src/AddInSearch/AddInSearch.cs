using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils;
using LinqToDB.DataProvider;

namespace hoTools.EaServices.AddInSearch
{
    /// <summary>
    /// Add-In Searches 
    /// </summary>
    public class AddInSearches
    {
        // Dictionary of all TaggedValues of a search
        private static Dictionary<string, string> _tv = new Dictionary<string, string>();

        ///  <summary>
        ///  Add-In simple Search to find nested Elements for:
        ///  -  Selected
        ///  -- Package
        ///  -- Element
        ///  - Comma separated GUID list of Packages or Elements in 'Search Term'  
        /// 
        ///   It outputs:
        ///  - All elements in it's hierarchical structure
        ///  - Tagged Values
        /// 
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
        public static string SearchObjectsNestedUsingEaApi(EA.Repository repository, string searchText)
        {
            // 1. Collect data into a data table
            DataTable dt = AddinSearchObjectsNestedInitTableUsingEaApi(repository, searchText);
            // 2. Order, Filter, Join, Format to XML
            return AddinSearchObjectsNestedMakeXml(dt);
        }

        /// <summary>
        /// Search objects nested optimized
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="searchText"></param>
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
        /// InitializeTable for search results using EA API
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="searchText">Optional: List of packages, EA elements as a comma separated list</param>
        /// <returns></returns>
        private static DataTable AddinSearchObjectsNestedInitTableUsingEaApi(EA.Repository rep, string searchText)
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
                    if (el.Type != "Package") NestedElementsRecursiveUsingEaApi(dt, el, level);
                    if (el.Type == "Package") NestedPackageElementsRecursiveUsingEaApi(dt, rep.GetPackageByGuid(txt), level);
                }

                EA.Package pkg = rep.GetPackageByGuid(searchText);
                if (pkg != null)
                {
                    NestedPackageElementsRecursiveUsingEaApi(dt, pkg, level);
                }
            }
            else
            {
                // handle context element
                rep.GetContextItem(out var item);
                if (item is EA.Element element) NestedElementsRecursiveUsingEaApi(dt, element, level);
                if (item is EA.Package package) NestedPackageElementsRecursiveUsingEaApi(dt, package, level);
            }
            return dt;
        }

        /// <summary>
        /// InitializeTable for search results using EA API
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="searchText">Optional: List of packages, EA elements as a comma separated list</param>
        /// <returns></returns>
        private static DataTable AddinSearchObjectsNestedInitTable(EA.Repository rep, string searchText)
        {
            // get connection string of repository
            string connectionString = LinqUtil.GetConnectionString(rep, out var provider);

            _tv = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            dt.Columns.Add("CLASSGUID", typeof(string));
            dt.Columns.Add("CLASSTYPE", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Alias", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            if (searchText.Trim().StartsWith("{"))
            {
                // handle string
                string[] guidList = GetEaFromCommaSeparatedList(searchText);
                foreach (var guid in guidList)
                {
                    EA.Element el = rep.GetElementByGuid(guid);
                    if (el == null) continue;
                    if (el.Type != "Package")
                    {
                        NestedElementsRecursive(connectionString, provider, dt, rep.GetPackageByID(el.PackageID).PackageGUID, el.ElementID);
                    }
                    if (el.Type == "Package") NestedPackageElementsRecursive(connectionString, provider, dt, guid);
                }

                EA.Package pkg = rep.GetPackageByGuid(searchText);
                if (pkg != null)
                {
                    NestedPackageElementsRecursive(connectionString, provider, dt, pkg.PackageGUID);
                }
            }
            else
            {
                // handle context element
                rep.GetContextItem(out var item);
                if (item is EA.Element element) NestedElementsRecursive(connectionString, provider, dt, rep.GetPackageByID(element.PackageID).PackageGUID, element.ElementID);
                if (item is EA.Package package) NestedPackageElementsRecursive(connectionString, provider, dt, package.PackageGUID);
            }
            return dt;
        }

        /// <summary>
        /// Make a Data Table from Package Elements recursive using EA API
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pkg"></param>
        /// <param name="level"></param>
        private static void NestedPackageElementsRecursiveUsingEaApi(DataTable dt, EA.Package pkg, int level)
        {
            foreach (EA.Element el in pkg.Elements)
            {
                NestedElementsRecursiveUsingEaApi(dt, el, level);
            }

        }

        /// <summary>
        /// Make a Data Table from Package Elements recursive using EA API
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="dt"></param>
        /// <param name="guid"></param>
        /// <param name="connectionString"></param>
        private static void NestedPackageElementsRecursive(string connectionString, IDataProvider provider, DataTable dt, string guid)
        {
            Dictionary<int, NestedObject> nestedElements = GetPackageNestedElements(connectionString, provider, guid);
            AddElementsToTable(nestedElements, dt, 0, 0);

        }

        /// <summary>
        /// Get Dictionary of nested elements for a package GUID.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="guid"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static Dictionary<int, NestedObject> GetPackageNestedElements(string connectionString, IDataProvider provider, string guid)
        {

            using (var db = new DataModels.EaDataModel(provider, connectionString))
            {
                // optimize Access to database, without this the query takes > 30 seconds
                // Split query and group into two queries. 
                // - The first query make the db acccess and writes to array
                // - The second query makes the grouping
                var reqTvAll0 = (from r in db.t_object
                                 join tv2 in db.t_objectproperties on r.Object_ID equals tv2.Object_ID into tv1
                                 from tv in tv1.DefaultIfEmpty()
                                 join pkg in db.t_package on r.Package_ID equals pkg.Package_ID
                                 where pkg.ea_guid == guid
                                 orderby r.Object_ID, tv.Property
                                 select new { r, tv }).ToArray();

                return (from r in reqTvAll0
                        group r by new { r.r.Object_ID, r.r.ParentID, r.r.TPos, r.r.Name, r.r.Alias, r.r.Note, r.r.ea_guid, r.r.Object_Type } into grp1
                        select new
                        {
                            Id = grp1.Key.Object_ID,
                            Property = new NestedObject(
                                grp1.Key.ParentID ?? 0,
                                grp1.Key.TPos ?? 0,
                                grp1.Key.Name ?? "",
                                grp1.Key.Alias ?? "",
                                grp1.Key.Note ?? "",
                                grp1.Key.ea_guid ?? "",
                                grp1.Key.Object_Type ?? "",
                                // Tagged Value
                                grp1.Select(g => new Tv(
                                    g?.tv?.Property ?? "",
                                    ((g?.tv?.Value ?? "") == "<memo>") ? g?.tv?.Notes ?? "" : g?.tv?.Value ?? "")
                                ).ToList())
                        }).ToDictionary(ta => ta.Id, ta => ta.Property);
            }

        }

        /// <summary>
        /// Get Dictionary of nested elements for a package GUID.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static Dictionary<int, NestedObject> GetElementNestedElements(string connectionString, IDataProvider provider, int pkgId, IndexOutOfRangeException elId)
        {

            using (var db = new DataModels.EaDataModel(provider, connectionString))
            {
                // optimize Access to database, without this the query takes > 30 seconds
                // Split query and group into two queries. 
                // - The first query make the db acccess and writes to array
                // - The second query makes the grouping
                var reqTvAll0 = (from r in db.t_object
                                 join pkg in db.t_package on r.Package_ID equals pkg.Package_ID
                                 join tv in db.t_objectproperties on r.Object_ID equals tv.Object_ID
                                 where pkg.Package_ID == pkgId
                                 orderby r.Object_ID, tv.Property
                                 select new { r, tv }).ToArray();

                return (from r in reqTvAll0
                        group r by new { r.r.Object_ID, r.r.ParentID, r.r.TPos, r.r.Name, r.r.Alias, r.r.Note, r.r.ea_guid, r.r.Object_Type } into grp1
                        select new
                        {
                            Id = grp1.Key.Object_ID,
                            Property = new NestedObject(
                                grp1.Key.ParentID ?? 0,
                                grp1.Key.TPos ?? 0,
                                grp1.Key.Name,
                                grp1.Key.Alias,
                                grp1.Key.Note,
                                grp1.Key.ea_guid,
                                grp1.Key.Object_Type,
                                // Tagged Value
                                grp1.Select(g => new Tv(
                                    g?.tv?.Property??"",
                                    ((g?.tv?.Value ?? "") == "<memo>") ? g?.tv?.Notes??"" : g?.tv?.Value??"")
                                ).ToList())
                        }).ToDictionary(ta => ta.Id, ta => ta.Property);
            }

        }
        /// <summary>
        /// Add elements to Data Table
        /// </summary>
        /// <param name="nestedElements"></param>
        /// <param name="dt"></param>
        /// <param name="parentId"></param>
        /// <param name="level"></param>
        private static void AddElementsToTable(Dictionary<int, NestedObject> nestedElements, DataTable dt, int parentId, int level)
        {
            // output current element


            var req = from r in nestedElements
                          //join tv in reqTvAll on r.Object_ID equals tv.Object_ID
                      where r.Value.ParentId == parentId
                      orderby r.Value.TPos
                      select r;
            foreach (var r in req)
            {
                NestedElementToDataTable(dt, r, level);
                AddElementsToTable(nestedElements, dt, r.Key, level + 1);
            }


        }
        /// <summary>
        /// Nested Elements to Data Table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nestedObject"></param>
        /// <param name="level"></param>
        private static void NestedElementToDataTable(DataTable dt, KeyValuePair<int, NestedObject> nestedObject, int level)
        {
            var row = dt.NewRow();
            row["CLASSGUID"] = nestedObject.Value.Guid;
            row["CLASSTYPE"] = nestedObject.Value.ObjectType;
            string t = $"{new string('.', 2 * level)}{nestedObject.Value.Name}";
            row["Name"] = t;
            row["Alias"] = nestedObject.Value.Alias;
            row["Note"] = nestedObject.Value.Notes;
            foreach (Tv tv in nestedObject.Value.Tv)
            {
                if (tv.Property == "") continue;
                if (!_tv.ContainsKey(tv.Property))
                {
                    _tv.Add(tv.Property, null);
                    dt.Columns.Add(tv.Property, typeof(string));
                }

                string value = tv.Value;
                row[$"{tv.Property}"] = value;
            }
            dt.Rows.Add(row);
        }

        /// <summary>
        /// Make a Data Table from nested Elements recursive using EA API
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="el"></param>
        /// <param name="level"></param>
        private static void NestedElementsRecursiveUsingEaApi(DataTable dt, EA.Element el, int level)
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

                NestedElementsRecursiveUsingEaApi(dt, elChild, level);
            }

        }

        /// <summary>
        /// Make a Data Table from nested Elements recursive using EA API
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="dt"></param>
        /// <param name="connectionString"></param>
        /// <param name="pkgGuid"></param>
        /// <param name="elId"></param>
        private static void NestedElementsRecursive(string connectionString, IDataProvider provider, DataTable dt, string pkgGuid, int elId)
        {
            Dictionary<int, NestedObject> nestedElements = GetPackageNestedElements(connectionString, provider, pkgGuid);
            int level = 0;
            KeyValuePair<int, NestedObject> r = new KeyValuePair<int, NestedObject>(elId, nestedElements[elId]);
            // Output the top level
            NestedElementToDataTable(dt, r, level);
            AddElementsToTable(nestedElements, dt, elId, level + 1);


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

                dataRow[$"{tv.Name}"] = GetEaTaggedValue(tv.Value, tv.Notes);
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
        /// <summary>
        /// Get Tagged EA Tagged Value. It handles memo fileds
        /// </summary>
        /// <param name="value"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string GetEaTaggedValue(string value, string note)
        {
            return (value ?? "").StartsWith("<memo>") ? note ?? "" : value ?? "";
        }
    }
}

