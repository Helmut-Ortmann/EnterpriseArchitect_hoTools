using System;
using System.Diagnostics;
using System.Windows.Forms;
using EA;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.MksUtil
{
    public class Mks
    {
        readonly string _vcPath;
        readonly EA.Package _pkg;
        readonly Repository _rep;

        // constructor
        public Mks(Repository rep, EA.Package pkg)  {
            _pkg = pkg;
            _rep = rep;
            _vcPath = "";
            if (pkg.IsControlled)
            {

                _vcPath = Util.GetVccFilePath(rep, pkg);
            }

            
        }
        
        public bool GetNewest()
        {
            // check nested packages
            foreach (EA.Package nestedPkg in _pkg.Packages)
            {
                var mks = new Mks(_rep, nestedPkg);
                mks.GetNewest();
            }
            if (_pkg.IsControlled)
            {
                // 
                _rep.ShowInProjectView(_pkg);
                try
                {
                    // preference head revision
                    var mks = new Mks(_rep, _pkg);
                    mks.Checkout();

                    // load package
                    _rep.CreateOutputTab("Debug");
                    _rep.EnsureOutputVisible("Debug");
                    _rep.WriteOutput("Debug", _pkg.Name + " " + _pkg.Notes, 0);

                    //MessageBox.Show(_pkg.Name + " " + _pkg.Packages.Count.ToString() + " " + _pkg.PackageGUID, "CountBefore");
                    Project prj = _rep.GetProjectInterface();
                    prj.LoadControlledPackage(_pkg.PackageGUID);


                    _rep.WriteOutput("Debug", _pkg.Name + " " + _pkg.Notes, 0);
                    //MessageBox.Show(_pkg.Name + " " + _pkg.Packages.Count.ToString() + " " + _pkg.PackageGUID, "CountAfter");


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), @"Error");
                }
            }
            


            return true;
        }

        public string ViewHistory()
        {
            if (_vcPath == null) return "";
            return Cmd(@"viewhistory");
        }

        public string Checkout()
        {
            if (_vcPath == null) return "";
            string txt = Cmd("co --batch --lock --forceConfirm=yes");
            //string txt = this.cmd("co --batch --nolock --unlock");
            return txt;
        }

        public string UndoCheckout()
        {
            if (_vcPath == null) return "";
            string txt = Cmd("unlock --action=remove --revision :head");
            return txt;
        }

        private string Cmd(string command)
        {
            string returnString = "";
            if (_vcPath == null) return returnString;
            var psi = new ProcessStartInfo(@"si")
            {
                Arguments = command + " \"" + _vcPath + "\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };
            // wrap file name in " to avoid problems with blank in name
            try
            {
                var p = Process.Start(psi);
                var output = p.StandardOutput;
                var standardError = p.StandardError;
                //outputError = p.StandardError;
                p.WaitForExit(10000);
                if (p.HasExited)
                {
                    if (p.ExitCode != 0)
                    {
                        MessageBox.Show($"ErrorCode:{p.ExitCode }\r\n{standardError.ReadToEnd()}",@"mks");
                       return "Error";
                    }
                    return output.ReadToEnd();

                }
                MessageBox.Show(@"Error: Timeout",@"mks");
                return "Error: Timeout";
            }
            catch (Exception e)
            {
                MessageBox.Show(e +
                    $"\r\n\r\nCommand:{psi}  {psi.Arguments}", @"Error mks");
            }


            return returnString;
        }
    }
}
