using System;
using System.Windows.Forms;
using EA;
using hoTools.Settings;
using hoTools.Utils.SQL;
using EAAddinFramework.Utils;
using File = System.IO.File;
using System.Text.RegularExpressions;

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
        public static readonly string MenuLoadTabFileText = "Load Tab from File (CTRL+L)";
        public static readonly string MenuLoadTabFileTooltip = "Load current SQL Tab by File Dialog.";

        public static readonly string MenuLoadTabFromRecentFileText = "Load Tab from recent File...";
        public static readonly string MenuLoadTabFromRecentFileTooltip = "Load current SQL Tab from recent File...";

        public static readonly string MenuReLoadTabText = "ReLoad Tab from File";
        public static readonly string MenuReLoadTabTooltip = "ReLoad current SQL Tab from File";

        public static readonly string MenuNewTabText = "New Tab";
        public static readonly string MenuNewTabTooltip = "Create new SQL Tab with Element Template";

        public static readonly string MenuNewTabFromRecentText = "New Tab from recent File...";
        public static readonly string MenuNewTabFromRecentTooltip = "Create new SQL Tab from recent File";

        public static readonly string MenuNewTabWithFileDialogText = "New Tab from File";
        public static readonly string MenuNewTabWithFileDialogTooltip = "Create new SQL Tab from File Dialog.";


        


        const string SqlTextBoxTooltip =
@"CTRL+L                        Load sql from File
CTRL+R                          Run sql
CTRL+S                          Store sql
CTRL+SHFT+S                     Store sql All
\\ Comment                      Just your comment as you may know from C, Java, C#,..
<Search Term>                   Replaced by Text from the Search Text
#Branch#                        Selected Package, Replaced by nested recursive as comma separated list of PackageIDs    
#Branch={....guid...}#          Package according to Id, Replaced by nested recursive as comma separated list of PackageIDs                    
#ConnectorID#                  Selected Connector, Replaced by ConnectorID
#ConveyedItemsIDS#             Selected Connector, Replaced by the Conveyed Items as comma separated list of ElementIDs
#CurrentElementGUID#            Alias for #CurrentItemGUID# (EA compatibility)
#CurrentElementID#              Alias for #CurrentItemID# (EA compatibility)
#CurrentItemGUID#               Selected Element, Diagram, Replaced by the Id
#CurrentItemID#                 Selected Element, Diagram, Replaced by the ID
#DiagramElements_IDS#            Diagram Elements of selected Diagram / current Diagram
#DiagramSelectedElements_IDS#    Selected Diagram Objects of selected Diagram / current Diagram 
#InBranch#                      Selected Package, Replaced by nested recursive as comma separated list of PackageIDs  like 'IN (13,14,15)'
#Package#                       Selected Package, Diagram, Element, Attribute, Operation, Replaced by containing Package ID
#PackageID#                     Selected Package, Diagram, Element, Attribute, Operation, Replaced by containing Package ID
#TreeSelectedGUIDS#             In Browser selected Elements as a list of comma separated GUIDS like 'IN (##TreeSelectedGUIDS##)'
#WC#                            Wild card, you can also simple use * (will automatically replaced by the DB specific wild card)
#DB=ACCESS2007#                 DB specif SQL for ACCESS2007
#DB=Asa#                        DB specif SQL for Asa
#DB=FIREBIRD#                   DB specif SQL for FIREBIRD
#DB=JET#                        DB specif SQL for JET
#DB=MySql#                      DB specif SQL for My SQL
#DB=ORACLE#                     DB specif SQL for Oracle
#DB=POSTGRES#                   DB specif SQL for POSTGRES
#DB=SqlSvr#                     DB specif SQL for SQL Server
* or %                          DB Wild Card (any arbitrary characters, automatic transformed into DB format)
? or _                          DB Wild Card (one arbitrary character caret, automatic transformed into DB format)
^ or !                          DB short for XOR (automatic transformed into DB format)
";

        /// <summary>
        /// Setting with the file history.
        /// </summary>
        /// 
        public AddinSettings Settings { get; }

        readonly Model _model;
        readonly System.ComponentModel.IContainer _components;

        readonly TabControl _tabControl;
        readonly TextBox _sqlTextBoxSearchTerm;

        /// <summary>
        /// The tab name of Addin (SQL or Script)
        /// </summary>
        readonly string _addinTabName;

        // File Menu:   New Tab Recent MenuItems to complete with recent files items
        readonly ToolStripMenuItem _fileNewTabAndLoadRecentFileMenuItem;
        // File Menu:   Load Tab Recent MenuItems to complete with recent files items
        readonly ToolStripMenuItem _fileLoadTabRecentFileMenuItem;

        // Tab Menu:   New Tab Recent MenuItems to complete with recent files items       
        readonly ToolStripMenuItem _tabNewTabFromRecentFileMenuItem = new ToolStripMenuItem("New Tab from...");
        // Tab Menu:   Load Tab Recent MenuItems to complete with recent files items
        readonly ToolStripMenuItem _tabLoadTabFromRecentFileMenuItem = new ToolStripMenuItem("Load from...");


        const string DefaultTabName = "noName";

        /// <summary>
        /// Constructor to Initialize TabControl, create ToolStripItems (New Tab from, Recent Files) with file history. 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="settings"></param>
        /// <param name="components"></param>
        /// <param name="tabControl"></param>
        /// <param name="sqlTextBoxSearchTerm"></param>
        /// <param name="fileNewTabAndLoadRecentFileMenuItem">File, New Tab from recent files</param>
        /// <param name="fileLoadTabRecentFileMenuItem">File, Load Tab from recent files</param>
        /// <param name="addinTabName"></param>
        public SqlTabPagesCntrl(Model model, AddinSettings settings,
            System.ComponentModel.IContainer components,
            TabControl tabControl, TextBox sqlTextBoxSearchTerm,
            ToolStripMenuItem fileNewTabAndLoadRecentFileMenuItem,
            ToolStripMenuItem fileLoadTabRecentFileMenuItem, 
            string addinTabName)
        {
            Settings = settings;
            _model = model;
            _tabControl = tabControl;
            _components = components;
            _sqlTextBoxSearchTerm = sqlTextBoxSearchTerm;



            // Recent MenuItems to complete with items of recent files
            _fileNewTabAndLoadRecentFileMenuItem = fileNewTabAndLoadRecentFileMenuItem;
            _fileLoadTabRecentFileMenuItem = fileLoadTabRecentFileMenuItem;

            // Update text and tooltip for menu item
            _tabLoadTabFromRecentFileMenuItem.Text = MenuLoadTabFromRecentFileText;
            _tabLoadTabFromRecentFileMenuItem.ToolTipText = MenuLoadTabFromRecentFileTooltip;



            _tabNewTabFromRecentFileMenuItem.Text = MenuNewTabFromRecentText;
            _tabNewTabFromRecentFileMenuItem.ToolTipText = MenuNewTabFromRecentTooltip;

            
            LoadOpenedTabsFromLastSession();
            

            // Load recent files into ToolStripMenu
            LoadRecentFilesIntoToolStripItems();
            _addinTabName = addinTabName;

        }

        public string MenuNewTabFromRecentFileText { get; set; }


        /// <summary>
        /// Add a tab to the tab control and load content into the tab.If the content is empty nothing is loaded. 
        /// </summary>
        /// <param name="content">Content of the Tab</param>
        /// <returns></returns>
        public TabPage AddTab(string content)
        {
            TabPage tabPage = AddTab();
            if (content != "") LoadTabPage(content);
            return tabPage;
        }
        /// <summary>
        /// Add Tab with file dialog to load file
        /// </summary>
        /// <returns></returns>
        public TabPage AddTabWithFileDialog()
        {
            TabPage tab = AddTab();
            LoadTabPagePerFileDialog();
            return tab;
        }
        /// <summary>
        /// Add an Tab to the tab control and load the Element Template as default. The Text box is unchanged because it's just a template.
        /// </summary>
        /// <returns></returns>
        public TabPage AddTab()
        {
            // create a new TabPage in TabControl
            TabPage tabPage = new TabPage();

            // Create a new empty file
            SqlFile sqlFile = new SqlFile(this, $"{DefaultTabName}{_tabControl.Controls.Count}.sql", isChanged:false);
            tabPage.Tag = sqlFile;
            tabPage.Text = sqlFile.DisplayName;
            tabPage.ToolTipText = sqlFile.FullName;


            // add tab to TabControl
            _tabControl.Controls.Add(tabPage);
            _tabControl.SelectTab(tabPage);




            //-----------------------------------------------------------------
            // Tab with ContextMenuStrip
            // Create a text box in TabPage for the SQL string
            var sqlTextBox = new TextBoxUndo(tabPage)
            {
                Text = SqlTemplates.GetTemplateText(SqlTemplates.SqlTemplateId.ElementTemplate)
            };
            // load element template
            sqlFile.IsChanged = false;
            tabPage.Text = sqlFile.DisplayName;  // after isChanged = false

            // register CTRL+S (store SQL) and CTRL+R (run SQL)
            sqlTextBox.KeyUp += sqlTextBox_KeyUp;
            ToolTip toolTip = new ToolTip
            {
                IsBalloon = false,
                InitialDelay = 0,
                ShowAlways = true
            };
            toolTip.SetToolTip(sqlTextBox, SqlTextBoxTooltip);
            tabPage.Controls.Add(sqlTextBox);



            // Load sql File into TabPage
            ToolStripMenuItem loadWithFileDialogMenuItem = new ToolStripMenuItem
            {
                Text = MenuLoadTabFileText,
                ToolTipText = MenuLoadTabFileTooltip
            };
            loadWithFileDialogMenuItem.Click += fileLoadMenuItem_Click;

            // ReLoad sql File
            ToolStripMenuItem reLoadTabMenuItem = new ToolStripMenuItem
            {
                Text = MenuReLoadTabText,
                ToolTipText = MenuReLoadTabTooltip
            };
            reLoadTabMenuItem.Click += reLoadTabMenuItem_Click;


            

            // New TabPage
            ToolStripMenuItem newTabMenuItem = new ToolStripMenuItem
            {
                Text = MenuNewTabText,
                ToolTipText = MenuNewTabTooltip
            };
            newTabMenuItem.Click += addTabMenuItem_Click;

            // New Tab and Load File via Dialog
            ToolStripMenuItem newTabWithFileDialogMenuItem = new ToolStripMenuItem
            {
                Text = MenuNewTabWithFileDialogText,
                ToolTipText = MenuNewTabWithFileDialogTooltip
            };
            newTabWithFileDialogMenuItem.Click += addTabFileDialogMenuItem_Click;


            // Save sql File from TabPage
            ToolStripMenuItem fileSaveMenuItem = new ToolStripMenuItem
            {
                Text = @"Save File (CTRL+S)"
            };
            fileSaveMenuItem.Click += fileSaveMenuItem_Click;

            // Save all sql files 
            ToolStripMenuItem fileSaveAllMenuItem = new ToolStripMenuItem { Text = @"Save All File (CTRL+SHFT+S)" };
            fileSaveAllMenuItem.Click += fileSaveAllMenuItem_Click;

            // Save As sql File from TabPage
            ToolStripMenuItem fileSaveAsMenuItem = new ToolStripMenuItem { Text = @"Save File As.." };
            fileSaveAsMenuItem.Click += fileSaveAsMenuItem_Click;



            // Close TabPage
            ToolStripMenuItem closeMenuItem = new ToolStripMenuItem {Text = @"Close Tab"};
            closeMenuItem.Click += closeMenuItem_Click;

            // Run sql File 
            ToolStripMenuItem fileRunMenuItem = new ToolStripMenuItem {Text = @"Run sql (CTRL+R)"};
            fileRunMenuItem.Click += fileRunMenuItem_Click;


            // Run selected sql 
            ToolStripMenuItem fileRunSelectedMenuItem = new ToolStripMenuItem
            {
                Text = @"Run selected string as sql",
                ToolTipText = @"Run selected string as SQL
1.  Select SQL part you want to run
2.  Run selected string as SQL

Useful to quickly test:
- Select of a UNION
- Only the a part of a long SQL 
"
            };

            fileRunSelectedMenuItem.Click += fileRunSelectedMenuItem_Click;


            //------------------------------------------------------------------------------------------------------------------
            // Insert Template
            ToolStripMenuItem insertTemplateMenuItem = GetTemplateMenuItems();


            //-----------------------------------------------------------------------------------------------------------------
            // Insert Macro
            ToolStripMenuItem insertMacroMenuItem = GetMacroMenuItems();

            // Load recent files into toolStrip menu
            LoadRecentFilesIntoToolStripItems();


            //----------------------------------------------------------------------------------------------------------
            // ToolStripItem for
            // - TabPage
            // - SQL TextBox
            var toolStripItems = new ToolStripItem[] {
                loadWithFileDialogMenuItem,                   // load Tab from file
                _tabLoadTabFromRecentFileMenuItem,              // load Tab from recent file  
                reLoadTabMenuItem,                 // ReLoad Tab
                new ToolStripSeparator(),
                newTabMenuItem,                     // new Tab
                newTabWithFileDialogMenuItem,       // New Tab with File Dialog
                _tabNewTabFromRecentFileMenuItem,      // new Tab from recent file

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
        ToolStripMenuItem GetMacroMenuItems()
        {
            ToolStripMenuItem insertMacroMenuItem = new ToolStripMenuItem {Text = @"Insert &Macro"};

            // Insert Macro
            var id = SqlTemplates.SqlTemplateId.SearchTerm;
            ToolStripMenuItem insertMacroSearchTermMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertMacroSearchTermMenuItem.Click += insertTemplate_Click;

            // Insert Package
            id = SqlTemplates.SqlTemplateId.PackageId;
            ToolStripMenuItem insertPackageMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertPackageMenuItem.Click += insertTemplate_Click;

            // Insert Branch
            id = SqlTemplates.SqlTemplateId.BranchIds;
            ToolStripMenuItem insertBranchMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertBranchMenuItem.Click += insertTemplate_Click;

            // Insert Branch for constant package
            id = SqlTemplates.SqlTemplateId.BranchIdsConstantPackage;
            ToolStripMenuItem insertBranchConstantPackageMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertBranchConstantPackageMenuItem.Click += insertTemplate_Click;

            // Insert InBranch
            id = SqlTemplates.SqlTemplateId.InBranchIds;
            ToolStripMenuItem insertInBranchMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertInBranchMenuItem.Click += insertTemplate_Click;

            // Insert CurrentID
            id = SqlTemplates.SqlTemplateId.CurrentItemId;
            ToolStripMenuItem insertCurrentIdMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertCurrentIdMenuItem.Click += insertTemplate_Click;

            // Insert CurrentGUID
            id = SqlTemplates.SqlTemplateId.CurrentItemGuid;
            ToolStripMenuItem insertCurrentGuidMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertCurrentGuidMenuItem.Click += insertTemplate_Click;

            // Insert ConnectorID
            id = SqlTemplates.SqlTemplateId.ConnectorId;
            ToolStripMenuItem insertConnectorIdMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertConnectorIdMenuItem.Click += insertTemplate_Click;

            // Insert Conveyed Item IDs
            id = SqlTemplates.SqlTemplateId.ConveyedItemIds;
            ToolStripMenuItem insertConveyedItemIdsMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertConveyedItemIdsMenuItem.Click += insertTemplate_Click;


            // Insert Diagram Selected Objects_IDS Item IDs
            id = SqlTemplates.SqlTemplateId.DiagramSelectedElementsIds;
            ToolStripMenuItem insertDiagramSelectedElementsIdsMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDiagramSelectedElementsIdsMenuItem.Click += insertTemplate_Click;

            // Insert Diagram Objects_IDS Item IDs
            id = SqlTemplates.SqlTemplateId.DiagramElementsIds;
            ToolStripMenuItem insertDiagramElementsIdsMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDiagramElementsIdsMenuItem.Click += insertTemplate_Click;


            // Insert Diagram IDs
            id = SqlTemplates.SqlTemplateId.DiagramId;
            ToolStripMenuItem insertDiagramElementsIdMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDiagramElementsIdMenuItem.Click += insertTemplate_Click;

            // Tree selected GUIDs
            id = SqlTemplates.SqlTemplateId.TreeSelectedGuids;
            ToolStripMenuItem insertTreeSelectedGuidsMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertTreeSelectedGuidsMenuItem.Click += insertTemplate_Click;

            // newGuid
            id = SqlTemplates.SqlTemplateId.NewGuid;
            ToolStripMenuItem insertNewGuid = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertNewGuid.Click += insertTemplate_Click;

            // Insert #WC#
            id = SqlTemplates.SqlTemplateId.Wc;
            ToolStripMenuItem insertWcMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertWcMenuItem.Click += insertTemplate_Click;

            // Insert Design ID
            id = SqlTemplates.SqlTemplateId.DesignId;
            ToolStripMenuItem insertDesignId = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDesignId.Click += insertTemplate_Click;

            // Insert Design Id
            id = SqlTemplates.SqlTemplateId.DesignGuid;
            ToolStripMenuItem insertDesignGuid = new ToolStripMenuItem
            {
                Text = @"Insert " + SqlTemplates.GetTemplateText(id),
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDesignGuid.Click += insertTemplate_Click;



            insertMacroMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                insertMacroSearchTermMenuItem,
                new ToolStripSeparator(),
                insertPackageMenuItem,
                insertBranchMenuItem,
                insertBranchConstantPackageMenuItem,
                new ToolStripSeparator(),
                insertCurrentIdMenuItem,
                insertCurrentGuidMenuItem,
                new ToolStripSeparator(),
                insertConnectorIdMenuItem,
                insertConveyedItemIdsMenuItem,
                new ToolStripSeparator(),
                insertDiagramElementsIdMenuItem,
                insertDiagramElementsIdsMenuItem,
                insertDiagramSelectedElementsIdsMenuItem,
                new ToolStripSeparator(),
                insertNewGuid,  // used for Insert 
                new ToolStripSeparator(),
                insertTreeSelectedGuidsMenuItem,
                insertWcMenuItem,
                new ToolStripSeparator(),
                insertDesignId,
                insertDesignGuid
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
        ToolStripMenuItem GetTemplateMenuItems()
        {
            ToolStripMenuItem insertTemplateMenuItem = new ToolStripMenuItem("Insert &Template");

            // Insert Branch Template
            var id = SqlTemplates.SqlTemplateId.BranchTemplate;
            ToolStripMenuItem insertBranchTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert Branch Template",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertBranchTemplateMenuItem.Click += insertTemplate_Click;

            // Insert CurrentItemId Template
            id = SqlTemplates.SqlTemplateId.CurrentItemIdTemplate;
            ToolStripMenuItem insertCurrentItemIdTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert CurrentItemId Template",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertCurrentItemIdTemplateMenuItem.Click += insertTemplate_Click;

            // Insert CurrentItemGuid Template
            id = SqlTemplates.SqlTemplateId.CurrentItemGuidTemplate;
            ToolStripMenuItem insertCurrentItemGuidTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert CurrentItemGuid Template",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertCurrentItemGuidTemplateMenuItem.Click += insertTemplate_Click;

            // Insert ConveyedItemIDS Template
            id = SqlTemplates.SqlTemplateId.ConveyedItemsFromConnectorTemplate;
            ToolStripMenuItem insertConveyedtemIdsTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert ConveyedItems from selected Connector Template",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertConveyedtemIdsTemplateMenuItem.Click += insertTemplate_Click;

            // Insert Connectors from Element Template
            id = SqlTemplates.SqlTemplateId.ConnectorsFromElementTemplate;
            ToolStripMenuItem insertConnectorsFromElementTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert Connectors from Element Template",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertConnectorsFromElementTemplateMenuItem.Click += insertTemplate_Click;


            // Insert Element Template
            id = SqlTemplates.SqlTemplateId.ElementTemplate;
            ToolStripMenuItem insertElementTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"Insert Element Template",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertElementTemplateMenuItem.Click += insertTemplate_Click;

            // Insert Element Type Template
            id = SqlTemplates.SqlTemplateId.ElementTypeTemplate;
            var insertElementTypeTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id,@"Insert Element Template");


            // Insert Diagram Elements Template
            id = SqlTemplates.SqlTemplateId.DiagramElementsIdsTemplate;
            ToolStripMenuItem insertDiagramElementTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Diagram Elements Template");



            // Insert Diagram Selected Elements Template
            id = SqlTemplates.SqlTemplateId.DiagramSelectedElementsIdsTemplate;
            ToolStripMenuItem insertDiagramSelectedElementTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Diagram selected Elements Template");

            // Insert Diagram Template
            id = SqlTemplates.SqlTemplateId.DiagramTemplate;
            ToolStripMenuItem insertDiagramTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Diagram Template");

            // Insert Diagram Type Template
            id = SqlTemplates.SqlTemplateId.DiagramTypeTemplate;
            ToolStripMenuItem insertDiagramTypeTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Diagram Type Template");


            // Insert Package Template
            id = SqlTemplates.SqlTemplateId.PackageTemplate;
            ToolStripMenuItem insertPackageTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Package Template");

            // Insert DiagramObject Template
            id = SqlTemplates.SqlTemplateId.DiagramObjectTemplate;
            ToolStripMenuItem insertDiagramObjectTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Diagram Object Template");

            // Insert Attribute Template
            id = SqlTemplates.SqlTemplateId.AttributeTemplate;
            ToolStripMenuItem insertAttributeTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Attribute Template");

            // Insert Operation Template
            id = SqlTemplates.SqlTemplateId.OperationTemplate;
            ToolStripMenuItem insertOperationTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Operation Template");

            // Insert Demo Run Script Template
            id = SqlTemplates.SqlTemplateId.DemoRunScriptTemplate;
            ToolStripMenuItem insertDemoRunScriptTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Demo Run Script Template");




            //----------------------------------------------------------------------------------
            // Insert Element in current Package
            // Insert Demo Run Script Template
            id = SqlTemplates.SqlTemplateId.InsertItemInPackageTemplate;
            ToolStripMenuItem insertItemIntoPackageMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Insert Template");

            // Update current selected Item
            id = SqlTemplates.SqlTemplateId.UpdateItemTemplate;
            ToolStripMenuItem updateCurrentElementMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Update Template");

            //  Delete tree selected Items
            id = SqlTemplates.SqlTemplateId.DeletedTreeSelectedItems;
            ToolStripMenuItem deleteTreeSelectedItemsMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Insert Delete Template");



           
            //---------------------------------------------------------------------------------------------------------
            // DB Templates
            ToolStripMenuItem insertDbTemplateMenuItem = new ToolStripMenuItem {Text = @"Insert DB dependent"};

            //  DB ACESS2007
            id = SqlTemplates.SqlTemplateId.DbAccess2007;
            ToolStripMenuItem insertDbaccess2007TemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"ACCESS2007");

            //  DB Asa
            id = SqlTemplates.SqlTemplateId.DbAsa;
            ToolStripMenuItem insertDbAsaTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Asa");

            //  DB Firebird
            id = SqlTemplates.SqlTemplateId.DbFirebird;
            ToolStripMenuItem insertDbFirebirdTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"Firebird");

            //  DB Jet
            id = SqlTemplates.SqlTemplateId.DbJet;
            ToolStripMenuItem insertDbJetTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"JET");

            //  DB MySql
            id = SqlTemplates.SqlTemplateId.DbMysql;
            ToolStripMenuItem insertDbmySqlTemplateMenuItem = ToolstripMenuItemInsertTemplateFromId(id, @"MySql");

            

            // DB Oracle
            id = SqlTemplates.SqlTemplateId.DbOracle;
            ToolStripMenuItem insertDbOracleTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"ORACLE",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDbOracleTemplateMenuItem.Click += insertTemplate_Click;

            // DB POSTGRES
            id = SqlTemplates.SqlTemplateId.DbPostgres;
            ToolStripMenuItem insertDbPostgresTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"POSTGRES",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDbPostgresTemplateMenuItem.Click += insertTemplate_Click;

            // DB SQL Server
            id = SqlTemplates.SqlTemplateId.DbSqlsvr;
            ToolStripMenuItem insertDbSqlServerTemplateMenuItem = new ToolStripMenuItem
            {
                Text = @"SQL Server",
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertDbSqlServerTemplateMenuItem.Click += insertTemplate_Click;

            // DB dependent SQL
            insertDbTemplateMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            insertDbJetTemplateMenuItem,
                insertDbAsaTemplateMenuItem,
                insertDbaccess2007TemplateMenuItem,
                insertDbFirebirdTemplateMenuItem,
                insertDbmySqlTemplateMenuItem,
                insertDbOracleTemplateMenuItem,
                insertDbPostgresTemplateMenuItem,
                insertDbSqlServerTemplateMenuItem
                });


            // Build item content Template
            insertTemplateMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                insertBranchTemplateMenuItem,
                insertPackageTemplateMenuItem,
                new ToolStripSeparator(),
                insertCurrentItemIdTemplateMenuItem,
                insertCurrentItemGuidTemplateMenuItem,
                new ToolStripSeparator(),
                insertConveyedtemIdsTemplateMenuItem,
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
                insertDbTemplateMenuItem
                });


            return insertTemplateMenuItem;
        }
        /// <summary>
        /// New ToolStripItem to insert a template. The EventHandler is: insertTemplate_Click. It sets: Caption text, Tooltip, EventHandler, Tag with the template itself
        /// </summary>
        /// <param name="id"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        ToolStripMenuItem ToolstripMenuItemInsertTemplateFromId(SqlTemplates.SqlTemplateId id, string caption)
        {
            ToolStripMenuItem insertElementTypeTemplateMenuItem = new ToolStripMenuItem
            {
                Text = caption,
                ToolTipText = SqlTemplates.GetTooltip(id),
                Tag = SqlTemplates.GetTemplate(id)
            };
            insertElementTypeTemplateMenuItem.Click += insertTemplate_Click;
            return insertElementTypeTemplateMenuItem;
        }

        #endregion

       
        /// <summary>
        /// Load RecentFiles MenuItems into MenuItemStrip
        /// </summary>
        /// <param name="loadRecentFileStripMenuItem">Item to load recent files as drop down items</param>
        /// <param name="eventHandlerClick">Function to handle event</param>
        void LoadRecentFilesMenuItems(ToolStripMenuItem loadRecentFileStripMenuItem, EventHandler eventHandlerClick)
        {
            // delete all previous entries
            loadRecentFileStripMenuItem.DropDownItems.Clear();
            // over all history files
            foreach (HistoryFile historyFile in Settings.HistorySqlFiles.lSqlHistoryFilesCfg)
            {
                // ignore empty entries
                if (historyFile.FullName == "") continue;
                ToolStripMenuItem historyEntry = new ToolStripMenuItem
                {
                    Text = historyFile.DisplayName,
                    Tag = historyFile,
                    ToolTipText = historyFile.FullName
                };
                historyEntry.Click += eventHandlerClick;
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
            TabPage tabPage = AddTab("");

            HistoryFile historyFile = (HistoryFile)((ToolStripMenuItem)sender).Tag;
            string file = historyFile.FullName;
            LoadTabPageFromFile(tabPage, file);
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
        void LoadTabPageFromFile(TabPage tabPage, string fileName, bool notUpdateLastOpenedList = false)
        {

            try
            {
                 TextBoxUndo textBox = (TextBoxUndo)tabPage.Controls[0];

                // set TabName
                SqlFile sqlFile = (SqlFile)tabPage.Tag;
                sqlFile.FullName = fileName;
                textBox.Text = sqlFile.Load(); // don't move behind sqlFile.IsChange=false !!!!
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
            }
        }
        /// <summary>
        /// Load string for tab Page
        /// </summary>
        /// <param name="tabContent">What do load in Tab</param>
        void LoadTabPage(string tabContent)
        {
            TabPage tabPage;
            // no tab exists
            if (_tabControl.TabPages.Count == 0)
            {
                tabPage = AddTab();
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
                tabPage = AddTab("");
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

            // Contend changed, need to be stored first
            if (sqlFile.IsChanged)
            {
                Settings.LastOpenedFiles.Remove(sqlFile.FullName);
                DialogResult result = MessageBox.Show($"Old File: '{sqlFile.FullName}'",
                    @"First store old File? ", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                {
                    SaveAs(tabPage);
                    sqlFile.IsChanged = false;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                }
            }


            // load File
            HistoryFile historyFile = (HistoryFile)((ToolStripMenuItem)sender).Tag;
            string file = historyFile.FullName;
            LoadTabPageFromFile(tabPage, file);
        }
        /// <summary>
        /// Inserts the selected macro at the cursor position or replace the selected text by the macro. 
        /// The template is identified by the .Tag property of the menuItem passed by the sender parameter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            var templateText = template.TemplateText;


            //---------------------------------------------------------------------------
            // special template handling 
            // it replaces the template if it matches
            templateText = BranchConstantPackageTemplateText(template, templateText);
            templateText = IdConstant(template, templateText);
            templateText = GuidConstant(template, templateText);


            // insert text and replace a selected range

            int iSelectionStart = textBox.SelectionStart;
            string sBefore = textBox.Text.Substring(0, iSelectionStart);
            string sAfter = textBox.Text.Substring(iSelectionStart + textBox.SelectionLength);
            textBox.Text = sBefore + templateText + sAfter;
            // select string
            textBox.SelectionStart = iSelectionStart;
            textBox.SelectionLength = templateText.Length;






        }
        /// <summary>
        /// Insert Branch for a constant package like: '#Branch={.....guid...}'. If the context Element is a package it inserts the Package Id.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateText"></param>
        /// <returns></returns>
        private string BranchConstantPackageTemplateText(SqlTemplate template, string templateText)
        {
            ObjectType objectType = _model.Repository.GetContextItemType();
            // #Branch={...guid...}#
            if (objectType == ObjectType.otPackage &&
                template == SqlTemplates.GetTemplate(SqlTemplates.SqlTemplateId.BranchIdsConstantPackage))
            {
                // get package Id
                string guid = ((Package) _model.Repository.GetContextObject()).PackageGUID;
                // replace {.....} by guid
                Regex pattern = new Regex(@"={[^}]*}");
                templateText = pattern.Replace(templateText, $"={guid}");
            }
            return templateText;
        }

        /// <summary>
        /// Returns string of id of it selected and has a supported type (Package, Diagram, Element, Attribute, Operation).It also copies the id to Clipboard.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateText"></param>
        /// <returns></returns>
        private string IdConstant(SqlTemplate template, string templateText)
        {

            // #Branch={...guid...}#
            if ( template == SqlTemplates.GetTemplate(SqlTemplates.SqlTemplateId.DesignId))
            {
                // get design ID
                ObjectType objectType = _model.Repository.GetContextItemType();
                int id;
                switch (objectType)
                {
                    case ObjectType.otElement:
                        id = ((Element)_model.Repository.GetContextObject()).ElementID;
                        break;
                    case ObjectType.otPackage:
                        id = ((Package)_model.Repository.GetContextObject()).PackageID;
                        break;
                    case ObjectType.otDiagram:
                        id = ((Diagram)_model.Repository.GetContextObject()).DiagramID;
                        break;
                    case ObjectType.otAttribute:
                        id = ((EA.Attribute)_model.Repository.GetContextObject()).AttributeID;
                        break;
                    case ObjectType.otMethod:
                        id = ((Method)_model.Repository.GetContextObject()).MethodID;
                        break;
                    default:
                        return templateText;
                }
                string sId = $"{id}";
                Clipboard.SetText(sId);
                return sId;


            }

            return templateText;
        }
        /// <summary>
        /// Returns guid of it selected and has a supported type (Package, Diagram, Element, Attribute, Operation)
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateText"></param>
        /// <returns></returns>
        private string GuidConstant(SqlTemplate template, string templateText)
        {

            // #Branch={...guid...}#
            if (template == SqlTemplates.GetTemplate(SqlTemplates.SqlTemplateId.DesignGuid))
            {
                // get design Id
                ObjectType objectType = _model.Repository.GetContextItemType();
                string guid;
                switch (objectType)
                {
                    case ObjectType.otElement:
                        guid = ((Element)_model.Repository.GetContextObject()).ElementGUID;
                        break;
                    case ObjectType.otPackage:
                        guid = ((Package)_model.Repository.GetContextObject()).PackageGUID;
                        break;
                    case ObjectType.otDiagram:
                        guid = ((Diagram)_model.Repository.GetContextObject()).DiagramGUID;
                        break;
                    case ObjectType.otAttribute:
                        guid = ((EA.Attribute)_model.Repository.GetContextObject()).AttributeGUID;
                        break;
                    case ObjectType.otMethod:
                        guid = ((Method)_model.Repository.GetContextObject()).MethodGUID;
                        break;
                    default:
                        return templateText;
                }
                Clipboard.SetText(guid);
                return guid;


            }
            return templateText;
        }




        /// <summary>
        /// Event File Save As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveSqlTabAs();

        }


        #region saveTabAs
        /// <summary>
        /// Save current Tab into desired file
        /// </summary>
        public void SaveSqlTabAs()
        {
            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            SaveAs(tabPage);
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
            Save();
        }
        /// <summary>
        /// Event File Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileSaveAllMenuItem_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        /// <summary>
        /// Save all unchanged Tabs. 
        /// </summary>
        public void SaveAll()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                Save(tabPage, configSave: false);
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
            LoadTabPagePerFileDialog();


        }
        /// <summary>
        /// Add tab fired by TabControl or TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addTabMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();


        }

        /// <summary>
        /// Add Tab with File Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addTabFileDialogMenuItem_Click(object sender, EventArgs e)
        {
            AddTabWithFileDialog();

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
            Close(tabPage);

        }

        void reLoadTabMenuItem_Click(object sender, EventArgs e)
        {
            ReloadTabPage();
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

            textBox.Text = sqlFile.Load();
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
            string sNew = sqlFile.Load().Trim();
            string sOld = textBox.Text.Trim();
            if (sNew.Equals(sOld)) return;

            string sDetails = $"{_addinTabName}: File '{sqlFile.FullName}' changed outside hoTools!\r\nReload:Yes\r\nIgnore:No";
            string sCaption = $"{_addinTabName}: ReLoad file because of changed outside?";
            DialogResult result = MessageBox.Show(textBox, sDetails, sCaption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // update textBox with changed content
                textBox.Text = sqlFile.Load();
            }

        }

        /// <summary>
        /// Load sql string from *.sql File into active TabPage with TextBox inside. 
        /// <para/>- Update and save the list of sql files 
        /// </summary>
        public void LoadTabPagePerFileDialog()
        {
            TabPage tabPage;
            // no tab page exists
            if (_tabControl.TabPages.Count == 0)
            {
                tabPage = AddTab();
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
                        @"First store old File? ", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Cancel) return;
                    if (result == DialogResult.Yes)
                    {
                        Save(tabPage);
                        sqlFile.IsChanged = false;
                        tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                    }
                }


            }
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"c:\temp\sql",
                RestoreDirectory = true,
                Filter = @"sql files (*.sql)|*.sql|All files (*.*)|*.*",
                FilterIndex = 1
            };


            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {
                string fileName = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    // get TextBox
                    var textBox = (TextBoxUndo)tabPage.Controls[0];
                    SqlFile sqlFile = new SqlFile(this, fileName, isChanged: false);
                    tabPage.Tag = sqlFile;
                    sqlFile.IsChanged = false;
                    tabPage.Text = sqlFile.DisplayName;
                    textBox.Text = sqlFile.Load();
                    sqlFile.IsChanged = false;

                    // store the complete filename in settings
                    InsertRecentFileLists(openFileDialog.FileName);
                    Settings.Save();

                    // Load recent files into ToolStripMenu
                    LoadRecentFilesIntoToolStripItems();

                }
            }
            // update Tab Caption
            tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
            tabPage.Text = ((SqlFile)tabPage.Tag).DisplayName;

        }
        /// <summary>
        /// Save sql Tab As...
        /// </summary>
        public void SaveAs()
        {

            // get TabPage
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            SaveAs(tabPage);
        }

        /// <summary>
        /// Save As... TabPage in *.sql File.
        /// </summary>
        /// <param name="tabPage"></param>
        void SaveAs(TabPage tabPage)
        {

            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = sqlFile.DirectoryName,
                FileName = sqlFile.FullName,
                RestoreDirectory = true,
                Filter = @"sql files (*.sql)|*.sql|All files (*.*)|*.*",
                FilterIndex = 1
            };

            // get File name

            if (saveFileDialog.ShowDialog() == DialogResult.OK)

            {
                string fileName = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    sqlFile.FullName = fileName;
                    var textBox = (TextBox)tabPage.Controls[0];

                    tabPage.Text = sqlFile.DisplayName;
                    tabPage.ToolTipText = ((SqlFile)tabPage.Tag).FullName;
                    sqlFile.Save(textBox.Text);

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
        void LoadRecentFilesIntoToolStripItems()
        {
            // File, Load Tab from
            LoadRecentFilesMenuItems(_fileLoadTabRecentFileMenuItem, loadFromHistoryEntry_Click);
            // File, Add Tab from..
            LoadRecentFilesMenuItems(_fileNewTabAndLoadRecentFileMenuItem, newTabAndLoadFromHistoryEntry_Click);

            // Tab,  Load Tab from..
            LoadRecentFilesMenuItems(_tabLoadTabFromRecentFileMenuItem, loadFromHistoryEntry_Click);
            // Tab,  Add Tab from..
            LoadRecentFilesMenuItems(_tabNewTabFromRecentFileMenuItem, newTabAndLoadFromHistoryEntry_Click);
        }

        /// <summary>
        /// Load all tabs which were opened in the last session
        /// </summary>
        void LoadOpenedTabsFromLastSession()
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

                TabPage tabPage = AddTab();
                // load 
                LoadTabPageFromFile(tabPage, lastOpenedFile.FullName, notUpdateLastOpenedList: true);

            }
        }
        #region save active Tab Page
        /// <summary>
        /// Save current active TabPage
        /// </summary>
        ///  <param name="configSave">Default: true, whether to store the configuration</param>
        public void Save(bool configSave = true)
        {
            if (_tabControl.SelectedIndex < 0) return;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];
            Save(tabPage, configSave);
        }
        #endregion

        #region Save Tab Page
        /// <summary>
        /// Save sql TabPage in *.sql File. Store the save time to distinguish hoTools writes from other
        /// </summary>
        /// <param name="tabPage"></param>
        ///  <param name="configSave">Default: true, whether to store the configuration</param>
        void Save(TabPage tabPage, bool configSave = true)
        {
            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            if (!sqlFile.IsPersistant)
            {
                SaveAs(tabPage);
                return;
            }

            try
            {
                var textBox = (TextBox)tabPage.Controls[0];
                sqlFile.Save(textBox.Text);
                

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
            }
        }

        
        #endregion

        /// <summary>
        /// Close all Tab Pages
        /// </summary>
        public void CloseAll()
        {
            foreach (TabPage tabPage in _tabControl.TabPages)
            {
                Close(tabPage);
            }
        }
        /// <summary>
        /// Close TabPage
        /// - Ask to store content if changed
        /// </summary>
        /// <param name="tabPage"></param>
        public void Close(TabPage tabPage)
        {

            SqlFile sqlFile = (SqlFile)tabPage.Tag;
            if (sqlFile.IsChanged)
            {

                DialogResult result = MessageBox.Show(@"", @"Close TabPage: Sql has changed, store content?", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.OK:
                        Save(tabPage);
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
        public void RunSqlTabPage()
        {
            if (_tabControl.SelectedIndex < 0) return;
            // get TabPage
            Cursor.Current = Cursors.WaitCursor;
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            var textBox = (TextBox)tabPage.Controls[0];
            _model.SqlRun(textBox.Text, _sqlTextBoxSearchTerm.Text);
            Cursor.Current = Cursors.Default;
        }
        #endregion

        void fileRunMenuItem_Click(object sender, EventArgs e)
        {
            RunSqlTabPage();
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

            _model.SqlRun(textBox.SelectedText, _sqlTextBoxSearchTerm.Text);
            Cursor.Current = Cursors.Default;
        }

        #region Key up
        
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

                LoadTabPagePerFileDialog();
                e.Handled = true;
                return;
            }
            // Load Tab from File
            if (e.KeyData == (Keys.Control | Keys.L))
            {

                LoadTabPagePerFileDialog();
                e.Handled = true;
                return;
            }

            // store SQL
            if (e.KeyData == (Keys.Control | Keys.S))
            {

                Save();
                e.Handled = true;
                return;
            }
            // store All SQL
            if (e.KeyData == (Keys.Control | Keys.Shift | Keys.S))
            {

                SaveAll();
                e.Handled = true;
                return;
            }
            // run SQL
            if (e.KeyData == (Keys.Control | Keys.R))
            {
                RunSqlTabPage();
                e.Handled = true;
            }

        }
        #endregion

    }
}
