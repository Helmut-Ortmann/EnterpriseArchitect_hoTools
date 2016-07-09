using System.IO;
using System.Windows.Forms;

namespace hoTools.Utils.Configuration
{
    public sealed class HoToolsGlobalCfg: IHoToolsGlobalCfg
    {
        string _paths;
        string[] _lpaths;
        /// <summary>
        /// Allocate ourselves.
        /// We have a private constructor, so no one else can.
        /// </summary>
        static readonly HoToolsGlobalCfg _instance = new HoToolsGlobalCfg();
        HoToolsGlobalCfg()
        {
            
        }
        /// <summary>
        /// Access HoToolsGlobalCfg.Instance to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static HoToolsGlobalCfg Instance
        {
            get { return _instance; }
        }
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
    }
}
