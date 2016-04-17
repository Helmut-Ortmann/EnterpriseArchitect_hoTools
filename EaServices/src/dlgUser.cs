using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hoTools.Utils;
using hoTools.Utils.SQL;

namespace hoTools.EaServices.Dlg
{

    public partial class dlgUser : Form
    {
        List<string> users = null;
        private string _user = "";
        private EA.Repository _rep = null;
        private bool _isSecurityEnabled = false;
        private UtilSql _sql = null;

        public dlgUser(EA.Repository rep)
        {
            _rep = rep;
            _sql = new UtilSql(rep);
            InitializeComponent();
            if (rep.IsSecurityEnabled)
            {
                _isSecurityEnabled = true;

                // check if user has the rights to manage users
                if (_sql.userHasPermission(rep.GetCurrentLoginUser(true)))
                {
                    users = _sql.getUsers();
                    txtStatus.Text = "Security is enabled: Choose user";
                }
                else
                {
                    txtStatus.Text = "Security is enabled: Only person with 'Manage User' are allowed to change users!";

                    MessageBox.Show("User has no 'Manage Users' right", "Insufficient user rights");
                    btnOk.Enabled = false;
                }

            }
            else
            {
                users = _sql.getUsers();
                txtStatus.Text = "Security isn't enabled: Choose or enter your desired author name!";
            }
            
            
            cmbUser.Text = _user;
            cmbUser.DataSource = users;
        }
        #region property User
        public string User {
               set  
               { 
                   _user = value;
                    cmbUser.Text = value;
               }
               get { return _user; }
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {
            _user = cmbUser.Text;
            if (_isSecurityEnabled & ! users.Contains(cmbUser.Text )) {
                _user = ""; 
            }
            

        }

    }
    
}
