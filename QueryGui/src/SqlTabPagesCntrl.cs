using System;
using System.IO;
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
        /// 

        // delegate to call update page from a different thread (SqlFile)
        public delegate void UpdatePageFromFile(TabPage tabPage, string fileNameChanged, bool notUpdateLastOpenedList);
        /// <summary>
        /// Delegate to update TAB Page
        /// </summary>
        public UpdatePageFromFile UpdatePageDelegate;

        public AddinSettings Settings { get; }
        Model _model;
        System.ComponentModel.IContainer _components;

        TabControl _tabControl;
        TextBox _sqlTextBoxSearchTerm;

        
        /// <summary>
        /// Reusable ToolStripMenuItem: File Menu: New Tab and Load Recent Files 
        /// </summary>
        public ToolStripMenuItem FileNewTabAndLoadRecentFileItem => _fileNewTabAndLoadRecentFileItem;
        readonly ToolStripMenuItem _fileNewTabAndLoadRecentFileItem;

        /// <summary>
        /// Reusable ToolStripMenuItem: File Menu: Load Recent Files in current Tab
        /// </summary>
        public ToolStripMenuItem FileLoadRecentFileItem => _fileLoadRecentFileItem;
        readonly ToolStripMenuItem _fileLoadRecentFileItem;

        readonly ToolStripMenuItem _newTabFromItem = new ToolStripMenuItem("New Tab from...");
        readonly ToolStripMenuItem _loadTabFromFileItem  = new ToolStripMenuItem("Load from...");


        const string DEFAULT_TAB_NAME = "noName";

        /// <summary>
        /// Constructor to initialize TabControl, create ToolStripItems (New Tab from, Recent Files) with file history. 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="settings"></param>
        /// <param name="components"></param>
        /// <param name="tabControl"></param>
        /// <param name="sqlTextBoxSearchTerm"></param>
        /// <param name="fileNewTabAndLoadRecentFileItem">File, New Tab from recent files</param>
        /// <param name="fileLoadRecentFileItem">File, Load Tab from recent files</param>
        public SqlTabPagesCntrl(Model model, AddinSettings settings, 
            System.ComponentModel.IContainer components, 
            TabControl tabControl, TextBox sqlTextBoxSearchTerm,
            ToolStripMenuItem fileNewTabAndLoadRecentFileItem,
            ToolStripMenuItem fileLoadRecentFileItem)
        {
            Settings = settings;
            _model = model;
            _tabControl = tabControl;
            _components = components;
            _sqlTextBoxSearchTerm = sqlTextBoxSearchTerm;

            _fileNewTabAndLoadRecentFileItem = fileNewTabAndLoadRecentFileItem;
            _fileLoadRecentFileItem = fileLoadRecentFileItem;

            // Set Function to update Tab Page
            UpdatePageDelegate = new UpdatePageFromFile(loadTabPageFromFile);

            loadOpenedTabsLastSession();

            // Load recent files into ToolStripMenu
            loadRecentFilesIntoToolStripItems();

        }

        /// <summary>
        /// Load all tabs which were opened in the last session
        /// </summary>
        void loadOpenedTabsLastSession()
        {
            // load last opened files into tab pages
            foreach (HistoryFile lastOpenedFile in Settings.sqlLastOpenedFiles.lSqlLastOpenedFilesCfg)
            {
                if (lastOpenedFile.FullName.Trim() == "") continue;
                TabPage tabPage = addTab();
                // load 
                loadTabPageFromFile(tabPage, lastOpenedFile.FullName, notUpdateLastOpenedList: true);

            }
        }

        /// <summary>
        /// Add a tab to the tab control and load content into the tab
        /// </summary>
        /// <param name="content">Content of the Tab</param>
        /// <returns></returns>
        public TabPage addTab(string content)
        {
            TabPage tabPage = addTab();
            loadTabPage(tabPage, content);
            return tabPage;
        }
        /// <summary>
        /// Add a tab empty Tab to the tab control
        /// </summary>
        /// <returns></returns>
        public TabPage addTab()
        {
            // create a new TabPage in TabControl
            TabPage tabPage = new TabPage();
            _tabControl.Controls.Add(tabPage);

            // default file name
            SqlFile sqlFile = new SqlFile(this, tabPage, $"{DEFAULT_TAB_NAME}{_tabControl.Controls.Count}.sql", false);
            tabPage.Tag = sqlFile;
            tabPage.Text = sqlFile.DisplayName;
            tabPage.ToolTipText = sqlFile.FullName;
            _tabControl.SelectTab(tabPage);

            //-----------------------------------------------------------------
            // Tab with ContextMenuStrip
            // Create a text box in TabPage for the SQL string
            var sqlTextBox = new TextBoxUndo(tabPage);

            tabPage.Controls.Add(sqlTextBox);

            // ContextMenu
            ContextMenuStrip tabPageContextMenuStrip = new ContextMenuStrip(_components);

            // Load sql File into TabPage
            ToolStripMenuItem _loadTabMenuItem = new ToolStripMenuItem();
            _loadTabMenuItem.Text = "Load File";
            _loadTabMenuItem.Click += new System.EventHandler(this.fileLoadMenuItem_Click);

            // Save sql File from TabPage
            ToolStripMenuItem fileSaveMenuItem = new ToolStripMenuItem();
            fileSaveMenuItem.Text = "Save File";
            fileSaveMenuItem.Click += new System.EventHandler(this.fileSaveMenuItem_Click);

            // Save As sql File from TabPage
            ToolStripMenuItem fileSaveAsMenuItem = new ToolStripMenuItem();
            fileSaveAsMenuItem.Text = "Save File As..";
            fileSaveAsMenuItem.Click += new System.EventHandler(this.fileSaveAsMenuItem_Click);

            // New TabPage
            ToolStripMenuItem _newTabMenuItem = new ToolStripMenuItem();
            _newTabMenuItem.Text = "New Tab";
            _newTabMenuItem.Click += new System.EventHandler(this.addTabMenuItem_Click);

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
            ToolStripMenuItem insertTemplateMenuItem = getTemplateMenuItems();


            //-----------------------------------------------------------------------------------------------------------------
            // Insert Macro
            ToolStripMenuItem insertMacroMenuItem = getMacroMenuItems();

            // Load recent files into toolStrip menu
            loadRecentFilesIntoToolStripItems();


            //----------------------------------------------------------------------------------------------------------
            // ToolStripItem for
            // - TabPage
            // - SQL TextBox
            var toolStripItems = new ToolStripItem[] {
                _loadTabMenuItem,                   // load Tab from file
                _loadTabFromFileItem,                // load Tab from recent file        
                new ToolStripSeparator(),
                _newTabMenuItem,                     // new Tab
                _newTabFromItem,       // new Tab from recent file 
                new ToolStripSeparator(),
                insertTemplateMenuItem,             // insert template
                insertMacroMenuItem,                // insert macro
                new ToolStripSeparator(),
                fileRunMenuItem,                    // run query
                new ToolStripSeparator(),
                fileSaveMenuItem,                   // save query
                fileSaveAsMenuItem,                 // save query as..
                closeMenuItem
                };

            // Context Menu
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip(_components);
            contextMenuStrip.Items.AddRange(toolStripItems);




            // Add ContextMenuStrip to TabControl an TextBox
            sqlTextBox.ContextMenuStrip = contextMenuStrip;
            _tabControl.ContextMenuStrip = contextMenuStrip;
            return tabPage;
        }

        #region Get Macro Menu Items
        /// <summary>
        /// Get Macro Menu Items. Every item contains:
        /// <para/>- Macro name
        /// <para/>- Tooltip
        /// <para/>- Tag Template
        /// <para/>- Event Handler insertTemplate_Click
        /// </summary>
        /// <returns>Tool Strip Menu with the items for each template</returns>
        ToolStripMenuItem getMacroMenuItems()
        {
            ToolStripMenuItem insertMacroMenuItem = new ToolStripMenuItem();
            insertMacroMenuItem.Text = "Insert &Macro";

            // Insert Macro
            ToolStripMenuItem insertMacroSearchTermMenuItem = new ToolStripMenuItem();
            insertMacroSearchTermMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.SEARCH_TERM);
            insertMacroSearchTermMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.SEARCH_TERM);
            insertMacroSearchTermMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.SEARCH_TERM);
            insertMacroSearchTermMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Package
            ToolStripMenuItem insertPackageMenuItem = new ToolStripMenuItem();
            insertPackageMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
            insertPackageMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
            insertPackageMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
            insertPackageMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Branch
            ToolStripMenuItem insertBranchMenuItem = new ToolStripMenuItem();
            insertBranchMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS); 
            insertBranchMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
            insertBranchMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
            insertBranchMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert InBranch
            ToolStripMenuItem insertInBranchMenuItem = new ToolStripMenuItem();
            insertInBranchMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            insertInBranchMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            insertInBranchMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            insertInBranchMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert CurrentID
            ToolStripMenuItem insertCurrentIdMenuItem = new ToolStripMenuItem();
            insertCurrentIdMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            insertCurrentIdMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            insertCurrentIdMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            insertCurrentIdMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert CurrentGUID
            ToolStripMenuItem insertCurrentGuidMenuItem = new ToolStripMenuItem();
            insertCurrentGuidMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            insertCurrentGuidMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            insertCurrentGuidMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            insertCurrentGuidMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert ConnectorID
            ToolStripMenuItem insertConnectorIdMenuItem = new ToolStripMenuItem();
            insertConnectorIdMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CONNECTOR_ID);
            insertConnectorIdMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CONNECTOR_ID);
            insertConnectorIdMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CONNECTOR_ID);
            insertConnectorIdMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Conveyed Item IDs
            ToolStripMenuItem insertConveyedItemIdsMenuItem = new ToolStripMenuItem();
            insertConveyedItemIdsMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);
            insertConveyedItemIdsMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);
            insertConveyedItemIdsMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);
            insertConveyedItemIdsMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert #WC#
            ToolStripMenuItem insertWcMenuItem = new ToolStripMenuItem();
            insertWcMenuItem.Text = "Insert "+ SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.WC);
            insertWcMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.WC);
            insertWcMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.WC);
            insertWcMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            insertMacroMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                insertMacroSearchTermMenuItem,
                insertPackageMenuItem,
                insertBranchMenuItem,
                insertCurrentIdMenuItem,
                insertCurrentGuidMenuItem,
                insertConnectorIdMenuItem,
                insertConveyedItemIdsMenuItem,
                insertWcMenuItem
                });
            return insertMacroMenuItem;
        }
        #endregion

        #region Get Template Menu Items
        /// <summary>
        /// Get Template Menu Items. Every item contains:
        /// <para/>- Template name
        /// <para/>- Tooltip
        /// <para/>- Tag Template
        /// <para/>- Event Handler insertTemplate_Click
        /// </summary>
        /// <returns>Tool Strip Menu with the items for each template</returns>
        ToolStripMenuItem getTemplateMenuItems()
        {
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem("Insert &Template");

            // Insert Element Template
            ToolStripMenuItem insertElementTemplateMenuItem = new ToolStripMenuItem();
            insertElementTemplateMenuItem.Text = "Insert Element Template";
            var id = SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE;
            insertElementTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
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

            //---------------------------------------------------------------------------------------------------------
            // DB Templates
            ToolStripMenuItem insertDBTemplateMenuItem = new ToolStripMenuItem();
            insertDBTemplateMenuItem.Text = "Insert DB dependent";

            // DB Other
            ToolStripMenuItem insertDBOtherTemplateMenuItem = new ToolStripMenuItem();
            insertDBOtherTemplateMenuItem.Text = "Other";
            insertDBOtherTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_OTHER);
            insertDBOtherTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_OTHER);
            insertDBOtherTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB ASA
            ToolStripMenuItem insertDBAsaTemplateMenuItem = new ToolStripMenuItem();
            insertDBAsaTemplateMenuItem.Text = "ASA";
            insertDBAsaTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_ASA);
            insertDBAsaTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_ASA);
            insertDBAsaTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB Firebird
            ToolStripMenuItem insertDBFirebirdTemplateMenuItem = new ToolStripMenuItem();
            insertDBFirebirdTemplateMenuItem.Text = "Firebird";
            insertDBFirebirdTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_FIREBIRD);
            insertDBFirebirdTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_FIREBIRD);
            insertDBFirebirdTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB Jet
            ToolStripMenuItem insertDBJetTemplateMenuItem = new ToolStripMenuItem();
            insertDBJetTemplateMenuItem.Text = "DB JET";
            insertDBJetTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_JET);
            insertDBJetTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_JET);
            insertDBJetTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB MySQL
            ToolStripMenuItem insertDBMySQLTemplateMenuItem = new ToolStripMenuItem();
            insertDBMySQLTemplateMenuItem.Text = "MYSQL";
            insertDBMySQLTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_MYSQL);
            insertDBMySQLTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_MYSQL);
            insertDBMySQLTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB OpenEdge
            ToolStripMenuItem insertDBOpenEdgeTemplateMenuItem = new ToolStripMenuItem();
            insertDBOpenEdgeTemplateMenuItem.Text = "OPENEDG";
            insertDBOpenEdgeTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_OPENEDGE);
            insertDBOpenEdgeTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_OPENEDGE);
            insertDBOpenEdgeTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB Oracle
            ToolStripMenuItem insertDBOracleTemplateMenuItem = new ToolStripMenuItem();
            insertDBOracleTemplateMenuItem.Text = "ORACLE";
            insertDBOracleTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_ORACLE);
            insertDBOracleTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_ORACLE);
            insertDBOracleTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB POSTGRES
            ToolStripMenuItem insertDBPostgresTemplateMenuItem = new ToolStripMenuItem();
            insertDBPostgresTemplateMenuItem.Text = "POSTGRES";
            insertDBPostgresTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_POSTGRES);
            insertDBPostgresTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_POSTGRES);
            insertDBPostgresTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB SQL Server
            ToolStripMenuItem insertDBSqlServerTemplateMenuItem = new ToolStripMenuItem();
            insertDBSqlServerTemplateMenuItem.Text = "SQL Server";
            insertDBSqlServerTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_SQLSVR);
            insertDBSqlServerTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_SQLSVR);
            insertDBSqlServerTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB dependent SQL
            insertDBTemplateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            insertDBJetTemplateMenuItem,
                insertDBAsaTemplateMenuItem,
                insertDBFirebirdTemplateMenuItem,
                insertDBMySQLTemplateMenuItem,
                insertDBOpenEdgeTemplateMenuItem,
                insertDBOracleTemplateMenuItem,
                insertDBPostgresTemplateMenuItem,
                insertDBSqlServerTemplateMenuItem
                });


            // Build item content Template
            insertTemplateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                insertElementTemplateMenuItem,
                insertElementTypeTemplateMenuItem,
                insertDiagramTemplateMenuItem,
                insertDiagramTypeTemplateMenuItem,
                insertPackageTemplateMenuItem,
                insertDiagramObjectTemplateMenuItem,
                insertAttributeTemplateMenuItem,
                insertOperationTemplateMenuItem,
                insertDBTemplateMenuItem
                });

            
            return insertTemplateMenuItem;
        }
        #endregion

        void sqlTextBox_TextChanged(object sender, EventArgs e)
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
        /// <param name="loadRecentFileStripMenuItem">Item to load recent files as drop down items</param>
        /// <param name="eventHandler_Click">Function to handle event</param>
         void loadRecentFilesMenuItems(ToolStripMenuItem loadRecentFileStripMenuItem, EventHandler eventHandler_Click)
        {
            // delete all previous entries
            loadRecentFileStripMenuItem.DropDownItems.Clear();
            // over all history files
            foreach (HistoryFile historyFile in Settings.sqlFiles.lSqlHistoryFilesCfg)
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
         void newTabAnLoadFromHistoryEntry_Click(object sender, EventArgs e)
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
                    save(tabPage);
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                }
            }
            HistoryFile historyFile = (HistoryFile)((ToolStripMenuItem)sender).Tag;
            string file = historyFile.FullName;
            loadTabPageFromFile(tabPage, file);
        }
        #region
        /// <summary>
        ///Undo changes in current active TextBoxUndo
        /// </summary>
        public void UndoText()
        {
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];
            textBox.UndoText();
        }
        #endregion

        #region
        /// <summary>
        /// Redo changes in current active TextBoxUndo
        /// </summary>
        public void RedoText()
        {
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];
            textBox.RedoText();
        }
        #endregion


        /// <summary>
        /// Load file for tab Page
        /// </summary>
        /// <param name="tabPage"></param>
        /// <param name="fileName"></param>
        /// <param name="notUpdateLastOpenedList">Don't update list of the opened files</param>
        public void loadTabPageFromFile(TabPage tabPage, string fileName, bool notUpdateLastOpenedList=false)
        {
            
            try
            {
                    TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];
                    textBox.Text = File.ReadAllText(fileName);

                    // set TabName
                    SqlFile sqlFile = (SqlFile)tabPage.Tag;
                    sqlFile.FullName = fileName;
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                    tabPage.Text = sqlFile.DisplayName;
                    if (! notUpdateLastOpenedList)
                    {
                        Settings.sqlLastOpenedFiles.insert(fileName);
                        Settings.save();
                    }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error reading File {fileName}");
                return;
            }
        }
        /// <summary>
        /// Load file for tab Page
        /// </summary>
        /// <param name="tabPage"></param>
        /// <param name="tabContent">What do load in Tab</param>
        public void loadTabPage(TabPage tabPage, string tabContent)
        {
                TextBox textBox = (TextBox)tabPage.Controls[0];
                textBox.Text = tabContent;
        }



        /// <summary>
        /// Load from history item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadFromHistoryEntry_Click(object sender, EventArgs e)
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            SqlFile sqlFile = (SqlFile)tabPage.Tag;

            // get TextBox
            TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];

            // Contend changed, need to be stored first
            if (sqlFile.IsChanged)
            {
                Settings.sqlLastOpenedFiles.remove(sqlFile.FullName);
                DialogResult result = MessageBox.Show($"Old File: '{sqlFile.FullName}'",
                    "First store old File? ", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes) { 
                    saveAs(tabPage);
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                }
            }


            // load File
            HistoryFile historyFile = (HistoryFile)((ToolStripMenuItem)sender).Tag;
            string file = historyFile.FullName;
            loadTabPageFromFile(tabPage, file);
        }
         void insertTemplate_Click(object sender, EventArgs e)
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            var textBox = (TextBoxUndo)tabPage.Controls[0];

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
         void fileSaveAsMenuItem_Click(object sender,  EventArgs e)
        {
            saveSqlTabAs();

        }

        
        #region saveTabAs
        /// <summary>
        /// Save current Tab into desired file
        /// </summary>
        public void saveSqlTabAs()
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            var textBox = (TextBoxUndo)tabPage.Controls[0];
            saveAs(tabPage);
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;
        }
        #endregion

        /// <summary>
        /// Event File Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileSaveMenuItem_Click(object sender, EventArgs e)
        {
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            save(tabPage);
        }
       
        /// <summary>
        /// Save all unchanged Tabs. 
        /// </summary>
        public void saveAll()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                save(tabPage);
            }
        }
       
        /// <summary>
        /// Event File Load fired by TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileLoadMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            

            loadTabPagePerFileDialog(tabPage);
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;

        }
        /// <summary>
        /// Add tab fired by TabControl or TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void addTabMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            var textBox = (TextBoxUndo)tabPage.Controls[0];

            addTab(SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE));


        }

        /// <summary>
        /// Event Close TabPage 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void closeMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            close(tabPage);

        }



        /// <summary>
        /// Load sql string from *.sql File into TabPage with TextBox inside.
        /// <para/>- Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPage"></param>
        void loadTabPagePerFileDialog(TabPage tabPage)
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
                    // get TextBox
                    var textBox = (TextBoxUndo)tabPage.Controls[0];
                    textBox.Text = myStream.ReadToEnd();
                    myStream.Close();
                    tabPage.Text = Path.GetFileName(openFileDialog.FileName);

                    // store the complete filename in settings
                    Settings.sqlFiles.insert(openFileDialog.FileName);
                    Settings.save();

                    // Store TabData in TabPage
                    SqlFile sqlFile = new SqlFile(this, tabPage, openFileDialog.FileName);
                    sqlFile.IsChanged = true;
                    tabPage.Tag = sqlFile;

                    // set TabName
                    tabPage.Text = sqlFile.DisplayName;

                    // Load recent files into ToolStripMenu
                    loadRecentFilesIntoToolStripItems();
                    
                }
            }

        }
        /// <summary>
        /// Re-Load Tab Page from file
        /// </summary>
        /// <param name="tabPage"></param>
        void reLoadTabPage(TabPage tabPage)
        {
            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            loadTabPage(tabPage, sqlFile.FullName);


        }
        
        /// <summary>
        /// Save As... TabPage in *.sql File.
        /// </summary>
        /// <param name="tabPage></param>
        void saveAs(TabPage tabPage)
        {

            SqlFile sqlFile = (SqlFile)tabPage.Tag;
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
                    var textBox = (TextBox)tabPage.Controls[0];
                    myStream.Write(textBox.Text);
                    myStream.Close();
                    tabPage.Text = Path.GetFileName(saveFileDialog.FileName);

                    // store the complete filename in settings
                    Settings.sqlFiles.insert(saveFileDialog.FileName);
                    Settings.save();

                    // Store TabData in TabPage
                    sqlFile.FullName = saveFileDialog.FileName;
                    sqlFile.IsChanged = false;

                    // set TabName
                    tabPage.Text = sqlFile.DisplayName;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                    loadRecentFilesIntoToolStripItems();
                    Settings.sqlLastOpenedFiles.insert(sqlFile.FullName);
                    Settings.save();
                }
            }
        }
        /// <summary>
        /// Update the following Menu Items with recent files:
        /// <para/>-File, Load Tab from..
        /// <para/>-File, Add Tab from..
        /// <para/>-Tab,  Load Tab from..
        /// <para/>-Tab,  Add Tab from..
        /// </summary>
        void loadRecentFilesIntoToolStripItems()
        {
            // File, Load Tab from
            loadRecentFilesMenuItems(_fileLoadRecentFileItem, loadFromHistoryEntry_Click);
            // File, Add Tab from..
            loadRecentFilesMenuItems(_fileNewTabAndLoadRecentFileItem, newTabAnLoadFromHistoryEntry_Click);

            // Tab,  Load Tab from..
            loadRecentFilesMenuItems(_loadTabFromFileItem, loadFromHistoryEntry_Click);
            // Tab,  Add Tab from..
            loadRecentFilesMenuItems(_newTabFromItem, newTabAnLoadFromHistoryEntry_Click);
        }

        /// <summary>
        /// Save sql TabPage in *.sql File.
        /// </summary>
        /// <param name="tabPage"></param>
        public void save(TabPage tabPage)
        {
            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            if (! sqlFile.IsPersistant )
            {
                saveAs(tabPage);
                return;
            }

            try {
                StreamWriter myStream = new StreamWriter(sqlFile.FullName);
                if (myStream != null)
                {
                    var textBox = (TextBox)tabPage.Controls[0];
                    myStream.Write(textBox.Text);
                    myStream.Close();
                    sqlFile.IsChanged = false;
                    Settings.sqlLastOpenedFiles.insert(sqlFile.FullName);
                    Settings.save();


                    // set TabName
                    tabPage.Text = sqlFile.DisplayName;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error writing File {sqlFile.FullName}");
                return;
            }
        }

        /// <summary>
        /// Close all Tab Pages
        /// </summary>
        public void closeAll()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                close(tabPage);
            }
        }
        /// <summary>
        /// Close TabPage
        /// - Ask to store content if changed
        /// </summary>
        /// <param name="tabPage"></param>
        public void close(TabPage tabPage)
        {

            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            if (sqlFile.IsChanged)
            {

                DialogResult result = MessageBox.Show($"", "Close TabPage: Sql has changed, store content?", MessageBoxButtons.YesNoCancel);
                switch (result) {
                    case DialogResult.OK:
                        save(tabPage);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;

                }
            }
            Settings.sqlLastOpenedFiles.remove(sqlFile.FullName);
            Settings.save();
            _tabControl.TabPages.Remove(tabPage);
        }

        #region runSqlTabPage
        /// <summary>
        /// Run SQL for selected TabPage
        /// </summary>
        public void runSqlTabPage()
        {
            if (_tabControl.SelectedIndex < 0) return;
            // get TabPage
            Cursor.Current = Cursors.WaitCursor;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            var textBox = (TextBox)tabPage.Controls[0];
            GuiFunction.RunSql(_model, textBox.Text, _sqlTextBoxSearchTerm.Text);
            Cursor.Current = Cursors.Default;
        }
        #endregion

        void fileRunMenuItem_Click(object sender, EventArgs e)
        {
            runSqlTabPage();
        }

        

    }
}
