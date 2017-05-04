using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using AddinFramework.Util;
using hoTools.Settings;
using hoTools.EaServices;
using hoTools.EAServicesPort;
using Control.EaAddinShortcuts;
using hoTools.Settings.Key;
using hoTools.Settings.Toolbar;
using EAAddinFramework.Utils;
using hoTools.EaServices.WiKiRefs;
using hoTools.Utils.SQL;
using hoTools.Utils;
using hoTools.Utils.Configuration;
using hoTools.Utils.Diagram;
using hoTools.Utils.Excel;
using DiagramStyle = EAAddinFramework.Utils.DiagramStyle;
using DiagramStyleItem = EAAddinFramework.Utils.DiagramStyleItem;


// ReSharper disable once CheckNamespace
namespace hoTools.ActiveX
{
    #region New region

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


        Model _model;

        // configuration as singleton
        readonly HoToolsGlobalCfg _globalCfg = HoToolsGlobalCfg.Instance;

        private const string JasonFile = @"Settings.json";
        private string _jasonFilePath;
        private EAAddinFramework.Utils.DiagramStyle _diagramStyle;

        // Do Menu entries already inserted
        private bool _doMenuDiagramStyleInserted = false;

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
        private Button _btnAddNoteAndLink;
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
        private Panel _panelPortButtons;
        private Button _btnConveyedItem;
        private ToolStripSeparator _toolStripSeparator6;
        private ToolStripButton _toolStripServiceBtn1;
        private ToolStripButton _toolStripServiceBtn2;
        private ToolStripButton _toolStripServiceBtn3;
        private ToolStripButton _toolStripServiceBtn4;
        private ToolStripButton _toolStripServiceBtn5;
        private ToolStripMenuItem _settingsGlobalKeysToolStripMenuItem;
        private Label _lblPorts;
        private TableLayoutPanel _panelQuickSearch;
        private ToolStripMenuItem _updateActivityFromOperationToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator10;
        private ToolStripMenuItem _getLastSqlErrorToolStripMenuItem;
        private ComboBox _cmbSearchName;
        private RichTextBox _rtfListOfSearches;
        private TextBox _txtSearchName;
        private ToolStripMenuItem _updateScriptsToolStripMenuItem;
        private ToolStripMenuItem gitHubToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolTip _toolTipRtfListOfSearches;
        private ContextMenuStrip contextMenuRtf;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem editSQLToolStripMenuItem;
        private ToolStripMenuItem showDescriptionToolStripMenuItem;
        private Button _btnReverseConnector;
        private ToolStripMenuItem exportExcelToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem exportCsvOfClipboardToExcelToolStripMenuItem;
        private Button _btnAddConstraint;
        private Button _btnAddNote;
        private Button _btnHidePort;
        private Button _btnShowPort;
        private Button _btnHidePortType;
        private Button _btnShowPortType;
        private Button _btnShowPortLabel;
        private Button _btnHidePortLabel;
        private Panel _panelConveyedItems;
        private ContextMenuStrip _contextMenuStripSearch;
        private ToolStripMenuItem editSQLSearchToolStripMenuItem;
        private ToolStripMenuItem showFolderToolStripMenuItem;
        private ToolStripMenuItem runQueryToolStripMenuItem;
        private ToolStripMenuItem runSQLAndExportToExcelToolStripMenuItem;
        private ToolStripMenuItem clipboardTocsvToolStripMenuItem;
        private ToolStripMenuItem showSQLPathToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem showSQLFolderToolStripMenuItem;
        private ToolStripMenuItem showSQLPathToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem runAndExportSQLToExcelToolStripMenuItem;
        private ToolStripMenuItem exportCsvOfClipboardToExcelToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem hoToolsToolStripMenuItem;
        private ToolStripMenuItem settingsGeneralToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem toolStripMenuIHome;
        private ToolStripMenuItem changeAuthorPackagestandardToolStripMenuItem;
        private ToolStripMenuItem readMeToolStripMenuItem;
        private Button _btnFeatureDown;
        private Button _btnFeatureUp;
        private ToolStripMenuItem setFolderToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem resetFactorySettingsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem settingsDiagramStylesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private TextBox _txtSearchText;
        #endregion

        #region Constructor
        public AddinControlGui()
        {
            try
            {
                InitializeComponent();
                // Initialize owner to prevent modal windows getting back behind main window
                _globalCfg.Owner = this;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}", @"Error Constructor hoTools");
            }

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
            _model = new Model(Repository);
            AddinSettings.UpdateModel(_model);

            _txtSearchName.ForeColor = SystemColors.WindowText;
            _txtSearchName.Text = AddinSettings.QuickSearchName;
            if (_txtSearchName.Text.Trim().Equals(""))
            {
                _txtSearchName.Text = "<Search Name>";
                _txtSearchName.ForeColor = SystemColors.ControlDark;
            }
            IntializeSearches();

           
            ParameterizeMenusAndButtons();
            // parameterize 5 Buttons to quickly run search
            ParameterizeToolbarSearchButton();
            // parameterize 5 Buttons to quickly run services
            ParameterizeToolbarServiceButton();
            //
            //_txtSearchName.DataSource = _globalCfg.getListFileCompleteName();

            //
            ResizeRtfListOfChanges();

            _rtfListOfSearches.Text = Search.GetRtf();


            GetValueSettingsFromJson();



        }
        /// <summary>
        /// Initializes all Searches. Load all searches and fill AutoSuggestionCollection
        /// </summary>
        private void IntializeSearches()
        {
             Search.LoadAllSearches(Repository,AddinSettings.ConfigFolderPath, AddinSettings.GetAutoLoadMdgFileName());
            _txtSearchName.AutoCompleteCustomSource = Search.GetSearchAutoCompleteSuggestion();

        } 


        /// <summary>
        /// Resize hight of rtf field according to available space
        /// </summary>
        private void ResizeRtfListOfChanges()
        {
            _rtfListOfSearches.Left = Left + 20;
            _rtfListOfSearches.Width = Width - 20;
            _rtfListOfSearches.Top = _txtSearchName.Bottom + 50;
            _rtfListOfSearches.Height = Bottom - _rtfListOfSearches.Top - 20;
            _rtfListOfSearches.BringToFront();
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
            EaService.DisplayOperationForSelectedElement(Repository, EaService.DisplayMode.Behavior);
        }
        void btnLocateOperation_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(Repository, EaService.DisplayMode.Method);
        }
        /// <summary>
        /// Add Note to the selected elements and link this Note to the description:<para/>
        /// - Element<para/>
        /// - Diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddNoteAndLinkDescription_Click(object sender, EventArgs e)
        {
            EaService.AddElementsToDiagram(Repository,"Note","NoteLink", true);
        }
        /// <summary>
        /// Add Note to the selected elements:<para/>
        /// - Elements<para/>
        /// - Diagram (nothing selected)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddNote_Click(object sender, EventArgs e)
        {
            EaService.AddElementsToDiagram(Repository, "Note" );
        }
        /// <summary>
        /// Add Constraint to the selected elements:<para/>
        /// - Elements<para/>
        /// - Diagram (nothing selected)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddConstraint_Click(object sender, EventArgs e)
        {
            EaService.AddElementsToDiagram(Repository, "Constraint");
        }


        void btnLocateType_Click(object sender, EventArgs e)
        {
            EaService.LocateType(Repository);
        }


        void btnShowSpecification_Click(object sender, EventArgs e)
        {
            EaService.ShowSpecification(Repository);
        }


        void btnFindUsage_Click(object sender, EventArgs e)
        {
            EaService.FindUsage(Repository);
        }

        void btnC_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "C");
        }


        void btnD_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "D");
        }



        void btnA_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "A");
        }


        void btnOR_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "OR");
        }
        /// <summary>
        /// Reverse direction of selected connector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnReverseConnector_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "R");
        }

        void btnComposite_Click(object sender, EventArgs e)
        {
            EaService.NavigateComposite(Repository);
        }



        void createActivityForOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.CreateActivityForOperation(Repository);
        }

        void updateActivityFromOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.UpdateOperationTypes(Repository);
        }


        /// <summary>
        /// Show Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowFolderElementPackage(Repository, isTotalCommander: AddinSettings.IsTotalCommander);
        }

        void copyGUIDSQLToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.CopyGuidSqlToClipboard(Repository);
        }

        void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsLineStyle = new FrmSettingsLineStyle(AddinSettings, this);
            _frmSettingsLineStyle.ShowDialog(this);
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
                    EaService.About(Release, configFilePath);
                    break;
                default:
                    EaService.About(Release, configFilePath);
                    break;
            }

        }

        void changeXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.SetNewXmlPath(Repository);
        }

        void btnBezier_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "B");
        }

        /// <summary>
        /// Open GitHub WiKi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Open GitHub Wiki
            WikiRef.Wiki();
        }
        /// <summary>
        /// Open GitHub Repository
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void githubToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Open GitHub Repository
            WikiRef.Repo();
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
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (AddinSettings.ButtonsConfigSearch[pos] is EaAddinShortcutSearch)
            {

                var button = (EaAddinShortcutSearch)AddinSettings.ButtonsConfigSearch[pos];
                string searchName = button.KeySearchName.Trim();
                if (searchName == "") return;

               _model.SearchRun(searchName, button.KeySearchTerm.Trim() );

            }
        }
        /// <summary>
        /// Run service of type Call or Script
        /// </summary>
        /// <param name="pos"></param>
        void RunService(int pos)
        {
            // ReSharper disable once IsExpressionAlwaysTrue
            if (AddinSettings.ButtonsServiceConfig[pos] is ServicesConfigCall)
            {

                var call = (ServicesConfigCall) AddinSettings.ButtonsServiceConfig[pos];
                if (call.Method == null) return;
                call.Invoke(_model, GetSearchTerm());

            }
            if (AddinSettings.ButtonsServiceConfig[pos] is ServicesConfigScript)
            {
                var script = (ServicesConfigScript)AddinSettings.ButtonsServiceConfig[pos];
                if (script.Function == null) return;
                script.Invoke(_model);
            }
        }

        void changeAuthorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ChangeAuthorItem(Repository);
        }

        void changeAuthorRecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ChangeAuthorRecursive(Repository);
        }
        private void changeAuthorPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ChangeAuthorPackage(Repository);
        }

        void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void showFolderVCorCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowFolderElementPackage(Repository, isTotalCommander: AddinSettings.IsTotalCommander);
        }

        void showTortoiseLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.GotoSvnLog(Repository, pkg);
        }

        void showTortoiseRepoBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.GotoSvnBrowser(Repository, pkg);
        }

        void getVCLatesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.GetVcLatestRecursive(Repository);
        }

        void setSvnKeywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.SetSvnProperty(Repository, pkg);
        }

        void setSvnModuleTaggedValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            if (!oType.Equals(EA.ObjectType.otPackage)) return;

            var pkg = (EA.Package)Repository.GetContextObject();
            EaService.SetDirectoryTaggedValues(Repository, pkg);
        }

        void setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.SetTaggedValueGui(Repository);
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
            EaService.HideEmbeddedElements(Repository);

        }
        #endregion

        #region showPortsInDiagramObjects
        void showPortsInDiagramObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowEmbeddedElements(Repository, isOptimizePortLayoutLocation: false, portSynchronizationKind: AddinSettings.PartPortSyncho);
        }
        #endregion
        /// <summary>
        /// Show Port Label for:
        /// - Diagram
        /// - Element
        /// - Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnShowPortLabel_Click(object sender, EventArgs e)
        {
            EaService.ShowEmbeddedElementsLabel(Repository);
        }
        /// <summary>
        /// Hide Port Label for:
        /// - Diagram
        /// - Element
        /// - Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnHidePortLabel_Click(object sender, EventArgs e)
        {
            EaService.HideEmbeddedElementsLabel(Repository);
        }
        /// <summary>
        /// Show Port Type for:
        /// - Diagram
        /// - Element
        /// - Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnShowPortType_Click(object sender, EventArgs e)
        {
            EaService.ShowEmbeddedElementsType(Repository);
        }
        /// <summary>
        /// Hide Port Type for:
        /// - Diagram
        /// - Element
        /// - Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnHidePortType_Click(object sender, EventArgs e)
        {
            EaService.HideEmbeddedElementsType(Repository);
        }



        #region showReceivingPortsLeftSendingPortsRight
        void showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.ShowEmbeddedElements(Repository, isOptimizePortLayoutLocation: false, portSynchronizationKind: AddinSettings.PartPortSyncho);
        }
        #endregion

        #region copyPorts
        void copyPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var port = new PortServices(Repository);
            port.CopyPortsGui();

        }
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
            EaService.HideEmbeddedElements(Repository);
        }

        void btnLeft_Click(object sender, EventArgs e)
        {
            EaService.MoveEmbeddedLeftGui(Repository);
        }

        void btnRight_Click(object sender, EventArgs e)
        {
            EaService.MoveEmbeddedRightGui(Repository);
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            EaService.MoveEmbeddedUpGui(Repository);
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            EaService.MoveEmbeddedDownGui(Repository);
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
            EaService.SetLineStyle(Repository, "LH");
        }
        void btnLV_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "LV");
        }
        void btnTH_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "TH");
        }
        void btnTV_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "TV");
        }
        void btnOS_Click(object sender, EventArgs e)
        {
            EaService.SetLineStyle(Repository, "OS");
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
                _model.SearchRun(GetSearchName(), GetSearchTerm());
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
            _model.SearchRun(GetSearchName(), GetSearchTerm());
        }


        //--------------------------------------------------------------------
        // Search Name Combo Box
        /// <summary>
        /// Double Mouse Click in SearchName runs the query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbSearchName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _model.SearchRun(GetSearchName(), GetSearchTerm());
        }
        // text field
        // There are special keys like "Enter" which require an enabling by 
        //---------------------------------------------------------
        // see at:  protected override boolean IsInputKey(Keys keyData)
        void cmbSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            //RtfSearchNameProcessKeys(e);

        }
        void _txtSearchName_KeyUp(object sender, KeyEventArgs e)
        {
            RtfSearchNameProcessKeys(e);
        }

        /// <summary>
        /// Process Keys for Textbox Search Name:
        /// <para />
        /// Enter: Run Query
        /// <para />
        /// up, down, Space: Open rtf list to choose a Search
        /// </summary>
        /// <param name="e"></param>
        private void RtfSearchNameProcessKeys(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // run the SQL
                case Keys.Enter:
                    _model.SearchRun(GetSearchName(), GetSearchTerm() );
                    _rtfListOfSearches.Visible = false;
                    e.Handled = true;
                    break;
                case Keys.Up:
                case Keys.Down:
                case Keys.Space:
                    if (_txtSearchName.Text.Trim() == "")
                    {
                        // Reset sort order of Searches
                        Search.ResetSort();
                        _rtfListOfSearches.Text = Search.GetRtf();
                    }
                    else
                    {
                        Search.CalulateAndSort(_txtSearchName.Text.Trim());
                        _rtfListOfSearches.Text = Search.GetRtf();
                        _rtfListOfSearches.Clear();
                        ColorCharacters(_rtfListOfSearches, Search.GetRtf(), _txtSearchName.Text, Color.Yellow);
                    }

                    _rtfListOfSearches.BringToFront();
                    _rtfListOfSearches.Visible = true;
                    e.Handled = true;
                    break;
                case Keys.Escape:
                case Keys.Back:
                    _rtfListOfSearches.Visible = false;
                    break;
            }
        }
        /// <summary>
        /// Color find the found search string in rtf textbox. The string has to exactly match.
        /// </summary>
        /// <param name="rtf"></param>
        /// <param name="fromText"></param>
        /// <param name="stringToColor"></param>
        /// <param name="color"></param>

        void ColorCharacters(RichTextBox rtf, string fromText, string stringToColor, Color color)
        {
            int posInText = 0;
            if (stringToColor.Trim() == "")
            {
                rtf.SelectionBackColor = Color.AliceBlue;
                rtf.AppendText(fromText);
            }
            else
            {

                string from = fromText;
                Regex pattern = new Regex($"{stringToColor.Trim()}",RegexOptions.IgnoreCase);
                Match match = pattern.Match(from);
                while (match.Success)
                {
                    // output not outputted text
                    if ((match.Index - posInText) > 0)
                    {
                        rtf.SelectionBackColor = Color.AliceBlue;
                        rtf.AppendText(from.Substring(posInText, match.Index - posInText));
                    }
                    rtf.SelectionBackColor = Color.Gold;
                    rtf.AppendText(match.Value);
                    posInText = match.Index + match.Length;
                    match = match.NextMatch();

                }
                if ((from.Length - 1 - posInText) >= 0)
                {
                    rtf.SelectionBackColor = Color.AliceBlue;
                    rtf.SelectionBackColor = Color.AliceBlue;
                    rtf.AppendText(from.Substring(posInText, from.Length - posInText));
                }
        }
    }

        private void _txtSearchName_TextChanged(object sender, EventArgs e)
        {
           

        }
        private void _txtSearchName_TextUpdate(object sender, EventArgs e)
        {
            //Search.ResetSort();




        }
        /// <summary>
        /// Get the Search Term
        /// </summary>
        /// <returns></returns>

        string GetSearchTerm()
        {
            string searchTerm = _txtSearchText.Text;
            if (searchTerm == "<Search Term>")
            {
                searchTerm = "";
            }

            return searchTerm;

        }
        /// <summary>
        /// Get Search Name from GUI text field.
        /// </summary>
        /// <returns></returns>
        string GetSearchName()
        {
            _txtSearchName.ForeColor = SystemColors.WindowText;
            string searchName = _txtSearchName.Text.Trim();
            if (searchName.Equals("<Search Name>")) searchName = "";
            return searchName;
        }
        #endregion
        #endregion

        #region InitializeComponent
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinControlGui));
            this._toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._toolStripQuery = new System.Windows.Forms.ToolStrip();
            this._toolStripSearchBtn1 = new System.Windows.Forms.ToolStripButton();
            this._toolStripSearchBtn2 = new System.Windows.Forms.ToolStripButton();
            this._toolStripSearchBtn3 = new System.Windows.Forms.ToolStripButton();
            this._toolStripSearchBtn4 = new System.Windows.Forms.ToolStripButton();
            this._toolStripSearchBtn5 = new System.Windows.Forms.ToolStripButton();
            this._toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripServiceBtn1 = new System.Windows.Forms.ToolStripButton();
            this._toolStripServiceBtn2 = new System.Windows.Forms.ToolStripButton();
            this._toolStripServiceBtn3 = new System.Windows.Forms.ToolStripButton();
            this._toolStripServiceBtn4 = new System.Windows.Forms.ToolStripButton();
            this._toolStripServiceBtn5 = new System.Windows.Forms.ToolStripButton();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._txtSearchText = new System.Windows.Forms.TextBox();
            this._contextMenuStripSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editSQLSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSQLPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.runQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSQLAndExportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.clipboardTocsvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._btnLabelRight = new System.Windows.Forms.Button();
            this._btnLabelLeft = new System.Windows.Forms.Button();
            this._btnUp = new System.Windows.Forms.Button();
            this._btnDown = new System.Windows.Forms.Button();
            this._btnLeft = new System.Windows.Forms.Button();
            this._btnRight = new System.Windows.Forms.Button();
            this._btnShowFavorites = new System.Windows.Forms.Button();
            this._btnRemoveFavorite = new System.Windows.Forms.Button();
            this._btnAddFavorite = new System.Windows.Forms.Button();
            this._btnBezier = new System.Windows.Forms.Button();
            this._btnUpdateActivityParameter = new System.Windows.Forms.Button();
            this._btnC = new System.Windows.Forms.Button();
            this._btnD = new System.Windows.Forms.Button();
            this._btnA = new System.Windows.Forms.Button();
            this._btnOr = new System.Windows.Forms.Button();
            this._btnComposite = new System.Windows.Forms.Button();
            this._btnDisplaySpecification = new System.Windows.Forms.Button();
            this._btnFindUsage = new System.Windows.Forms.Button();
            this._btnLocateType = new System.Windows.Forms.Button();
            this._btnLocateOperation = new System.Windows.Forms.Button();
            this._btnDisplayBehavior = new System.Windows.Forms.Button();
            this._btnOs = new System.Windows.Forms.Button();
            this._btnTv = new System.Windows.Forms.Button();
            this._btnTh = new System.Windows.Forms.Button();
            this._btnLv = new System.Windows.Forms.Button();
            this._btnLh = new System.Windows.Forms.Button();
            this._btnConveyedItem = new System.Windows.Forms.Button();
            this._panelPortButtons = new System.Windows.Forms.Panel();
            this._btnHidePortType = new System.Windows.Forms.Button();
            this._btnShowPortType = new System.Windows.Forms.Button();
            this._btnShowPortLabel = new System.Windows.Forms.Button();
            this._btnHidePortLabel = new System.Windows.Forms.Button();
            this._btnHidePort = new System.Windows.Forms.Button();
            this._btnShowPort = new System.Windows.Forms.Button();
            this._btnReverseConnector = new System.Windows.Forms.Button();
            this._cmbSearchName = new System.Windows.Forms.ComboBox();
            this._txtSearchName = new System.Windows.Forms.TextBox();
            this._btnAddNoteAndLink = new System.Windows.Forms.Button();
            this._btnAddNote = new System.Windows.Forms.Button();
            this._btnAddConstraint = new System.Windows.Forms.Button();
            this.contextMenuRtf = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSQLFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSQLPathToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runAndExportSQLToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.exportCsvOfClipboardToExcelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._btnFeatureDown = new System.Windows.Forms.Button();
            this._btnFeatureUp = new System.Windows.Forms.Button();
            this._rtfListOfSearches = new System.Windows.Forms.RichTextBox();
            this._menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._settingGeneralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._settingsGlobalKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._settingsToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._settingsQueryAndSctipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsDiagramStylesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this._updateScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.resetFactorySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._doToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCsvOfClipboardToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._createActivityForOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._updateActivityFromOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this._showFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this._copyGuidsqlToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.changeAuthorPackagestandardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._changeAuthorRecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._changeAuthorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._versionControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._changeXmlFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showFolderVCorCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._getVcLatesrecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._showTortoiseLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showTortoiseRepoBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._setSvnKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._setSvnModuleTaggedValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._setSvnModuleTaggedValuesrecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._movePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this._deletePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._connectPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._connectPortsInsideComponentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this._makeConnectorsUnspecifiedDirectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._showPortsInDiagramObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showSendingPortsLeftRecievingPortsRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._hidePortsInDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._unhidePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._hidePortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this._movePortLabelLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._movePortLabelRightPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this._movePortLabelLeftToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._movePortLabelRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this._orderDiagramItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._getLastSqlErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hoToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsGeneralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuIHome = new System.Windows.Forms.ToolStripMenuItem();
            this._panelButtons = new System.Windows.Forms.Panel();
            this._panelLineStyle = new System.Windows.Forms.Panel();
            this._panelFavorite = new System.Windows.Forms.Panel();
            this._panelNote = new System.Windows.Forms.Panel();
            this._panelPort = new System.Windows.Forms.Panel();
            this._lblPorts = new System.Windows.Forms.Label();
            this._panelAdvanced = new System.Windows.Forms.Panel();
            this._panelQuickSearch = new System.Windows.Forms.TableLayoutPanel();
            this._toolTipRtfListOfSearches = new System.Windows.Forms.ToolTip(this.components);
            this._panelConveyedItems = new System.Windows.Forms.Panel();
            this._toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer1.SuspendLayout();
            this._toolStripQuery.SuspendLayout();
            this._contextMenuStripSearch.SuspendLayout();
            this._panelPortButtons.SuspendLayout();
            this.contextMenuRtf.SuspendLayout();
            this._menuStrip1.SuspendLayout();
            this._panelButtons.SuspendLayout();
            this._panelLineStyle.SuspendLayout();
            this._panelFavorite.SuspendLayout();
            this._panelNote.SuspendLayout();
            this._panelPort.SuspendLayout();
            this._panelAdvanced.SuspendLayout();
            this._panelQuickSearch.SuspendLayout();
            this._panelConveyedItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStripContainer1
            // 
            this._toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // _toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(this._toolStripContainer1.ContentPanel, "_toolStripContainer1.ContentPanel");
            resources.ApplyResources(this._toolStripContainer1, "_toolStripContainer1");
            this._toolStripContainer1.LeftToolStripPanelVisible = false;
            this._toolStripContainer1.Name = "_toolStripContainer1";
            this._toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // _toolStripContainer1.TopToolStripPanel
            // 
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._toolStripQuery);
            // 
            // _toolStripQuery
            // 
            resources.ApplyResources(this._toolStripQuery, "_toolStripQuery");
            this._toolStripQuery.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._toolStripQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripSearchBtn1,
            this._toolStripSearchBtn2,
            this._toolStripSearchBtn3,
            this._toolStripSearchBtn4,
            this._toolStripSearchBtn5,
            this._toolStripSeparator6,
            this._toolStripServiceBtn1,
            this._toolStripServiceBtn2,
            this._toolStripServiceBtn3,
            this._toolStripServiceBtn4,
            this._toolStripServiceBtn5});
            this._toolStripQuery.Name = "_toolStripQuery";
            // 
            // _toolStripSearchBtn1
            // 
            this._toolStripSearchBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripSearchBtn1, "_toolStripSearchBtn1");
            this._toolStripSearchBtn1.Name = "_toolStripSearchBtn1";
            this._toolStripSearchBtn1.Click += new System.EventHandler(this.toolStripSearchBtn1_Click);
            // 
            // _toolStripSearchBtn2
            // 
            this._toolStripSearchBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._toolStripSearchBtn2.Name = "_toolStripSearchBtn2";
            resources.ApplyResources(this._toolStripSearchBtn2, "_toolStripSearchBtn2");
            this._toolStripSearchBtn2.Click += new System.EventHandler(this.toolStripSearchBtn2_Click);
            // 
            // _toolStripSearchBtn3
            // 
            this._toolStripSearchBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripSearchBtn3, "_toolStripSearchBtn3");
            this._toolStripSearchBtn3.Name = "_toolStripSearchBtn3";
            this._toolStripSearchBtn3.Click += new System.EventHandler(this.toolStripSearchBtn3_Click);
            // 
            // _toolStripSearchBtn4
            // 
            this._toolStripSearchBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripSearchBtn4, "_toolStripSearchBtn4");
            this._toolStripSearchBtn4.Name = "_toolStripSearchBtn4";
            this._toolStripSearchBtn4.Click += new System.EventHandler(this.toolStripSearchBtn4_Click);
            // 
            // _toolStripSearchBtn5
            // 
            this._toolStripSearchBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripSearchBtn5, "_toolStripSearchBtn5");
            this._toolStripSearchBtn5.Name = "_toolStripSearchBtn5";
            this._toolStripSearchBtn5.Click += new System.EventHandler(this.toolStripSearchBtn5_Click);
            // 
            // _toolStripSeparator6
            // 
            this._toolStripSeparator6.Name = "_toolStripSeparator6";
            resources.ApplyResources(this._toolStripSeparator6, "_toolStripSeparator6");
            // 
            // _toolStripServiceBtn1
            // 
            this._toolStripServiceBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripServiceBtn1, "_toolStripServiceBtn1");
            this._toolStripServiceBtn1.Name = "_toolStripServiceBtn1";
            this._toolStripServiceBtn1.Click += new System.EventHandler(this.toolStripServiceBtn1_Click);
            // 
            // _toolStripServiceBtn2
            // 
            this._toolStripServiceBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripServiceBtn2, "_toolStripServiceBtn2");
            this._toolStripServiceBtn2.Name = "_toolStripServiceBtn2";
            this._toolStripServiceBtn2.Click += new System.EventHandler(this.toolStripServiceBtn2_Click);
            // 
            // _toolStripServiceBtn3
            // 
            this._toolStripServiceBtn3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripServiceBtn3, "_toolStripServiceBtn3");
            this._toolStripServiceBtn3.Name = "_toolStripServiceBtn3";
            this._toolStripServiceBtn3.Click += new System.EventHandler(this.toolStripServiceBtn3_Click);
            // 
            // _toolStripServiceBtn4
            // 
            this._toolStripServiceBtn4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripServiceBtn4, "_toolStripServiceBtn4");
            this._toolStripServiceBtn4.Name = "_toolStripServiceBtn4";
            this._toolStripServiceBtn4.Click += new System.EventHandler(this.toolStripServiceBtn4_Click);
            // 
            // _toolStripServiceBtn5
            // 
            this._toolStripServiceBtn5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._toolStripServiceBtn5, "_toolStripServiceBtn5");
            this._toolStripServiceBtn5.Name = "_toolStripServiceBtn5";
            this._toolStripServiceBtn5.Click += new System.EventHandler(this.toolStripServiceBtn5_Click);
            // 
            // _txtSearchText
            // 
            this._txtSearchText.ContextMenuStrip = this._contextMenuStripSearch;
            resources.ApplyResources(this._txtSearchText, "_txtSearchText");
            this._txtSearchText.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._txtSearchText.Name = "_txtSearchText";
            this._toolTip.SetToolTip(this._txtSearchText, resources.GetString("_txtSearchText.ToolTip"));
            this._txtSearchText.Enter += new System.EventHandler(this._txtSearchText_Enter);
            this._txtSearchText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this._txtSearchText.Leave += new System.EventHandler(this._txtSearchText_Leave);
            this._txtSearchText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSearchText_MouseDoubleClick);
            this._txtSearchText.MouseLeave += new System.EventHandler(this._txtSearchText_MouseLeave);
            // 
            // _contextMenuStripSearch
            // 
            this._contextMenuStripSearch.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._contextMenuStripSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editSQLSearchToolStripMenuItem,
            this.showFolderToolStripMenuItem,
            this.showSQLPathToolStripMenuItem,
            this.toolStripSeparator4,
            this.runQueryToolStripMenuItem,
            this.runSQLAndExportToExcelToolStripMenuItem,
            this.toolStripSeparator5,
            this.clipboardTocsvToolStripMenuItem});
            this._contextMenuStripSearch.Name = "_contextMenuStripSearch";
            resources.ApplyResources(this._contextMenuStripSearch, "_contextMenuStripSearch");
            this._toolTip.SetToolTip(this._contextMenuStripSearch, resources.GetString("_contextMenuStripSearch.ToolTip"));
            // 
            // editSQLSearchToolStripMenuItem
            // 
            this.editSQLSearchToolStripMenuItem.Name = "editSQLSearchToolStripMenuItem";
            resources.ApplyResources(this.editSQLSearchToolStripMenuItem, "editSQLSearchToolStripMenuItem");
            this.editSQLSearchToolStripMenuItem.Click += new System.EventHandler(this.editSQLSearchToolStripMenuItem_Click);
            // 
            // showFolderToolStripMenuItem
            // 
            this.showFolderToolStripMenuItem.Name = "showFolderToolStripMenuItem";
            resources.ApplyResources(this.showFolderToolStripMenuItem, "showFolderToolStripMenuItem");
            this.showFolderToolStripMenuItem.Click += new System.EventHandler(this.showFolderSqlToolStripMenuItem_Click);
            // 
            // showSQLPathToolStripMenuItem
            // 
            this.showSQLPathToolStripMenuItem.Name = "showSQLPathToolStripMenuItem";
            resources.ApplyResources(this.showSQLPathToolStripMenuItem, "showSQLPathToolStripMenuItem");
            this.showSQLPathToolStripMenuItem.Click += new System.EventHandler(this.showSQLPathToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // runQueryToolStripMenuItem
            // 
            this.runQueryToolStripMenuItem.Name = "runQueryToolStripMenuItem";
            resources.ApplyResources(this.runQueryToolStripMenuItem, "runQueryToolStripMenuItem");
            this.runQueryToolStripMenuItem.Click += new System.EventHandler(this.runQueryToolStripMenuItem_Click);
            // 
            // runSQLAndExportToExcelToolStripMenuItem
            // 
            this.runSQLAndExportToExcelToolStripMenuItem.Name = "runSQLAndExportToExcelToolStripMenuItem";
            resources.ApplyResources(this.runSQLAndExportToExcelToolStripMenuItem, "runSQLAndExportToExcelToolStripMenuItem");
            this.runSQLAndExportToExcelToolStripMenuItem.Click += new System.EventHandler(this.exportExcelToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // clipboardTocsvToolStripMenuItem
            // 
            this.clipboardTocsvToolStripMenuItem.Name = "clipboardTocsvToolStripMenuItem";
            resources.ApplyResources(this.clipboardTocsvToolStripMenuItem, "clipboardTocsvToolStripMenuItem");
            this.clipboardTocsvToolStripMenuItem.Click += new System.EventHandler(this.exportCsvOfClipboardToExcelToolStripMenuItem_Click);
            // 
            // _btnLabelRight
            // 
            resources.ApplyResources(this._btnLabelRight, "_btnLabelRight");
            this._btnLabelRight.Name = "_btnLabelRight";
            this._toolTip.SetToolTip(this._btnLabelRight, resources.GetString("_btnLabelRight.ToolTip"));
            this._btnLabelRight.UseVisualStyleBackColor = true;
            this._btnLabelRight.Click += new System.EventHandler(this.movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // _btnLabelLeft
            // 
            resources.ApplyResources(this._btnLabelLeft, "_btnLabelLeft");
            this._btnLabelLeft.Name = "_btnLabelLeft";
            this._toolTip.SetToolTip(this._btnLabelLeft, resources.GetString("_btnLabelLeft.ToolTip"));
            this._btnLabelLeft.UseVisualStyleBackColor = true;
            this._btnLabelLeft.Click += new System.EventHandler(this.movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // _btnUp
            // 
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.Name = "_btnUp";
            this._toolTip.SetToolTip(this._btnUp, resources.GetString("_btnUp.ToolTip"));
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // _btnDown
            // 
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.Name = "_btnDown";
            this._toolTip.SetToolTip(this._btnDown, resources.GetString("_btnDown.ToolTip"));
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // _btnLeft
            // 
            resources.ApplyResources(this._btnLeft, "_btnLeft");
            this._btnLeft.Name = "_btnLeft";
            this._toolTip.SetToolTip(this._btnLeft, resources.GetString("_btnLeft.ToolTip"));
            this._btnLeft.UseVisualStyleBackColor = true;
            this._btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // _btnRight
            // 
            resources.ApplyResources(this._btnRight, "_btnRight");
            this._btnRight.Name = "_btnRight";
            this._toolTip.SetToolTip(this._btnRight, resources.GetString("_btnRight.ToolTip"));
            this._btnRight.UseVisualStyleBackColor = true;
            this._btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // _btnShowFavorites
            // 
            resources.ApplyResources(this._btnShowFavorites, "_btnShowFavorites");
            this._btnShowFavorites.Name = "_btnShowFavorites";
            this._toolTip.SetToolTip(this._btnShowFavorites, resources.GetString("_btnShowFavorites.ToolTip"));
            this._btnShowFavorites.UseVisualStyleBackColor = true;
            this._btnShowFavorites.Click += new System.EventHandler(this.btnFavorites_Click);
            // 
            // _btnRemoveFavorite
            // 
            resources.ApplyResources(this._btnRemoveFavorite, "_btnRemoveFavorite");
            this._btnRemoveFavorite.Name = "_btnRemoveFavorite";
            this._toolTip.SetToolTip(this._btnRemoveFavorite, resources.GetString("_btnRemoveFavorite.ToolTip"));
            this._btnRemoveFavorite.UseVisualStyleBackColor = true;
            this._btnRemoveFavorite.Click += new System.EventHandler(this.btnRemoveFavorite_Click);
            // 
            // _btnAddFavorite
            // 
            resources.ApplyResources(this._btnAddFavorite, "_btnAddFavorite");
            this._btnAddFavorite.Name = "_btnAddFavorite";
            this._toolTip.SetToolTip(this._btnAddFavorite, resources.GetString("_btnAddFavorite.ToolTip"));
            this._btnAddFavorite.UseVisualStyleBackColor = true;
            this._btnAddFavorite.Click += new System.EventHandler(this.btnAddFavorite_Click);
            // 
            // _btnBezier
            // 
            resources.ApplyResources(this._btnBezier, "_btnBezier");
            this._btnBezier.Name = "_btnBezier";
            this._toolTip.SetToolTip(this._btnBezier, resources.GetString("_btnBezier.ToolTip"));
            this._btnBezier.UseVisualStyleBackColor = true;
            this._btnBezier.Click += new System.EventHandler(this.btnBezier_Click);
            // 
            // _btnUpdateActivityParameter
            // 
            resources.ApplyResources(this._btnUpdateActivityParameter, "_btnUpdateActivityParameter");
            this._btnUpdateActivityParameter.Name = "_btnUpdateActivityParameter";
            this._toolTip.SetToolTip(this._btnUpdateActivityParameter, resources.GetString("_btnUpdateActivityParameter.ToolTip"));
            this._btnUpdateActivityParameter.UseVisualStyleBackColor = true;
            this._btnUpdateActivityParameter.Click += new System.EventHandler(this.btnUpdateActivityParametzer_Click);
            // 
            // _btnC
            // 
            resources.ApplyResources(this._btnC, "_btnC");
            this._btnC.Name = "_btnC";
            this._toolTip.SetToolTip(this._btnC, resources.GetString("_btnC.ToolTip"));
            this._btnC.UseVisualStyleBackColor = true;
            this._btnC.Click += new System.EventHandler(this.btnC_Click);
            // 
            // _btnD
            // 
            resources.ApplyResources(this._btnD, "_btnD");
            this._btnD.Name = "_btnD";
            this._toolTip.SetToolTip(this._btnD, resources.GetString("_btnD.ToolTip"));
            this._btnD.UseVisualStyleBackColor = true;
            this._btnD.Click += new System.EventHandler(this.btnD_Click);
            // 
            // _btnA
            // 
            resources.ApplyResources(this._btnA, "_btnA");
            this._btnA.Name = "_btnA";
            this._toolTip.SetToolTip(this._btnA, resources.GetString("_btnA.ToolTip"));
            this._btnA.UseVisualStyleBackColor = true;
            this._btnA.Click += new System.EventHandler(this.btnA_Click);
            // 
            // _btnOr
            // 
            resources.ApplyResources(this._btnOr, "_btnOr");
            this._btnOr.Name = "_btnOr";
            this._toolTip.SetToolTip(this._btnOr, resources.GetString("_btnOr.ToolTip"));
            this._btnOr.UseVisualStyleBackColor = true;
            this._btnOr.Click += new System.EventHandler(this.btnOR_Click);
            // 
            // _btnComposite
            // 
            resources.ApplyResources(this._btnComposite, "_btnComposite");
            this._btnComposite.Name = "_btnComposite";
            this._toolTip.SetToolTip(this._btnComposite, resources.GetString("_btnComposite.ToolTip"));
            this._btnComposite.UseVisualStyleBackColor = true;
            this._btnComposite.Click += new System.EventHandler(this.btnComposite_Click);
            // 
            // _btnDisplaySpecification
            // 
            resources.ApplyResources(this._btnDisplaySpecification, "_btnDisplaySpecification");
            this._btnDisplaySpecification.Name = "_btnDisplaySpecification";
            this._toolTip.SetToolTip(this._btnDisplaySpecification, resources.GetString("_btnDisplaySpecification.ToolTip"));
            this._btnDisplaySpecification.UseVisualStyleBackColor = true;
            this._btnDisplaySpecification.Click += new System.EventHandler(this.btnShowSpecification_Click);
            // 
            // _btnFindUsage
            // 
            resources.ApplyResources(this._btnFindUsage, "_btnFindUsage");
            this._btnFindUsage.Name = "_btnFindUsage";
            this._toolTip.SetToolTip(this._btnFindUsage, resources.GetString("_btnFindUsage.ToolTip"));
            this._btnFindUsage.UseVisualStyleBackColor = true;
            this._btnFindUsage.Click += new System.EventHandler(this.btnFindUsage_Click);
            // 
            // _btnLocateType
            // 
            resources.ApplyResources(this._btnLocateType, "_btnLocateType");
            this._btnLocateType.Name = "_btnLocateType";
            this._toolTip.SetToolTip(this._btnLocateType, resources.GetString("_btnLocateType.ToolTip"));
            this._btnLocateType.UseVisualStyleBackColor = true;
            this._btnLocateType.Click += new System.EventHandler(this.btnLocateType_Click);
            // 
            // _btnLocateOperation
            // 
            resources.ApplyResources(this._btnLocateOperation, "_btnLocateOperation");
            this._btnLocateOperation.Name = "_btnLocateOperation";
            this._toolTip.SetToolTip(this._btnLocateOperation, resources.GetString("_btnLocateOperation.ToolTip"));
            this._btnLocateOperation.UseVisualStyleBackColor = true;
            this._btnLocateOperation.Click += new System.EventHandler(this.btnLocateOperation_Click);
            // 
            // _btnDisplayBehavior
            // 
            resources.ApplyResources(this._btnDisplayBehavior, "_btnDisplayBehavior");
            this._btnDisplayBehavior.Name = "_btnDisplayBehavior";
            this._toolTip.SetToolTip(this._btnDisplayBehavior, resources.GetString("_btnDisplayBehavior.ToolTip"));
            this._btnDisplayBehavior.UseVisualStyleBackColor = true;
            this._btnDisplayBehavior.Click += new System.EventHandler(this.btnDisplayBehavior_Click);
            // 
            // _btnOs
            // 
            resources.ApplyResources(this._btnOs, "_btnOs");
            this._btnOs.Name = "_btnOs";
            this._toolTip.SetToolTip(this._btnOs, resources.GetString("_btnOs.ToolTip"));
            this._btnOs.UseVisualStyleBackColor = true;
            this._btnOs.Click += new System.EventHandler(this.btnOS_Click);
            // 
            // _btnTv
            // 
            resources.ApplyResources(this._btnTv, "_btnTv");
            this._btnTv.Name = "_btnTv";
            this._toolTip.SetToolTip(this._btnTv, resources.GetString("_btnTv.ToolTip"));
            this._btnTv.UseVisualStyleBackColor = true;
            this._btnTv.Click += new System.EventHandler(this.btnTV_Click);
            // 
            // _btnTh
            // 
            resources.ApplyResources(this._btnTh, "_btnTh");
            this._btnTh.Name = "_btnTh";
            this._toolTip.SetToolTip(this._btnTh, resources.GetString("_btnTh.ToolTip"));
            this._btnTh.UseVisualStyleBackColor = true;
            this._btnTh.Click += new System.EventHandler(this.btnTH_Click);
            // 
            // _btnLv
            // 
            resources.ApplyResources(this._btnLv, "_btnLv");
            this._btnLv.Name = "_btnLv";
            this._toolTip.SetToolTip(this._btnLv, resources.GetString("_btnLv.ToolTip"));
            this._btnLv.UseVisualStyleBackColor = true;
            this._btnLv.Click += new System.EventHandler(this.btnLV_Click);
            // 
            // _btnLh
            // 
            resources.ApplyResources(this._btnLh, "_btnLh");
            this._btnLh.Name = "_btnLh";
            this._toolTip.SetToolTip(this._btnLh, resources.GetString("_btnLh.ToolTip"));
            this._btnLh.UseVisualStyleBackColor = true;
            this._btnLh.Click += new System.EventHandler(this.btnLH_Click);
            // 
            // _btnConveyedItem
            // 
            resources.ApplyResources(this._btnConveyedItem, "_btnConveyedItem");
            this._btnConveyedItem.Name = "_btnConveyedItem";
            this._toolTip.SetToolTip(this._btnConveyedItem, resources.GetString("_btnConveyedItem.ToolTip"));
            this._btnConveyedItem.UseVisualStyleBackColor = true;
            this._btnConveyedItem.Click += new System.EventHandler(this.btnConveyedItem_Click);
            // 
            // _panelPortButtons
            // 
            this._panelPortButtons.Controls.Add(this._btnHidePortType);
            this._panelPortButtons.Controls.Add(this._btnShowPortType);
            this._panelPortButtons.Controls.Add(this._btnShowPortLabel);
            this._panelPortButtons.Controls.Add(this._btnHidePortLabel);
            this._panelPortButtons.Controls.Add(this._btnHidePort);
            this._panelPortButtons.Controls.Add(this._btnShowPort);
            resources.ApplyResources(this._panelPortButtons, "_panelPortButtons");
            this._panelPortButtons.Name = "_panelPortButtons";
            this._toolTip.SetToolTip(this._panelPortButtons, resources.GetString("_panelPortButtons.ToolTip"));
            // 
            // _btnHidePortType
            // 
            resources.ApplyResources(this._btnHidePortType, "_btnHidePortType");
            this._btnHidePortType.Name = "_btnHidePortType";
            this._toolTip.SetToolTip(this._btnHidePortType, resources.GetString("_btnHidePortType.ToolTip"));
            this._btnHidePortType.UseVisualStyleBackColor = true;
            this._btnHidePortType.Click += new System.EventHandler(this._btnHidePortType_Click);
            // 
            // _btnShowPortType
            // 
            resources.ApplyResources(this._btnShowPortType, "_btnShowPortType");
            this._btnShowPortType.Name = "_btnShowPortType";
            this._toolTip.SetToolTip(this._btnShowPortType, resources.GetString("_btnShowPortType.ToolTip"));
            this._btnShowPortType.UseVisualStyleBackColor = true;
            this._btnShowPortType.Click += new System.EventHandler(this._btnShowPortType_Click);
            // 
            // _btnShowPortLabel
            // 
            resources.ApplyResources(this._btnShowPortLabel, "_btnShowPortLabel");
            this._btnShowPortLabel.Name = "_btnShowPortLabel";
            this._toolTip.SetToolTip(this._btnShowPortLabel, resources.GetString("_btnShowPortLabel.ToolTip"));
            this._btnShowPortLabel.UseVisualStyleBackColor = true;
            this._btnShowPortLabel.Click += new System.EventHandler(this._btnShowPortLabel_Click);
            // 
            // _btnHidePortLabel
            // 
            resources.ApplyResources(this._btnHidePortLabel, "_btnHidePortLabel");
            this._btnHidePortLabel.Name = "_btnHidePortLabel";
            this._toolTip.SetToolTip(this._btnHidePortLabel, resources.GetString("_btnHidePortLabel.ToolTip"));
            this._btnHidePortLabel.UseVisualStyleBackColor = true;
            this._btnHidePortLabel.Click += new System.EventHandler(this._btnHidePortLabel_Click);
            // 
            // _btnHidePort
            // 
            resources.ApplyResources(this._btnHidePort, "_btnHidePort");
            this._btnHidePort.Name = "_btnHidePort";
            this._toolTip.SetToolTip(this._btnHidePort, resources.GetString("_btnHidePort.ToolTip"));
            this._btnHidePort.UseVisualStyleBackColor = true;
            this._btnHidePort.Click += new System.EventHandler(this.removePortsInDiagramToolStripMenuItem_Click);
            // 
            // _btnShowPort
            // 
            resources.ApplyResources(this._btnShowPort, "_btnShowPort");
            this._btnShowPort.Name = "_btnShowPort";
            this._toolTip.SetToolTip(this._btnShowPort, resources.GetString("_btnShowPort.ToolTip"));
            this._btnShowPort.UseVisualStyleBackColor = true;
            this._btnShowPort.Click += new System.EventHandler(this.showPortsInDiagramObjectsToolStripMenuItem_Click);
            // 
            // _btnReverseConnector
            // 
            resources.ApplyResources(this._btnReverseConnector, "_btnReverseConnector");
            this._btnReverseConnector.Name = "_btnReverseConnector";
            this._toolTip.SetToolTip(this._btnReverseConnector, resources.GetString("_btnReverseConnector.ToolTip"));
            this._btnReverseConnector.UseVisualStyleBackColor = true;
            this._btnReverseConnector.Click += new System.EventHandler(this._btnReverseConnector_Click);
            // 
            // _cmbSearchName
            // 
            this._cmbSearchName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._cmbSearchName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            resources.ApplyResources(this._cmbSearchName, "_cmbSearchName");
            this._cmbSearchName.FormattingEnabled = true;
            this._cmbSearchName.Name = "_cmbSearchName";
            this._toolTip.SetToolTip(this._cmbSearchName, resources.GetString("_cmbSearchName.ToolTip"));
            this._cmbSearchName.TextUpdate += new System.EventHandler(this._txtSearchName_TextUpdate);
            this._cmbSearchName.TextChanged += new System.EventHandler(this._txtSearchName_TextChanged);
            this._cmbSearchName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSearchName_KeyDown);
            this._cmbSearchName.KeyUp += new System.Windows.Forms.KeyEventHandler(this._txtSearchName_KeyUp);
            this._cmbSearchName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cmbSearchName_MouseDoubleClick);
            // 
            // _txtSearchName
            // 
            this._txtSearchName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._txtSearchName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this._txtSearchName.ContextMenuStrip = this._contextMenuStripSearch;
            resources.ApplyResources(this._txtSearchName, "_txtSearchName");
            this._txtSearchName.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._txtSearchName.Name = "_txtSearchName";
            this._toolTip.SetToolTip(this._txtSearchName, resources.GetString("_txtSearchName.ToolTip"));
            this._txtSearchName.Enter += new System.EventHandler(this._txtSearchName_Enter);
            this._txtSearchName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSearchName_KeyDown);
            this._txtSearchName.KeyUp += new System.Windows.Forms.KeyEventHandler(this._txtSearchName_KeyUp);
            this._txtSearchName.Leave += new System.EventHandler(this._txtSearchName_Leave);
            this._txtSearchName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cmbSearchName_MouseDoubleClick);
            // 
            // _btnAddNoteAndLink
            // 
            resources.ApplyResources(this._btnAddNoteAndLink, "_btnAddNoteAndLink");
            this._btnAddNoteAndLink.Name = "_btnAddNoteAndLink";
            this._toolTip.SetToolTip(this._btnAddNoteAndLink, resources.GetString("_btnAddNoteAndLink.ToolTip"));
            this._btnAddNoteAndLink.UseVisualStyleBackColor = true;
            this._btnAddNoteAndLink.Click += new System.EventHandler(this.btnAddNoteAndLinkDescription_Click);
            // 
            // _btnAddNote
            // 
            resources.ApplyResources(this._btnAddNote, "_btnAddNote");
            this._btnAddNote.Name = "_btnAddNote";
            this._toolTip.SetToolTip(this._btnAddNote, resources.GetString("_btnAddNote.ToolTip"));
            this._btnAddNote.UseVisualStyleBackColor = true;
            this._btnAddNote.Click += new System.EventHandler(this.btnAddNote_Click);
            // 
            // _btnAddConstraint
            // 
            resources.ApplyResources(this._btnAddConstraint, "_btnAddConstraint");
            this._btnAddConstraint.Name = "_btnAddConstraint";
            this._toolTip.SetToolTip(this._btnAddConstraint, resources.GetString("_btnAddConstraint.ToolTip"));
            this._btnAddConstraint.UseVisualStyleBackColor = true;
            this._btnAddConstraint.Click += new System.EventHandler(this.btnAddConstraint_Click);
            // 
            // contextMenuRtf
            // 
            this.contextMenuRtf.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuRtf.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editSQLToolStripMenuItem,
            this.showSQLFolderToolStripMenuItem,
            this.showSQLPathToolStripMenuItem1,
            this.showDescriptionToolStripMenuItem,
            this.toolStripSeparator6,
            this.runToolStripMenuItem,
            this.runAndExportSQLToExcelToolStripMenuItem,
            this.toolStripSeparator7,
            this.exportCsvOfClipboardToExcelToolStripMenuItem1});
            this.contextMenuRtf.Name = "contextMenuRtf";
            resources.ApplyResources(this.contextMenuRtf, "contextMenuRtf");
            this._toolTip.SetToolTip(this.contextMenuRtf, resources.GetString("contextMenuRtf.ToolTip"));
            // 
            // editSQLToolStripMenuItem
            // 
            this.editSQLToolStripMenuItem.Name = "editSQLToolStripMenuItem";
            resources.ApplyResources(this.editSQLToolStripMenuItem, "editSQLToolStripMenuItem");
            this.editSQLToolStripMenuItem.Click += new System.EventHandler(this.editSqlRtfToolStripMenuItem_Click);
            // 
            // showSQLFolderToolStripMenuItem
            // 
            this.showSQLFolderToolStripMenuItem.Name = "showSQLFolderToolStripMenuItem";
            resources.ApplyResources(this.showSQLFolderToolStripMenuItem, "showSQLFolderToolStripMenuItem");
            this.showSQLFolderToolStripMenuItem.Click += new System.EventHandler(this.showSqlFolderRtfToolStripMenuItem_Click);
            // 
            // showSQLPathToolStripMenuItem1
            // 
            this.showSQLPathToolStripMenuItem1.Name = "showSQLPathToolStripMenuItem1";
            resources.ApplyResources(this.showSQLPathToolStripMenuItem1, "showSQLPathToolStripMenuItem1");
            this.showSQLPathToolStripMenuItem1.Click += new System.EventHandler(this.showSQLPathToolStripMenuItem_Click);
            // 
            // showDescriptionToolStripMenuItem
            // 
            this.showDescriptionToolStripMenuItem.Name = "showDescriptionToolStripMenuItem";
            resources.ApplyResources(this.showDescriptionToolStripMenuItem, "showDescriptionToolStripMenuItem");
            this.showDescriptionToolStripMenuItem.Click += new System.EventHandler(this.showDescriptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            resources.ApplyResources(this.runToolStripMenuItem, "runToolStripMenuItem");
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // runAndExportSQLToExcelToolStripMenuItem
            // 
            this.runAndExportSQLToExcelToolStripMenuItem.Name = "runAndExportSQLToExcelToolStripMenuItem";
            resources.ApplyResources(this.runAndExportSQLToExcelToolStripMenuItem, "runAndExportSQLToExcelToolStripMenuItem");
            this.runAndExportSQLToExcelToolStripMenuItem.Click += new System.EventHandler(this.runAndExportSQLToExcelRtfToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // exportCsvOfClipboardToExcelToolStripMenuItem1
            // 
            this.exportCsvOfClipboardToExcelToolStripMenuItem1.Name = "exportCsvOfClipboardToExcelToolStripMenuItem1";
            resources.ApplyResources(this.exportCsvOfClipboardToExcelToolStripMenuItem1, "exportCsvOfClipboardToExcelToolStripMenuItem1");
            this.exportCsvOfClipboardToExcelToolStripMenuItem1.Click += new System.EventHandler(this.runAndExportCsvToExcelRtfToolStripMenuItem_Click);
            // 
            // _btnFeatureDown
            // 
            resources.ApplyResources(this._btnFeatureDown, "_btnFeatureDown");
            this._btnFeatureDown.Name = "_btnFeatureDown";
            this._toolTip.SetToolTip(this._btnFeatureDown, resources.GetString("_btnFeatureDown.ToolTip"));
            this._btnFeatureDown.UseVisualStyleBackColor = true;
            this._btnFeatureDown.Click += new System.EventHandler(this._btnFeatureDown_Click);
            // 
            // _btnFeatureUp
            // 
            resources.ApplyResources(this._btnFeatureUp, "_btnFeatureUp");
            this._btnFeatureUp.Name = "_btnFeatureUp";
            this._toolTip.SetToolTip(this._btnFeatureUp, resources.GetString("_btnFeatureUp.ToolTip"));
            this._btnFeatureUp.UseVisualStyleBackColor = true;
            this._btnFeatureUp.Click += new System.EventHandler(this._btnFeatureUp_Click);
            // 
            // _rtfListOfSearches
            // 
            this._rtfListOfSearches.ContextMenuStrip = this.contextMenuRtf;
            resources.ApplyResources(this._rtfListOfSearches, "_rtfListOfSearches");
            this._rtfListOfSearches.Name = "_rtfListOfSearches";
            this._rtfListOfSearches.ReadOnly = true;
            this._rtfListOfSearches.Enter += new System.EventHandler(this.rtfListOfSearches_Enter);
            this._rtfListOfSearches.Leave += new System.EventHandler(this.rtfListOfSearches_Leave);
            this._rtfListOfSearches.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtfListOfSearches_MouseDoubleClick);
            this._rtfListOfSearches.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtfListOfSearches_MouseDown);
            this._rtfListOfSearches.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtfListOfSearches_MouseUp);
            // 
            // _menuStrip1
            // 
            this._menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem,
            this._doToolStripMenuItem,
            this._versionControlToolStripMenuItem,
            this._portToolStripMenuItem,
            this._helpToolStripMenuItem,
            this.toolStripMenuIHome});
            resources.ApplyResources(this._menuStrip1, "_menuStrip1");
            this._menuStrip1.Name = "_menuStrip1";
            // 
            // _fileToolStripMenuItem
            // 
            this._fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._settingGeneralToolStripMenuItem,
            this._settingsToolStripMenuItem,
            this._settingsGlobalKeysToolStripMenuItem,
            this._settingsToolbarToolStripMenuItem,
            this._settingsQueryAndSctipToolStripMenuItem,
            this.toolStripSeparator12,
            this.settingsDiagramStylesToolStripMenuItem,
            this.toolStripSeparator11,
            this._updateScriptsToolStripMenuItem,
            this.toolStripSeparator10,
            this.resetFactorySettingsToolStripMenuItem});
            this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
            resources.ApplyResources(this._fileToolStripMenuItem, "_fileToolStripMenuItem");
            // 
            // _settingGeneralToolStripMenuItem
            // 
            this._settingGeneralToolStripMenuItem.Name = "_settingGeneralToolStripMenuItem";
            resources.ApplyResources(this._settingGeneralToolStripMenuItem, "_settingGeneralToolStripMenuItem");
            this._settingGeneralToolStripMenuItem.Click += new System.EventHandler(this.settingGeneralToolStripMenuItem_Click);
            // 
            // _settingsToolStripMenuItem
            // 
            this._settingsToolStripMenuItem.Name = "_settingsToolStripMenuItem";
            resources.ApplyResources(this._settingsToolStripMenuItem, "_settingsToolStripMenuItem");
            this._settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // _settingsGlobalKeysToolStripMenuItem
            // 
            this._settingsGlobalKeysToolStripMenuItem.Name = "_settingsGlobalKeysToolStripMenuItem";
            resources.ApplyResources(this._settingsGlobalKeysToolStripMenuItem, "_settingsGlobalKeysToolStripMenuItem");
            this._settingsGlobalKeysToolStripMenuItem.Click += new System.EventHandler(this.settingsKeysToolStripMenuItem_Click);
            // 
            // _settingsToolbarToolStripMenuItem
            // 
            this._settingsToolbarToolStripMenuItem.Name = "_settingsToolbarToolStripMenuItem";
            resources.ApplyResources(this._settingsToolbarToolStripMenuItem, "_settingsToolbarToolStripMenuItem");
            this._settingsToolbarToolStripMenuItem.Click += new System.EventHandler(this.settingsToolbarToolStripMenuItem_Click);
            // 
            // _settingsQueryAndSctipToolStripMenuItem
            // 
            this._settingsQueryAndSctipToolStripMenuItem.Name = "_settingsQueryAndSctipToolStripMenuItem";
            resources.ApplyResources(this._settingsQueryAndSctipToolStripMenuItem, "_settingsQueryAndSctipToolStripMenuItem");
            this._settingsQueryAndSctipToolStripMenuItem.Click += new System.EventHandler(this.settingsQueryAndSctipToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // settingsDiagramStylesToolStripMenuItem
            // 
            this.settingsDiagramStylesToolStripMenuItem.Name = "settingsDiagramStylesToolStripMenuItem";
            resources.ApplyResources(this.settingsDiagramStylesToolStripMenuItem, "settingsDiagramStylesToolStripMenuItem");
            this.settingsDiagramStylesToolStripMenuItem.Click += new System.EventHandler(this.settingsDiagramStylesToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // _updateScriptsToolStripMenuItem
            // 
            this._updateScriptsToolStripMenuItem.Name = "_updateScriptsToolStripMenuItem";
            resources.ApplyResources(this._updateScriptsToolStripMenuItem, "_updateScriptsToolStripMenuItem");
            this._updateScriptsToolStripMenuItem.Click += new System.EventHandler(this.updateScriptsToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // resetFactorySettingsToolStripMenuItem
            // 
            this.resetFactorySettingsToolStripMenuItem.Name = "resetFactorySettingsToolStripMenuItem";
            resources.ApplyResources(this.resetFactorySettingsToolStripMenuItem, "resetFactorySettingsToolStripMenuItem");
            this.resetFactorySettingsToolStripMenuItem.Click += new System.EventHandler(this.resetFactorySettingsToolStripMenuItem_Click);
            // 
            // _doToolStripMenuItem
            // 
            this._doToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportExcelToolStripMenuItem,
            this.exportCsvOfClipboardToExcelToolStripMenuItem,
            this.toolStripSeparator3,
            this._createActivityForOperationToolStripMenuItem,
            this._updateActivityFromOperationToolStripMenuItem,
            this._toolStripSeparator10,
            this._showFolderToolStripMenuItem,
            this.setFolderToolStripMenuItem,
            this.toolStripSeparator9,
            this._copyGuidsqlToClipboardToolStripMenuItem,
            this._toolStripSeparator1,
            this.changeAuthorPackagestandardToolStripMenuItem,
            this._changeAuthorRecursiveToolStripMenuItem,
            this._changeAuthorToolStripMenuItem});
            this._doToolStripMenuItem.Name = "_doToolStripMenuItem";
            resources.ApplyResources(this._doToolStripMenuItem, "_doToolStripMenuItem");
            // 
            // exportExcelToolStripMenuItem
            // 
            this.exportExcelToolStripMenuItem.Name = "exportExcelToolStripMenuItem";
            resources.ApplyResources(this.exportExcelToolStripMenuItem, "exportExcelToolStripMenuItem");
            this.exportExcelToolStripMenuItem.Click += new System.EventHandler(this.exportExcelToolStripMenuItem_Click);
            // 
            // exportCsvOfClipboardToExcelToolStripMenuItem
            // 
            this.exportCsvOfClipboardToExcelToolStripMenuItem.Name = "exportCsvOfClipboardToExcelToolStripMenuItem";
            resources.ApplyResources(this.exportCsvOfClipboardToExcelToolStripMenuItem, "exportCsvOfClipboardToExcelToolStripMenuItem");
            this.exportCsvOfClipboardToExcelToolStripMenuItem.Click += new System.EventHandler(this.exportCsvOfClipboardToExcelToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // _createActivityForOperationToolStripMenuItem
            // 
            this._createActivityForOperationToolStripMenuItem.Name = "_createActivityForOperationToolStripMenuItem";
            resources.ApplyResources(this._createActivityForOperationToolStripMenuItem, "_createActivityForOperationToolStripMenuItem");
            this._createActivityForOperationToolStripMenuItem.Click += new System.EventHandler(this.createActivityForOperationToolStripMenuItem_Click);
            // 
            // _updateActivityFromOperationToolStripMenuItem
            // 
            this._updateActivityFromOperationToolStripMenuItem.Name = "_updateActivityFromOperationToolStripMenuItem";
            resources.ApplyResources(this._updateActivityFromOperationToolStripMenuItem, "_updateActivityFromOperationToolStripMenuItem");
            this._updateActivityFromOperationToolStripMenuItem.Click += new System.EventHandler(this.updateActivityFromOperationToolStripMenuItem_Click);
            // 
            // _toolStripSeparator10
            // 
            this._toolStripSeparator10.Name = "_toolStripSeparator10";
            resources.ApplyResources(this._toolStripSeparator10, "_toolStripSeparator10");
            // 
            // _showFolderToolStripMenuItem
            // 
            this._showFolderToolStripMenuItem.Name = "_showFolderToolStripMenuItem";
            resources.ApplyResources(this._showFolderToolStripMenuItem, "_showFolderToolStripMenuItem");
            this._showFolderToolStripMenuItem.Click += new System.EventHandler(this.showFolderToolStripMenuItem_Click);
            // 
            // setFolderToolStripMenuItem
            // 
            this.setFolderToolStripMenuItem.Name = "setFolderToolStripMenuItem";
            resources.ApplyResources(this.setFolderToolStripMenuItem, "setFolderToolStripMenuItem");
            this.setFolderToolStripMenuItem.Click += new System.EventHandler(this.setFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // _copyGuidsqlToClipboardToolStripMenuItem
            // 
            this._copyGuidsqlToClipboardToolStripMenuItem.Name = "_copyGuidsqlToClipboardToolStripMenuItem";
            resources.ApplyResources(this._copyGuidsqlToClipboardToolStripMenuItem, "_copyGuidsqlToClipboardToolStripMenuItem");
            this._copyGuidsqlToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyGUIDSQLToClipboardToolStripMenuItem_Click);
            // 
            // _toolStripSeparator1
            // 
            this._toolStripSeparator1.Name = "_toolStripSeparator1";
            resources.ApplyResources(this._toolStripSeparator1, "_toolStripSeparator1");
            // 
            // changeAuthorPackagestandardToolStripMenuItem
            // 
            this.changeAuthorPackagestandardToolStripMenuItem.Name = "changeAuthorPackagestandardToolStripMenuItem";
            resources.ApplyResources(this.changeAuthorPackagestandardToolStripMenuItem, "changeAuthorPackagestandardToolStripMenuItem");
            this.changeAuthorPackagestandardToolStripMenuItem.Click += new System.EventHandler(this.changeAuthorPackageToolStripMenuItem_Click);
            // 
            // _changeAuthorRecursiveToolStripMenuItem
            // 
            this._changeAuthorRecursiveToolStripMenuItem.Name = "_changeAuthorRecursiveToolStripMenuItem";
            resources.ApplyResources(this._changeAuthorRecursiveToolStripMenuItem, "_changeAuthorRecursiveToolStripMenuItem");
            this._changeAuthorRecursiveToolStripMenuItem.Click += new System.EventHandler(this.changeAuthorRecursiveToolStripMenuItem_Click);
            // 
            // _changeAuthorToolStripMenuItem
            // 
            this._changeAuthorToolStripMenuItem.Name = "_changeAuthorToolStripMenuItem";
            resources.ApplyResources(this._changeAuthorToolStripMenuItem, "_changeAuthorToolStripMenuItem");
            this._changeAuthorToolStripMenuItem.Click += new System.EventHandler(this.changeAuthorToolStripMenuItem_Click);
            // 
            // _versionControlToolStripMenuItem
            // 
            this._versionControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._changeXmlFileToolStripMenuItem,
            this._showFolderVCorCodeToolStripMenuItem,
            this._getVcLatesrecursiveToolStripMenuItem,
            this._toolStripSeparator2,
            this._showTortoiseLogToolStripMenuItem,
            this._showTortoiseRepoBrowserToolStripMenuItem,
            this._setSvnKeywordsToolStripMenuItem,
            this._setSvnModuleTaggedValuesToolStripMenuItem,
            this._setSvnModuleTaggedValuesrecursiveToolStripMenuItem});
            this._versionControlToolStripMenuItem.Name = "_versionControlToolStripMenuItem";
            resources.ApplyResources(this._versionControlToolStripMenuItem, "_versionControlToolStripMenuItem");
            // 
            // _changeXmlFileToolStripMenuItem
            // 
            this._changeXmlFileToolStripMenuItem.Name = "_changeXmlFileToolStripMenuItem";
            resources.ApplyResources(this._changeXmlFileToolStripMenuItem, "_changeXmlFileToolStripMenuItem");
            this._changeXmlFileToolStripMenuItem.Click += new System.EventHandler(this.changeXMLFileToolStripMenuItem_Click);
            // 
            // _showFolderVCorCodeToolStripMenuItem
            // 
            this._showFolderVCorCodeToolStripMenuItem.Name = "_showFolderVCorCodeToolStripMenuItem";
            resources.ApplyResources(this._showFolderVCorCodeToolStripMenuItem, "_showFolderVCorCodeToolStripMenuItem");
            this._showFolderVCorCodeToolStripMenuItem.Click += new System.EventHandler(this.showFolderVCorCodeToolStripMenuItem_Click);
            // 
            // _getVcLatesrecursiveToolStripMenuItem
            // 
            this._getVcLatesrecursiveToolStripMenuItem.Name = "_getVcLatesrecursiveToolStripMenuItem";
            resources.ApplyResources(this._getVcLatesrecursiveToolStripMenuItem, "_getVcLatesrecursiveToolStripMenuItem");
            this._getVcLatesrecursiveToolStripMenuItem.Click += new System.EventHandler(this.getVCLatesrecursiveToolStripMenuItem_Click);
            // 
            // _toolStripSeparator2
            // 
            this._toolStripSeparator2.Name = "_toolStripSeparator2";
            resources.ApplyResources(this._toolStripSeparator2, "_toolStripSeparator2");
            // 
            // _showTortoiseLogToolStripMenuItem
            // 
            this._showTortoiseLogToolStripMenuItem.Name = "_showTortoiseLogToolStripMenuItem";
            resources.ApplyResources(this._showTortoiseLogToolStripMenuItem, "_showTortoiseLogToolStripMenuItem");
            this._showTortoiseLogToolStripMenuItem.Click += new System.EventHandler(this.showTortoiseLogToolStripMenuItem_Click);
            // 
            // _showTortoiseRepoBrowserToolStripMenuItem
            // 
            this._showTortoiseRepoBrowserToolStripMenuItem.Name = "_showTortoiseRepoBrowserToolStripMenuItem";
            resources.ApplyResources(this._showTortoiseRepoBrowserToolStripMenuItem, "_showTortoiseRepoBrowserToolStripMenuItem");
            this._showTortoiseRepoBrowserToolStripMenuItem.Click += new System.EventHandler(this.showTortoiseRepoBrowserToolStripMenuItem_Click);
            // 
            // _setSvnKeywordsToolStripMenuItem
            // 
            this._setSvnKeywordsToolStripMenuItem.Name = "_setSvnKeywordsToolStripMenuItem";
            resources.ApplyResources(this._setSvnKeywordsToolStripMenuItem, "_setSvnKeywordsToolStripMenuItem");
            this._setSvnKeywordsToolStripMenuItem.Click += new System.EventHandler(this.setSvnKeywordsToolStripMenuItem_Click);
            // 
            // _setSvnModuleTaggedValuesToolStripMenuItem
            // 
            this._setSvnModuleTaggedValuesToolStripMenuItem.Name = "_setSvnModuleTaggedValuesToolStripMenuItem";
            resources.ApplyResources(this._setSvnModuleTaggedValuesToolStripMenuItem, "_setSvnModuleTaggedValuesToolStripMenuItem");
            this._setSvnModuleTaggedValuesToolStripMenuItem.Click += new System.EventHandler(this.setSvnModuleTaggedValuesToolStripMenuItem_Click);
            // 
            // _setSvnModuleTaggedValuesrecursiveToolStripMenuItem
            // 
            this._setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Name = "_setSvnModuleTaggedValuesrecursiveToolStripMenuItem";
            resources.ApplyResources(this._setSvnModuleTaggedValuesrecursiveToolStripMenuItem, "_setSvnModuleTaggedValuesrecursiveToolStripMenuItem");
            this._setSvnModuleTaggedValuesrecursiveToolStripMenuItem.Click += new System.EventHandler(this.setSvnModuleTaggedValuesrecursiveToolStripMenuItem_Click);
            // 
            // _portToolStripMenuItem
            // 
            this._portToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._movePortsToolStripMenuItem,
            this._toolStripSeparator7,
            this._deletePortsToolStripMenuItem,
            this._toolStripSeparator3,
            this._connectPortsToolStripMenuItem,
            this._connectPortsInsideComponentsToolStripMenuItem,
            this._toolStripSeparator8,
            this._makeConnectorsUnspecifiedDirectionToolStripMenuItem,
            this._toolStripSeparator4,
            this._showPortsInDiagramObjectsToolStripMenuItem,
            this._showSendingPortsLeftRecievingPortsRightToolStripMenuItem,
            this._hidePortsInDiagramToolStripMenuItem,
            this._toolStripSeparator5,
            this._unhidePortsToolStripMenuItem,
            this._hidePortsToolStripMenuItem,
            this._toolStripSeparator11,
            this._movePortLabelLeftToolStripMenuItem,
            this._movePortLabelRightPositionToolStripMenuItem,
            this._toolStripSeparator12,
            this._movePortLabelLeftToolStripMenuItem1,
            this._movePortLabelRightToolStripMenuItem,
            this._toolStripSeparator9,
            this._orderDiagramItemsToolStripMenuItem});
            this._portToolStripMenuItem.Name = "_portToolStripMenuItem";
            resources.ApplyResources(this._portToolStripMenuItem, "_portToolStripMenuItem");
            // 
            // _movePortsToolStripMenuItem
            // 
            this._movePortsToolStripMenuItem.Name = "_movePortsToolStripMenuItem";
            resources.ApplyResources(this._movePortsToolStripMenuItem, "_movePortsToolStripMenuItem");
            this._movePortsToolStripMenuItem.Click += new System.EventHandler(this.copyPortsToolStripMenuItem_Click);
            // 
            // _toolStripSeparator7
            // 
            this._toolStripSeparator7.Name = "_toolStripSeparator7";
            resources.ApplyResources(this._toolStripSeparator7, "_toolStripSeparator7");
            // 
            // _deletePortsToolStripMenuItem
            // 
            this._deletePortsToolStripMenuItem.Name = "_deletePortsToolStripMenuItem";
            resources.ApplyResources(this._deletePortsToolStripMenuItem, "_deletePortsToolStripMenuItem");
            this._deletePortsToolStripMenuItem.Click += new System.EventHandler(this.deletePortsToolStripMenuItem_Click);
            // 
            // _toolStripSeparator3
            // 
            this._toolStripSeparator3.Name = "_toolStripSeparator3";
            resources.ApplyResources(this._toolStripSeparator3, "_toolStripSeparator3");
            // 
            // _connectPortsToolStripMenuItem
            // 
            this._connectPortsToolStripMenuItem.Name = "_connectPortsToolStripMenuItem";
            resources.ApplyResources(this._connectPortsToolStripMenuItem, "_connectPortsToolStripMenuItem");
            this._connectPortsToolStripMenuItem.Click += new System.EventHandler(this.connectPortsToolStripMenuItem_Click);
            // 
            // _connectPortsInsideComponentsToolStripMenuItem
            // 
            this._connectPortsInsideComponentsToolStripMenuItem.Name = "_connectPortsInsideComponentsToolStripMenuItem";
            resources.ApplyResources(this._connectPortsInsideComponentsToolStripMenuItem, "_connectPortsInsideComponentsToolStripMenuItem");
            this._connectPortsInsideComponentsToolStripMenuItem.Click += new System.EventHandler(this.connectPortsInsideComponentsToolStripMenuItem_Click);
            // 
            // _toolStripSeparator8
            // 
            this._toolStripSeparator8.Name = "_toolStripSeparator8";
            resources.ApplyResources(this._toolStripSeparator8, "_toolStripSeparator8");
            // 
            // _makeConnectorsUnspecifiedDirectionToolStripMenuItem
            // 
            this._makeConnectorsUnspecifiedDirectionToolStripMenuItem.Name = "_makeConnectorsUnspecifiedDirectionToolStripMenuItem";
            resources.ApplyResources(this._makeConnectorsUnspecifiedDirectionToolStripMenuItem, "_makeConnectorsUnspecifiedDirectionToolStripMenuItem");
            this._makeConnectorsUnspecifiedDirectionToolStripMenuItem.Click += new System.EventHandler(this.makeConnectorsUnspecifiedDirectionToolStripMenuItem_Click);
            // 
            // _toolStripSeparator4
            // 
            this._toolStripSeparator4.Name = "_toolStripSeparator4";
            resources.ApplyResources(this._toolStripSeparator4, "_toolStripSeparator4");
            // 
            // _showPortsInDiagramObjectsToolStripMenuItem
            // 
            this._showPortsInDiagramObjectsToolStripMenuItem.Name = "_showPortsInDiagramObjectsToolStripMenuItem";
            resources.ApplyResources(this._showPortsInDiagramObjectsToolStripMenuItem, "_showPortsInDiagramObjectsToolStripMenuItem");
            this._showPortsInDiagramObjectsToolStripMenuItem.Click += new System.EventHandler(this.showPortsInDiagramObjectsToolStripMenuItem_Click);
            // 
            // _showSendingPortsLeftRecievingPortsRightToolStripMenuItem
            // 
            this._showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Name = "_showSendingPortsLeftRecievingPortsRightToolStripMenuItem";
            resources.ApplyResources(this._showSendingPortsLeftRecievingPortsRightToolStripMenuItem, "_showSendingPortsLeftRecievingPortsRightToolStripMenuItem");
            this._showSendingPortsLeftRecievingPortsRightToolStripMenuItem.Click += new System.EventHandler(this.showReceivingPortsLeftSendingPortsRightToolStripMenuItem_Click);
            // 
            // _hidePortsInDiagramToolStripMenuItem
            // 
            this._hidePortsInDiagramToolStripMenuItem.Name = "_hidePortsInDiagramToolStripMenuItem";
            resources.ApplyResources(this._hidePortsInDiagramToolStripMenuItem, "_hidePortsInDiagramToolStripMenuItem");
            this._hidePortsInDiagramToolStripMenuItem.Click += new System.EventHandler(this.removePortsInDiagramToolStripMenuItem_Click);
            // 
            // _toolStripSeparator5
            // 
            this._toolStripSeparator5.Name = "_toolStripSeparator5";
            resources.ApplyResources(this._toolStripSeparator5, "_toolStripSeparator5");
            // 
            // _unhidePortsToolStripMenuItem
            // 
            this._unhidePortsToolStripMenuItem.Name = "_unhidePortsToolStripMenuItem";
            resources.ApplyResources(this._unhidePortsToolStripMenuItem, "_unhidePortsToolStripMenuItem");
            this._unhidePortsToolStripMenuItem.Click += new System.EventHandler(this.viewPortLabelToolStripMenuItem_Click);
            // 
            // _hidePortsToolStripMenuItem
            // 
            this._hidePortsToolStripMenuItem.Name = "_hidePortsToolStripMenuItem";
            resources.ApplyResources(this._hidePortsToolStripMenuItem, "_hidePortsToolStripMenuItem");
            this._hidePortsToolStripMenuItem.Click += new System.EventHandler(this.hidePortLabelToolStripMenuItem_Click);
            // 
            // _toolStripSeparator11
            // 
            this._toolStripSeparator11.Name = "_toolStripSeparator11";
            resources.ApplyResources(this._toolStripSeparator11, "_toolStripSeparator11");
            // 
            // _movePortLabelLeftToolStripMenuItem
            // 
            this._movePortLabelLeftToolStripMenuItem.Name = "_movePortLabelLeftToolStripMenuItem";
            resources.ApplyResources(this._movePortLabelLeftToolStripMenuItem, "_movePortLabelLeftToolStripMenuItem");
            this._movePortLabelLeftToolStripMenuItem.Click += new System.EventHandler(this.movePortLableLeftPositionToolStripMenuItem_Click);
            // 
            // _movePortLabelRightPositionToolStripMenuItem
            // 
            this._movePortLabelRightPositionToolStripMenuItem.Name = "_movePortLabelRightPositionToolStripMenuItem";
            resources.ApplyResources(this._movePortLabelRightPositionToolStripMenuItem, "_movePortLabelRightPositionToolStripMenuItem");
            this._movePortLabelRightPositionToolStripMenuItem.Click += new System.EventHandler(this.movePortLableRightPositionToolStripMenuItem_Click);
            // 
            // _toolStripSeparator12
            // 
            this._toolStripSeparator12.Name = "_toolStripSeparator12";
            resources.ApplyResources(this._toolStripSeparator12, "_toolStripSeparator12");
            // 
            // _movePortLabelLeftToolStripMenuItem1
            // 
            this._movePortLabelLeftToolStripMenuItem1.Name = "_movePortLabelLeftToolStripMenuItem1";
            resources.ApplyResources(this._movePortLabelLeftToolStripMenuItem1, "_movePortLabelLeftToolStripMenuItem1");
            this._movePortLabelLeftToolStripMenuItem1.Click += new System.EventHandler(this.movePortLableMinusPositionToolStripMenuItem_Click);
            // 
            // _movePortLabelRightToolStripMenuItem
            // 
            this._movePortLabelRightToolStripMenuItem.Name = "_movePortLabelRightToolStripMenuItem";
            resources.ApplyResources(this._movePortLabelRightToolStripMenuItem, "_movePortLabelRightToolStripMenuItem");
            this._movePortLabelRightToolStripMenuItem.Click += new System.EventHandler(this.movePortLablePlusPositionToolStripMenuItem_Click);
            // 
            // _toolStripSeparator9
            // 
            this._toolStripSeparator9.Name = "_toolStripSeparator9";
            resources.ApplyResources(this._toolStripSeparator9, "_toolStripSeparator9");
            // 
            // _orderDiagramItemsToolStripMenuItem
            // 
            this._orderDiagramItemsToolStripMenuItem.Name = "_orderDiagramItemsToolStripMenuItem";
            resources.ApplyResources(this._orderDiagramItemsToolStripMenuItem, "_orderDiagramItemsToolStripMenuItem");
            // 
            // _helpToolStripMenuItem
            // 
            this._helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._getLastSqlErrorToolStripMenuItem,
            this.toolStripSeparator1,
            this.hoToolsToolStripMenuItem,
            this.settingsGeneralToolStripMenuItem,
            this.toolStripSeparator2,
            this._helpToolStripMenuItem1,
            this.gitHubToolStripMenuItem,
            this.toolStripSeparator8,
            this.readMeToolStripMenuItem,
            this._aboutToolStripMenuItem});
            this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            resources.ApplyResources(this._helpToolStripMenuItem, "_helpToolStripMenuItem");
            this._helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // _getLastSqlErrorToolStripMenuItem
            // 
            this._getLastSqlErrorToolStripMenuItem.Name = "_getLastSqlErrorToolStripMenuItem";
            resources.ApplyResources(this._getLastSqlErrorToolStripMenuItem, "_getLastSqlErrorToolStripMenuItem");
            this._getLastSqlErrorToolStripMenuItem.Click += new System.EventHandler(this.getLastSQLErrorToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // hoToolsToolStripMenuItem
            // 
            this.hoToolsToolStripMenuItem.Name = "hoToolsToolStripMenuItem";
            resources.ApplyResources(this.hoToolsToolStripMenuItem, "hoToolsToolStripMenuItem");
            this.hoToolsToolStripMenuItem.Click += new System.EventHandler(this.hoToolsToolStripMenuItem_Click);
            // 
            // settingsGeneralToolStripMenuItem
            // 
            this.settingsGeneralToolStripMenuItem.Name = "settingsGeneralToolStripMenuItem";
            resources.ApplyResources(this.settingsGeneralToolStripMenuItem, "settingsGeneralToolStripMenuItem");
            this.settingsGeneralToolStripMenuItem.Click += new System.EventHandler(this.settingsGeneralToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // _helpToolStripMenuItem1
            // 
            this._helpToolStripMenuItem1.Name = "_helpToolStripMenuItem1";
            resources.ApplyResources(this._helpToolStripMenuItem1, "_helpToolStripMenuItem1");
            this._helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            resources.ApplyResources(this.gitHubToolStripMenuItem, "gitHubToolStripMenuItem");
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.githubToolStripMenuItem1_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // readMeToolStripMenuItem
            // 
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            resources.ApplyResources(this.readMeToolStripMenuItem, "readMeToolStripMenuItem");
            this.readMeToolStripMenuItem.Click += new System.EventHandler(this.readMeToolStripMenuItem_Click);
            // 
            // _aboutToolStripMenuItem
            // 
            this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            resources.ApplyResources(this._aboutToolStripMenuItem, "_aboutToolStripMenuItem");
            this._aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuIHome
            // 
            resources.ApplyResources(this.toolStripMenuIHome, "toolStripMenuIHome");
            this.toolStripMenuIHome.Name = "toolStripMenuIHome";
            this.toolStripMenuIHome.Click += new System.EventHandler(this.toolStripMenuIHome_Click);
            // 
            // _panelButtons
            // 
            this._panelButtons.Controls.Add(this._toolStripContainer1);
            resources.ApplyResources(this._panelButtons, "_panelButtons");
            this._panelButtons.Name = "_panelButtons";
            // 
            // _panelLineStyle
            // 
            this._panelLineStyle.Controls.Add(this._btnLv);
            this._panelLineStyle.Controls.Add(this._btnLh);
            this._panelLineStyle.Controls.Add(this._btnTv);
            this._panelLineStyle.Controls.Add(this._btnTh);
            this._panelLineStyle.Controls.Add(this._btnC);
            this._panelLineStyle.Controls.Add(this._btnBezier);
            this._panelLineStyle.Controls.Add(this._btnOs);
            this._panelLineStyle.Controls.Add(this._btnOr);
            this._panelLineStyle.Controls.Add(this._btnA);
            this._panelLineStyle.Controls.Add(this._btnD);
            resources.ApplyResources(this._panelLineStyle, "_panelLineStyle");
            this._panelLineStyle.Name = "_panelLineStyle";
            // 
            // _panelFavorite
            // 
            this._panelFavorite.Controls.Add(this._btnAddFavorite);
            this._panelFavorite.Controls.Add(this._btnRemoveFavorite);
            this._panelFavorite.Controls.Add(this._btnShowFavorites);
            this._panelFavorite.Controls.Add(this._btnDisplayBehavior);
            resources.ApplyResources(this._panelFavorite, "_panelFavorite");
            this._panelFavorite.Name = "_panelFavorite";
            // 
            // _panelNote
            // 
            this._panelNote.Controls.Add(this._btnAddConstraint);
            this._panelNote.Controls.Add(this._btnAddNote);
            this._panelNote.Controls.Add(this._btnAddNoteAndLink);
            resources.ApplyResources(this._panelNote, "_panelNote");
            this._panelNote.Name = "_panelNote";
            // 
            // _panelPort
            // 
            this._panelPort.Controls.Add(this._lblPorts);
            this._panelPort.Controls.Add(this._btnLeft);
            this._panelPort.Controls.Add(this._btnUp);
            this._panelPort.Controls.Add(this._btnRight);
            this._panelPort.Controls.Add(this._btnDown);
            this._panelPort.Controls.Add(this._btnLabelLeft);
            this._panelPort.Controls.Add(this._btnLabelRight);
            resources.ApplyResources(this._panelPort, "_panelPort");
            this._panelPort.Name = "_panelPort";
            // 
            // _lblPorts
            // 
            resources.ApplyResources(this._lblPorts, "_lblPorts");
            this._lblPorts.Name = "_lblPorts";
            // 
            // _panelAdvanced
            // 
            this._panelAdvanced.Controls.Add(this._btnComposite);
            this._panelAdvanced.Controls.Add(this._btnFindUsage);
            this._panelAdvanced.Controls.Add(this._btnUpdateActivityParameter);
            this._panelAdvanced.Controls.Add(this._btnLocateType);
            this._panelAdvanced.Controls.Add(this._btnLocateOperation);
            this._panelAdvanced.Controls.Add(this._btnDisplaySpecification);
            resources.ApplyResources(this._panelAdvanced, "_panelAdvanced");
            this._panelAdvanced.Name = "_panelAdvanced";
            // 
            // _panelQuickSearch
            // 
            resources.ApplyResources(this._panelQuickSearch, "_panelQuickSearch");
            this._panelQuickSearch.Controls.Add(this._txtSearchName, 0, 0);
            this._panelQuickSearch.Controls.Add(this._txtSearchText, 1, 0);
            this._panelQuickSearch.Name = "_panelQuickSearch";
            // 
            // _toolTipRtfListOfSearches
            // 
            this._toolTipRtfListOfSearches.AutomaticDelay = 0;
            this._toolTipRtfListOfSearches.OwnerDraw = true;
            this._toolTipRtfListOfSearches.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this._toolTipRtfListOfSearches_Draw);
            this._toolTipRtfListOfSearches.Popup += new System.Windows.Forms.PopupEventHandler(this._toolTipRtfListOfSearches_Popup);
            // 
            // _panelConveyedItems
            // 
            this._panelConveyedItems.Controls.Add(this._btnFeatureDown);
            this._panelConveyedItems.Controls.Add(this._btnFeatureUp);
            this._panelConveyedItems.Controls.Add(this._btnReverseConnector);
            this._panelConveyedItems.Controls.Add(this._btnConveyedItem);
            resources.ApplyResources(this._panelConveyedItems, "_panelConveyedItems");
            this._panelConveyedItems.Name = "_panelConveyedItems";
            // 
            // AddinControlGui
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._rtfListOfSearches);
            this.Controls.Add(this._cmbSearchName);
            this.Controls.Add(this._panelPort);
            this.Controls.Add(this._panelNote);
            this.Controls.Add(this._panelAdvanced);
            this.Controls.Add(this._panelFavorite);
            this.Controls.Add(this._panelConveyedItems);
            this.Controls.Add(this._panelPortButtons);
            this.Controls.Add(this._panelLineStyle);
            this.Controls.Add(this._panelQuickSearch);
            this.Controls.Add(this._panelButtons);
            this.Controls.Add(this._menuStrip1);
            this.Name = "AddinControlGui";
            this.Resize += new System.EventHandler(this.AddinControlGui_Resize);
            this._toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer1.TopToolStripPanel.PerformLayout();
            this._toolStripContainer1.ResumeLayout(false);
            this._toolStripContainer1.PerformLayout();
            this._toolStripQuery.ResumeLayout(false);
            this._toolStripQuery.PerformLayout();
            this._contextMenuStripSearch.ResumeLayout(false);
            this._panelPortButtons.ResumeLayout(false);
            this.contextMenuRtf.ResumeLayout(false);
            this._menuStrip1.ResumeLayout(false);
            this._menuStrip1.PerformLayout();
            this._panelButtons.ResumeLayout(false);
            this._panelLineStyle.ResumeLayout(false);
            this._panelFavorite.ResumeLayout(false);
            this._panelNote.ResumeLayout(false);
            this._panelPort.ResumeLayout(false);
            this._panelPort.PerformLayout();
            this._panelAdvanced.ResumeLayout(false);
            this._panelQuickSearch.ResumeLayout(false);
            this._panelQuickSearch.PerformLayout();
            this._panelConveyedItems.ResumeLayout(false);
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
        public void ParameterizeMenusAndButtons()
        {
            // Don't change the order
            _panelPort.Visible = false;
            _panelNote.Visible = false;
            _panelAdvanced.Visible = false;
            _panelFavorite.Visible = false;
            _panelConveyedItems.Visible = false;
            _panelPortButtons.Visible = false;
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
            _panelPortButtons.ResumeLayout(false);
            _panelPortButtons.PerformLayout();
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
            _panelPortButtons.Visible = false;
            _panelLineStyle.Visible = false;
            _panelQuickSearch.Visible = false;
            _panelButtons.Visible = false;


            // Port Panels
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
            _panelConveyedItems.Visible = AddinSettings.IsConveyedItemsSupport || AddinSettings.IsReverseEdgeDirection || AddinSettings.IsOrderFeatures;
            _btnConveyedItem.Visible = AddinSettings.IsConveyedItemsSupport;
            _btnReverseConnector.Visible = AddinSettings.IsReverseEdgeDirection;
            _btnFeatureDown.Visible = AddinSettings.IsOrderFeatures;
            _btnFeatureUp.Visible = AddinSettings.IsOrderFeatures;

            // Port Buttons
            _panelPortButtons.Visible = AddinSettings.IsPortBasic || AddinSettings.IsPortType;

            // Line style Panel
            _panelLineStyle.Visible = AddinSettings.IsLineStyleSupport;

            // no quick search defined
            _panelQuickSearch.Visible = AddinSettings.IsQuickSearchSupport;
 
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

            // Port Buttons
            _btnHidePort.Visible = AddinSettings.IsPortBasic;
            _btnShowPort.Visible = AddinSettings.IsPortBasic;
            _btnHidePortLabel.Visible = AddinSettings.IsPortBasic;
            _btnShowPortLabel.Visible = AddinSettings.IsPortBasic;
            _btnHidePortType.Visible = AddinSettings.IsPortType;
            _btnShowPortType.Visible = AddinSettings.IsPortType;

            // Note in diagram support
            bool visibleDiagramNote = AddinSettings.IsAdvancedDiagramNote;
            _btnAddNoteAndLink.Visible = visibleDiagramNote;
            _btnAddNote.Visible = visibleDiagramNote;
            _btnAddConstraint.Visible = visibleDiagramNote;

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
            _btnConveyedItem.Visible = AddinSettings.IsConveyedItemsSupport;

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
        public void ParameterizeToolbarSearchButton()
        {
            _toolStripSearchBtn1.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn2.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn3.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn4.Visible = AddinSettings.IsShowQueryButton;
            _toolStripSearchBtn5.Visible = AddinSettings.IsShowQueryButton;

            for (int pos = 0; pos < AddinSettings.ButtonsConfigSearch.Length; pos++)
            {
                bool empty = false;
                const string defaultHelptext = "Free Model Searches, Model Search not parametrized.";
                string buttonText = "";
                string helpText = defaultHelptext;
                if (AddinSettings.ButtonsConfigSearch[pos] != null)
                {
                    EaAddinShortcutSearch search = (EaAddinShortcutSearch)AddinSettings.ButtonsConfigSearch[pos];
                    {
                        if (search.IsEmpty())
                        {
                            empty = true;
                        }
                        else
                        {
                            buttonText = search.KeyText;
                            helpText = search.HelpTextLong;
                        }
                    }
                }

                switch (pos)
                {
                    case 0:
                        _toolStripSearchBtn1.Visible = ! empty;
                        _toolStripSearchBtn1.Text = buttonText;
                        _toolStripSearchBtn1.ToolTipText = helpText;
                        break;
                    case 1:
                        _toolStripSearchBtn2.Visible = !empty;
                        _toolStripSearchBtn2.Text = buttonText;
                        _toolStripSearchBtn2.ToolTipText = helpText;
                        break;
                    case 2:
                        _toolStripSearchBtn3.Visible = !empty;
                        _toolStripSearchBtn3.Text = buttonText;
                        _toolStripSearchBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        _toolStripSearchBtn4.Visible = !empty;
                        _toolStripSearchBtn4.Text = buttonText;
                        _toolStripSearchBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        _toolStripSearchBtn5.Visible = !empty;
                        _toolStripSearchBtn5.Text = buttonText;
                        _toolStripSearchBtn5.ToolTipText = helpText;
                        break;

                }
            }
        }
        #endregion
        #region ParameterizeToolbarServiceButtons
        /// <summary>
        /// Parameterize Service Toolbar Buttons of Type Call or Script.
        /// Prerequisite:
        /// </summary>
        public void ParameterizeToolbarServiceButton()
        {
            _toolStripServiceBtn1.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn2.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn3.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn4.Visible = AddinSettings.IsShowServiceButton;
            _toolStripServiceBtn5.Visible = AddinSettings.IsShowServiceButton;
            for (int pos = 0; pos < AddinSettings.ButtonsServiceConfig.Count; pos++)
            {
                bool empty = false;
                string buttonText = "";
                string helpText = "free Service, Service not parametrized";
                if (AddinSettings.ButtonsServiceConfig[pos] != null)
                {
                    ServicesConfig serviceConfig = AddinSettings.ButtonsServiceConfig[pos];
                    if (serviceConfig.IsEmpty())
                    {
                        empty = true;
                    }
                    else
                    {
                        buttonText = serviceConfig.ButtonText;

                        var servicesConfigCall = serviceConfig as ServicesConfigCall;
                        helpText = servicesConfigCall != null 
                            ? $"{servicesConfigCall.HelpTextLong}" 
                            : $"{((ServicesConfigScript)serviceConfig).HelpTextLong}";
                    }
                }

                switch (pos)
                {
                    case 0:
                        _toolStripServiceBtn1.Visible = ! empty;
                        _toolStripServiceBtn1.Text = buttonText;
                        _toolStripServiceBtn1.ToolTipText = helpText;
                        break;
                    case 1:
                        _toolStripServiceBtn2.Visible = !empty;
                        _toolStripServiceBtn2.Text = buttonText;
                        _toolStripServiceBtn2.ToolTipText = helpText;
                        break;
                    case 2:
                        _toolStripServiceBtn3.Visible = !empty;
                        _toolStripServiceBtn3.Text = buttonText; 
                        _toolStripServiceBtn3.ToolTipText = helpText;
                        break;
                    case 3:
                        _toolStripServiceBtn4.Visible = !empty;
                        _toolStripServiceBtn4.Text = buttonText; 
                        _toolStripServiceBtn4.ToolTipText = helpText;
                        break;
                    case 4:
                        _toolStripServiceBtn5.Visible = !empty;
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
            _frmQueryAndScript.ShowDialog(this);
        }

        void settingGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsGeneral = new FrmSettingsGeneral(AddinSettings, this);
            _frmSettingsGeneral.ShowDialog(this);

        }

        void settingsToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsToolbar = new FrmSettingsToolbar(AddinSettings, this);
            _frmSettingsToolbar.ShowDialog(this);
        }
        void settingsKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmSettingsKey = new FrmSettingsKey(AddinSettings, this);
            _frmSettingsKey.ShowDialog(this);
        }

        /// <summary>
        /// Get Connectors with conveyed items for selected Element or vice versa
        /// Run search with selected Element
        /// - Lists Elements which are source of connectors
        /// - Locate Diagram get you to the diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConveyedItem_Click(object sender, EventArgs e)
        {
            EA.ObjectType type = Model.Repository.GetContextItemType();
            string sql;
            switch (type)
            {
                case EA.ObjectType.otElement:

                     sql = @"
                        select  s.ea_guid AS CLASSGUID, s.object_type AS CLASSTYPE, 
                                        s.name As [Source],     s.Object_Type As [SourceType],
                                        d.name As [Destination], d.object_Type As [DestinationType]
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
                    Model.SqlRun("ConveyeyItems", sql, "");
                    break;

                case EA.ObjectType.otConnector:
                     sql = @"
                        select  o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE, o.name As [Element], o.object_type As [Type]
                        from t_object o
                        where  o.object_id in ( #ConveyedItemIDS# )
                        ORDER BY 3
                    ";
                    // Run SQL with macro replacement
                    Model.SqlRun("ConveyeyItems", sql, "");
                    break;

                default:
                    MessageBox.Show(@"Select Connector or Element", @"No Element or Connector Selected!!!");
                    break;
            }
            

        }

        

        void getLastSQLErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(SqlError.GetEaSqlErrorFilePath());
        }

        

        private void AddinControlGui_Resize(object sender, EventArgs e)
        {
            ResizeRtfListOfChanges();
        }
        /// <summary>
        /// Leave the TextBox Search Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _txtSearchName_Leave(object sender, EventArgs e)
        {
            _txtSearchName.ForeColor = SystemColors.WindowText;
            if (_txtSearchName.Text.Trim().Equals(""))
            {
                _txtSearchName.Text =  @"<Search Name>" ;
                _txtSearchName.ForeColor = SystemColors.ControlDark;
            }
        }

        private void rtfListOfSearches_Enter(object sender, EventArgs e)
        {
            _rtfListOfSearches.Visible = true;
        }

        /// <summary>
        /// Mouse in rtf List of Searches double clicked. Run the search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtfListOfSearches_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RunSearchForCurrentRtfLine();
        }
        /// <summary>
        /// Run Search for current line of rtf text box
        /// <para/>- EA Search
        /// <para/>- SQL path with absolute and relative path (according to SQL Path)
        /// </summary>
        private void RunSearchForCurrentRtfLine()
        {
            int startPosLine = _rtfListOfSearches.GetFirstCharIndexOfCurrentLine();
            int lineNumber = _rtfListOfSearches.GetLineFromCharIndex(startPosLine);
            SearchItem searchItem = Search.GetSearch(lineNumber);
            string searchName = searchItem.Name;
            _txtSearchName.Text = searchName;

            // run Search
            _model.SearchRun(searchName, GetSearchTerm());
            _rtfListOfSearches.Visible = false;
        }


        private void _txtSearchName_Enter(object sender, EventArgs e)
        {
            _txtSearchName.ForeColor = SystemColors.WindowText;
            if (_txtSearchName.Text.Equals("<Search Name>")) _txtSearchName.Text = "";
            //IntializeSearches();
            _rtfListOfSearches.Visible = false;
        }
        private void _txtSearchText_Leave(object sender, EventArgs e)
        {
            _txtSearchText.ForeColor = SystemColors.WindowText;
            if (_txtSearchText.Text.Trim().Equals(""))
            {
                _txtSearchText.Text = @"<Search Term>";
                _txtSearchText.ForeColor = SystemColors.ControlDark;
            }
            _rtfListOfSearches.Visible = false;
        }
        private void _txtSearchText_MouseLeave(object sender, EventArgs e)
        {
           _rtfListOfSearches.Visible = false;
        }
        private void _txtSearchText_Enter(object sender, EventArgs e)
        {
            _txtSearchText.ForeColor = SystemColors.WindowText;
            if (_txtSearchText.Text.Contains("<Search Term>")) _txtSearchText.Text = "";
            _rtfListOfSearches.Visible = false;
        }
        /// <summary>
        /// Leaves list of available Searches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtfListOfSearches_Leave(object sender, EventArgs e)
        {
            _rtfListOfSearches.Visible = false;
            _toolTipRtfListOfSearches.Hide(_rtfListOfSearches);
        }
        /// <summary>
        /// Update Scripts, Searches, Diagram Styles,..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IntializeSearches();
            AddinSettings.UpdateModel(_model);
            GetValueSettingsFromJson();
        }

        /// <summary>
        /// Get current line of the Textbox.
        /// </summary>
        /// <param name="rtf"></param>
        /// <returns></returns>
        private static int GetLineForRtf(RichTextBox rtf)
        {
            int index = rtf.SelectionStart;
            return rtf.GetLineFromCharIndex(index);
        }

        /// <summary>
        /// Mouse up on the rtf search list hides the comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtfListOfSearches_MouseUp(object sender, MouseEventArgs e)
        {
            RichTextBox rtf = (sender as RichTextBox);
            if (rtf == _rtfListOfSearches)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _toolTipRtfListOfSearches.Hide(rtf);
            }
        }

        /// <summary>
        /// Mouse down on the rtf search list displays the comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtfListOfSearches_MouseDown(object sender, MouseEventArgs e)
        {
            RichTextBox rtf = (sender as RichTextBox);
            if (rtf == _rtfListOfSearches)
            {
                int line = GetLineForRtf(rtf);
                SearchItem searchItem = Search.GetSearch(line);

               Point locationMouse = e.Location;
               _toolTipRtfListOfSearches.Show(
                    searchItem.Description,
                    _rtfListOfSearches,
                locationMouse.X + e.X - 60,
                locationMouse.Y + e.Y + 15);
                
            }

        }
        

        private void _toolTipRtfListOfSearches_Draw(object sender, DrawToolTipEventArgs e)
        {

            // Draw the custom background.
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

            // Draw the standard border.
            e.DrawBorder();

            // Draw the custom text.
            // The using block will dispose the StringFormat automatically.
            using (StringFormat sf = new StringFormat())
            {
                //sf.Alignment = StringAlignment.Near;
                //sf.LineAlignment = StringAlignment.Near;
                //sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                //sf.FormatFlags = StringFormatFlags.NoWrap;
                using (Font f = new Font("Courier New", 10))
                {
                    e.Graphics.DrawString(e.ToolTipText, f,
                        SystemBrushes.ActiveCaptionText, e.Bounds, sf);
                }
            }

        }

        /// <summary>
        /// Popup Event to size the pop-up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _toolTipRtfListOfSearches_Popup(object sender, PopupEventArgs e)
        {
            // Determine the correct size (don't use balloon)
            using (Font f = new Font("Courier New", 10))
            {
                e.ToolTipSize = TextRenderer.MeasureText(
                    _toolTipRtfListOfSearches.GetToolTip(e.AssociatedControl), f);
            }
        }
        /// <summary>
        /// Run Search of current rtf line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunSearchForCurrentRtfLine();
        }

        /// <summary>
        /// Context Menu rtf: Edit the file under the Mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void editSqlRtfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchItem searchItem = GetSearchItemFromRtfLine();
            if (searchItem is EaSearchItem) return;
            EditSqlFile(searchItem.Name);
        }
        /// <summary>
        /// Context Menu rtf: Show folder of hoTools SQL search under the Mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showSqlFolderRtfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchItem searchItem = GetSearchItemFromRtfLine();
            if (searchItem is EaSearchItem) return;
            ShowFolderForSql(searchItem.Name);
        }
        private void runAndExportSQLToExcelRtfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchItem searchItem = GetSearchItemFromRtfLine();
            if (searchItem is EaSearchItem) return;
            _model.SearchRun(searchItem.Name, GetSearchTerm(), exportToExcel: true);
        }
        private void runAndExportCsvToExcelRtfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.MakeExcelFileFromCsv();
        }

        private void showDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchItem searchItem = GetSearchItemFromRtfLine();
            if (searchItem is EaSearchItem)
            {
                var eaSearchItem = searchItem as EaSearchItem;
                MessageBox.Show($@"Category: {eaSearchItem.Category}{Environment.NewLine}" +
                                $@"Releases: '{eaSearchItem.EARelease}'{Environment.NewLine}" +
                                $@"MDG ID: '{eaSearchItem.MdgId}'{Environment.NewLine}" +
                                $@"Description: {eaSearchItem.Description}{Environment.NewLine}",
                                $@"Info EA-Search: '{searchItem.Name}'");

            }
            else
            {   // SQL-File Search
                string sqlString = _globalCfg.ReadSqlFile(searchItem.Name);

                MessageBox.Show($@"{ _globalCfg.GetSqlFileLong(searchItem.Name)}" +
                                $@"{Environment.NewLine}Category: {searchItem.Category}{Environment.NewLine}{sqlString}",
                                $@"Info SQL-File: '{Path.GetFileName(searchItem.Name)}'");

            }
            

        }
        /// <summary>
        /// Get the values from the 'Settings.json' file and update the File Menu to accomplish bulk change be Menu
        /// - DiagramTypes
        /// </summary>

        private void GetValueSettingsFromJson()
        {
            try
            {
                // If Settings.json don't exists: Copy delivery Setting.json file to settings folder
                // ReSharper disable once AssignNullToNotNullAttribute
                string sourceSettingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    JasonFile);
                string targetSettingsPath = AddinSettings.ConfigFolderPath + "Settings.json";

                // Copy delivery Setting.json file to settings folder 
                if (!File.Exists(targetSettingsPath))
                {
                    File.Copy(sourceSettingsPath, targetSettingsPath);
                }

                // Add Diagram Style 
                // ReSharper disable once AssignNullToNotNullAttribute
                _jasonFilePath = targetSettingsPath;
                _diagramStyle = new EAAddinFramework.Utils.DiagramStyle(_jasonFilePath);
                EaService.DiagramStyle = _diagramStyle;


                // check if the menu entries already exists
                if (_doMenuDiagramStyleInserted)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int index = _doToolStripMenuItem.DropDownItems.Count - 1;
                        _doToolStripMenuItem.DropDownItems.RemoveAt(index);

                    }
                    

                }
                else
                {
                    _doToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                }
                // Diagram Style/Theme
                _doToolStripMenuItem.DropDownItems.Add(_diagramStyle.GetToolStripMenuDiagramStyle(
                    "Bulk Diagram Style/Theme Recursive",
                    "Bulk Change the Diagram Style/Theme recursive\r\nSelect\r\n-Package \r\n-Element \r\n-Diagrams",
                    ChangeStyleRecursiv_Click));
                _doToolStripMenuItem.DropDownItems.Add(_diagramStyle.GetToolStripMenuDiagramStyle(
                    "Bulk Change Diagram Style/Theme",
                    "Bulk Change the Diagram/Theme Style\r\nSelect\r\n-Package \r\n-Element \r\n-Diagrams",
                    ChangeStylePackage_Click));

                // DiagramObject Style
                _doToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                _doToolStripMenuItem.DropDownItems.Add(_diagramStyle.GetToolStripMenuDiagramObjectStyle(
                    "Bulk Change DiagramObject Style",
                    "Bulk Change the DiagramObject Style\r\nSelect\r\n-Diagram \r\n-DiagramObject/Node",
                    ChangeStylePackage_Click));





                _doMenuDiagramStyleInserted = true;
            }
            catch (Exception e1)
            {
                MessageBox.Show($@"'{_jasonFilePath}'

{e1}", "Error loading 'Settings.json'");
            }
        }


        /// <summary>
        /// Get Search Item for line in rtf
        /// </summary>
        /// <returns></returns>
        SearchItem GetSearchItemFromRtfLine()
        {
            int line = GetLineForRtf(_rtfListOfSearches);
            return Search.GetSearch(line);
        }

        /// <summary>
        /// Export current SQL to Excel and show it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.SearchRun(GetSearchName(), GetSearchTerm(), exportToExcel:true);
            _rtfListOfSearches.Visible = false;
        }

        private void exportCsvOfClipboardToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.MakeExcelFileFromCsv();
        }
        /// <summary>
        /// Context menu Search: Run Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.SearchRun(GetSearchName(), GetSearchTerm());
            _rtfListOfSearches.Visible = false;
        }

        /// <summary>
        /// Open the file in the editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSQLSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSqlFile(GetSearchName());
        }

        private void EditSqlFile(string fileName)
        {
            string sqlAbsFileName = _globalCfg.GetSqlFileName(fileName);
            // run editor
            if (sqlAbsFileName != "") Util.StartFile(sqlAbsFileName);
        }

        /// <summary>
        /// Show folder for SQL Script according to patch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showFolderSqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFolderForSql(GetSearchName());
        }

        private void ShowFolderForSql(string file)
        {
            string sqlAbsFileName = _globalCfg.GetSqlFileName(file);


            // Show folder
            if (sqlAbsFileName != "") Util.ShowFolder(sqlAbsFileName);
            else
            {
                List<string> sqlList = _globalCfg.GetListSqlPaths();
                if (sqlList.Count > 0) Util.ShowFolder(sqlList[0]);
                else
                    MessageBox.Show($"Configure SQL path in:{Environment.NewLine}File, Settings SQL and Script",
                        "No SQL path in settings defined!");
            }
        }

        /// <summary>
        /// Show the SQL path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showSQLPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sqlPath = _globalCfg.GetSqlPaths();
            MessageBox.Show(
                $"The SQL path is:{Environment.NewLine}{sqlPath}{Environment.NewLine}{Environment.NewLine}"+
                       $"Change SQL path with 'File, Settings SQL and Script'",
                "The SQL path to search for scripts");
        }

        private void toolStripMenuIHome_Click(object sender, EventArgs e)
        {
            WikiRef.Wiki();
        }

        private void hoToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.WikiHoTools();
        }

        private void settingsGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.WikiSettingsGeneral();
        }

        private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WikiRef.ReadMe();
        }

        private void _btnFeatureUp_Click(object sender, EventArgs e)
        {
            EaService.FeatureUp(Repository);
        }

        private void _btnFeatureDown_Click(object sender, EventArgs e)
        {
            EaService.FeatureDown(Repository);
        }

        private void setFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EaService.SetFolder(Repository);
        }

        // Change style recursive
        void ChangeStyleRecursiv_Click(object sender, EventArgs e)
        {
            ChangeDiagramStyle(sender, ChangeScope.PackageRecursive);
        }
        void ChangeStylePackage_Click(object sender, EventArgs e)
        {
            ChangeDiagramStyle(sender, ChangeScope.Package);
        }


        public void ChangeDiagramStyle(object sender, ChangeScope changeScope)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem; //((ToolStripMenuItem) sender).Tag; DiagramStyleItem
            DiagramStyleItem style = (DiagramStyleItem)item.Tag;

            // [0] styles
            // [1] diagram types
            string[] styleEx = { "", "" };
            styleEx[0] = $@"{style.Pdata};{style.StyleEx};";
            styleEx[1] = style.Type;
            EaService.ChangeDiagramStyle(Repository, styleEx, changeScope);
        }


        /// <summary>
        /// Change diagram object style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changeScope"></param>
        public void ChangeDiagramObjectStyle(object sender, ChangeScope changeScope)
        {


            ToolStripMenuItem item = sender as ToolStripMenuItem; 
            DiagramObjectStyleItem style = (DiagramObjectStyleItem)item.Tag;
            EaDiagram eaDia = new EaDiagram(Repository);
            if (eaDia.Dia == null) return;


            foreach (var diaObj in eaDia.SelObjects)
            {
                DiagramStyle.SetDiagramObjectStyle(Repository, diaObj, style.Style);
            }


           
        }


        private void settingsDiagramStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(_jasonFilePath);
        }

        private void resetFactorySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = AddinSettings.Reset();
            MessageBox.Show($@"Configuration saved to 
'{filePath}'
and deleted.

Please restart EA. During restart hoTools loads the default settings.
You may copy the saved file to 'user.config' in the same folder", "Configuration reset to default. Please Restart!");
        }
    }

    #endregion
}