using System;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using hoTools.EaServices;
using hoTools.Connectors;
using System.Collections.Generic;
using Control.EaAddinShortcuts;
using System.Text.RegularExpressions;
using GlobalHotkeys;
//using hoTools.Connectors;

namespace hoTools.Settings
{
    /// <summary>
    /// Settings for hoTools.
    /// It merges default settings (ActiveX.dll.config) with current settings in install directory to the current settings. 
    /// </summary>
    /// <code>
    /// Default:         ..\Addin\ActiveX.dll.config   with copy to output directory       
    /// Current Debug:   Addin\ActiveX.dll.config      (With copy to output directory) 
    /// Current Release: APPData\Local\Apps\hoTools\   (App DLL-Install library) 
    /// </code>
    public class AddinSettings
    {
        public enum ShowInWindow { AddinWindow, TabWindow,Disabled};
        public enum AutoLoadMdg { Basic, Compilation, No};

        // File path of configuration file
        // %APPDATA%ho\hoTools\user.config
        public string ConfigFilePath { get; }
        public string ConfigPath { get; }

        /// <summary>
        /// List of history sql files (recent 20 used sql files)
        /// </summary>
        public SqlHistoryFilesCfg HistorySqlFiles { get; }

        /// <summary>
        /// List of 10 last opened sql files
        /// </summary>
        public SqlLastOpenedFilesCfg LastOpenedFiles { get; }

        // Configuration 5 button searches by key
        public EaAddinButtons[] ButtonsSearch;

        // Configuration 5 button services by key
        public List<ServicesCallConfig> ButtonsServices;
        // all available services
        public List<ServiceCall> AllServices = new List<ServiceCall>();

        public List<GlobalKeysConfig.GlobalKeysServiceConfig> GlobalShortcutsService = new List<GlobalKeysConfig.GlobalKeysServiceConfig>();
        public List<GlobalKeysConfig.GlobalKeysSearchConfig> GlobalShortcutsSearch = new  List<GlobalKeysConfig.GlobalKeysSearchConfig>();

        // Connectors
        public LogicalConnectors LogicalConnectors = new LogicalConnectors();
        public ActivityConnectors ActivityConnectors = new ActivityConnectors();

        /// <summary>
        /// Configuration delivered with the installation in the install directory
        /// <para/>c:\Users\user\AppData\Local\Apps\hoTools\ActiveX.dll.config
        /// </summary>
        protected Configuration DefaultConfig { get; set; }

        /// <summary>
        /// Configuration stored in Roaming of the user
        /// <para/>c:\Users\user\AppData\Roaming\ho\hoTools\user.config
        /// </summary>
        protected Configuration CurrentConfig { get; set; }
        #region Constructor
        /// <summary>
        /// Merge default settings (install DLLs) with current settings (user.config)
        /// Read settings from %APPDATA%\ho\hoTools\user.config or
        ///                    %APPDATA%\ho\hoTools_ZFLT\user.config
        /// </summary>
        public AddinSettings()
        {
            GetDefaultSettings();

            Configuration roamingConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);

            //the roamingConfig now get a path such as C:\Users\<user>\AppData\Roaming\Sparx_Systems_Pty_Ltd\DefaultDomain_Path_2epjiwj3etsq5yyljkyqqi2yc4elkrkf\9,_2,_0,_921\user.config
            // which I don't like. So we move up three directories and then add a directory for the EA Navigator so that we get
            // C:\Users\<user>\AppData\Roaming\ho\hoTools\user.config
            string configFileName = System.IO.Path.GetFileName(roamingConfig.FilePath);
            string configDirectory = System.IO.Directory.GetParent(roamingConfig.FilePath).Parent.Parent.Parent.FullName;
            string path = "";
            switch (Customer) {
                case CustomerCfg.HoTools:
                path =  @"\ho\hoTools\";
                break;
                case CustomerCfg.Var1:
                path =  @"\ho\hoTools_VAR1\";
                break;
                default: 
                path =  @"\ho\hoTools\";
                break;

            }
            // remember 
            ConfigPath = configDirectory + path;
            ConfigFilePath = ConfigPath + configFileName;

            // Map the roaming configuration file. This
            // enables the application to access 
            // the configuration file using the
            // System.Configuration.Configuration class
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = ConfigFilePath;
            // Get the mapped configuration file.
            CurrentConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);


            //merge the default settings
            // - For simple values that's all to do
            //   they uses Getter/Setter, no special handling here
            MergeDefaultSettings();

            // get list from config
            // for simple values nothing is to do here (there exists only a getter/setter)
            ButtonsSearch = GetShortcutsSearch();
            ButtonsServices = GetShortcutsServices();
            GlobalShortcutsService = GetGlobalShortcutsService();
            GlobalShortcutsSearch = GetGlobalShortcutsSearch();
            GetConnector(LogicalConnectors);
            GetConnector(ActivityConnectors);
            GetAllServices();
            HistorySqlFiles = new SqlHistoryFilesCfg(CurrentConfig);// history of sql files 
            LastOpenedFiles = new SqlLastOpenedFilesCfg(CurrentConfig); // last opened files

            // update lists
            UpdateSearchesAndServices();

            //-------------------------------------------
            // Simple values uses Getter/Setter, no special handling here
        }
        #endregion

        #region getDefaultSettings
        /// <summary>
        /// gets the default settings config.
        /// </summary>
        protected void GetDefaultSettings()
        {
            string defaultConfigFilePath = Assembly.GetExecutingAssembly().Location;
            DefaultConfig = ConfigurationManager.OpenExeConfiguration(defaultConfigFilePath);
        }
        #endregion
        #region mergeDefaultSettings
        /// <summary>
        /// merge the default settings with the current config.
        /// </summary>
        protected void MergeDefaultSettings()
        {
            //defaultConfig.AppSettings.Settings["menuOwnerEnabled"].Value
            if (DefaultConfig.AppSettings.Settings.Count == 0)
            {
                MessageBox.Show("No default settings in '" + DefaultConfig.FilePath + "' found!", "Installation wasn't successful!");
            }
            foreach (KeyValueConfigurationElement configEntry in DefaultConfig.AppSettings.Settings)
            {
                if (!CurrentConfig.AppSettings.Settings.AllKeys.Contains(configEntry.Key))
                {
                    CurrentConfig.AppSettings.Settings.Add(configEntry.Key, configEntry.Value);
                }
            }
            // save the configuration
            CurrentConfig.Save();
        }
        #endregion


        #region Properties


        #region Property: isAskForQueryUpdateOutside
        public bool IsAskForQueryUpdateOutside
        {
            get
            {
                return GetBoolConfigValue("isAskForQueryUpdateOutside");
            }
            set
            {
                SetBoolConfigValue("isAskForQueryUpdateOutside", value);
            }
        }
        #endregion

        #region Property: SqlEditor
        public string SqlEditor
        {
            get
            {
                return GetStringConfigValue("SqlEditor");
            }
            set
            {
                SetStringConfigValue("SqlEditor", value);
            }
        }

      


        #region Property: isLineStyleSupport
        public bool IsLineStyleSupport
        {
            get
            {
                bool result;
                var p = CurrentConfig.AppSettings.Settings["isLineStyleSupport"];
                if (p == null) return true;
                if (bool.TryParse(p.Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isLineStyleSupport"].Value = value.ToString();
            }
        }
        #endregion

        #region Property: isShortKeySupport
        public bool IsShortKeySupport
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isShortKeySupport"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isShortKeySupport"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isShowServiceButton
        public bool IsShowServiceButton
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isShowServiceButton"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isShowServiceButton"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isShowQueryButton
        public bool IsShowQueryButton
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isShowQueryButton"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isShowQueryButton"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isFavoriteSupport
        public bool IsFavoriteSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isFavoriteSupport"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isFavoriteSupport"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isConveyedItemsSupport
        public bool IsConveyedItemsSupport
        {
            get
            {
                return GetBoolConfigValue("isConveyedItemsSupport");
            }
            set
            {
                SetBoolConfigValue("isConveyedItemsSupport", value);
            }
        }
        #endregion





        #region CustomerCfg (possible customer) 
        /// <summary>
        /// To define different customer
        /// </summary>
        public enum CustomerCfg
        {
            Var1,
            HoTools
        }
        #endregion
        #region Property: isAdvancedFeatures
        public bool IsAdvancedFeatures
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isAdvancedFeatures"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isAdvancedFeatures"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isAdvancedPort

        public bool IsAdvancedPort
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isAdvancedPort"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isAdvancedPort"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: isAdvancedDiagramNote

        public bool IsAdvancedDiagramNote
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isAdvancedDiagramNote"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isAdvancedDiagramNote"].Value = value.ToString();

            }

        }
        #endregion


        #region Property: ScriptAndQueryWindow
        public ShowInWindow ScriptAndQueryWindow
        {
            get
            {
                ShowInWindow result;
                if (Enum.TryParse(CurrentConfig.AppSettings.Settings["ScriptAndQueryWindow"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["ScriptAndQueryWindow"].Value = value.ToString();

            }

        }
        #endregion

        #region Property: AutoLoadMdgXml
        /// <summary>
        /// Property which MDG to load (Basic, Compilation, No)
        /// </summary>
        public AutoLoadMdg AutoLoadMdgXml
        {
            get
            {
                AutoLoadMdg result;
                if (Enum.TryParse(CurrentConfig.AppSettings.Settings["AutoLoadMdg"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return AutoLoadMdg.No;
                }
            }
            set
            {
                if (CurrentConfig.AppSettings.Settings["AutoLoadMdg"] != null)
                {
                    CurrentConfig.AppSettings.Settings["AutoLoadMdg"].Value = value.ToString();
                } else
                {
                    MessageConfigValueNotExists("AutoLoadMdg", value.ToString());
                }

            }

        }
        #endregion

        #region Property: OnlyQueryWindow
        public ShowInWindow OnlyQueryWindow
        {
            get
            {
                ShowInWindow result;
                if (Enum.TryParse(CurrentConfig.AppSettings.Settings["OnlyQueryWindow"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                if (CurrentConfig.AppSettings.Settings["OnlyQueryWindow"] != null)
                {
                    CurrentConfig.AppSettings.Settings["OnlyQueryWindow"].Value = value.ToString();
                }
                else
                {
                    MessageConfigValueNotExists("OnlyQueryWindow", value.ToString());
                }

            }

        }
        #endregion

        #region Property: SearchAndReplaceWindow
        public ShowInWindow SearchAndReplaceWindow
        {
            get
            {
                ShowInWindow result;
                if (Enum.TryParse(CurrentConfig.AppSettings.Settings["SearchAndReplaceWindow"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["SearchAndReplaceWindow"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: LineStyleAndMoreWindow
        public ShowInWindow LineStyleAndMoreWindow
        {
            get
            {
                ShowInWindow result;
                if (Enum.TryParse(CurrentConfig.AppSettings.Settings["LineStyleAndMoreWindow"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["LineStyleAndMoreWindow"].Value = value.ToString();

            }

        }
        #endregion

        #region Property: isVcSupport

        public bool IsVcSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isVcSupport"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isVcSupport"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: isSvnSupport
        
        public bool IsSvnSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(CurrentConfig.AppSettings.Settings["isSvnSupport"].Value, out result))
                {
                    return result;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["isSvnSupport"].Value = value.ToString();

            }

        }
        #endregion
        
        
        #region Property: FileManagerPath
        public string FileManagerPath
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["FileManagerPath"].Value == null)
                {
                    return "FileManagerPath";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["FileManagerPath"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["FileManagerPath"].Value = value;

            }
        }
        #endregion
        #region Property: quickSearchName
        public string QuickSearchName
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["QuickSearchName"].Value == null) {
                    return "Quick Search";
                }else {
                return (CurrentConfig.AppSettings.Settings["QuickSearchName"].Value );
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["QuickSearchName"].Value =value;
               
            }
        }
        #endregion
        #region Property: sqlFolder
        /// <summary>
        /// Folder for SQL Searches
        /// </summary>
        public string SqlFolder
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["SqlFolder"].Value == null)
                {
                    return @"c:\temp\sql";
                }
                else {
                    return (CurrentConfig.AppSettings.Settings["SqlFolder"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["SqlFolder"].Value = value;

            }
        }
        #endregion

        #region Property: productName
        public string ProductName
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["ProductName"].Value == null)
                {
                    return "hoTools";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["ProductName"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["ProductName"].Value = value;

            }
        }
        #endregion
        #region Property: ActivityLineStyle
        public string ActivityLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["ActivityLineStyle"].Value == null)
                {
                    return "LV";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["ActivityLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["ActivityLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: StatechartLineStyle
        public string StatechartLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["StatechartLineStyle"].Value == null)
                {
                    return "B";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["StatechartLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["StatechartLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: CustomLineStyle
        public string CustomLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["CustomLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["CustomLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["CustomLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: ClassLineStyle
        public string ClassLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["ClassLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["ClassLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["ClassLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: PackageLineStyle
        public string PackageLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["PackageLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["PackageLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["PackageLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: UseCaseLineStyle
        public string UseCaseLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["UseCaseLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["UseCaseLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["UseCaseLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: DeploymentLineStyle
        public string DeploymentLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["DeploymentLineStyle"].Value == null)
                {
                    return "B";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["DeploymentLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["DeploymentLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: CompositeStructureLineStyle
        public string CompositeStructureLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: ComponentLineStyle
        public string ComponentLineStyle
        {
            get
            {
                if (CurrentConfig.AppSettings.Settings["ComponentLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (CurrentConfig.AppSettings.Settings["ComponentLineStyle"].Value);
                }
            }
            set
            {
                CurrentConfig.AppSettings.Settings["ComponentLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: Customer

        public CustomerCfg Customer
        {
            get
            {
                // get customer
                var s = DefaultConfig.AppSettings.Settings["Customer"];
                if (s == null)  return CustomerCfg.HoTools;
                switch (s.Value)
                {
                    case "F52AB09A-8ED0-4159-9AB4-FFD986983280":
                        return CustomerCfg.Var1;
                        
                    case "14F09211-3460-47A6-B837-A477491F0A67":
                        return CustomerCfg.HoTools;
                    case "4661B136-39E8-45B9-9015-C6C18A04EDCF":
                        return CustomerCfg.HoTools;
                    case "7A415B23-7D1E-49CB-84BA-BAC1C5BFD8FE":
                        return CustomerCfg.HoTools;
                    default:
                        return CustomerCfg.HoTools;

                }
                
            }
            

        }
        #endregion
        #endregion


        #region save
        /// <summary>
        /// saves the settings to the config file
        /// </summary>
        public void Save()
        {
            try
            {
                SetShortcuts(ButtonsSearch);
                SetServices(ButtonsServices);
                SetGlobalShortcutsSearch(GlobalShortcutsSearch);
                SetGlobalShortcutsService(GlobalShortcutsService);

                SetConnector(LogicalConnectors);
                SetConnector(ActivityConnectors);
                HistorySqlFiles.Save();
                LastOpenedFiles.Save();
                CurrentConfig.Save();
                CurrentConfig.Save();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), @"Error storing user.config");
            }
        }
#endregion
        #region refresh
        public void Refresh()
        {
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = CurrentConfig.FilePath};
            // Get the mapped configuration file.
            CurrentConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }
        #endregion
        #region setShortcuts
        public void SetShortcuts(EaAddinButtons[] l)
        {
            foreach (EaAddinButtons button in l)
            {
                if (button == null) continue;
                {

                    var el = (EaAddinShortcutSearch)button;
                    string basicKey = "Key" + el.KeyPos;
                    CurrentConfig.AppSettings.Settings[basicKey + "Text"].Value = el.KeyText;
                    CurrentConfig.AppSettings.Settings[basicKey + "Type"].Value = "Search";
                    CurrentConfig.AppSettings.Settings[basicKey + "Par1"].Value = el.KeySearchName;
                    CurrentConfig.AppSettings.Settings[basicKey + "Par2"].Value = el.KeySearchTerm;
                    CurrentConfig.AppSettings.Settings[basicKey + "Tooltip"].Value = el.KeySearchTooltip;
                }
            }
        }

        #endregion
        #region getShortcutsSearch
        private EaAddinButtons[] GetShortcutsSearch()
        {
            int pos = 0;
            string text = "";
            string type = "";
            string par1 = "";
            string par2 = "";
            EaAddinButtons[] l = new EaAddinButtons[10];
            foreach (KeyValueConfigurationElement configEntry in CurrentConfig.AppSettings.Settings)
            {
                var sKey = configEntry.Key;
                string regex = @"key([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    int posValue = Convert.ToInt16(match.Groups[1].Value);
                    switch (match.Groups[2].Value)
                    {
                        case "Type":
                            type = configEntry.Value;
                            break;
                        case "Text":
                            text = configEntry.Value;
                            break;
                        case "Par1":
                            par1 = configEntry.Value;
                            break;
                        case "Par2":
                            par2 = configEntry.Value;
                            break;
                        case "Tooltip":
                            switch (type)
                            {
                                case "Search":
                                    l[pos] = new EaAddinShortcutSearch(posValue, text, par1, par2, configEntry.Value);
                                    pos = pos + 1;
                                    break;
                            }
                            break;
                    }
                }
            }

            return l;
        }
        #endregion
        #region getShortcutsServices
        private List<ServicesCallConfig> GetShortcutsServices()
        {
            var l = new List<ServicesCallConfig>();
            string guid = "";
            foreach (KeyValueConfigurationElement configEntry in CurrentConfig.AppSettings.Settings)
            {
                var sKey = configEntry.Key;
                string regex = @"service([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    int pos = Convert.ToInt16(match.Groups[1].Value);
                    switch (match.Groups[2].Value)
                    {
                        case "GUID":
                            guid = configEntry.Value;
                            break;
                        case "Text":
                            var text = configEntry.Value;
                            l.Add(new ServicesCallConfig(pos, guid, text));
                            break;
                    }
                }
            }
            return l;

        }
        #endregion
        #region getAllServices
        // get all possible services
        private void GetAllServices()
        {
            Type type = typeof(EaService);
            AllServices.Add(new ServiceCall(null, "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}", "-- no --", "no service selected", false));
            foreach (MethodInfo method in type.GetMethods())
            {

                foreach (Attribute attr in method.GetCustomAttributes(true))
                {
                    var serviceOperation = attr as ServiceOperationAttribute;
                    if (null != serviceOperation)
                    {
                        AllServices.Add(new ServiceCall(method, serviceOperation.GUID, serviceOperation.Description, serviceOperation.Help, serviceOperation.IsTextRequired));
                    }
                }
            }
            AllServices.Sort(new ServicesCallDescriptionComparer());
        }
        #endregion
        #region updateSearchesAndServices
        /// <summary>
        /// Update the services and searches for:
        /// - Buttons
        /// - Global keys / global shortcuts / keyboard keys
        /// by 
        /// - Method
        /// - Tool tip
        /// </summary>
        public void UpdateSearchesAndServices()
        {
            foreach (ServicesCallConfig service in ButtonsServices)
            {
                if (service.GUID != "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}")
                {
                    //int index = allServices.BinarySearch(new EaServices.ServiceCall(null, service.GUID, "","", false), new EaServices.ServicesCallGUIDComparer());
                    foreach (ServiceCall s in AllServices) {
                        if (service.GUID == s.GUID)
                        {
                            service.Method = s.Method;
                            service.Help = s.Help;
                            service.Description = s.Description;
                            break;
                        }
                    }
                    
                }

            }
            foreach (GlobalKeysConfig.GlobalKeysServiceConfig service in GlobalShortcutsService)
            {
                if (service.GUID != "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}")
                {
                    //int index = allServices.BinarySearch(new EaServices.ServiceCall(null, service.GUID, "","", false), new EaServices.ServicesCallGUIDComparer());
                    foreach (ServiceCall s in AllServices)
                    {
                        if (service.GUID == s.GUID)
                        {
                            service.Method = s.Method;
                            service.Tooltip = s.Help;
                            service.Description = s.Description;
                            break;
                        }
                    }

                }

            }
        }
        #endregion
        #region getGlobalShortcutsService
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<GlobalKeysConfig.GlobalKeysServiceConfig> GetGlobalShortcutsService()
         {
             var l = new List<GlobalKeysConfig.GlobalKeysServiceConfig>();
            string key = "";
             string modifier1 = "";
             string modifier2 = "";
             string modifier3 = "";
             string modifier4 = "";

            foreach (KeyValueConfigurationElement configEntry in CurrentConfig.AppSettings.Settings)
             {
                 var sKey = configEntry.Key;
                 string regex = @"globalKeyService([0-9]+)([a-zA-Z_0-9]+)";
                 Match match = Regex.Match(sKey, regex);
                 if (match.Success)
                 {
                     switch (match.Groups[2].Value)
                     {
                         case "Key":
                             key = configEntry.Value;
                             break;
                         case "Modifier1":
                             modifier1 = configEntry.Value;
                             break;
                         case "Modifier2":
                             modifier2 = configEntry.Value;
                             break;
                         case "Modifier3":
                             modifier3 = configEntry.Value;
                             break;
                         case "Modifier4":
                             modifier4 = configEntry.Value;
                             break;
                         case "GUID":
                             var guid = configEntry.Value;
                             l.Add(new GlobalKeysConfig.GlobalKeysServiceConfig(key,modifier1,modifier2,modifier3,modifier4,"", guid, "",false));
                             break;
                     }
                 }
             }
             return l;
            //globalServiceKeys.Add(new GlobalKeysConfig.GlobalKeysServiceConfig("F", "Ctrl", "No", "No", "No","Help","","",false));
            //globalServiceKeys.Add(new GlobalKeysConfig.GlobalKeysServiceConfig("A", "Shift", "No", "No", "No", "Help", "", "", false));
            //globalServiceKeys.Add(new GlobalKeysConfig.GlobalKeysServiceConfig("B", "Win", "No", "No", "No", "Help", "", "", false));
            //globalServiceKeys.Add(new GlobalKeysConfig.GlobalKeysServiceConfig("C", "Alt", "No", "No", "No", "Help", "", "", false));
            //globalServiceKeys.Add(new GlobalKeysConfig.GlobalKeysServiceConfig("D", "No", "No", "No", "No", "Help", "", "", false));
        }
        #endregion
        #region getGlobalShortcutsSearch
        private List<GlobalKeysConfig.GlobalKeysSearchConfig> GetGlobalShortcutsSearch()
        {
            var l = new List<GlobalKeysConfig.GlobalKeysSearchConfig>();
            string key = "";
            string modifier1 = "";
            string modifier2 = "";
            string modifier3 = "";
            string modifier4 = "";
            string searchName = "";
            string searchTerm = "";

            foreach (KeyValueConfigurationElement configEntry in CurrentConfig.AppSettings.Settings)
            {
                var sKey = configEntry.Key;
                const string regex = @"globalKeySearch([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    switch (match.Groups[2].Value)
                    {
                        case "Key":
                            key = configEntry.Value;
                            break;
                        case "Modifier1":
                            modifier1 = configEntry.Value;
                            break;
                        case "Modifier2":
                            modifier2 = configEntry.Value;
                            break;
                        case "Modifier3":
                            modifier3 = configEntry.Value;
                            break;
                        case "Modifier4":
                            modifier4 = configEntry.Value;
                            break;
                        case "SearchName":
                            searchName = configEntry.Value;
                            break;
                        case "SearchTerm":
                            searchTerm = configEntry.Value;
                            break;
                        case "Tooltip":
                            var tooltip = configEntry.Value;

                            l.Add(new GlobalKeysConfig.GlobalKeysSearchConfig(key, modifier1, modifier2, modifier3, modifier4, tooltip, 
                                searchName, searchTerm));
                            break;
                    }
                }
            }
            return l;
        }
        #endregion
        #region setGlobalShortcutsSearch
        public void SetGlobalShortcutsSearch(List<GlobalKeysConfig.GlobalKeysSearchConfig> l) {
            for (int i = 0; i< l.Count;i++)
            {
                if (l[i] == null) continue;
                    GlobalKeysConfig.GlobalKeysSearchConfig el = l[i];
                    string basicKey = "globalKeySearch" + (i+1);
                    CurrentConfig.AppSettings.Settings[basicKey + "Key"].Value = el.Key;
                    CurrentConfig.AppSettings.Settings[basicKey + "Modifier1"].Value = el.Modifier1;
                    CurrentConfig.AppSettings.Settings[basicKey + "Modifier2"].Value = el.Modifier2;
                    CurrentConfig.AppSettings.Settings[basicKey + "Modifier3"].Value = el.Modifier3;
                    CurrentConfig.AppSettings.Settings[basicKey + "Modifier4"].Value = el.Modifier4;
                    CurrentConfig.AppSettings.Settings[basicKey + "SearchName"].Value = el.SearchName;
                    CurrentConfig.AppSettings.Settings[basicKey + "SearchTerm"].Value = el.SearchTerm;
                    CurrentConfig.AppSettings.Settings[basicKey + "Tooltip"].Value = el.Tooltip;
            }

        }
        #endregion
        #region setGlobalShortcutsService
        public void SetGlobalShortcutsService(List<GlobalKeysConfig.GlobalKeysServiceConfig> l)
        {
            
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                GlobalKeysConfig.GlobalKeysServiceConfig el = l[i];
                string basicKey = "globalKeyService" + (i+1);
                CurrentConfig.AppSettings.Settings[basicKey + "Key"].Value = el.Key;
                CurrentConfig.AppSettings.Settings[basicKey + "Modifier1"].Value = el.Modifier1;
                CurrentConfig.AppSettings.Settings[basicKey + "Modifier2"].Value = el.Modifier2;
                CurrentConfig.AppSettings.Settings[basicKey + "Modifier3"].Value = el.Modifier3;
                CurrentConfig.AppSettings.Settings[basicKey + "Modifier4"].Value = el.Modifier4;
                CurrentConfig.AppSettings.Settings[basicKey + "GUID"].Value = el.GUID;
            }

        }
        #endregion
        #region getConnector
        public void GetConnector(DiagramConnector l)
        {
            string diagramType = l.DiagramType;
            string type = "";
            string lineStyle = "";
            string stereotype = "";
            bool isDefault = false;

            foreach (KeyValueConfigurationElement configEntry in CurrentConfig.AppSettings.Settings)
            {
                var sKey = configEntry.Key;
                string regex = diagramType +"Connector([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    switch (match.Groups[2].Value)
                    {
                        case "Type":
                            type = configEntry.Value;
                            break;
                        case "Stereotype":
                            stereotype = configEntry.Value;
                            break;
                        case "LineStyle":
                            lineStyle = configEntry.Value;
                            break;
                        case "IsDefault":
                            isDefault = false;
                            isDefault |= configEntry.Value == "True";

                            break;
                        case "IsEnabled":
                            var isEnabled = false;
                            isEnabled |= configEntry.Value == "True";
                            l.Add(new Connector(type, stereotype, lineStyle, isDefault, isEnabled));
                            break;
                    }
                }
            }
           
        }
        #endregion
        #region setConnector
        public void SetConnector(DiagramConnector l)
        {

            string diagramType = l.DiagramType;
            string basicKey;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                Connector el = l[i];
                basicKey = diagramType + "Connector" + (i + 1);

                var key = basicKey+ "Type";
                if (! CurrentConfig.AppSettings.Settings.AllKeys.Contains(key))
                CurrentConfig.AppSettings.Settings.Add(key, el.Type); 
                else  CurrentConfig.AppSettings.Settings[key].Value = el.Type; 

                key = basicKey + "Stereotype";
                if (! CurrentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    CurrentConfig.AppSettings.Settings.Add(key, el.Stereotype);
                else CurrentConfig.AppSettings.Settings[key].Value = el.Stereotype;

                key = basicKey + "LineStyle";
                if (!CurrentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    CurrentConfig.AppSettings.Settings.Add(key, el.LineStyle);
                else CurrentConfig.AppSettings.Settings[key].Value = el.LineStyle;


                key = basicKey + "IsDefault";
                if (! CurrentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    CurrentConfig.AppSettings.Settings.Add(key, el.IsDefault.ToString());
                else CurrentConfig.AppSettings.Settings[key].Value = el.IsDefault.ToString();

                key = basicKey + "IsEnabled";
                if (! CurrentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    CurrentConfig.AppSettings.Settings.Add(key, el.IsEnabled.ToString());
                else CurrentConfig.AppSettings.Settings[key].Value = el.IsEnabled.ToString();
                               
            }
            // delete unused entries
            int index = l.Count +1;
            while (true)
            {
            basicKey = diagramType + "Connector" + index;
            if (CurrentConfig.AppSettings.Settings.AllKeys.Contains(basicKey+"Type"))
            {
                CurrentConfig.AppSettings.Settings.Remove(basicKey + "IsEnabled");
                CurrentConfig.AppSettings.Settings.Remove(basicKey + "IsDefault");
                CurrentConfig.AppSettings.Settings.Remove(basicKey + "Stereotype");
                CurrentConfig.AppSettings.Settings.Remove(basicKey + "Type");
                index = index + 1;
            }

            else {break;}
            }

        }
        #endregion
        #region setServices
        public void SetServices(List<ServicesCallConfig> l)
        {
            for (var i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                {

                    var el = l[i];
                    string basicKey = "service" + (i + 1);
                    CurrentConfig.AppSettings.Settings[basicKey + "GUID"].Value = el.GUID;
                    CurrentConfig.AppSettings.Settings[basicKey + "Text"].Value = el.ButtonText;
                }
            }

        }
        #endregion


        /// <summary>
        /// Get bool config value. If the value don't exists return a false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetBoolConfigValue(string name)
        {
            bool result;
            var p = CurrentConfig.AppSettings.Settings[name];
            if (p == null) return false;// default
            if (bool.TryParse(p.Value, out result))
            {
                return result;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Set bool config value. If error output a message
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetBoolConfigValue(string name, bool value)
        {
            var cfgValue = CurrentConfig.AppSettings.Settings[name];
            if (cfgValue != null)
            {
                cfgValue.Value = value.ToString();
            }else
            {
                MessageConfigValueNotExists(name, value.ToString());
            }
        }
        #endregion
        /// <summary>
        /// Get string value from configuration
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetStringConfigValue(string name)
        {
            var p = CurrentConfig.AppSettings.Settings[name];
            if (p == null) return "";// default
            return p.Value;
        }
        /// <summary>
        /// Set string value in configuration. If error output error message
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetStringConfigValue(string name, string value)
        {
            var p = CurrentConfig.AppSettings.Settings[name];
            if (p !=  null)
            {
                p.Value = value;
            }
            else
            {
                MessageConfigValueNotExists(name, value);
            }
        }

        /// <summary>
        /// Output error Message box for not existent configuration parameter or invalid value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void MessageConfigValueNotExists(string name, string value)
        {
            MessageBox.Show($"Parameter '{name}' with value '{value}' don't exists in configuration or is invalid!");
        }

        
    }
}

