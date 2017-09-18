using System;
using System.IO;
using System.Windows.Forms;
using ADODB;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.ODBC
{
    /// <summary>
    /// ODBC connection. Note: Wildcard is '%', also for Access
    /// </summary>
    public class Odbc
    {
        EA.Repository _rep;
        ADODB.Connection _cn;
        ADODB.Command _cmd;
        ADODB.Recordset _rs;

        public Connection Cn { get => _cn; set => _cn = value; }
        public Command Cmd { get => _cmd; set => _cmd = value; }
        public Recordset Rs { get => _rs; set => _rs = value; }

        /// <summary>
        /// ODBC connection. Note: Wildcard is '%', also for Access
        /// </summary>
        /// <param name="rep"></param>
        public Odbc(EA.Repository rep)
        {
            _rep = rep;
            _rs = new ADODB.Recordset();
            _cmd = new ADODB.Command();
            _cmd.CommandTimeout = 180;
            int start;

            string connectionString = _rep.ConnectionString;
            if (connectionString.Contains("Connect="))
            {
                start = connectionString.IndexOf("Connect=", StringComparison.Ordinal) + 8;
                connectionString = connectionString.Substring(start);
            }
            else
            {
                connectionString = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + connectionString;
            }

            try
            {
                _cn = new ADODB.Connection();
                _cn.CommandTimeout = 60;
                _cn.ConnectionTimeout = 60;
                _cn.Open(connectionString, "", "", 0);

            }
            catch (Exception ex)
            {
                // check if shortcut was chosen
                // The first line contains the connection string
                connectionString = connectionString.ToUpper();
                if (connectionString.Contains(".EAP"))
                {
                    start = connectionString.IndexOf("DATA SOURCE=") + 12;
                    connectionString = connectionString.Substring(start);
                    TextReader tr = new StreamReader(connectionString);


                    string shortcut = tr.ReadLine();
                    tr.Close();

                    if (shortcut.Contains("Connect="))
                    {
                        start = shortcut.IndexOf("Connect=", StringComparison.Ordinal) + 8;
                        connectionString = shortcut.Substring(start);
                        try
                        {
                            _cn = new ADODB.Connection();
                            _cn.Open(connectionString, "", "", 0);
                            _cmd.ActiveConnection = _cn;
                            _rs.ActiveConnection = _cn;

                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show("Error in ADODB connect: '" + connectionString + "'\n" + "Don't start EA with a shortcut like SDTL.eap!\n\n" + ex1.Message);
                        }
                    }
                }

                MessageBox.Show("Error in ADODB connect: '" + connectionString + "'\n" + "Don't start EA with a shortcut like SDTL.eap!\n\n" + ex.Message);

            }
           _cmd.ActiveConnection = _cn;
            _rs.ActiveConnection = _cn;

        }

        public bool OdbcCmd(string sql)
        {
            if (_cmd == null)
            {
                _cmd = new ADODB.Command();
                _cmd.ActiveConnection = _cn;
            }
            ADODB.Recordset rs1 = new ADODB.Recordset();
            rs1.ActiveConnection = _cn;
            _cmd.CommandText = sql;
            _cmd.CommandType = ADODB.CommandTypeEnum.adCmdText;
            object dummy = Type.Missing;
            try
            {
                rs1 = _cmd.Execute(out dummy, ref dummy, 0);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error in ADODB Execute" + _rep.ConnectionString + "\n'" + _cmd.CommandText + "'\n'" + ex.Message + "'");
                return false;
            }

            return true;
        }

        public void Close()
        {
            if (_cn != null)
            {
                try { _cn.Close(); }
                catch (Exception e) { }
            }
            _cn = null;
            _rs = null;
            _cmd = null;
        }
    }
}
 