namespace hoTools.Settings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(26, 188);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(107, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(75, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Settings Query and Script";
            // 
            // rbOnlyQueryAddinWindow
            // 
            this.rbOnlyQueryAddinWindow.AutoSize = true;
            this.rbOnlyQueryAddinWindow.Location = new System.Drawing.Point(11, 19);
            this.rbOnlyQueryAddinWindow.Name = "rbOnlyQueryAddinWindow";
            this.rbOnlyQueryAddinWindow.Size = new System.Drawing.Size(62, 22);
            this.rbOnlyQueryAddinWindow.TabIndex = 208;
            this.rbOnlyQueryAddinWindow.TabStop = true;
            this.rbOnlyQueryAddinWindow.Text = "Addin";
            this.toolTip1.SetToolTip(this.rbOnlyQueryAddinWindow, "Show SQL Query in Addin Window");
            this.rbOnlyQueryAddinWindow.UseVisualStyleBackColor = true;
            // 
            // rbOnlyQueryTabWindow
            // 
            this.rbOnlyQueryTabWindow.AutoSize = true;
            this.rbOnlyQueryTabWindow.Location = new System.Drawing.Point(70, 19);
            this.rbOnlyQueryTabWindow.Name = "rbOnlyQueryTabWindow";
            this.rbOnlyQueryTabWindow.Size = new System.Drawing.Size(51, 22);
            this.rbOnlyQueryTabWindow.TabIndex = 209;
            this.rbOnlyQueryTabWindow.TabStop = true;
            this.rbOnlyQueryTabWindow.Text = "Tab";
            this.toolTip1.SetToolTip(this.rbOnlyQueryTabWindow, "Show SQL Query in Tab Window");
            this.rbOnlyQueryTabWindow.UseVisualStyleBackColor = true;
            // 
            // rbOnlyQueryDisableWindow
            // 
            this.rbOnlyQueryDisableWindow.AutoSize = true;
            this.rbOnlyQueryDisableWindow.Location = new System.Drawing.Point(121, 19);
            this.rbOnlyQueryDisableWindow.Name = "rbOnlyQueryDisableWindow";
            this.rbOnlyQueryDisableWindow.Size = new System.Drawing.Size(75, 22);
            this.rbOnlyQueryDisableWindow.TabIndex = 210;
            this.rbOnlyQueryDisableWindow.TabStop = true;
            this.rbOnlyQueryDisableWindow.Text = "Disable";
            this.toolTip1.SetToolTip(this.rbOnlyQueryDisableWindow, "Disable SQL Query");
            this.rbOnlyQueryDisableWindow.UseVisualStyleBackColor = true;
            // 
            // rbScriptAndQueryAddinWindow
            // 
            this.rbScriptAndQueryAddinWindow.AutoSize = true;
            this.rbScriptAndQueryAddinWindow.Location = new System.Drawing.Point(11, 19);
            this.rbScriptAndQueryAddinWindow.Name = "rbScriptAndQueryAddinWindow";
            this.rbScriptAndQueryAddinWindow.Size = new System.Drawing.Size(62, 22);
            this.rbScriptAndQueryAddinWindow.TabIndex = 208;
            this.rbScriptAndQueryAddinWindow.TabStop = true;
            this.rbScriptAndQueryAddinWindow.Text = "Addin";
            this.toolTip1.SetToolTip(this.rbScriptAndQueryAddinWindow, "Show SQL Query and run Script in Addin Windows");
            this.rbScriptAndQueryAddinWindow.UseVisualStyleBackColor = true;
            // 
            // rbScriptAndQueryTabWindow
            // 
            this.rbScriptAndQueryTabWindow.AutoSize = true;
            this.rbScriptAndQueryTabWindow.Location = new System.Drawing.Point(70, 19);
            this.rbScriptAndQueryTabWindow.Name = "rbScriptAndQueryTabWindow";
            this.rbScriptAndQueryTabWindow.Size = new System.Drawing.Size(51, 22);
            this.rbScriptAndQueryTabWindow.TabIndex = 209;
            this.rbScriptAndQueryTabWindow.TabStop = true;
            this.rbScriptAndQueryTabWindow.Text = "Tab";
            this.toolTip1.SetToolTip(this.rbScriptAndQueryTabWindow, "Show SQL Query and run Script in Tab Windows");
            this.rbScriptAndQueryTabWindow.UseVisualStyleBackColor = true;
            // 
            // rbScriptAndQueryDisableWindow
            // 
            this.rbScriptAndQueryDisableWindow.AutoSize = true;
            this.rbScriptAndQueryDisableWindow.Location = new System.Drawing.Point(121, 19);
            this.rbScriptAndQueryDisableWindow.Name = "rbScriptAndQueryDisableWindow";
            this.rbScriptAndQueryDisableWindow.Size = new System.Drawing.Size(75, 22);
            this.rbScriptAndQueryDisableWindow.TabIndex = 210;
            this.rbScriptAndQueryDisableWindow.TabStop = true;
            this.rbScriptAndQueryDisableWindow.Text = "Disable";
            this.toolTip1.SetToolTip(this.rbScriptAndQueryDisableWindow, "Disable SQL Query and run Script");
            this.rbScriptAndQueryDisableWindow.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOnlyQueryDisableWindow);
            this.groupBox1.Controls.Add(this.rbOnlyQueryTabWindow);
            this.groupBox1.Controls.Add(this.rbOnlyQueryAddinWindow);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 43);
            this.groupBox1.TabIndex = 210;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit SQL Query and run it";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbScriptAndQueryDisableWindow);
            this.groupBox2.Controls.Add(this.rbScriptAndQueryTabWindow);
            this.groupBox2.Controls.Add(this.rbScriptAndQueryAddinWindow);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(26, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 43);
            this.groupBox2.TabIndex = 211;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Edit SQL Query and run Script with results";
            // 
            // FrmQueryAndScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(476, 225);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FrmQueryAndScript";
            this.Text = "hoTools: Settings Query and Script";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
    }
}