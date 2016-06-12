using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;


namespace hoTools.Query
{
    /// <summary>
    /// Information of a *.sql file:
    /// <para/>-FullName
    /// <para/>-DisplayName
    /// <para/>-isChanged
    /// <para/>-TabPage
    /// <para/>-SqlTabPages 
    /// </summary>
    public class SqlFile : IDisposable
    {
        #region local fields
        
        /// the name with complete file path is used as Mutex object 
        string _fullName; 
        DateTime _saveTime = DateTime.MinValue; // mostly diagnostic purpose
        DateTime _readTime = DateTime.MinValue; // mostly diagnostic purpose
        DateTime _onChangeTime;
        /// <summary>
        /// File System watcher to observe changes of the *.sql file.
        /// </summary>
        readonly FileSystemWatcher _watcher;
        readonly SqlTabPagesCntrl _sqlTabPagesCntrl;
        readonly TabPage _tabPage;
        #endregion

        #region properties



        /// <summary>
        /// Full Name of the file (it contains the full path)
        /// </summary>
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
        /// Time file last read into TextBox. Used to debounce FileSystemWatcher Events
        /// </summary>
        public DateTime ReadTime
        {
            get
            {
                return _readTime;
            }
            set
            {
                _readTime = value;
            }
        }
        /// <summary>
        /// get time last time the file was saved. 
        /// </summary>
        public DateTime SaveTime
        {
            get
            {
                if (IsPersistant)
                {
                    _saveTime = File.GetLastWriteTime(_fullName);
                }
                else
                {
                    _saveTime = DateTime.MinValue;
                }
                return _saveTime;
            }
        }
        #endregion

        #region Configuration
        readonly SynchronizationContext _syncContext = SynchronizationContext.Current;
        readonly int TIME_SPAN_DIFFERENT_SAVE_EVENTS_MS = 1000;
        /// <summary>
        /// Extra space for display name to draw 'x' to provide a simulated Close Button
        /// Use a non proportional font like courier new
        /// </summary>
        readonly string DISPLAY_NAME_EXTRA_SPACE = "   ";
        #endregion

        #region Constructors SqlFile
        /// <summary>
        /// Constructor. If the file is persistent then register a Watcher
        /// <para/>- Initialize file system watcher
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged">Default=true</param>
        public SqlFile(SqlTabPagesCntrl sqlTabPagesCntrl, TabPage tabPage, string fullName, bool isChanged = true)
        {
            _tabPage = tabPage;
            _sqlTabPagesCntrl = sqlTabPagesCntrl;
            initTabPageCaption(fullName, isChanged);
            ReadTime = DateTime.Now;

            // create Watcher
            _watcher = new FileSystemWatcher();
            _watcher.Changed += OnChanged;
            watcherUpdate();


        }
        #endregion

        public void Dispose()
        {
            _watcher.Dispose();
        }
        

        /// <summary>
        /// If current file is persistent update File Watcher with current file.
        /// </summary>
        void watcherUpdate()
        {
            // set FileSystemWatcher
            if (IsPersistant)
            {
                _watcher.Path = Path.GetDirectoryName(_fullName);
                _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                _watcher.Filter = Path.GetFileName(_fullName);
                _watcher.EnableRaisingEvents = true;
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
        void initTabPageCaption(string fullName, bool isChanged)
        {
            IsChanged = isChanged;
            _fullName = fullName;


        }
        #region OnChanged FileSystemEventArgs
        /// <summary>
        /// Handle Change Event of the *.sql file. Events may fire multiple times on different threads. Therefore a Mutex with the instance field _fullname is used to synchronize access.
        /// <para/>- If configured ask for update when file has changed
        /// <para/>- Debounce changes of the file if multiple events occur
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void OnChanged(object source, FileSystemEventArgs e)
        {
            if ( (!_sqlTabPagesCntrl.Settings.isAskForQueryUpdateOutside) && (_fullName != e.FullPath) ) return;
            var lockObject = _fullName; // Mutex
            try
            {
                if (Monitor.TryEnter(lockObject))
                {
                    try
                    {
                        TimeSpan d2 = DateTime.Now.Subtract(_onChangeTime);
                        if (Math.Abs(d2.TotalMilliseconds) > TIME_SPAN_DIFFERENT_SAVE_EVENTS_MS)
                        {
                           onChangeFileSingleThread();
                        } else
                        {

                        }
                    }
                    finally
                    {
                        _onChangeTime = DateTime.Now;
                        Monitor.Exit(lockObject);
                    }
                }
                else
                // Was locked
                {
                    // multiple events in the same file
                    return;
                }
            }
            catch (SynchronizationLockException syncEx)
            {
                MessageBox.Show($"File:'{_fullName}'\r\n{syncEx}", "Error Watcher File changes");
            }
        }
        /// <summary>
        /// Execute the onChange event as single thread for the Resource '_fullName'.
        /// If the Resource is already locked do nothing because the user will decide about file.
        /// </summary>
        void onChangeFileSingleThread()
        {
            // was own write what was notified
            TimeSpan diff = DateTime.Now.Subtract(_onChangeTime);
            
            if (Math.Abs(diff.TotalMilliseconds) < TIME_SPAN_DIFFERENT_SAVE_EVENTS_MS)
            {
                _onChangeTime = DateTime.Now; // to avoid multiple events for the same event
                return;
            }
                try
                {
                    TextBox t = (TextBox)_tabPage.Controls[0];
                    // run update TextBox in syncContext to make sure it works
                    _syncContext.Post(o => _sqlTabPagesCntrl.ReloadTabPageWithAsk(), null);
                    _onChangeTime = DateTime.Now; // to avoid multiple events
                } catch (Exception e)
                {
                    MessageBox.Show($"{e.ToString()}", "Error invoke foreign thread");
                    return;
                }
                return;
        }
        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool save(string text)
        {
            try
            {
                File.WriteAllText(_fullName, text.Trim());
                _saveTime = DateTime.Now; // avoid 
                IsChanged = false;
                return true;
            } catch (Exception e)
            {
                MessageBox.Show($"File '{_fullName}'\r\ne.toString()", "Error writing file!");
                return false;
            }
        }
        /// <summary>
        /// Load from file.
        /// </summary>
        /// <returns>File content</returns>
        public string load()
        {
            try
            {
                string s = File.ReadAllText(_fullName).Trim();
                _readTime = DateTime.Now; // avoid 
                IsChanged = false;
                return s;
            }
            catch (Exception e)
            {
                MessageBox.Show($"File '{_fullName}'\r\ne.toString()", "Error reading file!");
                return "";
            }
        }
        #endregion
    }
}
