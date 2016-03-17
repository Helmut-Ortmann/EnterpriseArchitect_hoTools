using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.Settings;
using hoTools.EaServices;
using hoTools.EAServicesPort;
using Control.EaAddinShortcuts;


namespace hoTools.ActiveX
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("82A06E9C-7568-4E4B-8D2C-A53B8D9A7272")]
    [ProgId("hoTools.ActiveXGUI")]
    [ComDefaultInterface(typeof(IAddinControl))]

    public class AddinControlGUI : AddinGUI, IAddinControl
    {
        public const string PROGID = "hoTools.ActiveXGUI";
        public const string TABULATOR = "Script";

        private EA.Repository _repository = null;


        #region Generated
        private frmSettings frmSettings;
        private Button btnLH;
        private Button btnLV;
        private Button btnTV;
        public Button btnTH;
        private Button btnOS;
        private ToolTip toolTip;
        private System.ComponentModel.IContainer components;
        private Button btnDisplayBehavior;
        private Button btnLocateOperation;
        private Button btnAddElementNote;
        private Button btnAddDiagramNote;
        private Button btnLocateType;
        private Button btnFindUsage;
        private Button btnDisplaySpecification;
        private Button btnComposite;
        private Button btnOR;
        private Button btnA;
        private Button btnD;
        private Button btnC;
        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem doToolStripMenuItem;
        private ToolStripMenuItem createActivityForOperationToolStripMenuItem;
        private ToolStripMenuItem showFolderToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem copyGUIDSQLToClipboardToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Button btnUpdateActivityParameter;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Button btnBezier;
        private ToolStripMenuItem versionControlToolStripMenuItem;
        private ToolStripMenuItem changeXMLFileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem1;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripBtn11;
        private ToolStripButton toolStripBtn12;
        private ToolStripButton toolStripBtn13;
        private ToolStripButton toolStripBtn14;
        private ToolStripButton toolStripBtn15;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripBtn1;
        private ToolStripButton toolStripBtn2;
        private ToolStripButton toolStripBtn3;
        private ToolStripButton toolStripBtn4;
        private ToolStripButton toolStripBtn5;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem changeAuthorToolStripMenuItem;
        private ToolStripMenuItem changeAuthorRecursiveToolStripMenuItem;
        private ToolStripMenuItem getVCLatesrecursiveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem showTortoiseLogToolStripMenuItem;
        private ToolStripMenuItem showTortoiseRepoBrowserToolStripMenuItem;
        private ToolStripMenuItem setSvnKeywordsToolStripMenuItem;
        private ToolStripMenuItem setSvnModuleTaggedValuesToolStripMenuItem;
        private ToolStripMenuItem setSvnModuleTaggedValuesrecursiveToolStripMenuItem;
        private ToolStripMenuItem showFolderVCorCodeToolStripMenuItem;
        private Button btnAddFavorite;
        private Button btnRemoveFavorite;
        private Button btnFavorites;
        private ToolStripMenuItem portToolStripMenuItem;
        private ToolStripMenuItem showPortsInDiagramObjectsToolStripMenuItem;
        private ToolStripMenuItem movePortsToolStripMenuItem;
        private ToolStripMenuItem hidePortsToolStripMenuItem;
        private ToolStripMenuItem unhidePortsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem connectPortsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem deletePortsToolStripMenuItem;
        private Button btnRight;
        private Button btnLeft;
        private Button btnUp;
        private Button btnDown;
        private ToolStripMenuItem hidePortsInDiagramToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem makeConnectorsUnspecifiedDirectionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem connectPortsInsideComponentsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem showSendingPortsLeftRecievingPortsRightToolStripMenuItem;
        private ToolStripMenuItem movePortLabelLeftToolStripMenuItem;
        private ToolStripMenuItem movePortLabelRightPositionToolStripMenuItem;
        private ToolStripMenuItem movePortLabelLeftToolStripMenuItem1;
        private ToolStripMenuItem movePortLabelRightToolStripMenuItem;
        private Button btnLabelLeft;
        private Button btnLabelRight;
        private ToolStripMenuItem orderDiagramItemsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripMenuItem openScriptAndQueryToolStripMenuItem;
        private TextBox txtUserText;
        #endregion

        #region Constructor
        public AddinControlGUI()
        {
            InitializeComponent();

        }
        #endregion
       
        public string getText() => txtUserText.Text;

        #region IActiveX Members
        public string getName() => "hoTools.AddinControl";



        //public int ShowDialog()
        //{
        //    MessageBox.Show("MessageBox text");
        //    return 0;
        //}

        #endregion

        #region save
        public void Save()
        {
            return;
        }
        #endregion
        #region User Actions
        #region Button & Menue
        void btnDisplayBehavior_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(_repository, EaService.displayMode.Behavior);
        }
        void btnLocateOperation_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(_repository, EaService.displayMode.Method);
        }

        void btnAddElementNote_Click(object sender, EventArgs e)
        {
            EaService.addElementNote(_repository);
        }

        void btnAddDiagramNote_Click(object sender, EventArgs e)
        {
            EaService.addDiagramNote(_repository);
        }

        void btnLocateType_Click(object sender, EventArgs e)
        {
            EaService.locateType(_repository);
        }


        void btnShowSpecification_Click(object sender, EventArgs e)
        {
            EaService.showSpecification(_repository);
        }


        void btnFindUsage_Click(object sender, EventArgs e)
        {
            EaService.findUsage(_repository);
        }

        void btnC_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "C");
        }


        void btnD_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "D");
        }



        void btnA_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "A");
        }


        void btnOR_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "OR");
        }

        private void btnComposite_Click(object sender, EventArgs e)
        {
            EaService.navigateComposite(_repository);
        }

        private void label1_ControlRemoved(object sender, ControlEventArgs e)
        {
            // think about informing Addin of this event
        }

        private void getAllLatestrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.getVcLatestRecursive(_repository);
        }

        private void createActivityForOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.CreateActivityForOperation(_repository);
        }

        private void showFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowFolder(_repository, isTotalCommander: false);
        }

        private void copyGUIDSQLToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.copyGuidSqlToClipboard(_repository);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.frmSettings = new frmSettings(AddinSettings, this);
            this.frmSettings.ShowDialog();
        }

        private void btnUpdateActivityParametzer_Click(object sender, EventArgs e)
        {
            EaService.UpdateActivityParameter(_repository);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string configFilePath = AddinSettings.ConfigFilePath;
            switch (AddinSettings.Customer)
            {
                
                case AddinSettings.CustomerCfg.VAR1:
                    EaService.aboutVAR1(Release, configFilePath);
                    break;
                case AddinSettings.CustomerCfg.hoTools:
                    EaService.about(Release, configFilePath);
                    break;
                default:
                    EaService.about(Release, configFilePath);
                    break;
            }

        }

        private void changeXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.setNewXmlPath(_repository);
        }

        private void btnBezier_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "B");
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, EaService.getAssemblyPath() + "\\" + "hoTools.chm");
        }



        private void toolStripBtn1_Click(object sender, EventArgs e)
        {
            runService(0);
        }

        private void toolStripBtn2_Click(object sender, EventArgs e)
        {
            runService(1);
        }
        private void toolStripBtn3_Click(object sender, EventArgs e)
        {
            runService(2);
        }

        private void toolStripBtn4_Click(object sender, EventArgs e)
        {
            runService(3);
        }

        private void toolStripBtn5_Click(object sender, EventArgs e)
        {
            runService(4);
        }

        private void toolStripBtn11_Click(object sender, EventArgs e)
        {
            runSearch(0);
        }

        private void toolStripBtn12_Click(object sender, EventArgs e)
        {
            runSearch(1);
        }

        private void toolStripBtn13_Click(object sender, EventArgs e)
        {
            runSearch(2);
        }

        private void toolStripBtn14_Click(object sender, EventArgs e)
        {
            runSearch(3);
        }

        private void toolStripBtn15_Click(object sender, EventArgs e)
        {
            runSearch(4);
        }
        private void runSearch(int pos)
        {
            if (AddinSettings.buttonsSearch[pos] is EaAddinShortcutSearch)
            {

                var sh = (EaAddinShortcutSearch)AddinSettings.buttonsSearch[pos];
                if (sh.keySearchName == "") return;
                try
                {
                    _repository.RunModelSearch(sh.keySearchName, sh.keySearchTerm, "", "");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error start search '" + sh.keySearchName +
                       " " + sh.keySearchTerm + "'");
                }
            }
        }
        private void runService(int pos)
        {
            if (AddinSettings.buttonsServices[pos] is hoTools.EaServices.ServicesCallConfig)
            {

                var sh = (hoTools.EaServices.ServicesCallConfig)AddinSettings.buttonsServices[pos];
                if (sh.Method == null) return;
                sh.Invoke(_repository, txtUserText.Text);

            }
        }

        private void changeAuthorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.changeAuthor(_repository);
        }

        private void changeAuthorRecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.changeUserRecursive(_repository);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showFolderVCorCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowFolder(_repository, isTotalCommander: false);
        }

        private void showTortoiseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = _repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)_repository.GetContextObject();
            EaService.gotoSvnLog(_repository, pkg);
        }

        private void showTortoiseRepoBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = _repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)_repository.GetContextObject();
            EaService.gotoSvnBrowser(_repository, pkg);
        }

        private void getVCLatesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.getVcLatestRecursive(_repository);
        }

        private void setSvnKeywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = _repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)_repository.GetContextObject();
            EaService.setSvnProperty(_repository, pkg);
        }

        private void setSvnModuleTaggedValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = _repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)_repository.GetContextObject();
            EaService.setDirectoryTaggedValues(_repository, pkg);
        }

        private void setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.setTaggedValueGui(_repository);
        }

        private void btnAddFavorite_Click(object sender, EventArgs e)
        {
            EaService.AddFavorite(_repository);
        }

        private void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            EaService.RemoveFavorite(_repository);
        }

        private void btnFavorites_Click(object sender, EventArgs e)
        {
            EaService.Favorites(_repository);
        }

        
        /// <summary>
        /// Remove ports from elements by:
        /// - Selected Elements
        /// - Selected Ports
        /// 
        /// Note: Selection isn't restored because selected objects might be deletet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region removePortsInDiagramToolStripMenuItem_Click
        private void removePortsInDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.removePortFromDiagramGUI();
           
        }
        #endregion

        #region showPortsInDiagramObjects
        private void showPortsInDiagramObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.showPortsInDiagram(false);

           
        }
        #endregion
        #region showReceivingPortsLeftSendingPortsRight
        private void showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.showPortsInDiagram(true);
        }
        #endregion

        #region copyPorts
        private void copyPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.copyPortsGUI();
           
        }
        #endregion
        #region deletePortsWhichAreMarkedForDeletion
        private void deletePortsWhichAreMarkedForDeletionfutureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.deletePortsMarkedPorts();
        }
        #endregion

        /// <summary>
        /// Hide labels from elements by:
        /// - Selected Elements
        /// - Selected Ports
        /// 
        /// Note: Selection isn't restored because selected objects might be deletet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region hidePortLabelToolStripMenuItem_Click
        private void hidePortLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.changeLabelGUI(PortServices.LabelStyle.IS_HIDDEN);
        }
        #endregion

        #region viewPortLabelToolStripMenuItem_Click
        private void viewPortLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.changeLabelGUI(PortServices.LabelStyle.IS_SHOWN);
       }
        #endregion
        #region movePortLableLeftPositionToolStripMenuItem_Click
        private void movePortLableLeftPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_LEFT);
        }
        #endregion

        #region movePortLableRightPositionToolStripMenuItem_Click
        private void movePortLableRightPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_RIGHT);
        }
        #endregion


        #region movePortLablePlusPositionToolStripMenuItem_Click
        private void movePortLablePlusPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_PLUS);
        }
        #endregion


        #region movePortLableMinusPositionToolStripMenuItem_Click
        private void movePortLableMinusPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_MINUS);
        }
        #endregion


        private void connectPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.connectPortsGUI();
            
        }

        private void connectPortsInsideComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.connectPortsInsideGUI();
        }

        private void deletePortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.deletePortsGUI();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedLeftGUI(_repository);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedRightGUI(_repository);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedUpGUI(_repository);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedDownGUI(_repository);
        }

        private void makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(_repository);
            port.setConnectionDirectionUnspecifiedGUI();
        }
       
        
       

       


        //---------------------------------------------------------------------------------------------------------------
        // linestyle
        // LH = "Line Style: Lateral Horizontal";
        // LV = "Line Style: Lateral Vertical";
        // TH = "Line Style: Tree Horizontal";
        // TV = "Line Style: Tree Vertical";
        // OS = "Line Style: Orthogonal Square";
        void btnLH_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "LH");
        }
        void btnLV_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "LV");
        }
        void btnTH_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "LH");
        }
        void btnTV_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "TV");
        }
        void btnOS_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "OS");
        }
        #endregion
        #region Keydown
        public class EnterTextBox : TextBox
        {
            protected override bool IsInputKey(Keys keyData)
            {
                if (keyData == Keys.Return)
                    return true;
                return base.IsInputKey(keyData);
            }

        }
        // text field
        // There are special keys like "Enter" which require an enabling by 
        //---------------------------------------------------------
        // see at:  protected override bool IsInputKey(Keys keyData)
        void txtUserText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EaService.runQuickSearch(_repository, AddinSettings.quickSearchName, txtUserText.Text);
                e.Handled = true;
            }
        }
        #endregion
        #region Mouse
        private void txtUserText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtUserText.Text = Clipboard.GetText();
            EaService.runQuickSearch(_repository, AddinSettings.quickSearchName, txtUserText.Text);
        }
        #endregion
        #endregion

        #region InitializeComponent
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLH = new System.Windows.Forms.Button();
            this.btnLV = new System.Windows.Forms.Button();
            this.btnTV = new System.Windows.Forms.Button();
            this.btnTH = new System.Windows.Forms.Button();
            this.btnOS = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnDisplayBehavior = new System.Windows.Forms.Button();
            this.btnLocateOperation = new System.Windows.Forms.Button();
            this.btnAddElementNote = new System.Windows.Forms.Button();
            this.btnAddDiagramNote = new System.Windows.Forms.Button();
            this.btnLocateType = new System.Windows.Forms.Button();
            this.btnFindUsage = new System.Windows.Forms.Button();
            this.btnDisplaySpecification = new System.Windows.Forms.Button();
            this.btnComposite = new System.Windows.Forms.Button();
            this.btnOR = new System.Windows.Forms.Button();
            this.btnA = new System.Windows.Forms.Button();
            this.btnD = new System.Windows.Forms.Button();
            this.btnC = new System.Windows.Forms.Button();
            this.btnUpdateActivityParameter = new System.Windows.Forms.Button();
            this.btnBezier = new System.Windows.Forms.Button();
            this.txtUserText = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtn11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn14 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn15 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtn1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn5 = new System.Windows.Forms.ToolStripButton();
            this.btnAddFavorite = new System.Windows.Forms.Button();
            this.btnRemoveFavorite = new System.Windows.Forms.Button();
            this.btnFavorites = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnLabelLeft = new System.Windows.Forms.Button();
            this.btnLabelRight = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openScriptAndQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createActivityForOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyGUIDSQLToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.changeAuthorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeAuthorRecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeXMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFolderVCorCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getVCLatesrecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showTortoiseLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTortoiseRepoBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSvnKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSvnModuleTaggedValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.deletePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.connectPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectPortsInsideComponentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.showPortsInDiagramObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hidePortsInDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.unhidePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hidePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.movePortLabelLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.movePortLabelRightPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.movePortLabelLeftToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.movePortLabelRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.orderDiagramItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLH
            // 
            this.btnLH.Location = new System.Drawing.Point(49, 80);
            this.btnLH.Name = "btnLH";
            this.btnLH.Size = new System.Drawing.Size(43, 23);
            this.btnLH.TabIndex = 0;
            this.btnLH.Text = "LH";
            this.toolTip.SetToolTip(this.btnLH, "Lateral Horizontal");
            this.btnLH.UseVisualStyleBackColor = true;
            this.btnLH.Click += new System.EventHandler(this.btnLH_Click);
            // 
            // btnLV
            // 
            this.btnLV.Location = new System.Drawing.Point(-2, 80);
            this.btnLV.Name = "btnLV";
            this.btnLV.Size = new System.Drawing.Size(45, 23);
            this.btnLV.TabIndex = 2;
            this.btnLV.Text = "LV";
            this.toolTip.SetToolTip(this.btnLV, "Lateral Vertical");
            this.btnLV.UseVisualStyleBackColor = true;
            this.btnLV.Click += new System.EventHandler(this.btnLV_Click);
            // 
            // btnTV
            // 
            this.btnTV.Location = new System.Drawing.Point(98, 80);
            this.btnTV.Name = "btnTV";
            this.btnTV.Size = new System.Drawing.Size(35, 23);
            this.btnTV.TabIndex = 4;
            this.btnTV.Text = "TV";
            this.toolTip.SetToolTip(this.btnTV, "Tree Vertical");
            this.btnTV.UseVisualStyleBackColor = true;
            this.btnTV.Click += new System.EventHandler(this.btnTV_Click);
            // 
            // btnTH
            // 
            this.btnTH.Location = new System.Drawing.Point(142, 80);
            this.btnTH.Name = "btnTH";
            this.btnTH.Size = new System.Drawing.Size(41, 23);
            this.btnTH.TabIndex = 3;
            this.btnTH.Text = "TH";
            this.toolTip.SetToolTip(this.btnTH, "Tree Horizontal");
            this.btnTH.UseVisualStyleBackColor = true;
            this.btnTH.Click += new System.EventHandler(this.btnTH_Click);
            // 
            // btnOS
            // 
            this.btnOS.Location = new System.Drawing.Point(51, 109);
            this.btnOS.Name = "btnOS";
            this.btnOS.Size = new System.Drawing.Size(38, 24);
            this.btnOS.TabIndex = 5;
            this.btnOS.Text = "OS";
            this.toolTip.SetToolTip(this.btnOS, "Orthogonal Square");
            this.btnOS.UseVisualStyleBackColor = true;
            this.btnOS.Click += new System.EventHandler(this.btnOS_Click);
            // 
            // btnDisplayBehavior
            // 
            this.btnDisplayBehavior.Location = new System.Drawing.Point(108, 136);
            this.btnDisplayBehavior.Name = "btnDisplayBehavior";
            this.btnDisplayBehavior.Size = new System.Drawing.Size(119, 25);
            this.btnDisplayBehavior.TabIndex = 7;
            this.btnDisplayBehavior.Text = "DisplayBehavior";
            this.toolTip.SetToolTip(this.btnDisplayBehavior, "Display behavior of an operation (activity, statemachine, interaction)");
            this.btnDisplayBehavior.UseVisualStyleBackColor = true;
            this.btnDisplayBehavior.Click += new System.EventHandler(this.btnDisplayBehavior_Click);
            // 
            // btnLocateOperation
            // 
            this.btnLocateOperation.Location = new System.Drawing.Point(-1, 197);
            this.btnLocateOperation.Name = "btnLocateOperation";
            this.btnLocateOperation.Size = new System.Drawing.Size(130, 25);
            this.btnLocateOperation.TabIndex = 8;
            this.btnLocateOperation.Text = "Locate Operation";
            this.toolTip.SetToolTip(this.btnLocateOperation, "Locate the operation for an action or behavior (statechart, activity, interaction" +
        ")");
            this.btnLocateOperation.UseVisualStyleBackColor = true;
            this.btnLocateOperation.Click += new System.EventHandler(this.btnLocateOperation_Click);
            // 
            // btnAddElementNote
            // 
            this.btnAddElementNote.Location = new System.Drawing.Point(-2, 257);
            this.btnAddElementNote.Name = "btnAddElementNote";
            this.btnAddElementNote.Size = new System.Drawing.Size(130, 25);
            this.btnAddElementNote.TabIndex = 9;
            this.btnAddElementNote.Text = "Add Element Note";
            this.toolTip.SetToolTip(this.btnAddElementNote, "Add an element note to an Element in a diagram");
            this.btnAddElementNote.UseVisualStyleBackColor = true;
            this.btnAddElementNote.Click += new System.EventHandler(this.btnAddElementNote_Click);
            // 
            // btnAddDiagramNote
            // 
            this.btnAddDiagramNote.Location = new System.Drawing.Point(138, 257);
            this.btnAddDiagramNote.Name = "btnAddDiagramNote";
            this.btnAddDiagramNote.Size = new System.Drawing.Size(158, 25);
            this.btnAddDiagramNote.TabIndex = 10;
            this.btnAddDiagramNote.Text = "Add Diagram Note";
            this.toolTip.SetToolTip(this.btnAddDiagramNote, "Add a diagram note to the diagram (connected to diagram note)");
            this.btnAddDiagramNote.UseVisualStyleBackColor = true;
            this.btnAddDiagramNote.Click += new System.EventHandler(this.btnAddDiagramNote_Click);
            // 
            // btnLocateType
            // 
            this.btnLocateType.Location = new System.Drawing.Point(-2, 226);
            this.btnLocateType.Name = "btnLocateType";
            this.btnLocateType.Size = new System.Drawing.Size(131, 25);
            this.btnLocateType.TabIndex = 11;
            this.btnLocateType.Text = "Locate Type";
            this.toolTip.SetToolTip(this.btnLocateType, "Locate to the type, trigger,signal...  of the selected element/connector");
            this.btnLocateType.UseVisualStyleBackColor = true;
            this.btnLocateType.Click += new System.EventHandler(this.btnLocateType_Click);
            // 
            // btnFindUsage
            // 
            this.btnFindUsage.Location = new System.Drawing.Point(138, 197);
            this.btnFindUsage.Name = "btnFindUsage";
            this.btnFindUsage.Size = new System.Drawing.Size(158, 25);
            this.btnFindUsage.TabIndex = 12;
            this.btnFindUsage.Text = "Find Usage";
            this.toolTip.SetToolTip(this.btnFindUsage, "Find the usage of the selected element");
            this.btnFindUsage.UseVisualStyleBackColor = true;
            this.btnFindUsage.Click += new System.EventHandler(this.btnFindUsage_Click);
            // 
            // btnDisplaySpecification
            // 
            this.btnDisplaySpecification.Location = new System.Drawing.Point(-1, 166);
            this.btnDisplaySpecification.Name = "btnDisplaySpecification";
            this.btnDisplaySpecification.Size = new System.Drawing.Size(130, 25);
            this.btnDisplaySpecification.TabIndex = 13;
            this.btnDisplaySpecification.Text = "Specification";
            this.toolTip.SetToolTip(this.btnDisplaySpecification, "Display the Specification according to file property");
            this.btnDisplaySpecification.UseVisualStyleBackColor = true;
            this.btnDisplaySpecification.Click += new System.EventHandler(this.btnShowSpecification_Click);
            // 
            // btnComposite
            // 
            this.btnComposite.Location = new System.Drawing.Point(138, 226);
            this.btnComposite.Name = "btnComposite";
            this.btnComposite.Size = new System.Drawing.Size(158, 25);
            this.btnComposite.TabIndex = 16;
            this.btnComposite.Text = "Composite";
            this.toolTip.SetToolTip(this.btnComposite, "Navigate between Element and Composite Diagram");
            this.btnComposite.UseVisualStyleBackColor = true;
            this.btnComposite.Click += new System.EventHandler(this.btnComposite_Click);
            // 
            // btnOR
            // 
            this.btnOR.Location = new System.Drawing.Point(95, 109);
            this.btnOR.Name = "btnOR";
            this.btnOR.Size = new System.Drawing.Size(38, 24);
            this.btnOR.TabIndex = 17;
            this.btnOR.Text = "OR";
            this.toolTip.SetToolTip(this.btnOR, "Orthogonal Rounded");
            this.btnOR.UseVisualStyleBackColor = true;
            this.btnOR.Click += new System.EventHandler(this.btnOR_Click);
            // 
            // btnA
            // 
            this.btnA.Location = new System.Drawing.Point(139, 109);
            this.btnA.Name = "btnA";
            this.btnA.Size = new System.Drawing.Size(44, 23);
            this.btnA.TabIndex = 18;
            this.btnA.Text = "A";
            this.toolTip.SetToolTip(this.btnA, "Orthogonal Rounded");
            this.btnA.UseVisualStyleBackColor = true;
            this.btnA.Click += new System.EventHandler(this.btnA_Click);
            // 
            // btnD
            // 
            this.btnD.Location = new System.Drawing.Point(189, 109);
            this.btnD.Name = "btnD";
            this.btnD.Size = new System.Drawing.Size(38, 23);
            this.btnD.TabIndex = 19;
            this.btnD.Text = "D";
            this.toolTip.SetToolTip(this.btnD, "Direct");
            this.btnD.UseVisualStyleBackColor = true;
            this.btnD.Click += new System.EventHandler(this.btnD_Click);
            // 
            // btnC
            // 
            this.btnC.Location = new System.Drawing.Point(189, 80);
            this.btnC.Name = "btnC";
            this.btnC.Size = new System.Drawing.Size(38, 23);
            this.btnC.TabIndex = 20;
            this.btnC.Text = "C";
            this.toolTip.SetToolTip(this.btnC, "Custom line");
            this.btnC.UseVisualStyleBackColor = true;
            this.btnC.Click += new System.EventHandler(this.btnC_Click);
            // 
            // btnUpdateActivityParameter
            // 
            this.btnUpdateActivityParameter.Location = new System.Drawing.Point(138, 166);
            this.btnUpdateActivityParameter.Name = "btnUpdateActivityParameter";
            this.btnUpdateActivityParameter.Size = new System.Drawing.Size(160, 25);
            this.btnUpdateActivityParameter.TabIndex = 23;
            this.btnUpdateActivityParameter.Text = "UpdateParameter";
            this.toolTip.SetToolTip(this.btnUpdateActivityParameter, "Update Activity Parameter from operation");
            this.btnUpdateActivityParameter.UseVisualStyleBackColor = true;
            this.btnUpdateActivityParameter.Click += new System.EventHandler(this.btnUpdateActivityParametzer_Click);
            // 
            // btnBezier
            // 
            this.btnBezier.Location = new System.Drawing.Point(-2, 109);
            this.btnBezier.Name = "btnBezier";
            this.btnBezier.Size = new System.Drawing.Size(45, 23);
            this.btnBezier.TabIndex = 25;
            this.btnBezier.Text = "B";
            this.toolTip.SetToolTip(this.btnBezier, "Bezier");
            this.btnBezier.UseVisualStyleBackColor = true;
            this.btnBezier.Click += new System.EventHandler(this.btnBezier_Click);
            // 
            // txtUserText
            // 
            this.txtUserText.Location = new System.Drawing.Point(0, 24);
            this.txtUserText.Name = "txtUserText";
            this.txtUserText.Size = new System.Drawing.Size(298, 20);
            this.txtUserText.TabIndex = 27;
            this.toolTip.SetToolTip(this.txtUserText, "Run EA Search \'Quick View\' with:\r\n- Input text + Enter\r\n- Double left Click with " +
        "insert Clipboard and start search\r\n\r\nSearch for:\r\n- Class / Component / Requirem" +
        "ent\r\n- GUID\r\n- Port");
            this.txtUserText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtUserText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtUserText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtUserText_MouseDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtn11,
            this.toolStripBtn12,
            this.toolStripBtn13,
            this.toolStripBtn14,
            this.toolStripBtn15});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(127, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolTip.SetToolTip(this.toolStrip1, "Services (configurable by settings)");
            // 
            // toolStripBtn11
            // 
            this.toolStripBtn11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn11.Name = "toolStripBtn11";
            this.toolStripBtn11.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn11.Text = "1";
            this.toolStripBtn11.Click += new System.EventHandler(this.toolStripBtn11_Click);
            // 
            // toolStripBtn12
            // 
            this.toolStripBtn12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn12.Name = "toolStripBtn12";
            this.toolStripBtn12.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn12.Text = "2";
            this.toolStripBtn12.Click += new System.EventHandler(this.toolStripBtn12_Click);
            // 
            // toolStripBtn13
            // 
            this.toolStripBtn13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn13.Name = "toolStripBtn13";
            this.toolStripBtn13.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn13.Text = "3";
            this.toolStripBtn13.Click += new System.EventHandler(this.toolStripBtn13_Click);
            // 
            // toolStripBtn14
            // 
            this.toolStripBtn14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn14.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn14.Name = "toolStripBtn14";
            this.toolStripBtn14.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn14.Text = "4";
            this.toolStripBtn14.Click += new System.EventHandler(this.toolStripBtn14_Click);
            // 
            // toolStripBtn15
            // 
            this.toolStripBtn15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn15.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn15.Name = "toolStripBtn15";
            this.toolStripBtn15.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn15.Text = "5";
            this.toolStripBtn15.Click += new System.EventHandler(this.toolStripBtn15_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtn1,
            this.toolStripBtn2,
            this.toolStripBtn3,
            this.toolStripBtn4,
            this.toolStripBtn5});
            this.toolStrip2.Location = new System.Drawing.Point(130, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(127, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolTip.SetToolTip(this.toolStrip2, "Shortcuts (configurable by settings)");
            // 
            // toolStripBtn1
            // 
            this.toolStripBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn1.Name = "toolStripBtn1";
            this.toolStripBtn1.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn1.Text = "1";
            this.toolStripBtn1.Click += new System.EventHandler(this.toolStripBtn1_Click);
            // 
            // toolStripBtn2
            // 
            this.toolStripBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn2.Name = "toolStripBtn2";
            this.toolStripBtn2.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn2.Text = "2";
            this.toolStripBtn2.Click += new System.EventHandler(this.toolStripBtn2_Click);
            // 
            // toolStripBtn3
            // 
            this.toolStripBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn3.Name = "toolStripBtn3";
            this.toolStripBtn3.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn3.Text = "3";
            this.toolStripBtn3.Click += new System.EventHandler(this.toolStripBtn3_Click);
            // 
            // toolStripBtn4
            // 
            this.toolStripBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn4.Name = "toolStripBtn4";
            this.toolStripBtn4.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn4.Text = "4";
            this.toolStripBtn4.Click += new System.EventHandler(this.toolStripBtn4_Click);
            // 
            // toolStripBtn5
            // 
            this.toolStripBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn5.Name = "toolStripBtn5";
            this.toolStripBtn5.Size = new System.Drawing.Size(23, 22);
            this.toolStripBtn5.Text = "5";
            this.toolStripBtn5.Click += new System.EventHandler(this.toolStripBtn5_Click);
            // 
            // btnAddFavorite
            // 
            this.btnAddFavorite.Location = new System.Drawing.Point(0, 138);
            this.btnAddFavorite.Name = "btnAddFavorite";
            this.btnAddFavorite.Size = new System.Drawing.Size(30, 23);
            this.btnAddFavorite.TabIndex = 29;
            this.btnAddFavorite.Text = "+";
            this.toolTip.SetToolTip(this.btnAddFavorite, "Add to Favorite");
            this.btnAddFavorite.UseVisualStyleBackColor = true;
            this.btnAddFavorite.Click += new System.EventHandler(this.btnAddFavorite_Click);
            // 
            // btnRemoveFavorite
            // 
            this.btnRemoveFavorite.Location = new System.Drawing.Point(36, 137);
            this.btnRemoveFavorite.Name = "btnRemoveFavorite";
            this.btnRemoveFavorite.Size = new System.Drawing.Size(30, 23);
            this.btnRemoveFavorite.TabIndex = 30;
            this.btnRemoveFavorite.Text = "-";
            this.toolTip.SetToolTip(this.btnRemoveFavorite, "Remove from to Favorite");
            this.btnRemoveFavorite.UseVisualStyleBackColor = true;
            this.btnRemoveFavorite.Click += new System.EventHandler(this.btnRemoveFavorite_Click);
            // 
            // btnFavorites
            // 
            this.btnFavorites.Location = new System.Drawing.Point(72, 137);
            this.btnFavorites.Name = "btnFavorites";
            this.btnFavorites.Size = new System.Drawing.Size(31, 23);
            this.btnFavorites.TabIndex = 31;
            this.btnFavorites.Text = "F";
            this.toolTip.SetToolTip(this.btnFavorites, "Display Favorites");
            this.btnFavorites.UseVisualStyleBackColor = true;
            this.btnFavorites.Click += new System.EventHandler(this.btnFavorites_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(36, 288);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(24, 23);
            this.btnRight.TabIndex = 32;
            this.btnRight.Text = ">";
            this.toolTip.SetToolTip(this.btnRight, "Move selected Ports right");
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(0, 287);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(24, 23);
            this.btnLeft.TabIndex = 33;
            this.btnLeft.Text = "<";
            this.toolTip.SetToolTip(this.btnLeft, "Move selected Ports left");
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(0, 314);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 23);
            this.btnUp.TabIndex = 35;
            this.btnUp.Text = "/\\";
            this.toolTip.SetToolTip(this.btnUp, "Move selected Ports down");
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(36, 315);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 23);
            this.btnDown.TabIndex = 34;
            this.btnDown.Text = "\\/";
            this.toolTip.SetToolTip(this.btnDown, "Move selected Ports up");
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnLabelLeft
            // 
            this.btnLabelLeft.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLabelLeft.Location = new System.Drawing.Point(138, 287);
            this.btnLabelLeft.Name = "btnLabelLeft";
            this.btnLabelLeft.Size = new System.Drawing.Size(103, 23);
            this.btnLabelLeft.TabIndex = 36;
            this.btnLabelLeft.Text = "< Label left";
            this.btnLabelLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.btnLabelLeft, "Move selected Ports label left");
            this.btnLabelLeft.UseVisualStyleBackColor = true;
            this.btnLabelLeft.Click += new System.EventHandler(this.movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // btnLabelRight
            // 
            this.btnLabelRight.Location = new System.Drawing.Point(138, 314);
            this.btnLabelRight.Name = "btnLabelRight";
            this.btnLabelRight.Size = new System.Drawing.Size(103, 23);
            this.btnLabelRight.TabIndex = 37;
            this.btnLabelRight.Text = "> Label right";
            this.btnLabelRight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.btnLabelRight, "Move selected Ports label right");
            this.btnLabelRight.UseVisualStyleBackColor = true;
            this.btnLabelRight.Click += new System.EventHandler(this.movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(304, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Quick Search";
            this.label1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.label1_ControlRemoved);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.doToolStripMenuItem,
            this.versionControlToolStripMenuItem,
            this.portToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(398, 24);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.openScriptAndQueryToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&FileHistory";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // openScriptAndQueryToolStripMenuItem
            // 
            this.openScriptAndQueryToolStripMenuItem.Name = "openScriptAndQueryToolStripMenuItem";
            this.openScriptAndQueryToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openScriptAndQueryToolStripMenuItem.Text = "&Open Script and Query";
            this.openScriptAndQueryToolStripMenuItem.ToolTipText = "Open anothe Addin page Script and Query";
            // 
            // doToolStripMenuItem
            // 
            this.doToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createActivityForOperationToolStripMenuItem,
            this.showFolderToolStripMenuItem,
            this.copyGUIDSQLToClipboardToolStripMenuItem,
            this.toolStripSeparator1,
            this.changeAuthorToolStripMenuItem,
            this.changeAuthorRecursiveToolStripMenuItem});
            this.doToolStripMenuItem.Name = "doToolStripMenuItem";
            this.doToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.doToolStripMenuItem.Text = "&Do";
            this.doToolStripMenuItem.ToolTipText = "Change Author of selected Package / Element";
            // 
            // createActivityForOperationToolStripMenuItem
            // 
            this.createActivityForOperationToolStripMenuItem.Name = "createActivityForOperationToolStripMenuItem";
            this.createActivityForOperationToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.createActivityForOperationToolStripMenuItem.Text = "&Create Activity for Operation";
            this.createActivityForOperationToolStripMenuItem.Click += new System.EventHandler(this.createActivityForOperationToolStripMenuItem_Click);
            // 
            // showFolderToolStripMenuItem
            // 
            this.showFolderToolStripMenuItem.Name = "showFolderToolStripMenuItem";
            this.showFolderToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.showFolderToolStripMenuItem.Text = "&Show Folder";
            this.showFolderToolStripMenuItem.Click += new System.EventHandler(this.showFolderToolStripMenuItem_Click);
            // 
            // copyGUIDSQLToClipboardToolStripMenuItem
            // 
            this.copyGUIDSQLToClipboardToolStripMenuItem.Name = "copyGUIDSQLToClipboardToolStripMenuItem";
            this.copyGUIDSQLToClipboardToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.copyGUIDSQLToClipboardToolStripMenuItem.Text = "&SQL select & update/create/insert";
            this.copyGUIDSQLToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyGUIDSQLToClipboardToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // changeAuthorToolStripMenuItem
            // 
            this.changeAuthorToolStripMenuItem.Name = "changeAuthorToolStripMenuItem";
            this.changeAuthorToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.changeAuthorToolStripMenuItem.Text = "Change Author";
            this.changeAuthorToolStripMenuItem.ToolTipText = "Change Author for:\r\n- Packages\r\n- Element";
            this.changeAuthorToolStripMenuItem.Click += new System.EventHandler(this.changeAuthorToolStripMenuItem_Click);
            // 
            // changeAuthorRecursiveToolStripMenuItem
            // 
            this.changeAuthorRecursiveToolStripMenuItem.Name = "changeAuthorRecursiveToolStripMenuItem";
            this.changeAuthorRecursiveToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.changeAuthorRecursiveToolStripMenuItem.Text = "Change Author recursive";
            this.changeAuthorRecursiveToolStripMenuItem.ToolTipText = "Change Author recursive for:\r\n- Packages\r\n- Elements";
            this.changeAuthorRecursiveToolStripMenuItem.Click += new System.EventHandler(this.changeAuthorRecursiveToolStripMenuItem_Click);
            // 
            // versionControlToolStripMenuItem
            // 
            this.versionControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeXMLFileToolStripMenuItem,
            this.showFolderVCorCodeToolStripMenuItem,
            this.getVCLatesrecursiveToolStripMenuItem,
            this.toolStripSeparator2,
            this.showTortoiseLogToolStripMenuItem,
            this.showTortoiseRepoBrowserToolStripMenuItem,
            this.setSvnKeywordsToolStripMenuItem,
            this.setSvnModuleTaggedValuesToolStripMenuItem,
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem});
            this.versionControlToolStripMenuItem.Name = "versionControlToolStripMenuItem";
            this.versionControlToolStripMenuItem.Size = new System.Drawing.Size(34, 20);
            this.versionControlToolStripMenuItem.Text = "&VC";
            this.versionControlToolStripMenuItem.ToolTipText = "Functions related to Version Control";
            // 
            // changeXMLFileToolStripMenuItem
            // 
            this.changeXMLFileToolStripMenuItem.Name = "changeXMLFileToolStripMenuItem";
            this.changeXMLFileToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.changeXMLFileToolStripMenuItem.Text = "&Change XML file";
            this.changeXMLFileToolStripMenuItem.ToolTipText = "Change the *.xml file patch for a Version controlled package";
            this.changeXMLFileToolStripMenuItem.Click += new System.EventHandler(this.changeXMLFileToolStripMenuItem_Click);
            // 
            // showFolderVCorCodeToolStripMenuItem
            // 
            this.showFolderVCorCodeToolStripMenuItem.Name = "showFolderVCorCodeToolStripMenuItem";
            this.showFolderVCorCodeToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.showFolderVCorCodeToolStripMenuItem.Text = "Show folder VC controlled package";
            this.showFolderVCorCodeToolStripMenuItem.ToolTipText = "Show the folder of the VC controlled package.\r\n\r\nNote:\r\nIn settings you may choos" +
    "e your file manager. \r\nDefault: Explorer.exe";
            this.showFolderVCorCodeToolStripMenuItem.Click += new System.EventHandler(this.showFolderVCorCodeToolStripMenuItem_Click);
            // 
            // getVCLatesrecursiveToolStripMenuItem
            // 
            this.getVCLatesrecursiveToolStripMenuItem.Name = "getVCLatesrecursiveToolStripMenuItem";
            this.getVCLatesrecursiveToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.getVCLatesrecursiveToolStripMenuItem.Text = "Get VC latest (recursive)";
            this.getVCLatesrecursiveToolStripMenuItem.ToolTipText = "Get Version Control latest package (recursive).";
            this.getVCLatesrecursiveToolStripMenuItem.Click += new System.EventHandler(this.getVCLatesrecursiveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(288, 6);
            // 
            // showTortoiseLogToolStripMenuItem
            // 
            this.showTortoiseLogToolStripMenuItem.Name = "showTortoiseLogToolStripMenuItem";
            this.showTortoiseLogToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.showTortoiseLogToolStripMenuItem.Text = "Show Tortoise Log";
            this.showTortoiseLogToolStripMenuItem.Click += new System.EventHandler(this.showTortoiseLogToolStripMenuItem_Click);
            // 
            // showTortoiseRepoBrowserToolStripMenuItem
            // 
            this.showTortoiseRepoBrowserToolStripMenuItem.Name = "showTortoiseRepoBrowserToolStripMenuItem";
            this.showTortoiseRepoBrowserToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.showTortoiseRepoBrowserToolStripMenuItem.Text = "Show Tortoise Repo Browser";
            this.showTortoiseRepoBrowserToolStripMenuItem.Click += new System.EventHandler(this.showTortoiseRepoBrowserToolStripMenuItem_Click);
            // 
            // setSvnKeywordsToolStripMenuItem
            // 
            this.setSvnKeywordsToolStripMenuItem.Name = "setSvnKeywordsToolStripMenuItem";
            this.setSvnKeywordsToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.setSvnKeywordsToolStripMenuItem.Text = "Set svn Keywords";
            this.setSvnKeywordsToolStripMenuItem.Click += new System.EventHandler(this.setSvnKeywordsToolStripMenuItem_Click);
            // 
            // setSvnModuleTaggedValuesToolStripMenuItem
            // 
            this.setSvnModuleTaggedValuesToolStripMenuItem.Name = "setSvnModuleTaggedValuesToolStripMenuItem";
            this.setSvnModuleTaggedValuesToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.setSvnModuleTaggedValuesToolStripMenuItem.Text = "Set svn Module Tagged Values";
            this.setSvnModuleTaggedValuesToolStripMenuItem.ToolTipText = "Set the svn Tagged Values of module for:\r\n- svnDate\r\n- svnRevision";
            this.setSvnModuleTaggedValuesToolStripMenuItem.Click += new System.EventHandler(this.setSvnModuleTaggedValuesToolStripMenuItem_Click);
            // 
            // setSvnModuleTaggedValuesrecursiveToolStripMenuItem
            // 
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Name = "setSvnModuleTaggedValuesrecursiveToolStripMenuItem";
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Text = "Set svn Module Tagged Values (recursive)";
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem.ToolTipText = "Set the svn Tagged Values of module (recursive packages) for:\r\n- svnDate\r\n- svnRe" +
    "vision\r\n\r\nSelect a package to proceed all recursive package.";
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Click += new System.EventHandler(this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.movePortsToolStripMenuItem,
            this.toolStripSeparator7,
            this.deletePortsToolStripMenuItem,
            this.toolStripSeparator3,
            this.connectPortsToolStripMenuItem,
            this.connectPortsInsideComponentsToolStripMenuItem,
            this.toolStripSeparator8,
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem,
            this.toolStripSeparator4,
            this.showPortsInDiagramObjectsToolStripMenuItem,
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem,
            this.hidePortsInDiagramToolStripMenuItem,
            this.toolStripSeparator5,
            this.unhidePortsToolStripMenuItem,
            this.hidePortsToolStripMenuItem,
            this.toolStripSeparator11,
            this.movePortLabelLeftToolStripMenuItem,
            this.movePortLabelRightPositionToolStripMenuItem,
            this.toolStripSeparator12,
            this.movePortLabelLeftToolStripMenuItem1,
            this.movePortLabelRightToolStripMenuItem,
            this.toolStripSeparator9,
            this.orderDiagramItemsToolStripMenuItem});
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.portToolStripMenuItem.Text = "&Port";
            this.portToolStripMenuItem.ToolTipText = "Functions related to port like:\r\n- Visibility\r\n- Move\r\n- Connect";
            // 
            // movePortsToolStripMenuItem
            // 
            this.movePortsToolStripMenuItem.Name = "movePortsToolStripMenuItem";
            this.movePortsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.movePortsToolStripMenuItem.Text = "&Copy ports";
            this.movePortsToolStripMenuItem.ToolTipText = "Copy Ports from selected sources to last selected target.\r\nSources:\r\n- 1 or more " +
    "Classes/Components\r\n- 1 or more Ports\r\nTarget (last selected item): \r\n- Class\r\n-" +
    " Component";
            this.movePortsToolStripMenuItem.Click += new System.EventHandler(this.copyPortsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(372, 6);
            // 
            // deletePortsToolStripMenuItem
            // 
            this.deletePortsToolStripMenuItem.Name = "deletePortsToolStripMenuItem";
            this.deletePortsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.deletePortsToolStripMenuItem.Text = "&Delete Ports";
            this.deletePortsToolStripMenuItem.ToolTipText = "Delete Ports\r\n- selected Ports\r\n- all Ports of selected Elements \r\n-- Class, Comp" +
    "onent";
            this.deletePortsToolStripMenuItem.Click += new System.EventHandler(this.deletePortsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(372, 6);
            // 
            // connectPortsToolStripMenuItem
            // 
            this.connectPortsToolStripMenuItem.Name = "connectPortsToolStripMenuItem";
            this.connectPortsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.connectPortsToolStripMenuItem.Text = "&Connect Ports between Components";
            this.connectPortsToolStripMenuItem.ToolTipText = "Connect all Ports of:\r\n- Selected Class / Components\r\n- Selected Ports\r\nto each s" +
    "elected Class / Component / port with the same port name.\r\n";
            this.connectPortsToolStripMenuItem.Click += new System.EventHandler(this.connectPortsToolStripMenuItem_Click);
            // 
            // connectPortsInsideComponentsToolStripMenuItem
            // 
            this.connectPortsInsideComponentsToolStripMenuItem.Name = "connectPortsInsideComponentsToolStripMenuItem";
            this.connectPortsInsideComponentsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.connectPortsInsideComponentsToolStripMenuItem.Text = "&Connect Ports inside Components";
            this.connectPortsInsideComponentsToolStripMenuItem.Click += new System.EventHandler(this.connectPortsInsideComponentsToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(372, 6);
            // 
            // makeConnectorsUnspecifiedDirectionToolStripMenuItem
            // 
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.Name = "makeConnectorsUnspecifiedDirectionToolStripMenuItem";
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.Text = "&Make Connectors: Unspecified direction";
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.ToolTipText = "Make all directions of connections of Port / Element unspecified.\r\n\r\nSelect Eleme" +
    "nt to make direction of all Port connections to unspecified.";
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.Click += new System.EventHandler(this.makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(372, 6);
            // 
            // showPortsInDiagramObjectsToolStripMenuItem
            // 
            this.showPortsInDiagramObjectsToolStripMenuItem.Name = "showPortsInDiagramObjectsToolStripMenuItem";
            this.showPortsInDiagramObjectsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.showPortsInDiagramObjectsToolStripMenuItem.Text = "&Show Ports on Diagram";
            this.showPortsInDiagramObjectsToolStripMenuItem.ToolTipText = "Show the ports on diagram of:\r\n- Selected Class / Component\r\n";
            this.showPortsInDiagramObjectsToolStripMenuItem.Click += new System.EventHandler(this.showPortsInDiagramObjectsToolStripMenuItem_Click);
            // 
            // showSendingPortsLeftRecievingPortsRightToolStripMenuItem
            // 
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Name = "showSendingPortsLeftRecievingPortsRightToolStripMenuItem";
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Text = "&Show receiving Ports left, sending Ports right on Diagram";
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.ToolTipText = "Show receiving Ports left, sending Ports right for selected Elements in Diagram\r\n" +
    "\r\n";
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Click += new System.EventHandler(this.showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click);
            // 
            // hidePortsInDiagramToolStripMenuItem
            // 
            this.hidePortsInDiagramToolStripMenuItem.Name = "hidePortsInDiagramToolStripMenuItem";
            this.hidePortsInDiagramToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.hidePortsInDiagramToolStripMenuItem.Text = "&Remove Ports from Diagram";
            this.hidePortsInDiagramToolStripMenuItem.ToolTipText = "Remove ports from Diagram:\r\n- Ports from selected Diagramelements\r\n- Selected por" +
    "ts";
            this.hidePortsInDiagramToolStripMenuItem.Click += new System.EventHandler(this.removePortsInDiagramToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(372, 6);
            // 
            // unhidePortsToolStripMenuItem
            // 
            this.unhidePortsToolStripMenuItem.Name = "unhidePortsToolStripMenuItem";
            this.unhidePortsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.unhidePortsToolStripMenuItem.Text = "&Show Port label";
            this.unhidePortsToolStripMenuItem.ToolTipText = "Show Port labels of selected:\r\n- Class / Component\r\n- Ports (embedded Elements)";
            this.unhidePortsToolStripMenuItem.Click += new System.EventHandler(this.viewPortLabelToolStripMenuItem_Click);
            // 
            // hidePortsToolStripMenuItem
            // 
            this.hidePortsToolStripMenuItem.Name = "hidePortsToolStripMenuItem";
            this.hidePortsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.hidePortsToolStripMenuItem.Text = "&Hide Port label";
            this.hidePortsToolStripMenuItem.ToolTipText = "Hide Port labels of selected:\r\n- Class / Component\r\n- Ports (embedded Elements)";
            this.hidePortsToolStripMenuItem.Click += new System.EventHandler(this.hidePortLabelToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(372, 6);
            // 
            // movePortLabelLeftToolStripMenuItem
            // 
            this.movePortLabelLeftToolStripMenuItem.Name = "movePortLabelLeftToolStripMenuItem";
            this.movePortLabelLeftToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.movePortLabelLeftToolStripMenuItem.Text = "&Move Port label left position";
            this.movePortLabelLeftToolStripMenuItem.Click += new System.EventHandler(this.movePortLableLeftPositionToolStripMenuItem_Click);
            // 
            // movePortLabelRightPositionToolStripMenuItem
            // 
            this.movePortLabelRightPositionToolStripMenuItem.Name = "movePortLabelRightPositionToolStripMenuItem";
            this.movePortLabelRightPositionToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.movePortLabelRightPositionToolStripMenuItem.Text = "&Move port label right position";
            this.movePortLabelRightPositionToolStripMenuItem.Click += new System.EventHandler(this.movePortLableRightPositionToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(372, 6);
            // 
            // movePortLabelLeftToolStripMenuItem1
            // 
            this.movePortLabelLeftToolStripMenuItem1.Name = "movePortLabelLeftToolStripMenuItem1";
            this.movePortLabelLeftToolStripMenuItem1.Size = new System.Drawing.Size(375, 22);
            this.movePortLabelLeftToolStripMenuItem1.Text = "&Move Port label left";
            this.movePortLabelLeftToolStripMenuItem1.Click += new System.EventHandler(this.movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // movePortLabelRightToolStripMenuItem
            // 
            this.movePortLabelRightToolStripMenuItem.Name = "movePortLabelRightToolStripMenuItem";
            this.movePortLabelRightToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.movePortLabelRightToolStripMenuItem.Text = "&Move Port label right";
            this.movePortLabelRightToolStripMenuItem.Click += new System.EventHandler(this.movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(372, 6);
            // 
            // orderDiagramItemsToolStripMenuItem
            // 
            this.orderDiagramItemsToolStripMenuItem.Name = "orderDiagramItemsToolStripMenuItem";
            this.orderDiagramItemsToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
            this.orderDiagramItemsToolStripMenuItem.Text = "&Order Diagram items";
            this.orderDiagramItemsToolStripMenuItem.ToolTipText = "Order selected Diagram objects alphabetically.\r\n";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.ToolTipText = "Information about hoTools";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "&Help";
            this.helpToolStripMenuItem1.ToolTipText = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(395, 2);
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(3, 47);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(395, 27);
            this.toolStripContainer1.TabIndex = 28;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // AddinControlGUI
            // 
            this.Controls.Add(this.btnLabelRight);
            this.Controls.Add(this.btnLabelLeft);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnFavorites);
            this.Controls.Add(this.btnRemoveFavorite);
            this.Controls.Add(this.btnAddFavorite);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.txtUserText);
            this.Controls.Add(this.btnBezier);
            this.Controls.Add(this.btnUpdateActivityParameter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnC);
            this.Controls.Add(this.btnD);
            this.Controls.Add(this.btnA);
            this.Controls.Add(this.btnOR);
            this.Controls.Add(this.btnComposite);
            this.Controls.Add(this.btnDisplaySpecification);
            this.Controls.Add(this.btnFindUsage);
            this.Controls.Add(this.btnLocateType);
            this.Controls.Add(this.btnAddDiagramNote);
            this.Controls.Add(this.btnAddElementNote);
            this.Controls.Add(this.btnLocateOperation);
            this.Controls.Add(this.btnDisplayBehavior);
            this.Controls.Add(this.btnOS);
            this.Controls.Add(this.btnTV);
            this.Controls.Add(this.btnTH);
            this.Controls.Add(this.btnLV);
            this.Controls.Add(this.btnLH);
            this.Controls.Add(this.menuStrip1);
            this.Name = "AddinControlGUI";
            this.Size = new System.Drawing.Size(398, 342);
            this.Load += new System.EventHandler(this.AddinControlGUI_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region parameterizeMenusAndButtons
        /// <summary>
        /// Parametrerize menue & buttons to visible/hidden due to
        /// - isAdvanced
        /// - isSvnSupported
        /// - isVcSupported
        /// </summary>
        public void parameterizeMenusAndButtons()
        {
            bool visibleSvnVC = true && !(AddinSettings.isSvnSupport == false | AddinSettings.isVcSupport == false);

            showTortoiseRepoBrowserToolStripMenuItem.Visible = visibleSvnVC;
            showTortoiseLogToolStripMenuItem.Visible = visibleSvnVC;
            setSvnModuleTaggedValuesToolStripMenuItem.Visible = visibleSvnVC;
            setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Visible = visibleSvnVC;
            setSvnKeywordsToolStripMenuItem.Visible = visibleSvnVC;

            // Vissible VC
            bool visibleVC = true && AddinSettings.isVcSupport != false;
           
            getVCLatesrecursiveToolStripMenuItem.Visible = visibleVC;
            changeXMLFileToolStripMenuItem.Visible = visibleVC;
            orderDiagramItemsToolStripMenuItem.Visible = visibleVC;

            if (AddinSettings.isSvnSupport == false && AddinSettings.isVcSupport == false)
            {
                versionControlToolStripMenuItem.Visible = false;
            }
            else
            {
                versionControlToolStripMenuItem.Visible = true;
            }

            // Visual Port Support
            bool visiblePorts = false || AddinSettings.isAdvancedPort;

            btnLeft.Visible = visiblePorts;
            btnRight.Visible = visiblePorts;
            btnUp.Visible = visiblePorts;
            btnDown.Visible = visiblePorts;

            btnLabelLeft.Visible = visiblePorts;
            btnLabelRight.Visible = visiblePorts;

            // Note in diagram support
            bool visibleDiagramNote = false || AddinSettings.isAdvancedDiagramNote;
            btnAddDiagramNote.Visible = visibleDiagramNote;
            btnAddElementNote.Visible = visibleDiagramNote;

            //bool visibleDiagramNote = false || _addinSettings.isAdvancedDiagramNote;


        }
        #endregion
        #region parameterizeButtonQueries
        public void parameterizeButtonQueries()
        {
            for (int pos = 0; pos < AddinSettings.buttonsSearch.Length; pos++)
            {
                if (AddinSettings.buttonsSearch[pos] == null) continue;
                EaAddinButtons shortcut = AddinSettings.buttonsSearch[pos];
                switch (pos)
                {
                    case 0:
                        toolStripBtn11.Text = shortcut.keyText.ToString();
                        toolStripBtn11.ToolTipText = shortcut.keySearchTooltip.ToString();
                        break;
                    case 1:
                        toolStripBtn12.Text = shortcut.keyText.ToString();
                        toolStripBtn12.ToolTipText = shortcut.keySearchTooltip.ToString();
                        break;
                    case 2:
                        toolStripBtn13.Text = shortcut.keyText.ToString();
                        toolStripBtn13.ToolTipText = shortcut.keySearchTooltip.ToString();
                        break;
                    case 3:
                        toolStripBtn14.Text = shortcut.keyText.ToString();
                        toolStripBtn14.ToolTipText = shortcut.keySearchTooltip.ToString();
                        break;
                    case 4:
                        toolStripBtn15.Text = shortcut.keyText.ToString();
                        toolStripBtn15.ToolTipText = shortcut.keySearchTooltip.ToString();
                        break;

                }
            }
        }
        #endregion
        #region parameterizeButtonServices
        public void parameterizeButtonServices()
        {
            for (int pos = 0; pos < AddinSettings.buttonsServices.Count; pos++)
            {
                if (AddinSettings.buttonsServices[pos] == null) continue;
                ServicesCallConfig service = AddinSettings.buttonsServices[pos];
                switch (pos)
                {
                    case 0:
                        toolStripBtn1.Text = service.ButtonText;
                        toolStripBtn1.ToolTipText = service.Help;
                        break;
                    case 1:
                        toolStripBtn2.Text = service.ButtonText;
                        toolStripBtn2.ToolTipText = service.Help;
                        break;
                    case 2:
                        toolStripBtn3.Text = service.ButtonText;
                        toolStripBtn3.ToolTipText = service.Help;
                        break;
                    case 3:
                        toolStripBtn4.Text = service.ButtonText;
                        toolStripBtn4.ToolTipText = service.Help;
                        break;
                    case 4:
                        toolStripBtn5.Text = service.ButtonText;
                        toolStripBtn5.ToolTipText = service.Help;
                        break;

                }
            }
        }
        #endregion

        

        private void AddinControlGUI_Load(object sender, EventArgs e)
        {

        }
    }
}