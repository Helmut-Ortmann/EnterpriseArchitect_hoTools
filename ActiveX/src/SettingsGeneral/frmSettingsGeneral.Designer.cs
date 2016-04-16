namespace hoTools.Settings
{
    partial class FrmSettingsGeneral
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettingsGeneral));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbSearchAndReplaceDisableWindow = new System.Windows.Forms.RadioButton();
            this.rbSearchAndReplaceTabWindow = new System.Windows.Forms.RadioButton();
            this.rbSearchAndReplaceAddinWindow = new System.Windows.Forms.RadioButton();
            this.chkAdvancedDiagramNote = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.chkVcSupport = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.chkAdvancedFeatures = new System.Windows.Forms.CheckBox();
            this.chkSvnSupport = new System.Windows.Forms.CheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txtQuickSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAdvancedPort = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtFileManagerPath = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(58, 390);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(168, 390);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbSearchAndReplaceDisableWindow);
            this.groupBox3.Controls.Add(this.rbSearchAndReplaceTabWindow);
            this.groupBox3.Controls.Add(this.rbSearchAndReplaceAddinWindow);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(32, 53);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(334, 43);
            this.groupBox3.TabIndex = 211;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Search and Replace Window";
            // 
            // rbSearchAndReplaceDisableWindow
            // 
            this.rbSearchAndReplaceDisableWindow.AutoSize = true;
            this.rbSearchAndReplaceDisableWindow.Location = new System.Drawing.Point(121, 19);
            this.rbSearchAndReplaceDisableWindow.Name = "rbSearchAndReplaceDisableWindow";
            this.rbSearchAndReplaceDisableWindow.Size = new System.Drawing.Size(75, 22);
            this.rbSearchAndReplaceDisableWindow.TabIndex = 210;
            this.rbSearchAndReplaceDisableWindow.TabStop = true;
            this.rbSearchAndReplaceDisableWindow.Text = "Disable";
            this.toolTip1.SetToolTip(this.rbSearchAndReplaceDisableWindow, "Disable Search and Replace Window");
            this.rbSearchAndReplaceDisableWindow.UseVisualStyleBackColor = true;
            // 
            // rbSearchAndReplaceTabWindow
            // 
            this.rbSearchAndReplaceTabWindow.AutoSize = true;
            this.rbSearchAndReplaceTabWindow.Location = new System.Drawing.Point(70, 19);
            this.rbSearchAndReplaceTabWindow.Name = "rbSearchAndReplaceTabWindow";
            this.rbSearchAndReplaceTabWindow.Size = new System.Drawing.Size(51, 22);
            this.rbSearchAndReplaceTabWindow.TabIndex = 209;
            this.rbSearchAndReplaceTabWindow.TabStop = true;
            this.rbSearchAndReplaceTabWindow.Text = "Tab";
            this.toolTip1.SetToolTip(this.rbSearchAndReplaceTabWindow, "Add Search and Replace Window as Tab Winow");
            this.rbSearchAndReplaceTabWindow.UseVisualStyleBackColor = true;
            // 
            // rbSearchAndReplaceAddinWindow
            // 
            this.rbSearchAndReplaceAddinWindow.AutoSize = true;
            this.rbSearchAndReplaceAddinWindow.Location = new System.Drawing.Point(11, 19);
            this.rbSearchAndReplaceAddinWindow.Name = "rbSearchAndReplaceAddinWindow";
            this.rbSearchAndReplaceAddinWindow.Size = new System.Drawing.Size(62, 22);
            this.rbSearchAndReplaceAddinWindow.TabIndex = 208;
            this.rbSearchAndReplaceAddinWindow.TabStop = true;
            this.rbSearchAndReplaceAddinWindow.Text = "Addin";
            this.toolTip1.SetToolTip(this.rbSearchAndReplaceAddinWindow, "Add Search and Replace Window as Addin Winow");
            this.rbSearchAndReplaceAddinWindow.UseVisualStyleBackColor = true;
            // 
            // chkAdvancedDiagramNote
            // 
            this.chkAdvancedDiagramNote.AutoSize = true;
            this.chkAdvancedDiagramNote.Location = new System.Drawing.Point(215, 277);
            this.chkAdvancedDiagramNote.Margin = new System.Windows.Forms.Padding(2);
            this.chkAdvancedDiagramNote.Name = "chkAdvancedDiagramNote";
            this.chkAdvancedDiagramNote.Size = new System.Drawing.Size(15, 14);
            this.chkAdvancedDiagramNote.TabIndex = 222;
            this.toolTip1.SetToolTip(this.chkAdvancedDiagramNote, "Add support for Nots on Diagrams:\r\n- Add Element Note with Element Note content\r\n" +
        "- Add Diagram Note with Diagram Note content");
            this.chkAdvancedDiagramNote.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(29, 273);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(154, 18);
            this.label36.TabIndex = 221;
            this.label36.Text = "Diagram Note support";
            // 
            // chkVcSupport
            // 
            this.chkVcSupport.AutoSize = true;
            this.chkVcSupport.Location = new System.Drawing.Point(215, 182);
            this.chkVcSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkVcSupport.Name = "chkVcSupport";
            this.chkVcSupport.Size = new System.Drawing.Size(15, 14);
            this.chkVcSupport.TabIndex = 220;
            this.toolTip1.SetToolTip(this.chkVcSupport, "Add Version Control support");
            this.chkVcSupport.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(29, 178);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(84, 18);
            this.label35.TabIndex = 219;
            this.label35.Text = "VC Support";
            // 
            // chkAdvancedFeatures
            // 
            this.chkAdvancedFeatures.AutoSize = true;
            this.chkAdvancedFeatures.Location = new System.Drawing.Point(215, 231);
            this.chkAdvancedFeatures.Margin = new System.Windows.Forms.Padding(2);
            this.chkAdvancedFeatures.Name = "chkAdvancedFeatures";
            this.chkAdvancedFeatures.Size = new System.Drawing.Size(15, 14);
            this.chkAdvancedFeatures.TabIndex = 218;
            this.toolTip1.SetToolTip(this.chkAdvancedFeatures, "Add advanved features");
            this.chkAdvancedFeatures.UseVisualStyleBackColor = true;
            // 
            // chkSvnSupport
            // 
            this.chkSvnSupport.AutoSize = true;
            this.chkSvnSupport.Location = new System.Drawing.Point(215, 209);
            this.chkSvnSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkSvnSupport.Name = "chkSvnSupport";
            this.chkSvnSupport.Size = new System.Drawing.Size(15, 14);
            this.chkSvnSupport.TabIndex = 217;
            this.toolTip1.SetToolTip(this.chkSvnSupport, "Add SVN support");
            this.chkSvnSupport.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(29, 205);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(94, 18);
            this.label33.TabIndex = 216;
            this.label33.Text = "SVN Support";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(29, 251);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(158, 18);
            this.label41.TabIndex = 214;
            this.label41.Text = "Advanced Port support";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(29, 227);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(129, 18);
            this.label29.TabIndex = 215;
            this.label29.Text = "Advanced features";
            // 
            // txtQuickSearch
            // 
            this.txtQuickSearch.Location = new System.Drawing.Point(215, 113);
            this.txtQuickSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtQuickSearch.Name = "txtQuickSearch";
            this.txtQuickSearch.Size = new System.Drawing.Size(314, 20);
            this.txtQuickSearch.TabIndex = 213;
            this.toolTip1.SetToolTip(this.txtQuickSearch, resources.GetString("txtQuickSearch.ToolTip"));
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 16);
            this.label1.TabIndex = 212;
            this.label1.Text = "(Quick) EA-Search Name";
            this.toolTip1.SetToolTip(this.label1, "Search Name for searching the value of the entry field after using enter");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(138, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 29);
            this.label2.TabIndex = 223;
            this.label2.Text = "General Setting";
            // 
            // chkAdvancedPort
            // 
            this.chkAdvancedPort.AutoSize = true;
            this.chkAdvancedPort.Location = new System.Drawing.Point(215, 255);
            this.chkAdvancedPort.Margin = new System.Windows.Forms.Padding(2);
            this.chkAdvancedPort.Name = "chkAdvancedPort";
            this.chkAdvancedPort.Size = new System.Drawing.Size(15, 14);
            this.chkAdvancedPort.TabIndex = 224;
            this.toolTip1.SetToolTip(this.chkAdvancedPort, "Add Tool support to easily handle multiple ports on an Element like:\r\n- Move left" +
        "\r\n- Move right\r\n- Move up\r\n- Move down");
            this.chkAdvancedPort.UseVisualStyleBackColor = true;
            // 
            // txtFileManagerPath
            // 
            this.txtFileManagerPath.Location = new System.Drawing.Point(215, 147);
            this.txtFileManagerPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtFileManagerPath.Name = "txtFileManagerPath";
            this.txtFileManagerPath.Size = new System.Drawing.Size(314, 20);
            this.txtFileManagerPath.TabIndex = 226;
            this.toolTip1.SetToolTip(this.txtFileManagerPath, resources.GetString("txtFileManagerPath.ToolTip"));
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(29, 148);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(177, 16);
            this.label34.TabIndex = 225;
            this.label34.Text = "Path File Manager / Explorer";
            // 
            // FrmSettingsGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(844, 458);
            this.Controls.Add(this.txtFileManagerPath);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.chkAdvancedPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkAdvancedDiagramNote);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.chkVcSupport);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.chkAdvancedFeatures);
            this.Controls.Add(this.chkSvnSupport);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.txtQuickSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FrmSettingsGeneral";
            this.Text = "frmSettingsGeneral";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbSearchAndReplaceDisableWindow;
        private System.Windows.Forms.RadioButton rbSearchAndReplaceTabWindow;
        private System.Windows.Forms.RadioButton rbSearchAndReplaceAddinWindow;
        private System.Windows.Forms.CheckBox chkAdvancedDiagramNote;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.CheckBox chkVcSupport;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckBox chkAdvancedFeatures;
        private System.Windows.Forms.CheckBox chkSvnSupport;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtQuickSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAdvancedPort;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtFileManagerPath;
        private System.Windows.Forms.Label label34;
    }
}