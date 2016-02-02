using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hoTools.EaServices.Dlg
{
    public partial class dlgUser : Form
    {
        string _user = "";
        public dlgUser()
        {
            InitializeComponent();
            cmbUser.Text = _user;

        }
        #region property user
        public string user
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
