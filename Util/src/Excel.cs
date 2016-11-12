using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data;
using ClosedXML.Excel;
using KBCsv;

namespace hoTools.Utils.Excel
{
    public static class Excel
    {
        public static bool MakeExcelFileFromCsv(string fileName = @"d:\temp\sql\csv.xlsx")
        {

            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                var csv = Clipboard.GetText(TextDataFormat.Text);
                var dt = new DataTable();
                using (var reader = CsvReader.FromCsvString(csv))
                {
                    char seperator = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                    reader.ValueSeparator = seperator;   // this will be used between each value
                    //reader.ValueDelimiter = ';';   // this will be used to wrap values that require it (because they contain the separator or a linefeed character)
                    reader.ReadHeaderRecord();
                   
                    dt.Fill(reader);
                }
                //using (var streamReader = new StreamReader(@"d:\temp\sql\test.csv"))
                Util.TryToDeleteFile(fileName);
                using (XLWorkbook wb = new XLWorkbook() )
                {
                    wb.Worksheets.Add(dt, Path.GetFileNameWithoutExtension(fileName));
                    wb.SaveAs(fileName);
                }
                HandleExcelFileByUser(fileName, dt);

            }

            return true;
        }

        /// <summary>
        /// Make EA Excel File from EA SQLQuery format (string)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        public static bool MakeExcelFileFromSqlResult(string x, string fileName = @"d:\temp\sql\sql.xlsx")
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


                HandleExcelFileByUser(fileName, dt);
                return true;
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error {e.Message}\r\n{e.Source}");
                return false;
            }
        }

        private static void HandleExcelFileByUser(string fileName, DataTable dt)
        {
            Cursor.Current = Cursors.Default;
            var ret = MessageBox.Show($"Excel File '{fileName}' with {dt.Rows.Count} rows and {dt.Columns.Count} column created!\r\n\r\nYes: Open Excel File\r\nNo: Open Folder\r\nCancel: Do nothing",
                $"Excel File created!", MessageBoxButtons.YesNoCancel);
            switch (ret)
            {
                case DialogResult.Yes:
                    Util.StartFile(fileName);
                    break;
                case DialogResult.No:
                    Util.ShowFolder(Path.GetDirectoryName(fileName));
                    break;
            }
        }
    }
}
