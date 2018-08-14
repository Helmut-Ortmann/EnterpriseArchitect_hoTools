using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataModels.VcSymbols;
using LinqToDB.DataProvider.SQLite;
using File = System.IO.File;

namespace hoLinqToSql.LinqUtils
{
    /// <summary>
    /// Utilities to handle VC Code Symbol database
    /// </summary>
    public static class VcDbUtilities
    {
        /// <summary>
        /// Get file path of the VC Symbol database
        /// </summary>
        /// <returns></returns>
        public static string GetVcPathSymbolDataBases()
        {
            return $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Code\User\workspaceStorage\";
        }
        /// <summary>
        /// Get path of source folder VC Code SQLite Symbol table
        /// </summary>
        /// <param name="folderPathSource"></param>
        /// <returns>"" if nothing found</returns>
        private static string GetPath(string folderPathSource)
        {
           
            // get the newest db paths as string
            // - of every release history 
            // - of every source folder supported by VS Code
            IEnumerable<string> vcDbs = (from f in Directory.GetFiles(GetVcPathSymbolDataBases(), "*.DB", SearchOption.AllDirectories)
                group f by Path.GetDirectoryName(f) into grpDir
                let newest = grpDir.Max(d => File.GetLastWriteTime(d))
                from n in grpDir
                where File.GetLastWriteTime(n) == newest
                select n);

            foreach (var vcDb in vcDbs)
            {
                string connectionString = $"DataSource={vcDb};Read Only=True;";

                //newConnectionString = vcDb;
                //using (BROWSEVCDB dc = new BROWSEVCDB(new SQLiteDataProvider("SQLite.Classic"), connectionString))
                using (BROWSEVCDB dc = new BROWSEVCDB(new SQLiteDataProvider("SQLite.Classic"), connectionString))

                {
                    try
                    {


                        var path = (from f in dc.Files
                                    where f.Name.ToLower().Contains(folderPathSource.ToLower())
                                    where f.LeafName.EndsWith("c")
                                    orderby f.Name.Length
                                    select f.Name).FirstOrDefault();
                        if (path != null) return vcDb;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($@"{e}", "Can't install SQLite database driver");
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Get connection string for C/C++ VC Code source folder
        /// </summary>
        /// <param name="folderPathSource"></param>
        /// <param name="withErrorMessage"></param>
        /// <returns>"" if nothing found</returns>
        public static string GetConnectionString(string folderPathSource, bool withErrorMessage = true)
        {
            string dbFilePath = GetPath(folderPathSource);
            if (withErrorMessage && dbFilePath == "")
            {
                MessageBox.Show($"SourceFolder:'{folderPathSource}'\r\nDatabase in appData\\Roaming\\Code\\..\\.BROWSE.VC.DB", "Can't find VC Code Symbol database, VS Code C/C++ installed and used?");
            }
            return dbFilePath == "" ? "" : $"DataSource={dbFilePath};Read Only=True;";
        }
    }
}
