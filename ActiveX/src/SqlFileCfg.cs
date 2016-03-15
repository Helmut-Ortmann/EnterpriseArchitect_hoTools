using System;
using System.Collections.Generic;
using System.Configuration;

namespace hoTools.Settings
{
    /// <summary>
    /// Administer sql file names of config file
    /// </summary>
    public class SqlFilesCfg
    {
        const string SQL_FILE_CFG_STRING = "SqlFile";
        List<string> _lSqlFilesCfg = new List<string>();
        public List<string> lSqlFilesCfg => _lSqlFilesCfg;

        int _lSqlFilesCfgLength;
        Configuration _config;

        /// <summary>
        /// Constructor which load all sql file names
        /// </summary>
        /// <param name="currentConfig">current Configuration</param>
        public SqlFilesCfg(Configuration currentConfig)
        {
            _config = currentConfig;
            load();
        }

        /// <summary>
        ///  Loads sql file names from configuration
        /// </summary>
        public void load()
        {
            // SqlFile<i> i=1..20
            foreach (KeyValueConfigurationElement entry in _config.AppSettings.Settings)
            {
                // find key
                string key = entry.Key;
                if (key.Substring(0,7).Equals(SQL_FILE_CFG_STRING))
                {
                    _lSqlFilesCfg.Add(entry.Value);


                }
            }
            _lSqlFilesCfgLength = _lSqlFilesCfg.Count;
        }
        /// <summary>
        /// Save sql file names to configuration
        /// </summary>
        public void save()
        {
            int i = 1;
            foreach (string s in _lSqlFilesCfg)
            {
                // SqlFile<i>
                string key = $"{SQL_FILE_CFG_STRING}{i}";
                _config.AppSettings.Settings[key].Value = s;
                i = i + 1;
                // stop if element length reached
                if (i > _lSqlFilesCfgLength) break;
            }
        }
        /// <summary>
        /// Insert an SQL File to the beginning, an 
        /// </summary>
        /// <param name="s"></param>
        public void insert(string s)
        {
            // delete an existing entry
            // add to first one
            _lSqlFilesCfg.Remove(s);
            _lSqlFilesCfg.Insert(0, s);

        }
    }
}
