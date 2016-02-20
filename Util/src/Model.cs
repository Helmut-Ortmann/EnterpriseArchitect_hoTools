using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace hoTools.Utils
{
    // Represents the current EA Model 
    class Model
    {
        public EA.Repository Repository { get; }
        public EA.App EaApp { get; }
        private static string _applicationFullPath;

        Model()
        {
            object obj = Marshal.GetActiveObject("EA.App");
            EaApp = obj as EA.App;
            Repository = EaApp.Repository;
        }

        /// <summary>
        /// returns the full path of the running ea.exe
        /// </summary>
        public static string applicationFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationFullPath))
                {
                    Process[] processes = Process.GetProcessesByName("EA");
                    if (processes.Length > 0)
                    {
                        _applicationFullPath = processes[0].MainModule.FileName;
                    }
                }
                return _applicationFullPath;
            }
        }
    }
}
