using System;
using System.Linq;

namespace hoLinqToSql.LinqUtils
{
    /// <summary>
    /// Handle EA ConnectionString
    ///
    /// If the EA-ConnectionString is an *.eap[x] file than:
    /// 1. If *.eap[x] file smaller than 1,000 byte ==> connectionString is file content
    /// </summary>
    public class EaConnectionString
    {
        /// <summary>
        /// The Database connection string. It a file (*.eap[x] or a database/web connection string)
        /// </summary>
        public string DbConnectionStr { get; }

        public string EaConnectionStr => _rep?.ConnectionString ?? "";


        public string DbConnectionStrToShow => LinqUtil.RemovePasswordFromConnectionString(DbConnectionStr); // show password: (PWD=*****;)

        public string EaConnectionStrToShow => LinqUtil.RemovePasswordFromConnectionString(_rep?.ConnectionString ?? ""); // show password: (PWD=*****;)

        private readonly EA.Repository _rep;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rep"></param>
        public EaConnectionString(EA.Repository rep)
        {
            _rep = rep;
            DbConnectionStr = "";
            if (_rep == null) return;
            var eaConnectionString = _rep.ConnectionString;
            DbConnectionStr = _rep.ConnectionString; // default

            //if it is a *.eap/*.qea file we check the size of it. if less then 1 MB then it is a shortcut file and we have to open it as a text file to find the actual connection string
            if (eaConnectionString.ToLower().EndsWith(@".eap", StringComparison.CurrentCulture) ||
                eaConnectionString.ToLower().EndsWith(@".eapx", StringComparison.CurrentCulture) ||
                eaConnectionString.ToLower().EndsWith(@".qea", StringComparison.CurrentCulture) ||
                eaConnectionString.ToLower().EndsWith(@".qeax", StringComparison.CurrentCulture)
                )
            {
                var fileInfo = new System.IO.FileInfo(eaConnectionString);
                if (fileInfo.Length > 1000)
                {
                    DbConnectionStr = eaConnectionString;
                }
                else
                {
                    //open the file as a text file to find the connection string.
                    var fileStream = new System.IO.FileStream(eaConnectionString, System.IO.FileMode.Open,
                        System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    var reader = new System.IO.StreamReader(fileStream);
                    //replace connection string with the file contents
                    DbConnectionStr = reader.ReadToEnd().Substring("EAConnectString:".Length);
                    reader.Close();
                }
            }
            // Adapt the native EA 16.* connection string to a linq2db connection string
            switch (_rep.RepositoryType())
            {
                case "JET":
                    break;

                case "FIREBIRD":
                    break;
                case "ACCESS2007":
                    break;
                case "ASA":
                    break;
                case "SQLSVR":

                    break;
                case "MYSQL":
                    DbConnectionStr = GetMySqlConnectionString(DbConnectionStr);
                    break;
                case "ORACLE":
                    break;
                case "POSTGRES":
                    break;

            }


        }

        /// <summary>
        /// Make a MySQL connection string from EA 16. connection string
        /// </summary>
        /// <param name="eaConnectionString"></param>
        /// <returns></returns>
        private string GetMySqlConnectionString(string eaConnectionString)
        {
            if (eaConnectionString.Contains("DBType=0;Connect=Provider=SSDB;"))
            {
                // Extract the necessary parts from the original string
                string server = eaConnectionString.Split(';').FirstOrDefault(s => s.StartsWith("SRC="))?.Split('=')[1]
                    .Split('@')[0];
                string database = eaConnectionString.Split(';').FirstOrDefault(s => s.StartsWith("SRC="))?.Split('=')[1]
                    .Split('@')[1];
                string uid = eaConnectionString.Split(';').FirstOrDefault(s => s.StartsWith("UID="))?.Split('=')[1];
                string pwd = eaConnectionString.Split(';').FirstOrDefault(s => s.StartsWith("PWD="))?.Split('=')[1];
                // Form the MySQL connection string
                string mySqlConnectionString = $"Server={server};Database={database};Uid={uid};Pwd={pwd};";
                return mySqlConnectionString;
            }

            return eaConnectionString;

        }


    }

}
