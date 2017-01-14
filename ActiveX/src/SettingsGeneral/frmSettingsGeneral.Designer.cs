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
            this.label2 = new System.Windows.Forms.Label();
            this.chkAdvancedPort = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtFileManagerPath = new System.Windows.Forms.TextBox();
            this.rbLineStyleAndMoreDisableWindow = new System.Windows.Forms.RadioButton();
            this.rbLineStyleAndMoreTabWindow = new System.Windows.Forms.RadioButton();
            this.rbLineStyleAndMoreAddinWindow = new System.Windows.Forms.RadioButton();
            this.chkLineStyleSupport = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShortKeySupport = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkShowServiceButtons = new System.Windows.Forms.CheckBox();
            this.chkShowQueryButtons = new System.Windows.Forms.CheckBox();
            this.chkFavoriteSupport = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAutoLoadMdgNo = new System.Windows.Forms.RadioButton();
            this.rbAutoLoadMdgCompilation = new System.Windows.Forms.RadioButton();
            this.rbAutoLoadMdgBasic = new System.Windows.Forms.RadioButton();
            this.chkConveyedItemSupport = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSqlSearchPath = new System.Windows.Forms.TextBox();
            this.txtAddinTabToFirstActivate = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this._chkQuickSearchSupport = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkReverseEdgeDirection = new System.Windows.Forms.CheckBox();
            this.chkPortBasicSupport = new System.Windows.Forms.CheckBox();
            this.lblPortBasicSupport = new System.Windows.Forms.Label();
            this.chkPortTypeSupport = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(27, 552);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(91, 31);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(124, 552);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 31);
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
            this.groupBox3.Location = new System.Drawing.Point(268, 49);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(269, 43);
            this.groupBox3.TabIndex = 211;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Search and Replace";
            this.toolTip1.SetToolTip(this.groupBox3, "Show Search and Replace Tab with:\r\n- Find simple string or Regular Expression\r\n- " +
        "Name, Description, Stereotype, Tagged Value\r\n- in Packages, Elements, Diagrams, " +
        "Attributes, Operations");
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
            this.chkAdvancedDiagramNote.Location = new System.Drawing.Point(208, 469);
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
            this.label36.Location = new System.Drawing.Point(22, 465);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(154, 18);
            this.label36.TabIndex = 221;
            this.label36.Text = "Diagram Note support";
            this.toolTip1.SetToolTip(this.label36, "Add support for Nots on Diagrams:\r\n- Add Element Note with Element Note content\r\n" +
        "- Add Diagram Note with Diagram Note content");
            // 
            // chkVcSupport
            // 
            this.chkVcSupport.AutoSize = true;
            this.chkVcSupport.Location = new System.Drawing.Point(209, 487);
            this.chkVcSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkVcSupport.Name = "chkVcSupport";
            this.chkVcSupport.Size = new System.Drawing.Size(15, 14);
            this.chkVcSupport.TabIndex = 220;
            this.toolTip1.SetToolTip(this.chkVcSupport, "Add Version Control support:\r\n- Change *.xml file of selected package\r\n- Show fol" +
        "der for selected package with Vesrion Control\r\n- Get VC latest for selected fold" +
        "er (package recursive)\r\n");
            this.chkVcSupport.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(24, 487);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(84, 18);
            this.label35.TabIndex = 219;
            this.label35.Text = "VC Support";
            this.toolTip1.SetToolTip(this.label35, "Add Version Control support:\r\n- Change *.xml file of selected package\r\n- Show fol" +
        "der for selected package with Vesrion Control\r\n- Get VC latest for selected fold" +
        "er (package recursive)");
            // 
            // chkAdvancedFeatures
            // 
            this.chkAdvancedFeatures.AutoSize = true;
            this.chkAdvancedFeatures.Location = new System.Drawing.Point(209, 400);
            this.chkAdvancedFeatures.Margin = new System.Windows.Forms.Padding(2);
            this.chkAdvancedFeatures.Name = "chkAdvancedFeatures";
            this.chkAdvancedFeatures.Size = new System.Drawing.Size(15, 14);
            this.chkAdvancedFeatures.TabIndex = 218;
            this.toolTip1.SetToolTip(this.chkAdvancedFeatures, resources.GetString("chkAdvancedFeatures.ToolTip"));
            this.chkAdvancedFeatures.UseVisualStyleBackColor = true;
            // 
            // chkSvnSupport
            // 
            this.chkSvnSupport.AutoSize = true;
            this.chkSvnSupport.Location = new System.Drawing.Point(209, 514);
            this.chkSvnSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkSvnSupport.Name = "chkSvnSupport";
            this.chkSvnSupport.Size = new System.Drawing.Size(15, 14);
            this.chkSvnSupport.TabIndex = 217;
            this.toolTip1.SetToolTip(this.chkSvnSupport, "Add SVN support\r\n- Show Tortoise Log for Package\r\n- Show Tortoise Repos Browser f" +
        "or Package\r\n");
            this.chkSvnSupport.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(24, 510);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(94, 18);
            this.label33.TabIndex = 216;
            this.label33.Text = "SVN Support";
            this.toolTip1.SetToolTip(this.label33, "Add SVN support");
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(397, 422);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(128, 18);
            this.label41.TabIndex = 214;
            this.label41.Text = "Advanced Support";
            this.toolTip1.SetToolTip(this.label41, "Add Buttons to move multiple Ports like:\r\n- Move left\r\n- Move right\r\n- Move up\r\n-" +
        " Move down");
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(23, 396);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(129, 18);
            this.label29.TabIndex = 215;
            this.label29.Text = "Advanced features";
            this.toolTip1.SetToolTip(this.label29, resources.GetString("label29.ToolTip"));
            // 
            // txtQuickSearch
            // 
            this.txtQuickSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuickSearch.Location = new System.Drawing.Point(273, 247);
            this.txtQuickSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtQuickSearch.Name = "txtQuickSearch";
            this.txtQuickSearch.Size = new System.Drawing.Size(496, 24);
            this.txtQuickSearch.TabIndex = 213;
            this.toolTip1.SetToolTip(this.txtQuickSearch, resources.GetString("txtQuickSearch.ToolTip"));
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
            this.chkAdvancedPort.Location = new System.Drawing.Point(542, 426);
            this.chkAdvancedPort.Margin = new System.Windows.Forms.Padding(2);
            this.chkAdvancedPort.Name = "chkAdvancedPort";
            this.chkAdvancedPort.Size = new System.Drawing.Size(15, 14);
            this.chkAdvancedPort.TabIndex = 224;
            this.toolTip1.SetToolTip(this.chkAdvancedPort, "Add Buttons to move multiple Ports like:\r\n- Move left\r\n- Move right\r\n- Move up\r\n-" +
        " Move down");
            this.chkAdvancedPort.UseVisualStyleBackColor = true;
            // 
            // txtFileManagerPath
            // 
            this.txtFileManagerPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileManagerPath.Location = new System.Drawing.Point(243, 510);
            this.txtFileManagerPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtFileManagerPath.Name = "txtFileManagerPath";
            this.txtFileManagerPath.Size = new System.Drawing.Size(314, 24);
            this.txtFileManagerPath.TabIndex = 226;
            this.toolTip1.SetToolTip(this.txtFileManagerPath, resources.GetString("txtFileManagerPath.ToolTip"));
            // 
            // rbLineStyleAndMoreDisableWindow
            // 
            this.rbLineStyleAndMoreDisableWindow.AutoSize = true;
            this.rbLineStyleAndMoreDisableWindow.Location = new System.Drawing.Point(121, 19);
            this.rbLineStyleAndMoreDisableWindow.Name = "rbLineStyleAndMoreDisableWindow";
            this.rbLineStyleAndMoreDisableWindow.Size = new System.Drawing.Size(75, 22);
            this.rbLineStyleAndMoreDisableWindow.TabIndex = 210;
            this.rbLineStyleAndMoreDisableWindow.TabStop = true;
            this.rbLineStyleAndMoreDisableWindow.Text = "Disable";
            this.toolTip1.SetToolTip(this.rbLineStyleAndMoreDisableWindow, "Disable Linestyle and more Window");
            this.rbLineStyleAndMoreDisableWindow.UseVisualStyleBackColor = true;
            // 
            // rbLineStyleAndMoreTabWindow
            // 
            this.rbLineStyleAndMoreTabWindow.AutoSize = true;
            this.rbLineStyleAndMoreTabWindow.Location = new System.Drawing.Point(70, 19);
            this.rbLineStyleAndMoreTabWindow.Name = "rbLineStyleAndMoreTabWindow";
            this.rbLineStyleAndMoreTabWindow.Size = new System.Drawing.Size(51, 22);
            this.rbLineStyleAndMoreTabWindow.TabIndex = 209;
            this.rbLineStyleAndMoreTabWindow.TabStop = true;
            this.rbLineStyleAndMoreTabWindow.Text = "Tab";
            this.toolTip1.SetToolTip(this.rbLineStyleAndMoreTabWindow, "Add Linestyle and more Window as Tab Winow");
            this.rbLineStyleAndMoreTabWindow.UseVisualStyleBackColor = true;
            // 
            // rbLineStyleAndMoreAddinWindow
            // 
            this.rbLineStyleAndMoreAddinWindow.AutoSize = true;
            this.rbLineStyleAndMoreAddinWindow.Location = new System.Drawing.Point(11, 19);
            this.rbLineStyleAndMoreAddinWindow.Name = "rbLineStyleAndMoreAddinWindow";
            this.rbLineStyleAndMoreAddinWindow.Size = new System.Drawing.Size(62, 22);
            this.rbLineStyleAndMoreAddinWindow.TabIndex = 208;
            this.rbLineStyleAndMoreAddinWindow.TabStop = true;
            this.rbLineStyleAndMoreAddinWindow.Text = "Addin";
            this.toolTip1.SetToolTip(this.rbLineStyleAndMoreAddinWindow, "Add Linestyle and more Window as Addin Winow");
            this.rbLineStyleAndMoreAddinWindow.UseVisualStyleBackColor = true;
            // 
            // chkLineStyleSupport
            // 
            this.chkLineStyleSupport.AutoSize = true;
            this.chkLineStyleSupport.Location = new System.Drawing.Point(209, 360);
            this.chkLineStyleSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkLineStyleSupport.Name = "chkLineStyleSupport";
            this.chkLineStyleSupport.Size = new System.Drawing.Size(15, 14);
            this.chkLineStyleSupport.TabIndex = 228;
            this.toolTip1.SetToolTip(this.chkLineStyleSupport, "Add Buttons for Linestyle");
            this.chkLineStyleSupport.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 356);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 18);
            this.label3.TabIndex = 227;
            this.label3.Text = "Linestyle Support";
            this.toolTip1.SetToolTip(this.label3, "Add Buttons for Linestyle");
            // 
            // chkShortKeySupport
            // 
            this.chkShortKeySupport.AutoSize = true;
            this.chkShortKeySupport.Location = new System.Drawing.Point(208, 154);
            this.chkShortKeySupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkShortKeySupport.Name = "chkShortKeySupport";
            this.chkShortKeySupport.Size = new System.Drawing.Size(15, 14);
            this.chkShortKeySupport.TabIndex = 230;
            this.toolTip1.SetToolTip(this.chkShortKeySupport, resources.GetString("chkShortKeySupport.ToolTip"));
            this.chkShortKeySupport.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(31, 150);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 18);
            this.label4.TabIndex = 229;
            this.label4.Text = "Global Key Support";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(37, 179);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 18);
            this.label5.TabIndex = 231;
            this.label5.Text = "Show Service Buttons";
            this.toolTip1.SetToolTip(this.label5, "Add Buttons to start a Service with just one click");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(37, 197);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 18);
            this.label6.TabIndex = 232;
            this.label6.Text = "Show Search Buttons";
            this.toolTip1.SetToolTip(this.label6, "Add Buttons to start a Search with just one click.\r\n\r\n- EA Search\r\n- SQL Search");
            // 
            // chkShowServiceButtons
            // 
            this.chkShowServiceButtons.AutoSize = true;
            this.chkShowServiceButtons.Location = new System.Drawing.Point(208, 183);
            this.chkShowServiceButtons.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowServiceButtons.Name = "chkShowServiceButtons";
            this.chkShowServiceButtons.Size = new System.Drawing.Size(15, 14);
            this.chkShowServiceButtons.TabIndex = 233;
            this.toolTip1.SetToolTip(this.chkShowServiceButtons, "Buttons to start a Service with just one click");
            this.chkShowServiceButtons.UseVisualStyleBackColor = true;
            // 
            // chkShowQueryButtons
            // 
            this.chkShowQueryButtons.AutoSize = true;
            this.chkShowQueryButtons.Location = new System.Drawing.Point(208, 201);
            this.chkShowQueryButtons.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowQueryButtons.Name = "chkShowQueryButtons";
            this.chkShowQueryButtons.Size = new System.Drawing.Size(15, 14);
            this.chkShowQueryButtons.TabIndex = 234;
            this.toolTip1.SetToolTip(this.chkShowQueryButtons, "Add Buttons to start a Query with just one click");
            this.chkShowQueryButtons.UseVisualStyleBackColor = true;
            // 
            // chkFavoriteSupport
            // 
            this.chkFavoriteSupport.AutoSize = true;
            this.chkFavoriteSupport.Location = new System.Drawing.Point(209, 382);
            this.chkFavoriteSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkFavoriteSupport.Name = "chkFavoriteSupport";
            this.chkFavoriteSupport.Size = new System.Drawing.Size(15, 14);
            this.chkFavoriteSupport.TabIndex = 236;
            this.toolTip1.SetToolTip(this.chkFavoriteSupport, "Add buttons to handle Favorites:\r\n- Element\r\n- Package\r\n- Diagram\r\nadd, remove, s" +
        "how/find");
            this.chkFavoriteSupport.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(23, 378);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 18);
            this.label7.TabIndex = 235;
            this.label7.Text = "Favorites Supports";
            this.toolTip1.SetToolTip(this.label7, "Add buttons to handle Favorites:\r\n- Element\r\n- Package\r\n- Diagram\r\nadd, remove, s" +
        "how/find");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAutoLoadMdgNo);
            this.groupBox2.Controls.Add(this.rbAutoLoadMdgCompilation);
            this.groupBox2.Controls.Add(this.rbAutoLoadMdgBasic);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(554, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 100);
            this.groupBox2.TabIndex = 238;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "hoTools MDG to load";
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // rbAutoLoadMdgNo
            // 
            this.rbAutoLoadMdgNo.AutoSize = true;
            this.rbAutoLoadMdgNo.Location = new System.Drawing.Point(11, 77);
            this.rbAutoLoadMdgNo.Name = "rbAutoLoadMdgNo";
            this.rbAutoLoadMdgNo.Size = new System.Drawing.Size(86, 22);
            this.rbAutoLoadMdgNo.TabIndex = 2;
            this.rbAutoLoadMdgNo.TabStop = true;
            this.rbAutoLoadMdgNo.Text = "No MDG";
            this.toolTip1.SetToolTip(this.rbAutoLoadMdgNo, "No install of MDG.");
            this.rbAutoLoadMdgNo.UseVisualStyleBackColor = true;
            // 
            // rbAutoLoadMdgCompilation
            // 
            this.rbAutoLoadMdgCompilation.AutoSize = true;
            this.rbAutoLoadMdgCompilation.Location = new System.Drawing.Point(11, 49);
            this.rbAutoLoadMdgCompilation.Name = "rbAutoLoadMdgCompilation";
            this.rbAutoLoadMdgCompilation.Size = new System.Drawing.Size(187, 22);
            this.rbAutoLoadMdgCompilation.TabIndex = 1;
            this.rbAutoLoadMdgCompilation.TabStop = true;
            this.rbAutoLoadMdgCompilation.Text = "hoToolsCompilation.xml";
            this.toolTip1.SetToolTip(this.rbAutoLoadMdgCompilation, "Install compilation of useful queries & scripts\r\n\r\nSee in installation folder:\r\nc" +
        ":\\user\\<userName>\\AppData\\Local\\Apps\\hoTools\\..");
            this.rbAutoLoadMdgCompilation.UseVisualStyleBackColor = true;
            // 
            // rbAutoLoadMdgBasic
            // 
            this.rbAutoLoadMdgBasic.AutoSize = true;
            this.rbAutoLoadMdgBasic.Location = new System.Drawing.Point(11, 26);
            this.rbAutoLoadMdgBasic.Name = "rbAutoLoadMdgBasic";
            this.rbAutoLoadMdgBasic.Size = new System.Drawing.Size(145, 22);
            this.rbAutoLoadMdgBasic.TabIndex = 0;
            this.rbAutoLoadMdgBasic.TabStop = true;
            this.rbAutoLoadMdgBasic.Text = "hoToolsBasic.xml";
            this.toolTip1.SetToolTip(this.rbAutoLoadMdgBasic, "Install Basic queries & scripts\r\n\r\nSee in installation folder:\r\nc:\\user\\<userName" +
        ">\\AppData\\Local\\Apps\\hoTools\\..");
            this.rbAutoLoadMdgBasic.UseVisualStyleBackColor = true;
            // 
            // chkConveyedItemSupport
            // 
            this.chkConveyedItemSupport.AutoSize = true;
            this.chkConveyedItemSupport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkConveyedItemSupport.Location = new System.Drawing.Point(243, 360);
            this.chkConveyedItemSupport.Name = "chkConveyedItemSupport";
            this.chkConveyedItemSupport.Size = new System.Drawing.Size(187, 22);
            this.chkConveyedItemSupport.TabIndex = 239;
            this.chkConveyedItemSupport.Text = "Conveyed Items support";
            this.toolTip1.SetToolTip(this.chkConveyedItemSupport, resources.GetString("chkConveyedItemSupport.ToolTip"));
            this.chkConveyedItemSupport.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbLineStyleAndMoreDisableWindow);
            this.groupBox1.Controls.Add(this.rbLineStyleAndMoreTabWindow);
            this.groupBox1.Controls.Add(this.rbLineStyleAndMoreAddinWindow);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(29, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 43);
            this.groupBox1.TabIndex = 212;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "hoTools: Linestyle and more";
            this.toolTip1.SetToolTip(this.groupBox1, "Show Tab hoTools with:\r\n- Linestyle\r\n- Toolbar for Searches and Services\r\n- Versi" +
        "on Control\r\n- Port Support\r\n- Favorites\r\n- Quick Search\r\n- and more");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(36, 296);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 20);
            this.label10.TabIndex = 243;
            this.label10.Text = "SQL path:";
            this.toolTip1.SetToolTip(this.label10, "Paths hoTools searches for SQL Queries to run:\r\n\r\nA semicolon separated list of p" +
        "aths hoTools searches for the SQL query to execute.");
            // 
            // txtSqlSearchPath
            // 
            this.txtSqlSearchPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlSearchPath.Location = new System.Drawing.Point(212, 296);
            this.txtSqlSearchPath.Name = "txtSqlSearchPath";
            this.txtSqlSearchPath.Size = new System.Drawing.Size(557, 24);
            this.txtSqlSearchPath.TabIndex = 242;
            this.toolTip1.SetToolTip(this.txtSqlSearchPath, "Paths hoTools searches for SQL Queries to run:\r\n\r\nA semicolon separated parts of " +
        "path hoTools searches for the SQL query to execute.");
            // 
            // txtAddinTabToFirstActivate
            // 
            this.txtAddinTabToFirstActivate.Location = new System.Drawing.Point(273, 126);
            this.txtAddinTabToFirstActivate.Name = "txtAddinTabToFirstActivate";
            this.txtAddinTabToFirstActivate.Size = new System.Drawing.Size(260, 20);
            this.txtAddinTabToFirstActivate.TabIndex = 245;
            this.toolTip1.SetToolTip(this.txtAddinTabToFirstActivate, "Define your Addin Tab Name to visualize first\r\n- \"\" EA decides\r\n- \"hoTools\"\r\n- \"S" +
        "QL\"\r\n- \"Script\"\r\n- \"your favorible Addin\"\r\n\r\nLeave it blank if you want EA to de" +
        "cide. ");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(37, 247);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 36);
            this.label13.TabIndex = 212;
            this.label13.Text = "Quick Search:\r\n\r\n";
            this.toolTip1.SetToolTip(this.label13, resources.GetString("label13.ToolTip"));
            // 
            // _chkQuickSearchSupport
            // 
            this._chkQuickSearchSupport.AutoSize = true;
            this._chkQuickSearchSupport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._chkQuickSearchSupport.Location = new System.Drawing.Point(208, 250);
            this._chkQuickSearchSupport.Name = "_chkQuickSearchSupport";
            this._chkQuickSearchSupport.Size = new System.Drawing.Size(62, 22);
            this._chkQuickSearchSupport.TabIndex = 247;
            this._chkQuickSearchSupport.Text = "use it";
            this.toolTip1.SetToolTip(this._chkQuickSearchSupport, "Use Quick Search in hoTools.");
            this._chkQuickSearchSupport.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 338);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 18);
            this.label1.TabIndex = 248;
            this.label1.Text = "Reverse edge direction";
            this.toolTip1.SetToolTip(this.label1, "Add Buttons for Linestyle");
            // 
            // chkReverseEdgeDirection
            // 
            this.chkReverseEdgeDirection.AutoSize = true;
            this.chkReverseEdgeDirection.Location = new System.Drawing.Point(208, 338);
            this.chkReverseEdgeDirection.Margin = new System.Windows.Forms.Padding(2);
            this.chkReverseEdgeDirection.Name = "chkReverseEdgeDirection";
            this.chkReverseEdgeDirection.Size = new System.Drawing.Size(15, 14);
            this.chkReverseEdgeDirection.TabIndex = 249;
            this.toolTip1.SetToolTip(this.chkReverseEdgeDirection, "Add Button for reverse edge direction\r\n");
            this.chkReverseEdgeDirection.UseVisualStyleBackColor = true;
            // 
            // chkPortBasicSupport
            // 
            this.chkPortBasicSupport.AutoSize = true;
            this.chkPortBasicSupport.Location = new System.Drawing.Point(243, 426);
            this.chkPortBasicSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkPortBasicSupport.Name = "chkPortBasicSupport";
            this.chkPortBasicSupport.Size = new System.Drawing.Size(15, 14);
            this.chkPortBasicSupport.TabIndex = 251;
            this.toolTip1.SetToolTip(this.chkPortBasicSupport, "Add Buttons to show/hide Port, Parameter, Pin\r\n- Icon\r\n- Label");
            this.chkPortBasicSupport.UseVisualStyleBackColor = true;
            // 
            // lblPortBasicSupport
            // 
            this.lblPortBasicSupport.AutoSize = true;
            this.lblPortBasicSupport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortBasicSupport.Location = new System.Drawing.Point(23, 422);
            this.lblPortBasicSupport.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPortBasicSupport.Name = "lblPortBasicSupport";
            this.lblPortBasicSupport.Size = new System.Drawing.Size(210, 18);
            this.lblPortBasicSupport.TabIndex = 250;
            this.lblPortBasicSupport.Text = "Port, Parameter Basic Support";
            this.toolTip1.SetToolTip(this.lblPortBasicSupport, "Add Buttons to show/hide Port, Parameter, Pin\r\n- Icon\r\n- Label");
            // 
            // chkPortTypeSupport
            // 
            this.chkPortTypeSupport.AutoSize = true;
            this.chkPortTypeSupport.Location = new System.Drawing.Point(365, 426);
            this.chkPortTypeSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkPortTypeSupport.Name = "chkPortTypeSupport";
            this.chkPortTypeSupport.Size = new System.Drawing.Size(15, 14);
            this.chkPortTypeSupport.TabIndex = 253;
            this.toolTip1.SetToolTip(this.chkPortTypeSupport, "Add Buttons to show/hide Port, Parameter, Pin Type");
            this.chkPortTypeSupport.UseVisualStyleBackColor = true;
            this.chkPortTypeSupport.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(265, 422);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 18);
            this.label15.TabIndex = 252;
            this.label15.Text = "Type Support";
            this.toolTip1.SetToolTip(this.label15, "Add Buttons to show/hide Port, Parameter, Pin Type");
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(240, 487);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(297, 18);
            this.label34.TabIndex = 225;
            this.label34.Text = "Path File Manager / Explorer for show folder";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(228, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 18);
            this.label8.TabIndex = 240;
            this.label8.Text = "on Toolbar";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(228, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 18);
            this.label9.TabIndex = 241;
            this.label9.Text = "on Toolbar";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(31, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(205, 18);
            this.label11.TabIndex = 244;
            this.label11.Text = "Addin Tab Name to select first";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(274, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(173, 18);
            this.label12.TabIndex = 246;
            this.label12.Text = "(hoTools, SQL, Script, ..)";
            // 
            // FrmSettingsGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(785, 624);
            this.Controls.Add(this.chkPortTypeSupport);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.chkPortBasicSupport);
            this.Controls.Add(this.lblPortBasicSupport);
            this.Controls.Add(this.chkReverseEdgeDirection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._chkQuickSearchSupport);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtAddinTabToFirstActivate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtSqlSearchPath);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkConveyedItemSupport);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkFavoriteSupport);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkShowQueryButtons);
            this.Controls.Add(this.chkShowServiceButtons);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkShortKeySupport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkLineStyleSupport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
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
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FrmSettingsGeneral";
            this.Text = "hoTools: General Settings";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAdvancedPort;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtFileManagerPath;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbLineStyleAndMoreDisableWindow;
        private System.Windows.Forms.RadioButton rbLineStyleAndMoreTabWindow;
        private System.Windows.Forms.RadioButton rbLineStyleAndMoreAddinWindow;
        private System.Windows.Forms.CheckBox chkLineStyleSupport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkShortKeySupport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkShowServiceButtons;
        private System.Windows.Forms.CheckBox chkShowQueryButtons;
        private System.Windows.Forms.CheckBox chkFavoriteSupport;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkConveyedItemSupport;
        private System.Windows.Forms.RadioButton rbAutoLoadMdgNo;
        private System.Windows.Forms.RadioButton rbAutoLoadMdgCompilation;
        private System.Windows.Forms.RadioButton rbAutoLoadMdgBasic;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSqlSearchPath;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtAddinTabToFirstActivate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox _chkQuickSearchSupport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkReverseEdgeDirection;
        private System.Windows.Forms.CheckBox chkPortBasicSupport;
        private System.Windows.Forms.Label lblPortBasicSupport;
        private System.Windows.Forms.CheckBox chkPortTypeSupport;
        private System.Windows.Forms.Label label15;
    }
}