
namespace hoTools.Query
{
    partial class QueryGUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryGUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.btnRunScriptForSql = new System.Windows.Forms.Button();
            this.btnRunScriptForSqlWithAsk = new System.Windows.Forms.Button();
            this.dataGridViewScripts = new System.Windows.Forms.DataGridView();
            this.contextMenuStripDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowErrorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlSql = new System.Windows.Forms.TabControl();
            this.btnLoadScripts = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTabFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.newTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTabFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lastsqlErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripts)).BeginInit();
            this.contextMenuStripDataGrid.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Location = new System.Drawing.Point(167, 3);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(223, 20);
            this.txtSearchTerm.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtSearchTerm, resources.GetString("txtSearchTerm.ToolTip"));
            this.txtSearchTerm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchTerm_KeyDown);
            this.txtSearchTerm.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchTerm_KeyDown);
            this.txtSearchTerm.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtSearchTerm_MouseDoubleClick);
            // 
            // btnRunScriptForSql
            // 
            this.btnRunScriptForSql.Location = new System.Drawing.Point(129, 3);
            this.btnRunScriptForSql.Name = "btnRunScriptForSql";
            this.btnRunScriptForSql.Size = new System.Drawing.Size(120, 23);
            this.btnRunScriptForSql.TabIndex = 4;
            this.btnRunScriptForSql.Text = "Run Query and Script";
            this.toolTip1.SetToolTip(this.btnRunScriptForSql, "Run Query and execute Script for all found Elements");
            this.btnRunScriptForSql.UseVisualStyleBackColor = true;
            this.btnRunScriptForSql.Click += new System.EventHandler(this.btnRunScriptForSql_Click);
            // 
            // btnRunScriptForSqlWithAsk
            // 
            this.btnRunScriptForSqlWithAsk.Location = new System.Drawing.Point(255, 3);
            this.btnRunScriptForSqlWithAsk.Name = "btnRunScriptForSqlWithAsk";
            this.btnRunScriptForSqlWithAsk.Size = new System.Drawing.Size(120, 23);
            this.btnRunScriptForSqlWithAsk.TabIndex = 8;
            this.btnRunScriptForSqlWithAsk.Text = "Run Query, Script and ask";
            this.toolTip1.SetToolTip(this.btnRunScriptForSqlWithAsk, "Run Query until next found element, execute Script for this element and ask to:\r\n" +
        "- Break\r\n- Proceed next\r\n- Proceed all");
            this.btnRunScriptForSqlWithAsk.UseVisualStyleBackColor = true;
            this.btnRunScriptForSqlWithAsk.Click += new System.EventHandler(this.btnRunScriptForSqlWithAsk_Click);
            // 
            // dataGridViewScripts
            // 
            this.dataGridViewScripts.AllowUserToOrderColumns = true;
            this.dataGridViewScripts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewScripts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewScripts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScripts.ContextMenuStrip = this.contextMenuStripDataGrid;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewScripts.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewScripts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScripts.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewScripts.Name = "dataGridViewScripts";
            this.dataGridViewScripts.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewScripts.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewScripts.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridViewScripts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewScripts.Size = new System.Drawing.Size(767, 146);
            this.dataGridViewScripts.TabIndex = 3;
            this.toolTip1.SetToolTip(this.dataGridViewScripts, resources.GetString("dataGridViewScripts.ToolTip"));
            this.dataGridViewScripts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewScripts_MouseClick);
            // 
            // contextMenuStripDataGrid
            // 
            this.contextMenuStripDataGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowErrorToolStripMenuItem1,
            this.runScriptToolStripMenuItem,
            this.showScriptToolStripMenuItem});
            this.contextMenuStripDataGrid.Name = "contextMenuStripDataGrid";
            this.contextMenuStripDataGrid.Size = new System.Drawing.Size(162, 70);
            this.contextMenuStripDataGrid.Text = "C";
            // 
            // ShowErrorToolStripMenuItem1
            // 
            this.ShowErrorToolStripMenuItem1.Name = "ShowErrorToolStripMenuItem1";
            this.ShowErrorToolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
            this.ShowErrorToolStripMenuItem1.Text = "Show&Script Error";
            this.ShowErrorToolStripMenuItem1.Click += new System.EventHandler(this.ShowScriptErrorToolStripMenuItem_Click);
            // 
            // runScriptToolStripMenuItem
            // 
            this.runScriptToolStripMenuItem.Name = "runScriptToolStripMenuItem";
            this.runScriptToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.runScriptToolStripMenuItem.Text = "&RunScript";
            this.runScriptToolStripMenuItem.Click += new System.EventHandler(this.runScriptToolStripMenuItem_Click);
            // 
            // showScriptToolStripMenuItem
            // 
            this.showScriptToolStripMenuItem.Name = "showScriptToolStripMenuItem";
            this.showScriptToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.showScriptToolStripMenuItem.Text = "&ShowScript";
            this.showScriptToolStripMenuItem.Click += new System.EventHandler(this.showScriptToolStripMenuItem_Click);
            // 
            // tabControlSql
            // 
            this.tabControlSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSql.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlSql.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlSql.HotTrack = true;
            this.tabControlSql.ItemSize = new System.Drawing.Size(0, 18);
            this.tabControlSql.Location = new System.Drawing.Point(0, 0);
            this.tabControlSql.Multiline = true;
            this.tabControlSql.Name = "tabControlSql";
            this.tabControlSql.SelectedIndex = 0;
            this.tabControlSql.ShowToolTips = true;
            this.tabControlSql.Size = new System.Drawing.Size(767, 307);
            this.tabControlSql.TabIndex = 5;
            this.toolTip1.SetToolTip(this.tabControlSql, "Enter SQL code. EA macros like:\r\n- #Branch#\r\n- #ObjectID#\r\n- #ObjectGUID#\r\n- #Pac" +
        "kage#\r\n- \'Search Term\'\r\n\r\nare allowed.");
            this.tabControlSql.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlSql_DrawItem);
            this.tabControlSql.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControlSql_DragDrop);
            this.tabControlSql.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControlSql_DragEnter);
            this.tabControlSql.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer_DragOver);
            this.tabControlSql.DragLeave += new System.EventHandler(this.tabControlSql_DragLeave);
            this.tabControlSql.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlSql_MouseDown);
            // 
            // btnLoadScripts
            // 
            this.btnLoadScripts.Location = new System.Drawing.Point(3, 3);
            this.btnLoadScripts.Name = "btnLoadScripts";
            this.btnLoadScripts.Size = new System.Drawing.Size(120, 23);
            this.btnLoadScripts.TabIndex = 2;
            this.btnLoadScripts.Text = "Load Scripts";
            this.btnLoadScripts.UseVisualStyleBackColor = true;
            this.btnLoadScripts.Click += new System.EventHandler(this.btnLoadScripts_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(396, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(196, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Scripts && Queries";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(89, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTabFromToolStripMenuItem,
            this.toolStripSeparator2,
            this.newTabToolStripMenuItem,
            this.newTabFromToolStripMenuItem,
            this.toolStripSeparator1,
            this.runToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator4,
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.fileToolStripMenuItem.ToolTipText = "Create a new Tab from recent *.sql files";
            // 
            // loadTabFromToolStripMenuItem
            // 
            this.loadTabFromToolStripMenuItem.Name = "loadTabFromToolStripMenuItem";
            this.loadTabFromToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.loadTabFromToolStripMenuItem.Text = "Load Tab from...";
            this.loadTabFromToolStripMenuItem.ToolTipText = "Load current Tab from recent file.";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // newTabToolStripMenuItem
            // 
            this.newTabToolStripMenuItem.Name = "newTabToolStripMenuItem";
            this.newTabToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.newTabToolStripMenuItem.Text = "&NewTab";
            this.newTabToolStripMenuItem.ToolTipText = "New empty tab";
            this.newTabToolStripMenuItem.Click += new System.EventHandler(this.FileNewTabToolStripMenuItem_Click);
            // 
            // newTabFromToolStripMenuItem
            // 
            this.newTabFromToolStripMenuItem.Name = "newTabFromToolStripMenuItem";
            this.newTabFromToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.newTabFromToolStripMenuItem.Text = "New Tab from...";
            this.newTabFromToolStripMenuItem.ToolTipText = "New Tab from recent file.";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runSqlTabToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(157, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveSqlTabToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.saveAsToolStripMenuItem.Text = "Save As..";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveSqlTabAsToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(157, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.ToolTipText = "View and change settings of Scripts and Queries";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1,
            this.lastsqlErrorToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem1.Text = "&Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // lastsqlErrorToolStripMenuItem
            // 
            this.lastsqlErrorToolStripMenuItem.Name = "lastsqlErrorToolStripMenuItem";
            this.lastsqlErrorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lastsqlErrorToolStripMenuItem.Text = "Last &sql error";
            this.lastsqlErrorToolStripMenuItem.ToolTipText = "Get the last EA sql error from %APPDATA%Sparx Systems\\EA\\DBError.txt";
            this.lastsqlErrorToolStripMenuItem.Click += new System.EventHandler(this.showSqlErrorToolStripMenuItem_Click);
            // 
            // btnRunSql
            // 
            this.btnRunSql.Location = new System.Drawing.Point(3, 3);
            this.btnRunSql.Name = "btnRunSql";
            this.btnRunSql.Size = new System.Drawing.Size(100, 23);
            this.btnRunSql.TabIndex = 6;
            this.btnRunSql.Text = "Run";
            this.btnRunSql.UseVisualStyleBackColor = true;
            this.btnRunSql.Click += new System.EventHandler(this.btnRunSql_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabControlSql);
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel3);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridViewScripts);
            this.splitContainer.Panel2.Controls.Add(this.flowLayoutPanel4);
            this.splitContainer.Size = new System.Drawing.Size(769, 513);
            this.splitContainer.SplitterDistance = 334;
            this.splitContainer.SplitterWidth = 6;
            this.splitContainer.TabIndex = 9;
            this.splitContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.splitContainer_DragDrop);
            this.splitContainer.DragOver += new System.Windows.Forms.DragEventHandler(this.splitContainer_DragOver);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.btnRunSql);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 307);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(767, 25);
            this.flowLayoutPanel3.TabIndex = 12;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.btnLoadScripts);
            this.flowLayoutPanel4.Controls.Add(this.btnRunScriptForSql);
            this.flowLayoutPanel4.Controls.Add(this.btnRunScriptForSqlWithAsk);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 146);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(767, 25);
            this.flowLayoutPanel4.TabIndex = 13;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.menuStrip1);
            this.flowLayoutPanel2.Controls.Add(this.btnUndo);
            this.flowLayoutPanel2.Controls.Add(this.btnRedo);
            this.flowLayoutPanel2.Controls.Add(this.txtSearchTerm);
            this.flowLayoutPanel2.Controls.Add(this.lblTitle);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(769, 25);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // btnUndo
            // 
            this.btnUndo.Image = global::hoTools.Query.Properties.Resources.icon_undo_h;
            this.btnUndo.Location = new System.Drawing.Point(92, 3);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(32, 23);
            this.btnUndo.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnUndo, "Undo Text change");
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Image = global::hoTools.Query.Properties.Resources.icon_redo_h;
            this.btnRedo.Location = new System.Drawing.Point(130, 3);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(31, 23);
            this.btnRedo.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnRedo, "Redo Text change");
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // QueryGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Name = "QueryGUI";
            this.Size = new System.Drawing.Size(769, 538);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripts)).EndInit();
            this.contextMenuStripDataGrid.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLoadScripts;
        private System.Windows.Forms.DataGridView dataGridViewScripts;
        private System.Windows.Forms.Button btnRunScriptForSql;
        private System.Windows.Forms.TabControl tabControlSql;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDataGrid;
        private System.Windows.Forms.ToolStripMenuItem ShowErrorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showScriptToolStripMenuItem;
        private System.Windows.Forms.Button btnRunSql;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.Button btnRunScriptForSqlWithAsk;
        private System.Windows.Forms.ToolStripMenuItem newTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastsqlErrorToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.ToolStripMenuItem loadTabFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTabFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
    }
}
