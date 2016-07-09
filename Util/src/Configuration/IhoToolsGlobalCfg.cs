
namespace hoTools.Utils.Configuration
{
    public interface IHoToolsGlobalCfg
    {
        /// <summary>
        /// Get the paths of SQL paths as a semicolon separated list
        /// </summary>
        /// <returns></returns>
        string GetSqlPaths();

        /// <summary>
        /// Get SQL File path from file name. It looks for an absolute filename or in the hoTools SQL Path Settings to search for file
        /// </summary>
        /// <param name="sqlFileName"></param>
        /// <param name="withErrMessage"></param>
        /// <returns></returns>
        string ReadSqlFile(string sqlFileName, bool withErrMessage = true);

        }
    }
