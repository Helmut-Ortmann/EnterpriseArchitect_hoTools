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

namespace hoTools.Query
{
    /// <summary>
    /// SqlTabPagesCntrl creates and handles TabPages of a ControlTab to work with *.sql files.
    /// - Create Menu Items
    /// -- Templates SQL
    /// -- Templates Macros
    /// - Events
    /// - SQL File properties
    /// 
    /// </summary>
    public class SqlTabPagesCntrl
    {
        /// <summary>
        /// Setting with the file history.
        /// </summary>
        AddinSettings _settings;
        Model _model;
        System.ComponentModel.IContainer _components;
        TabControl _tabControl;
        TextBox _sqlTextBoxSearchTerm;


        /// <summary>
        /// Reusable ToolStripMenuItem: New Tab and Load Recent Files 
        /// </summary>
        public ToolStripMenuItem NewTabAndLoadRecentFileItem => _newTabAndLoadRecentFileItem;
        readonly ToolStripMenuItem _newTabAndLoadRecentFileItem = new ToolStripMenuItem("Recent Files");

        /// <summary>
        /// Reusable ToolStripMenuItem: Load Recent Files 
        /// </summary>
        public ToolStripMenuItem LoadRecentFileItem => _loadRecentFileItem;
        readonly ToolStripMenuItem _loadRecentFileItem  = new ToolStripMenuItem("Recent Files");


        const string DEFAULT_TAB_NAME = "noName";

        /// <summary>
        /// Constructor to initialize TabControl, create ToolStripItems (New Tab from, Recent Files) with file history. 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="settings"></param>
        /// <param name="components"></param>
        /// <param name="tabControl"></param>
        public SqlTabPagesCntrl(Model model, AddinSettings settings, 
            System.ComponentModel.IContainer components, 
            TabControl tabControl, TextBox sqlTextBoxSearchTerm)
        {
            _settings = settings;
            _model = model;
            _tabControl = tabControl;
            _components = components;
            _sqlTextBoxSearchTerm = sqlTextBoxSearchTerm;

            // ToolStripItem 'New Tab from' with history of files
            loadRecentFilesMenuItems(_newTabAndLoadRecentFileItem, newTabAnLoadFromHistoryEntry_Click);

            // ToolStripItem 'Recent Files' with history of files
            loadRecentFilesMenuItems(_loadRecentFileItem, loadFromHistoryEntry_Click);

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

            SqlFile sqlFile = new SqlFile($"{DEFAULT_TAB_NAME}{_tabControl.Controls.Count}.sql", false);
            tabPage.Tag = sqlFile;
            tabPage.Text = sqlFile.DisplayName;
            tabPage.ToolTipText = sqlFile.FullName;
            _tabControl.SelectTab(tabPage);

            //-----------------------------------------------------------------
            // Tab with ContextMenuStrip
            // Create a text box in TabPage for the SQL string
            TextBox sqlTextBox = new TextBox();
            sqlTextBox.Multiline = true;
            sqlTextBox.ScrollBars = ScrollBars.Both;
            sqlTextBox.AcceptsReturn = true;
            sqlTextBox.AcceptsTab = true;
            sqlTextBox.TextChanged += sqlTextBox_TextChanged;

                        // Set WordWrap to true to allow text to wrap to the next line.
            sqlTextBox.WordWrap = true;
            sqlTextBox.Modified = false;
            sqlTextBox.Dock = DockStyle.Fill;

            tabPage.Controls.Add(sqlTextBox);

            // ContextMenu
            ContextMenuStrip tabPageContextMenuStrip = new ContextMenuStrip(_components);

            // Load sql File into TabPage
            ToolStripMenuItem fileLoadMenuItem = new ToolStripMenuItem();
            fileLoadMenuItem.Text = "Load File";
            fileLoadMenuItem.Click += new System.EventHandler(this.fileLoadMenuItem_Click);

            // Save sql File from TabPage
            ToolStripMenuItem fileSaveMenuItem = new ToolStripMenuItem();
            fileSaveMenuItem.Text = "Save File";
            fileSaveMenuItem.Click += new System.EventHandler(this.fileSaveMenuItem_Click);

            // Save As sql File from TabPage
            ToolStripMenuItem fileSaveAsMenuItem = new ToolStripMenuItem();
            fileSaveAsMenuItem.Text = "Save File As..";
            fileSaveAsMenuItem.Click += new System.EventHandler(this.fileSaveAsMenuItem_Click);

            // New TabPage
            ToolStripMenuItem newTabMenuItem = new ToolStripMenuItem();
            newTabMenuItem.Text = "New Tab";
            newTabMenuItem.Click += new System.EventHandler(this.addTabMenuItem_Click);

            // Close TabPage
            ToolStripMenuItem closeMenuItem = new ToolStripMenuItem();
            closeMenuItem.Text = "Close Tab";
            closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);

            // Run sql File 
            ToolStripMenuItem fileRunMenuItem = new ToolStripMenuItem();
            fileRunMenuItem.Text = "Run sql";
            fileRunMenuItem.Click += new System.EventHandler(this.fileRunMenuItem_Click);


            //------------------------------------------------------------------------------------------------------------------
            // Insert Template
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem("Insert &Template");

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


            //-----------------------------------------------------------------------------------------------------------------
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

            insertMacroMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                insertMacroSearchTermMenuItem,
                insertPackageMenuItem,
                insertBranchMenuItem,
                insertCurrentIdMenuItem,
                insertCurrentGuidMenuItem,
                insertWcMenuItem
                });

            //----------------------------------------------------------------------------------------------------------

            // load File history in ToolStripMenuItem
            loadRecentFilesMenuItems(LoadRecentFileItem, loadFromHistoryEntry_Click);
            // New Tab with File History in ToolStripMenuItem
            loadRecentFilesMenuItems(NewTabAndLoadRecentFileItem, newTabAnLoadFromHistoryEntry_Click);

            //----------------------------------------------------------------------------------------------------------
            // ToolStripItem for
            // - TabPage
            // - SQL TextBox
            var toolStripItems = new ToolStripItem[] {
                fileLoadMenuItem,
                LoadRecentFileItem,           // Reusable LoadRecentFileItem (contains menuItems of recent files)
                newTabMenuItem,
                NewTabAndLoadRecentFileItem,  // Reusable NewTabAndLoadItem (contains menuItems of recent files)
                insertTemplateMenuItem,
                insertMacroMenuItem,
                fileRunMenuItem,
                closeMenuItem,
                fileSaveMenuItem,
                fileSaveAsMenuItem
                };

            // Context Menu
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip(_components);
            contextMenuStrip.Items.AddRange(toolStripItems);




            // Add ContextMenuStrip to TabControl an TextBox
            sqlTextBox.ContextMenuStrip = contextMenuStrip; 
            _tabControl.ContextMenuStrip = contextMenuStrip;
            return tabPage;
        }

        private void sqlTextBox_TextChanged(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            sqlFile.IsChanged = true;
            tabPage.Text = sqlFile.DisplayName;
        }

        /// <summary>
        /// Load RecentFiles MenuItems into MenuItemStrip
        /// </summary>
        /// <param name="loadRecentFileStripMenuItem"></param>
        /// <param name="eventHandler_Click"></param>
        private void loadRecentFilesMenuItems(ToolStripMenuItem loadRecentFileStripMenuItem, EventHandler eventHandler_Click)
        {
            // delete all previous entries
            loadRecentFileStripMenuItem.DropDownItems.Clear();
            // over all history files
            foreach (HistoryFile historyFile in _settings.sqlFiles.lSqlHistoryFilesCfg)
            {
                // ignore empty entries
                if (historyFile.FullName == "") continue;
                ToolStripMenuItem historyEntry = new ToolStripMenuItem();
                historyEntry.Text = historyFile.DisplayName;
                historyEntry.Tag = historyFile;
                historyEntry.ToolTipText = historyFile.FullName;
                historyEntry.Click += eventHandler_Click;
                loadRecentFileStripMenuItem.DropDownItems.Add(historyEntry);

            }
        }
        /// <summary>
        /// New Tab and Load from history item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newTabAnLoadFromHistoryEntry_Click(object sender, EventArgs e)
        {
            // Add a new Tab
            TabPage tabPage = addTab();

            // get TabPage
            SqlFile sqlFile = (SqlFile)tabPage.Tag;

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // Contend changed, need to be stored first
            if (sqlFile.IsChanged)
            {
                DialogResult result = MessageBox.Show($"Old File: '{sqlFile.FullName}'",
                    "First store old File? ", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                {
                    saveTabPage(tabPage, textBox);
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                }
            }
            HistoryFile historyFile = (HistoryFile)((ToolStripMenuItem)sender).Tag;
            string file = historyFile.FullName;
            loadFileForTabPage(tabPage, file);
        }

        /// <summary>
        /// Load file for tab Page
        /// </summary>
        /// <param name="tabPage"></param>
        /// <param name="file"></param>
        private static void loadFileForTabPage(TabPage tabPage, string file)
        {
            
            try
            {
                StreamReader myStream = new StreamReader(file);
                if (myStream != null)
                {
                    TextBox textBox = (TextBox)tabPage.Controls[0];
                    textBox.Text = myStream.ReadToEnd();
                    myStream.Close();

                    // set TabName
                    SqlFile sqlFile = (SqlFile)tabPage.Tag;
                    sqlFile.FullName = file;
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                    tabPage.Text = sqlFile.DisplayName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error reading File {file}");
                return;
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
            SqlFile sqlFile = (SqlFile)tabPage.Tag;

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            // Contend changed, need to be stored first
            if (sqlFile.IsChanged)
            {
                DialogResult result = MessageBox.Show($"Old File: '{sqlFile.FullName}'",
                    "First store old File? ", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes) { 
                    saveTabPage(tabPage, textBox);
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                }
            }


            // load File
            HistoryFile historyFile = (HistoryFile)((ToolStripMenuItem)sender).Tag;
            string file = historyFile.FullName;
            loadFileForTabPage(tabPage, file);
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
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;

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
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;
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
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;

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
        /// Load sql string from *.sql File into TabPage with TextBox inside.
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
                    SqlFile sqlFile = new SqlFile(openFileDialog.FileName);
                    sqlFile.IsChanged = true;
                    tabPageSql.Tag = sqlFile;

                    // set TabName
                    tabPageSql.Text = sqlFile.DisplayName;

                    // load File history in ToolStripMenuItem
                    loadRecentFilesMenuItems(LoadRecentFileItem, loadFromHistoryEntry_Click);
                    // New Tab with File History in ToolStripMenuItem
                    loadRecentFilesMenuItems(NewTabAndLoadRecentFileItem, newTabAnLoadFromHistoryEntry_Click);
                }
            }

        }
        /// <summary>
        /// Save As sql TabPage in *.sql File.
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void saveAsTabPage(TabPage tabPageSql, TextBox txtBoxSql)
        {
            SqlFile sqlFile = (SqlFile)tabPageSql.Tag;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = sqlFile.DirectoryName;
            // get File name
            saveFileDialog.FileName = sqlFile.FullName;
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
                    sqlFile.FullName = saveFileDialog.FileName;
                    sqlFile.IsChanged = false;

                    // set TabName
                    tabPageSql.Text = sqlFile.DisplayName;

                    // load File history in ToolStripMenuItem
                    loadRecentFilesMenuItems(LoadRecentFileItem, loadFromHistoryEntry_Click);
                    // New Tab with File History in ToolStripMenuItem
                    loadRecentFilesMenuItems(NewTabAndLoadRecentFileItem, newTabAnLoadFromHistoryEntry_Click);
                }
            }
        }
        /// <summary>
        /// Save sql TabPage in *.sql File.
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void saveTabPage(TabPage tabPageSql, TextBox txtBoxSql)
        {

            SqlFile sqlFile = (SqlFile)tabPageSql.Tag;
            if (sqlFile.FullName.Substring(0,6) == "noName" )
            {
                saveAsTabPage(tabPageSql, txtBoxSql);
                return;
            }

            try {
                StreamWriter myStream = new StreamWriter(sqlFile.FullName);
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    myStream.Write(txtBoxSql.Text);
                    myStream.Close();
                    sqlFile.IsChanged = false;


                    // set TabName
                    tabPageSql.Text = sqlFile.DisplayName;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error writing File {sqlFile.FullName}");
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

            SqlFile sqlFile = (SqlFile)tabPageSql.Tag;
            if (sqlFile.IsChanged)
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

        /// <summary>
        /// Run SQL for selected TabPage
        /// </summary>
        public void runSqlForSelectedTabPage()
        {
            Cursor.Current = Cursors.WaitCursor;

            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];
            GuiFunction.RunSql(_model, textBox.Text, _sqlTextBoxSearchTerm.Text);
            Cursor.Current = Cursors.Default;
        }
        void fileRunMenuItem_Click(object sender, EventArgs e)
        {
            runSqlForSelectedTabPage();
        }



    }
}
