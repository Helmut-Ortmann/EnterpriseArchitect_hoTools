using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EA;
using HtmlAgilityPack;
using File = System.IO.File;

namespace hoLinqToSql.LinqUtils
{
    public class LinqPad
    {
        const string  lprunExeDefault = @"c:\Program Files (x86)\LINQPad5\lprun.exe";
        const string targetDirDefault = @"c:\temp\";

        private string _lprunExe;
        private string _linqDir;
        private string _targetDir;
        private string _targetFile;
        private string _format;
        private ProcessStartInfo _startInfo;

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
        public LinqPad(string lprun, string targetDir, string format)
        {
            LinqPadIni(lprun, targetDir, format);
        }

        /// <summary>
        /// Initialize LinqPad with default values
        /// </summary>
        public LinqPad()
        {
            LinqPadIni();
        }
        /// <summary>
        /// Run the LINQPad query via lprun.exe. It supports:
        /// - format: (html, csv, text)
        /// - arg: to pass information like GUID,..
        /// </summary>
        /// <param name="file">The LINQPad file, usually *.linq</param>
        /// <param name="format">"html", "csv", "text"</param>
        /// <param name="args">Things you want to pass to the LINQPad query, split by space</param>
        /// <returns></returns>
        public string Run(string file, string format, string args)
        {
            string lprunFormat = GetFormat(format);
            if (lprunFormat == "") return "";

            string outFile = Path.GetFileNameWithoutExtension(file) + "." + format; 
            string linqFile = Path.Combine(_targetDir, file);
            _targetFile = Path.Combine(_targetDir, outFile);
            DelTarget();
            string arg = $@"-lang=program -format={lprunFormat} ""{linqFile}""  {args} ";
            _startInfo.Arguments = arg;
            try
            {
                using (Process exeProcess = Process.Start(_startInfo))
                {
                    //* Read the output (or the error)
                    string output = exeProcess.StandardOutput.ReadToEnd();
                    string err = exeProcess.StandardError.ReadToEnd();
                    exeProcess.WaitForExit();
                    // Retrieve the app's exit code
                    int exitCode = exeProcess.ExitCode;
                    File.WriteAllText(_targetFile, output);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Query:{linqFile}\r\nLPRun.exe{_lprunExe}\r\nTarget:{_targetFile}{e}", " Error running LINQ query");
            }
            return "";
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
            //var headers = from table1 in doc.DocumentNode.SelectNodes("//table[@id='t1']").Cast<HtmlNode>()
            var headers = from table1 in nodeFirstTable.Cast<HtmlNode>()

                from row in table1.SelectNodes("tr").Cast<HtmlNode>().Skip(1) // skip heading
                from cell in row.SelectNodes("th|td").Cast<HtmlNode>()//"th|td"
                where cell.Name == "th"
                select new { Name = HtmlEntity.DeEntitize(cell.InnerText) };
            // it's possible that there is no header
            int countColumn = 0;
            foreach (var header in headers)
            {
                dt.Columns.Add(header.Name);
                countColumn++;
            }

            //-----------------------------------------------
            var node = nodeFirstTable.Elements("tr");
            // Skip LINQPad Heading and Column heading
            var rows = nodeFirstTable.Elements("tr").Skip(2).Select(tr => tr
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
        /// 
        /// </summary>
        /// <param name="lprunExe"></param>
        /// <param name="targetDir"></param>
        /// <param name="format"></param>

        private void LinqPadIni(string lprunExe=lprunExeDefault, string targetDir=targetDirDefault, string format="html")
        {
            _lprunExe = lprunExe;
            _targetDir = targetDir;
            _format = format;

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
        /// <param name="SearchTerm"></param>
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
                foreach (EA.Element el in dia.SelectedObjects)
                {
                    els = $"{els}{el.ElementID}{delimiter}";
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
                els = $"{els}{el.ElementID}{delimiter}";
                delimiter = ",";
            }
            arg = $"{arg} SelectedElementIds={els} SearchTerm={searchTerm}";
            return arg;
        }
    }
}
