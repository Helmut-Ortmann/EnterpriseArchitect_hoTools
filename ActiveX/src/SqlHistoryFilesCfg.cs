using System;
using System.Collections.Generic;
using System.Configuration;

namespace hoTools.Settings
{
    /// <summary>
    /// Administer sql history file names of config file
    /// </summary>
    public class SqlHistoryFilesCfg
    {
        const string SQL_HISTORY_FILE_CFG_STRING = "SqlFile";
        /// <summary>
        /// List of files loaded in history
        /// </summary>
        public List<HistoryFile> lSqlHistoryFilesCfg => _lSqlHistoryFilesCfg;

        readonly List<HistoryFile> _lSqlHistoryFilesCfg = new List<HistoryFile>();


        int _lSqlFilesCfgLength;
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
        ///  Ignore exiting files
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
                if (key.Substring(0,7).Equals(SQL_HISTORY_FILE_CFG_STRING))
                {
                    // ignore duplicated files
                    if (!(loadedFiles.ContainsKey(entry.Value)))
                    {
                        _lSqlHistoryFilesCfg.Add(new HistoryFile(entry.Value));
                        loadedFiles.Add(entry.Value,"");
                    }


                }
            }
            loadedFiles = null;
            _lSqlFilesCfgLength = _lSqlHistoryFilesCfg.Count;
        }
        /// <summary>
        /// Save sql file names to configuration
        /// </summary>
        public void save()
        {
            int i = 1;
            foreach (HistoryFile f in _lSqlHistoryFilesCfg)
            {
                // SqlFile<i>
                string key = $"{SQL_HISTORY_FILE_CFG_STRING}{i}";
                _config.AppSettings.Settings[key].Value = f.FullName;
                i = i + 1;
                // stop if element length reached
                if (i > _lSqlFilesCfgLength) break;
            }
        }
        /// <summary>
        /// Insert an SQL FileHistory to the beginning, an 
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
