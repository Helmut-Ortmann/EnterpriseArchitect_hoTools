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

        DataTable _tableFunctions; // Scripts and Functions







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
        public string GetName() => "hoTools.ExtensionGUI";

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
                    MessageBox.Show(e.ToString(), @"Extensions: Error Initialization");
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

        private void ExtensionGui_Load(object sender, EventArgs e)
        {

        }

        #region initDataGrid

        /// <summary>
        /// Init Script Data for Grid
        /// </summary>
        void InitScriptDataGrid()
        {
            dataGridViewExtensions.AutoGenerateColumns = false;

            dataGridViewExtensions.DataSource = null;


            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                Name = "Name",
                HeaderText = @"Name"
            };
            dataGridViewExtensions.Columns.Add(col);


            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Type",
                Name = "Type",
                HeaderText = @"Type of Assembly"
            };
            dataGridViewExtensions.Columns.Add(col);

            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Signiture",
                Name = "Signiture",
                HeaderText = @"Signiture"
            };
            dataGridViewExtensions.Columns.Add(col);


            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                Name = "Description",
                HeaderText = @"Description"
            };
            dataGridViewExtensions.Columns.Add(col);

           
        }

        #endregion

        #region initDataTable

        /// <summary>
        /// Init the Data Grid Table.
        /// </summary>
        void InitScriptDataTable()
        {
            dataGridViewExtensions.DataSource = null;
            _tableFunctions = new DataTable();
            DataColumn functionName = new DataColumn("Name", typeof(Script));
            DataColumn functionType = new DataColumn("Type", typeof(ScriptFunction));
            DataColumn functionSigniture = new DataColumn("Signiture", typeof(string));
            DataColumn functionDescription = new DataColumn("Description", typeof(string));
            // add columns
            _tableFunctions.Columns.AddRange(new[]
                {
                    functionName,
                    functionType,
                    functionSigniture,
                    functionDescription,

                }
            );
        }

        #endregion

        /// <summary>
        /// Close Addin:
        /// <para/>- Close not stored files
        /// </summary>
        void Close()
        {

        }
    }
}
