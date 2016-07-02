using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EA;
using hoTools.Utils.SQL;

namespace hoTools.EaServices.Dlg
{

    public partial class dlgUser : Form
    {
        List<string> users;
        string _user = "";
        Repository _rep;
        bool _isSecurityEnabled;
        UtilSql _sql;

        public dlgUser(Repository rep)
        {
            _rep = rep;
            _sql = new UtilSql(rep);
            InitializeComponent();
            if (rep.IsSecurityEnabled)
            {
                _isSecurityEnabled = true;

                // check if user has the rights to manage users
                if (_sql.UserHasPermission(rep.GetCurrentLoginUser(true)))
                {
                    users = _sql.GetUsers();
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
                users = _sql.GetUsers();
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
