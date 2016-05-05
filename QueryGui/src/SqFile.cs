using System.IO;


namespace hoTools.Query
{
    /// <summary>
    /// Information of a *.sql file:
    /// <para/>-FullName
    /// <para/>-DisplayName
    /// <para/>-isChanged
    /// </summary>
    public class SqlFile
    {
        /// <summary>
        /// Extra space for display name to draw 'x' to provide a simulated Close Button
        /// Use a non proportional font like courier new
        /// </summary>
        const string DISPLAY_NAME_EXTRA_SPACE = "   ";
        FileSystemWatcher watcher { get; }

        #region Constructors SqlFile
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged">Default=true</param>
        public SqlFile(string fullName, bool isChanged = true)
        {
            watcher = new FileSystemWatcher();
            init(fullName, isChanged);
        }
        #endregion


        public string FullName
        {
            get { return FullName; } 
            set
            {
                FullName = value;
                // set FileSystemWatcher
                if (IsPersistant)
                {
                    watcher.Path = FullName;
                    watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                    watcher.Changed += new FileSystemEventHandler(OnChanged);
                    watcher.EnableRaisingEvents = true;
                }
            }
        }
        public string DirectoryName => Path.GetDirectoryName(FullName);
        public string DisplayName
        {
            get
            {
                string fileExtension = "";
                if (IsChanged) fileExtension = " *";
                return Path.GetFileName(FullName) + fileExtension + DISPLAY_NAME_EXTRA_SPACE;
            }
        }
        public bool IsChanged { get; set; }

        /// <summary>
        /// True if a complete path exists (once stored to file system)
        /// </summary>
        public bool IsPersistant => File.Exists(FullName);




        /// <summary>
        /// Initialize the TabPage information.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged"></param>
        private void init(string fullName, bool isChanged)
        {
            FullName = fullName.Trim();
            IsChanged = isChanged;

        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {

        }
    }
}
