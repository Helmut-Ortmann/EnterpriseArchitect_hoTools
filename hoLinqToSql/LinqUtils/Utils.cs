using System;
using System.IO;
using System.Windows.Forms;

namespace hoLinqToSql.LinqUtils
{
    public class Utils
    {
        /// <summary>
        /// Save text to xml. It's enough to pass the filename without extension
        /// </summary>
        /// <param name="fileName">File name without extension</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SaveTextToXml(string fileName, string text)
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
    }
}
