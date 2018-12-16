using System;

// ReSharper disable once CheckNamespace
namespace hoHybridScriptAdvanced
{
    /// <summary>
    /// Example for a C# Script which can be called from 
    /// - VBScript
    /// - JScript
    /// - JavaScript
    /// from within EA.
    /// 
    /// With EA since 13. it's called HybridScripting and EA provides an IDE with Debugging features.
    /// It also works with EA before 13., you only need SparxSystems.Repository.dll to attach to EA Repository. 
    /// 
    /// Requirements: 'Needs: SparxSystems.Repository.dll' as reference
    /// 
    /// Principle:
    /// Call C# .exe and pass the EA process id. The C# uses the process id to connect to the Repository of the EA Instance.
    /// C# uses the SPARX DLL 'SparxSystems.Repository.dll' to connect to the Repository. C# can then use the full EA API. 
    /// 
    /// SparxSystems.Repository.dll gets a connection to the EA instance according to passed process id
    /// It gets the EA.App object from the EA instance according to the passed process id. After doing so you have full access to API.
    /// 
    /// References:
    /// https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/HybridScripting
    /// http://community.sparxsystems.com/community-resources/1065-use-c-java-for-your-vb-script
    /// </summary>
    class HybridScript
    {
        private readonly EA.Repository _repository;
        private readonly int _processId;

        private void Trace( string msg)
		{
			if(_repository != null)
			{
				// Displays the message in the 'Script' tab of Enterprise Architect System Output Window
				_repository.WriteOutput( "Script", msg, 0);
			}
            // output to standard output, can be read by EA vb script
			Console.WriteLine(msg);
		}

        private HybridScript(int pid)
        {
            _processId = pid;
            _repository = SparxSystems.Services.GetRepository(_processId);
			Trace("Running C# Console Application AppPattern .NET 4.0");
        }
        private void PrintPackage(EA.Package package)
        {
            Trace(package.Name);
            EA.Collection packages = package.Packages;
            for (short ip = 0; ip < packages.Count; ip++)
            {
                EA.Package child = (EA.Package)packages.GetAt(ip);
                _repository.ShowInProjectView(child);
                PrintPackage(child);
            }
        }

        private bool PrintModel()
        {
            if (_repository == null)
            {
				Trace($"Repository unavailable for pid {_processId}");
                return false;
            }
			Trace($"Target repository process pid {_processId}");
            EA.Collection packages = _repository.Models;
            for (short ip = 0; ip < packages.Count; ip++)
            {
                EA.Package child = (EA.Package)packages.GetAt(ip);
                PrintPackage(child);
            }
            return true;
        }

        /// <summary>
        /// Entry for calling the *.exe from vbscript in a Hybrid Scripting EA-Environment
        /// args[0] contains the process id to connect to the correct repository
        /// args[1-] are free for individual uses, note only string or value types
        /// 
        /// The folder vb contains the VBScript source file to call this C# exe from EA VBScript
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Standard Output or Error Output</returns>
        static void Main(string[] args)
        {
            int pid = 0;
            if (args.Length > 0)
            {
                pid = Int32.Parse(args[0]);
            }
            if (pid > 0)
            {
                HybridScript p = new HybridScript(pid);
                p.PrintModel();
            }

        }
    }
}
