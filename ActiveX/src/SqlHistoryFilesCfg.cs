
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace hoTools.Settings
{
    /// <summary>
    /// Administer the recent 20 used SQL files
    /// <para/>- Store and read them in config-file
    /// </summary>
    public class SqlHistoryFilesCfg
    {
        int MAX_FILE_COUNT_TO_REMEMBER = 20;
        const string SQL_HISTORY_FILE_CFG_STRING = "SqlFile";
        /// <summary>
        /// List of files loaded in history. Recent used files
        /// </summary>
        public List<HistoryFile> lSqlHistoryFilesCfg => _lSqlHistoryFilesCfg;

        readonly List<HistoryFile> _lSqlHistoryFilesCfg = new List<HistoryFile>();

        // count of SqlFile in *.cfg files
        const int SQL_FILE_COUNT = 20;
        Configuration _config;

        /// <summary>
        /// Constructor which load all sql history file names
        /// </summary>
        /// <param name="currentConfig">current Configuration</param>
        public SqlHistoryFilesCfg(Configuration currentConfig)
        {
            _config = currentConfig;
            load();
        }

        /// <summary>
        ///  Loads sql history file names from configuration.
        ///  Ignore not exiting files
        /// </summary>
        public void load()
        {
            // make file list unique
            Dictionary<string, string> loadedFiles = new Dictionary<string, string>();
            // SqlFile<i> i=1..20
            foreach (KeyValueConfigurationElement entry in _config.AppSettings.Settings)
            {
                // find key appropriate for file
                string key = entry.Key;
                if (key.Length <= SQL_HISTORY_FILE_CFG_STRING.Length) continue;
                if (key.Substring(0, SQL_HISTORY_FILE_CFG_STRING.Length).Equals(SQL_HISTORY_FILE_CFG_STRING))
                {
                    // filename found
                    string fileName = entry.Value.Trim();
                    // skip empty entries
                    if (fileName == "") continue;

                    // file isn't available, delete it from list 
                    if (!File.Exists(fileName))
                    {
                        continue;
                    }
                    // ignore duplicated files
                    if (!(loadedFiles.ContainsKey(entry.Value)))
                    {
                        _lSqlHistoryFilesCfg.Add(new HistoryFile(entry.Value));
                        loadedFiles.Add(entry.Value,"");
                    }


                }
            }
            loadedFiles = null;
        }
        /// <summary>
        /// Save history / recent sql file names to configuration. It stores all entries in the history list. The remaining files are reset to "":
        /// </summary>
        public void save()
        {
            int maxFileCount = MAX_FILE_COUNT_TO_REMEMBER;
            if (_lSqlHistoryFilesCfg.Count > maxFileCount) maxFileCount = _lSqlHistoryFilesCfg.Count;
            for (int i = 0; i < maxFileCount; i++)
            {
                string key = $"{SQL_HISTORY_FILE_CFG_STRING}{i + 1}";
                string value = "";
                // store the existing history files
                if (i < _lSqlHistoryFilesCfg.Count)
                {
                    HistoryFile f = _lSqlHistoryFilesCfg[i];
                    value = f.FullName;

                }
                // make sure the key exists
                var parCfg = _config.AppSettings.Settings[key];
                if (parCfg != null) parCfg.Value = value;

            }
        }
        /// <summary>
        /// Insert a SQL file to the beginning of SqlFileHistory 
        /// </summary>
        /// <param name="s"></param>
        public void insert(string s)
        {
            // delete an existing entry
            // add to first one
            var index = _lSqlHistoryFilesCfg.FindIndex(x => x.FullName == s);  
            if (index > -1)
            {
                _lSqlHistoryFilesCfg.RemoveAt(index);
            }
            _lSqlHistoryFilesCfg.Insert(0, new HistoryFile(s));

        }
    }
}
