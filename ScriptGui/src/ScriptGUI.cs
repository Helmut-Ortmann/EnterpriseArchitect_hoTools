using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using hoTools.ActiveX;

using System.Collections.Generic;
using EAAddinFramework.Utils;
using hoTools.Settings;
using hoTools.Utils.SQL;

using System.IO;



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

        SqlTabPagesCntrl _sqlTabCntrls = null;  // TAB Control with its TabPages

        /// <summary>
        /// The selected row in script list
        /// </summary>
        int rowScriptsIndex;

        // Coordinates of Close Rectangle relative to TabPage Caption Rectangle TopRight Position
        const int CLOSE_BUTTON_RECTANGLE_RIGHT_X = -15;
        const int CLOSE_BUTTON_RECTANGLE_TOP_Y = 4;
        const int CLOSE_BUTTON_RECTANGLE_WIDTH = 11;
        const int CLOSE_BUTTON_RECTANGLE_HIGHT = 16;


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
        /// Initialize setting. Only call after
        /// - Model
        /// - Settings
        /// updated
        /// </summary>
        /// <returns></returns>
        bool initializeSettings()
        {
            _sqlTabCntrls = new SqlTabPagesCntrl(Model, AddinSettings, components, tabControlSql, txtSearchTerm);
            fileToolStripMenuItem.DropDownItems.Add(_sqlTabCntrls.LoadRecentFileItem);
            fileToolStripMenuItem.DropDownItems.Add(_sqlTabCntrls.NewTabAndLoadRecentFileItem);
            fileToolStripMenuItem.ShowDropDown();
            this.ResumeLayout(false);
            this.PerformLayout();
            return true;
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
                    _sqlTabCntrls.addTab();

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

        void updateTableFunctions(bool isWithAll=false)
        {
            _tableFunctions.Rows.Clear();
            // fill list
            DataRow newRow;
            foreach (Script script in _lscripts)
            {

                foreach (ScriptFunction function in script.functions)
                {
                    if (isWithAll || (function.numberOfParameters > 1 && function.numberOfParameters < 4)) { 
                        newRow = _tableFunctions.NewRow();
                        newRow["ScriptObj"] = script;
                        newRow["Script"] = script.name;
                        newRow["Language"] = script.languageName;
                        newRow["Group"] = script.groupName;
                        newRow["Err"] = script.errorMessage;

                        newRow["FunctionObj"] = function;
                        newRow["Function"] = function.name;
                        newRow["ParCount"] = function.numberOfParameters;
                        _tableFunctions.Rows.Add(newRow);
                    }
                }
                
            }
            // bind to grid
            // Select column to view
            dataGridViewScripts.DataSource = _tableFunctions;

        }

        /// <summary>
        /// Run SQL with replacing macros and sending the result to EA Search Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRunSql_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.runSqlForSelectedTabPage();
        }

        

        private void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get selected element and type
            EA.ObjectType oType = Repository.GetContextItemType();
            object oContext = Repository.GetContextObject();

            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            var scriptFunction = row["FunctionObj"] as ScriptFunction;
            GuiFunction.RunScriptFunction(Model, scriptFunction, oType, oContext);

        }

        /// <summary>
        /// Show error of the selected Script
        /// </summary>
        private void ShowScriptErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridViewScripts.Rows[rowScriptsIndex];
            string scriptName = row.Cells["Script"].Value as string;
            string functionName = row.Cells["Function"].Value as string;
            string scriptLanguag = row.Cells["Language"].Value as string;
            string err = row.Cells["Err"].Value as string;
            if (String.IsNullOrWhiteSpace(err))
            { MessageBox.Show("", $"Function compiled fine {scriptName}.{functionName}"); }
            else
            { MessageBox.Show("Error:\n'" + err + "'", $"Error {scriptName}:{functionName}"); }


        }

        /// <summary>
        /// MouseClick in dataGridViewScripts
        /// - Estimate
        /// mate the clicked row
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

        
        private void insertText(TextBox txtBox, string text)
        {
            var selectionIndex = txtBox.SelectionStart;
            txtBox.Text = txtBox.Text.Insert(selectionIndex, text);
            txtBox.SelectionStart = selectionIndex + text.Length;
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
            // get file name
            saveFileDialog.FileName = GuiFunction.getFileNameFromCaptionUnchanged(tabPageSql.Text);
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
                    tabPageSql.Text = Path.GetFileName(openFileDialog.FileName) + " ";
                }
            }

        }

     
        private void addTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tab = new TabPage { Text = @"mySql.sql" };
            tabControlSql.TabPages.Add(tab);
            tabControlSql.SelectedTab = tab;
            TextBox txtBox = new TextBox { Parent = tab, Dock = DockStyle.Fill };
        }

       
        /// <summary>
        /// Run SQL and execute Script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunScriptForSql_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            // get TabPage
            TabPage tabPage = tabControlSql.TabPages[tabControlSql.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // get Script and its parameter to run
            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            var scriptFunction = row["FunctionObj"] as ScriptFunction;

            // run SQL, Script and ask whether to execute, skip script or break all together
            GuiFunction.RunScriptWithAsk(Model, textBox.Text, scriptFunction, isWithAsk:false);
            Cursor.Current = Cursors.Default;

        }

        private void btnRunScriptForSqlWithAsk_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            // get TabPage
            TabPage tabPage = tabControlSql.TabPages[tabControlSql.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // get Script and its parameter to run
            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            var scriptFunction = row["FunctionObj"] as ScriptFunction;

            // run SQL, Script and ask whether to execute, skip script or break all together
            GuiFunction.RunScriptWithAsk(Model, textBox.Text, scriptFunction, isWithAsk:true);

            Cursor.Current = Cursors.Default;
        }

       

        
        private void showSqlErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string appData = Environment.GetEnvironmentVariable("appdata");
            string filePath = appData + @"\Sparx Systems\EA\dberror.txt";
            try {
                Process.Start(filePath);
            } catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nFile:'{filePath}'", $"Can't open EA SQL Error file dberror.tx");
            }
        }

        private void txtBoxSql_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            TabPage tabPage = (TabPage)txtBox.Parent;
            if (!(tabPage.Text.Contains("*"))) tabPage.Text = tabPage.Text + " *";
        }

        private void FileNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.addTab();
        }

        /// <summary>
        /// Draw a 'x' in the tabPage at the end of the caption (Close Tab)
        /// Set property DrawMode to 'OwnerDrawFixed'
        /// Note: Extend the 'Text' Property by 3 blanks to get space for the extra 'x'
        ///       Use a non proportional font like courier new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlSql_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw a Rectangle with the background color
            Rectangle closeButton = new Rectangle(e.Bounds.Right + CLOSE_BUTTON_RECTANGLE_RIGHT_X,
                                                  e.Bounds.Top + CLOSE_BUTTON_RECTANGLE_TOP_Y, 
                                                  CLOSE_BUTTON_RECTANGLE_WIDTH, 
                                                  CLOSE_BUTTON_RECTANGLE_HIGHT);
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.ControlDark), closeButton);
            e.Graphics.DrawString("X", e.Font, Brushes.Black, e.Bounds.Right -15 , e.Bounds.Top + 4);
            e.Graphics.DrawString(tabControlSql.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Close TabPage if 'x' for close is selected
        /// Note: Use a non proportional font like 'courier new'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlSql_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle r = tabControlSql.GetTabRect(this.tabControlSql.SelectedIndex);
            Rectangle closeButton = new Rectangle(r.Right + CLOSE_BUTTON_RECTANGLE_RIGHT_X, 
                                                  r.Top + CLOSE_BUTTON_RECTANGLE_TOP_Y,
                                                  CLOSE_BUTTON_RECTANGLE_WIDTH,
                                                  CLOSE_BUTTON_RECTANGLE_HIGHT);
            if (closeButton.Contains(e.Location))
                this.tabControlSql.TabPages.Remove(this.tabControlSql.SelectedTab);
        }

        
        
        private void tabControlSql_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
