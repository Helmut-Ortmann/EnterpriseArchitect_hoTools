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
            this.components = new System.ComponentModel.Container();
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
            this.lblRelease = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regularExpressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtUserText = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            this.chkPackage = new System.Windows.Forms.CheckBox();
            this.chkElement = new System.Windows.Forms.CheckBox();
            this.chkDiagram = new System.Windows.Forms.CheckBox();
            this.chkCaseSensetive = new System.Windows.Forms.CheckBox();
            this.chkOperation = new System.Windows.Forms.CheckBox();
            this.chkAttribute = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFindString
            // 
            this.txtFindString.Location = new System.Drawing.Point(66, 259);
            this.txtFindString.Name = "txtFindString";
            this.txtFindString.Size = new System.Drawing.Size(175, 22);
            this.txtFindString.TabIndex = 0;
            // 
            // txtReplaceString
            // 
            this.txtReplaceString.Location = new System.Drawing.Point(254, 259);
            this.txtReplaceString.Name = "txtReplaceString";
            this.txtReplaceString.Size = new System.Drawing.Size(160, 22);
            this.txtReplaceString.TabIndex = 1;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(66, 288);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(53, 23);
            this.btnFindNext.TabIndex = 2;
            this.btnFindNext.Text = "Next";
            this.toolTip1.SetToolTip(this.btnFindNext, "Find next item like:\r\n- Package\r\n- Element\r\n- Duiagram\r\n- Attribute\r\n- Operation");
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(207, 287);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 23);
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
            this.btnReplaceAll.Location = new System.Drawing.Point(288, 287);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(99, 23);
            this.btnReplaceAll.TabIndex = 4;
            this.btnReplaceAll.Text = "Replace all";
            this.toolTip1.SetToolTip(this.btnReplaceAll, "Replace all \"Find string\" by \"Replace string\" in all found items like:\r\n- Package" +
        "s\r\n- Elements\r\n- Diagrams\r\n- Attributes\r\n- Operations");
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(398, 287);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.toolTip1.SetToolTip(this.btnCancel, "Cancel the current Find operation to start a new one.");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkRegularExpression
            // 
            this.chkRegularExpression.AutoSize = true;
            this.chkRegularExpression.Location = new System.Drawing.Point(3, 207);
            this.chkRegularExpression.Name = "chkRegularExpression";
            this.chkRegularExpression.Size = new System.Drawing.Size(153, 21);
            this.chkRegularExpression.TabIndex = 6;
            this.chkRegularExpression.Text = "Regular Expression";
            this.toolTip1.SetToolTip(this.chkRegularExpression, "Search with Regular Expression. \r\n\r\nSee also Help, Regular Expression");
            this.chkRegularExpression.UseVisualStyleBackColor = true;
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Location = new System.Drawing.Point(5, 89);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(67, 21);
            this.chkName.TabIndex = 7;
            this.chkName.Text = "Name";
            this.chkName.UseVisualStyleBackColor = true;
            // 
            // chkDescription
            // 
            this.chkDescription.AutoSize = true;
            this.chkDescription.Location = new System.Drawing.Point(5, 116);
            this.chkDescription.Name = "chkDescription";
            this.chkDescription.Size = new System.Drawing.Size(101, 21);
            this.chkDescription.TabIndex = 8;
            this.chkDescription.Text = "Description";
            this.chkDescription.UseVisualStyleBackColor = true;
            // 
            // lblRelease
            // 
            this.lblRelease.AutoSize = true;
            this.lblRelease.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelease.Location = new System.Drawing.Point(414, 0);
            this.lblRelease.Name = "lblRelease";
            this.lblRelease.Size = new System.Drawing.Size(49, 18);
            this.lblRelease.TabIndex = 9;
            this.lblRelease.Text = "V1.0.0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(551, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(110, 24);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem2,
            this.aboutToolStripMenuItem,
            this.regularExpressionToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem2
            // 
            this.helpToolStripMenuItem2.Name = "helpToolStripMenuItem2";
            this.helpToolStripMenuItem2.Size = new System.Drawing.Size(203, 24);
            this.helpToolStripMenuItem2.Text = "&Help";
            this.helpToolStripMenuItem2.Click += new System.EventHandler(this.helpToolStripMenuItem2_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // regularExpressionToolStripMenuItem
            // 
            this.regularExpressionToolStripMenuItem.Name = "regularExpressionToolStripMenuItem";
            this.regularExpressionToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.regularExpressionToolStripMenuItem.Text = "Regular Expression";
            this.regularExpressionToolStripMenuItem.Click += new System.EventHandler(this.regularExpressionToolStripMenuItem_Click);
            // 
            // txtUserText
            // 
            this.txtUserText.Location = new System.Drawing.Point(5, 32);
            this.txtUserText.Name = "txtUserText";
            this.txtUserText.Size = new System.Drawing.Size(252, 24);
            this.txtUserText.TabIndex = 11;
            this.txtUserText.Text = "";
            this.toolTip1.SetToolTip(this.txtUserText, "Run EA Search \'Quick View\' with:\r\n- Input text + Enter\r\n- Double left Click with " +
        "insert Clipboard and start search\r\n\r\nSearch for:\r\n- Class / Component / Requirem" +
        "ent\r\n- GUID\r\n- Port");
            this.txtUserText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtUserText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUserText_KeyDown);
            this.txtUserText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtUserText_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(263, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 13;
            this.label1.Text = "Quick Search";
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Location = new System.Drawing.Point(125, 287);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(71, 23);
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
            this.txtStatus.Location = new System.Drawing.Point(66, 317);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(470, 22);
            this.txtStatus.TabIndex = 25;
            this.toolTip1.SetToolTip(this.txtStatus, resources.GetString("txtStatus.ToolTip"));
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(2, 315);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(58, 23);
            this.btnShow.TabIndex = 26;
            this.btnShow.Text = "Show";
            this.toolTip1.SetToolTip(this.btnShow, "Show details of found item and allow changes");
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // chkStereotype
            // 
            this.chkStereotype.AutoSize = true;
            this.chkStereotype.Location = new System.Drawing.Point(3, 143);
            this.chkStereotype.Name = "chkStereotype";
            this.chkStereotype.Size = new System.Drawing.Size(99, 21);
            this.chkStereotype.TabIndex = 27;
            this.chkStereotype.Text = "Stereotype";
            this.toolTip1.SetToolTip(this.chkStereotype, "Search for Stereotype");
            this.chkStereotype.UseVisualStyleBackColor = true;
            // 
            // chkTaggedValue
            // 
            this.chkTaggedValue.AutoSize = true;
            this.chkTaggedValue.Location = new System.Drawing.Point(3, 170);
            this.chkTaggedValue.Name = "chkTaggedValue";
            this.chkTaggedValue.Size = new System.Drawing.Size(119, 21);
            this.chkTaggedValue.TabIndex = 28;
            this.chkTaggedValue.Text = "Tagged Value";
            this.toolTip1.SetToolTip(this.chkTaggedValue, "Search in Tagged Value of selected Item Types.\r\n\r\nYou may enter possible tagged v" +
        "alue names in the right handed input field.\r\n\r\nThe Tagged Value names are sepert" +
        "ed by:\r\n- \',\'\r\n- \';\'\r\n- \':\' ");
            this.chkTaggedValue.UseVisualStyleBackColor = true;
            // 
            // txtTaggedValue
            // 
            this.txtTaggedValue.Location = new System.Drawing.Point(138, 168);
            this.txtTaggedValue.Name = "txtTaggedValue";
            this.txtTaggedValue.Size = new System.Drawing.Size(351, 22);
            this.txtTaggedValue.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txtTaggedValue, "Enter TaggedValue Names to search in Item Types\r\n- Blank = all TaggedValue Names\r" +
        "\n- Tagged Value Names splitted by \',\' or \';\'");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Find String";
            this.toolTip1.SetToolTip(this.label2, "Enter the string to search for as:\r\n- Text\r\n- Regular Expression");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(245, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "Replace by";
            this.toolTip1.SetToolTip(this.label3, "Enter string to replace the found string.");
            // 
            // chkIgnoreWhiteSpaces
            // 
            this.chkIgnoreWhiteSpaces.AutoSize = true;
            this.chkIgnoreWhiteSpaces.Location = new System.Drawing.Point(298, 207);
            this.chkIgnoreWhiteSpaces.Name = "chkIgnoreWhiteSpaces";
            this.chkIgnoreWhiteSpaces.Size = new System.Drawing.Size(161, 21);
            this.chkIgnoreWhiteSpaces.TabIndex = 22;
            this.chkIgnoreWhiteSpaces.Text = "Ignore White Spaces";
            this.toolTip1.SetToolTip(this.chkIgnoreWhiteSpaces, "According to regukar Expression option \'Ignore White Space\'.");
            this.chkIgnoreWhiteSpaces.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(0, 234);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(60, 23);
            this.btnFind.TabIndex = 23;
            this.btnFind.Text = "Find";
            this.toolTip1.SetToolTip(this.btnFind, resources.GetString("btnFind.ToolTip"));
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblFieldType
            // 
            this.lblFieldType.AutoSize = true;
            this.lblFieldType.Location = new System.Drawing.Point(5, 69);
            this.lblFieldType.Name = "lblFieldType";
            this.lblFieldType.Size = new System.Drawing.Size(74, 17);
            this.lblFieldType.TabIndex = 30;
            this.lblFieldType.Text = "Field Type";
            this.toolTip1.SetToolTip(this.lblFieldType, "Field type to search in:\r\n- Name\r\n- Description\r\n- Stereotype\r\n- Element Type\r\n- " +
        "Tagged Value");
            // 
            // lblItemType
            // 
            this.lblItemType.AutoSize = true;
            this.lblItemType.Location = new System.Drawing.Point(191, 69);
            this.lblItemType.Name = "lblItemType";
            this.lblItemType.Size = new System.Drawing.Size(70, 17);
            this.lblItemType.TabIndex = 31;
            this.lblItemType.Text = "Item Type";
            this.toolTip1.SetToolTip(this.lblItemType, "Search in item types like:\r\n- Package\r\n- Element\r\n- Diagram\r\n- Attribute\r\n- Opera" +
        "tion");
            // 
            // chkPackage
            // 
            this.chkPackage.AutoSize = true;
            this.chkPackage.Location = new System.Drawing.Point(194, 89);
            this.chkPackage.Name = "chkPackage";
            this.chkPackage.Size = new System.Drawing.Size(92, 21);
            this.chkPackage.TabIndex = 16;
            this.chkPackage.Text = "Packages";
            this.chkPackage.UseVisualStyleBackColor = true;
            // 
            // chkElement
            // 
            this.chkElement.AutoSize = true;
            this.chkElement.Location = new System.Drawing.Point(194, 116);
            this.chkElement.Name = "chkElement";
            this.chkElement.Size = new System.Drawing.Size(88, 21);
            this.chkElement.TabIndex = 17;
            this.chkElement.Text = "Elements";
            this.chkElement.UseVisualStyleBackColor = true;
            // 
            // chkDiagram
            // 
            this.chkDiagram.AutoSize = true;
            this.chkDiagram.Location = new System.Drawing.Point(194, 144);
            this.chkDiagram.Name = "chkDiagram";
            this.chkDiagram.Size = new System.Drawing.Size(90, 21);
            this.chkDiagram.TabIndex = 18;
            this.chkDiagram.Text = "Diagrams";
            this.chkDiagram.UseVisualStyleBackColor = true;
            // 
            // chkCaseSensetive
            // 
            this.chkCaseSensetive.AutoSize = true;
            this.chkCaseSensetive.Location = new System.Drawing.Point(162, 207);
            this.chkCaseSensetive.Name = "chkCaseSensetive";
            this.chkCaseSensetive.Size = new System.Drawing.Size(121, 21);
            this.chkCaseSensetive.TabIndex = 19;
            this.chkCaseSensetive.Text = "Case sensitive";
            this.chkCaseSensetive.UseVisualStyleBackColor = true;
            // 
            // chkOperation
            // 
            this.chkOperation.AutoSize = true;
            this.chkOperation.Location = new System.Drawing.Point(292, 116);
            this.chkOperation.Name = "chkOperation";
            this.chkOperation.Size = new System.Drawing.Size(93, 21);
            this.chkOperation.TabIndex = 21;
            this.chkOperation.Text = "Operation";
            this.chkOperation.UseVisualStyleBackColor = true;
            // 
            // chkAttribute
            // 
            this.chkAttribute.AutoSize = true;
            this.chkAttribute.Location = new System.Drawing.Point(292, 89);
            this.chkAttribute.Name = "chkAttribute";
            this.chkAttribute.Size = new System.Drawing.Size(83, 21);
            this.chkAttribute.TabIndex = 20;
            this.chkAttribute.Text = "Attribute";
            this.chkAttribute.UseVisualStyleBackColor = true;
            // 
            // FindAndReplaceGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Controls.Add(this.lblRelease);
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
            this.Name = "FindAndReplaceGUI";
            this.Size = new System.Drawing.Size(551, 359);
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
        private System.Windows.Forms.Label lblRelease;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem2;
        private System.Windows.Forms.RichTextBox txtUserText;
        private System.Windows.Forms.Label label1;
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
    }
}
