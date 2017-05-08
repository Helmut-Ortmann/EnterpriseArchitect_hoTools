using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Control.EaAddinShortcuts;
using EAAddinFramework.Utils;
using hoTools.hoToolsGui;
using hoTools.EaServices;
using hoTools.EaServices.WiKiRefs;
using hoTools.Utils.Forms;

// ReSharper disable once CheckNamespace
namespace hoTools.Settings.Toolbar

{
    /// <summary>
    /// Key Settings (Shortcuts)
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsToolbar : Form
    {
        //-------------------------------------------------------------------------------------------------
        // Search Texts
        private readonly string SearchButtonRowTooltip = @"Configure a Search to use on the Search Toolbar:
- One Click to run a EA Model Search or hoTools SQL Query from file
- Configure:
-- Button Text
-- EA Model Search name or hoTools SQL file name (see SQL path in 'Settings SQL and Script') 
-- <Search Term>
-- Tooltip for easy use";



        readonly string SearchButtonTextTooltip = @"The text visualized on the Search Toolbar Button
- Make the text short to have enough room on the toolbar
- You may insert a Tooltip for more information";

        readonly string SearchButtonSearchNameTooltip =
            @"The EA Model Search Name or the hoTools File Name of the hoTools SQL file (*.sql)
- hoTools SQL file name: You may only input the file name without path (see SQL path in 'Settings SQL and Script')
- hoTools SQL file name: Make sure the 'SQL path' in 'Settings SQL and Script' in Addin Tab SQL or Script includes your SQL file";

        readonly string SearchButtonSearchTermTooltip = @"The <Search Term> to enter for the current Toobar Button";
        readonly string SearchButtonTooltipTooltip = @"The text to visualize as Tooltip on the Search Toolbar Button
- Make clear what to expect as results 
- Make clear what to enter as <Search Term> ";
        //-------------------------------------------------------------------------------------------------
        // Service Texts
        private readonly string ServiceButtonRowTooltip = @"Configure a Service to use on the Service Toolbar:
- One Click to run a Service
- Configure:
-- Button Text
-- Choose Service
-- See Tooltip of service";

        readonly string ServiceButtonTextTooltip = @"The text visualized on the Service Toolbar Button
- Make the text short to have enough room on the toolbar
- You may insert a Tooltip for more information";
        readonly string ServiceButtonServiceTooltip = @"Choose the service you want to run on the Toolbar Button";

        readonly string ServiceButtonTooltipTooltip =
            @"Every service has a tooltip. This Tooltip is visualized here and later on the Service Button";

        readonly AddinSettings _settings;
        readonly HoToolsGui _hoToolsGui;
        readonly Model _model;

        #region Constructor

        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="hoToolsGui">Object with Control</param>
        public FrmSettingsToolbar(AddinSettings settings, HoToolsGui hoToolsGui)
        {
            InitializeComponent();

            _settings = settings;
            _hoToolsGui = hoToolsGui;
            _model = hoToolsGui.Model;

        }
        #endregion

        #region OnCreateControl
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _settings.UpdateModel(_model);

            // Button support for Service and Search
            chkShowQueryButtons.Checked = _settings.IsShortKeySupport;
            chkShowServiceButtons.Checked = _settings.IsShowServiceButton;

            txtSqlSearchPath.Text = _settings.SqlPaths;

            #region load shortcuts search

            var sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[0];
            txtBtn1Text.Text = sh.KeyText;
            txtBtn1SearchName.Text = sh.KeySearchName;
            txtBtn1SearchTerm.Text = sh.KeySearchTerm;
            txtBtn1SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[1];
            txtBtn2Text.Text = sh.KeyText;
            txtBtn2SearchName.Text = sh.KeySearchName;
            txtBtn2SearchTerm.Text = sh.KeySearchTerm;
            txtBtn2SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[2];
            txtBtn3Text.Text = sh.KeyText;
            txtBtn3SearchName.Text = sh.KeySearchName;
            txtBtn3SearchTerm.Text = sh.KeySearchTerm;
            txtBtn3SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[3];
            txtBtn4Text.Text = sh.KeyText;
            txtBtn4SearchName.Text = sh.KeySearchName;
            txtBtn4SearchTerm.Text = sh.KeySearchTerm;
            txtBtn4SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[4];
            txtBtn5Text.Text = sh.KeyText;
            txtBtn5SearchName.Text = sh.KeySearchName;
            txtBtn5SearchTerm.Text = sh.KeySearchTerm;
            txtBtn5SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[5];
            txtBtn6Text.Text = sh.KeyText;
            txtBtn6SearchName.Text = sh.KeySearchName;
            txtBtn6SearchTerm.Text = sh.KeySearchTerm;
            txtBtn6SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[6];
            txtBtn7Text.Text = sh.KeyText;
            txtBtn7SearchName.Text = sh.KeySearchName;
            txtBtn7SearchTerm.Text = sh.KeySearchTerm;
            txtBtn7SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[7];
            txtBtn8Text.Text = sh.KeyText;
            txtBtn8SearchName.Text = sh.KeySearchName;
            txtBtn8SearchTerm.Text = sh.KeySearchTerm;
            txtBtn8SearchTooltip.Text = sh.KeySearchTooltip;

            #endregion

            #region load possible services


            var lServices1 = new List<Service>();
            var lServices2 = new List<Service>();
            var lServices3 = new List<Service>();
            var lServices4 = new List<Service>();
            var lServices5 = new List<Service>();
            var lServices6 = new List<Service>();
            var lServices7 = new List<Service>();
            var lServices8 = new List<Service>();
            var lServices9 = new List<Service>();
            var lServices10 = new List<Service>();


            // 
            foreach (Service service in _settings.AllServices)
            {
                lServices1.Add(service);
                lServices2.Add(service);
                lServices3.Add(service);
                lServices4.Add(service);
                lServices5.Add(service);
                lServices6.Add(service);
                lServices7.Add(service);
                lServices8.Add(service);
                lServices9.Add(service);
                lServices10.Add(service);
            }

            
            #region set Toolbar Button Services

            cmbService1.DataSource = lServices1;
            cmbService1.DisplayMember = "Description";
            cmbService1.ValueMember = "Id";
            cmbService1.SelectedValue = _settings.ButtonsServiceConfig[0].Id;
            txtButton1TextService.Text = _settings.ButtonsServiceConfig[0].ButtonText;
            txtServiceTooltip1.Text = _settings.ButtonsServiceConfig[0].Help;


            cmbService2.DataSource = lServices2;
            cmbService2.DisplayMember = "Description";
            cmbService2.ValueMember = "Id";
            cmbService2.SelectedValue = _settings.ButtonsServiceConfig[1].Id;
            txtButton2TextService.Text = _settings.ButtonsServiceConfig[1].ButtonText;
            txtServiceTooltip2.Text = _settings.ButtonsServiceConfig[1].Help;

            cmbService3.DataSource = lServices3;
            cmbService3.DisplayMember = "Description";
            cmbService3.ValueMember = "Id";
            cmbService3.SelectedValue = _settings.ButtonsServiceConfig[2].Id;
            txtButton3TextService.Text = _settings.ButtonsServiceConfig[2].ButtonText;
            txtServiceTooltip3.Text = _settings.ButtonsServiceConfig[2].Help;


            cmbService4.DataSource = lServices4;
            cmbService4.DisplayMember = "Description";
            cmbService4.ValueMember = "Id";
            cmbService4.SelectedValue = _settings.ButtonsServiceConfig[3].Id;
            txtButton4TextService.Text = _settings.ButtonsServiceConfig[3].ButtonText;
            txtServiceTooltip4.Text = _settings.ButtonsServiceConfig[3].Help;

            cmbService5.DataSource = lServices5;
            cmbService5.DisplayMember = "Description";
            cmbService5.ValueMember = "Id";
            cmbService5.SelectedValue = _settings.ButtonsServiceConfig[4].Id;
            txtButton5TextService.Text = _settings.ButtonsServiceConfig[4].ButtonText;
            txtServiceTooltip5.Text = _settings.ButtonsServiceConfig[4].Help;

            cmbService6.DataSource = lServices6;
            cmbService6.DisplayMember = "Description";
            cmbService6.ValueMember = "Id";
            cmbService6.SelectedValue = _settings.ButtonsServiceConfig[5].Id;
            txtButton6TextService.Text = _settings.ButtonsServiceConfig[5].ButtonText;
            txtServiceTooltip6.Text = _settings.ButtonsServiceConfig[5].Help;

            cmbService7.DataSource = lServices7;
            cmbService7.DisplayMember = "Description";
            cmbService7.ValueMember = "Id";
            cmbService7.SelectedValue = _settings.ButtonsServiceConfig[6].Id;
            txtButton7TextService.Text = _settings.ButtonsServiceConfig[6].ButtonText;
            txtServiceTooltip7.Text = _settings.ButtonsServiceConfig[6].Help;

            cmbService8.DataSource = lServices8;
            cmbService8.DisplayMember = "Description";
            cmbService8.ValueMember = "Id";
            cmbService8.SelectedValue = _settings.ButtonsServiceConfig[7].Id;
            txtButton8TextService.Text = _settings.ButtonsServiceConfig[7].ButtonText;
            txtServiceTooltip8.Text = _settings.ButtonsServiceConfig[7].Help;

            cmbService9.DataSource = lServices9;
            cmbService9.DisplayMember = "Description";
            cmbService9.ValueMember = "Id";
            cmbService9.SelectedValue = _settings.ButtonsServiceConfig[8].Id;
            txtButton9TextService.Text = _settings.ButtonsServiceConfig[8].ButtonText;
            txtServiceTooltip9.Text = _settings.ButtonsServiceConfig[8].Help;

            cmbService10.DataSource = lServices10;
            cmbService10.DisplayMember = "Description";
            cmbService10.ValueMember = "Id";
            cmbService10.SelectedValue = _settings.ButtonsServiceConfig[9].Id;
            txtButton10TextService.Text = _settings.ButtonsServiceConfig[9].ButtonText;
            txtServiceTooltip10.Text = _settings.ButtonsServiceConfig[9].Help;






            #endregion

            #endregion

            // Button Search Texts
            lblSearchButton1.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton2.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton3.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton4.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton5.SetTooltip(SearchButtonRowTooltip);
            //lblSearchButton6.SetTooltip(SearchButtonRowTooltip);
            //lblSearchButton7.SetTooltip(SearchButtonRowTooltip);
            //lblSearchButton8.SetTooltip(SearchButtonRowTooltip);
            //lblSearchButton9.SetTooltip(SearchButtonRowTooltip);

            // Search Button Text
            lblSearchButtonText.SetTooltip(SearchButtonTextTooltip);
            txtBtn1Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn2Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn3Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn4Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn5Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn6Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn7Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn8Text.SetTooltip(SearchButtonTextTooltip);
            //txtBtn9Text.SetTooltip(SearchButtonTextTooltip);
            //txtBtn10Text.SetTooltip(SearchButtonTextTooltip);


            // Search Name / SQL file name
            lblSearchSearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn1SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn2SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn3SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn4SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn5SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn6SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn7SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn8SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            //txtBtn9SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            //txtBtn10SearchName.SetTooltip(SearchButtonSearchNameTooltip);



            // Search term
            lblSearchSearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn1SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn2SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn3SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn4SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn5SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn6SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn7SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn8SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            //txtBtn9SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            //txtBtn10SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);


            // Search Tooltip
            lblSearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn1SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn2SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn3SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn4SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn5SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn6SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn7SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn8SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            //txtBtn9SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            //txtBtn10SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);



            // Service Button rows
            lblServiceButton1.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton2.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton3.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton4.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton5.SetTooltip(ServiceButtonRowTooltip);
            //lblServiceButton6.SetTooltip(ServiceButtonRowTooltip);
            //lblServiceButton7.SetTooltip(ServiceButtonRowTooltip);
            //lblServiceButton8.SetTooltip(ServiceButtonRowTooltip);
            //lblServiceButton9.SetTooltip(ServiceButtonRowTooltip);
            //lblServiceButton10.SetTooltip(ServiceButtonRowTooltip);


            // Service Button Text
            lblServiceButtonText.SetTooltip(ServiceButtonTextTooltip);
            txtButton1TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton2TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton3TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton4TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton5TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton6TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton7TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton8TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton9TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton10TextService.SetTooltip(ServiceButtonTextTooltip);

            // Service Name Tooltip
            lblServiceButtonName.SetTooltip(ServiceButtonServiceTooltip);
            cmbService1.SetTooltip(ServiceButtonServiceTooltip);
            cmbService2.SetTooltip(ServiceButtonServiceTooltip);
            cmbService3.SetTooltip(ServiceButtonServiceTooltip);
            cmbService4.SetTooltip(ServiceButtonServiceTooltip);
            cmbService5.SetTooltip(ServiceButtonServiceTooltip);
            cmbService6.SetTooltip(ServiceButtonServiceTooltip);
            cmbService7.SetTooltip(ServiceButtonServiceTooltip);
            cmbService8.SetTooltip(ServiceButtonServiceTooltip);
            cmbService9.SetTooltip(ServiceButtonServiceTooltip);
            cmbService10.SetTooltip(ServiceButtonServiceTooltip);

            // Service Button Tooltip
            lblServiceButtonTooltip.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip1.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip2.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip3.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip4.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip5.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip6.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip7.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip8.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip9.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip10.SetTooltip(ServiceButtonTooltipTooltip);
        }
        #endregion


        #region StoreAll ButtonOkClick()

        /// <summary>
        /// Store the settings, ok button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOk_Click(object sender, EventArgs e)
        {
            #region store shortcuts search

            var sh = (EaAddinShortcutSearch) _settings.ButtonsConfigSearch[0];
            sh.KeyText = txtBtn1Text.Text;
            sh.KeySearchName = txtBtn1SearchName.Text;
            sh.KeySearchTerm = txtBtn1SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn1SearchTooltip.Text;
            _settings.ButtonsConfigSearch[0] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsConfigSearch[1];
            sh.KeyText = txtBtn2Text.Text;
            sh.KeySearchName = txtBtn2SearchName.Text;
            sh.KeySearchTerm = txtBtn2SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn2SearchTooltip.Text;
            _settings.ButtonsConfigSearch[1] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsConfigSearch[2];
            sh.KeyText = txtBtn3Text.Text;
            sh.KeySearchName = txtBtn3SearchName.Text;
            sh.KeySearchTerm = txtBtn3SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn3SearchTooltip.Text;
            _settings.ButtonsConfigSearch[2] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsConfigSearch[3];
            sh.KeyText = txtBtn4Text.Text;
            sh.KeySearchName = txtBtn4SearchName.Text;
            sh.KeySearchTerm = txtBtn4SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn4SearchTooltip.Text;
            _settings.ButtonsConfigSearch[3] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsConfigSearch[4];
            sh.KeyText = txtBtn5Text.Text;
            sh.KeySearchName = txtBtn5SearchName.Text;
            sh.KeySearchTerm = txtBtn5SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn5SearchTooltip.Text;
            _settings.ButtonsConfigSearch[4] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[5];
            sh.KeyText = txtBtn6Text.Text;
            sh.KeySearchName = txtBtn6SearchName.Text;
            sh.KeySearchTerm = txtBtn6SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn6SearchTooltip.Text;
            _settings.ButtonsConfigSearch[5] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[6];
            sh.KeyText = txtBtn7Text.Text;
            sh.KeySearchName = txtBtn7SearchName.Text;
            sh.KeySearchTerm = txtBtn7SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn7SearchTooltip.Text;
            _settings.ButtonsConfigSearch[6] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsConfigSearch[7];
            sh.KeyText = txtBtn8Text.Text;
            sh.KeySearchName = txtBtn8SearchName.Text;
            sh.KeySearchTerm = txtBtn8SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn8SearchTooltip.Text;
            _settings.ButtonsConfigSearch[7] = sh;

            #endregion

            #region store Toolbar Buttons Services

            _settings.ButtonsServiceConfig[0].Id = cmbService1.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[0].ButtonText = txtButton1TextService.Text;
            _settings.ButtonsServiceConfig[1].Id = cmbService2.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[1].ButtonText = txtButton2TextService.Text;
            _settings.ButtonsServiceConfig[2].Id = cmbService3.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[2].ButtonText = txtButton3TextService.Text;
            _settings.ButtonsServiceConfig[3].Id = cmbService4.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[3].ButtonText = txtButton4TextService.Text;
            _settings.ButtonsServiceConfig[4].Id = cmbService5.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[4].ButtonText = txtButton5TextService.Text;

            _settings.ButtonsServiceConfig[5].Id = cmbService6.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[5].ButtonText = txtButton6TextService.Text;
            _settings.ButtonsServiceConfig[6].Id = cmbService7.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[6].ButtonText = txtButton7TextService.Text;
            _settings.ButtonsServiceConfig[7].Id = cmbService8.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[7].ButtonText = txtButton8TextService.Text;
            _settings.ButtonsServiceConfig[8].Id = cmbService9.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[8].ButtonText = txtButton9TextService.Text;
            _settings.ButtonsServiceConfig[9].Id = cmbService10.SelectedValue.ToString();
            _settings.ButtonsServiceConfig[9].ButtonText = txtButton10TextService.Text;

            #endregion

            _hoToolsGui.ParameterizeMenusAndButtons(); // hide / unhide Menus & Buttons
                                                         // Toolbar
            _settings.UpdateKeysAndToolbarsServices(); // update dynamic informations like method, texts from configuration

            _hoToolsGui.ParameterizeToolbarSearchButton(); // sets the EA Model Search Buttons on Toolbar
            _hoToolsGui.ParameterizeToolbarServiceButton(); // sets the Services Buttons on Toolbar



            // Button support for Service and Search
            _settings.IsShortKeySupport = chkShowQueryButtons.Checked;
            _settings.IsShowServiceButton = chkShowServiceButtons.Checked;
            // SQL paths
            _settings.SqlPaths = txtSqlSearchPath.Text;

            _settings.Save();
            Close();
        }

        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// The selected service changed. Update tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbService_SelectedIndexChanged(object sender, EventArgs e)
        {

            // get selected value
            ComboBox cmbBox = (ComboBox) sender;
            // get tooltip for selected index
            int index = cmbBox.SelectedIndex;
            if (index == -1) return;
            string tooltip = ""; 
            // Tooltip for service
            if (index < _settings.AllServices.Count)
            {
                tooltip = _settings.AllServices[index].Help;
            }

            if (sender == cmbService1) txtServiceTooltip1.Text = tooltip;
            if (sender == cmbService2) txtServiceTooltip2.Text = tooltip;
            if (sender == cmbService3) txtServiceTooltip3.Text = tooltip;
            if (sender == cmbService4) txtServiceTooltip4.Text = tooltip;
            if (sender == cmbService5) txtServiceTooltip5.Text = tooltip;
            if (sender == cmbService6) txtServiceTooltip6.Text = tooltip;
            if (sender == cmbService7) txtServiceTooltip7.Text = tooltip;
            if (sender == cmbService8) txtServiceTooltip8.Text = tooltip;
            if (sender == cmbService9) txtServiceTooltip9.Text = tooltip;
            if (sender == cmbService10) txtServiceTooltip10.Text = tooltip;


        }

        private void toolbarToolStripMenuSettings_Click(object sender, EventArgs e)
        {
            WikiRef.WikiSettingsToolbar();
        }

        private void wiKiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.Wiki();
        }

        private void hoToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.WikiHoTools();
        }

        private void sQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.WikiSql();
        }

        private void scriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.WikiScript();
        }

        private void findReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.WikiFindAndReplace();
        }
        /// <summary>
        /// Ensured that the modal windows is always on top
        /// - On 4K monitors the dialog sometimes get in the background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSettingsToolbar_Shown(object sender, EventArgs e)
        {
            TopMost = true;
        }
    }
}