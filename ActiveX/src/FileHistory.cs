using System.IO;

namespace hoTools.Settings
{
    /// <summary>
    /// Stores a file properties like FullName, DisplayName
    /// </summary>
    public class FileHistory
    {
        public string FullName => _fullName;
        public string DisplayName => Path.GetFileName(_fullName);

        readonly string _fullName;
        public FileHistory(string fullname)
        {
            _fullName = fullname;
        }
    }
}
