using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EA;
using hoTools.Utils.SQL;

namespace hoTools.EaServices.Dlg
{
    /// <summary>
    /// Enter the author with a dialog field. If security is enabled the must have rights to change the user.
    /// </summary>
    public partial class DlgAuthor : Form
    {
        readonly List<string> _users;
        string _user = "";
        Repository _rep;
        readonly bool _isSecurityEnabled;

        /// <summary>
        /// Dialog to ask and enter a user. Enter a user is only possible if the user has the rights.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="lToDelete"></param>
        public DlgAuthor(Repository rep, List<string> lToDelete )
        {
            _rep = rep;
            var sql = new UtilSql(rep);
            InitializeComponent();
            _listChanged.DataSource = lToDelete;
            if (rep.IsSecurityEnabled)
            {
                _isSecurityEnabled = true;

                // check if user has the rights to manage users
                if (sql.UserHasPermission(rep.GetCurrentLoginUser(true)))
                {
                    _users = sql.GetUsers();
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
                _users = sql.GetUsers();
                txtStatus.Text = "Security isn't enabled: Choose or enter your desired author name!";
            }
            
            
            cmbUser.Text = _user;
            cmbUser.DataSource = _users;
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
        /// <summary>
        /// Ok when changing the user. Ok is only allowed if the user has the rights to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            // get new user
            _user = cmbUser.Text;
            // if security is enabled the user must be in the list of available users
            if (_isSecurityEnabled & ! _users.Contains(cmbUser.Text )) {
                _user = ""; 
            }
            

        }
        /// <summary>
        /// Cancel when changing the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _user = "";
        }

    }
    
}
