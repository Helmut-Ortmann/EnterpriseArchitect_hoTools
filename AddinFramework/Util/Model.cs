using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Linq;



namespace EAAddinFramework.Utils
{
    // Represents the current EA Model 
    public class Model
    {
        public EA.Repository Repository { get; set; }
        public EA.App EaApp { get; }
        private static string _applicationFullPath;
        private IWin32Window _mainEAWindow;
        public RepositoryType? _repositoryType;  // a null able type


        /// <summary>
        /// List of databases supported as backend for an EA repository
        /// 0 - MYSQL
        ///	1 - SQLSVR
        /// 2 - ADOJET
        /// 3 - ORACLE
        /// 4 - POSTGRES
        /// 5 - ASA
        /// 7 - OPENEDGE
        /// 8 - ACCESS2007
        /// 9 - FireBird
        /// </summary>
        public enum RepositoryType
        {
            MYSQL,
            SQLSVR,
            ADOJET,
            ORACLE,
            POSTGRES,
            ASA,
            OPENEDGE,
            ACCESS2007,
            FIREBIRD
        }
        #region Constructor
        /// <summary>
        /// Create a model on the first running EA instance
        /// </summary>
        public Model()
        {
            object obj = Marshal.GetActiveObject("EA.App");
            EaApp = obj as EA.App;
            initialize(EaApp.Repository);
        }
        /// <summary>
        /// Create a Model
        /// </summary>
        /// <param name="Repository"></param>
        public Model(EA.Repository Repository)
        {
            initialize(Repository);
        }
        #endregion
        /// <summary>
        /// Initialize an Repository Model object
        /// Intended to use from a scripting environment
        /// </summary>
        /// <param name="Repository"></param>
        public void initialize(EA.Repository Repository)
        {
            this.Repository = Repository; 
        }


        /// <summary>
        /// returns the full path of the running ea.exe
        /// </summary>
        public static string applicationFullPath
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
        public RepositoryType repositoryType
        {
            get
            {
                if (!this._repositoryType.HasValue)
                {
                    this._repositoryType = getRepositoryType();
                }
                return _repositoryType.Value;
            }
        }
        /// <summary>
        /// the main EA window to use when opening properties dialogs
        /// </summary>
        public IWin32Window mainEAWindow
        {
            get
            {
                if //(true)
                    (this._mainEAWindow == null)
                {
                    var allProcesses = new List<Process>(Process.GetProcesses());
                    Process proc = allProcesses.Find(pr => pr.ProcessName == "EA");
                    //if we don't find the process then we set the main window to null
                    if (proc == null
                           || proc.MainWindowHandle == null)
                    {
                        this._mainEAWindow = null;
                    }
                    else
                    {
                        //found it. Create new WindowWrapper
                        this._mainEAWindow = new WindowWrapper(proc.MainWindowHandle);
                    }
                }
                return this._mainEAWindow;
            }
        }
        /// <summary>
        /// Gets the Repository type for this model
        /// </summary>
        /// <returns></returns>
        public RepositoryType getRepositoryType()
        {
            string connectionString = this.Repository.ConnectionString;
            RepositoryType repoType = RepositoryType.ADOJET; //default to .eap file

            // if it is a .feap file then it surely is a firebird db
            if (connectionString.ToLower().EndsWith(".feap"))
            {
                repoType = RepositoryType.FIREBIRD;
            }
            else
            {
                //if it is a .eap file we check the size of it. if less then 1 MB then it is a shortcut file and we have to open it as a text file to find the actual connection string
                if (connectionString.ToLower().EndsWith(".eap"))
                {
                    var fileInfo = new System.IO.FileInfo(connectionString);
                    if (fileInfo.Length > 1000)
                    {
                        //local .eap file, ms access syntax
                        repoType = RepositoryType.ADOJET;
                    }
                    else
                    {
                        //open the file as a text file to find the connection string.
                        var fileStream = new System.IO.FileStream(connectionString, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                        var reader = new System.IO.StreamReader(fileStream);
                        //replace connection string with the file contents
                        connectionString = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                if (!connectionString.ToLower().EndsWith(".eap"))
                {
                    string dbTypeString = "DBType=";
                    int dbIndex = connectionString.IndexOf(dbTypeString) + dbTypeString.Length;
                    if (dbIndex > dbTypeString.Length)
                    {
                        int dbNumber;
                        string dbNumberString = connectionString.Substring(dbIndex, 1);
                        if (int.TryParse(dbNumberString, out dbNumber))
                        {
                            repoType = (RepositoryType)dbNumber;
                        }
                    }
                }
            }
            return repoType;
        }
        public void executeSQL(string SQLString)
        {
            this.Repository.Execute(SQLString);
        }
        /// <summary>
        /// formats an xpath according to the type of database.
        /// For Oracle and Firebird it should be ALL CAPS
        /// </summary>
        /// <param name="xpath">the xpath to format</param>
        /// <returns>the formatted xpath</returns>
        public string formatXPath(string xpath)
        {
            switch (this.repositoryType)
            {

                case RepositoryType.ORACLE:
                case RepositoryType.FIREBIRD:
                    return xpath.ToUpper();
                case RepositoryType.POSTGRES:
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
        public string escapeSQLString(string sqlString)
        {
            string escapedString = sqlString;
            switch (this.repositoryType)
            {
                case RepositoryType.MYSQL:
                case RepositoryType.POSTGRES:
                    // replace backslash "\" by double backslash "\\"
                    escapedString = escapedString.Replace(@"\", @"\\");
                    break;
            }
            // ALL DBMS types: replace the single quotes "'" by double single quotes "''"
            escapedString = escapedString.Replace("'", "''");
            return escapedString;
        }
        /// <summary>
        /// EA SQL Query with:
        /// - formatSQL
        /// - runSQL
        /// - return SQL string as XmlDocument
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns>XmlDocuement</returns>
        public XmlDocument SQLQuery(string sqlQuery)
        {
            var results = new XmlDocument();
            results.LoadXml(SQLQueryNative(sqlQuery));
            return results;
        }
        /// <summary>
        /// Run EA SQL Query with Exception handling
        /// - return null if Exception
        /// . return "" if nothing found
        /// - return xml string if ok
        /// </summary>
        /// <param name="query"></param>
        /// <returns>string</returns>
        public string SqlQueryWithException(string query)
        {
            try
            {
                // run the query
                string xml = SQLQueryNative(query);
                // nothing found
                if (!xml.Contains("Row")) return null;
                return xml;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SQL:\r\n{query}\r\n{ex.Message}", "Error SQL");
                return null;
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
        public string SQLQueryNative(string sqlQuery)
        {
            sqlQuery = this.formatSQL(sqlQuery);
            return this.Repository.SQLQuery(sqlQuery);
        }

        /// <summary>
        /// Make EA XML output format from EA SQLQuery format (string)
        /// </summary>
        /// <param name="x"></param>
        /// <returns>string</returns>
        public string MakeEaXmlOutput(string x)
        {
            return MakeEaXmlOutput(XDocument.Parse(x));
        }


        /// <summary>
        /// Make EA XML output format from EA SQLQuery XDocument format (Linq to XML)
        /// </summary>
        /// <param name="x">Output from EA SQLQuery</param>
        /// <returns></returns>

        #pragma warning disable CSE0003 // Use expression-bodied members
        public string MakeEaXmlOutput(XDocument x)
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
        }
        #pragma warning restore CSE0003 // Use expression-bodied members

        /// <summary>
        /// Make EA XML output format from EA SQLQuery XDocument format (Linq to XML)
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
            string sqlObjectType = "";
            foreach (var field in fields)
            {

                string fieldName = field.Name.ToString();
                switch (fieldName) {
                    case "CLASSGUID":
                        guid = field.Value;
                        continue;
                    case "CLASSTYPE":
                        // valid class type
                        sqlObjectType = field.Value;
                        EA.ObjectType eaObjectType;
                        object eaObject = Repository.GetEaObject(sqlObjectType, guid, out eaObjectType);
                        if (eaObject == null)
                        {
                            MessageBox.Show($"CLASSTYPE='{sqlObjectType}' GUID='{guid}' ObjectType={eaObjectType}", "Couldn't find EA item, Break!!!");
                            return null;
                        }
                        eaItemList.Add(new EaItem(guid, sqlObjectType, eaObjectType, eaObject));
                        guid = "";
                        sqlObjectType = "";
                        continue;

                    default:
                        MessageBox.Show($"Column'{field.Value}' not expected! (expected CLASSGUID or CLASSTYPE)", "Invalid SQL results, column not expected");
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
        private string formatSQL(string sqlQuery)
        {
            sqlQuery = replaceSQLWildCards(sqlQuery);
            sqlQuery = formatSQLTop(sqlQuery);
            sqlQuery = formatSQLFunctions(sqlQuery);
            return sqlQuery;
        }

        /// <summary>
        /// Operation to translate SQL functions in there equivalents in different sql syntaxes
        /// supported functions:
        /// 
        /// - lcase -> lower in T-SQL (SQLSVR and ASA)
        /// </summary>
        /// <param name="sqlQuery">the query to format</param>
        /// <returns>a query with translated functions</returns>
        private string formatSQLFunctions(string sqlQuery)
        {
            string formattedSQL = sqlQuery;
            //lcase -> lower in T-SQL (SQLSVR and ASA and Oracle and FireBird)
            if (this.repositoryType == RepositoryType.SQLSVR ||
                this.repositoryType == RepositoryType.ASA ||
                   this.repositoryType == RepositoryType.ORACLE ||
                   this.repositoryType == RepositoryType.FIREBIRD ||
                   this.repositoryType == RepositoryType.POSTGRES)
            {
                formattedSQL = formattedSQL.Replace("lcase(", "lower(");
            }
            return formattedSQL;
        }

        /// <summary>
        /// limiting the number of results in an sql query is different on different platforms.
        /// 
        /// "SELECT TOP N" is used on
        /// SQLSVR
        /// ADOJET
        /// ASA
        /// OPENEDGE
        /// ACCESS2007
        /// 
        /// "WHERE rowcount <= N" is used on
        /// ORACLE
        /// 
        /// "LIMIT N" is used on
        /// MYSQL
        /// POSTGRES
        /// 
        /// This operation will replace the SELECT TOP N by the appropriate sql syntax depending on the repositorytype
        /// </summary>
        /// <param name="sqlQuery">the sql query to format</param>
        /// <returns>the formatted sql query </returns>
        private string formatSQLTop(string sqlQuery)
        {
            string formattedQuery = sqlQuery;
            string selectTop = "select top ";
            int begintop = sqlQuery.ToLower().IndexOf(selectTop);
            if (begintop >= 0)
            {
                int beginN = begintop + selectTop.Length;
                int endN = sqlQuery.ToLower().IndexOf(" ", beginN) + 1;
                if (endN > beginN)
                {
                    string N = sqlQuery.ToLower().Substring(beginN, endN - beginN);
                    string selectTopN = sqlQuery.Substring(begintop, endN);
                    switch (this.repositoryType)
                    {
                        case RepositoryType.ORACLE:
                            // remove "top N" clause
                            formattedQuery = formattedQuery.Replace(selectTopN, "select ");
                            // find where clause
                            string whereString = "where ";
                            int beginWhere = formattedQuery.ToLower().IndexOf(whereString);
                            string rowcountCondition = "rownum <= " + N + " and ";
                            // add the rowcount condition
                            formattedQuery = formattedQuery.Insert(beginWhere + whereString.Length, rowcountCondition);
                            break;
                        case RepositoryType.MYSQL:
                        case RepositoryType.POSTGRES:
                            // remove "top N" clause
                            formattedQuery = formattedQuery.Replace(selectTopN, "select ");
                            string limitString = " limit " + N;
                            // add limit clause
                            formattedQuery = formattedQuery + limitString;
                            break;
                        case RepositoryType.FIREBIRD:
                            // in Firebird top becomes first
                            formattedQuery = formattedQuery.Replace(selectTopN, selectTopN.Replace("top", "first"));
                            break;
                    }
                }
            }
            return formattedQuery;
        }
        /// <summary>
        /// replace the wild cards in the given sql query string to match either MSAccess or ANSI syntax
        /// </summary>
        /// <param name="sqlQuery">the sql string to edit</param>
        /// <returns>the same sql query, but with its wild cards replaced according to the required syntax</returns>
        private string replaceSQLWildCards(string sqlQuery)
        {
            bool msAccess = this.repositoryType == RepositoryType.ADOJET;
            int beginLike = sqlQuery.IndexOf("like", StringComparison.InvariantCultureIgnoreCase);
            if (beginLike > 1)
            {
                int beginString = sqlQuery.IndexOf("'", beginLike + "like".Length, StringComparison.CurrentCulture);
                if (beginString > 0)
                {
                    int endString = sqlQuery.IndexOf("'", beginString + 1);
                    if (endString > beginString)
                    {
                        string originalLikeString = sqlQuery.Substring(beginString + 1, endString - beginString);
                        string likeString = originalLikeString;
                        if (msAccess)
                        {
                            likeString = likeString.Replace('%', '*');
                            likeString = likeString.Replace('_', '?');
                            likeString = likeString.Replace('^', '!');
                        }
                        else
                        {
                            likeString = likeString.Replace('*', '%');
                            likeString = likeString.Replace('?', '_');
                            likeString = likeString.Replace('#', '_');
                            likeString = likeString.Replace('^', '!');
                        }
                        string next = string.Empty;
                        if (endString < sqlQuery.Length)
                        {
                            next = replaceSQLWildCards(sqlQuery.Substring(endString + 1));
                        }
                        sqlQuery = sqlQuery.Substring(0, beginString + 1) + likeString + next;

                    }
                }
            }
            return sqlQuery;
        }
        /// <summary>
        /// returns true if security is enabled in this model
        /// </summary>
        public bool isSecurityEnabled
        {
            get
            {
                try
                {
                    this.Repository.GetCurrentLoginUser();
                    return true;
                }
                catch (System.Runtime.InteropServices.COMException e)
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
        public List<WorkingSet> workingSets
        {
            get
            {
                var workingSetList = new List<WorkingSet>();
                string getWorkingSets = "select d.docid, d.DocName,d.Author from t_document d where d.DocType = 'WorkItem' order by d.Author, d.DocName";
                XmlDocument workingSets = this.SQLQuery(getWorkingSets);
                foreach (XmlNode workingSetNode in workingSets.SelectNodes("//Row"))
                {
                    string name = string.Empty;
                    string id = string.Empty;
                    string ownerFullName = string.Empty;
                    foreach (XmlNode subNode in workingSetNode.ChildNodes)
                    {
                        switch (subNode.Name.ToLower())
                        {
                            case "docid":
                                id = subNode.InnerText;
                                break;
                            case "docname":
                                name = subNode.InnerText;
                                break;
                            case "author":
                                ownerFullName = subNode.InnerText;
                                break;
                        }
                    }
                    User owner = this.users.Find(u => u.fullName.Equals(ownerFullName, StringComparison.InvariantCultureIgnoreCase));
                    workingSetList.Add(new WorkingSet(this, id, owner, name));
                }
                return workingSetList;
            }
        }
        /// <summary>
        /// all users defined in this model
        /// </summary>
        public List<User> users
        {
            get
            {

                var userList = new List<User>();
                if (this.isSecurityEnabled)
                {
                    string getUsers = "select u.UserLogin, u.FirstName, u.Surname from t_secuser u";
                    XmlDocument users = this.SQLQuery(getUsers);
                    foreach (XmlNode userNode in users.SelectNodes("//Row"))
                    {
                        string login = string.Empty;
                        string firstName = string.Empty;
                        string lastName = string.Empty;
                        foreach (XmlNode subNode in userNode.ChildNodes)
                        {
                            switch (subNode.Name.ToLower())
                            {
                                case "userlogin":
                                    login = subNode.InnerText;
                                    break;
                                case "firstname":
                                    firstName = subNode.InnerText;
                                    break;
                                case "surname":
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
                    XmlDocument users = this.SQLQuery(getUsers);
                    foreach (XmlNode authorNode in users.SelectNodes(formatXPath("//author")))
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

    }
}
