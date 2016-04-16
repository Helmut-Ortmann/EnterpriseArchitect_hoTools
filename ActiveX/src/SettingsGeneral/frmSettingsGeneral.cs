using System.Windows.Forms;
using hoTools.ActiveX;


namespace hoTools.Settings
{
    public partial class FrmSettingsGeneral : Form
    {
        AddinSettings _settings;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        public FrmSettingsGeneral(AddinSettings settings)
        {
            InitializeComponent();
            _settings = settings;


            #region miscellaneous
            txtQuickSearch.Text = settings.quickSearchName;
            txtFileManagerPath.Text = settings.FileManagerPath;
            chkAdvancedFeatures.Checked = settings.isAdvancedFeatures;
            chkSvnSupport.Checked = settings.isSvnSupport;
            chkVcSupport.Checked = settings.isVcSupport;
            chkAdvancedPort.Checked = settings.isAdvancedPort;
            chkAdvancedDiagramNote.Checked = settings.isAdvancedDiagramNote;
            #endregion

            #region SearchAndReplaceWindow
            // Initialize Search and Replace Window
            switch (_settings.SearchAndReplaceWindow)
            {
                case AddinSettings.ShowInWindow.AddinWindow:
                    rbSearchAndReplaceAddinWindow.Checked = true;
                    break;
                case AddinSettings.ShowInWindow.TabWindow:
                    rbSearchAndReplaceTabWindow.Checked = true;
                    break;
                default:
                    rbSearchAndReplaceDisableWindow.Checked = true;
                    break;
            }
            #endregion
        }
        #endregion

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            _settings.quickSearchName = txtQuickSearch.Text;
            _settings.FileManagerPath = txtFileManagerPath.Text;
            _settings.isSvnSupport = chkSvnSupport.Checked;
            _settings.isVcSupport = chkVcSupport.Checked;
            _settings.isAdvancedFeatures = chkAdvancedFeatures.Checked;
            _settings.isAdvancedPort = chkAdvancedPort.Checked;
            _settings.isAdvancedDiagramNote = chkAdvancedDiagramNote.Checked;

            #region SearchAndReplaceWindow
            _settings.SearchAndReplaceWindow = AddinSettings.ShowInWindow.Disabled;
            if (rbSearchAndReplaceAddinWindow.Checked) _settings.OnlyQueryWindow = AddinSettings.ShowInWindow.AddinWindow;
            if (rbSearchAndReplaceTabWindow.Checked) _settings.OnlyQueryWindow = AddinSettings.ShowInWindow.TabWindow;
            #endregion

            // save setting
            _settings.save();
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
