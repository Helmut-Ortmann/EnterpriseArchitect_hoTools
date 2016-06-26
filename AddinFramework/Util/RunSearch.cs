using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAAddinFramework.Utils
{
    /// <summary>
    /// Run the SQL Search. It looks for the hoTools Path for the possible locations of the SQL.
    /// </summary>
    public class RunSearch
    {
        readonly string[] _lpaths;
        public RunSearch(string paths)
        {
            _lpaths = paths.Split(';');
        }

        public string GetSql(string sqlFileName)
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


        //Repository.RunModelSearch("", "", "", target);
    }
}
