using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils;
using hoTools.Utils.src;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.Abouts
{
    public static class About
    {
        /// <summary>
        /// OutputAboutMessage.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="description"></param>
        /// <param name="caption"></param>
        /// <param name="lDllNames"></param>
        /// <param name="pathSettings"></param>
        public static void AboutMessage(EA.Repository rep, [NotNull] string description, [NotNull] string caption, string[] lDllNames, [NotNull] string pathSettings="No")
        {
        

            description = $@"{description}

Helmut.Ortmann@hoModeler.de
Helmut.Ortmann@t-online.de
(+49) 172 / 51 79 16 7

";


            // get product version
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var conString = new EaConnectionString(rep);

            string pathRoot = Assembly.GetExecutingAssembly().Location;
            string pathDll = Path.Combine(new[] { Path.GetDirectoryName(pathRoot), "hoToolsRoot.dll" });
            string versionRoot = FileVersionInfo.GetVersionInfo(pathDll).FileVersion;

            var runTimeEnvironment = Environment.Is64BitProcess == true ? "x64" : "x86";
            description =
                    $"{description}{"Product-Version (hoToolsRoot.dll):",-32}\tV{versionRoot}{Environment.NewLine}" +
                    $"{"EA Library Version:",-32}\t\t{ rep.LibraryVersion}{Environment.NewLine}" +
                    $"{"EA ConnectionString:", -32}\t\t{ rep.ConnectionString}{Environment.NewLine}"+
                    $"{"DB ConnectionString:",-32}\t\t'{conString.DbConnectionString ?? " "}'{Environment.NewLine}" +
                    $"{"RepositoryType:",-32}\t\t'{rep?.RepositoryType() ?? " "}'{Environment.NewLine}" +
                    $"{"Runtime:",-32}\t\t{runTimeEnvironment}{Environment.NewLine}{Environment.NewLine}";



            // Get file-version of every dll
            pathRoot = Assembly.GetExecutingAssembly().Location;
            pathRoot = Path.GetDirectoryName(pathRoot);
            foreach (string dllName in lDllNames)
            {

                pathDll = Path.Combine(new[] { pathRoot, dllName });
                try
                {
                    
                    string version = FileVersionInfo.GetVersionInfo(pathDll).FileVersion;
                    // proportional font, no easy formatting
                    if (dllName.Length > 23 )
                        description =
                        $"{description,-30}- {dllName,-50}: V{version}\t{PeArchitecture.GetPeArchitecture(pathDll),-6}{Environment.NewLine}";
                    else
                    // proportional font, no easy formatting
                    description =
                        $"{description,-30}- {dllName,-50}\t: V{version}\t {PeArchitecture.GetPeArchitecture(pathDll),-6}{Environment.NewLine}";
                }
                catch (Exception)
                {
                    description =
                        $"{description}- {dllName,-50}\t: dll not found!{Environment.NewLine}";
                }
            }
            description = $"{description}\r\n\r\nInstalled at:\t {pathRoot}\r\nSettings:\t\t{pathSettings}";
            MessageBox.Show(description, caption);
        }
    }
    
}
