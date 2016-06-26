
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
        /// Get SQL File path from file name. It looks in the hoTools SQL Path Settings to search for file
        /// </summary>
        /// <param name="sqlFileName"></param>
        /// <returns></returns>
        string GetSqlFilePathFromName(string sqlFileName);
    }
}
