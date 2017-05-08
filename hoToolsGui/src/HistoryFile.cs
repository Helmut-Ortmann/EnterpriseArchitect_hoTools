using System.IO;

namespace hoTools.Settings
{
    /// <summary>
    /// Stores file properties like FullName, DisplayName for recent files.
    /// </summary>
    public class HistoryFile
    {
        public string FullName => _fullName;
        public string DisplayName => Path.GetFileName(_fullName);

        readonly string _fullName;
        public HistoryFile(string fullname)
        {
            _fullName = fullname;
        }
    }
}
