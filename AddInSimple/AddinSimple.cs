using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using AddInSimple.EABasic;
using AddInSimple.Utils;
using hoLinqToSql.LinqUtils;

using LinqToDB.Configuration;
using LinqToDB.DataProvider;


//------------------------------------------------------------------------------------
// AddInSimple Simple Add-in
//------------------------------------------------------------------------------------
// Shows:
// - Create a little menu
// - React to user input
// - Two functions to call from Shape Script
// -- GetValueForShape()
// -- GetParentProperty()
// - Setup Project to install Add-In with an *.msi File
// -- See Project: AddInSimpleSetup
//
// Remarks:
// Add-Ins are Windows COM Objects which EA integrates in its visual and runtime environment. The Add-In may:
// - Have a standard menu to interact
// - Have a full EA view for easy interaction with the user (Shown in EA Add-In Window or as standalone view)
// - React to EA Events.
// - The Add-In has to obey the COM rules and has to register it's COM dll in Windows (this main dll and all view dll (shown in EA Add-In Window)). Other helpers don't need registration.
// - Add features like:
// -- Shape Script print (a simple public method which returns a string)
// -- Searches (a simple public method that returns a XML string to visualize by EA in the Search Window)
//
// Deployment
// - Registry entry to tell EA that there is an AddIn to integrate
// -- Key:    HKEY_CURRENT_USER\SOFTWARE\Sparx Systems\EAAddins\
// -- Value:  AddInSimple.AddInSimpleClass  (The COM ProgID, usually <Namespace>.<Classname>), case sensitive
// -- Same value as: [ProgId("AddInSimple.AddInSimpleClass")]
// - Install DLL as COM object
// -- Make COM Visible
// -- Register as COM
// -- Advice: Use WiX toolset (see AddInSimpleSetup Project)
//
// Debug (Visual Studio)
// -  Ensure Registry entry 'HKEY_CURRENT_USER\SOFTWARE\Sparx Systems\EAAddins\ 'AddInSimple.AddInSimpleClass'
// - Parametrize DEBUG Mode: COM Visible, COM register, Enable Unsafe code DEBUG
//
// Credit: 
// - Geert Bellekens
// - https://bellekens.com/2011/01/29/tutorial-create-your-first-c-enterprise-architect-addin-in-10-minutes/
namespace AddInSimple
{
    // Make sure Project Properties for Release has the Entry: 'Register for COM interop'
    // You may check registration with: https://exploringea.com/2015/11/18/ea-installation-inspectorv2/
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("58E7B70F-16C4-4538-A4E8-AF4EAC27519B")]
    // ProgID is the same as the string to register for EA in 'Sparx Systems:EAAddins:AddInSimple:Default'
    // In description: 'Namespace.ClassName'
    // EA uses always the ProId.
    [ProgId("AddInSimple.AddInSimpleClass")]
    public sealed class AddInSimpleClass : EaAddInBase
    {
        // define menu constants
        const string MenuName = "-&AddInSimple";
        const string MenuHello = "&Say Hello";
        const string MenuGoodbye = "&Say Goodbye";
        const string MenuOpenProperties = "&Open Properties";
        const string MenuRunDemoSearch = "&DemoSearch";
        const string MenuRunDemoPackageContent = "&DemoSearchPackageContent";
        const string MenuRunDemoSqlToDataTable = "DemoSqlToDataTable";
        const string MenuShowConnectionString = "DemoConnectionString";
        const string MenuShowRunLinq2Db = "DemoRunLinq2dbQuery";
        const string MenuShowRunLinq2DbAdvanced = "DemoRunLinq2dbQueryAdvanced";
        const string MenuShowRunLinq2DbToHtml = "DemoRunLinq2dbQueryToHtml";
        const string MenuShowRunLinqXml = "DemoRunLinqXmlAllOwnQueries";

        // remember if we have to say hello or goodbye
        private bool _shouldWeSayHello = true;
        
        /// <summary>
        /// constructor where we set the menu header and menuOptions
        /// </summary>
        public AddInSimpleClass()
        {
            menuHeader = MenuName;
            menuOptions = new[] { MenuHello, MenuGoodbye, MenuOpenProperties, MenuRunDemoSearch,
                MenuRunDemoPackageContent, MenuRunDemoSqlToDataTable, MenuShowConnectionString, MenuShowRunLinq2Db, MenuShowRunLinq2DbAdvanced,
                MenuShowRunLinq2DbToHtml,
                MenuShowRunLinqXml};
        }
        // ReSharper disable once RedundantOverriddenMember
        /// <summary>
        /// EA_Connect events enable Add-Ins to identify their type and to respond to Enterprise Architect start up.
        /// This event occurs when Enterprise Architect first loads your Add-In. Enterprise Architect itself is loading at this time so that while a Repository object is supplied, there is limited information that you can extract from it.
        /// The chief uses for EA_Connect are in initializing global Add-In data and for identifying the Add-In as an MDG Add-In.
        /// Also look at EA_Disconnect.
        /// </summary>
        /// <param name="repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <returns>String identifying a specialized type of Add-In: 
        /// - "MDG" : MDG Add-Ins receive MDG Events and extra menu options.
        /// - "" : None-specialized Add-In.</returns>
        public override string EA_Connect(EA.Repository repository)
        {
            return base.EA_Connect(repository);
        }
        /// <summary>
        /// Called once Menu has been opened to see what menu items should active.
        /// </summary>
        /// <param name="repository">the repository</param>
        /// <param name="location">the location of the menu</param>
        /// <param name="menuName">the name of the menu</param>
        /// <param name="itemName">the name of the menu item</param>
        /// <param name="isEnabled">boolean indicating whether the menu item is enabled</param>
        /// <param name="isChecked">boolean indicating whether the menu is checked</param>
        public override void EA_GetMenuState(EA.Repository repository, string location, string menuName, string itemName, ref bool isEnabled, ref bool isChecked)
        {
            if (IsProjectOpen(repository))
            {
                switch (itemName)
                {
                    // define the state of the hello menu option
                    case MenuHello:
                        isEnabled = _shouldWeSayHello;
                        break;
                    // define the state of the goodbye menu option
                    case MenuGoodbye:
                        isEnabled = !_shouldWeSayHello;
                        break;
                    case MenuOpenProperties:
                        isEnabled = true;
                        break;

                    // Test Add-In Search
                    case MenuRunDemoSearch:
                        isEnabled = true;
                        break;

                    // Test Add-In Search output package elements
                    case MenuRunDemoPackageContent:
                        isEnabled = true;
                        break;

                    // Test Run SQL and transform to DataTable
                    case MenuRunDemoSqlToDataTable:
                        isEnabled = true;
                        break;

                    // Show connection string, copy connection string to database and try to establish a connection with ADODB
                    case MenuShowConnectionString:
                        isEnabled = true;
                        break;

                    // Test Run Linq2db Query
                    case MenuShowRunLinq2Db:
                        isEnabled = true;
                        break;

                    // Test Run Linq2db Query
                    case MenuShowRunLinq2DbAdvanced:
                        isEnabled = true;
                        break;

                    // Test Run Linq2db Query
                    case MenuShowRunLinq2DbToHtml:
                        isEnabled = true;
                        break;

                    // Test Run Linq from XML for own EA Queries
                    case MenuShowRunLinqXml:
                        isEnabled = true;
                        break;


                    // there shouldn't be any other, but just in case disable it.
                    default:
                        isEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                isEnabled = false;
            }
        }

        /// <summary>
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        /// </summary>
        /// <param name="repository">the repository</param>
        /// <param name="location">the location of the menu</param>
        /// <param name="menuName">the name of the menu</param>
        /// <param name="itemName">the name of the selected menu item</param>
        public override void EA_MenuClick(EA.Repository repository, string location, string menuName, string itemName)
        {
            string xml;
            DataTable dt;

            // for LINQ to SQL
            IDataProvider provider;  // the provider to connect to database like Access, ..
            string connectionString; // The connection string to connect to database

            switch (itemName)
            {
                // user has clicked the menuHello menu option
                case MenuHello:
                    this.SayHello();
                    break;
                // user has clicked the menuGoodbye menu option
                case MenuGoodbye:
                    this.SayGoodbye();
                    break;


                case MenuOpenProperties:
                    this.testPropertiesDialog(repository);
                    break;
                
                // Test the Search and output the results to EA Search Window
                case MenuRunDemoSearch:
                    // 1. Collect data
                    dt = SetTable();
                    // 2. Order, Filter, Join, Format to XML
                    xml = QueryAndMakeXmlFromTable(dt);
                    // 3. Out put to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;

                case MenuRunDemoPackageContent:
                    // 1. Collect data into a data table
                    dt = SetTableFromContext(repository);
                    // 2. Order, Filter, Join, Format to XML
                    xml = QueryAndMakeXmlFromTable(dt);
                    // 3. Out put to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;
                
                    // Example to run SQL, convert to DataTable and output in EA Search Window
                case MenuRunDemoSqlToDataTable:
                    // 1. Run SQL
                    string sql = "select ea_guid AS CLASSGUID, object_type AS CLASSTYPE, name, stereotype, object_type from t_object order by name";
                    xml = repository.SQLQuery(sql);
                    // 2. Convert to DataTable
                    dt = Util.MakeDataTableFromSqlXml(xml);
                    // 2. Order, Filter, Join, Format to XML
                    xml = QueryAndMakeXmlFromTable(dt);
                    // 3. Out put to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;

                
                    // Read connection string from EA and try an ADODB Connection
                    // Copy connection string to clipboard
                case MenuShowConnectionString:
                    string eaConnectionString = repository.ConnectionString;
                    if (eaConnectionString != null)
                    {
                        connectionString = LinqUtil.GetConnectionString(repository, out provider);

                        string lConnectionString = $@"{eaConnectionString}\r\n\r\nProvider for Linq for SQL:\r\n'{provider}\r\n{connectionString}";
                        Clipboard.SetText(lConnectionString);
                        MessageBox.Show($"{lConnectionString}", "Connection string (EA+LINQ + SQL) copied to clipboard");
                        if (connectionString == "") return;


                        ADODB.Connection conn = new ADODB.Connection();
                        try
                            {
                            conn.Open(connectionString, "", "", -1);  // connection Open synchronously
                            
                            //conn.Open(connectionString, "", "", -1);  // connection Open synchronously
                                MessageBox.Show($@"EA ConnectionString:    '{eaConnectionString}'
ConnectionString:
- '{connectionString}'
Provider:
-  '{provider}'
Mode:             
- '{conn.Mode}' 
State:
- '{conn.State}'", "ODBC Connection established ");
                            }
                                catch (Exception e)
                            {
                                MessageBox.Show($@"EA ConnectionString:    '{eaConnectionString}'
ConnectionString:
- '{connectionString}'
Mode:             
- '{conn.Mode}' 
State:
- '{conn.State}'

{ e}", "ODBC Connection error ");
                            }
                    }
                    break;

                // Basis LINQ to SQL example
                case MenuShowRunLinq2Db:
                    // get connection string of repository
                    connectionString = LinqUtil.GetConnectionString(repository, out provider);
                    
                    // Run LINQ query to dataTable
                    dt = LinqUtil.RunLinq2Db(provider, connectionString);
                    // Make EA xml
                    OrderedEnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
                        orderby row.Field<string>(dt.Columns[0].Caption) 
                        select row;
                    xml = Util.MakeXml(dt, rows);

                    // Output to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;

                // Advanced LINQ to SQL example
                case MenuShowRunLinq2DbAdvanced:
                    // get connection string of repository
                    connectionString = LinqUtil.GetConnectionString(repository, out provider);

                    // Run LINQ query to dataTable
                    dt = LinqUtil.RunLinq2DbAdvanced(provider, connectionString);

                    // Make EA xml
                    xml = Util.MakeXmlFromDataTable(dt);
                    // Output to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;

                // run LINQPad query to HTML (uses lprun.exe)
                // - lprun installed at standard location (c:\Program Files (x86)\LINQPad5\lprun.exe)
                // - output to 'c:\temp\EaBasicQuery.html'
                // - EA standard installation (used for EAExample database)
                case MenuShowRunLinq2DbToHtml:
                    // Run query with lprun.exe 
                    string parametersToPassToQuery = @"""Test query EaBasicQuery.linq""";
                    string linqQueryFileName = "EaBasicQuery.linq";
                    string linqQueryFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + linqQueryFileName;

                    string linqLPRun = @"c:\Program Files (x86)\LINQPad5\lprun.exe";
                    string targetHtml = @"c:\temp\EaBasicQuery.html";
                    // Command for lprun.exe (see http://www.linqpad.net/lprun.aspx)
                    // -format=html
                    // -format=csv
                    // -format=text
                    string arg = $@"-lang=program -format=html {linqQueryFilePath}  parametersToPassToQuery ";

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = linqLPRun;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.Arguments = arg;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;

                    try
                    {
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            //* Read the output (or the error)
                            string output = exeProcess.StandardOutput.ReadToEnd();
                            string err = exeProcess.StandardError.ReadToEnd();
                            exeProcess.WaitForExit();
                            // Retrieve the app's exit code
                            int exitCode = exeProcess.ExitCode;
                            File.WriteAllText(targetHtml, output);

                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Query:{linqQueryFilePath}\r\nLPRun.exe{linqLPRun}\r\nTarget:{targetHtml}{e}", " Error running LINQ query");
                    }

                    System.Diagnostics.Process.Start(targetHtml);





                    break;


                // run LINQ XML query for own EA queries which are stored in *.xml
                case MenuShowRunLinqXml:
                    // Make DataTable with LINQ Search/Query
                    dt = EaSearches();

                    // Make 
                    xml = Util.MakeXmlFromDataTable(dt);

                    // Output to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;

                    




            }
        }
        /// <summary>
        /// Parse all local defined Filter and Searches from %APPDATA%Sparx Systems\EA\Search Data\EA_Search.xml
        /// </summary>
        /// <returns></returns>
        private DataTable EaSearches()
        {
            XDocument xelement;
            string filePath = $@"c:\Users\{Environment.UserName}\AppData\Roaming\Sparx Systems\EA\Search Data\EA_Search.xml";
            try
            {
                string t = System.IO.File.ReadAllText(filePath);
                xelement = XDocument.Parse(t);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Tried to xml parse '{filePath}'\r\n{e}","Cant read EA_Search.xml");
                return null;
            }

            // SQL Searches
            var sql = from el in xelement.Descendants("Search")
                where el.Attribute("CustomSearch").Value == "1"
                where el.Descendants("RootTable").First().Attribute("Filter") != null
                orderby (string)el.Attribute("Name")
                //Type="0" LnksToObj="0" CustomSearch="1" AddinAndMethodName=""
                select new
                {
                    QueryName = el.Attribute("Name").Value,
                    Type = "SQL",
                    Sql = el.Descendants("RootTable").First().Attribute("Filter").Value.Substring(0, 100)
                };
            // EA Filter
            var eaFilter = from el in xelement.Descendants("Search")
                where el.Attribute("CustomSearch").Value == "0"
                where el.Attribute("AddinAndMethodName").Value == ""
                orderby (string)el.Attribute("Name")
                //Type="0" LnksToObj="0" CustomSearch="1" AddinAndMethodName=""
                select new
                {
                    QueryName = el.Attribute("Name").Value,
                    Type = "Filter",
                    Sql = ""
                };
            // All Add-In Searches
            var addIn = from el in xelement.Descendants("Search")
                where el.Attribute("CustomSearch").Value == "0"
                where el.Attribute("AddinAndMethodName").Value != ""
                orderby (string)el.Attribute("Name")
                //Type="0" LnksToObj="0" CustomSearch="1" AddinAndMethodName=""
                select new
                {
                    QueryName = el.Attribute("Name").Value,
                    Type = "Add-In",
                    Sql = el.Attribute("AddinAndMethodName").Value
                };
            // Concatenate queries
            var sum = addIn.Concat(sql).Concat(eaFilter).OrderBy(n => n.QueryName);
            return sum.ToDataTable();

        }

        /// <summary>
        /// Called when EA start model validation. Just shows a message box
        /// </summary>
        /// <param name="repository">the repository</param>
        /// <param name="args">the arguments</param>
        public override void EA_OnStartValidation(EA.Repository repository, object args)
        {
            MessageBox.Show("Validation started");
        }
        /// <summary>
        /// Called when EA ends model validation. Just shows a message box
        /// </summary>
        /// <param name="repository">the repository</param>
        /// <param name="args">the arguments</param>
        public override void EA_OnEndValidation(EA.Repository repository, object args)
        {
            MessageBox.Show("Validation ended");
        }
        
        /// <summary>
        /// Say Hello to the world
        /// </summary>
        private void SayHello()
        {
            MessageBox.Show("Hello World");
            this._shouldWeSayHello = false;
        }

        /// <summary>
        /// Say Goodbye to the world
        /// </summary>
        private void SayGoodbye()
        {
            MessageBox.Show("Goodbye World");
            this._shouldWeSayHello = true;
        }

        private void testPropertiesDialog(EA.Repository repository)
        {
            int diagramId = repository.GetCurrentDiagram().DiagramID;
            // there is no current diagram
            if (diagramId == 0) return;
            repository.OpenDiagramPropertyDlg(diagramId);
        }


        /// <summary>
        /// Example for Addin which can be used from a ShapeScript (Origin: Aaron Bell, SPARX System)
        /// The script adds for every entry in theParams (array of Parameters):
        /// 'Stereotype' the Stereotype
        /// 'Alias'      the Alias
        /// 
        /// shape main
        ///{
        ///    //Draw the rect
        ///    rectangle(0,0,100,100);
        ///    //Replace CS_AddinFramework with your addin name and GetValueForShape with the function name you wish to call in your addin.
        ///    print("#ADDIN:CS_AddinFramework, GetValueForShape, Stereotype, Alias#");
        ///}
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="eaGuid"></param>
    /// <param name="theParams"></param>
    /// <returns></returns>
    public string GetValueForShape(EA.Repository repository, string eaGuid, object theParams)
        {
            //Convert the parameters passed in into a string array.
            string[] args = (string[])theParams;

            //Get the element calling this addin
            EA.Element element = repository.GetElementByGuid(eaGuid);
            string ret = "";
            //Create the name modified element name
            if (args.Length > 0 && element != null)
            {
                ret += element.Name;

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "Stereotype")
                        ret += " " + element.Stereotype;

                    if (args[i] == "Alias")
                        ret += " " + element.Alias;
                }
            }

            return ret;
        }
        /// <summary>
        /// GetParentProperties:
        /// - TAG=TagName
        /// - NAME
        /// - ALIAS
        /// - STEREOTYPE
        /// - TYPE
        /// - COMPLEXITY
        /// - VERSION
        /// - PHASE
        /// - Language
        /// - Filename
        /// - AUTHOR
        /// - STATUS
        /// - KEYWORDS
        /// - GUID
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="eaGuid"></param>
        /// <param name="theParams"></param>
        /// <returns></returns>
        public string GetParentProperty(EA.Repository repository, string eaGuid, object theParams)
        {
            //Convert the parameters passed in into a string array.
            string[] args = (string[])theParams;

            //Get the element calling this addin
            EA.Element element = repository.GetElementByGuid(eaGuid);
            string ret = "";
            if (element.ParentID == 0) return "";
            EA.Element el = repository.GetElementByID(element.ParentID);
            if (el == null) return "";
            if (args.Length != 1) return "";
            string type = args[0].Split('=')[0].ToUpper();
            string tagName;
            string[] par;
            switch (type)
            {
                case "KEYWORDS":
                    ret = el.Tag;
                    break;
                case "ALIAS":
                    ret = el.Alias;
                    break;
                case "NAME":
                    ret = el.Name;
                    break;
                case "STEREOTYPE":
                    ret = el.Stereotype;
                    break;
                case "Type":
                    ret = el.Type;
                    break;
                case "COMPLEXITY":
                    ret = el.Complexity;
                    break;
                case "VERSION":
                    ret = el.Version;
                    break;
                case "PHASE":
                    ret = el.Phase;
                    break;
                case "Language":
                    ret = el.Gentype;
                    break;

                case "Status":
                    ret = el.Status;
                    break;

                case "RUNSTATE":
                    ret = el.RunState;
                    break;

                case "TAG":
                     par = args[0].Split('=');
                     if (par.Length != 2) return "";
                     tagName = par[1];
                     foreach (EA.TaggedValue tag in el.TaggedValuesEx)
                     {
                         if (tagName == tag.Name)
                         {
                             ret = tag.Value;
                             break;
                         }
                     }

                    break;

                // Test result for a test
                case "TEST":
                    par = args[0].Split('=');
                    if (par.Length != 2) return "";
                    tagName = par[1];
                    foreach (EA.Test test in el.Tests)
                    {
                        if (tagName == test.Name)
                        {
                            ret = test.TestResults;
                            break;
                        }
                    }

                    break;

                // Role for a Resource
                case "RESOURCE":
                    par = args[0].Split('=');
                    if (par.Length != 2) return "";
                    tagName = par[1];
                    foreach (EA.Resource resource in el.Resources)
                    {
                        if (tagName == resource.Name)
                        {
                            ret = resource.Role;
                            break;
                        }
                    }

                    break;

                // Issues 
                case "ISSUE":
                    par = args[0].Split('=');
                    if (par.Length != 2) return "";
                    tagName = par[1];
                    foreach (EA.Issue issue in el.Issues)
                    {
                        if (tagName == issue.Name)
                        {
                            ret = issue.Reporter;
                            break;
                        }
                    }

                    break;

                // Role for a Resource
                case "RISK":
                    par = args[0].Split('=');
                    if (par.Length != 2) return "";
                    tagName = par[1];
                    foreach (EA.Risk risk in el.Risks)
                    {
                        if (tagName == risk.Name)
                        {
                            ret = risk.Weight.ToString();
                            break;
                        }
                    }

                    break;

            }

            return ret;
        }
        /// <summary>
        /// Add-In Search: Sample
        /// See: http://sparxsystems.com/enterprise_architect_user_guide/13.5/automation/add-in_search.html
        /// 
        /// how it's works:
        /// 1. Create a Table and fill it with your code
        /// 2. Adapt LINQ to output the table (powerful)
        ///    -- Where to select only certain rows
        ///    -- Order By to order the result set
        ///    -- Grouping
        ///    -- Filter
        ///    -- JOIN
        ///    -- etc.
        /// 3. Deploy and test 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="searchText"></param>
        /// <param name="xmlResults"></param>
        /// <returns></returns>
        public object AddInSearchSample(EA.Repository repository, string searchText, out string xmlResults)
        {
            // 1. Collect data into a data table
            DataTable dt = SetTable();
            // 2. Order, Filter, Join, Format to XML
            xmlResults = QueryAndMakeXmlFromTable(dt);
            return "ok";
        }

        /// <summary>
        /// Add-In Search: Sample
        /// See: http://sparxsystems.com/enterprise_architect_user_guide/13.5/automation/add-in_search.html
        /// 
        /// How it's works:
        /// 1. Create a Table and fill it with your code
        /// 2. Adapt LINQ to output the table (powerful)
        ///    -- Where to select only certain rows
        ///    -- Order By to order the result set
        ///    -- Grouping
        ///    -- Filter
        ///    -- JOIN
        ///    -- etc.
        /// 3. Deploy and test 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="searchText"></param>
        /// <param name="xmlResults"></param>
        /// <returns></returns>
        public object AddInSearchSamplePackageContent(EA.Repository repository, string searchText, out string xmlResults)
        {
            // 1. Collect data into a data table
            DataTable dt = SetTableFromContext(repository);
            // 2. Order, Filter, Join, Format to XML
            xmlResults = QueryAndMakeXmlFromTable(dt);
            return "ok";
        }


        /// <summary>
        /// Set DataTable with test data
        /// </summary>
        /// <returns></returns>
        private DataTable SetTable()
        {
            // Here we create a DataTable with three columns.
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Sex", typeof(string));
            table.Columns.Add("Address", typeof(string));

            // Here we add five DataRows.
            table.Rows.Add("Helmut", "male", "Hamburg");
            table.Rows.Add("Daniel", "male", "London");
            table.Rows.Add("Sofy", "female", "Cologne");
            table.Rows.Add("Lena", "female", "Petersburg");
            table.Rows.Add("Joker", "unknown", "??????");
            return table;

        }
        /// <summary>
        /// Set DataTable with test data
        /// </summary>
        /// <returns></returns>
        private DataTable SetTableFromContext(EA.Repository rep)
        {
            // Here we create a more realistic DataTable with:
            // - CLASSGUID for easy navigation within EA Model Search Window
            // - CLASSTYPE for easy navigation within EA Model Search Window
            DataTable table = new DataTable();
            table.Columns.Add("CLASSGUID", typeof(string));
            table.Columns.Add("CLASSTYPE", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Stereotype", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Alias", typeof(string));

            // Switch according to context object type
            object oContext;
            EA.ObjectType type = rep.GetContextItem(out oContext);
            switch (type)
            {
                 case EA.ObjectType.otPackage:
                     EA.Package pkg = (EA.Package) oContext;
                     foreach (EA.Element el in pkg.Elements)
                     {
                         table.Rows.Add(el.ElementGUID, type,  el.Name, el.Stereotype, el.Type, el.Alias);
                     }
                     break;
            }
            return table;

        }

        
        /// <summary>
        /// Test Query to show making EA xml from a Data table by using MakeXml. It queries the data table, orders the content according to Name columen and outputs it in EA xml format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string QueryAndMakeXmlFromTable(DataTable dt)
        {
            try
            {
                // Make a LINQ query (WHERE, JOIN, ORDER,)
                OrderedEnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
                    orderby row.Field<string>("Name") descending
                    select row;

                return Util.MakeXml(dt, rows);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}", "Error LINQ query Test query to show Table to EA xml format");
                return "";

            }
        }

        
        // DRIVER=Firebird/InterBase(r) driver;d:\temp\codeGeneration.feap; User =SYSDBA;Password=masterkey
        // DRIVER=Firebird/InterBase(r) driver;DBNAME=Database=d:\temp\codeGeneration.feap; User =SYSDBA;Password=masterkey;
        //if (eaConnectionString.ToLower().EndsWith(".feap"))
        //if (rep.RepositoryType() == "JET")
        


       

    }
   

    /// <summary>
    /// Some stuff I don't really understand
    /// </summary>
    public class InternalHelpers
    {
        static public IWin32Window GetMainWindow()
        {
            List<Process> allProcesses = new List<Process>(Process.GetProcesses());
            Process proc = allProcesses.Find(pr => pr.ProcessName == "EA");
            if (proc.MainWindowTitle == "")  //sometimes a wrong handle is returned, in this case also the title is empty
                return null;                   //return null in this case
            else                             //otherwise return the right handle
                return new WindowWrapper(proc.MainWindowHandle);
        }


        internal class WindowWrapper : System.Windows.Forms.IWin32Window
        {
            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }

            public IntPtr Handle
            {
                get { return _hwnd; }
            }

            private IntPtr _hwnd;
        }
    }

    public enum EaType
    {
        Package,
        Element,
        Attribute,
        Operation,
        Diagram
    }

    public static class EaRepositoryExtensions
    {
        static public DialogResult ShowDialogAtMainWindow(this Form form)
        {
            IWin32Window win32Window = InternalHelpers.GetMainWindow();
            if (win32Window != null)  // null means that the main window handle could not be evaluated
                return form.ShowDialog(win32Window);
            else
                return form.ShowDialog();  //fallback: use it without owner
        }
        public static void OpenEaPropertyDlg(this EA.Repository rep, int id, EaType type)
        {
            string dlg;
            switch (type)
            {
                case EaType.Package: dlg = "PKG"; break;
                case EaType.Element: dlg = "ELM"; break;
                case EaType.Attribute: dlg = "ATT"; break;
                case EaType.Operation: dlg = "OP"; break;
                case EaType.Diagram: dlg = "DGM"; break;
                default: dlg = String.Empty; break;
            }
            IWin32Window mainWindow = InternalHelpers.GetMainWindow();
            if (mainWindow != null)
            {
                string ret = rep.CustomCommand("CFormCommandHelper", "ProcessCommand", "Dlg=" + dlg + ";id=" + id + ";hwnd=" + mainWindow.Handle);
            }
        }

        public static void OpenPackagePropertyDlg(this EA.Repository rep, int packageId)
        {
            rep.OpenEaPropertyDlg(packageId, EaType.Package);
        }

        public static void OpenElementPropertyDlg(this EA.Repository rep, int elementId)
        {
            rep.OpenEaPropertyDlg(elementId, EaType.Element);
        }

        public static void OpenAttributePropertyDlg(this EA.Repository rep, int attributeId)
        {
            rep.OpenEaPropertyDlg(attributeId, EaType.Attribute);
        }

        public static void OpenOperationPropertyDlg(this EA.Repository rep, int operationId)
        {
            rep.OpenEaPropertyDlg(operationId, EaType.Operation);
        }

        public static void OpenDiagramPropertyDlg(this EA.Repository rep, int diagramId)
        {
            rep.OpenEaPropertyDlg(diagramId, EaType.Diagram);
        }
    }


    /// <summary>
    /// Set connection string
    /// </summary>
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class MySettings : ILinqToDBSettings
    {
        readonly string _provider;
        private readonly string _connectionString;

        public MySettings(string provider, string connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
        }


        public IEnumerable<IDataProviderSettings> DataProviders
        {
            get { yield break; }
        }

        public string DefaultConfiguration => "AccessForEA";    //??
        public string DefaultDataProvider => "Access";//??

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "AccessForEA",      // only name to show
                        ProviderName = _provider, // has to be correct driver name
                        ConnectionString = _connectionString
                    };
            }
        }
    }

}
