using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LinqToDB.Data;
using Microsoft.Win32;

namespace hoLinqToSql.Utils
{
    public static class LinqlUtil
    {

        /// <summary>
        /// Make a DataTable from a LINQ query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
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

        public static DataTable RunLinq2Db(string provider, string connectionString)
        {
            DataConnection.DefaultSettings = new hoLinq2DBSettings(provider, connectionString);
            string ret = "";
            try
            {
                {
                    
                    using (var db = new DataModels.EaDataModel())
                    {
                        var q =
                        (from c in db.t_object.AsEnumerable()
                            group c by c.Object_Type into g
                            orderby g.Key

                            select new
                            {
                                Type = g.Key,
                                Count = g.Count()
                            }) ;

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
        /// Get connection string of database
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// string dsnName = "DSN=MySqlEa;Trusted_Connection=Yes;";
        //  dsnName = "DSN=MySqlEa;";
        public static string GetConnectionString(EA.Repository rep, out string provider)
        {
            provider = "";
            string connectionString = rep.ConnectionString;
            string dsnConnectionString = "";
            // EAP file 
            // Provider=Microsoft.Jet.OLEDB.4.0;Data Source=d:\hoData\Work.eap;"
            switch (rep.RepositoryType())
            {

                case "JET":
                    provider = "Access";
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    if (connectionString.ToLower().EndsWith(".eap"))
                    {
                        
                        return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={connectionString};";
                    }
                    break;
                case "SQLSVR":
                // EA: 'EAExample --- DBType=1;Connect=Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=EAExample;Data Source=localhost\SQLEXPRESS;LazyLoad=1;'
                // Linq2DB: Server=localhost\SQLEXPRESS;Database=<dataBase>;Trusted_Connection=True;
                    provider = "System.Data.SqlClient";
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    return FilterConnectionString(connectionString);



                case "MYSQL":
                    provider = "MySql.Data.MySqlClient";
                    dsnConnectionString = GetConnectionStringForDsn(connectionString);
                    if (dsnConnectionString != "") return dsnConnectionString;
                    //return @"Server = localhost; Port = 3306; Database = eaexamplemysql; Uid = root; Pwd = m6655inden; charset = utf8;";
                    return FilterConnectionString(connectionString);

                default:
                    MessageBox.Show($@"Database: {rep.RepositoryType()}\r\nConnectionString:{connectionString} ","DataBase not supported for hoTools, only Access, SqlServer and MySQL");
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
        /// Get ConnectionString vom ODBC DSN
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
                where k != "Driver"
                select new
                {
                    Value = k + "=" + key.GetValue(k) + ";"
                };
            var a = l.ToArray();
            return String.Join("", l.Select(i => i.Value).ToArray());
        }
    }
}
