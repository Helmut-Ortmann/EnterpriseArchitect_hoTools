using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using hoTools.Settings;
using hoTools.Utils.SQL;
using EAAddinFramework.Utils;

namespace hoTools.Scripts
{
    public class SqlTabCntrls
    {
        AddinSettings _settings;
        Model _model;
        System.ComponentModel.IContainer _components;
        TabControl _tabControl;

        /// <summary>
        /// Load Recent Files to load by ToolStripMenu
        /// </summary>
        ToolStripMenuItem _loadRecentFileItem = new ToolStripMenuItem();

        /// <summary>
        /// List of TabPages in TabControl
        /// </summary>
        //List<SqlTabCntrl> _tabCntrls = new List<SqlTabCntrl>();

        const string DEFAULT_TAB_NAME = "noName";

        /// <summary>
        /// Constructor to initialize TabControl
        /// </summary>
        /// <param name="model"></param>
        /// <param name="settings"></param>
        /// <param name="components"></param>
        /// <param name="tabControl"></param>
        public SqlTabCntrls(Model model, AddinSettings settings, System.ComponentModel.IContainer components, TabControl tabControl)
        {
            _settings = settings;
            _model = model;
            _tabControl = tabControl;
            _components = components;

        }
        /// <summary>
        /// Add a tab to the tab control
        /// </summary>
        /// <returns></returns>
        public TabPage addTab()
        {
            // create a new TabPage in TabControl
            TabPage tabPage = new TabPage();
            _tabControl.Controls.Add(tabPage);
            SqlTabCntrl sqlTabCntrl = new SqlTabCntrl(DEFAULT_TAB_NAME + _tabControl.Controls.Count.ToString() + ".sql", false);
            tabPage.Tag = sqlTabCntrl;
            tabPage.Text = sqlTabCntrl.DisplayName;
            _tabControl.SelectTab(tabPage);

            //-----------------------------------------------------------------
            // Tab with ContextMenuStrip
            // ==> Load
            // ==> Save
            // ==> SaveAs
            // ==> Close
            // Create a text box in TabPage
            TextBox textBox = new TextBox();
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Both;
            textBox.AcceptsReturn = true;
            textBox.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            textBox.WordWrap = true;
            textBox.Modified = false;
            textBox.Dock = DockStyle.Fill;

            // ContextMenu
            ContextMenuStrip tabSqlContextMenuStrip = new ContextMenuStrip(_components);
            // Load sql file into TabPage
            ToolStripMenuItem fileLoadMenuItem = new ToolStripMenuItem();
            fileLoadMenuItem.Text = "Load file";
            fileLoadMenuItem.Click += new System.EventHandler(this.fileLoadMenuItem_Click);

            // Save sql file from TabPage
            ToolStripMenuItem fileSaveMenuItem = new ToolStripMenuItem();
            fileSaveMenuItem.Text = "Save file";
            fileSaveMenuItem.Click += new System.EventHandler(this.fileSaveMenuItem_Click);

            // Save As sql file from TabPage
            ToolStripMenuItem fileSaveAsMenuItem = new ToolStripMenuItem();
            fileSaveAsMenuItem.Text = "Save as file";
            fileSaveAsMenuItem.Click += new System.EventHandler(this.fileSaveAsMenuItem_Click);

            // Add TabPage
            ToolStripMenuItem newTabMenuItem = new ToolStripMenuItem();
            newTabMenuItem.Text = "New tab";
            newTabMenuItem.Click += new System.EventHandler(this.addTabMenuItem_Click);

            // Close TabPage
            ToolStripMenuItem closeMenuItem = new ToolStripMenuItem();
            closeMenuItem.Text = "Close Tab";
            closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);

            // Load Recent File
            
            _loadRecentFileItem.Text = "Recent Files";



            tabSqlContextMenuStrip.Items.AddRange(new ToolStripItem[] {
            fileLoadMenuItem,
            fileSaveMenuItem,
            fileSaveAsMenuItem,
            newTabMenuItem,
            _loadRecentFileItem,
            closeMenuItem
            });

            textBox.ContextMenuStrip = tabSqlContextMenuStrip;
            _tabControl.ContextMenuStrip = tabSqlContextMenuStrip;  // works, have to decide which tab is selected


            //--------------------------------------------------------------------------------------
            // Text Box:
            // ==> InsertMacro
            // ==> InsertTemplate
            // ContextMenu
            ContextMenuStrip textSqlContextMenuStrip = new ContextMenuStrip(_components);

            // Insert Template
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem();
            insertTemplateMenuItem.Text = "Insert &Template";

            // Insert Element Template
            ToolStripMenuItem insertElementTemplateMenuItem = new ToolStripMenuItem();
            insertElementTemplateMenuItem.Text = "Insert Element Template";
            insertElementTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE);
            insertElementTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE);
            insertElementTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Element Type Template
            ToolStripMenuItem insertElementTypeTemplateMenuItem = new ToolStripMenuItem();
            insertElementTypeTemplateMenuItem.Text = "Insert Element Type Template";
            insertElementTypeTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TYPE_TEMPLATE);
            insertElementTypeTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TYPE_TEMPLATE);
            insertElementTypeTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Diagram Template
            ToolStripMenuItem insertDiagramTemplateMenuItem = new ToolStripMenuItem();
            insertDiagramTemplateMenuItem.Text = "Insert Diagram Template";
            insertDiagramTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_TEMPLATE);
            insertDiagramTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_TEMPLATE);
            insertDiagramTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Diagram Type Template
            ToolStripMenuItem insertDiagramTypeTemplateMenuItem = new ToolStripMenuItem();
            insertDiagramTypeTemplateMenuItem.Text = "Insert Diagram Type Template";
            insertDiagramTypeTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_TYPE_TEMPLATE);
            insertDiagramTypeTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_TYPE_TEMPLATE);
            insertDiagramTypeTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Package Template
            ToolStripMenuItem insertPackageTemplateMenuItem = new ToolStripMenuItem();
            insertPackageTemplateMenuItem.Text = "Insert Package Template";
            insertPackageTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_TEMPLATE);
            insertPackageTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_TEMPLATE);
            insertPackageTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert DiagramObject Template
            ToolStripMenuItem insertDiagramObjectTemplateMenuItem = new ToolStripMenuItem();
            insertDiagramObjectTemplateMenuItem.Text = "Insert Diagram Object Template";
            insertDiagramObjectTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_OBJECT_TEMPLATE);
            insertDiagramObjectTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_OBJECT_TEMPLATE);
            insertDiagramObjectTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Attribute Template
            ToolStripMenuItem insertAttributeTemplateMenuItem = new ToolStripMenuItem();
            insertAttributeTemplateMenuItem.Text = "Insert Attribute Template";
            insertAttributeTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.ATTRIBUTE_TEMPLATE);
            insertAttributeTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.ATTRIBUTE_TEMPLATE);
            insertAttributeTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Operation Template
            ToolStripMenuItem insertOperationTemplateMenuItem = new ToolStripMenuItem();
            insertOperationTemplateMenuItem.Text = "Insert Operation Template";
            insertOperationTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.OPERATION_TEMPLATE);
            insertOperationTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.OPERATION_TEMPLATE);
            insertOperationTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

           



            insertTemplateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                insertElementTemplateMenuItem,
                insertElementTypeTemplateMenuItem,
                insertDiagramTemplateMenuItem,
                insertDiagramTypeTemplateMenuItem,
                insertPackageTemplateMenuItem,
                insertDiagramObjectTemplateMenuItem,
                insertAttributeTemplateMenuItem,
                insertOperationTemplateMenuItem


                
                });
            // Insert Macro
            ToolStripMenuItem insertMacroMenuItem = new ToolStripMenuItem();
            insertMacroMenuItem.Text = "Insert &Macro";

                // Insert Macro
                ToolStripMenuItem insertMacroSearchTermMenuItem = new ToolStripMenuItem();
                insertMacroSearchTermMenuItem.Text = "Insert <Search Term>";
                insertMacroSearchTermMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.SEARCH_TERM);
                insertMacroSearchTermMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.SEARCH_TERM);
                insertMacroSearchTermMenuItem.Click += new System.EventHandler(insertTemplate_Click);

                // Insert Package
                ToolStripMenuItem insertPackageMenuItem = new ToolStripMenuItem();
                insertPackageMenuItem.Text = "Insert #Package#";
                insertPackageMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
                insertPackageMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
                insertPackageMenuItem.Click += new System.EventHandler(insertTemplate_Click);

                // Insert Branch
                ToolStripMenuItem insertBranchMenuItem = new ToolStripMenuItem();
                insertBranchMenuItem.Text = "Insert #Branch#";
                insertBranchMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
                insertBranchMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
                insertBranchMenuItem.Click += new System.EventHandler(insertTemplate_Click);

                // Insert InBranch
                ToolStripMenuItem insertInBranchMenuItem = new ToolStripMenuItem();
                insertInBranchMenuItem.Text = "Insert #InBranch#";
                insertInBranchMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
                insertInBranchMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
                insertInBranchMenuItem.Click += new System.EventHandler(insertTemplate_Click);

                // Insert CurrentID
                ToolStripMenuItem insertCurrentIdMenuItem = new ToolStripMenuItem();
                insertCurrentIdMenuItem.Text = "Insert #CurrentElementID#";
                insertCurrentIdMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
                insertCurrentIdMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
                insertCurrentIdMenuItem.Click += new System.EventHandler(insertTemplate_Click);

                // Insert CurrentGUID
                ToolStripMenuItem insertCurrentGuidMenuItem = new ToolStripMenuItem();
                insertCurrentGuidMenuItem.Text = "Insert #CurrentElementGUID#";
                insertCurrentGuidMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
                insertCurrentGuidMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
                insertCurrentGuidMenuItem.Click += new System.EventHandler(insertTemplate_Click);

                // Insert #WC#
                ToolStripMenuItem insertWcMenuItem = new ToolStripMenuItem();
                insertWcMenuItem.Text = "Insert #CurrentElementGUID#";
                insertWcMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.WC);
                insertWcMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.WC);
                insertWcMenuItem.Click += new System.EventHandler(insertTemplate_Click);




            insertMacroMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                insertMacroSearchTermMenuItem,
                insertPackageMenuItem,
                insertBranchMenuItem,
                insertCurrentIdMenuItem,
                insertCurrentGuidMenuItem,
                insertWcMenuItem,
                });



            textSqlContextMenuStrip.Items.AddRange(new ToolStripItem[] {
            insertTemplateMenuItem,
            insertMacroMenuItem
            });
            // load recent file items
            loadRectFiles(_loadRecentFileItem);
            // Add ContextMenuStrip to text box
            textBox.ContextMenuStrip = textSqlContextMenuStrip; 

            tabPage.Controls.Add(textBox);
            return tabPage;
        }
        private void loadRectFiles(ToolStripMenuItem loadRecentFileItem)
        {
            // delete all previous entries
            loadRecentFileItem.DropDownItems.Clear();
            // over all history files
            foreach (var file in _settings.sqlFiles.lSqlHistoryFilesCfg)
            {
                // ignore empty entries
                if (file.FullName == "") continue;
                ToolStripMenuItem historyEntry = new ToolStripMenuItem();
                historyEntry.Text = file.DisplayName;
                historyEntry.Tag = file;
                historyEntry.ToolTipText = file.FullName;
                historyEntry.Click += new System.EventHandler(loadFromHistoryEntry_Click);
                loadRecentFileItem.DropDownItems.Add(historyEntry);

            }
        }
        /// <summary>
        /// Load from history item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadFromHistoryEntry_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            SqlTabCntrl sqlTabCntrl = (SqlTabCntrl)tabPage.Tag;

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // Contend changed, need to be stored first
            if (sqlTabCntrl.IsChanged)
            {
                DialogResult result = MessageBox.Show($"Old file: '{sqlTabCntrl.FullName}'",
                    "First store old file? ", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                    saveTabPage(tabPage, textBox);
                {

                }
            }


            // load file
            FileHistory file = (FileHistory)((ToolStripMenuItem)sender).Tag;
            try
            {
                StreamReader myStream = new StreamReader(file.FullName);
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    textBox.Text = myStream.ReadToEnd();
                    myStream.Close();



                    // set TabName
                    sqlTabCntrl.FullName = file.FullName;
                    sqlTabCntrl.IsChanged = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error reading file {sqlTabCntrl.FullName}");
                return;
            }
        }
        private void insertTemplate_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // get the template to insert text
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            SqlTemplate template = (SqlTemplate)menuItem.Tag;

            // Insert text
            var selectionIndex = textBox.SelectionStart;
            var templateText = template.TemplateText;
            textBox.Text = textBox.Text.Insert(selectionIndex, templateText);
            textBox.SelectionStart = selectionIndex + templateText.Length;


        }
       

        /// <summary>
        /// Event File Save As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSaveAsMenuItem_Click(object sender,  EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];
            saveAsTabPage(tabPage, textBox);

        }
        /// <summary>
        /// Event File Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSaveMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            saveTabPage(tabPage, textBox);
        }
        /// <summary>
        /// Event File Load fired by TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileLoadMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            loadTabPage(tabPage, textBox);
           
        }
        /// <summary>
        /// Add tab fired by TabControl or TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTabMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            addTab();


        }

        /// <summary>
        /// Event Close TabPage As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];
            closeTabPage(tabPage, textBox);

        }



        /// <summary>
        /// Load sql string from *.sql file into TabPage with TextBox inside.
        /// - Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void loadTabPage(TabPage tabPageSql, TextBox txtBoxSql)
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

                    // store the complete filename in settings
                    _settings.sqlFiles.insert(openFileDialog.FileName);
                    _settings.save();

                    // Store TabData in TabPage
                    SqlTabCntrl sqlTabCntrl = new SqlTabCntrl(openFileDialog.FileName);
                    sqlTabCntrl.IsChanged = true;
                    tabPageSql.Tag = sqlTabCntrl;

                    // set TabName
                    tabPageSql.Text = sqlTabCntrl.DisplayName;

                    // load file history
                    loadRectFiles(_loadRecentFileItem);
                }
            }

        }
        /// <summary>
        /// Save As sql TabPage in *.sql file.
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void saveAsTabPage(TabPage tabPageSql, TextBox txtBoxSql)
        {
            SqlTabCntrl sqlTabCntrl = (SqlTabCntrl)tabPageSql.Tag;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = sqlTabCntrl.DirectoryName;
            // get file name
            saveFileDialog.FileName = sqlTabCntrl.FullName;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)

            {
                StreamWriter myStream = new StreamWriter(saveFileDialog.OpenFile());
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    myStream.Write(txtBoxSql.Text);
                    myStream.Close();
                    tabPageSql.Text = Path.GetFileName(saveFileDialog.FileName);

                    // store the complete filename in settings
                    _settings.sqlFiles.insert(saveFileDialog.FileName);
                    _settings.save();

                    // Store TabData in TabPage
                    sqlTabCntrl.FullName = saveFileDialog.FileName;
                    sqlTabCntrl.IsChanged = false;

                    // set TabName
                    tabPageSql.Text = sqlTabCntrl.DisplayName;

                    // load file history
                    loadRectFiles(_loadRecentFileItem);
                }
            }
        }
        /// <summary>
        /// Save sql TabPage in *.sql file.
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void saveTabPage(TabPage tabPageSql, TextBox txtBoxSql)
        {

            SqlTabCntrl sqlTabCntrl = (SqlTabCntrl)tabPageSql.Tag;
            if (sqlTabCntrl.FullName.Substring(0,6) == "noName" )
            {
                saveAsTabPage(tabPageSql, txtBoxSql);
                return;
            }

            try {
                StreamWriter myStream = new StreamWriter(sqlTabCntrl.FullName);
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    myStream.Write(txtBoxSql.Text);
                    myStream.Close();
                    sqlTabCntrl.IsChanged = false;


                    // set TabName
                    tabPageSql.Text = sqlTabCntrl.DisplayName;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error writing file {sqlTabCntrl.FullName}");
                return;
            }
        }
        /// <summary>
        /// Close TabPage
        /// - Ask to store content if changed
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void closeTabPage(TabPage tabPageSql, TextBox txtBoxSql)
        {

            SqlTabCntrl sqlTabCntrl = (SqlTabCntrl)tabPageSql.Tag;
            if (sqlTabCntrl.IsChanged)
            {

                DialogResult result = MessageBox.Show($"", "Close TabPage: Sql has changed, store content?", MessageBoxButtons.YesNoCancel);
                switch (result) {
                    case DialogResult.OK:
                        saveTabPage(tabPageSql, txtBoxSql);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;

                }
            }
            _tabControl.TabPages.Remove(_tabControl.SelectedTab);
        }



    }
}
