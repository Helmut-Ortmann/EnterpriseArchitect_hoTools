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
        public SqlHistoryFilesCfg historySqlFiles { get; }

        /// <summary>
        /// List of 10 last opened sql files
        /// </summary>
        public SqlLastOpenedFilesCfg lastOpenedFiles { get; }

        // Configuration 5 button searches by key
        public EaAddinButtons[] buttonsSearch;

        // Configuration 5 button services by key
        public List<hoTools.EaServices.ServicesCallConfig> buttonsServices;
        // all available services
        public List<hoTools.EaServices.ServiceCall> allServices = new List<hoTools.EaServices.ServiceCall>();

        public List<GlobalKeysConfig.GlobalKeysServiceConfig> globalShortcutsService = new List<GlobalKeysConfig.GlobalKeysServiceConfig>();
        public List<GlobalKeysConfig.GlobalKeysSearchConfig> globalShortcutsSearch = new  List<GlobalKeysConfig.GlobalKeysSearchConfig>();

        // Connectors
        public LogicalConnectors _logicalConnectors = new LogicalConnectors();
        public ActivityConnectors _activityConnectors = new ActivityConnectors();

        /// <summary>
        /// Configuration delivered with the installation in the install directory
        /// <para/>c:\Users\<user>\AppData\Local\Apps\hoTools\ActiveX.dll.config
        /// </summary>
        protected Configuration _defaultConfig { get; set; }

        /// <summary>
        /// Configuration stored in Roaming of the user
        /// <para/>c:\Users\<user>\AppData\Roaming\ho\hoTools\user.config
        /// </summary>
        protected Configuration _currentConfig { get; set; }
        #region Constructor
        /// <summary>
        /// Merge default settings (install DLLs) with current settings (user.config)
        /// Read settings from %APPDATA%\ho\hoTools\user.config or
        ///                    %APPDATA%\ho\hoTools_ZFLT\user.config
        /// </summary>
        public AddinSettings()
        {
            getDefaultSettings();

            Configuration roamingConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);

            //the roamingConfig now get a path such as C:\Users\<user>\AppData\Roaming\Sparx_Systems_Pty_Ltd\DefaultDomain_Path_2epjiwj3etsq5yyljkyqqi2yc4elkrkf\9,_2,_0,_921\user.config
            // which I don't like. So we move up three directories and then add a directory for the EA Navigator so that we get
            // C:\Users\<user>\AppData\Roaming\ho\hoTools\user.config
            string configFileName = System.IO.Path.GetFileName(roamingConfig.FilePath);
            string configDirectory = System.IO.Directory.GetParent(roamingConfig.FilePath).Parent.Parent.Parent.FullName;
            string path = "";
            switch (Customer) {
                case CustomerCfg.hoTools:
                path =  @"\ho\hoTools\";
                break;
                case CustomerCfg.VAR1:
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
            _currentConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);


            //merge the default settings
            // - For simple values that's all to do
            //   they uses Getter/Setter, no special handling here
            mergeDefaultSettings();

            // get list from config
            // for simple values nothing is to do here (there exists only a getter/setter)
            buttonsSearch = getShortcutsSearch();
            buttonsServices = getShortcutsServices();
            globalShortcutsService = getGlobalShortcutsService();
            globalShortcutsSearch = getGlobalShortcutsSearch();
            getConnector(_logicalConnectors);
            getConnector(_activityConnectors);
            getAllServices();
            historySqlFiles = new SqlHistoryFilesCfg(_currentConfig);// history of sql files 
            lastOpenedFiles = new SqlLastOpenedFilesCfg(_currentConfig); // last opened files

            // update lists
            updateSearchesAndServices();

            //-------------------------------------------
            // Simple values uses Getter/Setter, no special handling here
        }
        #endregion

        #region getDefaultSettings
        /// <summary>
        /// gets the default settings config.
        /// </summary>
        protected void getDefaultSettings()
        {
            string defaultConfigFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            _defaultConfig = ConfigurationManager.OpenExeConfiguration(defaultConfigFilePath);
        }
        #endregion
        #region mergeDefaultSettings
        /// <summary>
        /// merge the default settings with the current config.
        /// </summary>
        protected void mergeDefaultSettings()
        {
            //defaultConfig.AppSettings.Settings["menuOwnerEnabled"].Value
            if (_defaultConfig.AppSettings.Settings.Count == 0)
            {
                MessageBox.Show("No default settings in '" + _defaultConfig.FilePath + "' found!", "Installation wasn't successful!");
            }
            foreach (KeyValueConfigurationElement configEntry in _defaultConfig.AppSettings.Settings)
            {
                if (!_currentConfig.AppSettings.Settings.AllKeys.Contains(configEntry.Key))
                {
                    _currentConfig.AppSettings.Settings.Add(configEntry.Key, configEntry.Value);
                }
            }
            // save the configuration
            _currentConfig.Save();
        }
        #endregion


        #region Properties


        #region Property: isAskForQueryUpdateOutside
        public bool isAskForQueryUpdateOutside
        {
            get
            {
                return getBoolConfigValue("isAskForQueryUpdateOutside");
            }
            set
            {
                setBoolConfigValue("isAskForQueryUpdateOutside", value);
            }
        }
        #endregion

        #region Property: SqlEditor
        public string SqlEditor
        {
            get
            {
                return getStringConfigValue("SqlEditor");
            }
            set
            {
                setStringConfigValue("SqlEditor", value);
            }
        }

      


        #region Property: isLineStyleSupport
        public bool isLineStyleSupport
        {
            get
            {
                bool result;
                var p = _currentConfig.AppSettings.Settings["isLineStyleSupport"];
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
                _currentConfig.AppSettings.Settings["isLineStyleSupport"].Value = value.ToString();
            }
        }
        #endregion

        #region Property: isShortKeySupport
        public bool isShortKeySupport
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isShortKeySupport"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isShortKeySupport"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isShowServiceButton
        public bool isShowServiceButton
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isShowServiceButton"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isShowServiceButton"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isShowQueryButton
        public bool isShowQueryButton
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isShowQueryButton"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isShowQueryButton"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isFavoriteSupport
        public bool isFavoriteSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isFavoriteSupport"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isFavoriteSupport"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isConveyedItemsSupport
        public bool isConveyedItemsSupport
        {
            get
            {
                return getBoolConfigValue("isConveyedItemsSupport");
            }
            set
            {
                setBoolConfigValue("isConveyedItemsSupport", value);
            }
        }
        #endregion





        #region CustomerCfg (possible customer) 
        /// <summary>
        /// To define different customer
        /// </summary>
        public enum CustomerCfg
        {
            VAR1,
            hoTools
        }
        #endregion
        #region Property: isAdvancedFeatures
        public bool isAdvancedFeatures
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isAdvancedFeatures"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isAdvancedFeatures"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isAdvancedPort

        public bool isAdvancedPort
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isAdvancedPort"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isAdvancedPort"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: isAdvancedDiagramNote

        public bool isAdvancedDiagramNote
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isAdvancedDiagramNote"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isAdvancedDiagramNote"].Value = value.ToString();

            }

        }
        #endregion


        #region Property: ScriptAndQueryWindow
        public ShowInWindow ScriptAndQueryWindow
        {
            get
            {
                ShowInWindow result;
                if (Enum.TryParse<ShowInWindow>(_currentConfig.AppSettings.Settings["ScriptAndQueryWindow"].Value, out result))
                {
                    return (ShowInWindow)result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["ScriptAndQueryWindow"].Value = value.ToString();

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
                if (Enum.TryParse<AutoLoadMdg>(_currentConfig.AppSettings.Settings["AutoLoadMdg"].Value, out result))
                {
                    return (AutoLoadMdg)result;
                }
                else
                {
                    return AutoLoadMdg.No;
                }
            }
            set
            {
                if (_currentConfig.AppSettings.Settings["AutoLoadMdg"] != null)
                {
                    _currentConfig.AppSettings.Settings["AutoLoadMdg"].Value = value.ToString();
                } else
                {
                    messageConfigValueNotExists("AutoLoadMdg", value.ToString());
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
                if (Enum.TryParse<ShowInWindow>(_currentConfig.AppSettings.Settings["OnlyQueryWindow"].Value, out result))
                {
                    return (ShowInWindow)result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                if (_currentConfig.AppSettings.Settings["OnlyQueryWindow"] != null)
                {
                    _currentConfig.AppSettings.Settings["OnlyQueryWindow"].Value = value.ToString();
                }
                else
                {
                    messageConfigValueNotExists("OnlyQueryWindow", value.ToString());
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
                if (Enum.TryParse<ShowInWindow>(_currentConfig.AppSettings.Settings["SearchAndReplaceWindow"].Value, out result))
                {
                    return (ShowInWindow)result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["SearchAndReplaceWindow"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: LineStyleAndMoreWindow
        public ShowInWindow LineStyleAndMoreWindow
        {
            get
            {
                ShowInWindow result;
                if (Enum.TryParse<ShowInWindow>(_currentConfig.AppSettings.Settings["LineStyleAndMoreWindow"].Value, out result))
                {
                    return (ShowInWindow)result;
                }
                else
                {
                    return ShowInWindow.Disabled;
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["LineStyleAndMoreWindow"].Value = value.ToString();

            }

        }
        #endregion

        #region Property: isVcSupport

        public bool isVcSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isVcSupport"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isVcSupport"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: isSvnSupport
        
        public bool isSvnSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(_currentConfig.AppSettings.Settings["isSvnSupport"].Value, out result))
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
                _currentConfig.AppSettings.Settings["isSvnSupport"].Value = value.ToString();

            }

        }
        #endregion
        
        
        #region Property: FileManagerPath
        public string FileManagerPath
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["FileManagerPath"].Value == null)
                {
                    return "FileManagerPath";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["FileManagerPath"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["FileManagerPath"].Value = value;

            }
        }
        #endregion
        #region Property: quickSearchName
        public string quickSearchName
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["QuickSearchName"].Value == null) {
                    return "Quick Search";
                }else {
                return (_currentConfig.AppSettings.Settings["QuickSearchName"].Value );
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["QuickSearchName"].Value =value;
               
            }
        }
        #endregion
        #region Property: sqlFolder
        /// <summary>
        /// Folder for SQL Searches
        /// </summary>
        public string sqlFolder
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["SqlFolder"].Value == null)
                {
                    return @"c:\temp\sql";
                }
                else {
                    return (_currentConfig.AppSettings.Settings["SqlFolder"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["SqlFolder"].Value = value;

            }
        }
        #endregion

        #region Property: productName
        public string productName
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["ProductName"].Value == null)
                {
                    return "hoTools";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["ProductName"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["ProductName"].Value = value;

            }
        }
        #endregion
        #region Property: ActivityLineStyle
        public string ActivityLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["ActivityLineStyle"].Value == null)
                {
                    return "LV";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["ActivityLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["ActivityLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: StatechartLineStyle
        public string StatechartLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["StatechartLineStyle"].Value == null)
                {
                    return "B";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["StatechartLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["StatechartLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: CustomLineStyle
        public string CustomLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["CustomLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["CustomLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["CustomLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: ClassLineStyle
        public string ClassLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["ClassLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["ClassLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["ClassLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: PackageLineStyle
        public string PackageLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["PackageLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["PackageLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["PackageLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: UseCaseLineStyle
        public string UseCaseLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["UseCaseLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["UseCaseLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["UseCaseLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: DeploymentLineStyle
        public string DeploymentLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["DeploymentLineStyle"].Value == null)
                {
                    return "B";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["DeploymentLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["DeploymentLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: CompositeStructureLineStyle
        public string CompositeStructureLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: ComponentLineStyle
        public string ComponentLineStyle
        {
            get
            {
                if (_currentConfig.AppSettings.Settings["ComponentLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (_currentConfig.AppSettings.Settings["ComponentLineStyle"].Value);
                }
            }
            set
            {
                _currentConfig.AppSettings.Settings["ComponentLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: Customer

        public CustomerCfg Customer
        {
            get
            {
                // get customer
                var s = _defaultConfig.AppSettings.Settings["Customer"];
                if (s == null)  return CustomerCfg.hoTools;
                switch (s.Value)
                {
                    case "F52AB09A-8ED0-4159-9AB4-FFD986983280":
                        return CustomerCfg.VAR1;
                        
                    case "14F09211-3460-47A6-B837-A477491F0A67":
                        return CustomerCfg.hoTools;
                    case "4661B136-39E8-45B9-9015-C6C18A04EDCF":
                        return CustomerCfg.hoTools;
                    case "7A415B23-7D1E-49CB-84BA-BAC1C5BFD8FE":
                        return CustomerCfg.hoTools;
                    default:
                        return CustomerCfg.hoTools;

                }
                
            }
            

        }
        #endregion
        #endregion


        #region save
        /// <summary>
        /// saves the settings to the config file
        /// </summary>
        public void save()
        {
            try
            {
                setShortcuts(buttonsSearch);
                setServices(buttonsServices);
                setGlobalShortcutsSearch(globalShortcutsSearch);
                setGlobalShortcutsService(globalShortcutsService);

                setConnector(_logicalConnectors);
                setConnector(_activityConnectors);
                historySqlFiles.save();
                lastOpenedFiles.save();
                _currentConfig.Save();
                _currentConfig.Save();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error storing user.config");
            }
        }
#endregion
        #region refresh
        public void refresh()
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = _currentConfig.FilePath;
            // Get the mapped configuration file.
            _currentConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }
        #endregion
        #region setShortcuts
        public void setShortcuts(EaAddinButtons[] l)
        {
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] == null) continue;
                if (l[i] is EaAddinShortcutSearch)
                {

                    var el = (EaAddinShortcutSearch)l[i];
                    string basicKey = "Key" + el.keyPos;
                    _currentConfig.AppSettings.Settings[basicKey + "Text"].Value = el.keyText;
                    _currentConfig.AppSettings.Settings[basicKey + "Type"].Value = "Search";
                    _currentConfig.AppSettings.Settings[basicKey + "Par1"].Value = el.keySearchName;
                    _currentConfig.AppSettings.Settings[basicKey + "Par2"].Value = el.keySearchTerm;
                    _currentConfig.AppSettings.Settings[basicKey + "Tooltip"].Value = el.keySearchTooltip;
                }
            }

        }
        #endregion
        #region getShortcutsSearch
        private EaAddinButtons[] getShortcutsSearch()
        {
            int pos = 0;
            int posValue = 0;
            string text = "";
            string type = "";
            string par1 = "";
            string par2 = "";
            string sKey = "";
            EaAddinButtons[] l = new EaAddinButtons[10];
            foreach (KeyValueConfigurationElement configEntry in _currentConfig.AppSettings.Settings)
            {
                sKey = configEntry.Key;
                string regex = @"key([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    posValue = Convert.ToInt16(match.Groups[1].Value);
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
        private List<hoTools.EaServices.ServicesCallConfig> getShortcutsServices()
        {
            var l = new List<hoTools.EaServices.ServicesCallConfig>();
            int pos = 0;
            string sKey = "";
            string text = "";
            string GUID = "";
            foreach (KeyValueConfigurationElement configEntry in _currentConfig.AppSettings.Settings)
            {
                sKey = configEntry.Key;
                string regex = @"service([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    pos = Convert.ToInt16(match.Groups[1].Value);
                    switch (match.Groups[2].Value)
                    {
                        case "GUID":
                            GUID = configEntry.Value;
                            break;
                        case "Text":
                            text = configEntry.Value;
                            l.Add(new hoTools.EaServices.ServicesCallConfig(pos, GUID, text));
                            break;
                    }
                }
            }
            return l;

        }
        #endregion
        #region getAllServices
        // get all possible services
        private void getAllServices()
        {
            ServiceOperationAttribute ServiceOperation;
            Type type = typeof(EaService);
            allServices.Add(new hoTools.EaServices.ServiceCall(null, "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}", "-- no --", "no service selected", false));
            foreach (MethodInfo method in type.GetMethods())
            {

                foreach (Attribute attr in method.GetCustomAttributes(true))
                {
                    ServiceOperation = attr as ServiceOperationAttribute;
                    if (null != ServiceOperation)
                    {
                        allServices.Add(new hoTools.EaServices.ServiceCall(method, ServiceOperation.GUID, ServiceOperation.Description, ServiceOperation.Help, ServiceOperation.IsTextRequired));
                    }
                }
            }
            allServices.Sort(new hoTools.EaServices.ServicesCallDescriptionComparer());
            return;
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
        public void updateSearchesAndServices()
        {
            foreach (ServicesCallConfig service in buttonsServices)
            {
                if (service.GUID != "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}")
                {
                    //int index = allServices.BinarySearch(new EaServices.ServiceCall(null, service.GUID, "","", false), new EaServices.ServicesCallGUIDComparer());
                    foreach (hoTools.EaServices.ServiceCall s in allServices) {
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
            foreach (GlobalKeysConfig.GlobalKeysServiceConfig service in globalShortcutsService)
            {
                if (service.GUID != "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}")
                {
                    //int index = allServices.BinarySearch(new EaServices.ServiceCall(null, service.GUID, "","", false), new EaServices.ServicesCallGUIDComparer());
                    foreach (hoTools.EaServices.ServiceCall s in allServices)
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
        private List<GlobalKeysConfig.GlobalKeysServiceConfig> getGlobalShortcutsService()
         {
             var l = new List<GlobalKeysConfig.GlobalKeysServiceConfig>();
             int pos = 0;
             string sKey = "";
             string key = "";
             string Modifier1 = "";
             string Modifier2 = "";
             string Modifier3 = "";
             string Modifier4 = "";
             string GUID = "";

             foreach (KeyValueConfigurationElement configEntry in _currentConfig.AppSettings.Settings)
             {
                 sKey = configEntry.Key;
                 string regex = @"globalKeyService([0-9]+)([a-zA-Z_0-9]+)";
                 Match match = Regex.Match(sKey, regex);
                 if (match.Success)
                 {
                     pos = Convert.ToInt16(match.Groups[1].Value);
                     switch (match.Groups[2].Value)
                     {
                         case "Key":
                             key = configEntry.Value;
                             break;
                         case "Modifier1":
                             Modifier1 = configEntry.Value;
                             break;
                         case "Modifier2":
                             Modifier2 = configEntry.Value;
                             break;
                         case "Modifier3":
                             Modifier3 = configEntry.Value;
                             break;
                         case "Modifier4":
                             Modifier4 = configEntry.Value;
                             break;
                         case "GUID":
                             GUID = configEntry.Value;
                             l.Add(new GlobalKeysConfig.GlobalKeysServiceConfig(key,Modifier1,Modifier2,Modifier3,Modifier4,"", GUID, "",false));
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
        private List<GlobalKeysConfig.GlobalKeysSearchConfig> getGlobalShortcutsSearch()
        {
            var l = new List<GlobalKeysConfig.GlobalKeysSearchConfig>();
            int pos = 0;
            string sKey = "";
            string key = "";
            string Modifier1 = "";
            string Modifier2 = "";
            string Modifier3 = "";
            string Modifier4 = "";
            string SearchName = "";
            string SearchTerm = "";
            string Tooltip = "";

            foreach (KeyValueConfigurationElement configEntry in _currentConfig.AppSettings.Settings)
            {
                sKey = configEntry.Key;
                string regex = @"globalKeySearch([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    pos = Convert.ToInt16(match.Groups[1].Value);
                    switch (match.Groups[2].Value)
                    {
                        case "Key":
                            key = configEntry.Value;
                            break;
                        case "Modifier1":
                            Modifier1 = configEntry.Value;
                            break;
                        case "Modifier2":
                            Modifier2 = configEntry.Value;
                            break;
                        case "Modifier3":
                            Modifier3 = configEntry.Value;
                            break;
                        case "Modifier4":
                            Modifier4 = configEntry.Value;
                            break;
                        case "SearchName":
                            SearchName = configEntry.Value;
                            break;
                        case "SearchTerm":
                            SearchTerm = configEntry.Value;
                            break;
                        case "Tooltip":
                            Tooltip = configEntry.Value;

                            l.Add(new GlobalKeysConfig.GlobalKeysSearchConfig(key, Modifier1, Modifier2, Modifier3, Modifier4, Tooltip, 
                                SearchName, SearchTerm));
                            break;
                    }
                }
            }
            return l;
        }
        #endregion
        #region setGlobalShortcutsSearch
        public void setGlobalShortcutsSearch(List<GlobalKeysConfig.GlobalKeysSearchConfig> l) {
            for (int i = 0; i< l.Count;i++)
            {
                if (l[i] == null) continue;
                    GlobalKeysConfig.GlobalKeysSearchConfig el = l[i];
                    string basicKey = "globalKeySearch" + (i+1);
                    _currentConfig.AppSettings.Settings[basicKey + "Key"].Value = el.Key;
                    _currentConfig.AppSettings.Settings[basicKey + "Modifier1"].Value = el.Modifier1;
                    _currentConfig.AppSettings.Settings[basicKey + "Modifier2"].Value = el.Modifier2;
                    _currentConfig.AppSettings.Settings[basicKey + "Modifier3"].Value = el.Modifier3;
                    _currentConfig.AppSettings.Settings[basicKey + "Modifier4"].Value = el.Modifier4;
                    _currentConfig.AppSettings.Settings[basicKey + "SearchName"].Value = el.SearchName;
                    _currentConfig.AppSettings.Settings[basicKey + "SearchTerm"].Value = el.SearchTerm;
                    _currentConfig.AppSettings.Settings[basicKey + "Tooltip"].Value = el.Tooltip;
            }

        }
        #endregion
        #region setGlobalShortcutsService
        public void setGlobalShortcutsService(List<GlobalKeysConfig.GlobalKeysServiceConfig> l)
        {
            
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                GlobalKeysConfig.GlobalKeysServiceConfig el = l[i];
                string basicKey = "globalKeyService" + (i+1);
                _currentConfig.AppSettings.Settings[basicKey + "Key"].Value = el.Key;
                _currentConfig.AppSettings.Settings[basicKey + "Modifier1"].Value = el.Modifier1;
                _currentConfig.AppSettings.Settings[basicKey + "Modifier2"].Value = el.Modifier2;
                _currentConfig.AppSettings.Settings[basicKey + "Modifier3"].Value = el.Modifier3;
                _currentConfig.AppSettings.Settings[basicKey + "Modifier4"].Value = el.Modifier4;
                _currentConfig.AppSettings.Settings[basicKey + "GUID"].Value = el.GUID;
            }

        }
        #endregion
        #region getConnector
        public void getConnector(DiagramConnector l)
        {
            string DiagramType = l.DiagramType;
            int pos = 0;
            string sKey = "";
            string type = "";
            string lineStyle = "";
            string stereotype = "";
            bool isDefault = false;
            bool isEnabled = false;

            foreach (KeyValueConfigurationElement configEntry in _currentConfig.AppSettings.Settings)
            {
                sKey = configEntry.Key;
                string regex = DiagramType +"Connector([0-9]+)([a-zA-Z_0-9]+)";
                Match match = Regex.Match(sKey, regex);
                if (match.Success)
                {
                    pos = Convert.ToInt16(match.Groups[1].Value);
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
                            isEnabled = false;
                            isEnabled |= configEntry.Value == "True";
                            l.Add(new Connector(type, stereotype, lineStyle, isDefault, isEnabled));
                            break;
                    }
                }
            }
            return;
            
        }
        #endregion
        #region setConnector
        public void setConnector(DiagramConnector l)
        {

            string DiagramType = l.DiagramType;
            string basicKey;
            string key;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                Connector el = l[i];
                basicKey = DiagramType + "Connector" + (i + 1);

                key = basicKey+ "Type";
                if (! _currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                _currentConfig.AppSettings.Settings.Add(key, el.Type); 
                else  _currentConfig.AppSettings.Settings[key].Value = el.Type; 

                key = basicKey + "Stereotype";
                if (! _currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    _currentConfig.AppSettings.Settings.Add(key, el.Stereotype);
                else _currentConfig.AppSettings.Settings[key].Value = el.Stereotype;

                key = basicKey + "LineStyle";
                if (!_currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    _currentConfig.AppSettings.Settings.Add(key, el.LineStyle);
                else _currentConfig.AppSettings.Settings[key].Value = el.LineStyle;


                key = basicKey + "IsDefault";
                if (! _currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    _currentConfig.AppSettings.Settings.Add(key, el.IsDefault.ToString());
                else _currentConfig.AppSettings.Settings[key].Value = el.IsDefault.ToString();

                key = basicKey + "IsEnabled";
                if (! _currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    _currentConfig.AppSettings.Settings.Add(key, el.IsEnabled.ToString());
                else _currentConfig.AppSettings.Settings[key].Value = el.IsEnabled.ToString();
                               
            }
            // delete unused entries
            int index = l.Count +1;
            while (true)
            {
            basicKey = DiagramType + "Connector" + index;
            if (_currentConfig.AppSettings.Settings.AllKeys.Contains(basicKey+"Type"))
            {
                _currentConfig.AppSettings.Settings.Remove(basicKey + "IsEnabled");
                _currentConfig.AppSettings.Settings.Remove(basicKey + "IsDefault");
                _currentConfig.AppSettings.Settings.Remove(basicKey + "Stereotype");
                _currentConfig.AppSettings.Settings.Remove(basicKey + "Type");
                index = index + 1;
            }

            else {break;}
            }

        }
        #endregion
        #region setServices
        public void setServices(List<hoTools.EaServices.ServicesCallConfig> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                if (l[i] is hoTools.EaServices.ServicesCallConfig)
                {

                    var el = (hoTools.EaServices.ServicesCallConfig)l[i];
                    string basicKey = "service" + (i + 1);
                    _currentConfig.AppSettings.Settings[basicKey + "GUID"].Value = el.GUID;
                    _currentConfig.AppSettings.Settings[basicKey + "Text"].Value = el.ButtonText;
                }
            }

        }
        #endregion


        /// <summary>
        /// Get bool config value. If the value don't exists return a false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool getBoolConfigValue(string name)
        {
            bool result;
            var p = _currentConfig.AppSettings.Settings[name];
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
        /// <param name="value"></param>
        void setBoolConfigValue(string name, bool value)
        {
            var cfgValue = _currentConfig.AppSettings.Settings[name];
            if (cfgValue != null)
            {
                cfgValue.Value = value.ToString();
            }else
            {
                messageConfigValueNotExists(name, value.ToString());
            }
        }
        #endregion
        /// <summary>
        /// Get string value from configuration
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string getStringConfigValue(string name)
        {
            var p = _currentConfig.AppSettings.Settings[name];
            if (p == null) return "";// default
            return p.Value;
        }
        /// <summary>
        /// Set string value in configuration. If error output error message
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void setStringConfigValue(string name, string value)
        {
            var p = _currentConfig.AppSettings.Settings[name];
            if (p !=  null)
            {
                p.Value = value;
            }
            else
            {
                messageConfigValueNotExists(name, value);
            }
        }

        /// <summary>
        /// Output error Message box for not existent configuration parameter or invalid value
        /// </summary>
        /// <param name="name"></param>
        void messageConfigValueNotExists(string name, string value)
        {
            MessageBox.Show($"Parameter '{name}' with value '{value}' don't exists in configuration or is invalid!");
        }

        
    }
}

