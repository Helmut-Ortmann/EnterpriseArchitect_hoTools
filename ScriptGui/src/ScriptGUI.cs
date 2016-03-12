using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.ActiveX;

using System.Collections.Generic;
using EAAddinFramework.Utils;

using System.Drawing;



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

        /// <summary>
        /// The selected row in script list
        /// </summary>
        int rowScriptsIndex;


        #region Constructor
        public ScriptGUI()
        {
            InitializeComponent();

            // individual initialization
            initScriptDataGrid();
            initScriptDataTable();

        }
        #endregion
        #region initDataGrid
        private void initScriptDataGrid()
        {
            dataGridViewScripts.AutoGenerateColumns = false;

            dataGridViewScripts.DataSource = null;

            DataGridViewTextBoxColumn col;

            
            col = new DataGridViewTextBoxColumn();
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
        private void initScriptDataTable()
        {
            dataGridViewScripts.DataSource = null;
            _tableFunctions = new DataTable();
            DataColumn functionScriptObj = new DataColumn("ScriptObj", typeof(Script));
            DataColumn functionFunctionObj = new DataColumn("FunctionObj", typeof(ScriptFunction));
            DataColumn functionScriptColumn = new DataColumn("Script", typeof(string));
            DataColumn functionLanguageColumn = new DataColumn("Language", typeof(string));
            DataColumn functionGroupColumn = new DataColumn("Group", typeof(string));
            DataColumn functionErrColumn = new DataColumn("Err", typeof(string));
            DataColumn functionFunctionColumn = new DataColumn("Function", typeof(string));
            DataColumn functionParCountColumn = new DataColumn("ParCount", typeof(int));
            // add columns
            _tableFunctions.Columns.AddRange(new DataColumn[]
                {
                    functionScriptObj,
                    functionFunctionObj,
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
                newRow["ScriptObj"] = script;
                newRow["Script"] = script.name;
                newRow["Language"] = script.languageName;
                newRow["Group"] = script.groupName;
                newRow["Err"] = script.errorMessage;
                foreach (ScriptFunction function in script.functions)
                {
                    newRow["FunctionObj"] = function;
                    newRow["Function"] = function.name;
                    newRow["ParCount"] = function.numberOfParameters;
                }
                _tableFunctions.Rows.Add(newRow);
            }
            // bind to grid
            // Select column to view
            dataGridViewScripts.DataSource = _tableFunctions;

        }

        /// <summary>
        /// Run the selected scripts with:
        /// - itemObject The selected item
        /// - objectType ObjectType of the selected object
        /// - Model object (only if parameter count = 3)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRunScript_Click(object sender, EventArgs e)
        {
            // get selected element and type
            EA.ObjectType oType = Repository.GetContextItemType();
            object oContext = Repository.GetContextObject();

            foreach (DataGridViewRow row in dataGridViewScripts.SelectedRows)
            {
                // get parameter of Script and selected function
                DataRowView rowToRunView = row.DataBoundItem as DataRowView;
                DataRow rowToRun = rowToRunView.Row;
                var scriptFunction = rowToRun["FunctionObj"] as ScriptFunction;
                runScriptFromContext(scriptFunction, oType, oContext);

            }
        }
        /// <summary>
        /// Run function for EA item of arbitrary type
        /// - If parameter count = 2 it calls the function with oType, oContext
        /// - If parameter count = 3 it calls the funtion with oType, oContext, Model
        /// </summary>
        /// <param name="function">Function</param>
        /// <param name="oType">EA Object type</param>
        /// <param name="oContext">EA Object</param>
        /// <returns></returns>
        private bool runScriptFromContext(ScriptFunction function, EA.ObjectType oType, object oContext)
        {
                // run script according to parameter count
                switch (function.numberOfParameters)
                {
                    case 2:
                        object[] par2 = { oContext, oType };
                        return new ScriptFuntionWrapper(function).execute(par2);
                    case 3:
                        object[] par3 = { oContext, oType, Model };
                        return new ScriptFuntionWrapper(function).execute(par3);
                    default:
                        MessageBox.Show($"Script {function.fullName}  has {function.numberOfParameters} parameter",
                            "Script function parameter count not 2 or 3, Break!");
                        return false;
                }
           
        }

        // Context item of dataGrid
        private void contextMenuStripDataGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get selected element and type
            EA.ObjectType oType = Repository.GetContextItemType();
            object oContext = Repository.GetContextObject();

            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRow row = rowToRun.DataBoundItem as DataRow;
            var scriptFunction = row["FunctionObj"] as ScriptFunction;
            runScriptFromContext(scriptFunction, oType, oContext);

        }

        /// <summary>
        /// Show error of the selected Script
        /// </summary>
        private void ShowErrorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridViewScripts.Rows[rowScriptsIndex];
            string scriptName = row.Cells["Script"].Value as string;
            string functionName = row.Cells["Function"].Value as string;
            string scriptLanguag = row.Cells["Language"].Value as string;
            string err = row.Cells["Err"].Value as string;
            if (err.Equals(""))
            MessageBox.Show("", $"Funtion compiled fine {scriptName}.{functionName}");
            else MessageBox.Show("Error:\n'" + err + "'", $"Error {scriptName}:{functionName}");


        }

        /// <summary>
        /// MouseClick in dataGridViewScripts
        /// - Estiminate the clicked row
        /// - store the current row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewScripts_MouseClick(object sender, MouseEventArgs e)
        {
            rowScriptsIndex = dataGridViewScripts.HitTest(e.X, e.Y).RowIndex;
        }

       
    }
}
