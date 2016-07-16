using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace hoTools.Utils.Configuration
{
    public sealed class HoToolsGlobalCfg: IHoToolsGlobalCfg
    {
        string _paths;
        string[] _lpaths;

        HoToolsGlobalCfg()
        {
            
        }
        /// <summary>
        /// Access HoToolsGlobalCfg.Instance to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static HoToolsGlobalCfg Instance { get; } = new HoToolsGlobalCfg();

        public string GetSqlPaths()
        {
            return _paths;
        }
        /// <summary>
        ///  Set hoTools SQL path from Settings to search for SQL files. 
        /// </summary>
        /// <param name="paths"></param>
        public void SetSqlPaths(string paths)
        {
            _paths = paths;
            _lpaths = paths.Split(';');
        }
       
        /// <summary>
        /// Read the SQL file. If it is an absolute path it uses this. If not it uses the SQL Path to find the complete sql file name. 
        /// <para />
        /// In case of IO errors or file not found it return "". If a not existing absolute patch is used an error message is output 
        /// (if withErrMessage = true, default) 
        /// </summary>
        /// <param name="sqlFileName"></param>
        /// <param name="withErrMessage"></param>
        /// <returns></returns>
        public string ReadSqlFile(string sqlFileName, bool withErrMessage = true)
        {
            // Absolute path
            if (Path.IsPathRooted(sqlFileName))
            {
                try
                {
                    return File.ReadAllText(sqlFileName);
                }
                catch (IOException e)
                {
                    if (withErrMessage)
                    {
                        MessageBox.Show($"Error reading sql file '{sqlFileName}'\n\n{e}", @"Error Reading *.sql file");
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            else
            {


                // over all files
                foreach (string path in _lpaths)
                {
                    string fileName = Path.Combine(path, sqlFileName);
                    if (File.Exists(fileName))
                    {
                        return File.ReadAllText(fileName);
                    }
                }
            }
            // nothing found
            return "";
        }

        /// <summary>
        /// Get list of *.sql files of SQL path
        /// </summary>
        /// <returns></returns>
        public List<string> getListFileCompleteName()
        {
            List<string> files = new List<string>();
            // over all files
            foreach (string path in _lpaths)
            {
                if (Directory.Exists(path))
                    files.AddRange(Directory.GetFiles(path, "*.sql"));
            }
            return files;

        }

        public AutoCompleteStringCollection getListFileName()
        {
            AutoCompleteStringCollection files = new AutoCompleteStringCollection();
            foreach (string file in getListFileCompleteName())
            {
                files.Add(Path.GetFileName(file));
            }
            return files;
        }
    }
}
