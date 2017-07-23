using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using AddInSimple.EABasic;


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
    public class AddInSimpleClass : EaAddInBase
    {
        // define menu constants
        const string MenuName = "-&AddInSimple";
        const string MenuHello = "&Say Hello";
        const string MenuGoodbye = "&Say Goodbye";
        const string MenuOpenProperties = "&Open Properties";
        const string MenuRunDemoSearch = "&DemoSearch";

        // remember if we have to say hello or goodbye
        private bool _shouldWeSayHello = true;
        
        /// <summary>
        /// constructor where we set the menu header and menuOptions
        /// </summary>
        public AddInSimpleClass()
        {
            this.menuHeader = MenuName;
            this.menuOptions = new[] { MenuHello, MenuGoodbye, MenuOpenProperties, MenuRunDemoSearch };
        }
        /// <summary>
        /// EA_Connect events enable Add-Ins to identify their type and to respond to Enterprise Architect start up.
        /// This event occurs when Enterprise Architect first loads your Add-In. Enterprise Architect itself is loading at this time so that while a Repository object is supplied, there is limited information that you can extract from it.
        /// The chief uses for EA_Connect are in initializing global Add-In data and for identifying the Add-In as an MDG Add-In.
        /// Also look at EA_Disconnect.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.
        /// Poll its members to retrieve model data and user interface status information.</param>
        /// <returns>String identifying a specialized type of Add-In: 
        /// - "MDG" : MDG Add-Ins receive MDG Events and extra menu options.
        /// - "" : None-specialized Add-In.</returns>
        public override string EA_Connect(EA.Repository Repository)
        {
            return base.EA_Connect(Repository);
        }
        /// <summary>
        /// Called once Menu has been opened to see what menu items should active.
        /// </summary>
        /// <param name="repository">the repository</param>
        /// <param name="location">the location of the menu</param>
        /// <param name="menuName">the name of the menu</param>
        /// <param name="itemName">the name of the menu item</param>
        /// <param name="isEnabled">boolean indicating whethe the menu item is enabled</param>
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
                    DataTable dt = SetTable();
                    // 2. Order, Filter, Join, Format to XML
                    string xml = QueryAndMakeXml(dt);
                    // 3. Out put to EA
                    repository.RunModelSearch("", "", "", xml);
                    break;
            }
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

        public void testPropertiesDialog(EA.Repository repository)
        {
            int diagramID = repository.GetCurrentDiagram().DiagramID;
            // there is no current diagram
            if (diagramID == 0) return;
            repository.OpenDiagramPropertyDlg(diagramID);
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

                // Testresult for a test
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
        /// Who it's works:
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
            xmlResults = QueryAndMakeXml(dt);
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
        /// Test Query
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string QueryAndMakeXml(DataTable dt)
        {
            try
            {
                // Make a LINQ query (WHERE, JOIN, ORDER,)
                OrderedEnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
                    orderby row.Field<string>("Name") descending
                    select row;

                return MakeXml(dt, rows);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}", "Error LINQ query");
                return "";

            }
        }
        /// <summary>
        /// Make EA xml from a DataTable (for column names) and the ordered Enumeration provided by LINQ. Set the Captions in DataTable to ensure column names. 
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static string MakeXml(DataTable dt, OrderedEnumerableRowCollection<DataRow> rows)
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

}
