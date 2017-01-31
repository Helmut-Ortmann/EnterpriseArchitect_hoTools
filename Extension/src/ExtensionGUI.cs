using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinFramework.Util;
using AddinFramework.Util.Script;
using EA;
using EAAddinFramework.Utils;
using hoTools.ActiveX;
using hoTools.EaServices;
using hoTools.EaServices.WiKiRefs;
using hoTools.Settings;
using hoTools.Utils;
using hoTools.Utils.Excel;
using hoTools.Utils.SQL;

// Resource Manager



namespace hoTools.Extensions
{


    /// <summary>
    /// ActiveX COM Component 'hoTools.ExtensionsGUI' to show as tab in the EA Addin window
    /// this.Tag object with string of:
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("07081FF7-5B36-4487-994B-B69DA4D4C530")]
    [ProgId(Progid)]
    [ComDefaultInterface(typeof(IExtensionGui))]
    public partial class ExtensionGui : AddinGui, IExtensionGui
    {
        public const string Progid = "hoTools.ExtensionGUI";
        public const string Tabulator = "Extensions";





        // settings
        FrmQueryAndScript _frmQueryAndScript;


       

        #region Constructor

        /// <summary>
        /// Constructor QueryGUI. Constructor make the basic initialization. 
        /// The real initialization is done after Setting the Repository in setter of property:
        /// 'Repository'
        /// </summary>
        public ExtensionGui()
        {
            InitializeComponent();

           
        }

        #endregion

        // Interface IQueryGUI implementation
        public string GetName() => "hoTools.QueryGUI";

        #region Set Repository

        /// <summary>
        /// Initialize Window after the repository is known/updated
        /// </summary>
        public override EA.Repository Repository
        {
            set
            {
                base.Repository = value;
                // only if there is a repository available
                if (Repository == null || value.ProjectGUID == "") return;

                try
                {
                    InitializeSettings();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), @"SQL,Script: Error Initialization");
                }
            }
        }

        #endregion

        /// <summary>
        /// Initialize setting. Only call after Repository is known.
        /// <para/>- Tag (
        /// <para/>- Model
        /// <para/>- Settings
        /// updated
        /// </summary>
        /// <returns></returns>
        bool InitializeSettings()
        {
            // default 
           
            // Tab Pages for *.sql queries update
            // Make sure the Container is initialized
            if (components == null)
            {
                components = new System.ComponentModel.Container();
            }
           

            return true;
        }

        
    }
}
