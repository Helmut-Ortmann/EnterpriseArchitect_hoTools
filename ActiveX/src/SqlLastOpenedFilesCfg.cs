using System;
using System.Collections.Generic;
using System.Configuration;

namespace hoTools.Settings
{
    /// <summary>
    /// Administer 10 last opened SQL files
    /// <para/>- remember the last at max 10 opened SQL files
    /// </summary>
    public class SqlLastOpenedFilesCfg
    {
        const string SQL_LAST_OPENED_FILE_CFG_STRING = "SqlLastOpenedFile";
        /// <summary>
        /// List of files last opened before EA closed
        /// </summary>
        public List<HistoryFile> lSqlLastOpenedFilesCfg => _lSqlLastOpenedFilesCfg;

        readonly List<HistoryFile> _lSqlLastOpenedFilesCfg = new List<HistoryFile>();

        /// <summary>
        /// Count of open files to remember. This is the minimum to save.
        /// </summary>
        int _openFileCount = 0;


        Configuration _config;

        /// <summary>
        /// Constructor which load all last opened file names
        /// </summary>
        /// <param name="currentConfig">current Configuration</param>
        public SqlLastOpenedFilesCfg(Configuration currentConfig)
        {
            _config = currentConfig;
            load();
        }

        /// <summary>
        ///  Loads last opened sql file names from configuration.
        /// </summary>
        public void load()
        {
            // make file list unique
            Dictionary<string, string> loadedFiles = new Dictionary<string, string>();
            // SqlFileOpendFile<i> i=1..10
            foreach (KeyValueConfigurationElement entry in _config.AppSettings.Settings)
            {
                // find key appropriate for file
                string key = entry.Key;
                if (key.Length <= SQL_LAST_OPENED_FILE_CFG_STRING.Length) continue;
                if (key.Substring(0, SQL_LAST_OPENED_FILE_CFG_STRING.Length).Equals(SQL_LAST_OPENED_FILE_CFG_STRING))
                {
                    // key found
                    _openFileCount += 1;
                    // skip empty entries
                    if (entry.Value.Trim() == "") continue;
                    // ignore duplicated files
                    if (!(loadedFiles.ContainsKey(entry.Value)))
                    {
                        _lSqlLastOpenedFilesCfg.Add(new HistoryFile(entry.Value));
                        loadedFiles.Add(entry.Value,"");
                    }


                }
            }
            // only needed to ensure unique file names
            loadedFiles = null;
        }
        /// <summary>
        /// Save sql file names to configuration
        /// <para/>Make sure the loaded amount is written back
        /// </summary>
        public void save()
        {
            int maxOpenFileCount = _openFileCount;
            if (_lSqlLastOpenedFilesCfg.Count > maxOpenFileCount) maxOpenFileCount = _lSqlLastOpenedFilesCfg.Count;
            for (int i = 0; i < maxOpenFileCount; i++)
            {
                string key = $"{SQL_LAST_OPENED_FILE_CFG_STRING}{i + 1}";
                string value = "";
                // store the opened files
                if (i < _lSqlLastOpenedFilesCfg.Count)
                {
                    HistoryFile f = _lSqlLastOpenedFilesCfg[i];
                    value = f.FullName;

                }
                _config.AppSettings.Settings[key].Value = value;

            }
        }
        /// <summary>
        /// Insert a SQL file to the beginning of last opened SQL files 
        /// </summary>
        /// <param name="fileName"></param>
        public void insert(string fileName)
        {
            // delete an existing entry
            // add to first one
            var index = _lSqlLastOpenedFilesCfg.FindIndex(x => x.FullName == fileName);  
            if (index > -1)
            {
                _lSqlLastOpenedFilesCfg.RemoveAt(index);
            }
            _lSqlLastOpenedFilesCfg.Insert(0, new HistoryFile(fileName));


        }
        /// <summary>
        /// Remove fileName from last opened files
        /// </summary>
        public void remove(string fileName)
        {
            // delete an existing entry
            var index = _lSqlLastOpenedFilesCfg.FindIndex(x => x.FullName == fileName);
            if (index > -1)
            {
                _lSqlLastOpenedFilesCfg.RemoveAt(index);
            }
        }
        /// <summary>
        /// Remove all file items from last opened files
        /// </summary>
        public void removeAll()
        {
            _lSqlLastOpenedFilesCfg.Clear();
        }
    }
}
