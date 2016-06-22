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
            chkLineStyleSupport.Checked = settings.IsLineStyleSupport;
            chkShortKeySupport.Checked = settings.IsShortKeySupport;
            chkShowServiceButtons.Checked = settings.IsShowServiceButton;
            chkShowQueryButtons.Checked = settings.IsShowQueryButton;
            chkFavoriteSupport.Checked = settings.IsFavoriteSupport;
            chkConveyedItemSupport.Checked = settings.IsConveyedItemsSupport;


            txtQuickSearch.Text = settings.QuickSearchName;
            txtFileManagerPath.Text = settings.FileManagerPath;
            chkAdvancedFeatures.Checked = settings.IsAdvancedFeatures;
            chkSvnSupport.Checked = settings.IsSvnSupport;
            chkVcSupport.Checked = settings.IsVcSupport;
            chkAdvancedPort.Checked = settings.IsAdvancedPort;
            chkAdvancedDiagramNote.Checked = settings.IsAdvancedDiagramNote;
            #endregion

            #region AutoLoadMdg
            // Initialize Auto Load MDG
            switch (_settings.AutoLoadMdgXml)
            {
                case AddinSettings.AutoLoadMdg.Basic:
                    rbAutoLoadMdgBasic.Checked = true;
                    break;
                case AddinSettings.AutoLoadMdg.Compilation:
                    rbAutoLoadMdgCompilation.Checked = true;
                    break;
                default:
                    rbAutoLoadMdgNo.Checked = true;
                    break;
            }
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
            _settings.QuickSearchName = txtQuickSearch.Text;
            _settings.FileManagerPath = txtFileManagerPath.Text;


            _settings.IsLineStyleSupport = chkLineStyleSupport.Checked;
            _settings.IsShortKeySupport = chkShortKeySupport.Checked;
            _settings.IsShowServiceButton = chkShowServiceButtons.Checked ;
            _settings.IsShowQueryButton = chkShowQueryButtons.Checked;
            _settings.IsFavoriteSupport = chkFavoriteSupport.Checked;
            _settings.IsConveyedItemsSupport = chkConveyedItemSupport.Checked;

            _settings.IsSvnSupport = chkSvnSupport.Checked;
            _settings.IsVcSupport = chkVcSupport.Checked;
            _settings.IsAdvancedFeatures = chkAdvancedFeatures.Checked;
            _settings.IsAdvancedPort = chkAdvancedPort.Checked;
            _settings.IsAdvancedDiagramNote = chkAdvancedDiagramNote.Checked;

            #region AutoLoadMdg
            _settings.AutoLoadMdgXml = AddinSettings.AutoLoadMdg.No;
            if (rbAutoLoadMdgBasic.Checked) _settings.AutoLoadMdgXml = AddinSettings.AutoLoadMdg.Basic;
            if (rbAutoLoadMdgCompilation.Checked) _settings.AutoLoadMdgXml = AddinSettings.AutoLoadMdg.Compilation;
            #endregion

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
            _settings.Save();
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
