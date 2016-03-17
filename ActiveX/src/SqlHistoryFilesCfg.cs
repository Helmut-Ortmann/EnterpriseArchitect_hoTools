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
        public List<FileHistory> lSqlHistoryFilesCfg => _lSqlHistoryFilesCfg;

        readonly List<FileHistory> _lSqlHistoryFilesCfg = new List<FileHistory>();


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
        ///  Loads sql history file names from configuration
        /// </summary>
        public void load()
        {
            // SqlFile<i> i=1..20
            foreach (KeyValueConfigurationElement entry in _config.AppSettings.Settings)
            {
                // find key
                string key = entry.Key;
                if (key.Substring(0,7).Equals(SQL_HISTORY_FILE_CFG_STRING))
                {
                    _lSqlHistoryFilesCfg.Add(new FileHistory(entry.Value));


                }
            }
            _lSqlFilesCfgLength = _lSqlHistoryFilesCfg.Count;
        }
        /// <summary>
        /// Save sql file names to configuration
        /// </summary>
        public void save()
        {
            int i = 1;
            foreach (FileHistory f in _lSqlHistoryFilesCfg)
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
            _lSqlHistoryFilesCfg.Insert(0, new FileHistory(s));

        }
    }
}
