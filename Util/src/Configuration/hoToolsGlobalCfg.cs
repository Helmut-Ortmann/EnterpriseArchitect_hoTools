using System.IO;
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
        /// Read the SQL file. It uses the SQL file path to find the file. The sequence of search is:
        /// <para />1. path (it accept '.' as current directory)
        /// <para />2. filename as it is
        /// </summary>
        /// <param name="sqlFileName"></param>
        /// <returns></returns>
        public string ReadSqlFile(string sqlFileName)
        {
            // over all files
            foreach (string path in _lpaths)
            {
                string fileName = Path.Combine(path, sqlFileName);
                if (File.Exists(fileName))
                {
                    return File.ReadAllText((fileName));
                }
            }
            // nothing found
            return "";
        }
    }
}
