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
        string release = "X.X.XXX.XX"; // Major, Minor, Build, free,
                                       
        // static due to global key definitions
        static EA.Repository Repository ;
        static AddinSettings AddinSettings;
        static AddinControlGUI AddinControlGUI;
        static FindAndReplaceGUI FindAndReplaceGUI;
        static QueryGUI ScriptGUI;
        static QueryGUI QueryGUI;

        // ActiveX Controls
        AddinControlGUI _MyControlGUI;
        FindAndReplaceGUI _FindAndReplaceGUI;
        QueryGUI _ScriptGUI;
        QueryGUI _QueryGUI;

        // settings
        AddinSettings _AddinSettings;
 

        EA.Repository _repository;
        // define menu constants
        const string menuName = "-hoTools";

        const string menuShowWindow = "Show Window";
        const string menuChangeXmlFile = "Change *.xml file for a version controlled package";

        const string menuDisplayBehavior = "Display Behavior";
        const string menuDisplayMethodDefinition = "Locate Operation";
        const string menuLocateType = "Locate Type";
        const string menuLocateCompositeElementorDiagram = "Locate CompositeElementOfDiagram";
        const string menuLocateCompositeDiagramOfElement = "Locate CompositeDiagramOfElement";
        const string menuShowSpecification = "Show Specification";
        const string menuUnlockDiagram = "UnlockDiagram";

        const string menuDeviderLineStyleDia = "---------------Line style Diagram-----------------";
        const string menuLineStyleDiaLH = "Line Style Diagram (Object): Lateral Horizontal";
        const string menuLineStyleDiaLV = "Line Style Diagram (Object): Lateral Vertical";
        const string menuLineStyleDiaTH = "Line Style Diagram (Object): Tree Horizontal";
        const string menuLineStyleDiaTV = "Line Style Diagram (Object): Tree Vertical";
        const string menuLineStyleDiaOS = "Line Style Diagram (Object): Orthogonal Square";

        const string menuDeviderActivity = "-------------Create Activity for Operation ---------------------------";
        const string menuCreateActivityForOperation = "Create Activity for Operation (select operation or class)";
        const string menuUpdateOperationParameter = "Update Operation Parameter for Activity (select Package(recursive), Activity, Class, Interface or Operation)";
        const string menuUpdateActionPin = "Update Action Pin for Package (recursive)";

        const string menuDeviderInteraction = "-------------Create Interaction for Operation ---------------------------";
        const string menuCreateInteractionForOperation = "&Create Interaction for Operation (select operation or class)  ";

        const string menuDeviderStateMachine = "-------------Create State Machine for Operation ---------------------------";
        const string menuCreateStateMachineForOperation = "&Create State Machine for Operation (select operation)  ";


        const string menuCorrectTypes = "-------------Correct Type ---------------------------";
        const string menuCorrectType = "Correct types of Attribute, Function (selected Attribute, Function, Class or Package)";

        const string menuDeviderCopyPast = "-------------Move links---------------------------"; 
        const string menuCopyGUIDToClipboard = "Copy GUID / Select Statement to Clipboard";
        const string menuCopyLinksToClipboard = "Copy Links to Clipboard";
        const string menuPasteLinksFromClipboard = "Paste Links from Clipboard";

        const string menuDeviderNote = "-------------Note      ---------------------------"; 
        const string menuAddLinkedNote = "Add linked Note";
        const string menuAddLinkedDiagramNote = "Add linked Diagram Note";

        const string menuUsage = "Find Usage";
        const string menuAbout = "About + Help";

        const string menuDevider = "-----------------------------------------------"; 


        public enum displayMode
        {
            Behavior,
            Method
        }

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

                _AddinSettings = new AddinSettings();
                AddinSettings = _AddinSettings; // static
            }
            catch (Exception e)
            {
                MessageBox.Show("Error setup 'hoTools' Addin. Error:\n" + e, "hoTools Installation error");
            }
            this.menuHeader = "-" + _AddinSettings.productName;
            this.menuOptions = new string[] { 
                //-------------------------- General  --------------------------//
                //    menuLocateCompositeElementorDiagram,
                //-------------------------- LineStyle --------------------------//
                                        
                //-------------------------- Activity --------------------------//
                menuDeviderActivity,
                menuCreateActivityForOperation, menuUpdateOperationParameter, 
                //menuUpdateActionPin,
                //-------------------------- Interaction --------------------------//
                menuDeviderInteraction,
                menuCreateInteractionForOperation,
                //-------------------------- Interaction --------------------------//
                menuDeviderStateMachine,
                menuCreateStateMachineForOperation,
                //-------------------------- Correct Types ------------------------//
                //menuCorrectTypes, menuCorrectType, 
                //-------------------------- Add Note -----------------------------//
                menuDeviderNote,
                menuAddLinkedNote,menuAddLinkedDiagramNote,

                //-------------------------- Move links ---------------------------//
                menuDeviderCopyPast,
                menuChangeXmlFile,menuCopyLinksToClipboard, menuPasteLinksFromClipboard, 

                menuShowWindow,    
                //---------------------------- About -------------------------------//
                menuDevider, menuAbout };
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

                Dictionary<string, Keys> keys = GlobalKeysConfig.getKeys();
                Dictionary<string, Modifiers> modifiers = GlobalKeysConfig.getModifiers();
                Keys key;
                Modifiers modifier1;
                Modifiers modifier2;
                Modifiers modifier3;
                Modifiers modifier4;

                for (int i = 0; i < AddinSettings.globalShortcutsService.Count; i = i + 1)
                {
                    GlobalKeysConfig.GlobalKeysSearchConfig search = AddinSettings.globalShortcutsSearch[i];
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
                for (int i = 0; i < AddinSettings.globalShortcutsService.Count; i = i + 1)
                {
                    GlobalKeysConfig.GlobalKeysServiceConfig service = AddinSettings.globalShortcutsService[i];
                    if (service.Key != "None" & service.GUID != "")
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
            public static void runGlobalKeySearch(int pos)
            {
                
                    GlobalKeysConfig.GlobalKeysSearchConfig sh = AddinSettings.globalShortcutsSearch[pos];
                    if (sh.SearchName == "") return;
                    try
                    {
                        Repository.RunModelSearch(sh.SearchName, sh.SearchTerm, "", "");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Error start search '" + sh.SearchName +
                           " " + sh.SearchTerm + "'");
                    }
            }
            public static void runGlobalKeyService(int pos)
            {
                GlobalKeysConfig.GlobalKeysServiceConfig sh = AddinSettings.globalShortcutsService[pos];
                    if (sh.Method == null) return;
                    sh.Invoke(Repository, AddinControlGUI.getText());
            }

            private static void HandleGlobalKeySearch0()
            {
                runGlobalKeySearch(0); 
            }
            private static void HandleGlobalKeySearch1()
            {
                runGlobalKeySearch(1);
            }
            private static void HandleGlobalKeySearch2()
            {
                runGlobalKeySearch(2);
            }
            private static void HandleGlobalKeySearch3()
            {
                runGlobalKeySearch(3);
            }
            private static void HandleGlobalKeySearch4()
            {
                runGlobalKeySearch(4);
            }
            private static void HandleGlobalKeyService0()
            {
                runGlobalKeyService(0);
            }
            private static void HandleGlobalKeyService1()
            {
                runGlobalKeyService(1);
            }
            private static void HandleGlobalKeyService2()
            {
                runGlobalKeyService(2);
            }
            private static void HandleGlobalKeyService3()
            {
                runGlobalKeyService(3);
            }
            private static void HandleGlobalKeyService4()
            {
                runGlobalKeyService(4);
            }
                      
        }
        #endregion


        #region EA_On
        #region EA_OnContextItemChanged
        /// <summary>
        /// called when the selected item changes
        /// This operation will show the guid of the selected element in the eaControl
        /// </summary>
        /// <param name="Repository">the EA.Repository</param>
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
            release = productVersion;
            AddinClass.Repository = Repository;
            _repository = Repository;
            ShowAddinControlWindows();
        }
        #endregion
        #region EA_OnPostNewConnector
        public override bool EA_OnPostNewConnector(EA.Repository Repository, EA.EventProperties Info)
        {
            EA.EventProperty eventProperty = Info.Get(0);
            var s = (string)eventProperty.Value;
            int connectorID = int.Parse(s);
            EA.Diagram dia = Repository.GetCurrentDiagram();
            switch (dia.Type)
            {
                case "Activity":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.ActivityLineStyle.Substring(0, 2));


                case "Statechart":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.StatechartLineStyle.Substring(0, 2));

                case "Logical":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.ClassLineStyle.Substring(0, 2));


                case "Custom":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.CustomLineStyle.Substring(0, 2));

                case "Component":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.ComponentLineStyle.Substring(0, 2));

                case "Deployment":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.DeploymentLineStyle.Substring(0, 2));

                case "Package":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.PackageLineStyle.Substring(0, 2));

                case "Use Case":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.UseCaseLineStyle.Substring(0, 2));

                case "CompositeStructure":
                    return updateLineStyle(Repository, dia, connectorID, _AddinSettings.CompositeStructureLineStyle.Substring(0, 2));

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
        /// This event occurs when Enterprise Architect first loads your Add-In. Enterprise Architect itself is loading at this time so that while a Repository object is supplied, there is limited information that you can extract from it.
        /// The chief uses for EA_Connect are in initializing global Add-In data and for identifying the Add-In as an MDG Add-In.
        /// Also look at EA_Disconnect.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <returns>String identifying a specialized type of Add-In: 
        /// - "MDG" : MDG Add-Ins receive MDG Events and extra menu options.
        /// - "" : None-specialized Add-In.</returns>
        public override string EA_Connect(EA.Repository Repository)
        {
            // register only if configured
            if (AddinSettings.isShortKeySupport) HotkeyHandlers.SetupGlobalHotkeys();
            _repository = Repository;
            int v = Repository.LibraryVersion;
            if (Repository.IsSecurityEnabled)
            {
                //logInUser = Repository.GetCurrentLoginUser(false);
                //if ((logInUser.Contains("ho")) ||
                //     (logInUser.Contains("admin")) ||
                //     (logInUser.Equals(""))
                //    ) logInUserRights = UserRights.ADMIN;
            }
            Favorite.installSearches(_repository); // install searches
            return "a string";
        }
        #endregion
        public override void EA_FileOpen(EA.Repository Repository)
        {
            initializeForRepository(Repository);
        }
        public override void EA_FileClose(EA.Repository Repository)
        {
            if (_MyControlGUI != null)  _MyControlGUI.Save(); // save settings
            initializeForRepository(null);


        }
        #endregion
        #region EA Validation
        /// <summary>
        /// Called when EA start model validation. Just shows a message box
        /// </summary>
        /// <param name="Repository">the Repository</param>
        /// <param name="Args">the arguments</param>
        public override void EA_OnStartValidation(EA.Repository Repository, object Args)
        {
            MessageBox.Show("Validation started");
        }
        /// <summary>
        /// Called when EA ends model validation. Just shows a message box
        /// </summary>
        /// <param name="Repository">the Repository</param>
        /// <param name="Args">the arguments</param>
        public override void EA_OnEndValidation(EA.Repository Repository, object Args)
        {
            MessageBox.Show("Validation ended");
        }
        #endregion
        #region EA Technology Events
        /// <summary>
        /// Returns xml string for MDG to load. Possible values: Basic, Compilation, No). See AddinSettings.AutoLoadMdgXml.
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public override object EA_OnInitializeTechnologies(EA.Repository Repository) {
           

            string fileNameMdg;
            switch (AddinSettings.AutoLoadMdgXml)
            {
                case AddinSettings.AutoLoadMdg.Basic:
                    fileNameMdg = "hoToolsBasic.xml";
                    break;
                case AddinSettings.AutoLoadMdg.Compilation:
                    fileNameMdg = "hoToolsCompilation.xml";
                    break;
                default:
                    fileNameMdg = null;
                    break;
            }
            if (fileNameMdg == null) return null;
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string combinedPathMdg = Path.Combine(assemblyFolder, fileNameMdg);
            return System.IO.File.ReadAllText(combinedPathMdg);
        }
        #endregion

        #region initializeForRepository
        /// <summary>
        /// Initialize repositories for all EA hoTools Addin Tabulators
        /// </summary>
        /// <param name="rep"></param>
        private void initializeForRepository(EA.Repository rep)
        {
            _repository = rep;
            Repository = rep;
            if (_MyControlGUI != null) _MyControlGUI.Repository = rep;
            if (_FindAndReplaceGUI != null) _FindAndReplaceGUI.Repository = rep;
            if (_ScriptGUI != null) _ScriptGUI.Repository = rep;
            if (_QueryGUI != null) _QueryGUI.Repository = rep;


        }
        #endregion

        #region updateLineStyle
        private bool updateLineStyle(EA.Repository rep, EA.Diagram dia, int connectorID, string style)
        {
            if (style.ToUpper() == "NO") return false;
            foreach (EA.DiagramLink link in dia.DiagramLinks)
            {
                if (link.ConnectorID == connectorID)
                {
                    EaService.setLineStyle(rep, style);
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
        /// <param name="Repository">the Repository</param>
        /// <param name="MenuLocation">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <param name="ItemName">the name of the menu item</param>
        /// <param name="IsEnabled">boolean indicating whether the menu item is enabled</param>
        /// <param name="IsChecked">boolean indicating whether the menu is checked</param>
        public override void EA_GetMenuState(EA.Repository Repository, string MenuLocation, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            if (IsProjectOpen(Repository))
            {
                switch (ItemName)
                {
                    case menuChangeXmlFile:
                        IsChecked = false;
                        break;

                    case menuShowWindow:
                        IsChecked = false;
                        break;
                    case  menuShowSpecification:
                        IsChecked = false;
                        break;

                    case menuUnlockDiagram:
                        IsChecked = false;
                        break;

                    case menuLineStyleDiaTH:
                        IsChecked = false;
                        break;
                    case menuLineStyleDiaTV:
                        IsChecked = false;
                        break;
                    case menuLineStyleDiaLH:
                        IsChecked = false;
                        break;
                    case menuLineStyleDiaLV:
                        IsChecked = false;
                        break;
                    case menuLineStyleDiaOS:
                        IsChecked = false;
                        break;

                    case menuLocateCompositeElementorDiagram:
                        IsChecked = false;
                        break;
                    
                    case menuLocateCompositeDiagramOfElement:
                        IsChecked = false;
                        break;
                        

                case menuUsage:
                        IsChecked = false;
                        break;    
                       
                case menuCreateInteractionForOperation:
                        IsChecked = false;
                        break;

                case menuCreateStateMachineForOperation:
                        IsChecked = false;
                        break;  
                    
                case menuCorrectType:
                        IsChecked = false;
                        break;

               case menuDisplayBehavior:
                        IsChecked = false;
                        break;


               case menuUpdateActionPin:
                        IsChecked = false;
                        break;

                    case menuUpdateOperationParameter:
                        IsChecked = false;
                        break;

                    case menuCreateActivityForOperation:
                        IsChecked = false;
                        break;

                    case menuDisplayMethodDefinition:
                        IsChecked = false;
                        break;

                    
                    case menuLocateType:
                        IsChecked = false;
                        break;


                    case menuCopyGUIDToClipboard:
                        IsChecked = false;
                        break;

                    case menuCopyLinksToClipboard:
                        IsChecked = false;
                        break;

                    case menuPasteLinksFromClipboard:
                        IsChecked = false;
                        break;

                    
                    case menuAddLinkedNote:
                        IsChecked = false;
                        break;

                    case menuAddLinkedDiagramNote:
                        IsChecked = false;
                        break;

                    case menuAbout:
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
        /// <param name="Repository">the Repository</param>
        /// <param name="MenuLocation">the location of the menu</param>
        /// <param name="MenuName">the name of the menu</param>
        /// <param name="ItemName">the name of the selected menu item</param>
        public override void EA_MenuClick(EA.Repository Repository, string MenuLocation, string MenuName, string ItemName)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            EA.Diagram diaCurrent = Repository.GetCurrentDiagram();
            EA.Connector conCurrent = null;
            EA.Element el = null;

            if (diaCurrent != null) 
            {
                conCurrent = diaCurrent.SelectedConnector;
            }
            switch (ItemName)
            {

                    case menuShowWindow:
                    ShowAddinControlWindows();
                    
                    
                    break;
                case menuAbout:

                    var fAbout = new About();
                    fAbout.lblTabName.Text = release;
                    fAbout.ShowDialog();
                    break;

                    // Line style: Lateral Horizontal 
                case menuChangeXmlFile:
                     EaService.setNewXmlPath(Repository);
                     break;
                // Line style: Lateral Horizontal 
                case menuLineStyleDiaLH:
                     EaService.setLineStyle(Repository, "LH");
                   
                    break;
                // Line style: Lateral Vertical 
                case menuLineStyleDiaLV:
                    // all connections of diagram
                    EaService.setLineStyle(Repository, "LV");
                    break;
                // Line style: Tree Vertical 
                case menuLineStyleDiaTV:
                    EaService.setLineStyle(Repository, "V");
                    
                    break;

                // Line style: Tree Horizontal 
                case menuLineStyleDiaTH:
                    EaService.setLineStyle(Repository, "H");
                    
                    break;
                // Line style: Orthogonal square 
                case menuLineStyleDiaOS:
                    EaService.setLineStyle(Repository, "OS");
                    
                    break;


                //if (ItemName == menuHelp)
                //{
                //    Help fHelp = new Help();
                //    fHelp.ShowDialog();
                //    return;
                //}
                case menuUnlockDiagram:
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
                        catch { }
                        #pragma warning restore RECS0022
                    }

                    break;

                // Start specification (file parameter)
                case  menuShowSpecification:
                      EaService.showSpecification(Repository);

                    break;

                // Create Interaction for selected operation or class (all operations)
                case   menuCreateInteractionForOperation:
                    // Check selected Elements in tree
                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)Repository.GetContextObject();
                        // test multiple selection

                        // Create Activity
                        Appl.createInteractionForOperation(Repository, m);

                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        var cls = (EA.Element)Repository.GetContextObject();
                        // over all operations of class
                        foreach (EA.Method m in cls.Methods)
                        {
                            // Create Activity
                            Appl.createInteractionForOperation(Repository, m);

                        }
                    }

                    break;

                // Create Interaction for selected operation or class (all operations)
                case menuCreateStateMachineForOperation:
                    // Check selected Elements in tree
                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)Repository.GetContextObject();
                        // test multiple selection

                        // Create State Machine
                        Appl.createStateMachineForOperation(Repository, m);

                    }
                   break;



                case menuLocateCompositeElementorDiagram:
                   EaService.navigateComposite(Repository);
                    break;
                    
                // 
                case menuCorrectType:
                    if (oType.Equals(EA.ObjectType.otAttribute))
                    {
                        var a = (EA.Attribute)Repository.GetContextObject();

                        Util.updateAttribute(Repository, a);
                    }

                    if (oType.Equals(EA.ObjectType.otMethod))
                    {
                        var m = (EA.Method)Repository.GetContextObject();

                        Util.updateMethod(Repository, m);
                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        el = (EA.Element)Repository.GetContextObject();
                        Util.updateClass(Repository, el);
                    }
                    if (oType.Equals(EA.ObjectType.otPackage))
                    {
                        var pkg = (EA.Package)Repository.GetContextObject();
                        Util.updatePackage(Repository, pkg);
                    }
                    break;

                
                case menuCreateActivityForOperation:
                    EaService.CreateActivityForOperation(Repository);

                    break;

                    // get Parameter for Activity
                case menuUpdateOperationParameter:
                    EaService.UpdateActivityParameter(Repository);
                    break;

                case menuUpdateActionPin:
                    if (oType.Equals(EA.ObjectType.otPackage))
                    {
                        var pkg = (EA.Package)Repository.GetContextObject();
                        ActionPin.updateActionPinForPackage(Repository, pkg);
                    }
                    if (oType.Equals(EA.ObjectType.otElement))
                    {
                        el = (EA.Element)Repository.GetContextObject();
                        ActionPin.updateActionPinForElement(Repository, el);
                    }
                    break;
                

                case menuAddLinkedDiagramNote:
                    EaService.addDiagramNote(Repository); 
                               
                    break;

                case menuAddLinkedNote:
                    EaService.addElementNote(Repository);

                    break;

                case menuLocateType:
                    EaService.locateType(Repository);
                    
                    break;

                case menuUsage:
                    EaService.findUsage(Repository);
                    
                    break;

                case menuPasteLinksFromClipboard:
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
                                            string fText = String.Format("Error Name {0}, Error={1}", el1.Name, con.GetLastError());
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
                                            string fText = String.Format("Error Name {0}, Error={1}", el1.Name, con.GetLastError());
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

                                }
                                
                            }
                            MessageBox.Show(string.Format("Copied:{0}\r\nErrors:{1}", countInserted,countError));
                        }

                        

                    }
                    break;

                case menuCopyGUIDToClipboard:
                   EaService.copyGuidSqlToClipboard(Repository);
                   break;


                // put on Clipboard
                // 'ConnectorGUID', 'Client' if element is a client/source in this dependency
                // 'ConnectorGUID', 'Supplier' if element is a supplier/target in this dependency

                case menuCopyLinksToClipboard:
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

                case menuDisplayMethodDefinition:
                    EaService.DisplayOperationForSelectedElement(Repository, EaService.displayMode.Method);
                    break;

                case menuDisplayBehavior:
                    EaService.DisplayOperationForSelectedElement(Repository, EaService.displayMode.Behavior);
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
            if (_MyControlGUI == null)
            {

                try
                {
                    // LineStyle and more
                    if (!(_AddinSettings.LineStyleAndMoreWindow == AddinSettings.ShowInWindow.Disabled))
                    {
                    AddinControlGUI = addAddinControl<AddinControlGUI>(_AddinSettings.productName,
                        AddinControlGUI.PROGID, null,
                        AddinSettings.ShowInWindow.AddinWindow);
                        _MyControlGUI = AddinControlGUI; // static + instance
                    }

                    // with Search & Replace EA Addin Windows
                    if (!  (_AddinSettings.SearchAndReplaceWindow == AddinSettings.ShowInWindow.Disabled) ) { 
                        FindAndReplaceGUI = addAddinControl<FindAndReplaceGUI>(FindAndReplaceGUI.TABULATOR, 
                            FindAndReplaceGUI.PROGID, null, 
                            AddinSettings.ShowInWindow.AddinWindow);
                       _FindAndReplaceGUI = FindAndReplaceGUI; // static + instance
                    }

                    // with Query EA Addin Windows
                    if (! (_AddinSettings.OnlyQueryWindow == AddinSettings.ShowInWindow.Disabled) )
                    {
                        // Run as Query
                        QueryGUI = addAddinControl<QueryGUI>(QueryGUI.TABULATOR_QUERY, 
                            QueryGUI.PROGID, QueryGUI.TABULATOR_QUERY, 
                            _AddinSettings.OnlyQueryWindow);
                        _QueryGUI = QueryGUI; // static + instance
                    }

                    // with Script & Query EA Addin Windows
                    if (!(_AddinSettings.ScriptAndQueryWindow == AddinSettings.ShowInWindow.Disabled))
                    {
                        // Run as Script
                        ScriptGUI = addAddinControl<QueryGUI>(QueryGUI.TABULATOR_SCRIPT, 
                            QueryGUI.PROGID, QueryGUI.TABULATOR_SCRIPT, 
                            _AddinSettings.ScriptAndQueryWindow);
                    _ScriptGUI = ScriptGUI; // static + instance
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                    
            }
        }


        /// <summary>
        /// Add AddinGUI as a tab to EA. It sets the following properties in the following sequence: Tag, AddinSettings, Release, Repository. 
        /// </summary>
        /// <param name="tabName">Tabulator name to show Addin</param>
        /// <param name="progId">ProgID under which the Addin is registered</param>
        /// <param name="tag">Information to pass to Control</param>
        /// <returns>AddinGUI</returns>
        private T addAddinControl<T>(string tabName, string progId, object tag, AddinSettings.ShowInWindow showInWindowType)
        {
            var c = default(T);
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
                control.AddinSettings = this._AddinSettings;
                control.Release = "V" + release;
                control.Repository = _repository;



            }
            return c;
        }
        #endregion


        #region Display Behavior
        /// <summary>
        /// Display behavior or definition for selected element
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="showBehavior"></param>
        private static void DisplayOperationForSelectedElement(EA.Repository rep, displayMode showBehavior)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            // Method found
            if (oType.Equals(EA.ObjectType.otMethod))
            {
                // display behavior for method
                Appl.DisplayBehaviorForOperation(rep, (EA.Method)rep.GetContextObject());

            }
            if (oType.Equals(EA.ObjectType.otDiagram))
            {
                // find parent element
                var dia = (EA.Diagram)rep.GetContextObject();
                if (dia.ParentID > 0)
                {
                    // find parent element
                    EA.Element parentEl = rep.GetElementByID(dia.ParentID);
                    //
                    locateOperationFromBehavior(rep, parentEl, showBehavior);
                }
                else
                {
                    // open diagram
                    rep.OpenDiagram(dia.DiagramID);
                }
            }


            // Connector / Message found
            if (oType.Equals(EA.ObjectType.otConnector))
            {
                var con = (EA.Connector)rep.GetContextObject();
                if (con.Type.Equals("StateFlow"))
                {

                    EA.Method m = Util.getOperationFromConnector(rep, con);
                    if (m != null)
                    {
                        if (showBehavior.Equals(displayMode.Behavior))
                        {
                            Appl.DisplayBehaviorForOperation(rep, m);
                        }
                        else
                        {
                            rep.ShowInProjectView(m);
                        }

                    }


                }

                if (con.Type.Equals("Sequence"))
                {
                    // If name is of the form: OperationName(..) the operation is associated to an method
                    string opName = con.Name;
                    if (opName.EndsWith(")", StringComparison.CurrentCulture))
                    {
                        // extract the name
                        int pos = opName.IndexOf("(", StringComparison.CurrentCulture);
                        opName = opName.Substring(0, pos);
                        EA.Element el = rep.GetElementByID(con.SupplierID);
                        // find operation by name
                        foreach (EA.Method op in el.Methods)
                        {
                            if (op.Name == opName)
                            {
                                rep.ShowInProjectView(op);
                                //Appl.DisplayBehaviorForOperation(Repository, op);
                                return;
                            }
                        }
                        if ((el.Type.Equals("Sequence") || el.Type.Equals("Object")) && el.ClassfierID > 0)
                        {
                            el = (EA.Element)rep.GetElementByID(el.ClassifierID);
                            foreach (EA.Method op in el.Methods)
                            {
                                if (op.Name == opName)
                                {
                                    if (showBehavior.Equals(displayMode.Behavior))
                                    {
                                        Appl.DisplayBehaviorForOperation(rep, op);
                                    }
                                    else
                                    {
                                        rep.ShowInProjectView(op);
                                    }

                                }
                            }
                        }

                    }
                }
            }

            // Element
            if (oType.Equals(EA.ObjectType.otElement))
            {
                var el = (EA.Element)rep.GetContextObject();

                if (el.Type.Equals("Activity"))
                {
                    // Open Behavior for Activity
                    Util.OpenBehaviorForElement(rep, el);


                }
               if (el.Type.Equals("State"))
                {
                    // get operations
                    foreach (EA.Method m in el.Methods)
                    {
                        // display behaviors for methods
                        Appl.DisplayBehaviorForOperation(rep, m);
                    }
                }
                if (el.Type.Equals("Action"))
                {
                    foreach (EA.CustomProperty custproperty in el.CustomProperties)
                    {
                        if (custproperty.Name.Equals("kind") && custproperty.Value.Equals("CallOperation"))
                        {
                            showMethodFromAction(rep, el, showBehavior);
                        }
                        if (custproperty.Name.Equals("kind") && custproperty.Value.Equals("CallBehavior"))
                        {
                            el = rep.GetElementByID(el.ClassfierID);
                            Util.OpenBehaviorForElement(rep, el);
                        }
                    }

                }
                if (el.Type.Equals("Activity") || el.Type.Equals("StateMachine") || el.Type.Equals("Interaction"))
                {
                    locateOperationFromBehavior(rep, el, showBehavior);
                }
            }
        }
        #endregion

        #region showMethodFromAction
        /// <summary>
        /// Show for Action: Behavior or Method in Browser
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="showBehavior"></param>
        static void showMethodFromAction(EA.Repository rep, EA.Element el, displayMode showBehavior)
        {
            EA.Method method = Util.getOperationFromAction(rep, el);
            if (method != null)
            {
                if (showBehavior.Equals(displayMode.Behavior))
                {
                    Appl.DisplayBehaviorForOperation(rep, method);
                }
                else
                {
                    rep.ShowInProjectView(method);
                }
            }
        }
        #endregion
        #region locateOperationFromBehavior
        static void locateOperationFromBehavior(EA.Repository rep, EA.Element el, displayMode showBehavior)
        {
            EA.Method method = Util.getOperationFromBrehavior(rep, el);
            if (method != null)
            {
                if (showBehavior.Equals(displayMode.Behavior))
                {
                    Appl.DisplayBehaviorForOperation(rep, method);
                }
                else
                {
                    rep.ShowInProjectView(method);
                }
            }
        }
        #endregion
        #region BehaviorForOperation
        static void BehaviorForOperation(EA.Repository rep, EA.Method method)
        {
            string behavior = method.Behavior;
            if (behavior.StartsWith("{", StringComparison.CurrentCulture) & behavior.EndsWith("}", StringComparison.CurrentCulture))
            {
                // get object according to behavior
                EA.Element el = rep.GetElementByGuid(behavior);
                // Activity
                if (el == null) { }
                else
                {
                    if (el.Type.Equals("Activity") || el.Type.Equals("Interaction") || el.Type.Equals("StateMachine"))
                    {
                        Util.OpenBehaviorForElement(rep, el);
                    }
                }
            }
        }
        #endregion

        #region LineStyle
        //---------------------------------------------------------------------------------------------------------------
        // line style
        // LH = "Line Style: Lateral Horizontal";
        // LV = "Line Style: Lateral Vertical";
        // TH  = "Line Style: Tree Horizontal";
        // TV = "Line Style: Tree Vertical";
        // OS = "Line Style: Orthogonal Square";
        // OR =              Orthogonal Round
        // A =               Automatic
        // D =               Direct
        // C =               Customer

#pragma warning disable RECS0154
        void btnLH_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "LH");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnLV_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "LV");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnTH_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "H");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnTV_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "V");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnOS_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "OS");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnOR_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "OR");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnA_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "A");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154    
        void btnD_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "D");
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnC_Click(object sender, EventArgs e)
        {
            EaService.setLineStyle(_repository, "C");
        }
        #pragma warning restore RECS0154
        #pragma warning disable RECS0154
        void btnComposite_Click(object sender, EventArgs e)
        {
            EaService.navigateComposite(_repository);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnDisplayBehavior_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(_repository, EaService.displayMode.Behavior);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnLocateOperation_Click(object sender, EventArgs e)
        {
            EaService.DisplayOperationForSelectedElement(_repository, EaService.displayMode.Method);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnAddElementNote_Click(object sender, EventArgs e)
        {
            EaService.addElementNote(_repository);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnAddDiagramNote_Click(object sender, EventArgs e)
        {
            EaService.addDiagramNote(_repository);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnLocateType_Click(object sender, EventArgs e)
        {
            EaService.locateType(_repository);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnFindUsage_Click(object sender, EventArgs e)
        {
            EaService.findUsage(_repository);
        }
        #pragma warning restore RECS0154

        #pragma warning disable RECS0154
        void btnDisplaySpecification_Click(object sender, EventArgs e)
        {
            EaService.showSpecification(_repository);
        }
        #pragma warning restore RECS0154
        #endregion


    }
}
