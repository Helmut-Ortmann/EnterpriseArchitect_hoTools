using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Control.EaAddinShortcuts;
using EAAddinFramework.Utils;
using hoTools.ActiveX;
using hoTools.EaServices;
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
        readonly AddinControlGui _addinControl;
        readonly Model _model;

        #region Constructor

        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="addinControl">Object with Control</param>
        public FrmSettingsToolbar(AddinSettings settings, AddinControlGui addinControl)
        {
            InitializeComponent();

            _settings = settings;
            _addinControl = addinControl;
            _model = addinControl.Model;

        }
        #endregion

        #region OnCreateControl
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // Button support for Service and Search
            chkShowQueryButtons.Checked = _settings.IsShortKeySupport;
            chkShowServiceButtons.Checked = _settings.IsShowServiceButton;

            txtSqlSearchPath.Text = _settings.SqlPaths;

            #region load shortcuts search

            var sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[0];
            txtBtn1Text.Text = sh.KeyText;
            txtBtn1SearchName.Text = sh.KeySearchName;
            txtBtn1SearchTerm.Text = sh.KeySearchTerm;
            txtBtn1SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[1];
            txtBtn2Text.Text = sh.KeyText;
            txtBtn2SearchName.Text = sh.KeySearchName;
            txtBtn2SearchTerm.Text = sh.KeySearchTerm;
            txtBtn2SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[2];
            txtBtn3Text.Text = sh.KeyText;
            txtBtn3SearchName.Text = sh.KeySearchName;
            txtBtn3SearchTerm.Text = sh.KeySearchTerm;
            txtBtn3SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[3];
            txtBtn4Text.Text = sh.KeyText;
            txtBtn4SearchName.Text = sh.KeySearchName;
            txtBtn4SearchTerm.Text = sh.KeySearchTerm;
            txtBtn4SearchTooltip.Text = sh.KeySearchTooltip;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[4];
            txtBtn5Text.Text = sh.KeyText;
            txtBtn5SearchName.Text = sh.KeySearchName;
            txtBtn5SearchTerm.Text = sh.KeySearchTerm;
            txtBtn5SearchTooltip.Text = sh.KeySearchTooltip;

            #endregion

            #region load possible services


            var lServices1 = new List<EaServices.Service>();
            var lServices2 = new List<EaServices.Service>();
            var lServices3 = new List<EaServices.Service>();
            var lServices4 = new List<EaServices.Service>();
            var lServices5 = new List<EaServices.Service>();


            // 
            foreach (EaServices.ServiceCall service in _settings.AllServices)
            {
                lServices1.Add(service);
                lServices2.Add(service);
                lServices3.Add(service);
                lServices4.Add(service);
                lServices5.Add(service);
            }

            // Load Scripts ant their functions
            List<Script> lscripts = Script.GetEaMaticScripts(_model);
            foreach (var script in lscripts)
            {
                foreach (var scriptFunction in script.Functions)
                {
                    lServices1.Add(new ServiceScript(scriptFunction));
                    lServices2.Add(new ServiceScript(scriptFunction));
                    lServices3.Add(new ServiceScript(scriptFunction));
                    lServices4.Add(new ServiceScript(scriptFunction));
                    lServices5.Add(new ServiceScript(scriptFunction));
                }
            }


            #region set services

            cmbService1.DataSource = lServices1;
            cmbService1.DisplayMember = "Description";
            cmbService1.ValueMember = "Id";
            cmbService1.SelectedValue = _settings.ButtonsServices[0].Guid;
            txtButton1TextService.Text = _settings.ButtonsServices[0].ButtonText;
            txtServiceTooltip1.Text = _settings.ButtonsServices[0].Help;


            cmbService2.DataSource = lServices2;
            cmbService2.DisplayMember = "Description";
            cmbService2.ValueMember = "Id";
            cmbService2.SelectedValue = _settings.ButtonsServices[1].Guid;
            txtButton2TextService.Text = _settings.ButtonsServices[1].ButtonText;
            txtServiceTooltip2.Text = _settings.ButtonsServices[1].Help;

            cmbService3.DataSource = lServices3;
            cmbService3.DisplayMember = "Description";
            cmbService3.ValueMember = "Id";
            cmbService3.SelectedValue = _settings.ButtonsServices[2].Guid;
            txtButton3TextService.Text = _settings.ButtonsServices[2].ButtonText;
            txtServiceTooltip3.Text = _settings.ButtonsServices[2].Help;


            cmbService4.DataSource = lServices4;
            cmbService4.DisplayMember = "Description";
            cmbService4.ValueMember = "Id";
            cmbService4.SelectedValue = _settings.ButtonsServices[3].Guid;
            txtButton4TextService.Text = _settings.ButtonsServices[3].ButtonText;
            txtServiceTooltip4.Text = _settings.ButtonsServices[3].Help;

            cmbService5.DataSource = lServices5;
            cmbService5.DisplayMember = "Description";
            cmbService5.ValueMember = "Id";
            cmbService5.SelectedValue = _settings.ButtonsServices[4].Guid;
            txtButton5TextService.Text = _settings.ButtonsServices[4].ButtonText;
            txtServiceTooltip5.Text = _settings.ButtonsServices[4].Help;
            #endregion

            #endregion

            // Button Texts
            lblSearchButton1.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton2.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton3.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton4.SetTooltip(SearchButtonRowTooltip);
            lblSearchButton5.SetTooltip(SearchButtonRowTooltip);

            // Search Button Text
            lblSearchButtonText.SetTooltip(SearchButtonTextTooltip);
            txtBtn1Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn2Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn3Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn4Text.SetTooltip(SearchButtonTextTooltip);
            txtBtn5Text.SetTooltip(SearchButtonTextTooltip);

            // Search Name / SQL file name
            lblSearchSearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn1SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn2SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn3SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn4SearchName.SetTooltip(SearchButtonSearchNameTooltip);
            txtBtn5SearchName.SetTooltip(SearchButtonSearchNameTooltip);

            // Search term
            lblSearchSearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn1SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn2SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn3SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn4SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);
            txtBtn5SearchTerm.SetTooltip(SearchButtonSearchTermTooltip);

            // Search Tooltip
            lblSearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn1SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn2SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn3SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn4SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);
            txtBtn5SearchTooltip.SetTooltip(SearchButtonTooltipTooltip);


            // Service Button rows
            lblServiceButton1.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton2.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton3.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton4.SetTooltip(ServiceButtonRowTooltip);
            lblServiceButton5.SetTooltip(ServiceButtonRowTooltip);


            // Service Button Text
            lblServiceButtonText.SetTooltip(ServiceButtonTextTooltip);
            txtButton1TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton2TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton3TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton4TextService.SetTooltip(ServiceButtonTextTooltip);
            txtButton5TextService.SetTooltip(ServiceButtonTextTooltip);

            // Service Name Tooltip
            lblServiceButtonName.SetTooltip(ServiceButtonServiceTooltip);
            cmbService1.SetTooltip(ServiceButtonServiceTooltip);
            cmbService2.SetTooltip(ServiceButtonServiceTooltip);
            cmbService3.SetTooltip(ServiceButtonServiceTooltip);
            cmbService4.SetTooltip(ServiceButtonServiceTooltip);
            cmbService5.SetTooltip(ServiceButtonServiceTooltip);

            // Service Button Tooltip
            lblServiceButtonTooltip.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip1.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip2.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip3.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip4.SetTooltip(ServiceButtonTooltipTooltip);
            txtServiceTooltip5.SetTooltip(ServiceButtonTooltipTooltip);
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

            var sh = (EaAddinShortcutSearch) _settings.ButtonsSearch[0];
            sh.KeyText = txtBtn1Text.Text;
            sh.KeySearchName = txtBtn1SearchName.Text;
            sh.KeySearchTerm = txtBtn1SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn1SearchTooltip.Text;
            _settings.ButtonsSearch[0] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsSearch[1];
            sh.KeyText = txtBtn2Text.Text;
            sh.KeySearchName = txtBtn2SearchName.Text;
            sh.KeySearchTerm = txtBtn2SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn2SearchTooltip.Text;
            _settings.ButtonsSearch[1] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsSearch[2];
            sh.KeyText = txtBtn3Text.Text;
            sh.KeySearchName = txtBtn3SearchName.Text;
            sh.KeySearchTerm = txtBtn3SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn3SearchTooltip.Text;
            _settings.ButtonsSearch[2] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsSearch[3];
            sh.KeyText = txtBtn4Text.Text;
            sh.KeySearchName = txtBtn4SearchName.Text;
            sh.KeySearchTerm = txtBtn4SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn4SearchTooltip.Text;
            _settings.ButtonsSearch[3] = sh;

            sh = (EaAddinShortcutSearch) _settings.ButtonsSearch[4];
            sh.KeyText = txtBtn5Text.Text;
            sh.KeySearchName = txtBtn5SearchName.Text;
            sh.KeySearchTerm = txtBtn5SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn5SearchTooltip.Text;
            _settings.ButtonsSearch[4] = sh;

            #endregion

            #region store shortcut services

            _settings.ButtonsServices[0].Guid = cmbService1.SelectedValue.ToString();
            _settings.ButtonsServices[0].ButtonText = txtButton1TextService.Text;
            _settings.ButtonsServices[1].Guid = cmbService2.SelectedValue.ToString();
            _settings.ButtonsServices[1].ButtonText = txtButton2TextService.Text;
            _settings.ButtonsServices[2].Guid = cmbService3.SelectedValue.ToString();
            _settings.ButtonsServices[2].ButtonText = txtButton3TextService.Text;
            _settings.ButtonsServices[3].Guid = cmbService4.SelectedValue.ToString();
            _settings.ButtonsServices[3].ButtonText = txtButton4TextService.Text;
            _settings.ButtonsServices[4].Guid = cmbService5.SelectedValue.ToString();
            _settings.ButtonsServices[4].ButtonText = txtButton5TextService.Text;

            #endregion

            _addinControl.ParameterizeMenusAndButtons(); // hide / unhide Menus & Buttons
            _addinControl.ParameterizeSearchButton(); // sets the EA Model Search Buttons on Toolbar
            _addinControl.ParameterizeServiceButton(); // sets the Services Buttons on Toolbar

            _settings.UpdateSearchesAndServices(); // update dynamic informations like method, texts from configuration

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


        }
    }
}