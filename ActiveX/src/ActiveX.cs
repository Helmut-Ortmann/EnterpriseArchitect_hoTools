using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Control.EaAddinShortcuts;
using EA;
using hoTools.EaServices;
using hoTools.EAServicesPort;
using hoTools.Settings;
using hoTools.Settings.Key;
using hoTools.Settings.Toolbar;
using hoTools.Utils;
using hoTools.Utils.SQL;
using Package = EA.Package;

namespace hoTools.ActiveX
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("82A06E9C-7568-4E4B-8D2C-A53B8D9A7272")]
    [ProgId("hoTools.ActiveXGUI")]
    [ComDefaultInterface(typeof(IAddinControl))]

    public class AddinControlGui : AddinGui, IAddinControl
    {
        public const string Progid = "hoTools.ActiveXGUI";

        // Windows/Frames
        FrmQueryAndScript _frmQueryAndScript;
        FrmSettingsGeneral _frmSettingsGeneral;

        FrmSettingsToolbar _frmSettingsToolbar;
        FrmSettingsKey _frmSettingsKey;
        FrmSettingsLineStyle _frmSettingsLineStyle;


        #region Generated

        private Button _btnLh;
        private Button _btnLv;
        private Button _btnTv;
        public Button BtnTh;
        private Button _btnOs;
        private ToolTip _toolTip;
        private IContainer components;
        private Button _btnDisplayBehavior;
        private Button _btnLocateOperation;
        private Button _btnAddElementNote;
        private Button _btnAddDiagramNote;
        private Button _btnLocateType;
        private Button _btnFindUsage;
        private Button _btnDisplaySpecification;
        private Button _btnComposite;
        private Button _btnOr;
        private Button _btnA;
        private Button _btnD;
        private Button _btnC;
        private MenuStrip _menuStrip1;
        private ToolStripMenuItem _fileToolStripMenuItem;
        private ToolStripMenuItem _doToolStripMenuItem;
        private ToolStripMenuItem _createActivityForOperationToolStripMenuItem;
        private ToolStripMenuItem _showFolderToolStripMenuItem;
        private ToolStripMenuItem _helpToolStripMenuItem;
        private ToolStripMenuItem _copyGuidsqlToClipboardToolStripMenuItem;
        private ToolStripMenuItem _settingsToolStripMenuItem;
        private Button _btnUpdateActivityParameter;
        private ToolStripMenuItem _aboutToolStripMenuItem;
        private Button _btnBezier;
        private ToolStripMenuItem _versionControlToolStripMenuItem;
        private ToolStripMenuItem _changeXmlFileToolStripMenuItem;
        private ToolStripMenuItem _helpToolStripMenuItem1;
        private ToolStripContainer _toolStripContainer1;
        private ToolStrip _toolStripQuery;
        private ToolStripButton _toolStripSearchBtn1;
        private ToolStripButton _toolStripSearchBtn2;
        private ToolStripButton _toolStripSearchBtn3;
        private ToolStripButton _toolStripSearchBtn4;
        private ToolStripButton _toolStripSearchBtn5;
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripMenuItem _changeAuthorToolStripMenuItem;
        private ToolStripMenuItem _changeAuthorRecursiveToolStripMenuItem;
        private ToolStripMenuItem _getVcLatesrecursiveToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator2;
        private ToolStripMenuItem _showTortoiseLogToolStripMenuItem;
        private ToolStripMenuItem _showTortoiseRepoBrowserToolStripMenuItem;
        private ToolStripMenuItem _setSvnKeywordsToolStripMenuItem;
        private ToolStripMenuItem _setSvnModuleTaggedValuesToolStripMenuItem;
        private ToolStripMenuItem _setSvnModuleTaggedValuesrecursiveToolStripMenuItem;
        private ToolStripMenuItem _showFolderVCorCodeToolStripMenuItem;
        private Button _btnAddFavorite;
        private Button _btnRemoveFavorite;
        private Button _btnShowFavorites;
        private ToolStripMenuItem _portToolStripMenuItem;
        private ToolStripMenuItem _showPortsInDiagramObjectsToolStripMenuItem;
        private ToolStripMenuItem _movePortsToolStripMenuItem;
        private ToolStripMenuItem _hidePortsToolStripMenuItem;
        private ToolStripMenuItem _unhidePortsToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator3;
        private ToolStripMenuItem _connectPortsToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator4;
        private ToolStripMenuItem _deletePortsToolStripMenuItem;
        private Button _btnRight;
        private Button _btnLeft;
        private Button _btnUp;
        private Button _btnDown;
        private ToolStripMenuItem _hidePortsInDiagramToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator5;
        private ToolStripMenuItem _makeConnectorsUnspecifiedDirectionToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator7;
        private ToolStripMenuItem _connectPortsInsideComponentsToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator8;
        private ToolStripMenuItem _showSendingPortsLeftRecievingPortsRightToolStripMenuItem;
        private ToolStripMenuItem _movePortLabelLeftToolStripMenuItem;
        private ToolStripMenuItem _movePortLabelRightPositionToolStripMenuItem;
        private ToolStripMenuItem _movePortLabelLeftToolStripMenuItem1;
        private ToolStripMenuItem _movePortLabelRightToolStripMenuItem;
        private Button _btnLabelLeft;
        private Button _btnLabelRight;
        private ToolStripMenuItem _orderDiagramItemsToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator9;
        private ToolStripSeparator _toolStripSeparator11;
        private ToolStripSeparator _toolStripSeparator12;
        private ToolStripMenuItem _settingsQueryAndSctipToolStripMenuItem;
        private ToolStripMenuItem _settingGeneralToolStripMenuItem;
        private ToolStripMenuItem _settingsToolbarToolStripMenuItem;
        private Panel _panelButtons;
        private Panel _panelLineStyle;
        private Panel _panelFavorite;
        private Panel _panelNote;
        private Panel _panelPort;
        private Panel _panelAdvanced;
        private TextBox _txtSearchName;
        private Panel _panelConveyedItems;
        private Button _btnConveyedItemElement;
        private Button _btnConveyedItemConnector;
        private ToolStripSeparator _toolStripSeparator6;
        private ToolStripButton _toolStripServiceBtn1;
        private ToolStripButton _toolStripServiceBtn2;
        private ToolStripButton _toolStripServiceBtn3;
        private ToolStripButton _toolStripServiceBtn4;
        private ToolStripButton _toolStripServiceBtn5;
        private ToolStripMenuItem _settingsGlobalKeysToolStripMenuItem;
        private Label _lblPorts;
        private Label _lblConveyedItems;
        private TableLayoutPanel _panelQuickSearch;
        private ToolStripMenuItem _updateActivityFromOperationToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator10;
        private ToolStripMenuItem _getLastSqlErrorToolStripMenuItem;
        private TextBox _txtSearchText;
        #endregion

        #region Constructor
        public AddinControlGui()
        {
            InitializeComponent();

        }
        #endregion

        public string GetText() => _txtSearchText.Text;


        /// <summary>
        /// Repository. Make sure settings are updated before.
        /// </summary>
        public override Repository Repository
        {
            set
            {
                base.Repository = value;
                // only if there is a repository available
                if (Repository == null || value.ProjectGUID == "" ) return;

                try
                {
                    InitializeSettings();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "ActiveX: Error Initialization");
                }

            }
        }
        #region initializingSettings
        /// <summary>
        /// Initialize Setting (not Keys). Be sure Repository is loaded! Also don't change the sequence of hide/visible.
        /// </summary>
        public void InitializeSettings()
        {
            ParameterizeMenusAndButtons();
            // parameterize 5 Buttons to quickly run search
            ParameterizeSearchButton();
            // parameterize 5 Buttons to quickly run services
            ParameterizeServiceButton();
        }
        #endregion

        #region IActiveX Members
        public string GetName() => "hoTools.AddinControl";


        #endregion

        #region save
        public void Save()
        {
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

        void updateActivityFromOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.updateOperationTypes(Repository);
        }


        /// <summary>
        /// Show Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            _frmSettingsLineStyle = new FrmSettingsLineStyle(AddinSettings, this);
            _frmSettingsLineStyle.ShowDialog();
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

                case AddinSettings.CustomerCfg.Var1:
                    EaService.aboutVAR1(Release, configFilePath);
                    break;
                case AddinSettings.CustomerCfg.HoTools:
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
            RunService(0);
        }

        void toolStripBtn2_Click(object sender, EventArgs e)
        {
            RunService(1);
        }
        void toolStripBtn3_Click(object sender, EventArgs e)
        {
            RunService(2);
        }

        void toolStripBtn4_Click(object sender, EventArgs e)
        {
            RunService(3);
        }

        void toolStripBtn5_Click(object sender, EventArgs e)
        {
            RunService(4);
        }

        void toolStripBtn11_Click(object sender, EventArgs e)
        {
            RunSearch(0);
        }

        void toolStripBtn12_Click(object sender, EventArgs e)
        {
            RunSearch(1);
        }

        void toolStripBtn13_Click(object sender, EventArgs e)
        {
            RunSearch(2);
        }

        void toolStripBtn14_Click(object sender, EventArgs e)
        {
            RunSearch(3);
        }

        void toolStripBtn15_Click(object sender, EventArgs e)
        {
            RunSearch(4);
        }
        void RunSearch(int pos)
        {
            if (AddinSettings.ButtonsSearch[pos] is EaAddinShortcutSearch)
            {

                var sh = (EaAddinShortcutSearch)AddinSettings.ButtonsSearch[pos];
                if (sh.KeySearchName == "") return;
                try
                {
                    Repository.RunModelSearch(sh.KeySearchName, sh.KeySearchTerm, "", "");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error start search '" + sh.KeySearchName +
                       " " + sh.KeySearchTerm + "'");
                }
            }
        }
        void RunService(int pos)
        {
            if (AddinSettings.ButtonsServices[pos] is ServicesCallConfig)
            {

                var sh = AddinSettings.ButtonsServices[pos];
                if (sh.Method == null) return;
                sh.Invoke(Repository, _txtSearchText.Text);

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
            ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(ObjectType.otPackage)) return;

            var pkg = (Package)Repository.GetContextObject();
            EaService.gotoSvnLog(Repository, pkg);
        }

        void showTortoiseRepoBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(ObjectType.otPackage)) return;

            var pkg = (Package)Repository.GetContextObject();
            EaService.gotoSvnBrowser(Repository, pkg);
        }

        void getVCLatesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.getVcLatestRecursive(Repository);
        }

        void setSvnKeywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(ObjectType.otPackage)) return;

            var pkg = (Package)Repository.GetContextObject();
            EaService.setSvnProperty(Repository, pkg);
        }

        void setSvnModuleTaggedValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(ObjectType.otPackage)) return;

            var pkg = (Package)Repository.GetContextObject();
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

        /// <summary>
        /// Connect ports with the same name in a component / class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                EaService.runQuickSearch(Repository, GetSearchName(), _txtSearchText.Text);
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
            _txtSearchText.Text = Clipboard.GetText();
            EaService.runQuickSearch(Repository, GetSearchName(), _txtSearchText.Text);
        }

        /// <summary>
        /// Double Mouse Click in SearchName runs the query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtSearchName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EaService.runQuickSearch(Repository, GetSearchName(), _txtSearchText.Text);
        }

        /// <summary>
        /// Get Search Name from GUI text field. If empty use Search name from settings
        /// </summary>
        /// <returns></returns>
        string GetSearchName()
        {
            string searchName = _txtSearchName.Text.Trim();
            if (searchName == "")
            {
                searchName = AddinSettings.QuickSearchName;
                _txtSearchName.Text = searchName;
            }

            return searchName;
        }
        #endregion
        #endregion

        #region InitializeComponent
        void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AddinControlGui));
            _toolStripContainer1 = new ToolStripContainer();
            _toolStripQuery = new ToolStrip();
            _toolStripSearchBtn1 = new ToolStripButton();
            _toolStripSearchBtn2 = new ToolStripButton();
            _toolStripSearchBtn3 = new ToolStripButton();
            _toolStripSearchBtn4 = new ToolStripButton();
            _toolStripSearchBtn5 = new ToolStripButton();
            _toolStripSeparator6 = new ToolStripSeparator();
            _toolStripServiceBtn1 = new ToolStripButton();
            _toolStripServiceBtn2 = new ToolStripButton();
            _toolStripServiceBtn3 = new ToolStripButton();
            _toolStripServiceBtn4 = new ToolStripButton();
            _toolStripServiceBtn5 = new ToolStripButton();
            _toolTip = new ToolTip(components);
            _btnLabelRight = new Button();
            _btnLabelLeft = new Button();
            _btnUp = new Button();
            _btnDown = new Button();
            _btnLeft = new Button();
            _btnRight = new Button();
            _btnShowFavorites = new Button();
            _btnRemoveFavorite = new Button();
            _btnAddFavorite = new Button();
            _txtSearchText = new TextBox();
            _btnBezier = new Button();
            _btnUpdateActivityParameter = new Button();
            _btnC = new Button();
            _btnD = new Button();
            _btnA = new Button();
            _btnOr = new Button();
            _btnComposite = new Button();
            _btnDisplaySpecification = new Button();
            _btnFindUsage = new Button();
            _btnLocateType = new Button();
            _btnAddDiagramNote = new Button();
            _btnAddElementNote = new Button();
            _btnLocateOperation = new Button();
            _btnDisplayBehavior = new Button();
            _btnOs = new Button();
            _btnTv = new Button();
            BtnTh = new Button();
            _btnLv = new Button();
            _btnLh = new Button();
            _txtSearchName = new TextBox();
            _btnConveyedItemConnector = new Button();
            _btnConveyedItemElement = new Button();
            _menuStrip1 = new MenuStrip();
            _fileToolStripMenuItem = new ToolStripMenuItem();
            _settingGeneralToolStripMenuItem = new ToolStripMenuItem();
            _settingsToolStripMenuItem = new ToolStripMenuItem();
            _settingsGlobalKeysToolStripMenuItem = new ToolStripMenuItem();
            _settingsToolbarToolStripMenuItem = new ToolStripMenuItem();
            _settingsQueryAndSctipToolStripMenuItem = new ToolStripMenuItem();
            _doToolStripMenuItem = new ToolStripMenuItem();
            _createActivityForOperationToolStripMenuItem = new ToolStripMenuItem();
            _updateActivityFromOperationToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator10 = new ToolStripSeparator();
            _showFolderToolStripMenuItem = new ToolStripMenuItem();
            _copyGuidsqlToClipboardToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator1 = new ToolStripSeparator();
            _changeAuthorToolStripMenuItem = new ToolStripMenuItem();
            _changeAuthorRecursiveToolStripMenuItem = new ToolStripMenuItem();
            _versionControlToolStripMenuItem = new ToolStripMenuItem();
            _changeXmlFileToolStripMenuItem = new ToolStripMenuItem();
            _showFolderVCorCodeToolStripMenuItem = new ToolStripMenuItem();
            _getVcLatesrecursiveToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator2 = new ToolStripSeparator();
            _showTortoiseLogToolStripMenuItem = new ToolStripMenuItem();
            _showTortoiseRepoBrowserToolStripMenuItem = new ToolStripMenuItem();
            _setSvnKeywordsToolStripMenuItem = new ToolStripMenuItem();
            _setSvnModuleTaggedValuesToolStripMenuItem = new ToolStripMenuItem();
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem = new ToolStripMenuItem();
            _portToolStripMenuItem = new ToolStripMenuItem();
            _movePortsToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator7 = new ToolStripSeparator();
            _deletePortsToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator3 = new ToolStripSeparator();
            _connectPortsToolStripMenuItem = new ToolStripMenuItem();
            _connectPortsInsideComponentsToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator8 = new ToolStripSeparator();
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator4 = new ToolStripSeparator();
            _showPortsInDiagramObjectsToolStripMenuItem = new ToolStripMenuItem();
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem = new ToolStripMenuItem();
            _hidePortsInDiagramToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator5 = new ToolStripSeparator();
            _unhidePortsToolStripMenuItem = new ToolStripMenuItem();
            _hidePortsToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator11 = new ToolStripSeparator();
            _movePortLabelLeftToolStripMenuItem = new ToolStripMenuItem();
            _movePortLabelRightPositionToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator12 = new ToolStripSeparator();
            _movePortLabelLeftToolStripMenuItem1 = new ToolStripMenuItem();
            _movePortLabelRightToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator9 = new ToolStripSeparator();
            _orderDiagramItemsToolStripMenuItem = new ToolStripMenuItem();
            _helpToolStripMenuItem = new ToolStripMenuItem();
            _aboutToolStripMenuItem = new ToolStripMenuItem();
            _helpToolStripMenuItem1 = new ToolStripMenuItem();
            _panelButtons = new Panel();
            _panelLineStyle = new Panel();
            _panelFavorite = new Panel();
            _panelNote = new Panel();
            _panelPort = new Panel();
            _lblPorts = new Label();
            _panelAdvanced = new Panel();
            _panelConveyedItems = new Panel();
            _lblConveyedItems = new Label();
            _panelQuickSearch = new TableLayoutPanel();
            _getLastSqlErrorToolStripMenuItem = new ToolStripMenuItem();
            _toolStripContainer1.TopToolStripPanel.SuspendLayout();
            _toolStripContainer1.SuspendLayout();
            _toolStripQuery.SuspendLayout();
            _menuStrip1.SuspendLayout();
            _panelButtons.SuspendLayout();
            _panelLineStyle.SuspendLayout();
            _panelFavorite.SuspendLayout();
            _panelNote.SuspendLayout();
            _panelPort.SuspendLayout();
            _panelAdvanced.SuspendLayout();
            _panelConveyedItems.SuspendLayout();
            _panelQuickSearch.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer1
            // 
            _toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(_toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            resources.ApplyResources(_toolStripContainer1, "_toolStripContainer1");
            _toolStripContainer1.LeftToolStripPanelVisible = false;
            _toolStripContainer1.Name = "_toolStripContainer1";
            _toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            _toolStripContainer1.TopToolStripPanel.Controls.Add(_toolStripQuery);
            // 
            // toolStripQuery
            // 
            resources.ApplyResources(_toolStripQuery, "_toolStripQuery");
            _toolStripQuery.ImageScalingSize = new Size(20, 20);
            _toolStripQuery.Items.AddRange(new ToolStripItem[] {
            _toolStripSearchBtn1,
            _toolStripSearchBtn2,
            _toolStripSearchBtn3,
            _toolStripSearchBtn4,
            _toolStripSearchBtn5,
            _toolStripSeparator6,
            _toolStripServiceBtn1,
            _toolStripServiceBtn2,
            _toolStripServiceBtn3,
            _toolStripServiceBtn4,
            _toolStripServiceBtn5});
            _toolStripQuery.Name = "_toolStripQuery";
            // 
            // toolStripSearchBtn1
            // 
            _toolStripSearchBtn1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn1, "_toolStripSearchBtn1");
            _toolStripSearchBtn1.Name = "_toolStripSearchBtn1";
            _toolStripSearchBtn1.Click += toolStripBtn11_Click;
            // 
            // toolStripSearchBtn2
            // 
            _toolStripSearchBtn2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _toolStripSearchBtn2.Name = "_toolStripSearchBtn2";
            resources.ApplyResources(_toolStripSearchBtn2, "_toolStripSearchBtn2");
            _toolStripSearchBtn2.Click += toolStripBtn12_Click;
            // 
            // toolStripSearchBtn3
            // 
            _toolStripSearchBtn3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn3, "_toolStripSearchBtn3");
            _toolStripSearchBtn3.Name = "_toolStripSearchBtn3";
            _toolStripSearchBtn3.Click += toolStripBtn13_Click;
            // 
            // toolStripSearchBtn4
            // 
            _toolStripSearchBtn4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn4, "_toolStripSearchBtn4");
            _toolStripSearchBtn4.Name = "_toolStripSearchBtn4";
            _toolStripSearchBtn4.Click += toolStripBtn14_Click;
            // 
            // toolStripSearchBtn5
            // 
            _toolStripSearchBtn5.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn5, "_toolStripSearchBtn5");
            _toolStripSearchBtn5.Name = "_toolStripSearchBtn5";
            _toolStripSearchBtn5.Click += toolStripBtn15_Click;
            // 
            // toolStripSeparator6
            // 
            _toolStripSeparator6.Name = "_toolStripSeparator6";
            resources.ApplyResources(_toolStripSeparator6, "_toolStripSeparator6");
            // 
            // toolStripServiceBtn1
            // 
            _toolStripServiceBtn1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn1, "_toolStripServiceBtn1");
            _toolStripServiceBtn1.Name = "_toolStripServiceBtn1";
            // 
            // toolStripServiceBtn2
            // 
            _toolStripServiceBtn2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn2, "_toolStripServiceBtn2");
            _toolStripServiceBtn2.Name = "_toolStripServiceBtn2";
            // 
            // toolStripServiceBtn3
            // 
            _toolStripServiceBtn3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn3, "_toolStripServiceBtn3");
            _toolStripServiceBtn3.Name = "_toolStripServiceBtn3";
            // 
            // toolStripServiceBtn4
            // 
            _toolStripServiceBtn4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn4, "_toolStripServiceBtn4");
            _toolStripServiceBtn4.Name = "_toolStripServiceBtn4";
            // 
            // toolStripServiceBtn5
            // 
            _toolStripServiceBtn5.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn5, "_toolStripServiceBtn5");
            _toolStripServiceBtn5.Name = "_toolStripServiceBtn5";
            // 
            // btnLabelRight
            // 
            resources.ApplyResources(_btnLabelRight, "_btnLabelRight");
            _btnLabelRight.Name = "_btnLabelRight";
            _toolTip.SetToolTip(_btnLabelRight, resources.GetString("btnLabelRight.ToolTip"));
            _btnLabelRight.UseVisualStyleBackColor = true;
            _btnLabelRight.Click += movePortLablePlusPositionToolStripMenuItem_Click;
            // 
            // btnLabelLeft
            // 
            resources.ApplyResources(_btnLabelLeft, "_btnLabelLeft");
            _btnLabelLeft.Name = "_btnLabelLeft";
            _toolTip.SetToolTip(_btnLabelLeft, resources.GetString("btnLabelLeft.ToolTip"));
            _btnLabelLeft.UseVisualStyleBackColor = true;
            _btnLabelLeft.Click += movePortLableMinusPositionToolStripMenuItem_Click;
            // 
            // btnUp
            // 
            resources.ApplyResources(_btnUp, "_btnUp");
            _btnUp.Name = "_btnUp";
            _toolTip.SetToolTip(_btnUp, resources.GetString("btnUp.ToolTip"));
            _btnUp.UseVisualStyleBackColor = true;
            _btnUp.Click += btnUp_Click;
            // 
            // btnDown
            // 
            resources.ApplyResources(_btnDown, "_btnDown");
            _btnDown.Name = "_btnDown";
            _toolTip.SetToolTip(_btnDown, resources.GetString("btnDown.ToolTip"));
            _btnDown.UseVisualStyleBackColor = true;
            _btnDown.Click += btnDown_Click;
            // 
            // btnLeft
            // 
            resources.ApplyResources(_btnLeft, "_btnLeft");
            _btnLeft.Name = "_btnLeft";
            _toolTip.SetToolTip(_btnLeft, resources.GetString("btnLeft.ToolTip"));
            _btnLeft.UseVisualStyleBackColor = true;
            _btnLeft.Click += btnLeft_Click;
            // 
            // btnRight
            // 
            resources.ApplyResources(_btnRight, "_btnRight");
            _btnRight.Name = "_btnRight";
            _toolTip.SetToolTip(_btnRight, resources.GetString("btnRight.ToolTip"));
            _btnRight.UseVisualStyleBackColor = true;
            _btnRight.Click += btnRight_Click;
            // 
            // btnShowFavorites
            // 
            resources.ApplyResources(_btnShowFavorites, "_btnShowFavorites");
            _btnShowFavorites.Name = "_btnShowFavorites";
            _toolTip.SetToolTip(_btnShowFavorites, resources.GetString("btnShowFavorites.ToolTip"));
            _btnShowFavorites.UseVisualStyleBackColor = true;
            _btnShowFavorites.Click += btnFavorites_Click;
            // 
            // btnRemoveFavorite
            // 
            resources.ApplyResources(_btnRemoveFavorite, "_btnRemoveFavorite");
            _btnRemoveFavorite.Name = "_btnRemoveFavorite";
            _toolTip.SetToolTip(_btnRemoveFavorite, resources.GetString("btnRemoveFavorite.ToolTip"));
            _btnRemoveFavorite.UseVisualStyleBackColor = true;
            _btnRemoveFavorite.Click += btnRemoveFavorite_Click;
            // 
            // btnAddFavorite
            // 
            resources.ApplyResources(_btnAddFavorite, "_btnAddFavorite");
            _btnAddFavorite.Name = "_btnAddFavorite";
            _toolTip.SetToolTip(_btnAddFavorite, resources.GetString("btnAddFavorite.ToolTip"));
            _btnAddFavorite.UseVisualStyleBackColor = true;
            _btnAddFavorite.Click += btnAddFavorite_Click;
            // 
            // txtSearchText
            // 
            resources.ApplyResources(_txtSearchText, "_txtSearchText");
            _txtSearchText.Name = "_txtSearchText";
            _toolTip.SetToolTip(_txtSearchText, resources.GetString("txtSearchText.ToolTip"));
            _txtSearchText.KeyUp += txtUserText_KeyDown;
            _txtSearchText.MouseDoubleClick += txtSearchText_MouseDoubleClick;
            // 
            // btnBezier
            // 
            resources.ApplyResources(_btnBezier, "_btnBezier");
            _btnBezier.Name = "_btnBezier";
            _toolTip.SetToolTip(_btnBezier, resources.GetString("btnBezier.ToolTip"));
            _btnBezier.UseVisualStyleBackColor = true;
            _btnBezier.Click += btnBezier_Click;
            // 
            // btnUpdateActivityParameter
            // 
            resources.ApplyResources(_btnUpdateActivityParameter, "_btnUpdateActivityParameter");
            _btnUpdateActivityParameter.Name = "_btnUpdateActivityParameter";
            _toolTip.SetToolTip(_btnUpdateActivityParameter, resources.GetString("btnUpdateActivityParameter.ToolTip"));
            _btnUpdateActivityParameter.UseVisualStyleBackColor = true;
            _btnUpdateActivityParameter.Click += btnUpdateActivityParametzer_Click;
            // 
            // btnC
            // 
            resources.ApplyResources(_btnC, "_btnC");
            _btnC.Name = "_btnC";
            _toolTip.SetToolTip(_btnC, resources.GetString("btnC.ToolTip"));
            _btnC.UseVisualStyleBackColor = true;
            _btnC.Click += btnC_Click;
            // 
            // btnD
            // 
            resources.ApplyResources(_btnD, "_btnD");
            _btnD.Name = "_btnD";
            _toolTip.SetToolTip(_btnD, resources.GetString("btnD.ToolTip"));
            _btnD.UseVisualStyleBackColor = true;
            _btnD.Click += btnD_Click;
            // 
            // btnA
            // 
            resources.ApplyResources(_btnA, "_btnA");
            _btnA.Name = "_btnA";
            _toolTip.SetToolTip(_btnA, resources.GetString("btnA.ToolTip"));
            _btnA.UseVisualStyleBackColor = true;
            _btnA.Click += btnA_Click;
            // 
            // btnOR
            // 
            resources.ApplyResources(_btnOr, "_btnOr");
            _btnOr.Name = "_btnOr";
            _toolTip.SetToolTip(_btnOr, resources.GetString("btnOR.ToolTip"));
            _btnOr.UseVisualStyleBackColor = true;
            _btnOr.Click += btnOR_Click;
            // 
            // btnComposite
            // 
            resources.ApplyResources(_btnComposite, "_btnComposite");
            _btnComposite.Name = "_btnComposite";
            _toolTip.SetToolTip(_btnComposite, resources.GetString("btnComposite.ToolTip"));
            _btnComposite.UseVisualStyleBackColor = true;
            _btnComposite.Click += btnComposite_Click;
            // 
            // btnDisplaySpecification
            // 
            resources.ApplyResources(_btnDisplaySpecification, "_btnDisplaySpecification");
            _btnDisplaySpecification.Name = "_btnDisplaySpecification";
            _toolTip.SetToolTip(_btnDisplaySpecification, resources.GetString("btnDisplaySpecification.ToolTip"));
            _btnDisplaySpecification.UseVisualStyleBackColor = true;
            _btnDisplaySpecification.Click += btnShowSpecification_Click;
            // 
            // btnFindUsage
            // 
            resources.ApplyResources(_btnFindUsage, "_btnFindUsage");
            _btnFindUsage.Name = "_btnFindUsage";
            _toolTip.SetToolTip(_btnFindUsage, resources.GetString("btnFindUsage.ToolTip"));
            _btnFindUsage.UseVisualStyleBackColor = true;
            _btnFindUsage.Click += btnFindUsage_Click;
            // 
            // btnLocateType
            // 
            resources.ApplyResources(_btnLocateType, "_btnLocateType");
            _btnLocateType.Name = "_btnLocateType";
            _toolTip.SetToolTip(_btnLocateType, resources.GetString("btnLocateType.ToolTip"));
            _btnLocateType.UseVisualStyleBackColor = true;
            _btnLocateType.Click += btnLocateType_Click;
            // 
            // btnAddDiagramNote
            // 
            resources.ApplyResources(_btnAddDiagramNote, "_btnAddDiagramNote");
            _btnAddDiagramNote.Name = "_btnAddDiagramNote";
            _toolTip.SetToolTip(_btnAddDiagramNote, resources.GetString("btnAddDiagramNote.ToolTip"));
            _btnAddDiagramNote.UseVisualStyleBackColor = true;
            _btnAddDiagramNote.Click += btnAddDiagramNote_Click;
            // 
            // btnAddElementNote
            // 
            resources.ApplyResources(_btnAddElementNote, "_btnAddElementNote");
            _btnAddElementNote.Name = "_btnAddElementNote";
            _toolTip.SetToolTip(_btnAddElementNote, resources.GetString("btnAddElementNote.ToolTip"));
            _btnAddElementNote.UseVisualStyleBackColor = true;
            _btnAddElementNote.Click += btnAddElementNote_Click;
            // 
            // btnLocateOperation
            // 
            resources.ApplyResources(_btnLocateOperation, "_btnLocateOperation");
            _btnLocateOperation.Name = "_btnLocateOperation";
            _toolTip.SetToolTip(_btnLocateOperation, resources.GetString("btnLocateOperation.ToolTip"));
            _btnLocateOperation.UseVisualStyleBackColor = true;
            _btnLocateOperation.Click += btnLocateOperation_Click;
            // 
            // btnDisplayBehavior
            // 
            resources.ApplyResources(_btnDisplayBehavior, "_btnDisplayBehavior");
            _btnDisplayBehavior.Name = "_btnDisplayBehavior";
            _toolTip.SetToolTip(_btnDisplayBehavior, resources.GetString("btnDisplayBehavior.ToolTip"));
            _btnDisplayBehavior.UseVisualStyleBackColor = true;
            _btnDisplayBehavior.Click += btnDisplayBehavior_Click;
            // 
            // btnOS
            // 
            resources.ApplyResources(_btnOs, "_btnOs");
            _btnOs.Name = "_btnOs";
            _toolTip.SetToolTip(_btnOs, resources.GetString("btnOS.ToolTip"));
            _btnOs.UseVisualStyleBackColor = true;
            _btnOs.Click += btnOS_Click;
            // 
            // btnTV
            // 
            resources.ApplyResources(_btnTv, "_btnTv");
            _btnTv.Name = "_btnTv";
            _toolTip.SetToolTip(_btnTv, resources.GetString("btnTV.ToolTip"));
            _btnTv.UseVisualStyleBackColor = true;
            _btnTv.Click += btnTV_Click;
            // 
            // btnTH
            // 
            resources.ApplyResources(BtnTh, "BtnTh");
            BtnTh.Name = "BtnTh";
            _toolTip.SetToolTip(BtnTh, resources.GetString("btnTH.ToolTip"));
            BtnTh.UseVisualStyleBackColor = true;
            BtnTh.Click += btnTH_Click;
            // 
            // btnLV
            // 
            resources.ApplyResources(_btnLv, "_btnLv");
            _btnLv.Name = "_btnLv";
            _toolTip.SetToolTip(_btnLv, resources.GetString("btnLV.ToolTip"));
            _btnLv.UseVisualStyleBackColor = true;
            _btnLv.Click += btnLV_Click;
            // 
            // btnLH
            // 
            resources.ApplyResources(_btnLh, "_btnLh");
            _btnLh.Name = "_btnLh";
            _toolTip.SetToolTip(_btnLh, resources.GetString("btnLH.ToolTip"));
            _btnLh.UseVisualStyleBackColor = true;
            _btnLh.Click += btnLH_Click;
            // 
            // txtSearchName
            // 
            _txtSearchName.BackColor = SystemColors.Control;
            resources.ApplyResources(_txtSearchName, "_txtSearchName");
            _txtSearchName.Name = "_txtSearchName";
            _toolTip.SetToolTip(_txtSearchName, resources.GetString("txtSearchName.ToolTip"));
            _txtSearchName.KeyDown += txtUserText_KeyDown;
            _txtSearchName.KeyUp += txtUserText_KeyDown;
            _txtSearchName.MouseDoubleClick += txtSearchName_MouseDoubleClick;
            // 
            // btnConveyedItemConnector
            // 
            resources.ApplyResources(_btnConveyedItemConnector, "_btnConveyedItemConnector");
            _btnConveyedItemConnector.Name = "_btnConveyedItemConnector";
            _toolTip.SetToolTip(_btnConveyedItemConnector, resources.GetString("btnConveyedItemConnector.ToolTip"));
            _btnConveyedItemConnector.UseVisualStyleBackColor = true;
            _btnConveyedItemConnector.Click += btnConveyedItemConnector_Click;
            // 
            // btnConveyedItemElement
            // 
            resources.ApplyResources(_btnConveyedItemElement, "_btnConveyedItemElement");
            _btnConveyedItemElement.Name = "_btnConveyedItemElement";
            _toolTip.SetToolTip(_btnConveyedItemElement, resources.GetString("btnConveyedItemElement.ToolTip"));
            _btnConveyedItemElement.UseVisualStyleBackColor = true;
            _btnConveyedItemElement.Click += btnConveyedItemElement_Click;
            // 
            // menuStrip1
            // 
            _menuStrip1.ImageScalingSize = new Size(20, 20);
            _menuStrip1.Items.AddRange(new ToolStripItem[] {
            _fileToolStripMenuItem,
            _doToolStripMenuItem,
            _versionControlToolStripMenuItem,
            _portToolStripMenuItem,
            _helpToolStripMenuItem});
            resources.ApplyResources(_menuStrip1, "_menuStrip1");
            _menuStrip1.Name = "_menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            _fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            _settingGeneralToolStripMenuItem,
            _settingsToolStripMenuItem,
            _settingsGlobalKeysToolStripMenuItem,
            _settingsToolbarToolStripMenuItem,
            _settingsQueryAndSctipToolStripMenuItem});
            _fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
            resources.ApplyResources(_fileToolStripMenuItem, "_fileToolStripMenuItem");
            // 
            // settingGeneralToolStripMenuItem
            // 
            _settingGeneralToolStripMenuItem.Name = "_settingGeneralToolStripMenuItem";
            resources.ApplyResources(_settingGeneralToolStripMenuItem, "_settingGeneralToolStripMenuItem");
            _settingGeneralToolStripMenuItem.Click += settingGeneralToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            _settingsToolStripMenuItem.Name = "_settingsToolStripMenuItem";
            resources.ApplyResources(_settingsToolStripMenuItem, "_settingsToolStripMenuItem");
            _settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // settingsGlobalKeysToolStripMenuItem
            // 
            _settingsGlobalKeysToolStripMenuItem.Name = "_settingsGlobalKeysToolStripMenuItem";
            resources.ApplyResources(_settingsGlobalKeysToolStripMenuItem, "_settingsGlobalKeysToolStripMenuItem");
            _settingsGlobalKeysToolStripMenuItem.Click += settingsKeysToolStripMenuItem_Click;
            // 
            // settingsToolbarToolStripMenuItem
            // 
            _settingsToolbarToolStripMenuItem.Name = "_settingsToolbarToolStripMenuItem";
            resources.ApplyResources(_settingsToolbarToolStripMenuItem, "_settingsToolbarToolStripMenuItem");
            _settingsToolbarToolStripMenuItem.Click += settingsToolbarToolStripMenuItem_Click;
            // 
            // settingsQueryAndSctipToolStripMenuItem
            // 
            _settingsQueryAndSctipToolStripMenuItem.Name = "_settingsQueryAndSctipToolStripMenuItem";
            resources.ApplyResources(_settingsQueryAndSctipToolStripMenuItem, "_settingsQueryAndSctipToolStripMenuItem");
            _settingsQueryAndSctipToolStripMenuItem.Click += settingsQueryAndSctipToolStripMenuItem_Click;
            // 
            // doToolStripMenuItem
            // 
            _doToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            _createActivityForOperationToolStripMenuItem,
            _updateActivityFromOperationToolStripMenuItem,
            _toolStripSeparator10,
            _showFolderToolStripMenuItem,
            _copyGuidsqlToClipboardToolStripMenuItem,
            _toolStripSeparator1,
            _changeAuthorToolStripMenuItem,
            _changeAuthorRecursiveToolStripMenuItem});
            _doToolStripMenuItem.Name = "_doToolStripMenuItem";
            resources.ApplyResources(_doToolStripMenuItem, "_doToolStripMenuItem");
            // 
            // createActivityForOperationToolStripMenuItem
            // 
            _createActivityForOperationToolStripMenuItem.Name = "_createActivityForOperationToolStripMenuItem";
            resources.ApplyResources(_createActivityForOperationToolStripMenuItem, "_createActivityForOperationToolStripMenuItem");
            _createActivityForOperationToolStripMenuItem.Click += createActivityForOperationToolStripMenuItem_Click;
            // 
            // updateActivityFromOperationToolStripMenuItem
            // 
            _updateActivityFromOperationToolStripMenuItem.Name = "_updateActivityFromOperationToolStripMenuItem";
            resources.ApplyResources(_updateActivityFromOperationToolStripMenuItem, "_updateActivityFromOperationToolStripMenuItem");
            _updateActivityFromOperationToolStripMenuItem.Click += updateActivityFromOperationToolStripMenuItem_Click;
            // 
            // toolStripSeparator10
            // 
            _toolStripSeparator10.Name = "_toolStripSeparator10";
            resources.ApplyResources(_toolStripSeparator10, "_toolStripSeparator10");
            // 
            // showFolderToolStripMenuItem
            // 
            _showFolderToolStripMenuItem.Name = "_showFolderToolStripMenuItem";
            resources.ApplyResources(_showFolderToolStripMenuItem, "_showFolderToolStripMenuItem");
            _showFolderToolStripMenuItem.Click += showFolderToolStripMenuItem_Click;
            // 
            // copyGUIDSQLToClipboardToolStripMenuItem
            // 
            _copyGuidsqlToClipboardToolStripMenuItem.Name = "_copyGuidsqlToClipboardToolStripMenuItem";
            resources.ApplyResources(_copyGuidsqlToClipboardToolStripMenuItem, "_copyGuidsqlToClipboardToolStripMenuItem");
            _copyGuidsqlToClipboardToolStripMenuItem.Click += copyGUIDSQLToClipboardToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            _toolStripSeparator1.Name = "_toolStripSeparator1";
            resources.ApplyResources(_toolStripSeparator1, "_toolStripSeparator1");
            // 
            // changeAuthorToolStripMenuItem
            // 
            _changeAuthorToolStripMenuItem.Name = "_changeAuthorToolStripMenuItem";
            resources.ApplyResources(_changeAuthorToolStripMenuItem, "_changeAuthorToolStripMenuItem");
            _changeAuthorToolStripMenuItem.Click += changeAuthorToolStripMenuItem_Click;
            // 
            // changeAuthorRecursiveToolStripMenuItem
            // 
            _changeAuthorRecursiveToolStripMenuItem.Name = "_changeAuthorRecursiveToolStripMenuItem";
            resources.ApplyResources(_changeAuthorRecursiveToolStripMenuItem, "_changeAuthorRecursiveToolStripMenuItem");
            _changeAuthorRecursiveToolStripMenuItem.Click += changeAuthorRecursiveToolStripMenuItem_Click;
            // 
            // versionControlToolStripMenuItem
            // 
            _versionControlToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            _changeXmlFileToolStripMenuItem,
            _showFolderVCorCodeToolStripMenuItem,
            _getVcLatesrecursiveToolStripMenuItem,
            _toolStripSeparator2,
            _showTortoiseLogToolStripMenuItem,
            _showTortoiseRepoBrowserToolStripMenuItem,
            _setSvnKeywordsToolStripMenuItem,
            _setSvnModuleTaggedValuesToolStripMenuItem,
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem});
            _versionControlToolStripMenuItem.Name = "_versionControlToolStripMenuItem";
            resources.ApplyResources(_versionControlToolStripMenuItem, "_versionControlToolStripMenuItem");
            // 
            // changeXMLFileToolStripMenuItem
            // 
            _changeXmlFileToolStripMenuItem.Name = "_changeXmlFileToolStripMenuItem";
            resources.ApplyResources(_changeXmlFileToolStripMenuItem, "_changeXmlFileToolStripMenuItem");
            _changeXmlFileToolStripMenuItem.Click += changeXMLFileToolStripMenuItem_Click;
            // 
            // showFolderVCorCodeToolStripMenuItem
            // 
            _showFolderVCorCodeToolStripMenuItem.Name = "_showFolderVCorCodeToolStripMenuItem";
            resources.ApplyResources(_showFolderVCorCodeToolStripMenuItem, "_showFolderVCorCodeToolStripMenuItem");
            _showFolderVCorCodeToolStripMenuItem.Click += showFolderVCorCodeToolStripMenuItem_Click;
            // 
            // getVCLatesrecursiveToolStripMenuItem
            // 
            _getVcLatesrecursiveToolStripMenuItem.Name = "_getVcLatesrecursiveToolStripMenuItem";
            resources.ApplyResources(_getVcLatesrecursiveToolStripMenuItem, "_getVcLatesrecursiveToolStripMenuItem");
            _getVcLatesrecursiveToolStripMenuItem.Click += getVCLatesrecursiveToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            _toolStripSeparator2.Name = "_toolStripSeparator2";
            resources.ApplyResources(_toolStripSeparator2, "_toolStripSeparator2");
            // 
            // showTortoiseLogToolStripMenuItem
            // 
            _showTortoiseLogToolStripMenuItem.Name = "_showTortoiseLogToolStripMenuItem";
            resources.ApplyResources(_showTortoiseLogToolStripMenuItem, "_showTortoiseLogToolStripMenuItem");
            _showTortoiseLogToolStripMenuItem.Click += showTortoiseLogToolStripMenuItem_Click;
            // 
            // showTortoiseRepoBrowserToolStripMenuItem
            // 
            _showTortoiseRepoBrowserToolStripMenuItem.Name = "_showTortoiseRepoBrowserToolStripMenuItem";
            resources.ApplyResources(_showTortoiseRepoBrowserToolStripMenuItem, "_showTortoiseRepoBrowserToolStripMenuItem");
            _showTortoiseRepoBrowserToolStripMenuItem.Click += showTortoiseRepoBrowserToolStripMenuItem_Click;
            // 
            // setSvnKeywordsToolStripMenuItem
            // 
            _setSvnKeywordsToolStripMenuItem.Name = "_setSvnKeywordsToolStripMenuItem";
            resources.ApplyResources(_setSvnKeywordsToolStripMenuItem, "_setSvnKeywordsToolStripMenuItem");
            _setSvnKeywordsToolStripMenuItem.Click += setSvnKeywordsToolStripMenuItem_Click;
            // 
            // setSvnModuleTaggedValuesToolStripMenuItem
            // 
            _setSvnModuleTaggedValuesToolStripMenuItem.Name = "_setSvnModuleTaggedValuesToolStripMenuItem";
            resources.ApplyResources(_setSvnModuleTaggedValuesToolStripMenuItem, "_setSvnModuleTaggedValuesToolStripMenuItem");
            _setSvnModuleTaggedValuesToolStripMenuItem.Click += setSvnModuleTaggedValuesToolStripMenuItem_Click;
            // 
            // setSvnModuleTaggedValuesrecursiveToolStripMenuItem
            // 
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Name = "_setSvnModuleTaggedValuesrecursiveToolStripMenuItem";
            resources.ApplyResources(_setSvnModuleTaggedValuesrecursiveToolStripMenuItem, "_setSvnModuleTaggedValuesrecursiveToolStripMenuItem");
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Click += setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click;
            // 
            // portToolStripMenuItem
            // 
            _portToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            _movePortsToolStripMenuItem,
            _toolStripSeparator7,
            _deletePortsToolStripMenuItem,
            _toolStripSeparator3,
            _connectPortsToolStripMenuItem,
            _connectPortsInsideComponentsToolStripMenuItem,
            _toolStripSeparator8,
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem,
            _toolStripSeparator4,
            _showPortsInDiagramObjectsToolStripMenuItem,
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem,
            _hidePortsInDiagramToolStripMenuItem,
            _toolStripSeparator5,
            _unhidePortsToolStripMenuItem,
            _hidePortsToolStripMenuItem,
            _toolStripSeparator11,
            _movePortLabelLeftToolStripMenuItem,
            _movePortLabelRightPositionToolStripMenuItem,
            _toolStripSeparator12,
            _movePortLabelLeftToolStripMenuItem1,
            _movePortLabelRightToolStripMenuItem,
            _toolStripSeparator9,
            _orderDiagramItemsToolStripMenuItem});
            _portToolStripMenuItem.Name = "_portToolStripMenuItem";
            resources.ApplyResources(_portToolStripMenuItem, "_portToolStripMenuItem");
            // 
            // movePortsToolStripMenuItem
            // 
            _movePortsToolStripMenuItem.Name = "_movePortsToolStripMenuItem";
            resources.ApplyResources(_movePortsToolStripMenuItem, "_movePortsToolStripMenuItem");
            _movePortsToolStripMenuItem.Click += copyPortsToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            _toolStripSeparator7.Name = "_toolStripSeparator7";
            resources.ApplyResources(_toolStripSeparator7, "_toolStripSeparator7");
            // 
            // deletePortsToolStripMenuItem
            // 
            _deletePortsToolStripMenuItem.Name = "_deletePortsToolStripMenuItem";
            resources.ApplyResources(_deletePortsToolStripMenuItem, "_deletePortsToolStripMenuItem");
            _deletePortsToolStripMenuItem.Click += deletePortsToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            _toolStripSeparator3.Name = "_toolStripSeparator3";
            resources.ApplyResources(_toolStripSeparator3, "_toolStripSeparator3");
            // 
            // connectPortsToolStripMenuItem
            // 
            _connectPortsToolStripMenuItem.Name = "_connectPortsToolStripMenuItem";
            resources.ApplyResources(_connectPortsToolStripMenuItem, "_connectPortsToolStripMenuItem");
            _connectPortsToolStripMenuItem.Click += connectPortsToolStripMenuItem_Click;
            // 
            // connectPortsInsideComponentsToolStripMenuItem
            // 
            _connectPortsInsideComponentsToolStripMenuItem.Name = "_connectPortsInsideComponentsToolStripMenuItem";
            resources.ApplyResources(_connectPortsInsideComponentsToolStripMenuItem, "_connectPortsInsideComponentsToolStripMenuItem");
            _connectPortsInsideComponentsToolStripMenuItem.Click += connectPortsInsideComponentsToolStripMenuItem_Click;
            // 
            // toolStripSeparator8
            // 
            _toolStripSeparator8.Name = "_toolStripSeparator8";
            resources.ApplyResources(_toolStripSeparator8, "_toolStripSeparator8");
            // 
            // makeConnectorsUnspecifiedDirectionToolStripMenuItem
            // 
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem.Name = "_makeConnectorsUnspecifiedDirectionToolStripMenuItem";
            resources.ApplyResources(_makeConnectorsUnspecifiedDirectionToolStripMenuItem, "_makeConnectorsUnspecifiedDirectionToolStripMenuItem");
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem.Click += makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            _toolStripSeparator4.Name = "_toolStripSeparator4";
            resources.ApplyResources(_toolStripSeparator4, "_toolStripSeparator4");
            // 
            // showPortsInDiagramObjectsToolStripMenuItem
            // 
            _showPortsInDiagramObjectsToolStripMenuItem.Name = "_showPortsInDiagramObjectsToolStripMenuItem";
            resources.ApplyResources(_showPortsInDiagramObjectsToolStripMenuItem, "_showPortsInDiagramObjectsToolStripMenuItem");
            _showPortsInDiagramObjectsToolStripMenuItem.Click += showPortsInDiagramObjectsToolStripMenuItem_Click;
            // 
            // showSendingPortsLeftRecievingPortsRightToolStripMenuItem
            // 
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Name = "_showSendingPortsLeftRecievingPortsRightToolStripMenuItem";
            resources.ApplyResources(_showSendingPortsLeftRecievingPortsRightToolStripMenuItem, "_showSendingPortsLeftRecievingPortsRightToolStripMenuItem");
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Click += showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click;
            // 
            // hidePortsInDiagramToolStripMenuItem
            // 
            _hidePortsInDiagramToolStripMenuItem.Name = "_hidePortsInDiagramToolStripMenuItem";
            resources.ApplyResources(_hidePortsInDiagramToolStripMenuItem, "_hidePortsInDiagramToolStripMenuItem");
            _hidePortsInDiagramToolStripMenuItem.Click += removePortsInDiagramToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            _toolStripSeparator5.Name = "_toolStripSeparator5";
            resources.ApplyResources(_toolStripSeparator5, "_toolStripSeparator5");
            // 
            // unhidePortsToolStripMenuItem
            // 
            _unhidePortsToolStripMenuItem.Name = "_unhidePortsToolStripMenuItem";
            resources.ApplyResources(_unhidePortsToolStripMenuItem, "_unhidePortsToolStripMenuItem");
            _unhidePortsToolStripMenuItem.Click += viewPortLabelToolStripMenuItem_Click;
            // 
            // hidePortsToolStripMenuItem
            // 
            _hidePortsToolStripMenuItem.Name = "_hidePortsToolStripMenuItem";
            resources.ApplyResources(_hidePortsToolStripMenuItem, "_hidePortsToolStripMenuItem");
            _hidePortsToolStripMenuItem.Click += hidePortLabelToolStripMenuItem_Click;
            // 
            // toolStripSeparator11
            // 
            _toolStripSeparator11.Name = "_toolStripSeparator11";
            resources.ApplyResources(_toolStripSeparator11, "_toolStripSeparator11");
            // 
            // movePortLabelLeftToolStripMenuItem
            // 
            _movePortLabelLeftToolStripMenuItem.Name = "_movePortLabelLeftToolStripMenuItem";
            resources.ApplyResources(_movePortLabelLeftToolStripMenuItem, "_movePortLabelLeftToolStripMenuItem");
            _movePortLabelLeftToolStripMenuItem.Click += movePortLableLeftPositionToolStripMenuItem_Click;
            // 
            // movePortLabelRightPositionToolStripMenuItem
            // 
            _movePortLabelRightPositionToolStripMenuItem.Name = "_movePortLabelRightPositionToolStripMenuItem";
            resources.ApplyResources(_movePortLabelRightPositionToolStripMenuItem, "_movePortLabelRightPositionToolStripMenuItem");
            _movePortLabelRightPositionToolStripMenuItem.Click += movePortLableRightPositionToolStripMenuItem_Click;
            // 
            // toolStripSeparator12
            // 
            _toolStripSeparator12.Name = "_toolStripSeparator12";
            resources.ApplyResources(_toolStripSeparator12, "_toolStripSeparator12");
            // 
            // movePortLabelLeftToolStripMenuItem1
            // 
            _movePortLabelLeftToolStripMenuItem1.Name = "_movePortLabelLeftToolStripMenuItem1";
            resources.ApplyResources(_movePortLabelLeftToolStripMenuItem1, "_movePortLabelLeftToolStripMenuItem1");
            _movePortLabelLeftToolStripMenuItem1.Click += movePortLableMinusPositionToolStripMenuItem_Click;
            // 
            // movePortLabelRightToolStripMenuItem
            // 
            _movePortLabelRightToolStripMenuItem.Name = "_movePortLabelRightToolStripMenuItem";
            resources.ApplyResources(_movePortLabelRightToolStripMenuItem, "_movePortLabelRightToolStripMenuItem");
            _movePortLabelRightToolStripMenuItem.Click += movePortLablePlusPositionToolStripMenuItem_Click;
            // 
            // toolStripSeparator9
            // 
            _toolStripSeparator9.Name = "_toolStripSeparator9";
            resources.ApplyResources(_toolStripSeparator9, "_toolStripSeparator9");
            // 
            // orderDiagramItemsToolStripMenuItem
            // 
            _orderDiagramItemsToolStripMenuItem.Name = "_orderDiagramItemsToolStripMenuItem";
            resources.ApplyResources(_orderDiagramItemsToolStripMenuItem, "_orderDiagramItemsToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            _helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            _aboutToolStripMenuItem,
            _getLastSqlErrorToolStripMenuItem,
            _helpToolStripMenuItem1});
            _helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            resources.ApplyResources(_helpToolStripMenuItem, "_helpToolStripMenuItem");
            _helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            _aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            resources.ApplyResources(_aboutToolStripMenuItem, "_aboutToolStripMenuItem");
            _aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem1
            // 
            _helpToolStripMenuItem1.Name = "_helpToolStripMenuItem1";
            resources.ApplyResources(_helpToolStripMenuItem1, "_helpToolStripMenuItem1");
            _helpToolStripMenuItem1.Click += helpToolStripMenuItem1_Click;
            // 
            // panelButtons
            // 
            _panelButtons.Controls.Add(_toolStripContainer1);
            resources.ApplyResources(_panelButtons, "_panelButtons");
            _panelButtons.Name = "_panelButtons";
            // 
            // panelLineStyle
            // 
            _panelLineStyle.Controls.Add(_btnLv);
            _panelLineStyle.Controls.Add(_btnLh);
            _panelLineStyle.Controls.Add(_btnTv);
            _panelLineStyle.Controls.Add(BtnTh);
            _panelLineStyle.Controls.Add(_btnC);
            _panelLineStyle.Controls.Add(_btnBezier);
            _panelLineStyle.Controls.Add(_btnOs);
            _panelLineStyle.Controls.Add(_btnOr);
            _panelLineStyle.Controls.Add(_btnA);
            _panelLineStyle.Controls.Add(_btnD);
            resources.ApplyResources(_panelLineStyle, "_panelLineStyle");
            _panelLineStyle.Name = "_panelLineStyle";
            // 
            // panelFavorite
            // 
            _panelFavorite.Controls.Add(_btnAddFavorite);
            _panelFavorite.Controls.Add(_btnRemoveFavorite);
            _panelFavorite.Controls.Add(_btnShowFavorites);
            _panelFavorite.Controls.Add(_btnDisplayBehavior);
            resources.ApplyResources(_panelFavorite, "_panelFavorite");
            _panelFavorite.Name = "_panelFavorite";
            // 
            // panelNote
            // 
            _panelNote.Controls.Add(_btnAddElementNote);
            _panelNote.Controls.Add(_btnAddDiagramNote);
            resources.ApplyResources(_panelNote, "_panelNote");
            _panelNote.Name = "_panelNote";
            // 
            // panelPort
            // 
            _panelPort.Controls.Add(_lblPorts);
            _panelPort.Controls.Add(_btnLeft);
            _panelPort.Controls.Add(_btnUp);
            _panelPort.Controls.Add(_btnRight);
            _panelPort.Controls.Add(_btnDown);
            _panelPort.Controls.Add(_btnLabelLeft);
            _panelPort.Controls.Add(_btnLabelRight);
            resources.ApplyResources(_panelPort, "_panelPort");
            _panelPort.Name = "_panelPort";
            // 
            // lblPorts
            // 
            resources.ApplyResources(_lblPorts, "_lblPorts");
            _lblPorts.Name = "_lblPorts";
            // 
            // panelAdvanced
            // 
            _panelAdvanced.Controls.Add(_btnComposite);
            _panelAdvanced.Controls.Add(_btnFindUsage);
            _panelAdvanced.Controls.Add(_btnUpdateActivityParameter);
            _panelAdvanced.Controls.Add(_btnLocateType);
            _panelAdvanced.Controls.Add(_btnLocateOperation);
            _panelAdvanced.Controls.Add(_btnDisplaySpecification);
            resources.ApplyResources(_panelAdvanced, "_panelAdvanced");
            _panelAdvanced.Name = "_panelAdvanced";
            // 
            // panelConveyedItems
            // 
            _panelConveyedItems.Controls.Add(_lblConveyedItems);
            _panelConveyedItems.Controls.Add(_btnConveyedItemElement);
            _panelConveyedItems.Controls.Add(_btnConveyedItemConnector);
            resources.ApplyResources(_panelConveyedItems, "_panelConveyedItems");
            _panelConveyedItems.Name = "_panelConveyedItems";
            // 
            // lblConveyedItems
            // 
            resources.ApplyResources(_lblConveyedItems, "_lblConveyedItems");
            _lblConveyedItems.Name = "_lblConveyedItems";
            // 
            // panelQuickSearch
            // 
            resources.ApplyResources(_panelQuickSearch, "_panelQuickSearch");
            _panelQuickSearch.Controls.Add(_txtSearchName, 0, 0);
            _panelQuickSearch.Controls.Add(_txtSearchText, 0, 0);
            _panelQuickSearch.Name = "_panelQuickSearch";
            // 
            // getLastSQLErrorToolStripMenuItem
            // 
            _getLastSqlErrorToolStripMenuItem.Name = "_getLastSqlErrorToolStripMenuItem";
            resources.ApplyResources(_getLastSqlErrorToolStripMenuItem, "_getLastSqlErrorToolStripMenuItem");
            _getLastSqlErrorToolStripMenuItem.Click += getLastSQLErrorToolStripMenuItem_Click;
            // 
            // AddinControlGUI
            // 
            resources.ApplyResources(this, "$this");
            Controls.Add(_panelPort);
            Controls.Add(_panelNote);
            Controls.Add(_panelAdvanced);
            Controls.Add(_panelFavorite);
            Controls.Add(_panelConveyedItems);
            Controls.Add(_panelLineStyle);
            Controls.Add(_panelQuickSearch);
            Controls.Add(_panelButtons);
            Controls.Add(_menuStrip1);
            Name = "AddinControlGui";
            _toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            _toolStripContainer1.TopToolStripPanel.PerformLayout();
            _toolStripContainer1.ResumeLayout(false);
            _toolStripContainer1.PerformLayout();
            _toolStripQuery.ResumeLayout(false);
            _toolStripQuery.PerformLayout();
            _menuStrip1.ResumeLayout(false);
            _menuStrip1.PerformLayout();
            _panelButtons.ResumeLayout(false);
            _panelLineStyle.ResumeLayout(false);
            _panelFavorite.ResumeLayout(false);
            _panelNote.ResumeLayout(false);
            _panelPort.ResumeLayout(false);
            _panelPort.PerformLayout();
            _panelAdvanced.ResumeLayout(false);
            _panelConveyedItems.ResumeLayout(false);
            _panelConveyedItems.PerformLayout();
            _panelQuickSearch.ResumeLayout(false);
            _panelQuickSearch.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        #region parameterizeMenusAndButtons
        /// <summary>
        /// Parameterize menu and buttons to visible/hidden due to
        /// - isAdvanced
        /// - isSvnSupported
        /// - isVcSupported
        /// </summary>
        public void ParameterizeMenusAndButtons()
        {
            // Don't change the order
            _panelPort.Visible = false;
            _panelNote.Visible = false;
            _panelAdvanced.Visible = false;
            _panelFavorite.Visible = false;
            _panelConveyedItems.Visible = false;
            _panelLineStyle.Visible = false;
            _panelQuickSearch.Visible = false;
            _panelButtons.Visible = false;



            _panelPort.ResumeLayout(false);
            _panelPort.PerformLayout();
            _panelNote.ResumeLayout(false);
            _panelNote.PerformLayout();
            _panelAdvanced.ResumeLayout(false);
            _panelAdvanced.PerformLayout();
            _panelFavorite.ResumeLayout(false);
            _panelFavorite.PerformLayout();
            _panelConveyedItems.ResumeLayout(false);
            _panelConveyedItems.PerformLayout();
            _panelLineStyle.ResumeLayout(false);
            _panelLineStyle.PerformLayout();
            _panelQuickSearch.ResumeLayout(false);
            _panelQuickSearch.PerformLayout();
            _panelButtons.ResumeLayout(false);
            _panelButtons.PerformLayout();





            _panelAdvanced.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

            // Don't change the order
            _panelPort.Visible = false;
            _panelNote.Visible = false;
            _panelAdvanced.Visible = false;
            _panelFavorite.Visible = false;
            _panelConveyedItems.Visible = false;
            _panelLineStyle.Visible = false;
            _panelQuickSearch.Visible = false;
            _panelButtons.Visible = false;


            // Port
            _panelPort.Visible = AddinSettings.IsAdvancedPort;
            _panelNote.Visible = AddinSettings.IsAdvancedDiagramNote;
            _lblPorts.Visible = AddinSettings.IsAdvancedPort;


            // Advanced
            _panelAdvanced.Visible = AddinSettings.IsAdvancedFeatures;

            // Advanced Features
            _btnDisplayBehavior.Visible = AddinSettings.IsAdvancedFeatures;
            _btnDisplaySpecification.Visible = AddinSettings.IsAdvancedFeatures;
            _btnUpdateActivityParameter.Visible = AddinSettings.IsAdvancedFeatures;
            _btnLocateOperation.Visible = AddinSettings.IsAdvancedFeatures;
            _btnFindUsage.Visible = AddinSettings.IsAdvancedFeatures;
            _btnLocateType.Visible = AddinSettings.IsAdvancedFeatures;
            _btnComposite.Visible = AddinSettings.IsAdvancedFeatures;

            // Favorite
            _panelFavorite.Visible = AddinSettings.IsFavoriteSupport || AddinSettings.IsAdvancedFeatures;
            _btnAddFavorite.Visible = AddinSettings.IsFavoriteSupport;
            _btnRemoveFavorite.Visible = AddinSettings.IsFavoriteSupport;
            _btnShowFavorites.Visible = AddinSettings.IsFavoriteSupport;

            // Conveyed Item support
            _panelConveyedItems.Visible = AddinSettings.IsConveyedItemsSupport;
            _btnConveyedItemConnector.Visible = AddinSettings.IsConveyedItemsSupport;
            _btnConveyedItemElement.Visible = AddinSettings.IsConveyedItemsSupport;
            _lblConveyedItems.Visible = true;

            // Line style Panel
            _panelLineStyle.Visible = AddinSettings.IsLineStyleSupport;

            // no quick search defined
            _panelQuickSearch.Visible = (AddinSettings.QuickSearchName.Trim() != "");
            _txtSearchName.Text = AddinSettings.QuickSearchName.Trim();

            // Buttons for queries and services
            _panelButtons.Visible = AddinSettings.IsShowQueryButton || AddinSettings.IsShowServiceButton;


            // SVN support
            bool visibleSvnVc = true && !(AddinSettings.IsSvnSupport == false | AddinSettings.IsVcSupport == false);
            _showTortoiseRepoBrowserToolStripMenuItem.Visible = visibleSvnVc;
            _showTortoiseLogToolStripMenuItem.Visible = visibleSvnVc;
            _setSvnModuleTaggedValuesToolStripMenuItem.Visible = visibleSvnVc;
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Visible = visibleSvnVc;
            _setSvnKeywordsToolStripMenuItem.Visible = visibleSvnVc;

            // Visible VC
            bool visibleVc = true && AddinSettings.IsVcSupport;

            _getVcLatesrecursiveToolStripMenuItem.Visible = visibleVc;
            _changeXmlFileToolStripMenuItem.Visible = visibleVc;
            _orderDiagramItemsToolStripMenuItem.Visible = visibleVc;

            if (AddinSettings.IsSvnSupport == false && AddinSettings.IsVcSupport == false)
            {
                _versionControlToolStripMenuItem.Visible = false;
            }
            else
            {
                _versionControlToolStripMenuItem.Visible = true;
            }

            // Visual Port Support
            bool visiblePorts = false || AddinSettings.IsAdvancedPort;

            _btnLeft.Visible = visiblePorts;
            _btnRight.Visible = visiblePorts;
            _btnUp.Visible = visiblePorts;
            _btnDown.Visible = visiblePorts;

            _btnLabelLeft.Visible = visiblePorts;
            _btnLabelRight.Visible = visiblePorts;

            // Note in diagram support
            bool visibleDiagramNote = false || AddinSettings.IsAdvancedDiagramNote;
            _btnAddDiagramNote.Visible = visibleDiagramNote;
            _btnAddElementNote.Visible = visibleDiagramNote;

            // LineStyle
            _btnLv.Visible = AddinSettings.IsLineStyleSupport;
            _btnLh.Visible = AddinSettings.IsLineStyleSupport;
            _btnTv.Visible = AddinSettings.IsLineStyleSupport;
            BtnTh.Visible = AddinSettings.IsLineStyleSupport;
            _btnC.Visible = AddinSettings.IsLineStyleSupport;
            _btnBezier.Visible = AddinSettings.IsLineStyleSupport;
            _btnOs.Visible = AddinSettings.IsLineStyleSupport;
            _btnOr.Visible = AddinSettings.IsLineStyleSupport;
            _btnA.Visible = AddinSettings.IsLineStyleSupport;
            _btnD.Visible = AddinSettings.IsLineStyleSupport;

            // Conveyed Items support
            _btnConveyedItemConnector.Visible = AddinSettings.IsConveyedItemsSupport;
            _btnConveyedItemElement.Visible = AddinSettings.IsConveyedItemsSupport;

            // Favorite
            _btnAddFavorite.Visible = AddinSettings.IsFavoriteSupport;
            _btnRemoveFavorite.Visible = AddinSettings.IsFavoriteSupport;
            _btnShowFavorites.Visible = AddinSettings.IsFavoriteSupport;

            // Advance features
            _btnDisplayBehavior.Visible = AddinSettings.IsAdvancedFeatures;

            //boolean visibleDiagramNote = false || _addinSettings.isAdvancedDiagramNote;


        }
        #endregion
        #region parameterizeSearchButtons
        /// <summary>
        /// Parametrize 5 quick buttons for search with:
        /// <para/>- Search Name
        /// <para/>- Search Tooltip
        /// </summary>
        public void ParameterizeSearchButton()
        {
            _toolStripSearchBtn1.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn2.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn3.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn4.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn5.Visible = AddinSettings.IsShowQueryButton;

            for (int pos = 0; pos < AddinSettings.ButtonsSearch.Length; pos++)
            {
                const string defaultHelptext = "Free Model Searches, Model Search not parametrized.";
                string buttonText = "";
                string helpText = defaultHelptext;
                if (AddinSettings.ButtonsSearch[pos] != null)
                {
                    EaAddinButtons search = AddinSettings.ButtonsSearch[pos];
                    {
                        if (search.KeyText.Trim() != "")
                        {
                            buttonText = search.KeyText;
                            helpText = search.KeySearchTooltip;
                        }
                    }
                }

                switch (pos)
                {
                    case 0:
                        _toolStripSearchBtn1.Text = buttonText;
                        _toolStripSearchBtn1.ToolTipText = helpText;
                        break;
                    case 1:
                        _toolStripSearchBtn2.Text = buttonText;
                        _toolStripSearchBtn2.ToolTipText = helpText;
                        break;
                    case 2:
                        _toolStripSearchBtn3.Text = buttonText;
                        _toolStripSearchBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        _toolStripSearchBtn4.Text = buttonText;
                        _toolStripSearchBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        _toolStripSearchBtn5.Text = buttonText;
                        _toolStripSearchBtn5.ToolTipText = helpText;
                        break;

                }
            }
        }
        #endregion
        #region parameterizeServiceButton
        public void ParameterizeServiceButton()
        {
            _toolStripServiceBtn1.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn2.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn3.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn4.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn5.Visible = AddinSettings.IsShowServiceButton;
            for (int pos = 0; pos < AddinSettings.ButtonsServices.Count; pos++)
            {
                string buttonText = "";
                string helpText = "free Service, Service not parametrized";
                if (AddinSettings.ButtonsServices[pos] != null)
                {
                    ServicesCallConfig service = AddinSettings.ButtonsServices[pos];
                    if (service.ButtonText.Trim() != "")
                    {
                        buttonText = service.ButtonText;
                        helpText = service.HelpTextLong; //  Long Help text
                    }
                }

                switch (pos)
                {
                    case 0:
                        _toolStripServiceBtn1.Text = buttonText;
                        _toolStripServiceBtn1.ToolTipText = helpText;
                        break;
                    case 1:
                        _toolStripServiceBtn2.Text = buttonText;
                        _toolStripServiceBtn2.ToolTipText = helpText;
                        break;
                    case 2:
                        _toolStripServiceBtn3.Text = buttonText; ;
                        _toolStripServiceBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        _toolStripServiceBtn4.Text = buttonText; ;
                        _toolStripServiceBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        _toolStripServiceBtn5.Text = buttonText; ;
                        _toolStripServiceBtn5.ToolTipText = helpText;
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
            ObjectType type = Model.Repository.GetContextItemType();
            if (type == ObjectType.otElement)
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

            }
            else
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
            ObjectType type = Model.Repository.GetContextItemType();
            if (type == ObjectType.otConnector)
            {
                string sql = @"
                        select  o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE, o.name As Element
                        from t_object o
                        where  o.object_id in ( #ConveyedItemIDS# )
                        ORDER BY 3
                ";
                // Run SQL with macro replacement
                Model.SQLRun(sql, "");

            }
            else
            {
                MessageBox.Show("To get the Elements on the Connector you have to select an Connector.", "No Connector is selected, break!!!");
            }
        }

        void getLastSQLErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(SqlError.getEaSqlErrorFilePath());
        }
    }
}