using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.Abouts
{
    public static class About
    {
        /// <summary>
        /// OutputAboutMessage.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="caption"></param>
        /// <param name="lDllNames"></param>
        /// <param name="pathSettings"></param>
        public static void AboutMessage([NotNull] string description, [NotNull] string caption, string[] lDllNames, [NotNull] string pathSettings="No")
        {
        

            description = $@"{description}

Helmut.Ortmann@hoModeler.de
Helmut.Ortmann@t-online.de
(+49) 172 / 51 79 16 7

";
            // get product version
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            description =
                    $"{description}{"Product-Version:",-32}\t: V{fileVersionInfo.ProductVersion}{Environment.NewLine}";

            // Get file-version of every dll
            string pathRoot = Assembly.GetExecutingAssembly().Location;
            pathRoot = Path.GetDirectoryName(pathRoot);
            foreach (string dllName in lDllNames)
            {

                string pathDll = Path.Combine(new[] { pathRoot, dllName });
                try
                {
                    
                    string version = FileVersionInfo.GetVersionInfo(pathDll).FileVersion;
                    // proportional font, no easy formatting
                    if (dllName.Length > 23 )
                    description =
                        $"{description}- {dllName,-50}: V{version}{Environment.NewLine}";
                    else
                    // proportional font, no easy formatting
                    description =
                        $"{description}- {dllName,-50}\t: V{version}{Environment.NewLine}";
                }
                catch (Exception e)
                {
                    MessageBox.Show($@"File:
'{pathDll}'

{e}","Can't find file");
                }
            }
            description = $"{description}\r\n\r\nInstalled at:\t {pathRoot}\r\nSettings:\t\t{pathSettings}";
            MessageBox.Show(description, caption);
        }
    }
    
}
