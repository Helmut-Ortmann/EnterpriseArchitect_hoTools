using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data;
using ClosedXML.Excel;

namespace hoTools.Utils.Excel
{
    public static class Excel
    {
        /// <summary>
        /// Make EA Excel File from EA SQLQuery format (string)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        public static bool MakeExcelFile(string x, string fileName = @"d:\temp\sql\sql.xlsx")
        {

            if (string.IsNullOrEmpty(x)) return false;
            XDocument xDoc = XDocument.Parse(x);

            // get field names
            var fieldNames = xDoc.Descendants("Row").FirstOrDefault()?.Descendants();
            if (fieldNames == null) return false;

            DataTable dt = new DataTable();

            Util.TryToDeleteFile(fileName);
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                foreach (var field in fieldNames)
                {
                    dt.Columns.Add(field.Name.ToString(), typeof(string));
                }


                var xRows = xDoc.Descendants("Row");
                foreach (var xRow in xRows)
                {
                    DataRow row = dt.NewRow();
                    int column = 0;
                    foreach (var value in xRow.Elements())
                    {
                        row[column] = value.Value;
                        column = column + 1;

                    }
                    dt.Rows.Add(row);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, Path.GetFileNameWithoutExtension(fileName));
                    wb.SaveAs(fileName);
                }


                Cursor.Current = Cursors.Default;
                var ret = MessageBox.Show("Yes: Open Excel File\r\nNo: Open Folder\r\nCancel",
                    $"Excel File '{fileName}' created!", MessageBoxButtons.YesNoCancel);
                switch (ret)
                {
                    case DialogResult.Yes:
                        Util.StartFile(fileName);
                        break;
                    case DialogResult.No:
                        Util.ShowFolder(Path.GetDirectoryName(fileName));
                        break;

                }
                return true;
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error {e.Message}\r\n{e.Source}");
                return false;
            }
        }
       
    }
}
