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
            this.label34 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(30, 486);
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
            this.btnCancel.Location = new System.Drawing.Point(127, 486);
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
            this.groupBox3.Size = new System.Drawing.Size(334, 43);
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
            this.chkAdvancedDiagramNote.Location = new System.Drawing.Point(212, 374);
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
            this.label36.Location = new System.Drawing.Point(26, 370);
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
            this.chkVcSupport.Location = new System.Drawing.Point(212, 421);
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
            this.label35.Location = new System.Drawing.Point(27, 421);
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
            this.chkAdvancedFeatures.Location = new System.Drawing.Point(212, 334);
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
            this.chkSvnSupport.Location = new System.Drawing.Point(212, 448);
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
            this.label33.Location = new System.Drawing.Point(27, 444);
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
            this.label41.Location = new System.Drawing.Point(26, 348);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(158, 18);
            this.label41.TabIndex = 214;
            this.label41.Text = "Advanced Port support";
            this.toolTip1.SetToolTip(this.label41, "Add Tool support to easily handle multiple ports on an Element like:\r\n- Move left" +
        "\r\n- Move right\r\n- Move up\r\n- Move down");
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(26, 330);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(129, 18);
            this.label29.TabIndex = 215;
            this.label29.Text = "Advanced features";
            this.toolTip1.SetToolTip(this.label29, resources.GetString("label29.ToolTip"));
            // 
            // txtQuickSearch
            // 
            this.txtQuickSearch.Location = new System.Drawing.Point(215, 185);
            this.txtQuickSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtQuickSearch.Name = "txtQuickSearch";
            this.txtQuickSearch.Size = new System.Drawing.Size(314, 20);
            this.txtQuickSearch.TabIndex = 213;
            this.toolTip1.SetToolTip(this.txtQuickSearch, resources.GetString("txtQuickSearch.ToolTip"));
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 185);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 72);
            this.label1.TabIndex = 212;
            this.label1.Text = "(Quick) Search Name\r\n- EA Search Name\r\n- SQL file (\'mySql.sql\')\r\nBlank to hide it" +
    "";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
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
            this.chkAdvancedPort.Location = new System.Drawing.Point(212, 352);
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
            this.txtFileManagerPath.Location = new System.Drawing.Point(246, 444);
            this.txtFileManagerPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtFileManagerPath.Name = "txtFileManagerPath";
            this.txtFileManagerPath.Size = new System.Drawing.Size(314, 20);
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
            this.chkLineStyleSupport.Location = new System.Drawing.Point(212, 294);
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
            this.label3.Location = new System.Drawing.Point(26, 290);
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
            this.chkShortKeySupport.Location = new System.Drawing.Point(215, 112);
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
            this.label4.Location = new System.Drawing.Point(29, 108);
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
            this.label5.Location = new System.Drawing.Point(29, 137);
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
            this.label6.Location = new System.Drawing.Point(29, 155);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 18);
            this.label6.TabIndex = 232;
            this.label6.Text = "Show Query Buttons";
            this.toolTip1.SetToolTip(this.label6, "Add Buttons to start a Query with just one click");
            // 
            // chkShowServiceButtons
            // 
            this.chkShowServiceButtons.AutoSize = true;
            this.chkShowServiceButtons.Location = new System.Drawing.Point(215, 141);
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
            this.chkShowQueryButtons.Location = new System.Drawing.Point(215, 159);
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
            this.chkFavoriteSupport.Location = new System.Drawing.Point(212, 316);
            this.chkFavoriteSupport.Margin = new System.Windows.Forms.Padding(2);
            this.chkFavoriteSupport.Name = "chkFavoriteSupport";
            this.chkFavoriteSupport.Size = new System.Drawing.Size(15, 14);
            this.chkFavoriteSupport.TabIndex = 236;
            this.toolTip1.SetToolTip(this.chkFavoriteSupport, "Add buttons to handle Favorites:\r\n- Element\r\n- Package\r\n- Diagram\r\nadd, remove, s" +
        "how/find");
            this.chkFavoriteSupport.UseVisualStyleBackColor = true;
            this.chkFavoriteSupport.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 312);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 18);
            this.label7.TabIndex = 235;
            this.label7.Text = "Favorites Supports";
            this.toolTip1.SetToolTip(this.label7, "Add buttons to handle Favorites:\r\n- Element\r\n- Package\r\n- Diagram\r\nadd, remove, s" +
        "how/find");
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAutoLoadMdgNo);
            this.groupBox2.Controls.Add(this.rbAutoLoadMdgCompilation);
            this.groupBox2.Controls.Add(this.rbAutoLoadMdgBasic);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(265, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 100);
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
            this.rbAutoLoadMdgBasic.Location = new System.Drawing.Point(11, 24);
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
            this.chkConveyedItemSupport.Location = new System.Drawing.Point(285, 112);
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
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(243, 421);
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
            this.label8.Location = new System.Drawing.Point(235, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 18);
            this.label8.TabIndex = 240;
            this.label8.Text = "on Toolbar";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(235, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 18);
            this.label9.TabIndex = 241;
            this.label9.Text = "on Toolbar";
            // 
            // FrmSettingsGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(844, 529);
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
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
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
    }
}