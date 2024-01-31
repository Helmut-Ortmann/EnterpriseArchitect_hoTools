using System;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using hoLinqToSql.LinqUtils;

namespace hoTools.Utils.General
{
    public class Basic
    {
        /// <summary>
        /// Handle ApplicationDoEvents()
        ///
        ///  </summary>
        public static bool DoHandleApplicationDoEvents = false;
        /// <summary>
        /// Handle do events
        /// </summary>
        public static void ApplicationDoEvents()
        {
            if (DoHandleApplicationDoEvents) ApplicationDoEvents();
        }

        /// <summary>
        /// Save text to xml. It's enough to pass the filename without extension
        /// - In dialog it asks to open the file
        /// </summary>
        /// <param name="fileName">File name without extension</param>
        /// <param name="text"></param>
        /// <param name="isDialog"></param>
        /// <returns></returns>
        public static string SaveTextToXml(string fileName, string text, bool isDialog = true)
        {
            fileName = fileName.Replace("->", " to ");
            if (!Directory.Exists(fileName))
            {
                SaveFileDialog saveFile = new SaveFileDialog
                {
                    FileName = Path.GetFileNameWithoutExtension(fileName),
                    Filter = @"xml file|*.xml"
                };
                if (saveFile.ShowDialog() == DialogResult.OK) fileName = saveFile.FileName;
                else return "";
            }

            try
            {
                File.WriteAllText(fileName, text);
                if (isDialog)
                {

                    Basic.HandleFileByUser(fileName);
                }
                return fileName;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"
File: {fileName}

{e}", @"Can't write to file ");
                return "";
            }





        }
        /// <summary>
        /// Check if filename has invalid characters
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileNameCorrect(string fileName)
        {
            return fileName.All(f => !Path.GetInvalidFileNameChars().Contains(f));
        }
        /// <summary>
        /// Start file or show url
        /// </summary>
        /// <param name="file">file(path) or url to start</param>
        public static void StartFile(string file)
        {
            try
            {
                Process.Start(file);
            }

            catch (Exception e)
            {
                MessageBox.Show($@"File: '{file}'{Environment.NewLine}{Environment.NewLine}{e}", @"Error start file!");
            }
        }
        /// <summary>
        /// Handle file by user
        /// - Open it
        /// - Show Folder
        /// - Do nothing
        /// </summary>
        /// <param name="fileName"></param>
        public static void HandleFileByUser(string fileName)
        {
            Cursor.Current = Cursors.Default;
            var ret = MessageBox.Show($@"File '{fileName}' created!{Environment.NewLine}{Environment.NewLine}Yes: Open File with default application{Environment.NewLine}No: Open Folder{Environment.NewLine}Cancel: Do nothing",
                @"File created!", MessageBoxButtons.YesNoCancel);
            switch (ret)
            {
                case DialogResult.Yes:
                    StartFile(fileName);
                    break;
                case DialogResult.No:
                    ShowFolder(Path.GetDirectoryName(fileName));
                    break;
            }
        }
        /// <summary>
        /// Open Directory of a directory or a file with Explorer or Totalcommander.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isTotalCommander"></param>
        public static void ShowFolder(string path, bool isTotalCommander = false)
        {
            if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);

            if (isTotalCommander)
                StartApp(@"totalcmd.exe", "/o " + path);
            else
                StartApp(@"Explorer.exe", "/e, " + path);
        }
        /// <summary>
        /// StartType Application with parameters.
        ///
        /// If you use blanks in path enclose the path with ".
        /// You may have to set the correct %PATH% variable
        /// </summary>
        /// <param name="app">Path to app</param>
        /// <param name="par">Parameters</param>
        public static void StartApp(string app, string par)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = app.Trim(),
                    Arguments = par
                }
            };
            try
            {
                p.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show($@"App-path: '{p.StartInfo.FileName}' 
Parameter: '{p.StartInfo.Arguments}'

Have you set the %path% environment variable?

{e}",
                    @"Can't start the App");
            }
        }
        /// <summary>
        /// File is locked 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get a unique temp file. Deletes old temp files (3 days). 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="extension"></param>
        /// <param name="deleteOldFiles">bool = yes: Delete old files</param>
        /// <returns></returns>
        public static string GetTempFilePathWithExtension(string prefix, string extension, bool deleteOldFiles = true)
        {
            var path = Path.GetTempPath();
            if (deleteOldFiles)
            {
                string[] filePaths = Directory.GetFiles(path, $@"*{extension}");
                foreach (string filePath in filePaths)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(filePath);
                        if (fi.LastAccessTime < DateTime.Now.AddDays(-3))
                        {
                            DeleteFile(filePath);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                }
            }
            var fileName = prefix + Guid.NewGuid() + extension;
            return Path.Combine(path, fileName);
        }
        /// <summary>
        /// Delete file. Remove a readonly and don't delete locked files.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath, bool noMessage = true)
        {
            if (String.IsNullOrWhiteSpace(filePath)) return true;
            try
            {
                if (!IsFileLocked(filePath))
                {
                    System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                return true;
            }
            catch (Exception e)
            {
                if (!noMessage)
                {
                    MessageBox.Show($@"
file: {filePath}
{e}", @"Can't delete file");
                }

                return false;
            }

        }
        /// <summary>
        /// Copy Repository to temp file
        /// </summary>
        /// <param name="repPath"></param>
        /// <param name="prefix"></param>
        /// <param name="oldPath"></param>
        /// <param name="isDialog"></param>
        /// <returns></returns>
        public static string CopyRepToTempFile(string repPath, string prefix = @"SWADM", string oldPath = "", bool isDialog = true)
        {
            if (!String.IsNullOrWhiteSpace(oldPath)) Basic.DeleteFile(oldPath);
            var newPath = Basic.GetTempFilePathWithExtension(prefix, Path.GetExtension(repPath));
            try
            {
                System.IO.File.Copy(repPath,
                    newPath, overwrite: true);
                return newPath;
            }
            catch (Exception e)
            {
                if (isDialog)
                {
                    MessageBox.Show($@"
Source: '{repPath}'
Target: '{newPath}'

{e}", @"Error copying SWADM to temp");

                }
                else
                {
                    Console.WriteLine($@"Can't copy to temporary *.eap file, break!
Source: {repPath}
Target: {newPath} 

{e}");

                }
                return "";
            }
        }

        /// <summary>
        /// Trace DataTable to xml
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file">Default</param>
        /// <param name="isDialog">Default: true</param>
        /// <returns></returns>
        public static string TraceDtToXml(DataTable dt, string file = @"c:\temp\traceDt.xml", bool isDialog = true)
        {
            string text = dt.ToXml("trace").ToString();
            if (isDialog)
            {
                string fileName = Basic.SaveTextToXml(file, text);
                Basic.HandleFileByUser(fileName);
            }
            else
            {
                File.WriteAllText(file, text);
            }

            return text;
        }

        /// <summary>
        /// Get Element from delimiter separated list
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetElementFromDelString(string text, int position, char separator = ',')
        {
            if (String.IsNullOrEmpty(text)) return "";
            var t = text.Split(separator);
            if (position > t.Length - 1) return "";

            return t[position];
        }



    }
}
