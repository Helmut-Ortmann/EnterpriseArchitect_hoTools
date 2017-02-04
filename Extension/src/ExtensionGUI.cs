using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AddinFramework.Util.Script;
using EAAddinFramework.Utils;
using hoTools.ActiveX;
using hoTools.Settings;
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
        private List<ExtensionItem> _lExtensions;
        private FrmQueryAndScript _frmQueryAndScript;

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
            _lExtensions = _globalCfg.Extensions.LExtensions;
            InitExtensionDataGrid();
            InitExtensionDataTable();
            LoadExtensions();
        }
        /// <summary>
        /// Load all extensions.
        /// </summary>
        private void LoadExtensions()
        {
            LoadExtensionDataTable();
            dataGridExtensions.DataSource = _tableExtensions;
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
                DataPropertyName = "ExtensionName",
                Name = "ExtensionName",
                HeaderText = @"ExtensionName",
                Visible = false
            };

            col = new DataGridViewTextBoxColumn
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
                DataPropertyName = "Signature",
                Name = "Signature",
                HeaderText = @"Signature"
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
            DataColumn extensionItem = new DataColumn("ExtensionItem", typeof(ExtensionItem));
            DataColumn name = new DataColumn("Name", typeof(string));
            DataColumn type = new DataColumn("Type", typeof(string));
            DataColumn signature = new DataColumn("Signature", typeof(string));
            DataColumn description = new DataColumn("Description", typeof(string));
            // add columns
            _tableExtensions.Columns.AddRange(new[]
                {
                    extensionItem,
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

            foreach (var row in _lExtensions)
            {

                _tableExtensions.Rows.Add(row, row.Name, row.Type, row.Signature, row.Description);
                row.AnalyzeAssembly();
                string methods = row.PublicStaticMethods.ToString();
            }


        }

        /// <summary>
        /// Close Addin:
        /// <para/>- Close not stored files
        /// </summary>
        void Close()
        {

        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _frmQueryAndScript = new FrmQueryAndScript(AddinSettings);
            _frmQueryAndScript.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadExtensions();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = dataGridExtensions.CurrentCell.RowIndex;
            if (index < 0) return;
            ExtensionItem extensionItem = _tableExtensions.Rows[index].Field<ExtensionItem>(0);
            MessageBox.Show(extensionItem.ExtensionDetails(), "Details Extension");

        }
    }
}
