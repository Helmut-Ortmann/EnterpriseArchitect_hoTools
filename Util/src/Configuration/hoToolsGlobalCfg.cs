using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        /// <summary>
        /// hoTools config path (..user\&lt;users>\AppData\Roaming\ho\hoTools\)
        /// </summary>
        public string ConfigPath { get; set; }
        public string GetSqlPaths()
        {
            return _paths;
        }
        /// <summary>
        /// Get list of sql paths
        /// </summary>
        /// <returns></returns>
        public List<string> GetListSqlPaths()
        {
            return _lpaths.ToList();
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
            string absFileName = GetSqlFileName(sqlFileName);
            if (absFileName != "") return File.ReadAllText(absFileName);
            if ( absFileName == ""  ) return "";

            if (withErrMessage)
            {
               MessageBox.Show($@"Error reading sql file '{sqlFileName}'", @"Error Reading *.sql file");
            }
            return "";

        }
        /// <summary>
        /// Get the absolute file name. It searches according to the in settings specified SQL path. If the file don't exists it return "".
        /// </summary>
        /// <param name="sqlFileName"></param>
        /// <returns></returns>
        public string GetSqlFileName(string sqlFileName)
        {
            // Absolute path
            if (Path.IsPathRooted(sqlFileName))
            {
                if (File.Exists(sqlFileName)) return sqlFileName;
                else return "";
                
            }
            else
            {


                // over all files
                foreach (string path in _lpaths)
                {
                    string f = Path.Combine(path, sqlFileName);
                    if (File.Exists(f))
                    {
                        return f;
                    }
                   
                }
            }
            // nothing found
            return "";
        }



        /// <summary>
        /// Get complete filepath from file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileLong(string fileName )
        {
            // compare complete file name with extension
            foreach (string fileNameLong in GetListFileCompleteName())
            {
                if (Path.GetFileName(fileNameLong) == fileName) return fileNameLong;
            }

            // compare file name without extension
            fileName = Path.GetFileNameWithoutExtension(fileName);
            foreach (string fileNameLong in GetListFileCompleteName())
            {
                if (Path.GetFileNameWithoutExtension(fileNameLong) == fileName) return fileNameLong;
            }
            return "";

        }
        /// <summary>
        /// Get list of *.sql files of SQL path
        /// </summary>
        /// <returns></returns>
        public List<string> GetListFileCompleteName()
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

        public AutoCompleteStringCollection GetListFileName()
        {
            AutoCompleteStringCollection files = new AutoCompleteStringCollection();
            foreach (string file in GetListFileCompleteName())
            {
                files.Add(Path.GetFileName(file));
            }
            return files;
        }

    }
}
