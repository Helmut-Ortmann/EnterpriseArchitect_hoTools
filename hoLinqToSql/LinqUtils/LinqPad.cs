using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            string arg = $@"-lang=program -format={lprunFormat} {linqFile}  args ";
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
                    MessageBox.Show($@"File:\r\n{_targetFile}", "File to show doesn't exists");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"File:\r\n{_targetFile}\r\n{e}", "Error showing file");
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
                    MessageBox.Show($@"Possible LPRun format values: 'htm','html', 'csv', 'txt', 'txt'\r\nCurrent value='{format}'",
                        "Can't understand format value");
                    return "";

            }
            
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
                    MessageBox.Show($@"File:\r\n{_targetFile}\r\n{e}", "Error deleting target file");
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
    }
}
