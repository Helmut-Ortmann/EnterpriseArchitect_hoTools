using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using FileSystem;


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
        readonly SqlTabPagesCntrl _sqlTabPagesCntrl;
        readonly TabPage _tabPage;
        readonly FileMonitor _fileMonitor;
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
                if (IsPersistant) _fileMonitor.Update(_fullName);
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

            // create FileMonitor
            _fileMonitor = new FileMonitor(_fullName);
            _fileMonitor.Change += OnChanged;
            if (IsPersistant)
            {

                _fileMonitor.Start();
            }

        }
        #endregion

        public void Dispose()
        {
           
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
        #region OnChanged 
        /// <summary>
        /// Wait for event from FileSystemMonitor. 
        /// </summary>
        /// <param name="path"></param>
        void OnChanged(string path)
        {
            if ( (!_sqlTabPagesCntrl.Settings.isAskForQueryUpdateOutside) && (_fullName != path) ) return;

            try
            {
                TextBox t = (TextBox)_tabPage.Controls[0];
                // run update TextBox in syncContext to make sure it works
                _syncContext.Post(o => _sqlTabPagesCntrl.ReloadTabPageWithAsk(), null);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.ToString()}", "Error invoke foreign thread");
                return;
            }

        }
        #endregion

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
    }
}