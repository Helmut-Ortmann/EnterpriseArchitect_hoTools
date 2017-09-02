using System;
using System.Collections.Generic;
using System.Data;
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
        /// Get connection string of database
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// string dsnName = "DSN=MySqlEa;Trusted_Connection=Yes;";
        //  dsnName = "DSN=MySqlEa;";
        public static string GetConnectionString(EA.Repository rep, out IDataProvider provider)
        {
            provider = null;
            string connectionString = rep.ConnectionString;
            string dsnConnectionString;
            // EAP file 
            // Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\hoData\Work.eap;"
            switch (rep.RepositoryType())
            {

                case "JET":
                    provider = new AccessDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    if (connectionString.ToLower().EndsWith(".eap"))
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
                    provider = new MySqlDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);

                case "ACCESS2007":
                    provider = new AccessDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);


                case "ASA":
                    provider = new SybaseDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);

                case "ORACLE":
                    provider = new OracleDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);


                case "POSTGRES":
                    provider = new PostgreSQLDataProvider();
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);



                default:
                    MessageBox.Show($"Database: {rep.RepositoryType()}\r\nConnectionString:{connectionString} ","DataBase not supported for hoTools, only Access, SqlServer and MySQL");
                    break;

            }
            return "";
        }

        /// <summary>
        /// Filter connection string:
        /// - First 2 elements delete (e.g.:'EAExample --- DBType=1;Connect=Provider=SQLOLEDB.1'
        /// - Last Element delete: (LazyLoad=1;)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static string FilterConnectionString(string connectionString)
        {
            string[] c = connectionString.Split(';').Skip(2).ToArray();
            Array.Resize(ref c, c.Length - 2);
            return String.Join(";", c) + ";";
        } 
        /// <summary>
        /// Get connectionString if a DSN is part of the connectionString or "" if no DSN available.
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
        static string GetConnectionString(RegistryKey rootKey, string dsn)
        {
            string registryKey = $@"Software\ODBC\ODBC.INI\{dsn}";

            RegistryKey key =
                Registry.CurrentUser.OpenSubKey(registryKey);
            if (key == null) return "";

            var l = from k in key.GetValueNames()
                where k.ToLower() != "driver" && k.ToLower() != "lastuser"
                select new
                {
                    Value = k + "=" + key.GetValue(k) + ";"
                };
            return String.Join("", l.Select(i => i.Value).ToArray());
        }
    }
}
