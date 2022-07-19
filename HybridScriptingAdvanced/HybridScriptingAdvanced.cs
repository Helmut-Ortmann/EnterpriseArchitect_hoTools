using System;
using System.Linq;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils;
using System.Data;
using hoLinqToSql.LinqUtils.Extensions;

// ReSharper disable once CheckNamespace
namespace hoHybridScriptAdvanced
{
    /// <summary>
    /// Example for an advanced C# Script which can be called from 
    /// - VBScript
    /// - JScript
    /// - JavaScript
    /// from within EA.
    ///
    /// It uses the hoTools library hoLinqToSql to simplify SQL access by LINQ2DB (https://github.com/linq2db/linq2db).
    /// linq2db offers a db independent access and is highly integrated with LINQ.
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
    class HybridScriptAdvanced
    {
        private readonly EA.Repository _repository;
        private readonly int _processId;


        /// <summary>
        /// Output to calling process vio stdout and via EA output window, tab 'Script'
        /// </summary>
        /// <param name="msg"></param>
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

        private HybridScriptAdvanced(int pid)
        {
            _processId = pid;
            _repository = SparxSystems.Services.GetRepository(_processId);
			Trace("Running C# Console Application 'HybridScriptingAdvanced.exe'");
        }

        /// <summary>
        /// Print all packages and locate each package
        /// </summary>
        /// <param name="package"></param>
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
        /// <summary>
        /// Get the object count of all t_object
        /// </summary>
        /// <returns></returns>
        private int GetObjectCount()
        {
            int count;
            // get connection string of repository
            string connectionString = LinqUtil.GetConnectionString(_repository, out var provider, out string providerName);
            using (var db = new DataModels.EaDataModel(provider, connectionString))
            {
                count = (from o in db.t_object
                    select o.Name).Count();
            }

            return count;
        }
        /// <summary>
        /// Query the EA model and returns the results in the EA Search Window
        /// </summary>
        /// <returns></returns>
        private bool QueryModel()
        {
            // get connection string of repository
            string connectionString = LinqUtil.GetConnectionString(_repository, out var provider, out string providerName);
            DataTable dt;
            using (var db = new DataModels.EaDataModel(provider, connectionString))
            {
                dt = (from o in db.t_object
                        orderby new {o.Name, o.Object_Type,o.Stereotype}
                        select new {CLASSGUID = o.ea_guid, CLASSTYPE = o.Object_Type, Name = o.Name,Type=o.Object_Type, Stereotype=o.Stereotype}
                      ).Distinct()
                      .ToDataTable();

            }
            // 2. Order, Filter, Join, Format to XML
            string xml = Xml.MakeXmlFromDataTable(dt);
            // 3. Out put to EA
            _repository.RunModelSearch("", "", "", xml);
            return true;
        }
        /// <summary>
        /// Show some basic technics
        /// - Got through packages
        /// - SQL with Aggregate to get something
        /// - SQL query with output to EA Search Window  
        /// </summary>
        /// <returns></returns>
        private bool PrintModel()
        {
            if (_repository == null)
            {
				Trace($"Repository unavailable for pid {_processId}");
                return false;
            }
            // 1. Walk through package structure
			Trace($"Target repository process pid {_processId}");
            EA.Collection packages = _repository.Models;
            for (short ip = 0; ip < packages.Count; ip++)
            {
                EA.Package child = (EA.Package)packages.GetAt(ip);
                PrintPackage(child);
            }

            // 2. Output the count of objects
            Trace($"Count={GetObjectCount()} objects");

            // 3. Query and output to EA Search Windows
            QueryModel();

            return true;
        }

        /// <summary>
        /// Entry for calling the *.exe from vbscript in a Hybrid Scripting EA-Environment
        /// args[0] contains the process id to connect to the correct repository
        /// args[1-] are free for individual uses, note only string or value types
        ///          here:
        ///          arg[1] Commands to distinguish different tasks 
        /// 
        /// The folder vb contains the VBScript source file to call this C# exe from EA VBScript
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Standard Output or Error Output</returns>
        static void Main(string[] args)
        {
            int pid;
            string command = "Unknown";
            if (args.Length > 0)
            {
                if (! Int32.TryParse(args[0], out pid))
                {
                    MessageBox.Show($"pid: {args[0]}", "Can't read pid (process id) from the first parameter");
                    ReturnError();
                    return;
                }
            }
            else
            {
                MessageBox.Show($"No first parameter available. Process ID required", "Can't read pid (process id) from the first parameter");
                ReturnError();
                return;

            }
            // Handle commands
            if (args.Length > 1)
            {
                command = args[1];
                switch (command)
                {
                    case "Message":
                        MessageBox.Show($"Command: {command}", "Message command");
                        break;
                    default:
                        MessageBox.Show($"Command: {command}", "Unknown command");
                        break;
                }
            }
            if (pid > 0)
            {
                try
                {
                    HybridScriptAdvanced p = new HybridScriptAdvanced(pid);
                    p.PrintModel();

                }
                catch (Exception e)
                {
                    MessageBox.Show($@"PID: {pid}
Command: {command}


{e}", "Error detected, break!");
                    ReturnError();
                }

            }

        }
        /// <summary>
        /// Return error
        /// </summary>
        /// <param name="msg"></param>
        private static void ReturnError(string msg = "")
        {
            Console.WriteLine("Error");
        }
    }
}
