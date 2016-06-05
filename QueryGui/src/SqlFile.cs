using System;
using System.IO;
using System.Windows.Forms;


namespace hoTools.Query
{
    /// <summary>
    /// Information of a *.sql file:
    /// <para/>-FullName
    /// <para/>-DisplayName
    /// <para/>-isChanged
    /// <para/>-TabPage
    /// <para/>-SqlTabPages (for calling the delegate to update the text box with sql)
    /// </summary>
    public class SqlFile : IDisposable
    {
        /// <summary>
        /// Extra space for display name to draw 'x' to provide a simulated Close Button
        /// Use a non proportional font like courier new
        /// </summary>
        const string DISPLAY_NAME_EXTRA_SPACE = "   ";
        SqlTabPagesCntrl _sqlTabPagesCntrl;
        TabPage _tabPage;
        public delegate void UpdatePage(TabPage tabPage, string fileNameChanged);

        /// <summary>
        /// Time file last read. Used to debounce FileSystemWatcher Events
        /// </summary>
        DateTime _lastRead = DateTime.MinValue; // Time

        /// <summary>
        /// File System watcher to observe changes of the *.sql file.
        /// </summary>
        FileSystemWatcher watcher { get; }

        #region Constructors SqlFile
        /// <summary>
        /// Constructor.
        /// <para/>- Initialize file system watcher
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged">Default=true</param>
        public SqlFile(SqlTabPagesCntrl sqlTabPagesCntrl, TabPage tabPage, string fullName, bool isChanged = true)
        {
            _tabPage = tabPage;
            _sqlTabPagesCntrl = sqlTabPagesCntrl;
            //
            watcher = new FileSystemWatcher();
            watcher.Changed += OnChanged;
            init(fullName, isChanged);
        }
        #endregion

        public void Dispose()
        {
            watcher.Dispose();
        }
        string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                watcherUpdate();
            }
        }

        /// <summary>
        /// File Watcher update with current file.
        /// </summary>
        void watcherUpdate()
        {
            // set FileSystemWatcher
            if (IsPersistant)
            {
                watcher.Path = Path.GetDirectoryName(_fullName);
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                watcher.Filter = Path.GetFileName(_fullName);
                watcher.EnableRaisingEvents = true;
            }
        }

        public string DirectoryName => Path.GetDirectoryName(_fullName);
        public string DisplayName
        {
            get
            {
                string fileExtension = "";
                if (IsChanged) fileExtension = " *";
                return Path.GetFileName(_fullName) + fileExtension + DISPLAY_NAME_EXTRA_SPACE;
            }
        }
        public bool IsChanged { get; set; }

        /// <summary>
        /// True if a complete path exists (isPathRouted) and the file exists
        /// </summary>
        public bool IsPersistant => (Path.IsPathRooted(_fullName) && File.Exists(_fullName));

        /// <summary>
        /// Initialize the TabPage information.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged"></param>
        void init(string fullName, bool isChanged)
        {
            IsChanged = isChanged;
            _fullName = fullName;


        }
        #region OnChanged FileSystemEventArgs
        /// <summary>
        /// Handle Change Event of the *.sql file.
        /// <para/>- If configured ask for update when file has changed
        /// <para/>- Debounce changes of the file if multiple events occur
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void OnChanged(object source, FileSystemEventArgs e)
        {
            string fileNameChanged = e.FullPath;
            if (_fullName == fileNameChanged)
            {
                // debounce multiple events of Watcher
                DateTime lastWriteTime = File.GetLastWriteTime(_fullName);
                TimeSpan diff = lastWriteTime.Subtract(_lastRead);
                if (diff.TotalMilliseconds < 100) return;
                _lastRead = lastWriteTime;

                // update with or without asking
                bool updateSqlPage = true;
                bool askForUpdate = _sqlTabPagesCntrl.Settings.isAskForQueryUpdateOutside;
                if (askForUpdate)
                {
                    DialogResult result = MessageBox.Show($"'{fileNameChanged}'\nYes: Reload\nNo: Do nothing", "File has changed outside EA", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No) updateSqlPage = false;
                }
                if (updateSqlPage)
                {
                    _tabPage.Invoke(_sqlTabPagesCntrl.UpdatePageDelegate,      // delegate
                                    new object[] { _tabPage,
                                        fileNameChanged,
                                        true }                                  // don't update last opened file
                                   );
                }
            }
        }
        #endregion
    }
}
