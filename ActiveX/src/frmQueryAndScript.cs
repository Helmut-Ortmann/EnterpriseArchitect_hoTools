using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hoTools.Settings
{
    public partial class FrmQueryAndScript : Form
    {
        private AddinSettings _settings;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmQueryAndScript(AddinSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            chkScriptAndQuery.Checked = _settings.isScriptAndQuery;
            chkOnlyQuery.Checked = _settings.isOnlyQuery;
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _settings.isOnlyQuery = chkOnlyQuery.Checked;
            _settings.isScriptAndQuery = chkScriptAndQuery.Checked;

            // save setting
            this._settings.save();
            Close();
        }
    }
}
