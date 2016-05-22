using System;
using System.Collections.Generic;
using System.Windows.Forms;
using hoTools.ActiveX;
using GlobalHotkeys;
using hoTools.Settings;
using Control.EaAddinShortcuts;

namespace hoTools.Settings.Key

{
    /// <summary>
    /// Key Settings (Shortcuts)
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsKey : Form
    {   
        AddinSettings _settings;
        AddinControlGUI _addinControl ;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="addinControl">Object with Control</param>
        public FrmSettingsKey(AddinSettings settings, AddinControlGUI addinControl)
        {
            InitializeComponent();

            _settings = settings;
            _addinControl = addinControl;

          
          
           

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


            foreach (hoTools.EaServices.ServiceCall service in _settings.allServices)
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

           

           

            #region Global Shortcuts Service
            // Global Keys/Shortcuts
            cmbGlobalKey1Service.DataSource = l_services11;
            cmbGlobalKey1Service.DisplayMember = "Description";
            cmbGlobalKey1Service.ValueMember = "GUID";
            cmbGlobalKey1Service.SelectedValue = _settings.globalShortcutsService[0].GUID;
            //cmbGlobalKey1Service.Text = _settings.shortcutsServices[0].Help;


            cmbGlobalKey2Service.DataSource = l_services12;
            cmbGlobalKey2Service.DisplayMember = "Description";
            cmbGlobalKey2Service.ValueMember = "GUID";
            cmbGlobalKey2Service.SelectedValue = _settings.globalShortcutsService[1].GUID;
            //cmbGlobalKey2Service.Text = _settings.shortcutsServices[1].Help;

            cmbGlobalKey3Service.DataSource = l_services13;
            cmbGlobalKey3Service.DisplayMember = "Description";
            cmbGlobalKey3Service.ValueMember = "GUID";
            cmbGlobalKey3Service.SelectedValue = _settings.globalShortcutsService[2].GUID;
            //cmbGlobalKey3Service.Text = _settings.shortcutsServices[2].Help;


            cmbGlobalKey4Service.DataSource = l_services14;
            cmbGlobalKey4Service.DisplayMember = "Description";
            cmbGlobalKey4Service.ValueMember = "GUID";
            cmbGlobalKey4Service.SelectedValue = _settings.globalShortcutsService[3].GUID;
            //cmbGlobalKey4Service.Text = _settings.shortcutsServices[3].Help;


            cmbGlobalKey5Service.DataSource = l_services15;
            cmbGlobalKey5Service.DisplayMember = "Description";
            cmbGlobalKey5Service.ValueMember = "GUID";
            cmbGlobalKey5Service.SelectedValue = _settings.globalShortcutsService[4].GUID;
            //cmbGlobalKey5Service.Text = _settings.shortcutsServices[4].Help;

            cmbGlobalKey1Tooltip.Text = _settings.globalShortcutsService[0].Tooltip;
            cmbGlobalKey2Tooltip.Text = _settings.globalShortcutsService[1].Tooltip;
            cmbGlobalKey3Tooltip.Text = _settings.globalShortcutsService[2].Tooltip;
            cmbGlobalKey4Tooltip.Text = _settings.globalShortcutsService[3].Tooltip;
            cmbGlobalKey5Tooltip.Text = _settings.globalShortcutsService[4].Tooltip;

            Dictionary<string, Keys> l_GlobalKeys1;
            Dictionary<string, Keys> l_GlobalKeys = GlobalKeysConfig.getKeys();
            Dictionary<string, Modifiers> l_GlobalModifiers = GlobalKeysConfig.getModifiers();

            // Hot key services
            cmbGlobalKeyService1Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeyService1Key.DisplayMember = "Key";
            cmbGlobalKeyService1Key.ValueMember = "Key";

            cmbGlobalKeyService1Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService1Mod1.DisplayMember = "Key";
            cmbGlobalKeyService1Mod1.ValueMember = "Key";

            cmbGlobalKeyService1Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService1Mod2.DisplayMember = "Key";
            cmbGlobalKeyService1Mod2.ValueMember = "Key";

            cmbGlobalKeyService1Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService1Mod3.DisplayMember = "Key";
            cmbGlobalKeyService1Mod3.ValueMember = "Key";

            cmbGlobalKeyService1Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService1Mod4.DisplayMember = "Key";
            cmbGlobalKeyService1Mod4.ValueMember = "Key";

            cmbGlobalKeyService2Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeyService2Key.DisplayMember = "Key";
            cmbGlobalKeyService2Key.ValueMember = "Key";

            cmbGlobalKeyService2Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService2Mod1.DisplayMember = "Key";
            cmbGlobalKeyService2Mod1.ValueMember = "Key";

            cmbGlobalKeyService2Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService2Mod2.DisplayMember = "Key";
            cmbGlobalKeyService2Mod2.ValueMember = "Key";

            cmbGlobalKeyService2Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService2Mod3.DisplayMember = "Key";
            cmbGlobalKeyService2Mod3.ValueMember = "Key";

            cmbGlobalKeyService2Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService2Mod4.DisplayMember = "Key";
            cmbGlobalKeyService2Mod4.ValueMember = "Key";

            cmbGlobalKeyService3Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeyService3Key.DisplayMember = "Key";
            cmbGlobalKeyService3Key.ValueMember = "Key";

            cmbGlobalKeyService3Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService3Mod1.DisplayMember = "Key";
            cmbGlobalKeyService3Mod1.ValueMember = "Key";

            cmbGlobalKeyService3Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService3Mod2.DisplayMember = "Key";
            cmbGlobalKeyService3Mod2.ValueMember = "Key";

            cmbGlobalKeyService3Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService3Mod3.DisplayMember = "Key";
            cmbGlobalKeyService3Mod3.ValueMember = "Key";

            cmbGlobalKeyService3Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService3Mod4.DisplayMember = "Key";
            cmbGlobalKeyService3Mod4.ValueMember = "Key";

            cmbGlobalKeyService4Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeyService4Key.DisplayMember = "Key";
            cmbGlobalKeyService4Key.ValueMember = "Key";

            cmbGlobalKeyService4Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService4Mod1.DisplayMember = "Key";
            cmbGlobalKeyService4Mod1.ValueMember = "Key";

            cmbGlobalKeyService4Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService4Mod2.DisplayMember = "Key";
            cmbGlobalKeyService4Mod2.ValueMember = "Key";

            cmbGlobalKeyService4Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService4Mod3.DisplayMember = "Key";
            cmbGlobalKeyService4Mod3.ValueMember = "Key";

            cmbGlobalKeyService4Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService4Mod4.DisplayMember = "Key";
            cmbGlobalKeyService4Mod4.ValueMember = "Key";

            cmbGlobalKeyService5Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeyService5Key.DisplayMember = "Key";
            cmbGlobalKeyService5Key.ValueMember = "Key";

            cmbGlobalKeyService5Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService5Mod1.DisplayMember = "Key";
            cmbGlobalKeyService5Mod1.ValueMember = "Key";

            cmbGlobalKeyService5Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService5Mod2.DisplayMember = "Key";
            cmbGlobalKeyService5Mod2.ValueMember = "Key";

            cmbGlobalKeyService5Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService5Mod3.DisplayMember = "Key";
            cmbGlobalKeyService5Mod3.ValueMember = "Key";

            cmbGlobalKeyService5Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeyService5Mod4.DisplayMember = "Key";
            cmbGlobalKeyService5Mod4.ValueMember = "Key";

            cmbGlobalKeyService1Key.SelectedValue = _settings.globalShortcutsService[0].Key;
            cmbGlobalKeyService1Mod1.SelectedValue = _settings.globalShortcutsService[0].Modifier1;
            cmbGlobalKeyService1Mod2.SelectedValue = _settings.globalShortcutsService[0].Modifier2;
            cmbGlobalKeyService1Mod3.SelectedValue = _settings.globalShortcutsService[0].Modifier3;
            cmbGlobalKeyService1Mod4.SelectedValue = _settings.globalShortcutsService[0].Modifier4;

            cmbGlobalKeyService2Key.SelectedValue = _settings.globalShortcutsService[1].Key;
            cmbGlobalKeyService2Mod1.SelectedValue = _settings.globalShortcutsService[1].Modifier1;
            cmbGlobalKeyService2Mod2.SelectedValue = _settings.globalShortcutsService[1].Modifier2;
            cmbGlobalKeyService2Mod3.SelectedValue = _settings.globalShortcutsService[1].Modifier3;
            cmbGlobalKeyService2Mod4.SelectedValue = _settings.globalShortcutsService[1].Modifier4;

            cmbGlobalKeyService3Key.SelectedValue = _settings.globalShortcutsService[2].Key;
            cmbGlobalKeyService3Mod1.SelectedValue = _settings.globalShortcutsService[2].Modifier1;
            cmbGlobalKeyService3Mod2.SelectedValue = _settings.globalShortcutsService[2].Modifier2;
            cmbGlobalKeyService3Mod3.SelectedValue = _settings.globalShortcutsService[2].Modifier3;
            cmbGlobalKeyService3Mod4.SelectedValue = _settings.globalShortcutsService[2].Modifier4;

            cmbGlobalKeyService4Key.SelectedValue = _settings.globalShortcutsService[3].Key;
            cmbGlobalKeyService4Mod1.SelectedValue = _settings.globalShortcutsService[3].Modifier1;
            cmbGlobalKeyService4Mod2.SelectedValue = _settings.globalShortcutsService[3].Modifier2;
            cmbGlobalKeyService4Mod3.SelectedValue = _settings.globalShortcutsService[3].Modifier3;
            cmbGlobalKeyService4Mod4.SelectedValue = _settings.globalShortcutsService[3].Modifier4;

            cmbGlobalKeyService5Key.SelectedValue = _settings.globalShortcutsService[4].Key;
            cmbGlobalKeyService5Mod1.SelectedValue = _settings.globalShortcutsService[4].Modifier1;
            cmbGlobalKeyService5Mod2.SelectedValue = _settings.globalShortcutsService[4].Modifier2;
            cmbGlobalKeyService5Mod3.SelectedValue = _settings.globalShortcutsService[4].Modifier3;
            cmbGlobalKeyService5Mod4.SelectedValue = _settings.globalShortcutsService[4].Modifier4;
            #endregion

            // Search
            #region Global Key Search
            // Search
            cmbGlobalKeySearch1Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeySearch1Key.DisplayMember = "Key";
            cmbGlobalKeySearch1Key.ValueMember = "Key";

            cmbGlobalKeySearch1Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch1Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod1.ValueMember = "Key";

            cmbGlobalKeySearch1Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch1Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod2.ValueMember = "Key";

            cmbGlobalKeySearch1Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch1Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod3.ValueMember = "Key";

            cmbGlobalKeySearch1Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch1Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod4.ValueMember = "Key";

            cmbGlobalKeySearch2Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeySearch2Key.DisplayMember = "Key";
            cmbGlobalKeySearch2Key.ValueMember = "Key";

            cmbGlobalKeySearch2Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch2Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod1.ValueMember = "Key";

            cmbGlobalKeySearch2Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch2Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod2.ValueMember = "Key";

            cmbGlobalKeySearch2Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch2Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod3.ValueMember = "Key";

            cmbGlobalKeySearch2Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch2Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod4.ValueMember = "Key";

            cmbGlobalKeySearch3Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeySearch3Key.DisplayMember = "Key";
            cmbGlobalKeySearch3Key.ValueMember = "Key";

            cmbGlobalKeySearch3Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch3Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod1.ValueMember = "Key";

            cmbGlobalKeySearch3Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch3Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod2.ValueMember = "Key";

            cmbGlobalKeySearch3Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch3Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod3.ValueMember = "Key";

            cmbGlobalKeySearch3Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch3Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod4.ValueMember = "Key";

            cmbGlobalKeySearch4Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeySearch4Key.DisplayMember = "Key";
            cmbGlobalKeySearch4Key.ValueMember = "Key";

            cmbGlobalKeySearch4Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch4Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod1.ValueMember = "Key";

            cmbGlobalKeySearch4Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch4Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod2.ValueMember = "Key";

            cmbGlobalKeySearch4Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch4Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod3.ValueMember = "Key";

            cmbGlobalKeySearch4Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch4Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod4.ValueMember = "Key";

            cmbGlobalKeySearch5Key.DataSource = new BindingSource(l_GlobalKeys, null);
            cmbGlobalKeySearch5Key.DisplayMember = "Key";
            cmbGlobalKeySearch5Key.ValueMember = "Key";

            cmbGlobalKeySearch5Mod1.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch5Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod1.ValueMember = "Key";

            cmbGlobalKeySearch5Mod2.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch5Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod2.ValueMember = "Key";

            cmbGlobalKeySearch5Mod3.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch5Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod3.ValueMember = "Key";

            cmbGlobalKeySearch5Mod4.DataSource = new BindingSource(l_GlobalModifiers, null);
            cmbGlobalKeySearch5Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod4.ValueMember = "Key";

            cmbGlobalKeySearch1Key.SelectedValue = _settings.globalShortcutsSearch[0].Key;
            cmbGlobalKeySearch1Mod1.SelectedValue = _settings.globalShortcutsSearch[0].Modifier1;
            cmbGlobalKeySearch1Mod2.SelectedValue = _settings.globalShortcutsSearch[0].Modifier2;
            cmbGlobalKeySearch1Mod3.SelectedValue = _settings.globalShortcutsSearch[0].Modifier3;
            cmbGlobalKeySearch1Mod4.SelectedValue = _settings.globalShortcutsSearch[0].Modifier4;
            cmbGlobalKeySearch1SearchName.Text = _settings.globalShortcutsSearch[0].SearchName;
            cmbGlobalKeySearch1SearchTerm.Text = _settings.globalShortcutsSearch[0].SearchTerm;

            cmbGlobalKeySearch2Key.SelectedValue = _settings.globalShortcutsSearch[1].Key;
            cmbGlobalKeySearch2Mod1.SelectedValue = _settings.globalShortcutsSearch[1].Modifier1;
            cmbGlobalKeySearch2Mod2.SelectedValue = _settings.globalShortcutsSearch[1].Modifier2;
            cmbGlobalKeySearch2Mod3.SelectedValue = _settings.globalShortcutsSearch[1].Modifier3;
            cmbGlobalKeySearch2Mod4.SelectedValue = _settings.globalShortcutsSearch[1].Modifier4;
            cmbGlobalKeySearch2SearchName.Text = _settings.globalShortcutsSearch[1].SearchName;
            cmbGlobalKeySearch2SearchTerm.Text = _settings.globalShortcutsSearch[1].SearchTerm;

            cmbGlobalKeySearch3Key.SelectedValue = _settings.globalShortcutsSearch[2].Key;
            cmbGlobalKeySearch3Mod1.SelectedValue = _settings.globalShortcutsSearch[2].Modifier1;
            cmbGlobalKeySearch3Mod2.SelectedValue = _settings.globalShortcutsSearch[2].Modifier2;
            cmbGlobalKeySearch3Mod3.SelectedValue = _settings.globalShortcutsSearch[2].Modifier3;
            cmbGlobalKeySearch3Mod4.SelectedValue = _settings.globalShortcutsSearch[2].Modifier4;
            cmbGlobalKeySearch3SearchName.Text = _settings.globalShortcutsSearch[2].SearchName;
            cmbGlobalKeySearch3SearchTerm.Text = _settings.globalShortcutsSearch[2].SearchTerm;

            cmbGlobalKeySearch4Key.SelectedValue = _settings.globalShortcutsSearch[3].Key;
            cmbGlobalKeySearch4Mod1.SelectedValue = _settings.globalShortcutsSearch[3].Modifier1;
            cmbGlobalKeySearch4Mod2.SelectedValue = _settings.globalShortcutsSearch[3].Modifier2;
            cmbGlobalKeySearch4Mod3.SelectedValue = _settings.globalShortcutsSearch[3].Modifier3;
            cmbGlobalKeySearch4Mod4.SelectedValue = _settings.globalShortcutsSearch[3].Modifier4;
            cmbGlobalKeySearch4SearchName.Text = _settings.globalShortcutsSearch[3].SearchName;
            cmbGlobalKeySearch4SearchTerm.Text = _settings.globalShortcutsSearch[3].SearchTerm;

            cmbGlobalKeySearch5Key.SelectedValue = _settings.globalShortcutsSearch[4].Key;
            cmbGlobalKeySearch5Mod1.SelectedValue = _settings.globalShortcutsSearch[4].Modifier1;
            cmbGlobalKeySearch5Mod2.SelectedValue = _settings.globalShortcutsSearch[4].Modifier2;
            cmbGlobalKeySearch5Mod3.SelectedValue = _settings.globalShortcutsSearch[4].Modifier3;
            cmbGlobalKeySearch5Mod4.SelectedValue = _settings.globalShortcutsSearch[4].Modifier4;
            cmbGlobalKeySearch5SearchName.Text = _settings.globalShortcutsSearch[4].SearchName;
            cmbGlobalKeySearch5SearchTerm.Text = _settings.globalShortcutsSearch[4].SearchTerm;
            #endregion Global Key Search


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
           


    
           

            #region store global services
            // Global Services via hot key
            _settings.globalShortcutsService[0].Key = cmbGlobalKeyService1Key.SelectedValue.ToString();
            _settings.globalShortcutsService[0].Modifier1 = cmbGlobalKeyService1Mod1.SelectedValue.ToString();
            _settings.globalShortcutsService[0].Modifier2 = cmbGlobalKeyService1Mod2.SelectedValue.ToString();
            _settings.globalShortcutsService[0].Modifier3 = cmbGlobalKeyService1Mod3.SelectedValue.ToString();
            _settings.globalShortcutsService[0].Modifier4 = cmbGlobalKeyService1Mod4.SelectedValue.ToString();
            _settings.globalShortcutsService[0].GUID = cmbGlobalKey1Service.SelectedValue.ToString();
            _settings.globalShortcutsService[0].Description = cmbGlobalKey1Tooltip.Text;

            _settings.globalShortcutsService[1].Key = cmbGlobalKeyService2Key.SelectedValue.ToString();
            _settings.globalShortcutsService[1].Modifier1 = cmbGlobalKeyService2Mod1.SelectedValue.ToString();
            _settings.globalShortcutsService[1].Modifier2 = cmbGlobalKeyService2Mod2.SelectedValue.ToString();
            _settings.globalShortcutsService[1].Modifier3 = cmbGlobalKeyService2Mod3.SelectedValue.ToString();
            _settings.globalShortcutsService[1].Modifier4 = cmbGlobalKeyService2Mod4.SelectedValue.ToString();
            _settings.globalShortcutsService[1].GUID = cmbGlobalKey2Service.SelectedValue.ToString();
            _settings.globalShortcutsService[1].Description = cmbGlobalKey2Tooltip.Text;

            _settings.globalShortcutsService[2].Key = cmbGlobalKeyService3Key.SelectedValue.ToString();
            _settings.globalShortcutsService[2].Modifier1 = cmbGlobalKeyService3Mod1.SelectedValue.ToString();
            _settings.globalShortcutsService[2].Modifier2 = cmbGlobalKeyService3Mod2.SelectedValue.ToString();
            _settings.globalShortcutsService[2].Modifier3 = cmbGlobalKeyService3Mod3.SelectedValue.ToString();
            _settings.globalShortcutsService[2].Modifier4 = cmbGlobalKeyService3Mod4.SelectedValue.ToString();
            _settings.globalShortcutsService[2].GUID = cmbGlobalKey3Service.SelectedValue.ToString();
            _settings.globalShortcutsService[2].Description = cmbGlobalKey3Tooltip.Text;

            _settings.globalShortcutsService[3].Key = cmbGlobalKeyService4Key.SelectedValue.ToString();
            _settings.globalShortcutsService[3].Modifier1 = cmbGlobalKeyService4Mod1.SelectedValue.ToString();
            _settings.globalShortcutsService[3].Modifier2 = cmbGlobalKeyService4Mod2.SelectedValue.ToString();
            _settings.globalShortcutsService[3].Modifier3 = cmbGlobalKeyService4Mod3.SelectedValue.ToString();
            _settings.globalShortcutsService[3].Modifier4 = cmbGlobalKeyService4Mod4.SelectedValue.ToString();
            _settings.globalShortcutsService[3].GUID = cmbGlobalKey4Service.SelectedValue.ToString();
            _settings.globalShortcutsService[3].Description = cmbGlobalKey4Tooltip.Text;

            _settings.globalShortcutsService[4].Key = cmbGlobalKeyService5Key.SelectedValue.ToString();
            _settings.globalShortcutsService[4].Modifier1 = cmbGlobalKeyService5Mod1.SelectedValue.ToString();
            _settings.globalShortcutsService[4].Modifier2 = cmbGlobalKeyService5Mod2.SelectedValue.ToString();
            _settings.globalShortcutsService[4].Modifier3 = cmbGlobalKeyService5Mod3.SelectedValue.ToString();
            _settings.globalShortcutsService[4].Modifier4 = cmbGlobalKeyService5Mod4.SelectedValue.ToString();
            _settings.globalShortcutsService[4].GUID = cmbGlobalKey5Service.SelectedValue.ToString();
            _settings.globalShortcutsService[4].Description = cmbGlobalKey5Tooltip.Text;
            #endregion

            #region store global searches
            // Global Searches via hot key
            _settings.globalShortcutsSearch[0].Key = cmbGlobalKeySearch1Key.SelectedValue.ToString();
            _settings.globalShortcutsSearch[0].Modifier1 = cmbGlobalKeySearch1Mod1.SelectedValue.ToString();
            _settings.globalShortcutsSearch[0].Modifier2 = cmbGlobalKeySearch1Mod2.SelectedValue.ToString();
            _settings.globalShortcutsSearch[0].Modifier3 = cmbGlobalKeySearch1Mod3.SelectedValue.ToString();
            _settings.globalShortcutsSearch[0].Modifier4 = cmbGlobalKeySearch1Mod4.SelectedValue.ToString();
            _settings.globalShortcutsSearch[0].SearchName = cmbGlobalKeySearch1SearchName.Text;
            _settings.globalShortcutsSearch[0].SearchTerm = cmbGlobalKeySearch1SearchTerm.Text;

            _settings.globalShortcutsSearch[1].Key = cmbGlobalKeySearch2Key.SelectedValue.ToString();
            _settings.globalShortcutsSearch[1].Modifier1 = cmbGlobalKeySearch2Mod1.SelectedValue.ToString();
            _settings.globalShortcutsSearch[1].Modifier2 = cmbGlobalKeySearch2Mod2.SelectedValue.ToString();
            _settings.globalShortcutsSearch[1].Modifier3 = cmbGlobalKeySearch2Mod3.SelectedValue.ToString();
            _settings.globalShortcutsSearch[1].Modifier4 = cmbGlobalKeySearch2Mod4.SelectedValue.ToString();
            _settings.globalShortcutsSearch[1].SearchName = cmbGlobalKeySearch2SearchName.Text;
            _settings.globalShortcutsSearch[1].SearchTerm = cmbGlobalKeySearch2SearchTerm.Text;

            _settings.globalShortcutsSearch[2].Key = cmbGlobalKeySearch3Key.SelectedValue.ToString();
            _settings.globalShortcutsSearch[2].Modifier1 = cmbGlobalKeySearch3Mod1.SelectedValue.ToString();
            _settings.globalShortcutsSearch[2].Modifier2 = cmbGlobalKeySearch3Mod2.SelectedValue.ToString();
            _settings.globalShortcutsSearch[2].Modifier3 = cmbGlobalKeySearch3Mod3.SelectedValue.ToString();
            _settings.globalShortcutsSearch[2].Modifier4 = cmbGlobalKeySearch3Mod4.SelectedValue.ToString();
            _settings.globalShortcutsSearch[2].SearchName = cmbGlobalKeySearch3SearchName.Text;
            _settings.globalShortcutsSearch[2].SearchTerm = cmbGlobalKeySearch3SearchTerm.Text;

            _settings.globalShortcutsSearch[3].Key = cmbGlobalKeySearch4Key.SelectedValue.ToString();
            _settings.globalShortcutsSearch[3].Modifier1 = cmbGlobalKeySearch4Mod1.SelectedValue.ToString();
            _settings.globalShortcutsSearch[3].Modifier2 = cmbGlobalKeySearch4Mod2.SelectedValue.ToString();
            _settings.globalShortcutsSearch[3].Modifier3 = cmbGlobalKeySearch4Mod3.SelectedValue.ToString();
            _settings.globalShortcutsSearch[3].Modifier4 = cmbGlobalKeySearch4Mod4.SelectedValue.ToString();
            _settings.globalShortcutsSearch[3].SearchName = cmbGlobalKeySearch4SearchName.Text;
            _settings.globalShortcutsSearch[3].SearchTerm = cmbGlobalKeySearch4SearchTerm.Text;

            _settings.globalShortcutsSearch[4].Key = cmbGlobalKeySearch5Key.SelectedValue.ToString();
            _settings.globalShortcutsSearch[4].Modifier1 = cmbGlobalKeySearch5Mod1.SelectedValue.ToString();
            _settings.globalShortcutsSearch[4].Modifier2 = cmbGlobalKeySearch5Mod2.SelectedValue.ToString();
            _settings.globalShortcutsSearch[4].Modifier3 = cmbGlobalKeySearch5Mod3.SelectedValue.ToString();
            _settings.globalShortcutsSearch[4].Modifier4 = cmbGlobalKeySearch5Mod4.SelectedValue.ToString();
            _settings.globalShortcutsSearch[4].SearchName = cmbGlobalKeySearch5SearchName.Text;
            _settings.globalShortcutsSearch[4].SearchTerm = cmbGlobalKeySearch5SearchTerm.Text;
            #endregion

            _addinControl.parameterizeMenusAndButtons(); // hide / unhide Menus & Buttons
            _addinControl.parameterizeSearchButton(); // sets the shortcuts
            _addinControl.parameterizeServiceButton(); // sets the shortcuts

            _settings.updateSearchesAndServices(); // update dynamic informations like method, texts from configuration
            this._settings.save();
            this.Close();

        }
        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
