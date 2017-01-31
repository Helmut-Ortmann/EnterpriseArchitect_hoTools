using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace hoTools.Utils.Configuration
{
    public sealed class HoToolsGlobalCfg: IHoToolsGlobalCfg
    {
        string _sqlPaths;
        string[] _lSqlPaths;

        string _extensionPaths;
        string[] _lExtensionPaths;
        // the owner of the windows, used to prevent modal windows in background
        private Control _owner;

        HoToolsGlobalCfg()
        {
            
        }
        /// <summary>
        /// Access HoToolsGlobalCfg.Instance to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static HoToolsGlobalCfg Instance { get; } = new HoToolsGlobalCfg();

        /// <summary>
        /// The owner of all windows. Used to prevent modal windows stuck in background.
        /// </summary>
        public Control Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// hoTools config path (..user\&lt;users>\AppData\Roaming\ho\hoTools\)
        /// </summary>
        public string ConfigPath { get; set; }

        #region ExtensionPath

        /// <summary>
        ///  Set hoTools SQL path from Settings to search for SQL files. 
        /// </summary>
        /// <param name="paths"></param>
        public void SetExtensionPaths(string paths)
        {
            _extensionPaths = paths;
            _lExtensionPaths = paths.Split(';');
        }

        /// <summary>
        /// Get the absolute file name. It searches according to the in settings specified SQL path. If the file don't exists it return "".
        /// </summary>
        /// <param name="extensionFileName"></param>
        /// <returns></returns>
        public string GetExtensionFileName(string extensionFileName)
        {
            return GetFileNameFromPath(_lExtensionPaths, extensionFileName);

        }
        /// <summary>
        /// Get complete extension filepath from file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetExtensionFileLong(string fileName)
        {
            return FileNameLong(GetExtensionListFileCompleteName(), fileName);
        }

        /// <summary>
        /// Get list of Extension files of SQL path
        /// </summary>
        /// <returns></returns>
        public List<string> GetExtensionListFileCompleteName()
        {
            return Files(_lExtensionPaths);

        }

        /// <summary>
        /// Get list of sql paths
        /// </summary>
        /// <returns></returns>
        public List<string> GetListExtensionPaths()
        {
            return _lExtensionPaths.ToList();
        }
        /// <summary>
        ///  Set hoTools SQL path from Settings to search for SQL files. 
        /// </summary>
        /// <param name="paths"></param>
        public void SetExtensionlPaths(string paths)
        {
            _extensionPaths = paths;
            _lExtensionPaths = paths.Split(';');
        }
        #endregion


        #region SqlPath
        public string GetSqlPaths()
        {
            return _sqlPaths;
        }
        /// <summary>
        /// Get list of sql paths
        /// </summary>
        /// <returns></returns>
        public List<string> GetListSqlPaths()
        {
            return _lSqlPaths.ToList();
        }
        /// <summary>
        ///  Set hoTools SQL path from Settings to search for SQL files. 
        /// </summary>
        /// <param name="paths"></param>
        public void SetSqlPaths(string paths)
        {
            _sqlPaths = paths;
            _lSqlPaths = paths.Split(';');
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
            return GetFileNameFromPath(_lSqlPaths, sqlFileName);
            
        }

        /// <summary>
        /// Get the absolute file name. It searches according to the in settings specified path. If the file don't exists it return "".
        /// </summary>
        /// <param name="lPaths"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileNameFromPath(string[] lPaths, string fileName)
        {
            // Absolute path
            if (Path.IsPathRooted(fileName))
            {
                if (File.Exists(fileName)) return fileName;
                else return "";
                
            }
            else
            {


                // over all files
                foreach (string path in lPaths)
                {
                    string f = Path.Combine(path, fileName);
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
        /// Get complete sql filepath from file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetSqlFileLong(string fileName )
        {
            return FileNameLong(GetSqlListFileCompleteName(), fileName);
        }

        private string FileNameLong(List<string> liFileCompleteName, string fileName)
        {
            // compare complete file name with extension
            foreach (string fileNameLong in liFileCompleteName)
            {
                if (Path.GetFileName(fileNameLong) == fileName) return fileNameLong;
            }

            // compare file name without extension
            fileName = Path.GetFileNameWithoutExtension(fileName);
            foreach (string fileNameLong in liFileCompleteName)
            {
                if (Path.GetFileNameWithoutExtension(fileNameLong) == fileName) return fileNameLong;
            }
            return "";
        }


        /// <summary>
        /// Get list of *.sql files of SQL path
        /// </summary>
        /// <returns></returns>
        public List<string> GetSqlListFileCompleteName()
        {
            return Files(_lSqlPaths);

        }

        private List<string> Files(string[] lPaths)
        {
            List<string> files = new List<string>();
            // over all files
            foreach (string path in lPaths)
            {
                if (Directory.Exists(path))
                    files.AddRange(Directory.GetFiles(path, "*.sql"));
            }
            return files;
        }

        public AutoCompleteStringCollection GetListFileName()
        {
            AutoCompleteStringCollection files = new AutoCompleteStringCollection();
            foreach (string file in GetSqlListFileCompleteName())
            {
                files.Add(Path.GetFileName(file));
            }
            return files;
        }
        #endregion

    }
}
