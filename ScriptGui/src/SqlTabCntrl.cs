using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hoTools.Scripts
{
    /// <summary>
    /// Information to store and handle for a TabPage:
    /// - FullName
    /// - DisplayName
    /// - isChanged
    /// </summary>
    public class SqlTabCntrl
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
        public SqlTabCntrl(string fullName)
        {
            init(fullName, false);
        }
        public SqlTabCntrl(string fullName, bool isChanged)
        {
            init(fullName, isChanged);            
        }
        #endregion
        private void init(string fullName, bool isChanged)
        {
            FullName = fullName.Trim();
            IsChanged = isChanged;

        }
        /*
        public static string getFileNameFromCaptionUnchanged(string caption)
          => caption.Replace("*", "").Trim();
        public static string getFileNameCaptionChanged(string caption)
            => getFileNameFromCaptionUnchanged(caption) + " *";
        public static bool isFileNameCaptionChanged(string caption)
            => caption == getFileNameFromCaptionUnchanged(caption);

        public static bool getFileNameCaptionInitial(string caption)
            => caption == "noName1.sql";
        */

    }
}
