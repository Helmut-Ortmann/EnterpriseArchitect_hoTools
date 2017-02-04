using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinFramework.Util.Script;
using EAAddinFramework.Utils;
using hoTools.ActiveX;
using hoTools.Utils.Configuration;
using hoTools.Utils.Extensions;

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


        // configuration as singleton
        readonly HoToolsGlobalCfg _globalCfg = HoToolsGlobalCfg.Instance;

        DataTable _tableExtensions; // Scripts and Functions
        private List<ExtensionItem> _lExtension;








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


            // get list of extensions
            _lExtension = _globalCfg.Extensions.LExtension;
            InitExtensionDataGrid();
            InitExtensionDataTable();
            LoadExtensionDataTable();
        }

        #region initDataGrid

        /// <summary>
        /// Init Script Data for Grid
        /// </summary>
        void InitExtensionDataGrid()
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
        /// Init the Data Grid Table with the Extensions
        /// </summary>
        void InitExtensionDataTable()
        {
            dataGridViewExtensions.DataSource = null;
            _tableExtensions = new DataTable();
            DataColumn name = new DataColumn("Name", typeof(Script));
            DataColumn type = new DataColumn("Type", typeof(ScriptFunction));
            DataColumn signature = new DataColumn("Signiture", typeof(string));
            DataColumn description = new DataColumn("Description", typeof(string));
            // add columns
            _tableExtensions.Columns.AddRange(new[]
                {
                    name,
                    type,
                    signature,
                    description,

                }
            );
        }

        #endregion

        void LoadExtensionDataTable()
        {
            _tableExtensions.Rows.Clear();
            _globalCfg.Extensions.LoadExtensions();

            foreach (var row in _lExtension)
            {

                _tableExtensions.Rows.Add(row.Name, row.Type, row.Signiture, row.Description);
            }


        }

        /// <summary>
        /// Close Addin:
        /// <para/>- Close not stored files
        /// </summary>
        void Close()
        {

        }
    }
}
