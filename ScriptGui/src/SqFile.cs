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
    /// Information of a *.sql file: FullName, DisplayName, isChanged
    /// </summary>
    public class SqlFile
    {
        /// <summary>
        /// Extra space for display name to draw 'x' to provide a simulated Close Button
        /// Use a non proportional font like courier new
        /// </summary>
        const string DISPLAY_NAME_EXTRA_SPACE = "   ";
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


        #region Constructor TabPage
        /// <summary>
        /// Information of a TabPage: FullName, DisplayName, IsChanged
        /// </summary>
        /// <param name="fullName"></param>
        public SqlFile(string fullName)
        {
            init(fullName, false);
        }
        /// <summary>
        /// Information of a TabPage: FullName, DisplayName, IsChanged
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="isChanged"></param>
        public SqlFile(string fullName, bool isChanged)
        {
            init(fullName, isChanged);            
        }
        #endregion
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
