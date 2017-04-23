using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EA;

namespace HybridScriptingDll
{
    /// <summary>
    /// Example of a c# Hybrid Script Server 
    /// - Assessable by EA Scripting
    /// 
    /// Procedure to use (example vb script):
    /// dim myObj
    /// Set myObj = CreateObject("HybridScriptingDll.HybridScript")
    /// Session.Output "Test hybrid scripting '" & Repository.LibraryVersion & "' "
    /// myObj.ProcessId = ProcessId("EA.exe")    ' needs the process id to connect to the right repository
    /// myObj.PrintModel()
    /// 
    ///  Note: 
    /// - You have to register the 'HybridScriptDll.dll' with 'RegisterAsCom.bat'
    /// - You have to register the 'HybridScriptDll.dll' with 'RegisterAsCom.bat'
    /// - You need local administration rights to register the *.dll
    /// 
    /// Delivery:
    /// - HybridScriptingDll.dll
    /// - Interop.EA.dll
    /// - SparxSystems.Repository.dll
    /// </summary>
    [ComVisible(true)]
    [Guid("15A3C485-6A97-4785-A74C-567B0B3EE7E7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("HybridScriptingDll.HybridScript")]
    [ComDefaultInterface(typeof(IHybridScriptingDll))]
    public class HybridScript : IHybridScriptingDll
    {
        private void Trace( string msg)
		{
			if(Repository != null)
			{
				// Displays the message in the 'Script' tab of Enterprise Architect System Output Window
				Repository.WriteOutput( "Script", msg, 0);
			}
            // output to standard output, can be read by EA vb script
			Console.WriteLine(msg);
		}

        private string _processId;
        public string ProcessId
        {
            get => _processId;
            set
            {
                int pid = Int32.Parse(value);
                MessageBox.Show($@"ProcessId={value},{pid}");
                try
                {
                    EA.Repository rep = SparxSystems.Services.GetRepository(pid);
                    Repository = rep;
                }
                catch (Exception e)
                {
                    MessageBox.Show($@"e", $@"ProcessId={value}");
                }
                _processId = value;
                MessageBox.Show($@"ProcessId={value}");
            }
        }

        public Repository Repository { set; get; }


        public HybridScript()
        {
            // This Message box is used to test the Script in debug mode:
            // Attach to SScripter.exe after running on breakpoint.
            // MessageBox.Show($@"Test Constructor reached, {Process.GetCurrentProcess().Id}");

        }

        //public HybridScript(EA.Repository rep)
        //{
        //    _repository = rep;
		//	Trace("Running C# Console Application with DLL, need to register");
        //}

        public void PrintPackage(EA.Package package)
        {
            Trace(package.Name);
            EA.Collection packages = package.Packages;
            for (short ip = 0; ip < packages.Count; ip++)
            {
                EA.Package child = (EA.Package)packages.GetAt(ip);
                Repository.ShowInProjectView(child);
                PrintPackage(child);
            }
        }

        private bool PrintModel(EA.Repository rep)
        {
            EA.Collection packages = Repository.Models;
            for (short ip = 0; ip < packages.Count; ip++)
            {
                EA.Package child = (EA.Package)packages.GetAt(ip);
                PrintPackage(child);
            }
            return true;

        }

        public bool PrintModel()
        {
            if (Repository == null)
            {
                return false;
            }
            PrintModel(Repository);
            return true;
        }
        
    }
}
