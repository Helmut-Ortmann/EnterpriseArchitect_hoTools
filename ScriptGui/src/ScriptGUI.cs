using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.ActiveX;

using System.Collections.Generic;
using EAAddinFramework.Utils;



namespace hoTools.Scripts
{
    /// <summary>
    /// ActiveX COM Component 'hoTools.ScriptGUI' to show as tab in the EA Addin window
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("23722D72-7C6A-4246-B8B8-8D421CEBCD65")]
    [ProgId(PROGID)]
    [ComDefaultInterface(typeof(IScriptGUI))]
    public partial class ScriptGUI : AddinGUI, IScriptGUI
    {
        public const string PROGID = "hoTools.ScriptGUI";
        public const string TABULATOR = "Scripts";

        List<Script> _lscripts = null;  // list off all scripts
        DataTable _tableFunctions = null; // Scripts and Functions

        Script _script = null;

        #region Constructor
        public ScriptGUI()
        {
            InitializeComponent();

            initDataGrid();
            initDataTable();

        }
        #endregion
        #region initDataGrid
        private void initDataGrid()
        {
            dataGridViewScripts.AutoGenerateColumns = false;
            dataGridViewScripts.DataSource = null;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Script";
            col.Name = "Script";
            col.HeaderText = "Script";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Language";
            col.Name = "Language";
            col.HeaderText = "Language";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Group";
            col.Name = "Group";
            col.HeaderText = "Group";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Err";
            col.Name = "Err";
            col.HeaderText = "Err";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Function";
            col.Name = "Function";
            col.HeaderText = "Function";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "ParCount";
            col.Name = "ParCount";
            col.HeaderText = "Par count";
            dataGridViewScripts.Columns.Add(col);
        }
        #endregion
        #region initDataTable
        private void initDataTable()
        {
            dataGridViewScripts.DataSource = null;
            _tableFunctions = new DataTable();
            DataColumn functionScriptColumn = new DataColumn("Script", typeof(string));
            DataColumn functionLanguageColumn = new DataColumn("Language", typeof(string));
            DataColumn functionGroupColumn = new DataColumn("Group", typeof(string));
            DataColumn functionErrColumn = new DataColumn("Err", typeof(string));
            DataColumn functionFunctionColumn = new DataColumn("Function", typeof(string));
            DataColumn functionParCountColumn = new DataColumn("ParCount", typeof(int));
            // add columns
            _tableFunctions.Columns.AddRange(new DataColumn[]
                {
                    functionScriptColumn,
                    functionLanguageColumn,
                    functionGroupColumn,
                    functionErrColumn,
                    functionFunctionColumn,
                    functionParCountColumn
                }
                );
        }
        #endregion


        // Interface IScriptGUI implementation
        public string getName() => "hoTools.ScriptGUI";

        public override EA.Repository Repository
        {
            set
            {
                base.Repository = value;
                // only if there is a repository available
                if (value.ProjectGUID != "")
                {
                    _lscripts = Script.getEAMaticScripts(Model);
                    updateTableFunctions();
                }
            }
        }


        private void ScriptGUI_Load(object sender, EventArgs e)
        {
         
        }

       
        /// <summary>
        /// Load all usable script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadScripts_Click(object sender, EventArgs e)
        {
            _lscripts = Script.getEAMaticScripts(Model);
            updateTableFunctions();
        }

        void updateTableFunctions()
        {
            _tableFunctions.Rows.Clear();
            // fill list
            foreach (Script script in _lscripts)
            {
                DataRow newRow = _tableFunctions.NewRow();
                newRow["Script"] = script.name;
                newRow["Language"] = script.languageName;
                newRow["Group"] = script.groupName;
                newRow["Err"] = script.errorMessage;
                foreach (ScriptFunction function in script.functions)
                {
                    newRow["Function"] = function.name;
                    newRow["ParCount"] = function.numberOfParameters;
                }
                _tableFunctions.Rows.Add(newRow);
            }
            // bind to grid
            // Select column to view
            dataGridViewScripts.DataSource = _tableFunctions;

        }

        private void btnRunScript_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewScripts.SelectedRows)
            {
                Script selScript = row.DataBoundItem as Script;
                if (selScript != null)
                {
                    string s = "";
                    foreach (ScriptFunction function in selScript.functions)
                    {
                        //s = string.Format("Function '{0}' function:'{1} NumberOfParameters:{2}", function.fullName, function.name, function.numberOfParameters);
                        s = s + "\n" + $"Fullname '{function.fullName}' Function:'{ function.name} NumberOfParameters:{function.numberOfParameters}";
                    }
                    MessageBox.Show(s, "Script:" + selScript.displayName);

                }
            }
        }
    }
}
