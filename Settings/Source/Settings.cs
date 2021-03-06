﻿using System;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using EaServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Control.EaAddinShortcuts;
using System.Text.RegularExpressions;
using System.Text;
using GlobalHotkeys;
using Connectors;

namespace ho_Tools.Settings
{
    /// <summary>
    /// Merge default settings with current settings
    /// Default:   
    ///    Debug:   ..\Addin\ActiveX.dll.config   with copy to output directory
    ///    Release: DLL-Install library
    ///    Make sure to copy the default settings in project output (VS Properties)
    /// Read current setting
    /// Write current settings
    /// </summary>
/// 

    //----------------------------------------------------------------------------------
    // Maintaining of configuration settings in 
    // Current:   %appdata%\ho\ho_addin\user.config
    // Default:   ..\Addin\ActiveX.dll.config   with copy to output directory             
    //----------------------------------------------------------------------------------
    // Make sure to copy the default settings in project output.
    public class AddinSettings
    {
        // Configuration 5 shortcut searches by key
        public EaAddinShortcut[] shortcutsSearch = null;

        // Configuration 5 shortcut services by key
        public List<EaServices.ServicesCallConfig> shortcutsServices = null;
        // all available services
        public List<EaServices.ServiceCall> allServices = new List<EaServices.ServiceCall>();

        public List<GlobalKeysConfig.GlobalKeysServiceConfig> globalShortcutsService = new List<GlobalKeysConfig.GlobalKeysServiceConfig>();
        public List<GlobalKeysConfig.GlobalKeysSearchConfig> globalShortcutsSearch = new  List<GlobalKeysConfig.GlobalKeysSearchConfig>();

        // Connectors
        public LogicalConnectors _logicalConnectors = new LogicalConnectors();
        public ActivityConnectors _activityConnectors = new ActivityConnectors();


        protected Configuration defaultConfig { get; set; }
        protected Configuration currentConfig { get; set; }
        #region Constructor
        /// <summary>
        /// Merge default settings (install DLLs) with current settings (user.config)
        /// Read settings from %APPDATA%\ho\ho_tools\user.config
        /// </summary>
        public AddinSettings()
        {
            Configuration roamingConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);

            //the roamingConfig now get a path such as C:\Users\<user>\AppData\Roaming\Sparx_Systems_Pty_Ltd\DefaultDomain_Path_2epjiwj3etsq5yyljkyqqi2yc4elkrkf\9,_2,_0,_921\user.config
            // which I don't like. So we move up three directories and then add a directory for the EA Navigator so that we get
            // C:\Users\<user>\AppData\Roaming\GeertBellekens\EANavigator\user.config
            string configFileName = System.IO.Path.GetFileName(roamingConfig.FilePath);
            string configDirectory = System.IO.Directory.GetParent(roamingConfig.FilePath).Parent.Parent.Parent.FullName;

            string newConfigFilePath = configDirectory + @"\ho\ho_Tools\" + configFileName;
            // Map the roaming configuration file. This
            // enables the application to access 
            // the configuration file using the
            // System.Configuration.Configuration class
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = newConfigFilePath;
            // Get the mapped configuration file.
            currentConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            //merge the default settings
            this.mergeDefaultSettings();
            this.shortcutsSearch = getShortcutsSearch();
            this.shortcutsServices = getShortcutsServices();
            this.globalShortcutsService = getGlobalShortcutsService();
            this.globalShortcutsSearch = getGlobalShortcutsSearch();

            getConnector(_logicalConnectors);
            getConnector(_activityConnectors);
            getAllServices();
            updateSearchesAndServices();
        }
        #endregion

        /// <summary>
        /// gets the default settings config.
        /// </summary>
        protected void getDefaultSettings()
        {
            string defaultConfigFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            defaultConfig = ConfigurationManager.OpenExeConfiguration(defaultConfigFilePath);
        }
        /// <summary>
        /// merge the default settings with the current config.
        /// </summary>
        protected void mergeDefaultSettings()
        {
            if (this.defaultConfig == null)
            {
                this.getDefaultSettings();
            }
            //defaultConfig.AppSettings.Settings["menuOwnerEnabled"].Value
            if (defaultConfig.AppSettings.Settings.Count == 0)
            {
                MessageBox.Show("No default settings in '" + defaultConfig.FilePath + "' found!", "Installation wasn't successful!");
            }
            foreach (KeyValueConfigurationElement configEntry in defaultConfig.AppSettings.Settings)
            {
                if (!currentConfig.AppSettings.Settings.AllKeys.Contains(configEntry.Key))
                {
                    currentConfig.AppSettings.Settings.Add(configEntry.Key, configEntry.Value);
                }
            }
            // save the configuration
            currentConfig.Save();
        }
        //public bool isOptionEnabled(UML.UMLItem parentElement, string option)
        //{
        //    //default
        //    return true;
        //}
        /// <summary>
        /// returns true when the selecting an element in the project browser is the default action on doubleclick.
        /// </summary>
        /// <returns>true when the selecting an element in the project browser is the default action on doubleclick.</returns>
#region Properties

        #region Property: isAdvancedFeatures
        public bool isAdvancedFeatures
        {
            get
            {
                bool result;
                if (bool.TryParse(this.currentConfig.AppSettings.Settings["isAdvancedFeatures"].Value, out result))
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
                this.currentConfig.AppSettings.Settings["isAdvancedFeatures"].Value = value.ToString();
            }
        }
        #endregion
        #region Property: isVcSupport
        
        public bool isVcSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(this.currentConfig.AppSettings.Settings["isVcSupport"].Value, out result))
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
                this.currentConfig.AppSettings.Settings["isVcSupport"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: isSvnSupport
        
        public bool isSvnSupport
        {
            get
            {
                bool result;
                if (bool.TryParse(this.currentConfig.AppSettings.Settings["isSvnSupport"].Value, out result))
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
                this.currentConfig.AppSettings.Settings["isSvnSupport"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: isSearchAndReplace
        
        public bool isSearchAndReplace
        {
            get
            {
                bool result;
                if (bool.TryParse(this.currentConfig.AppSettings.Settings["isSearchAndReplace"].Value, out result))
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
                this.currentConfig.AppSettings.Settings["isSearchAndReplace"].Value = value.ToString();

            }

        }
        #endregion
        #region Property: FileManagerPath
        public string FileManagerPath
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["FileManagerPath"].Value == null)
                {
                    return "FileManagerPath";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["FileManagerPath"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["FileManagerPath"].Value = value;

            }
        }
        #endregion
        #region Property: quickSearchName
        public string quickSearchName
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["QuickSearchName"].Value == null) {
                    return "Quick Search";
                }else {
                return (this.currentConfig.AppSettings.Settings["QuickSearchName"].Value );
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["QuickSearchName"].Value =value;
               
            }
        }
        #endregion
        #region Property: productName
        public string productName
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["ProductName"].Value == null)
                {
                    return "ho_Tools";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["ProductName"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["ProductName"].Value = value;

            }
        }
        #endregion
        #region Property: ActivityLineStyle
        public string ActivityLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["ActivityLineStyle"].Value == null)
                {
                    return "LV";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["ActivityLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["ActivityLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: StatechartLineStyle
        public string StatechartLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["StatechartLineStyle"].Value == null)
                {
                    return "B";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["StatechartLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["StatechartLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: CustomLineStyle
        public string CustomLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["CustomLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["CustomLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["CustomLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: ClassLineStyle
        public string ClassLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["ClassLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["ClassLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["ClassLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: PackageLineStyle
        public string PackageLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["PackageLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["PackageLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["PackageLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: UseCaseLineStyle
        public string UseCaseLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["UseCaseLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["UseCaseLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["UseCaseLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: DeploymentLineStyle
        public string DeploymentLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["DeploymentLineStyle"].Value == null)
                {
                    return "B";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["DeploymentLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["DeploymentLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: CompositeStructureLineStyle
        public string CompositeStructureLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["CompositeStructureLineStyle"].Value = value;

            }
        }
        #endregion
        #region Property: ComponentLineStyle
        public string ComponentLineStyle
        {
            get
            {
                if (this.currentConfig.AppSettings.Settings["ComponentLineStyle"].Value == null)
                {
                    return "no";
                }
                else
                {
                    return (this.currentConfig.AppSettings.Settings["ComponentLineStyle"].Value);
                }
            }
            set
            {
                this.currentConfig.AppSettings.Settings["ComponentLineStyle"].Value = value;

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
                this.setShortcuts(shortcutsSearch);
                this.setServices(shortcutsServices);
                this.setGlobalShortcutsSearch(globalShortcutsSearch);
                this.setGlobalShortcutsService(globalShortcutsService);

                this.setConnector(_logicalConnectors);
                this.setConnector(_activityConnectors);
                this.currentConfig.Save();



                this.currentConfig.Save();
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
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = currentConfig.FilePath;
            // Get the mapped configuration file.
            currentConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }
        #endregion
        #region setShortcuts
        public void setShortcuts(EaAddinShortcut[] l)
        {
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] == null) continue;
                if (l[i] is EaAddinShortcutSearch)
                {

                    EaAddinShortcutSearch el = (EaAddinShortcutSearch)l[i];
                    string basicKey = "Key" + el.keyPos;
                    this.currentConfig.AppSettings.Settings[basicKey + "Text"].Value = el.keyText;
                    this.currentConfig.AppSettings.Settings[basicKey + "Type"].Value = "Search";
                    this.currentConfig.AppSettings.Settings[basicKey + "Par1"].Value = el.keySearchName;
                    this.currentConfig.AppSettings.Settings[basicKey + "Par2"].Value = el.keySearchTerm;
                    this.currentConfig.AppSettings.Settings[basicKey + "Tooltip"].Value = el.keySearchTooltip;
                }
            }

        }
        #endregion
        #region getShortcutsSearch
        private EaAddinShortcut[] getShortcutsSearch()
        {
            int pos = 0;
            int posValue = 0;
            string text = "";
            string type = "";
            string par1 = "";
            string par2 = "";
            string sKey = "";
            EaAddinShortcut[] l = new EaAddinShortcut[10];
            foreach (KeyValueConfigurationElement configEntry in currentConfig.AppSettings.Settings)
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
        private List<EaServices.ServicesCallConfig> getShortcutsServices()
        {
            List<EaServices.ServicesCallConfig> l = new List<EaServices.ServicesCallConfig>();
            int pos = 0;
            string sKey = "";
            string text = "";
            string GUID = "";
            foreach (KeyValueConfigurationElement configEntry in currentConfig.AppSettings.Settings)
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
                            l.Add(new EaServices.ServicesCallConfig(pos, GUID, text));
                            break;
                    }
                }
            }
            return l;

        }
        #endregion
        #region getAllServices
        // get all possible sevrices
        private void getAllServices()
        {
            ServiceOperationAttribute ServiceOperation;
            Type type = typeof(EaService);
            allServices.Add(new EaServices.ServiceCall(null, "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}", "-- no --", "no service selected", false));
            foreach (MethodInfo method in type.GetMethods())
            {

                foreach (Attribute attr in method.GetCustomAttributes(true))
                {
                    ServiceOperation = attr as ServiceOperationAttribute;
                    if (null != ServiceOperation)
                    {
                        allServices.Add(new EaServices.ServiceCall(method, ServiceOperation.GUID, ServiceOperation.Description, ServiceOperation.Help, ServiceOperation.IsTextRequired));
                    }
                }
            }
            allServices.Sort(new EaServices.ServicesCallDescriptionComparer());
            return;
        }
        #endregion
        #region updateSearchesAndServices
        /// <summary>
        /// Update the services & searches for:
        /// - Buttons
        /// - Global keys / global shortcuts / keyboard keys
        /// by 
        /// - Method
        /// - Tooltip
        /// </summary>
        public void updateSearchesAndServices()
        {
            foreach (ServicesCallConfig service in shortcutsServices)
            {
                if (service.GUID != "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}")
                {
                    //int index = allServices.BinarySearch(new EaServices.ServiceCall(null, service.GUID, "","", false), new EaServices.ServicesCallGUIDComparer());
                    foreach (EaServices.ServiceCall s in allServices) {
                        if (service.GUID == s.GUID)
                        {
                            service.Method = s.Method;
                            service.Help = s.Help;
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
                    foreach (EaServices.ServiceCall s in allServices)
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
             List<GlobalKeysConfig.GlobalKeysServiceConfig> l = new List<GlobalKeysConfig.GlobalKeysServiceConfig>();
             int pos = 0;
             string sKey = "";
             string key = "";
             string Modifier1 = "";
             string Modifier2 = "";
             string Modifier3 = "";
             string Modifier4 = "";
             string GUID = "";

             foreach (KeyValueConfigurationElement configEntry in currentConfig.AppSettings.Settings)
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
            List<GlobalKeysConfig.GlobalKeysSearchConfig> l = new List<GlobalKeysConfig.GlobalKeysSearchConfig>();
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

            foreach (KeyValueConfigurationElement configEntry in currentConfig.AppSettings.Settings)
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
                    string basicKey = "globalKeySearch" + (i+1).ToString();
                    this.currentConfig.AppSettings.Settings[basicKey + "Key"].Value = el.Key;
                    this.currentConfig.AppSettings.Settings[basicKey + "Modifier1"].Value = el.Modifier1;
                    this.currentConfig.AppSettings.Settings[basicKey + "Modifier2"].Value = el.Modifier2;
                    this.currentConfig.AppSettings.Settings[basicKey + "Modifier3"].Value = el.Modifier3;
                    this.currentConfig.AppSettings.Settings[basicKey + "Modifier4"].Value = el.Modifier4;
                    this.currentConfig.AppSettings.Settings[basicKey + "SearchName"].Value = el.SearchName;
                    this.currentConfig.AppSettings.Settings[basicKey + "SearchTerm"].Value = el.SearchTerm;
                    this.currentConfig.AppSettings.Settings[basicKey + "Tooltip"].Value = el.Tooltip;
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
                string basicKey = "globalKeyService" + (i+1).ToString();
                this.currentConfig.AppSettings.Settings[basicKey + "Key"].Value = el.Key;
                this.currentConfig.AppSettings.Settings[basicKey + "Modifier1"].Value = el.Modifier1;
                this.currentConfig.AppSettings.Settings[basicKey + "Modifier2"].Value = el.Modifier2;
                this.currentConfig.AppSettings.Settings[basicKey + "Modifier3"].Value = el.Modifier3;
                this.currentConfig.AppSettings.Settings[basicKey + "Modifier4"].Value = el.Modifier4;
                this.currentConfig.AppSettings.Settings[basicKey + "GUID"].Value = el.GUID;
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

            foreach (KeyValueConfigurationElement configEntry in currentConfig.AppSettings.Settings)
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
                            if (configEntry.Value == "True") isDefault = true;

                            break;
                        case "IsEnabled":
                            isEnabled = false;
                            if (configEntry.Value == "True") isEnabled = true;
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
                basicKey = DiagramType + "Connector" + (i + 1).ToString();

                key = basicKey+ "Type";
                if (! this.currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                this.currentConfig.AppSettings.Settings.Add(key, el.Type); 
                else  this.currentConfig.AppSettings.Settings[key].Value = el.Type; 

                key = basicKey + "Stereotype";
                if (! this.currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    this.currentConfig.AppSettings.Settings.Add(key, el.Stereotype);
                else this.currentConfig.AppSettings.Settings[key].Value = el.Stereotype;

                key = basicKey + "LineStyle";
                if (!this.currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    this.currentConfig.AppSettings.Settings.Add(key, el.LineStyle);
                else this.currentConfig.AppSettings.Settings[key].Value = el.LineStyle;


                key = basicKey + "IsDefault";
                if (! this.currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    this.currentConfig.AppSettings.Settings.Add(key, el.IsDefault.ToString());
                else this.currentConfig.AppSettings.Settings[key].Value = el.IsDefault.ToString();

                key = basicKey + "IsEnabled";
                if (! this.currentConfig.AppSettings.Settings.AllKeys.Contains(key))
                    this.currentConfig.AppSettings.Settings.Add(key, el.IsEnabled.ToString());
                else this.currentConfig.AppSettings.Settings[key].Value = el.IsEnabled.ToString();
                               
            }
            // delete unused entries
            int index = l.Count +1;
            while (true)
            {
            basicKey = DiagramType + "Connector" + index.ToString();
            if (this.currentConfig.AppSettings.Settings.AllKeys.Contains(basicKey+"Type"))
            {
                this.currentConfig.AppSettings.Settings.Remove(basicKey + "IsEnabled");
                this.currentConfig.AppSettings.Settings.Remove(basicKey + "IsDefault");
                this.currentConfig.AppSettings.Settings.Remove(basicKey + "Stereotype");
                this.currentConfig.AppSettings.Settings.Remove(basicKey + "Type");
                index = index + 1;
            }

            else {break;}
            }

        }
        #endregion
        #region setServices
        public void setServices(List<EaServices.ServicesCallConfig> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null) continue;
                if (l[i] is EaServices.ServicesCallConfig)
                {

                    EaServices.ServicesCallConfig el = (EaServices.ServicesCallConfig)l[i];
                    string basicKey = "service" + (i + 1).ToString();
                    this.currentConfig.AppSettings.Settings[basicKey + "GUID"].Value = el.GUID;
                    this.currentConfig.AppSettings.Settings[basicKey + "Text"].Value = el.ButtonText;
                }
            }

        }
        #endregion
    }
}

