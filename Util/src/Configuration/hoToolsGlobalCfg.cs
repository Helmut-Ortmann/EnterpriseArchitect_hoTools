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
        /// Get SQL File path from file name. It looks in the hoTools SQL Path Settings to search for file
        /// </summary>
        /// <param name="sqlFileName"></param>
        /// <returns></returns>
        public string GetSqlFilePathFromName(string sqlFileName)
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
