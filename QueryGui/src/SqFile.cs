using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Constructors SqlFile
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged">Default=true</param>
        public SqlFile(string fullName, bool isChanged = true)
        {
            init(fullName, isChanged);
        }
        #endregion


        public string FullName { get; set; }
        public string DirectoryName => Path.GetDirectoryName(FullName);
        public string DisplayName
        { get
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
       
    }
}
