using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.ActiveX;

using System.Collections.Generic;
using EAAddinFramework.Utils;
using hoTools.Settings;

using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

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
            // Script
            initScriptDataGrid();
            initScriptDataTable();

            

        }
        #endregion
        /// <summary>
        /// Initialize setting
        /// </summary>
        /// <returns></returns>
        bool initializeSettings()
        {
            toolStripComboBoxHistoryUpdate(toolStripComboBoxHistory);

            return true;
        }

        private void toolStripComboBoxHistoryUpdate(ToolStripComboBox toolStripCombo)
        {
            toolStripCombo.ComboBox.DataSource = null;
            toolStripCombo.ComboBox.BindingContext = this.BindingContext;
            toolStripCombo.ComboBox.DisplayMember = "DisplayName";
            toolStripCombo.ComboBox.ValueMember = "FullName";
            toolStripCombo.ComboBox.DataSource = AddinSettings.sqlFiles.lSqlHistoryFilesCfg;
        }


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
            col.DataPropertyName = "Function";
            col.Name = "Function";
            col.HeaderText = "Function";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "ParCount";
            col.Name = "ParCount";
            col.HeaderText = "Par count";
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Err";
            col.Name = "Err";
            col.HeaderText = "Err";
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
                    initializeSettings();
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

        
        private void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get selected element and type
            EA.ObjectType oType = Repository.GetContextItemType();
            object oContext = Repository.GetContextObject();

            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
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
            if (String.IsNullOrWhiteSpace(err))
            { MessageBox.Show("", $"Funtion compiled fine {scriptName}.{functionName}"); }
            else
            { MessageBox.Show("Error:\n'" + err + "'", $"Error {scriptName}:{functionName}"); }


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

        /// <summary>
        /// Output the script code of the selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            

            var script = row["ScriptObj"] as Script;
            MessageBox.Show(script._code, $"Code of {script.displayName}");

        }

        private void contextMenuStripSql_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        /// <summary>
        /// Insert Element Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemSqlElement_Click(object sender, EventArgs e)
        {
            const string ELEMENT_TEMPLATE =
                "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                "from t_object o\r\n" +
                "where o.object_type in (\"Class\",\"Component\")";
            insertText(txtBoxSql, ELEMENT_TEMPLATE);
        }

        private void insertText(TextBox txtBox, string ELEMENT_TEMPLATE)
        {
            var selectionIndex = txtBox.SelectionStart;
            txtBox.Text = txtBox.Text.Insert(selectionIndex, ELEMENT_TEMPLATE);
            txtBox.SelectionStart = selectionIndex + ELEMENT_TEMPLATE.Length;
        }

        private void diagramTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string DIAGRAM_TEMPLATE =
               "select d.ea_guid AS CLASSGUID, d.diagram_type AS CLASSTYPE,d.Name AS Name,d.diagram_type As Type, * \r\n" +
               "from t_diagram d\r\n" +
               "where d.diagram_type in \r\n" +
               "(\"Activity\",\"Analysis\",\"Collaboration\",\"Component\",\"CompositeStructure\", \"Custom\",\"Deployment\",\"Logical\",\r\n"+
               "\"Object\",\"Package\",  \"Sequence\",\"Statechart\",\"Timing\", \"UMLDiagram\", \"Use Case\", )";
            insertText(txtBoxSql, DIAGRAM_TEMPLATE);

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            safeTabAs(tabSqlPage1, txtBoxSql);
        }

        Dictionary<string, string> a = null;
        /// <summary>
        /// Save sql string from TabPage with TextBox inside it to *.sql file.
        /// - Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPageSql">The TabPage</param>
        /// <param name="txtBoxSql"></param>
        private void safeTabAs(TabPage tabPageSql, TextBox txtBoxSql)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = @"c:\temp\sql";
            saveFileDialog.FileName = tabPageSql.Text;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            //saveFileDialog.DefaultExt = "sql";


            if (saveFileDialog.ShowDialog() == DialogResult.OK)

            {
                StreamWriter myStream = new StreamWriter(saveFileDialog.OpenFile());
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    myStream.Write(txtBoxSql.Text);
                    myStream.Close();
                    tabPageSql.Text = Path.GetFileName(saveFileDialog.FileName);

                    // store the complete filename
                    AddinSettings.sqlFiles.insert(saveFileDialog.FileName);
                    AddinSettings.save();
                }
            }
        }
        private void  safeTab(TabPage tabPageSql, TextBox txtBoxSql)
        {

        }


        /// <summary>
        /// Load sql string from *.sql file into TabPage with TextBox inside.
        /// - Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void loadTabFrom(TabPage tabPageSql, TextBox txtBoxSql)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"c:\temp\sql";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {
                StreamReader myStream = new StreamReader(openFileDialog.OpenFile());
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    txtBoxSql.Text = myStream.ReadToEnd();
                    myStream.Close();
                    tabPageSql.Text = Path.GetFileName(openFileDialog.FileName);

                    // store the complete filename
                    AddinSettings.sqlFiles.insert(openFileDialog.FileName);
                    AddinSettings.save();
                }
            }

        }
        private void addTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tab = new TabPage() { Text = @"mySql.sql" };
            tabControlSql.TabPages.Add(tab);
            tabControlSql.SelectedTab = tab;
            TextBox txtBox = new TextBox { Parent = tab, Dock = DockStyle.Fill };
        }

        private void loadSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadTabFrom(tabSqlPage1, txtBoxSql);
        }

        /// <summary>
        /// Run SQL and execute Script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {

            try {
                // run the query
                XmlDocument xml = Model.SQLQuery(txtBoxSql.Text);
            } catch (Exception ex)
            {
                MessageBox.Show($"SQL:\r\n{txtBoxSql.Text}\r\n{ex.Message}", "Error SQL");
                return;
            }
            XDocument x = XDocument.Parse(xml.OuterXml);
            //---------------------------------------------------------------------
            // make the output format:
            // From Query:
            //<EADATA><Dataset_0>
            // <Data>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            // </Data>
            //</Dataset_0><EADATA>
            //
            //-----------------------------------
            // To output EA XML:
            //<ReportViewData>
            // <Fields>
            //   <Field name=""/>
            //   <Field name=""/>
            // </Fields>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            //</reportViewData>
            XDocument target = new XDocument(
                new XElement("ReportViewData",
                    new XElement("Fields",
                           from field in x.Descendants("Row").FirstOrDefault().Descendants()
                           select new XElement("Field", new XAttribute("name", field.Name))
                    ),
                    new XElement("Rows",
                                from row in x.Descendants("Row")
                                select new XElement(row.Name,
                                       from field in row.Nodes()
                                       select new XElement("Field", new XAttribute("name", ((XElement)field).Name),
                                                                    new XAttribute("value", ((XElement)field).Value)))
                    
                )
            ));
            Repository.RunModelSearch("", "", "", target.ToString());

            


        }      
    }
}
