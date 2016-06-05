using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.Settings;
using hoTools.EaServices;
using hoTools.EAServicesPort;
using Control.EaAddinShortcuts;
using hoTools.Settings.Key;
using hoTools.Settings.Toolbar;

using hoTools.Utils.SQL;

using System.Threading;
using System.Globalization;


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

        // Windows/Frames
        FrmQueryAndScript _frmQueryAndScript;
        FrmSettingsGeneral _frmSettingsGeneral;

        FrmSettingsToolbar _frmSettingsToolbar;
        FrmSettingsKey _frmSettingsKey;
        FrmSettingsLineStyle _frmSettingsLineStyle;

       
        #region Generated

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
        private ToolStrip toolStripQuery;
        private ToolStripButton toolStripSearchBtn1;
        private ToolStripButton toolStripSearchBtn2;
        private ToolStripButton toolStripSearchBtn3;
        private ToolStripButton toolStripSearchBtn4;
        private ToolStripButton toolStripSearchBtn5;
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
        private Button btnShowFavorites;
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
        private ToolStripMenuItem settingsQueryAndSctipToolStripMenuItem;
        private ToolStripMenuItem settingGeneralToolStripMenuItem;
        private ToolStripMenuItem settingsToolbarToolStripMenuItem;
        private Panel panelButtons;
        private Panel panelLineStyle;
        private Panel panelFavorite;
        private Panel panelNote;
        private Panel panelPort;
        private Panel panelAdvanced;
        private TextBox txtSearchName;
        private Panel panelConveyedItems;
        private Button btnConveyedItemElement;
        private Button btnConveyedItemConnector;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton toolStripServiceBtn1;
        private ToolStripButton toolStripServiceBtn2;
        private ToolStripButton toolStripServiceBtn3;
        private ToolStripButton toolStripServiceBtn4;
        private ToolStripButton toolStripServiceBtn5;
        private ToolStripMenuItem settingsGlobalKeysToolStripMenuItem;
        private Label lblPorts;
        private Label lblConveyedItems;
        private TableLayoutPanel panelQuickSearch;
        private TextBox txtSearchText;
        #endregion

        #region Constructor
        public AddinControlGUI()
        {
            InitializeComponent();
            
        }
        #endregion
       
        public string getText() => txtSearchText.Text;


        /// <summary>
        /// Repository. Make sure settings are updated before.
        /// </summary>
        public override EA.Repository Repository
        {
            set
            {
                base.Repository = value;
                // only if there is a repository available
                if (value.ProjectGUID != "")
                {
                    try
                    {
                        initializeSettings();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "ActiveX: Error Initialization");
                    }
                }
            }
        }
        #region initializingSettings
        /// <summary>
        /// Initialize Setting (not Keys). Be sure Repository is loaded! Also don't change the sequence of hide/visible.
        /// </summary>
        public void initializeSettings()
        {
            parameterizeMenusAndButtons();
            // parameterize 5 Buttons to quickly run search
            parameterizeSearchButton();
            // parameterize 5 Buttons to quickly run services
            parameterizeServiceButton();
        }
        #endregion

        #region IActiveX Members
        public string getName() => "hoTools.AddinControl";


        #endregion

        #region save
        public void Save()
        {
            return;
        }
        #endregion
        #region User Actions
        #region Button & Menu
        void btnDisplayBehavior_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(Repository, EaService.displayMode.Behavior);
        }
        void btnLocateOperation_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(Repository, EaService.displayMode.Method);
        }

        void btnAddElementNote_Click(object sender, EventArgs e)
        {
            EaService.addElementNote(Repository);
        }

        void btnAddDiagramNote_Click(object sender, EventArgs e)
        {
            EaService.addDiagramNote(Repository);
        }

        void btnLocateType_Click(object sender, EventArgs e)
        {
            EaService.locateType(Repository);
        }


        void btnShowSpecification_Click(object sender, EventArgs e)
        {
            EaService.showSpecification(Repository);
        }


        void btnFindUsage_Click(object sender, EventArgs e)
        {
            EaService.findUsage(Repository);
        }

        void btnC_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "C");
        }


        void btnD_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "D");
        }



        void btnA_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "A");
        }


        void btnOR_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "OR");
        }

        void btnComposite_Click(object sender, EventArgs e)
        {
            EaService.navigateComposite(Repository);
        }

       
        #pragma warning disable RECS0154
        void getAllLatestrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.getVcLatestRecursive(Repository);
        }
        #pragma warning restore RECS0154
        void createActivityForOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.CreateActivityForOperation(Repository);
        }

        void showFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowFolder(Repository, isTotalCommander: false);
        }

        void copyGUIDSQLToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.copyGuidSqlToClipboard(Repository);
        }

        void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._frmSettingsLineStyle = new FrmSettingsLineStyle(AddinSettings, this);
            this._frmSettingsLineStyle.ShowDialog();
        }

        void btnUpdateActivityParametzer_Click(object sender, EventArgs e)
        {
            EaService.UpdateActivityParameter(Repository);
        }

        void aboutToolStripMenuItem_Click(object sender, EventArgs e)
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

        void changeXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.setNewXmlPath(Repository);
        }

        void btnBezier_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "B");
        }

        void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, EaService.getAssemblyPath() + "\\" + "hoTools.chm");
        }



        void toolStripBtn1_Click(object sender, EventArgs e)
        {
            runService(0);
        }

         void toolStripBtn2_Click(object sender, EventArgs e)
        {
            runService(1);
        }
         void toolStripBtn3_Click(object sender, EventArgs e)
        {
            runService(2);
        }

        void toolStripBtn4_Click(object sender, EventArgs e)
        {
            runService(3);
        }

        void toolStripBtn5_Click(object sender, EventArgs e)
        {
            runService(4);
        }

        void toolStripBtn11_Click(object sender, EventArgs e)
        {
            runSearch(0);
        }

        void toolStripBtn12_Click(object sender, EventArgs e)
        {
            runSearch(1);
        }

        void toolStripBtn13_Click(object sender, EventArgs e)
        {
            runSearch(2);
        }

        void toolStripBtn14_Click(object sender, EventArgs e)
        {
            runSearch(3);
        }

        void toolStripBtn15_Click(object sender, EventArgs e)
        {
            runSearch(4);
        }
        void runSearch(int pos)
        {
            if (AddinSettings.buttonsSearch[pos] is EaAddinShortcutSearch)
            {

                var sh = (EaAddinShortcutSearch)AddinSettings.buttonsSearch[pos];
                if (sh.keySearchName == "") return;
                try
                {
                    Repository.RunModelSearch(sh.keySearchName, sh.keySearchTerm, "", "");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error start search '" + sh.keySearchName +
                       " " + sh.keySearchTerm + "'");
                }
            }
        }
        void runService(int pos)
        {
            if (AddinSettings.buttonsServices[pos] is hoTools.EaServices.ServicesCallConfig)
            {

                var sh = (hoTools.EaServices.ServicesCallConfig)AddinSettings.buttonsServices[pos];
                if (sh.Method == null) return;
                sh.Invoke(Repository, txtSearchText.Text);

            }
        }

        void changeAuthorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.changeAuthor(Repository);
        }

        void changeAuthorRecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.changeUserRecursive(Repository);
        }

        void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void showFolderVCorCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowFolder(Repository, isTotalCommander: false);
        }

        void showTortoiseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.gotoSvnLog(Repository, pkg);
        }

        void showTortoiseRepoBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.gotoSvnBrowser(Repository, pkg);
        }

        void getVCLatesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.getVcLatestRecursive(Repository);
        }

        void setSvnKeywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.setSvnProperty(Repository, pkg);
        }

        void setSvnModuleTaggedValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.setDirectoryTaggedValues(Repository, pkg);
        }

        void setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.setTaggedValueGui(Repository);
        }

        void btnAddFavorite_Click(object sender, EventArgs e)
        {
            EaService.AddFavorite(Repository);
        }

        void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            EaService.RemoveFavorite(Repository);
        }

        void btnFavorites_Click(object sender, EventArgs e)
        {
            EaService.Favorites(Repository);
        }

        
        /// <summary>
        /// Remove ports from elements by:
        /// - Selected Elements
        /// - Selected Ports
        /// 
        /// Note: Selection isn't restored because selected objects might be deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region removePortsInDiagramToolStripMenuItem_Click
        void removePortsInDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.removePortFromDiagramGUI();
           
        }
        #endregion

        #region showPortsInDiagramObjects
        void showPortsInDiagramObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.showPortsInDiagram(false);

           
        }
        #endregion
        #region showReceivingPortsLeftSendingPortsRight
        void showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.showPortsInDiagram(true);
        }
        #endregion

        #region copyPorts
        void copyPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.copyPortsGUI();
           
        }
        #endregion
        #region deletePortsWhichAreMarkedForDeletion
        #pragma warning disable RECS0154
        void deletePortsWhichAreMarkedForDeletionfutureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.deletePortsMarkedPorts();
        }
       #pragma warning restore RECS0154
        #endregion

        /// <summary>
        /// Hide labels from elements by:
        /// - Selected Elements
        /// - Selected Ports
        /// 
        /// Note: Selection isn't restored because selected objects might be deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region hidePortLabelToolStripMenuItem_Click
        void hidePortLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.changeLabelGUI(PortServices.LabelStyle.IS_HIDDEN);
        }
        #endregion

        #region viewPortLabelToolStripMenuItem_Click
        void viewPortLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.changeLabelGUI(PortServices.LabelStyle.IS_SHOWN);
       }
        #endregion
        #region movePortLableLeftPositionToolStripMenuItem_Click
        void movePortLableLeftPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_LEFT);
        }
        #endregion

        #region movePortLableRightPositionToolStripMenuItem_Click
        void movePortLableRightPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_RIGHT);
        }
        #endregion


        #region movePortLablePlusPositionToolStripMenuItem_Click
        void movePortLablePlusPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_PLUS);
        }
        #endregion


        #region movePortLableMinusPositionToolStripMenuItem_Click
        void movePortLableMinusPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.changeLabelGUI(PortServices.LabelStyle.POSITION_MINUS);
        }
        #endregion


        void connectPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.connectPortsGUI();
            
        }

        void connectPortsInsideComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.connectPortsInsideGUI();
        }

        void deletePortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.deletePortsGUI();
        }

        void btnLeft_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedLeftGUI(Repository);
        }

        void btnRight_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedRightGUI(Repository);
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedUpGUI(Repository);
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            EaService.moveEmbeddedDownGUI(Repository);
        }

        void makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.setConnectionDirectionUnspecifiedGUI();
        }
       
        
       

       


        //---------------------------------------------------------------------------------------------------------------
        // line style
        // LH = "Line Style: Lateral Horizontal";
        // LV = "Line Style: Lateral Vertical";
        // TH = "Line Style: Tree Horizontal";
        // TV = "Line Style: Tree Vertical";
        // OS = "Line Style: Orthogonal Square";
        void btnLH_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "LH");
        }
        void btnLV_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "LV");
        }
        void btnTH_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "LH");
        }
        void btnTV_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "TV");
        }
        void btnOS_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(Repository, "OS");
        }
        #endregion
        #region Key down
        /// <summary>
        /// Overrides TextBox to handle the enter key. Per default it isn't passed
        /// </summary>
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
        // see at:  protected override boolean IsInputKey(Keys keyData)
        void txtUserText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EaService.runQuickSearch(Repository, getSearchName(), txtSearchText.Text);
                e.Handled = true;
            }
        }
        #endregion
        #region Mouse
        /// <summary>
        /// Double Mouse Click in SearchText inserts Clipboard content and runs the query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtSearchText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtSearchText.Text = Clipboard.GetText();
            EaService.runQuickSearch(Repository, getSearchName(), txtSearchText.Text);
        }

        /// <summary>
        /// Double Mouse Click in SearchName runs the query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtSearchName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EaService.runQuickSearch(Repository, getSearchName(), txtSearchText.Text);
        }

        /// <summary>
        /// Get Search Name from GUI text field. If empty use Search name from settings
        /// </summary>
        /// <returns></returns>
        string getSearchName()
        {
            string searchName = txtSearchName.Text.Trim();
            if (searchName == "")
            {
                searchName = AddinSettings.quickSearchName;
                txtSearchName.Text = searchName;
            }

            return searchName;
        }
        #endregion
        #endregion

        #region InitializeComponent
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinControlGUI));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStripQuery = new System.Windows.Forms.ToolStrip();
            this.toolStripSearchBtn1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSearchBtn2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSearchBtn3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSearchBtn4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSearchBtn5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripServiceBtn1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripServiceBtn2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripServiceBtn3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripServiceBtn4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripServiceBtn5 = new System.Windows.Forms.ToolStripButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnLabelRight = new System.Windows.Forms.Button();
            this.btnLabelLeft = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnShowFavorites = new System.Windows.Forms.Button();
            this.btnRemoveFavorite = new System.Windows.Forms.Button();
            this.btnAddFavorite = new System.Windows.Forms.Button();
            this.txtSearchText = new System.Windows.Forms.TextBox();
            this.btnBezier = new System.Windows.Forms.Button();
            this.btnUpdateActivityParameter = new System.Windows.Forms.Button();
            this.btnC = new System.Windows.Forms.Button();
            this.btnD = new System.Windows.Forms.Button();
            this.btnA = new System.Windows.Forms.Button();
            this.btnOR = new System.Windows.Forms.Button();
            this.btnComposite = new System.Windows.Forms.Button();
            this.btnDisplaySpecification = new System.Windows.Forms.Button();
            this.btnFindUsage = new System.Windows.Forms.Button();
            this.btnLocateType = new System.Windows.Forms.Button();
            this.btnAddDiagramNote = new System.Windows.Forms.Button();
            this.btnAddElementNote = new System.Windows.Forms.Button();
            this.btnLocateOperation = new System.Windows.Forms.Button();
            this.btnDisplayBehavior = new System.Windows.Forms.Button();
            this.btnOS = new System.Windows.Forms.Button();
            this.btnTV = new System.Windows.Forms.Button();
            this.btnTH = new System.Windows.Forms.Button();
            this.btnLV = new System.Windows.Forms.Button();
            this.btnLH = new System.Windows.Forms.Button();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.btnConveyedItemConnector = new System.Windows.Forms.Button();
            this.btnConveyedItemElement = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingGeneralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsQueryAndSctipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsGlobalKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelLineStyle = new System.Windows.Forms.Panel();
            this.panelFavorite = new System.Windows.Forms.Panel();
            this.panelNote = new System.Windows.Forms.Panel();
            this.panelPort = new System.Windows.Forms.Panel();
            this.lblPorts = new System.Windows.Forms.Label();
            this.panelAdvanced = new System.Windows.Forms.Panel();
            this.panelConveyedItems = new System.Windows.Forms.Panel();
            this.lblConveyedItems = new System.Windows.Forms.Label();
            this.panelQuickSearch = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripQuery.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelLineStyle.SuspendLayout();
            this.panelFavorite.SuspendLayout();
            this.panelNote.SuspendLayout();
            this.panelPort.SuspendLayout();
            this.panelAdvanced.SuspendLayout();
            this.panelConveyedItems.SuspendLayout();
            this.panelQuickSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripQuery);
            // 
            // toolStripQuery
            // 
            resources.ApplyResources(this.toolStripQuery, "toolStripQuery");
            this.toolStripQuery.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSearchBtn1,
            this.toolStripSearchBtn2,
            this.toolStripSearchBtn3,
            this.toolStripSearchBtn4,
            this.toolStripSearchBtn5,
            this.toolStripSeparator6,
            this.toolStripServiceBtn1,
            this.toolStripServiceBtn2,
            this.toolStripServiceBtn3,
            this.toolStripServiceBtn4,
            this.toolStripServiceBtn5});
            this.toolStripQuery.Name = "toolStripQuery";
            // 
            // toolStripSearchBtn1
            // 
            this.toolStripSearchBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripSearchBtn1, "toolStripSearchBtn1");
            this.toolStripSearchBtn1.Name = "toolStripSearchBtn1";
            this.toolStripSearchBtn1.Click += new System.EventHandler(this.toolStripBtn11_Click);
            // 
            // toolStripSearchBtn2
            // 
            this.toolStripSearchBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSearchBtn2.Name = "toolStripSearchBtn2";
            resources.ApplyResources(this.toolStripSearchBtn2, "toolStripSearchBtn2");
            this.toolStripSearchBtn2.Click += new System.EventHandler(this.toolStripBtn12_Click);
            // 
            // toolStripSearchBtn3
            // 
            this.toolStripSearchBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripSearchBtn3, "toolStripSearchBtn3");
            this.toolStripSearchBtn3.Name = "toolStripSearchBtn3";
            this.toolStripSearchBtn3.Click += new System.EventHandler(this.toolStripBtn13_Click);
            // 
            // toolStripSearchBtn4
            // 
            this.toolStripSearchBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripSearchBtn4, "toolStripSearchBtn4");
            this.toolStripSearchBtn4.Name = "toolStripSearchBtn4";
            this.toolStripSearchBtn4.Click += new System.EventHandler(this.toolStripBtn14_Click);
            // 
            // toolStripSearchBtn5
            // 
            this.toolStripSearchBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripSearchBtn5, "toolStripSearchBtn5");
            this.toolStripSearchBtn5.Name = "toolStripSearchBtn5";
            this.toolStripSearchBtn5.Click += new System.EventHandler(this.toolStripBtn15_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // toolStripServiceBtn1
            // 
            this.toolStripServiceBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripServiceBtn1, "toolStripServiceBtn1");
            this.toolStripServiceBtn1.Name = "toolStripServiceBtn1";
            // 
            // toolStripServiceBtn2
            // 
            this.toolStripServiceBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripServiceBtn2, "toolStripServiceBtn2");
            this.toolStripServiceBtn2.Name = "toolStripServiceBtn2";
            // 
            // toolStripServiceBtn3
            // 
            this.toolStripServiceBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripServiceBtn3, "toolStripServiceBtn3");
            this.toolStripServiceBtn3.Name = "toolStripServiceBtn3";
            // 
            // toolStripServiceBtn4
            // 
            this.toolStripServiceBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripServiceBtn4, "toolStripServiceBtn4");
            this.toolStripServiceBtn4.Name = "toolStripServiceBtn4";
            // 
            // toolStripServiceBtn5
            // 
            this.toolStripServiceBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripServiceBtn5, "toolStripServiceBtn5");
            this.toolStripServiceBtn5.Name = "toolStripServiceBtn5";
            // 
            // btnLabelRight
            // 
            resources.ApplyResources(this.btnLabelRight, "btnLabelRight");
            this.btnLabelRight.Name = "btnLabelRight";
            this.toolTip.SetToolTip(this.btnLabelRight, resources.GetString("btnLabelRight.ToolTip"));
            this.btnLabelRight.UseVisualStyleBackColor = true;
            this.btnLabelRight.Click += new System.EventHandler(this.movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // btnLabelLeft
            // 
            resources.ApplyResources(this.btnLabelLeft, "btnLabelLeft");
            this.btnLabelLeft.Name = "btnLabelLeft";
            this.toolTip.SetToolTip(this.btnLabelLeft, resources.GetString("btnLabelLeft.ToolTip"));
            this.btnLabelLeft.UseVisualStyleBackColor = true;
            this.btnLabelLeft.Click += new System.EventHandler(this.movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Name = "btnUp";
            this.toolTip.SetToolTip(this.btnUp, resources.GetString("btnUp.ToolTip"));
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Name = "btnDown";
            this.toolTip.SetToolTip(this.btnDown, resources.GetString("btnDown.ToolTip"));
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnLeft
            // 
            resources.ApplyResources(this.btnLeft, "btnLeft");
            this.btnLeft.Name = "btnLeft";
            this.toolTip.SetToolTip(this.btnLeft, resources.GetString("btnLeft.ToolTip"));
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            resources.ApplyResources(this.btnRight, "btnRight");
            this.btnRight.Name = "btnRight";
            this.toolTip.SetToolTip(this.btnRight, resources.GetString("btnRight.ToolTip"));
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnShowFavorites
            // 
            resources.ApplyResources(this.btnShowFavorites, "btnShowFavorites");
            this.btnShowFavorites.Name = "btnShowFavorites";
            this.toolTip.SetToolTip(this.btnShowFavorites, resources.GetString("btnShowFavorites.ToolTip"));
            this.btnShowFavorites.UseVisualStyleBackColor = true;
            this.btnShowFavorites.Click += new System.EventHandler(this.btnFavorites_Click);
            // 
            // btnRemoveFavorite
            // 
            resources.ApplyResources(this.btnRemoveFavorite, "btnRemoveFavorite");
            this.btnRemoveFavorite.Name = "btnRemoveFavorite";
            this.toolTip.SetToolTip(this.btnRemoveFavorite, resources.GetString("btnRemoveFavorite.ToolTip"));
            this.btnRemoveFavorite.UseVisualStyleBackColor = true;
            this.btnRemoveFavorite.Click += new System.EventHandler(this.btnRemoveFavorite_Click);
            // 
            // btnAddFavorite
            // 
            resources.ApplyResources(this.btnAddFavorite, "btnAddFavorite");
            this.btnAddFavorite.Name = "btnAddFavorite";
            this.toolTip.SetToolTip(this.btnAddFavorite, resources.GetString("btnAddFavorite.ToolTip"));
            this.btnAddFavorite.UseVisualStyleBackColor = true;
            this.btnAddFavorite.Click += new System.EventHandler(this.btnAddFavorite_Click);
            // 
            // txtSearchText
            // 
            resources.ApplyResources(this.txtSearchText, "txtSearchText");
            this.txtSearchText.Name = "txtSearchText";
            this.toolTip.SetToolTip(this.txtSearchText, resources.GetString("txtSearchText.ToolTip"));
            this.txtSearchText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtSearchText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSearchText_MouseDoubleClick);
            // 
            // btnBezier
            // 
            resources.ApplyResources(this.btnBezier, "btnBezier");
            this.btnBezier.Name = "btnBezier";
            this.toolTip.SetToolTip(this.btnBezier, resources.GetString("btnBezier.ToolTip"));
            this.btnBezier.UseVisualStyleBackColor = true;
            this.btnBezier.Click += new System.EventHandler(this.btnBezier_Click);
            // 
            // btnUpdateActivityParameter
            // 
            resources.ApplyResources(this.btnUpdateActivityParameter, "btnUpdateActivityParameter");
            this.btnUpdateActivityParameter.Name = "btnUpdateActivityParameter";
            this.toolTip.SetToolTip(this.btnUpdateActivityParameter, resources.GetString("btnUpdateActivityParameter.ToolTip"));
            this.btnUpdateActivityParameter.UseVisualStyleBackColor = true;
            this.btnUpdateActivityParameter.Click += new System.EventHandler(this.btnUpdateActivityParametzer_Click);
            // 
            // btnC
            // 
            resources.ApplyResources(this.btnC, "btnC");
            this.btnC.Name = "btnC";
            this.toolTip.SetToolTip(this.btnC, resources.GetString("btnC.ToolTip"));
            this.btnC.UseVisualStyleBackColor = true;
            this.btnC.Click += new System.EventHandler(this.btnC_Click);
            // 
            // btnD
            // 
            resources.ApplyResources(this.btnD, "btnD");
            this.btnD.Name = "btnD";
            this.toolTip.SetToolTip(this.btnD, resources.GetString("btnD.ToolTip"));
            this.btnD.UseVisualStyleBackColor = true;
            this.btnD.Click += new System.EventHandler(this.btnD_Click);
            // 
            // btnA
            // 
            resources.ApplyResources(this.btnA, "btnA");
            this.btnA.Name = "btnA";
            this.toolTip.SetToolTip(this.btnA, resources.GetString("btnA.ToolTip"));
            this.btnA.UseVisualStyleBackColor = true;
            this.btnA.Click += new System.EventHandler(this.btnA_Click);
            // 
            // btnOR
            // 
            resources.ApplyResources(this.btnOR, "btnOR");
            this.btnOR.Name = "btnOR";
            this.toolTip.SetToolTip(this.btnOR, resources.GetString("btnOR.ToolTip"));
            this.btnOR.UseVisualStyleBackColor = true;
            this.btnOR.Click += new System.EventHandler(this.btnOR_Click);
            // 
            // btnComposite
            // 
            resources.ApplyResources(this.btnComposite, "btnComposite");
            this.btnComposite.Name = "btnComposite";
            this.toolTip.SetToolTip(this.btnComposite, resources.GetString("btnComposite.ToolTip"));
            this.btnComposite.UseVisualStyleBackColor = true;
            this.btnComposite.Click += new System.EventHandler(this.btnComposite_Click);
            // 
            // btnDisplaySpecification
            // 
            resources.ApplyResources(this.btnDisplaySpecification, "btnDisplaySpecification");
            this.btnDisplaySpecification.Name = "btnDisplaySpecification";
            this.toolTip.SetToolTip(this.btnDisplaySpecification, resources.GetString("btnDisplaySpecification.ToolTip"));
            this.btnDisplaySpecification.UseVisualStyleBackColor = true;
            this.btnDisplaySpecification.Click += new System.EventHandler(this.btnShowSpecification_Click);
            // 
            // btnFindUsage
            // 
            resources.ApplyResources(this.btnFindUsage, "btnFindUsage");
            this.btnFindUsage.Name = "btnFindUsage";
            this.toolTip.SetToolTip(this.btnFindUsage, resources.GetString("btnFindUsage.ToolTip"));
            this.btnFindUsage.UseVisualStyleBackColor = true;
            this.btnFindUsage.Click += new System.EventHandler(this.btnFindUsage_Click);
            // 
            // btnLocateType
            // 
            resources.ApplyResources(this.btnLocateType, "btnLocateType");
            this.btnLocateType.Name = "btnLocateType";
            this.toolTip.SetToolTip(this.btnLocateType, resources.GetString("btnLocateType.ToolTip"));
            this.btnLocateType.UseVisualStyleBackColor = true;
            this.btnLocateType.Click += new System.EventHandler(this.btnLocateType_Click);
            // 
            // btnAddDiagramNote
            // 
            resources.ApplyResources(this.btnAddDiagramNote, "btnAddDiagramNote");
            this.btnAddDiagramNote.Name = "btnAddDiagramNote";
            this.toolTip.SetToolTip(this.btnAddDiagramNote, resources.GetString("btnAddDiagramNote.ToolTip"));
            this.btnAddDiagramNote.UseVisualStyleBackColor = true;
            this.btnAddDiagramNote.Click += new System.EventHandler(this.btnAddDiagramNote_Click);
            // 
            // btnAddElementNote
            // 
            resources.ApplyResources(this.btnAddElementNote, "btnAddElementNote");
            this.btnAddElementNote.Name = "btnAddElementNote";
            this.toolTip.SetToolTip(this.btnAddElementNote, resources.GetString("btnAddElementNote.ToolTip"));
            this.btnAddElementNote.UseVisualStyleBackColor = true;
            this.btnAddElementNote.Click += new System.EventHandler(this.btnAddElementNote_Click);
            // 
            // btnLocateOperation
            // 
            resources.ApplyResources(this.btnLocateOperation, "btnLocateOperation");
            this.btnLocateOperation.Name = "btnLocateOperation";
            this.toolTip.SetToolTip(this.btnLocateOperation, resources.GetString("btnLocateOperation.ToolTip"));
            this.btnLocateOperation.UseVisualStyleBackColor = true;
            this.btnLocateOperation.Click += new System.EventHandler(this.btnLocateOperation_Click);
            // 
            // btnDisplayBehavior
            // 
            resources.ApplyResources(this.btnDisplayBehavior, "btnDisplayBehavior");
            this.btnDisplayBehavior.Name = "btnDisplayBehavior";
            this.toolTip.SetToolTip(this.btnDisplayBehavior, resources.GetString("btnDisplayBehavior.ToolTip"));
            this.btnDisplayBehavior.UseVisualStyleBackColor = true;
            this.btnDisplayBehavior.Click += new System.EventHandler(this.btnDisplayBehavior_Click);
            // 
            // btnOS
            // 
            resources.ApplyResources(this.btnOS, "btnOS");
            this.btnOS.Name = "btnOS";
            this.toolTip.SetToolTip(this.btnOS, resources.GetString("btnOS.ToolTip"));
            this.btnOS.UseVisualStyleBackColor = true;
            this.btnOS.Click += new System.EventHandler(this.btnOS_Click);
            // 
            // btnTV
            // 
            resources.ApplyResources(this.btnTV, "btnTV");
            this.btnTV.Name = "btnTV";
            this.toolTip.SetToolTip(this.btnTV, resources.GetString("btnTV.ToolTip"));
            this.btnTV.UseVisualStyleBackColor = true;
            this.btnTV.Click += new System.EventHandler(this.btnTV_Click);
            // 
            // btnTH
            // 
            resources.ApplyResources(this.btnTH, "btnTH");
            this.btnTH.Name = "btnTH";
            this.toolTip.SetToolTip(this.btnTH, resources.GetString("btnTH.ToolTip"));
            this.btnTH.UseVisualStyleBackColor = true;
            this.btnTH.Click += new System.EventHandler(this.btnTH_Click);
            // 
            // btnLV
            // 
            resources.ApplyResources(this.btnLV, "btnLV");
            this.btnLV.Name = "btnLV";
            this.toolTip.SetToolTip(this.btnLV, resources.GetString("btnLV.ToolTip"));
            this.btnLV.UseVisualStyleBackColor = true;
            this.btnLV.Click += new System.EventHandler(this.btnLV_Click);
            // 
            // btnLH
            // 
            resources.ApplyResources(this.btnLH, "btnLH");
            this.btnLH.Name = "btnLH";
            this.toolTip.SetToolTip(this.btnLH, resources.GetString("btnLH.ToolTip"));
            this.btnLH.UseVisualStyleBackColor = true;
            this.btnLH.Click += new System.EventHandler(this.btnLH_Click);
            // 
            // txtSearchName
            // 
            this.txtSearchName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtSearchName, "txtSearchName");
            this.txtSearchName.Name = "txtSearchName";
            this.toolTip.SetToolTip(this.txtSearchName, resources.GetString("txtSearchName.ToolTip"));
            this.txtSearchName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtSearchName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtSearchName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSearchName_MouseDoubleClick);
            // 
            // btnConveyedItemConnector
            // 
            resources.ApplyResources(this.btnConveyedItemConnector, "btnConveyedItemConnector");
            this.btnConveyedItemConnector.Name = "btnConveyedItemConnector";
            this.toolTip.SetToolTip(this.btnConveyedItemConnector, resources.GetString("btnConveyedItemConnector.ToolTip"));
            this.btnConveyedItemConnector.UseVisualStyleBackColor = true;
            this.btnConveyedItemConnector.Click += new System.EventHandler(this.btnConveyedItemConnector_Click);
            // 
            // btnConveyedItemElement
            // 
            resources.ApplyResources(this.btnConveyedItemElement, "btnConveyedItemElement");
            this.btnConveyedItemElement.Name = "btnConveyedItemElement";
            this.toolTip.SetToolTip(this.btnConveyedItemElement, resources.GetString("btnConveyedItemElement.ToolTip"));
            this.btnConveyedItemElement.UseVisualStyleBackColor = true;
            this.btnConveyedItemElement.Click += new System.EventHandler(this.btnConveyedItemElement_Click);
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
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingGeneralToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.settingsToolbarToolStripMenuItem,
            this.settingsQueryAndSctipToolStripMenuItem,
            this.settingsGlobalKeysToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // settingGeneralToolStripMenuItem
            // 
            this.settingGeneralToolStripMenuItem.Name = "settingGeneralToolStripMenuItem";
            resources.ApplyResources(this.settingGeneralToolStripMenuItem, "settingGeneralToolStripMenuItem");
            this.settingGeneralToolStripMenuItem.Click += new System.EventHandler(this.settingGeneralToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // settingsToolbarToolStripMenuItem
            // 
            this.settingsToolbarToolStripMenuItem.Name = "settingsToolbarToolStripMenuItem";
            resources.ApplyResources(this.settingsToolbarToolStripMenuItem, "settingsToolbarToolStripMenuItem");
            this.settingsToolbarToolStripMenuItem.Click += new System.EventHandler(this.settingsToolbarToolStripMenuItem_Click);
            // 
            // settingsQueryAndSctipToolStripMenuItem
            // 
            this.settingsQueryAndSctipToolStripMenuItem.Name = "settingsQueryAndSctipToolStripMenuItem";
            resources.ApplyResources(this.settingsQueryAndSctipToolStripMenuItem, "settingsQueryAndSctipToolStripMenuItem");
            this.settingsQueryAndSctipToolStripMenuItem.Click += new System.EventHandler(this.settingsQueryAndSctipToolStripMenuItem_Click);
            // 
            // settingsGlobalKeysToolStripMenuItem
            // 
            this.settingsGlobalKeysToolStripMenuItem.Name = "settingsGlobalKeysToolStripMenuItem";
            resources.ApplyResources(this.settingsGlobalKeysToolStripMenuItem, "settingsGlobalKeysToolStripMenuItem");
            this.settingsGlobalKeysToolStripMenuItem.Click += new System.EventHandler(this.settingsKeysToolStripMenuItem_Click);
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
            resources.ApplyResources(this.doToolStripMenuItem, "doToolStripMenuItem");
            // 
            // createActivityForOperationToolStripMenuItem
            // 
            this.createActivityForOperationToolStripMenuItem.Name = "createActivityForOperationToolStripMenuItem";
            resources.ApplyResources(this.createActivityForOperationToolStripMenuItem, "createActivityForOperationToolStripMenuItem");
            this.createActivityForOperationToolStripMenuItem.Click += new System.EventHandler(this.createActivityForOperationToolStripMenuItem_Click);
            // 
            // showFolderToolStripMenuItem
            // 
            this.showFolderToolStripMenuItem.Name = "showFolderToolStripMenuItem";
            resources.ApplyResources(this.showFolderToolStripMenuItem, "showFolderToolStripMenuItem");
            this.showFolderToolStripMenuItem.Click += new System.EventHandler(this.showFolderToolStripMenuItem_Click);
            // 
            // copyGUIDSQLToClipboardToolStripMenuItem
            // 
            this.copyGUIDSQLToClipboardToolStripMenuItem.Name = "copyGUIDSQLToClipboardToolStripMenuItem";
            resources.ApplyResources(this.copyGUIDSQLToClipboardToolStripMenuItem, "copyGUIDSQLToClipboardToolStripMenuItem");
            this.copyGUIDSQLToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyGUIDSQLToClipboardToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // changeAuthorToolStripMenuItem
            // 
            this.changeAuthorToolStripMenuItem.Name = "changeAuthorToolStripMenuItem";
            resources.ApplyResources(this.changeAuthorToolStripMenuItem, "changeAuthorToolStripMenuItem");
            this.changeAuthorToolStripMenuItem.Click += new System.EventHandler(this.changeAuthorToolStripMenuItem_Click);
            // 
            // changeAuthorRecursiveToolStripMenuItem
            // 
            this.changeAuthorRecursiveToolStripMenuItem.Name = "changeAuthorRecursiveToolStripMenuItem";
            resources.ApplyResources(this.changeAuthorRecursiveToolStripMenuItem, "changeAuthorRecursiveToolStripMenuItem");
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
            resources.ApplyResources(this.versionControlToolStripMenuItem, "versionControlToolStripMenuItem");
            // 
            // changeXMLFileToolStripMenuItem
            // 
            this.changeXMLFileToolStripMenuItem.Name = "changeXMLFileToolStripMenuItem";
            resources.ApplyResources(this.changeXMLFileToolStripMenuItem, "changeXMLFileToolStripMenuItem");
            this.changeXMLFileToolStripMenuItem.Click += new System.EventHandler(this.changeXMLFileToolStripMenuItem_Click);
            // 
            // showFolderVCorCodeToolStripMenuItem
            // 
            this.showFolderVCorCodeToolStripMenuItem.Name = "showFolderVCorCodeToolStripMenuItem";
            resources.ApplyResources(this.showFolderVCorCodeToolStripMenuItem, "showFolderVCorCodeToolStripMenuItem");
            this.showFolderVCorCodeToolStripMenuItem.Click += new System.EventHandler(this.showFolderVCorCodeToolStripMenuItem_Click);
            // 
            // getVCLatesrecursiveToolStripMenuItem
            // 
            this.getVCLatesrecursiveToolStripMenuItem.Name = "getVCLatesrecursiveToolStripMenuItem";
            resources.ApplyResources(this.getVCLatesrecursiveToolStripMenuItem, "getVCLatesrecursiveToolStripMenuItem");
            this.getVCLatesrecursiveToolStripMenuItem.Click += new System.EventHandler(this.getVCLatesrecursiveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // showTortoiseLogToolStripMenuItem
            // 
            this.showTortoiseLogToolStripMenuItem.Name = "showTortoiseLogToolStripMenuItem";
            resources.ApplyResources(this.showTortoiseLogToolStripMenuItem, "showTortoiseLogToolStripMenuItem");
            this.showTortoiseLogToolStripMenuItem.Click += new System.EventHandler(this.showTortoiseLogToolStripMenuItem_Click);
            // 
            // showTortoiseRepoBrowserToolStripMenuItem
            // 
            this.showTortoiseRepoBrowserToolStripMenuItem.Name = "showTortoiseRepoBrowserToolStripMenuItem";
            resources.ApplyResources(this.showTortoiseRepoBrowserToolStripMenuItem, "showTortoiseRepoBrowserToolStripMenuItem");
            this.showTortoiseRepoBrowserToolStripMenuItem.Click += new System.EventHandler(this.showTortoiseRepoBrowserToolStripMenuItem_Click);
            // 
            // setSvnKeywordsToolStripMenuItem
            // 
            this.setSvnKeywordsToolStripMenuItem.Name = "setSvnKeywordsToolStripMenuItem";
            resources.ApplyResources(this.setSvnKeywordsToolStripMenuItem, "setSvnKeywordsToolStripMenuItem");
            this.setSvnKeywordsToolStripMenuItem.Click += new System.EventHandler(this.setSvnKeywordsToolStripMenuItem_Click);
            // 
            // setSvnModuleTaggedValuesToolStripMenuItem
            // 
            this.setSvnModuleTaggedValuesToolStripMenuItem.Name = "setSvnModuleTaggedValuesToolStripMenuItem";
            resources.ApplyResources(this.setSvnModuleTaggedValuesToolStripMenuItem, "setSvnModuleTaggedValuesToolStripMenuItem");
            this.setSvnModuleTaggedValuesToolStripMenuItem.Click += new System.EventHandler(this.setSvnModuleTaggedValuesToolStripMenuItem_Click);
            // 
            // setSvnModuleTaggedValuesrecursiveToolStripMenuItem
            // 
            this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Name = "setSvnModuleTaggedValuesrecursiveToolStripMenuItem";
            resources.ApplyResources(this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem, "setSvnModuleTaggedValuesrecursiveToolStripMenuItem");
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
            resources.ApplyResources(this.portToolStripMenuItem, "portToolStripMenuItem");
            // 
            // movePortsToolStripMenuItem
            // 
            this.movePortsToolStripMenuItem.Name = "movePortsToolStripMenuItem";
            resources.ApplyResources(this.movePortsToolStripMenuItem, "movePortsToolStripMenuItem");
            this.movePortsToolStripMenuItem.Click += new System.EventHandler(this.copyPortsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // deletePortsToolStripMenuItem
            // 
            this.deletePortsToolStripMenuItem.Name = "deletePortsToolStripMenuItem";
            resources.ApplyResources(this.deletePortsToolStripMenuItem, "deletePortsToolStripMenuItem");
            this.deletePortsToolStripMenuItem.Click += new System.EventHandler(this.deletePortsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // connectPortsToolStripMenuItem
            // 
            this.connectPortsToolStripMenuItem.Name = "connectPortsToolStripMenuItem";
            resources.ApplyResources(this.connectPortsToolStripMenuItem, "connectPortsToolStripMenuItem");
            this.connectPortsToolStripMenuItem.Click += new System.EventHandler(this.connectPortsToolStripMenuItem_Click);
            // 
            // connectPortsInsideComponentsToolStripMenuItem
            // 
            this.connectPortsInsideComponentsToolStripMenuItem.Name = "connectPortsInsideComponentsToolStripMenuItem";
            resources.ApplyResources(this.connectPortsInsideComponentsToolStripMenuItem, "connectPortsInsideComponentsToolStripMenuItem");
            this.connectPortsInsideComponentsToolStripMenuItem.Click += new System.EventHandler(this.connectPortsInsideComponentsToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // makeConnectorsUnspecifiedDirectionToolStripMenuItem
            // 
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.Name = "makeConnectorsUnspecifiedDirectionToolStripMenuItem";
            resources.ApplyResources(this.makeConnectorsUnspecifiedDirectionToolStripMenuItem, "makeConnectorsUnspecifiedDirectionToolStripMenuItem");
            this.makeConnectorsUnspecifiedDirectionToolStripMenuItem.Click += new System.EventHandler(this.makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // showPortsInDiagramObjectsToolStripMenuItem
            // 
            this.showPortsInDiagramObjectsToolStripMenuItem.Name = "showPortsInDiagramObjectsToolStripMenuItem";
            resources.ApplyResources(this.showPortsInDiagramObjectsToolStripMenuItem, "showPortsInDiagramObjectsToolStripMenuItem");
            this.showPortsInDiagramObjectsToolStripMenuItem.Click += new System.EventHandler(this.showPortsInDiagramObjectsToolStripMenuItem_Click);
            // 
            // showSendingPortsLeftRecievingPortsRightToolStripMenuItem
            // 
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Name = "showSendingPortsLeftRecievingPortsRightToolStripMenuItem";
            resources.ApplyResources(this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem, "showSendingPortsLeftRecievingPortsRightToolStripMenuItem");
            this.showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Click += new System.EventHandler(this.showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click);
            // 
            // hidePortsInDiagramToolStripMenuItem
            // 
            this.hidePortsInDiagramToolStripMenuItem.Name = "hidePortsInDiagramToolStripMenuItem";
            resources.ApplyResources(this.hidePortsInDiagramToolStripMenuItem, "hidePortsInDiagramToolStripMenuItem");
            this.hidePortsInDiagramToolStripMenuItem.Click += new System.EventHandler(this.removePortsInDiagramToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // unhidePortsToolStripMenuItem
            // 
            this.unhidePortsToolStripMenuItem.Name = "unhidePortsToolStripMenuItem";
            resources.ApplyResources(this.unhidePortsToolStripMenuItem, "unhidePortsToolStripMenuItem");
            this.unhidePortsToolStripMenuItem.Click += new System.EventHandler(this.viewPortLabelToolStripMenuItem_Click);
            // 
            // hidePortsToolStripMenuItem
            // 
            this.hidePortsToolStripMenuItem.Name = "hidePortsToolStripMenuItem";
            resources.ApplyResources(this.hidePortsToolStripMenuItem, "hidePortsToolStripMenuItem");
            this.hidePortsToolStripMenuItem.Click += new System.EventHandler(this.hidePortLabelToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // movePortLabelLeftToolStripMenuItem
            // 
            this.movePortLabelLeftToolStripMenuItem.Name = "movePortLabelLeftToolStripMenuItem";
            resources.ApplyResources(this.movePortLabelLeftToolStripMenuItem, "movePortLabelLeftToolStripMenuItem");
            this.movePortLabelLeftToolStripMenuItem.Click += new System.EventHandler(this.movePortLableLeftPositionToolStripMenuItem_Click);
            // 
            // movePortLabelRightPositionToolStripMenuItem
            // 
            this.movePortLabelRightPositionToolStripMenuItem.Name = "movePortLabelRightPositionToolStripMenuItem";
            resources.ApplyResources(this.movePortLabelRightPositionToolStripMenuItem, "movePortLabelRightPositionToolStripMenuItem");
            this.movePortLabelRightPositionToolStripMenuItem.Click += new System.EventHandler(this.movePortLableRightPositionToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // movePortLabelLeftToolStripMenuItem1
            // 
            this.movePortLabelLeftToolStripMenuItem1.Name = "movePortLabelLeftToolStripMenuItem1";
            resources.ApplyResources(this.movePortLabelLeftToolStripMenuItem1, "movePortLabelLeftToolStripMenuItem1");
            this.movePortLabelLeftToolStripMenuItem1.Click += new System.EventHandler(this.movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // movePortLabelRightToolStripMenuItem
            // 
            this.movePortLabelRightToolStripMenuItem.Name = "movePortLabelRightToolStripMenuItem";
            resources.ApplyResources(this.movePortLabelRightToolStripMenuItem, "movePortLabelRightToolStripMenuItem");
            this.movePortLabelRightToolStripMenuItem.Click += new System.EventHandler(this.movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // orderDiagramItemsToolStripMenuItem
            // 
            this.orderDiagramItemsToolStripMenuItem.Name = "orderDiagramItemsToolStripMenuItem";
            resources.ApplyResources(this.orderDiagramItemsToolStripMenuItem, "orderDiagramItemsToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            resources.ApplyResources(this.helpToolStripMenuItem1, "helpToolStripMenuItem1");
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.toolStripContainer1);
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.Name = "panelButtons";
            // 
            // panelLineStyle
            // 
            this.panelLineStyle.Controls.Add(this.btnLV);
            this.panelLineStyle.Controls.Add(this.btnLH);
            this.panelLineStyle.Controls.Add(this.btnTV);
            this.panelLineStyle.Controls.Add(this.btnTH);
            this.panelLineStyle.Controls.Add(this.btnC);
            this.panelLineStyle.Controls.Add(this.btnBezier);
            this.panelLineStyle.Controls.Add(this.btnOS);
            this.panelLineStyle.Controls.Add(this.btnOR);
            this.panelLineStyle.Controls.Add(this.btnA);
            this.panelLineStyle.Controls.Add(this.btnD);
            resources.ApplyResources(this.panelLineStyle, "panelLineStyle");
            this.panelLineStyle.Name = "panelLineStyle";
            // 
            // panelFavorite
            // 
            this.panelFavorite.Controls.Add(this.btnAddFavorite);
            this.panelFavorite.Controls.Add(this.btnRemoveFavorite);
            this.panelFavorite.Controls.Add(this.btnShowFavorites);
            this.panelFavorite.Controls.Add(this.btnDisplayBehavior);
            resources.ApplyResources(this.panelFavorite, "panelFavorite");
            this.panelFavorite.Name = "panelFavorite";
            // 
            // panelNote
            // 
            this.panelNote.Controls.Add(this.btnAddElementNote);
            this.panelNote.Controls.Add(this.btnAddDiagramNote);
            resources.ApplyResources(this.panelNote, "panelNote");
            this.panelNote.Name = "panelNote";
            // 
            // panelPort
            // 
            this.panelPort.Controls.Add(this.lblPorts);
            this.panelPort.Controls.Add(this.btnLeft);
            this.panelPort.Controls.Add(this.btnUp);
            this.panelPort.Controls.Add(this.btnRight);
            this.panelPort.Controls.Add(this.btnDown);
            this.panelPort.Controls.Add(this.btnLabelLeft);
            this.panelPort.Controls.Add(this.btnLabelRight);
            resources.ApplyResources(this.panelPort, "panelPort");
            this.panelPort.Name = "panelPort";
            // 
            // lblPorts
            // 
            resources.ApplyResources(this.lblPorts, "lblPorts");
            this.lblPorts.Name = "lblPorts";
            // 
            // panelAdvanced
            // 
            this.panelAdvanced.Controls.Add(this.btnComposite);
            this.panelAdvanced.Controls.Add(this.btnFindUsage);
            this.panelAdvanced.Controls.Add(this.btnUpdateActivityParameter);
            this.panelAdvanced.Controls.Add(this.btnLocateType);
            this.panelAdvanced.Controls.Add(this.btnLocateOperation);
            this.panelAdvanced.Controls.Add(this.btnDisplaySpecification);
            resources.ApplyResources(this.panelAdvanced, "panelAdvanced");
            this.panelAdvanced.Name = "panelAdvanced";
            // 
            // panelConveyedItems
            // 
            this.panelConveyedItems.Controls.Add(this.lblConveyedItems);
            this.panelConveyedItems.Controls.Add(this.btnConveyedItemElement);
            this.panelConveyedItems.Controls.Add(this.btnConveyedItemConnector);
            resources.ApplyResources(this.panelConveyedItems, "panelConveyedItems");
            this.panelConveyedItems.Name = "panelConveyedItems";
            // 
            // lblConveyedItems
            // 
            resources.ApplyResources(this.lblConveyedItems, "lblConveyedItems");
            this.lblConveyedItems.Name = "lblConveyedItems";
            // 
            // panelQuickSearch
            // 
            resources.ApplyResources(this.panelQuickSearch, "panelQuickSearch");
            this.panelQuickSearch.Controls.Add(this.txtSearchName, 0, 0);
            this.panelQuickSearch.Controls.Add(this.txtSearchText, 0, 0);
            this.panelQuickSearch.Name = "panelQuickSearch";
            // 
            // AddinControlGUI
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelPort);
            this.Controls.Add(this.panelNote);
            this.Controls.Add(this.panelAdvanced);
            this.Controls.Add(this.panelFavorite);
            this.Controls.Add(this.panelConveyedItems);
            this.Controls.Add(this.panelLineStyle);
            this.Controls.Add(this.panelQuickSearch);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.menuStrip1);
            this.Name = "AddinControlGUI";
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripQuery.ResumeLayout(false);
            this.toolStripQuery.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelLineStyle.ResumeLayout(false);
            this.panelFavorite.ResumeLayout(false);
            this.panelNote.ResumeLayout(false);
            this.panelPort.ResumeLayout(false);
            this.panelPort.PerformLayout();
            this.panelAdvanced.ResumeLayout(false);
            this.panelConveyedItems.ResumeLayout(false);
            this.panelConveyedItems.PerformLayout();
            this.panelQuickSearch.ResumeLayout(false);
            this.panelQuickSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region parameterizeMenusAndButtons
        /// <summary>
        /// Parameterize menu and buttons to visible/hidden due to
        /// - isAdvanced
        /// - isSvnSupported
        /// - isVcSupported
        /// </summary>
        public void parameterizeMenusAndButtons()
        {
            // Don't change the order
            panelPort.Visible = false;
            panelNote.Visible = false;
            panelAdvanced.Visible = false;
            panelFavorite.Visible = false;
            panelConveyedItems.Visible = false;
            panelLineStyle.Visible = false;
            panelQuickSearch.Visible = false;
            panelButtons.Visible = false;



            panelPort.ResumeLayout(false);
            panelPort.PerformLayout();
            panelNote.ResumeLayout(false);
            panelNote.PerformLayout();
            panelAdvanced.ResumeLayout(false);
            panelAdvanced.PerformLayout();
            panelFavorite.ResumeLayout(false);
            panelFavorite.PerformLayout();
            panelConveyedItems.ResumeLayout(false);
            panelConveyedItems.PerformLayout();
            panelLineStyle.ResumeLayout(false);
            panelLineStyle.PerformLayout();
            panelQuickSearch.ResumeLayout(false);
            panelQuickSearch.PerformLayout();
            panelButtons.ResumeLayout(false);
            panelButtons.PerformLayout();





            this.panelAdvanced.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            // Don't change the order
            panelPort.Visible = false;
            panelNote.Visible = false;
            panelAdvanced.Visible = false;
            panelFavorite.Visible = false;
            panelConveyedItems.Visible = false;
            panelLineStyle.Visible = false;
            panelQuickSearch.Visible = false;
            panelButtons.Visible = false;


            // Port
            panelPort.Visible = AddinSettings.isAdvancedPort;
            panelNote.Visible = AddinSettings.isAdvancedDiagramNote;
            lblPorts.Visible = AddinSettings.isAdvancedPort;


            // Advanced
            panelAdvanced.Visible = AddinSettings.isAdvancedFeatures;

            // Advanced Features
            btnDisplayBehavior.Visible = AddinSettings.isAdvancedFeatures;
            btnDisplaySpecification.Visible = AddinSettings.isAdvancedFeatures;
            btnUpdateActivityParameter.Visible = AddinSettings.isAdvancedFeatures;
            btnLocateOperation.Visible = AddinSettings.isAdvancedFeatures;
            btnFindUsage.Visible = AddinSettings.isAdvancedFeatures;
            btnLocateType.Visible = AddinSettings.isAdvancedFeatures;
            btnComposite.Visible = AddinSettings.isAdvancedFeatures;

            // Favorite
            panelFavorite.Visible = AddinSettings.isFavoriteSupport || AddinSettings.isAdvancedFeatures;
            btnAddFavorite.Visible = AddinSettings.isFavoriteSupport;
            btnRemoveFavorite.Visible = AddinSettings.isFavoriteSupport;
            btnShowFavorites.Visible = AddinSettings.isFavoriteSupport;

            // Conveyed Item support
            panelConveyedItems.Visible = AddinSettings.isConveyedItemsSupport;
            btnConveyedItemConnector.Visible = AddinSettings.isConveyedItemsSupport;
            btnConveyedItemElement.Visible = AddinSettings.isConveyedItemsSupport;
            lblConveyedItems.Visible = true;

            // Line style Panel
            panelLineStyle.Visible = AddinSettings.isLineStyleSupport;

            // no quick search defined
            panelQuickSearch.Visible = (AddinSettings.quickSearchName.Trim() != "");
            txtSearchName.Text = AddinSettings.quickSearchName.Trim();

            // Buttons for queries and services
            panelButtons.Visible = AddinSettings.isShowQueryButton || AddinSettings.isShowServiceButton;


            // SVN support
            bool visibleSvnVC = true && !(AddinSettings.isSvnSupport == false | AddinSettings.isVcSupport == false);
            showTortoiseRepoBrowserToolStripMenuItem.Visible = visibleSvnVC;
            showTortoiseLogToolStripMenuItem.Visible = visibleSvnVC;
            setSvnModuleTaggedValuesToolStripMenuItem.Visible = visibleSvnVC;
            setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Visible = visibleSvnVC;
            setSvnKeywordsToolStripMenuItem.Visible = visibleSvnVC;

            // Visible VC
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

            // LineStyle
            btnLV.Visible = AddinSettings.isLineStyleSupport;
            btnLH.Visible = AddinSettings.isLineStyleSupport;
            btnTV.Visible = AddinSettings.isLineStyleSupport;
            btnTH.Visible = AddinSettings.isLineStyleSupport;
            btnC.Visible = AddinSettings.isLineStyleSupport;
            btnBezier.Visible = AddinSettings.isLineStyleSupport;
            btnOS.Visible = AddinSettings.isLineStyleSupport;
            btnOR.Visible = AddinSettings.isLineStyleSupport;
            btnA.Visible = AddinSettings.isLineStyleSupport;
            btnD.Visible = AddinSettings.isLineStyleSupport;

            // Conveyed Items support
            btnConveyedItemConnector.Visible = AddinSettings.isConveyedItemsSupport;
            btnConveyedItemElement.Visible = AddinSettings.isConveyedItemsSupport;

            // Favorite
            btnAddFavorite.Visible = AddinSettings.isFavoriteSupport;
            btnRemoveFavorite.Visible = AddinSettings.isFavoriteSupport;
            btnShowFavorites.Visible = AddinSettings.isFavoriteSupport;

            // Advance features
            btnDisplayBehavior.Visible = AddinSettings.isAdvancedFeatures;

            //boolean visibleDiagramNote = false || _addinSettings.isAdvancedDiagramNote;


        }
        #endregion
        #region parameterizeSearchButtons
        /// <summary>
        /// Parametrize 5 quick buttons for search with:
        /// <para/>- Search Name
        /// <para/>- Search Tooltip
        /// </summary>
        public void parameterizeSearchButton()
        {
            toolStripSearchBtn1.Visible = AddinSettings.isShowQueryButton;
            toolStripSearchBtn2.Visible = AddinSettings.isShowQueryButton;
            toolStripSearchBtn3.Visible = AddinSettings.isShowQueryButton;
            toolStripSearchBtn4.Visible = AddinSettings.isShowQueryButton;
            toolStripSearchBtn5.Visible = AddinSettings.isShowQueryButton;

            for (int pos = 0; pos < AddinSettings.buttonsSearch.Length; pos++)
            {
                const string defaultHelptext = "Free Model Searches, Model Search not parametrized.";
                string buttonText = "";
                string helpText = defaultHelptext;
                if (AddinSettings.buttonsSearch[pos] != null)
                {
                    EaAddinButtons search = AddinSettings.buttonsSearch[pos];
                    {
                        if (search.keyText.Trim() != "")
                        {
                            buttonText = search.keyText;
                            helpText = search.keySearchTooltip;
                        }
                    }
                }
                
                switch (pos)
                {
                    case 0:
                        toolStripSearchBtn1.Text = buttonText;
                        toolStripSearchBtn1.ToolTipText = helpText;
                        break;
                    case 1:
                        toolStripSearchBtn2.Text = buttonText;
                        toolStripSearchBtn2.ToolTipText = helpText;
                        break;
                    case 2:
                        toolStripSearchBtn3.Text = buttonText;
                        toolStripSearchBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        toolStripSearchBtn4.Text = buttonText;
                        toolStripSearchBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        toolStripSearchBtn5.Text = buttonText;
                        toolStripSearchBtn5.ToolTipText = helpText;
                        break;

                }
            }
        }
        #endregion
        #region parameterizeServiceButton
        public void parameterizeServiceButton()
        {
            toolStripServiceBtn1.Visible = AddinSettings.isShowServiceButton;
            toolStripServiceBtn2.Visible = AddinSettings.isShowServiceButton;
            toolStripServiceBtn3.Visible = AddinSettings.isShowServiceButton;
            toolStripServiceBtn4.Visible = AddinSettings.isShowServiceButton;
            toolStripServiceBtn5.Visible = AddinSettings.isShowServiceButton;
            for (int pos = 0; pos < AddinSettings.buttonsServices.Count; pos++)
            {
                string buttonText= "";
                string helpText = "free Service, Service not parametrized";
                if (AddinSettings.buttonsServices[pos] != null)
                {
                    ServicesCallConfig service = AddinSettings.buttonsServices[pos];
                    if (service.ButtonText.Trim() != "")
                    {
                        buttonText = service.ButtonText;
                        helpText = service.HelpTextLong; //  Long Help text
                    }
                }

                switch (pos)
                {
                    case 0:
                        toolStripServiceBtn1.Text = buttonText;
                        toolStripServiceBtn1.ToolTipText = helpText;
                        break;
                    case 1:
                        toolStripServiceBtn2.Text = buttonText;
                        toolStripServiceBtn2.ToolTipText = helpText;
                        break;
                    case 2:
                        toolStripServiceBtn3.Text = buttonText; ;
                        toolStripServiceBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        toolStripServiceBtn4.Text = buttonText; ;
                        toolStripServiceBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        toolStripServiceBtn5.Text = buttonText; ;
                        toolStripServiceBtn5.ToolTipText = helpText;
                        break;

                }
            }
        }
        #endregion

         
        /// <summary>
        /// Open Query and Scripts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void settingsQueryAndSctipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmQueryAndScript = new FrmQueryAndScript(AddinSettings);
            _frmQueryAndScript.ShowDialog();
        }

        void settingGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsGeneral = new FrmSettingsGeneral(AddinSettings, this);
            _frmSettingsGeneral.ShowDialog();

        }

        void settingsToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsToolbar = new FrmSettingsToolbar(AddinSettings, this);
            _frmSettingsToolbar.ShowDialog();
        }
        void settingsKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsKey = new FrmSettingsKey(AddinSettings, this);
            _frmSettingsKey.ShowDialog();
        }

        /// <summary>
        /// Get Connectors for selected Element
        /// Run search with selected Element
        /// - Lists Elements which are source of connectors
        /// - Locate Diagram get you to the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConveyedItemConnector_Click(object sender, EventArgs e)
        {
            EA.ObjectType type = Model.Repository.GetContextItemType();
            if (type == EA.ObjectType.otElement)
            {
                string sql = @"
                        select  s.ea_guid AS CLASSGUID, s.object_type AS CLASSTYPE, s.name As Source , d.name As Destination
                        from t_xref x,   // a lot of things like properties,..
                             t_connector c,
                             t_object s, // Source element
                             t_object d  // destination element

                        where  x.description like  '#WC##CurrentItemGUID##WC#' AND
                               x.Behavior = 'conveyed' AND
                               c.ea_guid = x.client    

                        and    c.ea_guid = x.client
                        and    c.start_object_id = s.object_id
                        and    c.end_object_id = d.object_id
                        ORDER BY 3,4
                ";
                // Run SQL with macro replacement
                Model.SQLRun(sql, "");
               
            } else
            {
                MessageBox.Show("To get the connectors which convey Elements you have to select an Element.", "No Element is selected, break!!!");
            }

        }
        /// <summary>
        /// Get Elements for select Connector
        /// Run search with selected Connector
        /// - Lists Elements which have Conveyed Elements on Connector
        /// - Locate Element 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConveyedItemElement_Click(object sender, EventArgs e)
        {
            EA.ObjectType type = Model.Repository.GetContextItemType();
            if (type == EA.ObjectType.otConnector)
            {
                string sql = @"
                        select  o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE, o.name As Element
                        from t_object o,
                        where  o.element_id in ( #ConveyedItemsIDS# )
                        ORDER BY 3
                ";
                // Run SQL with macro replacement
                Model.SQLRun(sql, "");

        } else
            {
                MessageBox.Show("To get the Elements on the Connector you have to select an Connector.", "No Connector is selected, break!!!");
            }
}
    }
}