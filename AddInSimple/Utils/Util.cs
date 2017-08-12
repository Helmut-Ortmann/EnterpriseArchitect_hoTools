using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AddInSimple.Utils
{
    /// <summary>
    /// Utilities for general use
    /// </summary>
    public static class Util
    {

        /// <summary>
        /// Make EA xml from a DataTable (for column names) and the ordered Enumeration provided by LINQ. Set the Captions in DataTable to ensure column names. 
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rows">LINQ Query for the data to get</param>
        /// <returns></returns>
        public static string MakeXml(DataTable dt, OrderedEnumerableRowCollection<DataRow> rows)
        {
            XElement xFields = new XElement("Fields");
            foreach (DataColumn col in dt.Columns)
            {
                XElement xField = new XElement("Field");
                xField.Add(new XAttribute("name", col.Caption));
                xFields.Add(xField);
            }
            try
            {
                XElement xRows = new XElement("Rows");

                foreach (var row in rows)
                {
                    XElement xRow = new XElement("Row");
                    int i = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        XElement xField = new XElement("Field");
                        xField.Add(new XAttribute("value", row[i].ToString()));
                        xField.Add(new XAttribute("name", col.Caption));
                        xRow.Add(xField);
                        i = i + 1;
                    }
                    xRows.Add(xRow);
                }
                XElement xDoc = new XElement("ReportViewData");
                xDoc.Add(xFields);
                xDoc.Add(xRows);
                return xDoc.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}", "Error enumerating through LINQ query");
                return "";
            }
        }
        /// <summary>
        /// Make DataTable from EA sql results
        /// </summary>
        /// <param name="sqlXml"></param>
        /// <returns></returns>
        public static DataTable MakeDataTableFromSqlXml(string sqlXml)
        {

            DataSet dataSet = new DataSet();
            dataSet.ReadXml(new StringReader(XElement.Parse(sqlXml).Descendants("Data").FirstOrDefault().ToString()));
            return dataSet.Tables[0];
        }
    }
}
