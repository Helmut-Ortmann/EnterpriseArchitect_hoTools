using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using hoTools.Settings;
using EAAddinFramework.Utils;

namespace hoTools.Scripts
{
    public class SqlTabCntrls
    {
        AddinSettings _settings;
        Model _model;
        TabControl _tabControl;

        /// <summary>
        /// List of TabPages in TabControl
        /// </summary>
        List<SqlTabCntrl> _tabCntrls = new List<SqlTabCntrl>();

        const string DEFAULT_TAB_NAME = "noName";

        public SqlTabCntrls(Model model, AddinSettings settings, TabControl tabControl)
        {
            _settings = settings;
            _model = model;
            _tabControl = tabControl;

        }
        /// <summary>
        /// Add a tab to the tab control
        /// </summary>
        /// <returns></returns>
        public TabPage addTab()
        {
            // create a new TabPage in TabControl
            TabPage tabPage = new TabPage();
            _tabControl.Controls.Add(tabPage);

            _tabCntrls.Add(new SqlTabCntrl(tabPage, DEFAULT_TAB_NAME + _tabControl.Controls.Count.ToString() + ".sql" ));
            return tabPage;
        }
       
    }
}
