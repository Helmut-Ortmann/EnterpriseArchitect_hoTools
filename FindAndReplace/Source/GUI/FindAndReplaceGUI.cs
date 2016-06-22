using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using hoTools.Settings;
using hoTools.EaServices;
using hoTools.ActiveX;


namespace hoTools.Find
{
    /// <summary>
    /// ActiveX COM Component 'hoTools.FindAndReplaceGUI' to show as tab in the EA Addin window
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2703ED16-2C93-4B13-83DC-F72FB6EDBA9E")]
    [ProgId(PROGID)]
    [ComDefaultInterface(typeof(IFindAndReplaceGUI))]

    public partial class FindAndReplaceGUI : AddinGUI, IFindAndReplaceGUI
    {
        public const string PROGID = "hoTools.FindAndReplaceGUI";
        public const string TABULATOR = "Find";

        FindAndReplace _fr ;


        #region Constructor
        public FindAndReplaceGUI()
        {
            InitializeComponent();
            chkDescription.Checked = true;
            chkName.Checked = true;
            chkPackage.Checked = true;
            chkElement.Checked = true;
            chkAttribute.Checked = false;
            chkOperation.Checked = false;
            chkDiagram.Checked = true;
            txtStatus.Text = "";

            activateFindChangeParameters(true);


        }
        #endregion
        #region Properties

        public string getName() => "hoTools.FindAndReplace";
       
        #endregion
        
        #region btnFind_Click
        /// <summary>
        /// btnFind_Click
        /// Find all matching items in recursive elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtFindString.Text.Length < 3)
            {
                DialogResult result = MessageBox.Show("Yes=Continue",
                                            "Short find string (<3 characters)", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel) return;
            }
            EA.Package pkg = Repository.GetTreeSelectedPackage();
            _fr = new FindAndReplace(Repository, pkg,
                txtFindString.Text,
                txtReplaceString.Text,
                chkCaseSensetive.Checked, chkRegularExpression.Checked, chkIgnoreWhiteSpaces.Checked,
                chkName.Checked, chkDescription.Checked, chkStereotype.Checked, chkTaggedValue.Checked,
                txtTaggedValue.Text,
                chkPackage.Checked, chkElement.Checked, chkDiagram.Checked,
                chkAttribute.Checked, chkOperation.Checked);

            _fr.FindInPackageRecursive();
            _fr.LocateCurrentElement();
            if (_fr.Index >= 0)
            {
                txtStatus.Text = _fr.ItemShortDescription();
                activateFindChangeParameters(false);
            }

        }
        #endregion
        #region btnFindNext_Click
        private void btnFindNext_Click(object sender, EventArgs e)
        {
            if (_fr == null)
            {
                MessageBox.Show("Start search with 'Find'");
                return;
            }
            _fr.FindNext();
            _fr.LocateCurrentElement();
            txtStatus.Text = _fr.ItemShortDescription();
        }
#endregion
        #region btnFindPrevious_Click
        private void btnFindPrevious_Click(object sender, EventArgs e)
        {
            if (_fr == null)
            {
                MessageBox.Show("Start search with 'Find'");
                return;
            }
            
            _fr.FindPrevious();
            _fr.LocateCurrentElement();
            txtStatus.Text = _fr.ItemShortDescription();

        }
        #endregion
        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _fr = null;
            txtStatus.Text = "";
            activateFindChangeParameters(true);
        }
        #endregion
        #region btnReplace
        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (_fr == null || _fr.l_items.Count == 0 )
            {
                MessageBox.Show("Start search with 'Find'");
                return;
            }
            if (_fr.l_items[_fr.Index].isUpdated)
            {
                MessageBox.Show("Changes already done!");
                return;
            }
            
            // update replace string
            _fr.replaceString = txtReplaceString.Text;
            if (txtReplaceString.Text.Trim() == "")
            {
                DialogResult result = MessageBox.Show("", "Replace with string empty?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No) return;
            }
            _fr.ReplaceItem();
            _fr.FindNext();
            _fr.LocateCurrentElement();
            txtStatus.Text = _fr.ItemShortDescription();

        }
        #endregion
        #region activateFindChangeParameters
        private void activateFindChangeParameters (bool activate){
            btnFind.Enabled = activate;
            chkAttribute.Enabled = activate;
            chkCaseSensetive.Enabled = activate;
            chkDescription.Enabled = activate;
            chkDiagram.Enabled = activate;
            chkElement.Enabled = activate;
            txtFindString.Enabled = activate;
            chkIgnoreWhiteSpaces.Enabled = activate;
            chkName.Enabled = activate;
            chkOperation.Enabled = activate;
            chkPackage.Enabled = activate;
            chkRegularExpression.Enabled = activate;
            
            bool xorActivate = activate ^ true;
            btnShow.Enabled = activate ^ true;
            btnFindNext.Enabled = xorActivate;
            btnFindPrevious.Enabled = xorActivate;
            btnReplace.Enabled = xorActivate;
            btnReplaceAll.Enabled = xorActivate;
        }
        #endregion
        #region btnReplaceAll_Click
        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (txtReplaceString.Text.Trim() == "")
            {
                DialogResult result = MessageBox.Show("", "Replace with string empty?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No) return;
            }
            _fr.ReplaceAll();
            txtStatus.Text = _fr.ItemShortDescription();
            MessageBox.Show(String.Format("{0} items updated",_fr.l_items.Count),"All findings replaced");
        }
#endregion
        #region btnShow_Click
        private void btnShow_Click(object sender, EventArgs e)
        {
            var frmItem = new ShowAndChangeItemGUI(_fr);
            frmItem.Show();
            _fr.LocateCurrentElement();
            txtStatus.Text = _fr.ItemShortDescription();
        }
#endregion
        #region aboutToolStripMenuItem_Click
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string configFilePath = AddinSettings.ConfigFilePath;
            switch (AddinSettings.Customer)
            {
                case AddinSettings.CustomerCfg.Var1:
                    EaService.aboutVAR1(Release, configFilePath);
                    break;
                case AddinSettings.CustomerCfg.HoTools:
                    EaService.about(Release, configFilePath);
                    break;
                default:
                    EaService.about(Release, configFilePath);
                    break;
            }
        }
#endregion
        #region helpToolStripMenuItem2_Click
        private void helpToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, EaService.getAssemblyPath() + "\\" + "hoTools.chm");
        }
        #endregion
        #region OverwriteIsInputKey of quick search
        //----------------------------------------------------------------------------
        // Overwrite IsInputKey
        //----------------------------------------------------------------------------
        // The KeyDown event only triggered at the standard TextBox or MaskedTextBox by "normal" input keys, 
        // not ENTER or TAB and so on.
        public class EnterTextBox : TextBox
        {
            protected override bool IsInputKey(Keys keyData)
            {
                if (keyData == Keys.Return)
                    return true;
                return base.IsInputKey(keyData);
            }

        }
        #endregion
        #region txtUserText_KeyDown
        // text field
        // There are special keys like "Enter" which require an enabling by 
        //---------------------------------------------------------
        // see at:  protected override bool IsInputKey(Keys keyData)
        void txtUserText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EaService.runQuickSearch(Repository, AddinSettings.QuickSearchName, txtUserText.Text);
                e.Handled = true;
            }
        }
        #endregion
        #region txtUserText_MouseDoubleClick
        private void txtUserText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtUserText.Text = Clipboard.GetText();
            EaService.runQuickSearch(Repository, AddinSettings.QuickSearchName, txtUserText.Text);
        }
#endregion
        #region regularExpressionToolStripMenuItem_Click
        private void regularExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmItem = new RegularExpression();
            frmItem.Show();
        }
#endregion       

        private void FindAndReplaceGUI_Load(object sender, EventArgs e)
        {

        }
    }
}
