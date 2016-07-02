using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using hoTools.Utils.Favorites;
using hoTools.ActiveX;
using hoTools.Settings;
using hoTools.Query;

using hoTools.EaServices;
using hoTools.Utils;
using hoTools.Utils.Appls;
using hoTools.Utils.ActionPins;
using System.Reflection;

using hoTools.Find;
using hoTools.Utils.Configuration;

using GlobalHotkeys;

#region description
//---------------------------------------------------------------
//hoTools
//---------------------------------------------------------------
//Tools developed by Helmut Ortmann
//---------------------------------------------------------------
//Configuration:
//hoTools:    ..\Setup\ActiveXdll.config.xml 
//VAR1:        ..\SetpVAR1\ActiveXdll.config.xml 
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
//4. Copy the correct ActiveXdll.config.xml into the ..\AddinClass\src\.. !!
//5. Possibly delete old config file (%APPDATA%ho\hoTools or %APPDATA%ho\hoTools_VAR1)
//---------------------------------------------------------------
#endregion

namespace hoTools
{
    public class AddinClass : EAAddinFramework.EAAddinBase
    {
        // Overwritten by AdinClass AssemblyFileVersion
        // This should be identical to installed product version from WIX installer (ProductVersion)
        string _release = "X.X.XXX.XX"; // Major, Minor, Build, free,


        // static due to global key definitions
        // ReSharper disable once InconsistentNaming
        static EA.Repository _Repository ;
        // ReSharper disable once InconsistentNaming
        static AddinSettings _AddinSettings;
        // ReSharper disable once InconsistentNaming
        static AddinControlGui _AddinControlGui;
        // ReSharper disable once InconsistentNaming
        static FindAndReplaceGUI _FindAndReplaceGUI;
        // ReSharper disable once InconsistentNaming
        static QueryGui _ScriptGUI;
        // ReSharper disable once InconsistentNaming
        static QueryGui _QueryGUI;

        // ActiveX Controls
        AddinControlGui _myControlGui;
        FindAndReplaceGUI _findAndReplaceGui;
        QueryGui _scriptGui;
        QueryGui _queryGui;

        // settings
        readonly AddinSettings _addinSettings;
 

        EA.Repository _repository;
        // define menu constants
        const string MenuName = "-hoTools";

        const string MenuShowWindow = "Show Window";
        const string MenuChangeXmlFile = "Change *.xml file for a version controlled package";

        const string MenuDisplayBehavior = "Display Behavior";
        const string MenuDisplayMethodDefinition = "Locate Operation";
        const string MenuLocateType = "Locate Type";
        const string MenuLocateCompositeElementorDiagram = "Locate CompositeElementOfDiagram";
        const string MenuLocateCompositeDiagramOfElement = "Locate CompositeDiagramOfElement";
        const string MenuShowSpecification = "Show Specification";
        const string MenuUnlockDiagram = "UnlockDiagram";

        const string MenuDeviderLineStyleDia = "---------------Line style Diagram-----------------";
        const string MenuLineStyleDiaLh = "Line Style Diagram (Object): Lateral Horizontal";
        const string MenuLineStyleDiaLv = "Line Style Diagram (Object): Lateral Vertical";
        const string MenuLineStyleDiaTh = "Line Style Diagram (Object): Tree Horizontal";
        const string MenuLineStyleDiaTv = "Line Style Diagram (Object): Tree Vertical";
        const string MenuLineStyleDiaOs = "Line Style Diagram (Object): Orthogonal Square";

        const string MenuDeviderActivity = "-------------Create Activity for Operation ---------------------------";
        const string MenuCreateActivityForOperation = "Create Activity for Operation (select operation or class)";
        const string MenuUpdateOperationParameter = "Update Operation Parameter for Activity (select Package(recursive), Activity, Class, Interface or Operation)";
        const string MenuUpdateActionPin = "Update Action Pin for Package (recursive)";

        const string MenuDeviderInteraction = "-------------Create Interaction for Operation ---------------------------";
        const string MenuCreateInteractionForOperation = "&Create Interaction for Operation (select operation or class)  ";

        const string MenuDeviderStateMachine = "-------------Create State Machine for Operation ---------------------------";
        const string MenuCreateStateMachineForOperation = "&Create State Machine for Operation (select operation)  ";


        const string MenuCorrectTypes = "-------------Correct Type ---------------------------";
        const string MenuCorrectType = "Correct types of Attribute, Function (selected Attribute, Function, Class or Package)";

        const string MenuDeviderCopyPast = "-------------Move links---------------------------"; 
        const string MenuCopyGuidToClipboard = "Copy GUID / Select Statement to Clipboard";
        const string MenuCopyLinksToClipboard = "Copy Links to Clipboard";
        const string MenuPasteLinksFromClipboard = "Paste Links from Clipboard";

        const string MenuDeviderNote = "-------------Note      ---------------------------"; 
        const string MenuAddLinkedNote = "Add linked Note";
        const string MenuAddLinkedDiagramNote = "Add linked Diagram Note";

        const string MenuUsage = "Find Usage";
        const string MenuAbout = "About + Help";

        const string MenuDevider = "-----------------------------------------------"; 


       
        #region Constructor
        /// <summary>
        /// Constructor: Reade settings, set the menu header and menuOptions
        /// </summary>
        public AddinClass()
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

               

            }
            catch (Exception e)
            {
                MessageBox.Show($"Error setup 'hoTools' Addin. Error:\n\n{e}", @"hoTools Installation error");
            }
            // global configuration parameters independent from EA-Instance and used by services
            var globalCfg = HoToolsGlobalCfg.Instance;
            globalCfg.SetSqlPaths(_addinSettings.SqlPaths);

            this.MenuHeader = "-" + _addinSettings.ProductName;
            this.menuOptions = new string[] { 
                //-------------------------- General  --------------------------//
                //    menuLocateCompositeElementorDiagram,
                //-------------------------- LineStyle --------------------------//
                                        
                //-------------------------- Activity --------------------------//
                MenuDeviderActivity,
                MenuCreateActivityForOperation, MenuUpdateOperationParameter, 
                //menuUpdateActionPin,
                //-------------------------- Interaction --------------------------//
                MenuDeviderInteraction,
                MenuCreateInteractionForOperation,
                //-------------------------- Interaction --------------------------//
                MenuDeviderStateMachine,
                MenuCreateStateMachineForOperation,
                //-------------------------- Correct Types ------------------------//
                //menuCorrectTypes, menuCorrectType, 
                //-------------------------- Add Note -----------------------------//
                MenuDeviderNote,
                MenuAddLinkedNote,MenuAddLinkedDiagramNote,

                //-------------------------- Move links ---------------------------//
                MenuDeviderCopyPast,
                MenuChangeXmlFile,MenuCopyLinksToClipboard, MenuPasteLinksFromClipboard, 

                MenuShowWindow,    
                //---------------------------- About -------------------------------//
                MenuDevider, MenuAbout };
        }
        #endregion

        #region HotkeyHandlers
        /// <summary>
        /// Handle Hotkeys
        /// </summary>
        internal static class HotkeyHandlers
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

                for (int i = 0; i < _AddinSettings.GlobalShortcutsService.Count; i = i + 1)
                {
                    GlobalKeysConfig.GlobalKeysSearchConfig search = _AddinSettings.GlobalShortcutsSearch[i];
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
                for (int i = 0; i < _AddinSettings.GlobalShortcutsService.Count; i = i + 1)
                {
                    GlobalKeysConfig.GlobalKeysServiceConfig service = _AddinSettings.GlobalShortcutsService[i];
                    if (service.Key != "None" & service.Guid != "")
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
            public static void RunGlobalKeySearch(int pos)
            {
                
                    GlobalKeysConfig.GlobalKeysSearchConfig sh = _AddinSettings.GlobalShortcutsSearch[pos];
                    if (sh.SearchName == "") return;
                    try
                    {
                        _Repository.RunModelSearch(sh.SearchName, sh.SearchTerm, "", "");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Error start search '" + sh.SearchName +
                           " " + sh.SearchTerm + "'");
                    }
            }
            public static void RunGlobalKeyService(int pos)
            {
                GlobalKeysConfig.GlobalKeysServiceConfig sh = _AddinSettings.GlobalShortcutsService[pos];
                    if (sh.Method == null) return;
                    sh.Invoke(_Repository, _AddinControlGui.GetText());
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
        /// <param name="Repository">the EA.rep</param>
        /// <param name="GUID">the guid of the selected item</param>
        /// <param name="ot">the object type of the selected item</param>
        public override void EA_OnContextItemChanged(EA.Repository Repository, string GUID, EA.ObjectType ot)
        { }
        #endregion
        #region EA_OnOutputItemDoubleClicked
        public override void EA_OnOutputItemDoubleClicked(EA.Repository Repository, string TabName, string LineText, long ID)
        {

        }
        #endregion
        #region EA_OnPostInitialized
        public override void EA_OnPostInitialized(EA.Repository Repository)
        {
            // gets the file 'AssemblyFileVersion' of file AddinClass.dll
            string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            _release = productVersion;
            AddinClass._Repository = Repository;
            _repository = Repository;
            ShowAddinControlWindows();
        }
        #endregion
        #region EA_OnPostNewConnector
        public override bool EA_OnPostNewConnector(EA.Repository rep, EA.EventProperties info)
        {
            EA.EventProperty eventProperty = info.Get(0);
            var s = (string)eventProperty.Value;

            // check if it is a diagram
            int connectorId;
            if (Int32.TryParse(s, out connectorId) == false)
            {
                return false;
            }


            
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return false; // e.g. Matrix has a diagramm id but no diagram object
            switch (dia.Type)
            {
                case "Activity":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.ActivityLineStyle.Substring(0, 2));


                case "Statechart":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.StatechartLineStyle.Substring(0, 2));

                case "Logical":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.ClassLineStyle.Substring(0, 2));


                case "Custom":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.CustomLineStyle.Substring(0, 2));

                case "Component":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.ComponentLineStyle.Substring(0, 2));

                case "Deployment":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.DeploymentLineStyle.Substring(0, 2));

                case "Package":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.PackageLineStyle.Substring(0, 2));

                case "Use Case":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.UseCaseLineStyle.Substring(0, 2));

                case "CompositeStructure":
                    return UpdateLineStyle(rep, dia, connectorId, _addinSettings.CompositeStructureLineStyle.Substring(0, 2));

                default:
                    return false;



            }
        }
        #endregion
        #endregion
        #region EA_ Connect File Open/Close
        #region EA_Connect
        /// <summary>
        /// EA_Connect events enable Add-Ins to identify their type and to respond to Enterprise Architect start up.
        /// This event occurs when Enterprise Architect first loads your Add-In. Enterprise Architect itself is loading at this time so that while a rep object is supplied, there is limited information that you can extract from it.
        /// The chief uses for EA_Connect are in initializing global Add-In data and for identifying the Add-In as an MDG Add-In.
        /// Also look at EA_Disconnect.
        /// </summary>
        /// <param name="Repository">An EA.rep object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <returns>String identifying a specialized type of Add-In: 
        /// - "MDG" : MDG Add-Ins receive MDG Events and extra menu options.
        /// - "" : None-specialized Add-In.</returns>
        public override string EA_Connect(EA.Repository Repository)
        {
            // register only if configured
            if (_AddinSettings.IsShortKeySupport) HotkeyHandlers.SetupGlobalHotkeys();
            _repository = Repository;
            if (Repository.IsSecurityEnabled)
            {
                //logInUser = rep.GetCurrentLoginUser(false);
                //if ((logInUser.Contains("ho")) ||
                //     (logInUser.Contains("admin")) ||
                //     (logInUser.Equals(""))
                //    ) logInUserRights = UserRights.ADMIN;
            }
            Favorite.InstallSearches(_repository); // install searches
            return "a string";
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
            if (_myControlGui != null)  _myControlGui.Save(); // save settings
            InitializeForRepository(null);


        }
        #endregion
        #region EA Validation
        /// <summary>
        /// Called when EA start model validation. Just shows a message box
        /// </summary>
        /// <param name="Repository">the rep</param>
        /// <param name="Args">the arguments</param>
        // ReSharper disable once InconsistentNaming
        public override void EA_OnStartValidation(EA.Repository Repository, object Args)
        {
            MessageBox.Show(@"Validation started");
        }
        /// <summary>
        /// Called when EA ends model validation. Just shows a message box
        /// </summary>
        /// <param name="Repository">the rep</param>
        /// <param name="Args">the arguments</param>
        public override void EA_OnEndValidation(EA.Repository Repository, object Args)
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
           

            string fileNameMdg;
            switch (_AddinSettings.AutoLoadMdgXml)
            {
                case AddinSettings.AutoLoadMdg.Basic:
                    fileNameMdg = "hoToolsBasic.xml";
                    break;
                case AddinSettings.AutoLoadMdg.Compilation:
                    fileNameMdg = "hoToolsCompilation.xml";
                    break;
                default:
                    fileNameMdg = "";
                    break;
            }
            if (fileNameMdg == "") return "";

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string combinedPathMdg = Path.Combine(assemblyFolder, fileNameMdg);

            try
            {
                return File.ReadAllText(combinedPathMdg);
            } catch (Exception e)
            {
                MessageBox.Show($"MDG file='{combinedPathMdg}'\r\n{e}", $"Can't load MDG '{fileNameMdg}'");
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
            _repository = rep;
            _Repository = rep;
            try
            {
                if (_myControlGui != null) _myControlGui.Repository = rep;
                if (_findAndReplaceGui != null) _findAndReplaceGui.Repository = rep;
                if (_scriptGui != null) _scriptGui.Repository = rep;
                if (_queryGui != null) _queryGui.Repository = rep;
            } catch (Exception e)
            {
                MessageBox.Show($"{e.Message}",@"hoTools: Error initializing Addin Tabs");
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
        /// <param name="MenuLocation">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <param name="ItemName">the name of the menu item</param>
        /// <param name="IsEnabled">boolean indicating whether the menu item is enabled</param>
        /// <param name="IsChecked">boolean indicating whether the menu is checked</param>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once ParameterHidesMember
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once InconsistentNaming
        public override void EA_GetMenuState(EA.Repository Repository, string MenuLocation, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            if (IsProjectOpen(Repository))
            {
                switch (ItemName)
                {
                    case MenuChangeXmlFile:
                        IsChecked = false;
                        break;

                    case MenuShowWindow:
                        IsChecked = false;
                        break;
                    case  MenuShowSpecification:
                        IsChecked = false;
                        break;

                    case MenuUnlockDiagram:
                        IsChecked = false;
                        break;

                    case MenuLineStyleDiaTh:
                        IsChecked = false;
                        break;
                    case MenuLineStyleDiaTv:
                        IsChecked = false;
                        break;
                    case MenuLineStyleDiaLh:
                        IsChecked = false;
                        break;
                    case MenuLineStyleDiaLv:
                        IsChecked = false;
                        break;
                    case MenuLineStyleDiaOs:
                        IsChecked = false;
                        break;

                    case MenuLocateCompositeElementorDiagram:
                        IsChecked = false;
                        break;
                    
                    case MenuLocateCompositeDiagramOfElement:
                        IsChecked = false;
                        break;
                        

                case MenuUsage:
                        IsChecked = false;
                        break;    
                       
                case MenuCreateInteractionForOperation:
                        IsChecked = false;
                        break;

                case MenuCreateStateMachineForOperation:
                        IsChecked = false;
                        break;  
                    
                case MenuCorrectType:
                        IsChecked = false;
                        break;

               case MenuDisplayBehavior:
                        IsChecked = false;
                        break;


               case MenuUpdateActionPin:
                        IsChecked = false;
                        break;

                    case MenuUpdateOperationParameter:
                        IsChecked = false;
                        break;

                    case MenuCreateActivityForOperation:
                        IsChecked = false;
                        break;

                    case MenuDisplayMethodDefinition:
                        IsChecked = false;
                        break;

                    
                    case MenuLocateType:
                        IsChecked = false;
                        break;


                    case MenuCopyGuidToClipboard:
                        IsChecked = false;
                        break;

                    case MenuCopyLinksToClipboard:
                        IsChecked = false;
                        break;

                    case MenuPasteLinksFromClipboard:
                        IsChecked = false;
                        break;

                    
                    case MenuAddLinkedNote:
                        IsChecked = false;
                        break;

                    case MenuAddLinkedDiagramNote:
                        IsChecked = false;
                        break;

                    case MenuAbout:
                        IsChecked = false;
                        break;

                    // there shouldn't be any other, but just in case disable it.
                    default:
                        IsEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                IsEnabled = false;
            }
        }
        #endregion

        #region EA_MenuClick
        /// <summary>
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        /// </summary>
        /// <param name="Repository">the rep</param>
        /// <param name="MenuLocation">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <param name="ItemName">the name of the selected menu item</param>
        public override void EA_MenuClick(EA.Repository Repository, string MenuLocation, string MenuName, string ItemName)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            EA.Diagram diaCurrent = Repository.GetCurrentDiagram();
            EA.Element el = null;

            if (diaCurrent != null) 
            {
            }
            switch (ItemName)
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
                     EaService.SetNewXmlPath(Repository);
                     break;
                // Line style: Lateral Horizontal 
                case MenuLineStyleDiaLh:
                     EaService.SetLineStyle(Repository, "LH");
                   
                    break;
                // Line style: Lateral Vertical 
                case MenuLineStyleDiaLv:
                    // all connections of diagram
                    EaService.SetLineStyle(Repository, "LV");
                    break;
                // Line style: Tree Vertical 
                case MenuLineStyleDiaTv:
                    EaService.SetLineStyle(Repository, "V");
                    
                    break;

                // Line style: Tree Horizontal 
                case MenuLineStyleDiaTh:
                    EaService.SetLineStyle(Repository, "H");
                    
                    break;
                // Line style: Orthogonal square 
                case MenuLineStyleDiaOs:
                    EaService.SetLineStyle(Repository, "OS");
                    
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
                            Repository.Execute(sql);
                            // reload view
                            Repository.ReloadDiagram(diaCurrent.DiagramID);
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
                      EaService.ShowSpecification(Repository);

                    break;

                // Create Interaction for selected operation or class (all operations)
                case   MenuCreateInteractionForOperation:
                    // Check selected Elements in tree
                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)Repository.GetContextObject();
                        // test multiple selection

                        // Create Activity
                        Appl.CreateInteractionForOperation(Repository, m);

                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        var cls = (EA.Element)Repository.GetContextObject();
                        // over all operations of class
                        foreach (EA.Method m in cls.Methods)
                        {
                            // Create Activity
                            Appl.CreateInteractionForOperation(Repository, m);

                        }
                    }

                    break;

                // Create Interaction for selected operation or class (all operations)
                case MenuCreateStateMachineForOperation:
                    // Check selected Elements in tree
                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)Repository.GetContextObject();
                        // test multiple selection

                        // Create State Machine
                        Appl.CreateStateMachineForOperation(Repository, m);

                    }
                   break;



                case MenuLocateCompositeElementorDiagram:
                   EaService.NavigateComposite(Repository);
                    break;
                    
                // 
                case MenuCorrectType:
                    if (oType.Equals(EA.ObjectType.otAttribute))
                    {
                        var a = (EA.Attribute)Repository.GetContextObject();

                        Util.UpdateAttribute(Repository, a);
                    }

                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)Repository.GetContextObject();

                        Util.UpdateMethod(Repository, m);
                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        el = (EA.Element)Repository.GetContextObject();
                        Util.UpdateClass(Repository, el);
                    }
                    if (oType.Equals(EA.ObjectType.otPackage))
                    {
                        var pkg = (EA.Package)Repository.GetContextObject();
                        Util.UpdatePackage(Repository, pkg);
                    }
                    break;

                
                case MenuCreateActivityForOperation:
                    EaService.CreateActivityForOperation(Repository);

                    break;

                    // get Parameter for Activity
                case MenuUpdateOperationParameter:
                    EaService.UpdateActivityParameter(Repository);
                    break;

                case MenuUpdateActionPin:
                    if (oType.Equals(EA.ObjectType.otPackage))
                    {
                        var pkg = (EA.Package)Repository.GetContextObject();
                        ActionPin.UpdateActionPinForPackage(Repository, pkg);
                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        el = (EA.Element)Repository.GetContextObject();
                        ActionPin.UpdateActionPinForElement(Repository, el);
                    }
                    break;
                

                case MenuAddLinkedDiagramNote:
                    EaService.AddDiagramNote(Repository); 
                               
                    break;

                case MenuAddLinkedNote:
                    EaService.AddElementNote(Repository);

                    break;

                case MenuLocateType:
                    EaService.LocateType(Repository);
                    
                    break;

                case MenuUsage:
                    EaService.FindUsage(Repository);
                    
                    break;

                case MenuPasteLinksFromClipboard:
                    if (oType.Equals(EA.ObjectType.otElement)) // only Element
                    {
                        el = (EA.Element)Repository.GetContextObject();
                        string conStr = Clipboard.GetText();  // get Clipboard
                        if (conStr.StartsWith("{", StringComparison.CurrentCulture) && conStr.Substring(37,1)=="}" && conStr.EndsWith("\r\n", StringComparison.CurrentCulture)) {
                            Repository.CreateOutputTab("DEBUG");
                            Repository.EnsureOutputVisible("DEBUG");
                            int countError = 0;
                            int countInserted = 0;
                            string[] conStrList = conStr.Split('\n');
                            foreach (string line in conStrList)
                            {
                                if (line.Length > 38)
                                {
                                    string guid = line.Substring(0, 38);
                                    EA.Connector con = Repository.GetConnectorByGuid(guid);

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
                                            EA.Element el1 = Repository.GetElementByID(con.SupplierID);
                                            string fText = $"Error Name {el1.Name}, Error={con.GetLastError()}";
                                            Repository.WriteOutput("Debug", fText, 0);
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
                                            EA.Element el1 = Repository.GetElementByID(con.ClientID);
                                            string fText = $"Error Name {el1.Name}, Error={con.GetLastError()}";
                                            Repository.WriteOutput("Debug",fText,0);
                                        }

                                    }
                                }
                            }
                            // update diagram
                            EA.Diagram dia = Repository.GetCurrentDiagram();
                            if (dia != null)
                            {
                                try
                                {
                                    dia.Update();
                                    Repository.ReloadDiagram(dia.DiagramID);
                                }
                                #pragma warning disable RECS0022
                                catch
#pragma warning restore RECS0022
                                {
                                    // ignored
                                }
                            }
                            MessageBox.Show($"Copied:{countInserted}\r\nErrors:{countError}");
                        }

                        

                    }
                    break;

                case MenuCopyGuidToClipboard:
                   EaService.CopyGuidSqlToClipboard(Repository);
                   break;


                // put on Clipboard
                // 'ConnectorGUID', 'Client' if element is a client/source in this dependency
                // 'ConnectorGUID', 'Supplier' if element is a supplier/target in this dependency

                case MenuCopyLinksToClipboard:
                    if (oType.Equals(EA.ObjectType.otElement)) // only Element
                    {
                        el = (EA.Element)Repository.GetContextObject();
                        string conStr = "";
                        foreach (EA.Connector con in el.Connectors)
                        {
                            conStr = conStr + con.ConnectorGUID;
                            // check if client or supplier
                            if (con.ClientID == el.ElementID) conStr = conStr + "Client  \r\n";
                            if (con.SupplierID == el.ElementID) conStr = conStr + "Supplier\r\n";

                        }
                        if (conStr.Length > 0)
                        {
                            Clipboard.SetText(conStr);
                        }
                    }
                    break;

                case MenuDisplayMethodDefinition:
                    EaService.DisplayOperationForSelectedElement(Repository, EaService.DisplayMode.Method);
                    break;

                case MenuDisplayBehavior:
                    EaService.DisplayOperationForSelectedElement(Repository, EaService.DisplayMode.Behavior);
                    break;

                
            }
        }
        #endregion



        #region ShowAddinControlWindows
        /// <summary>
        /// Show all AddinControl Addin Windows
        /// </summary>
        private void ShowAddinControlWindows()
        {
            if (_myControlGui == null)
            {

                try
                {
                    // LineStyle and more
                    if (!(_addinSettings.LineStyleAndMoreWindow == AddinSettings.ShowInWindow.Disabled))
                    {
                    _AddinControlGui = AddAddinControl<AddinControlGui>(_addinSettings.ProductName,
                        AddinControlGui.Progid, null,
                        AddinSettings.ShowInWindow.AddinWindow);
                        _myControlGui = _AddinControlGui; // static + instance
                    }

                    // with Search & Replace EA Addin Windows
                    if (!  (_addinSettings.SearchAndReplaceWindow == AddinSettings.ShowInWindow.Disabled) ) { 
                        _FindAndReplaceGUI = AddAddinControl<FindAndReplaceGUI>(FindAndReplaceGUI.TABULATOR, 
                            FindAndReplaceGUI.PROGID, null, 
                            AddinSettings.ShowInWindow.AddinWindow);
                       _findAndReplaceGui = _FindAndReplaceGUI; // static + instance
                    }

                    // with Query EA Addin Windows
                    if (! (_addinSettings.OnlyQueryWindow == AddinSettings.ShowInWindow.Disabled) )
                    {
                        // Run as Query
                        _QueryGUI = AddAddinControl<QueryGui>(QueryGui.TabulatorSql, 
                            QueryGui.Progid, QueryGui.TabulatorSql, 
                            _addinSettings.OnlyQueryWindow);
                        _queryGui = _QueryGUI; // static + instance
                    }

                    // with Script & Query EA Addin Windows
                    if (!(_addinSettings.ScriptAndQueryWindow == AddinSettings.ShowInWindow.Disabled))
                    {
                        // Run as Script
                        _ScriptGUI = AddAddinControl<QueryGui>(QueryGui.TabulatorScript, 
                            QueryGui.Progid, QueryGui.TabulatorScript, 
                            _addinSettings.ScriptAndQueryWindow);
                    _scriptGui = _ScriptGUI; // static + instance
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                    
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
            AddinGUI control = c as AddinGUI;
            if (null == control)
            {
                MessageBox.Show($"Unable to start progId='{progId}', tab='{tabName}'");
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

    

    }
}
