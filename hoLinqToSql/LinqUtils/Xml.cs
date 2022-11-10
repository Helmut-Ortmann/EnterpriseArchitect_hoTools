using hoLinqToSql.LinqUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// 
        /// Remark:   For great tables use MakeDataTableFromSqlXmlBd
        /// Rational: DataSet.ReadXML might split xml in multiple DataTables
        /// </summary>
        /// <param name="sqlXml"></param>
        /// <param name="sql">sql string to output in error message</param>
        /// <param name="debug"></param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static DataTable MakeDataTableFromSqlXml(string sqlXml, string sql = null, bool debug = false, EA.Repository rep = null)
        {
            string sqlText = "";
            if (!String.IsNullOrEmpty(sql)) sqlText = $@"SQL: {sql}{Environment.NewLine}";
            try
            {
                DataSet dataSet = new DataSet();
                var xml = XElement.Parse(sqlXml).Descendants("Data").FirstOrDefault()?.ToString();

                // check if
                if (xml == null)
                {
                    DataTable dt = new DataTable("Empty");
                    dt.Columns.Add("Empty");
                    if (debug && rep != null)
                    {
                        rep.WriteOutput("TRACE", $@"{DateTime.Now} MakeDataTableFromSqlXml: Collect rows and columns, empty '{sqlXml.Prefix(200)}'", 0);
                        rep.EnsureOutputVisible("TRACE");
                    }
                    return dt;
                }
                if (debug && rep != null)
                {
                    rep.WriteOutput("TRACE", $@"{DateTime.Now} MakeDataTableFromSqlXml: Collect rows and columns, '{sqlXml.Prefix(200)}'", 0);
                    rep.EnsureOutputVisible("TRACE");
                }

                dataSet.ReadXml(new StringReader(xml));
                // Read xml makes more than one table from XML, use different solution
                if (dataSet.Tables.Count > 1)
                {
                    return MakeDataTableFromSqlXmlBd(sqlXml, sqlText);
                }
                return dataSet.Tables[0];

            }
            catch (Exception e)
            {
                MessageBox.Show($@"{sqlText} 

Xml:
{sqlXml?.Prefix(500)}

{e}", "Exception MakeDataTableFromSqlXml");
                if (debug) Utils.SaveTextToXml("DebugToXml", sqlXml);
                return GetEmptyDataTable();
            }
            
        }
        /// <summary>
        /// Get an empty DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEmptyDataTable()
        {
            DataTable dt = new DataTable("Empty");
            dt.Columns.Add("Empty");
            return dt;
        }
        /// <summary>
        /// Make EA xml from a DataTable (for column names) and the ordered Enumeration provided by LINQ. Set the Captions in DataTable to ensure column names. 
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rows">LINQ Query for the data to get</param>
        /// <returns></returns>
        public static string MakeXml(DataTable dt, EnumerableRowCollection<DataRow> rows)
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
        /// Make DataTable from EA sql results for Big Data 
        /// 
        /// Remark:   For small tables use MakeDataTableFromSqlXml
        /// Rational: DataSet.ReadXML might split xml in multiple DataTables
        /// </summary>
        /// <param name="sqlXml"></param>
        /// <param name="sql">sql string to output in error message</param>
        /// <param name="debug"></param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static DataTable MakeDataTableFromSqlXmlBd(string sqlXml, string sql = null, bool debug = false, EA.Repository rep = null)
        {
            string sqlText = "";
            if (!String.IsNullOrEmpty(sql)) sqlText = $@"SQL: {sql}{Environment.NewLine}";
            try
            {
                if (debug && rep != null)
                {
                    rep.WriteOutput("TRACE", $@"{DateTime.Now} MakeDataTableFromSqlXmlBd: Calculate columns {sqlXml.Prefix(200)}", 0);
                    rep.EnsureOutputVisible("TRACE");
                }
                // Columns
                var columns = XElement.Parse(sqlXml).Descendants("Row").FirstOrDefault();
                if (columns == null)
                {
                    if (debug) Utils.SaveTextToXml("DebugToXml", sqlXml);
                    return GetEmptyDataTable();
                }

                // create rows, columns in table
                var dt1 = new DataTable();
                foreach (var column in columns.Elements())
                {
                    dt1.Columns.Add(column.Name.ToString(), typeof(string));
                }


                if (debug && rep != null)
                {
                    rep.WriteOutput("TRACE", $@"{DateTime.Now} MakeDataTableFromSqlXmlB: Collect rows and columns, '{sqlXml.Prefix(200)}'", 0);
                    rep.EnsureOutputVisible("TRACE");
                }
                // capture rows and their columns
                var rows = XElement.Parse(sqlXml).Descendants("Row");
                int rowCount = 0;
                foreach (XElement row in rows)
                {
                    List<string> rContent = new List<string>();
                    foreach (var col in row.Descendants())
                    {
                        rContent.Add(col.Value);
                    }
                    dt1.Rows.Add(rContent.ToArray());
                    rContent.Clear();
                    ++rowCount;
                    if (rowCount % 5000 == 0)
                    {
                        if (debug && rep != null)
                        {
                            rep.WriteOutput("TRACE", $@"{DateTime.Now} Calculate rows {rowCount}", 0);
                            rep.EnsureOutputVisible("TRACE");
                        }
                    }
                }
                return dt1;

            }
            catch (Exception e)
            {
                MessageBox.Show($@"{sqlText}

{e}", "Exception MakeDataTableFromSqlXmlBd");
                if (debug) Utils.SaveTextToXml("DebugToXmlBd", sqlXml);
                return GetEmptyDataTable();
            }



        }
        /// <summary>
        /// Make an xml name of the string
        /// - Starts with letter or underscore
        /// - followed by digits or letters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MakeXmlName(string text)
        {
            text = Regex.Replace(text, @"[^A-Za-z0-9_.~]", "_");
            if (!String.IsNullOrEmpty(text) && (Char.IsLetter(text[0]) || text[0] == '_'))
            {
                return text;
            }
            else return $"_{text}";


        }

    }
}
