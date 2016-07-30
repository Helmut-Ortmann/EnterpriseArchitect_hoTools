using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EAAddinFramework.Utils;
using hoTools.ActiveX;
using hoTools.EaServices;
using GlobalHotkeys;


// ReSharper disable once CheckNamespace
namespace hoTools.Settings.Key

{
    /// <summary>
    /// Key Settings (Shortcuts)
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsKey : Form
    {
        readonly AddinSettings _settings;
        readonly AddinControlGui _addinControl ;
        private readonly Model _model;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="addinControl">Object with Control</param>
        public FrmSettingsKey(AddinSettings settings, AddinControlGui addinControl)
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
            // Global key support
            chkShortKeySupport.Checked = _settings.IsShortKeySupport;
            // SQL Paths
            txtSqlSearchPath.Text = _settings.SqlPaths;

            // Load Scripts
            List<Script> _lscripts = Script.GetEaMaticScripts(_model);

            #region set possible services
            // set 5 lists of all possible services
            var lServices1 = new List<Service>();
            var lServices2 = new List<Service>();
            var lServices3 = new List<Service>();
            var lServices4 = new List<Service>();
            var lServices5 = new List<Service>();
            // set all Service Calls
            foreach (Service service in _settings.AllServices)
            {
                lServices1.Add(service);
                lServices2.Add(service);
                lServices3.Add(service);
                lServices4.Add(service);
                lServices5.Add(service);
            }
            #endregion





            #region Global Shortcuts Service
            // Global Keys/Shortcuts
            cmbGlobalKey1Service.DataSource = lServices1;
            cmbGlobalKey1Service.DisplayMember = "Description";
            cmbGlobalKey1Service.ValueMember = "Id";
            cmbGlobalKey1Service.SelectedValue = _settings.GlobalKeysConfig[0].Id;
            cmbGlobalKey1Tooltip.Text = _settings.GlobalKeysConfig[0].Tooltip;


            cmbGlobalKey2Service.DataSource = lServices2;
            cmbGlobalKey2Service.DisplayMember = "Description";
            cmbGlobalKey2Service.ValueMember = "Id";
            cmbGlobalKey2Service.SelectedValue = _settings.GlobalKeysConfig[1].Id;
            cmbGlobalKey2Tooltip.Text = _settings.GlobalKeysConfig[1].Tooltip;


            cmbGlobalKey3Service.DataSource = lServices3;
            cmbGlobalKey3Service.DisplayMember = "Description";
            cmbGlobalKey3Service.ValueMember = "Id";
            cmbGlobalKey3Service.SelectedValue = _settings.GlobalKeysConfig[2].Id;
            cmbGlobalKey3Tooltip.Text = _settings.GlobalKeysConfig[2].Tooltip;


            cmbGlobalKey4Service.DataSource = lServices4;
            cmbGlobalKey4Service.DisplayMember = "Description";
            cmbGlobalKey4Service.ValueMember = "Id";
            cmbGlobalKey4Service.SelectedValue = _settings.GlobalKeysConfig[3].Id;
            cmbGlobalKey4Tooltip.Text = _settings.GlobalKeysConfig[3].Tooltip;


            cmbGlobalKey5Service.DataSource = lServices5;
            cmbGlobalKey5Service.DisplayMember = "Description";
            cmbGlobalKey5Service.ValueMember = "Id";
            cmbGlobalKey5Service.SelectedValue = _settings.GlobalKeysConfig[4].Id;
            cmbGlobalKey5Tooltip.Text = _settings.GlobalKeysConfig[4].Tooltip;








            Dictionary<string, Keys> lGlobalKeys = GlobalKeysConfig.GetKeys();
            Dictionary<string, Modifiers> lGlobalModifiers = GlobalKeysConfig.GetModifiers();

            // Hot key services
            cmbGlobalKeyService1Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeyService1Key.DisplayMember = "Key";
            cmbGlobalKeyService1Key.ValueMember = "Key";

            cmbGlobalKeyService1Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService1Mod1.DisplayMember = "Key";
            cmbGlobalKeyService1Mod1.ValueMember = "Key";

            cmbGlobalKeyService1Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService1Mod2.DisplayMember = "Key";
            cmbGlobalKeyService1Mod2.ValueMember = "Key";

            cmbGlobalKeyService1Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService1Mod3.DisplayMember = "Key";
            cmbGlobalKeyService1Mod3.ValueMember = "Key";

            cmbGlobalKeyService1Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService1Mod4.DisplayMember = "Key";
            cmbGlobalKeyService1Mod4.ValueMember = "Key";

            cmbGlobalKeyService2Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeyService2Key.DisplayMember = "Key";
            cmbGlobalKeyService2Key.ValueMember = "Key";

            cmbGlobalKeyService2Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService2Mod1.DisplayMember = "Key";
            cmbGlobalKeyService2Mod1.ValueMember = "Key";

            cmbGlobalKeyService2Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService2Mod2.DisplayMember = "Key";
            cmbGlobalKeyService2Mod2.ValueMember = "Key";

            cmbGlobalKeyService2Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService2Mod3.DisplayMember = "Key";
            cmbGlobalKeyService2Mod3.ValueMember = "Key";

            cmbGlobalKeyService2Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService2Mod4.DisplayMember = "Key";
            cmbGlobalKeyService2Mod4.ValueMember = "Key";

            cmbGlobalKeyService3Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeyService3Key.DisplayMember = "Key";
            cmbGlobalKeyService3Key.ValueMember = "Key";

            cmbGlobalKeyService3Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService3Mod1.DisplayMember = "Key";
            cmbGlobalKeyService3Mod1.ValueMember = "Key";

            cmbGlobalKeyService3Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService3Mod2.DisplayMember = "Key";
            cmbGlobalKeyService3Mod2.ValueMember = "Key";

            cmbGlobalKeyService3Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService3Mod3.DisplayMember = "Key";
            cmbGlobalKeyService3Mod3.ValueMember = "Key";

            cmbGlobalKeyService3Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService3Mod4.DisplayMember = "Key";
            cmbGlobalKeyService3Mod4.ValueMember = "Key";

            cmbGlobalKeyService4Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeyService4Key.DisplayMember = "Key";
            cmbGlobalKeyService4Key.ValueMember = "Key";

            cmbGlobalKeyService4Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService4Mod1.DisplayMember = "Key";
            cmbGlobalKeyService4Mod1.ValueMember = "Key";

            cmbGlobalKeyService4Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService4Mod2.DisplayMember = "Key";
            cmbGlobalKeyService4Mod2.ValueMember = "Key";

            cmbGlobalKeyService4Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService4Mod3.DisplayMember = "Key";
            cmbGlobalKeyService4Mod3.ValueMember = "Key";

            cmbGlobalKeyService4Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService4Mod4.DisplayMember = "Key";
            cmbGlobalKeyService4Mod4.ValueMember = "Key";

            cmbGlobalKeyService5Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeyService5Key.DisplayMember = "Key";
            cmbGlobalKeyService5Key.ValueMember = "Key";

            cmbGlobalKeyService5Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService5Mod1.DisplayMember = "Key";
            cmbGlobalKeyService5Mod1.ValueMember = "Key";

            cmbGlobalKeyService5Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService5Mod2.DisplayMember = "Key";
            cmbGlobalKeyService5Mod2.ValueMember = "Key";

            cmbGlobalKeyService5Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService5Mod3.DisplayMember = "Key";
            cmbGlobalKeyService5Mod3.ValueMember = "Key";

            cmbGlobalKeyService5Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeyService5Mod4.DisplayMember = "Key";
            cmbGlobalKeyService5Mod4.ValueMember = "Key";

            cmbGlobalKeyService1Key.SelectedValue = _settings.GlobalKeysConfig[0].Key;
            cmbGlobalKeyService1Mod1.SelectedValue = _settings.GlobalKeysConfig[0].Modifier1;
            cmbGlobalKeyService1Mod2.SelectedValue = _settings.GlobalKeysConfig[0].Modifier2;
            cmbGlobalKeyService1Mod3.SelectedValue = _settings.GlobalKeysConfig[0].Modifier3;
            cmbGlobalKeyService1Mod4.SelectedValue = _settings.GlobalKeysConfig[0].Modifier4;

            cmbGlobalKeyService2Key.SelectedValue = _settings.GlobalKeysConfig[1].Key;
            cmbGlobalKeyService2Mod1.SelectedValue = _settings.GlobalKeysConfig[1].Modifier1;
            cmbGlobalKeyService2Mod2.SelectedValue = _settings.GlobalKeysConfig[1].Modifier2;
            cmbGlobalKeyService2Mod3.SelectedValue = _settings.GlobalKeysConfig[1].Modifier3;
            cmbGlobalKeyService2Mod4.SelectedValue = _settings.GlobalKeysConfig[1].Modifier4;

            cmbGlobalKeyService3Key.SelectedValue = _settings.GlobalKeysConfig[2].Key;
            cmbGlobalKeyService3Mod1.SelectedValue = _settings.GlobalKeysConfig[2].Modifier1;
            cmbGlobalKeyService3Mod2.SelectedValue = _settings.GlobalKeysConfig[2].Modifier2;
            cmbGlobalKeyService3Mod3.SelectedValue = _settings.GlobalKeysConfig[2].Modifier3;
            cmbGlobalKeyService3Mod4.SelectedValue = _settings.GlobalKeysConfig[2].Modifier4;

            cmbGlobalKeyService4Key.SelectedValue = _settings.GlobalKeysConfig[3].Key;
            cmbGlobalKeyService4Mod1.SelectedValue = _settings.GlobalKeysConfig[3].Modifier1;
            cmbGlobalKeyService4Mod2.SelectedValue = _settings.GlobalKeysConfig[3].Modifier2;
            cmbGlobalKeyService4Mod3.SelectedValue = _settings.GlobalKeysConfig[3].Modifier3;
            cmbGlobalKeyService4Mod4.SelectedValue = _settings.GlobalKeysConfig[3].Modifier4;

            cmbGlobalKeyService5Key.SelectedValue = _settings.GlobalKeysConfig[4].Key;
            cmbGlobalKeyService5Mod1.SelectedValue = _settings.GlobalKeysConfig[4].Modifier1;
            cmbGlobalKeyService5Mod2.SelectedValue = _settings.GlobalKeysConfig[4].Modifier2;
            cmbGlobalKeyService5Mod3.SelectedValue = _settings.GlobalKeysConfig[4].Modifier3;
            cmbGlobalKeyService5Mod4.SelectedValue = _settings.GlobalKeysConfig[4].Modifier4;
            #endregion

            // Search
            #region Global Key Search
            // Search
            cmbGlobalKeySearch1Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeySearch1Key.DisplayMember = "Key";
            cmbGlobalKeySearch1Key.ValueMember = "Key";

            cmbGlobalKeySearch1Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch1Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod1.ValueMember = "Key";

            cmbGlobalKeySearch1Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch1Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod2.ValueMember = "Key";

            cmbGlobalKeySearch1Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch1Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod3.ValueMember = "Key";

            cmbGlobalKeySearch1Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch1Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch1Mod4.ValueMember = "Key";

            cmbGlobalKeySearch2Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeySearch2Key.DisplayMember = "Key";
            cmbGlobalKeySearch2Key.ValueMember = "Key";

            cmbGlobalKeySearch2Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch2Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod1.ValueMember = "Key";

            cmbGlobalKeySearch2Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch2Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod2.ValueMember = "Key";

            cmbGlobalKeySearch2Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch2Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod3.ValueMember = "Key";

            cmbGlobalKeySearch2Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch2Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch2Mod4.ValueMember = "Key";

            cmbGlobalKeySearch3Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeySearch3Key.DisplayMember = "Key";
            cmbGlobalKeySearch3Key.ValueMember = "Key";

            cmbGlobalKeySearch3Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch3Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod1.ValueMember = "Key";

            cmbGlobalKeySearch3Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch3Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod2.ValueMember = "Key";

            cmbGlobalKeySearch3Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch3Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod3.ValueMember = "Key";

            cmbGlobalKeySearch3Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch3Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch3Mod4.ValueMember = "Key";

            cmbGlobalKeySearch4Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeySearch4Key.DisplayMember = "Key";
            cmbGlobalKeySearch4Key.ValueMember = "Key";

            cmbGlobalKeySearch4Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch4Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod1.ValueMember = "Key";

            cmbGlobalKeySearch4Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch4Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod2.ValueMember = "Key";

            cmbGlobalKeySearch4Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch4Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod3.ValueMember = "Key";

            cmbGlobalKeySearch4Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch4Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch4Mod4.ValueMember = "Key";

            cmbGlobalKeySearch5Key.DataSource = new BindingSource(lGlobalKeys, null);
            cmbGlobalKeySearch5Key.DisplayMember = "Key";
            cmbGlobalKeySearch5Key.ValueMember = "Key";

            cmbGlobalKeySearch5Mod1.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch5Mod1.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod1.ValueMember = "Key";

            cmbGlobalKeySearch5Mod2.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch5Mod2.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod2.ValueMember = "Key";

            cmbGlobalKeySearch5Mod3.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch5Mod3.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod3.ValueMember = "Key";

            cmbGlobalKeySearch5Mod4.DataSource = new BindingSource(lGlobalModifiers, null);
            cmbGlobalKeySearch5Mod4.DisplayMember = "Key";
            cmbGlobalKeySearch5Mod4.ValueMember = "Key";

            cmbGlobalKeySearch1Key.SelectedValue = _settings.GlobalKeysConfigSearch[0].Key;
            cmbGlobalKeySearch1Mod1.SelectedValue = _settings.GlobalKeysConfigSearch[0].Modifier1;
            cmbGlobalKeySearch1Mod2.SelectedValue = _settings.GlobalKeysConfigSearch[0].Modifier2;
            cmbGlobalKeySearch1Mod3.SelectedValue = _settings.GlobalKeysConfigSearch[0].Modifier3;
            cmbGlobalKeySearch1Mod4.SelectedValue = _settings.GlobalKeysConfigSearch[0].Modifier4;
            cmbGlobalKeySearch1SearchName.Text = _settings.GlobalKeysConfigSearch[0].SearchName;
            cmbGlobalKeySearch1SearchTerm.Text = _settings.GlobalKeysConfigSearch[0].SearchTerm;

            cmbGlobalKeySearch2Key.SelectedValue = _settings.GlobalKeysConfigSearch[1].Key;
            cmbGlobalKeySearch2Mod1.SelectedValue = _settings.GlobalKeysConfigSearch[1].Modifier1;
            cmbGlobalKeySearch2Mod2.SelectedValue = _settings.GlobalKeysConfigSearch[1].Modifier2;
            cmbGlobalKeySearch2Mod3.SelectedValue = _settings.GlobalKeysConfigSearch[1].Modifier3;
            cmbGlobalKeySearch2Mod4.SelectedValue = _settings.GlobalKeysConfigSearch[1].Modifier4;
            cmbGlobalKeySearch2SearchName.Text = _settings.GlobalKeysConfigSearch[1].SearchName;
            cmbGlobalKeySearch2SearchTerm.Text = _settings.GlobalKeysConfigSearch[1].SearchTerm;

            cmbGlobalKeySearch3Key.SelectedValue = _settings.GlobalKeysConfigSearch[2].Key;
            cmbGlobalKeySearch3Mod1.SelectedValue = _settings.GlobalKeysConfigSearch[2].Modifier1;
            cmbGlobalKeySearch3Mod2.SelectedValue = _settings.GlobalKeysConfigSearch[2].Modifier2;
            cmbGlobalKeySearch3Mod3.SelectedValue = _settings.GlobalKeysConfigSearch[2].Modifier3;
            cmbGlobalKeySearch3Mod4.SelectedValue = _settings.GlobalKeysConfigSearch[2].Modifier4;
            cmbGlobalKeySearch3SearchName.Text = _settings.GlobalKeysConfigSearch[2].SearchName;
            cmbGlobalKeySearch3SearchTerm.Text = _settings.GlobalKeysConfigSearch[2].SearchTerm;

            cmbGlobalKeySearch4Key.SelectedValue = _settings.GlobalKeysConfigSearch[3].Key;
            cmbGlobalKeySearch4Mod1.SelectedValue = _settings.GlobalKeysConfigSearch[3].Modifier1;
            cmbGlobalKeySearch4Mod2.SelectedValue = _settings.GlobalKeysConfigSearch[3].Modifier2;
            cmbGlobalKeySearch4Mod3.SelectedValue = _settings.GlobalKeysConfigSearch[3].Modifier3;
            cmbGlobalKeySearch4Mod4.SelectedValue = _settings.GlobalKeysConfigSearch[3].Modifier4;
            cmbGlobalKeySearch4SearchName.Text = _settings.GlobalKeysConfigSearch[3].SearchName;
            cmbGlobalKeySearch4SearchTerm.Text = _settings.GlobalKeysConfigSearch[3].SearchTerm;

            cmbGlobalKeySearch5Key.SelectedValue = _settings.GlobalKeysConfigSearch[4].Key;
            cmbGlobalKeySearch5Mod1.SelectedValue = _settings.GlobalKeysConfigSearch[4].Modifier1;
            cmbGlobalKeySearch5Mod2.SelectedValue = _settings.GlobalKeysConfigSearch[4].Modifier2;
            cmbGlobalKeySearch5Mod3.SelectedValue = _settings.GlobalKeysConfigSearch[4].Modifier3;
            cmbGlobalKeySearch5Mod4.SelectedValue = _settings.GlobalKeysConfigSearch[4].Modifier4;
            cmbGlobalKeySearch5SearchName.Text = _settings.GlobalKeysConfigSearch[4].SearchName;
            cmbGlobalKeySearch5SearchTerm.Text = _settings.GlobalKeysConfigSearch[4].SearchTerm;
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
            // Global Key support
            _settings.IsShortKeySupport = chkShortKeySupport.Checked;
            _settings.SqlPaths = txtSqlSearchPath.Text;

            #region store global services
            // Global Services via hot key
            _settings.GlobalKeysConfig[0].Key = cmbGlobalKeyService1Key.SelectedValue.ToString();
            _settings.GlobalKeysConfig[0].Modifier1 = cmbGlobalKeyService1Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfig[0].Modifier2 = cmbGlobalKeyService1Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfig[0].Modifier3 = cmbGlobalKeyService1Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfig[0].Modifier4 = cmbGlobalKeyService1Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfig[0].Id = cmbGlobalKey1Service.SelectedValue.ToString();
            _settings.GlobalKeysConfig[0].Description = cmbGlobalKey1Tooltip.Text;

            _settings.GlobalKeysConfig[1].Key = cmbGlobalKeyService2Key.SelectedValue.ToString();
            _settings.GlobalKeysConfig[1].Modifier1 = cmbGlobalKeyService2Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfig[1].Modifier2 = cmbGlobalKeyService2Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfig[1].Modifier3 = cmbGlobalKeyService2Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfig[1].Modifier4 = cmbGlobalKeyService2Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfig[1].Id = cmbGlobalKey2Service.SelectedValue.ToString();
            _settings.GlobalKeysConfig[1].Description = cmbGlobalKey2Tooltip.Text;

            _settings.GlobalKeysConfig[2].Key = cmbGlobalKeyService3Key.SelectedValue.ToString();
            _settings.GlobalKeysConfig[2].Modifier1 = cmbGlobalKeyService3Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfig[2].Modifier2 = cmbGlobalKeyService3Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfig[2].Modifier3 = cmbGlobalKeyService3Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfig[2].Modifier4 = cmbGlobalKeyService3Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfig[2].Id = cmbGlobalKey3Service.SelectedValue.ToString();
            _settings.GlobalKeysConfig[2].Description = cmbGlobalKey3Tooltip.Text;

            _settings.GlobalKeysConfig[3].Key = cmbGlobalKeyService4Key.SelectedValue.ToString();
            _settings.GlobalKeysConfig[3].Modifier1 = cmbGlobalKeyService4Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfig[3].Modifier2 = cmbGlobalKeyService4Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfig[3].Modifier3 = cmbGlobalKeyService4Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfig[3].Modifier4 = cmbGlobalKeyService4Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfig[3].Id = cmbGlobalKey4Service.SelectedValue.ToString();
            _settings.GlobalKeysConfig[3].Description = cmbGlobalKey4Tooltip.Text;

            _settings.GlobalKeysConfig[4].Key = cmbGlobalKeyService5Key.SelectedValue.ToString();
            _settings.GlobalKeysConfig[4].Modifier1 = cmbGlobalKeyService5Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfig[4].Modifier2 = cmbGlobalKeyService5Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfig[4].Modifier3 = cmbGlobalKeyService5Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfig[4].Modifier4 = cmbGlobalKeyService5Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfig[4].Id = cmbGlobalKey5Service.SelectedValue.ToString();
            _settings.GlobalKeysConfig[4].Description = cmbGlobalKey5Tooltip.Text;
            #endregion

            #region store global searches
            // Global Searches via hot key
            _settings.GlobalKeysConfigSearch[0].Key = cmbGlobalKeySearch1Key.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[0].Modifier1 = cmbGlobalKeySearch1Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[0].Modifier2 = cmbGlobalKeySearch1Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[0].Modifier3 = cmbGlobalKeySearch1Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[0].Modifier4 = cmbGlobalKeySearch1Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[0].SearchName = cmbGlobalKeySearch1SearchName.Text;
            _settings.GlobalKeysConfigSearch[0].SearchTerm = cmbGlobalKeySearch1SearchTerm.Text;

            _settings.GlobalKeysConfigSearch[1].Key = cmbGlobalKeySearch2Key.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[1].Modifier1 = cmbGlobalKeySearch2Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[1].Modifier2 = cmbGlobalKeySearch2Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[1].Modifier3 = cmbGlobalKeySearch2Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[1].Modifier4 = cmbGlobalKeySearch2Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[1].SearchName = cmbGlobalKeySearch2SearchName.Text;
            _settings.GlobalKeysConfigSearch[1].SearchTerm = cmbGlobalKeySearch2SearchTerm.Text;

            _settings.GlobalKeysConfigSearch[2].Key = cmbGlobalKeySearch3Key.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[2].Modifier1 = cmbGlobalKeySearch3Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[2].Modifier2 = cmbGlobalKeySearch3Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[2].Modifier3 = cmbGlobalKeySearch3Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[2].Modifier4 = cmbGlobalKeySearch3Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[2].SearchName = cmbGlobalKeySearch3SearchName.Text;
            _settings.GlobalKeysConfigSearch[2].SearchTerm = cmbGlobalKeySearch3SearchTerm.Text;

            _settings.GlobalKeysConfigSearch[3].Key = cmbGlobalKeySearch4Key.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[3].Modifier1 = cmbGlobalKeySearch4Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[3].Modifier2 = cmbGlobalKeySearch4Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[3].Modifier3 = cmbGlobalKeySearch4Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[3].Modifier4 = cmbGlobalKeySearch4Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[3].SearchName = cmbGlobalKeySearch4SearchName.Text;
            _settings.GlobalKeysConfigSearch[3].SearchTerm = cmbGlobalKeySearch4SearchTerm.Text;

            _settings.GlobalKeysConfigSearch[4].Key = cmbGlobalKeySearch5Key.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[4].Modifier1 = cmbGlobalKeySearch5Mod1.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[4].Modifier2 = cmbGlobalKeySearch5Mod2.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[4].Modifier3 = cmbGlobalKeySearch5Mod3.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[4].Modifier4 = cmbGlobalKeySearch5Mod4.SelectedValue.ToString();
            _settings.GlobalKeysConfigSearch[4].SearchName = cmbGlobalKeySearch5SearchName.Text;
            _settings.GlobalKeysConfigSearch[4].SearchTerm = cmbGlobalKeySearch5SearchTerm.Text;
            #endregion

            _addinControl.ParameterizeMenusAndButtons(); // hide / unhide Menus & Buttons
            _addinControl.ParameterizeSearchButton(); // sets the shortcuts
            _addinControl.ParameterizeServiceButton(); // sets the shortcuts

            _settings.UpdateServices(); // update dynamic informations like method, texts from configuration
            _settings.Save();
            Close();

        }
        #endregion

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// The Service index has changed. Update the tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGlobalKeyService_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get selected value
            ComboBox cmbBox = (ComboBox)sender;
            // get tooltip for selected index
            int index = cmbBox.SelectedIndex;
            if (index == -1) return;
            string tooltip = _settings.AllServices[index].Help;
            if (sender == cmbGlobalKey1Service)
                cmbGlobalKey1Tooltip.Text = tooltip;
            if (sender == cmbGlobalKey2Service)
                cmbGlobalKey2Tooltip.Text = tooltip;
            if (sender == cmbGlobalKey3Service)
                cmbGlobalKey3Tooltip.Text = tooltip;
            if (sender == cmbGlobalKey4Service)
                cmbGlobalKey4Tooltip.Text = tooltip;
            if (sender == cmbGlobalKey5Service)
                cmbGlobalKey5Tooltip.Text = tooltip;
        }
    }
}
