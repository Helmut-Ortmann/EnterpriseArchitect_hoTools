using System.Windows.Forms;
using hoTools.ActiveX;


namespace hoTools.Settings
{
    public partial class FrmSettingsGeneral : Form
    {
        AddinSettings _settings;
        AddinControlGUI _addinControl;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        public FrmSettingsGeneral(AddinSettings settings, AddinControlGUI addinControl)
        {
            InitializeComponent();
            _settings = settings;
            _addinControl = addinControl;


            #region miscellaneous
            chkLineStyleSupport.Checked = settings.isLineStyleSupport;
            chkShortKeySupport.Checked = settings.isShortKeySupport;
            chkShowServiceButtons.Checked = settings.isShowServiceButton;
            chkShowQueryButtons.Checked = settings.isShowQueryButton;
            chkFavoriteSupport.Checked = settings.isFavoriteSupport;


            txtQuickSearch.Text = settings.quickSearchName;
            txtFileManagerPath.Text = settings.FileManagerPath;
            chkAdvancedFeatures.Checked = settings.isAdvancedFeatures;
            chkSvnSupport.Checked = settings.isSvnSupport;
            chkVcSupport.Checked = settings.isVcSupport;
            chkAdvancedPort.Checked = settings.isAdvancedPort;
            chkAdvancedDiagramNote.Checked = settings.isAdvancedDiagramNote;
            #endregion


            #region LineStyleAndMoreWindow
            // Initialize LineStyle Window
            switch (_settings.LineStyleAndMoreWindow)
            {
                case AddinSettings.ShowInWindow.AddinWindow:
                    rbLineStyleAndMoreAddinWindow.Checked = true;
                    break;
                case AddinSettings.ShowInWindow.TabWindow:
                    rbLineStyleAndMoreTabWindow.Checked = true;
                    break;
                default:
                    rbLineStyleAndMoreDisableWindow.Checked = true;
                    break;
            }
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


            _settings.isLineStyleSupport = chkLineStyleSupport.Checked;
            _settings.isShortKeySupport = chkShortKeySupport.Checked;
            _settings.isShowServiceButton = chkShowServiceButtons.Checked ;
            _settings.isShowQueryButton = chkShowQueryButtons.Checked;
            _settings.isFavoriteSupport = chkFavoriteSupport.Checked;

            _settings.isSvnSupport = chkSvnSupport.Checked;
            _settings.isVcSupport = chkVcSupport.Checked;
            _settings.isAdvancedFeatures = chkAdvancedFeatures.Checked;
            _settings.isAdvancedPort = chkAdvancedPort.Checked;
            _settings.isAdvancedDiagramNote = chkAdvancedDiagramNote.Checked;

            #region LineStyleAndMoreWindow
            _settings.LineStyleAndMoreWindow = AddinSettings.ShowInWindow.Disabled;
            if (rbLineStyleAndMoreAddinWindow.Checked) _settings.LineStyleAndMoreWindow = AddinSettings.ShowInWindow.AddinWindow;
            if (rbLineStyleAndMoreTabWindow.Checked) _settings.LineStyleAndMoreWindow = AddinSettings.ShowInWindow.TabWindow;
            #endregion

            #region SearchAndReplaceWindow
            _settings.SearchAndReplaceWindow = AddinSettings.ShowInWindow.Disabled;
            if (rbSearchAndReplaceAddinWindow.Checked) _settings.SearchAndReplaceWindow = AddinSettings.ShowInWindow.AddinWindow;
            if (rbSearchAndReplaceTabWindow.Checked) _settings.SearchAndReplaceWindow = AddinSettings.ShowInWindow.TabWindow;
            #endregion

            // save setting
            _settings.save();
            _addinControl.initializeSettings(); // update settings
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void label7_Click(object sender, System.EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {

        }
    }
}
