using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
        public static void AboutMessage(string description, string caption, string[] lDllNames)
        {
            string pathRoot = Assembly.GetExecutingAssembly().Location;
            pathRoot = Path.GetDirectoryName(pathRoot);

            description = $@"{description}

Helmut.Ortmann@hoModeler.de
Helmut.Ortmann@t-online.de
(+49) 172 / 51 79 16 7

";
            foreach (string dllName in lDllNames)
            {
                
                string pathDll = Path.Combine(new[] { pathRoot, dllName });
                try
                {
                    // proportional font, no easy formatting
                    description =
                        $"{description}- {dllName,-50}\t: V{FileVersionInfo.GetVersionInfo(pathDll).FileVersion}{Environment.NewLine}";
                }
                catch (Exception e)
                {
                    MessageBox.Show($@"File:
'{pathDll}'

{e}","Can't find file");
                }
            }
            MessageBox.Show(description, caption);
        }
    }
}
