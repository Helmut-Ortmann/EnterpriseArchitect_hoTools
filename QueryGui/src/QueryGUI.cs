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

using System.Reflection;
using AddinFramework.Util.Script;

// Resource Manager



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
    [ProgId(Progid)]
    [ComDefaultInterface(typeof(IQueryGui))]
    public partial class QueryGui : AddinGui, IQueryGui
    {
        public const string Progid = "hoTools.QueryGUI";
        public const string TabulatorScript = "Script";
        public const string TabulatorSql = "SQL";

        /// <summary>
        /// Type of Addin. Use the same string for enum as the string to visualize
        /// </summary>
        private enum AddinType { Sql, Script};

        // default value for Addin Tab Name
        AddinType _addinType = AddinType.Sql;
        string _addinTabName = TabulatorSql;

        List<Script> _lscripts;  // list off all scripts
        DataTable _tableFunctions; // Scripts and Functions

        SqlTabPagesCntrl _sqlTabCntrls;  // TAB Control with its TabPages

        // settings
        FrmQueryAndScript _frmQueryAndScript;


        /// <summary>
        /// The selected row in script list
        /// </summary>
        int _rowScriptsIndex;

        // Coordinates of Close Rectangle relative to TabPage Caption Rectangle TopRight Position
        const int CloseButtonRectangleRightX = -15;
        const int CloseButtonRectangleTopY = 4;
        const int CloseButtonRectangleWidth = 11;
        const int CloseButtonRectangleHight = 16;


        #region Constructor

        /// <summary>
        /// Constructor QueryGUI. Constructor make the basic initialization. 
        /// The real initialization is done after Setting the Repository in setter of property:
        /// 'Repository'
        /// </summary>
        public QueryGui()
        {
            InitializeComponent();

            // set properties to enable drawing tab caption
            tabControlSql.Multiline = true;
            tabControlSql.SizeMode = TabSizeMode.FillToRight;
            tabControlSql.DrawMode = TabDrawMode.OwnerDrawFixed;
            //ResumeLayout(false);
            //PerformLayout();

            ResizeRedraw = true;


            //--------------------------------------------------------------------------------------
            // FileMenu
            //--------------------------------------------------------------------------------------
            // Load current Tab from file (CTRL+L Load)
            _loadTabCtrlLToolStripMenuItem.Text = SqlTabPagesCntrl.MenuLoadTabFileText;
            _loadTabCtrlLToolStripMenuItem.ToolTipText = SqlTabPagesCntrl.MenuLoadTabFileTooltip;

            // Load Tab from Recent file
            _loadTabFromRecentToolStripMenuItem.Text = SqlTabPagesCntrl.MenuLoadTabFromRecentFileText;
            _loadTabFromRecentToolStripMenuItem.ToolTipText = SqlTabPagesCntrl.MenuLoadTabFromRecentFileTooltip;
            //--------------------------------------------------------------------------------------
            // Reload
            _reloadTabToolStripMenuItem.Text = SqlTabPagesCntrl.MenuReLoadTabText;
            _reloadTabToolStripMenuItem.ToolTipText = SqlTabPagesCntrl.MenuReLoadTabTooltip;

            //--------------------------------------------------------------------------------------
            // New Tab
            _newTabToolStripMenuItem.Text = SqlTabPagesCntrl.MenuNewTabText;
            _newTabToolStripMenuItem.ToolTipText = SqlTabPagesCntrl.MenuNewTabTooltip;

            // New Tab from File with File Dialog
            _newTabWithFileDialogToolStripMenuItem.Text = SqlTabPagesCntrl.MenuNewTabWithFileDialogText;
            _newTabWithFileDialogToolStripMenuItem.ToolTipText = SqlTabPagesCntrl.MenuNewTabWithFileDialogTooltip;

            // New Tab from recent file
            _newTabFromRecentToolStripMenuItem.Text = SqlTabPagesCntrl.MenuNewTabFromRecentText;
            _newTabFromRecentToolStripMenuItem.ToolTipText = SqlTabPagesCntrl.MenuNewTabFromRecentTooltip;




            // Script
            InitScriptDataGrid();
            InitScriptDataTable();
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
        /// Initialize setting. Only call after
        /// <para/>- Tag (
        /// <para/>- Model
        /// <para/>- Settings
        /// updated
        /// </summary>
        /// <returns></returns>
        bool InitializeSettings()
        {
            // default 
            _addinType = AddinType.Sql;
            _addinTabName = TabulatorSql;
            if ((string)Tag != TabulatorSql)
            {
                _addinType = AddinType.Script;
                _addinTabName = TabulatorScript;
            }
            // set title
            lblTitle.Text = _addinTabName;

            // Tab Pages for *.sql queries update
            // Make sure the Container is initialized
            if (components == null)
            {
                components = new System.ComponentModel.Container();
            }
            // the sql tabulators might already be available
            if (_sqlTabCntrls == null)
            {
                _sqlTabCntrls = new SqlTabPagesCntrl(Model, AddinSettings, components, tabControlSql, txtSearchTerm,
                    _newTabFromRecentToolStripMenuItem,
                    _loadTabFromRecentToolStripMenuItem, _addinTabName);
            }

            if (tabControlSql.TabPages.Count == 0)
            {
                // first tab with Element Template
                _sqlTabCntrls.AddTab(SqlTemplates.GetTemplateText(SqlTemplates.SqlTemplateId.ElementTemplate));
            }


            // run for SQL / Query
            if (_addinType == AddinType.Sql)
            {
                // don't show Script container
                splitContainer.Panel2Collapsed = true;
                // don't show Menu item LoadScripts
                //loadStandardScriptsToolStripMenuItem.Visible = false;
            }
            else // run for Script (includes SQL / Query)
            {

                splitContainer.SplitterDistance = 330;
                // available script updates
                _lscripts = Script.GetEaMaticScripts(Model);
                UpdateTableFunctions();
            }

            return true;
        }

        
        #region initDataGrid
        void InitScriptDataGrid()
        {
            dataGridViewScripts.AutoGenerateColumns = false;

            dataGridViewScripts.DataSource = null;


            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Script",
                Name = "Script",
                HeaderText = @"Script"
            };
            dataGridViewScripts.Columns.Add(col);


            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Language",
                Name = "Language",
                HeaderText = @"Language"
            };
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Group",
                Name = "Group",
                HeaderText = @"Group"
            };
            dataGridViewScripts.Columns.Add(col);


            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Function",
                Name = "Function",
                HeaderText = @"Function"
            };
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ParCount",
                Name = "ParCount",
                HeaderText = @"Par count"
            };
            dataGridViewScripts.Columns.Add(col);

            col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Err",
                Name = "Err",
                HeaderText = @"Err"
            };
            dataGridViewScripts.Columns.Add(col);
        }
        #endregion
        #region initDataTable
         void InitScriptDataTable()
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
            _tableFunctions.Columns.AddRange(new[]
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
      void Close()
        {

        }



        /// <summary>
        /// Load all usable script. They shall contain 'EA-Matic'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLoadScripts_Click(object sender, EventArgs e)
        {
            _lscripts = Script.GetEaMaticScripts(Model);
            UpdateTableFunctions();
        }
        /// <summary>
        /// Compile, load script with may run on SQL Query result rows. The conditions:
        /// <para/>- Contains string 'EA-Matic'
        /// <para/>- With 2 or 3 parameters
        /// <para/>From Repository, MDG Technology folder, Registry (Tools, MDG, Advanced,..)
        /// </summary>
        /// <param name="isWithAll"></param>
         void UpdateTableFunctions(bool isWithAll=false)
        {
            _tableFunctions.Rows.Clear();
            // fill list
            foreach (Script script in _lscripts)
            {

                foreach (ScriptFunction function in script.Functions)
                {
                    // 2 or 3 parameters
                    if (isWithAll || (function.NumberOfParameters > 1 && function.NumberOfParameters < 4)) { 
                        var newRow = _tableFunctions.NewRow();
                        newRow["ScriptObj"] = script;
                        newRow["Script"] = script.Name;
                        newRow["Language"] = script.LanguageName;
                        newRow["Group"] = script.GroupName;
                        newRow["Err"] = script.ErrorMessage;

                        newRow["FunctionObj"] = function;
                        newRow["Function"] = function.Name;
                        newRow["ParCount"] = function.NumberOfParameters;
                        _tableFunctions.Rows.Add(newRow);
                    }
                }
                
            }
            // bind to grid
            // Select column to view
            dataGridViewScripts.DataSource = _tableFunctions;

        }

        /// <summary>
        /// Show error of the selected Script
        /// </summary>
        void ShowScriptErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridViewScripts.Rows[_rowScriptsIndex];
            string scriptName = row.Cells["Script"].Value as string;
            string functionName = row.Cells["Function"].Value as string;
            string err = row.Cells["Err"].Value as string;
            if (String.IsNullOrWhiteSpace(err))
            { MessageBox.Show("", $"Function compiled fine {scriptName}.{functionName}"); }
            else
            { MessageBox.Show($"Error:\r\n'{err}'", $"Error {scriptName}:{functionName}"); }


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
            _rowScriptsIndex = dataGridViewScripts.HitTest(e.X, e.Y).RowIndex;
        }

        /// <summary>
        /// Output the script code of the selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void showScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridViewRow rowToRun = dataGridViewScripts.Rows[_rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            

            var script = row["ScriptObj"] as Script;
            MessageBox.Show(script.Code, $"Code of {script.DisplayName}");

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
            DataGridViewRow rowToRun = dataGridViewScripts.Rows[_rowScriptsIndex];
            DataRowView row = rowToRun.DataBoundItem as DataRowView;
            var scriptFunction = row["FunctionObj"] as ScriptFunction;

            // replace templates, search term and more
            string sql = SqlTemplates.ReplaceMacro(Repository, textBox.Text, txtSearchTerm.Text);
            if (sql == "") return;

            // run SQL, Script and ask whether to execute, skip script or break all together
            GuiFunction.RunScriptWithAsk(Model, sql, scriptFunction, isWithAsk: isWithAsk);

            Cursor.Current = Cursors.Default;
        }

      

         void FileNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.AddTab();
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
            Rectangle closeButton = new Rectangle(e.Bounds.Right + CloseButtonRectangleRightX,
                                                  e.Bounds.Top + CloseButtonRectangleTopY,
                                                  CloseButtonRectangleWidth,
                                                  CloseButtonRectangleHight);
            
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
            Rectangle r = tabControlSql.GetTabRect(tabControlSql.SelectedIndex);
            Rectangle closeButton = new Rectangle(r.Right + CloseButtonRectangleRightX, 
                                                  r.Top + CloseButtonRectangleTopY,
                                                  CloseButtonRectangleWidth,
                                                  CloseButtonRectangleHight);
            if (closeButton.Contains(e.Location))
            {
                TabPage tabPage = tabControlSql.SelectedTab;
                _sqlTabCntrls.Close(tabPage);

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

                case AddinSettings.CustomerCfg.Var1:
                    EaService.AboutVar1(Release, configFilePath);
                    break;
                case AddinSettings.CustomerCfg.HoTools:
                    EaService.About(Release, configFilePath);
                    break;
                default:
                    EaService.About(Release, configFilePath);
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
            Help.ShowHelp(this, EaService.GetAssemblyPath() + "\\" + "hoTools.chm");
        }
        #endregion

        void runSqlTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.RunSqlTabPage();
        }

        void saveSqlTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.Save();
        }

        void saveSqlTabAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.SaveSqlTabAs();
        }


        void txtSearchTerm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtSearchTerm.Text = Clipboard.GetText();
            _sqlTabCntrls.RunSqlTabPage();
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
                _sqlTabCntrls.RunSqlTabPage();
                e.Handled = true;
            }
        }

        void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.SaveAll();
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
            Util.StartFile(SqlError.GetEaSqlErrorFilePath());
        }

        

        /// <summary>
        /// Output the last from hoTools Query sent sql string to EA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lastSqlStringSentToEAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.StartFile(SqlError.GetHoToolsLastSqlFilePath());
           
        }

        /// <summary>
        /// CTRL+L Load TabPage from File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadTabCtrlLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.LoadTabPagePerFileDialog();
        }

        /// <summary>
        /// Save sql Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.Save();
        }
        /// <summary>
        /// Save all sql Tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveAll_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.SaveAll();
        }
        /// <summary>
        /// Save sql Tab As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveAs_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.SaveAs();
        }

        /// <summary>
        /// Run sql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.RunSqlTabPage();
        }

        /// <summary>
        /// Output Help of macros and templates in a text editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void templatesAndMacrosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string content = SqlTemplates.GetTemplateText(SqlTemplates.SqlTemplateId.MacrosHelp);
            // write it do EA home (%appdata%Sparx System\EA\hoTools_SqlTemplatesAndMacros.txt)
            SqlError.WriteSqlTemplatesAndMacros(content);
            // Show it in Editor
            Util.StartFile(SqlError.GetSqlTemplatesAndMacrosFilePath());

        }

        private void contextMenuStripDataGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// Load Standard Scripts into EA: ScriptGroup:hoTools, Script: hoDemo2Par, hoDemo3Par 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadStandardScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new script group "hoTools"
            var group = new EaScriptGroup(Model, "hoTools",EaScriptGroup.EaScriptGroupType.Normal);
            if (! group.Exists()) group.Save();

            // get scripts to create
            ResourceManager rm = new ResourceManager("hoTools.Query.Resources.Scripts", Assembly.GetExecutingAssembly());

            // new script for script group "hoTools"
            string code = rm.GetString("hoDemo2ParScript");
            var script = new EaScript(Model, "hoDemo2Par", "Internal", "VBScript", group.Guid, code);
            script.Save();

            // new script for script group "hoTools"
            code = rm.GetString("hoDemo3ParScript");
            script = new EaScript(Model, "hoDemo3Par", "Internal", "VBScript", group.Guid, code);
            script.Save();

            // new script for script group "hoTools"
            code = rm.GetString("hoDemoRunSql");
            script = new EaScript(Model, "hoDemoRunSql", "Internal", "VBScript", group.Guid, code);
            script.Save();

           
            MessageBox.Show(@"ScriptGroup: hoTools
- Script1: hoDemo2Par
- Script2: hoDemo3Par
- Script3: hoDemoRunSql",
                @"Demo Scripts to run on query loaded.");
        }

        void QueryGUI_Resize(object sender, EventArgs e)
        {
            tabControlSql.Invalidate();

        }


       void reloadTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.ReloadTabPage();
        }

        /// <summary>
        /// New Tab via File Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _newTabWithFileDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sqlTabCntrls.AddTabWithFileDialog();
        }
    }
}
