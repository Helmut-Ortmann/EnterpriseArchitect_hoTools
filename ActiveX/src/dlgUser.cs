using System;
using System.Windows.Forms;

namespace hoTools.EaServices.Dlg
{
    public partial class DlgUser : Form
    {
        string _user = "";
        public DlgUser()
        {
            InitializeComponent();
            cmbUser.Text = _user;

        }
        #region property user
        public string User
        {
            set { _user = value;  }
            get { return _user; }
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            _user = cmbUser.Text; 
        }

    }
}
