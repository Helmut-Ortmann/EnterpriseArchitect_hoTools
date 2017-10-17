using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace hoLinqToSql.LinqUtils
{
    public class Xml
    {
        /// <summary>
        /// Make ea xml from data table. The xml is ready for out put by 'repository.RunModelSearch("", "", "", xml);' 
        /// If DataTable is empty it returns the empty EA xml string.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string MakeXmlFromDataTable(DataTable dt)
        {
            if (dt == null) return MakeEmptyXml();
            // Make EA xml
            OrderedEnumerableRowCollection<DataRow> rowsDt = from row in dt.AsEnumerable()
                orderby row.Field<string>(dt.Columns[0].Caption)
                select row;
            return MakeXml(dt, rowsDt);
        }
        /// <summary>
        /// Returns an Empty query result
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MakeEmptyXml(string name = "Empty")
        {

            XElement x = new XElement("ReportViewData",
                new XElement("Fields",
                    new XElement("Field", new XAttribute("name", "Empty"))));
            return x.ToString();
        }



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
            var xml = XElement.Parse(sqlXml).Descendants("Data")?.FirstOrDefault()?.ToString();
            if (xml == null)
            {
                DataTable dt = new DataTable("Empty");
                dt.Columns.Add("Empty");
                return dt;
            }
            dataSet.ReadXml(new StringReader(xml));
            return dataSet.Tables[0];
        }
    }
}
