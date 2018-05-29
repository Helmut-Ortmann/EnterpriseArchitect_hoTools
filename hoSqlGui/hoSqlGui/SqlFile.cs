using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using FileSystem;
using hoTools.Utils;


namespace hoTools.hoSqlGui
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
                _saveTime = IsPersistant ? File.GetLastWriteTime(_fullName) : DateTime.MinValue;
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
        readonly string _displayNameExtraSpace = "   ";
        #endregion

        #region Constructors SqlFile

        /// <summary>
        /// Constructor. If the file is persistent then register a Watcher
        /// <para/>- Initialize file system watcher
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged">Default=true</param>
        /// <param name="sqlTabPagesCntrl"></param>
        public SqlFile(SqlTabPagesCntrl sqlTabPagesCntrl,  string fullName, bool isChanged = true)
        {
            _sqlTabPagesCntrl = sqlTabPagesCntrl;
            InitTabPageCaption(fullName, isChanged);
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
                return Path.GetFileName(_fullName) + fileExtension + _displayNameExtraSpace;
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
        void InitTabPageCaption(string fullName, bool isChanged)
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
            if ( (!_sqlTabPagesCntrl.Settings.IsAskForQueryUpdateOutside) && (_fullName != path) ) return;

            try
            {
                // run update TextBox in syncContext to make sure it works
                _syncContext.Post(o => _sqlTabPagesCntrl.ReloadTabPageWithAsk(), null);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}", @"Error invoke foreign thread");
            }

        }
        #endregion

        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMethodReturnValue.Global
        public bool Save(string text)
        {
            try
            {
                File.WriteAllText(_fullName, text.Trim());
                _saveTime = DateTime.Now; // avoid 
                IsChanged = false;
                return true;
            } catch (Exception e)
            {
                MessageBox.Show($"File '{_fullName}'\r\n{e}", @"Error writing file!");
                return false;
            }
        }
        /// <summary>
        /// Load from file. Note currently the change event will make the text box change. Therefore you have later to set IsChanged to false; 
        /// </summary>
        /// <returns>File content</returns>
        public string Load()
        {
            try
            {
                string s = Util.ReadAllText(_fullName).Trim();
                _readTime = DateTime.Now; // avoid 
                IsChanged = false;
                return s;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"File '{_fullName}'{Environment.NewLine}{e}", @"Error reading file!");
                return "";
            }
        }
    }
}