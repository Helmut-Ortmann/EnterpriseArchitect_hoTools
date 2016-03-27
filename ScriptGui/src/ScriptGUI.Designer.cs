
namespace hoTools.Scripts
{
    partial class ScriptGUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.btnRunScriptForSql = new System.Windows.Forms.Button();
            this.btnRunScriptForSqlWithAsk = new System.Windows.Forms.Button();
            this.txtBoxSql = new System.Windows.Forms.TextBox();
            this.contextSqlTextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSqlElement = new System.Windows.Forms.ToolStripMenuItem();
            this.diagramTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attributeTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertSearchTermForStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertCurrentItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertCurrentItemGUIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxHistory = new System.Windows.Forms.ToolStripComboBox();
            this.btnLoadScripts = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLastSqlErrorToolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewScripts = new System.Windows.Forms.DataGridView();
            this.contextMenuStripDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowErrorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabSqlPage1 = new System.Windows.Forms.TabPage();
            this.tabControlSql = new System.Windows.Forms.TabControl();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FileNewTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextSqlTextMenuStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripts)).BeginInit();
            this.contextMenuStripDataGrid.SuspendLayout();
            this.tabSqlPage1.SuspendLayout();
            this.tabControlSql.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Location = new System.Drawing.Point(160, 5);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(237, 20);
            this.txtSearchTerm.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtSearchTerm, "Enter a search text");
            // 
            // btnRunScriptForSql
            // 
            this.btnRunScriptForSql.Location = new System.Drawing.Point(206, 435);
            this.btnRunScriptForSql.Name = "btnRunScriptForSql";
            this.btnRunScriptForSql.Size = new System.Drawing.Size(173, 23);
            this.btnRunScriptForSql.TabIndex = 4;
            this.btnRunScriptForSql.Text = "Run Query and Script";
            this.toolTip1.SetToolTip(this.btnRunScriptForSql, "Run Query and execurte Script for all found Elements");
            this.btnRunScriptForSql.UseVisualStyleBackColor = true;
            this.btnRunScriptForSql.Click += new System.EventHandler(this.btnRunScriptForSql_Click);
            // 
            // btnRunScriptForSqlWithAsk
            // 
            this.btnRunScriptForSqlWithAsk.Location = new System.Drawing.Point(395, 435);
            this.btnRunScriptForSqlWithAsk.Name = "btnRunScriptForSqlWithAsk";
            this.btnRunScriptForSqlWithAsk.Size = new System.Drawing.Size(173, 23);
            this.btnRunScriptForSqlWithAsk.TabIndex = 8;
            this.btnRunScriptForSqlWithAsk.Text = "Run Query, Script and ask";
            this.toolTip1.SetToolTip(this.btnRunScriptForSqlWithAsk, "Run Query until next found element, execute Script for this element and ask to:\r\n" +
        "- Break\r\n- Proceed next\r\n- Proceed all");
            this.btnRunScriptForSqlWithAsk.UseVisualStyleBackColor = true;
            this.btnRunScriptForSqlWithAsk.Click += new System.EventHandler(this.btnRunScriptForSqlWithAsk_Click);
            // 
            // txtBoxSql
            // 
            this.txtBoxSql.AcceptsReturn = true;
            this.txtBoxSql.AcceptsTab = true;
            this.txtBoxSql.ContextMenuStrip = this.contextSqlTextMenuStrip;
            this.txtBoxSql.Location = new System.Drawing.Point(10, 0);
            this.txtBoxSql.Multiline = true;
            this.txtBoxSql.Name = "txtBoxSql";
            this.txtBoxSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxSql.Size = new System.Drawing.Size(726, 205);
            this.txtBoxSql.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtBoxSql, "Insert SQL.\r\n\r\nYou may use context menues for advanced features like:\r\n- template" +
        "s\r\n- EA macros (#Branch#, #Package#,..)");
            this.txtBoxSql.TextChanged += new System.EventHandler(this.txtBoxSql_TextChanged);
            // 
            // contextSqlTextMenuStrip
            // 
            this.contextSqlTextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSqlElement,
            this.diagramTemplateToolStripMenuItem,
            this.packageTemplateToolStripMenuItem,
            this.attributeTemplateToolStripMenuItem,
            this.operationTemplateToolStripMenuItem,
            this.insertToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadSQLToolStripMenuItem,
            this.addTabToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripComboBoxHistory});
            this.contextSqlTextMenuStrip.Name = "contextMenuStripSql";
            this.contextSqlTextMenuStrip.Size = new System.Drawing.Size(182, 257);
            // 
            // toolStripMenuItemSqlElement
            // 
            this.toolStripMenuItemSqlElement.Name = "toolStripMenuItemSqlElement";
            this.toolStripMenuItemSqlElement.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItemSqlElement.Text = "ElementTemplate";
            this.toolStripMenuItemSqlElement.Click += new System.EventHandler(this.elementTemplateToolStripMenu_Click);
            // 
            // diagramTemplateToolStripMenuItem
            // 
            this.diagramTemplateToolStripMenuItem.Name = "diagramTemplateToolStripMenuItem";
            this.diagramTemplateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.diagramTemplateToolStripMenuItem.Text = "DiagramTemplate";
            this.diagramTemplateToolStripMenuItem.Click += new System.EventHandler(this.diagramTemplateToolStripMenuItem_Click);
            // 
            // packageTemplateToolStripMenuItem
            // 
            this.packageTemplateToolStripMenuItem.Name = "packageTemplateToolStripMenuItem";
            this.packageTemplateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.packageTemplateToolStripMenuItem.Text = "PackageTemplate";
            this.packageTemplateToolStripMenuItem.Click += new System.EventHandler(this.packageTemplateToolStripMenuItem_Click);
            // 
            // attributeTemplateToolStripMenuItem
            // 
            this.attributeTemplateToolStripMenuItem.Name = "attributeTemplateToolStripMenuItem";
            this.attributeTemplateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.attributeTemplateToolStripMenuItem.Text = "AttributeTemplate";
            this.attributeTemplateToolStripMenuItem.Click += new System.EventHandler(this.attributeTemplateToolStripMenuItem_Click);
            // 
            // operationTemplateToolStripMenuItem
            // 
            this.operationTemplateToolStripMenuItem.Name = "operationTemplateToolStripMenuItem";
            this.operationTemplateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.operationTemplateToolStripMenuItem.Text = "OperationTemplate";
            this.operationTemplateToolStripMenuItem.Click += new System.EventHandler(this.operationTemplateToolStripMenuItem_Click);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertSearchTermForStripMenuItem,
            this.insertPackageToolStripMenuItem,
            this.insertBranchToolStripMenuItem,
            this.insertCurrentItemToolStripMenuItem,
            this.insertCurrentItemGUIDToolStripMenuItem});
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.insertToolStripMenuItem.Text = "Insert";
            // 
            // insertSearchTermForStripMenuItem
            // 
            this.insertSearchTermForStripMenuItem.Name = "insertSearchTermForStripMenuItem";
            this.insertSearchTermForStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.insertSearchTermForStripMenuItem.Text = "Insert Search Term";
            this.insertSearchTermForStripMenuItem.Click += new System.EventHandler(this.insertSearchTermForStripMenuItem_Click);
            // 
            // insertPackageToolStripMenuItem
            // 
            this.insertPackageToolStripMenuItem.Name = "insertPackageToolStripMenuItem";
            this.insertPackageToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.insertPackageToolStripMenuItem.Text = "Insert Package";
            this.insertPackageToolStripMenuItem.Click += new System.EventHandler(this.insertPackageToolStripMenuItem_Click);
            // 
            // insertBranchToolStripMenuItem
            // 
            this.insertBranchToolStripMenuItem.Name = "insertBranchToolStripMenuItem";
            this.insertBranchToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.insertBranchToolStripMenuItem.Text = "Insert Branch";
            this.insertBranchToolStripMenuItem.Click += new System.EventHandler(this.insertBranchToolStripMenuItem_Click);
            // 
            // insertCurrentItemToolStripMenuItem
            // 
            this.insertCurrentItemToolStripMenuItem.Name = "insertCurrentItemToolStripMenuItem";
            this.insertCurrentItemToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.insertCurrentItemToolStripMenuItem.Text = "Insert Current Item";
            this.insertCurrentItemToolStripMenuItem.Click += new System.EventHandler(this.insertCurrentItemToolStripMenuItem_Click);
            // 
            // insertCurrentItemGUIDToolStripMenuItem
            // 
            this.insertCurrentItemGUIDToolStripMenuItem.Name = "insertCurrentItemGUIDToolStripMenuItem";
            this.insertCurrentItemGUIDToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.insertCurrentItemGUIDToolStripMenuItem.Text = "Insert Current Item GUID";
            this.insertCurrentItemGUIDToolStripMenuItem.Click += new System.EventHandler(this.insertCurrentItemGUIDToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveToolStripMenuItem.Text = "Save SQL";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveAsToolStripMenuItem.Text = "Save SQL as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadSQLToolStripMenuItem
            // 
            this.loadSQLToolStripMenuItem.Name = "loadSQLToolStripMenuItem";
            this.loadSQLToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loadSQLToolStripMenuItem.Text = "Load SQL";
            this.loadSQLToolStripMenuItem.ToolTipText = "Load *.sql from file";
            this.loadSQLToolStripMenuItem.Click += new System.EventHandler(this.loadSQLToolStripMenuItem_Click);
            // 
            // addTabToolStripMenuItem
            // 
            this.addTabToolStripMenuItem.Name = "addTabToolStripMenuItem";
            this.addTabToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.addTabToolStripMenuItem.Text = "AddTab";
            this.addTabToolStripMenuItem.Click += new System.EventHandler(this.addTabToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // toolStripComboBoxHistory
            // 
            this.toolStripComboBoxHistory.Name = "toolStripComboBoxHistory";
            this.toolStripComboBoxHistory.Size = new System.Drawing.Size(121, 23);
            // 
            // btnLoadScripts
            // 
            this.btnLoadScripts.Location = new System.Drawing.Point(17, 435);
            this.btnLoadScripts.Name = "btnLoadScripts";
            this.btnLoadScripts.Size = new System.Drawing.Size(173, 23);
            this.btnLoadScripts.TabIndex = 2;
            this.btnLoadScripts.Text = "Load Scripts";
            this.btnLoadScripts.UseVisualStyleBackColor = true;
            this.btnLoadScripts.Click += new System.EventHandler(this.btnLoadScripts_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(403, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scripts && Queries";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(769, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.FileNewTabToolStripMenuItem,
            this.showLastSqlErrorToolStripTextBox1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // showLastSqlErrorToolStripTextBox1
            // 
            this.showLastSqlErrorToolStripTextBox1.Name = "showLastSqlErrorToolStripTextBox1";
            this.showLastSqlErrorToolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.showLastSqlErrorToolStripTextBox1.Text = "Show&LastSqlError";
            this.showLastSqlErrorToolStripTextBox1.Click += new System.EventHandler(this.showSqlErrorToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "&Help";
            // 
            // dataGridViewScripts
            // 
            this.dataGridViewScripts.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewScripts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewScripts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScripts.ContextMenuStrip = this.contextMenuStripDataGrid;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewScripts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewScripts.Location = new System.Drawing.Point(17, 289);
            this.dataGridViewScripts.Name = "dataGridViewScripts";
            this.dataGridViewScripts.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewScripts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewScripts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewScripts.Size = new System.Drawing.Size(730, 140);
            this.dataGridViewScripts.TabIndex = 3;
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
            // tabSqlPage1
            // 
            this.tabSqlPage1.Controls.Add(this.txtBoxSql);
            this.tabSqlPage1.Location = new System.Drawing.Point(4, 22);
            this.tabSqlPage1.Name = "tabSqlPage1";
            this.tabSqlPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabSqlPage1.Size = new System.Drawing.Size(736, 211);
            this.tabSqlPage1.TabIndex = 0;
            this.tabSqlPage1.Text = "NoName1.sql";
            this.tabSqlPage1.UseVisualStyleBackColor = true;
            // 
            // tabControlSql
            // 
            this.tabControlSql.Controls.Add(this.tabSqlPage1);
            this.tabControlSql.Location = new System.Drawing.Point(3, 27);
            this.tabControlSql.Name = "tabControlSql";
            this.tabControlSql.SelectedIndex = 0;
            this.tabControlSql.Size = new System.Drawing.Size(744, 237);
            this.tabControlSql.TabIndex = 5;
            // 
            // btnRunSql
            // 
            this.btnRunSql.Location = new System.Drawing.Point(17, 260);
            this.btnRunSql.Name = "btnRunSql";
            this.btnRunSql.Size = new System.Drawing.Size(75, 23);
            this.btnRunSql.TabIndex = 6;
            this.btnRunSql.Text = "Run";
            this.btnRunSql.UseVisualStyleBackColor = true;
            this.btnRunSql.Click += new System.EventHandler(this.btnRunSql_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // FileNewTabToolStripMenuItem
            // 
            this.FileNewTabToolStripMenuItem.Name = "FileNewTabToolStripMenuItem";
            this.FileNewTabToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.FileNewTabToolStripMenuItem.Text = "&NewTab";
            this.FileNewTabToolStripMenuItem.Click += new System.EventHandler(this.FileNewTabToolStripMenuItem_Click);
            // 
            // ScriptGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRunScriptForSqlWithAsk);
            this.Controls.Add(this.txtSearchTerm);
            this.Controls.Add(this.btnRunSql);
            this.Controls.Add(this.tabControlSql);
            this.Controls.Add(this.btnRunScriptForSql);
            this.Controls.Add(this.dataGridViewScripts);
            this.Controls.Add(this.btnLoadScripts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ScriptGUI";
            this.Size = new System.Drawing.Size(769, 461);
            this.Load += new System.EventHandler(this.ScriptGUI_Load);
            this.contextSqlTextMenuStrip.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripts)).EndInit();
            this.contextMenuStripDataGrid.ResumeLayout(false);
            this.tabSqlPage1.ResumeLayout(false);
            this.tabSqlPage1.PerformLayout();
            this.tabControlSql.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadScripts;
        private System.Windows.Forms.DataGridView dataGridViewScripts;
        private System.Windows.Forms.Button btnRunScriptForSql;
        private System.Windows.Forms.TabPage tabSqlPage1;
        private System.Windows.Forms.TabControl tabControlSql;
        private System.Windows.Forms.TextBox txtBoxSql;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDataGrid;
        private System.Windows.Forms.ToolStripMenuItem ShowErrorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showScriptToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextSqlTextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSqlElement;
        private System.Windows.Forms.ToolStripMenuItem diagramTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packageTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxHistory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnRunSql;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnRunScriptForSqlWithAsk;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertSearchTermForStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertPackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertBranchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertCurrentItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertCurrentItemGUIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attributeTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox showLastSqlErrorToolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem FileNewTabToolStripMenuItem;
    }
}
