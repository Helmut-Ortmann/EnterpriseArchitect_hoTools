using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils.Extensions;
using LinqToDB;
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
        /// - All object  types
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
                    // Get linq2db options
                    var options = GetLinq2DataOptions(provider, providerName, connectionString);
                    using (var db = new DataModels.EaDataModel(options))
                    {
                        var count = db.t_object.Count();
                        var q = from c in db.t_object.AsEnumerable()
                            group c by c.Object_Type
                            into g
                            orderby g.Key

                            select new
                            {
                                Type = g.Key,
                                Prozent = $"{(float)g.Count() * 100 / count:00.00}%",
                                Count = g.Count(),
                                Total = count
                            };

                        return q.ToDataTable();
                    }
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show($"Provider: {provider}\r\nConnection: {connectionString}\r\n\r\n{e}", "hoTools: Error Linq2DB");
                return new DataTable();
            }
            

        }
        /// <summary>
        /// Get linq2db data options
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataOptions GetLinq2DataOptions(IDataProvider provider, string providerName, string connectionString)
        {
            // Handle file based connection string which doesn't contain Data Source=
            if (!connectionString.Contains("Data Source="))
            {
                connectionString = $"Data Source={connectionString};";
            }
            DataOptions options = null;
            try
            {

                if (provider != null)
                {
                    options = new DataOptions()
                        .UseConnectionString(connectionString)
                        .UseDataProvider(provider);

                }
                else
                {
                    options = new DataOptions()
                        .UseConnectionString(connectionString)
                        .UseProvider(providerName);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($@"ConnectionString {connectionString}
ProviderName: '{providerName ?? ""}'
Provider:     '{provider?.Name ?? ""}'

{e}", "Linq2db: can't estimate connection");
            }


            return options;
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
                    // Get linq2db options
                    var conOption = GetLinq2DataOptions(provider, providerName, connectionString);
                    using (var db = new DataModels.EaDataModel(conOption))
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
                        var countReq = db.t_object.Count(e => e.Object_Type == "Requirement");
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
                MessageBox.Show($"Provider: {provider}\r\nConnection: {connectionString}\r\n\r\n{e}", "hoTools: Error Linq2DB");
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
        /// Remove PID from ConnectionString
        ///
        /// Show password as: PWD=***;
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string RemovePasswordFromConnectionString(string connectionString)
        {
            return Regex.Replace(connectionString, "PWD=([^;]*)", "PWD=***", RegexOptions.IgnoreCase);
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
            string connectionString = eaConnectionString.DbConnectionStr;

            try
            {
                // EAP file 
                // Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\hoData\Work.eap;"
                string dsnConnectionString;
                switch (rep.RepositoryType())
                {

                    case "JET":
                        provider = null; // new AccessOleDbDataProvider();
                        providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        if (connectionString.ToLower().EndsWith(".eap") || connectionString.ToLower().EndsWith(".eapx"))
                        {
                            providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
                            return $"Provider={DataModels.DbConfig.GetDbProperties(rep).OdbcDiverName};Data Source={connectionString};";
                        }

                        break;
                    case @"SQLSVR":
                        provider = null; //new SqlServerDataProvider("", SqlServerVersion.v2012);
                        //provider = new SqlServerDataProvider("", SqlServerVersion.v2012)
                        providerName = "Microsoft.Data.SqlClient";
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        // Add Encrypt=False;
                        if (dsnConnectionString != "")
                        {
                            return $@"{dsnConnectionString};Encrypt=False;";
                        }
                        return $@"{FilterConnectionString(connectionString)};Encrypt=False;";



                    case "MYSQL":
                        provider = null; // new MySqlDataProvider("SQL Server");
                        providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;

                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        dsnConnectionString = Regex.Replace(dsnConnectionString, "NO_SCHEMA=[01];", "");
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return FilterConnectionString(connectionString);

                    case "ACCESS2007":
                    case "ACCESS":
                        provider = new AccessOleDbDataProvider();
                        providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        if (connectionString.ToLower().EndsWith(".eap") || connectionString.ToLower().EndsWith(".eapx"))
                        {

                            return $"Provider={DataModels.DbConfig.GetDbProperties(rep).OdbcDiverName};Data Source={connectionString};";
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
                        providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
                        dsnConnectionString = GetConnectionStringForDsn(connectionString);
                        if (dsnConnectionString != "") return dsnConnectionString;
                        return $@"Data Source={connectionString};";




                    default:
                        MessageBox.Show($@"Database: '{rep.RepositoryType()}'
ConnectionString: '{connectionString}'",
                            "DataBase not supported, only JET, Access, SqlServer, MySQL, SQLite/SL3 (*.qea)");
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Type: {rep.RepositoryType()}
ConnectionString: '{connectionString}'

{ex}", "hoTools: Error connections string");
            }

            return "";
        }

        /// <summary>
        /// Filter EA connection string:
        /// - First 2 elements delete (e.g.:'EAExample --- DBType=1;Connect=Provider=SQLOLEDB.1'
        /// - Last Element delete: (LazyLoad=1;)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>"" no connection found</returns>
        private static string FilterConnectionString(string connectionString)
        {
            if (connectionString.Contains(" --- "))
            {
                string[] c = connectionString.Split(';').Skip(2).ToArray();
                // no connection string, e.g. Access and SQLite uses a path
                if (c.Length == 0) return connectionString;
                Array.Resize(ref c, c.Length - 2);
                return String.Join(";", c) + ";";
            }
            return connectionString;
        }

        public static string GetConnectionStringAccess(EA.Repository rep, string path)
        {

            return $"Provider={DataModels.DbConfig.GetDbProperties(rep).OdbcDiverName};Data Source={path};";
        }
        /// <summary>
        /// Get ConnectionOptions from path of Repository
        /// Doesn't work for MySQL or other non file based databases.
        /// 
        /// From the path it gets the LINQDB options:
        /// - qea  "System.Data.Sqlite"
        /// - eap  JetDbOdbcDriver      (32 bit),    JET 3
        /// - eapx AccessDbOdbcDriver   (32+64 bit), JET 4
        /// - eadb AccessDbOdbcDriver   (32+64 Bit)
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static DataOptions GetConnectionOptions(EA.Repository rep)
        {
            EaConnectionString eaConnectionString = new EaConnectionString(rep);
            string connectionString = eaConnectionString.DbConnectionStr;

            var providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
            if (connectionString.ToLower().EndsWith(".qea") || connectionString.ToLower().EndsWith(".qeax")) providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
            if (connectionString.ToLower().EndsWith(".eapx") || connectionString.ToLower().EndsWith("eadb")) providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;
            if (connectionString.ToLower().EndsWith(".eap")) providerName = DataModels.DbConfig.GetDbProperties(rep).LinqName;

            // Get linq2db options
            var conOption = GetLinq2DataOptions(null, providerName, connectionString);
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
