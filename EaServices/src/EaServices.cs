using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using hoTools.Utils;
using hoTools.Utils.Favorites;
using hoTools.Utils.svnUtil;
using hoTools.Utils.Appls;
using hoTools.Utils.ActivityParameter;
using System.Reflection;
using hoTools.Connectors;
using hoTools.EaServices.Dlg;
using hoTools.Utils.SQL;



namespace hoTools.EaServices
{
    #region Definition of Service Attribute
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false) ]

    /// <summary>
    /// Attribute to define services which might be called without parameters
    /// Example:
    /// [ServiceOperation("{1C78E1C0-AAC8-4284-8C25-2D776FF373BC}", "Copy release information to clipboard", "Select Component", false)].
    /// </summary>
    /// <param name="GUID">GUID of the method</param>
    /// <param name="Description">the brief description for the service</param>
    /// <param name="Help">the help text / tooltip for the service </param>
    /// <param name="IsTextRequired">text is required, default false</param>
    public class ServiceOperationAttribute : Attribute 
    {
        public ServiceOperationAttribute(String GUID, String Description, String Help, bool IsTextRequired = false)
        {
            _description = Description;
            _GUID = GUID;
            _Help = Help;
            _IsTextRequired = IsTextRequired;
        }
        protected bool _IsTextRequired;
        public bool IsTextRequired => this._IsTextRequired;
        protected String _description;
        public String Description => this._description;
        protected String _Help;
        public String Help => this._Help;
        protected String _GUID;
        public String GUID => this._GUID;
    }
    #endregion

    public static class EaService 
    {
          public static string release = "1.0.01";
          private const string EMBEDDED_ELEMENT_TYPES = "Port Parameter Pin"; 
        

        // define menu constants
        public enum displayMode
        {
            Behavior,
            Method
        }

        #region runQuickSearch
        //---------------------------------------------------------------------------------------------------------------
        // Search for Elements, Operation, Attributes, GUID
        public static void runQuickSearch(EA.Repository rep, string searchName, string searchString)
        {
            // get the search from setting
            try
            {
                rep.RunModelSearch(searchName, searchString, "", "");
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString(), "Search name:'" + searchName + "'");
            }
        }
        #endregion
        #region addDiagramNote
        public static void addDiagramNote(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(EA.ObjectType.otDiagram))
            {
                EA.Diagram dia = rep.GetCurrentDiagram();
                EA.Package pkg = rep.GetPackageByID(dia.PackageID);
                if (pkg.IsProtected || dia.IsLocked || dia == null) return;

                // save diagram
                rep.SaveDiagram(dia.DiagramID);

                EA.Element elNote = null;
                try
                {
                    elNote = (EA.Element)pkg.Elements.AddNew("", "Note");
                    elNote.Update();
                    pkg.Update();
                }
                catch { return; }

                // add element to diagram
                // "l=200;r=400;t=200;b=600;"

                // get the position of the Element

                int left = (dia.cx / 2) - 100;
                int right = left + 200;
                int top = dia.cy - 150;
                int bottom = top + 120;
                //int right = diaObj.right + 2 * (diaObj.right - diaObj.left);

                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";

                var diaObject = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
                dia.Update();
                diaObject.ElementID = elNote.ElementID;
                diaObject.Update();
                pkg.Elements.Refresh();

                Util.setDiagramHasAttchaedLink(rep, elNote);
                rep.ReloadDiagram(dia.DiagramID);

            }
        }
        #endregion
        #region changeAuthorPackage
        public static void changeAuthorPackage( EA.Repository rep, EA.Package pkg, string[] args) {
            EA.Element el = rep.GetElementByGuid(pkg.PackageGUID);
            el.Author = args[0];
            el.Update();
            return;
        }
        #endregion
        #region changeAuthorElement
        public static void changeAuthorElement(EA.Repository rep, EA.Element el, string[] args)
        {
            el.Author = args[0];
            el.Update();
            return;
        }
        #endregion
        #region changeAuthorDiagram
        public static void changeAuthorDiagram(EA.Repository rep, EA.Diagram dia, string[] args)
        {
            dia.Author = args[0];
            dia.Update();
            return;
        }
        #endregion
        #region change User Recursive
        [ServiceOperation("{F0038D4B-CCAA-4F05-9401-AAAADF431ECB}", "Change user of package/element recursive", "Select package or element", IsTextRequired: false)]
        public static void changeUserRecursive(EA.Repository rep)
        {
            // get the user
            string[] s = {""};
            string oldAuthor = "";
            EA.Element el = null;
            EA.Package pkg = null;
            EA.Diagram dia = null;
            EA.ObjectType oType = rep.GetContextItemType();

            // get the element
            switch (oType)
            {
                case EA.ObjectType.otPackage:
                    pkg = (EA.Package)rep.GetContextObject();
                    el = rep.GetElementByGuid(pkg.PackageGUID);
                    oldAuthor = el.Author;
                    break;
                case EA.ObjectType.otElement:
                    el = (EA.Element)rep.GetContextObject();
                    oldAuthor = el.Author;
                    break;
                case EA.ObjectType.otDiagram:
                    dia = (EA.Diagram)rep.GetContextObject();
                    oldAuthor = dia.Author;
                    break;
                default:
                    return;
            }
            // ask for new user
            var dlg = new dlgUser(rep);
            dlg.User = oldAuthor;
            DialogResult res = dlg.ShowDialog();
            s[0] = dlg.User; 
            dlg = null;
            if (s[0] == "")
            {
                MessageBox.Show("Author:'" + s[0] + "'", "no or invalid user");
                return;
            }
            switch (oType)
            {
                case EA.ObjectType.otPackage:
                    RecursivePackages.doRecursivePkg(rep, pkg, changeAuthorPackage, changeAuthorElement, changeAuthorDiagram, s);
                    MessageBox.Show("New author:'" + s[0] + "'", "Author changed for packages/elements (recursive)");
                    break;
                case EA.ObjectType.otElement:
                    RecursivePackages.doRecursiveEl(rep, el, changeAuthorElement, changeAuthorDiagram, s);
                    MessageBox.Show("New author:'" + s[0] + "'", "Author changed for elements (recursive)");
                    break;
                case EA.ObjectType.otDiagram:
                    changeAuthorDiagram(rep, dia, s);
                    MessageBox.Show("New author:'" + s[0] + "'", "Author changed for diagram");
                    break;
                default:
                    return;
            }


        }
        #endregion
        #region change User
        [ServiceOperation("{4161D769-825F-494A-9389-962CC1C16E4F}", "Change Author of package/element", "Select package or element", IsTextRequired: false)]
        public static void changeAuthor(EA.Repository rep)
        {

            string[] args = {""};
            string oldAuthor = "";
            EA.Element el = null;
            EA.Package pkg = null;
            EA.Diagram dia = null;
            EA.ObjectType oType = rep.GetContextItemType();

            // get the element
            switch (oType)
            {
                case EA.ObjectType.otPackage:
                    pkg = (EA.Package)rep.GetContextObject();
                    el = rep.GetElementByGuid(pkg.PackageGUID);
                    oldAuthor = el.Author;
                    break;
                case EA.ObjectType.otElement:
                    el = (EA.Element)rep.GetContextObject();
                    oldAuthor = el.Author;
                    break;
                case EA.ObjectType.otDiagram:
                    dia = (EA.Diagram)rep.GetContextObject();
                    oldAuthor = dia.Author;
                    break;
                default:
                    return;
            }
            // ask for new user
            var dlg = new dlgUser(rep);
            dlg.User = oldAuthor;
            DialogResult res = dlg.ShowDialog();
            args[0] = dlg.User;
            dlg = null;
            if (args[0] == "")
            {
                MessageBox.Show("Author:'" + args[0] + "'", "no or invalid user");
                return;
            }
            switch (oType)
            {
                case EA.ObjectType.otPackage:
                    changeAuthorPackage(rep,pkg, args);
                    MessageBox.Show("New author:'" + args[0] + "'", "Author changed for package");
                    break;
                case EA.ObjectType.otElement:
                    changeAuthorElement(rep, el, args);
                    MessageBox.Show("New author:'" + args[0] + "'", "Author changed for element");
                    break;
                case EA.ObjectType.otDiagram:
                    changeAuthorDiagram(rep, dia, args);
                    MessageBox.Show("New author:'" + args[0] + "'", "Author changed for element");
                    break;
                default:
                    return;
            }


        }
#endregion
        #region ShowFolder
        // Show folder with Explorer or Total Commander for:
        // - Package: If Version Controlled package
        // - File: If Source Code implementation exists
        // See also: Global Settings
        [ServiceOperation("{C007C59A-FABA-4280-9B66-5AD10ACB4B13}", "Show folder of *.xml, *.h,*.c", "Select VC controlled package or element with file path (Source Code Generation)", IsTextRequired: false)]
        public static void ShowFolder(EA.Repository rep, bool isTotalCommander=false)
        {
            string path;
            EA.ObjectType oType = rep.GetContextItemType();
            switch (oType)
            {
                case EA.ObjectType.otPackage:
                    var pkg = (EA.Package)rep.GetContextObject();
                    path = Util.getVccFilePath(rep, pkg);
                    // remove filename
                    path = Regex.Replace(path, @"[a-zA-Z0-9\s_:.]*\.xml", "");

                    if (isTotalCommander)
                        Util.startApp(@"totalcmd.exe", "/o " + path);
                    else
                        Util.startApp(@"Explorer.exe", "/e, " + path);
                    break;

                case EA.ObjectType.otElement:
                    var el = (EA.Element)rep.GetContextObject();
                    path = Util.getGenFilePath(rep, el);
                    // remove filename
                    path = Regex.Replace(path, @"[a-zA-Z0-9\s_:.]*\.[a-zA-Z0-9]{0,4}$", "");

                    if (isTotalCommander)
                        Util.startApp(@"totalcmd.exe", "/o " + path);
                    else
                        Util.startApp(@"Explorer.exe", "/e, " + path);

                    break;
            }
        }
        #endregion

        #region CreateActivityForOperation
        [ServiceOperation("{17D09C06-8FAE-4D76-B808-5EC2362B1953}", "Create Activity for Operation, Class/Interface", "Select Package, Class/Interface or operation", IsTextRequired: false)]
        public static void CreateActivityForOperation(EA.Repository rep) {
            EA.ObjectType oType = rep.GetContextItemType();
            switch (oType) {
            case EA.ObjectType.otMethod:
                var m = (EA.Method)rep.GetContextObject();
                
                // Create Activity at the end
                EA.Element el = rep.GetElementByID(m.ParentID);
                EA.Package pkg = rep.GetPackageByID(el.PackageID);
                int pos = pkg.Packages.Count + 1;
                ActivityPar.createActivityForOperation(rep, m, pos);
                rep.Models.Refresh();
                rep.RefreshModelView(0);
                rep.ShowInProjectView(m);
                break;

            case EA.ObjectType.otElement:
                el = (EA.Element)rep.GetContextObject();
                if (el.Locked) return;

                CreateActivityForOperationsInElement(rep, el);
                rep.Models.Refresh();
                rep.RefreshModelView(0);
                rep.ShowInProjectView(el);
                break;

            case EA.ObjectType.otPackage:
                pkg = (EA.Package)rep.GetContextObject();
                CreateActivityForOperationsInPackage(rep, pkg);
                // update sort order of packages
                rep.Models.Refresh();
                rep.RefreshModelView(0);
                rep.ShowInProjectView(pkg);
                break;
            }

            return;
        }
        #endregion
        public static void CreateActivityForOperationsInElement(EA.Repository rep, EA.Element el)
        {
            if (el.Locked) return;
            EA.Package pkg = rep.GetPackageByID(el.PackageID);
            int treePos = pkg.Packages.Count + 1;
            foreach (EA.Method m1 in el.Methods)
            {
                // Create Activity
                ActivityPar.createActivityForOperation(rep, m1, treePos);
                treePos = treePos + 1;

            }

        }
        public static void CreateActivityForOperationsInPackage(EA.Repository rep, EA.Package pkg)
        {
            foreach (EA.Element el in pkg.Elements)
            {
                CreateActivityForOperationsInElement(rep, el);

            }
            foreach (EA.Package pkg1 in pkg.Packages)
            {
                CreateActivityForOperationsInPackage(rep, pkg1);
            }
            
        }
        private static bool locateTextOrFrame(EA.Repository rep, EA.Element el)
        {
            if (el.Type == "Text")
            {
                string s = el.get_MiscData(0);
                int id = Convert.ToInt32(s);
                EA.Diagram dia = rep.GetDiagramByID(id);
                rep.ShowInProjectView(dia);
                return true;
            }
            // display the original diagram on what the frame is based
            if (el.Type == "UMLDiagram")
            {
                int id = Convert.ToInt32(el.get_MiscData(0));
                EA.Diagram dia = rep.GetDiagramByID(id);
                rep.ShowInProjectView(dia);
                return true;

            }
            return false;
        }
        #region showAllEmbeddedElementsGUI
        [ServiceOperation("{678AD901-1D2F-4FB0-BAAD-AEB775EE18AC}", "Show all ports for Component", "Select Class, Interface or Component", IsTextRequired: false)]
        public static void showEmbeddedElementsGUI(
            EA.Repository rep, 
            string EmbeddedElementType="Port Pin Parameter", 
            bool isOptimizePortLayout=false)
        {

            EA.Diagram dia = null;
            EA.Element elSource = null;
            EA.DiagramObject diaObjSource = null;

            dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            rep.SaveDiagram(dia.DiagramID);

            var sqlUtil = new UtilSql(rep); 
            // over all selected elements
            foreach (EA.DiagramObject diaObj in dia.SelectedObjects)
            {
                elSource = rep.GetElementByID(diaObj.ElementID);
                if (! "Class Component Activity".Contains(elSource.Type)) continue; 
                // find object on Diagram
                diaObjSource = Util.getDiagramObjectByID(rep, dia, elSource.ElementID);
                //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");
                if (diaObjSource == null) return;

                List<int> l_ports = null;
                string[] portTypes = {"left","right"};
                foreach (string portBoundTo in portTypes )
                {
                    // arrange sequence of ports
                    if (isOptimizePortLayout == false && portBoundTo == "left") continue;
                    int pos = 0;
                    if (isOptimizePortLayout == false) { 
                        l_ports = sqlUtil.getAndSortEmbeddedElements(elSource, "", "", "");
                    }
                    else {
                        if (portBoundTo == "left") l_ports = sqlUtil.getAndSortEmbeddedElements(elSource, "Port", "'Server', 'Receiver' ", "DESC");
                        else l_ports = sqlUtil.getAndSortEmbeddedElements(elSource, "Port", "'Client', 'Sender' ", "");
                    }
                    // over all sorted ports
                    string oldStereotype = ""; 
                    foreach (int i in l_ports) { 
                        EA.Element portEmbedded = rep.GetElementByID(i);
                        if (EmbeddedElementType == "" | EmbeddedElementType.Contains(portEmbedded.Type))
                        {
                            // only ports / parameters (port has no further embedded elements
                            if (portEmbedded.Type == "ActivityParameter" | portEmbedded.EmbeddedElements.Count == 0)
                            {
                                if (isOptimizePortLayout)
                                {
                                    if (portBoundTo == "left")
                                    {
                                        if ("Sender Client".Contains(portEmbedded.Stereotype)) continue;
                                    }
                                    else
                                    {
                                        if ("Receiver Server".Contains(portEmbedded.Stereotype)) continue;
                                    }

                                }
                                // Make a gap between different stereotypes
                                if (pos == 0 && "Sender Receiver".Contains(portEmbedded.Stereotype)) oldStereotype = portEmbedded.Stereotype;
                                if (pos > 0 && "Sender Receiver".Contains(oldStereotype) && oldStereotype != portEmbedded.Stereotype)
                                {
                                    pos = pos + 1; // make a gap
                                    oldStereotype = portEmbedded.Stereotype;

                                }
                                Util.visualizePortForDiagramobject(pos, dia, diaObjSource, portEmbedded, null, portBoundTo);
                                pos = pos + 1;
                            }
                            else
                            {
                                // Port: Visualize Port + Interface
                                foreach (EA.Element interf in portEmbedded.EmbeddedElements)
                                {
                                    Util.visualizePortForDiagramobject(pos, dia, diaObjSource, portEmbedded, interf);
                                    pos = pos + 1;
                                }
                            }
                        }
                    }
                }
            }
            rep.ReloadDiagram(dia.DiagramID);

            
        }
        #endregion
        public static void navigateComposite(EA.Repository Repository)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            // find composite element of diagram
            if (oType.Equals(EA.ObjectType.otDiagram))
            {
                var d = (EA.Diagram)Repository.GetContextObject();
                string guid = Util.getElementFromCompositeDiagram(Repository, d.DiagramGUID);
                if (guid != "")
                {
                    Repository.ShowInProjectView(Repository.GetElementByGuid(guid));
                }

            }
            // find composite diagram of element of element
            if (oType.Equals(EA.ObjectType.otElement))
            {
                var e = (EA.Element)Repository.GetContextObject();
                // locate text or frame
                if (locateTextOrFrame(Repository, e)) return;

                Repository.ShowInProjectView(e.CompositeDiagram);
            }
        }
        #region findUsage
        /// <summary>
        /// Find usage of selected item. The following searches are required
        /// - "Element usage"
        /// - "Method usage"
        /// - "Diagram usage"
        /// - "Connector usage"
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{755DD068-94A2-4AD9-84EE-F3D1350BC9B7}", "Find usage of Element,Method, Attribute, Diagram, Connector ", "Select item", false)]
        public static void findUsage(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            EA.Element el = null;
            if (oType.Equals(EA.ObjectType.otElement))
            {
                // locate text or frame
                el = (EA.Element)rep.GetContextObject();
                if (locateTextOrFrame(rep, el)) return;
                rep.RunModelSearch("Element usage", el.ElementGUID, "", "");
            }
            if (oType.Equals(EA.ObjectType.otMethod))
            {
                var method = (EA.Method)rep.GetContextObject();
                rep.RunModelSearch("Method usage", method.MethodGUID, "", "");
            }
            if (oType.Equals(EA.ObjectType.otDiagram))
            {
                var dia = (EA.Diagram)rep.GetContextObject();
                rep.RunModelSearch("Diagram usage", dia.DiagramGUID, "", "");
            }
            if (oType.Equals(EA.ObjectType.otConnector))
            {
                var con = (EA.Connector)rep.GetContextObject();
                rep.RunModelSearch("Connector is visible in Diagrams",
                    con.ConnectorID.ToString(), "", "");
            }
            return;
        }
        #endregion
        /// <summary>
        /// Show all specifications of selected Element. It shows all files in specified in properties.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{899C9C5F-39B8-47E3-B253-7C5730F1AA7D}", "Show all specifications of selected element (all files defined in properties)", "Select item", false)]
        public static void showSpecification(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            EA.Element el = null;
            if (oType.Equals(EA.ObjectType.otElement))
            {
                el = (EA.Element)rep.GetContextObject();
                //over all file
                foreach (EA.File f in el.Files)
                {
                    if (f.Name.Length > 2)
                    {
                        Process.Start(f.Name);
                    }
                }
            }
            return;
        }
        #region LineStyle
        #region setLineStyleLV
        [ServiceOperation("{5F5CB088-1DDD-4A00-B641-273CAC017AE5}", "Set line style LV(Lateral Vertical)", "Select Diagram, connector, nodes", IsTextRequired: false)]
        #endregion
        public static void setLineStyleLV(EA.Repository rep)
        {
            setLineStyle(rep, "LV");
        }
         [ServiceOperation("{9F1E7448-3B3B-4058-83AB-CBA97F24B90B}", "Set line style LH(Lateral Horizontal)", "Select Diagram, connector, nodes", IsTextRequired: false)]
         public static void setLineStyleLH(EA.Repository rep)
         {
             setLineStyle(rep, "LH");
         }
         [ServiceOperation("{A8199FFF-A9BA-4875-9529-45B2801F0DB3}", "Set line style TV(Tree Vertical)", "Select Diagram, connector, nodes", IsTextRequired: false)]
         public static void setLineStyleTV(EA.Repository rep)
         {
             setLineStyle(rep, "TV");
         }
         [ServiceOperation("{5E481745-C684-431D-BD02-AD22EE39C252}", "Set line style TH(Tree Horizontal)", "Select Diagram, connector, nodes", IsTextRequired: false)]
         public static void setLineStyleTH(EA.Repository rep)
         {
             setLineStyle(rep, "TH");
         }
         [ServiceOperation("{A8199FFF-A9BA-4875-9529-45B2801F0DB3}", "Set line style OS(Orthogonal Square)", "Select Diagram, connector, nodes", IsTextRequired: false)]
         public static void setLineStyleOS(EA.Repository rep)
         {
             setLineStyle(rep, "OS");
         }
         [ServiceOperation("{D7B75725-60B7-4C73-913F-164E6EE847D3}", "Set line style OR(Orthogonal Round)", "Select Diagram, connector, nodes", IsTextRequired: false)]
         public static void setLineStyleOR(EA.Repository rep)
         {
             setLineStyle(rep, "OR");
         }
         [ServiceOperation("{99F31FC7-8326-468B-B1D8-2542BBC8D4EB}", "Set line style B(Bezier)", "Select Diagram, connector, nodes", IsTextRequired: false)]
         public static void setLineStyleB(EA.Repository rep)
         {
             setLineStyle(rep, "B");
         }

        public static void setLineStyle(EA.Repository Repository, string lineStyle)
        {
          EA.Connector con = null;
            EA.Collection objCol = null;
            EA.ObjectType oType = Repository.GetContextItemType();
            EA.Diagram diaCurrent = Repository.GetCurrentDiagram();
            if (diaCurrent != null)
            {
                con = diaCurrent.SelectedConnector;
                objCol = diaCurrent.SelectedObjects;
            }
            // all connections of diagram
            if (oType.Equals(EA.ObjectType.otDiagram))
            {
                Util.SetLineStyleDiagram(Repository, diaCurrent, lineStyle);
            }
            // all connections of diagram elements
            if (objCol.Count >0 | con != null)
            {
                Util.SetLineStyleDiagramObjectsAndConnectors(Repository, diaCurrent, lineStyle);
            }
            
        }
        #endregion
        #region DisplayOperationForSelectedElement
        // display behavior or definition for selected element
        // displayMode: "Behavior" or "Method"
        public static void DisplayOperationForSelectedElement(EA.Repository Repository, displayMode showBehavior)
        {
            EA.ObjectType oType = Repository.GetContextItemType();
            // Method found
            if (oType.Equals(EA.ObjectType.otMethod))
            {
                // display behavior for method
                Appl.DisplayBehaviorForOperation(Repository, (EA.Method)Repository.GetContextObject());

            }
            if (oType.Equals(EA.ObjectType.otDiagram))
            {
                // find parent element
                var dia = (EA.Diagram)Repository.GetContextObject();
                if (dia.ParentID > 0)
                {
                    // find parent element
                    EA.Element parentEl = Repository.GetElementByID(dia.ParentID);
                    //
                    locateOperationFromBehavior(Repository, parentEl, showBehavior);
                }
                else
                {
                    // open diagram
                    Repository.OpenDiagram(dia.DiagramID);
                }
            }


            // Connector / Message found
            if (oType.Equals(EA.ObjectType.otConnector))
            {
                var con = (EA.Connector)Repository.GetContextObject();
                if (con.Type.Equals("StateFlow"))
                {

                    EA.Method m = Util.getOperationFromConnector(Repository, con);
                    if (m != null)
                    {
                        if (showBehavior.Equals(displayMode.Behavior))
                        {
                            Appl.DisplayBehaviorForOperation(Repository, m);
                        }
                        else
                        {
                            Repository.ShowInProjectView(m);
                        }

                    }


                }

                if (con.Type.Equals("Sequence"))
                {
                    // If name is of the form: OperationName(..) the operation is associated to an method
                    string opName = con.Name;
                    if (opName.EndsWith(")", StringComparison.Ordinal))
                    {
                        // extract the name
                        int pos = opName.IndexOf("(", StringComparison.Ordinal);
                        opName = opName.Substring(0, pos);
                        EA.Element el = Repository.GetElementByID(con.SupplierID);
                        // find operation by name
                        foreach (EA.Method op in el.Methods)
                        {
                            if (op.Name == opName)
                            {
                                Repository.ShowInProjectView(op);
                                //Appl.DisplayBehaviorForOperation(Repository, op);
                                return;
                            }
                        }
                        if ((el.Type.Equals("Sequence") || el.Type.Equals("Object")) && el.ClassfierID > 0)
                        {
                            el = (EA.Element)Repository.GetElementByID(el.ClassifierID);
                            foreach (EA.Method op in el.Methods)
                            {
                                if (op.Name == opName)
                                {
                                    if (showBehavior.Equals(displayMode.Behavior))
                                    {
                                        Appl.DisplayBehaviorForOperation(Repository, op);
                                    }
                                    else
                                    {
                                        Repository.ShowInProjectView(op);
                                    }

                                }
                            }
                        }

                    }
                }
            }

            // Element
            if (oType.Equals(EA.ObjectType.otElement))
            {
                var el = (EA.Element)Repository.GetContextObject();
                // locate text or frame
                if (locateTextOrFrame(Repository, el)) return;

                if (el.Type.Equals("Activity") & showBehavior.Equals(displayMode.Behavior))
                {
                    // Open Behavior for Activity
                    Util.OpenBehaviorForElement(Repository, el);


                }
                if (el.Type.Equals("State"))
                {
                    // get operations
                    foreach (EA.Method m in el.Methods)
                    {
                        // display behaviors for methods
                        Appl.DisplayBehaviorForOperation(Repository, m);
                    }
                }

                if (el.Type.Equals("Action"))
                {
                    foreach (EA.CustomProperty custproperty in el.CustomProperties)
                    {
                        if (custproperty.Name.Equals("kind") && custproperty.Value.Equals("CallOperation"))
                        {
                            showFromElement(Repository, el, showBehavior);
                        }
                        if (custproperty.Name.Equals("kind") && custproperty.Value.Equals("CallBehavior"))
                        {
                            el = Repository.GetElementByID(el.ClassfierID);
                            Util.OpenBehaviorForElement(Repository, el);
                        }
                    }

                }
                if (showBehavior.Equals(displayMode.Method) & (
                    el.Type.Equals("Activity") || el.Type.Equals("StateMachine") || el.Type.Equals("Interaction")) )
                {
                    locateOperationFromBehavior(Repository, el, showBehavior);
                }
            }
        }
        #endregion
        private static void showFromElement(EA.Repository Repository, EA.Element el, displayMode showBehavior)
        {
            EA.Method method = Util.getOperationFromAction(Repository, el);
            if (method != null)
            {
                if (showBehavior.Equals(displayMode.Behavior))
                {
                    Appl.DisplayBehaviorForOperation(Repository, method);
                }
                else
                {
                    Repository.ShowInProjectView(method);
                }
            }
        }

        private static void locateOperationFromBehavior(EA.Repository Repository, EA.Element el, displayMode showBehavior)
        {
            EA.Method method = Util.getOperationFromBrehavior(Repository, el);
            if (method != null)
            {
                if (showBehavior.Equals(displayMode.Behavior))
                {
                    Appl.DisplayBehaviorForOperation(Repository, method);
                }
                else
                {
                    Repository.ShowInProjectView(method);
                }
            }
        }
        private static void BehaviorForOperation(EA.Repository Repository, EA.Method method)
        {
            string behavior = method.Behavior;
            if (behavior.StartsWith("{", StringComparison.Ordinal) & behavior.EndsWith("}", StringComparison.Ordinal))
            {
                // get object according to behavior
                EA.Element el = Repository.GetElementByGuid(behavior);
                // Activity
                if (el == null) { }
                else
                {
                    if (el.Type.Equals("Activity") || el.Type.Equals("Interaction") || el.Type.Equals("StateMachine"))
                    {
                        Util.OpenBehaviorForElement(Repository, el);
                    }
                }
            }
        }
        #region createDiagramObjectFromContext
        //----------------------------------------------------------------------------------------
        // type:      "Action", "Activity","Decision", "MergeNode","StateNode"
        // extension: "CallOperation" ,"101"=StateNode, Final, "no"= else/no Merge
        //             comp=yes:  Activity with composite Diagram
        //----------------------------------------------------------------------------------------
        public static EA.DiagramObject  createDiagramObjectFromContext(EA.Repository rep, string name, string type,
            string extension, int offsetHorizental = 0, int offsetVertical = 0, string guardString = "", EA.Element srcEl=null)
        {
            int WidthPerCharacter = 60;
            // filter out linefeed, tab
            name = Regex.Replace(name, @"(\n|\r|\t)", "", RegexOptions.Singleline);

            if (name.Length > 255)
            {
                MessageBox.Show(type + ": '" + name + "' has more than 255 characters.", "Name is to long");
                return null;
            }
            EA.DiagramObject diaObjSource = null;
            EA.DiagramObject diaObjTarget = null;
            EA.Element elSource = null;
            EA.Element elParent = null;
            EA.Element elTarget = null;
            EA.DiagramObject diaObjParent = null;
            EA.Package pkg = null;

            string basicType = type;
            if (type == "CallOperation") basicType = "Action";

            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return null;

            rep.SaveDiagram(dia.DiagramID);

            // only one diagram object selected as source
            if (srcEl == null) elSource = Util.getElementFromContextObject(rep);
            else elSource = srcEl;
            if (elSource == null)  return null;
            diaObjSource = Util.getDiagramObjectByID(rep, dia, elSource.ElementID);
            //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");

            string noValifTypes = "Note, Constraint, Boundary, Text, UMLDiagram, DiagramFrame";
            if (noValifTypes.Contains(elSource.Type)) return null;
           

            if (!(elSource.ParentID == 0))
            {
                diaObjParent = Util.getDiagramObjectByID(rep, dia, elSource.ParentID);
                //diaObjParent = dia.GetDiagramObjectByID(elSource.ParentID, "");
            }
                
                try
                {
                    if (elSource.ParentID > 0 ) {
                        elParent = rep.GetElementByID(elSource.ParentID);
                        elTarget = (EA.Element)elParent.Elements.AddNew(name, basicType);
                        if (basicType == "StateNode") elTarget.Subtype = Convert.ToInt32(extension);
                        elParent.Elements.Refresh();
                       
                    }
                    else 
                    {
                        pkg = rep.GetPackageByID(elSource.PackageID);
                        elTarget = (EA.Element)pkg.Elements.AddNew(name, basicType);
                        if (basicType == "StateNode") elTarget.Subtype = Convert.ToInt32(extension);
                        pkg.Elements.Refresh();
                    }
                    elTarget.ParentID = elSource.ParentID;
                    elTarget.Update();
                    if (basicType == "Activity" & extension.ToLower() == "comp=yes")
                    {
                        EA.Diagram actDia = ActivityPar.createActivityCompositeDiagram(rep, elTarget);
                        Util.setActivityCompositeDiagram(rep, elTarget, actDia.DiagramID.ToString());
                        //elTarget.
                    }

                }
                catch { return null; }

                int left = diaObjSource.left + offsetHorizental;
                int right = diaObjSource.right + offsetHorizental;
                int top = diaObjSource.top + offsetVertical;
                int bottom = diaObjSource.bottom + offsetVertical;
                int length = 0;

                if (basicType == "StateNode")
                {
                    left = left - 10 + (right - left) / 2;
                    right = left + 20;
                    top = bottom - 20;
                    bottom = top - 20;
                }
                if ((basicType == "Decision") | (basicType == "MergeNode"))
                {
                    if (guardString == "no")
                    {
                        if (elSource.Type == "Decision") left = left + (right -left) + 200;
                        else left = left + (right -left) + 50;
                        bottom = bottom - 5;
                    }
                    left = left - 15 + (right - left)/2;
                    right = left + 30; 
                    top = bottom - 20;
                    bottom = top - 40;
                }
                if (basicType == "Action" | basicType == "Activity")
                {
                    length = (int)(name.Length * WidthPerCharacter / 10);

                    if (extension.ToLower() == "comp=no")
                    { /* Activity ind diagram */
                        if (length < 500) length = 500;
                        left = left + ((right - left) / 2) - (length / 2);
                        right = left + length;
                        top = bottom - 20;
                        bottom = top - 200;
                        if (basicType == "Activity") bottom = top - 400;


                    }
                    else if (extension.ToLower() == "comp=yes")
                    {
                        if (length < 220) length = 220;
                        left = left + ((right - left) / 2) - (length / 2);
                        right = left + length;
                        top = bottom - 40;
                        bottom = top - 40;
                    }
                    else
                    {

                        if (length < 220) length = 220;
                        left = left + ((right - left) / 2) - (length / 2);
                        right = left + length;
                        top = bottom - 20;
                        bottom = top - 20;
                    }

                }
                // limit values
                if (left < 5) left = 5;
                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
                // end note
                if ( elParent != null && elParent.Type == "Activity" && extension == "101")
                {
                    EA.DiagramObject diaObj = Util.getDiagramObjectByID(rep, dia, elParent.ElementID);
                    //EA.DiagramObject diaObj = dia.GetDiagramObjectByID(elParent.ElementID,"");
                    if (diaObj != null)
                    {
                        diaObj.bottom = bottom - 40;
                        diaObj.Update();
                    }
                }


                Util.addSequenceNumber(rep, dia);
                diaObjTarget = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
                diaObjTarget.ElementID = elTarget.ElementID;
                diaObjTarget.Sequence = 1;
                diaObjTarget.Update();
                Util.setSequenceNumber(rep, dia, diaObjTarget, "1");

                // position the label:
                // LBL=CX=180:  length of label
                // CY=13:       hight of label
                // OX=26:       x-position of label (relative object)
                // CY=13:       y-position of label (relative object)
                if (basicType == "Decision" & name.Length > 0)
                {
                    if (name.Length > 25) length = 25 * WidthPerCharacter / 10;
                    else length = (int)(name.Length * WidthPerCharacter / 10);
                    // string s = "DUID=E2352ABC;LBL=CX=180:CY=13:OX=29:OY=-4:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;;"; 
                    string s = "DUID=E2352ABC;LBL=CX=180:CY=13:OX=-"+ length + ":OY=-4:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;;"; 
                    Util.setDiagramObjectLabel(rep,
                        diaObjTarget.ElementID, diaObjTarget.DiagramID, diaObjTarget.InstanceID, s);
                }
                
                EA.DiagramObject initDiaObj = null;
                if (extension == "Comp=no")
                { /* Activity ind diagram */
                    // place an init
                    int initLeft = left + ((right - left) / 2) - 10;
                    int initRight = initLeft + 20;
                    int initTop = top - 25;
                    int initBottom = initTop - 20;
                    string initPosition = "l=" + initLeft + ";r=" + initRight + ";t=" + initTop + ";b=" + initBottom + ";";
                    initDiaObj = ActivityPar.CreateInitFinalNode(rep, dia,
                        elTarget, 100, initPosition);
                    
                    
                    
                }

                // draw a Control Flow
                var con = (EA.Connector)elSource.Connectors.AddNew("", "ControlFlow");
                con.SupplierID = elTarget.ElementID;
                con.Update();
                elSource.Connectors.Refresh();
                // set linestyle LV
                foreach (EA.DiagramLink link in dia.DiagramLinks)
                {
                    if (link.ConnectorID == con.ConnectorID)
                    {
                        if (!(guardString == "no"))
                        {
                            link.Geometry = "EDGE=3;$LLB=;LLT=;LMT=;LMB=CX=21:CY=13:OX=-20:OY=-19:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:DIR=0:ROT=0;LRT=;LRB=;IRHS=;ILHS=;";
                        }
                        // in case of switch case linestyle = LH
                        string style = "LV";
                        if ((elSource.Type == "Action" | elSource.Type == "Activity") & guardString == "no") style = "LH";
                        if (Regex.IsMatch(elSource.Name, @"switch[\s]*\(")) style = "OS";
                        Util.setLineStyleForDiagramLink(style, link);
                        
                        break;
                    }
                }
             

                // set Guard
                if (guardString != "") {
                    if (guardString == "no" && elSource.Type != "Decision")
                    {
                       // mo GUARD
                    }
                    else
                    {
                        // GUARD
                        Util.setConnectorGuard(rep, con.ConnectorID, guardString);
                    }
                }
                else if (elSource.Type.Equals("Decision") & !elSource.Name.Trim().Equals(""))
                {
                    if (guardString == "no")
                    {
                        Util.setConnectorGuard(rep, con.ConnectorID, "no");
                    }
                    else
                    {
                        Util.setConnectorGuard(rep, con.ConnectorID, "yes");
                    }
                }

                // handle subtypes of action
                if (type == "CallOperation")
                {

                    EA.Method method = hoTools.Utils.CallOperationAction.getMethodFromMethodName(rep, extension);
                    if (!(method == null))
                    {
                        hoTools.Utils.CallOperationAction.createCallAction(rep, elTarget, method);

                    }
                    
                }
                
                rep.ReloadDiagram(dia.DiagramID);

            // set selected object
                dia.SelectedObjects.AddNew(diaObjTarget.ElementID.ToString(), diaObjTarget.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
                int count = dia.SelectedObjects.Count;
                return diaObjTarget;
                
            
        }
        #endregion
        #region insertInterface
        public static void insertInterface(EA.Repository rep, EA.Diagram dia, string text)
        {

            bool isComponent = false;
            EA.Element elSource = null;
            EA.DiagramObject diaObjSource = null;
            EA.Package pkg = rep.GetPackageByID(dia.PackageID);
            int pos = 0;

            // only one diagram object selected as source
            if (dia.SelectedObjects.Count != 1) return;

            // save selected object
            EA.DiagramObject objSelected = null;
            if (!(dia == null && dia.SelectedObjects.Count > 0))
            {
                objSelected = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            }

            rep.SaveDiagram(dia.DiagramID);
            diaObjSource = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            elSource = rep.GetElementByID(diaObjSource.ElementID);
            isComponent |= elSource.Type == "Component";
            // remember selected object

            List<EA.Element> ifList = getInterfacesFromText(rep, pkg, text);
            foreach (EA.Element elTarget in ifList)
            {
                if (elSource.Locked )
                {
                    MessageBox.Show("Source #" + elSource.Name + "' is locked", "Element locked");
                    continue;
                }
                if (isComponent)
                {
                    addPortToComponent(elSource, elTarget);
                    
                }
                else
                {
                    addInterfaceToElement(rep, pos, elSource, elTarget, dia, diaObjSource);

                }
                pos = pos + 1;
            }
            // visualize ports
            if (isComponent)
            {
                dia.SelectedObjects.AddNew(diaObjSource.ElementID.ToString(), EA.ObjectType.otElement.ToString());
                dia.SelectedObjects.Refresh();
                EaService.showEmbeddedElementsGUI(rep);
            }
            else
            {// set line style
                
            }

            // reload selected object
            if (!(objSelected == null))
            {
                dia.SelectedObjects.AddNew(elSource.ElementID.ToString(), elSource.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
            }	
            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(elSource.ElementID.ToString(), elSource.ObjectType.ToString());
            dia.SelectedObjects.Refresh();

        }
        #endregion
        private static void addInterfaceToElement(EA.Repository rep, int pos, EA.Element elSource, EA.Element elTarget, EA.Diagram dia, EA.DiagramObject diaObjSource)
        {
            EA.DiagramObject diaObjTarget = null;
            
            // check if interface already exists on diagram

            diaObjTarget = Util.getDiagramObjectByID(rep, dia, elTarget.ElementID);
            //diaObjTarget = dia.GetDiagramObjectByID(elTarget.ElementID, "");
            if (diaObjTarget == null)
            {

                int length = 250;
                //if (elTarget.Type != "Interface") length = 250;
                // calculate target position
                // int left = diaObjSource.right - 75;
                int left = diaObjSource.right ;
                int right = left + length;
                int top = diaObjSource.bottom - 25;
                int bottom;

                top = top - 20 - pos * 70;
                bottom = top - 50;
                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";




                // create target diagram object
                diaObjTarget = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");

                diaObjTarget.ElementID = elTarget.ElementID;
                diaObjTarget.Sequence = 1;
                // suppress attributes/operations
                diaObjTarget.Style = "DUID=1263D775;AttPro=0;AttPri=0;AttPub=0;AttPkg=0;AttCustom=0;OpCustom=0;PType=0;RzO=1;OpPro=0;OpPri=0;OpPub=0;OpPkg=0;";
                diaObjTarget.Update();
            }
            // connect source to target by Usage

            // make a connector/ or link if notes
            // check if connector already exists
            EA.Connector con = null;
            foreach (EA.Connector c in elSource.Connectors) {
                if (c.SupplierID == elTarget.ElementID &
                    ( c.Type == "Usage"  | c.Stereotype == "use" |
                      c.Type == "Realisation")                            ) return;
                    
            }


            if (elTarget.Type.Equals("Interface") )
            {
                 con = (EA.Connector)elSource.Connectors.AddNew("", "Usage");
            } else {
                con = (EA.Connector)elSource.Connectors.AddNew("", "NoteLink");
            }
            con.SupplierID = elTarget.ElementID;
            try
            {
                con.Update();
                elSource.Connectors.Refresh();
                elTarget.Connectors.Refresh();
                // set line style
                dia.DiagramLinks.Refresh();
                //rep.ReloadDiagram(dia.DiagramID);
                foreach (EA.DiagramLink link in dia.DiagramLinks)
                {
                    if (link.ConnectorID == con.ConnectorID)
                    {
                        Util.setLineStyleForDiagramLink("LV", link);
                        string s = link.Style;
                    }
                }
            }
            catch (Exception e)
            {
                string s = rep.GetLastError();
                MessageBox.Show(e.ToString(), "Error create connector between '" + elSource.Name + " and '" + elTarget.Name + "' ");
            }
           

        }
        
        static void addPortToComponent(EA.Element elSource, EA.Element elInterface)
        {
            EA.Element port = null;
            EA.Element interf = null;
            if (elInterface.Type != "Interface") return;

            // check if port with interface already exists
            foreach (EA.Element p in elSource.EmbeddedElements)
            {
                if (p.Name == elInterface.Name) return;
            }
            // create a port
            port = (EA.Element)elSource.EmbeddedElements.AddNew(elInterface.Name, "Port");
            elSource.EmbeddedElements.Refresh();
            // add interface
            interf = (EA.Element)port.EmbeddedElements.AddNew(elInterface.Name, "RequiredInterface");
            // set classifier
            interf.ClassfierID = elInterface.ElementID;
            interf.Update();


          }

               
        private static List<EA.Element> getInterfacesFromText(EA.Repository rep, EA.Package pkg, string s, bool createWarningNote = true)
        {
            var elList = new List<EA.Element>();
            s = deleteComment(s);
            // string pattern = @"#include\s*[""<]([^.]*)\.h";
            string patternPath = @"#include\s*[""<]([^"">]*)";

            Match matchPath = Regex.Match(s, patternPath, RegexOptions.Multiline);
            while (matchPath.Success)
            {
                string includePath = matchPath.Groups[1].Value;
                // get includeName
                string includeName = Regex.Match(includePath, @"([\w-]*)\.h").Groups[1].Value;


                EA.Element el = CallOperationAction.getElementFromName(rep, includeName, "Interface");
                if (el == null && createWarningNote )
                {
                    // create a note
                    el = (EA.Element)pkg.Elements.AddNew("", "Note");
                    el.Notes = "Interface '" + includeName + "' not available!";
                    el.Update();

                }
                elList.Add(el);
                matchPath = matchPath.NextMatch();

            }



            return elList;
        }


        public static void insertInActivtyDiagram(EA.Repository rep, string text)
        {
            
            // remember selected object
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (dia.Type != "Activity") return;

            EA.Element elSource = Util.getElementFromContextObject(rep);
            if (elSource == null) return;
            EA.DiagramObject objSource = null;
            bool isSwitchCase = false;

            if (Regex.IsMatch(elSource.Name, @"switch[\s]*\("))
            {
                isSwitchCase = true;
                objSource = Util.getDiagramObjectByID(rep, dia, elSource.ElementID);
                //objSource = dia.GetDiagramObjectByID(elSource.ElementID, "");
            }

            int offsetHorizontal = 0;
            int offsetVertical = 0;
            string s1 = "";
            // delete comments /* to */
            string s0 = deleteComment(text);

            // delete casts
            Match match = Regex.Match(s0, @"(\([A-Za-z0-9_*& ]*\))[*&_A-Za-z]+", RegexOptions.Multiline);
            while (match.Success)
            {
                s0 = s0.Replace(match.Groups[1].Value, "");
                match = match.NextMatch();
            }
 


            // concatenate lines =..;
            match = Regex.Match(s0, @"[^=]*(=[^;{}]*)", RegexOptions.Singleline);
            
            while (match.Success)
            {
                string old = match.Groups[1].Value;
                if (! (match.Value.Contains("#")))
                {

                    //if (Regex.IsMatch(old, @"#[\s]*(if|elseif|else)", RegexOptions.Singleline)) continue;
                    s0 = s0.Replace(match.Groups[1].Value, Regex.Replace(old, "\r\n", ""));
                }
                match = match.NextMatch();
            }
            // concatenate lines nnnn(..);
            match = Regex.Match(s0, @"[A-Za-z0-9_]+[\s]*\([^;}{]*\)", RegexOptions.Singleline);
            while (match.Success)
            {
                string old = match.Groups[1].Value;
                if (! (match.Value.Contains("#")))
                {
                //if (Regex.IsMatch(old, @"#[\s]*(if|elseif|else)", RegexOptions.Singleline)) continue;
                    //s0 = s0.Replace(match.Groups[1].Value, Regex.Replace(old, "\r\n", ""));
                    // check if this is no if(..)
                    if (match.Value.StartsWith("if", StringComparison.CurrentCulture)) { }
                    else s0 = s0.Replace(match.Value, Regex.Replace(match.Value, "\r\n", ""));
                }
                match = match.NextMatch();
            }
            // remove empty lines
            s0 = Regex.Replace(s0, @"\r\n\s*\r\n", "\r\n"); 
            
           string[] lines = Regex.Split(s0, "\r\n");

            // first line start with an "else"
            bool skipFirstLine = false;
            // case nnnnn:
            string guardString = "";
            if (lines.Length > 0) {
                string line0 = lines[0].Trim();
                if (line0.StartsWith("else", StringComparison.Ordinal) |
                    Regex.IsMatch(lines[0],"#[ ]*else") )                     
                {
                    
                    offsetHorizontal = 300;
                    guardString = "no";
                    Match matchElseIf = Regex.Match(line0, @"^else[\s]*if");
                    if (matchElseIf.Success)  {
                        offsetHorizontal = 0;
                        //lines[0] = lines[0].Replace(matchElseIf.Value, "");
                    } else {
                    skipFirstLine = true;
                    }
                }
            }
            int lineNumber = -1;

            foreach (string s in lines)
            {
                lineNumber += 1;
                s1 = s.Trim();
                // case: of switch case
                if (isSwitchCase & (s1.StartsWith("case", StringComparison.Ordinal) | s1.StartsWith("default", StringComparison.Ordinal)))
                {   // set the selected element to the switch case
                    int l = dia.SelectedObjects.Count;
                    for (int i = l - 1; i >= 0; i--)
                    {
                        dia.SelectedObjects.DeleteAt((short)i, true);
                        dia.SelectedObjects.Refresh();
                    }
                    dia.SelectedObjects.Refresh();
                    
                    foreach (EA.DiagramObject obj in dia.SelectedObjects)
                    {
                        dia.SelectedObjects.DeleteAt(0, true);
                        //int elementID = obj.ElementID;
                        
                    }
                    dia.SelectedObjects.Refresh();
                    dia.SelectedObjects.AddNew(objSource.ElementID.ToString(), objSource.ObjectType.ToString());
                    dia.SelectedObjects.Refresh();
                }
                if (skipFirstLine) {
                    skipFirstLine = false;
                    // check if this was the only line
                    if (lines.Length == 1)
                    {
                        // switch case with only one line
                        if (lines[0].Contains("case"))
                        {
                            createActionFromText(rep, s1, offsetHorizontal, offsetVertical, guardString);
                        }
                    }
                    continue;
                }
               
                // check if do, while, for, #if, #else, #endif, #elseif
                if (    Regex.IsMatch(s1, @"^(if|else|else[\s]if|switch[\s]*\()") |
                        Regex.IsMatch(s1, @"#[ ]*(if|endif|else|ifdef|elseif)")  )
                {
                    createDecisionFromText(rep, s1, offsetHorizontal, offsetVertical, guardString);
                    offsetHorizontal = 0;
                    guardString = "";
                }
                    
                else
                {
                    if (s1.Length > 1)
                    {
                        if (s1.StartsWith("case", StringComparison.Ordinal) | s1.StartsWith("default", StringComparison.Ordinal))
                        {
                            if (s1.StartsWith("case", StringComparison.Ordinal)) s1 = s1.Substring(4);
                            guardString = s1.Replace(":", "").Trim();
                            offsetHorizontal = 300;
                            offsetVertical = offsetVertical - 80; // room for two actions (40 = one action)
                        }
                        else
                        {
                            if (s1.StartsWith("break;", StringComparison.Ordinal) & isSwitchCase & lineNumber > 0)
                            {
                                string s2 = lines[lineNumber - 1].Trim();
                                if (s2.StartsWith("case", StringComparison.Ordinal) | s2.StartsWith("default", StringComparison.Ordinal))
                                    s1 = "nothing to do";
                                else s1 = "";
                            } 
                            if ( ! s1.Equals(""))  createActionFromText(rep, s1, offsetHorizontal, offsetVertical, guardString);
                            offsetHorizontal = 0;
                            guardString = "";

                        }
                       
                       
                    }
                }
            }
        }
        static string deleteCurleyBrackets(string s)
        {
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           s = Regex.Replace(s, @"{[^{}]*}", "", RegexOptions.Multiline);
           return s;
        }

        #region deleteComment
        /// <summary>
        /// Delete Comment for C like languages.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string deleteComment(string s)
        {
            // delete comments /* to */

            //s = Regex.Replace(s, @"/\*[^\n]*\*/", "", RegexOptions.Singleline);
            // ? for greedy behavior (find shortest matching string)
            s = Regex.Replace(s, @"/\*.*?\*/", "", RegexOptions.Singleline);
            // delete comments //....
            s = Regex.Replace(s, @"//[^\n]*", "\r\n");
            // delete comments /*....
            s = Regex.Replace(s, @"/\*[^\n]*", "\r\n");
            // delete empty lines
            s = Regex.Replace(s, "(\r\n){2,100}", "\r\n");
            return s;
        }
        #endregion
        #region createActionFromText
        private static void createActionFromText(EA.Repository rep, string s1, int offsetHorizental = 0, int offsetVertical = 0, string guardString = "", 
            bool removeModuleNameFromMethodName= false)
        {
            // check if return
            Match matchReturn = Regex.Match(s1, @"\s*return\s*([^;]*);");
            if (matchReturn.Success)
            {
                string returnValue = "";
                if (matchReturn.Groups.Count == 2) returnValue = matchReturn.Groups[1].Value;
                createDiagramObjectFromContext(rep, returnValue, "StateNode", "101", 0,0, "");
            }
                // single "case"==> composite activity  
            else if (s1.Contains("case") ){
                s1 = CallOperationAction.removeUnwantedStringsFromText(s1);
                createDiagramObjectFromContext(rep, s1, "Activity", "comp=yes", offsetHorizental, offsetVertical, guardString);
            }
            else if (Regex.IsMatch(s1, @"^(for|while|do[\s]*$)"))
            {
                s1 = CallOperationAction.removeUnwantedStringsFromText(s1);
                createDiagramObjectFromContext(rep, s1, "Activity", "comp=no", offsetHorizental, offsetVertical, guardString);
            }
            else
            {
                string methodString = CallOperationAction.removeUnwantedStringsFromText(s1);
                string methodName = CallOperationAction.getMethodNameFromCallString(methodString);
                // remove module name from method name (text before '_')
                if (removeModuleNameFromMethodName)
                {
                    methodString = CallOperationAction.removeModuleNameFromCallString(methodString);
                }

                // check if function is available
                if (methodName != "")
                {
                    if (hoTools.Utils.CallOperationAction.getMethodFromMethodName(rep, methodName) == null)
                    {
                        createDiagramObjectFromContext(rep, methodString, "Action", "", offsetHorizental, offsetVertical, guardString);
                    }
                    else
                    {
                        createDiagramObjectFromContext(rep, methodString, "CallOperation", methodName, offsetHorizental, offsetVertical, guardString);
                    }
                }
                else { createDiagramObjectFromContext(rep, methodString, "CallOperation", methodName, offsetHorizental, offsetVertical, guardString);}
            }
            
        }
        #endregion
        #region createDecisionFromText
        public static string createDecisionFromText(EA.Repository rep, string decisionName, int offsetHorizental = 0, int offsetVertical = 0, string guardString = "")
        {
            decisionName = CallOperationAction.removeUnwantedStringsFromText(decisionName);
            string loops = "for, while, switch";
            if (!loops.Contains(decisionName.Substring(0, 3)))
            {
                decisionName = CallOperationAction.removeFirstParenthesisPairFromString(decisionName);
                decisionName = CallOperationAction.addQuestionMark(decisionName);
            }
            Match match = Regex.Match(decisionName, @"else[\s]*if");
            if (match.Success)
            {
                decisionName = decisionName.Replace(match.Value, "");
                guardString = "no";
            }
            if (decisionName.StartsWith("if", StringComparison.Ordinal)) {
                decisionName = decisionName.Substring(2).Trim();
            }
            
            if (Regex.IsMatch(decisionName, @"#[ ]*endif") ) {
                createDiagramObjectFromContext(rep, decisionName, "MergeNode", "", offsetHorizental, offsetVertical, guardString);
            }else
            {
                createDiagramObjectFromContext(rep, decisionName, "Decision", "", offsetHorizental, offsetVertical, guardString);
            }
            return decisionName;
        }
        #endregion
        #region addElementNote
        public static void addElementNote(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            EA.Element el = el = Util.getElementFromContextObject(rep);
            if (! (el == null))
            {
                EA.Diagram dia = rep.GetCurrentDiagram();
                EA.Package pkg = rep.GetPackageByID(el.PackageID);
                if (pkg.IsProtected || dia.IsLocked || el.Locked || dia == null) return;

                // save diagram
                rep.SaveDiagram(dia.DiagramID);

                EA.Element elNote = null;
                try
                {
                    elNote = (EA.Element)pkg.Elements.AddNew("", "Note");
                    elNote.Update();
                    pkg.Update();
                }
                catch { return; }

                // add element to diagram
                // "l=200;r=400;t=200;b=600;"

                EA.DiagramObject diaObj = getDiagramObjectFromElement(el, dia);
               
                int left = diaObj.right + 50;
                int right = left + 100;
                int top = diaObj.top;
                int bottom = top - 100;

                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
                var diaObject = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
                dia.Update();
                diaObject.ElementID = elNote.ElementID;
                diaObject.Sequence = 1; // put element to top
                diaObject.Update();
                pkg.Elements.Refresh();
                // make a connector
                var con = (EA.Connector)el.Connectors.AddNew("test", "NoteLink");
                con.SupplierID = elNote.ElementID;
                con.Update();
                el.Connectors.Refresh();
                Util.setElementHasAttchaedLink(rep, el, elNote);
                rep.ReloadDiagram(dia.DiagramID);
            } 
            else if (oType.Equals(EA.ObjectType.otConnector))
            {

            }
            return;
        }
        #endregion

        public static EA.DiagramObject getDiagramObjectFromElement(EA.Element el, EA.Diagram dia)
        {
            // get the position of the Element
            EA.DiagramObject diaObj = null;
            foreach (EA.DiagramObject dObj in dia.DiagramObjects)
            {
                if (dObj.ElementID == el.ElementID)
                {
                    diaObj = dObj;
                    break;
                }
            }
            return diaObj;
        }
        #region UpdateActivityParameter
        public static void UpdateActivityParameter(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(EA.ObjectType.otElement))
            {

                var el = (EA.Element)rep.GetContextObject();
                if (el.Type.Equals("Activity"))
                {
                    // get the associated operation
                    EA.Method m = Util.getOperationFromBrehavior(rep, el);
                    if (el.Locked) return;
                    if (m == null) return;
                    ActivityPar.updateParameterFromOperation(rep, el, m);// get parameters from Operation for Activity
                    EA.Diagram dia = rep.GetCurrentDiagram();
                    if (dia == null) return;
                   
                    EA.DiagramObject diaObj = Util.getDiagramObjectByID(rep, dia, el.ElementID);
                    //EA.DiagramObject diaObj = dia.GetDiagramObjectByID(el.ElementID,"");
                    if (diaObj == null) return;
                    
                    int pos = 0;
                    rep.SaveDiagram(dia.DiagramID);
                    foreach (EA.Element actPar in el.EmbeddedElements)
                    {
                        if (!actPar.Type.Equals("ActivityParameter")) continue;
                        Util.visualizePortForDiagramobject(pos, dia, diaObj, actPar, null);
                        pos = pos + 1;
                    }
                    rep.ReloadDiagram(dia.DiagramID);
                }
                if (el.Type.Equals("Class") | el.Type.Equals("Interface"))
                {
                    updateActivityParameterForElement(rep, el);
                }
            }
            if (oType.Equals(EA.ObjectType.otMethod))
            {
                var m = (EA.Method)rep.GetContextObject();
                EA.Element act = Appl.getBehaviorForOperation(rep, m);
                if (act == null) return;
                if (act.Locked) return;
                ActivityPar.updateParameterFromOperation(rep, act, m);// get parameters from Operation for Activity
            }
            if (oType.Equals(EA.ObjectType.otPackage))
            {
                var pkg = (EA.Package)rep.GetContextObject();
                updateActivityParameterForPackage(rep, pkg);
            }
        }
        #endregion

        private static void updateActivityParameterForElement(EA.Repository rep, EA.Element el)
        {
            foreach (EA.Method m in el.Methods)
            {
                EA.Element act = Appl.getBehaviorForOperation(rep, m);
                if (act == null) continue;
                if (act.Locked) continue;
                ActivityPar.updateParameterFromOperation(rep, act, m);// get parameters from Operation for Activity
            }
            foreach (EA.Element elSub in el.Elements)
            {
                updateActivityParameterForElement(rep, elSub);
            }
        }
        public static bool updateActivityParameterForPackage(EA.Repository rep, EA.Package pkg)
        {
            foreach (EA.Element el in pkg.Elements)
            {
                updateActivityParameterForElement(rep, el);
            }
            foreach (EA.Package pkgSub in pkg.Packages)
            {
                // update all packages
                updateActivityParameterForPackage(rep, pkgSub);
            }
            return true;

        }

        public static void locateType(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            EA.Element el = null;
            int id = 0;
            string trigger_GUID = "";
            // connector
            // links to trigger
            switch (oType)
            {

                case EA.ObjectType.otConnector:
                    var con = (EA.Connector)rep.GetContextObject();
                    trigger_GUID = Util.getTrigger(rep, con.ConnectorGUID);
                    if (trigger_GUID.StartsWith("{", StringComparison.Ordinal) && trigger_GUID.EndsWith("}", StringComparison.Ordinal))
                    {
                        EA.Element trigger = rep.GetElementByGuid(trigger_GUID);

                        if (trigger != null) rep.ShowInProjectView(trigger);

                    }
                    break;


                case EA.ObjectType.otMethod:
                    var m = (EA.Method)rep.GetContextObject();
                    if (m.ClassifierID != "")
                    {
                        id = Convert.ToInt32(m.ClassifierID);
                        // get type
                        if (id > 0)
                        {
                            el = rep.GetElementByID(id);
                            rep.ShowInProjectView(el);
                        }
                    }
                    break;

                case EA.ObjectType.otAttribute:
                    var attr = (EA.Attribute)rep.GetContextObject();
                    id = attr.ClassifierID;
                    // get type
                    if (id > 0)
                    {
                        el = rep.GetElementByID(attr.ClassifierID);
                        if (el.Type.Equals("Package"))
                        {
                            EA.Package pkg = rep.GetPackageByID(Convert.ToInt32(el.MiscData[0]));
                            rep.ShowInProjectView(pkg);
                        }
                        else
                        {
                            rep.ShowInProjectView(el);
                        }
                    }
                    break;

                // Locate Diagram (e.g. from Search Window)
                case EA.ObjectType.otDiagram:
                    var d = (EA.Diagram)rep.GetContextObject();
                    rep.ShowInProjectView(d);
                    break;


                case EA.ObjectType.otElement:
                    el = (EA.Element)rep.GetContextObject();
                    if (el.ClassfierID > 0)
                    {
                        el = rep.GetElementByID(el.ClassfierID);
                        rep.ShowInProjectView(el);
                        return;
                    }
                    else
                    {//MiscData(0) PDATA1,PDATA2,
                        // pdata1 GUID for parts, UmlElement
                        // object_id   for text with Hyper link to diagram

                        // locate text or frame
                        if (locateTextOrFrame(rep, el)) return;

                        string GUID = el.get_MiscData(0);
                        if (GUID.EndsWith("}", StringComparison.Ordinal))
                        {
                            el = rep.GetElementByGuid(GUID);
                            rep.ShowInProjectView(el);
                        }
                        else
                        {
                            if (el.Type.Equals("Action"))
                            {
                                foreach (EA.CustomProperty custproperty in el.CustomProperties)
                                {
                                    if (custproperty.Name.Equals("kind") && custproperty.Value.Contains("AcceptEvent"))
                                    {
                                        // get the trigger
                                        trigger_GUID = Util.getTrigger(rep, el.ElementGUID);
                                        if (trigger_GUID.StartsWith("{", StringComparison.Ordinal) && trigger_GUID.EndsWith("}", StringComparison.Ordinal))
                                        {
                                            EA.Element trigger = rep.GetElementByGuid(trigger_GUID);
                                            rep.ShowInProjectView(trigger);
                                            break;
                                        }
                                    }


                                }
                            }
                            if (el.Type.Equals("Trigger"))
                            {
                                // get the signal
                                string signal_GUID = Util.getSignal(rep, el.ElementGUID);
                                if (signal_GUID.StartsWith("RefGUID={", StringComparison.Ordinal))
                                {
                                    EA.Element signal = rep.GetElementByGuid(signal_GUID.Substring(8, 38));
                                    rep.ShowInProjectView(signal);
                                }
                            }

                            if (el.Type.Equals("RequiredInterface") || el.Type.Equals("ProvidedInterface"))
                            {
                                rep.ShowInProjectView(el);
                            }

                        }


                    }
                    break;

                case EA.ObjectType.otPackage:
                    var pkgSrc = (EA.Package)rep.GetContextObject();
                    EA.Package pkgTrg = Util.getModelDocumentFromPackage(rep, pkgSrc);
                    if (pkgTrg != null) rep.ShowInProjectView(pkgTrg);
                    break;
            }



        }
        public static void createNoteFromText(EA.Repository rep, string text)
        {
            if (rep.GetContextItemType().Equals(EA.ObjectType.otElement))
            {
                var el = (EA.Element)rep.GetContextObject();
                string s0 = CallOperationAction.removeUnwantedStringsFromText(text.Trim(), false);
                s0 = Regex.Replace(s0, @"\/\*", "//"); // /* ==> //
                s0 = Regex.Replace(s0, @"\*\/", "");   // delete */
                el.Notes = s0;
                el.Update();
            }
        }

        public static void getVcLatestRecursive(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(EA.ObjectType.otPackage) || oType.Equals(EA.ObjectType.otNone))
            {
                // start preparation
                int count = 0;
                int errorCount = 0;
                DateTime startTime = DateTime.Now;

                rep.CreateOutputTab("Debug");
                rep.EnsureOutputVisible("Debug");
                rep.WriteOutput("Debug", "Start GetLatestRecursive", 0);
                var pkg = (EA.Package)rep.GetContextObject();
                Util.getLatest(rep, pkg, true, ref count, 0, ref errorCount);
                string s = "";
                if (errorCount > 0) s = " with " + errorCount + " errors";

                // finished
                TimeSpan span = DateTime.Now - startTime;
                rep.WriteOutput("Debug", "End GetLatestRecursive in " + span.Hours + ":" + span.Minutes + " hh:mm. " + s, 0);

            }
        }
        public static void copyGuidSqlToClipboard(EA.Repository rep)
        {
            string str = "";
            string str1 = "";
            string str2 = "";
            EA.ObjectType oType = rep.GetContextItemType();
            EA.Diagram diaCurrent = rep.GetCurrentDiagram();
            EA.Connector conCurrent = null;
            EA.Element el = null;

            if (diaCurrent != null)
            {
                conCurrent = diaCurrent.SelectedConnector;
            }

            if (conCurrent != null)
            {// Connector 
                EA.Connector con = conCurrent;
                str = con.ConnectorGUID + " " + con.Name + ' ' + con.Type + "\r\n" +

                    "\r\n Connector: Select ea_guid As CLASSGUID, connector_type As CLASSTYPE,* from t_connector con where ea_guid = '" + con.ConnectorGUID + "'" +

                    "\r\n\r\nSelect o.ea_guid As CLASSGUID, o.object_type As CLASSTYPE,o.name As Name, o.object_type AS ObjectType, o.PDATA1, o.Stereotype, " +
                    "\r\n                       con.Name, con.connector_type, con.Stereotype, con.ea_guid As ConnectorGUID, dia.Name As DiagrammName, dia.ea_GUID As DiagramGUID," +
                    "\r\n                       o.ea_guid, o.Classifier_GUID,o.Classifier " +
                "\r\nfrom (((t_connector con INNER JOIN t_object o              on con.start_object_id   = o.object_id) " +
                "\r\nINNER JOIN t_diagramlinks diaLink  on con.connector_id      = diaLink.connectorid ) " +
                "\r\nINNER JOIN t_diagram dia           on diaLink.DiagramId     = dia.Diagram_ID) " +
                "\r\nINNER JOIN t_diagramobjects diaObj on diaObj.diagram_ID     = dia.Diagram_ID and o.object_id = diaObj.object_id " +
                "\r\nwhere         con.ea_guid = '" + con.ConnectorGUID + "' " +
                "\r\nAND dialink.Hidden  = 0 ";
                Clipboard.SetText(str);
                return;
            }
            if (oType.Equals(EA.ObjectType.otElement))
            {// Element 
                el = (EA.Element)rep.GetContextObject();
                string PDATA1 = el.get_MiscData(0);
                string PDATA1string = "";
                if (PDATA1.EndsWith("}", StringComparison.Ordinal))
                {
                    PDATA1string = "/" + PDATA1;
                }
                else
                {
                    PDATA1 = "";
                    PDATA1string = "";
                }
                string classifier = Util.getClassifierGUID(rep, el.ElementGUID);
                str = el.ElementGUID + ":" + classifier + PDATA1string + " " + el.Name + ' ' + el.Type + "\r\n" +
                 "\r\nSelect ea_guid As CLASSGUID, object_type As CLASSTYPE,* from t_object o where ea_guid = '" + el.ElementGUID + "'";
                if (classifier != "")
                {
                    if (el.Type.Equals("ActionPin"))
                    {
                        str = str + "\r\n Type:\r\nSelect ea_guid As CLASSGUID, 'Parameter' As CLASSTYPE,* from t_operationparams op where ea_guid = '" + classifier + "'";
                    }
                    else
                    {
                        str = str + "\r\n Type:\r\nSelect ea_guid As CLASSGUID, object_type As CLASSTYPE,* from t_object o where ea_guid = '" + classifier + "'";
                    }
                }
                if (PDATA1 != "")
                {
                    str = str + "\r\n PDATA1:  Select ea_guid As CLASSGUID, object_type As CLASSTYPE,* from t_object o where ea_guid = '" + PDATA1 + "'";
                }

                // Look for diagram object
                EA.Diagram curDia = rep.GetCurrentDiagram();
                if (curDia != null)
                {
                    foreach (EA.DiagramObject diaObj in curDia.DiagramObjects)
                    {
                        if (diaObj.ElementID == el.ElementID)
                        {
                            str = str + "\r\n\r\n" +
                                "select * from t_diagramobjects where object_id = " + diaObj.ElementID;
                            break;

                        }
                    }
                }

            }

            if (oType.Equals(EA.ObjectType.otDiagram))
            {// Element 
                var dia = (EA.Diagram)rep.GetContextObject();
                str = dia.DiagramGUID + " " + dia.Name + ' ' + dia.Type + "\r\n" +
                       "\r\nSelect ea_guid As CLASSGUID, diagram_type As CLASSTYPE,* from t_diagram dia where ea_guid = '" + dia.DiagramGUID + "'";
            }
            if (oType.Equals(EA.ObjectType.otPackage))
            {// Element 
                var pkg = (EA.Package)rep.GetContextObject();
                str = pkg.PackageGUID + " " + pkg.Name + ' ' + " Package " + "\r\n" +
                 "\r\nSelect ea_guid As CLASSGUID, 'Package' As CLASSTYPE,* from t_package pkg where ea_guid = '" + pkg.PackageGUID + "'";

            }
            if (oType.Equals(EA.ObjectType.otAttribute))
            {// Element 
                str1 = "LEFT JOIN  t_object typAttr on (attr.Classifier = typAttr.object_id)";
                if (rep.ConnectionString.Contains(".eap"))
                {
                    str1 = "LEFT JOIN  t_object typAttr on (attr.Classifier = Format(typAttr.object_id))";

                }
                var attr = (EA.Attribute)rep.GetContextObject();
                str = attr.AttributeID + " " + attr.Name + ' ' + " Attribute " + "\r\n" +
                      "\r\n " +
                      "\r\nSelect ea_guid As CLASSGUID, 'Attribute' As CLASSTYPE,* from t_attribute attr where ea_guid = '" + attr.AttributeGUID + "'" +
                        "\r\n Class has Attributes:" +
                        "\r\nSelect attr.ea_guid As CLASSGUID, 'Attribute' As CLASSTYPE, " +
                        "\r\n       o.Name As Class, o.object_type, " +
                        "\r\n       attr.Name As AttrName, attr.Type As Type, " +
                        "\r\n       typAttr.Name " +
                        "\r\n   from (t_object o INNER JOIN t_attribute attr on (o.object_id = attr.object_id)) " +
                        "\r\n                   " + str1 +
                        "\r\n   where attr.ea_guid = '" + attr.AttributeGUID + "'";
            }
            if (oType.Equals(EA.ObjectType.otMethod))
            {// Element 
                str1 = "LEFT JOIN t_object parTyp on (par.classifier = parTyp.object_id))";
                str2 = "LEFT JOIN t_object opTyp on (op.classifier = opTyp.object_id)";
                if (rep.ConnectionString.Contains(".eap"))
                {
                    str1 = " LEFT JOIN t_object parTyp on (par.classifier = Format(parTyp.object_id))) ";
                    str2 = " LEFT JOIN t_object opTyp  on (op.classifier  = Format(opTyp.object_id))";
                }

                var op = (EA.Method)rep.GetContextObject();
                str = op.MethodGUID + " " + op.Name + ' ' + " Operation " +
                      "\r\nOperation may have type " +
                      "\r\nSelect op.ea_guid As CLASSGUID, 'Operation' As CLASSTYPE,opTyp As OperationType, op.Name As OperationName, typ.Name As TypName,*" +
                      "\r\n   from t_operation op LEFT JOIN t_object typ on (op.classifier = typ.object_id)" +
                      "\r\n   where op.ea_guid = '" + op.MethodGUID + "';" +
                      "\r\n\r\nClass has Operation " +
                      "\r\nSelect op.ea_guid As CLASSGUID, 'Operation' As CLASSTYPE,* " +
                      "\r\n    from t_operation op INNER JOIN t_object o on (o.object_id = op.object_id)" +
                      "\r\n    where op.ea_guid = '" + op.MethodGUID + "';" +
                      "\r\n\r\nClass has Operation has Parameters/Typ and may have operationtype" +
                      "\r\nSelect op.ea_guid As CLASSGUID, 'Operation' As CLASSTYPE,op.Name As ClassName, op.Name As OperationName, opTyp.Name As OperationTyp, par.Name As ParName,parTyp.name As ParTypeName " +
                      "\r\n   from ((t_operation op INNER JOIN t_operationparams par on (op.OperationID = par.OperationID) )" +
                      "\r\n                        " + str1 +
                      "\r\n                        " + str2 +
                      "\r\n   where op.ea_guid = '" + op.MethodGUID + "' " +
                       "\r\n  Order by par.Pos ";

            }
            Clipboard.SetText(str);
        }
        #region createSharedMemoryFromText
        /// <summary>
        /// createSharedMemoryFromText
        /// 
        /// Create: Shared Memory as Class and interface
        ///         - Class Realize shared memory
        ///         - Class has port shared memory
        ///         Tagged values:
        ///         - StartAdress:
        ///         - EndAddress
        ///         
        /// </summary>
        /// <param name="rep">EA.Repository</param>
        /// <param name="txt">string</param>
        // #define SP_SHM_HW_MIC_START     0x40008000u
        // #define SP_SHM_HW_MIC_END       0x400083FFu
        public static void createSharedMemoryFromText(EA.Repository rep, string txt) {
            string shmStartAddr;
            string shmEndAddr;
            EA.TaggedValue tagStart = null;
            EA.TaggedValue tagEnd = null;

            EA.Package pkg = null;

            EA.Element shm = null;
            EA.Element Ishm = null;
            EA.ObjectType oType = rep.GetContextItemType();
            if (! oType.Equals(EA.ObjectType.otPackage)) return;
            pkg = (EA.Package)rep.GetContextObject();

            string regexShm = @"#define\sSP_SHM_(.*)_(START|END)\s*(0x[0-9ABCDEF]*)";
            Match matchShm = Regex.Match(txt, regexShm, RegexOptions.Multiline);
            while (matchShm.Success)
            {
                shm = Element.createElement(rep, pkg, matchShm.Groups[1].Value, "Class","shm");
                Ishm = Element.createElement(rep, pkg, "SHM_"+ matchShm.Groups[1].Value, "Interface", "");

                if (matchShm.Groups[2].Value == "START")
                {
                    shmStartAddr = matchShm.Groups[3].Value;
                    // add Tagged Value "StartAddr"
                    tagStart = TaggedValue.addTaggedValue(shm, "StartAddr");
                    tagStart.Value = shmStartAddr;
                    tagStart.Update();

                }else if (matchShm.Groups[2].Value == "END"){
                    shmEndAddr = matchShm.Groups[3].Value;
                    // add Tagged Value "StartAddr"
                    tagEnd = TaggedValue.addTaggedValue(shm, "EndAddr");
                    tagEnd.Value = shmEndAddr;
                    tagEnd.Update();
                }
                // make realize dependency from Interface to shared memory
                EA.Connector con = null;
                bool found = false;
                foreach (EA.Connector c in shm.Connectors)
                {
                    if (c.SupplierID == Ishm.ElementID & c.Type == "Realisation") {
                        found = false;
                        break;
                    }

                }
                if (found == false)
                {
                    con = (EA.Connector)shm.Connectors.AddNew("", "Realisation");
                    con.SupplierID = Ishm.ElementID;
                    try
                    {
                        con.Update();
                    }
                    catch
                    {
                        string s = rep.GetLastError();
                    }
                    shm.Connectors.Refresh();
        
                }
                // make a port with a provided interface
                Element.createPortWithInterface(shm, Ishm, "ProvidedInterface");



                matchShm = matchShm.NextMatch();

            }
        }
        #endregion
        #region createOperationsFromTextService
        [ServiceOperation("{E56C2722-605A-49BB-84FA-F3782697B6F9}", "Insert Operations in selected Class, Interface, Component", "Insert text with prototype(s)", IsTextRequired: true)]
         static public void createOperationsFromTextService(EA.Repository rep, string txt) {
         try
            {
                Cursor.Current = Cursors.WaitCursor;
                EaService.createOperationsFromText(rep, txt);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString() , "Error Insert Function");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
}
        #endregion
        public static void createOperationsFromText(EA.Repository rep, string txt)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            EA.Element el = Util.getElementFromContextObject(rep);
            if (el == null) return;

            if ( dia != null && dia.SelectedObjects.Count != 1)
            {
                dia = null;
            }

            if (!(dia == null)) rep.SaveDiagram(dia.DiagramID);
            // delete comment
            txt = deleteComment(txt);
            txt = deleteCurleyBrackets(txt);

            txt = txt.Replace(";", " ");
            // delete macros
            txt = Regex.Replace(txt, @"^[\s]*#[^\n]*\n", "",RegexOptions.Multiline);

            string[] l_txt = Regex.Split(txt, @"\)[\s]*\r\n");
            for (int i = 0; i < l_txt.Length; i++)
            {
                txt = l_txt[i].Trim();
                if (!txt.Equals(""))
                {
                    if (!txt.EndsWith(")", StringComparison.Ordinal)) txt = txt + ")";
                    
                    createOperationFromText(rep, el, txt);

                }
            }
            if (dia != null) rep.ReloadDiagram(dia.DiagramID);
            if (el != null)
            {
                dia.SelectedObjects.AddNew(el.ElementID.ToString(), el.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
            }

        }
        public static void createOperationFromText(EA.Repository rep, EA.Element el, 
                                string txt)
        {

            EA.Method m = null;
            string functionName = "";
            string parameters = "";
            string leftover = "";
            string functionType = "";
            int typeClassifier = 0;
            bool isStatic = false;
            // delete comment
            leftover = deleteComment(txt);



            // get function name
            string regex = @"[\s]+([a-zA-Z0-9_*]*)[\s]*\(";
            Match match = Regex.Match(leftover, regex, RegexOptions.Multiline);
            if (match.Success)
            {
                functionName = match.Groups[1].Value;
                leftover = leftover.Remove(match.Groups[1].Index, match.Groups[1].Length); // delete name
            }
            else
            {
                MessageBox.Show(txt, "No function definition");
                return;
            }
            // get parameters
            regex = @"\(([^\)]*)\)";
            match = Regex.Match(leftover, regex, RegexOptions.Multiline);
            if (match.Success)
            {
                parameters = match.Groups[1].Value;
                leftover = leftover.Remove(match.Index, match.Length); // delete name
                //leftover = leftover.Replace(match.Value, "").Trim();
            }
            if (leftover.ToLower().Contains("static "))
            {
                isStatic = true;
                leftover = leftover.Replace("static ", "").Trim();
                leftover = leftover.Replace("STATIC ", "").Trim();
            }

            // get type
            string pointer = "";
            if (leftover.Contains("*"))
            {
                pointer = "*";
                leftover = leftover.Replace("*","").Trim();
            }
            leftover = leftover.Trim();
            regex = @"[a-zA-Z_0-9*]*$";
            match = Regex.Match(leftover, regex, RegexOptions.Multiline);
            if (match.Success)
            {
                functionType = match.Value + pointer;
                leftover = leftover.Replace(functionType, "").Trim();
                // get classifierID of type
                typeClassifier = Util.getTypeFromName(rep, ref functionName, ref functionType);
                if (typeClassifier == 0 & functionType.Contains("*"))
                {
                    functionType = functionType.Remove(functionName.IndexOf("*", StringComparison.Ordinal), 1);
                    functionName = "*" + functionName;
                    typeClassifier = Util.getTypeID(rep, functionType);
                    if (typeClassifier == 0 & functionType.Contains("*"))
                    {
                        functionType = functionType.Replace("*", "");
                        functionName = "*" + functionName;
                        typeClassifier = Util.getTypeID(rep, functionType);
                    }
                }
            }
            // create function if not exists, else update function
            bool isNewFunctions = true;
            foreach (EA.Method m1 in el.Methods) {
                if (m1.Name == functionName)
                {
                    isNewFunctions = false;
                    m = m1;
                    // delete all parameters
                    for (short i = (short)(m.Parameters.Count-1); i >=0; i--)
                    {
                        m.Parameters.Delete(i);
                        m.Parameters.Refresh();
                    }
                }
            }

            if (isNewFunctions)
            {
                m = (EA.Method)el.Methods.AddNew(functionName, "");
                m.Pos = el.Methods.Count + 1;
                el.Methods.Refresh();
            }
            m.ReturnType = functionType;
            m.ClassifierID = typeClassifier.ToString();
            // static
            m.IsStatic = isStatic;
            if (el.Type.Equals("Interface")) m.Visibility = "public";
            else m.Visibility = "private";
            m.Update();

 
            el.Methods.Refresh();
            string[] lpar = parameters.Split(',');

            
            
            int pos = 1;
            foreach (string para in lpar)
            {
                string par = para.Trim();
                if (par == "void" | par == "") continue;
                int classifierID = 0;
                EA.Parameter elPar = null;
                bool isConst = false;
                if (par.Contains("const"))
                {
                    isConst = true;
                    par = par.Replace("const", "");
                }
                pointer = "";
                if (par.IndexOf("*", StringComparison.Ordinal) > -1)
                {
                    pointer = "*";
                    par = par.Remove(par.IndexOf("*", StringComparison.Ordinal), 1);
                    if (par.IndexOf("*", StringComparison.Ordinal) > -1)
                    {
                        pointer = "**";
                        par = par.Remove(par.IndexOf("*", StringComparison.Ordinal), 1);
                    }
                }
                par = Regex.Replace(par.Trim(),@"[\s]+", " ");
                string[] lparElements = par.Split(' ');
                if (lparElements.Length != 2)
                {
                    MessageBox.Show(txt, "Can't evaluate parameters");
                    return;

                }
                string name = lparElements[lparElements.Length - 1];
                string type = lparElements[lparElements.Length - 2]+pointer;
                
                // get classifier ID
                classifierID = Util.getTypeFromName(rep, ref name, ref type);

                elPar = (EA.Parameter)m.Parameters.AddNew(name, "");
                m.Parameters.Refresh();
                if (isConst) elPar.IsConst = true;
                else elPar.IsConst = false;

                elPar.Type = type;
                elPar.Kind = "in";
                elPar.Position = pos;
                elPar.ClassifierID = classifierID.ToString();

                int i = elPar.OperationID;
                try
                {
                    elPar.Update();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e + "\n\n"+elPar.GetLastError(), "Error creating parameter:" + type +" " + name);
                }
                pos = pos + 1;

            }
            return;
        }
        #region createTypeDefStructFromTextService
        [ServiceOperation("{6784026E-1B54-47CA-898F-A49EEB8A6ECB}", "Create/Update typedef for struct or enum from C-text for selected Class/Interface/Component", 
                "Insert text with typedef\nSelect Class to generate it beneath class\nSelect typedef to update it", IsTextRequired: true)]
        static public void createTypeDefStructFromTextService(EA.Repository rep, string txt)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EA.Diagram dia = rep.GetCurrentDiagram();
                if (dia == null) return;

                EA.Element el = null;
                EA.Package pkg = rep.GetPackageByID(dia.PackageID);
                if (dia.SelectedObjects.Count != 1) return;

                el = Util.getElementFromContextObject(rep);

                // delete comment
                txt = deleteComment(txt);

                // delete macros
                txt = Regex.Replace(txt, @"^[\s]*#[^\n]*\n", "", RegexOptions.Multiline);

                MatchCollection matches = Regex.Matches(txt, @".*?}[\s]*[A-Za-z0-9_]*[\s]*;", RegexOptions.Singleline);
                foreach (Match match in matches)
                {
                    createTypeDefStructFromText(rep, dia, pkg, el,match.Value);

                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), "Error insert Attributes");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        public static void createTypeDefStructFromText(EA.Repository rep, EA.Diagram dia, EA.Package pkg, EA.Element el, string txt)
        {
            
            EA.Element elTypedef = null;
           

            // delete comment
            txt = deleteComment(txt);

            bool update = false;
            bool isStruct = true;
            string elType = "Class";

            // find start
            string regex = @"[\s]*typedef[\s]+(struct|enum)[\s]*([^{]*){";
            Match match = Regex.Match(txt, regex);
            if (!match.Success) return;
            if (txt.Contains(" enum"))
            {
                elType = "Enumeration";
                isStruct = false;
            }
            txt = txt.Replace(match.Value, "");

            // find name
            regex = @"}[\s]*([^;]*);";
            match = Regex.Match(txt, regex);
            if (!match.Success) return;
            string name = match.Groups[1].Value.Trim();
            if (name == "") return;
            txt = txt.Remove(match.Index, match.Length); 
            
             // check if typedef already exists
            int targetID = Util.getTypeID(rep,name);
            if (targetID != 0)
            {
                elTypedef = rep.GetElementByID(targetID);
                update = true;
                for (int i = elTypedef.Attributes.Count - 1; i > -1; i = i - 1)
                {
                    elTypedef.Attributes.DeleteAt((short)i, true);
                }
                
            }


            // create typedef
            if (update == false) 
            {
                if (el != null ) { // create class below element
                   if ("Interface Class Component".Contains(el.Type))
                    {
                        elTypedef = (EA.Element)el.Elements.AddNew(name, elType);
                        el.Elements.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Can't create element below selected Element");
                    }
               
                }
                 else // create class in package
                { 
                    elTypedef = (EA.Element)pkg.Elements.AddNew(name, elType);
                    pkg.Elements.Refresh();

                }
            }
            if (isStruct)
            {
                elTypedef.Stereotype = "struct";
                EA.TaggedValue tag = TaggedValue.addTaggedValue(elTypedef, "typedef");
                tag.Value = "true";
                tag.Update();
            }
            if (update == false)
            {
                elTypedef.Name = name;
                elTypedef.Update();
            }

            // add elements
            if (isStruct) createClassAttributesFromText(rep, elTypedef, txt);
            else createEnumerationAttributesFromText(rep, elTypedef, txt);

            if (update == true)
            {
                rep.RefreshModelView(elTypedef.PackageID);
                rep.ShowInProjectView(elTypedef);
                
            }
            else
            {
                // put diagram object on diagram
                int left = 0;
                int right = left + 200;
                int top = 0;
                int bottom = top + 100;
                //int right = diaObj.right + 2 * (diaObj.right - diaObj.left);
                rep.SaveDiagram(dia.DiagramID);
                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
                var diaObj = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
                dia.DiagramObjects.Refresh();
                diaObj.ElementID = elTypedef.ElementID;
                diaObj.Update();
            }
            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(elTypedef.ElementID.ToString(), elTypedef.ObjectType.ToString());
            dia.SelectedObjects.Refresh();
            
                
        }
        #region insertAttributeService
        [ServiceOperation("{BE4759E5-2E8D-454D-83F7-94AA2FF3D50A}", "Insert/Update Attributes in Class, Interface, Component", "Insert text with variables, macros or enums", IsTextRequired: true)]
        public static void insertAttributeService(EA.Repository rep, string txt)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EaService.createAttributesFromText(rep, txt);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), "Error insert Attributes");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        #endregion
        public static void createAttributesFromText(EA.Repository rep, string txt)
        {
            EA.DiagramObject objSelected = null;
            EA.Element el = Util.getElementFromContextObject(rep);
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (!(dia == null && dia.SelectedObjects.Count > 0 ))
            {
                objSelected = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            }
 


            if (el == null ) {
                MessageBox.Show("No Element selected, probably nothing or an attribute / operation");
                return;
            }
            if (el.Type.Equals("Class") | el.Type.Equals("Interface")) createClassAttributesFromText(rep, el, txt);
            if (el.Type.Equals("Enumeration")) createEnumerationAttributesFromText(rep, el, txt);

            if (!(objSelected == null))
            {
                el = rep.GetElementByID(objSelected.ElementID);
                dia.SelectedObjects.AddNew(el.ElementID.ToString(), el.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
            }
        }
        
        public static void createClassAttributesFromText(EA.Repository rep, EA.Element el, string txt)
        {
            
            
            // delete comment
            txt = deleteComment(txt);

            // delete macro's like #if, #else, #end,..
            txt = Regex.Replace(txt, @"^[\s]*(#[\s]*if|#[\s]*else|#[\s]*end)[^\n]*\n", "", RegexOptions.Multiline);



            // get all lines
            string[] lines = Regex.Split(txt, "\r\n");
            //string[] lines = Regex.Split(txt, ";");
            string stereotype = "";
            string part1 = "";
            foreach (string s in lines)
            {


                string sRaw = part1 + " " + s.Trim();
                if (sRaw == "") continue;
                //sRaw = sRaw + ";";
                bool isConst = false;
                bool isStatic = false;
                string pointerValue = "";
                int pos;
                // remove everything behind ";"
                pos = sRaw.IndexOf(";", StringComparison.Ordinal);
                if (pos >=0 & pos >= sRaw.Length) sRaw = sRaw.Substring(pos+1);


                // remove everything behind "//"
                sRaw = Regex.Replace(sRaw, @"//.*", "");
                // remove everything behind "/*"
                sRaw = Regex.Replace(sRaw, @"/\*.*", "");
                sRaw = sRaw.Trim();
                if (sRaw == "") continue;
                sRaw = CallOperationAction.removeCasts(sRaw);

                

                if (sRaw.Equals("")) continue;
                if (sRaw.Contains("#") && sRaw.Contains("define") )
                {
                    createMacro(rep, el, sRaw, stereotype);
                    continue;
                }
               
                //-----------------------------------------------------------------------------
                // attributes
                // remove macros
                sRaw = Regex.Replace(sRaw, @"[\s]*#[^$]*", "");

                string name = "";
                string type = "";
                string prefix = "";
                string defaultValue = "";
                string collectionValue = "";
                EA.Attribute a = null;
                // Attributes over more than one line
                if (sRaw.Contains(";") || sRaw.Contains("MODULE_NAME("))
                {
                   part1 = "";
                }
                else
                {
                    part1 = part1 + " " + sRaw;
                    continue;
                }

                // Module-Name
                string regexModuleName = @"(MODULE_NAME)(\([^)]*\))";
                Match match = Regex.Match(sRaw, regexModuleName);
                if (match.Success)
                {
                    name = match.Groups[1].Value;
                    defaultValue = match.Groups[2].Value;
                    a = (EA.Attribute)el.Attributes.AddNew(name + defaultValue, "");
                    a.IsConst = isConst;
                    if (el.Type.Equals("Class")) a.Visibility = "Private";
                    else a.Visibility = "Public";

                    a.Pos = el.Attributes.Count + 1;
                    if (!stereotype.Contains("")) a.Stereotype = stereotype;
                    a.IsStatic |= prefix.ToLower().Contains("static");
                    a.Update();
                    continue;
                }
                
                pos = sRaw.IndexOf("*", StringComparison.Ordinal);
                if (pos > -1)
                {
                    sRaw = sRaw.Remove(pos, 1);
                    pointerValue = "*";
                }
                pos = sRaw.IndexOf("*", StringComparison.Ordinal);
                if (pos > -1)
                {
                    sRaw = sRaw.Remove(pos, 1);
                    pointerValue = "**";
                }

                isConst |= sRaw.Contains("const");
                isStatic |= sRaw.ToLower().Contains("static");


                string sCompact = sRaw.Replace("*", "");
                sCompact = sCompact.Replace("const", "");
                sCompact = sCompact.Replace("static ", "");
                sCompact = sCompact.Replace("STATIC ", "");

                // get name
               

                // get default value
                string regexDefault = @"=[\s]*([\(\)a-zA-Z0-9_ *+-{}\%]*)";
                match = Regex.Match(sCompact, regexDefault);
                if (match.Success)
                {
                    defaultValue = match.Groups[1].Value.Trim();
                    sCompact = sCompact.Remove(match.Groups[1].Index, match.Groups[1].Length); 
                    sCompact = sCompact.Replace("=", "");
                    if (!sCompact.EndsWith(";", StringComparison.Ordinal)) sCompact = sCompact + ";";
                }
                

                // get array
                //string regexArray = @"(\[[^;]*);";
                string regexArray = @"(\[[()A-Za-z0-9\]\[_ +-]*)";
                match = Regex.Match(sCompact, regexArray);
                if (match.Success)
                {
                    collectionValue = match.Groups[1].Value.Trim();
                    sCompact = sCompact.Remove(match.Groups[1].Index, match.Groups[1].Length); 
                    if (!sCompact.EndsWith(";", StringComparison.Ordinal)) sCompact = sCompact + ";";
                }

                

                string regexName = @"[\s]*([a-zA-Z_0-9]+)[\s]+([a-zA-Z_0-9:]+)[\s]*[\[;]";
                match = Regex.Match(sCompact, regexName);
                if (match.Success)
                {
                    name = match.Groups[2].Value.Trim();
                    type = match.Groups[1].Value.Trim();
                    sCompact = sCompact.Remove(match.Groups[2].Index, match.Groups[2].Length); // delete name
                    sCompact = sCompact.Remove(match.Groups[1].Index, match.Groups[1].Length); // delete type
                    sCompact = sCompact.Replace(";", ""); // ;
                    prefix = sCompact.Trim();



                    // add attribute
                    string aName = prefix + " " + pointerValue + name;
                    aName = aName.Trim();
                    bool isNewAttribute = true;
                    foreach (EA.Attribute attr in el.Attributes)
                    {
                        if (attr.Name == aName)
                        {
                            a = attr;
                            isNewAttribute = false;
                            break;
                        }
                    }

                    if (isNewAttribute)
                    {
                        a = (EA.Attribute)el.Attributes.AddNew(aName, "");
                        if (a.Name.Length > 255)
                        {
                            MessageBox.Show(a.Name + " has " + a.Name.Length, "Name longer than 255");
                            continue;
                        }
                        a.Pos = el.Attributes.Count+1;
                        el.Attributes.Refresh();
                    }

                    a.Type = type;
                    a.IsConst = isConst;
                    a.Default = defaultValue;
                    a.ClassifierID = Util.getTypeID(rep, type);
                    if (el.Type.Equals("Class")) a.Visibility = "Private";
                    else a.Visibility = "Public";
                    if (!collectionValue.Equals(""))
                    {
                        a.IsCollection = true;
                        a.Container = collectionValue;
                        if (collectionValue.Length > 50)
                        {
                            MessageBox.Show("Collection '" + collectionValue + "' has " + collectionValue.Length + " characters.", "Break! Collection length need to be <=50 characters");
                            continue;
                        }
                    }
                    if (!stereotype.Contains("")) a.Stereotype = stereotype;
                    a.IsStatic = isStatic;
                    try
                    {
                        a.Update();
                        el.Attributes.Refresh();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Error updating attribute");
                    }
                }
                else
                {
                    MessageBox.Show(s+"\n\n"+sCompact, "Couldn't understand attribute syntax");
                }
            }

        }
        #region createMacro
        public static void createMacro(EA.Repository rep, EA.Element el, string s, string stereotype) {

             string name = "";
             string value = "";
             EA.Attribute a = null;
             bool isNewAttribute = true;

             // delete spaces between parameters
            s = Regex.Replace(s, @",[\s]+",",");
            s = Regex.Replace(s, @"[\s]+,", ",");

             string regexDefine = @"#[\s]*define[\s]*([a-zA-Z0-9_(),]*)[\s]*(.*)";
                Match match = Regex.Match(s, regexDefine);
                if (match.Success)
                {
                    name = match.Groups[1].Value.Trim();
                    value = match.Groups[2].Value.Trim();
                }

             if ( ! name.Equals("")) 
             {
                 value = CallOperationAction.removeCasts(value);
                 foreach (EA.Attribute attr in el.Attributes)
                 {
                     if (attr.Name == name)
                     {
                         a = attr;
                         isNewAttribute = false;
                         break;
                     }
                 }
                 if (isNewAttribute)
                 {
                     a = (EA.Attribute)el.Attributes.AddNew(name, "");
                     a.Pos = el.Attributes.Count;
                     el.Attributes.Refresh();
                 }
                 a.Default = value;
                 if (el.Type.Equals("Interface")) a.Visibility = "public";
                 else a.Visibility = "private";
             
                 a.IsConst = true;
                 if (! stereotype.Equals("")) a.Stereotype = stereotype;
                 else a.Stereotype = "define";
                 a.ClassifierID = 0;
                 a.Type = "";
                 a.Update();
             } else {
                 MessageBox.Show(s,"Can't identify macro");
             }

        }
        #endregion
       
        public static void createEnumerationAttributesFromText(EA.Repository rep, EA.Element el, string txt)
        {
            // delete comment
            txt = deleteComment(txt);



            // check for (with or without comma):
            // abc = 5 ,           or
            // abc  ,
            string regexEnum = @"([a-zA-Z_0-9]+)[\s]*(=[\s]*([a-zA-Z_0-9| ]+)|,|$)";
            Match match = Regex.Match(txt, regexEnum, RegexOptions.Multiline);
            EA.Attribute a = null;
            int pos = 0;
            while (match.Success)
            {
                a = (EA.Attribute)el.Attributes.AddNew(match.Groups[1].Value, "");
                // with/without default value
                if (match.Groups[2].Value != ",") a.Default = match.Groups[3].Value;
                a.Stereotype = "enum";
                a.Pos = pos;
                a.Update();
                el.Attributes.Refresh();
                pos = pos + 1;
                match = match.NextMatch();
                

            }
            return;
        }
        public static void updateOperationTypeForPackage(EA.Repository rep, EA.Package pkg)
        {
            foreach (EA.Element el1 in pkg.Elements)
            {

                foreach (EA.Method m in el1.Methods)
                {
                    updateOperationType(rep, m);
                }
            }
            foreach (EA.Package pkgSub in pkg.Packages)
            {
                updateOperationTypeForPackage(rep, pkgSub);
            }
        }

        // update the types of operations
        public static void updateOperationTypes(EA.Repository rep)
        {
            EA.ObjectType oType = rep.GetContextItemType();
            switch (oType)
            {
                case EA.ObjectType.otMethod:
                    updateOperationType(rep, (EA.Method)rep.GetContextObject());
                    break;
                case EA.ObjectType.otElement:
                    var el = (EA.Element)rep.GetContextObject();
                    if (el.Type == "Activity")
                    {
                        EA.Method m = Util.getOperationFromBrehavior(rep, el);
                        if (m == null)
                        {
                            MessageBox.Show("Activity hasn't an operation");
                            return;
                        }
                        updateOperationType(rep, m);
                    }
                    else
                    {
                        foreach (EA.Method m in el.Methods)
                        {
                            updateOperationType(rep, m);
                        }
                    }
                    break;

                case EA.ObjectType.otPackage:
                    var pkg = (EA.Package)rep.GetContextObject();
                    updateOperationTypeForPackage(rep, pkg);
                    break;
            }

        }

        #region updateOperationType
        /// <summary> 
        /// Update the types of the operation
        /// </summary>
        public static void updateOperationType(EA.Repository rep, EA.Method m)
        {
            // update method type
            string methodName = m.Name;
            string methodType = m.ReturnType;
            if (methodType == "") methodType = "void";
            int methodClassifierID = 0;
            if (m.ClassifierID != "")  methodClassifierID = Convert.ToInt32(m.ClassifierID);
            bool typeChanged = false;
            typeChanged = updateTypeName(rep, ref methodClassifierID, ref methodName, ref methodType);
            if (typeChanged)
            {
                if (methodType == "")
                {
                    MessageBox.Show("Method " + m.Name + " Type '" + m.ReturnType + "' ",
                        "Method type undefined");
                }
                else
                {
                    m.ClassifierID = methodClassifierID.ToString();
                    m.Name = methodName;
                    m.ReturnType = methodType;
                    m.Update();
                }
            }

            // update parameter
            // set parameter direction to "in"
            foreach (EA.Parameter par in m.Parameters)
            {
                bool parameterUpdated = false;
                if (!par.Kind.Equals("in"))
                {
                    par.Kind = "in";
                    parameterUpdated = true;
                }
                string parName = par.Name;
                string parType = par.Type;
                if (parType == "") parType = "void";
                int classifierID = 0;
                if (! par.ClassifierID.Equals("")) classifierID = Convert.ToInt32(par.ClassifierID);
                typeChanged = false;
                typeChanged = updateTypeName(rep,ref classifierID , ref parName, ref parType);
                if (typeChanged)
                {
                    if (parType == "")
                    {
                        MessageBox.Show("Method " + m.Name + " Parameter '" + par.Name + ":" + par.Type + "' ",
                            "Parameter type undefined");
                    }
                    else
                    {
                        par.ClassifierID = classifierID.ToString();
                        par.Name = parName;
                        par.Type = parType;
                        parameterUpdated = true;
                    }
                }
                if (parameterUpdated) par.Update();

            }
        }
        #endregion

        private static bool updateTypeName( EA.Repository rep, ref int classifierID, ref string parName, ref string parType)
        {
            
            // no classifier defined
            // check if type is correct
            EA.Element el = null;
            if (!classifierID.Equals(0))
            {
                try
                {
                    el = rep.GetElementByID(classifierID);
                    if (el.Name != parType) el = null;
                }
                // empty catch, el = null
                #pragma warning disable RECS0022
                catch //(Exception e)
                { }
                #pragma warning restore RECS0022

            }

            if (el == null)
            {

                // get the type
                // find type from type_name
                classifierID = Util.getTypeID(rep, parType);
                // type not defined
                if (classifierID == 0)
                {
                    if (parType.EndsWith("*", StringComparison.Ordinal))
                    {
                        parType = parType.Substring(0, parType.Length - 1);
                        parName = "*" + parName;

                    }
                    if (parType.EndsWith("*", StringComparison.Ordinal))
                    {
                        parType = parType.Substring(0, parType.Length - 1);
                        parName = "*" + parName;

                    }
                    classifierID = Util.getTypeID(rep, parType);

                }

                if (classifierID != 0)
                {

                    return true;
                }
                else
                {

                    parType = "";
                    return true;
                }

            }
            else return false;
        }
        static public string getAssemblyPath() => Path.GetDirectoryName(
            Assembly.GetAssembly(typeof(EaService)).CodeBase);

        static public bool setNewXmlPath(EA.Repository rep)
        {
            if (rep.GetContextItemType().Equals(EA.ObjectType.otPackage))
            {
                var pkg  = (EA.Package)rep.GetContextObject();
                string guid = pkg.PackageGUID;

                string path = string.Empty;
                var openFileDialogXML = new OpenFileDialog();
                openFileDialogXML.Filter = "xml files (*.xml)|*.xml";
                openFileDialogXML.FileName = Util.getVccFilePath(rep, pkg);
                if (openFileDialogXML.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialogXML.FileName;
                    string rootPath = Util.getVccRootPath(rep, pkg);
                    // delete root path and an preceding '\'
                    string shortPath = path.Replace(rootPath, "");
                    if (shortPath.StartsWith(@"\", StringComparison.Ordinal)) shortPath = shortPath.Substring(1);
                    
                    // write to repository
                    Util.setXmlPath(rep, guid, shortPath);

                    // write to file
                    try
                    {
                        // set readonly attribute to false
                        File.SetAttributes(path, FileAttributes.Normal);

                        String strFile = File.ReadAllText(path);
                        string replace = @"value=[.0-9a-zA-Z_\\-]*\.xml";
                        string replacement = shortPath;
                        strFile = Regex.Replace(strFile, replace, replacement);
                        File.WriteAllText(path, strFile);

                        // checkout + checkin to make the change permanent
                        pkg.VersionControlCheckout();
                        pkg.VersionControlCheckin("Re- organization *.xml files");
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString(), "Error writing '"+path+"'"); 
                    }

                    MessageBox.Show(path , "Changed"); 
                }
               
            }
            return true;
        }
        #region VcReconcile
        [ServiceOperation("{EAC9246F-96FA-40E7-885A-A572E907AF86}", "Scan XMI and reconcile", "no selection required", false)]
        public static void VcReconcile(EA.Repository rep)
        {
                 //
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    rep.ScanXMIAndReconcile();
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e + "\n\n" , "Error VC reconcile");
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
        }
        #endregion
        #region checkOutService
        [ServiceOperation("{1BF01759-DD99-4552-8B68-75F19A3C593E}", "Check out", "Select Package",false)]
        public static void checkOutService(EA.Repository rep)
        {

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EaService.checkOut(rep);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error Checkout");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        #endregion

        public static void checkOut(EA.Repository rep,EA.Package pkg=null)
        {
            if (pkg == null) pkg = rep.GetTreeSelectedPackage();
            if (pkg == null) return;

            pkg = Util.getFirstControlledPackage(rep, pkg);
            if (pkg == null) return;

            var svnHandle = new svn(rep, pkg);
            string userNameLockedPackage = svnHandle.getLockingUser();
            svnHandle = null;
            if (userNameLockedPackage != "")
            {
                MessageBox.Show("Package is checked out by " + userNameLockedPackage);
                return;
            }

            //
            try 
            {
                Cursor.Current = Cursors.WaitCursor;
                pkg.VersionControlCheckout("");
                Cursor.Current = Cursors.Default;
            } catch (Exception e) 
            {
                MessageBox.Show(e + "\n\n"+ pkg.GetLastError(), "Error Checkout");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #region checkInService
        [ServiceOperation("{085C84D2-7B51-4783-8189-06E956411B94}", "Check in ", "Select package or something in package", false)]
        public static void checkInService(EA.Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EaService.checkIn(rep, pkg: null, withGetLatest: false, comment: "code changed");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error Checkin");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
        #region checkInServiceWithUpdateKeyword
        [ServiceOperation("{C5BB52C6-F300-42AE-B4DC-DC97D57D8F7D}", "Check in with get latest (update VC keywords, if Tagged Values 'svnDate'/'svnRevision')", "Select package or something in package", false)]
         public static void checkInServiceWithUpdateKeyword (EA.Repository rep) {
         try
            {
                Cursor.Current = Cursors.WaitCursor;
                EaService.checkIn(rep, pkg:null, withGetLatest: true, comment:"code changed");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error Checkin");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
         }
        #endregion
        #region checkIn
        /// <summary>
            /// Check in of a package. If there are the following package tagged values a get latest is performed to update keywords:
            /// -- svnDoc
            /// -- svnRevision
            /// </summary>
            /// <param name="rep">Repository</param>
            /// <param name="pkg">Package, default null</param>
            /// <param name="withGetLatest">false if you want to avoid a getLatest to update VC keywords
            /// Tagged Value "svnDoc" or "svnRevision" of package are true</param>
            /// <param name="comment">A checkin comment, default="0" = aks for checkin comment</param>
            public static void checkIn(EA.Repository rep, EA.Package pkg=null, bool withGetLatest = false, string comment="0")
        {
                
                if (pkg == null) pkg = rep.GetTreeSelectedPackage();
                if (pkg == null) return;

                pkg = Util.getFirstControlledPackage(rep, pkg);
                if (pkg == null) return;

                var svnHandle = new svn(rep, pkg);
                string userNameLockedPackage = svnHandle.getLockingUser();
                svnHandle = null;
                if (userNameLockedPackage == "")
                {
                    MessageBox.Show("Package isn't checked out");
                    return;
                }


                if (InputBox("Checkin comment", "Checkin", ref comment) == DialogResult.OK)
                {

                    //
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        pkg.VersionControlCheckin(comment);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e + "\n\n" + pkg.GetLastError(), "Error Checkin");
                        return;
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                       
                    }
                }
                if (withGetLatest)
                {
                    // check if GetLatest is appropriate
                    EA.Element el = rep.GetElementByGuid(pkg.PackageGUID);
                    foreach (EA.TaggedValue t in el.TaggedValues)
                    {
                        if (t.Name == "svnDoc" | t.Name == "svnRevision")
                        {

                            pkg.VersionControlResynchPkgStatus(false);
                            if (pkg.Flags.Contains("Checkout"))
                            {
                                MessageBox.Show("Flags=" + pkg.Flags, "Package Checked out, Break!");
                                return;

                            }
                            pkg.VersionControlGetLatest(true);
                            return;
                        }
                    }
                }
            
        }
        #endregion
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        static public bool checkTaggedValuePackage(EA.Package pkg)
        {
            bool workForPackage = false;
            foreach (EA.Package pkg1 in pkg.Packages)
            {
                if (pkg1.Name.Equals("Architecture") | pkg1.Name.Equals("Behavior"))
                {
                    workForPackage = true;
                    break;
                }
            }
            return workForPackage;
        }
        static public void setDirectoryTaggedValueRecursive(EA.Repository rep, EA.Package pkg)
        {
            // remember GUID, because of reloading package from xmi
            string pkgGUID = pkg.PackageGUID;
            if (checkTaggedValuePackage(pkg)) setDirectoryTaggedValues(rep, pkg);

            pkg = rep.GetPackageByGuid(pkgGUID);
            foreach (EA.Package pkg1 in pkg.Packages)
            {
                setDirectoryTaggedValueRecursive(rep, pkg1);
            }
               

        }
        public static void setTaggedValueGui(EA.Repository rep)
        {

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EA.ObjectType oType = rep.GetContextItemType();
                if (!oType.Equals(EA.ObjectType.otPackage)) return;
                var pkg = (EA.Package)rep.GetContextObject();
                EaService.setDirectoryTaggedValueRecursive(rep, pkg);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error set directory tagged values");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        static private bool isTaggedValuesComplete(EA.Element el)
        {
            bool isRevision = false;
            bool isDate = false;
            foreach (EA.TaggedValue tag in el.TaggedValues) {
                isRevision |= tag.Name == "svnRevision";
                isDate |= tag.Name == "svnDate";
            }
            if (isRevision & isDate) return true;
            else return false;
        }

        static public void setDirectoryTaggedValues(EA.Repository rep, EA.Package pkg) {
            bool withCheckIn = false;
            string guid = pkg.PackageGUID;

            EA.Element el = rep.GetElementByGuid(guid);
            if (isTaggedValuesComplete(el)) return;
            if (pkg.IsVersionControlled)
            {
                int state = pkg.VersionControlGetStatus();
                if (state == 4)
                {
                    MessageBox.Show("","Package checked out by another user, break");
                        return;
                }
                if (state == 1)// checked in
                {
                    EaService.checkOut(rep, pkg);
                    withCheckIn = true;
                }
            }
            pkg = rep.GetPackageByGuid(guid);
            setSvnProperty(rep, pkg);

            // set tagged values
            el = rep.GetElementByGuid(guid);
            bool createSvnDate = true;
            bool createSvnRevision = true;
            foreach (EA.TaggedValue t in el.TaggedValues)
            {
                createSvnDate &= t.Name != "svnDate";
                createSvnRevision &= t.Name != "svnRevision";

            }
            EA.TaggedValue tag = null;
            if (createSvnDate)
            {
                tag = (EA.TaggedValue)el.TaggedValues.AddNew("svnDate", "");
                tag.Value = "$Date: $";
                el.TaggedValues.Refresh();
                tag.Update();

            }
            if (createSvnRevision)
            {
                tag = (EA.TaggedValue)el.TaggedValues.AddNew("svnRevision", "");
                tag.Value = "$Revision: $";
                el.TaggedValues.Refresh();
                tag.Update();
            }


            if (pkg.IsVersionControlled)
            {
                int state = pkg.VersionControlGetStatus();
                if (state == 2 & withCheckIn)// checked out to this user
                {
                    //EaService.checkIn(rep, pkg, "");
                    EaService.checkIn(rep,pkg, withGetLatest:true, comment:"svn tags added");
                }
             }
        }

        public static void setSvnProperty(EA.Repository rep, EA.Package pkg)
        {
            // set svn properties
            if (pkg.IsVersionControlled)
            {
                var svnHandle = new svn(rep, pkg);
                svnHandle.setProperty();
                svnHandle = null;
            }
        }
        public static void gotoSvnLog(EA.Repository rep, EA.Package pkg)
        {
            // set svn properties
            if (pkg.IsVersionControlled)
            {
                var svnHandle = new svn(rep, pkg);
                svnHandle.gotoLog();
                svnHandle = null;
            }
        }
        public static void gotoSvnBrowser(EA.Repository rep, EA.Package pkg)
        {
            // set svn properties
            if (pkg.IsVersionControlled)
            {
                var svnHandle = new svn(rep, pkg);
                svnHandle.gotoRepoBrowser();
                svnHandle = null;
            }
        }

        #region insertDiagramElementAndConnect
        /// <summary>insertDiagramElement insert a diagram element and connects it to all selected diagramobject 
        /// <para>type: type of the node like "Action", Activity", "MergeNode"</para>
        ///       MergeNode may have the subType "no" to draw a transition with a "no" guard.
        /// <para>subTyp: subType of the node:
        ///       StateNode: 100=ActivityInitial, 101 ActivityFinal
        /// </para>guardString  of the connector "","yes","no",..
        ///        if "yes" or "" it will locate the node under the last selected element
        /// </summary> 
        public static void insertDiagramElementAndConnect(EA.Repository rep, string type, string subType, string guardString="") 
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (dia.Type != "Activity") return;

            int count = dia.SelectedObjects.Count;
            if (count == 0) return;

            rep.SaveDiagram(dia.DiagramID);
            var oldCollection = new List<EA.DiagramObject>();

            // get context element (last selected element)
            EA.Element originalSrcEl = Util.getElementFromContextObject(rep);
            if (originalSrcEl == null) return;
            int  originalSrcID =  originalSrcEl.ElementID;

            for (int i = count - 1; i > -1; i = i - 1)
            {
                oldCollection.Add((EA.DiagramObject)dia.SelectedObjects.GetAt((short)i));
                // keep last selected element
                //if (i > 0) dia.SelectedObjects.DeleteAt((short)i, true);
            }

            EA.DiagramObject originalSrcObj = Util.getDiagramObjectByID(rep, dia, originalSrcID);
            //EA.DiagramObject originalSrcObj = dia.GetDiagramObjectByID(originalSrcID, "");

            EA.DiagramObject trgObj = EaService.createDiagramObjectFromContext(rep, "", type, subType,0,0,guardString, originalSrcEl);
            EA.Element trgtEl = rep.GetElementByID(trgObj.ElementID);

            // if connection to more than one element make sure the new element is on the deepest position
            int offset = 50;
            if (guardString == "yes") offset = 0;
            int bottom = 1000;
            int diff = trgObj.top - trgObj.bottom;

            
            EA.DiagramObject srcObj = null;
            EA.DiagramLink link = null;
            foreach (EA.DiagramObject diaObj in oldCollection)
                {
                    EA.Element srcEl = rep.GetElementByID(diaObj.ElementID);
                    // don't connect two times
                    if (originalSrcID != diaObj.ElementID)
                    {
                        var con = (EA.Connector)srcEl.Connectors.AddNew("", "ControlFlow");
                        con.SupplierID = trgObj.ElementID;
                        if (type == "MergeNode" && guardString == "no" && srcEl.Type == "Decision") con.TransitionGuard = "no";
                        con.Update();
                        srcEl.Connectors.Refresh();
                        dia.DiagramLinks.Refresh();
                        //trgtEl.Connectors.Refresh();

                        // set line style
                        string style = "LV";
                        if ((srcEl.Type == "Action" | srcEl.Type == "Activity") & guardString == "no") style = "LH";
                        link = getDiagramLinkForConnector(dia, con.ConnectorID);
                        if (link != null) Util.setLineStyleForDiagramLink(style, link);

                    }
                    // set new high/bottom_Position
                    srcObj = Util.getDiagramObjectByID(rep, dia, srcEl.ElementID);
                    //srcObj = dia.GetDiagramObjectByID(srcEl.ElementID, "");
                    if (srcObj.bottom < bottom) bottom = srcObj.bottom;

            }
            if (oldCollection.Count > 1)
            {
                // set bottom/high of target
                trgObj.top = bottom + diff - offset;
                trgObj.bottom = bottom - offset;
                trgObj.Sequence = 1;
                trgObj.Update();
                // final
                if (subType == "101" && trgtEl.ParentID > 0)
                {
                    EA.Element parEl = rep.GetElementByID(trgtEl.ParentID);
                    if (parEl.Type == "Activity")
                    {
                        EA.DiagramObject parObj = Util.getDiagramObjectByID(rep, dia, parEl.ElementID);
                        //EA.DiagramObject parObj = dia.GetDiagramObjectByID(parEl.ElementID, "");
                        if (parObj != null)
                        {
                            parObj.bottom = trgObj.bottom - 30;
                            parObj.Update();
                        }

                    }
                }
            }

            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(trgtEl.ElementID.ToString(), trgtEl.ObjectType.ToString());
        }
        #endregion
        #region joinDiagramObjectsToLastSelected
        [ServiceOperation("{6946E63E-3237-4F45-B4D8-7EE0D6580FA5}", "Join nodes to the last selected node", "Only Activity Diagram", false)]
        public static void joinDiagramObjectsToLastSelected(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count < 2) return;
            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject srcObj;
            EA.Element srcEl;
            EA.Connector con;
            var trgEl = (EA.Element)rep.GetContextObject();
           
            for (int i = 0; i < count; i = i + 1)
            {
                srcObj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                srcEl = (EA.Element)rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                Connector connector = getConnectionDefault(dia);

                con = (EA.Connector)srcEl.Connectors.AddNew("", connector.Type);
                con.SupplierID = trgEl.ElementID;
                con.Stereotype = connector.Stereotype;
                con.Update();
                srcEl.Connectors.Refresh();
                trgEl.Connectors.Refresh();
                dia.DiagramLinks.Refresh();
                // set line style
                EA.DiagramLink link = getDiagramLinkForConnector(dia, con.ConnectorID);
                if (link != null) Util.setLineStyleForDiagramLink("LV", link);
               
            }
    
            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(trgEl.ElementID.ToString(), trgEl.ObjectType.ToString());
        }
        #endregion
        public static Connector getConnectionDefault(EA.Diagram dia) => new Connector("DataFlow", "");

        #region splitDiagramObjectsToLastSelected
        [ServiceOperation("{521FCFEB-984B-43F0-A710-E97C29E4C8EE}", "Split last selected Diagram object from previous selected Diagram Objects", "Incoming and outgoing connections", false)]
        public static void splitDiagramObjectsToLastSelected(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count < 2) return;
            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject srcObj;
            EA.Element srcEl;
            EA.ObjectType objType = rep.GetContextItemType();
            if (!(objType.Equals(EA.ObjectType.otElement))) return;
            var trgEl = (EA.Element)rep.GetContextObject();

            for (int i = 0; i < count; i = i + 1)
            {
                srcObj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                splitElementsByConnectorType(srcEl, trgEl, "ControlFlow");
            }

            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(trgEl.ElementID.ToString(), trgEl.ObjectType.ToString());
        }
        #endregion
        #region splitAllDiagramObjectsToLastSelected
        [ServiceOperation("{CA29CB67-77EA-4BCC-B3B4-8893F6B0DAE2}", "Split last selected Diagram object from all connected Diagram Objects", "Incoming and outgoing connections", false)]
        public static void splitAllDiagramObjectsToLastSelected(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.Element srcEl;
            EA.ObjectType objType = rep.GetContextItemType();
            if (!(objType.Equals(EA.ObjectType.otElement))) return;
            var trgEl = (EA.Element)rep.GetContextObject();

            foreach (EA.DiagramObject srcObj in dia.DiagramObjects)
            {
                srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                splitElementsByConnectorType(srcEl, trgEl);
            }

            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(trgEl.ElementID.ToString(), trgEl.ObjectType.ToString());
        }
        #endregion
        #region splitElementsByConnectorType
        /// <summary>
        /// Split / delete the connection of two elements
        /// </summary>
        /// <param name="srcEl">Source element of the connector</param>
        /// <param name="trgEl">Target element of the connector</param>
        /// <param name="connectorType">Connector type or default "All"</param>
        /// <param name="direction">Direction of connection ("in","out","all" or default "All"</param>
        public static void splitElementsByConnectorType(EA.Element srcEl, EA.Element trgEl, string connectorType="all", string direction="all")
        {
            EA.Connector con;
            for (int i = srcEl.Connectors.Count - 1;i >= 0; i=i-1 ) {
                con = (EA.Connector)srcEl.Connectors.GetAt((short)i);
                if (con.SupplierID == trgEl.ElementID && (con.Type == connectorType | connectorType == "all" | direction == "all" | direction == "in") )
                {
                    srcEl.Connectors.DeleteAt((short)i, true);
                }
                if (con.ClientID == trgEl.ElementID && (con.Type == connectorType | connectorType == "all" | direction == "all" | direction == "out"))
                {
                    srcEl.Connectors.DeleteAt((short)i, true);
                }
            }
        }
        #endregion
        public static void makeNested(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count < 2) return;

            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject srcObj;
            EA.Element srcEl;
            EA.Element trgEl;

            EA.ObjectType objType = rep.GetContextItemType();
            if (!(objType.Equals(EA.ObjectType.otElement))) return;

            trgEl = (EA.Element)rep.GetContextObject();
            if  (!(trgEl.Type.Equals("Activity"))) {
                MessageBox.Show("Target '" + trgEl.Name + ":" + trgEl.Type + "' isn't an Activity", " Only move below Activity is allowed");
                return;
            }
            var diaObj = new List<EA.DiagramObject>();
            for (int i = 0; i < count; i = i + 1)
            {
                srcObj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                srcEl.ParentID = trgEl.ElementID;
                srcEl.Update();
               
            }
           
        }
        public static void deleteInvisibleUseRealizationDependencies (EA.Repository rep)
        {
            EA.Element elSource = null;
            int elSourceID = 0;
            EA.Element elTarget = null;
            EA.DiagramObject diaObjSource = null;
            EA.Connector con = null;
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (!rep.GetContextItemType().Equals(EA.ObjectType.otElement)) return;

            // only one diagram object selected as source
            if (dia.SelectedObjects.Count != 1) return;

            rep.SaveDiagram(dia.DiagramID);
            diaObjSource = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            elSource = rep.GetElementByID(diaObjSource.ElementID);
            elSourceID = elSource.ElementID;
            if (! ("Interface Class".Contains(elSource.Type))) return;

            // list of all connectorIDs
            var l_InternalID = new List<int>();
            foreach (EA.DiagramLink link in dia.DiagramLinks)
            {
               con = rep.GetConnectorByID(link.ConnectorID);
               if (con.ClientID != elSourceID) continue;
               if (!("Usage Realisation".Contains(con.Type))) continue;
               l_InternalID.Add(con.ConnectorID);

            }


            string conType;

            for (int i = elSource.Connectors.Count - 1; i >=0; i = i - 1)
            {
                con = (EA.Connector)elSource.Connectors.GetAt((short)i);
                conType = con.Type;
                if ("Usage Realisation".Contains(conType))
                {
                    // check if target is..
                    elTarget = rep.GetElementByID(con.SupplierID);
                    if (elTarget.Type == "Interface")
                    {
                        if (l_InternalID.BinarySearch(con.ConnectorID) < 0)
                        {
                            elSource.Connectors.DeleteAt((short)i, true);
                            continue;
                               
                        }
                    }
                }
               
            }

            
        }
        #region copyReleaseInfoOfModuleService
        [ServiceOperation("{1C78E1C0-AAC8-4284-8C25-2D776FF373BC}", "Copy release information to clipboard", "Select Component", false)]
        public static void copyReleaseInfoOfModuleService(EA.Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                copyReleaseInfoOfModule(rep);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), "Error generating ports for component");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        #endregion
        public static void copyReleaseInfoOfModule(EA.Repository rep)
         {

             EA.Element elSource = null;
             EA.Element elTarget = null;
             EA.DiagramObject diaObjSource = null;
             EA.Diagram dia = rep.GetCurrentDiagram();
             if (dia == null) return;
             if (!rep.GetContextItemType().Equals(EA.ObjectType.otElement)) return;
             elSource = (EA.Element)rep.GetContextObject();
             if (elSource.Type != "Component") return;

             diaObjSource = Util.getDiagramObjectByID(rep, dia, elSource.ElementID);
             //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");

             string txt = "";
             string nl = "";
             foreach (EA.DiagramObject obj in dia.DiagramObjects)
             {
                 elTarget = rep.GetElementByID(obj.ElementID);
                 if (!("Class Interface".Contains(elTarget.Type))) continue;
                 txt = txt + nl + addReleaseInformation(rep, elTarget);
                 nl = "\r\n";
             }
             Clipboard.SetText(txt);
         }

        public static string addReleaseInformation(EA.Repository rep, EA.Element el) {
            string txt = "";
            string path = Util.getGenFilePath(rep, el);
            if (path == "")
            {
                MessageBox.Show("No file defined in property for: '" + el.Name + "':" + el.Type);
                return "";
            }
            try
            {
                txt = System.IO.File.ReadAllText(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error Reading file '" + el.Name + "':" + el.Type);
                return "";
            }
            string extension = ".c";
            if (el.Type == "Interface") extension = ".h";
            string name = el.Name + extension;
            if (name.Length > 58) name = name + "   ";
            else name = name.PadRight(60);
            return name + getReleaseInformationFromText(txt);
        }
        private static string getReleaseInformationFromText(string txt) {
            string patternRev = @"\$Rev(ision|):[\s]*([0-9]*)";
            string patternDate = @"\$Date:[\s][^\$]*";
            string txtResult = "";
            

            Match matchPath = Regex.Match(txt, patternRev, RegexOptions.Multiline);
            if (matchPath.Success)
            {
                txtResult = matchPath.Value;
            }
            Match matchDate = Regex.Match(txt, patternDate, RegexOptions.Multiline);
            if (matchDate.Success)
            {
                txtResult = txtResult + " " + matchDate.Value;
            }
            return txtResult;
        }
        #region generateComponentPortsService
        [ServiceOperation("{00602D5F-D581-4926-A31F-806F2D06691C}", "Generate ports for component", "Select Component", false)]
        public static void generateComponentPortsService(EA.Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                generateComponentPorts(rep);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), "Error generating ports for component");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        #endregion


        public static void generateComponentPorts(EA.Repository rep) {

            int pos = 0;
            EA.Element elSource = null;
            EA.Element elTarget = null;
            EA.DiagramObject diaObjSource = null;
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (!rep.GetContextItemType().Equals(EA.ObjectType.otElement)) return;
            elSource = (EA.Element)rep.GetContextObject();
            if (elSource.Type != "Component") return;

            diaObjSource = Util.getDiagramObjectByID(rep, dia, elSource.ElementID);
            //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");

            rep.SaveDiagram(dia.DiagramID);
            foreach (EA.DiagramObject obj in dia.DiagramObjects)
            {
                elTarget = rep.GetElementByID(obj.ElementID);
                if (! ("Class Interface".Contains(elTarget.Type))) continue;
                if (!(elTarget.Name.EndsWith("_i", StringComparison.Ordinal)))
                {
                    addPortToComponent(elSource, elTarget);
                    pos = pos + 1;
                }

                if ("Interface Class".Contains(elTarget.Type)) {
                    List<EA.Element> l_el =  getIncludedHeaderFiles(rep, elTarget);
                    foreach ( EA.Element el in l_el) {
                        if (el == null) continue; 
                        if (!(el.Name.EndsWith("_i", StringComparison.Ordinal)))
                        {
                            addPortToComponent(elSource, el);
                            pos = pos + 1;
                        }
                    }
                }
            }
            EaService.showEmbeddedElementsGUI(rep);

        }
        public static List<EA.Element> getIncludedHeaderFiles(EA.Repository rep, EA.Element el)
        {
            var l_el = new List<EA.Element>();
            string path = Util.getGenFilePath(rep, el);
            if (path == "")
            {
                MessageBox.Show("No file defined in property for: '" + el.Name + "':" + el.Type);
                return l_el;
            }
            string text;
            try
            {
                text = System.IO.File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Clipboard.SetText(e.ToString());
                MessageBox.Show(e + "\n\nsee Clipboard!", "Error Reading file '" + el.Name + "':" + el.Type);
                return l_el;
            }
            l_el = getInterfacesFromText(rep, null, text, false);
            
            return l_el;

        }
        #region vCGetState
        [ServiceOperation("{597608A2-5C3F-4AE6-9B18-86C1B3C27382}", "Get and update VC state of selected package", "Select Packages", false)]
        public static void vCGetState(EA.Repository rep)
        {
            EA.Package pkg = rep.GetTreeSelectedPackage();
            if (pkg != null)
            {
                if (pkg.IsControlled)
                {
                    string pkgState = Util.getVCstate(rep, pkg, true);
                     DialogResult result = MessageBox.Show("Package is "+ pkgState +"\nPath=" + pkg.XMLPath + "\nFlags=" + pkg.Flags, "Update package state?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)  Util.updateVC(rep, pkg);
                }
            }
            else MessageBox.Show("No package selected");
        }
        #endregion
        #region updateVcStateOfSelectedPackageRecursiveService
        [ServiceOperation("{A521EB65-3F3C-4C5D-9B82-D12FFCEC71D4}", "Update VC-State of package(recursive)", "Select Packages or model", false)]
        public static void updateVcStateOfSelectedPackageRecursiveService(EA.Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
               
                
                EA.Package pkg = rep.GetTreeSelectedPackage();
                updateVcStateRecursive(rep, pkg);
                    //pkg = rep.GetTreeSelectedPackage();
                //if (pkg != null && pkg.ParentID == 0)
                //{
                //    foreach (EA.Package p in rep.Models)
                //    {
                //        updateVcStateRecursive(rep,p);
                //    }
                //}
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error Insert Function");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            
        }
        #endregion
        #region updateVcStateRecursive
        public static void updateVcStateRecursive(EA.Repository rep, EA.Package pkg,bool recursive=true)
        {
            if (pkg.IsControlled) Util.updateVC(rep, pkg);
            if (recursive)
            {
                foreach (EA.Package p in pkg.Packages)
                {
                    if (p.IsControlled) Util.updateVC(rep, p);
                    updateVcStateRecursive(rep, p);
                }
            }
        }
        #endregion
        #region getDiagramLinkForConnector
        public static EA.DiagramLink getDiagramLinkForConnector(EA.Diagram dia, int connectorID)
        {
            foreach (EA.DiagramLink link in dia.DiagramLinks)
            {
                if (connectorID == link.ConnectorID) return link;
            }
            return null;
        }
        #endregion
        #region AddFavorite
        /// <summary>
        /// Add selected item to Favorite:
        /// - element, package, diagram, attribute, operation
        /// - Favorite is stored in t_xref under the type: 'Favorite'
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{7B3D065F-34AF-436E-AF96-F83DC8C3505E}", "Add selected item to Favorite",
            "Element, package, diagram, attribute, operation",
            IsTextRequired: false)]
        public static void AddFavorite(EA.Repository rep)
        {
            var f = new Favorite(rep, getGuidfromSelectedItem(rep));
            f.save();

        }
        #endregion
        #region RemoveFavorite
        /// <summary>
        /// Remove selected item to Favorite:
        /// - element, package, diagram, attribute, operation
        /// - Favorite is stored in t_xref under the type: 'Favorite'
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{41BFF6D9-DE73-481B-A3EC-7E158AE9BE9E}", "Remove selected item from Favorite",
            "Element, package, diagram, attribute, operation",
            IsTextRequired: false)]
        public static void RemoveFavorite(EA.Repository rep)
        {
            var f = new Favorite(rep, getGuidfromSelectedItem(rep));
            f.delete();

        }
        #endregion
        #region Favorites
        /// <summary>
        /// List Favorite:
        /// - List Favorites in the search window
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{756710FA-A99E-40D3-B265-518DDF1014D1}", "Favorites",
            "Element, package, diagram, attribute, operation",
            IsTextRequired: false)]
        public static void Favorites(EA.Repository rep)
        {
            var f = new Favorite(rep);
            f.search();

        }
        #endregion
        #region getGuidfromSelectedItem
        private static string getGuidfromSelectedItem(EA.Repository rep) {
            EA.ObjectType objectType = rep.GetContextItemType();
            string GUID = "";
            switch (objectType)
            {
                case EA.ObjectType.otAttribute:
                    var a = (EA.Attribute)rep.GetContextObject();
                    GUID = a.AttributeGUID;
                    break;
                case EA.ObjectType.otMethod:
                    var m = (EA.Method)rep.GetContextObject();
                    GUID = m.MethodGUID;
                    break;
                case EA.ObjectType.otElement:
                    var el = (EA.Element)rep.GetContextObject();
                    GUID = el.ElementGUID;
                    break;
                case EA.ObjectType.otDiagram:
                    var dia = (EA.Diagram)rep.GetContextObject();
                    GUID = dia.DiagramGUID;
                    break;
                case EA.ObjectType.otPackage:
                    var pkg = (EA.Package)rep.GetContextObject();
                    GUID = pkg.PackageGUID;
                    break;
                default:
                    MessageBox.Show("Nothing useful selected");
                    break;
            }
            return GUID;
        }
        #endregion
        #region moveEmbeddedLeftGUI
        /// <summary>
        /// Move the selected ports left
        /// - If left level is crossed it locates ports to top left corner.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{28188D09-7B40-4396-8FCF-90EA901CFE12}", "Embedded Elements left", "Select embedded elements", IsTextRequired: false)]
        public static void moveEmbeddedLeftGUI(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            EA.Element port = rep.GetElementByID(objPort0.ElementID);
            if (!EMBEDDED_ELEMENT_TYPES.Contains(port.Type)) return;

            // get parent of embedded element
            EA.Element el = rep.GetElementByID(port.ParentID);

            EA.DiagramObject obj = Util.getDiagramObjectByID(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");


            // check if left limit element is crossed
            int LeftLimit = obj.left - 0;// limit cross over left 
            bool isRightLimitCrossed = false;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (objPort.left < LeftLimit)
                {
                    isRightLimitCrossed = true;
                    break;
                }

            }
            // move all to left upper corner of element
            int startValueTop = obj.top - 8;
            int startValueLeft = obj.left - 8;
            int pos = 0;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (!isRightLimitCrossed)
                {
                    // move to right
                    objPort.left = objPort.left - 10;
                    objPort.Update();
                }
                else
                {
                    // move from top to down
                    objPort.top = startValueTop - pos * 20;
                    objPort.left = startValueLeft;
                    objPort.Update();
                    pos = pos + 1;
                }

            }
          
        }
        #endregion
        #region moveEmbeddedRightGUI
        /// <summary>
        /// Move the selected ports right
        /// - If right level is crossed it locates ports to top right corner.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{91998805-D1E6-4A3E-B9AA-8218B1C9F4AB}", "Embedded Elements right", "Select embedded elements", IsTextRequired: false)]
        public static void moveEmbeddedRightGUI(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            EA.Element port = rep.GetElementByID(objPort0.ElementID);
            if (!EMBEDDED_ELEMENT_TYPES.Contains(port.Type)) return;

            // get parent of embedded element
            EA.Element el = rep.GetElementByID(port.ParentID);

            EA.DiagramObject obj = Util.getDiagramObjectByID(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");


            // check if right limit element is crossed
            int rightLimit = obj.right - 16;// limit cross over right 
            bool isRightLimitCrossed = false;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (objPort.left > rightLimit)
                {
                    isRightLimitCrossed = true;
                    break;
                }

            }
            // move all to left upper corner of element
            int startValueTop = obj.top - 8;
            int startValueLeft = obj.right - 8;
            int pos = 0;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (!isRightLimitCrossed)
                {
                    // move to right
                    objPort.left = objPort.left + 10;
                    objPort.Update();
                }
                else
                {
                    // move from top to down
                    objPort.top = startValueTop - pos * 20;
                    objPort.left = startValueLeft ;
                    objPort.Update();
                    pos = pos + 1;
                }

            }

        }
        #endregion
        #region moveEmbeddedDownGUI
        /// <summary>
        /// Move the selected ports down
        /// - If lower level is crossed it locates ports to bottom left corners.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{1F5BA798-F9AC-4F80-8004-A8E8236AF629}", "Embedded Elements down", "Select embedded elements", IsTextRequired: false)]
        public static void moveEmbeddedDownGUI(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            EA.Element port = rep.GetElementByID(objPort0.ElementID);
            if (!EMBEDDED_ELEMENT_TYPES.Contains(port.Type)) return;

            // get parent of embedded element
            EA.Element el = rep.GetElementByID(port.ParentID);

            EA.DiagramObject obj = Util.getDiagramObjectByID(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");

            // check if lower limit element is crossed
            int lowerLimit = obj.bottom + 12;// limit cross over upper 
            bool isLowerLimitCrossed = false;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (objPort.bottom < lowerLimit)
                {
                    isLowerLimitCrossed = true;
                    break;
                }

            }
            // move all to left upper corner of element
            int startValueTop = obj.bottom + 8;
            int startValueLeft = obj.left + 8;
            int pos = 0;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (!isLowerLimitCrossed)
                {
                    // move to bottom
                    objPort.top = objPort.top - 10;
                    objPort.Update();
                }
                else
                {
                    // move from left to right
                    objPort.top = startValueTop;
                    objPort.left = startValueLeft + pos * 20;
                    objPort.Update();
                    pos = pos + 1;
                }

            }


            

        }
        #endregion
        #region moveEmbeddedUpGUI
        /// <summary>
        /// Move the selected ports up
        /// - If upper level is crossed it locates ports to top left corners.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{26F5F957-4CFD-4684-9417-305A1615460A}", "Embedded Elements up", "Select embedded elements", IsTextRequired: false)]
        public static void moveEmbeddedUpGUI(EA.Repository rep)
        {
            EA.Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
            EA.Element port = rep.GetElementByID(objPort0.ElementID);
            if (  ! EMBEDDED_ELEMENT_TYPES.Contains(port.Type) ) return;

            // get parent of embedded element
            EA.Element el = rep.GetElementByID(port.ParentID);

            EA.DiagramObject obj = Util.getDiagramObjectByID(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");
          

            // check if upper limit element is crossed
            int upLimit = obj.top - 10;// limit cross over upper 
            bool isUpperLimitCrossed = false;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects)
            {
                if (objPort.top > upLimit)
                {
                    isUpperLimitCrossed = true;
                    break;
                }

            }
            // move all to left upper corner of element
            int startValueTop = obj.top + 8;
            int startValueLeft = obj.left + 8;
            int pos = 0;
            foreach (EA.DiagramObject objPort in dia.SelectedObjects) 
            {
                if (! isUpperLimitCrossed)
                {
                    // move to top
                    objPort.top = objPort.top + 10;
                    objPort.Update();
                }else 
                {
                    // move from left to right
                    objPort.top = startValueTop;
                    objPort.left = startValueLeft + pos * 20;
                    objPort.Update();
                    pos = pos + 1;
                }
               
            }


        }
        #endregion

        #region About
        /// <summary>
        /// Outputs the About window
        /// </summary>
        /// <param name="release"></param>
        /// <param name="configFilePath"></param>
        public static void about(string release, string configFilePath)
        {
            string installDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EaService)).CodeBase);
            MessageBox.Show("!!!Make live with EA easier and more efficient!!!\n\n\n"+
                      "Helmut.Ortmann@t-online.de\n +49 172 / 51 79 16 7\n\n"+
                      " Workshops, Training Coaching, Project Work\n"+
                      " - Processes (RUP / Functional Safety)\n"+
                      " - Requirements\n" +
                      " - Enterprise Architect\n"+
                      " -- UML / SysML\n"+
                      " -- Method- development and -implementation\n" +
                      " -- Addin\n" +
                      " -- Query & Script\n\n\n"+
                      "!!!Make live with EA easier and more efficient!!!"+
                      "\n\nInstall:\t"+installDir +
                      "\nConfig:\t" + configFilePath
                    , "hoTools  " + release + " (AddinClass.dll AssemblyFileVersion)");
        }
        #endregion

        public static void aboutVAR1(string release, string configFilePath) {
            string installDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EaService)).CodeBase);
            MessageBox.Show("Helmut.Ortmann@t-online.de\n"+
                            "Helmut.Ortmann@t-online.de\n" +
                            "Germany\n" +
                            "private: +49 172 / 51 79 16 7\n"+
                            "\n\nInstall:\t" + installDir +
                            "\nConfig:\t" + configFilePath
                    , "hoTools for VAR1  " + release);
        }
    }
}
