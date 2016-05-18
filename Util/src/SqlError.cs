using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils.SQL
{
    /// <summary>
    /// Handles EA SQL errors
    /// </summary>
    public static class SqlError
    {
        const string DBERROR_FILE_NAME = "dberror.txt";
        const string hoTools_LAST_SQL_FILE_NAME = "hoTools_LastSql.txt";
        const string hoTools_SQL_TEMPLATE_MACRO_FILE__NAME = "hoTools_SqlTemplatesAndMacros.txt";


        static string getEaSqlErrorPath()
           => Environment.GetEnvironmentVariable("appdata") + @"\Sparx Systems\EA";

        /// <summary>
        /// Get EA file complete file name in EA home directory (%appdata%Sparx System\EA\ + file).
        /// That's the path where dberror.txt is located.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static string getEaHomeFileName(string file)
        {
            string path = getEaSqlErrorPath();
            return Path.Combine(path, file);
        }
        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="fileName"></param>
        static void delete(string fileName)
        {
            string path = getEaHomeFileName(fileName);
            try
            {
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $"Error delete file '{path}'");
            }
        }
        /// <summary>
        /// Write file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        static void write(string fileName, string content)
        {
            string path = getEaHomeFileName(fileName);
            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $"Error writing file '{path}'");
            }
        }
        /// <summary>
        /// read content from file
        /// </summary>
        static string read(string fileName)
        {
            string path = getEaHomeFileName(fileName);
            try
            {

                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $"Error reading from file '{path}'");
                return "";
            }
        }
        //------------------------------------------------------------------------------
        // SqlTemplatesAndMacros
        public static string getSqlTemplatesAndMacrosFilePath() 
            => getEaHomeFileName(hoTools_SQL_TEMPLATE_MACRO_FILE__NAME);
        public static void  writeSqlTemplatesAndMacros(string text)
        {
            write(getSqlTemplatesAndMacrosFilePath(), text);
        }
        public static string readSqlTemplatesAndMacros()
            => read(getSqlTemplatesAndMacrosFilePath());

        //------------------------------------------------------------------------------
        // DBERROR_FILE_NAME
        /// <summary>
        /// Get the error string which EA stores.
        /// </summary>
        /// <returns>Error message + SQL the error is based on</returns>
        public static string getEaSqlErrorFilePath() 
            => getEaHomeFileName(DBERROR_FILE_NAME);
        /// <summary>
        /// Read SQL error file
        /// </summary>
        /// <returns></returns>
        public static string readEaSqlError()
           => read(getEaSqlErrorFilePath());
        /// <summary>
        /// Returns true if an EA SQL error file exists
        /// </summary>
        /// <returns></returns>
        public static bool existsEaSqlErrorFile()
            => File.Exists(getEaSqlErrorFilePath());

        // write SQL error file
        public static void writeEaSqlError(string text)
        {
            write(getEaSqlErrorFilePath(), text);
        }
        /// <summary>
        /// Delete SQL error file
        /// </summary>
        public static void deleteEaSqlError()
        {
            delete(getEaSqlErrorFilePath());
        }

        //---------------------------------------------------------------------------------
        // hoTools_LAST_SQL
        /// <summary>
        /// Get the sql string which is sent to EA.
        /// </summary>
        /// <returns>The SQL sent to EA</returns>
        public static string getHoToolsLastSqlFilePath() 
            => getEaHomeFileName(hoTools_LAST_SQL_FILE_NAME);
        /// <summary>
        /// Write last SQL to EA home 
        /// </summary>
        /// <returns></returns>
        public static void writeHoToolsLastSql(string text)
        {
            write(getHoToolsLastSqlFilePath(), text);
        }
        /// <summary>
        /// Read last SQL from EA home 
        /// </summary>
        /// <returns></returns>
        public static string readHoToolsLastSql()
          => read(getHoToolsLastSqlFilePath());






    }
}
