using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using hoTools.Settings;
using EAAddinFramework.Utils;

namespace hoTools.Scripts
{
    public class SqlTabCntrls
    {
        AddinSettings _settings;
        Model _model;
        System.ComponentModel.IContainer _components;
        TabControl _tabControl;

        /// <summary>
        /// List of TabPages in TabControl
        /// </summary>
        //List<SqlTabCntrl> _tabCntrls = new List<SqlTabCntrl>();

        const string DEFAULT_TAB_NAME = "noName";

        public SqlTabCntrls(Model model, AddinSettings settings, System.ComponentModel.IContainer components, TabControl tabControl)
        {
            _settings = settings;
            _model = model;
            _tabControl = tabControl;
            _components = components;

        }
        /// <summary>
        /// Add a tab to the tab control
        /// </summary>
        /// <returns></returns>
        public TabPage addTab()
        {
            // create a new TabPage in TabControl
            TabPage tabPage = new TabPage();
            _tabControl.Controls.Add(tabPage);
            SqlTabCntrl sqlTabCntrl = new SqlTabCntrl(DEFAULT_TAB_NAME + _tabControl.Controls.Count.ToString() + ".sql", false);
            tabPage.Tag = sqlTabCntrl;
            tabPage.Text = sqlTabCntrl.DisplayName;
            _tabControl.SelectTab(tabPage);

            //-----------------------------------------------------------------
            // Create a text box in TabPage
            TextBox textBox = new TextBox();
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Both;
            textBox.AcceptsReturn = true;
            textBox.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            textBox.WordWrap = true;
            textBox.Modified = false;
            textBox.Dock = DockStyle.Fill;

            ContextMenuStrip tabSqlContextMenuStrip = new ContextMenuStrip(_components);
            ToolStripMenuItem fileLoadMenuItem = new ToolStripMenuItem();
            fileLoadMenuItem.Text = "Load file";
            fileLoadMenuItem.Click += new System.EventHandler(this.fileLoadMenuItem_Click);

            ToolStripMenuItem fileSaveMenuItem = new ToolStripMenuItem();
            fileSaveMenuItem.Text = "Save file";
            fileSaveMenuItem.Click += new System.EventHandler(this.fileSaveMenuItem_Click);
            ToolStripMenuItem fileSaveAsMenuItem = new ToolStripMenuItem();
            fileSaveAsMenuItem.Text = "Save as file";
            fileSaveAsMenuItem.Click += new System.EventHandler(this.fileSaveAsMenuItem_Click);


            tabSqlContextMenuStrip.Items.AddRange(new ToolStripItem[] {
            fileLoadMenuItem,
            fileSaveMenuItem,
            fileSaveAsMenuItem});

            textBox.ContextMenuStrip = tabSqlContextMenuStrip;
            tabPage.ContextMenuStrip = tabSqlContextMenuStrip;  // don't work
            _tabControl.ContextMenuStrip = tabSqlContextMenuStrip;  // works, have to decide which tab is selected



            tabPage.Controls.Add(textBox);
            return tabPage;
        }

        /// <summary>
        /// Event File Save As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSaveAsMenuItem_Click(object sender,  EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];
            saveTabPageAs(tabPage, textBox);

        }
        /// <summary>
        /// Event File Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSaveMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            saveTabPage(tabPage, textBox);
        }
        /// <summary>
        /// Event File Load fired by TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileLoadMenuItem_Click(object sender, EventArgs e)
        {
            // get TabPage
            TabPage tabPage = _tabControl.TabPages[_tabControl.SelectedIndex];

            // get TextBox
            TextBox textBox = (TextBox)tabPage.Controls[0];

            loadTabPageFrom(tabPage, textBox);
           
        }



        /// <summary>
        /// Load sql string from *.sql file into TabPage with TextBox inside.
        /// - Update and save the list of sql files 
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void loadTabPageFrom(TabPage tabPageSql, TextBox txtBoxSql)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"c:\temp\sql";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {
                StreamReader myStream = new StreamReader(openFileDialog.OpenFile());
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    txtBoxSql.Text = myStream.ReadToEnd();
                    myStream.Close();
                    tabPageSql.Text = Path.GetFileName(openFileDialog.FileName);

                    // store the complete filename in settings
                    _settings.sqlFiles.insert(openFileDialog.FileName);
                    _settings.save();

                    // Store TabData in TabPage
                    SqlTabCntrl sqlTabCntrl = new SqlTabCntrl(openFileDialog.FileName);
                    sqlTabCntrl.IsChanged = true;
                    tabPageSql.Tag = sqlTabCntrl;

                    // set TabName
                    tabPageSql.Text = sqlTabCntrl.DisplayName;
                }
            }

        }
        /// <summary>
        /// Save As sql TabPage in *.sql file.
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void saveTabPageAs(TabPage tabPageSql, TextBox txtBoxSql)
        {
            SqlTabCntrl sqlTabCntrl = (SqlTabCntrl)tabPageSql.Tag;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = sqlTabCntrl.DirectoryName;
            // get file name
            saveFileDialog.FileName = sqlTabCntrl.FullName;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            //saveFileDialog.DefaultExt = "sql";


            if (saveFileDialog.ShowDialog() == DialogResult.OK)

            {
                StreamWriter myStream = new StreamWriter(saveFileDialog.OpenFile());
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    myStream.Write(txtBoxSql.Text);
                    myStream.Close();
                    tabPageSql.Text = Path.GetFileName(saveFileDialog.FileName);

                    // store the complete filename in settings
                    _settings.sqlFiles.insert(saveFileDialog.FileName);
                    _settings.save();

                    // Store TabData in TabPage
                    sqlTabCntrl.FullName = saveFileDialog.FileName;
                    sqlTabCntrl.IsChanged = false;

                    // set TabName
                    tabPageSql.Text = sqlTabCntrl.DisplayName;
                }
            }
        }
        /// <summary>
        /// Save sql TabPage in *.sql file.
        /// </summary>
        /// <param name="tabPageSql"></param>
        /// <param name="txtBoxSql"></param>
        private void saveTabPage(TabPage tabPageSql, TextBox txtBoxSql)
        {

            SqlTabCntrl sqlTabCntrl = (SqlTabCntrl)tabPageSql.Tag;
            if (sqlTabCntrl.FullName.Substring(0,6) == "noName" )
            {
                saveTabPageAs(tabPageSql, txtBoxSql);
                return;
            }

            try {
                StreamWriter myStream = new StreamWriter(sqlTabCntrl.FullName);
                if (myStream != null)
                {
                    // Code to write the stream goes here.
                    myStream.Write(txtBoxSql.Text);
                    myStream.Close();
                    sqlTabCntrl.IsChanged = false;


                    // set TabName
                    tabPageSql.Text = sqlTabCntrl.DisplayName;
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", $"Error writing file {sqlTabCntrl.FullName}");
                return;
            }
        }

    }
}
