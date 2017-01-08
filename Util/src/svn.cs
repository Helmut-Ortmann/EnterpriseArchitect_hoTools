using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EA;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.svnUtil
{
    public class Svn
    {
        readonly string _vcPath;
        EA.Package _pkg;
        Repository _rep;

        // constructor
        public Svn(Repository rep, EA.Package pkg)  {
            _pkg = pkg;
            _rep = rep;
            _vcPath = "";
            if (pkg.IsControlled)
            {

                _vcPath = Util.GetVccFilePath(rep, pkg);
            }

            
        }

        public bool SetProperty()
        {
            Cmd(@"svn","propset svn:keywords \"Date Author Rev Id Header\" ");
            return true;
        }
        public void GotoLog()
        {
            string repUrl = GetRepositoryPath();
            Cmd("tortoiseProc", "/command:log /findtype:2 /path:",repUrl);
        }
        public string GetLockingUser()
        {
            string repUrl = GetRepositoryPath();
            string s = Cmd("svn", "info " + repUrl);
            Match match = Regex.Match(s, @"Lock Owner:[\s]*([A-Za-z]*)");
            return match.Groups[1].Value;
        }
        public void GotoRepoBrowser()
        {
            string repUrl = GetRepositoryPath();
            Cmd("tortoiseProc", "/command:repobrowser /path:", repUrl);
        }
        private string GetRepositoryPath()
        {
            string url = "";
            string s = Cmd("svn", "info");
            string[] lines = Regex.Split(s, "\r\n");
            url = lines[3].Substring(5);
            
            return url;
        }

        // proc: svn, tortoiseProc
        public string Cmd(string procInfo, string cmd, string url="")
        {
            string returnString = "";
            
            var psi = new ProcessStartInfo(procInfo);
            string path = _vcPath;
            if (!url.Equals("")) path = url;
            if (path == null) return returnString;
            string space = "";
            if (procInfo.ToUpper().Equals("SVN")) space = " "; 

            psi.Arguments = cmd + space + "\"" + path + "\"";  // wrap file name in " to avoid problems with blank in name
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process p;
            try
            {
                p = Process.Start(psi);
                var output = p.StandardOutput;
                var standardError = p.StandardError;
                //outputError = p.StandardError;
                p.WaitForExit(20000);
                if (p.HasExited)
                {
                    if (p.ExitCode != 0)
                    {
                        MessageBox.Show($"ErrorCode:{p.ExitCode}\r\n{standardError.ReadToEnd()}",@"svn");
                       return "Error";
                    }
                    return output.ReadToEnd();

                }
                MessageBox.Show(@"Error: Timeout",@"svn");
                return "Error: Timeout";
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}\r\n\r\nCommand:{psi} {psi.Arguments}", @"Error svn");
            }


            return returnString;
        }
    }
}
