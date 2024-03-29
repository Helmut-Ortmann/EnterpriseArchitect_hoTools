﻿namespace hoTools.Settings
{
    partial class FrmQueryAndScript
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQueryAndScript));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rbOnlyQueryAddinWindow = new System.Windows.Forms.RadioButton();
            this.rbOnlyQueryTabWindow = new System.Windows.Forms.RadioButton();
            this.rbOnlyQueryDisableWindow = new System.Windows.Forms.RadioButton();
            this.rbScriptAndQueryAddinWindow = new System.Windows.Forms.RadioButton();
            this.rbScriptAndQueryTabWindow = new System.Windows.Forms.RadioButton();
            this.rbScriptAndQueryDisableWindow = new System.Windows.Forms.RadioButton();
            this.chkIsAskForUpdate = new System.Windows.Forms.CheckBox();
            this.txtSqlEditor = new System.Windows.Forms.TextBox();
            this.btnSqlEditor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSqlSearchPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddinTabToFirstActivate = new System.Windows.Forms.TextBox();
            this.txtExtensionPath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsSQLScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(67, 1135);
            this.btnOk.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(280, 74);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(363, 1135);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(280, 74);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(328, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1059, 69);
            this.label1.TabIndex = 2;
            this.label1.Text = "Settings SQL,  Script and c# Extension";
            // 
            // rbOnlyQueryAddinWindow
            // 
            this.rbOnlyQueryAddinWindow.AutoSize = true;
            this.rbOnlyQueryAddinWindow.Location = new System.Drawing.Point(29, 45);
            this.rbOnlyQueryAddinWindow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rbOnlyQueryAddinWindow.Name = "rbOnlyQueryAddinWindow";
            this.rbOnlyQueryAddinWindow.Size = new System.Drawing.Size(153, 48);
            this.rbOnlyQueryAddinWindow.TabIndex = 208;
            this.rbOnlyQueryAddinWindow.TabStop = true;
            this.rbOnlyQueryAddinWindow.Text = "Addin";
            this.toolTip1.SetToolTip(this.rbOnlyQueryAddinWindow, "Show SQL in Addin Window / Addin Tab");
            this.rbOnlyQueryAddinWindow.UseVisualStyleBackColor = true;
            // 
            // rbOnlyQueryTabWindow
            // 
            this.rbOnlyQueryTabWindow.AutoSize = true;
            this.rbOnlyQueryTabWindow.Location = new System.Drawing.Point(187, 45);
            this.rbOnlyQueryTabWindow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rbOnlyQueryTabWindow.Name = "rbOnlyQueryTabWindow";
            this.rbOnlyQueryTabWindow.Size = new System.Drawing.Size(121, 48);
            this.rbOnlyQueryTabWindow.TabIndex = 209;
            this.rbOnlyQueryTabWindow.TabStop = true;
            this.rbOnlyQueryTabWindow.Text = "Tab";
            this.toolTip1.SetToolTip(this.rbOnlyQueryTabWindow, "Show SQL in Tab Window");
            this.rbOnlyQueryTabWindow.UseVisualStyleBackColor = true;
            // 
            // rbOnlyQueryDisableWindow
            // 
            this.rbOnlyQueryDisableWindow.AutoSize = true;
            this.rbOnlyQueryDisableWindow.Location = new System.Drawing.Point(323, 45);
            this.rbOnlyQueryDisableWindow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rbOnlyQueryDisableWindow.Name = "rbOnlyQueryDisableWindow";
            this.rbOnlyQueryDisableWindow.Size = new System.Drawing.Size(183, 48);
            this.rbOnlyQueryDisableWindow.TabIndex = 210;
            this.rbOnlyQueryDisableWindow.TabStop = true;
            this.rbOnlyQueryDisableWindow.Text = "Disable";
            this.toolTip1.SetToolTip(this.rbOnlyQueryDisableWindow, "Disable SQL");
            this.rbOnlyQueryDisableWindow.UseVisualStyleBackColor = true;
            // 
            // rbScriptAndQueryAddinWindow
            // 
            this.rbScriptAndQueryAddinWindow.AutoSize = true;
            this.rbScriptAndQueryAddinWindow.Location = new System.Drawing.Point(29, 45);
            this.rbScriptAndQueryAddinWindow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rbScriptAndQueryAddinWindow.Name = "rbScriptAndQueryAddinWindow";
            this.rbScriptAndQueryAddinWindow.Size = new System.Drawing.Size(153, 48);
            this.rbScriptAndQueryAddinWindow.TabIndex = 208;
            this.rbScriptAndQueryAddinWindow.TabStop = true;
            this.rbScriptAndQueryAddinWindow.Text = "Addin";
            this.toolTip1.SetToolTip(this.rbScriptAndQueryAddinWindow, "Show SQL Query and run Script in Addin Windows");
            this.rbScriptAndQueryAddinWindow.UseVisualStyleBackColor = true;
            // 
            // rbScriptAndQueryTabWindow
            // 
            this.rbScriptAndQueryTabWindow.AutoSize = true;
            this.rbScriptAndQueryTabWindow.Location = new System.Drawing.Point(187, 45);
            this.rbScriptAndQueryTabWindow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rbScriptAndQueryTabWindow.Name = "rbScriptAndQueryTabWindow";
            this.rbScriptAndQueryTabWindow.Size = new System.Drawing.Size(121, 48);
            this.rbScriptAndQueryTabWindow.TabIndex = 209;
            this.rbScriptAndQueryTabWindow.TabStop = true;
            this.rbScriptAndQueryTabWindow.Text = "Tab";
            this.toolTip1.SetToolTip(this.rbScriptAndQueryTabWindow, "Show SQL Query and run Script in Tab Windows");
            this.rbScriptAndQueryTabWindow.UseVisualStyleBackColor = true;
            // 
            // rbScriptAndQueryDisableWindow
            // 
            this.rbScriptAndQueryDisableWindow.AutoSize = true;
            this.rbScriptAndQueryDisableWindow.Location = new System.Drawing.Point(323, 45);
            this.rbScriptAndQueryDisableWindow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rbScriptAndQueryDisableWindow.Name = "rbScriptAndQueryDisableWindow";
            this.rbScriptAndQueryDisableWindow.Size = new System.Drawing.Size(183, 48);
            this.rbScriptAndQueryDisableWindow.TabIndex = 210;
            this.rbScriptAndQueryDisableWindow.TabStop = true;
            this.rbScriptAndQueryDisableWindow.Text = "Disable";
            this.toolTip1.SetToolTip(this.rbScriptAndQueryDisableWindow, "Disable SQL Query and run Script");
            this.rbScriptAndQueryDisableWindow.UseVisualStyleBackColor = true;
            // 
            // chkIsAskForUpdate
            // 
            this.chkIsAskForUpdate.AutoSize = true;
            this.chkIsAskForUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsAskForUpdate.Location = new System.Drawing.Point(1003, 537);
            this.chkIsAskForUpdate.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chkIsAskForUpdate.Name = "chkIsAskForUpdate";
            this.chkIsAskForUpdate.Size = new System.Drawing.Size(34, 33);
            this.chkIsAskForUpdate.TabIndex = 212;
            this.toolTip1.SetToolTip(this.chkIsAskForUpdate, "If checked: SQL will ask to update  query display content if *.sql file has chang" +
        "ed outside.\r\n\r\nYou may use your beloved Editor.");
            this.chkIsAskForUpdate.UseVisualStyleBackColor = true;
            // 
            // txtSqlEditor
            // 
            this.txtSqlEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlEditor.Location = new System.Drawing.Point(75, 577);
            this.txtSqlEditor.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtSqlEditor.Name = "txtSqlEditor";
            this.txtSqlEditor.Size = new System.Drawing.Size(713, 50);
            this.txtSqlEditor.TabIndex = 214;
            this.toolTip1.SetToolTip(this.txtSqlEditor, resources.GetString("txtSqlEditor.ToolTip"));
            this.txtSqlEditor.Visible = false;
            // 
            // btnSqlEditor
            // 
            this.btnSqlEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSqlEditor.Location = new System.Drawing.Point(1003, 579);
            this.btnSqlEditor.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnSqlEditor.Name = "btnSqlEditor";
            this.btnSqlEditor.Size = new System.Drawing.Size(488, 55);
            this.btnSqlEditor.TabIndex = 215;
            this.btnSqlEditor.Text = "Find SQL editor";
            this.toolTip1.SetToolTip(this.btnSqlEditor, "Click to specify your beloved SQL editor. If nothing is entered hoTools uses the " +
        "Windows preferences for *.sql type.\r\n\r\nCurrently not supported.");
            this.btnSqlEditor.UseVisualStyleBackColor = true;
            this.btnSqlEditor.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOnlyQueryDisableWindow);
            this.groupBox1.Controls.Add(this.rbOnlyQueryTabWindow);
            this.groupBox1.Controls.Add(this.rbOnlyQueryAddinWindow);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(67, 272);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Size = new System.Drawing.Size(891, 103);
            this.groupBox1.TabIndex = 210;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit and run SQL";
            this.toolTip1.SetToolTip(this.groupBox1, "Select if you want to show the SQL:\r\n- As Addin Tab\r\n- As Own Window\r\n- Not");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbScriptAndQueryDisableWindow);
            this.groupBox2.Controls.Add(this.rbScriptAndQueryTabWindow);
            this.groupBox2.Controls.Add(this.rbScriptAndQueryAddinWindow);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(67, 410);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox2.Size = new System.Drawing.Size(891, 103);
            this.groupBox2.TabIndex = 211;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Run Script on SQL results";
            this.toolTip1.SetToolTip(this.groupBox2, "Select if you want to show:\r\n- As Addin Tab\r\n- As Own Window\r\n- Not");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(77, 527);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(808, 44);
            this.label2.TabIndex = 213;
            this.label2.Text = "Ask for update if *.sql file has changed outside.";
            this.toolTip1.SetToolTip(this.label2, "Ask for update if File has changed outside hoTools.");
            // 
            // txtSqlSearchPath
            // 
            this.txtSqlSearchPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlSearchPath.Location = new System.Drawing.Point(75, 696);
            this.txtSqlSearchPath.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtSqlSearchPath.Multiline = true;
            this.txtSqlSearchPath.Name = "txtSqlSearchPath";
            this.txtSqlSearchPath.Size = new System.Drawing.Size(1650, 121);
            this.txtSqlSearchPath.TabIndex = 216;
            this.toolTip1.SetToolTip(this.txtSqlSearchPath, resources.GetString("txtSqlSearchPath.ToolTip"));
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(75, 651);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(671, 44);
            this.label3.TabIndex = 217;
            this.label3.Text = "SQL path to search for SQL to execute";
            this.toolTip1.SetToolTip(this.label3, "Paths hoTools searches for SQL Queries to run:\r\n\r\nA semicolon seperated list of p" +
        "aths hoTools searches for the SQL query to execute.\r\n\r\nLike: c:\\temp\\sql;d:\\temp" +
        "\\sql");
            // 
            // txtAddinTabToFirstActivate
            // 
            this.txtAddinTabToFirstActivate.Location = new System.Drawing.Point(629, 844);
            this.txtAddinTabToFirstActivate.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtAddinTabToFirstActivate.Name = "txtAddinTabToFirstActivate";
            this.txtAddinTabToFirstActivate.Size = new System.Drawing.Size(687, 38);
            this.txtAddinTabToFirstActivate.TabIndex = 248;
            this.toolTip1.SetToolTip(this.txtAddinTabToFirstActivate, "Define your Addin Tab Name to visualize first\r\n- \"\" EA decides\r\n- \"hoTools\"\r\n- \"S" +
        "QL\"\r\n- \"Script\"\r\n- \"your favorible Addin\"\r\n\r\nLeave it blank if you want EA to de" +
        "cide. ");
            // 
            // txtExtensionPath
            // 
            this.txtExtensionPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtensionPath.Location = new System.Drawing.Point(75, 980);
            this.txtExtensionPath.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.txtExtensionPath.Name = "txtExtensionPath";
            this.txtExtensionPath.Size = new System.Drawing.Size(1423, 50);
            this.txtExtensionPath.TabIndex = 251;
            this.toolTip1.SetToolTip(this.txtExtensionPath, "Paths hoTools Extensions developed in C#:\r\n\r\nA semicolon seperated list of paths " +
        "hoTools Extensions.\r\n\r\nLike: c:\\hoToolsExtensions;d:\\hoToolsExtension");
            this.txtExtensionPath.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(1339, 844);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(439, 44);
            this.label12.TabIndex = 249;
            this.label12.Text = "(hoTools, SQL, Script, ..)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(67, 849);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(528, 44);
            this.label11.TabIndex = 247;
            this.label11.Text = "Addin Tab Name to select first";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(16, 5, 0, 5);
            this.menuStrip1.Size = new System.Drawing.Size(2069, 65);
            this.menuStrip1.TabIndex = 250;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsSQLScriptToolStripMenuItem,
            this.sQLToolStripMenuItem,
            this.scriptToolStripMenuItem});
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(124, 55);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // settingsSQLScriptToolStripMenuItem
            // 
            this.settingsSQLScriptToolStripMenuItem.Name = "settingsSQLScriptToolStripMenuItem";
            this.settingsSQLScriptToolStripMenuItem.Size = new System.Drawing.Size(545, 60);
            this.settingsSQLScriptToolStripMenuItem.Text = "Settings SQL + Script";
            this.settingsSQLScriptToolStripMenuItem.Click += new System.EventHandler(this.settingsSQLScriptToolStripMenuItem_Click);
            // 
            // sQLToolStripMenuItem
            // 
            this.sQLToolStripMenuItem.Name = "sQLToolStripMenuItem";
            this.sQLToolStripMenuItem.Size = new System.Drawing.Size(545, 60);
            this.sQLToolStripMenuItem.Text = "SQL";
            this.sQLToolStripMenuItem.Click += new System.EventHandler(this.sQLToolStripMenuItem_Click);
            // 
            // scriptToolStripMenuItem
            // 
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(545, 60);
            this.scriptToolStripMenuItem.Text = "Script";
            this.scriptToolStripMenuItem.Click += new System.EventHandler(this.scriptToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(63, 55);
            this.toolStripMenuItem1.Text = "?";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(75, 935);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(735, 44);
            this.label4.TabIndex = 252;
            this.label4.Text = "Path to hoTools .NET Extensions (e.g.:C#)";
            this.label4.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(341, 188);
            this.textBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1276, 38);
            this.textBox1.TabIndex = 253;
            this.textBox1.Text = "More settings: File, Settings General (LINQPad and more)";
            // 
            // FrmQueryAndScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2069, 1362);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtExtensionPath);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtAddinTabToFirstActivate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSqlSearchPath);
            this.Controls.Add(this.btnSqlEditor);
            this.Controls.Add(this.txtSqlEditor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkIsAskForUpdate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "FrmQueryAndScript";
            this.Text = "hoTools: Settings SQL, Script, .NET Extensions";
            this.Shown += new System.EventHandler(this.FrmQueryAndScript_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbOnlyQueryDisableWindow;
        private System.Windows.Forms.RadioButton rbOnlyQueryTabWindow;
        private System.Windows.Forms.RadioButton rbOnlyQueryAddinWindow;
        private System.Windows.Forms.RadioButton rbScriptAndQueryAddinWindow;
        private System.Windows.Forms.RadioButton rbScriptAndQueryTabWindow;
        private System.Windows.Forms.RadioButton rbScriptAndQueryDisableWindow;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkIsAskForUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtSqlEditor;
        private System.Windows.Forms.Button btnSqlEditor;
        private System.Windows.Forms.TextBox txtSqlSearchPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtAddinTabToFirstActivate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsSQLScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TextBox txtExtensionPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
    }
}