
using System.Collections.Generic;
using System.Configuration;

namespace hoTools.Settings
{
    /// <summary>
    /// Administer the recent 20 used SQL files
    /// <para/>- Store and read them in config-file
    /// </summary>
    public class SqlHistoryFilesCfg
    {
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
                if (key.Length <= SQL_HISTORY_FILE_CFG_STRING.Length) continue;
                if (key.Substring(0, SQL_HISTORY_FILE_CFG_STRING.Length).Equals(SQL_HISTORY_FILE_CFG_STRING))
                {
                    // skip empty entries
                    if (entry.Value.Trim() == "") continue;
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
        /// Save history / recent sql file names to configuration
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
                if (i > SQL_FILE_COUNT) break;
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
