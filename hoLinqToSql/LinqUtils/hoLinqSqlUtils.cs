using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils.Extensions;
using LinqToDB.Configuration;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.Access;
using Microsoft.Win32;

namespace hoLinqToSql.LinqUtils
{
    public static class LinqUtil
    {
        ///
        /// ODBC Driver names
        /// See:
        /// https://stackoverflow.com/questions/56442152/microsoft-jet-oledb-4-0-vs-microsoft-ace-oledb-12-0-which-should-i-use
        ///
        /// Here: Access2007 + JET 4.0:  Microsoft.ACE.OLEDB.12.0 (32+64 Bit), maybe Microsoft.ACE.OLEDB.16.0 
        ///       JET 3.0    + JET 4.0:  Microsoft.JET.OLEDB.4.0 (32 Bit)
        ///       SQLite:                 System.Data.Sqlite
        public const string SqLiteOdbcDriver = "System.Data.Sqlite";
        public const string JetDbOdbcDriver = "Microsoft.JET.OLEDB.4.0";
        public const string AccessDbOdbcDriver = "Microsoft.ACE.OLEDB.12.0";
        public const string SqlServerOdbcDriver = "Microsoft.Data.SqlClient";
        /// <Summary>
        /// Convert a IEnumerable to a DataTable.
        /// <TypeParam name="T">Type representing the type to convert.</TypeParam>
        /// <param name="source">List of requested type representing the values to convert.</param>
        /// <returns> returns a DataTable</returns>
        /// </Summary>
        public static DataTable ToDataTable11<T>(this IEnumerable<T> source)
        {
            // Use reflection to get the properties for the type we’re converting to a DataTable.
            var props = typeof(T).GetProperties();

            // Build the structure of the DataTable by converting the PropertyInfo[] into DataColumn[] using property list
            // Add each DataColumn to the DataTable at one time with the AddRange method.
            var dt = new DataTable();
            dt.Columns.AddRange(
                props.Select(p => new DataColumn(p.Name, p.PropertyType.BaseType??"".GetType())).ToArray());
            
            // Populate the property values to the DataTable
            source.ToList().ForEach(
                i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
            );

            return dt;
        }

        /// <summary>
        /// Make a DataTable from a LINQ query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static DataTable ToDataTable22<T>(this IEnumerable<T> source)
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
        /// Example LINQ to SQL
        /// - All object object types
        /// - Count of object types
        /// - Percentage of object types
        /// - Total count
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable RunLinq2Db(IDataProvider provider, string providerName, string connectionString)
        {
            //DataConnection.DefaultSettings = new hoLinq2DBSettings(provider, connectionString);
            try
            {
                {
                    var conBuilder = new LinqToDBConnectionOptionsBuilder();
                    if (provider != null)
                        conBuilder.UseConnectionString(provider, connectionString);
                    else
                        conBuilder.UseConnectionString(providerName, connectionString);

                    var conOption = new LinqToDBConnectionOptions(conBuilder);

                    using (var db = new DataModels.EaDataModel(conOption)) {

                        var count = db.t_object.Count();
                        var q = from c in db.t_object.AsEnumerable()
                            group c by c.Object_Type into g
                            orderby g.Key

                            select new
                            {
                                Type = g.Key,
                                Prozent = $"{ (float)g.Count() * 100 / count:00.00}%",
                                Count = g.Count(),
                                Total = count
                            };

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
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable RunLinq2DbAdvanced(IDataProvider provider, string providerName, string connectionString)
        {
            // set provider and connection string
            // DataConnection.DefaultSettings = new hoLinq2DBSettings(provider, connectionString);
            try
            {
                {
                    var conBuilder = new LinqToDBConnectionOptionsBuilder();
                    if (provider != null)
                        conBuilder.UseConnectionString(provider, connectionString);
                    else
                        conBuilder.UseConnectionString(providerName, connectionString);
                    var conOption = new LinqToDBConnectionOptions(conBuilder);
                    using (var db = new DataModels.EaDataModel(conOption)) {
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
        /// Get connection string of EA database
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="provider">can be null</param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        /// string dsnName = "DSN=MySqlEa;Trusted_Connection=Yes;";
        //  dsnName = "DSN=MySqlEa;";
        public static string GetConnectionString(EA.Repository rep, out IDataProvider provider, out string providerName)
        {
            provider = null;
            providerName = "";
            EaConnectionString eaConnectionString = new EaConnectionString(rep);
            string connectionString = eaConnectionString.DbConnectionString;

            try
            {
                // EAP file 
                // Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\hoData\Work.eap;"
                string dsnConnectionString;
                 switch (rep.RepositoryType())
                {

                    case "JET":
                        provider = new AccessOleDbDataProvider();
                        providerName = JetDbOdbcDriver;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        if (connectionString.ToLower().EndsWith(".eap") || connectionString.ToLower().EndsWith(".eapx"))
                        {

                            return $"Provider={JetDbOdbcDriver};Data Source={connectionString};";
                        }

                        break;
                    case @"SQLSVR":
                        provider = null;
                        providerName = SqlServerOdbcDriver;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        // Add Encrypt=False;
                        if (dsnConnectionString != "")
                        {
                            return $@"{dsnConnectionString};Encrypt=False;";
                        }
                        return $@"{FilterConnectionString(connectionString)};Encrypt=False;";



                    case "MYSQL":
                        provider = null; // new MySqlDataProvider("SQL Server");
                        providerName = "MySqlConnector";

                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        dsnConnectionString = Regex.Replace(dsnConnectionString, "NO_SCHEMA=[01];","");
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return FilterConnectionString(connectionString);

                    case "ACCESS2007":
                        provider = new AccessOleDbDataProvider();
                        providerName = AccessDbOdbcDriver;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        if (connectionString.ToLower().EndsWith(".eap") || connectionString.ToLower().EndsWith(".eapx"))
                        {

                            return $"{AccessDbOdbcDriver};Data Source={connectionString};";
                        }

                        break;
                        
                      
                    case "ASA":
                        provider = null; //new SybaseDataProvider("Sybase");
                        providerName = "AdoNetCore.AseClient";
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return FilterConnectionString(connectionString);

                    case "ORACLE":
                        provider = null;// new OracleDataProvider("Oracle");
                        providerName = "Oracle.ManagedDataAccess";
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return FilterConnectionString(connectionString);


                    case "POSTGRES":
                        provider = null; // new PostgreSQLDataProvider();
                        providerName = "Npgsq";
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return FilterConnectionString(connectionString);

                    case "SQLITE":
                    case "SL3":
                        provider = null; //new SqlServerDataProvider("", SqlServerVersion.v2012);
                        providerName = SqLiteOdbcDriver;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return $@"DataSource={connectionString};";



                    default:
                        MessageBox.Show($@"Database: '{rep.RepositoryType()}'
ConnectionString: '{connectionString}'",
                            "DataBase not supported for hoTools, only Access, SqlServer, MySQL, SQLite/SL3 (*.qea)");
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Type: {rep.RepositoryType()}
ConnectionString: '{connectionString}'

{ex}", "VW Tools: Error connections string");
            }

            return "";
        }

        /// <summary>
        /// Filter connection string:
        /// - First 2 elements delete (e.g.:'EAExample --- DBType=1;Connect=Provider=SQLOLEDB.1'
        /// - Last Element delete: (LazyLoad=1;)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>"" no connection found</returns>
        private static string FilterConnectionString(string connectionString)
        {
            string[] c = connectionString.Split(';').Skip(2).ToArray();
            // no connection string, e.g. Access and SQLite uses a path
            if (c.Length == 0) return connectionString;
            Array.Resize(ref c, c.Length - 2);
            return String.Join(";", c) + ";";
        }

        public static string GetConnectionStringAccess(string path)
        {
            return $"Provider={JetDbOdbcDriver};Data Source={path};";
        }
        /// <summary>
        /// Get ConnectionOptions
        ///
        /// From the path it gets the LINQDB options.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LinqToDBConnectionOptions GetConnectionOptions(string path)
        {

            var conBuilder = new LinqToDBConnectionOptionsBuilder();
            var providerName = JetDbOdbcDriver;
            if (path.ToLower().EndsWith(".qea") || path.ToLower().EndsWith(".qeax")) providerName = SqLiteOdbcDriver;
            conBuilder.UseConnectionString(providerName, path);
            var conOption = new LinqToDBConnectionOptions(conBuilder);
            return conOption;
        }

        /// <summary>
        /// Get ConnectionOptions
        /// 
        /// From the path it gets the LINQDB options.
        /// </summary>
        /// <returns></returns>
        public static LinqToDBConnectionOptions GetConnectionOptions(EA.Repository rep)
        {
            string connectionString = GetConnectionString(rep, out IDataProvider _, out string providerName);

            var conBuilder = new LinqToDBConnectionOptionsBuilder();
            conBuilder.UseConnectionString(providerName, connectionString);
            var conOption = new LinqToDBConnectionOptions(conBuilder);
            return conOption;
        }


        /// <summary>
        /// Get connectionString if a DSN is part of the connectionString or "" if no DSN available.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        /// 
        public static string GetConnectionStringForDsn(string connectionString)
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
        static string GetConnectionString(RegistryKey rootKey, string dsn)
        {
            string registryKey = $@"Software\ODBC\ODBC.INI\{dsn}";

            RegistryKey key =
                Registry.CurrentUser.OpenSubKey(registryKey);
            if (key == null) return "";

            var l = from k in key.GetValueNames()
                where k.ToLower() != "driver" && (k.ToLower() != "last-user" && k.ToLower() != "lastuser")
                select new
                {
                    Value = k + "=" + key.GetValue(k) + ";"
                };
            return String.Join("", l.Select(i => i.Value).ToArray());
        }
        /// <summary>
        /// Returns an empty Data Table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable GetEmpty(string name)
        {
            var dt = new DataTable();
            dt.TableName = name;
            dt.Columns.Add(name);
            return dt;
        }
    }
}
