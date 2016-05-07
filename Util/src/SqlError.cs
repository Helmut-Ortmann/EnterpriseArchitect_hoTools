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
        #region getEaSqlErrorFilePatch
        public static string getEaSqlErrorFilePath()
        {
            string appData = Environment.GetEnvironmentVariable("appdata");
            string filePath = appData + @"\Sparx Systems\EA\dberror.txt";
            return filePath;
        }
        #endregion

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
