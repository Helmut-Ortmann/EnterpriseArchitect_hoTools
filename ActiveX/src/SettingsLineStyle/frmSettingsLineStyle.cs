using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Control.EaAddinShortcuts;
using hoTools.ActiveX;
using GlobalHotkeys;

namespace hoTools.Settings

{
    /// <summary>
    /// frmSettings  Settings of hoTools
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsLineStyle : Form
    {   
        AddinSettings _settings;
        AddinControlGUI _addinControl;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="addinControl">Object with Control</param>
        public FrmSettingsLineStyle(AddinSettings settings, AddinControlGUI addinControl)
        {
            InitializeComponent();

            _settings = settings;
            _addinControl = addinControl;

          
            #region line style
            // line style
            var items = new string[]{"A Automatic","C Custom","D Direct","B Bezier curve",
                "LV Lateral Vertical","LH Lateral Horizontal","no","OR Orthogonal Rounded",
                "OS Orthogonal Square","TH Tree Horizontal","TV Tree Vertical"};
            string[] itemsActivity = new string[items.Length];
            items.CopyTo(itemsActivity, 0);

            string[] itemsStatechart = new string[items.Length];
            items.CopyTo(itemsStatechart, 0);

            string[] itemsCustom = new string[items.Length];
            items.CopyTo(itemsCustom, 0);

            string[] itemsClass = new string[items.Length];
            items.CopyTo(itemsClass, 0);

            string[] itemsComponent = new string[items.Length];
            items.CopyTo(itemsComponent, 0);

            string[] itemsUseCase = new string[items.Length];
            items.CopyTo(itemsUseCase, 0);

            string[] itemsPackage = new string[items.Length];
            items.CopyTo(itemsPackage, 0);

            string[] itemsDeployment = new string[items.Length];
            items.CopyTo(itemsDeployment, 0);

            string[] itemsCompositeStructure = new string[items.Length];
            items.CopyTo(itemsCompositeStructure, 0);



            this.cboActivityLineStyle.DataSource = itemsActivity;
            this.cboStatechartLineStyle.DataSource = itemsStatechart;
            this.cboClassLineStyle.DataSource = itemsClass; 
            this.cboComponentLineStyle.DataSource = itemsComponent;
            this.cboPackageLineStyle.DataSource = itemsPackage; 
            this.cboCustomLineStyle.DataSource = itemsCustom; 
            this.cboUseCaseLineStyle.DataSource = itemsUseCase; 
            this.cboDeploymentLineStyle.DataSource = itemsDeployment;
            this.cboCompositeStructureLineStyle.DataSource = itemsCompositeStructure; 

            this.cboActivityLineStyle.Text = settings.ActivityLineStyle;
            this.cboStatechartLineStyle.Text = settings.StatechartLineStyle;
            this.cboClassLineStyle.Text = settings.ClassLineStyle;
            this.cboComponentLineStyle.Text = settings.ComponentLineStyle;
            this.cboPackageLineStyle.Text = settings.PackageLineStyle;
            this.cboCustomLineStyle.Text = settings.CustomLineStyle;
            this.cboUseCaseLineStyle.Text = settings.UseCaseLineStyle;
            this.cboDeploymentLineStyle.Text = settings.DeploymentLineStyle;
            this.cboCompositeStructureLineStyle.Text = settings.CompositeStructureLineStyle;
            #endregion

            
            // Search
           


        }
        #endregion

        #region StoreAll ButtonOkClick()
        /// <summary>
        /// Store the settings, ok button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {

            #region Line style
            _settings.ActivityLineStyle = this.cboActivityLineStyle.Text;
            _settings.StatechartLineStyle = this.cboStatechartLineStyle.Text;
            _settings.ClassLineStyle = this.cboClassLineStyle.Text;
            _settings.ComponentLineStyle = this.cboComponentLineStyle.Text;
            _settings.PackageLineStyle = this.cboPackageLineStyle.Text;
            _settings.CustomLineStyle = this.cboCustomLineStyle.Text;
            _settings.UseCaseLineStyle = this.cboUseCaseLineStyle.Text;
            _settings.DeploymentLineStyle = this.cboDeploymentLineStyle.Text;
            _settings.CompositeStructureLineStyle = this.cboCompositeStructureLineStyle.Text;
            #endregion
           
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
