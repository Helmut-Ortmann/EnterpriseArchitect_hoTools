using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ClosedXML.Excel;
using KBCsv;

namespace hoTools.Utils.Export
{
    public static class Excel
    {
        /// <summary>
        /// Make Data Table from *.csv file. It uses the delimiter from Windows settings
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separator">default: curent TextInfo.ListSeparator</param>
        /// <returns></returns>
        public static DataTable MakeDataTableFromCsvFile(string fileName, char separator='\0')
        {
            // get the separator according to current culture
            if (separator == '\0')
                separator =
                Convert.ToChar(CultureInfo.CurrentCulture.TextInfo.ListSeparator);

            var table = new DataTable();

            try
            {
                // open without lock
                using (FileStream fs = new FileStream(fileName, FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    using (var streamReader = new StreamReader(fs))
                    using (var reader = new CsvReader(streamReader))
                    {
                        reader.ValueSeparator = separator; // this will be used between each value
                        reader.ReadHeaderRecord();
                        table.Fill(reader);
                    }
                    return table;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($@"Separator:'{separator}'\r\n {e.Message}", @"Cannot convert file content to Excel");
                return null;
            }
            

        }
        /// <summary>
        /// Make Excel file from *.csv stored in Clipboard.
        /// It uses the List Separator of the current culture. Usually it's ';' but might differ.
        /// The ValueDelimiter of the CSV converter is with it's default value. EA don't support Value Delimiter.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool MakeExcelFileFromCsv(string fileName = @"x:\temp\sql\csv.xlsx")
        {

            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                var csv = Clipboard.GetText(TextDataFormat.Text);
                var dt = new DataTable();
                // get the separator according to current culture
                char separator =
                            Convert.ToChar(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                try
                {


                    using (var reader = CsvReader.FromCsvString(csv))
                    {
                        
                        reader.ValueSeparator = separator; // this will be used between each value
                        //reader.ValueDelimiter = ';';   // this will be used to wrap values that require it (because they contain the separator or a linefeed character)
                        reader.ReadHeaderRecord();

                        dt.Fill(reader);
                    }
                    if (!SaveTableToExcel(ref fileName, dt)) return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show($@"Separator:'{separator}'\r\n {e.Message}",@"Cant't convert Clipboard content to Excel");
                    return false;
                }
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
                if (!SaveTableToExcel(ref fileName, dt)) return false;
               
                return true;
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($@"Error {e.Message} 

Module: {e.Source}");
                return false;
            }
        }
        /// <summary>
        /// Save Data Table to Excel. It's enough to pass the filename without extension
        /// </summary>
        /// <param name="fileName">File name without extension</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool SaveTableToExcel(ref string fileName, DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, Path.GetFileNameWithoutExtension(fileName));
                if (!Directory.Exists(fileName))
                {
                    SaveFileDialog saveFile = new SaveFileDialog
                    {
                        FileName = Path.GetFileNameWithoutExtension(fileName),
                        Filter = @"Excel file|*.xlsx|Excel file with macro|*.xlsm"
                    };
                    if (saveFile.ShowDialog() == DialogResult.OK) fileName = saveFile.FileName;
                    else return false;
                }
                //Util.TryToDeleteFile(fileName);
                wb.SaveAs(fileName);
                HandleExcelFileByUser(fileName, dt);
            }
            return true;
        }

        private static void HandleExcelFileByUser(string fileName, DataTable dt)
        {
            Cursor.Current = Cursors.Default;
            var ret = MessageBox.Show($"Excel File '{fileName}' with {dt.Rows.Count} rows and {dt.Columns.Count} column created!\r\n\r\nYes: Open Excel File\r\nNo: Open Folder\r\nCancel: Do nothing",
                @"Excel File created!", MessageBoxButtons.YesNoCancel);
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
