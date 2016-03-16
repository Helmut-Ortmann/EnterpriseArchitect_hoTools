using System.IO;

namespace hoTools.Scripts
{
    /// <summary>
    /// Stores a file properties like FullName, DisplayName
    /// </summary>
    class File
    {
        public string FullName => _fullName;
        public string DisplayName => Path.GetFileName(_fullName);
        string _displayName;
        string _fullName;
        public File(string fullname)
        {
            _fullName = fullname;
        }
    }
}
