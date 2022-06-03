using System;
using System.IO;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.SQL
{
    /// <summary>
    /// Handles EA SQL errors
    /// </summary>
    public static class SqlError
    {
        const string DberrorFileName = "dberror.txt";
        const string HoToolsLastSqlFileName = "hoTools_LastSql.txt";
        const string HoToolsSqlTemplateMacroFileName = "hoTools_SqlTemplatesAndMacros.txt";


        public static string GetEaSqlErrorPath()
           => Environment.GetEnvironmentVariable("appdata") + @"\Sparx Systems\EA";

        /// <summary>
        /// Get EA file complete file name in EA home directory (%appdata%Sparx System\EA\ + file).
        /// That's the path where dberror.txt is located.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static string GetEaHomeFileName(string file)
        {
            string path = GetEaSqlErrorPath();
            return Path.Combine(path, file);
        }
        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="fileName"></param>
        static void Delete(string fileName)
        {
            string path = GetEaHomeFileName(fileName);
            try
            {
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $@"Error delete file '{path}'");
            }
        }
        /// <summary>
        /// Write file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        static void Write(string fileName, string content)
        {
            string path = GetEaHomeFileName(fileName);
            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $@"Error writing file '{path}'");
            }
        }
        /// <summary>
        /// read content from file
        /// </summary>
        static string Read(string fileName)
        {
            string path = GetEaHomeFileName(fileName);
            try
            {

                return Util.ReadAllText(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $@"Error reading from file '{path}'");
                return "";
            }
        }
        //------------------------------------------------------------------------------
        // SqlTemplatesAndMacros
        public static string GetSqlTemplatesAndMacrosFilePath() 
            => GetEaHomeFileName(HoToolsSqlTemplateMacroFileName);
        public static void  WriteSqlTemplatesAndMacros(string text)
        {
            Write(GetSqlTemplatesAndMacrosFilePath(), text);
        }
        public static string ReadSqlTemplatesAndMacros()
            => Read(GetSqlTemplatesAndMacrosFilePath());

        //------------------------------------------------------------------------------
        // DBERROR_FILE_NAME
        /// <summary>
        /// Get the error string which EA stores.
        /// </summary>
        /// <returns>Error message + SQL the error is based on</returns>
        public static string GetEaSqlErrorFilePath() 
            => GetEaHomeFileName(DberrorFileName);
        /// <summary>
        /// Read SQL error file
        /// </summary>
        /// <returns></returns>
        public static string ReadEaSqlError()
           => Read(GetEaSqlErrorFilePath());
        /// <summary>
        /// Returns true if an EA SQL error file exists
        /// </summary>
        /// <returns></returns>
        public static bool ExistsEaSqlErrorFile()
            => File.Exists(GetEaSqlErrorFilePath());

        // write SQL error file
        public static void WriteEaSqlError(string text)
        {
            Write(GetEaSqlErrorFilePath(), text);
        }
        /// <summary>
        /// Delete SQL error file
        /// </summary>
        public static void DeleteEaSqlError()
        {
            Delete(GetEaSqlErrorFilePath());
        }

        //---------------------------------------------------------------------------------
        // hoTools_LAST_SQL
        /// <summary>
        /// Get the sql string which is sent to EA.
        /// </summary>
        /// <returns>The SQL sent to EA</returns>
        public static string GetHoToolsLastSqlFilePath() 
            => GetEaHomeFileName(HoToolsLastSqlFileName);
        /// <summary>
        /// Write last SQL to EA home 
        /// </summary>
        /// <returns></returns>
        public static void WriteHoToolsLastSql(string text)
        {
            Write(GetHoToolsLastSqlFilePath(), text);
        }
        /// <summary>
        /// Read last SQL from EA home 
        /// </summary>
        /// <returns></returns>
        public static string ReadHoToolsLastSql()
          => Read(GetHoToolsLastSqlFilePath());






    }
}
