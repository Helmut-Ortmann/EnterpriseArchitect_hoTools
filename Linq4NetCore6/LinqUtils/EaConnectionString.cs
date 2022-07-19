using System;

namespace LinqToSql
{
    /// <summary>
    /// Handle EA ConnectionString
    ///
    /// If the EA-ConnectionString is an *.eap[x] file than:
    /// 1. If *.eap[x] file smaller than 1,000 byte ==> connectionString is file content
    /// </summary>
    public class EaConnectionString
    {
        private readonly EA.Repository _rep;
        private readonly string _dbConnectionString;
        private readonly string _eaConnectionString;

        /// <summary>
        /// The Database connection string. It a file (*.eap[x] or a database/web connection string)
        /// </summary>
        public string DbConnectionString => _dbConnectionString;

        public EaConnectionString(EA.Repository rep)
        {
            _rep = rep;
            if (_rep == null) return;
            _eaConnectionString = _rep.ConnectionString;
            _dbConnectionString = _rep.ConnectionString; // default

            //if it is a .eap file we check the size of it. if less then 1 MB then it is a shortcut file and we have to open it as a text file to find the actual connection string
            if (_eaConnectionString.ToLower().EndsWith(@".eap", StringComparison.CurrentCulture) ||
                _eaConnectionString.ToLower().EndsWith(@".eapx", StringComparison.CurrentCulture))
            {
                var fileInfo = new System.IO.FileInfo(_eaConnectionString);
                if (fileInfo.Length > 1000)
                {
                    _dbConnectionString = _eaConnectionString;
                }
                else
                {
                    //open the file as a text file to find the connection string.
                    var fileStream = new System.IO.FileStream(_eaConnectionString, System.IO.FileMode.Open,
                        System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    var reader = new System.IO.StreamReader(fileStream);
                    //replace connection string with the file contents
                    _dbConnectionString = reader.ReadToEnd().Substring("EAConnectString:".Length);
                    reader.Close();
                }
            }


        }
    }

}
