using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Control.EaAddinShortcuts;
using hoTools.ActiveX;

namespace hoTools.Settings.Toolbar

{
    /// <summary>
    /// Key Settings (Shortcuts)
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsToolbar : Form
    {   
        AddinSettings _settings;
        AddinControlGUI _addinControl ;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="addinControl">Object with Control</param>
        public FrmSettingsToolbar(AddinSettings settings, AddinControlGUI addinControl)
        {
            InitializeComponent();

            _settings = settings;
            _addinControl = addinControl;

          
          
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

            #region set possible services
            var l_services1 = new List<hoTools.EaServices.ServiceCall>();
            var l_services2 = new List<hoTools.EaServices.ServiceCall>();
            var l_services3 = new List<hoTools.EaServices.ServiceCall>();
            var l_services4 = new List<hoTools.EaServices.ServiceCall>();
            var l_services11 = new List<hoTools.EaServices.ServiceCall>();
            var l_services12 = new List<hoTools.EaServices.ServiceCall>();
            var l_services13 = new List<hoTools.EaServices.ServiceCall>();
            var l_services14 = new List<hoTools.EaServices.ServiceCall>();
            var l_services15 = new List<hoTools.EaServices.ServiceCall>();


            foreach (hoTools.EaServices.ServiceCall service in _settings.AllServices)
            {
                l_services1.Add(service);
                l_services2.Add(service);
                l_services3.Add(service);
                l_services4.Add(service);
                l_services11.Add(service);
                l_services12.Add(service);
                l_services13.Add(service);
                l_services14.Add(service);
                l_services15.Add(service);
            }
            #endregion

           

            #region set services
            cmbService1.DataSource = _settings.AllServices;
            cmbService1.DisplayMember = "Description";
            cmbService1.ValueMember = "GUID";
            cmbService1.SelectedValue = _settings.ButtonsServices[0].GUID;
            txtButton1TextService.Text = _settings.ButtonsServices[0].ButtonText;
            txtServiceTooltip1.Text = _settings.ButtonsServices[0].Help;


            cmbService2.DataSource = l_services1;
            cmbService2.DisplayMember = "Description";
            cmbService2.ValueMember = "GUID";
            cmbService2.SelectedValue = _settings.ButtonsServices[1].GUID;
            txtButton2TextService.Text = _settings.ButtonsServices[1].ButtonText;
            txtServiceTooltip2.Text = _settings.ButtonsServices[1].Help;

            cmbService3.DataSource = l_services2;
            cmbService3.DisplayMember = "Description";
            cmbService3.ValueMember = "GUID";
            cmbService3.SelectedValue = _settings.ButtonsServices[2].GUID;
            txtButton3TextService.Text = _settings.ButtonsServices[2].ButtonText;
            txtServiceTooltip3.Text = _settings.ButtonsServices[2].Help;


            cmbService4.DataSource = l_services3;
            cmbService4.DisplayMember = "Description";
            cmbService4.ValueMember = "GUID";
            cmbService4.SelectedValue = _settings.ButtonsServices[3].GUID;
            txtButton4TextService.Text = _settings.ButtonsServices[3].ButtonText;
            txtServiceTooltip4.Text = _settings.ButtonsServices[3].Help;

            cmbService5.DataSource = l_services4;
            cmbService5.DisplayMember = "Description";
            cmbService5.ValueMember = "GUID";
            cmbService5.SelectedValue = _settings.ButtonsServices[4].GUID;
            txtButton5TextService.Text = _settings.ButtonsServices[4].ButtonText;
            txtServiceTooltip5.Text = _settings.ButtonsServices[4].Help;
            #endregion

            
            


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
            var sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[0];
            sh.KeyText = txtBtn1Text.Text;
            sh.KeySearchName = txtBtn1SearchName.Text;
            sh.KeySearchTerm = txtBtn1SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn1SearchTooltip.Text;
            _settings.ButtonsSearch[0] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[1];
            sh.KeyText = txtBtn2Text.Text;
            sh.KeySearchName = txtBtn2SearchName.Text;
            sh.KeySearchTerm = txtBtn2SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn2SearchTooltip.Text;
            _settings.ButtonsSearch[1] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[2];
            sh.KeyText = txtBtn3Text.Text;
            sh.KeySearchName = txtBtn3SearchName.Text;
            sh.KeySearchTerm = txtBtn3SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn3SearchTooltip.Text;
            _settings.ButtonsSearch[2] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[3];
            sh.KeyText = txtBtn4Text.Text;
            sh.KeySearchName = txtBtn4SearchName.Text;
            sh.KeySearchTerm = txtBtn4SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn4SearchTooltip.Text;
            _settings.ButtonsSearch[3] = sh;

            sh = (EaAddinShortcutSearch)_settings.ButtonsSearch[4];
            sh.KeyText = txtBtn5Text.Text;
            sh.KeySearchName = txtBtn5SearchName.Text;
            sh.KeySearchTerm = txtBtn5SearchTerm.Text;
            sh.KeySearchTooltip = txtBtn5SearchTooltip.Text;
            _settings.ButtonsSearch[4] = sh;
            #endregion

            #region store shortcut services
            _settings.ButtonsServices[0].GUID = cmbService1.SelectedValue.ToString();
            _settings.ButtonsServices[0].ButtonText = txtButton1TextService.Text;
            _settings.ButtonsServices[1].GUID = cmbService2.SelectedValue.ToString();
            _settings.ButtonsServices[1].ButtonText = txtButton2TextService.Text;
            _settings.ButtonsServices[2].GUID = cmbService3.SelectedValue.ToString();
            _settings.ButtonsServices[2].ButtonText = txtButton3TextService.Text;
            _settings.ButtonsServices[3].GUID = cmbService4.SelectedValue.ToString();
            _settings.ButtonsServices[3].ButtonText = txtButton4TextService.Text;
            _settings.ButtonsServices[4].GUID = cmbService5.SelectedValue.ToString();
            _settings.ButtonsServices[4].ButtonText = txtButton5TextService.Text;
            #endregion

            _addinControl.parameterizeMenusAndButtons(); // hide / unhide Menus & Buttons
            _addinControl.ParameterizeSearchButton(); // sets the EA Model Search Buttons on Toolbar
            _addinControl.ParameterizeServiceButton(); // sets the Services Buttons on Toolbar

            _settings.UpdateSearchesAndServices(); // update dynamic informations like method, texts from configuration
            this._settings.Save();
            this.Close();

        }
        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
