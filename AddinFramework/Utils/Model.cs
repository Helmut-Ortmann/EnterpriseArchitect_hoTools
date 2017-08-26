using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
//using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using hoTools.Utils.Configuration;
using hoTools.Utils.Excel;
using hoLinqToSql.LinqUtils;


using hoTools.Utils.SQL;
using System.IO;
using hoTools.Utils;
using Microsoft.Office.Interop.Excel;


namespace EAAddinFramework.Utils
{
    // Represents the current EA Model 
    public class Model
    {
        public EA.Repository Repository { get; set; }
        public EA.App EaApp { get; }
        static string _applicationFullPath;
        IWin32Window _mainEaWindow;
        UtilSql.RepositoryType? _repositoryType; // a null able type

        // configuration as singleton
        readonly HoToolsGlobalCfg _globalCfg = HoToolsGlobalCfg.Instance;


        

        #region Constructor

        /// <summary>
        /// Create a model on the first running EA instance
        /// </summary>
        public Model()
        {
            object obj = Marshal.GetActiveObject("EA.App");
            EaApp = obj as EA.App;
            Initialize(EaApp.Repository);
        }

        /// <summary>
        /// Create a Model
        /// </summary>
        /// <param name="repository"></param>
        public Model(EA.Repository repository)
        {
            Initialize(repository);
        }

        #endregion

        /// <summary>
        /// Initialize an rep Model object
        /// Intended to use from a scripting environment
        /// </summary>
        /// <param name="rep"></param>
        public void Initialize(EA.Repository rep)
        {
            Repository = rep;
        }


        /// <summary>
        /// returns the full path of the running ea.exe
        /// </summary>
        public static string ApplicationFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationFullPath))
                {
                    Process[] processes = Process.GetProcessesByName("EA");
                    if (processes.Length > 0)
                    {
                        _applicationFullPath = processes[0].MainModule.FileName;
                    }
                }
                return _applicationFullPath;
            }
        }

        /// <summary>
        /// returns the type of repository backend.
        /// This is mostly needed to adjust to sql to the specific sql dialect
        /// </summary>
        public UtilSql.RepositoryType RepositoryType
        {
            get
            {
                if (!_repositoryType.HasValue)
                {
                    _repositoryType = UtilSql.GetRepositoryType(Repository);
                }
                return _repositoryType.Value;
            }
        }

        /// <summary>
        /// the main EA window to use when opening properties dialogs
        /// </summary>
        public IWin32Window MainEaWindow
        {
            get
            {
                if //(true)
                    (_mainEaWindow == null)
                {
                    var allProcesses = new List<Process>(Process.GetProcesses());
                    Process proc = allProcesses.Find(pr => pr.ProcessName == "EA");
                    //if we don't find the process then we set the main window to null
                    if (proc == null
                        || proc.MainWindowHandle == null)
                    {
                        this._mainEaWindow = null;
                    }
                    else
                    {
                        //found it. Create new WindowWrapper
                        _mainEaWindow = new WindowWrapper(proc.MainWindowHandle);
                    }
                }
                return _mainEaWindow;
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
                Repository.Execute(sqlString);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"SQL execute:\r\n{sqlString}\r\n{e.Message}", @"Error SQL execute");
                return false;
            }
        }

        /// <summary>
        /// formats an xpath according to the type of database.
        /// For Oracle and Firebird it should be ALL CAPS
        /// </summary>
        /// <param name="xpath">the xpath to format</param>
        /// <returns>the formatted xpath</returns>
        public string FormatXPath(string xpath)
        {
            switch (RepositoryType)
            {

                case UtilSql.RepositoryType.Oracle:
                case UtilSql.RepositoryType.Firebird:
                    return xpath.ToUpper();
                case UtilSql.RepositoryType.Postgres:
                    return xpath.ToLower();
                default:
                    return xpath;
            }
        }

        /// <summary>
        /// escapes a literal string so it can be inserted using sql
        /// </summary>
        /// <param name="sqlString">the string to be escaped</param>
        /// <returns>the escaped string</returns>
        public string EscapeSqlString(string sqlString)
        {
            string escapedString = sqlString;
            switch (RepositoryType)
            {
                case UtilSql.RepositoryType.MySql:
                case UtilSql.RepositoryType.Postgres:
                    // replace backslash "\" by double backslash "\\"
                    escapedString = escapedString.Replace(@"\", @"\\");
                    break;
            }
            // ALL DBMS types: replace the single quotes "'" by double single quotes "''"
            escapedString = escapedString.Replace("'", "''");
            return escapedString;
        }

        /// <summary>
        /// Runs the search (hoTools SQL file, EA search or LINQ Search). It handles the exceptions.
        /// It converts wild cards of the &lt;Search Term>. 
        /// - First search for SQL-File
        /// - Search the LINQ Search if LINQ is supported
        /// - If no SQL file found run EA Search
        /// </summary>
        /// <param name="searchName">EA Search name or SQL file name (uses path to find absolute path)</param>
        /// <param name="searchTerm"></param>
        /// <param name="exportToExcel"></param>
        public string SearchRun(string searchName, string searchTerm, bool exportToExcel=false)
        {
            searchName = searchName.Trim();
            if (searchName == "") return "";

            
            // SQL file?
            if (_globalCfg.ReadSqlFile(searchName, withErrMessage: false) != "")
               
            {
                string sqlString = _globalCfg.ReadSqlFile(searchName);

                // run search
                searchTerm = UtilSql.ReplaceSqlWildCards(Repository, searchTerm, RepositoryType);
                return SqlRun(searchName, sqlString, searchTerm, exportToExcel);


            }
            string linqPadFileName = _globalCfg.GetLinqPadQueryFileName(searchName);
            if (linqPadFileName != "")
            {
                LinqPad linqPad = new LinqPad(_globalCfg.LprunPath, _globalCfg.TempFolder, "html");
                linqPad.Run(linqPadFileName, "html", linqPad.GetArg(Repository, searchTerm));
                System.Data.DataTable dtHtml = linqPad.ReadHtml();

                // Make EA xml
                string xml = Xml.MakeXmlFromDataTable(dtHtml);
                // Output to EA
                Repository.RunModelSearch("", "", "", xml);
                return "";

            }
            else
            {
                // EA Search
                try
                {
                    // run SQL search and display in Search Window
                    Repository.RunModelSearch(searchName, searchTerm, "", "");
                    return "";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(),
                        $@"Error start search '{searchName} {searchTerm}'");
                    return "";
                }
            }
        }


        /// <summary>
        /// Run an SQL string and if query output the result in EA Search Window. If update, insert, delete execute SQL.
        /// It return "" for nothing found or the SQL result string.
        /// <para/>- replacement of macros
        /// <para/>- run query
        /// <para/>- format to output
        /// </summary>
        /// <param name="sqlName"></param>
        /// <param name="sql"></param>
        /// <param name="searchText">Search Text to replace 'Search Term' macro</param>
        /// <returns>"" for nothing found or the EA SQL XML string with the found information</returns>
        public string SqlRun(string sqlName, string sql, string searchText, bool exportToExcel=false)
        {
            // replace templates
            sql = SqlTemplates.ReplaceMacro(Repository, sql, searchText);
            if (String.IsNullOrWhiteSpace(sql)) return "";

            // check whether select or update, delete, insert sql
            if (Regex.IsMatch(sql, @"^\s*select ", RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                // run the SQL select query
                var xmlSqlQueryResult = SqlQueryWithException(sql) ?? "";

                // output the query in EA Search Window
                string xmlEaOutput = MakeEaXmlOutput(xmlSqlQueryResult);
                if (exportToExcel)
                    Excel.MakeExcelFileFromSqlResult(xmlSqlQueryResult,
                        @"d:\temp\sql\" + Path.GetFileNameWithoutExtension(sqlName) + ".xlsx");
                Repository.RunModelSearch("", "", "", xmlEaOutput);
                return xmlSqlQueryResult;
            }
            else
            {
                // run the update, delete, insert sql
                bool ret = SqlExecuteWithException(sql);
                // if ok output the SQL
                if (ret)
                {
                    string sqlText =
                        $"Path SQL:\r\n{SqlError.GetHoToolsLastSqlFilePath()}\r\n\r\n{SqlError.ReadHoToolsLastSql()}";
                    MessageBox.Show(sqlText, @"SQL executed!\r\n\r\nCtrl+C to copy it to clipboard (ignore beep).");

                }
                return "";

            }
        }



        /// <summary>
        /// EA SQL Query with:
        /// - formatSQL
        /// - runSQL
        /// - return SQL string as XmlDocument
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns>XmlDocuement</returns>
        public XmlDocument SqlQuery(string sqlQuery)
        {
            var results = new XmlDocument();
            results.LoadXml(SqlQueryNative(sqlQuery));
            return results;
        }

        /// <summary>
        /// Run EA SQL Query with Exception handling. It deletes the error file and reads it back to detect errors.
        /// <para/>return null if error, it also displays the error message in MessageBox
        /// <para/>return "" if nothing found
        /// <para/>return xml string if ok
        /// </summary>
        /// <param name="query"></param>
        /// <returns>string</returns>
        public string SqlQueryWithException(string query)
        {
            // delete an existing error file
            SqlError.DeleteEaSqlError();
            try
            {
                // is query or a changing SQL?
                //if (Regex.IsMatch(query, "^\s*select ", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                // run the query (select * .. from
                string xml = SqlQueryNative(query);
                if (!SqlError.ExistsEaSqlErrorFile())
                {
                    return xml;
                }
                else
                {
                    MessageBox.Show(SqlError.ReadEaSqlError(), @"Error SQL (CTRL+C to copy it!!)");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"SQL:\r\n{query}\r\n{ex.Message}", @"Error SQL");
                return null;
            }
        }

        /// <summary>
        /// Run EA SQL Query with Exception handling
        /// <para/>return null if error, it also displays the error message in MessageBox
        /// <para/>return "" if nothing found
        /// <para/>return xml string if ok
        /// </summary>
        /// <returns>string</returns>
        public bool SqlExecuteWithException(string sql)
        {
            // delete an existing error file
            SqlError.DeleteEaSqlError();
            try
            {
                // run the query (select * .. from
                sql = FormatSql(sql);
                // store final/last query which is executed
                SqlError.WriteHoToolsLastSql(sql);

                Repository.Execute(sql);
                if (!SqlError.ExistsEaSqlErrorFile())
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(SqlError.ReadEaSqlError(), @"Error SQL (CTRL+C to copy it!!)");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"SQL:\r\n{sql}\r\n{ex.Message}", @"Error SQL");
                return false;
            }


        }




        /// <summary>
        /// EA SQL Query native with:
        /// - formatSQL
        /// - runSQL
        /// - return SQL string
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns>string</returns>
        public string SqlQueryNative(string sqlQuery)
        {
            sqlQuery = FormatSql(sqlQuery);
            // store final/last query which is executed
            SqlError.WriteHoToolsLastSql(sqlQuery);

            return Repository.SQLQuery(sqlQuery); // no error, only no result rows
        }

        /// <summary>
        /// EA Execute SQL:
        /// - formatSQL
        /// - runSQL
        /// - return SQL string
        /// </summary>
        /// <returns>false=error</returns>
        public bool SqlExecuteNative(string sqlExecute)
        {
            sqlExecute = FormatSql(sqlExecute);
            // store final/last query which is executed
            SqlError.WriteHoToolsLastSql(sqlExecute);

            return ExecuteSql(sqlExecute); // no error, only no result rows
        }

        /// <summary>
        /// Make EA Excel File from EA SQLQuery format (string)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        public bool MakeExcelFile(string x, string fileName = @"d:\temp\sql\sql.xlsx")
        {

            if (string.IsNullOrEmpty(x)) return false;
            XDocument xDoc = XDocument.Parse(x);

            // get field names
            var fieldNames = xDoc.Descendants("Row").FirstOrDefault()?.Descendants();
            if (fieldNames == null) return false;

            TryToDeleteFile(fileName);
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                //Excel.Application xlApp = new Excel.Application();


                object misValue = System.Reflection.Missing.Value;

                //var xlWorkBook = xlApp.Workbooks.Add(misValue);
                //var xlWorkSheet = (Excel.Worksheet) xlWorkBook.Worksheets.Item[1];

                int column = 1;
                int row = 1;
                foreach (var field in fieldNames)
                {
                    //xlWorkSheet.Cells[row, column] = field.Name.ToString();
                    column = column + 1;
                }


                var xRows = xDoc.Descendants("Row");
                foreach (var xRow in xRows)
                {
                    Console.WriteLine(@"New Record found");
                    row = row + 1;
                    column = 1;
                    foreach (var value in xRow.Elements())
                    {
                        //xlWorkSheet.Cells[row, column] = value.Value;
                        column = column + 1;

                    }
                }
                //xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                //    Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();

                // release objects
                //Marshal.ReleaseComObject(xlWorkSheet);
                //Marshal.ReleaseComObject(xlWorkBook);
                //Marshal.ReleaseComObject(xlApp);
                Cursor.Current = Cursors.Default;
                var ret = MessageBox.Show($@"Yes: Open Excel File\r\nNo: Open Folder\r\nCancel",
                    $"Excel File '{fileName}' created!", MessageBoxButtons.YesNoCancel);
                switch (ret)
                {
                        case DialogResult.Yes:
                            Util.StartFile(fileName);
                            break;
                        case DialogResult.No:
                            Util.ShowFolder(Path.GetDirectoryName(fileName));
                        break;
                        default:
                        break;

                }
                return true;
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($@"Error {e.Message}\r\n{e.Source}");
                return false;
            }
        }

       



        #region MakeEaXmlOutput
        /// <summary>
        /// Make EA XML output format from EA SQLQuery format (string)
        /// </summary>
        /// <param name="x"></param>
        /// <returns>string</returns>
        public string MakeEaXmlOutput(string x)
        {
            if (string.IsNullOrEmpty(x) ) return EmptyQueryResult();
            return MakeEaXmlOutput(XDocument.Parse(x));
        }

        /// <summary>
        /// Make EA XML output format from EA SQLQuery XDocument format (LINQ to XML). If nothing found or an error has occurred nothing is displayed.
        /// </summary>
        /// <param name="x">Output from EA SQLQuery</param>
        /// <returns></returns>
        #pragma warning disable CSE0003 // Use expression-bodied members
        private string MakeEaXmlOutput(XDocument x)
        {
            //---------------------------------------------------------------------
            // make the output format:
            // From Query:
            //<EADATA><Dataset_0>
            // <Data>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            // </Data>
            //</Dataset_0><EADATA>
            //
            //-----------------------------------
            // To output EA XML:
            //<ReportViewData>
            // <Fields>
            //   <Field name=""/>
            //   <Field name=""/>
            // </Fields>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            //</reportViewData>
            try
            {
                return new XDocument(
                    new XElement("ReportViewData",
                        new XElement("Fields",
                               from field in x.Descendants("Row").FirstOrDefault().Descendants()
                               select new XElement("Field", new XAttribute("name", field.Name))
                        ),
                        new XElement("Rows",
                                    from row in x.Descendants("Row")
                                    select new XElement(row.Name,
                                           from field in row.Nodes()
                                           select new XElement("Field", new XAttribute("name", ((XElement)field).Name),
                                                                        new XAttribute("value", ((XElement)field).Value)))

                    )
                )).ToString();
            } catch (Exception )
            {
                // empty query result
                return EmptyQueryResult();

            }
        }
        #region Empty Query Result
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
            #endregion
        }
#pragma warning restore CSE0003 // Use expression-bodied members
        #endregion

        /// <summary>
        /// Make EA XML output format from EA SQLQuery XDocument format (LINQ to XML)
        /// </summary>
        /// <param name="x">Output from EA SQLQuery</param>
        /// <returns></returns>
        public List<EaItem> MakeEaItemListFromQuery(XDocument x)
        {
            //---------------------------------------------------------------------
            // make the output format:
            // From Query:
            //<EADATA><Dataset_0>
            // <Data>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            //  <Row>
            //    <Name1>value1</name1>
            //    <Name2>value2</name2>
            //  </Row>
            // </Data>
            //</Dataset_0><EADATA>
            //
            //-----------------------------------
            // To output EA XML:
            //<ReportViewData>
            // <Fields>
            //   <Field name=""/>
            //   <Field name=""/>
            // </Fields>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            // <Rows>
            //   <Row>
            //      <Field name="" value=""/>
            //      <Field name="" value=""/>
            // </Rows>
            //</reportViewData>

            List<EaItem> eaItemList = new List<EaItem>();
            var fields = from row in x.Descendants("Row").Descendants()
                     where row.Name == "CLASSGUID" ||
                           row.Name == "CLASSTYPE"     // 'Class','Action','Diagram', 
                     select row;
            string guid = "";
            foreach (var field in fields)
            {

                string fieldName = field.Name.ToString();
                switch (fieldName) {
                    case "CLASSGUID":
                        guid = field.Value;
                        continue;
                    case "CLASSTYPE":
                        // valid class type
                        var sqlObjectType = field.Value;
                        EA.ObjectType eaObjectType;
                        object eaObject = Repository.GetEaObject(sqlObjectType, guid, out eaObjectType);
                        if (eaObject == null)
                        {
                            MessageBox.Show($@"CLASSTYPE='{sqlObjectType}' GUID='{guid}' ObjectType={eaObjectType}", @"Couldn't find EA item, Break!!!");
                            return null;
                        }
                        eaItemList.Add(new EaItem(guid, sqlObjectType, eaObjectType, eaObject));
                        guid = "";
                        continue;

                    default:
                        MessageBox.Show($@"Column'{field.Value}' not expected! (expected CLASSGUID or CLASSTYPE)", @"Invalid SQL results, column not expected");
                        return null;
                }
            }

            return eaItemList;


        }

        /// <summary>
        /// sets the correct wild cards depending on the database type.
        /// changes '%' into '*' if on ms access
        /// and _ into ? on msAccess
        /// </summary>
        /// <param name="sqlQuery">the original query</param>
        /// <returns>the fixed query</returns>
        private string FormatSql(string sqlQuery)
        {
            sqlQuery = UtilSql.ReplaceSqlWildCards(Repository, sqlQuery, RepositoryType);
            sqlQuery = FormatSqlTop(sqlQuery);
            sqlQuery = FormatSqlFunctions(sqlQuery);
            sqlQuery = FormatSqldBspecific(sqlQuery); // DB specifics like #DB=ORACLE#.... #DB=ORACLE#
            return sqlQuery.Trim();
        }

        /// <summary>
        /// Operation to translate SQL functions in there equivalents in different sql syntaxes
        /// supported functions:
        /// 
        /// - lcase -> lower in T-SQL (SqlSvr and Asa)
        /// </summary>
        /// <param name="sqlQuery">the query to format</param>
        /// <returns>a query with translated functions</returns>
        private string FormatSqlFunctions(string sqlQuery)
        {
            string formattedSql = sqlQuery;
            //lcase -> lower in T-SQL (SqlSvr and Asa and Oracle and FireBird)
            if (RepositoryType == UtilSql.RepositoryType.SqlSvr ||
                RepositoryType == UtilSql.RepositoryType.Asa ||
                RepositoryType == UtilSql.RepositoryType.Oracle ||
                RepositoryType == UtilSql.RepositoryType.Firebird ||
                RepositoryType == UtilSql.RepositoryType.Postgres)
            {
                formattedSql = formattedSql.Replace(@"lcase(", "lower(");
            }
            return formattedSql;
        }
        //"SELECT TOP N" is used on
        // SqlSvr
        // AdoJet
        // Asa
        // OPENEDGE
        // ACCESS2007
        // 
        // "WHERE rowcount <= N" is used on
        // ORACLE
        // 
        // "LIMIT N" is used on
        // MySql
        // POSTGRES
        // <summary>
        // limiting the number of results in an sql query is different on different platforms.
        // 
        /// <summary>
        /// This operation will replace the SELECT TOP N by the appropriate sql syntax depending on the repository type
        /// </summary>
        /// <param name="sqlQuery">the sql query to format</param>
        /// <returns>the formatted sql query </returns>
        string FormatSqlTop(string sqlQuery)
        {
            string formattedQuery = sqlQuery;
            string selectTop = "select top ";
            int begintop = sqlQuery.ToLower().IndexOf(selectTop, StringComparison.CurrentCulture);
            if (begintop >= 0)
            {
                int beginN = begintop + selectTop.Length;
                int endN = sqlQuery.ToLower().IndexOf(" ", beginN, StringComparison.Ordinal) + 1;
                if (endN > beginN)
                {
                    string N = sqlQuery.ToLower().Substring(beginN, endN - beginN);
                    string selectTopN = sqlQuery.Substring(begintop, endN);
                    switch (RepositoryType)
                    {
                        case UtilSql.RepositoryType.Oracle:
                            // remove "top N" clause
                            formattedQuery = formattedQuery.Replace(selectTopN, "select ");
                            // find where clause
                            string whereString = "where ";
                            int beginWhere = formattedQuery.ToLower().IndexOf(whereString, StringComparison.CurrentCulture);
                            string rowcountCondition = @"rownum <= " + N + " and ";
                            // add the row count condition
                            formattedQuery = formattedQuery.Insert(beginWhere + whereString.Length, rowcountCondition);
                            break;
                        case UtilSql.RepositoryType.MySql:
                        case UtilSql.RepositoryType.Postgres:
                            // remove "top N" clause
                            formattedQuery = formattedQuery.Replace(selectTopN, "select ");
                            string limitString = " limit " + N;
                            // add limit clause
                            formattedQuery = formattedQuery + limitString;
                            break;
                        case UtilSql.RepositoryType.Firebird:
                            // in Firebird top becomes first
                            formattedQuery = formattedQuery.Replace(selectTopN, selectTopN.Replace("top", "first"));
                            break;
                    }
                }
            }
            return formattedQuery;
        }
        /// <summary>
        /// Format DB specific by removing unnecessary DB specific string parts.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //#DB=Asa#                DB specif SQL for Asa
        //#DB=FIREBIRD#           DB specif SQL for FIREBIRD
        //#DB=JET#                DB specif SQL for JET
        //#DB=MySql#              DB specif SQL for My SQL
        //#DB=ACCESS2007#         DB specif SQL for ACCESS2007
        //#DB=ORACLE#             DB specif SQL for Oracle
        //#DB=POSTGRES#           DB specif SQL for POSTGRES
        //#DB=SqlSvr#             DB specif SQL for SQL Server
        string FormatSqldBspecific(string sql) {
            // available DBs
            var dbs = new Dictionary<UtilSql.RepositoryType, string>()
            {
                { UtilSql.RepositoryType.Access2007, "#DB=ACCESS2007#" },
                { UtilSql.RepositoryType.Asa, "#DB=Asa#" },
                { UtilSql.RepositoryType.Firebird, "#DB=FIREBIRD#" },
                { UtilSql.RepositoryType.AdoJet, "#DB=JET#" },
                { UtilSql.RepositoryType.MySql, "#DB=MySql#" },
                { UtilSql.RepositoryType.Oracle, "#DB=ORACLE#" },
                { UtilSql.RepositoryType.Postgres, "#DB=POSTGRES#" },
                { UtilSql.RepositoryType.SqlSvr, "#DB=SqlSvr#" },
            };
            UtilSql.RepositoryType dbType = UtilSql.GetRepositoryType(Repository);
            string s = sql;
            foreach (var curDb in dbs )
            {
                if (curDb.Key != dbType)
                {   // delete not used DBs
                    string delete = $"{curDb.Value}.*?{curDb.Value}";
                    s = Regex.Replace(s, delete, "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                }
                
            }
            // delete remaining DB identifying string
            s = Regex.Replace(s, @"#DB=(Asa|FIREBIRD|JET|MySql|ORACLE|ACCESS2007|POSTGRES|SqlSvr)#", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            
            // delete multiple empty lines
            for (int i = 0;  i < 4;i++)
            {
                s = Regex.Replace(s, "\r\n\r\n", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
            return s;
        }


        
        /// <summary>
        /// returns true if security is enabled in this model
        /// </summary>
        public bool IsSecurityEnabled
        {
            get
            {
                try
                {
                    Repository.GetCurrentLoginUser();
                    return true;
                }
                catch (COMException e)
                {
                    if (e.Message == "Security not enabled")
                    {
                        return false;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// The working sets defined in this model
        /// </summary>
        public List<WorkingSet> WorkingSets
        {
            get
            {
                var workingSetList = new List<WorkingSet>();
                string getWorkingSets = "select d.docid, d.DocName,d.Author from t_document d where d.DocType = 'WorkItem' order by d.Author, d.DocName";
                XmlDocument workingSets = SqlQuery(getWorkingSets);
                foreach (XmlNode workingSetNode in workingSets.SelectNodes("//Row"))
                {
                    string name = string.Empty;
                    string id = string.Empty;
                    string ownerFullName = string.Empty;
                    foreach (XmlNode subNode in workingSetNode.ChildNodes)
                    {
                        switch (subNode.Name.ToLower())
                        {
                            case @"docid":
                                id = subNode.InnerText;
                                break;
                            case @"docname":
                                name = subNode.InnerText;
                                break;
                            case @"author":
                                ownerFullName = subNode.InnerText;
                                break;
                        }
                    }
                    User owner = Users.Find(u => u.FullName.Equals(ownerFullName, StringComparison.InvariantCultureIgnoreCase));
                    workingSetList.Add(new WorkingSet(this, id, owner, name));
                }
                return workingSetList;
            }
        }
        /// <summary>
        /// all users defined in this model
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public List<User> Users
        {
            get
            {

                var userList = new List<User>();
                if (IsSecurityEnabled)
                {
                    string getUsers = "select u.UserLogin, u.FirstName, u.Surname from t_secuser u";
                    XmlDocument users = SqlQuery(getUsers);
                    foreach (XmlNode userNode in users.SelectNodes("//Row"))
                    {
                        string login = string.Empty;
                        string firstName = string.Empty;
                        string lastName = string.Empty;
                        foreach (XmlNode subNode in userNode.ChildNodes)
                        {
                            switch (subNode.Name.ToLower())
                            {
                                case @"userlogin":
                                    login = subNode.InnerText;
                                    break;
                                case @"firstname":
                                    firstName = subNode.InnerText;
                                    break;
                                case @"surname":
                                    lastName = subNode.InnerText;
                                    break;
                            }
                        }
                        userList.Add( new User(this, login, firstName, lastName));
                    }
                }
                else
                {
                    //security not enabled. List of all users is the list of all authors mentioned in the t_object table.
                    string getUsers = "select distinct o.author from t_object o";
                    XmlDocument users = SqlQuery(getUsers);
                    foreach (XmlNode authorNode in users.SelectNodes(FormatXPath("//author")))
                    {
                        string login = authorNode.InnerText;
                        string firstName = string.Empty;
                        string lastName = string.Empty;
                        //add user	
                        userList.Add(new User(this, login, firstName, lastName));
                    }
                }
                return userList;
            }
        }
        /// <summary>
        /// Expose Clipboard.Clear()
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void ClipboardClear()
        {
            Clipboard.Clear();
        }
        /// <summary>
        /// Expose Clipboard.SetText()
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public string ClipboardGetText()
        {
            string s = Clipboard.GetText();
            return s;
        }
        /// <summary>
        /// Expose Clipboard.SetText()
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public void ClipboardSetText(object text)
        {
            if (String.IsNullOrWhiteSpace((string)text)) Clipboard.Clear();
            else Clipboard.SetText((string)text);
        }
        /// <summary>
        /// Exposes ClipboardGetText(TestDataFormat);
        /// <para/> 0=TextDataFormat.CommaSeparatedValue;
        /// <para/>1=TextDataFormat.Html;
        /// <para/>2=TextDataFormat.Rtf;
        /// <para/>3=TextDataFormat.Text;
        /// <para/>4=TextDataFormat.UnicodeText;
        /// </summary>
        /// <param name="textDataType"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public string ClipboardGetText(int textDataType)
        {
            switch (textDataType)
            {
                case 0:
                    return Clipboard.GetText(TextDataFormat.CommaSeparatedValue);
                case 1:
                    return Clipboard.GetText(TextDataFormat.Html);
                case 2:
                    return Clipboard.GetText(TextDataFormat.Rtf);
                case 3:
                    return Clipboard.GetText(TextDataFormat.Text);
                case 4: 
                    return Clipboard.GetText(TextDataFormat.UnicodeText);
                default:
                    MessageBox.Show($@"Valid Clipboard Get (TextDataFormat):0-4, was '{textDataType}'", @"Invalid Clipboard Parameter Clipboard.GetText(TextDataFormat");
                    return "";
            } 

        }

        /// <summary>
        /// Exposes ClipboardSetText(text, TestDataFormat);
        /// <para/> 0=TextDataFormat.CommaSeparatedValue;
        /// <para/>1=TextDataFormat.Html;
        /// <para/>2=TextDataFormat.Rtf;
        /// <para/>3=TextDataFormat.Text;
        /// <para/>4=TextDataFormat.UnicodeText;
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textDataType"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public void ClipboardSetText(object text, object textDataType)
        {
            int dataType = (int) textDataType;
            switch (dataType)
            {
                case 0:
                    Clipboard.SetText((string)text, TextDataFormat.CommaSeparatedValue);
                    return;
                case 1:
                    Clipboard.SetText((string)text, TextDataFormat.Html);
                    return;
                case 2:
                    Clipboard.SetText((string)text, TextDataFormat.Rtf);
                    return;
                case 3:
                    Clipboard.SetText((string)text, TextDataFormat.Text);
                    return;
                case 4:
                    Clipboard.SetText((string)text, TextDataFormat.UnicodeText);
                    return;
                default:
                    MessageBox.Show($@"Valid Clipboard Set (TextDataFormat):0-4, was '{textDataType}'", @"Invalid Clipboard Parameter Clipboard.SetText(text, TextDataFormat");
                    return;
            }

        }
        /// <summary>
        /// Delete file with error handling
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static bool TryToDeleteFile(string fileName)
        {
            try
            {
                // A.
                // Try to delete the file.
                if (File.Exists(fileName)) File.Delete(fileName);
                return true;
            }
            catch (IOException)
            {
                // B.
                // We could not delete the file.
                return false;
            }
        }


    }
    
}
