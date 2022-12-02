using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using EA;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.SQL
{
    /// <summary>
    /// SQL Utilities like:
    /// - Embedded Elements
    /// - User
    /// </summary>
    public class UtilSql
    {
        /// <summary>
        /// List of databases supported as backend for an EA repository
        /// 0 - MySql
        ///	1 - SqlSvr
        /// 2 - AdoJet
        /// 3 - ORACLE
        /// 4 - POSTGRES
        /// 5 - Asa
        /// 7 - OPENEDGE
        /// 8 - ACCESS2007
        /// 9 - FireBird
        /// 10 - SQLite
        /// </summary>
        public enum RepositoryType
        {
            MySql,      // 0 DBType number of EA connection string
            SqlSvr,     // 1
            AdoJet,     // 2
            Oracle,     // 3
            Postgres,   // 4
            Asa,        // 5
            Openedge,   // 7
            Access2007, // 8
            Firebird,   // 9
            SQLite,     // 10
            Other,
            Unknown
        }
        readonly Repository _rep;
        #region Constructor
        public UtilSql(Repository rep)
        {
            _rep = rep;
        }
        #endregion

        /// <summary>
        /// Gets the rep type for this model
        /// </summary>
        /// <returns></returns>
        public static RepositoryType GetRepositoryType(EA.Repository rep)
        {
            string connectionString = rep.ConnectionString;
            RepositoryType repoType = RepositoryType.AdoJet; //default to .eap file

            // if it is a .feap file then it surely is a Firebird db
            if (connectionString.ToLower().EndsWith(".feap", StringComparison.Ordinal))
            {
                repoType = RepositoryType.Firebird;
            }
            else
            {
                //if it is a .eap file we check the size of it. if less then 1 MB then it is a shortcut file and we have to open it as a text file to find the actual connection string
                if (connectionString.ToLower().EndsWith(".eap", StringComparison.CurrentCulture) ||
                    connectionString.ToLower().EndsWith(".eapx", StringComparison.CurrentCulture))
                {
                    var fileInfo = new System.IO.FileInfo(connectionString);
                    if (fileInfo.Length > 1000)
                    {
                        //local .eap / *.eapx file, ms access syntax
                        repoType = RepositoryType.AdoJet;
                    }
                    else
                    {
                        //open the file as a text file to find the connection string.
                        var fileStream = new System.IO.FileStream(connectionString, System.IO.FileMode.Open,
                            System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                        var reader = new System.IO.StreamReader(fileStream);
                        //replace connection string with the file contents
                        connectionString = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                if (! (connectionString.ToLower().EndsWith(".eap", StringComparison.CurrentCulture) ||
                       connectionString.ToLower().EndsWith(".eapx", StringComparison.CurrentCulture)
                       ))
                {
                    string dbTypeString = "DBType=";
                    int dbIndex = connectionString.IndexOf(dbTypeString, StringComparison.CurrentCulture) +
                                  dbTypeString.Length;
                    if (dbIndex > dbTypeString.Length)
                    {
                        string dbNumberString = connectionString.Substring(dbIndex, 1);
                        if (int.TryParse(dbNumberString, out int dbNumber))
                        {
                            repoType = (RepositoryType)dbNumber;
                        }
                    }
                }
                //if it is a .qea file we check the size of it. if less then 1 MB then it is a shortcut file and we have to open it as a text file to find the actual connection string
                if (connectionString.ToLower().EndsWith(".qea", StringComparison.CurrentCulture) ||
                    connectionString.ToLower().EndsWith(".qeax", StringComparison.CurrentCulture))
                {
                    var fileInfo = new System.IO.FileInfo(connectionString);
                    if (fileInfo.Length > 1000)
                    {
                        //local .qea / *.qeax file, ms access syntax
                        repoType = RepositoryType.SQLite;
                    }
                    else
                    {
                        //open the file as a text file to find the connection string.
                        var fileStream = new System.IO.FileStream(connectionString, System.IO.FileMode.Open,
                            System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                        var reader = new System.IO.StreamReader(fileStream);
                        //replace connection string with the file contents
                        connectionString = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                if (!(connectionString.ToLower().EndsWith(".qea", StringComparison.CurrentCulture) ||
                       connectionString.ToLower().EndsWith(".qeax", StringComparison.CurrentCulture)
                       ))
                {
                    string dbTypeString = "DBType=";
                    int dbIndex = connectionString.IndexOf(dbTypeString, StringComparison.CurrentCulture) +
                                  dbTypeString.Length;
                    if (dbIndex > dbTypeString.Length)
                    {
                        string dbNumberString = connectionString.Substring(dbIndex, 1);
                        if (int.TryParse(dbNumberString, out int dbNumber))
                        {
                            repoType = (RepositoryType)dbNumber;
                        }
                    }
                }
            }
            return repoType;
        }

        /// <summary>
        /// Replace the wild cards in the given sql query string to match either MSAccess or ANSI syntax. It works for:
        /// <para />
        /// % or * or #WC# Any character
        /// <para />
        /// _ or ? a single character
        /// <para />
        /// '^' or '!' a shortcut for XOR
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sqlQuery">the sql string to edit</param>
        /// <param name="repositoryType"></param>
        /// <returns>the same sql query, but with its wild cards replaced according to the required syntax</returns>
        public static string ReplaceSqlWildCards(EA.Repository rep, string sqlQuery, RepositoryType repositoryType = RepositoryType.Unknown)
        {
            if (repositoryType == RepositoryType.Unknown) repositoryType = GetRepositoryType(rep);
            bool isJet = repositoryType == RepositoryType.AdoJet;
            int beginLike = sqlQuery.IndexOf("like", StringComparison.InvariantCultureIgnoreCase);
            if (beginLike > 1)
            {
                // Handle ' and " to encapsulate strings
                int beginString1 = sqlQuery.IndexOf("'", beginLike + "like".Length, StringComparison.CurrentCulture);
                int beginString2 = sqlQuery.IndexOf(@"""", beginLike + "like".Length, StringComparison.CurrentCulture);
                int beginString = beginString1 > -1 ? beginString1 : beginString2;
                if (beginString > 0)
                {
                    int endString1 = sqlQuery.IndexOf("'", beginString + 1, StringComparison.CurrentCulture);
                    int endString2 = sqlQuery.IndexOf(@"""", beginString + 1, StringComparison.CurrentCulture);
                    int endString = beginString1 > 0 ? endString1 : endString2;
                    if (endString > beginString)
                    {
                        string originalLikeString = sqlQuery.Substring(beginString + 1, endString - beginString);
                        string likeString = originalLikeString;
                        if (isJet)
                        {
                            likeString = likeString.Replace("#WC#", "*");
                            likeString = likeString.Replace('%', '*');
                            likeString = likeString.Replace('_', '?');
                            likeString = likeString.Replace('^', '!');
                            
                        }
                        else
                        {
                            likeString = likeString.Replace("#WC#", "%");
                            likeString = likeString.Replace('*', '%');
                            likeString = likeString.Replace('?', '_');
                            likeString = likeString.Replace('#', '_');
                            likeString = likeString.Replace('^', '!');

                        }
                        string next = string.Empty;
                        if (endString < sqlQuery.Length)
                        {
                            next = ReplaceSqlWildCards(rep, sqlQuery.Substring(endString + 1), repositoryType);
                        }
                        sqlQuery = sqlQuery.Substring(0, beginString + 1) + likeString + next;

                    }
                }
            }
            return sqlQuery.Trim();
        }
        /// <summary>
        /// Make EA xml from a DataTable (for column names) and the ordered Enumeration provided by LINQ. Set the Captions in DataTable to ensure column names. 
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static string MakeXml(DataTable dt, OrderedEnumerableRowCollection<DataRow> rows)
        {
            XElement xFields = new XElement("Fields");
            foreach (DataColumn col in dt.Columns)
            {
                XElement xField = new XElement("Field");
                xField.Add(new XAttribute("name", col.Caption));
                xFields.Add(xField);
            }
            try
            {
                XElement xRows = new XElement("Rows");

                foreach (var row in rows)
                {
                    XElement xRow = new XElement("Row");
                    int i = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        XElement xField = new XElement("Field");
                        xField.Add(new XAttribute("value", row[i].ToString()));
                        xField.Add(new XAttribute("name", col.Caption));
                        xRow.Add(xField);
                        i = i + 1;
                    }
                    xRows.Add(xRow);
                }
                XElement xDoc = new XElement("ReportViewData");
                xDoc.Add(xFields);
                xDoc.Add(xRows);
                return xDoc.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}", @"Error enumerating through LINQ query");
                return "";
            }
        }
        #region Empty Query Result

        /// <summary>
        /// Empty Query Result
        /// </summary>
        /// <returns></returns>
        static string EmptyQueryResult()
        {
            return new XDocument(
                new XElement("ReportViewData",
                    new XElement("Fields",
                        new XElement("Field", new XAttribute("name", "Empty"))
                    ),
                    new XElement("Rows",
                        new XElement("Row",
                            new XElement("Field",
                                new XAttribute("name", "Empty"),
                                new XAttribute("value", "__empty___")))

                    )
                )).ToString();
            #endregion
        }
        /// <summary>
        /// Make DataTable from EA sql results
        /// </summary>
        /// <param name="sqlXml"></param>
        /// <returns></returns>
        public static DataTable MakeDataTableFromSqlXml(string sqlXml)
        {

            DataSet dataSet = new DataSet();
            dataSet.ReadXml(new StringReader(XElement.Parse(sqlXml).Descendants("Data").FirstOrDefault()?.ToString()));
            return dataSet.Tables[0];
        }

    #region getAndSortEmbeddedElements
        /// <summary>
        /// Get embedded elements and sort them according to name (ASC)
        /// - objectType which embedded element is selected
        /// - stereotype: Inner part of SQL in clause
        /// - direction:  If stereotype != "" then this is the sort order of the column stereotype
        /// </summary>
        /// <param name="el"></param>
        /// <param name="objectType"></param>
        /// <param name="stereotype"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<int> GetAndSortEmbeddedElements(EA.Element el, string objectType, string stereotype, string direction)
        {
            var lPorts = new List<int>();

            string queryStereotype = "";
            string queryOrderBy = @" order by o.name ";
            if (stereotype != "")
            {
                queryStereotype = @" o.stereotype in ({2}) AND";
                queryOrderBy = @" order by o.stereotype {3}, o.name  ";
            }
            
            string queryObjectType = "";
            if (objectType != "") queryObjectType = @" o.object_type = '{1}'  AND ";


            string query = @"SELECT o.object_id As [OBJECT_ID]" +
                           @"from t_object o " +
                           @"where " +
                           queryObjectType +
                           queryStereotype +
                           @"      o.ParentID = {0}  " +
                          queryOrderBy;
            query = String.Format(query, el.ElementID, objectType, stereotype, direction);
                          

            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            foreach (XElement xEle in xelement.Descendants("Row"))
            {
                lPorts.Add(Convert.ToInt32(xEle.Element("OBJECT_ID").Value));
            }

            return lPorts;
            
        }
        #endregion

        #region userHasPermission
        public Boolean UserHasPermission(string userGuid)
        {
            string query = @"SELECT 'Group' As PermissionType " +
                        @"from (t_secgrouppermission p inner join t_secusergroup grp on (p.GroupID = grp.GroupID)) " +
                        @"where grp.UserID = '" + userGuid + "' " +
                        @"UNION " +
                        @"select 'User'  from t_secuserpermission p " +
                        @"where p.UserID = '" + userGuid + "' ;";

            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            // something found????
            var result = xelement.Descendants("Row").Any();
            
            return result;
        }
        #endregion
        #region isConnectionAvailable
        public Boolean IsConnectionAvailable(EA.Element srcEl, EA.Element trgtEl)
        {
            string sql = "SELECT Start_Object_ID  " +
                         " From t_connector " +
                         " where Start_Object_ID in ( {0},{1} ) AND " +
                         "       End_Object_ID in  ( {0},{1} ) ";
             string query = String.Format(sql, srcEl.ElementID, trgtEl.ElementID);

            
            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            // something found????
            var result = xelement.Descendants("Row").Any();

            return result;
        }
        #endregion

        #region getUsers
        /// <summary>
        /// Get users of EA element
        /// - t_secuser
        /// </summary>
         /// <returns></returns>
        public List<string> GetUsers()
        {
            var l = new List<string>();
            string query;
            if (_rep.IsSecurityEnabled)
            {
                // authors under security
                query = @"select UserLogin As [User] from t_secuser order by 1";
            }
            else {
                // all used authors
                query = @"select distinct Author As [User] from t_object order by 1";
            }
            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            foreach (XElement xEle in xelement.Descendants("Row"))
            {
                l.Add(xEle.Element("User").Value);
            }

            return l;
        }
        #endregion
        /// <summary>
        /// Get list of strings from a SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public List<string> GetListOfStringFromSql(string sql, string columnName)
        {
            List<string> l = new List<string>();
            string str = _rep.SQLQuery(sql);
            XElement xelement = XElement.Parse(str);
            foreach (XElement xEle in xelement.Descendants("Row"))
            {
                l.Add(xEle.Element(columnName).Value);
            }

            return l;
        }

        /// <summary>
        /// Get list of strings from a SQL
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static List<string> GetListOfStringFromSql(EA.Repository rep, string sql, string columnName)
        {
            List<string> l = new List<string>();
            string str = rep.SQLQuery(sql);
            XElement xelement = XElement.Parse(str);
            foreach (XElement xEle in xelement.Descendants("Row"))
            {
                l.Add(xEle.Element(columnName).Value);
            }

            return l;
        }

        public static bool SqlUpdate(EA.Repository rep, string updateString)
        {
            try
            {
                rep.Execute(updateString);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Update:\r\n{updateString}\r\n\r\n{e}", @"Error update SQL");
                return false;
            }
            return true;
        }
        
    }
}
