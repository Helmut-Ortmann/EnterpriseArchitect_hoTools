using System;
using System.Windows.Forms;
using hoTools.ActiveX;

namespace hoTools.Settings

{
    /// <summary>
    /// frmSettings  Settings of hoTools
    /// Reads from configuration, displays the content and write to configuration.
    /// </summary>
    public partial class FrmSettingsLineStyle : Form
    {
        readonly AddinSettings _settings;
        AddinControlGui _addinControl;

        #region Constructor
        /// <summary>
        /// Constructor with
        /// </summary>
        /// <param name="settings">Object with settings</param>
        /// <param name="addinControl">Object with Control</param>
        public FrmSettingsLineStyle(AddinSettings settings, AddinControlGui addinControl)
        {
            InitializeComponent();

            _settings = settings;
            _addinControl = addinControl;

          
            #region line style
            // line style
            var items = new[]{"A Automatic","C Custom","D Direct","B Bezier curve",
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



            cboActivityLineStyle.DataSource = itemsActivity;
            cboStatechartLineStyle.DataSource = itemsStatechart;
            cboClassLineStyle.DataSource = itemsClass; 
            cboComponentLineStyle.DataSource = itemsComponent;
            cboPackageLineStyle.DataSource = itemsPackage; 
            cboCustomLineStyle.DataSource = itemsCustom; 
            cboUseCaseLineStyle.DataSource = itemsUseCase; 
            cboDeploymentLineStyle.DataSource = itemsDeployment;
            cboCompositeStructureLineStyle.DataSource = itemsCompositeStructure; 

            cboActivityLineStyle.Text = settings.ActivityLineStyle;
            cboStatechartLineStyle.Text = settings.StatechartLineStyle;
            cboClassLineStyle.Text = settings.ClassLineStyle;
            cboComponentLineStyle.Text = settings.ComponentLineStyle;
            cboPackageLineStyle.Text = settings.PackageLineStyle;
            cboCustomLineStyle.Text = settings.CustomLineStyle;
            cboUseCaseLineStyle.Text = settings.UseCaseLineStyle;
            cboDeploymentLineStyle.Text = settings.DeploymentLineStyle;
            cboCompositeStructureLineStyle.Text = settings.CompositeStructureLineStyle;
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
            _settings.ActivityLineStyle = cboActivityLineStyle.Text;
            _settings.StatechartLineStyle = cboStatechartLineStyle.Text;
            _settings.ClassLineStyle = cboClassLineStyle.Text;
            _settings.ComponentLineStyle = cboComponentLineStyle.Text;
            _settings.PackageLineStyle = cboPackageLineStyle.Text;
            _settings.CustomLineStyle = cboCustomLineStyle.Text;
            _settings.UseCaseLineStyle = cboUseCaseLineStyle.Text;
            _settings.DeploymentLineStyle = cboDeploymentLineStyle.Text;
            _settings.CompositeStructureLineStyle = cboCompositeStructureLineStyle.Text;
            #endregion
           
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
