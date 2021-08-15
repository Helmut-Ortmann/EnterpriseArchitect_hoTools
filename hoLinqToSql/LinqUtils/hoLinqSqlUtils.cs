using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

using LinqToDB.DataProvider;
using LinqToDB.DataProvider.Access;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.Sybase;

namespace hoLinqToSql.LinqUtils
{
    public static class LinqUtil
    {

        /// <summary>
        /// Make a DataTable from a LINQ query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            DataTable output = new DataTable();

            foreach (var prop in properties)
            {
                output.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in source)
            {
                DataRow row = output.NewRow();

                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item, null);
                }

                output.Rows.Add(row);
            }

            return output;
        }
        /// <summary>
        /// Test Query to show making EA xml from a Data table by using MakeXml. It queries the data table, orders the content according to Name columen and outputs it in EA xml format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string QueryAndMakeXmlFromTable(DataTable dt)
        {
            try
            {
                // Make a LINQ query (WHERE, JOIN, ORDER,)
                OrderedEnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
                    orderby row.Field<string>("Name") descending
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
        /// Example LINQ to SQL
        /// - All object object types
        /// - Count of object types
        /// - Percentage of object types
        /// - Total count
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable RunLinq2Db(IDataProvider provider, string connectionString)
        {
            //DataConnection.DefaultSettings = new hoLinq2DBSettings(provider, connectionString);
            try
            {
                {
                    
                    using (var db = new DataModels.EaDataModel(provider, connectionString))
                    {
                        var count = db.t_object.Count();
                        var q = (from c in db.t_object.AsEnumerable()
                            group c by c.Object_Type into g
                            orderby g.Key

                            select new
                            {
                                Type = g.Key,
                                Prozent = $"{ (float)g.Count() * 100 / count:00.00}%",
                                Count = g.Count(),
                                Total = count
                            });

                        return q.ToDataTable();

                    }
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show($"Provider: {provider}\r\nConnection: {connectionString}\r\n\r\n{e}", "Error Linq2DB");
                return new DataTable();
            }
            

        }
        /// <summary>
        /// Example LINQ to SQL
        /// All object types and all Requirement Types
        /// - First object types than requirement types
        /// - Count of object types
        /// - Percentage of object types
        /// - Total count
        /// 
        /// Strategies to see:
        /// - Write multiple queries
        /// - Combine multiple queries
        /// - Use C# code for advanced features 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable RunLinq2DbAdvanced(IDataProvider provider, string connectionString)
        {
            // set provider and connection string
            // DataConnection.DefaultSettings = new hoLinq2DBSettings(provider, connectionString);
            try
            {
                {

                    using (var db = new DataModels.EaDataModel(provider, connectionString))
                    {
                        // Total amount of Object_Types
                        var countObjectTypes = db.t_object.Count();

                        // All object_types summary:
                        // - Type
                        // - Count
                        // - Percentage
                        // - Total count of object_types
                        var q =
                        (from c in db.t_object.AsEnumerable()
                            group c by c.Object_Type into g
                            orderby g.Key

                            select new
                            {
                                Type = $"{g.Key}",
                                Prozent = $"{ (float)g.Count() * 100 / countObjectTypes:00.00}%",
                                Count = g.Count(),
                                Total = countObjectTypes
                            });


                        // Requirement summary:
                        // - Type
                        // - Count
                        // - Percentage
                        // - Total count of requirements
                        var countReq = db.t_object.Where(e => e.Object_Type == "Requirement").Count();
                        var q1 =
                        (from c in db.t_object.AsEnumerable()
                            where c.Object_Type == "Requirement"
                            group c by c.Stereotype into g
                            orderby g.Key

                            select new
                            {
                                Type = $"Req:<<{g.Key}>>",
                                Prozent = $"{ (float)g.Count() * 100 / countReq:00.00}%",
                                Count = g.Count(),
                                Total = countReq
                            });

                        // Concatenate Object Types with Requirement Types
                        var sum = q.Concat(q1);

                        return sum.ToDataTable();

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"Provider: {provider}\r\nConnection: {connectionString}\r\n\r\n{e}", "Error Linq2DB");
                return new DataTable();
            }


        }
        /// <summary>
        /// Get data source from connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string  GetDataSourceFromConnectionString(string connectionString)
        {
            Regex rx = new Regex("DataSource=([^;]*)");
            Match match =  rx.Match(connectionString);
            return match.Success ? match.Groups[1].Value : "";
        }
        /// <summary>
        /// Get connection string for VC Code Symbol table
        /// </summary>
        /// <param name="vcCodeFolderPath">Folder of VC Code C/C++ Source Code</param>
        /// <param name="provider"></param>
        /// <param name="withErrorMessage"></param>
        /// <returns>"" = No connection found</returns>
        public static string GetConnectionString(string vcCodeFolderPath, out IDataProvider provider, bool withErrorMessage= true)
        {
            provider = new SQLiteDataProvider("SQLite.Classic");
            return VcDbUtilities.GetConnectionString(vcCodeFolderPath, withErrorMessage );
        }
        /// <summary>
        /// Get connection string of EA database
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// string dsnName = "DSN=MySqlEa;Trusted_Connection=Yes;";
        //  dsnName = "DSN=MySqlEa;";
        public static string GetConnectionString(EA.Repository rep, out IDataProvider provider)
        {
            provider = null;

                
            var (connectionString, dbType)   = GetConnectionStringFromRepository(rep);
            string dsnConnectionString;
            // EAP file 
            // Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\hoData\Work.eap;"
            switch (dbType)
            {

                case "JET":
                    provider = new AccessOleDbDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    if (connectionString.ToLower().EndsWith(".eap") || connectionString.ToLower().EndsWith(".eapx"))
                    {
                        
                        return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={connectionString};";
                    }
                    break;
                case @"SQLSVR":
                    provider = new SqlServerDataProvider("", SqlServerVersion.v2012);
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);



                case "MYSQL":
                    provider = new MySqlDataProvider("MySQL");
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);

                case "ACCESS2007":
                    provider = new AccessOleDbDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);


                case "ASA":
                    provider = new SybaseDataProvider("Sybase");
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);

                case "ORACLE":
                    provider = new OracleDataProvider("Oracle");
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);


                case "POSTGRES":
                    provider = new PostgreSQLDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);
            }
            MessageBox.Show($"Database: {rep.RepositoryType()}\r\nConnectionString:{connectionString} ", "DataBase not supported, only Access (*.eap/*.eapx), SqlServer and MySQL");
            return "";
        }

        /// <summary>
        /// Filter connection string:
        /// - First 2 elements delete (e.g.:'EAExample --- DBType=1;Connect=Provider=SQLOLEDB.1')
        /// - Last Element delete: (LazyLoad=1;)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>"" no connection found</returns>
        private static string FilterConnectionString(string connectionString)
        {
            try
            {
                // skip first two entries of connection string (Name/DBType;Provider;)
                string[] c = connectionString.Split(';').Skip(2).ToArray();
                if (c.Length < 3)
                {
                    MessageBox.Show($"Connection string: '{connectionString}'\r\n\r\n", "Can't detect correct connection string, empty?");
                    return "";
                }
                string s = String.Join(";", c) + ";";
                // remove LazyLoad=0;
                s = s.Replace("LazyLoad=1", "");
                return s.Replace("LazyLoad=0", "");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Connection string: '{connectionString}'\r\n\r\n{e.Message} ", "Can't decode connection string");
                return "";
            }
            //string[] c = connectionString.Split(';').Skip(2).ToArray();
            //Array.Resize(ref c, c.Length - 2);
            //return String.Join(";", c) + ";";
        } 
        /// <summary>
        /// Get connectionString if a DSN is part of the connectionString or "" if no DSN available.
        /// DSN is Data Source=...;
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static string GetConnectionStringForDsn(string connectionString)
        {
            Regex rgx = new Regex("Data Source=([^;]*);");
            Match match = rgx.Match(connectionString);
            if (match.Success)
            {

                return GetConnectionStringFromDsn(match.Groups[1].Value);

            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Get ConnectionString from ODBC DSN (System, User, no file DSN)
        /// - Supports ODBC System and User DSN
        /// - Concatenates all of the registry entries of the odbc dsn definition 
        /// - Ignores the entries for: Driver, Lastuser
        /// Tested with: Access, SqlServer, MySql
        /// </summary>
        /// <param name="dsn"></param>
        /// <returns></returns>
        static string GetConnectionStringFromDsn(string dsn)
        {
            string con = GetConnectionString(Registry.CurrentUser, dsn);
            if (con == "") con = GetConnectionString(Registry.LocalMachine, dsn);
            return con;

        }
        /// <summary>
        /// Fix from Andreas Nebenführ, not used rootKey
        /// </summary>
        /// <param name="rootKey"></param>
        /// <param name="dsn"></param>
        /// <returns></returns>
        static string GetConnectionString(RegistryKey rootKey, string dsn)
        {
            string registryKey = $@"Software\ODBC\ODBC.INI\{dsn}";

            RegistryKey key =
                rootKey.OpenSubKey(registryKey);
            if (key == null) return "";

            var l = from k in key.GetValueNames()
                where k.ToLower() != "driver" && k.ToLower() != "lastuser"
                select new
                {
                    Value = k + "=" + key.GetValue(k) + ";"
                };
            return String.Join("", l.Select(i => i.Value).ToArray());
        }
        /// <summary>
        /// Get connection string from Repository
        /// Note: The type of repository isn't currently evaluated
        ///
        /// Connection String: 'EAExample --- DBType=1;Connect=Provider=SQLOLEDB.1')
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <returns>ConnectionString, Type of connection as defined by EA</returns>
        static Tuple<string,string> GetConnectionStringFromRepository(EA.Repository rep)
        {
            string cnString = rep.ConnectionString.ToUpper();
            // Check if shortcut in the *.eap
            if (cnString.EndsWith(".EAP") || cnString.Contains(".EAPX"))
            {
                FileInfo f = new FileInfo(cnString);

                // Just a normal *.eap file
                if (f.Length > 1024) return new Tuple<string, string>(rep.ConnectionString, "JET"); 

                // Shortcut file: Should contain the file name or a connection string
                TextReader tr = new StreamReader(cnString);
                // ReSharper disable once PossibleNullReferenceException
                string shortcut = tr.ReadLine().ToUpper();
                tr.Close();

                // find start of connectionstring
                string magicStart = @"EACONNECTSTRING:";
                if (!shortcut.StartsWith(magicStart)) return new Tuple<string, string>(rep.ConnectionString, "JET");

                string connectionString = shortcut.Substring(magicStart.Length);
                string dbType = "";

                // link to *.eap file
                if (!connectionString.Contains(@"DBTYPE=") || connectionString.Contains(".EAP") )
                {
                    dbType = "JET";
                }
                else
                {
                    // get Type Database 0-99
                    Match matchDbType = Regex.Match(connectionString, @"DBTYPE=([0-9]*);");
                    if (matchDbType.Success && matchDbType.Groups.Count == 2)
                    {
                        dbType = OdbcTypeToString(matchDbType.Groups[1].Value);
                    }
                }
                // Estimate DBType from stored number
                
                return new Tuple<string, string>(connectionString, dbType);

            }
            // no shortcut, just return EA values
            return new Tuple<string, string>(rep.ConnectionString, rep.RepositoryType());
        }
        /// <summary>
        /// Convert the ODBC dbType to the string dbtype of EA
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string OdbcTypeToString(string type)
        {
            
            switch (type.Trim())
            {
                case "0":
                    return "MYSQL";
                case "1":
                    return @"SQLSVR";
                case "2":
                    return "ADOJET";
                case "3":
                    return "ORACLE";
                case "4":
                    return "POSTGRES";
                case "5":
                    return "ASA";
                case "6":
                    return "";
                case "7":
                    return "";
                case "8":
                    return "ACCESS2007";
                case "9":
                    return "FIREBIRD";
                default:
                    return "";
            }
            
        }
    }
}
