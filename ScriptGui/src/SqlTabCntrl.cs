using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hoTools.Scripts
{
    public class SqlTabCntrl
    {
        TabPage _tabPage;
        string FullName { get; set; }
        string DisplayName
        { get
            {
                string fileExtension = "";
                if (IsPersistant) fileExtension = " *";
                return Path.GetFileName(FullName) + fileExtension;
            }
        }
        Boolean IsPersistant { get; set; }


        #region Constructor TabPage
        public SqlTabCntrl(TabPage tabPage, string fullName)
        {
            init(tabPage, fullName, false);
        }
        public SqlTabCntrl(TabPage tabPage, string fullName, bool isPersistant)
        {
            init(tabPage, fullName, isPersistant);            
        }
        #endregion
        private void init(TabPage tabPage, string fullName, bool isPersistant)
        {
            _tabPage = tabPage;
            FullName = fullName;
            IsPersistant = isPersistant;
            tabPage.Text = FullName;  // set Tab name

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
