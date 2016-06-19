using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Control.EaAddinShortcuts;
using hoTools.ActiveX;
using hoTools.EaServices;

namespace hoTools.Settings.Toolbar

{
    /// <summary>
    /// Key Settings (Shortcuts)
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsToolbar : Form
    {
        readonly AddinSettings _settings;
        readonly AddinControlGui _addinControl ;

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
            var lServices1 = new List<ServiceCall>();
            var lServices2 = new List<ServiceCall>();
            var lServices3 = new List<ServiceCall>();
            var lServices4 = new List<ServiceCall>();
            var lServices11 = new List<ServiceCall>();
            var lServices12 = new List<ServiceCall>();
            var lServices13 = new List<ServiceCall>();
            var lServices14 = new List<ServiceCall>();
            var lServices15 = new List<ServiceCall>();


            foreach (ServiceCall service in _settings.AllServices)
            {
                lServices1.Add(service);
                lServices2.Add(service);
                lServices3.Add(service);
                lServices4.Add(service);
                lServices11.Add(service);
                lServices12.Add(service);
                lServices13.Add(service);
                lServices14.Add(service);
                lServices15.Add(service);
            }
            #endregion

           

            #region set services
            cmbService1.DataSource = _settings.AllServices;
            cmbService1.DisplayMember = "Description";
            cmbService1.ValueMember = "GUID";
            cmbService1.SelectedValue = _settings.ButtonsServices[0].GUID;
            txtButton1TextService.Text = _settings.ButtonsServices[0].ButtonText;
            txtServiceTooltip1.Text = _settings.ButtonsServices[0].Help;


            cmbService2.DataSource = lServices1;
            cmbService2.DisplayMember = "Description";
            cmbService2.ValueMember = "GUID";
            cmbService2.SelectedValue = _settings.ButtonsServices[1].GUID;
            txtButton2TextService.Text = _settings.ButtonsServices[1].ButtonText;
            txtServiceTooltip2.Text = _settings.ButtonsServices[1].Help;

            cmbService3.DataSource = lServices2;
            cmbService3.DisplayMember = "Description";
            cmbService3.ValueMember = "GUID";
            cmbService3.SelectedValue = _settings.ButtonsServices[2].GUID;
            txtButton3TextService.Text = _settings.ButtonsServices[2].ButtonText;
            txtServiceTooltip3.Text = _settings.ButtonsServices[2].Help;


            cmbService4.DataSource = lServices3;
            cmbService4.DisplayMember = "Description";
            cmbService4.ValueMember = "GUID";
            cmbService4.SelectedValue = _settings.ButtonsServices[3].GUID;
            txtButton4TextService.Text = _settings.ButtonsServices[3].ButtonText;
            txtServiceTooltip4.Text = _settings.ButtonsServices[3].Help;

            cmbService5.DataSource = lServices4;
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

            _addinControl.ParameterizeMenusAndButtons(); // hide / unhide Menus & Buttons
            _addinControl.ParameterizeSearchButton(); // sets the EA Model Search Buttons on Toolbar
            _addinControl.ParameterizeServiceButton(); // sets the Services Buttons on Toolbar

            _settings.UpdateSearchesAndServices(); // update dynamic informations like method, texts from configuration
            _settings.Save();
            Close();

        }
        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
