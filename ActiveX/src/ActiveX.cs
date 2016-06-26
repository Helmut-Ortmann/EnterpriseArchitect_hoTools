using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using hoTools.Settings;
using hoTools.EaServices;
using hoTools.EAServicesPort;
using Control.EaAddinShortcuts;
using hoTools.Settings.Key;
using hoTools.Settings.Toolbar;

using hoTools.Utils.SQL;
using hoTools.Utils;
using hoTools.Utils.Configuration;


namespace hoTools.ActiveX
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("82A06E9C-7568-4E4B-8D2C-A53B8D9A7272")]
    [ProgId("hoTools.ActiveXGUI")]
    [ComDefaultInterface(typeof(IAddinControl))]

    public class AddinControlGui : AddinGUI, IAddinControl
    {
        public const string Progid = "hoTools.ActiveXGUI";

        // Windows/Frames
        FrmQueryAndScript _frmQueryAndScript;
        FrmSettingsGeneral _frmSettingsGeneral;

        FrmSettingsToolbar _frmSettingsToolbar;
        FrmSettingsKey _frmSettingsKey;
        FrmSettingsLineStyle _frmSettingsLineStyle;

        HoToolsGlobalCfg _globalCfg;


        #region Generated

        private Button _btnLh;
        private Button _btnLv;
        private Button _btnTv;
        private Button _btnTh;
        private Button _btnOs;
        private ToolTip _toolTip;
        private System.ComponentModel.IContainer components;
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
        public override EA.Repository Repository
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
                    MessageBox.Show(e.ToString(), @"ActiveX: Error Initialization");
                }

            }
        }
        #region initializingSettings
        /// <summary>
        /// Initialize Setting (not Keys). Be sure Repository is loaded! Also don't change the sequence of hide/visible.
        /// </summary>
        public void InitializeSettings()
        {
            // get global settings
            _globalCfg = HoToolsGlobalCfg.Instance;
            ParameterizeMenusAndButtons();
            // parameterize 5 Buttons to quickly run search
            ParameterizeSearchButton();
            // parameterize 5 Buttons to quickly run services
            ParameterizeServiceButton();
        }
        #endregion

        #region IActiveX Members
        public string getName() => "hoTools.AddinControl";


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
                    EaService.AboutVar1(Release, configFilePath);
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



        void toolStripServiceBtn1_Click(object sender, EventArgs e)
        {
            RunService(0);
        }

        void toolStripServiceBtn2_Click(object sender, EventArgs e)
        {
            RunService(1);
        }
        void toolStripServiceBtn3_Click(object sender, EventArgs e)
        {
            RunService(2);
        }

        void toolStripServiceBtn4_Click(object sender, EventArgs e)
        {
            RunService(3);
        }

        void toolStripServiceBtn5_Click(object sender, EventArgs e)
        {
            RunService(4);
        }

        void toolStripSearchBtn1_Click(object sender, EventArgs e)
        {
            RunSearch(0);
        }

        void toolStripSearchBtn2_Click(object sender, EventArgs e)
        {
            RunSearch(1);
        }

        void toolStripSearchBtn3_Click(object sender, EventArgs e)
        {
            RunSearch(2);
        }

        void toolStripSearchBtn4_Click(object sender, EventArgs e)
        {
            RunSearch(3);
        }

        void toolStripSearchBtn5_Click(object sender, EventArgs e)
        {
            RunSearch(4);
        }
        /// <summary>
        /// Run search according to configuration.
        /// </summary>
        /// <param name="pos"></param>
        void RunSearch(int pos)
        {
            if (AddinSettings.ButtonsSearch[pos] is EaAddinShortcutSearch)
            {

                var button = (EaAddinShortcutSearch)AddinSettings.ButtonsSearch[pos];
                string searchName = button.KeySearchName.Trim();
                if (searchName == "") return;

               RunSearch(searchName, button.KeySearchTerm.Trim() );

            }
        }
        /// <summary>
        /// Run a search. Depending of the Search name is runs the SQL Search or the EA Model Search.
        /// </summary>
        public void RunSearch(string searchName, string searchTerm)
        {
            if (searchName == "") return;

            // SQL file?
            Regex pattern = new Regex(@"\.sql", RegexOptions.IgnoreCase);
            if (pattern.IsMatch(searchName))
            {
                string sqlString = _globalCfg.GetSqlFilePathFromName(searchName);

                // run search
                Model.SqlRun(sqlString, searchTerm);


            }
            else
            {
                try
                {
                    Repository.RunModelSearch(searchName, searchName, "", "");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(),
                        $"Error start search '{searchName} {searchName}'");
                }
            }
        }
        void RunService(int pos)
        {
            if (AddinSettings.ButtonsServices[pos] is hoTools.EaServices.ServicesCallConfig)
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
            port.RemovePortFromDiagramGui();

        }
        #endregion

        #region showPortsInDiagramObjects
        void showPortsInDiagramObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ShowPortsInDiagram(false);


        }
        #endregion
        #region showReceivingPortsLeftSendingPortsRight
        void showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ShowPortsInDiagram(true);
        }
        #endregion

        #region copyPorts
        void copyPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.CopyPortsGui();

        }
        #endregion
        #region deletePortsWhichAreMarkedForDeletion
#pragma warning disable RECS0154
        void deletePortsWhichAreMarkedForDeletionfutureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.DeletePortsMarkedPorts();
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
            port.ChangeLabelGui(PortServices.LabelStyle.IsHidden);
        }
        #endregion

        #region viewPortLabelToolStripMenuItem_Click
        void viewPortLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ChangeLabelGui(PortServices.LabelStyle.IsShown);
        }
        #endregion
        #region movePortLableLeftPositionToolStripMenuItem_Click
        void movePortLableLeftPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ChangeLabelGui(PortServices.LabelStyle.PositionLeft);
        }
        #endregion

        #region movePortLableRightPositionToolStripMenuItem_Click
        void movePortLableRightPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ChangeLabelGui(PortServices.LabelStyle.PositionRight);
        }
        #endregion


        #region movePortLablePlusPositionToolStripMenuItem_Click
        void movePortLablePlusPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ChangeLabelGui(PortServices.LabelStyle.PositionPlus);
        }
        #endregion


        #region movePortLableMinusPositionToolStripMenuItem_Click
        void movePortLableMinusPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ChangeLabelGui(PortServices.LabelStyle.PositionMinus);
        }
        #endregion


        void connectPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ConnectPortsGui();

        }

        /// <summary>
        /// Connect ports with the same name in a component / class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void connectPortsInsideComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.ConnectPortsInsideGui();
        }

        void deletePortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.DeletePortsGui();
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
            port.SetConnectionDirectionUnspecifiedGui();
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
                RunSearch(GetSearchName(), _txtSearchText.Text);
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
            RunSearch(GetSearchName(), _txtSearchText.Text);
        }

        /// <summary>
        /// Double Mouse Click in SearchName runs the query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtSearchName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RunSearch(GetSearchName(), _txtSearchText.Text);
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinControlGui));
            _toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            _toolStripQuery = new System.Windows.Forms.ToolStrip();
            _toolStripSearchBtn1 = new System.Windows.Forms.ToolStripButton();
            _toolStripSearchBtn2 = new System.Windows.Forms.ToolStripButton();
            _toolStripSearchBtn3 = new System.Windows.Forms.ToolStripButton();
            _toolStripSearchBtn4 = new System.Windows.Forms.ToolStripButton();
            _toolStripSearchBtn5 = new System.Windows.Forms.ToolStripButton();
            _toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            _toolStripServiceBtn1 = new System.Windows.Forms.ToolStripButton();
            _toolStripServiceBtn2 = new System.Windows.Forms.ToolStripButton();
            _toolStripServiceBtn3 = new System.Windows.Forms.ToolStripButton();
            _toolStripServiceBtn4 = new System.Windows.Forms.ToolStripButton();
            _toolStripServiceBtn5 = new System.Windows.Forms.ToolStripButton();
            _toolTip = new System.Windows.Forms.ToolTip(components);
            _txtSearchText = new System.Windows.Forms.TextBox();
            _btnLabelRight = new System.Windows.Forms.Button();
            _btnLabelLeft = new System.Windows.Forms.Button();
            _btnUp = new System.Windows.Forms.Button();
            _btnDown = new System.Windows.Forms.Button();
            _btnLeft = new System.Windows.Forms.Button();
            _btnRight = new System.Windows.Forms.Button();
            _btnShowFavorites = new System.Windows.Forms.Button();
            _btnRemoveFavorite = new System.Windows.Forms.Button();
            _btnAddFavorite = new System.Windows.Forms.Button();
            _btnBezier = new System.Windows.Forms.Button();
            _btnUpdateActivityParameter = new System.Windows.Forms.Button();
            _btnC = new System.Windows.Forms.Button();
            _btnD = new System.Windows.Forms.Button();
            _btnA = new System.Windows.Forms.Button();
            _btnOr = new System.Windows.Forms.Button();
            _btnComposite = new System.Windows.Forms.Button();
            _btnDisplaySpecification = new System.Windows.Forms.Button();
            _btnFindUsage = new System.Windows.Forms.Button();
            _btnLocateType = new System.Windows.Forms.Button();
            _btnLocateOperation = new System.Windows.Forms.Button();
            _btnDisplayBehavior = new System.Windows.Forms.Button();
            _btnOs = new System.Windows.Forms.Button();
            _btnTv = new System.Windows.Forms.Button();
            _btnTh = new System.Windows.Forms.Button();
            _btnLv = new System.Windows.Forms.Button();
            _btnLh = new System.Windows.Forms.Button();
            _txtSearchName = new System.Windows.Forms.TextBox();
            _btnConveyedItemConnector = new System.Windows.Forms.Button();
            _btnConveyedItemElement = new System.Windows.Forms.Button();
            _panelConveyedItems = new System.Windows.Forms.Panel();
            _lblConveyedItems = new System.Windows.Forms.Label();
            _btnAddDiagramNote = new System.Windows.Forms.Button();
            _btnAddElementNote = new System.Windows.Forms.Button();
            _menuStrip1 = new System.Windows.Forms.MenuStrip();
            _fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _settingGeneralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _settingsGlobalKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _settingsToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _settingsQueryAndSctipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _doToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _createActivityForOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _updateActivityFromOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            _showFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _copyGuidsqlToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            _changeAuthorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _changeAuthorRecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _versionControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _changeXmlFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _showFolderVCorCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _getVcLatesrecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            _showTortoiseLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _showTortoiseRepoBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _setSvnKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _setSvnModuleTaggedValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _movePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            _deletePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            _connectPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _connectPortsInsideComponentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            _showPortsInDiagramObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _hidePortsInDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            _unhidePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _hidePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            _movePortLabelLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _movePortLabelRightPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            _movePortLabelLeftToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            _movePortLabelRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            _orderDiagramItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _getLastSqlErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            _panelButtons = new System.Windows.Forms.Panel();
            _panelLineStyle = new System.Windows.Forms.Panel();
            _panelFavorite = new System.Windows.Forms.Panel();
            _panelNote = new System.Windows.Forms.Panel();
            _panelPort = new System.Windows.Forms.Panel();
            _lblPorts = new System.Windows.Forms.Label();
            _panelAdvanced = new System.Windows.Forms.Panel();
            _panelQuickSearch = new System.Windows.Forms.TableLayoutPanel();
            _toolStripContainer1.TopToolStripPanel.SuspendLayout();
            _toolStripContainer1.SuspendLayout();
            _toolStripQuery.SuspendLayout();
            _panelConveyedItems.SuspendLayout();
            _menuStrip1.SuspendLayout();
            _panelButtons.SuspendLayout();
            _panelLineStyle.SuspendLayout();
            _panelFavorite.SuspendLayout();
            _panelNote.SuspendLayout();
            _panelPort.SuspendLayout();
            _panelAdvanced.SuspendLayout();
            _panelQuickSearch.SuspendLayout();
            SuspendLayout();
            // 
            // _toolStripContainer1
            // 
            _toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // _toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(_toolStripContainer1.ContentPanel, "_toolStripContainer1.ContentPanel");
            resources.ApplyResources(_toolStripContainer1, "_toolStripContainer1");
            _toolStripContainer1.LeftToolStripPanelVisible = false;
            _toolStripContainer1.Name = "_toolStripContainer1";
            _toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // _toolStripContainer1.TopToolStripPanel
            // 
            _toolStripContainer1.TopToolStripPanel.Controls.Add(_toolStripQuery);
            // 
            // _toolStripQuery
            // 
            resources.ApplyResources(_toolStripQuery, "_toolStripQuery");
            _toolStripQuery.ImageScalingSize = new System.Drawing.Size(20, 20);
            _toolStripQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            // _toolStripSearchBtn1
            // 
            _toolStripSearchBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn1, "_toolStripSearchBtn1");
            _toolStripSearchBtn1.Name = "_toolStripSearchBtn1";
            _toolStripSearchBtn1.Click += new System.EventHandler(toolStripSearchBtn1_Click);
            // 
            // _toolStripSearchBtn2
            // 
            _toolStripSearchBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _toolStripSearchBtn2.Name = "_toolStripSearchBtn2";
            resources.ApplyResources(_toolStripSearchBtn2, "_toolStripSearchBtn2");
            _toolStripSearchBtn2.Click += new System.EventHandler(toolStripSearchBtn2_Click);
            // 
            // _toolStripSearchBtn3
            // 
            _toolStripSearchBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn3, "_toolStripSearchBtn3");
            _toolStripSearchBtn3.Name = "_toolStripSearchBtn3";
            _toolStripSearchBtn3.Click += new System.EventHandler(toolStripSearchBtn3_Click);
            // 
            // _toolStripSearchBtn4
            // 
            _toolStripSearchBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn4, "_toolStripSearchBtn4");
            _toolStripSearchBtn4.Name = "_toolStripSearchBtn4";
            _toolStripSearchBtn4.Click += new System.EventHandler(toolStripSearchBtn4_Click);
            // 
            // _toolStripSearchBtn5
            // 
            _toolStripSearchBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripSearchBtn5, "_toolStripSearchBtn5");
            _toolStripSearchBtn5.Name = "_toolStripSearchBtn5";
            _toolStripSearchBtn5.Click += new System.EventHandler(toolStripSearchBtn5_Click);
            // 
            // _toolStripSeparator6
            // 
            _toolStripSeparator6.Name = "_toolStripSeparator6";
            resources.ApplyResources(_toolStripSeparator6, "_toolStripSeparator6");
            // 
            // _toolStripServiceBtn1
            // 
            _toolStripServiceBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn1, "_toolStripServiceBtn1");
            _toolStripServiceBtn1.Name = "_toolStripServiceBtn1";
            _toolStripServiceBtn1.Click += new System.EventHandler(toolStripServiceBtn1_Click);
            // 
            // _toolStripServiceBtn2
            // 
            _toolStripServiceBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn2, "_toolStripServiceBtn2");
            _toolStripServiceBtn2.Name = "_toolStripServiceBtn2";
            _toolStripServiceBtn2.Click += new System.EventHandler(toolStripServiceBtn2_Click);
            // 
            // _toolStripServiceBtn3
            // 
            _toolStripServiceBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn3, "_toolStripServiceBtn3");
            _toolStripServiceBtn3.Name = "_toolStripServiceBtn3";
            _toolStripServiceBtn3.Click += new System.EventHandler(toolStripServiceBtn3_Click);
            // 
            // _toolStripServiceBtn4
            // 
            _toolStripServiceBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn4, "_toolStripServiceBtn4");
            _toolStripServiceBtn4.Name = "_toolStripServiceBtn4";
            _toolStripServiceBtn4.Click += new System.EventHandler(toolStripServiceBtn4_Click);
            // 
            // _toolStripServiceBtn5
            // 
            _toolStripServiceBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(_toolStripServiceBtn5, "_toolStripServiceBtn5");
            _toolStripServiceBtn5.Name = "_toolStripServiceBtn5";
            _toolStripServiceBtn5.Click += new System.EventHandler(toolStripServiceBtn5_Click);
            // 
            // _txtSearchText
            // 
            resources.ApplyResources(_txtSearchText, "_txtSearchText");
            _txtSearchText.Name = "_txtSearchText";
            _toolTip.SetToolTip(_txtSearchText, resources.GetString("_txtSearchText.ToolTip"));
            _txtSearchText.KeyUp += new System.Windows.Forms.KeyEventHandler(txtUserText_KeyDown);
            _txtSearchText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(txtSearchText_MouseDoubleClick);
            // 
            // _btnLabelRight
            // 
            resources.ApplyResources(_btnLabelRight, "_btnLabelRight");
            _btnLabelRight.Name = "_btnLabelRight";
            _toolTip.SetToolTip(_btnLabelRight, resources.GetString("_btnLabelRight.ToolTip"));
            _btnLabelRight.UseVisualStyleBackColor = true;
            _btnLabelRight.Click += new System.EventHandler(movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // _btnLabelLeft
            // 
            resources.ApplyResources(_btnLabelLeft, "_btnLabelLeft");
            _btnLabelLeft.Name = "_btnLabelLeft";
            _toolTip.SetToolTip(_btnLabelLeft, resources.GetString("_btnLabelLeft.ToolTip"));
            _btnLabelLeft.UseVisualStyleBackColor = true;
            _btnLabelLeft.Click += new System.EventHandler(movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // _btnUp
            // 
            resources.ApplyResources(_btnUp, "_btnUp");
            _btnUp.Name = "_btnUp";
            _toolTip.SetToolTip(_btnUp, resources.GetString("_btnUp.ToolTip"));
            _btnUp.UseVisualStyleBackColor = true;
            _btnUp.Click += new System.EventHandler(btnUp_Click);
            // 
            // _btnDown
            // 
            resources.ApplyResources(_btnDown, "_btnDown");
            _btnDown.Name = "_btnDown";
            _toolTip.SetToolTip(_btnDown, resources.GetString("_btnDown.ToolTip"));
            _btnDown.UseVisualStyleBackColor = true;
            _btnDown.Click += new System.EventHandler(btnDown_Click);
            // 
            // _btnLeft
            // 
            resources.ApplyResources(_btnLeft, "_btnLeft");
            _btnLeft.Name = "_btnLeft";
            _toolTip.SetToolTip(_btnLeft, resources.GetString("_btnLeft.ToolTip"));
            _btnLeft.UseVisualStyleBackColor = true;
            _btnLeft.Click += new System.EventHandler(btnLeft_Click);
            // 
            // _btnRight
            // 
            resources.ApplyResources(_btnRight, "_btnRight");
            _btnRight.Name = "_btnRight";
            _toolTip.SetToolTip(_btnRight, resources.GetString("_btnRight.ToolTip"));
            _btnRight.UseVisualStyleBackColor = true;
            _btnRight.Click += new System.EventHandler(btnRight_Click);
            // 
            // _btnShowFavorites
            // 
            resources.ApplyResources(_btnShowFavorites, "_btnShowFavorites");
            _btnShowFavorites.Name = "_btnShowFavorites";
            _toolTip.SetToolTip(_btnShowFavorites, resources.GetString("_btnShowFavorites.ToolTip"));
            _btnShowFavorites.UseVisualStyleBackColor = true;
            _btnShowFavorites.Click += new System.EventHandler(btnFavorites_Click);
            // 
            // _btnRemoveFavorite
            // 
            resources.ApplyResources(_btnRemoveFavorite, "_btnRemoveFavorite");
            _btnRemoveFavorite.Name = "_btnRemoveFavorite";
            _toolTip.SetToolTip(_btnRemoveFavorite, resources.GetString("_btnRemoveFavorite.ToolTip"));
            _btnRemoveFavorite.UseVisualStyleBackColor = true;
            _btnRemoveFavorite.Click += new System.EventHandler(btnRemoveFavorite_Click);
            // 
            // _btnAddFavorite
            // 
            resources.ApplyResources(_btnAddFavorite, "_btnAddFavorite");
            _btnAddFavorite.Name = "_btnAddFavorite";
            _toolTip.SetToolTip(_btnAddFavorite, resources.GetString("_btnAddFavorite.ToolTip"));
            _btnAddFavorite.UseVisualStyleBackColor = true;
            _btnAddFavorite.Click += new System.EventHandler(btnAddFavorite_Click);
            // 
            // _btnBezier
            // 
            resources.ApplyResources(_btnBezier, "_btnBezier");
            _btnBezier.Name = "_btnBezier";
            _toolTip.SetToolTip(_btnBezier, resources.GetString("_btnBezier.ToolTip"));
            _btnBezier.UseVisualStyleBackColor = true;
            _btnBezier.Click += new System.EventHandler(btnBezier_Click);
            // 
            // _btnUpdateActivityParameter
            // 
            resources.ApplyResources(_btnUpdateActivityParameter, "_btnUpdateActivityParameter");
            _btnUpdateActivityParameter.Name = "_btnUpdateActivityParameter";
            _toolTip.SetToolTip(_btnUpdateActivityParameter, resources.GetString("_btnUpdateActivityParameter.ToolTip"));
            _btnUpdateActivityParameter.UseVisualStyleBackColor = true;
            _btnUpdateActivityParameter.Click += new System.EventHandler(btnUpdateActivityParametzer_Click);
            // 
            // _btnC
            // 
            resources.ApplyResources(_btnC, "_btnC");
            _btnC.Name = "_btnC";
            _toolTip.SetToolTip(_btnC, resources.GetString("_btnC.ToolTip"));
            _btnC.UseVisualStyleBackColor = true;
            _btnC.Click += new System.EventHandler(btnC_Click);
            // 
            // _btnD
            // 
            resources.ApplyResources(_btnD, "_btnD");
            _btnD.Name = "_btnD";
            _toolTip.SetToolTip(_btnD, resources.GetString("_btnD.ToolTip"));
            _btnD.UseVisualStyleBackColor = true;
            _btnD.Click += new System.EventHandler(btnD_Click);
            // 
            // _btnA
            // 
            resources.ApplyResources(_btnA, "_btnA");
            _btnA.Name = "_btnA";
            _toolTip.SetToolTip(_btnA, resources.GetString("_btnA.ToolTip"));
            _btnA.UseVisualStyleBackColor = true;
            _btnA.Click += new System.EventHandler(btnA_Click);
            // 
            // _btnOr
            // 
            resources.ApplyResources(_btnOr, "_btnOr");
            _btnOr.Name = "_btnOr";
            _toolTip.SetToolTip(_btnOr, resources.GetString("_btnOr.ToolTip"));
            _btnOr.UseVisualStyleBackColor = true;
            _btnOr.Click += new System.EventHandler(btnOR_Click);
            // 
            // _btnComposite
            // 
            resources.ApplyResources(_btnComposite, "_btnComposite");
            _btnComposite.Name = "_btnComposite";
            _toolTip.SetToolTip(_btnComposite, resources.GetString("_btnComposite.ToolTip"));
            _btnComposite.UseVisualStyleBackColor = true;
            _btnComposite.Click += new System.EventHandler(btnComposite_Click);
            // 
            // _btnDisplaySpecification
            // 
            resources.ApplyResources(_btnDisplaySpecification, "_btnDisplaySpecification");
            _btnDisplaySpecification.Name = "_btnDisplaySpecification";
            _toolTip.SetToolTip(_btnDisplaySpecification, resources.GetString("_btnDisplaySpecification.ToolTip"));
            _btnDisplaySpecification.UseVisualStyleBackColor = true;
            _btnDisplaySpecification.Click += new System.EventHandler(btnShowSpecification_Click);
            // 
            // _btnFindUsage
            // 
            resources.ApplyResources(_btnFindUsage, "_btnFindUsage");
            _btnFindUsage.Name = "_btnFindUsage";
            _toolTip.SetToolTip(_btnFindUsage, resources.GetString("_btnFindUsage.ToolTip"));
            _btnFindUsage.UseVisualStyleBackColor = true;
            _btnFindUsage.Click += new System.EventHandler(btnFindUsage_Click);
            // 
            // _btnLocateType
            // 
            resources.ApplyResources(_btnLocateType, "_btnLocateType");
            _btnLocateType.Name = "_btnLocateType";
            _toolTip.SetToolTip(_btnLocateType, resources.GetString("_btnLocateType.ToolTip"));
            _btnLocateType.UseVisualStyleBackColor = true;
            _btnLocateType.Click += new System.EventHandler(btnLocateType_Click);
            // 
            // _btnLocateOperation
            // 
            resources.ApplyResources(_btnLocateOperation, "_btnLocateOperation");
            _btnLocateOperation.Name = "_btnLocateOperation";
            _toolTip.SetToolTip(_btnLocateOperation, resources.GetString("_btnLocateOperation.ToolTip"));
            _btnLocateOperation.UseVisualStyleBackColor = true;
            _btnLocateOperation.Click += new System.EventHandler(btnLocateOperation_Click);
            // 
            // _btnDisplayBehavior
            // 
            resources.ApplyResources(_btnDisplayBehavior, "_btnDisplayBehavior");
            _btnDisplayBehavior.Name = "_btnDisplayBehavior";
            _toolTip.SetToolTip(_btnDisplayBehavior, resources.GetString("_btnDisplayBehavior.ToolTip"));
            _btnDisplayBehavior.UseVisualStyleBackColor = true;
            _btnDisplayBehavior.Click += new System.EventHandler(btnDisplayBehavior_Click);
            // 
            // _btnOs
            // 
            resources.ApplyResources(_btnOs, "_btnOs");
            _btnOs.Name = "_btnOs";
            _toolTip.SetToolTip(_btnOs, resources.GetString("_btnOs.ToolTip"));
            _btnOs.UseVisualStyleBackColor = true;
            _btnOs.Click += new System.EventHandler(btnOS_Click);
            // 
            // _btnTv
            // 
            resources.ApplyResources(_btnTv, "_btnTv");
            _btnTv.Name = "_btnTv";
            _toolTip.SetToolTip(_btnTv, resources.GetString("_btnTv.ToolTip"));
            _btnTv.UseVisualStyleBackColor = true;
            _btnTv.Click += new System.EventHandler(btnTV_Click);
            // 
            // _btnTh
            // 
            resources.ApplyResources(_btnTh, "_btnTh");
            _btnTh.Name = "_btnTh";
            _toolTip.SetToolTip(_btnTh, resources.GetString("_btnTh.ToolTip"));
            _btnTh.UseVisualStyleBackColor = true;
            _btnTh.Click += new System.EventHandler(btnTH_Click);
            // 
            // _btnLv
            // 
            resources.ApplyResources(_btnLv, "_btnLv");
            _btnLv.Name = "_btnLv";
            _toolTip.SetToolTip(_btnLv, resources.GetString("_btnLv.ToolTip"));
            _btnLv.UseVisualStyleBackColor = true;
            _btnLv.Click += new System.EventHandler(btnLV_Click);
            // 
            // _btnLh
            // 
            resources.ApplyResources(_btnLh, "_btnLh");
            _btnLh.Name = "_btnLh";
            _toolTip.SetToolTip(_btnLh, resources.GetString("_btnLh.ToolTip"));
            _btnLh.UseVisualStyleBackColor = true;
            _btnLh.Click += new System.EventHandler(btnLH_Click);
            // 
            // _txtSearchName
            // 
            _txtSearchName.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(_txtSearchName, "_txtSearchName");
            _txtSearchName.Name = "_txtSearchName";
            _toolTip.SetToolTip(_txtSearchName, resources.GetString("_txtSearchName.ToolTip"));
            _txtSearchName.KeyDown += new System.Windows.Forms.KeyEventHandler(txtUserText_KeyDown);
            _txtSearchName.KeyUp += new System.Windows.Forms.KeyEventHandler(txtUserText_KeyDown);
            _txtSearchName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(txtSearchName_MouseDoubleClick);
            // 
            // _btnConveyedItemConnector
            // 
            resources.ApplyResources(_btnConveyedItemConnector, "_btnConveyedItemConnector");
            _btnConveyedItemConnector.Name = "_btnConveyedItemConnector";
            _toolTip.SetToolTip(_btnConveyedItemConnector, resources.GetString("_btnConveyedItemConnector.ToolTip"));
            _btnConveyedItemConnector.UseVisualStyleBackColor = true;
            _btnConveyedItemConnector.Click += new System.EventHandler(btnConveyedItemConnector_Click);
            // 
            // _btnConveyedItemElement
            // 
            resources.ApplyResources(_btnConveyedItemElement, "_btnConveyedItemElement");
            _btnConveyedItemElement.Name = "_btnConveyedItemElement";
            _toolTip.SetToolTip(_btnConveyedItemElement, resources.GetString("_btnConveyedItemElement.ToolTip"));
            _btnConveyedItemElement.UseVisualStyleBackColor = true;
            _btnConveyedItemElement.Click += new System.EventHandler(btnConveyedItemElement_Click);
            // 
            // _panelConveyedItems
            // 
            _panelConveyedItems.Controls.Add(_lblConveyedItems);
            _panelConveyedItems.Controls.Add(_btnConveyedItemElement);
            _panelConveyedItems.Controls.Add(_btnConveyedItemConnector);
            resources.ApplyResources(_panelConveyedItems, "_panelConveyedItems");
            _panelConveyedItems.Name = "_panelConveyedItems";
            _toolTip.SetToolTip(_panelConveyedItems, resources.GetString("_panelConveyedItems.ToolTip"));
            // 
            // _lblConveyedItems
            // 
            resources.ApplyResources(_lblConveyedItems, "_lblConveyedItems");
            _lblConveyedItems.Name = "_lblConveyedItems";
            // 
            // _btnAddDiagramNote
            // 
            resources.ApplyResources(_btnAddDiagramNote, "_btnAddDiagramNote");
            _btnAddDiagramNote.Name = "_btnAddDiagramNote";
            _btnAddDiagramNote.UseVisualStyleBackColor = true;
            _btnAddDiagramNote.Click += new System.EventHandler(btnAddDiagramNote_Click);
            // 
            // _btnAddElementNote
            // 
            resources.ApplyResources(_btnAddElementNote, "_btnAddElementNote");
            _btnAddElementNote.Name = "_btnAddElementNote";
            _btnAddElementNote.UseVisualStyleBackColor = true;
            _btnAddElementNote.Click += new System.EventHandler(btnAddElementNote_Click);
            // 
            // _menuStrip1
            // 
            _menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            _menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _fileToolStripMenuItem,
            _doToolStripMenuItem,
            _versionControlToolStripMenuItem,
            _portToolStripMenuItem,
            _helpToolStripMenuItem});
            resources.ApplyResources(_menuStrip1, "_menuStrip1");
            _menuStrip1.Name = "_menuStrip1";
            // 
            // _fileToolStripMenuItem
            // 
            _fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _settingGeneralToolStripMenuItem,
            _settingsToolStripMenuItem,
            _settingsGlobalKeysToolStripMenuItem,
            _settingsToolbarToolStripMenuItem,
            _settingsQueryAndSctipToolStripMenuItem});
            _fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
            resources.ApplyResources(_fileToolStripMenuItem, "_fileToolStripMenuItem");
            // 
            // _settingGeneralToolStripMenuItem
            // 
            _settingGeneralToolStripMenuItem.Name = "_settingGeneralToolStripMenuItem";
            resources.ApplyResources(_settingGeneralToolStripMenuItem, "_settingGeneralToolStripMenuItem");
            _settingGeneralToolStripMenuItem.Click += new System.EventHandler(settingGeneralToolStripMenuItem_Click);
            // 
            // _settingsToolStripMenuItem
            // 
            _settingsToolStripMenuItem.Name = "_settingsToolStripMenuItem";
            resources.ApplyResources(_settingsToolStripMenuItem, "_settingsToolStripMenuItem");
            _settingsToolStripMenuItem.Click += new System.EventHandler(settingsToolStripMenuItem_Click);
            // 
            // _settingsGlobalKeysToolStripMenuItem
            // 
            _settingsGlobalKeysToolStripMenuItem.Name = "_settingsGlobalKeysToolStripMenuItem";
            resources.ApplyResources(_settingsGlobalKeysToolStripMenuItem, "_settingsGlobalKeysToolStripMenuItem");
            _settingsGlobalKeysToolStripMenuItem.Click += new System.EventHandler(settingsKeysToolStripMenuItem_Click);
            // 
            // _settingsToolbarToolStripMenuItem
            // 
            _settingsToolbarToolStripMenuItem.Name = "_settingsToolbarToolStripMenuItem";
            resources.ApplyResources(_settingsToolbarToolStripMenuItem, "_settingsToolbarToolStripMenuItem");
            _settingsToolbarToolStripMenuItem.Click += new System.EventHandler(settingsToolbarToolStripMenuItem_Click);
            // 
            // _settingsQueryAndSctipToolStripMenuItem
            // 
            _settingsQueryAndSctipToolStripMenuItem.Name = "_settingsQueryAndSctipToolStripMenuItem";
            resources.ApplyResources(_settingsQueryAndSctipToolStripMenuItem, "_settingsQueryAndSctipToolStripMenuItem");
            _settingsQueryAndSctipToolStripMenuItem.Click += new System.EventHandler(settingsQueryAndSctipToolStripMenuItem_Click);
            // 
            // _doToolStripMenuItem
            // 
            _doToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            // _createActivityForOperationToolStripMenuItem
            // 
            _createActivityForOperationToolStripMenuItem.Name = "_createActivityForOperationToolStripMenuItem";
            resources.ApplyResources(_createActivityForOperationToolStripMenuItem, "_createActivityForOperationToolStripMenuItem");
            _createActivityForOperationToolStripMenuItem.Click += new System.EventHandler(createActivityForOperationToolStripMenuItem_Click);
            // 
            // _updateActivityFromOperationToolStripMenuItem
            // 
            _updateActivityFromOperationToolStripMenuItem.Name = "_updateActivityFromOperationToolStripMenuItem";
            resources.ApplyResources(_updateActivityFromOperationToolStripMenuItem, "_updateActivityFromOperationToolStripMenuItem");
            _updateActivityFromOperationToolStripMenuItem.Click += new System.EventHandler(updateActivityFromOperationToolStripMenuItem_Click);
            // 
            // _toolStripSeparator10
            // 
            _toolStripSeparator10.Name = "_toolStripSeparator10";
            resources.ApplyResources(_toolStripSeparator10, "_toolStripSeparator10");
            // 
            // _showFolderToolStripMenuItem
            // 
            _showFolderToolStripMenuItem.Name = "_showFolderToolStripMenuItem";
            resources.ApplyResources(_showFolderToolStripMenuItem, "_showFolderToolStripMenuItem");
            _showFolderToolStripMenuItem.Click += new System.EventHandler(showFolderToolStripMenuItem_Click);
            // 
            // _copyGuidsqlToClipboardToolStripMenuItem
            // 
            _copyGuidsqlToClipboardToolStripMenuItem.Name = "_copyGuidsqlToClipboardToolStripMenuItem";
            resources.ApplyResources(_copyGuidsqlToClipboardToolStripMenuItem, "_copyGuidsqlToClipboardToolStripMenuItem");
            _copyGuidsqlToClipboardToolStripMenuItem.Click += new System.EventHandler(copyGUIDSQLToClipboardToolStripMenuItem_Click);
            // 
            // _toolStripSeparator1
            // 
            _toolStripSeparator1.Name = "_toolStripSeparator1";
            resources.ApplyResources(_toolStripSeparator1, "_toolStripSeparator1");
            // 
            // _changeAuthorToolStripMenuItem
            // 
            _changeAuthorToolStripMenuItem.Name = "_changeAuthorToolStripMenuItem";
            resources.ApplyResources(_changeAuthorToolStripMenuItem, "_changeAuthorToolStripMenuItem");
            _changeAuthorToolStripMenuItem.Click += new System.EventHandler(changeAuthorToolStripMenuItem_Click);
            // 
            // _changeAuthorRecursiveToolStripMenuItem
            // 
            _changeAuthorRecursiveToolStripMenuItem.Name = "_changeAuthorRecursiveToolStripMenuItem";
            resources.ApplyResources(_changeAuthorRecursiveToolStripMenuItem, "_changeAuthorRecursiveToolStripMenuItem");
            _changeAuthorRecursiveToolStripMenuItem.Click += new System.EventHandler(changeAuthorRecursiveToolStripMenuItem_Click);
            // 
            // _versionControlToolStripMenuItem
            // 
            _versionControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            // _changeXmlFileToolStripMenuItem
            // 
            _changeXmlFileToolStripMenuItem.Name = "_changeXmlFileToolStripMenuItem";
            resources.ApplyResources(_changeXmlFileToolStripMenuItem, "_changeXmlFileToolStripMenuItem");
            _changeXmlFileToolStripMenuItem.Click += new System.EventHandler(changeXMLFileToolStripMenuItem_Click);
            // 
            // _showFolderVCorCodeToolStripMenuItem
            // 
            _showFolderVCorCodeToolStripMenuItem.Name = "_showFolderVCorCodeToolStripMenuItem";
            resources.ApplyResources(_showFolderVCorCodeToolStripMenuItem, "_showFolderVCorCodeToolStripMenuItem");
            _showFolderVCorCodeToolStripMenuItem.Click += new System.EventHandler(showFolderVCorCodeToolStripMenuItem_Click);
            // 
            // _getVcLatesrecursiveToolStripMenuItem
            // 
            _getVcLatesrecursiveToolStripMenuItem.Name = "_getVcLatesrecursiveToolStripMenuItem";
            resources.ApplyResources(_getVcLatesrecursiveToolStripMenuItem, "_getVcLatesrecursiveToolStripMenuItem");
            _getVcLatesrecursiveToolStripMenuItem.Click += new System.EventHandler(getVCLatesrecursiveToolStripMenuItem_Click);
            // 
            // _toolStripSeparator2
            // 
            _toolStripSeparator2.Name = "_toolStripSeparator2";
            resources.ApplyResources(_toolStripSeparator2, "_toolStripSeparator2");
            // 
            // _showTortoiseLogToolStripMenuItem
            // 
            _showTortoiseLogToolStripMenuItem.Name = "_showTortoiseLogToolStripMenuItem";
            resources.ApplyResources(_showTortoiseLogToolStripMenuItem, "_showTortoiseLogToolStripMenuItem");
            _showTortoiseLogToolStripMenuItem.Click += new System.EventHandler(showTortoiseLogToolStripMenuItem_Click);
            // 
            // _showTortoiseRepoBrowserToolStripMenuItem
            // 
            _showTortoiseRepoBrowserToolStripMenuItem.Name = "_showTortoiseRepoBrowserToolStripMenuItem";
            resources.ApplyResources(_showTortoiseRepoBrowserToolStripMenuItem, "_showTortoiseRepoBrowserToolStripMenuItem");
            _showTortoiseRepoBrowserToolStripMenuItem.Click += new System.EventHandler(showTortoiseRepoBrowserToolStripMenuItem_Click);
            // 
            // _setSvnKeywordsToolStripMenuItem
            // 
            _setSvnKeywordsToolStripMenuItem.Name = "_setSvnKeywordsToolStripMenuItem";
            resources.ApplyResources(_setSvnKeywordsToolStripMenuItem, "_setSvnKeywordsToolStripMenuItem");
            _setSvnKeywordsToolStripMenuItem.Click += new System.EventHandler(setSvnKeywordsToolStripMenuItem_Click);
            // 
            // _setSvnModuleTaggedValuesToolStripMenuItem
            // 
            _setSvnModuleTaggedValuesToolStripMenuItem.Name = "_setSvnModuleTaggedValuesToolStripMenuItem";
            resources.ApplyResources(_setSvnModuleTaggedValuesToolStripMenuItem, "_setSvnModuleTaggedValuesToolStripMenuItem");
            _setSvnModuleTaggedValuesToolStripMenuItem.Click += new System.EventHandler(setSvnModuleTaggedValuesToolStripMenuItem_Click);
            // 
            // _setSvnModuleTaggedValuesrecursiveToolStripMenuItem
            // 
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Name = "_setSvnModuleTaggedValuesrecursiveToolStripMenuItem";
            resources.ApplyResources(_setSvnModuleTaggedValuesrecursiveToolStripMenuItem, "_setSvnModuleTaggedValuesrecursiveToolStripMenuItem");
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Click += new System.EventHandler(setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click);
            // 
            // _portToolStripMenuItem
            // 
            _portToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            // _movePortsToolStripMenuItem
            // 
            _movePortsToolStripMenuItem.Name = "_movePortsToolStripMenuItem";
            resources.ApplyResources(_movePortsToolStripMenuItem, "_movePortsToolStripMenuItem");
            _movePortsToolStripMenuItem.Click += new System.EventHandler(copyPortsToolStripMenuItem_Click);
            // 
            // _toolStripSeparator7
            // 
            _toolStripSeparator7.Name = "_toolStripSeparator7";
            resources.ApplyResources(_toolStripSeparator7, "_toolStripSeparator7");
            // 
            // _deletePortsToolStripMenuItem
            // 
            _deletePortsToolStripMenuItem.Name = "_deletePortsToolStripMenuItem";
            resources.ApplyResources(_deletePortsToolStripMenuItem, "_deletePortsToolStripMenuItem");
            _deletePortsToolStripMenuItem.Click += new System.EventHandler(deletePortsToolStripMenuItem_Click);
            // 
            // _toolStripSeparator3
            // 
            _toolStripSeparator3.Name = "_toolStripSeparator3";
            resources.ApplyResources(_toolStripSeparator3, "_toolStripSeparator3");
            // 
            // _connectPortsToolStripMenuItem
            // 
            _connectPortsToolStripMenuItem.Name = "_connectPortsToolStripMenuItem";
            resources.ApplyResources(_connectPortsToolStripMenuItem, "_connectPortsToolStripMenuItem");
            _connectPortsToolStripMenuItem.Click += new System.EventHandler(connectPortsToolStripMenuItem_Click);
            // 
            // _connectPortsInsideComponentsToolStripMenuItem
            // 
            _connectPortsInsideComponentsToolStripMenuItem.Name = "_connectPortsInsideComponentsToolStripMenuItem";
            resources.ApplyResources(_connectPortsInsideComponentsToolStripMenuItem, "_connectPortsInsideComponentsToolStripMenuItem");
            _connectPortsInsideComponentsToolStripMenuItem.Click += new System.EventHandler(connectPortsInsideComponentsToolStripMenuItem_Click);
            // 
            // _toolStripSeparator8
            // 
            _toolStripSeparator8.Name = "_toolStripSeparator8";
            resources.ApplyResources(_toolStripSeparator8, "_toolStripSeparator8");
            // 
            // _makeConnectorsUnspecifiedDirectionToolStripMenuItem
            // 
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem.Name = "_makeConnectorsUnspecifiedDirectionToolStripMenuItem";
            resources.ApplyResources(_makeConnectorsUnspecifiedDirectionToolStripMenuItem, "_makeConnectorsUnspecifiedDirectionToolStripMenuItem");
            _makeConnectorsUnspecifiedDirectionToolStripMenuItem.Click += new System.EventHandler(makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click);
            // 
            // _toolStripSeparator4
            // 
            _toolStripSeparator4.Name = "_toolStripSeparator4";
            resources.ApplyResources(_toolStripSeparator4, "_toolStripSeparator4");
            // 
            // _showPortsInDiagramObjectsToolStripMenuItem
            // 
            _showPortsInDiagramObjectsToolStripMenuItem.Name = "_showPortsInDiagramObjectsToolStripMenuItem";
            resources.ApplyResources(_showPortsInDiagramObjectsToolStripMenuItem, "_showPortsInDiagramObjectsToolStripMenuItem");
            _showPortsInDiagramObjectsToolStripMenuItem.Click += new System.EventHandler(showPortsInDiagramObjectsToolStripMenuItem_Click);
            // 
            // _showSendingPortsLeftRecievingPortsRightToolStripMenuItem
            // 
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Name = "_showSendingPortsLeftRecievingPortsRightToolStripMenuItem";
            resources.ApplyResources(_showSendingPortsLeftRecievingPortsRightToolStripMenuItem, "_showSendingPortsLeftRecievingPortsRightToolStripMenuItem");
            _showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Click += new System.EventHandler(showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click);
            // 
            // _hidePortsInDiagramToolStripMenuItem
            // 
            _hidePortsInDiagramToolStripMenuItem.Name = "_hidePortsInDiagramToolStripMenuItem";
            resources.ApplyResources(_hidePortsInDiagramToolStripMenuItem, "_hidePortsInDiagramToolStripMenuItem");
            _hidePortsInDiagramToolStripMenuItem.Click += new System.EventHandler(removePortsInDiagramToolStripMenuItem_Click);
            // 
            // _toolStripSeparator5
            // 
            _toolStripSeparator5.Name = "_toolStripSeparator5";
            resources.ApplyResources(_toolStripSeparator5, "_toolStripSeparator5");
            // 
            // _unhidePortsToolStripMenuItem
            // 
            _unhidePortsToolStripMenuItem.Name = "_unhidePortsToolStripMenuItem";
            resources.ApplyResources(_unhidePortsToolStripMenuItem, "_unhidePortsToolStripMenuItem");
            _unhidePortsToolStripMenuItem.Click += new System.EventHandler(viewPortLabelToolStripMenuItem_Click);
            // 
            // _hidePortsToolStripMenuItem
            // 
            _hidePortsToolStripMenuItem.Name = "_hidePortsToolStripMenuItem";
            resources.ApplyResources(_hidePortsToolStripMenuItem, "_hidePortsToolStripMenuItem");
            _hidePortsToolStripMenuItem.Click += new System.EventHandler(hidePortLabelToolStripMenuItem_Click);
            // 
            // _toolStripSeparator11
            // 
            _toolStripSeparator11.Name = "_toolStripSeparator11";
            resources.ApplyResources(_toolStripSeparator11, "_toolStripSeparator11");
            // 
            // _movePortLabelLeftToolStripMenuItem
            // 
            _movePortLabelLeftToolStripMenuItem.Name = "_movePortLabelLeftToolStripMenuItem";
            resources.ApplyResources(_movePortLabelLeftToolStripMenuItem, "_movePortLabelLeftToolStripMenuItem");
            _movePortLabelLeftToolStripMenuItem.Click += new System.EventHandler(movePortLableLeftPositionToolStripMenuItem_Click);
            // 
            // _movePortLabelRightPositionToolStripMenuItem
            // 
            _movePortLabelRightPositionToolStripMenuItem.Name = "_movePortLabelRightPositionToolStripMenuItem";
            resources.ApplyResources(_movePortLabelRightPositionToolStripMenuItem, "_movePortLabelRightPositionToolStripMenuItem");
            _movePortLabelRightPositionToolStripMenuItem.Click += new System.EventHandler(movePortLableRightPositionToolStripMenuItem_Click);
            // 
            // _toolStripSeparator12
            // 
            _toolStripSeparator12.Name = "_toolStripSeparator12";
            resources.ApplyResources(_toolStripSeparator12, "_toolStripSeparator12");
            // 
            // _movePortLabelLeftToolStripMenuItem1
            // 
            _movePortLabelLeftToolStripMenuItem1.Name = "_movePortLabelLeftToolStripMenuItem1";
            resources.ApplyResources(_movePortLabelLeftToolStripMenuItem1, "_movePortLabelLeftToolStripMenuItem1");
            _movePortLabelLeftToolStripMenuItem1.Click += new System.EventHandler(movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // _movePortLabelRightToolStripMenuItem
            // 
            _movePortLabelRightToolStripMenuItem.Name = "_movePortLabelRightToolStripMenuItem";
            resources.ApplyResources(_movePortLabelRightToolStripMenuItem, "_movePortLabelRightToolStripMenuItem");
            _movePortLabelRightToolStripMenuItem.Click += new System.EventHandler(movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // _toolStripSeparator9
            // 
            _toolStripSeparator9.Name = "_toolStripSeparator9";
            resources.ApplyResources(_toolStripSeparator9, "_toolStripSeparator9");
            // 
            // _orderDiagramItemsToolStripMenuItem
            // 
            _orderDiagramItemsToolStripMenuItem.Name = "_orderDiagramItemsToolStripMenuItem";
            resources.ApplyResources(_orderDiagramItemsToolStripMenuItem, "_orderDiagramItemsToolStripMenuItem");
            // 
            // _helpToolStripMenuItem
            // 
            _helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _aboutToolStripMenuItem,
            _getLastSqlErrorToolStripMenuItem,
            _helpToolStripMenuItem1});
            _helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            resources.ApplyResources(_helpToolStripMenuItem, "_helpToolStripMenuItem");
            _helpToolStripMenuItem.Click += new System.EventHandler(helpToolStripMenuItem_Click);
            // 
            // _aboutToolStripMenuItem
            // 
            _aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            resources.ApplyResources(_aboutToolStripMenuItem, "_aboutToolStripMenuItem");
            _aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click);
            // 
            // _getLastSqlErrorToolStripMenuItem
            // 
            _getLastSqlErrorToolStripMenuItem.Name = "_getLastSqlErrorToolStripMenuItem";
            resources.ApplyResources(_getLastSqlErrorToolStripMenuItem, "_getLastSqlErrorToolStripMenuItem");
            _getLastSqlErrorToolStripMenuItem.Click += new System.EventHandler(getLastSQLErrorToolStripMenuItem_Click);
            // 
            // _helpToolStripMenuItem1
            // 
            _helpToolStripMenuItem1.Name = "_helpToolStripMenuItem1";
            resources.ApplyResources(_helpToolStripMenuItem1, "_helpToolStripMenuItem1");
            _helpToolStripMenuItem1.Click += new System.EventHandler(helpToolStripMenuItem1_Click);
            // 
            // _panelButtons
            // 
            _panelButtons.Controls.Add(_toolStripContainer1);
            resources.ApplyResources(_panelButtons, "_panelButtons");
            _panelButtons.Name = "_panelButtons";
            // 
            // _panelLineStyle
            // 
            _panelLineStyle.Controls.Add(_btnLv);
            _panelLineStyle.Controls.Add(_btnLh);
            _panelLineStyle.Controls.Add(_btnTv);
            _panelLineStyle.Controls.Add(_btnTh);
            _panelLineStyle.Controls.Add(_btnC);
            _panelLineStyle.Controls.Add(_btnBezier);
            _panelLineStyle.Controls.Add(_btnOs);
            _panelLineStyle.Controls.Add(_btnOr);
            _panelLineStyle.Controls.Add(_btnA);
            _panelLineStyle.Controls.Add(_btnD);
            resources.ApplyResources(_panelLineStyle, "_panelLineStyle");
            _panelLineStyle.Name = "_panelLineStyle";
            // 
            // _panelFavorite
            // 
            _panelFavorite.Controls.Add(_btnAddFavorite);
            _panelFavorite.Controls.Add(_btnRemoveFavorite);
            _panelFavorite.Controls.Add(_btnShowFavorites);
            _panelFavorite.Controls.Add(_btnDisplayBehavior);
            resources.ApplyResources(_panelFavorite, "_panelFavorite");
            _panelFavorite.Name = "_panelFavorite";
            // 
            // _panelNote
            // 
            _panelNote.Controls.Add(_btnAddElementNote);
            _panelNote.Controls.Add(_btnAddDiagramNote);
            resources.ApplyResources(_panelNote, "_panelNote");
            _panelNote.Name = "_panelNote";
            // 
            // _panelPort
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
            // _lblPorts
            // 
            resources.ApplyResources(_lblPorts, "_lblPorts");
            _lblPorts.Name = "_lblPorts";
            // 
            // _panelAdvanced
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
            // _panelQuickSearch
            // 
            resources.ApplyResources(_panelQuickSearch, "_panelQuickSearch");
            _panelQuickSearch.Controls.Add(_txtSearchName, 0, 0);
            _panelQuickSearch.Controls.Add(_txtSearchText, 0, 0);
            _panelQuickSearch.Name = "_panelQuickSearch";
            // 
            // AddinControlGui
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
            _panelConveyedItems.ResumeLayout(false);
            _panelConveyedItems.PerformLayout();
            _menuStrip1.ResumeLayout(false);
            _menuStrip1.PerformLayout();
            _panelButtons.ResumeLayout(false);
            _panelLineStyle.ResumeLayout(false);
            _panelFavorite.ResumeLayout(false);
            _panelNote.ResumeLayout(false);
            _panelPort.ResumeLayout(false);
            _panelPort.PerformLayout();
            _panelAdvanced.ResumeLayout(false);
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
            bool visibleSvnVc = !(AddinSettings.IsSvnSupport == false | AddinSettings.IsVcSupport == false);
            _showTortoiseRepoBrowserToolStripMenuItem.Visible = visibleSvnVc;
            _showTortoiseLogToolStripMenuItem.Visible = visibleSvnVc;
            _setSvnModuleTaggedValuesToolStripMenuItem.Visible = visibleSvnVc;
            _setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Visible = visibleSvnVc;
            _setSvnKeywordsToolStripMenuItem.Visible = visibleSvnVc;

            // Visible VC
            bool visibleVc = AddinSettings.IsVcSupport;

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
            bool visiblePorts =  AddinSettings.IsAdvancedPort;

            _btnLeft.Visible = visiblePorts;
            _btnRight.Visible = visiblePorts;
            _btnUp.Visible = visiblePorts;
            _btnDown.Visible = visiblePorts;

            _btnLabelLeft.Visible = visiblePorts;
            _btnLabelRight.Visible = visiblePorts;

            // Note in diagram support
            bool visibleDiagramNote = AddinSettings.IsAdvancedDiagramNote;
            _btnAddDiagramNote.Visible = visibleDiagramNote;
            _btnAddElementNote.Visible = visibleDiagramNote;

            // LineStyle
            _btnLv.Visible = AddinSettings.IsLineStyleSupport;
            _btnLh.Visible = AddinSettings.IsLineStyleSupport;
            _btnTv.Visible = AddinSettings.IsLineStyleSupport;
            _btnTh.Visible = AddinSettings.IsLineStyleSupport;
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
        #region ParameterizeSearchButtons
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
                    EaAddinShortcutSearch search = (EaAddinShortcutSearch)AddinSettings.ButtonsSearch[pos];
                    {
                        if (search.KeyText.Trim() != "")
                        {
                            buttonText = search.KeyText;
                            helpText = search.HelpTextLong;
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
        #region ParameterizeServiceButton
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
                        helpText = $"{service.HelpTextLong}"; //  Long Help text
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
                        _toolStripServiceBtn3.Text = buttonText; 
                        _toolStripServiceBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        _toolStripServiceBtn4.Text = buttonText; 
                        _toolStripServiceBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        _toolStripServiceBtn5.Text = buttonText; 
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
                Model.SqlRun(sql, "");

            }
            else
            {
                MessageBox.Show(@"To get the connectors which convey Elements you have to select an Element.", @"No Element is selected, break!!!");
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
                        from t_object o
                        where  o.object_id in ( #ConveyedItemIDS# )
                        ORDER BY 3
                ";
                // Run SQL with macro replacement
                Model.SqlRun(sql, "");

            }
            else
            {
                MessageBox.Show(@"To get the Elements on the Connector you have to select an Connector.", @"No Connector is selected, break!!!");
            }
        }

        void getLastSQLErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(SqlError.GetEaSqlErrorFilePath());
        }
    }
}