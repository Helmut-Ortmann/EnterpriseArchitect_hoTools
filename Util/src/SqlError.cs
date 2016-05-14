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
        const string hoTools_SQL_FILE_NAME = "hoTools_LastSql.sql";

        #region getEaSqlErrorFilePatch
        /// <summary>
        /// Get the error string which EA stores.
        /// </summary>
        /// <returns>Error message + SQL the error is based on</returns>
        public static string getEaSqlErrorFilePath()
        {
            string path = getEaSqlErrorPath();
            return Path.Combine(path, DBERROR_FILE_NAME);
        }
        #endregion
        #region getHoToolsSqlFilePath
        /// <summary>
        /// Get the sql string which is sent to EA.
        /// </summary>
        /// <returns>The SQL sent to EA</returns>
        public static string getHoToolsSqlFilePath()
        {
            string path = getEaSqlErrorPath();
            return Path.Combine(path, hoTools_SQL_FILE_NAME);
        }
        #endregion
        #region writeEaSqlFile
        /// <summary>
        /// Writes the sql-file before sending it to EA.
        /// </summary>
        public static void writeEaSqlFile(string sql)
        {
            string path = getHoToolsSqlFilePath();
            try
            {
                File.WriteAllText(path, sql);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),$"Error writing file '{path}'");
            }
        }
        #endregion
        #region readEaSqlFile
        /// <summary>
        /// Read the sql-file before it was sent to EA.
        /// </summary>
        /// <returns>The SQL sent to EA</returns>
        public static string readEaSqlFile()
        {
            string path = getHoToolsSqlFilePath();
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), $"Error writing file '{path}'");
                return "";
            }
        }
        #endregion

        public static string getEaSqlErrorPath()
        {
            return Environment.GetEnvironmentVariable("appdata") + @"\Sparx Systems\EA";

        }

        #region getEaSqlError
        /// <summary>
        /// Gets the SQL error from EA
        /// </summary>
        /// <returns></returns>
        public static string getEaSqlError()
        {
            string filePath = getEaSqlErrorFilePath();
            try
            {

                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\r\nFile:'{filePath}'", $"Can't read EA SQL Error file dberror.tx, nor error occurred?");
                return "";
            }
        }
        #endregion

        #region deleteEaSqlErrorFile
        /// <summary>
        /// Delete the EA SQL error file if it exists. If it don't exists no error message is issued.
        /// </summary>
        /// <returns></returns>
        public static bool deleteEaSqlErrorFile()
        {
            string filePath = getEaSqlErrorFilePath();
            try
            {

                File.Delete(filePath);
            }
            catch (Exception e)
            {
                //MessageBox.Show($"{e.Message}\nFile:'{filePath}'", $"Can't delete EA SQL Error file dberror.tx");
                return false;
            }
            return true;
        }
        #endregion

        #region existsEaSqlErrorFile
        /// <summary>
        /// Returns true if an EA SQL error file exists
        /// </summary>
        /// <returns></returns>
        public static bool existsEaSqlErrorFile()
        {

                return File.Exists(getEaSqlErrorFilePath());
        }
        #endregion
    }
}
