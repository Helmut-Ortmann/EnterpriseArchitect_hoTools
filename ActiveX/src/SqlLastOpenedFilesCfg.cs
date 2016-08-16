using System.IO;
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
        const int MaxOpenFileCountToRemember = 10;
        const string SqlLastOpenedFileCfgString = "SqlLastOpenedFile";
        /// <summary>
        /// List of files last opened before EA closed
        /// </summary>
        public List<HistoryFile> lSqlLastOpenedFilesCfg => _lSqlLastOpenedFilesCfg;

        readonly List<HistoryFile> _lSqlLastOpenedFilesCfg = new List<HistoryFile>();


        readonly Configuration _config;

        /// <summary>
        /// Constructor which load all last opened file names
        /// </summary>
        /// <param name="currentConfig">current Configuration</param>
        public SqlLastOpenedFilesCfg(Configuration currentConfig)
        {
            _config = currentConfig;
            Load();
        }

        /// <summary>
        ///  Loads last opened sql file names from configuration.
        ///  File that don't exists are removed from the list.
        /// </summary>
        public void Load()
        {
            // make file list unique
            Dictionary<string, string> loadedFiles = new Dictionary<string, string>();
            // SqlFileOpendFile<i> i=1..10
            foreach (KeyValueConfigurationElement entry in _config.AppSettings.Settings)
            {
                // find key appropriate for file
                string key = entry.Key;
                if (key.Length <= SqlLastOpenedFileCfgString.Length) continue;
                if (key.Substring(0, SqlLastOpenedFileCfgString.Length).Equals(SqlLastOpenedFileCfgString))
                {
                    // key with fileName found
                    string fileName = entry.Value.Trim();
 

                    // skip empty entries
                    if (fileName == "") continue;

                    // file isn't available, delete it from list of last opened filed
                    if (!File.Exists(fileName))
                    {
                        continue;
                    }
                    // ignore duplicated files
                    if (!(loadedFiles.ContainsKey(entry.Value)))
                    {
                        _lSqlLastOpenedFilesCfg.Add(new HistoryFile(entry.Value));
                        loadedFiles.Add(entry.Value,"");
                    }


                }
            }
            // only needed to ensure unique file names
        }
        /// <summary>
        /// Save sql file names to configuration. Don't store duplicated file names
        /// <para/>Make sure the loaded amount is written back
        /// </summary>
        public void Save()
        {
            int maxOpenFileCount = MaxOpenFileCountToRemember;
            if (_lSqlLastOpenedFilesCfg.Count > maxOpenFileCount) maxOpenFileCount = _lSqlLastOpenedFilesCfg.Count;
            for (int i = 0; i < maxOpenFileCount; i++)
            {
                string key = $"{SqlLastOpenedFileCfgString}{i + 1}";
                string value = "";
                // store the opened files
                if (i < _lSqlLastOpenedFilesCfg.Count)
                {
                    HistoryFile f = _lSqlLastOpenedFilesCfg[i];
                    value = f.FullName;

                }
                // make sure the key exists
                var parCfg = _config.AppSettings.Settings[key];
                if (parCfg != null) parCfg.Value = value;

            }
        }
        /// <summary>
        /// Insert a SQL file to the beginning of last opened SQL files 
        /// </summary>
        /// <param name="fileName"></param>
        public void Insert(string fileName)
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
        public void Remove(string fileName)
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
        public void RemoveAll()
        {
            _lSqlLastOpenedFilesCfg.Clear();
        }
    }
}
