using System;
using System.Windows.Forms;

namespace hoTools.Settings
{
    public partial class FrmQueryAndScript : Form
    {
        /// <summary>
        /// Global Settings of hoTools
        /// </summary>
        AddinSettings _settings;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmQueryAndScript(AddinSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            // Query window without script
            switch (_settings.OnlyQueryWindow) {
                case AddinSettings.ShowInWindow.AddinWindow:
                    rbOnlyQueryAddinWindow.Checked = true;
                    break;
                case AddinSettings.ShowInWindow.TabWindow:
                    rbOnlyQueryTabWindow.Checked = true;
                    break;
                default:
                    rbOnlyQueryDisableWindow.Checked = true;
                    break;
            }
            // Query window wit script
            switch (_settings.ScriptAndQueryWindow)
            {
                case AddinSettings.ShowInWindow.AddinWindow:
                    rbScriptAndQueryAddinWindow.Checked = true;
                    break;
                case AddinSettings.ShowInWindow.TabWindow:
                    rbScriptAndQueryTabWindow.Checked = true;
                    break;
                default:
                    rbScriptAndQueryDisableWindow.Checked = true;
                    break;
            }

            // Ask for update if sql file has changed
            chkIsAskForUpdate.Checked = _settings.isAskForQueryUpdateOutside;
            // SQL editor
            txtSqlEditor.Text = _settings.SqlEditor;


        }
        #endregion

        #region Button Close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Button ok
        private void btnOk_Click(object sender, EventArgs e)
        {
            // only SQL query window
            _settings.OnlyQueryWindow = AddinSettings.ShowInWindow.Disabled;
            if (rbOnlyQueryAddinWindow.Checked) _settings.OnlyQueryWindow = AddinSettings.ShowInWindow.AddinWindow;
            if (rbOnlyQueryTabWindow.Checked) _settings.OnlyQueryWindow = AddinSettings.ShowInWindow.TabWindow;

            // SQL query + script window
            _settings.ScriptAndQueryWindow = AddinSettings.ShowInWindow.Disabled;
            if (rbScriptAndQueryAddinWindow.Checked) _settings.ScriptAndQueryWindow = AddinSettings.ShowInWindow.AddinWindow;
            if (rbScriptAndQueryTabWindow.Checked) _settings.ScriptAndQueryWindow = AddinSettings.ShowInWindow.TabWindow;

            _settings.isAskForQueryUpdateOutside = chkIsAskForUpdate.Checked;
            _settings.SqlEditor = txtSqlEditor.Text;


            // save setting
            this._settings.save();
            Close();
        }
        #endregion

    }
}
