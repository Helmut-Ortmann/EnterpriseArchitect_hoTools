﻿using System;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using hoTools.Utils.Favorites;
using hoTools.hoToolsGui;
using hoTools.Settings;
using hoTools.hoSqlGui;

using hoTools.EaServices;
using hoTools.Utils;
using hoTools.Utils.Appls;
using hoTools.Utils.ActionPins;
using EAAddinFramework.Utils;

using System.Reflection;
using hoTools.Find;
using hoTools.Utils.Configuration;
using GlobalHotkeys;
using hoLinqToSql.LinqUtils.Extensions;
using hoTools.ea;
using hoTools.Extensions;
using hoTools.EaServices.Names;
using LinqToDB;
using LinqToDB.DataProvider;
using VwTools_Utils.General;
using System.Runtime.InteropServices;

#region description
//---------------------------------------------------------------
//hoTools
//---------------------------------------------------------------
//Tools developed by Helmut Ortmann
//---------------------------------------------------------------
//Configuration:
//hoTools:    ..\Setup\HoToolsGui.dll.config.xml 
//VAR1:        ..\SetpVAR1\HoToolsGui.dll.config.xml 
//
//Customer=hoTools: "14F09211-3460-47A6-B837-A477491F0A67"
//         VAR1:    "F52AB09A-8ED0-4159-9AB4-FFD986983280"
//
//To initialize configuration:
//1. Delete: 
//hoTools:    %APPDATA%ho\hoTools\user.config
//VAR1:        %APPDATA%ho\VAR1\user.config
//2. (De)Install hoTools/hoTools_VAR1
//---------------------------------------------------------------
//Debug:
//1. Build in release mode        (only once to establish correct configuration)
//2. (De)Install Addin            (only once to establish correct configuration)
//3. Rebuild in debug mode
//4. Copy the correct HoToolsGui.dll.config.xml into the ..\AddinClass\src\.. !!
//5. Possibly delete old config file (%APPDATA%ho\hoTools or %APPDATA%ho\hoTools_VAR1)
//---------------------------------------------------------------
#endregion

namespace hoTools
{
    /// <summary>
    /// The main Addin class which calls the other tasks
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("661B3D5E-F2A4-385F-BCD8-6C5E8CB56929")]
    [ProgId("hoTools.hoToolsRoot")]
    public partial class HoToolsRoot : EaAddinBase
    {
        // Overwritten by AdinClass AssemblyFileVersion
        // This should be identical to installed product version from WIX installer (ProductVersion)
        string _release = "X.X.XXX.XX"; // Major, Minor, Build, free,


        // static due to global key definitions
        // ReSharper disable once InconsistentNaming
        //private static EA.Repository _Repository ;
        // ReSharper disable once InconsistentNaming
        static AddinSettings _AddinSettings;
        // ReSharper disable once InconsistentNaming
        static HoToolsGui _HoToolsGui;
        // ReSharper disable once InconsistentNaming
        static FindAndReplaceGUI _FindAndReplaceGUI;
        // ReSharper disable once InconsistentNaming
        static HoSqlGui _ScriptGUI;
        // ReSharper disable once InconsistentNaming
        static HoSqlGui _HoSqlGui;
        static ExtensionGui _extensionGui2;

        // ActiveX Controls
        HoToolsGui _myControlGui; // hoTools main window
        FindAndReplaceGUI _findAndReplaceGui;
        HoSqlGui _scriptGui;
        HoSqlGui _hoSqlGui;
        ExtensionGui _extensionGui;

        private static Model _model;        // to run SQL query file from global key


        // settings
        readonly AddinSettings _addinSettings;
 

        EA.Repository _repository;
        // define menu constants

        const string MenuGetContextInfo = "GetContextInfo";
        const string MenuShowWindow = "Show Window";
        const string MenuChangeXmlFile = "Change *.xml file for a version controlled package";

        const string MenuDisplayBehavior = "Display Behavior";
        const string MenuDisplayMethodDefinition = "Locate Operation";
        const string MenuLocateType = "Locate Type";
        const string MenuLocateCompositeElementorDiagram = "Locate CompositeElementOfDiagram";
        const string MenuLocateCompositeDiagramOfElement = "Locate CompositeDiagramOfElement";
        const string MenuShowSpecification = "Show Specification";
        const string MenuUnlockDiagram = "UnlockDiagram";

        // const string MenuDeviderLineStyleDia = "---------------Line style Diagram-----------------";
        const string MenuLineStyleDiaLh = "Line Style Diagram (Object): Lateral Horizontal";
        const string MenuLineStyleDiaLv = "Line Style Diagram (Object): Lateral Vertical";
        const string MenuLineStyleDiaTh = "Line Style Diagram (Object): Tree Horizontal";
        const string MenuLineStyleDiaTv = "Line Style Diagram (Object): Tree Vertical";
        const string MenuLineStyleDiaOs = "Line Style Diagram (Object): Orthogonal Square";

        const string MenuDividerActivity = "-------------Create Activity for Operation ---------------------------";
        const string MenuCreateActivityForOperation = "Create Activity for Operation (select operation or class)";
        const string MenuUpdateOperationParameter = "Update Operation Parameter for Activity (select Package(recursive), Activity, Class, Interface or Operation)";
        const string MenuUpdateActionPin = "Update Action Pin for Package (recursive)";

        const string MenuDividerInteraction = "-------------Create Interaction for Operation ---------------------------";
        const string MenuCreateInteractionForOperation = "&Create Interaction for Operation (select operation or class)  ";

        const string MenuDividerStateMachine = "-------------Create State Machine for Operation ---------------------------";
        const string MenuCreateStateMachineForOperation = "&Create State Machine for Operation (select operation)  ";


        // const string MenuCorrectTypes = "-------------Correct Type ---------------------------";
        const string MenuCorrectType = "Correct types of Attribute, Function (selected Attribute, Function, Class or Package)";

        const string MenuDividerCopyPast = "-------------Move links---------------------------"; 
        const string MenuCopyGuidToClipboard = "Copy Id / Select Statement to Clipboard";
        const string MenuCopyLinksToClipboard = "Copy Links to Clipboard";
        const string MenuPasteLinksFromClipboard = "Paste Links from Clipboard";

        const string MenuDividerNote = "-------------Note      ---------------------------"; 
        const string MenuAddLinkedNote = "Add linked Note";
        const string MenuAddLinkedDiagramNote = "Add linked Diagram Note";

        const string MenuTestDbAccess = "Test DB access (JET, SQlite, MySql)";

        const string MenuUsage = "Find Usage";
        const string MenuAbout = "About + Help";

        const string MenuDivider = "-----------------------------------------------";

        private bool _initFavoriteSearches;

        #region Constructor
        /// <summary>
        /// Constructor: Reade settings, set the menu header and menuOptions
        /// </summary>
        public HoToolsRoot()
        {
            try
            {
                // To check if addin is started
                // If the constructor has an exception th Addin isn't loaded, EA error is unspecific
                // Try{} Catch() don't always help
                //string s = $"0:AddinConstructor {DateTime.Now}";
                //System.IO.File.WriteAllText(@"D:\temp\AddinClass.log", s);

                _addinSettings = new AddinSettings();
                _AddinSettings = _addinSettings; // static

                // Initialize the names generator
                _addinSettings.NameGenerator = new NamesGenerator(_repository, _addinSettings.JasonFilePath);




            }
            catch (Exception e)
            {
                MessageBox.Show($@"Error setup 'hoTools' Addin. Error:{Environment.NewLine}{e}", 
                    @"hoTools Installation error");
            }
            // global configuration parameters independent of EA-Instance and used by services
            var globalCfg = HoToolsGlobalCfg.Instance;
            globalCfg.SetSqlPaths(_addinSettings.SqlPaths);
            globalCfg.SetLinqPadQueryPaths(_addinSettings.LinqPadQueryPath);
            globalCfg.TempFolder = _addinSettings.TempFolder;
            globalCfg.LprunPath = _addinSettings.LprunPath;
            globalCfg.IsLinqPadSupported = _addinSettings.IsLinqPadSupport;
            globalCfg.UseLinqPadConnection = _addinSettings.UseLinqPadConnection;
            globalCfg.LinqPadConnectionPath = _addinSettings.LinqPadConnectionPath;
            globalCfg.JasonFilePath = _addinSettings.JasonFilePath;

            // Extensions: c# Assemblies (*.dll, *.exe)
            globalCfg.SetExtensionPaths(_addinSettings.CodeExtensionsPath);
            globalCfg.Extensions = new hoTools.Utils.Extensions.Extension();

            globalCfg.ConfigPath = _addinSettings.ConfigFolderPath;



            // ReSharper disable once VirtualMemberCallInConstructor
            MenuHeader = "-" + _addinSettings.ProductName;
            // ReSharper disable once VirtualMemberCallInConstructor
            // ReSharper disable once RedundantExplicitArrayCreation
            menuOptions = new string[] {
                MenuGetContextInfo,
                //-------------------------- General  --------------------------//
                //    menuLocateCompositeElementorDiagram,
                //-------------------------- LineStyle --------------------------//
                                        
                //-------------------------- Activity --------------------------//
                MenuDividerActivity,
                MenuCreateActivityForOperation, MenuUpdateOperationParameter, 
                //menuUpdateActionPin,
                //-------------------------- Interaction --------------------------//
                MenuDividerInteraction,
                MenuCreateInteractionForOperation,
                //-------------------------- Interaction --------------------------//
                MenuDividerStateMachine,
                MenuCreateStateMachineForOperation,
                //-------------------------- Correct Types ------------------------//
                //menuCorrectTypes, menuCorrectType, 
                //-------------------------- Add Note -----------------------------//
                MenuDividerNote,
                MenuAddLinkedNote,MenuAddLinkedDiagramNote,

                MenuDivider,
                MenuTestDbAccess,

                //-------------------------- Move links ---------------------------//
                MenuDividerCopyPast,
                MenuChangeXmlFile, MenuCopyLinksToClipboard, MenuPasteLinksFromClipboard, 

                MenuShowWindow,    
                //---------------------------- About -------------------------------//
                MenuDivider, MenuAbout };
        }
        #endregion

        public object HoAddInSearchSample4(EA.Repository repository, String searchText, out String xmlResults)
        {
            xmlResults = "";
            return "";
        }
        public object HoAddInSearchSample5(EA.Repository repository, string searchText, out string xmlResults)
        {
            xmlResults = "";
            return "";
        }

        #region HotkeyHandlers
        /// <summary>
        /// Handle Global Keys
        /// </summary>
        private static class HotkeyHandlers
        {
            public static void SetupGlobalHotkeys()
            {
                // global hot keys
                var hotkeys = new List<Hotkey>();

                Dictionary<string, Keys> keys = GlobalKeysConfig.GetKeys();
                Dictionary<string, Modifiers> modifiers = GlobalKeysConfig.GetModifiers();
                Keys key;
                Modifiers modifier1;
                Modifiers modifier2;
                Modifiers modifier3;
                Modifiers modifier4;

                for (int i = 0; i < _AddinSettings.GlobalKeysConfig.Count; i = i + 1)
                {
                    GlobalKeysConfigSearch search = _AddinSettings.GlobalKeysConfigSearch[i];
                    if (search.Key != "None" & search.SearchName != "")
                    {
                        keys.TryGetValue(search.Key, out key);
                        modifiers.TryGetValue(search.Modifier1, out modifier1);
                        modifiers.TryGetValue(search.Modifier2, out modifier2);
                        modifiers.TryGetValue(search.Modifier3, out modifier3);
                        modifiers.TryGetValue(search.Modifier4, out modifier4);
                        switch (i)
                        {
                            case 0:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeySearch0));
                                break;
                            case 1:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeySearch1));
                                break;
                            case 2:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeySearch2));
                                break;
                            case 3:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeySearch3));
                                break;
                            case 4:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeySearch4));
                                break;
                        }
                    }

                }
                for (int i = 0; i < _AddinSettings.GlobalKeysConfig.Count; i = i + 1)
                {
                    GlobalKeysConfig service = _AddinSettings.GlobalKeysConfig[i];
                    if (service.Key != "None" & service.Id != "")
                    {
                        keys.TryGetValue(service.Key, out key);
                        modifiers.TryGetValue(service.Modifier1, out modifier1);
                        modifiers.TryGetValue(service.Modifier2, out modifier2);
                        modifiers.TryGetValue(service.Modifier3, out modifier3);
                        modifiers.TryGetValue(service.Modifier4, out modifier4);
                        switch (i)
                        {
                            case 0:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeyService0));
                                break;
                            case 1:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeyService1));
                                break;
                            case 2:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeyService2));
                                break;
                            case 3:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeyService3));
                                break;
                            case 4:
                                hotkeys.Add(new Hotkey(key, modifier1 | modifier2 | modifier3 | modifier4, HandleGlobalKeyService4));
                                break;
                        }
                    }

                }
                Form hotkeyForm = new InvisibleHotKeyForm(hotkeys);
                hotkeyForm.Show();
            }
            /// <summary>
            /// Service run global key search
            /// </summary>
            /// <param name="pos"></param>
            private static void RunGlobalKeySearch(int pos)
            {
                
                    GlobalKeysConfigSearch sh = _AddinSettings.GlobalKeysConfigSearch[pos];
                    if (sh.SearchName == "") return;
                    _model.SearchRun(sh.SearchName, sh.SearchTerm);
                    
            }
            /// <summary>
            /// Service function to run global keys
            /// </summary>
            /// <param name="pos"></param>
            private static void RunGlobalKeyService(int pos)
            {
                GlobalKeysConfig keyConfig = _AddinSettings.GlobalKeysConfig[pos];
                if (keyConfig is GlobalKeysConfigService)
                {
                    GlobalKeysConfigService keyConfigService = keyConfig as GlobalKeysConfigService;
                    if (keyConfigService.Method == null) return;
                    keyConfigService.Invoke(_model, _HoToolsGui.GetText());
                }
                if (keyConfig is GlobalKeysConfigScript)
                {
                    GlobalKeysConfigScript keyConfigScript= keyConfig as GlobalKeysConfigScript;
                    if (keyConfigScript.ScriptFunction == null) return;
                    keyConfigScript.Invoke(_model);
                }
            }

            private static void HandleGlobalKeySearch0()
            {
                RunGlobalKeySearch(0); 
            }
            private static void HandleGlobalKeySearch1()
            {
                RunGlobalKeySearch(1);
            }
            private static void HandleGlobalKeySearch2()
            {
                RunGlobalKeySearch(2);
            }
            private static void HandleGlobalKeySearch3()
            {
                RunGlobalKeySearch(3);
            }
            private static void HandleGlobalKeySearch4()
            {
                RunGlobalKeySearch(4);
            }
            private static void HandleGlobalKeyService0()
            {
                RunGlobalKeyService(0);
            }
            private static void HandleGlobalKeyService1()
            {
                RunGlobalKeyService(1);
            }
            private static void HandleGlobalKeyService2()
            {
                RunGlobalKeyService(2);
            }
            private static void HandleGlobalKeyService3()
            {
                RunGlobalKeyService(3);
            }
            private static void HandleGlobalKeyService4()
            {
                RunGlobalKeyService(4);
            }
                      
        }
        #endregion


        #region EA_On
        #region EA_OnContextItemChanged
        /// <summary>
        /// called when the selected item changes
        /// This operation will show the guid of the selected element in the eaControl
        /// </summary>
        /// <param name="repository">the EA.rep</param>
        /// <param name="guid">the guid of the selected item</param>
        /// <param name="objectType">the object type of the selected item</param>
        public override void EA_OnContextItemChanged(EA.Repository repository, string guid, EA.ObjectType objectType)
        { }
        #endregion
        #region EA_OnOutputItemDoubleClicked
        public override void EA_OnOutputItemDoubleClicked(EA.Repository repository, string tabName, 
            string lineText, long id)
        {

        }
        #endregion
        #region EA_OnPostInitialized
        public override void EA_OnPostInitialized(EA.Repository repository)
        {
            // gets the file 'AssemblyFileVersion' of file AddinClass.dll
            // ReSharper disable once AssignNullToNotNullAttribute
            string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            _release = productVersion;
            _repository = repository;
            ShowAddinControlWindows();

            // Activates the Addin tab
            ShowAddinTab(_AddinSettings.AddinTabToFirstActivate);


        }
        #endregion
        #region EA_OnPostNewConnector

        public override bool EA_OnPostNewConnector(EA.Repository rep, EA.EventProperties info)
        {
            if (rep == null) return false;
            EA.EventProperty eventProperty = info.Get(0);
            var s = (string) eventProperty.Value;
            // check if it is a diagram

            if (Int32.TryParse(s, out int connectorId) == false)
            {
                return false;
            }

            var dia = rep.GetCurrentDiagram();
            if (dia == null) return false; // e.g. Matrix has a diagram id but no diagram object
            if (connectorId == 0) return false;

            // unidentified error, exception in creating a connector in a relationship matrix.
            try
            {
                switch (dia.Type)
                {
                    case "Activity":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.ActivityLineStyle);


                    case "Statechart":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.StatechartLineStyle);

                    case "Logical":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.ClassLineStyle);


                    case "Custom":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.CustomLineStyle);

                    case "Component":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.ComponentLineStyle);

                    case "Deployment":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.DeploymentLineStyle);

                    case "Package":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.PackageLineStyle);

                    case "Use Case":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.UseCaseLineStyle);

                    case "CompositeStructure":
                        return UpdateLineStyle(rep, dia, connectorId, _addinSettings.CompositeStructureLineStyle);

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public override bool EA_OnPreNewElement(EA.Repository rep, EA.EventProperties info)
        {
            // to suppress EA Dialog after change
            rep.SuppressEADialogs = true;

            return true;
        }

        /// <summary>
        /// AutoIncrement generated elements.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool EA_OnPostNewElement(EA.Repository rep, EA.EventProperties info)
        {
            if (!_AddinSettings.IsAutoCounter) return false;
            EA.EventProperty eventProperty = info.Get(0);
            var s = (string)eventProperty.Value;
            // get element ID
            if (Int32.TryParse(s, out var elementId) == false)
            {
                return false;
            }
            EA.Element el = rep.GetElementByID(elementId);
            // Find the correct AutoIncrement configuration
            _addinSettings.NameGenerator.Rep = rep; // update repository in kind of repository change
            foreach (NamesGeneratorItem item in _addinSettings.NameGenerator.NameGeneratorItems)
            {
                if (item.ObjectType == el.Type && item.Stereotype == el.Stereotype)
                {
                    int highNumber = _addinSettings.NameGenerator.GetNextMost(item);
                    if (item.IsNameUpdate()) el.Name = item.GetString(highNumber);
                    else el.Alias = item.GetString(highNumber);

                    el.Update();
                    // Enable EA Property Dialog
                    rep.SuppressEADialogs = true;
                    return true;
                }
            }
            return false;
        }

        
        #endregion
        #region EA_ Connect File Open/Close
        #region EA_Connect
        /// <summary>
        /// EA_Connect events enable Add-Ins to identify their type and to respond to Enterprise Architect start up.
        /// This event occurs when Enterprise Architect first loads your Add-In. Enterprise Architect itself is loading at this time so that while a rep object is supplied, there is limited information that you can extract from it.
        /// The chief uses for EA_Connect are in initializing global Add-In data and for identifying the Add-In as an MDG Add-In.
        /// Also look at EA_Disconnect.
        /// </summary>
        /// <param name="repository">An EA.rep object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <returns>String identifying a specialized type of Add-In: 
        /// - "MDG" : MDG Add-Ins receive MDG Events and extra menu options.
        /// - "" : None-specialized Add-In.</returns>
        public override string EA_Connect(EA.Repository repository)
        {
            _repository = repository;
            try
            {
                // register only if configured
                if (_AddinSettings.IsShortKeySupport) HotkeyHandlers.SetupGlobalHotkeys();

                if (repository.IsSecurityEnabled)
                {
                    //logInUser = rep.GetCurrentLoginUser(false);
                    //if ((logInUser.Contains("ho")) ||
                    //     (logInUser.Contains("admin")) ||
                    //     (logInUser.Equals(""))
                    //    ) logInUserRights = UserRights.ADMIN;
                }
              
                // get all services of type Call and Script which are available 
                return "a string";

            }
            catch (Exception)
            {
                MessageBox.Show(@"{e]", @"hoTools: Error EA Connect");
                return "a string";
            }
           
        }
        #endregion
        // ReSharper disable once InconsistentNaming
        public override void EA_FileOpen(EA.Repository Repository)
        {
            InitializeForRepository(Repository);
        }
        // ReSharper disable once InconsistentNaming
        public override void EA_FileClose(EA.Repository Repository)
        {
            _myControlGui?.Save(); // save settings
            InitializeForRepository(null);


        }
        #endregion
        #region EA Validation
        /// <summary>
        /// Called when EA start model validation. Just shows a message box
        /// </summary>
        /// <param name="Repository">the rep</param>
        /// <param name="args">the arguments</param>
        // ReSharper disable once InconsistentNaming
        public override void EA_OnStartValidation(EA.Repository Repository, object args)
        {
            MessageBox.Show(@"Validation started");
        }
        /// <summary>
        /// Called when EA ends model validation. Just shows a message box
        /// </summary>
        /// <param name="repository">the rep</param>
        /// <param name="args">the arguments</param>
        public override void EA_OnEndValidation(EA.Repository repository, object args)
        {
            MessageBox.Show(@"Validation ended");
        }
        #endregion
        #region EA Technology Events
        /// <summary>
        /// Returns xml string for MDG to load. Possible values: Basic, Compilation, No). See _AddinSettings.AutoLoadMdgXml.
        /// It loads the *.xml from the *.dll install directory
        /// <para/>- DEBUG  Addin\bin\debug\
        /// <para/>- RELEASE  AppData\Local\Apps\hoTools\
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public override object EA_OnInitializeTechnologies(EA.Repository repository) {
           

            string fileNameMdg = _AddinSettings.GetAutoLoadMdgFileName();
            try
            {
                return Util.ReadAllText(fileNameMdg);
            } catch (Exception e)
            {
                MessageBox.Show($@"MDG file='{fileNameMdg}'{Environment.NewLine}{e}", 
                    $@"Can't load MDG '{fileNameMdg}'");
                return "";
            } 
        }
        #endregion

        #region initializeForRepository
        /// <summary>
        /// Initialize repositories for all EA hoTools Addin Tabulators
        /// </summary>
        /// <param name="rep"></param>
        void InitializeForRepository(EA.Repository rep)
        {
            if (rep == null) return;
            _repository = rep;
            _model = new Model(rep);
            _addinSettings.UpdateModel(_model);
            try
            {
                if (_myControlGui != null) _myControlGui.Repository = rep;
                if (_findAndReplaceGui != null) _findAndReplaceGui.Repository = rep;
                if (_scriptGui != null) _scriptGui.Repository = rep;
                if (_hoSqlGui != null) _hoSqlGui.Repository = rep;
                if (_extensionGui != null) _extensionGui.Repository = rep;
            } catch (Exception e)
            {
                MessageBox.Show($@"{e.Message}",@"hoTools: Error initializing Addin Tabs with repository");
            }

            if (_initFavoriteSearches)
            {
                Favorite.InstallSearches(_repository); // install searches
                _initFavoriteSearches = true;
            }


        }
        #endregion

        #region updateLineStyle
        private bool UpdateLineStyle(EA.Repository rep, EA.Diagram dia, int connectorId, string style)
        {
            if (style.ToUpper() == "NO") return false;
            foreach (EA.DiagramLink link in dia.DiagramLinks)
            {
                if (link.ConnectorID == connectorId)
                {
                    EaService.SetLineStyle(rep, style);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region EA_GetMenuState
        /// <summary>
        /// Called once Menu has been opened to see what menu items should active.
        /// </summary>
        /// <param name="Repository">the rep</param>
        /// <param name="menuLocation">the location of the menu</param>
        /// <param name="menuName">the name of the menu</param>
        /// <param name="itemName">the name of the menu item</param>
        /// <param name="isEnabled">boolean indicating whether the menu item is enabled</param>
        /// <param name="isChecked">boolean indicating whether the menu is checked</param>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once ParameterHidesMember
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        public override void EA_GetMenuState(EA.Repository Repository, string menuLocation, string menuName, string itemName, ref bool isEnabled, ref bool isChecked)
        {
            if (Repository == null) return;
            if (IsProjectOpen(Repository))
            {
                switch (itemName)
                {
                    case MenuChangeXmlFile:
                        isChecked = false;
                        break;

                    case MenuShowWindow:
                        isChecked = false;
                        break;
                    case  MenuShowSpecification:
                        isChecked = false;
                        break;

                    case MenuUnlockDiagram:
                        isChecked = false;
                        break;

                    case MenuLineStyleDiaTh:
                        isChecked = false;
                        break;
                    case MenuLineStyleDiaTv:
                        isChecked = false;
                        break;
                    case MenuLineStyleDiaLh:
                        isChecked = false;
                        break;
                    case MenuLineStyleDiaLv:
                        isChecked = false;
                        break;
                    case MenuLineStyleDiaOs:
                        isChecked = false;
                        break;

                    case MenuLocateCompositeElementorDiagram:
                        isChecked = false;
                        break;
                    
                    case MenuLocateCompositeDiagramOfElement:
                        isChecked = false;
                        break;
                        

                case MenuUsage:
                        isChecked = false;
                        break;    
                       
                case MenuCreateInteractionForOperation:
                        isChecked = false;
                        break;

                case MenuCreateStateMachineForOperation:
                        isChecked = false;
                        break;  
                    
                case MenuCorrectType:
                        isChecked = false;
                        break;

               case MenuDisplayBehavior:
                        isChecked = false;
                        break;


               case MenuUpdateActionPin:
                        isChecked = false;
                        break;

                    case MenuUpdateOperationParameter:
                        isChecked = false;
                        break;

                    case MenuCreateActivityForOperation:
                        isChecked = false;
                        break;

                    case MenuDisplayMethodDefinition:
                        isChecked = false;
                        break;

                    
                    case MenuLocateType:
                        isChecked = false;
                        break;


                    case MenuCopyGuidToClipboard:
                        isChecked = false;
                        break;

                    case MenuCopyLinksToClipboard:
                        isChecked = false;
                        break;

                    case MenuPasteLinksFromClipboard:
                        isChecked = false;
                        break;

                    
                    case MenuAddLinkedNote:
                        isChecked = false;
                        break;

                    case MenuAddLinkedDiagramNote:
                        isChecked = false;
                        break;

                    case MenuTestDbAccess:
                        isChecked = false;
                        isEnabled = true;
                        break;

                    case MenuAbout:
                        isChecked = false;
                        break;
                    case MenuGetContextInfo:
                        isChecked = false;
                        break;

                    // there shouldn't be any other, but just in case disable it.
                    default:
                        isEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                isEnabled = false;
            }
        }

        #endregion

        #region EA_MenuClick
        /// <summary>
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        /// </summary>
        /// <param name="repository">the rep</param>
        /// <param name="menuLocation">the location of the menu</param>
        /// <param name="menuName">the name of the menu</param>
        /// <param name="itemName">the name of the selected menu item</param>
        public override void EA_MenuClick(EA.Repository repository, string menuLocation, string menuName, string itemName)
        {
            EA.ObjectType oType = repository.GetContextItemType();
            EA.Diagram diaCurrent = repository.GetCurrentDiagram();
            EA.Element el;

            switch (itemName)
            {

                    case MenuShowWindow:
                    ShowAddinControlWindows();
                    
                    
                    break;
                case MenuAbout:

                    var fAbout = new About {lblTabName = {Text = _release}};
                    fAbout.ShowDialog();
                    break;

                    // Line style: Lateral Horizontal 
                case MenuChangeXmlFile:
                     EaService.SetNewXmlPath(repository);
                     break;
                // Line style: Lateral Horizontal 
                case MenuLineStyleDiaLh:
                     EaService.SetLineStyle(repository, "LH");
                   
                    break;
                // Line style: Lateral Vertical 
                case MenuLineStyleDiaLv:
                    // all connections of diagram
                    EaService.SetLineStyle(repository, "LV");
                    break;
                // Line style: Tree Vertical 
                case MenuLineStyleDiaTv:
                    EaService.SetLineStyle(repository, "V");
                    
                    break;

                // Line style: Tree Horizontal 
                case MenuLineStyleDiaTh:
                    EaService.SetLineStyle(repository, "H");
                    
                    break;
                // Line style: Orthogonal square 
                case MenuLineStyleDiaOs:
                    EaService.SetLineStyle(repository, "OS");
                    
                    break;

                case MenuGetContextInfo:
                    WriteInfoContext(repository);
                    break;

               



                //if (ItemName == menuHelp)
                //{
                //    Help fHelp = new Help();
                //    fHelp.ShowDialog();
                //    return;
                //}
                case MenuUnlockDiagram:
                    if (oType.Equals(EA.ObjectType.otDiagram))
                    {
                        try
                        {
                            string sql = @"update t_diagram set locked = 0" +
                           " where diagram_ID = " + diaCurrent.DiagramID;
                            repository.Execute(sql);
                            // reload view
                            repository.ReloadDiagram(diaCurrent.DiagramID);
                        }
                        #pragma warning disable RECS0022
                        catch
                        {
                            // ignored
                        }
#pragma warning restore RECS0022
                    }

                    break;

                // Start specification (file parameter)
                case  MenuShowSpecification:
                      EaService.ShowSpecification(repository);

                    break;

                // Create Interaction for selected operation or class (all operations)
                case   MenuCreateInteractionForOperation:
                    // Check selected Elements in tree
                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)repository.GetContextObject();
                        // test multiple selection

                        // Create Activity
                        Appl.CreateInteractionForOperation(repository, m);

                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        var cls = (EA.Element)repository.GetContextObject();
                        // over all operations of class
                        foreach (EA.Method m in cls.Methods)
                        {
                            // Create Activity
                            Appl.CreateInteractionForOperation(repository, m);

                        }
                    }

                    break;

                // Create Interaction for selected operation or class (all operations)
                case MenuCreateStateMachineForOperation:
                    // Check selected Elements in tree
                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)repository.GetContextObject();
                        // test multiple selection

                        // Create State Machine
                        Appl.CreateStateMachineForOperation(repository, m);

                    }
                    break;



                case MenuLocateCompositeElementorDiagram:
                   EaService.NavigateComposite(repository);
                    break;
                    
                // 
                case MenuCorrectType:
                    if (oType.Equals(EA.ObjectType.otAttribute))
                    {
                        var a = (EA.Attribute)repository.GetContextObject();

                        Util.UpdateAttribute(repository, a);
                    }

                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)repository.GetContextObject();

                        Util.UpdateMethod(repository, m);
                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        el = (EA.Element)repository.GetContextObject();
                        Util.UpdateClass(repository, el);
                    }
                    if (oType.Equals(EA.ObjectType.otPackage))
                    {
                        var pkg = (EA.Package)repository.GetContextObject();
                        Util.UpdatePackage(repository, pkg);
                    }
                    break;

                
                case MenuCreateActivityForOperation:
                    EaService.CreateActivityForOperation(repository);

                    break;

                    // get Parameter for Activity
                case MenuUpdateOperationParameter:
                    EaService.UpdateActivityParameter(repository);
                    break;

                case MenuUpdateActionPin:
                    if (oType.Equals(EA.ObjectType.otPackage))
                    {
                        var pkg = (EA.Package)repository.GetContextObject();
                        ActionPin.UpdateActionPinForPackage(repository, pkg);
                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        el = (EA.Element)repository.GetContextObject();
                        ActionPin.UpdateActionPinForElement(repository, el);
                    }
                    break;
                

                case MenuAddLinkedDiagramNote:
                    EaService.AddDiagramNote(repository); 
                               
                    break;

                case MenuAddLinkedNote:
                    EaService.AddElementsToDiagram(repository,"Note","NoteLink");

                    break;

                case MenuLocateType:
                    EaService.LocateType(repository);
                    
                    break;

                case MenuUsage:
                    EaService.FindUsage(repository);
                    
                    break;

                case MenuPasteLinksFromClipboard:
                    if (oType.Equals(EA.ObjectType.otElement)) // only Element
                    {
                        el = (EA.Element)repository.GetContextObject();
                        string conStr = Clipboard.GetText();  // get Clipboard
                        if (conStr.StartsWith("{", StringComparison.CurrentCulture) && conStr.Substring(37,1)=="}" && conStr.EndsWith("\r\n", StringComparison.CurrentCulture)) {
                            repository.CreateOutputTab("DEBUG");
                            repository.EnsureOutputVisible("DEBUG");
                            int countError = 0;
                            int countInserted = 0;
                            string[] conStrList = conStr.Split('\n');
                            foreach (string line in conStrList)
                            {
                                if (line.Length > 38)
                                {
                                    string guid = line.Substring(0, 38);
                                    EA.Connector con = repository.GetConnectorByGuid(guid);

                                    // Client/Source
                                    if (line.Substring(38, 1) == "C")
                                    {
                                        try
                                        {
                                            con.ClientID = el.ElementID;
                                            con.Update();
                                            countInserted = countInserted + 1;
                                        }
                                        catch
                                        {
                                            countError = countError + 1;
                                            EA.Element el1 = repository.GetElementByID(con.SupplierID);
                                            string fText = $"Error Name {el1.Name}, Error={con.GetLastError()}";
                                            repository.WriteOutput("Debug", fText, 0);
                                        }
                                    }
                                    //Supplier / Target
                                    if (line.Substring(38, 1) == "S")
                                    {
                                        try
                                        {

                                            con.SupplierID = el.ElementID;
                                            con.Update();
                                            countInserted = countInserted + 1;
                                        }
                                        catch
                                        {
                                            countError = countError + 1;
                                            EA.Element el1 = repository.GetElementByID(con.ClientID);
                                            string fText = $"Error Name {el1.Name}, Error={con.GetLastError()}";
                                            repository.WriteOutput("Debug",fText,0);
                                        }

                                    }
                                }
                            }
                            // update diagram
                            EA.Diagram dia = repository.GetCurrentDiagram();
                            if (dia != null)
                            {
                                try
                                {
                                    dia.Update();
                                    repository.ReloadDiagram(dia.DiagramID);
                                }
                                #pragma warning disable RECS0022
                                catch
#pragma warning restore RECS0022
                                {
                                    // ignored
                                }
                            }
                            MessageBox.Show($@"Copied:{countInserted}{Environment.NewLine}Errors:{countError}");
                        }

                        

                    }
                    break;

                case MenuCopyGuidToClipboard:
                   EaService.CopyGuidSqlToClipboard(repository);
                   break;


                // put on Clipboard
                // 'ConnectorGUID', 'Client' if element is a client/source in this dependency
                // 'ConnectorGUID', 'Supplier' if element is a supplier/target in this dependency

                case MenuCopyLinksToClipboard:
                    if (oType.Equals(EA.ObjectType.otElement)) // only Element
                    {
                        el = (EA.Element)repository.GetContextObject();
                        string conStr = "";
                        foreach (EA.Connector con in el.Connectors)
                        {
                            conStr = conStr + con.ConnectorGUID;
                            // check if client or supplier
                            if (con.ClientID == el.ElementID) conStr = conStr + "Client  \r\n";
                            if (con.SupplierID == el.ElementID) conStr = conStr + "Supplier\r\n";

                        }
                        if (String.IsNullOrWhiteSpace(conStr)) Clipboard.Clear();
                        else Clipboard.SetText(conStr);
                        
                    }
                    break;

                case MenuDisplayMethodDefinition:
                    EaService.DisplayOperationForSelectedElement(repository, EaService.DisplayMode.Method);
                    break;

                case MenuDisplayBehavior:
                    EaService.DisplayOperationForSelectedElement(repository, EaService.DisplayMode.Behavior);
                    break;

                // Test access via linq2db for
                // - Current EA connection
                // - MySQL
                // - SQLite
                // - Jet
                case MenuTestDbAccess:
                    //_myControlGui.TextBoxStatus.Text = $@"Linq DB access SQLite, JET, MySQL running";
                    string connectionString = "";
                    IDataProvider provider = null;
                    string providerName = "";
                    try
                    {
                        // get current EA connection 
                        connectionString = hoLinqToSql.LinqUtils.LinqUtil.GetConnectionString(_repository, out provider, out providerName);
                        var tests = new List<(string connectionString, string driverName, IDataProvider driver)>
                        {
                            (connectionString, providerName, provider),                                                                                         // current EA connection
                            ($@"server=localhost; database=ironman_16_01_2024; user=root; password=minden;", "MySql", null),                                    // MySQL
                            (@"Data Source=c:\Users\hoXps\AppData\Roaming\Sparx Systems\EA\EAExample.qea;Version=3;", "SQLite", null),                          // SQLite
                            (@"Data Source=c:\Users\hoXps\AppData\Roaming\Sparx Systems\EA\EAExample.qea;", "SQLite", null),                                    // SQLite without version
                            (providerName == "MySql"?connectionString:null, "MySqlConnector", null),                                                          // also test MySqlConnector
                            (@"Provider=Microsoft.JET.OLEDB.4.0; Data Source=c:\Users\hoXps\AppData\Roaming\Sparx Systems\EA\EAExample.eap;", "Access", null),  // Jet
                            ($@"", "", null)                                                                                                                    // free

                        };
                        foreach (var (connection, driverName, driver) in tests)
                        {
                            //_myControlGui.TextBoxStatus.Text = $@"Linq DB access {driverName ?? ""} {connectionString} running";
                            if (String.IsNullOrEmpty(connection)) continue;
                            var conOption = hoLinqToSql.LinqUtils.LinqUtil.GetLinq2DataOptions(driver, driverName, connection);
                            var dt = Load(conOption);
                            if (dt != null) ProtTable(repository, conOption, dt);

                        }

                        //_myControlGui.TextBoxStatus.Text = $@"Linq DB access SQLite, JET, MySQL finished";
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($@"ConnectionString: {connectionString}
Provider:      {provider?.Name ?? ""}
ProviderName:  {providerName}
{e}", @"Error linq2db access");
                    }

                    break;


            }
        }
        #endregion
        /// <summary>
        /// Protocol a DataTable to EA output
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="options"></param>
        /// <param name="dt"></param>
        private static void ProtTable(EA.Repository rep, DataOptions options, DataTable dt)
        {
            rep.CreateOutputTab("Test");
            rep.EnsureOutputVisible("Test");
            rep.WriteOutput("Test", "", 0);
            rep.WriteOutput("Test", $@"ConnectionString: '{options.ConnectionOptions.ConnectionString}'", 0);
            rep.WriteOutput("Test", $@"ProviderName:     '{options.ConnectionOptions.ProviderName}'", 0);
            var env = Environment.Is64BitProcess ? "x64" : "x86";
            rep.WriteOutput("Test", $@"Environment '{env}'", 0);
            rep.WriteOutput("Test", $"Count={dt.Rows.Count}", 0);
            //foreach (DataRow row in dt.Rows)
            //{
            //    rep.WriteOutput("Test", $@"{row.Field<string>("GUID")} {row.Field<string>("ColName")}", 0);
            //}
            rep.WriteOutput("Test", "", 0);
        }
        /// <summary>
        /// Load all components
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static DataTable Load(DataOptions options)
        {
            try
            {
                using (var db = new hoLinqToSql.DataModels.EaDataModel(options))
                {
                    try
                    {
                        var objects = (from o in db.t_object

                                       where o.Object_Type == "Component"
                                       select new { Guid = o.ea_guid, Name = o.Name }
                            )
                            .Distinct()
                            .ToArray();

                        return objects.ToDataTable();

                    }
                    catch (Exception e)
                    {
                        var env = Environment.Is64BitProcess ? "x64" : "x86";
                        MessageBox.Show($@"Exception:
ConnectionString: '{options?.ConnectionOptions.ConnectionString ?? " "}'
DataProviderName: '{options?.ConnectionOptions.ProviderName ?? " "}' 
Environment:      '{env}'
            
                    {e}", @"Error access t_object");
                        return null;

                    }
                }

            }
            catch (Exception e)
            {
                var env = Environment.Is64BitProcess ? "x64" : "x86";
                MessageBox.Show($@"Exception:
ConnectionString: '{options?.ConnectionOptions.ConnectionString ?? " "}'
DataProviderName: '{options?.ConnectionOptions.ProviderName ?? " "}' 
Environment:      '{env}'
            
                    {e}", @"Error access t_object");
                return null;

            }


        }



        #region ShowAddinControlWindows
        /// <summary>
        /// Show all AddinControl Addin Windows. If configured set the tab to display.
        /// </summary>
        private void ShowAddinControlWindows()
        {

            if (_myControlGui == null)
            {
                //-----------  hoTools main window -------------------------------------------------
                try
                {
                    // LineStyle and more
                    if (_addinSettings.LineStyleAndMoreWindow != AddinSettings.ShowInWindow.Disabled)
                    {
                        _HoToolsGui = AddAddinControl<HoToolsGui>(
                            _addinSettings.ProductName, // Tab Name
                            HoToolsGui.Progid, null,
                            AddinSettings.ShowInWindow.AddinWindow);
                        _myControlGui = _HoToolsGui; // static + instance
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($@"ProgID='{HoToolsGui.Progid}'{Environment.NewLine}{e.Message}", @"Error Starting hoTool GUI window.");
                }
                //-----------  hoSearchAndReplace -------------------------------------------------
                try
                {

                    if (_addinSettings.SearchAndReplaceWindow != AddinSettings.ShowInWindow.Disabled)
                    {
                        _FindAndReplaceGUI = AddAddinControl<FindAndReplaceGUI>(
                            FindAndReplaceGUI.Tabulator,
                            FindAndReplaceGUI.ProgId, null,
                            AddinSettings.ShowInWindow.AddinWindow);
                        _findAndReplaceGui = _FindAndReplaceGUI; // static + instance
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, @"Error Starting hoFindAndReplace window.");
                }
                //-----------  hoSql -------------------------------------------------
                try
                {
                    // with Query EA Addin Windows
                    if (_addinSettings.OnlyQueryWindow != AddinSettings.ShowInWindow.Disabled)
                    {
                        // Run as Query
                        _hoSqlGui = AddAddinControl<HoSqlGui>(HoSqlGui.TabulatorSql,
                            HoSqlGui.Progid, HoSqlGui.TabulatorSql,
                            _addinSettings.OnlyQueryWindow);
                        _HoSqlGui = _hoSqlGui; // static + instance
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, @"Error Starting SQL window.");
                }
                
                //-----------  hoScript -------------------------------------------------
                try { 
                    // with Script & Query EA Addin Windows
                    if (_addinSettings.ScriptAndQueryWindow != AddinSettings.ShowInWindow.Disabled)
                    {
                        // Run as Script
                        _ScriptGUI = AddAddinControl<HoSqlGui>(HoSqlGui.TabulatorScript, 
                            HoSqlGui.Progid, HoSqlGui.TabulatorScript, 
                            _addinSettings.ScriptAndQueryWindow);
                    _scriptGui = _ScriptGUI; // static + instance
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message,@"Error Starting hoScript window");
                }
                //-----------  hoExtension -------------------------------------------------
                try
                {
                    // with Extension EA Addin Windows
                    if (_addinSettings.ExtensionWindow != AddinSettings.ShowInWindow.Disabled)
                    {
                        // Run as Query
                        _extensionGui2 = AddAddinControl<ExtensionGui>(
                            ExtensionGui.Tabulator,
                            ExtensionGui.Progid, null,
                            AddinSettings.ShowInWindow.AddinWindow);
                        _extensionGui = _extensionGui2; // static + instance
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, @"Error Starting hoExtension window.");
                }

            }
        }
        /// <summary>
        /// Show the specified Addin Tab. If blank it shows nothing (default EA behavior).
        /// </summary>
        /// <param name="tabName"></param>
        private void ShowAddinTab(string tabName)
        {
            if (String.IsNullOrWhiteSpace(tabName)) return;
            try
            {
                // Activate the correct tab by sending the tab Name of the Tab
                _repository.ShowAddinWindow("SQL");
                _repository.ShowAddinWindow(tabName);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Tab name={tabName}{Environment.NewLine}{e}", @"Can't activate Addin Tab 'SQL'");
            }
        }


        /// <summary>
        /// Add AddinGUI as a tab to EA. It sets the following properties in the following sequence: Tag, _AddinSettings, Release, rep. 
        /// </summary>
        /// <param name="tabName">Tabulator name to show Addin</param>
        /// <param name="progId">ProgID under which the Addin is registered</param>
        /// <param name="tag">Information to pass to Control</param>
        /// <param name="showInWindowType"></param>
        /// <returns>AddinGUI</returns>
        private T AddAddinControl<T>(string tabName, string progId, object tag, AddinSettings.ShowInWindow showInWindowType)
        {
            T c;
            switch (showInWindowType) {
                case AddinSettings.ShowInWindow.AddinWindow:
                    c = (T)_repository.AddWindow(tabName, progId);
                    break;
                case AddinSettings.ShowInWindow.TabWindow:
                    c = (T)_repository.AddTab(tabName, progId);
                    break;
            default:
                    return default(T);
            }
            AddinGui control = c as AddinGui;
            if (null == control)
            {
                MessageBox.Show($@"Unable to start ProgId='{progId}', tab='{tabName}'");
            }
            else
            {
                control.Tag = tag;
                control.AddinSettings = _addinSettings;
                control.Release = "V" + _release;
                control.Repository = _repository;



            }
            return c;
        }
        #endregion
        /// <summary>
        /// Write EA-Context information to Clipboard and EA output Tab "Debug"
        /// - GUID
        /// - ID
        /// - Type 
        /// </summary>
        /// <returns></returns>
        public string WriteInfoContext(EA.Repository rep)
        {

            if (_repository == null) return "";
            var contrEaInfo = new EaInfo(rep);
            contrEaInfo.WriteInfoContext();
            return "";

        }



    }
}
