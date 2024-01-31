using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.InkML;
using hoLinqToSql.LinqUtils;
using hoLinqToSql.LinqUtils.Extensions;
using hoTools.Utils.Clipboard;
using hoTools.Utils.Export;
using hoTools.Utils.General;

namespace hoTools.Utils.Sql
{
    /// <summary>
    /// EA SQL access for
    /// - Query (select ...)
    /// - Update, Insert, Delete
    ///
    /// Limitations:
    /// - EA has no reliable error and exception handling
    /// - In case of errors EA often outputs a message and then simply returns nothing
    /// </summary>
    public class EaSql
    {
        private readonly EA.Repository _rep;

        // JET(.EAP file, MS Access 97 to 2013 format)
        // FIREBIRD
        // ACCESS2007(.accdb file, MS Access 2007+ format)
        // ASA(Sybase SQL Anywhere)
        // SQLSVR(Microsoft SQL Server)
        // MYSQL(MySQL)
        // ORACLE(Oracle)
        // POSTGRES(PostgreSQL)
        // SQLite/SL3
        private readonly string _repType;
        private readonly bool _access;
        public List<string> LogSql = new List<string>();

        private readonly bool _debug;

        // ReSharper disable once UnusedMember.Local
        private readonly int? _sqlTraceLength;

        // Background-worker
        private readonly DoWorkEventArgs _doWorkEventArgs;
        private readonly Func<int, DoWorkEventArgs, bool> _isCancelledProgress;
        // percentage to complete. Used to have more than one task in background
        private readonly int _percentProgressToStart;
        private readonly int _percentProgressToComplete;



        /// <summary>
        /// Get the native SQL String
        /// </summary>
        public string Sql => _sql;
        private string _sql;
        public int RowCount;
        public EA.Repository Rep => _rep;

        /// <summary>
        /// Make an Exception if EA returns no record.
        ///
        /// EA limitations:
        /// - EA has no reliable error and exception handling
        /// - In case of errors EA often outputs a message and then simply returns nothing
        ///
        /// Handle EA limitations:
        /// - Make an exception if EA doesn't returns a row (errorIfEmptyRecord=true)
        /// - You can enable/inhibit the 0 record return Exception by: ErrorIfEmptyRecord=false;
        /// </summary>
        public bool ErrorIfEmptyRecord { get; set; }

        /// <summary>
        /// Constructor EA SQL access
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="debug"></param>
        /// <param name="errorIfEmptyRecord">Make an Exception if no record is returned</param>
        public EaSql(EA.Repository rep, bool debug = false, bool errorIfEmptyRecord = false)
        {
            _rep = rep;
            _repType = _rep.RepositoryType();
            _access = _repType == "JET"; // || _repType.StartsWith("ACCESS");
            _debug = debug;
            ErrorIfEmptyRecord = errorIfEmptyRecord;

        }

        /// <summary>
        /// Constructor EA SQL background access
        /// 
        /// EA limitations:
        /// - EA has no reliable error and exception handling
        /// - In case of errors EA often outputs a message and then simply returns nothing
        /// 
        /// Handle EA limitations:
        /// - Make an exception if EA doesn't returns a row (errorIfEmptyRecord=true)
        /// - You can enable/inhibit the 0 record return Exception by: ErrorIfEmptyRecord=false;
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="debug">Overwrites LogLevel, always log</param>
        /// <param name="errorIfEmptyRecord">If no record returned. throw an error</param>
        /// <param name="isCancelledProgress">Function to cancel and set progress</param>
        /// <param name="doWorkEventArgs"></param>
        /// <param name="percentProgressToStart"></param>
        /// <param name="percentProgressToComplete"></param>
        public EaSql(EA.Repository rep, bool debug, bool errorIfEmptyRecord,
            Func<int, DoWorkEventArgs,
                bool> isCancelledProgress,
            DoWorkEventArgs doWorkEventArgs,
            int percentProgressToStart = 0,            // percent progress to start
            int percentProgressToComplete = 100        // percentage to complete. Used to have more than one task in background
            ) : this(rep,  debug)
        {
            _isCancelledProgress = isCancelledProgress;
            _doWorkEventArgs = doWorkEventArgs;
            _percentProgressToStart = percentProgressToStart;
            _percentProgressToComplete = percentProgressToComplete;
            ErrorIfEmptyRecord = errorIfEmptyRecord;
        }
        /// <summary>
        /// Constructor EA SQL access
        /// </summary>
        /// <param name="rep"></param>
        public EaSql(EA.Repository rep)
        {
            try
            {
                _rep = rep;
                _repType = _rep.RepositoryType();
                _access = _repType == "JET"; // || _repType.StartsWith("ACCESS");
                _debug = false;


            }
            catch (Exception e)
            {
                MessageBox.Show($@"

{e}
", @"Error");
            }

        }

        /// <summary>
        /// Execute SQL and catch Exception
        /// </summary>
        /// <param name="sqlString"></param>
        public bool ExecuteSql(string sqlString)
        {
            try
            {
                sqlString = sqlString.Trim();
                _rep.Execute(sqlString);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"ConnectionString: {_rep.ConnectionString}
SQL:
{sqlString}

{e.GetType().FullName}
{e}", @"SQL execute");
                return false;
            }
        }
        /// <summary>
        /// Run and output Query in EA Find/Search Windows.
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="searchTerm"></param>
        public string RunEaQuery(string sqlString, string searchTerm)
        {

            var xmlResultFromSql = SqlQuery(sqlString);

            // output the query in EA Search Window format
            RowCount = 0;
            var xml = new Xml();
            var res = xml.MakeEaXmlOutput(xmlResultFromSql);
            string xmlEaOutput = res.xmlEaQueryResult;
            RowCount = res.rowCount;

            _rep.RunModelSearch("", "", "", xmlEaOutput);
            return xmlEaOutput;
        }

        /// <summary>
        /// Execute SQL Query and catch Exception
        /// </summary>
        /// <param name="sqlString"></param>
        public string SqlQuery(string sqlString)
        {
            try
            {
                sqlString = SqlMacros.ReplaceSqlDbSpecifics(_rep, sqlString);
                return _rep.SQLQuery(sqlString);
            }
            catch (Exception e)
            {
                MessageBox.Show( $@"ConnectionString: {_rep.ConnectionString}
SQL:
{sqlString}", @"SQL Query");
                return null;
            }
        }
        /// <summary>
        /// Runs SQL select and returns an xml document
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="searchText"></param>
        /// <param name="errorHeader"></param>
        /// <param name="traceEaFileXml"></param>
        /// <param name="traceDtFileXml"></param>
        /// <param name="debug"></param>
        /// <param name="bigData"></param>
        /// <returns></returns>
        public XmlDocument SqlQueryNative(string sqlString, string searchText = "", string errorHeader = "",
            string traceEaFileXml = null,
            string traceDtFileXml = null,
            bool debug = false,
            bool bigData = false)
        {
            sqlString = SqlMacros.ReplaceSqlDbSpecifics(_rep, sqlString);

            // Copy SQL to clipboard if verbose
            if (IsSqlTrace()) ClipboardNoException.SetText(sqlString);
            var eaXml = RunSelectNative(sqlString, errorHeader);

            var results = new XmlDocument();
            results.LoadXml(eaXml);
            return results;

        }

        /// <summary>
        /// Execute SQL Query, Insert, Update and Delete and catch Exception
        /// Returns: null if error detected, SQL string copied to Clipboard
        ///
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="searchText">EA Search Term</param>
        /// <param name="errorHeader"></param>
        /// <param name="traceEaFileXml">File to trace the EA *.xml output of query</param>
        /// <param name="traceDtFileXml">File to trace the Datatable to *.xml</param>
        /// <param name="debug"></param>
        /// <param name="bigData">Optimized for Big Data</param>
        public DataTable SqlQueryDt(string sqlString, string searchText = "", string errorHeader = "",
            string traceEaFileXml = null,
            string traceDtFileXml = null,
            bool debug = false,
            bool bigData = false
            )
        {
            try
            {
                Basic.ApplicationDoEvents();
                // replace templates
                sqlString = SqlMacros.ReplaceSqlDbSpecifics(_rep, sqlString);

                var startTime = DateTime.Now;

                // Copy SQL to clipboard if verbose
                if (IsSqlTrace()) ClipboardNoException.SetText(sqlString);

                _sql = sqlString;

                // check whether select or update, delete, insert sql
                if (Regex.IsMatch(sqlString, @"^\s*select\s+", RegexOptions.IgnoreCase))
                {
                    return RunSqlSelect(sqlString, errorHeader, traceDtFileXml, bigData);
                }
                else
                {
                    return ExecuteSql(sqlString, errorHeader);
                }


            }
            catch (Exception e)
            {
                // error has occurred or an empty string was returned
                ClipboardNoException.SetText(sqlString);
                MessageBox.Show( $@"ConnectionString: {_rep.ConnectionString}
SQL:
See also Clipboard.
{sqlString}

{e}", $@"SQL Query {errorHeader} ");
                return null;
            }
        }
        /// <summary>
        /// Native SQL select
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="errorHeader"></param>
        /// <returns></returns>
        private string RunSelectNative(string sqlString, string errorHeader)
        {
            var startTime = DateTime.Now;
            // Ensure Select is followed by something (no crlf)
            Regex regex = new Regex(@"^\s*select\s+");


            sqlString = regex.Replace(sqlString, "Select ", 1);
            

            string xmlResultFromSql;
            // Run SQL
            try
            {
                xmlResultFromSql = _rep.SQLQuery(sqlString);

            }
            // Be aware: EA often outputs a message and doesn't throw an Exception
            catch (Exception e)
            {
                ClipboardNoException.SetText(sqlString);
                MessageBox.Show( $@"ConnectionString: {_rep.ConnectionString}
SQL:
See also Clipboard.
{sqlString}

", $@"SQL Query {errorHeader} ");
                return null;
            }


            if (IsSqlTrace())
            {
                var lengthText = xmlResultFromSql?.Length > 1023
                    ? $@"{xmlResultFromSql.Length / 1024} kbyte"
                    : $@"{xmlResultFromSql?.Length} byte";
               
            }

            return xmlResultFromSql;
        }
        /// <summary>
        /// Run SQL Selects
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="errorHeader"></param>
        /// <param name="traceDtFileXml"></param>
        /// <param name="bigData"></param>
        /// <returns></returns>
        private DataTable RunSqlSelect(string sqlString, string errorHeader, string traceDtFileXml, bool bigData)
        {
            var xmlResultFromSql = RunSelectNative(sqlString, errorHeader);



            //Basic.SaveTextToXml(traceEaFileXml, resultSql);
            // No results found
            if (xmlResultFromSql == null || !xmlResultFromSql.Contains("DATA"))
            {
                // Empty results: TIMEOUT, EA RPC Server error or other errors
                if (String.IsNullOrEmpty(xmlResultFromSql) || !xmlResultFromSql.Contains("<?xml version='1.0'?>"))
                {
                    MessageBox.Show(
                        $@"ConnectionString: {_rep.ConnectionString}
EA SQL doesn't returns a valid XML (StartType with '<?xml version='1.0'?>')

Possible cause: Timeout, EA RPC Exception (not enough resources?)!

XML-Result from EA query:
'{xmlResultFromSql?.Prefix(_sqlTraceLength ?? 0)}'

SQL (see also Clipboard):
{sqlString}
", $@"SQL EA Error (empty/invalid return) {errorHeader} ");



                    return null;
                }

                // valid result, bit empty
                var res = Xml.GetEmptyDataTable();

                // Check for unexpected 0 count of found records
                if (res.Rows.Count == 0 && ErrorIfEmptyRecord)
                {
                   MessageBox.Show(
                        $@"ConnectionString: {_rep.ConnectionString}
EA SQL doesn't return any record. At least one record was expected.

Possible cause:
- Timeout
- EA RPC Exception (not enough resources?)!
- Wrong repository

XML-Result from EA query:
'{xmlResultFromSql.Prefix(_sqlTraceLength ?? 0)}'

SQL (see also Clipboard):
{sqlString}
", $@"SQL EA Error (empty/invalid return) {errorHeader} ");



                    return null;
                }
                return res;
            }

            try
            {
                DataTable dt;
                if (bigData)
                    dt = Xml.MakeDataTableFromSqlXmlBd(xmlResultFromSql, sqlString, _debug, _rep);
                else
                    dt = Xml.MakeDataTableFromSqlXml(xmlResultFromSql, sqlString, _debug, _rep);

                // trace big date
                if (IsSqlTrace() && String.IsNullOrEmpty(traceDtFileXml))
                {
                    
                }

                return dt;
            }
            catch (Exception e)
            {
                ClipboardNoException.SetText(xmlResultFromSql);
                MessageBox.Show( $@"ConnectionString: {_rep.ConnectionString}
XML-Result from EA query:
See also Clipboard:
{xmlResultFromSql.Prefix(_sqlTraceLength ?? 0)}

SQL:
{sqlString}
", $@"SQL Query converting to DataTable {errorHeader} ");
                return null;
            }
        }

        /// <summary>
        /// Execute SQL
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="errorHeader"></param>
        /// <returns></returns>
        private DataTable ExecuteSql(string sqlString, string errorHeader)
        {
            try
            {
                // Execute Update, Delete
                if (IsSqlTrace())
                {
                   
                }


                sqlString = sqlString.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
                if (sqlString.ToLower().Contains("update") || sqlString.ToLower().Contains("delete"))
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    LogSql.Append(sqlString);
                }

                ExecuteSql(sqlString);
                return null;
            }
            catch (Exception e)
            {
                // error has occurred or an empty string was returned
                ClipboardNoException.SetText(sqlString);
                MessageBox.Show( $@"ConnectionString: {_rep.ConnectionString}
SQL:
See also Clipboard.
{sqlString}
", $@"SQL Update/Delete {errorHeader} ");
                return null;
            }
        }



        /// <summary>
        /// Run SQL Sql statement
        /// - Run SQL (with or without regex, so called advanced SQL)
        /// - Show results in EA Search Windows
        /// - Export to Excel if filename is passed
        /// - Copy SQL to Clipboard (after replacing macros)
        /// 
        /// SQl supports macros (CariadTools, CariadTools)
        /// https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/EaSqlMacros
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="searchText"></param>
        /// <param name="withCopyToClipboard"></param>
        /// <param name="exportFileName"></param>
        public string RunSqlStatement(string sql, string searchText, bool withCopyToClipboard = true, string exportFileName = "")
        {
            float progress = _percentProgressToStart;
            float steps = 5;
            float progressStep = _percentProgressToComplete / steps;

            Basic.ApplicationDoEvents();

            // Report progress
            progress += progressStep;
            if (_doWorkEventArgs != null && _isCancelledProgress((int)progress, _doWorkEventArgs)) return "";

            // used for advanced SQL (with Regex to get special columns)
            var sqlTemplatesCtrl = new SqlMacros(_rep, sql);
            var sqlString = sqlTemplatesCtrl.SqlString;

            sqlString = SqlMacros.ReplaceSqlDbSpecifics(_rep, sqlString);
            // run sql update, etc.
            if (sqlString.Trim().ToLower().StartsWith("select"))
            {
                Basic.ApplicationDoEvents();
                var xmlResultFromSql = _rep.SQLQuery(sqlString);
                Basic.ApplicationDoEvents();

                // Empty result set
                if (String.IsNullOrEmpty(xmlResultFromSql) || !xmlResultFromSql.Contains("Data"))
                {
                    xmlResultFromSql = Xml.EmptyQueryResult();
                }

                string xmlEaOutput;
                // SQL advanced (with Regex)
                if (sqlTemplatesCtrl.IsAdvanced)
                {
                    xmlEaOutput = sqlTemplatesCtrl.PerformRegExpression(xmlResultFromSql);
                }
                else
                {

                    // output the query in EA Search Window format
                    RowCount = 0;
                    var xml = new Xml();
                    var res = xml.MakeEaXmlOutput(xmlResultFromSql);
                    xmlEaOutput = res.xmlEaQueryResult;
                    RowCount = res.rowCount;


                }
                // In Batch generating: Excel export doesn't work
                if (_isCancelledProgress != null)
                {
                    _doWorkEventArgs.Result = new SqlQueryItem(
                        ((SqlQueryItem)_doWorkEventArgs.Argument).Sql,
                        ((SqlQueryItem)_doWorkEventArgs.Argument).SqlTerm,
                        ((SqlQueryItem)_doWorkEventArgs.Argument).SqlFileName,
                        xmlResultFromSql,
                        RowCount,
                        xmlEaOutput);


                }
                else
                {

                    if (!String.IsNullOrEmpty(exportFileName))
                    {
                        var downLoadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                                             @"\Downloads";
                        Excel.MakeExcelFileFromSqlResult(xmlResultFromSql,
                            $@"{downLoadFolder}{Path.GetFileNameWithoutExtension(exportFileName)}.xlsx");

                    }


                    _rep.RunModelSearch("", "", "", xmlEaOutput);

                    if (withCopyToClipboard)
                    {
                        // Copy SQL to Clipboard
                        ClipboardNoException.SetText(sqlString);
                    }
                }
            }
            else
            {
                // Update, Insert, Delete
                Rep.Execute(sqlString);
            }
            // Report progress
            progress = 100;
            if (_doWorkEventArgs != null && _isCancelledProgress((int)progress, _doWorkEventArgs)) return "";


            return sqlString;
        }

        /// <summary>
        /// Run SQL file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="searchTerm"></param>
        /// <param name="withCopyClipboard"></param>
        public void RunSqlFile(string fileName, string searchTerm, bool withCopyClipboard = true)
        {
//            // Execute sql
//            foreach (var p in _settings.SettingsItem.GetPathSqlList)
//            {
//                var filePath = Path.Combine(p, Path.GetFileName(fileName));
//                if (File.Exists(filePath))
//                {
//                    var sql = File.ReadAllText(filePath);
//                    RunSqlStatement(sql, searchTerm, withCopyClipboard);
//                    return;
//                }
//            }

//            MessageBox.Show($@"SQL-File '{fileName}' to run SQL doesn't exists

//Path:  
//- {string.Join($"{Environment.NewLine}- ", _settings.SettingsItem.GetPathSqlList.Select(x => x))}
//Json:  {_settings.SettingsPath}", @"File for SQL not found");
        }

        /// <summary>
        /// Get list of strings from a EA SQL Query
        ///
        /// Handles macros and wild cards
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public List<string> GetListOfStringFromSql(string sql, string columnName)
        {
            // replace templates
            var sqlString = SqlMacros.ReplaceSqlDbSpecifics(_rep, sql);


            List<string> l = new List<string>();
            string str = _rep.SQLQuery(sqlString);
            // Empty
            if (!str.Contains(columnName)) return l;
            XElement xElement = XElement.Parse(str);
            foreach (XElement xEle in xElement.Descendants("Row"))
            {
                l.Add(xEle.Element(columnName).Value);
            }

            return l;
        }
        /// <summary>
        /// Empty Query Result
        /// </summary>
        /// <returns></returns>
        static string EmptyQueryResult()
        {
            return new XDocument(
                new XElement("ReportViewData",
                    new XElement("Fields",
                        new XElement("Field", new XAttribute("name", "Empty"))
                    ),
                    new XElement("Rows",
                        new XElement("Row",
                            new XElement("Field",
                                new XAttribute("name", "Empty"),
                                new XAttribute("value", "__empty___")))

                    )
                )).ToString();
        }
        /// <summary>
        /// escapes a literal string so it can be inserted using sql
        /// </summary>
        /// <param name="sqlString">the string to be escaped</param>
        /// <returns>the escaped string</returns>
        public string EscapeSqlString(string sqlString)
        {
            string escapedString = sqlString;
            switch (_repType)
            {
                case "MYSQL":
                case "POSTGRES":
                    // replace backslash "\" by double backslash "\\"
                    escapedString = escapedString.Replace(@"\", @"\\");
                    break;
            }
            // ALL DBMS types: replace the single quotes "'" by double single quotes "''"
            escapedString = escapedString.Replace("'", "''");
            return escapedString;
        }
        /// <summary>
        /// Replace DataColumnName
        ///
        /// Rational:
        /// - Some Column-names are not allowed
        /// -- Access  ADD, Count
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnNameFrom"></param>
        /// <param name="columnNameTo"></param>
        public void DtColumnReplace(DataTable dt, string columnNameFrom, string columnNameTo)
        {
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == columnNameFrom) { col.ColumnName = columnNameTo; break; }
            }
        }
        /// <summary>
        /// Perform an SQL update
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="updateString"></param>
        /// <returns></returns>
        public static bool SqlUpdate(EA.Repository rep, string updateString)
        {
            try
            {
                rep.Execute(updateString);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Update:\r\n{updateString}\r\n\r\n{e}", @"Error update SQL");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Get list of strings from a EA SQL Query
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static List<string> GetListOfStringFromSql(EA.Repository rep, string sql, string columnName)
        {
            List<string> l = new List<string>();
            string str = rep.SQLQuery(sql);
            // Empty
            if (!str.Contains(columnName)) return l;
            XElement xElement = XElement.Parse(str);
            foreach (XElement xEle in xElement.Descendants("Row"))
            {
                l.Add(xEle.Element(columnName).Value);
            }

            return l;
        }
        /// <summary>
        /// Trace SQL is requested by
        /// _debug
        /// _debugLevel
        /// </summary>
        /// <returns></returns>
        private bool IsSqlTrace()
        {
            return false;
        }



    }
    /// <summary>
    /// SQL Query Item
    /// </summary>
    public readonly struct SqlQueryItem
    {
        public readonly string Sql;
        public readonly string SqlTerm;
        public readonly string SqlFileName;
        /// <summary>
        /// XML result string of query in batch
        /// </summary>
        public readonly string ResXmlOfQuery;

        /// <summary>
        /// Row count of query in batch
        /// </summary>
        public readonly int ResXmlRowCount;
        /// <summary>
        /// EA xml output from EA-Query
        /// </summary>
        public readonly string ResXmlEaOutput;

        /// <summary>
        /// Initialize as result
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlTerm"></param>
        /// <param name="sqlFileName"></param>
        /// <param name="resXmlOfQuery"></param>
        /// <param name="xmlRowCount"></param>
        /// <param name="resXmlEaOutput"></param>
        public SqlQueryItem(string sql, string sqlTerm, string sqlFileName, string resXmlOfQuery, int xmlRowCount, string resXmlEaOutput)
        {
            Sql = sql;
            SqlTerm = sqlTerm;
            SqlFileName = sqlFileName;
            ResXmlOfQuery = resXmlOfQuery;
            ResXmlRowCount = xmlRowCount;
            ResXmlEaOutput = resXmlEaOutput;

        }
        /// <summary>
        /// Initialize as Argument
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlTerm"></param>
        /// <param name="sqlFileName"></param>
        public SqlQueryItem(string sql, string sqlTerm, string sqlFileName)
        {
            Sql = sql;
            SqlTerm = sqlTerm;
            SqlFileName = sqlFileName;
            ResXmlOfQuery = "";
            ResXmlRowCount = 0;
            ResXmlEaOutput = "";

        }
    }
}

