using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.Utils;
using hoTools.ActiveX;
using hoTools.Utils.SQL;

using System.Collections.Generic;
using EAAddinFramework.Utils;
using hoTools.Settings;
using hoTools.EaServices;
using System.Resources;

using System.IO;
using System.Reflection; // Resource Manager



namespace hoTools.Query
{


    /// <summary>
    /// ActiveX COM Component 'hoTools.QueryGUI' to show as tab in the EA Addin window
    /// this.Tag object with string of:
    /// <para/>- TABULATOR_QUERY if Query mode is used
    /// <para/>- TABULATOR_SCRPT if Script mode is used
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("3F23B705-62F1-43D7-9F6F-085105FDF752")]
    [ProgId(PROGID)]
    [ComDefaultInterface(typeof(IQueryGUI))]
    public partial class QueryGUI : AddinGUI, IQueryGUI
    {
        public const string PROGID = "hoTools.QueryGUI";
        public const string TABULATOR_SCRIPT = "Scripts";
        public const string TABULATOR_QUERY = "SQL";

        List<Script> _lscripts;  // list off all scripts
        DataTable _tableFunctions; // Scripts and Functions

        SqlTabPagesCntrl _sqlTabCntrls;  // TAB Control with its TabPages

        // settings
        FrmQueryAndScript _frmQueryAndScript;


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
        /// <summary>
        /// Constructor QueryGUI. Constructor make the basic initialization. 
        /// The real initialization is done after Setting the Repository in setter of property:
        /// 'Repository'
        /// </summary>
        public QueryGUI()
        {
            InitializeComponent();

            // individual initialization
            // Script
            initScriptDataGrid();
            initScriptDataTable();
        }
        #endregion

        // Interface IQueryGUI implementation
        public string getName() => "hoTools.QueryGUI";
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
                if (value.ProjectGUID != "")
                {
                    initializeSettings();


                }
            }
        }
        #endregion
        /// <summary>
        /// Initialize setting. Only call after
        /// <para/>- Tag (
        /// <para/>- Model
        /// <para/>- Settings
        /// updated
        /// </summary>
        /// <returns></returns>
        bool initializeSettings()
        {
            // set title
            if ((string)Tag == TABULATOR_QUERY)
            {
                lblTitle.Text = TABULATOR_QUERY;
            }
            else { lblTitle.Text = TABULATOR_SCRIPT; }

            // Tab Pages for *.sql queries update
            _sqlTabCntrls = new SqlTabPagesCntrl(Model, AddinSettings, components, tabControlSql, txtSearchTerm,
                newTabFromToolStripMenuItem, 
                loadTabFromToolStripMenuItem);

            if (tabControlSql.TabPages.Count == 0)
            {
                // first tab with Element Template
                _sqlTabCntrls.addTab(SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE));
            }


            // run for query
            if ((string)this.Tag == TABULATOR_QUERY)
            {
                // don't show Script container
                splitContainer.Panel2Collapsed = true;
            }
            else // run for Script (includes Query)
            {

                splitContainer.SplitterDistance = 330;
                // available script updates
                _lscripts = Script.getEAMaticScripts(Model);
                updateTableFunctions();
            }

            // enable drag and drop
            AllowDrop = true;
            DragEnter += new DragEventHandler(tabControlSql_DragEnter);
            DragDrop += new DragEventHandler(tabControlSql_DragDrop);

            tabControlSql.AllowDrop = true;
            return true;
        }

        
        #region initDataGrid
        void initScriptDataGrid()
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
         void initScriptDataTable()
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

    /// <summary>
    /// Close Addin:
    /// <para/>- Close not stored files
    /// </summary>
      void close()
        {

        }



        /// <summary>
        /// Load all usable script. They shall contain 'EA-Matic'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLoadScripts_Click(object sender, EventArgs e)
        {
            _lscripts = Script.getEAMaticScripts(Model);
            updateTableFunctions();
        }
        /// <summary>
        /// Compile, load script with may run on SQL Query result rows. The conditions:
        /// <para/>- Contains string 'EA-Matic'
        /// <para/>- With 2 or 3 parameters
        /// </summary>
        /// <param name="isWithAll"></param>
         void updateTableFunctions(bool isWithAll=false)
        {
            _tableFunctions.Rows.Clear();
            // fill list
            DataRow newRow;
            foreach (Script script in _lscripts)
            {

                foreach (ScriptFunction function in script.functions)
                {
                    // 2 or 3 parameters
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
            _sqlTabCntrls.runSqlTabPage();
        }

        
        /// <summary>
        /// Show error of the selected Script
        /// </summary>
        void ShowScriptErrorToolStripMenuItem_Click(object sender, EventArgs e)
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
         void dataGridViewScripts_MouseClick(object sender, MouseEventArgs e)
        {
            rowScriptsIndex = dataGridViewScripts.HitTest(e.X, e.Y).RowIndex;
        }

        /// <summary>
        /// Output the script code of the selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void showScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            

            var script = row["ScriptObj"] as Script;
            MessageBox.Show(script._code, $"Code of {script.displayName}");

        }

        
        void insertText(TextBox txtBox, string text)
        {
            var selectionIndex = txtBox.SelectionStart;
            txtBox.Text = txtBox.Text.Insert(selectionIndex, text);
            txtBox.SelectionStart = selectionIndex + text.Length;
        }

      
        /// <summary>
        /// Save sql string from TabPage with TextBox inside it to *.sql file.
        /// - Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPageSql">The TabPage</param>
        /// <param name="txtBoxSql"></param>
        void safeTabAs(TabPage tabPageSql, TextBox txtBoxSql)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = @"c:\temp\sql";
            // get file name
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
                    AddinSettings.historySqlFiles.insert(saveFileDialog.FileName);
                    AddinSettings.save();
                }
            }
        }
        


        /// <summary>
        /// Load sql string from *.sql file into TabPage with TextBox inside.
        /// - Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
         void loadTabFrom(TabPage tabPageSql, TextBox txtBoxSql)
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
                    AddinSettings.historySqlFiles.insert(openFileDialog.FileName);
                    AddinSettings.save();
                    tabPageSql.Text = Path.GetFileName(openFileDialog.FileName) + " ";
                }
            }

        }

     
              
        /// <summary>
        /// Run SQL and execute Script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void btnRunScriptForSql_Click(object sender, EventArgs e)
        {
            RunScriptWithAskGui(isWithAsk: false);

        }
        /// <summary>
        /// Run sql query and execute Script for found rows. Ask if script is to execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRunScriptForSqlWithAsk_Click(object sender, EventArgs e)
        {
            RunScriptWithAskGui(isWithAsk:true);
        }
        /// <summary>
        /// Run sql query and execute Script for found rows. This function is intended to use from Dialog.
        /// </summary>
        /// <param name="isWithAsk">True: Ask for each found rows if to execute</param>
        void RunScriptWithAskGui(bool isWithAsk = false)
        {
            if (tabControlSql.SelectedIndex == -1) return;
            Cursor.Current = Cursors.WaitCursor;
            // get TabPage
            TabPage tabPage = tabControlSql.TabPages[tabControlSql.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // get Script and its parameter to run
            DataGridViewRow rowToRun = dataGridViewScripts.Rows[rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            var scriptFunction = row["FunctionObj"] as ScriptFunction;

            // replace templates, search term and more
            string sql = SqlTemplates.replaceMacro(Repository, textBox.Text, txtSearchTerm.Text);
            if (sql == "") return;

            // run SQL, Script and ask whether to execute, skip script or break all together
            GuiFunction.RunScriptWithAsk(Model, sql, scriptFunction, isWithAsk: isWithAsk);

            Cursor.Current = Cursors.Default;
        }







        void txtBoxSql_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            TabPage tabPage = (TabPage)txtBox.Parent;
            if (!(tabPage.Text.Contains("*"))) tabPage.Text = tabPage.Text + " *";
        }

         void FileNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.addTab();
        }

        /// <summary>
        /// Draw all the Tab after change of TAB (focus, text,.. changed)
        /// <para/>
        /// If active than change TAB properties for easier seeing the active tab
        /// <para/>
        /// Draw an 'x' in the tabPage at the end of the caption (Close Tab)
        /// Set property DrawMode to 'OwnerDrawFixed'
        /// Note: Extend the 'Text' Property by 3 blanks to get space for the extra 'x'
        ///       Use a non proportional font like courier new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControlSql_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw a Rectangle with the background color
            Rectangle closeButton = new Rectangle(e.Bounds.Right + CLOSE_BUTTON_RECTANGLE_RIGHT_X,
                                                  e.Bounds.Top + CLOSE_BUTTON_RECTANGLE_TOP_Y,
                                                  CLOSE_BUTTON_RECTANGLE_WIDTH,
                                                  CLOSE_BUTTON_RECTANGLE_HIGHT);
            
            // Output Close simulated button
            if (e.Index == tabControlSql.SelectedIndex)
            {   // selected Tab
                //change background color
                e = new DrawItemEventArgs(e.Graphics,
                                e.Font,
                                e.Bounds,
                                e.Index,
                                e.State ^ DrawItemState.Selected,
                                e.ForeColor,
                                Color.LightGray);//Choose the color
                e.DrawBackground();
                e.Graphics.DrawString(tabControlSql.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                //e.DrawFocusRectangle();
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.ActiveCaption), closeButton);
                e.Graphics.DrawString("X", e.Font, Brushes.Red, e.Bounds.Right - 15, e.Bounds.Top + 4);

                // Draw the background of the ListBox control for each item.

            }

            else
            {   // not selected tab
                // output TAB Caption
                e.Graphics.DrawString(tabControlSql.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.InactiveCaption), closeButton);
                e.Graphics.DrawString("X", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4); }

            
            // If the TAB has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
    

        }

        /// <summary>
        /// Close TabPage if 'x' for close is selected
        /// Note: Use a non proportional font like 'courier new'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void tabControlSql_MouseDown(object sender, MouseEventArgs e)
        {
            if (tabControlSql.SelectedIndex == -1) return;
            Rectangle r = tabControlSql.GetTabRect(this.tabControlSql.SelectedIndex);
            Rectangle closeButton = new Rectangle(r.Right + CLOSE_BUTTON_RECTANGLE_RIGHT_X, 
                                                  r.Top + CLOSE_BUTTON_RECTANGLE_TOP_Y,
                                                  CLOSE_BUTTON_RECTANGLE_WIDTH,
                                                  CLOSE_BUTTON_RECTANGLE_HIGHT);
            if (closeButton.Contains(e.Location))
            {
                TabPage tabPage = tabControlSql.SelectedTab;
                _sqlTabCntrls.close(tabPage);

            }
        }

        
        
       
         void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmQueryAndScript = new FrmQueryAndScript(AddinSettings);
            _frmQueryAndScript.ShowDialog();
        }

        #region About
        /// <summary>
        /// About Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string configFilePath = AddinSettings.ConfigFilePath;
            switch (AddinSettings.Customer)
            {

                case AddinSettings.CustomerCfg.VAR1:
                    EaService.aboutVAR1(Release, configFilePath);
                    break;
                case AddinSettings.CustomerCfg.hoTools:
                    EaService.about(Release, configFilePath);
                    break;
                default:
                    EaService.about(Release, configFilePath);
                    break;
            }
        }
        #endregion

        #region Help
        /// <summary>
        /// ShowHelp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, EaService.getAssemblyPath() + "\\" + "hoTools.chm");
        }
        #endregion

        void runSqlTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.runSqlTabPage();
        }

        void saveSqlTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.save();
        }

        void saveSqlTabAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.saveSqlTabAs();
        }


        void txtSearchTerm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtSearchTerm.Text = Clipboard.GetText();
            _sqlTabCntrls.runSqlTabPage();
        }


        #region Key down & Enter
        /// <summary>
        /// Overrides TextBox 'IsInputKey' to handle the enter key. Per default it isn't passed
        /// </summary>
        public class EnterTextBox : TextBox
        {
            protected override bool IsInputKey(Keys keyData)
            {
                if (keyData == Keys.Return)
                    return true;
                return base.IsInputKey(keyData);
            }

        }
        #endregion
        // text field
        // There are special keys like "Enter" which require an enabling by 
        //---------------------------------------------------------
        // see at:  protected override boolean IsInputKey(Keys keyData)
        void txtSearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _sqlTabCntrls.runSqlTabPage();
                e.Handled = true;
            }
        }

        void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.saveAll();
        }

        #region Drag one or more into TextBox
        /// <summary>
        /// Drag one or more files into TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControlSql_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;

        }
        #endregion

        void tabControlSql_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    Console.WriteLine(filePath);
                }
            }
        }

        private void splitContainer_DragOver(object sender, DragEventArgs e)
        {
            Repository.WriteOutput("Test", e.ToString(),0);
        }

        private void splitContainer_DragDrop(object sender, DragEventArgs e)
        {
            Repository.WriteOutput("Test", e.ToString(), 0);
        }

        private void tabControlSql_DragLeave(object sender, EventArgs e)
        {
            Repository.WriteOutput("Test", e.ToString(), 0);
        }

        #region Undo SQL Text
        void btnUndo_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.UndoText();
        }
        #endregion

        #region Redo SQL Text
        void btnRedo_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.RedoText();
        }
        #endregion




        /// <summary>
        /// Output the last EA SQL error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showSqlErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(SqlError.getEaSqlErrorFilePath());
        }

        

        /// <summary>
        /// Output the last from hoTools Query sent sql string to EA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lastSqlStringSentToEAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(SqlError.getHoToolsLastSqlFilePath());
           
        }

        /// <summary>
        /// CTRL+L Load TabPage from File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadTabCTRLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.loadTabPagePerFileDialog();
        }

        /// <summary>
        /// Save sql Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.save();
        }
        /// <summary>
        /// Save all sql Tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveAll_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.saveAll();
        }
        /// <summary>
        /// Save sql Tab As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveAs_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.saveAs();
        }

        /// <summary>
        /// Run sql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.runSqlTabPage();
        }

        /// <summary>
        /// Output Help of macros and templates in a text editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void templatesAndMacrosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string content = SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.MACROS_HELP);
            // write it do EA home (%appdata%Sparx System\EA\hoTools_SqlTemplatesAndMacros.txt)
            SqlError.writeSqlTemplatesAndMacros(content);
            // Show it in Editor
            Util.StartFile(SqlError.getSqlTemplatesAndMacrosFilePath());

        }

        private void contextMenuStripDataGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// Load Standard Scripts into EA 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadStandardScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new script group "hoTools"
            var group = new EaScriptGroup(Model, "hoTools",EaScriptGroup.EaScriptGroupType.NORMAL);
            if (! group.exists()) group.save();

            // get scripts to create
            ResourceManager rm = new ResourceManager("hoTools.Query.Resources.Scripts", Assembly.GetExecutingAssembly());

            // new script for script group "hoTools"
            string code = rm.GetString("hoDemo2ParScript");
            var script = new EaScript(Model, "hoDemo2Par", "Internal", "VBScript", group.GUID, code);
            script.save();

            // new script for script group "hoTools"
            code = rm.GetString("hoDemo3ParScript");
            script = new EaScript(Model, "hoDemo3Par", "Internal", "VBScript", group.GUID, code);
            script.save();
        }

        void QueryGUI_Resize(object sender, EventArgs e)
        {


        }
    }
}
