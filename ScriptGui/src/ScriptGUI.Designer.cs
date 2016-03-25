
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
            this.txtSearchText = new System.Windows.Forms.TextBox();
            this.btnRunScriptForSql = new System.Windows.Forms.Button();
            this.btnRunScriptForSqlWithAsk = new System.Windows.Forms.Button();
            this.btnLoadScripts = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewScripts = new System.Windows.Forms.DataGridView();
            this.contextMenuStripDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowErrorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabSqlPage1 = new System.Windows.Forms.TabPage();
            this.txtBoxSql = new System.Windows.Forms.TextBox();
            this.contextMenuStripSql = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSqlElement = new System.Windows.Forms.ToolStripMenuItem();
            this.diagramTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxHistory = new System.Windows.Forms.ToolStripComboBox();
            this.tabControlSql = new System.Windows.Forms.TabControl();
            this.btnRunSql = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripts)).BeginInit();
            this.contextMenuStripDataGrid.SuspendLayout();
            this.tabSqlPage1.SuspendLayout();
            this.contextMenuStripSql.SuspendLayout();
            this.tabControlSql.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchText
            // 
            this.txtSearchText.Location = new System.Drawing.Point(160, 5);
            this.txtSearchText.Name = "txtSearchText";
            this.txtSearchText.Size = new System.Drawing.Size(237, 20);
            this.txtSearchText.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtSearchText, "Enter a search text");
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
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.fileToolStripMenuItem.Text = "&FileHistory";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
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
            this.contextMenuStripDataGrid.Size = new System.Drawing.Size(134, 70);
            this.contextMenuStripDataGrid.Text = "C";
            // 
            // ShowErrorToolStripMenuItem1
            // 
            this.ShowErrorToolStripMenuItem1.Name = "ShowErrorToolStripMenuItem1";
            this.ShowErrorToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.ShowErrorToolStripMenuItem1.Text = "Show&Error";
            this.ShowErrorToolStripMenuItem1.Click += new System.EventHandler(this.ShowErrorToolStripMenuItem1_Click);
            // 
            // runScriptToolStripMenuItem
            // 
            this.runScriptToolStripMenuItem.Name = "runScriptToolStripMenuItem";
            this.runScriptToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.runScriptToolStripMenuItem.Text = "&RunScript";
            this.runScriptToolStripMenuItem.Click += new System.EventHandler(this.runScriptToolStripMenuItem_Click);
            // 
            // showScriptToolStripMenuItem
            // 
            this.showScriptToolStripMenuItem.Name = "showScriptToolStripMenuItem";
            this.showScriptToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
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
            // txtBoxSql
            // 
            this.txtBoxSql.ContextMenuStrip = this.contextMenuStripSql;
            this.txtBoxSql.Location = new System.Drawing.Point(10, 0);
            this.txtBoxSql.Multiline = true;
            this.txtBoxSql.Name = "txtBoxSql";
            this.txtBoxSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxSql.Size = new System.Drawing.Size(726, 205);
            this.txtBoxSql.TabIndex = 0;
            // 
            // contextMenuStripSql
            // 
            this.contextMenuStripSql.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSqlElement,
            this.diagramTemplateToolStripMenuItem,
            this.packageTemplateToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadSQLToolStripMenuItem,
            this.addTabToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripComboBoxHistory});
            this.contextMenuStripSql.Name = "contextMenuStripSql";
            this.contextMenuStripSql.Size = new System.Drawing.Size(182, 213);
            // 
            // toolStripMenuItemSqlElement
            // 
            this.toolStripMenuItemSqlElement.Name = "toolStripMenuItemSqlElement";
            this.toolStripMenuItemSqlElement.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItemSqlElement.Text = "ElementTemplate";
            this.toolStripMenuItemSqlElement.Click += new System.EventHandler(this.toolStripMenuItemSqlElement_Click);
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
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveToolStripMenuItem.Text = "Save SQL";
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
            // ScriptGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRunScriptForSqlWithAsk);
            this.Controls.Add(this.txtSearchText);
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
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScripts)).EndInit();
            this.contextMenuStripDataGrid.ResumeLayout(false);
            this.tabSqlPage1.ResumeLayout(false);
            this.tabSqlPage1.PerformLayout();
            this.contextMenuStripSql.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSql;
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
        private System.Windows.Forms.TextBox txtSearchText;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnRunScriptForSqlWithAsk;
    }
}
