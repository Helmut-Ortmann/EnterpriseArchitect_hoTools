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
        const string SQL_TEXT_BOX_TOOLTIP =
@"CTRL+L                        Load sql from File
CTRL+R                          Run sql
CTRL+S                          Store sql
CTRL+SHFT+S                     Store sql All
\\ Comment                      Just your comment as you may know from C, Java, C#,..
<Search Term>                   Replaced by Text from the Search Text
#Branch#                        Selected Package, Replaced by nested recursive as comma separated list of PackageIDs            
#ConnectorID#                  Selected Connector, Replaced by ConnectorID
#ConveyedItemsIDS#             Selected Connector, Replaced by the Conveyed Items as comma separated list of ElementIDs
#CurrentElementGUID#            Alias for #CurrentItemGUID# (EA compatibility)
#CurrentElementID#              Alias for #CurrentItemID# (EA compatibility)
#CurrentItemGUID#               Selected Element, Diagram, Replaced by the GUID
#CurrentItemID#                 Selected Element, Diagram, Replaced by the ID
#DiagramElements_IDS#            Diagram Elements of selected Diagram / current Diagram
#DiagramSelectedElements_IDS#    Selected Diagram Objects of selected Diagram / current Diagram 
#InBranch#                      Selected Package, Replaced by nested recursive as comma separated list of PackageIDs  like 'IN (13,14,15)'
#Package#                       Selected Package, Diagram, Element, Attribute, Operation, Replaced by containing Package ID
#PackageID#                     Selected Package, Diagram, Element, Attribute, Operation, Replaced by containing Package ID
#TreeSelectedGUIDS#             In Browser selected Elements as a list of comma separated GUIDS like 'IN (##TreeSelectedGUIDS##)'
#WC#                            Wild card, you can also simple use * (will automatically replaced by the DB specific wild card)
#DB=ACCESS2007#                 DB specif SQL for ACCESS2007
#DB=ASA#                        DB specif SQL for ASA
#DB=FIREBIRD#                   DB specif SQL for FIREBIRD
#DB=JET#                        DB specif SQL for JET
#DB=MYSQL#                      DB specif SQL for My SQL
#DB=ORACLE#                     DB specif SQL for Oracle
#DB=POSTGRES#                   DB specif SQL for POSTGRES
#DB=SQLSVR#                     DB specif SQL for SQL Server
* or %                          DB specific Wild Card (automatic transformed into DB format)
? or _                          DB specific Wild Card (automatic transformed into DB format)
^ or !                          DB specific Wild Card (automatic transformed into DB format)
";

        /// <summary>
        /// Setting with the file history.
        /// </summary>
        /// 
        public AddinSettings Settings { get; }
        Model _model;
        System.ComponentModel.IContainer _components;

        TabControl _tabControl;
        TextBox _sqlTextBoxSearchTerm;

        /// <summary>
        /// The tab name of Addin (SQL or Script)
        /// </summary>
        string _addinTabName;

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
        readonly ToolStripMenuItem _loadTabFromFileItem = new ToolStripMenuItem("Load from...");


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
            ToolStripMenuItem fileLoadRecentFileItem, 
            string addinTabName)
        {
            Settings = settings;
            _model = model;
            _tabControl = tabControl;
            _components = components;
            _sqlTextBoxSearchTerm = sqlTextBoxSearchTerm;

            _fileNewTabAndLoadRecentFileItem = fileNewTabAndLoadRecentFileItem;
            _fileLoadRecentFileItem = fileLoadRecentFileItem;

            loadOpenedTabsFromLastSession();

            // Load recent files into ToolStripMenu
            loadRecentFilesIntoToolStripItems();
            _addinTabName = addinTabName;

        }




        /// <summary>
        /// Add a tab to the tab control and load content into the tab.If the content is empty nothing is loaded. 
        /// </summary>
        /// <param name="content">Content of the Tab</param>
        /// <returns></returns>
        public TabPage addTab(string content)
        {
            TabPage tabPage = addTab();
            if (content != "") loadTabPage(content);
            return tabPage;
        }
        /// <summary>
        /// Add an Tab to the tab control and load the Element Template as default. The Text box is unchanged because it's just a template.
        /// </summary>
        /// <returns></returns>
        public TabPage addTab(bool withDefaultTabContent = true)
        {
            // create a new TabPage in TabControl
            TabPage tabPage = new TabPage();

            // Create a new empty file
            SqlFile sqlFile = new SqlFile(this, tabPage, $"{DEFAULT_TAB_NAME}{_tabControl.Controls.Count}.sql", isChanged:false);
            tabPage.Tag = sqlFile;
            tabPage.Text = sqlFile.DisplayName;
            tabPage.ToolTipText = sqlFile.FullName;


            // add tab to TabControl
            _tabControl.Controls.Add(tabPage);
            _tabControl.SelectTab(tabPage);




            //-----------------------------------------------------------------
            // Tab with ContextMenuStrip
            // Create a text box in TabPage for the SQL string
            var sqlTextBox = new TextBoxUndo(tabPage);
            // load element template
            sqlTextBox.Text = SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE);
            sqlFile.IsChanged = false;
            tabPage.Text = sqlFile.DisplayName;  // after isChanged = false

            // register CTRL+S (store SQL) and CTRL+R (run SQL)
            sqlTextBox.KeyUp += sqlTextBox_KeyUp;
            //sqlTextBox.KeyDown += sqlTextBox_KeyDown;
            ToolTip toolTip = new ToolTip();
            toolTip.IsBalloon = false;
            toolTip.InitialDelay = 0;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(sqlTextBox, SQL_TEXT_BOX_TOOLTIP);


            tabPage.Controls.Add(sqlTextBox);


            // ContextMenu
            ContextMenuStrip tabPageContextMenuStrip = new ContextMenuStrip(_components);

            // Load sql File into TabPage
            ToolStripMenuItem _loadTabMenuItem = new ToolStripMenuItem();
            _loadTabMenuItem.Text = "Load File (CTRL+L)";
            _loadTabMenuItem.Click += fileLoadMenuItem_Click;

            // Save sql File from TabPage
            ToolStripMenuItem fileSaveMenuItem = new ToolStripMenuItem();
            fileSaveMenuItem.Text = "Save File (CTRL+S)";
            fileSaveMenuItem.Click += fileSaveMenuItem_Click;

            // Save all sql files 
            ToolStripMenuItem fileSaveAllMenuItem = new ToolStripMenuItem();
            fileSaveAllMenuItem.Text = "Save All File (CTRL+SHFT+S)";
            fileSaveAllMenuItem.Click += fileSaveMenuItem_Click;

            // Save As sql File from TabPage
            ToolStripMenuItem fileSaveAsMenuItem = new ToolStripMenuItem();
            fileSaveAsMenuItem.Text = "Save File As..";
            fileSaveAsMenuItem.Click += fileSaveAsMenuItem_Click;

            // New TabPage
            ToolStripMenuItem _newTabMenuItem = new ToolStripMenuItem();
            _newTabMenuItem.Text = "New Tab ";
            _newTabMenuItem.Click += addTabMenuItem_Click;

            // Close TabPage
            ToolStripMenuItem closeMenuItem = new ToolStripMenuItem();
            closeMenuItem.Text = "Close Tab";
            closeMenuItem.Click += closeMenuItem_Click;

            // Run sql File 
            ToolStripMenuItem fileRunMenuItem = new ToolStripMenuItem();
            fileRunMenuItem.Text = "Run sql (CTRL+R)";
            fileRunMenuItem.Click += fileRunMenuItem_Click;


            // Run selected sql 
            ToolStripMenuItem fileRunSelectedMenuItem = new ToolStripMenuItem();
            fileRunSelectedMenuItem.Text = "Run selected sql";
            fileRunSelectedMenuItem.Click += fileRunSelectedMenuItem_Click;


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
                fileRunMenuItem,                    // run sql
                fileRunSelectedMenuItem,            // run sql for selected area
                // run query
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
            insertMacroSearchTermMenuItem.Click += insertTemplate_Click;

            // Insert Package
            ToolStripMenuItem insertPackageMenuItem = new ToolStripMenuItem();
            insertPackageMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
            insertPackageMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
            insertPackageMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.PACKAGE_ID);
            insertPackageMenuItem.Click += insertTemplate_Click;

            // Insert Branch
            ToolStripMenuItem insertBranchMenuItem = new ToolStripMenuItem();
            insertBranchMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
            insertBranchMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
            insertBranchMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.BRANCH_IDS);
            insertBranchMenuItem.Click += insertTemplate_Click;

            // Insert InBranch
            ToolStripMenuItem insertInBranchMenuItem = new ToolStripMenuItem();
            insertInBranchMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            insertInBranchMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            insertInBranchMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            insertInBranchMenuItem.Click += insertTemplate_Click;

            // Insert CurrentID
            ToolStripMenuItem insertCurrentIdMenuItem = new ToolStripMenuItem();
            insertCurrentIdMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            insertCurrentIdMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            insertCurrentIdMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            insertCurrentIdMenuItem.Click += insertTemplate_Click;

            // Insert CurrentGUID
            ToolStripMenuItem insertCurrentGuidMenuItem = new ToolStripMenuItem();
            insertCurrentGuidMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            insertCurrentGuidMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            insertCurrentGuidMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            insertCurrentGuidMenuItem.Click += insertTemplate_Click;

            // Insert ConnectorID
            ToolStripMenuItem insertConnectorIdMenuItem = new ToolStripMenuItem();
            insertConnectorIdMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CONNECTOR_ID);
            insertConnectorIdMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CONNECTOR_ID);
            insertConnectorIdMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CONNECTOR_ID);
            insertConnectorIdMenuItem.Click += insertTemplate_Click;

            // Insert Conveyed Item IDs
            ToolStripMenuItem insertConveyedItemIdsMenuItem = new ToolStripMenuItem();
            insertConveyedItemIdsMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);
            insertConveyedItemIdsMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);
            insertConveyedItemIdsMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);
            insertConveyedItemIdsMenuItem.Click += insertTemplate_Click;


            // Insert Diagram Selected Objects_IDS Item IDs
            ToolStripMenuItem insertDiagramSelectedElements_IDSMenuItem = new ToolStripMenuItem();
            insertDiagramSelectedElements_IDSMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.DiagramSelectedElements_IDS);
            insertDiagramSelectedElements_IDSMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DiagramSelectedElements_IDS);
            insertDiagramSelectedElements_IDSMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DiagramSelectedElements_IDS);
            insertDiagramSelectedElements_IDSMenuItem.Click += insertTemplate_Click;

            // Insert Diagram Objects_IDS Item IDs
            ToolStripMenuItem insertDiagramElements_IDSMenuItem = new ToolStripMenuItem();
            insertDiagramElements_IDSMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.DiagramElements_IDS);
            insertDiagramElements_IDSMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DiagramElements_IDS);
            insertDiagramElements_IDSMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DiagramElements_IDS);
            insertDiagramElements_IDSMenuItem.Click += insertTemplate_Click;


            // Insert Diagram IDs
            ToolStripMenuItem insertDiagramElements_IDMenuItem = new ToolStripMenuItem();
            var id = SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_ID;
            insertDiagramElements_IDMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(id);
            insertDiagramElements_IDMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDiagramElements_IDMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDiagramElements_IDMenuItem.Click += insertTemplate_Click;

            // Tree selected GUIDs
            ToolStripMenuItem insertTreeSelectedGUIDSMenuItem = new ToolStripMenuItem();
            insertTreeSelectedGUIDSMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.TREE_SELECTED_GUIDS);
            insertTreeSelectedGUIDSMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.TREE_SELECTED_GUIDS);
            insertTreeSelectedGUIDSMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.TREE_SELECTED_GUIDS);
            insertTreeSelectedGUIDSMenuItem.Click += insertTemplate_Click;

            // newGuid
            ToolStripMenuItem insertNewGuid = new ToolStripMenuItem();
            id = SqlTemplates.SQL_TEMPLATE_ID.NEW_GUID;
            insertNewGuid.Text = "Insert " + SqlTemplates.getTemplateText(id);
            insertNewGuid.ToolTipText = SqlTemplates.getTooltip(id);
            insertNewGuid.Tag = SqlTemplates.getTemplate(id);
            insertNewGuid.Click += insertTemplate_Click;

            // Insert #WC#
            ToolStripMenuItem insertWcMenuItem = new ToolStripMenuItem();
            insertWcMenuItem.Text = "Insert " + SqlTemplates.getTemplateText(SqlTemplates.SQL_TEMPLATE_ID.WC);
            insertWcMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.WC);
            insertWcMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.WC);
            insertWcMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            insertMacroMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                insertMacroSearchTermMenuItem,
                new ToolStripSeparator(),
                insertPackageMenuItem,
                insertBranchMenuItem,
                new ToolStripSeparator(),
                insertCurrentIdMenuItem,
                insertCurrentGuidMenuItem,
                new ToolStripSeparator(),
                insertConnectorIdMenuItem,
                insertConveyedItemIdsMenuItem,
                new ToolStripSeparator(),
                insertDiagramElements_IDMenuItem,
                insertDiagramElements_IDSMenuItem,
                insertDiagramSelectedElements_IDSMenuItem,
                new ToolStripSeparator(),
                insertNewGuid,
                new ToolStripSeparator(),
                insertTreeSelectedGUIDSMenuItem,
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

            // Insert Branch Template
            ToolStripMenuItem insertBranchTemplateMenuItem = new ToolStripMenuItem();
            insertBranchTemplateMenuItem.Text = "Insert Branch Template";
            var id = SqlTemplates.SQL_TEMPLATE_ID.BRANCH_TEMPLATE;
            insertBranchTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertBranchTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertBranchTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert CurrentItemId Template
            ToolStripMenuItem insertCurrentItemIdTemplateMenuItem = new ToolStripMenuItem();
            insertCurrentItemIdTemplateMenuItem.Text = "Insert CurrentItemId Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_ID_TEMPLATE;
            insertCurrentItemIdTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertCurrentItemIdTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertCurrentItemIdTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert CurrentItemGuid Template
            ToolStripMenuItem insertCurrentItemGuidTemplateMenuItem = new ToolStripMenuItem();
            insertCurrentItemGuidTemplateMenuItem.Text = "Insert CurrentItemGuid Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.CURRENT_ITEM_GUID_TEMPLATE;
            insertCurrentItemGuidTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertCurrentItemGuidTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertCurrentItemGuidTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert ConveyedItemIDS Template
            ToolStripMenuItem insertConveyedtemIDSTemplateMenuItem = new ToolStripMenuItem();
            insertConveyedtemIDSTemplateMenuItem.Text = "Insert ConveyedItems from selected Connector Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.CONVEYED_ITEMS_FROM_CONNECTOR_TEMPLATE;
            insertConveyedtemIDSTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertConveyedtemIDSTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertConveyedtemIDSTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Connectors from Element Template
            ToolStripMenuItem insertConnectorsFromElementTemplateMenuItem = new ToolStripMenuItem();
            insertConnectorsFromElementTemplateMenuItem.Text = "Insert Connectors from Element Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.CONNECTORS_FROM_ELEMENT_TEMPLATE;
            insertConnectorsFromElementTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertConnectorsFromElementTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertConnectorsFromElementTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);


            // Insert Element Template
            ToolStripMenuItem insertElementTemplateMenuItem = new ToolStripMenuItem();
            insertElementTemplateMenuItem.Text = "Insert Element Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TEMPLATE;
            insertElementTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertElementTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertElementTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Element Type Template
            ToolStripMenuItem insertElementTypeTemplateMenuItem = new ToolStripMenuItem();
            insertElementTypeTemplateMenuItem.Text = "Insert Element Type Template";
            insertElementTypeTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TYPE_TEMPLATE);
            insertElementTypeTemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.ELEMENT_TYPE_TEMPLATE);
            insertElementTypeTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);


            // Insert Diagram Elements Template
            ToolStripMenuItem insertDiagramElementTemplateMenuItem = new ToolStripMenuItem();
            insertDiagramElementTemplateMenuItem.Text = "Insert Diagram Elements Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_ELEMENTS_IDS_TEMPLATE;
            insertDiagramElementTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDiagramElementTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDiagramElementTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // Insert Diagram Selected Elements Template
            ToolStripMenuItem insertDiagramSelectedElementTemplateMenuItem = new ToolStripMenuItem();
            insertDiagramSelectedElementTemplateMenuItem.Text = "Insert Diagram selected Elements Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.DIAGRAM_SELECTED_ELEMENTS_IDS_TEMPLATE;
            insertDiagramSelectedElementTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDiagramSelectedElementTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDiagramSelectedElementTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);


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

            // Insert Demo Run Script Template
            ToolStripMenuItem insertDemoRunScriptTemplateMenuItem = new ToolStripMenuItem();
            insertDemoRunScriptTemplateMenuItem.Text = "Insert Demo Run Script Template";
            id = SqlTemplates.SQL_TEMPLATE_ID.DEMO_RUN_SCRIPT_TEMPLATE;
            insertDemoRunScriptTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDemoRunScriptTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDemoRunScriptTemplateMenuItem.Click += insertTemplate_Click;

            //----------------------------------------------------------------------------------
            // Insert Element in current Package
            ToolStripMenuItem insertItemIntoPackageMenuItem = new ToolStripMenuItem();
            id = SqlTemplates.SQL_TEMPLATE_ID.INSERT_ITEM_IN_PACKAGE_TEMPLATE;
            insertItemIntoPackageMenuItem.Text = "Insert Insert Template";
            insertItemIntoPackageMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertItemIntoPackageMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertItemIntoPackageMenuItem.Click += insertTemplate_Click;

            // Update current selected Item
            ToolStripMenuItem updateCurrentElementMenuItem = new ToolStripMenuItem();
            id = SqlTemplates.SQL_TEMPLATE_ID.UPDATE_ITEM_TEMPLATE;
            updateCurrentElementMenuItem.Text = "Insert Update Template";
            updateCurrentElementMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            updateCurrentElementMenuItem.Tag = SqlTemplates.getTemplate(id);
            updateCurrentElementMenuItem.Click += insertTemplate_Click;

            // Delete tree selected Items
            ToolStripMenuItem deleteTreeSelectedItemsMenuItem = new ToolStripMenuItem();
            id = SqlTemplates.SQL_TEMPLATE_ID.DELETED_TREE_SELECTED_ITEMS;
            deleteTreeSelectedItemsMenuItem.Text = "Insert Delete Template";
            deleteTreeSelectedItemsMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            deleteTreeSelectedItemsMenuItem.Tag = SqlTemplates.getTemplate(id);
            deleteTreeSelectedItemsMenuItem.Click += insertTemplate_Click;

            //---------------------------------------------------------------------------------------------------------
            // DB Templates
            ToolStripMenuItem insertDBTemplateMenuItem = new ToolStripMenuItem();
            insertDBTemplateMenuItem.Text = "Insert DB dependent";

            // DB ACESS7007
            ToolStripMenuItem insertDBACCESS2007TemplateMenuItem = new ToolStripMenuItem();
            insertDBACCESS2007TemplateMenuItem.Text = "ACCESS2007";
            insertDBACCESS2007TemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(SqlTemplates.SQL_TEMPLATE_ID.DB_ACCESS2007);
            insertDBACCESS2007TemplateMenuItem.Tag = SqlTemplates.getTemplate(SqlTemplates.SQL_TEMPLATE_ID.DB_ACCESS2007);
            insertDBACCESS2007TemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB ASA
            ToolStripMenuItem insertDBAsaTemplateMenuItem = new ToolStripMenuItem();
            insertDBAsaTemplateMenuItem.Text = "ASA";


            id = SqlTemplates.SQL_TEMPLATE_ID.DB_ASA;
            insertDBAsaTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBAsaTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBAsaTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB Firebird
            ToolStripMenuItem insertDBFirebirdTemplateMenuItem = new ToolStripMenuItem();
            insertDBFirebirdTemplateMenuItem.Text = "Firebird";
            id = SqlTemplates.SQL_TEMPLATE_ID.DB_FIREBIRD;
            insertDBFirebirdTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBFirebirdTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBFirebirdTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB Jet
            ToolStripMenuItem insertDBJetTemplateMenuItem = new ToolStripMenuItem();
            insertDBJetTemplateMenuItem.Text = "JET";
            id = SqlTemplates.SQL_TEMPLATE_ID.DB_JET;
            insertDBJetTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBJetTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBJetTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB MySQL
            ToolStripMenuItem insertDBMySQLTemplateMenuItem = new ToolStripMenuItem();
            insertDBMySQLTemplateMenuItem.Text = "MYSQL";
            id = SqlTemplates.SQL_TEMPLATE_ID.DB_MYSQL;
            insertDBMySQLTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBMySQLTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBMySQLTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB Oracle
            ToolStripMenuItem insertDBOracleTemplateMenuItem = new ToolStripMenuItem();
            insertDBOracleTemplateMenuItem.Text = "ORACLE";
            id = SqlTemplates.SQL_TEMPLATE_ID.DB_ORACLE;
            insertDBOracleTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBOracleTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBOracleTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB POSTGRES
            ToolStripMenuItem insertDBPostgresTemplateMenuItem = new ToolStripMenuItem();
            insertDBPostgresTemplateMenuItem.Text = "POSTGRES";
            id = SqlTemplates.SQL_TEMPLATE_ID.DB_POSTGRES;
            insertDBPostgresTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBPostgresTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBPostgresTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB SQL Server
            ToolStripMenuItem insertDBSqlServerTemplateMenuItem = new ToolStripMenuItem();
            insertDBSqlServerTemplateMenuItem.Text = "SQL Server";
            id = SqlTemplates.SQL_TEMPLATE_ID.DB_SQLSVR;
            insertDBSqlServerTemplateMenuItem.ToolTipText = SqlTemplates.getTooltip(id);
            insertDBSqlServerTemplateMenuItem.Tag = SqlTemplates.getTemplate(id);
            insertDBSqlServerTemplateMenuItem.Click += new System.EventHandler(insertTemplate_Click);

            // DB dependent SQL
            insertDBTemplateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            insertDBJetTemplateMenuItem,
                insertDBAsaTemplateMenuItem,
                insertDBACCESS2007TemplateMenuItem,
                insertDBFirebirdTemplateMenuItem,
                insertDBMySQLTemplateMenuItem,
                insertDBOracleTemplateMenuItem,
                insertDBPostgresTemplateMenuItem,
                insertDBSqlServerTemplateMenuItem
                });


            // Build item content Template
            insertTemplateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                insertBranchTemplateMenuItem,
                insertPackageTemplateMenuItem,
                new ToolStripSeparator(),
                insertCurrentItemIdTemplateMenuItem,
                insertCurrentItemGuidTemplateMenuItem,
                new ToolStripSeparator(),
                insertConveyedtemIDSTemplateMenuItem,
                insertConnectorsFromElementTemplateMenuItem,
                new ToolStripSeparator(),
                insertElementTemplateMenuItem,
                insertElementTypeTemplateMenuItem,
                new ToolStripSeparator(),
                insertDiagramTemplateMenuItem,
                insertDiagramTypeTemplateMenuItem,
                insertDiagramObjectTemplateMenuItem,
                new ToolStripSeparator(),
                insertDiagramElementTemplateMenuItem,
                insertDiagramSelectedElementTemplateMenuItem,
                new ToolStripSeparator(),
                insertAttributeTemplateMenuItem,
                insertOperationTemplateMenuItem,

                new ToolStripSeparator(),  // Insert, Update, Delete
                insertItemIntoPackageMenuItem,
                updateCurrentElementMenuItem,
                deleteTreeSelectedItemsMenuItem,
                new ToolStripSeparator(),
                insertDemoRunScriptTemplateMenuItem,
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
            foreach (HistoryFile historyFile in Settings.HistorySqlFiles.lSqlHistoryFilesCfg)
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
        void newTabAndLoadFromHistoryEntry_Click(object sender, EventArgs e)
        {
            // Add a new Tab
            TabPage tabPage = addTab("");

            // get TabPage
            SqlFile sqlFile = (SqlFile)tabPage.Tag;

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            
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
        public void loadTabPageFromFile(TabPage tabPage, string fileName, bool notUpdateLastOpenedList = false)
        {

            try
            {
                 TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];

                // set TabName
                SqlFile sqlFile = (SqlFile)tabPage.Tag;
                sqlFile.FullName = fileName;
                textBox.Text = sqlFile.load(); // don't move behind sqlFile.IsChange=false !!!!
                sqlFile.IsChanged = false;
                tabPage.ToolTipText = sqlFile.FullName;
                tabPage.Text = sqlFile.DisplayName;


                if (!notUpdateLastOpenedList)
                {
                    Settings.LastOpenedFiles.Insert(fileName);
                    Settings.Save();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error reading File {fileName}");
                return;
            }
        }
        /// <summary>
        /// Load string for tab Page
        /// </summary>
        /// <param name="tabContent">What do load in Tab</param>
        public void loadTabPage(string tabContent)
        {
            TabPage tabPage;
            // no tab exists
            if (_tabControl.TabPages.Count == 0)
            {
                tabPage = addTab();
            }
            else
            {

                // get TabPage
                if (_tabControl.SelectedIndex < 0) return;
                tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            }
            TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];
            textBox.Text = tabContent;


        }



        /// <summary>
        /// Load from history item in active tab. If no active tab exists create one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void loadFromHistoryEntry_Click(object sender, EventArgs e)
        {
            TabPage tabPage;
            SqlFile sqlFile;
            // no tab exists
            if (_tabControl.TabPages.Count == 0)
            {
                tabPage = addTab("");
                sqlFile = (SqlFile)tabPage.Tag;
                sqlFile.IsChanged = false; // new tab just created
            }
            else
            {
                // get TabPage
                if (_tabControl.SelectedIndex < 0) return;
                tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
                sqlFile = (SqlFile)tabPage.Tag;
            }


            // get TextBox
            TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];

            // Contend changed, need to be stored first
            if (sqlFile.IsChanged)
            {
                Settings.LastOpenedFiles.Remove(sqlFile.FullName);
                DialogResult result = MessageBox.Show($"Old File: '{sqlFile.FullName}'",
                    "First store old File? ", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                {
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


            // insert text and replace a selected range
            var templateText = template.TemplateText;
            int iSelectionStart = textBox.SelectionStart;
            string sBefore = textBox.Text.Substring(0, iSelectionStart);
            string sAfter = textBox.Text.Substring(iSelectionStart + textBox.SelectionLength);
            textBox.Text = sBefore + templateText + sAfter;
            // select string
            textBox.SelectionStart = iSelectionStart;
            textBox.SelectionLength = templateText.Length;






        }


        /// <summary>
        /// Event File Save As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileSaveAsMenuItem_Click(object sender, EventArgs e)
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
            save();
        }
        /// <summary>
        /// Event File Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileSaveAllMenuItem_Click(object sender, EventArgs e)
        {
            saveAll();
        }

        /// <summary>
        /// Save all unchanged Tabs. 
        /// </summary>
        public void saveAll()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                save(tabPage, configSave: false);
            }
            Settings.Save();
        }

        /// <summary>
        /// Event File Load fired by TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileLoadMenuItem_Click(object sender, EventArgs e)
        {
            loadTabPagePerFileDialog();


        }
        /// <summary>
        /// Add tab fired by TabControl or TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addTabMenuItem_Click(object sender, EventArgs e)
        {
            addTab();


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
        /// Reload current Tab
        /// </summary>
        public void ReloadTabPage()
        {
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            var textBox = (TextBox)tabPage.Controls[0];
            SqlFile sqlFile = (SqlFile)tabPage.Tag;

            textBox.Text = sqlFile.load();
        }

        
        /// <summary>
        /// Reload current tab and ask if the user wants it. It checks the file for differences before asking.
        /// </summary>
        public void ReloadTabPageWithAsk()
        {
            if (_tabControl.SelectedIndex < 0 || (!Settings.IsAskForQueryUpdateOutside) ) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            var textBox = (TextBox)tabPage.Controls[0];
            SqlFile sqlFile = (SqlFile)tabPage.Tag;

            // check if really changed
            string sNew = sqlFile.load().Trim();
            string sOld = textBox.Text.Trim();
            if (sNew.Equals(sOld)) return;

            string sDetails = $"{_addinTabName}: File '{sqlFile.FullName}' changed outside hoTools!\r\nReload:Yes\r\nIgnore:No";
            string sCaption = $"{_addinTabName}: ReLoad file because of changed outside?";
            DialogResult result = MessageBox.Show(textBox, sDetails, sCaption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // update textBox with changed content
                textBox.Text = sqlFile.load();
            }

        }

        /// <summary>
        /// Load sql string from *.sql File into active TabPage with TextBox inside. 
        /// <para/>- Update and save the list of sql files 
        /// </summary>
        public void loadTabPagePerFileDialog()
        {
            TabPage tabPage;
            // no tab page exists
            if (_tabControl.TabPages.Count == 0)
            {
                tabPage = addTab();
            }
            else
            {
                // get TabPage
                if (_tabControl.SelectedIndex < 0) return;
                tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

                // If contend changed: Store first
                SqlFile sqlFile = (SqlFile)tabPage.Tag;
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


            }
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"c:\temp\sql";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {
                string fileName = openFileDialog.FileName;
                if (fileName != null && fileName != "")
                {
                    // get TextBox
                    var textBox = (TextBoxUndo)tabPage.Controls[0];
                    SqlFile sqlFile = new SqlFile(this, tabPage, fileName, isChanged: false);
                    tabPage.Tag = sqlFile;
                    tabPage.Text = sqlFile.DisplayName;
                    textBox.Text = sqlFile.load();

                    // store the complete filename in settings
                    InsertRecentFileLists(openFileDialog.FileName);
                    Settings.Save();

                    // Load recent files into ToolStripMenu
                    loadRecentFilesIntoToolStripItems();

                }
            }
            // update Tab Caption
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;

        }
        /// <summary>
        /// Re-Load Tab Page from file
        /// </summary>
        /// <param name="tabPage"></param>
        void reLoadTabPage(TabPage tabPage)
        {
            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            loadTabPageFromFile(tabPage, sqlFile.FullName);


        }
        /// <summary>
        /// Save sql Tab As...
        /// </summary>
        public void saveAs()
        {

            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            saveAs(tabPage);
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
                string fileName = saveFileDialog.FileName;
                if (fileName != null && fileName != "")
                {
                    sqlFile.FullName = fileName;
                    var textBox = (TextBox)tabPage.Controls[0];

                    tabPage.Text = sqlFile.DisplayName;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                    sqlFile.save(textBox.Text);

                    // update files
                    InsertRecentFileLists(saveFileDialog.FileName);

                    // Store TabData in TabPage
                    sqlFile.FullName = saveFileDialog.FileName;





                    Settings.Save();
                }
            }
        }

        #region InsertRecentFilesList
        /// <summary>
        /// Insert recent file lists
        /// <para/>- SqlFile
        /// <para/>-OpenedTabs
        /// </summary>
        void InsertRecentFileLists(string fileName)
        {
            Settings.HistorySqlFiles.Insert(fileName);
            Settings.LastOpenedFiles.Insert(fileName);
        }
        #endregion
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
            loadRecentFilesMenuItems(_fileNewTabAndLoadRecentFileItem, newTabAndLoadFromHistoryEntry_Click);

            // Tab,  Load Tab from..
            loadRecentFilesMenuItems(_loadTabFromFileItem, loadFromHistoryEntry_Click);
            // Tab,  Add Tab from..
            loadRecentFilesMenuItems(_newTabFromItem, newTabAndLoadFromHistoryEntry_Click);
        }

        /// <summary>
        /// Load all tabs which were opened in the last session
        /// </summary>
        void loadOpenedTabsFromLastSession()
        {
            // load last opened files into tab pages
            foreach (HistoryFile lastOpenedFile in Settings.LastOpenedFiles.lSqlLastOpenedFilesCfg)
            {
                string fileName = lastOpenedFile.FullName.Trim();
                if (fileName == "") continue;
                // file isn't available, delete it from list of last opened filed
                if (!File.Exists(fileName))
                {
                    Settings.LastOpenedFiles.Remove(fileName);
                    continue;
                }

                TabPage tabPage = addTab();
                // load 
                loadTabPageFromFile(tabPage, lastOpenedFile.FullName, notUpdateLastOpenedList: true);

            }
        }
        #region save active Tab Page
        /// <summary>
        /// Save current active TabPage
        /// </summary>
        ///  <param name="configSave">Default: true, whether to store the configuration</param>
        public void save(bool configSave = true)
        {
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            save(tabPage, configSave);
        }
        #endregion

        #region Save Tab Page
        /// <summary>
        /// Save sql TabPage in *.sql File. Store the save time to distinguish hoTools writes from other
        /// </summary>
        /// <param name="tabPage"></param>
        ///  <param name="configSave">Default: true, whether to store the configuration</param>
        void save(TabPage tabPage, bool configSave = true)
        {
            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            if (!sqlFile.IsPersistant)
            {
                saveAs(tabPage);
                return;
            }

            try
            {
                var textBox = (TextBox)tabPage.Controls[0];
                sqlFile.save(textBox.Text);
                

                // update history
                InsertRecentFileLists(sqlFile.FullName);
                // save configuration
                if (configSave) Settings.Save();
                // set TabName
                tabPage.Text = sqlFile.DisplayName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error writing File {sqlFile.FullName}");
                return;
            }
        }

        
        #endregion

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
                switch (result)
                {
                    case DialogResult.OK:
                        save(tabPage);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;

                }
            }
            Settings.LastOpenedFiles.Remove(sqlFile.FullName);
            Settings.Save();
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
            _model.SQLRun(textBox.Text, _sqlTextBoxSearchTerm.Text);
            Cursor.Current = Cursors.Default;
        }
        #endregion

        void fileRunMenuItem_Click(object sender, EventArgs e)
        {
            runSqlTabPage();
        }

        /// <summary>
        /// Run sql for selected Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileRunSelectedMenuItem_Click(object sender, EventArgs e)
        {
            if (_tabControl.SelectedIndex < 0) return;
            // get TabPage
            Cursor.Current = Cursors.WaitCursor;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            var textBox = (TextBox)tabPage.Controls[0];

            _model.SQLRun(textBox.SelectedText, _sqlTextBoxSearchTerm.Text);
            Cursor.Current = Cursors.Default;
        }

        #region Key up
        /// <summary>
        /// Handle CTRL sequences for CTRL+S (Store sql) and CTRL+R (RUN sql)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sqlTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// Handle CTRL sequences for CTRL+S (Store sql) and CTRL+R (RUN sql)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sqlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // New Tab, add tab
            if (e.KeyData == (Keys.Control | Keys.N))
            {

                loadTabPagePerFileDialog();
                e.Handled = true;
                return;
            }
            // Load Tab from File
            if (e.KeyData == (Keys.Control | Keys.L))
            {

                loadTabPagePerFileDialog();
                e.Handled = true;
                return;
            }

            // store SQL
            if (e.KeyData == (Keys.Control | Keys.S))
            {

                save();
                e.Handled = true;
                return;
            }
            // store All SQL
            if (e.KeyData == (Keys.Control | Keys.Shift | Keys.S))
            {

                saveAll();
                e.Handled = true;
                return;
            }
            // run SQL
            if (e.KeyData == (Keys.Control | Keys.R))
            {
                runSqlTabPage();
                e.Handled = true;
                return;
            }

        }
        #endregion

        /// <summary>
        /// Output the MessageBox in the required context. 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="details"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        DialogResult BackgroundThreadMessageBox(System.Windows.Forms.Control owner, string details, string caption,MessageBoxButtons buttons )
        {
            //if (owner.InvokeRequired)
            //{
                return (DialogResult)owner.Invoke(new Func<DialogResult>(
                                       () => { return MessageBox.Show(owner, details, caption, buttons); }));
            //}
            //else
            //{
            //    return MessageBox.Show(owner, details, caption, buttons);
            //}
        }



    }
}
