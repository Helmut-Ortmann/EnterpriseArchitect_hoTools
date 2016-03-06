namespace hoTools.Find
{
    partial class FindAndReplaceGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAndReplaceGUI));
            this.txtFindString = new System.Windows.Forms.TextBox();
            this.txtReplaceString = new System.Windows.Forms.TextBox();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkRegularExpression = new System.Windows.Forms.CheckBox();
            this.chkName = new System.Windows.Forms.CheckBox();
            this.chkDescription = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regularExpressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.btnFindPrevious = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.chkStereotype = new System.Windows.Forms.CheckBox();
            this.chkTaggedValue = new System.Windows.Forms.CheckBox();
            this.txtTaggedValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkIgnoreWhiteSpaces = new System.Windows.Forms.CheckBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblFieldType = new System.Windows.Forms.Label();
            this.lblItemType = new System.Windows.Forms.Label();
            this.txtUserText = new System.Windows.Forms.RichTextBox();
            this.chkPackage = new System.Windows.Forms.CheckBox();
            this.chkElement = new System.Windows.Forms.CheckBox();
            this.chkDiagram = new System.Windows.Forms.CheckBox();
            this.chkCaseSensetive = new System.Windows.Forms.CheckBox();
            this.chkOperation = new System.Windows.Forms.CheckBox();
            this.chkAttribute = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Filter = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFindString
            // 
            this.txtFindString.Location = new System.Drawing.Point(0, 282);
            this.txtFindString.Margin = new System.Windows.Forms.Padding(2);
            this.txtFindString.Name = "txtFindString";
            this.txtFindString.Size = new System.Drawing.Size(215, 20);
            this.txtFindString.TabIndex = 0;
            // 
            // txtReplaceString
            // 
            this.txtReplaceString.Location = new System.Drawing.Point(224, 282);
            this.txtReplaceString.Margin = new System.Windows.Forms.Padding(2);
            this.txtReplaceString.Name = "txtReplaceString";
            this.txtReplaceString.Size = new System.Drawing.Size(200, 20);
            this.txtReplaceString.TabIndex = 1;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(18, 305);
            this.btnFindNext.Margin = new System.Windows.Forms.Padding(2);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(40, 19);
            this.btnFindNext.TabIndex = 2;
            this.btnFindNext.Text = "Next";
            this.toolTip1.SetToolTip(this.btnFindNext, "Find next item like:\r\n- Package\r\n- Element\r\n- Duiagram\r\n- Attribute\r\n- Operation");
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(175, 305);
            this.btnReplace.Margin = new System.Windows.Forms.Padding(2);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(56, 19);
            this.btnReplace.TabIndex = 3;
            this.btnReplace.Text = "Replace";
            this.toolTip1.SetToolTip(this.btnReplace, "Replace \"Find string\" by \"Replace string\" in selected item like:\r\n- Package\r\n- El" +
        "ement\r\n- Diagram\r\n- Attribute\r\n- Operation\r\n\r\nUse \"Show\" to decide which item in" +
        " element to change.");
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(261, 305);
            this.btnReplaceAll.Margin = new System.Windows.Forms.Padding(2);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(74, 19);
            this.btnReplaceAll.TabIndex = 4;
            this.btnReplaceAll.Text = "Replace all";
            this.toolTip1.SetToolTip(this.btnReplaceAll, "Replace all \"Find string\" by \"Replace string\" in all found items like:\r\n- Package" +
        "s\r\n- Elements\r\n- Diagrams\r\n- Attributes\r\n- Operations");
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(365, 306);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 19);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel the current Find operation to start a new one.");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkRegularExpression
            // 
            this.chkRegularExpression.AutoSize = true;
            this.chkRegularExpression.Location = new System.Drawing.Point(2, 220);
            this.chkRegularExpression.Margin = new System.Windows.Forms.Padding(2);
            this.chkRegularExpression.Name = "chkRegularExpression";
            this.chkRegularExpression.Size = new System.Drawing.Size(117, 17);
            this.chkRegularExpression.TabIndex = 6;
            this.chkRegularExpression.Text = "Regular Expression";
            this.toolTip1.SetToolTip(this.chkRegularExpression, "Search with Regular Expression. \r\n\r\nSee also Help, Regular Expression");
            this.chkRegularExpression.UseVisualStyleBackColor = true;
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Location = new System.Drawing.Point(2, 124);
            this.chkName.Margin = new System.Windows.Forms.Padding(2);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(54, 17);
            this.chkName.TabIndex = 7;
            this.chkName.Text = "Name";
            this.chkName.UseVisualStyleBackColor = true;
            // 
            // chkDescription
            // 
            this.chkDescription.AutoSize = true;
            this.chkDescription.Location = new System.Drawing.Point(2, 146);
            this.chkDescription.Margin = new System.Windows.Forms.Padding(2);
            this.chkDescription.Name = "chkDescription";
            this.chkDescription.Size = new System.Drawing.Size(79, 17);
            this.chkDescription.TabIndex = 8;
            this.chkDescription.Text = "Description";
            this.chkDescription.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(429, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(99, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem2,
            this.aboutToolStripMenuItem,
            this.regularExpressionToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem2
            // 
            this.helpToolStripMenuItem2.Name = "helpToolStripMenuItem2";
            this.helpToolStripMenuItem2.Size = new System.Drawing.Size(172, 22);
            this.helpToolStripMenuItem2.Text = "&Help";
            this.helpToolStripMenuItem2.Click += new System.EventHandler(this.helpToolStripMenuItem2_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // regularExpressionToolStripMenuItem
            // 
            this.regularExpressionToolStripMenuItem.Name = "regularExpressionToolStripMenuItem";
            this.regularExpressionToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.regularExpressionToolStripMenuItem.Text = "Regular Expression";
            this.regularExpressionToolStripMenuItem.Click += new System.EventHandler(this.regularExpressionToolStripMenuItem_Click);
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Location = new System.Drawing.Point(88, 305);
            this.btnFindPrevious.Margin = new System.Windows.Forms.Padding(2);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(57, 19);
            this.btnFindPrevious.TabIndex = 24;
            this.btnFindPrevious.Text = "Previous";
            this.toolTip1.SetToolTip(this.btnFindPrevious, "Find previous item like:\r\n- Package\r\n- Element\r\n- Duiagram\r\n- Attribute\r\n- Operat" +
        "ion");
            this.btnFindPrevious.UseVisualStyleBackColor = true;
            this.btnFindPrevious.Click += new System.EventHandler(this.btnFindPrevious_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Enabled = false;
            this.txtStatus.Location = new System.Drawing.Point(70, 346);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(2);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(354, 20);
            this.txtStatus.TabIndex = 25;
            this.toolTip1.SetToolTip(this.txtStatus, resources.GetString("txtStatus.ToolTip"));
            // 
            // btnShow
            // 
            this.btnShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShow.Location = new System.Drawing.Point(2, 338);
            this.btnShow.Margin = new System.Windows.Forms.Padding(2);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(58, 27);
            this.btnShow.TabIndex = 26;
            this.btnShow.Text = "Show";
            this.toolTip1.SetToolTip(this.btnShow, "Show details of found item and allow changes");
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // chkStereotype
            // 
            this.chkStereotype.AutoSize = true;
            this.chkStereotype.Location = new System.Drawing.Point(2, 168);
            this.chkStereotype.Margin = new System.Windows.Forms.Padding(2);
            this.chkStereotype.Name = "chkStereotype";
            this.chkStereotype.Size = new System.Drawing.Size(77, 17);
            this.chkStereotype.TabIndex = 27;
            this.chkStereotype.Text = "Stereotype";
            this.toolTip1.SetToolTip(this.chkStereotype, "Search for Stereotype");
            this.chkStereotype.UseVisualStyleBackColor = true;
            // 
            // chkTaggedValue
            // 
            this.chkTaggedValue.AutoSize = true;
            this.chkTaggedValue.Location = new System.Drawing.Point(2, 190);
            this.chkTaggedValue.Margin = new System.Windows.Forms.Padding(2);
            this.chkTaggedValue.Name = "chkTaggedValue";
            this.chkTaggedValue.Size = new System.Drawing.Size(98, 17);
            this.chkTaggedValue.TabIndex = 28;
            this.chkTaggedValue.Text = "Tagged Values";
            this.toolTip1.SetToolTip(this.chkTaggedValue, "Search in Tagged Value of selected Item Types.\r\n\r\nYou may enter possible tagged v" +
        "alue names in the right handed input field.\r\n\r\nThe Tagged Value names are sepert" +
        "ed by:\r\n- \',\'\r\n- \';\'\r\n- \':\' ");
            this.chkTaggedValue.UseVisualStyleBackColor = true;
            // 
            // txtTaggedValue
            // 
            this.txtTaggedValue.Location = new System.Drawing.Point(146, 188);
            this.txtTaggedValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtTaggedValue.Name = "txtTaggedValue";
            this.txtTaggedValue.Size = new System.Drawing.Size(278, 20);
            this.txtTaggedValue.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txtTaggedValue, "Enter TaggedValue Names to search in Item Types\r\n- Blank = all TaggedValue Names\r" +
        "\n- Tagged Value Names splitted by \',\' or \';\'");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 262);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Find String";
            this.toolTip1.SetToolTip(this.label2, "Enter the string to search for as:\r\n- Text\r\n- Regular Expression");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 262);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Replace by";
            this.toolTip1.SetToolTip(this.label3, "Enter string to replace the found string.");
            // 
            // chkIgnoreWhiteSpaces
            // 
            this.chkIgnoreWhiteSpaces.AutoSize = true;
            this.chkIgnoreWhiteSpaces.Location = new System.Drawing.Point(249, 220);
            this.chkIgnoreWhiteSpaces.Margin = new System.Windows.Forms.Padding(2);
            this.chkIgnoreWhiteSpaces.Name = "chkIgnoreWhiteSpaces";
            this.chkIgnoreWhiteSpaces.Size = new System.Drawing.Size(126, 17);
            this.chkIgnoreWhiteSpaces.TabIndex = 22;
            this.chkIgnoreWhiteSpaces.Text = "Ignore White Spaces";
            this.toolTip1.SetToolTip(this.chkIgnoreWhiteSpaces, "According to regukar Expression option \'Ignore White Space\'.");
            this.chkIgnoreWhiteSpaces.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(0, 254);
            this.btnFind.Margin = new System.Windows.Forms.Padding(2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(60, 27);
            this.btnFind.TabIndex = 23;
            this.btnFind.Text = "Find";
            this.toolTip1.SetToolTip(this.btnFind, resources.GetString("btnFind.ToolTip"));
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblFieldType
            // 
            this.lblFieldType.AutoSize = true;
            this.lblFieldType.Location = new System.Drawing.Point(4, 108);
            this.lblFieldType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFieldType.Name = "lblFieldType";
            this.lblFieldType.Size = new System.Drawing.Size(56, 13);
            this.lblFieldType.TabIndex = 30;
            this.lblFieldType.Text = "Field Type";
            this.toolTip1.SetToolTip(this.lblFieldType, "Field type to search in:\r\n- Name\r\n- Description\r\n- Stereotype\r\n- Element Type\r\n- " +
        "Tagged Value");
            // 
            // lblItemType
            // 
            this.lblItemType.AutoSize = true;
            this.lblItemType.Location = new System.Drawing.Point(143, 108);
            this.lblItemType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblItemType.Name = "lblItemType";
            this.lblItemType.Size = new System.Drawing.Size(54, 13);
            this.lblItemType.TabIndex = 31;
            this.lblItemType.Text = "Item Type";
            this.toolTip1.SetToolTip(this.lblItemType, "Search in item types like:\r\n- Package\r\n- Element\r\n- Diagram\r\n- Attribute\r\n- Opera" +
        "tion");
            // 
            // txtUserText
            // 
            this.txtUserText.Location = new System.Drawing.Point(347, 41);
            this.txtUserText.Margin = new System.Windows.Forms.Padding(2);
            this.txtUserText.Name = "txtUserText";
            this.txtUserText.Size = new System.Drawing.Size(67, 20);
            this.txtUserText.TabIndex = 11;
            this.txtUserText.Text = "";
            this.toolTip1.SetToolTip(this.txtUserText, "Run EA Search \'Quick View\' with:\r\n- Input text + Enter\r\n- Double left Click with " +
        "insert Clipboard and start search\r\n\r\nSearch for:\r\n- Class / Component / Requirem" +
        "ent\r\n- GUID\r\n- Port");
            this.txtUserText.Visible = false;
            this.txtUserText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtUserText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtUserText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtUserText_MouseDoubleClick);
            // 
            // chkPackage
            // 
            this.chkPackage.AutoSize = true;
            this.chkPackage.Location = new System.Drawing.Point(146, 124);
            this.chkPackage.Margin = new System.Windows.Forms.Padding(2);
            this.chkPackage.Name = "chkPackage";
            this.chkPackage.Size = new System.Drawing.Size(74, 17);
            this.chkPackage.TabIndex = 16;
            this.chkPackage.Text = "Packages";
            this.chkPackage.UseVisualStyleBackColor = true;
            // 
            // chkElement
            // 
            this.chkElement.AutoSize = true;
            this.chkElement.Location = new System.Drawing.Point(146, 146);
            this.chkElement.Margin = new System.Windows.Forms.Padding(2);
            this.chkElement.Name = "chkElement";
            this.chkElement.Size = new System.Drawing.Size(69, 17);
            this.chkElement.TabIndex = 17;
            this.chkElement.Text = "Elements";
            this.chkElement.UseVisualStyleBackColor = true;
            // 
            // chkDiagram
            // 
            this.chkDiagram.AutoSize = true;
            this.chkDiagram.Location = new System.Drawing.Point(146, 169);
            this.chkDiagram.Margin = new System.Windows.Forms.Padding(2);
            this.chkDiagram.Name = "chkDiagram";
            this.chkDiagram.Size = new System.Drawing.Size(70, 17);
            this.chkDiagram.TabIndex = 18;
            this.chkDiagram.Text = "Diagrams";
            this.chkDiagram.UseVisualStyleBackColor = true;
            // 
            // chkCaseSensetive
            // 
            this.chkCaseSensetive.AutoSize = true;
            this.chkCaseSensetive.Location = new System.Drawing.Point(147, 220);
            this.chkCaseSensetive.Margin = new System.Windows.Forms.Padding(2);
            this.chkCaseSensetive.Name = "chkCaseSensetive";
            this.chkCaseSensetive.Size = new System.Drawing.Size(94, 17);
            this.chkCaseSensetive.TabIndex = 19;
            this.chkCaseSensetive.Text = "Case sensitive";
            this.chkCaseSensetive.UseVisualStyleBackColor = true;
            // 
            // chkOperation
            // 
            this.chkOperation.AutoSize = true;
            this.chkOperation.Location = new System.Drawing.Point(219, 146);
            this.chkOperation.Margin = new System.Windows.Forms.Padding(2);
            this.chkOperation.Name = "chkOperation";
            this.chkOperation.Size = new System.Drawing.Size(72, 17);
            this.chkOperation.TabIndex = 21;
            this.chkOperation.Text = "Operation";
            this.chkOperation.UseVisualStyleBackColor = true;
            // 
            // chkAttribute
            // 
            this.chkAttribute.AutoSize = true;
            this.chkAttribute.Location = new System.Drawing.Point(219, 124);
            this.chkAttribute.Margin = new System.Windows.Forms.Padding(2);
            this.chkAttribute.Name = "chkAttribute";
            this.chkAttribute.Size = new System.Drawing.Size(65, 17);
            this.chkAttribute.TabIndex = 20;
            this.chkAttribute.Text = "Attribute";
            this.chkAttribute.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(30, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(305, 37);
            this.label4.TabIndex = 32;
            this.label4.Text = "Search and Replace";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(344, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Quick Search";
            this.label1.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(319, 24);
            this.label5.TabIndex = 33;
            this.label5.Text = "Search selected packages recursive ";
            // 
            // Filter
            // 
            this.Filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Filter.Location = new System.Drawing.Point(0, 85);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(429, 164);
            this.Filter.TabIndex = 34;
            this.Filter.TabStop = false;
            this.Filter.Text = "Filter";
            // 
            // FindAndReplaceGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblItemType);
            this.Controls.Add(this.lblFieldType);
            this.Controls.Add(this.txtTaggedValue);
            this.Controls.Add(this.chkTaggedValue);
            this.Controls.Add(this.chkStereotype);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnFindPrevious);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.chkIgnoreWhiteSpaces);
            this.Controls.Add(this.chkOperation);
            this.Controls.Add(this.chkAttribute);
            this.Controls.Add(this.chkCaseSensetive);
            this.Controls.Add(this.chkDiagram);
            this.Controls.Add(this.chkElement);
            this.Controls.Add(this.chkPackage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUserText);
            this.Controls.Add(this.chkDescription);
            this.Controls.Add(this.chkName);
            this.Controls.Add(this.chkRegularExpression);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnFindNext);
            this.Controls.Add(this.txtReplaceString);
            this.Controls.Add(this.txtFindString);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Filter);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FindAndReplaceGUI";
            this.Size = new System.Drawing.Size(429, 379);
            this.toolTip1.SetToolTip(this, "Show details and allow changes");
            this.Load += new System.EventHandler(this.FindAndReplaceGUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFindString;
        private System.Windows.Forms.TextBox txtReplaceString;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkRegularExpression;
        private System.Windows.Forms.CheckBox chkName;
        private System.Windows.Forms.CheckBox chkDescription;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkPackage;
        private System.Windows.Forms.CheckBox chkElement;
        private System.Windows.Forms.CheckBox chkDiagram;
        private System.Windows.Forms.CheckBox chkCaseSensetive;
        private System.Windows.Forms.CheckBox chkOperation;
        private System.Windows.Forms.CheckBox chkAttribute;
        private System.Windows.Forms.CheckBox chkIgnoreWhiteSpaces;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnFindPrevious;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regularExpressionToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkStereotype;
        private System.Windows.Forms.CheckBox chkTaggedValue;
        private System.Windows.Forms.TextBox txtTaggedValue;
        private System.Windows.Forms.Label lblFieldType;
        private System.Windows.Forms.Label lblItemType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtUserText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox Filter;
    }
}
