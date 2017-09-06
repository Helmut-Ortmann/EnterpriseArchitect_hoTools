using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using EA;
using HtmlAgilityPack;
using LinqToDB.DataProvider;
using File = System.IO.File;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace hoLinqToSql.LinqUtils
{
    public class LinqPad
    {
        const string LprunExeDefault = @"c:\Program Files (x86)\LINQPad5\lprun.exe";
        const string TargetDirDefault = @"c:\temp\";
        const string LinqPadConnectionsV2Default = @"C:\Users\helmu_000\AppData\Roaming\LINQPad\ConnectionsV2.xml";

        private string _lprunExe;
        private string _linqDir;
        private string _targetDir;
        private string _targetFile;
        private string _format;
        private EA.Repository _rep;
        private ProcessStartInfo _startInfo;

        private string _linqPadConnectionName;

        public string LinqPadConnections { get; set; }

        public bool UseLinqPadConnections { get; set; }


        public string TargetFile
        {
            get => _targetFile;
            set => _targetFile = value;
        }

        public string LinqDir
        {
            get => _linqDir;
            set => _linqDir = value;
        }

        public string TargetDir
        {
            get => _targetDir;
            set => _targetDir = value;
        }

        public string Format
        {
            get => _format;
            set => _format = value;
        }

        public string LprunExe
        {
            get => _lprunExe;
            set => _lprunExe = value;
        }

        /// <summary>
        /// Initialize LinqPad with parameters
        /// </summary>
        /// <param name="lprun"></param>
        /// <param name="targetDir"></param>
        /// <param name="format"></param>
        /// <param name="useLinqPadConnection"></param>
        public LinqPad(EA.Repository rep, string lprun, string targetDir, string format,
            bool useLinqPadConnection = false)
        {
            LinqPadIni(rep, lprun, targetDir, format, useLinqPadConnection);
        }

        /// <summary>
        /// Initialize LinqPad with default values
        /// </summary>
        public LinqPad(EA.Repository rep)
        {
            LinqPadIni(rep);
        }

        /// <summary>
        /// Run the LINQPad query via lprun.exe. It supports:
        /// - format: (html, csv, text)
        /// - arg: to pass information like GUID,..
        /// LinqPad outputs errors to MessageBox. 
        /// </summary>
        /// <param name="linqPadQueryFile">The LINQPad file, usually *.linq</param>
        /// <param name="outputFormat">"html", "csv", "text"</param>
        /// <param name="args">Things you want to pass to the LINQPad query, split by space</param>
        /// <param name="noConnection"></param>
        /// <returns></returns>
        public bool Run(string linqPadQueryFile, string outputFormat, string args, bool noConnection = false)
        {

            
            string _format = GetFormat(outputFormat);
            if (_format == "") return false;

            string outFile = Path.GetFileNameWithoutExtension(linqPadQueryFile) + "." + _format;
            string linqFile = Path.Combine(_targetDir, linqPadQueryFile);
            _targetFile = Path.Combine(_targetDir, outFile);
            DelTarget();
            // connection name  (Server.DB-Name)
            // Server: Server or file name (e.g. if JET/ACCESS)
            // .DB-Name optional
            // get LinqPad connection name
            string cxName = "";
            if (!(UseLinqPadConnections || noConnection == true || IsConnectionRequired(linqFile) == false))
            {
                _linqPadConnectionName = GetLinqPadName();
                if (String.IsNullOrWhiteSpace(_linqPadConnectionName)) return false;
                cxName = $@"""-cxname={_linqPadConnectionName}""";

            }

            _startInfo.Arguments = $@"-lang=program -format={_format} {cxName} ""{linqFile}""  {args} ";
            try
            {
                using (Process exeProcess = Process.Start(_startInfo))
                {
                    //* Read the output (or the error)
                    string output = exeProcess.StandardOutput.ReadToEnd();
                    //exeProcess.BeginErrorReadLine();
                    exeProcess.WaitForExit();
                    // Retrieve the app's exit code
                    var exitCode = exeProcess.ExitCode;
                    // the error handling is optimized and tested for html, other formats might not be covered best
                    if (_format== @"html" && (! output.StartsWith("<!DOCTYPE HTML>")) )
                    {
                        //string errText = exeProcess.StandardError.ReadToEnd();
                        MessageBox.Show($@"Query: '{linqPadQueryFile}'
EA Connection: '{_rep.ConnectionString}'
EA DB Type:       '{_rep.RepositoryType()}'
Command: '{_startInfo.Arguments}'


Tips:
- LINQPad Language: Program (C#, VB, F#)
Tips for LINQPad connections:
- Try the query with LINQPad and the designated LINQPad driver
- Have you specified the database (don't select Display all in a tree view)
- Deselect: Pluralize EntitySet and table properties
- Deselect: Capitalize property names




Error:    
{output}",
                            "Error returned from LINQPad via LPRun.exe");
                        return false;

                    }
                    File.WriteAllText(_targetFile, output);
                    // Error is visualized in Error code
                    // optimized for html
                    if (_format == @"html" & exitCode != 0)
                    {
                        // Show html
                        Process.Start(_targetFile);
                        
                        var doc = new HtmlDocument();
                        doc.LoadHtml(output);
                        string errorMessage = doc.DocumentNode.ChildNodes?.Nodes()?.ElementAt(3)?.InnerText;
                        if (errorMessage == null) errorMessage = "No error message found, see browser!";
                        MessageBox.Show($"Find error message also in browser\r\nFile: '{_targetFile}'\r\nLINQPad-File:'{linqFile}'\n\r\n\r{HtmlEntity.DeEntitize(errorMessage)}", 
                            "Error LINQPad");

                        return false;
                    }
                         
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Query:{linqFile}\r\nLPRun.exe{_lprunExe}\r\nTarget:{_targetFile}\r\n{e}",
                    " Error running LINQ query");

               
                return false;
            }
            return true;
        }

        static void CaptureError(object sender, DataReceivedEventArgs e)
        {
            MessageBox.Show($"e.Data", "Error returned from LINQPad via LPRun.exe");
        }

        /// <summary>
        /// Shows the generated file
        /// </summary>
        public void Show()
        {
            try
            {
                if (File.Exists(_targetFile))
                    Process.Start(_targetFile);
                else
                {
                    MessageBox.Show($"File:\r\n{_targetFile}", "File to show doesn't exists");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"File:\r\n{_targetFile}\r\n{e}", "Error showing file");
            }
        }
        /// <summary>
        /// Check if LINQ file requires a connection
        /// 
        /// </summary>
        /// <param name="linqFile"></param>
        /// <returns></returns>
        private bool IsConnectionRequired(string linqFile) { 
            Regex rgx = new Regex(@"<Connection>\s*<ID>[0-9a-f-]*</ID>");
            Match match = rgx.Match(File.ReadAllText(linqFile));
            return match.Success;
        }

    /// <summary>
        /// Get LINQPad connection name:
        /// - FileBased:  DataSourceName
        /// - DB: Server.Database
        /// </summary>
        /// <returns></returns>
        public string GetLinqPadName()
        {
            IDataProvider provider;
            string connectionString = LinqUtil.GetConnectionString(_rep, out provider);
            string server = "";
            string db = null;
            string dataSource = null;
            // File based
            Regex rgx = new Regex("(Data Source|Server|DataBase|Catalog)=([^;]*)",RegexOptions.IgnoreCase);
            Match match = rgx.Match(connectionString);
            while (match.Success)
            {

                switch (match.Groups[1].Value.ToLower())
                {
                    
                    case @"data source":
                    case "server":
                        server = $"{match.Groups[2].Value}";
                        break;
                    case "catalog":
                    case "database":
                        db = $"{match.Groups[2].Value}";
                        break;
                }
                match = match.NextMatch();
            }

            XDocument xConnection = XDocument.Load(LinqPadConnections);
            var connection = (from c in xConnection.Descendants("Connection")
                where c.Element("Server")?.Value.ToLower() == server.ToLower() &&
                      (String.IsNullOrWhiteSpace(db) || c.Element("Database")?.Value == db)
                //&& c.Element("DriverData")?.Element("providerName")?.Value == provider
                select new
                {
                    Provider = c.Element("DriverData")?.Element("providerName")?.Value == null ? "" : $"[{c.Element("DriverData").Element("providerName").Value}]",
                    Server = c.Element("Server")?.Value,
                    DataBase = c.Element("Database")?.Value == null ? "" : $"{c.Element("Database")?.Value}",
                    DbVersion = c.Element("DbVersion")?.Value == null ? "" : $"(v.{c.Element("DbVersion").Value})"
                }).FirstOrDefault();
            string linqPadConnectionName = "";
            if (connection == null)
            {
                MessageBox.Show($@"Do you have a valid LINQPad connection for your EA Model
ConnectionServer:    '{_rep.ConnectionString}'
Database type:       '{_rep.RepositoryType()}'
Server:                     '{server}'
Database:                 '{db}'
LINQPad connections: '{LinqPadConnections}'

Change File, Settings General to Use LINQPAd connection.
",
                    "Query needs a connection, can't determine LINQPad connection name.");
                return "";
            }
            linqPadConnectionName = $@"{connection.Provider} {connection.Server}\{connection.DataBase} {connection.DbVersion}";
            // adapt string to special provider needs
            switch (connection.Provider)
            {
                // LINQPad standard driver
                case "":
                case null:
                    linqPadConnectionName = $@"{connection.Server}.{connection.DataBase}";
                    break;
            }
            return linqPadConnectionName;






            if (server == "" || db == "")
            {
                MessageBox.Show($"ConnectionString: '{connectionString}'\r\nServer:  '{server}'\r\n'{db}'", "Can't determine LINQPad connection from EA connection string");
                return "";
            }

            return $"{server}.{db}";


        }
        /// <summary>
        /// Check format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private string GetFormat(string format)
        {
            string f = format.Trim().ToLower();
            switch (f)
            {
                case @"htm":
                case @"html":
                    return @"html";
                case @"csv":
                    return @"csv";
                case @"txt":
                case @"text":
                    return "text";
                default:
                    MessageBox.Show($"Possible LPRun format values: 'htm','html', 'csv', 'txt', 'txt'\r\nCurrent value='{format}'",
                        "Can't understand format value");
                    return "";

            }
            
        }


        /// <summary>
        /// ReadHtml table from specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable ReadHtml(string fileName, string tableName )
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            if (! File.Exists(fileName))
            {
                MessageBox.Show($"File: '{fileName}'","HTML File doesn't exists, Break!!!");
                return null;

            }
            try
            {
                doc.LoadHtml(File.ReadAllText(fileName));
            }
            catch (Exception e)
            {
                MessageBox.Show($"File: '{fileName}'\r\n{e}", "Error scan HTML File, Break!!!");
                return null;

            }

            if (String.IsNullOrWhiteSpace(tableName)) tableName = "t1";

            DataTable dt = new DataTable();
            var nodeFirstTable = doc.DocumentNode.SelectNodes($@"//table[@id='{tableName}']");
            if (nodeFirstTable == null || ! nodeFirstTable.Any())
            {
                MessageBox.Show($"File: '{fileName}'\r\nTableName: '{tableName}'", "Can't find HTML table");
                return dt;
            }


            // determine columns header, they may not be defined.
            int countColumn = 0;
            try
            {
                var nodeHeader = doc.DocumentNode.SelectNodes(@"//table[@id='t1']//th");
                if (nodeHeader != null)
                {
                    foreach (var row in nodeHeader)
                    {
                        dt.Columns.Add(row.InnerText);
                        countColumn = countColumn + 1;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("hoTools only supports simple tables. It looks as if the table 't1' / hierarchy is to complex", "Can't decode HTML table!");
                return dt;
            }

            try
            {
                //-----------------------------------------------
                var node = nodeFirstTable.Elements("tr");
                // Skip LINQPad Heading and filter not td child elements
                var rows = nodeFirstTable.Elements("tr").Skip(1)
                    .Where(x => x.FirstChild.Name == "td")
                    .Select(tr => tr
                        .Elements(@"td")
                        .Select(td => HtmlEntity.DeEntitize(td.InnerText.Trim()))
                        .ToArray());
                //Fill DataTable
                foreach (object[] row in rows)
                {
                    // if there is no header
                    if (countColumn == 0)
                    {
                        countColumn = row.Length;
                        for (int i = 0; i < countColumn; i++)
                        {
                            dt.Columns.Add(" ");
                        }
                    }
                    dt.Rows.Add(row);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("hoTools only supports simple tables. hoToolls can't understand table 't1'", "Can't decode HTML table!");
                return dt;
            }
            return dt;

        }
        /// <summary>
        /// Read HTML from LINQPad into DataTable. It uses the stored file from generating via LINQPad
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable ReadHtml(string tableName = "t1")
        {
            return ReadHtml(_targetFile, tableName);
        }
        /// <summary>
        /// Delete target
        /// </summary>
        public void DelTarget()
        {
            if (File.Exists(_targetFile))
            {
                try
                {
                    File.Delete(_targetFile);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"File:\r\n{_targetFile}\r\n{e}", "Error deleting target file");
                }
            }

        }

        /// <summary>
        /// Constructure LINQPad
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="lprunExe"></param>
        /// <param name="targetDir"></param>
        /// <param name="format"></param>
        /// <param name="useLinqPadConnection"></param>
        private void LinqPadIni(EA.Repository rep, string lprunExe=LprunExeDefault, string targetDir=TargetDirDefault, string format=@"html", bool useLinqPadConnection=false)
        {
            _rep = rep;
            _lprunExe = lprunExe;
            _targetDir = targetDir;
            _format = format;
            UseLinqPadConnections = useLinqPadConnection;
            LinqPadConnections = LinqPadConnectionsV2Default;

            // initialize ProzessInfo
            _startInfo = new ProcessStartInfo();
            _startInfo.CreateNoWindow = false;
            _startInfo.UseShellExecute = false;
            _startInfo.FileName = _lprunExe;
            _startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _startInfo.Arguments = "";
            _startInfo.RedirectStandardOutput = true;
            _startInfo.RedirectStandardError = true;

            
        }

        /// <summary>
        /// Get the args parameter to pass to LINQ with the EA context information
        /// - Element
        /// - Diagram
        /// - Selected Diagram connector
        /// - Selected Diagram items
        /// - Tree Selected elements
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public string GetArg(EA.Repository rep, string searchTerm)
        {
            string arg = "";

            //---------
            // 1. Context Element
            object item;
            EA.ObjectType type = rep.GetContextItem(out item);
            switch (type)
            {
                case ObjectType.otPackage:
                    arg = $"ContextPackage={((EA.Package)item).PackageGUID}";
                    break;

                case ObjectType.otAttribute:
                    arg = $"ContextAttribute={((EA.Attribute)item).AttributeGUID}";
                    break;

                case ObjectType.otConnector:
                    arg = $"ContextConnector={((EA.Attribute)item).AttributeGUID}";
                    break;
                case ObjectType.otMethod:
                    arg = $"ContextMethod={((EA.Method)item).MethodGUID}";
                    break;
                case ObjectType.otModel:
                    arg = $"ContextModel={((EA.Package)item).PackageGUID}";
                    break;
                case ObjectType.otElement:
                    arg = $"ContextElement={((EA.Element) item).ElementGUID}";
                    break;
                    // not defined
                default:
                    arg = $"Context=";
                    break;


            }
            string els = "";
            string delimiter = "";
            // Diagram Info
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia != null)
            {
                arg = $"{arg} CurrentDiagram={dia.DiagramGUID}";
                if (dia.SelectedConnector != null)
                    arg = $"{arg} SelectedConnectorId={dia.SelectedConnector.ConnectorID}";
                else
                    arg = $"{arg} SelectedConnectorId=";
                foreach (EA.DiagramObject diaObj in dia.SelectedObjects)
                {
                    els = $"{els}{delimiter}{diaObj.ElementID}";
                    delimiter = ",";
                }
                arg = $"{arg} SelectedElementIds={els}";
            }
            else
            {
                arg = $"{arg} CurrentDiagram=";
                arg = $"{arg} SelectedConnectorId=";
                arg = $"{arg} SelectedElementIds=";
            }

            // Tree Selected
            els = "";
            delimiter = "";
            EA.Collection lel = rep.GetTreeSelectedElements();
            foreach (EA.Element el in rep.GetTreeSelectedElements())
            {
                els = $"{els}{delimiter}{el.ElementID}";
                delimiter = ",";
            }
            arg = $"{arg} SelectedElementIds={els} SearchTerm={searchTerm}";
            return arg;
        }
    }
}
