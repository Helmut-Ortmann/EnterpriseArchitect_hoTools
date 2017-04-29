using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using AddinFramework.Extension;
using EA;
using hoTools.EaServices.Dlg;
using hoTools.Utils;
using hoTools.Utils.ActivityParameter;
using hoTools.Utils.Appls;
using hoTools.Utils.Favorites;
using hoTools.Utils.svnUtil;
using hoTools.Utils.SQL;
using Attribute = System.Attribute;
using DiagramObject = EA.DiagramObject;
using Element = EA.Element;
using File = EA.File;
using Package = EA.Package;
using TaggedValue = hoTools.Utils.TaggedValue;
using hoTools.EAServicesPort;
using hoTools.Utils.Configuration;
using hoTools.Utils.Diagram;

// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ArgumentsStyleLiteral

namespace hoTools.EaServices
{

    #region Definition of Service Attribute

    // ReSharper disable once RedundantAttributeUsageProperty
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ServiceOperationAttribute : Attribute
    {
        /// <summary>
        /// Attribute to define services which might be called without parameters
        /// Example:
        /// [ServiceOperation("{1C78E1C0-AAC8-4284-8C25-2D776FF373BC}", "Copy release information to clipboard", "Select Component", false)]. 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="description">the brief description for the service</param>
        /// <param name="help">the help text / tooltip for the service </param>
        /// <param name="isTextRequired">text is required to run service, default false</param>
        public ServiceOperationAttribute(string guid, string description, string help, bool isTextRequired = false)
        {
            Description = description;
            Guid = guid;
            Help = help;
            IsTextRequired = isTextRequired;
        }

        public bool IsTextRequired { get; }

        public string Description { get; }

        public string Guid { get; }

        public string Help { get; }
    }

    #endregion

    public static class EaService
    {
        // configuration as singleton
        static HoToolsGlobalCfg _globalCfg;
        // remember Diagram Style Settings
        public static DiagramStyle DiagramStyle;

        // define menu constants
        public enum DisplayMode
        {
            Behavior,
            Method
        }

        #region runQuickSearch

        //---------------------------------------------------------------------------------------------------------------
        // Search for Elements, Operation, Attributes, Id
        public static void RunQuickSearch(Repository rep, string searchName, string searchString)
        {
            // get the search from setting
            try
            {
                rep.RunModelSearch(searchName, searchString, "", "");
            }
            catch (Exception)
            {
                MessageBox.Show($@"Search name:'{searchName}' not available", @"Error run search, break!");
            }
        }

        #endregion

        #region AddDiagramNote

        public static void AddDiagramNote(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(ObjectType.otDiagram))
            {
                Diagram dia = rep.GetCurrentDiagram();
                if (dia == null) return;
                Package pkg = rep.GetPackageByID(dia.PackageID);
                if (pkg.IsProtected || dia.IsLocked) return;

                // save diagram
                rep.SaveDiagram(dia.DiagramID);

                Element elNote;
                try
                {
                    elNote = (Element) pkg.Elements.AddNew("", "Note");
                    elNote.Update();
                    pkg.Update();
                }
                catch
                {
                    return;
                }

                // add element to diagram
                // "l=200;r=400;t=200;b=600;"

                // get the position of the Element

                int left = (dia.cx / 2) - 100;
                int right = left + 200;
                int top = dia.cy - 150;
                int bottom = top + 120;
                //int right = diaObj.right + 2 * (diaObj.right - diaObj.left);

                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";

                var diaObject = (DiagramObject) dia.DiagramObjects.AddNew(position, "");
                dia.Update();
                diaObject.ElementID = elNote.ElementID;
                diaObject.Update();
                pkg.Elements.Refresh();

                Util.SetDiagramHasAttchaedLink(rep, elNote);
                rep.ReloadDiagram(dia.DiagramID);
            }
        }

        #endregion

        #region changeAuthorPackage

        static void ChangeAuthorPackage(Repository rep, Package pkg, string[] args)
        {
            Element el = rep.GetElementByGuid(pkg.PackageGUID);
            el.Author = args[0];
            el.Update();
        }

        #endregion

        #region changeAuthorElement

        static void ChangeAuthorElement(Repository rep, Element el, string[] args)
        {
            el.Author = args[0];
            el.Update();
        }

        #endregion

        #region changeAuthorDiagram

        private static void ChangeAuthorDiagram(Repository rep, Diagram dia, string[] args)
        {
            dia.Author = args[0];
            dia.Update();
        }

        #endregion

        #region LockSelected

        /// <summary>
        /// Lock selected item (Package, Diagram, Element)
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{0A731169-C983-404C-AB20-E4E478A38DB4}",
            "Lock Item (Package, Diagram or Element)", // Description
            "Lock Item (select Package, Diagram or Element). Security has to be enabled!", //Tooltip
            isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void LockSelected(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;
            bool success;
            switch (rep.GetContextItemType())
            {
                case ObjectType.otPackage:
                    Package pkg = (Package) rep.GetContextObject();
                    pkg.ApplyUserLockRecursive(true, true, false);
                    success = pkg.ApplyUserLock();
                    break;
                case ObjectType.otElement:
                    Element el = (Element) rep.GetContextObject();
                    success = el.ApplyUserLock();
                    break;
                case ObjectType.otDiagram:
                    Diagram dia = (Diagram) rep.GetContextObject();
                    success = dia.ApplyUserLock();
                    break;
                default:
                    return;
            }
            if (success) return;
            MessageBox.Show($@"Error:'{rep.GetLastError()}'", @"Error lock item");
        }

        #endregion

        #region UnLockAllForCurrentUser

        /// <summary>
        /// UnLock all for current user 
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{77275F9D-DDE6-4807-A1B1-5152416B3235}", "UnLock all locks for current user",
            "UnLock all locks for current user!", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void UnLockAllForCurrentUser(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;

            string logInUserGuid = rep.GetCurrentLoginUser(true);

            string sqlItems =
                $"select EntityType as [TYPE], EntityID as [GUID] from t_seclocks where UserId = '{logInUserGuid}'";
            XDocument x = XDocument.Parse(rep.SQLQuery(sqlItems));

            var fields = from row in x.Descendants("Row").Descendants()
                where row.Name == "TYPE" ||
                      row.Name == "Id"
                // 'Class','Action','Diagram', 
                select row;
            int i = 0;
            string type = "";
            foreach (var field in fields)
            {
                switch (i % 2)
                {
                    case 0:
                        type = field.Value;
                        break;
                    case 1:
                        var guid = field.Value;
                        switch (type)
                        {
                            case "Element":
                                Element el = rep.GetElementByGuid(guid);
                                el.ReleaseUserLock();
                                break;
                            case "Diagram":
                                EA.Diagram dia = (EA.Diagram) rep.GetDiagramByGuid(guid);
                                dia.ReleaseUserLock();
                                break;
                        }
                        break;
                }
                i = i + 1;
            }
        }

        #endregion



        #region UnLockSelected

        /// <summary>
        /// UnLock selected item (Package, Diagram, Element)
        /// </summary>
        /// <param name="rep"></param>
        [
            ServiceOperation("{1ABCB8FB-56F9-412F-B6C1-9FE5E2B4824E}", "UnLock Package, Diagram or Element",
                "UnLock selected Package, Diagram or Element. Security has to be enabled!", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void UnLockSelected(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;
            bool success;
            switch (rep.GetContextItemType())
            {
                case ObjectType.otPackage:
                    Package pkg = (Package) rep.GetContextObject();
                    pkg.ReleaseUserLockRecursive(true, true, false);
                    success = pkg.ReleaseUserLock();
                    break;
                case ObjectType.otElement:
                    Element el = (Element) rep.GetContextObject();
                    success = el.ReleaseUserLock();
                    break;
                case ObjectType.otDiagram:
                    Diagram dia = (Diagram) rep.GetContextObject();
                    success = dia.ReleaseUserLock();
                    break;
                default:
                    return;
            }
            if (success) return;
            MessageBox.Show($@"Error:'{rep.GetLastError()}'", @"Error unlock item");
        }

        #endregion

        //--------------------------------------------------------------------------------------------

        #region UnLockPackageRecursive

        /// <summary>
        /// UnLock Package recursive (Package, Diagram or Element selected)
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{42788062-3578-49CC-BBD0-87032B764B3D}", "UnLock Package recursive",
            "UnLock Package recursive (select Package, Element or Diagram). Security has to be enabled!",
            isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void UnLockPackageRecursive(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;
            Package pkg = GetContextPackage(rep);
            if (pkg == null) return;
            if (!pkg.ReleaseUserLockRecursive(true, true, true))
                MessageBox.Show($@"Error:'{rep.GetLastError()}'", @"Error Unlock Package recursive");
        }

        #endregion

        #region LockPackageRecursive

        /// <summary>
        /// UnLock Package recursive (Package, Diagram or Element selected)
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{F1B97839-0E68-4019-95C2-8F745CCDA484}", "Lock Package recursive",
            "Lock Package recursive (select Package, Element or Diagram). Security has to be enabled!",
            isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void LockPackageRecursive(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;
            Package pkg = GetContextPackage(rep);
            if (pkg == null) return;
            bool success = pkg.ApplyUserLockRecursive(true, true, true);
            if (!success)
                MessageBox.Show($@"Error:'{rep.GetLastError()}'", @"Error lock Package recursive");
        }

        #endregion

        //-------------------------------------------------------------------------------------------

        #region UnLockPackage

        /// <summary>
        /// UnLock Package (Package, Diagram, Element may be selected)
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{06FD450C-9B18-453A-821F-955CFFE299DA}", "UnLock Package",
            "UnLock Package (select Package, Element or Diagram). Security has to be enabled!", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void UnLockPackage(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;
            Package pkg = GetContextPackage(rep);
            if (pkg == null) return;

            pkg.ReleaseUserLock();
            if (!pkg.ReleaseUserLockRecursive(true, true, false))
                MessageBox.Show($@"Error:'{rep.GetLastError()}'", @"Error Unlock Package");
        }

        /// <summary>
        /// Return false and outputs an error message if no security is enabled
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        private static bool IsSecurityEnabled(Repository rep)
        {
            if (rep.IsSecurityEnabled == false)
            {
                MessageBox.Show(@"Can't perform Lock/Unlock because no Security is enabled!",
                    @"No security enabled for current Repository");
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region LockPackage

        /// <summary>
        /// Lock Package (Package, Diagram, Element may be selected)
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{BA09245C-21E3-4A3C-A9AF-5DF6ED703201}", "Lock Package",
            "Lock Package (select Package, Element, Diagram). Security has to be enabled!", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void LockPackage(Repository rep)
        {
            if (!IsSecurityEnabled(rep)) return;
            Package pkg = GetContextPackage(rep);
            if (pkg == null) return;
            pkg.ApplyUserLock();
            if (!pkg.ApplyUserLockRecursive(true, true, false))
                MessageBox.Show($@"Error:'{rep.GetLastError()}'", @"Error lock package");
        }

        #endregion

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get context Package from Package, Element, Diagram
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        static Package GetContextPackage(Repository rep)
        {
            switch (rep.GetContextItemType())
            {
                case ObjectType.otPackage:
                    return (Package) rep.GetContextObject();

                case ObjectType.otElement:
                    Element el = (Element) rep.GetContextObject();
                    return rep.GetPackageByID(el.PackageID);

                case ObjectType.otDiagram:
                    Diagram dia = (Diagram) rep.GetContextObject();
                    return rep.GetPackageByID(dia.PackageID);

                default:
                    return null;
            }
        }
        [ServiceOperation("{4FF89921-595B-4F16-8813-39789EB53730}", "Change Author Package (selected Items + Package content)",
           "Select Package, Element or Diagram in Browser or Diagram", isTextRequired: false)]
        public static void ChangeAuthorPackage(Repository rep)
        {
            ChangeAuthor(rep, ChangeScope.Package);
        }

        #region change Author for selected item

        [ServiceOperation("{4161D769-825F-494A-9389-962CC1C16E4F}", "Change Author Items",
            "Select Package, Element or Diagram in Browser or Diagram", isTextRequired: false)]
        public static void ChangeAuthorItem(Repository rep)
        {
            ChangeAuthor(rep, ChangeScope.Item);

        }

        #endregion

        #region Change Author recursive
        /// <summary>
        /// Change Author recursive for the selected items (Package, Element, Diagram)
        /// Use:
        /// - If nothing in Diagram selected: Use TreeSelectedElements
        /// - If DiagramObjects selected: Use DiagramObjects
        /// - Else: Use ContextItem 
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{F0038D4B-CCAA-4F05-9401-AAAADF431ECB}",
            "Change Author Package recursive",
            "Select Package, Element or Diagram in Browser or Diagram", isTextRequired: false)]
        public static void ChangeAuthorRecursive(Repository rep)
        {
            ChangeAuthor(rep, ChangeScope.PackageRecursive);
        }
        /// <summary>
        /// Changes the Author of selected items and the chosen 'DlgAuthor.ChangeScope'.
        /// - Selected items
        /// - Standard (selected Items, Elements Recursive, Package and it's content)
        /// - All recursive (also all Packages recursive)
        /// - 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="changeScope"></param>
        static void ChangeAuthor(Repository rep, ChangeScope changeScope)
        {
            // configuration as singleton
            _globalCfg = HoToolsGlobalCfg.Instance;
            // Parameter for change
            // - [0]Author to change to
            // - [1]changeScope
            string[] liParameter = { "", changeScope.ToString() };

            // Get Elements of type Element and Package
            List<Element> lEl = GetSelectedElements(rep);


            // Selected elements (Diagram or Project Browser Key)
            // No element returns: Check Context item
            if (lEl.Count > 0)
            {
                // Make a list of 'to change item names' from selected elements
                // This list to show the user to check the Items
                List<String> lToChange = new List<String>();
                foreach (EA.Element el0 in lEl)
                {
                    lToChange.Add(el0.Name);
                }

                var dlg0 = new DlgAuthor(rep, changeScope, lToChange) { User = lEl[0].Author };
                dlg0.ShowDialog(_globalCfg.Owner);
                // use string to use recursive call of function
                if (dlg0.User == "") return;
                liParameter[0] = dlg0.User;
                foreach (EA.Element el in lEl)
                {
                    if (el.Type == "Package")
                    {
                        EA.Package pkg1 = rep.GetPackageByGuid(el.ElementGUID);
                        RecursivePackages.DoRecursivePkg(rep, pkg1, ChangeAuthorPackage, ChangeAuthorElement,
                                ChangeAuthorDiagram, liParameter, changeScope);

                    }
                    else
                    {
                            RecursivePackages.DoRecursiveEl(rep, el, ChangeAuthorElement, ChangeAuthorDiagram, liParameter, changeScope);
                       
                    }
                }
                string items = string.Join($"{Environment.NewLine}", lToChange.ToArray());
                MessageBox.Show($@"New author:'{dlg0.User}'{Environment.NewLine}{items}", 
                    $@"Author changed, {changeScope.ToString()}");
            }
            else
            // No selected item (Diagram or Project Browser)
            // Use Context Element
            {
                EA.Element el = null;
                EA.Package pkg = null;
                Diagram dia = null;
                string oldAuthor;
                List<string> liAuthors = new List<string>();
                ObjectType oType = rep.GetContextItemType();

                // get the element
                switch (oType)
                {
                    case ObjectType.otPackage:
                        pkg = (Package)rep.GetContextObject();
                        oldAuthor = rep.GetElementByGuid(pkg.PackageGUID).Author;
                        liAuthors.Add(pkg.Name);
                        break;
                    case ObjectType.otElement:
                        el = (Element)rep.GetContextObject();
                        oldAuthor = el.Author;
                        liAuthors.Add(el.Name);
                        break;
                    case ObjectType.otDiagram:
                        dia = (Diagram)rep.GetContextObject();
                        oldAuthor = dia.Author;
                        liAuthors.Add(dia.Name);
                        break;
                    default:
                        return;
                }
                // ask for new user
                var dlg = new DlgAuthor(rep, ChangeScope.PackageRecursive, liAuthors) { User = oldAuthor };
                dlg.ShowDialog(_globalCfg.Owner);
                // use string to use recursive call of function
                liParameter[0] = dlg.User;
                if (dlg.User == "") return;
                switch (oType)
                {
                    case ObjectType.otPackage:
                        RecursivePackages.DoRecursivePkg(rep, pkg, ChangeAuthorPackage, ChangeAuthorElement,
                            ChangeAuthorDiagram, liParameter, changeScope);
                        MessageBox.Show($@"New author:'{dlg.User}'", $@"Author changed for package '{pkg.Name}', recursive");
                        break;
                    case ObjectType.otElement:
                        RecursivePackages.DoRecursiveEl(rep, el, ChangeAuthorElement, ChangeAuthorDiagram, liParameter, changeScope);
                        MessageBox.Show($@"New author:'{dlg.User}'", $@"Author changed for element '{el.Name}', recursive");
                        break;
                    case ObjectType.otDiagram:
                        ChangeAuthorDiagram(rep, dia, liParameter);
                        MessageBox.Show($@"New author:'{dlg.User}'", $@"Author changed for diagram '{dia.Name}'");
                        break;
                    default:
                        return;
                }
            }
        }

        #endregion
        #region Get selected Elements
        /// <summary>
        /// Get selected Elements from:
        /// - Diagram
        /// - Tree Selected Elements
        /// - Elements can also be Packages
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        private static List<Element> GetSelectedElements(Repository rep)
        {
            List<Element> lEl = new List<Element>();
            var eaDia = new EaDiagram(rep);
            // nothing selected
            if (! eaDia.IsSelectedObjects && eaDia.SelectedConnector == null)
            {
                // get all tree selected elements
                foreach (EA.Element el1 in rep.GetTreeSelectedElements())
                {
                    lEl.Add(el1);
                }
            }
            else
            {
                lEl = eaDia.SelElements;
            }
            return lEl;
        }

        #endregion
        // Set folder of package for easy access of implementation:
        [ServiceOperation("{7D0298DF-3AC2-4563-9593-699138657018}", "Set folder of implementation",
                "Select package to set the implementation folder", isTextRequired: false)]
        public static void SetFolder(Repository rep)
        {

            switch (rep.GetContextItemType())
            {
                case EA.ObjectType.otPackage:

                    EA.Package pkg = (EA.Package)rep.GetContextObject();
                    string folderPath = pkg.CodePath;
                    // try to infer the right folder from package class/interfaces
                    if (folderPath.Trim() == "")
                    {
                        foreach (EA.Element el in pkg.Elements)
                        {
                            if ("Interface Component Class".Contains(el.Type))
                            {
                                if (el.Genfile != "")
                                {
                                    folderPath = Path.GetDirectoryName(Util.GetGenFilePathElement(rep, el));
                                    break;
                                }
                            }
                        }
                    }

                    using (var fbd = new FolderBrowserDialog())
                    {
                        fbd.SelectedPath = folderPath;
                        DialogResult result = fbd.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {

                            pkg.CodePath = fbd.SelectedPath;
                            pkg.Update();
                        }
                    }

                    break;
            }
        }

        #region ShowFolderElementPackage

        // Show folder with Explorer or Total Commander for:
        // - Package: If Version Controlled package
        // - File: If Source Code implementation exists
        // See also: Global Settings
        [ServiceOperation("{C007C59A-FABA-4280-9B66-5AD10ACB4B13}", "Show folder of *.xml, *.h,*.c",
            "Select VC controlled package or element with file path (Source Code Generation)", isTextRequired: false)]
        public static void ShowFolderElementPackage(Repository rep, bool isTotalCommander = false)
        {
            string pathFolder = "";
            ObjectType oType = rep.GetContextItemType();
            switch (oType)
            {
                case EA.ObjectType.otPackage:
                    EA.Package pkg = (EA.Package)rep.GetContextObject();
                    if (pkg.CodePath.Trim() != "")
                    {
                        // consider gentype (C,C++,..)
                        EA.Element el1 = rep.GetElementByGuid(pkg.PackageGUID);
                        if (el1.Gentype == "")
                        {
                            MessageBox.Show("Package has no language configured. Please select a language!");
                            return;
                        }
                        pathFolder = Util.GetFilePath(rep, el1.Gentype, pkg.CodePath);
                    }
                    else
                    {
                        if (pkg.IsControlled)
                        {
                            pathFolder = Util.GetVccFilePath(rep, pkg);
                            // remove filename
                            pathFolder = Regex.Replace(pathFolder, @"[a-zA-Z0-9\s_:.]*\.xml", "");
                        }
                    }
                    if (pathFolder == "") return;


                    if (isTotalCommander)
                        Util.StartApp(@"totalcmd.exe", "/o " + pathFolder);
                    else
                        Util.StartApp(@"Explorer.exe", "/e, " + pathFolder);
                    break;

                case EA.ObjectType.otElement:
                    EA.Element el = (EA.Element)rep.GetContextObject();
                    pathFolder = Util.GetGenFilePathElement(rep, el);
                    // remove filename
                    pathFolder = Regex.Replace(pathFolder, @"[a-zA-Z0-9\s_:.]*\.[a-zA-Z0-9]{0,4}$", "");

                    if (isTotalCommander)
                        Util.StartApp(@"totalcmd.exe", "/o " + pathFolder);
                    else
                        Util.StartApp(@"Explorer.exe", "/e, " + pathFolder);

                    break;
            }
        }

        #endregion

        #region CreateActivityForOperation

        [ServiceOperation("{17D09C06-8FAE-4D76-B808-5EC2362B1953}", "Create Activity for Operation, Class/Interface",
            "Select Package, Class/Interface or operation", isTextRequired: false)]
        public static void CreateActivityForOperation(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            switch (oType)
            {
                case ObjectType.otMethod:
                    var m = (Method) rep.GetContextObject();

                    // Create Activity at the end
                    Element el = rep.GetElementByID(m.ParentID);
                    Package pkg = rep.GetPackageByID(el.PackageID);
                    int pos = pkg.Packages.Count + 1;
                    ActivityPar.CreateActivityForOperation(rep, m, pos);
                    rep.Models.Refresh();
                    rep.RefreshModelView(0);
                    rep.ShowInProjectView(m);
                    break;

                case ObjectType.otElement:
                    el = (Element) rep.GetContextObject();
                    if (el.Locked) return;

                    CreateActivityForOperationsInElement(rep, el);
                    rep.Models.Refresh();
                    rep.RefreshModelView(0);
                    rep.ShowInProjectView(el);
                    break;

                case ObjectType.otPackage:
                    pkg = (Package) rep.GetContextObject();
                    CreateActivityForOperationsInPackage(rep, pkg);
                    // update sort order of packages
                    rep.Models.Refresh();
                    rep.RefreshModelView(0);
                    rep.ShowInProjectView(pkg);
                    break;
            }
        }

        #endregion

        private static void CreateActivityForOperationsInElement(Repository rep, Element el)
        {
            if (el.Locked) return;
            Package pkg = rep.GetPackageByID(el.PackageID);
            int treePos = pkg.Packages.Count + 1;
            foreach (Method m1 in el.Methods)
            {
                // Create Activity
                ActivityPar.CreateActivityForOperation(rep, m1, treePos);
                treePos = treePos + 1;
            }
        }

        private static void CreateActivityForOperationsInPackage(Repository rep, Package pkg)
        {
            foreach (Element el in pkg.Elements)
            {
                CreateActivityForOperationsInElement(rep, el);
            }
            foreach (Package pkg1 in pkg.Packages)
            {
                CreateActivityForOperationsInPackage(rep, pkg1);
            }
        }

        /// <summary>
        /// If passed element is of type "Text" or UMLDiagram"
        /// - show in project view
        /// - return true
        /// If not return false
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <returns>true Element is "Text" or "UMLDiagram"</returns>
        static bool LocateTextOrFrame(Repository rep, Element el)
        {
            if (el.Type == "Text")
            {
                string s = el.MiscData[0];
                int id = Convert.ToInt32(s);
                Diagram dia = rep.GetDiagramByID(id);
                rep.ShowInProjectView(dia);
                return true;
            }
            // display the original diagram on what the frame is based
            if (el.Type == "UMLDiagram")
            {
                int id = Convert.ToInt32(el.MiscData[0]);
                Diagram dia = rep.GetDiagramByID(id);
                rep.ShowInProjectView(dia);
                return true;
            }
            return false;
        }

        #region showAllEmbeddedElements

        /// <summary>
        /// Show all embedded Elements in diagram for
        /// - Seleted Elements
        /// - All elements if nothing is selected
        /// - If Port or Object with a Block/Classs then update the Ports
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="isOptimizePortLayoutLocation"></param>
        /// <param name="portSynchronizationKind"></param>
        [ServiceOperation("{678AD901-1D2F-4FB0-BAAD-AEB775EE18AC}", "Show Ports, Pins, Parameter",
            "Selected Diagram Objects or all", isTextRequired: false)]
        public static void ShowEmbeddedElements(
            Repository rep,
            bool isOptimizePortLayoutLocation = false,
            PartPortSynchronization portSynchronizationKind = PartPortSynchronization.Delete)
        {
            var eaDia = new EaDiagram(rep);
            if (eaDia.Dia == null) return;
            Cursor.Current = Cursors.WaitCursor;
            // remember Diagram data of current selected diagram
            
            // Save to avoid indifferent states
            rep.SaveDiagram(eaDia.Dia.DiagramID);

            // SQL for Embedded Elements
            var sqlUtil = new UtilSql(rep);


            // over all selected diagram objects elements (Class, Block,.. Part, Parameter, Action Pin,..)
            int count = -1;
            foreach (DiagramObject diaObj in eaDia.SelObjects)
            {
                count = count + 1;
                var elSource = eaDia.SelElements[count];
                // Are Ports to be synchronized (Type = Part and synchronizing configured)
                if (elSource.Type == "Part" && (portSynchronizationKind != PartPortSynchronization.Off) )
                    UpdatePortsForPart(rep, elSource, portSynchronizationKind);


                // arrange sequence of ports
                string[] embededElementLocation = { "right" };
                if (isOptimizePortLayoutLocation)
                
                {
                    embededElementLocation = new[] { "left", "right" };
                }
                // Over all possible locations of embedded elements 
                foreach (string portBoundTo in embededElementLocation)
                {
                    
                    int pos = 0;
                    List<int> lEmbeddedElements;
                    if (isOptimizePortLayoutLocation == false)
                    {
                        lEmbeddedElements = sqlUtil.GetAndSortEmbeddedElements(elSource, "", "", "");
                    }
                    else
                    {
                        if (portBoundTo == "left")
                            lEmbeddedElements = sqlUtil.GetAndSortEmbeddedElements(elSource, "Port",
                                "'Server', 'Receiver' ",
                                "DESC");
                        else
                            lEmbeddedElements = sqlUtil.GetAndSortEmbeddedElements(elSource, "Port",
                                "'Client', 'Sender' ", "");
                    }
                    // over all sorted ports
                    string oldStereotype = "";
                    foreach (int i in lEmbeddedElements)
                    {
                        Element portEmbedded = rep.GetElementByID(i);
                        if (portEmbedded.IsEmbeddedElement())
                        {
                            // only ports / parameters (port has no further embedded elements
                            if (portEmbedded.Type == "ActivityParameter" | portEmbedded.EmbeddedElements.Count == 0)
                            {
                                if (isOptimizePortLayoutLocation)
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
                                if (pos == 0 && "Sender Receiver".Contains(portEmbedded.Stereotype))
                                    oldStereotype = portEmbedded.Stereotype;
                                if (pos > 0 && "Sender Receiver".Contains(oldStereotype) &&
                                    oldStereotype != portEmbedded.Stereotype)
                                {
                                    pos = pos + 1; // make a gap
                                    oldStereotype = portEmbedded.Stereotype;
                                }
                                Util.VisualizePortForDiagramobject(rep, pos, eaDia.Dia, diaObj, portEmbedded, null,
                                    portBoundTo);
                                pos = pos + 1;
                            }
                            else
                            {
                                // Port: Visualize Port + Interface
                                foreach (Element interf in portEmbedded.EmbeddedElements)
                                {
                                    Util.VisualizePortForDiagramobject(rep, pos, eaDia.Dia, diaObj, portEmbedded, interf);
                                    pos = pos + 1;
                                }
                            }
                        }
                    }
                }
            }
            // display changes
            rep.ReloadDiagram(eaDia.Dia.DiagramID);
            eaDia.ReloadSelectedObjectsAndConnector();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        // Handle Synchronization of a Part Port which has a Block that types the Part
        public enum PartPortSynchronization
        {
            Off,  // No synchronization Part Ports from its typed Block
            New,  // Add new Part Port if Block has a Port which isn't available in Part 
            Mark, // Mark Part Port as to 'DeleteMe' if Part Port isn't available in Block
            Delete // Delete Part Port if Part Port isn't available in Block
        }

        /// <summary>
        /// Update Ports for a Part if a PropertyType (defining Block/Class the part depends on) is defined.
        /// If a port is dependent on another port PDATA3 (MiscData(2)) contains ea_guid of the master port.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="elTarget"></param>
        /// <param name="synchronizationKind"></param>
        /// <returns></returns>
        // ReSharper disable once UnusedMethodReturnValue.Local
        static bool UpdatePortsForPart(Repository rep, EA.Element elTarget, PartPortSynchronization synchronizationKind)
        {
            // no Property defined (Block that type this part)
            if (elTarget.PropertyType == 0) return true;


            // Copy ports from the typing block/class to the dependent class/block
            EA.Element elSource = rep.GetElementByID(elTarget.PropertyType);
            foreach (EA.Element portSource in elSource.EmbeddedElements)
            {
                if (portSource.Type == "Port")
                {
                    PortServices.CopyPort(rep, portSource, elTarget);

                }
            }
            // handling of additional ports
            if (synchronizationKind == PartPortSynchronization.Delete ||
                synchronizationKind == PartPortSynchronization.Mark)
            {
                // delete all ports that are not part of the PropertyType (the defining type (Class/Block)
                bool foundAtLeastOneToDelete = false;
                for (int i = elTarget.EmbeddedElements.Count - 1; i >= 0; i -= 1)
                {
                    EA.Element portTarget = (EA.Element) elTarget.EmbeddedElements.GetAt((short) i);
                    if (portTarget.Type == "Port")
                    {
                        bool found = false;
                        foreach (EA.Element portSource in elSource.EmbeddedElements)
                        {
                            // Port has to be connected to source Port (GUID source = PDATA3)
                            if ( portSource.ElementGUID != portTarget.MiscData[2]  )
                                continue;
                            // port found in target and in source
                            found = true;
                            break;
                        }
                        // port not found in source, Perform action according to settings:
                        // - Delete
                        // - Rename it (_DeleteMe)
                        // - Do nothing
                        if (!found)
                        {
                            // Delete Port
                            if (synchronizationKind == PartPortSynchronization.Delete)
                            {
                                portTarget.Locked = false;
                                elTarget.EmbeddedElements.Delete((short) i);
                                foundAtLeastOneToDelete = true;
                            }
                            // Mark Port as to delete
                            if (synchronizationKind == PartPortSynchronization.Mark)
                            {
                                portTarget.Locked = false;
                                string suffixDeleteMe = "_DeleteMe";
                                if (! portTarget.Name.Contains(suffixDeleteMe))
                                    portTarget.Name = portTarget.Name + suffixDeleteMe;
                                portTarget.Update();
                                portTarget.Locked = true;
                            }
                        }


                    }
                }
                // Update Embedded Elements list if one Port was updated
                if (foundAtLeastOneToDelete) elTarget.EmbeddedElements.Refresh();
            }


            return true;

        }
        #region HideAllEmbeddedElementI

        /// <summary>
        /// Hide all embedded Elements for:
        /// - selected nodes
        /// - all if nothing is selected
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{5ED3DABA-367E-4575-A161-D79F838A5A17}", "Hide Ports, Pins, Parameter",
            "Selected Diagram Objects or all", isTextRequired: false)]
        public static void HideEmbeddedElements(
            Repository rep)
        {
            Cursor.Current = Cursors.WaitCursor;
            // remember Diagram data of current selected diagram
            var eaDia = new EaDiagram(rep);
            if (eaDia.Dia == null) return;
            // Save to avoid indifferent states
            rep.SaveDiagram(eaDia.Dia.DiagramID);

            
            // over all selected elements
            int count = -1;
            foreach (DiagramObject diaObj in eaDia.SelObjects)
            {
                count = count + 1;
                var elSource = eaDia.SelElements[count];
                if (elSource.IsEmbeddedElement())
                {
                    // selected element was port
                    RemoveEmbeddedElementFromDiagram(eaDia.Dia, elSource);
                }
                else
                {
                    // selected element was "Element"
                    foreach (Element embeddedElement in elSource.EmbeddedElements)
                    {
                        if (embeddedElement.IsEmbeddedElement())
                        {
                            RemoveEmbeddedElementFromDiagram(eaDia.Dia, embeddedElement);
                        }
                    }
                }




            }
            // display changes
            rep.ReloadDiagram(eaDia.Dia.DiagramID);
            eaDia.ReloadSelectedObjectsAndConnector();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region ShowEmbeddedElementLabel

        /// <summary>
        /// Show embdeded Element Labels for:
        /// - selected nodes
        /// - all if nothing is selected
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{FBEF4500-DD24-4D23-BC7F-08D70DDA2B57}", "Show Port, Pin Parameter Labels",
            "Selected Diagram Objects or all", isTextRequired: false)]
        public static void ShowEmbeddedElementsLabel(Repository rep)
        {
            UpdateEmbeddedElementStyle(rep, PortServices.LabelStyle.IsShown);
        }

        #endregion

        #region HideEmbeddedElementsLabel

        /// <summary>
        /// Hide embdeded Element Labels for:
        /// - selected nodes
        /// - all if nothing is selected
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{3493B9E6-F6DA-478E-A161-DD95D1D34B44}", "Hide Ports, Pins, Parameter Label",
            "Selected Diagram Objects or all", isTextRequired: false)]
        public static void HideEmbeddedElementsLabel(Repository rep)
        {
            UpdateEmbeddedElementStyle(rep, PortServices.LabelStyle.IsHidden);
        }

        #endregion

        #region HideEmbeddedElementsType

        /// <summary>
        /// Hide embdeded Element Labels for:
        /// - selected nodes
        /// - all if nothing is selected
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{CF59707B-35A3-4E0C-AA0D-16722DB61F7D}", "Hide Port Type",
            "Selected Diagram Objects or all", isTextRequired: false)]
        public static void HideEmbeddedElementsType(Repository rep)
        {
            UpdateEmbeddedElementStyle(rep, PortServices.LabelStyle.IsTypeHidden);
        }

        #endregion

        #region ShowEmbeddedElementsType

        /// <summary>
        /// Hide embdeded Element Labels for:
        /// - selected nodes
        /// - all if nothing is selected
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{CF59707B-35A3-4E0C-AA0D-16722DB61F7D}", "Show Port Type",
            "Selected Diagram Objects or all", isTextRequired: false)]
        public static void ShowEmbeddedElementsType(Repository rep)
        {
            UpdateEmbeddedElementStyle(rep, PortServices.LabelStyle.IsTypeShown);
        }

        #endregion

        #region UpdateEmbeddedElementStyle

        private static void UpdateEmbeddedElementStyle(Repository rep, PortServices.LabelStyle style)
        {
            Cursor.Current = Cursors.WaitCursor;
            // remember Diagram data of current selected diagram
            var eaDia = new EaDiagram(rep);
            if (eaDia.Dia == null) return;
            // Save to avoid indifferent states
            rep.SaveDiagram(eaDia.Dia.DiagramID);

            // over all selected elements
            int count = -1;
            foreach (DiagramObject diaObj in eaDia.SelObjects)
            {
                count = count + 1;
                var elSource = eaDia.SelElements[count];
                // Update Embedded Element, RequiredInterface, ProvidedInterface
                if (elSource.IsEmbeddedElement() |
                    "ProvidedInterface RequiredInterface".Contains(elSource.Type))
                {
                    
                    PortServices.DoChangeLabelStyle(diaObj, style);
                }
                else
                {
                    // selected element was "Element"
                    foreach (Element embeddedElement in elSource.EmbeddedElements)
                    {
                        if (embeddedElement.IsEmbeddedElement())
                        {
                            var diagramObject = eaDia.Dia.GetDiagramObjectByID(embeddedElement.ElementID, "");
                            if (diagramObject == null) continue;
                            PortServices.DoChangeLabelStyle(diagramObject, style);
                        }
                    }
                }




            }
            // display changes
            rep.ReloadDiagram(eaDia.Dia.DiagramID);
            eaDia.ReloadSelectedObjectsAndConnector();
            Cursor.Current = Cursors.Default;
        }

        #endregion



        /// <summary>
        /// Delete the embedded element from Diagram (Port, Parameter, Pin)
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="embeddedElement"></param>
        private static void RemoveEmbeddedElementFromDiagram(Diagram dia, EA.Element embeddedElement)
        {
            if (!embeddedElement.IsEmbeddedElement()) return;
            for (int i = dia.DiagramObjects.Count - 1; i >= 0; i -= 1)
            {
                var obj = (DiagramObject) dia.DiagramObjects.GetAt((short) i);
                if (obj.ElementID == embeddedElement.ElementID)
                {
                    dia.DiagramObjects.Delete((short) i);
                    dia.DiagramObjects.Refresh();
                    break;
                }
            }


        }

        public static void NavigateComposite(Repository repository)
        {
            ObjectType oType = repository.GetContextItemType();
            // find composite element of diagram
            if (oType.Equals(ObjectType.otDiagram))
            {
                var d = (Diagram) repository.GetContextObject();
                string guid = Util.GetElementFromCompositeDiagram(repository, d.DiagramGUID);
                if (guid != "")
                {
                    repository.ShowInProjectView(repository.GetElementByGuid(guid));
                }
            }
            // find composite diagram of element of element
            if (oType.Equals(ObjectType.otElement))
            {
                var e = (Element) repository.GetContextObject();
                // locate text or frame
                if (LocateTextOrFrame(repository, e)) return;
                if (e.CompositeDiagram == null) return;
                repository.ShowInProjectView(e.CompositeDiagram);
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
        [ServiceOperation("{755DD068-94A2-4AD9-84EE-F3D1350BC9B7}",
            "Find usage of Element,Method, Attribute, Diagram, Connector ", "Select item", false)]
        public static void FindUsage(Repository rep)
        {
            string errorMessageSearchNotFind = @"Have you installed and enabled search, e.g. by standard MDG:
- hoToolsBasic.xml or
- hoToolsBasicCompilation.xml
from %APPDATA%Local\Apps\hoTools\
?
";
            ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(ObjectType.otElement))
            {
                // locate text or frame
                var el = (Element) rep.GetContextObject();
                if (LocateTextOrFrame(rep, el)) return;
                try
                {
                    rep.RunModelSearch("Element usage", el.ElementGUID, "", "");
                }
                catch
                {
                    MessageBox.Show(errorMessageSearchNotFind, "Search 'Element usage not defined' missing, Break!!");
                }
            }
            if (oType.Equals(ObjectType.otMethod))
            {
                try { 

                    var method = (Method) rep.GetContextObject();
                    rep.RunModelSearch("Method usage", method.MethodGUID, "", "");
                }
                    catch
                {
                    MessageBox.Show(errorMessageSearchNotFind, "Search 'Element usage not defined' missing, Break!!");
                }
            }
            if (oType.Equals(ObjectType.otDiagram))
            {
                try { 
                var dia = (Diagram) rep.GetContextObject();
                rep.RunModelSearch("Diagram usage", dia.DiagramGUID, "", "");
                }
                        catch
                {
                    MessageBox.Show(errorMessageSearchNotFind, "Search 'Diagram usage' missing, Break!!");
                }
            }
            if (oType.Equals(ObjectType.otConnector))
            {
                try { 
                var con = (Connector) rep.GetContextObject();
                rep.RunModelSearch("Connector is visible in Diagrams",
                    con.ConnectorID.ToString(), "", "");
                 }
                        catch
                    {
                        MessageBox.Show(errorMessageSearchNotFind, "Search 'Connector is visible in Diagrams', Break!!");
                    }
                }
            }

        #endregion

        /// <summary>
        /// Show all specifications of selected Element. It shows all files in specified in properties.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{899C9C5F-39B8-47E3-B253-7C5730F1AA7D}",
            "Show all specifications of selected element (all files defined in properties)",
            "Select item", isTextRequired: false)]
        public static void ShowSpecification(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(ObjectType.otElement))
            {
                var el = (Element) rep.GetContextObject();
                //over all file
                foreach (File f in el.Files)
                {
                    if (f.Name.Length > 2)
                    {
                        Util.StartFile(f.Name);
                    }
                }
            }
        }

        #region LineStyle

        #region setLineStyleLV

        [ServiceOperation("{5F5CB088-1DDD-4A00-B641-273CAC017AE5}", "Set line style LV(Lateral Vertical)",
            "Select Diagram, connector, nodes", isTextRequired: false)]

        #endregion

        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SetLineStyleLv(Repository rep)
        {
            SetLineStyle(rep, "LV");
        }

        [ServiceOperation("{9F1E7448-3B3B-4058-83AB-CBA97F24B90B}", "Set line style LH(Lateral Horizontal)",
            "Select Diagram, connector, nodes", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SetLineStyleLh(Repository rep)
        {
            SetLineStyle(rep, "LH");
        }

        [ServiceOperation("{A8199FFF-A9BA-4875-9529-45B2801F0DB3}", "Set line style TV(Tree Vertical)",
            "Select Diagram, connector, nodes", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SetLineStyleTv(Repository rep)
        {
            SetLineStyle(rep, "TV");
        }

        [ServiceOperation("{5E481745-C684-431D-BD02-AD22EE39C252}", "Set line style TH(Tree Horizontal)",
            "Select Diagram, connector, nodes", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SetLineStyleTh(Repository rep)
        {
            SetLineStyle(rep, "TH");
        }

        [ServiceOperation("{A8199FFF-A9BA-4875-9529-45B2801F0DB3}", "Set line style OS(Orthogonal Square)",
            "Select Diagram, connector, nodes", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SetLineStyleOs(Repository rep)
        {
            SetLineStyle(rep, "OS");
        }

        [ServiceOperation("{D7B75725-60B7-4C73-913F-164E6EE847D3}", "Set line style OR(Orthogonal Round)",
            "Select Diagram, connector, nodes", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        public static void SetLineStyleOr(Repository rep)
        {
            SetLineStyle(rep, "OR");
        }

        [ServiceOperation("{99F31FC7-8326-468B-B1D8-2542BBC8D4EB}", "Set line style B(Bezier)",
            "Select Diagram, connector, nodes", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        public static void SetLineStyleB(Repository rep)
        {
            SetLineStyle(rep, "B");
        }

        public static void SetLineStyle(Repository repository, string lineStyle)
        {
            Connector con = null;
            Collection objCol = null;
            ObjectType oType = repository.GetContextItemType();
            Diagram diaCurrent = repository.GetCurrentDiagram();
            if (diaCurrent != null)
            {
                con = diaCurrent.SelectedConnector;
                objCol = diaCurrent.SelectedObjects;
            }
            // all connections of diagram
            if (oType.Equals(ObjectType.otDiagram))
            {
                Util.SetLineStyleDiagram(repository, diaCurrent, lineStyle);
            }
            // current connector + all connections of selected diagram elements
            if (con != null || objCol?.Count > 0)
            {
                Util.SetLineStyleDiagramObjectsAndConnectors(repository, diaCurrent, lineStyle);
            }
        }

        #endregion

        #region DisplayOperationForSelectedElement

        // display behavior or definition for selected element
        // enum displayMode: "Behavior" or "Method"
        public static void DisplayOperationForSelectedElement(Repository repository, DisplayMode showBehavior)
        {
            ObjectType oType = repository.GetContextItemType();
            // Method found
            if (oType.Equals(ObjectType.otMethod))
            {
                // display behavior for method
                Appl.DisplayBehaviorForOperation(repository, (Method) repository.GetContextObject());
            }
            if (oType.Equals(ObjectType.otDiagram))
            {
                // find parent element
                var dia = (Diagram) repository.GetContextObject();
                if (dia.ParentID > 0)
                {
                    // find parent element
                    Element parentEl = repository.GetElementByID(dia.ParentID);
                    //
                    LocateOperationFromBehavior(repository, parentEl, showBehavior);
                }
                else
                {
                    // open diagram
                    repository.OpenDiagram(dia.DiagramID);
                }
            }


            // Connector / Message found
            if (oType.Equals(ObjectType.otConnector))
            {
                var con = (Connector) repository.GetContextObject();
                SelectBehaviorFromConnector(repository, con, showBehavior);
            }

            // Element
            if (oType.Equals(ObjectType.otElement))
            {
                var el = (Element) repository.GetContextObject();
                // locate text or frame
                if (LocateTextOrFrame(repository, el)) return;

                if (el.Type.Equals("Activity") & showBehavior.Equals(DisplayMode.Behavior))
                {
                    // Open Behavior for Activity
                    Util.OpenBehaviorForElement(repository, el);
                }
                if (el.Type.Equals("State"))
                {
                    // get operations
                    foreach (Method m in el.Methods)
                    {
                        // display behaviors for methods
                        Appl.DisplayBehaviorForOperation(repository, m);
                    }
                }

                if (el.Type.Equals("Action"))
                {
                    foreach (CustomProperty custproperty in el.CustomProperties)
                    {
                        if (custproperty.Name.Equals("kind") && custproperty.Value.Equals("CallOperation"))
                        {
                            ShowFromElement(repository, el, showBehavior);
                        }
                        if (custproperty.Name.Equals("kind") && custproperty.Value.Equals("CallBehavior"))
                        {
                            el = repository.GetElementByID(el.ClassfierID);
                            Util.OpenBehaviorForElement(repository, el);
                        }
                    }
                }
                if (showBehavior.Equals(DisplayMode.Method) & (
                        el.Type.Equals("Activity") || el.Type.Equals("StateMachine") || el.Type.Equals("Interaction")))
                {
                    LocateOperationFromBehavior(repository, el, showBehavior);
                }
            }
        }

        #endregion

        /// <summary>
        /// Display behavior or definition for selected connector ("StateFlow","Sequence"). It Displays the operation (displayMode=Method) or the Behavior (displayMode=Behavior).
        /// <para/>- enum displayMode: "Behavior" or "Method"
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="con"></param>
        /// <param name="showBehavior"></param>
        static void SelectBehaviorFromConnector(Repository repository, Connector con, DisplayMode showBehavior)
        {
            if (con.Type.Equals("StateFlow"))
            {
                Method m = Util.GetOperationFromConnector(repository, con);
                if (m != null)
                {
                    if (showBehavior.Equals(DisplayMode.Behavior))
                    {
                        Appl.DisplayBehaviorForOperation(repository, m);
                    }
                    else
                    {
                        repository.ShowInProjectView(m);
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
                    Element el = repository.GetElementByID(con.SupplierID);
                    // find operation by name
                    foreach (Method op in el.Methods)
                    {
                        if (op.Name == opName)
                        {
                            repository.ShowInProjectView(op);
                            //Appl.DisplayBehaviorForOperation(Repository, op);
                            return;
                        }
                    }
                    // If connector 0 Sequence and Classifier exists
                    if (con.Type.Equals("Sequence") && (el.ClassfierID > 0 || el.PropertyType > 0))
                    {
                        if ("PartPort".Contains(el.Type))
                        {
                            if (el.PropertyType > 0)
                            {
                                el = repository.GetElementByID(el.PropertyType);
                            }
                            else
                            {
                                if (el.ClassifierID > 0)
                                    el = repository.GetElementByID(el.ClassifierID);
                                else return;

                            }
                        }
                        else
                        {
                            if (el.ClassifierID > 0)
                                el = repository.GetElementByID(el.ClassifierID);
                        }

                        foreach (Method op in el.Methods)
                        {
                            if (op.Name == opName)
                            {
                                if (showBehavior.Equals(DisplayMode.Behavior))
                                {
                                    Appl.DisplayBehaviorForOperation(repository, op);
                                }
                                else
                                {
                                    repository.ShowInProjectView(op);
                                }
                            }
                        }
                    }
                }
            }
        }


        static void ShowFromElement(Repository repository, Element el, DisplayMode showBehavior)
        {
            Method method = Util.GetOperationFromAction(repository, el);
            if (method != null)
            {
                if (showBehavior.Equals(DisplayMode.Behavior))
                {
                    Appl.DisplayBehaviorForOperation(repository, method);
                }
                else
                {
                    repository.ShowInProjectView(method);
                }
            }
        }

        static void LocateOperationFromBehavior(Repository repository, Element el, DisplayMode showBehavior)
        {
            Method method = Util.GetOperationFromBrehavior(repository, el);
            if (method != null)
            {
                if (showBehavior.Equals(DisplayMode.Behavior))
                {
                    Appl.DisplayBehaviorForOperation(repository, method);
                }
                else
                {
                    repository.ShowInProjectView(method);
                }
            }
        }

        // ReSharper disable once UnusedMember.Local
        static void BehaviorForOperation(Repository repository, Method method)
        {
            string behavior = method.Behavior;
            if (behavior.StartsWith("{", StringComparison.Ordinal) & behavior.EndsWith("}", StringComparison.Ordinal))
            {
                // get object according to behavior
                Element el = repository.GetElementByGuid(behavior);
                // Activity
                if (el == null)
                {
                }
                else
                {
                    if (el.Type.Equals("Activity") || el.Type.Equals("Interaction") || el.Type.Equals("StateMachine"))
                    {
                        Util.OpenBehaviorForElement(repository, el);
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
        private static DiagramObject CreateDiagramObjectFromContext(Repository rep, string name, string type,
            string extension, int offsetHorizental = 0, int offsetVertical = 0, string guardString = "",
            Element srcEl = null)
        {
            int widthPerCharacter = 60;
            // filter out linefeed, tab
            name = Regex.Replace(name, @"(\n|\r|\t)", "", RegexOptions.Singleline);

            if (name.Length > 255)
            {
                MessageBox.Show($@"{type}: '{name}' has more than 255 characters.", @"Name is to long");
                return null;
            }
            Element elParent = null;
            Element elTarget;

            string basicType = type;
            if (type == "CallOperation") basicType = "Action";

            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return null;

            rep.SaveDiagram(dia.DiagramID);

            // only one diagram object selected as source
            var elSource = srcEl ?? Util.GetElementFromContextObject(rep);
            if (elSource == null) return null;
            var diaObjSource = Util.GetDiagramObjectById(rep, dia, elSource.ElementID);
            //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");

            string noValifTypes = "Note, Constraint, Boundary, Text, UMLDiagram, DiagramFrame";
            if (noValifTypes.Contains(elSource.Type)) return null;


            if (elSource.ParentID != 0)
            {
                Util.GetDiagramObjectById(rep, dia, elSource.ParentID);
                //diaObjParent = dia.GetDiagramObjectByID(elSource.ParentID, "");
            }

            try
            {
                if (elSource.ParentID > 0)
                {
                    elParent = rep.GetElementByID(elSource.ParentID);
                    elTarget = (Element) elParent.Elements.AddNew(name, basicType);
                    if (basicType == "StateNode") elTarget.Subtype = Convert.ToInt32(extension);
                    elParent.Elements.Refresh();
                }
                else
                {
                    var pkg = rep.GetPackageByID(elSource.PackageID);
                    elTarget = (Element) pkg.Elements.AddNew(name, basicType);
                    if (basicType == "StateNode") elTarget.Subtype = Convert.ToInt32(extension);
                    pkg.Elements.Refresh();
                }
                elTarget.ParentID = elSource.ParentID;
                elTarget.Update();
                if (basicType == "Activity" & extension.ToLower() == "comp=yes")
                {
                    EA.Diagram actDia = ActivityPar.CreateActivityCompositeDiagram(rep, elTarget);
                    Util.SetActivityCompositeDiagram(rep, elTarget, actDia.DiagramID.ToString());
                    //elTarget.
                }
            }
            catch
            {
                return null;
            }

            int left = diaObjSource.left + offsetHorizental;
            int right = diaObjSource.right + offsetHorizental;
            int top = diaObjSource.top + offsetVertical;
            int bottom = diaObjSource.bottom + offsetVertical;
            int length;

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
                    if (elSource.Type == "Decision") left = left + (right - left) + 200;
                    else left = left + (right - left) + 50;
                    bottom = bottom - 5;
                }
                left = left - 15 + (right - left) / 2;
                right = left + 30;
                top = bottom - 20;
                bottom = top - 40;
            }
            if (basicType == "Action" | basicType == "Activity")
            {
                length = name.Length * widthPerCharacter / 10;

                if (extension.ToLower() == "comp=no")
                {
                    /* Activity ind diagram */
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
            if (elParent != null && elParent.Type == "Activity" && extension == "101")
            {
                DiagramObject diaObj = Util.GetDiagramObjectById(rep, dia, elParent.ElementID);
                //EA.DiagramObject diaObj = dia.GetDiagramObjectByID(elParent.ElementID,"");
                if (diaObj != null)
                {
                    diaObj.bottom = bottom - 40;
                    diaObj.Update();
                }
            }


            Util.AddSequenceNumber(rep, dia);
            var diaObjTarget = (DiagramObject) dia.DiagramObjects.AddNew(position, "");
            diaObjTarget.ElementID = elTarget.ElementID;
            diaObjTarget.Sequence = 1;
            diaObjTarget.Update();
            Util.SetSequenceNumber(rep, dia, diaObjTarget, "1");

            // position the label:
            // LBL=CX=180:  length of label
            // CY=13:       hight of label
            // OX=26:       x-position of label (relative object)
            // CY=13:       y-position of label (relative object)
            if (basicType == "Decision" & name.Length > 0)
            {
                if (name.Length > 25) length = 25 * widthPerCharacter / 10;
                else length = name.Length * widthPerCharacter / 10;
                // string s = "DUID=E2352ABC;LBL=CX=180:CY=13:OX=29:OY=-4:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;;"; 
                string s = "DUID=E2352ABC;LBL=CX=180:CY=13:OX=-" + length +
                           ":OY=-4:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;;";
                Util.SetDiagramObjectLabel(rep,
                    diaObjTarget.ElementID, diaObjTarget.DiagramID, diaObjTarget.InstanceID, s);
            }

            if (extension == "Comp=no")
            {
                /* Activity ind diagram */
                // place an init
                int initLeft = left + ((right - left) / 2) - 10;
                int initRight = initLeft + 20;
                int initTop = top - 25;
                int initBottom = initTop - 20;
                string initPosition = "l=" + initLeft + ";r=" + initRight + ";t=" + initTop + ";b=" + initBottom + ";";
                ActivityPar.CreateInitFinalNode(rep, dia,
                    elTarget, 100, initPosition);
            }

            // draw a Control Flow
            var con = (Connector) elSource.Connectors.AddNew("", "ControlFlow");
            con.SupplierID = elTarget.ElementID;
            con.Update();
            elSource.Connectors.Refresh();
            // set line style LV
            foreach (DiagramLink link in dia.DiagramLinks)
            {
                if (link.ConnectorID == con.ConnectorID)
                {
                    if (guardString != "no")
                    {
                        link.Geometry =
                            "EDGE=3;$LLB=;LLT=;LMT=;LMB=CX=21:CY=13:OX=-20:OY=-19:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:DIR=0:ROT=0;LRT=;LRB=;IRHS=;ILHS=;";
                    }
                    // in case of switch case line style = LH
                    string style = "LV";
                    if ((elSource.Type == "Action" | elSource.Type == "Activity") & guardString == "no") style = "LH";
                    if (Regex.IsMatch(elSource.Name, @"switch[\s]*\(")) style = "OS";
                    Util.SetLineStyleForDiagramLink(style, link);

                    break;
                }
            }


            // set Guard
            if (guardString != "")
            {
                if (guardString == "no" && elSource.Type != "Decision")
                {
                    // mo GUARD
                }
                else
                {
                    // GUARD
                    Util.SetConnectorGuard(rep, con.ConnectorID, guardString);
                }
            }
            else if (elSource.Type.Equals("Decision") & !elSource.Name.Trim().Equals(""))
            {
                Util.SetConnectorGuard(rep, con.ConnectorID, guardString == "no" ? "no" : "yes");
            }

            // handle subtypes of action
            if (type == "CallOperation")
            {
                Method method = CallOperationAction.GetMethodFromMethodName(rep, extension);
                if (method != null)
                {
                    CallOperationAction.CreateCallAction(rep, elTarget, method);
                }
            }

            rep.ReloadDiagram(dia.DiagramID);

            // set selected object
            dia.SelectedObjects.AddNew(diaObjTarget.ElementID.ToString(), diaObjTarget.ObjectType.ToString());
            dia.SelectedObjects.Refresh();
            return diaObjTarget;
        }

        #endregion

        #region insertInterface

        // ReSharper disable once UnusedMember.Local
        static void InsertInterface(Repository rep, Diagram dia, string text)
        {
            bool isComponent = false;
            Package pkg = rep.GetPackageByID(dia.PackageID);
            int pos = 0;

            // only one diagram object selected as source
            if (dia.SelectedObjects.Count != 1) return;

            // save selected object
            DiagramObject objSelected = (DiagramObject) dia.SelectedObjects.GetAt(0);


            rep.SaveDiagram(dia.DiagramID);
            var diaObjSource = (DiagramObject) dia.SelectedObjects.GetAt(0);
            var elSource = rep.GetElementByID(diaObjSource.ElementID);
            isComponent |= elSource.Type == "Component";
            // remember selected object

            List<Element> ifList = GetInterfacesFromText(rep, pkg, text);
            foreach (Element elTarget in ifList)
            {
                if (elSource.Locked)
                {
                    MessageBox.Show($@"Source '{elSource.Name}' is locked", @"Element locked");
                    continue;
                }
                if (isComponent)
                {
                    AddPortToComponent(elSource, elTarget);
                }
                else
                {
                    AddInterfaceToElement(rep, pos, elSource, elTarget, dia, diaObjSource);
                }
                pos = pos + 1;
            }
            // visualize ports
            if (isComponent)
            {
                dia.SelectedObjects.AddNew(diaObjSource.ElementID.ToString(), ObjectType.otElement.ToString());
                dia.SelectedObjects.Refresh();
                ShowEmbeddedElements(rep);
            }

            // reload selected object
            if (objSelected != null)
            {
                dia.SelectedObjects.AddNew(elSource.ElementID.ToString(), elSource.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
            }
            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(elSource.ElementID.ToString(), elSource.ObjectType.ToString());
            dia.SelectedObjects.Refresh();
        }

        #endregion

        static void AddInterfaceToElement(Repository rep, int pos, Element elSource, Element elTarget, Diagram dia,
            DiagramObject diaObjSource)
        {
            // check if interface already exists on diagram

            var diaObjTarget = Util.GetDiagramObjectById(rep, dia, elTarget.ElementID);
            //diaObjTarget = dia.GetDiagramObjectByID(elTarget.ElementID, "");
            if (diaObjTarget == null)
            {
                int length = 250;
                //if (elTarget.Type != "Interface") length = 250;
                // calculate target position
                // int left = diaObjSource.right - 75;
                int left = diaObjSource.right;
                int right = left + length;
                int top = diaObjSource.bottom - 25;

                top = top - 20 - pos * 70;
                var bottom = top - 50;
                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";


                // create target diagram object
                diaObjTarget = (DiagramObject) dia.DiagramObjects.AddNew(position, "");

                diaObjTarget.ElementID = elTarget.ElementID;
                diaObjTarget.Sequence = 1;
                // suppress attributes/operations
                //"DUID=1263D775;AttPro=0;AttPri=0;AttPub=0;AttPkg=0;AttCustom=0;OpCustom=0;PType=0;RzO=1;OpPro=0;OpPri=0;OpPub=0;OpPkg=0;";
                diaObjTarget.Style =
                    "DUID=1263D775;AttPro=0;AttPri=0;AttPub=0;AttPkg=0;AttCustom=0;OpCustom=0;PType=0;RzO=1;OpPro=0;OpPri=0;OpPub=0;OpPkg=0;";
                diaObjTarget.Update();
            }
            // connect source to target by Usage

            // make a connector/ or link if notes
            // check if connector already exists
            Connector con;
            foreach (Connector c in elSource.Connectors)
            {
                if (c.SupplierID == elTarget.ElementID &
                    (c.Type == "Usage" | c.Stereotype == "use" |
                     c.Type == "Realisation")) return;
            }


            if (elTarget.Type.Equals("Interface"))
            {
                con = (Connector) elSource.Connectors.AddNew("", "Usage");
            }
            else
            {
                con = (Connector) elSource.Connectors.AddNew("", "NoteLink");
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
                foreach (DiagramLink link in dia.DiagramLinks)
                {
                    if (link.ConnectorID == con.ConnectorID)
                    {
                        Util.SetLineStyleForDiagramLink("LV", link);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),
                    $@"Error create connector between '{elSource.Name}  and '{elTarget.Name}' ");
            }
        }

        static void AddPortToComponent(Element elSource, Element elInterface)
        {
            if (elInterface.Type != "Interface") return;

            // check if port with interface already exists
            foreach (Element p in elSource.EmbeddedElements)
            {
                if (p.Name == elInterface.Name) return;
            }
            // create a port
            var port = (Element) elSource.EmbeddedElements.AddNew(elInterface.Name, "Port");
            elSource.EmbeddedElements.Refresh();
            // add interface
            var interf = (Element) port.EmbeddedElements.AddNew(elInterface.Name, "RequiredInterface");
            // set classifier
            interf.ClassfierID = elInterface.ElementID;
            interf.Update();
        }


        private static List<Element> GetInterfacesFromText(Repository rep, Package pkg, string s,
            bool createWarningNote = true)
        {
            var elList = new List<Element>();
            s = DeleteComment(s);
            // string pattern = @"#include\s*[""<]([^.]*)\.h";
            string patternPath = @"#include\s*[""<]([^"">]*)";

            Match matchPath = Regex.Match(s, patternPath, RegexOptions.Multiline);
            while (matchPath.Success)
            {
                string includePath = matchPath.Groups[1].Value;
                // get includeName
                string includeName = Regex.Match(includePath, @"([\w-]*)\.h").Groups[1].Value;


                Element el = CallOperationAction.GetElementFromName(rep, includeName, "Interface");
                if (el == null && createWarningNote)
                {
                    // create a note
                    el = (Element) pkg.Elements.AddNew("", "Note");
                    el.Notes = "Interface '" + includeName + "' not available!";
                    el.Update();
                }
                elList.Add(el);
                matchPath = matchPath.NextMatch();
            }


            return elList;
        }


        // ReSharper disable once UnusedMember.Global
        public static void InsertInActivtyDiagram(Repository rep, string text)
        {
            // remember selected object
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (dia.Type != "Activity") return;

            Element elSource = Util.GetElementFromContextObject(rep);
            if (elSource == null) return;
            DiagramObject objSource = null;
            bool isSwitchCase = false;

            if (Regex.IsMatch(elSource.Name, @"switch[\s]*\("))
            {
                isSwitchCase = true;
                objSource = Util.GetDiagramObjectById(rep, dia, elSource.ElementID);
                //objSource = dia.GetDiagramObjectByID(elSource.ElementID, "");
            }

            int offsetHorizontal = 0;
            int offsetVertical = 0;
            // delete comments /* to */
            string s0 = DeleteComment(text);

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
                if (!(match.Value.Contains("#")))
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
                if (!(match.Value.Contains("#")))
                {
                    //if (Regex.IsMatch(old, @"#[\s]*(if|elseif|else)", RegexOptions.Singleline)) continue;
                    //s0 = s0.Replace(match.Groups[1].Value, Regex.Replace(old, "\r\n", ""));
                    // check if this is no if(..)
                    if (match.Value.StartsWith("if", StringComparison.CurrentCulture))
                    {
                    }
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
            if (lines.Length > 0)
            {
                string line0 = lines[0].Trim();
                if (line0.StartsWith("else", StringComparison.Ordinal) |
                    Regex.IsMatch(lines[0], "#[ ]*else"))
                {
                    offsetHorizontal = 300;
                    guardString = "no";
                    Match matchElseIf = Regex.Match(line0, @"^else[\s]*if");
                    if (matchElseIf.Success)
                    {
                        offsetHorizontal = 0;
                        //lines[0] = lines[0].Replace(matchElseIf.Value, "");
                    }
                    else
                    {
                        skipFirstLine = true;
                    }
                }
            }
            int lineNumber = -1;

            foreach (string s in lines)
            {
                lineNumber += 1;
                var s1 = s.Trim();
                // case: of switch case
                if (isSwitchCase &
                    (s1.StartsWith("case", StringComparison.Ordinal) |
                     s1.StartsWith("default", StringComparison.Ordinal)))
                {
                    // set the selected element to the switch case
                    int l = dia.SelectedObjects.Count;
                    for (int i = l - 1; i >= 0; i--)
                    {
                        dia.SelectedObjects.DeleteAt((short) i, true);
                        dia.SelectedObjects.Refresh();
                    }
                    dia.SelectedObjects.Refresh();

                    foreach (DiagramObject obj in dia.SelectedObjects)
                    {
                        dia.SelectedObjects.DeleteAt(0, true);
                    }
                    dia.SelectedObjects.Refresh();
                    dia.SelectedObjects.AddNew(objSource.ElementID.ToString(), objSource.ObjectType.ToString());
                    dia.SelectedObjects.Refresh();
                }
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                    // check if this was the only line
                    if (lines.Length == 1)
                    {
                        // switch case with only one line
                        if (lines[0].Contains("case"))
                        {
                            CreateActionFromText(rep, s1, offsetHorizontal, offsetVertical, guardString);
                        }
                    }
                    continue;
                }

                // check if do, while, for, #if, #else, #endif, #elseif
                if (Regex.IsMatch(s1, @"^(if|else|else[\s]if|switch[\s]*\()") |
                    Regex.IsMatch(s1, @"#[ ]*(if|endif|else|ifdef|elseif)"))
                {
                    CreateDecisionFromText(rep, s1, offsetHorizontal, offsetVertical, guardString);
                    offsetHorizontal = 0;
                    guardString = "";
                }

                else
                {
                    if (s1.Length > 1)
                    {
                        if (s1.StartsWith("case", StringComparison.Ordinal) |
                            s1.StartsWith("default", StringComparison.Ordinal))
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
                                if (s2.StartsWith("case", StringComparison.Ordinal) |
                                    s2.StartsWith("default", StringComparison.Ordinal))
                                    s1 = "nothing to do";
                                else s1 = "";
                            }
                            if (!s1.Equals(""))
                                CreateActionFromText(rep, s1, offsetHorizontal, offsetVertical, guardString);
                            offsetHorizontal = 0;
                            guardString = "";
                        }
                    }
                }
            }
        }

        static string DeleteCurleyBrackets(string s)
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
        static string DeleteComment(string s)
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

        private static void CreateActionFromText(Repository rep, string s1, int offsetHorizental = 0,
            int offsetVertical = 0, string guardString = "",
            bool removeModuleNameFromMethodName = false)
        {
            // check if return
            Match matchReturn = Regex.Match(s1, @"\s*return\s*([^;]*);");
            if (matchReturn.Success)
            {
                string returnValue = "";
                if (matchReturn.Groups.Count == 2) returnValue = matchReturn.Groups[1].Value;
                CreateDiagramObjectFromContext(rep, returnValue, "StateNode", "101", 0, 0, "");
            }
            // single "case"==> composite activity  
            else if (s1.Contains("case"))
            {
                s1 = CallOperationAction.RemoveUnwantedStringsFromText(s1);
                CreateDiagramObjectFromContext(rep, s1, "Activity", "comp=yes", offsetHorizental, offsetVertical,
                    guardString);
            }
            else if (Regex.IsMatch(s1, @"^(for|while|do[\s]*$)"))
            {
                s1 = CallOperationAction.RemoveUnwantedStringsFromText(s1);
                CreateDiagramObjectFromContext(rep, s1, "Activity", "comp=no", offsetHorizental, offsetVertical,
                    guardString);
            }
            else
            {
                string methodString = CallOperationAction.RemoveUnwantedStringsFromText(s1);
                string methodName = CallOperationAction.GetMethodNameFromCallString(methodString);
                // remove module name from method name (text before '_')
                if (removeModuleNameFromMethodName)
                {
                    methodString = CallOperationAction.RemoveModuleNameFromCallString(methodString);
                }

                // check if function is available
                if (methodName != "")
                {
                    if (CallOperationAction.GetMethodFromMethodName(rep, methodName) == null)
                    {
                        CreateDiagramObjectFromContext(rep, methodString, "Action", "", offsetHorizental, offsetVertical,
                            guardString);
                    }
                    else
                    {
                        CreateDiagramObjectFromContext(rep, methodString, "CallOperation", methodName, offsetHorizental,
                            offsetVertical, guardString);
                    }
                }
                else
                {
                    CreateDiagramObjectFromContext(rep, methodString, "CallOperation", methodName, offsetHorizental,
                        offsetVertical, guardString);
                }
            }
        }

        #endregion

        #region createDecisionFromText

        // ReSharper disable once UnusedMethodReturnValue.Local
        static string CreateDecisionFromText(Repository rep, string decisionName, int offsetHorizental = 0,
            int offsetVertical = 0, string guardString = "")
        {
            decisionName = CallOperationAction.RemoveUnwantedStringsFromText(decisionName);
            string loops = "for, while, switch";
            if (!loops.Contains(decisionName.Substring(0, 3)))
            {
                decisionName = CallOperationAction.RemoveFirstParenthesisPairFromString(decisionName);
                decisionName = CallOperationAction.AddQuestionMark(decisionName);
            }
            Match match = Regex.Match(decisionName, @"else[\s]*if");
            if (match.Success)
            {
                decisionName = decisionName.Replace(match.Value, "");
                guardString = "no";
            }
            if (decisionName.StartsWith("if", StringComparison.Ordinal))
            {
                decisionName = decisionName.Substring(2).Trim();
            }

            if (Regex.IsMatch(decisionName, @"#[ ]*endif"))
            {
                CreateDiagramObjectFromContext(rep, decisionName, "MergeNode", "", offsetHorizental, offsetVertical,
                    guardString);
            }
            else
            {
                CreateDiagramObjectFromContext(rep, decisionName, "Decision", "", offsetHorizental, offsetVertical,
                    guardString);
            }
            return decisionName;
        }

        #endregion


        #region AddElementsToDiagram

        /// <summary>
        /// Add Elements (Note, Constraint,..) to diagram and link to selected Nodes in Diagram.
        /// If nothing selected add the wanted Element to the diagram
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="elementType"></param>
        /// <param name="connectorType"></param>
        /// <param name="attachNote"></param>
        public static void AddElementsToDiagram(Repository rep,
            string elementType = "Note", string connectorType = "NoteLink", Boolean attachNote = false)
        {
            // handle multiple selected elements
            Diagram diaCurrent = rep.GetCurrentDiagram();
            if (diaCurrent == null) return;
            var eaDia = new EaDiagram(rep);
            rep.SaveDiagram(diaCurrent.DiagramID);

            switch (rep.GetContextItemType())
            {
                case ObjectType.otDiagram:
                    AddDiagramNote(rep);
                    break;
                case ObjectType.otConnector:
                    AddElementWithLinkToConnector(rep, diaCurrent.SelectedConnector, elementType, connectorType,
                       attachNote);
                    break;
                case ObjectType.otElement:
                    // check for selected DiagramObjects
                    var objCol = diaCurrent.SelectedObjects;
                    if (objCol?.Count > 0)
                    {
                        foreach (EA.DiagramObject obj in objCol)
                        {
                            AddElementWithLink(rep, obj, elementType, connectorType, attachNote);
                        }
                    }
                    break;
                case ObjectType.otMethod:
                    if (attachNote == false) return;
                    AddFeatureWithNoteLink(rep, (EA.Method)rep.GetContextObject());
                    break;
                case ObjectType.otAttribute:
                    if (attachNote == false) return;
                    AddFeatureWithNoteLink(rep, (EA.Attribute)rep.GetContextObject());
                    break;
            }
            eaDia.ReloadSelectedObjectsAndConnector();
        }
        /// <summary>
        /// Add Attribute Link to Note (Feature Link)
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="attr"></param>
        private static void AddFeatureWithNoteLink(EA.Repository rep, EA.Attribute attr)
        {

            string featureType = "Attribute";
            int featureId = attr.AttributeID;
            string featureName = attr.Name;
            EA.Element elNote = rep.GetElementByID(attr.ParentID);

            SetFeatureLink(rep, elNote, featureType, featureId, featureName);
        }
        /// <summary>
        /// Add Operation Link to Note (Feature Link)
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="op"></param>
        private static void AddFeatureWithNoteLink(EA.Repository rep, EA.Method op)
        {

            string featureType = "Operation";
            int featureId = op.MethodID;
            string featureName = op.Name;
            EA.Element elNote = rep.GetElementByID(op.ParentID);

            SetFeatureLink(rep, elNote, featureType, featureId, featureName);
        }
        /// <summary>
        /// Set Link Feature to Note. The link is stored inside the note object.
        /// - Attribute
        /// - Operation
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="elNote"></param>
        /// <param name="featureType"></param>
        /// <param name="featureId"></param>
        /// <param name="featureName"></param>
        private static void SetFeatureLink(Repository rep, EA.Element elNote, string featureType,
            int featureId, string featureName)
        {
            string connectorType = "NoteLink";

            if (elNote != null)
            {
                EA.Diagram dia = rep.GetCurrentDiagram();
                EA.Package pkg = rep.GetPackageByID(elNote.PackageID);
                if (pkg.IsProtected || dia.IsLocked || elNote.Locked) return;

                EA.Element elNewNote;
                try
                {
                    elNewNote = (EA.Element)pkg.Elements.AddNew("", "Note");
                    elNewNote.Update();
                    pkg.Update();
                }
                catch
                {
                    return;
                }

                // add element to diagram
                // "l=200;r=400;t=200;b=600;"
                EA.DiagramObject diaObj = dia.GetDiagramObjectByID(elNote.ElementID, "");
                int left = diaObj.right + 50;
                int right = left + 100;
                int top = diaObj.top;
                int bottom = top - 100;

                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
                var diaObject = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
                dia.Update();
                diaObject.ElementID = elNewNote.ElementID;
                diaObject.Sequence = 1; // put element to top
                diaObject.Update();
                pkg.Elements.Refresh();

                // connect Element to node
                if (!String.IsNullOrWhiteSpace(connectorType))
                {
                    // make a connector
                    EA.Connector con = (EA.Connector)elNote.Connectors.AddNew("test", connectorType);
                    con.SupplierID = elNewNote.ElementID;
                    con.Update();
                    elNote.Connectors.Refresh();

                    // set attached link to feature (Attribute/Operation)
                    Util.SetElementLink(rep, elNewNote.ElementID, featureType, featureId, featureName, "Yes", 0);
                }
            }
        }


        /// <summary>
        /// Add Element and optionally link to  Object from:<para/>
        /// Element, Attribute, Operation, Package
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="diaObj"></param>
        /// <param name="elementType">Default Note</param>
        /// <param name="connectorType">Default: null</param>
        /// <param name="isAttchedLink"></param>
        // ReSharper disable once MemberCanBePrivate.Global
        public static void AddElementWithLink(Repository rep, DiagramObject diaObj,
            string elementType = @"Note", string connectorType = "NoteLink", bool isAttchedLink = false)
        {
            Element el = rep.GetElementByID(diaObj.ElementID);
            if (el != null)
            {
                Diagram dia = rep.GetCurrentDiagram();
                Package pkg = rep.GetPackageByID(el.PackageID);
                if (pkg.IsProtected || dia.IsLocked || el.Locked) return;

                Element elNewElement;
                try
                {
                    elNewElement = (Element) pkg.Elements.AddNew("", elementType);
                    elNewElement.Update();
                    pkg.Update();
                }
                catch
                {
                    return;
                }

                // add element to diagram
                // "l=200;r=400;t=200;b=600;"

                int left = diaObj.right + 50;
                int right = left + 100;
                int top = diaObj.top;
                int bottom = top - 100;

                string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
                var diaObject = (DiagramObject) dia.DiagramObjects.AddNew(position, "");
                dia.Update();
                diaObject.ElementID = elNewElement.ElementID;
                diaObject.Sequence = 1; // put element to top
                diaObject.Update();
                pkg.Elements.Refresh();

                // connect Element to node
                if (!String.IsNullOrWhiteSpace(connectorType))
                {
                    // make a connector
                    var con = (Connector) el.Connectors.AddNew("test", connectorType);
                    con.SupplierID = elNewElement.ElementID;
                    con.Update();
                    el.Connectors.Refresh();

                    // set attached link
                    if (isAttchedLink)
                    {
                        Util.SetElementHasAttachedElementLink(rep, el, elNewElement);
                    }
                }
            }

        }

        #endregion

        #region AddElementWithLinkToConnector

        /// <summary>
        /// Add Element and optionally link to  Object from:<para/>
        /// Element, Attribute, Operation, Package
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="con"></param>
        /// <param name="elementType">Default Note</param>
        /// <param name="connectorType">Default: null</param>
        /// <param name="isAttachedLink"></param>
        // ReSharper disable once MemberCanBePrivate.Global
        public static void AddElementWithLinkToConnector(Repository rep, Connector con,
            string elementType = @"Note", string connectorType = "NoteLink", bool isAttachedLink = false)
        {
            Diagram dia = rep.GetCurrentDiagram();
            Package pkg = rep.GetPackageByID(dia.PackageID);
            if (pkg.IsProtected || dia.IsLocked) return;

            Element elNewElement;
            try
            {
                elNewElement = (Element) pkg.Elements.AddNew("", elementType);
                elNewElement.Update();
                pkg.Update();
            }
            catch
            {
                return;
            }

            Element sourceEl = rep.GetElementByID(con.SupplierID);
            Element targetEl = rep.GetElementByID(con.ClientID);
            DiagramObject sourceObj = dia.GetDiagramObjectByID(sourceEl.ElementID, "");
            dia.GetDiagramObjectByID(targetEl.ElementID, "");

            // add element to diagram
            // "l=200;r=400;t=200;b=600;"

            int left = sourceObj.right + 50;
            int right = left + 100;
            int top = sourceObj.top;
            int bottom = top - 100;

            string position = "l=" + left + ";r=" + right + ";t=" + top + ";b=" + bottom + ";";
            var diaObject = (DiagramObject) dia.DiagramObjects.AddNew(position, "");
            dia.Update();
            diaObject.ElementID = elNewElement.ElementID;
            diaObject.Sequence = 1; // put element to top
            diaObject.Update();
            pkg.Elements.Refresh();

            Util.SetElementHasAttachedConnectorLink(rep, con, elNewElement, isAttachedLink);
            elNewElement.Refresh();
            diaObject.Update();
            dia.Update();
            pkg.Elements.Refresh();
        }

        #endregion


        // ReSharper disable once UnusedMember.Local
        private static DiagramObject GetDiagramObjectFromElement(Element el, Diagram dia)
        {
            // get the position of the Element
            DiagramObject diaObj = null;
            foreach (DiagramObject dObj in dia.DiagramObjects)
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

        public static void UpdateActivityParameter(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(ObjectType.otElement))
            {
                var el = (Element) rep.GetContextObject();
                if (el.Type.Equals("Activity"))
                {
                    // get the associated operation
                    Method m = Util.GetOperationFromBrehavior(rep, el);
                    if (el.Locked) return;
                    if (m == null) return;
                    ActivityPar.UpdateParameterFromOperation(rep, el, m); // get parameters from Operation for Activity
                    Diagram dia = rep.GetCurrentDiagram();
                    if (dia == null) return;

                    DiagramObject diaObj = Util.GetDiagramObjectById(rep, dia, el.ElementID);
                    //EA.DiagramObject diaObj = dia.GetDiagramObjectByID(el.ElementID,"");
                    if (diaObj == null) return;

                    int pos = 0;
                    rep.SaveDiagram(dia.DiagramID);
                    foreach (Element actPar in el.EmbeddedElements)
                    {
                        if (!actPar.Type.Equals("ActivityParameter")) continue;
                        Util.VisualizePortForDiagramobject(rep, pos, dia, diaObj, actPar, null);
                        pos = pos + 1;
                    }
                    rep.ReloadDiagram(dia.DiagramID);
                }
                if (el.Type.Equals("Class") | el.Type.Equals("Interface"))
                {
                    UpdateActivityParameterForElement(rep, el);
                }
            }
            if (oType.Equals(ObjectType.otMethod))
            {
                var m = (Method) rep.GetContextObject();
                Element act = Appl.GetBehaviorForOperation(rep, m);
                if (act == null) return;
                if (act.Locked) return;
                ActivityPar.UpdateParameterFromOperation(rep, act, m); // get parameters from Operation for Activity
            }
            if (oType.Equals(ObjectType.otPackage))
            {
                var pkg = (Package) rep.GetContextObject();
                UpdateActivityParameterForPackage(rep, pkg);
            }
        }

        #endregion

        static void UpdateActivityParameterForElement(Repository rep, Element el)
        {
            foreach (Method m in el.Methods)
            {
                Element act = Appl.GetBehaviorForOperation(rep, m);
                if (act == null) continue;
                if (act.Locked) continue;
                ActivityPar.UpdateParameterFromOperation(rep, act, m); // get parameters from Operation for Activity
            }
            foreach (Element elSub in el.Elements)
            {
                UpdateActivityParameterForElement(rep, elSub);
            }
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        static bool UpdateActivityParameterForPackage(Repository rep, Package pkg)
        {
            foreach (Element el in pkg.Elements)
            {
                UpdateActivityParameterForElement(rep, el);
            }
            foreach (Package pkgSub in pkg.Packages)
            {
                // update all packages
                UpdateActivityParameterForPackage(rep, pkgSub);
            }
            return true;
        }

        /// <summary>
        /// Locate the type for Connector, Method, Attribute, Diagram, Element, Package
        /// </summary>
        /// <param name="rep"></param>
        public static void LocateType(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            Element el;
            int id;
            string triggerGuid;
            // connector
            // links to trigger
            switch (oType)
            {
                case ObjectType.otConnector:
                    var con = (Connector) rep.GetContextObject();
                    triggerGuid = Util.GetTrigger(rep, con.ConnectorGUID);
                    if (triggerGuid.StartsWith("{", StringComparison.Ordinal) &&
                        triggerGuid.EndsWith("}", StringComparison.Ordinal))
                    {
                        Element trigger = rep.GetElementByGuid(triggerGuid);

                        if (trigger != null) rep.ShowInProjectView(trigger);
                    }
                    else
                    {
                        SelectBehaviorFromConnector(rep, con, DisplayMode.Method);
                    }
                    break;


                case ObjectType.otMethod:
                    var m = (Method) rep.GetContextObject();
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

                case ObjectType.otAttribute:
                    var attr = (EA.Attribute) rep.GetContextObject();
                    id = attr.ClassifierID;
                    // get type
                    if (id > 0)
                    {
                        el = rep.GetElementByID(attr.ClassifierID);
                        if (el.Type.Equals("Package"))
                        {
                            Package pkg = rep.GetPackageByID(Convert.ToInt32(el.MiscData[0]));
                            rep.ShowInProjectView(pkg);
                        }
                        else
                        {
                            rep.ShowInProjectView(el);
                        }
                    }
                    break;

                // Locate Diagram (e.g. from Search Window)
                case ObjectType.otDiagram:
                    var d = (Diagram) rep.GetContextObject();
                    rep.ShowInProjectView(d);
                    break;


                case ObjectType.otElement:
                    el = (Element) rep.GetContextObject();
                    if (el.ClassfierID > 0)
                    {
                        el = rep.GetElementByID(el.ClassfierID);
                        rep.ShowInProjectView(el);
                    }
                    else
                    {
                        //MiscData(0) PDATA1,PDATA2,
                        // pdata1 Id for parts, UmlElement
                        // object_id   for text with Hyper link to diagram

                        // locate text or frame
                        if (LocateTextOrFrame(rep, el)) return;

                        string guid = el.MiscData[0];
                        if (guid.EndsWith("}", StringComparison.Ordinal))
                        {
                            el = rep.GetElementByGuid(guid);
                            rep.ShowInProjectView(el);
                        }
                        else
                        {
                            if (el.Type.Equals("Action"))
                            {
                                foreach (CustomProperty custproperty in el.CustomProperties)
                                {
                                    if (custproperty.Name.Equals("kind") && custproperty.Value.Contains("AcceptEvent"))
                                    {
                                        // get the trigger
                                        triggerGuid = Util.GetTrigger(rep, el.ElementGUID);
                                        if (triggerGuid.StartsWith("{", StringComparison.Ordinal) &&
                                            triggerGuid.EndsWith("}", StringComparison.Ordinal))
                                        {
                                            Element trigger = rep.GetElementByGuid(triggerGuid);
                                            rep.ShowInProjectView(trigger);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (el.Type.Equals("Trigger"))
                            {
                                // get the signal
                                string signalGuid = Util.GetSignal(rep, el.ElementGUID);
                                if (signalGuid.StartsWith("RefGUID={", StringComparison.Ordinal))
                                {
                                    Element signal = rep.GetElementByGuid(signalGuid.Substring(8, 38));
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

                case ObjectType.otPackage:
                    var pkgSrc = (Package) rep.GetContextObject();
                    Package pkgTrg = Util.GetModelDocumentFromPackage(rep, pkgSrc);
                    if (pkgTrg != null) rep.ShowInProjectView(pkgTrg);
                    break;
            }
        }

        // ReSharper disable once UnusedMember.Local
        static void CreateNoteFromText(Repository rep, string text)
        {
            if (rep.GetContextItemType().Equals(ObjectType.otElement))
            {
                var el = (Element) rep.GetContextObject();
                string s0 = CallOperationAction.RemoveUnwantedStringsFromText(text.Trim(), false);
                s0 = Regex.Replace(s0, @"\/\*", "//"); // /* ==> //
                s0 = Regex.Replace(s0, @"\*\/", ""); // delete */
                el.Notes = s0;
                el.Update();
            }
        }

        public static void GetVcLatestRecursive(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            if (oType.Equals(ObjectType.otPackage) || oType.Equals(ObjectType.otNone))
            {
                // start preparation
                int count = 0;
                int errorCount = 0;
                DateTime startTime = DateTime.Now;

                rep.CreateOutputTab("Debug");
                rep.EnsureOutputVisible("Debug");
                rep.WriteOutput("Debug", "Start GetLatestRecursive", 0);
                var pkg = (Package) rep.GetContextObject();
                Util.GetLatest(rep, pkg, true, ref count, 0, ref errorCount);
                string s = "";
                if (errorCount > 0) s = " with " + errorCount + " errors";

                // finished
                TimeSpan span = DateTime.Now - startTime;
                rep.WriteOutput("Debug", "End GetLatestRecursive in " + span.Hours + ":" + span.Minutes + " hh:mm. " + s,
                    0);
            }
        }
        /// <summary>
        /// Copy SQL to Clipboard
        /// - Connector
        /// - Diagram Object
        /// - Element
        /// </summary>
        /// <param name="rep"></param>
        public static void CopyGuidSqlToClipboard(Repository rep)
        {
            string str = @"";
            string str1;
            ObjectType oType = rep.GetContextItemType();
            Diagram diaCurrent = rep.GetCurrentDiagram();
            Connector conCurrent = null;

            if (diaCurrent != null)
            {
                conCurrent = diaCurrent.SelectedConnector;
            }

            if (conCurrent != null)
            {
// Connector 
                Connector con = conCurrent;
                str = con.ConnectorGUID + " " + con.Name + ' ' + con.Type + "\r\n" +
                      "\r\n Connector: Select ea_guid As CLASSGUID, connector_type As CLASSTYPE,* from t_connector con where ea_guid = '" +
                      con.ConnectorGUID + "'" +
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
            if (oType.Equals(ObjectType.otElement))
            {
// Element 
                var el = (Element) rep.GetContextObject();
                string pdata1 = el.MiscData[0];
                string pdata1String;
                if (pdata1.EndsWith("}", StringComparison.Ordinal))
                {
                    pdata1String = "/" + pdata1;
                }
                else
                {
                    pdata1 = "";
                    pdata1String = "";
                }
                string classifier = Util.GetClassifierGuid(rep, el.ElementGUID);
                str = el.ElementGUID + ":" + classifier + pdata1String + " " + el.Name + ' ' + el.Type + "\r\n" +
                      "\r\nSelect ea_guid As CLASSGUID, object_type As CLASSTYPE,* from t_object o where ea_guid = '" +
                      el.ElementGUID + "'";
                if (classifier != "")
                {
                    if (el.Type.Equals("ActionPin"))
                    {
                        str = str +
                              "\r\n Type:\r\nSelect ea_guid As CLASSGUID, 'Parameter' As CLASSTYPE,* from t_operationparams op where ea_guid = '" +
                              classifier + "'";
                    }
                    else
                    {
                        str = str +
                              "\r\n Type:\r\nSelect ea_guid As CLASSGUID, object_type As CLASSTYPE,* from t_object o where ea_guid = '" +
                              classifier + "'";
                    }
                }
                if (pdata1 != "")
                {
                    str = str +
                          "\r\n PDATA1:  Select ea_guid As CLASSGUID, object_type As CLASSTYPE,* from t_object o where ea_guid = '" +
                          pdata1 + "'";
                }

                // Look for diagram object
                Diagram curDia = rep.GetCurrentDiagram();
                if (curDia != null)
                {
                    foreach (DiagramObject diaObj in curDia.DiagramObjects)
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

            if (oType.Equals(ObjectType.otDiagram))
            {
// Element 
                var dia = (Diagram) rep.GetContextObject();
                str = dia.DiagramGUID + " " + dia.Name + ' ' + dia.Type + "\r\n" +
                      "\r\nSelect ea_guid As CLASSGUID, diagram_type As CLASSTYPE,* from t_diagram dia where ea_guid = '" +
                      dia.DiagramGUID + "'";
            }
            if (oType.Equals(ObjectType.otPackage))
            {
// Element 
                var pkg = (Package) rep.GetContextObject();
                str = pkg.PackageGUID + " " + pkg.Name + ' ' + " Package " + "\r\n" +
                      "\r\nSelect ea_guid As CLASSGUID, 'Package' As CLASSTYPE,* from t_package pkg where ea_guid = '" +
                      pkg.PackageGUID + "'";
            }
            if (oType.Equals(ObjectType.otAttribute))
            {
// Element 
                str1 = "LEFT JOIN  t_object typAttr on (attr.Classifier = typAttr.object_id)";
                if (rep.ConnectionString.Contains(".eap"))
                {
                    str1 = "LEFT JOIN  t_object typAttr on (attr.Classifier = Format(typAttr.object_id))";
                }
                var attr = (EA.Attribute) rep.GetContextObject();
                str = attr.AttributeID + " " + attr.Name + ' ' + " Attribute " + "\r\n" +
                      "\r\n " +
                      "\r\nSelect ea_guid As CLASSGUID, 'Attribute' As CLASSTYPE,* from t_attribute attr where ea_guid = '" +
                      attr.AttributeGUID + "'" +
                      "\r\n Class has Attributes:" +
                      "\r\nSelect attr.ea_guid As CLASSGUID, 'Attribute' As CLASSTYPE, " +
                      "\r\n       o.Name As Class, o.object_type, " +
                      "\r\n       attr.Name As AttrName, attr.Type As Type, " +
                      "\r\n       typAttr.Name " +
                      "\r\n   from (t_object o INNER JOIN t_attribute attr on (o.object_id = attr.object_id)) " +
                      "\r\n                   " + str1 +
                      "\r\n   where attr.ea_guid = '" + attr.AttributeGUID + "'";
            }
            if (oType.Equals(ObjectType.otMethod))
            {
// Element 
                str1 = "LEFT JOIN t_object parTyp on (par.classifier = parTyp.object_id))";
                var str2 = "LEFT JOIN t_object opTyp on (op.classifier = opTyp.object_id)";
                if (rep.ConnectionString.Contains(".eap"))
                {
                    str1 = " LEFT JOIN t_object parTyp on (par.classifier = Format(parTyp.object_id))) ";
                    str2 = " LEFT JOIN t_object opTyp  on (op.classifier  = Format(opTyp.object_id))";
                }

                var op = (Method) rep.GetContextObject();
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
        // ReSharper disable once UnusedMember.Local
        static void CreateSharedMemoryFromText(Repository rep, string txt)
        {
            ObjectType oType = rep.GetContextItemType();
            if (!oType.Equals(ObjectType.otPackage)) return;
            var pkg = (Package) rep.GetContextObject();

            string regexShm = @"#define\sSP_SHM_(.*)_(START|END)\s*(0x[0-9ABCDEF]*)";
            Match matchShm = Regex.Match(txt, regexShm, RegexOptions.Multiline);
            while (matchShm.Success)
            {
                var shm = Utils.Element.CreateElement(rep, pkg, matchShm.Groups[1].Value, "Class", @"shm");
                var ishm = Utils.Element.CreateElement(rep, pkg, "SHM_" + matchShm.Groups[1].Value, "Interface", "");

                if (matchShm.Groups[2].Value == "START")
                {
                    var shmStartAddr = matchShm.Groups[3].Value;
                    // add Tagged Value "StartAddr"
                    var tagStart = TaggedValue.AddTaggedValue(shm, "StartAddr");
                    tagStart.Value = shmStartAddr;
                    tagStart.Update();
                }
                else if (matchShm.Groups[2].Value == "END")
                {
                    var shmEndAddr = matchShm.Groups[3].Value;
                    // add Tagged Value "StartAddr"
                    var tagEnd = TaggedValue.AddTaggedValue(shm, "EndAddr");
                    tagEnd.Value = shmEndAddr;
                    tagEnd.Update();
                }
                // make realize dependency from Interface to shared memory
                bool found = false;
                foreach (Connector c in shm.Connectors)
                {
                    if (c.SupplierID == ishm.ElementID & c.Type == "Realisation")
                    {
                        found = false;
                        break;
                    }
                }
                // currently switched off
                if (found == false)
                {
                    var con = (Connector) shm.Connectors.AddNew("", "Realisation");
                    con.SupplierID = ishm.ElementID;
                    try
                    {
                        con.Update();
                    }
                    catch
                    {
                        //
                    }
                    shm.Connectors.Refresh();
                }
                // make a port with a provided interface
                Utils.Element.CreatePortWithInterface(shm, ishm, "ProvidedInterface");


                matchShm = matchShm.NextMatch();
            }
        }

        #endregion

        #region createOperationsFromTextService

        [ServiceOperation("{E56C2722-605A-49BB-84FA-F3782697B6F9}",
            "Insert Operations in selected Class, Interface, Component", "Insert text with prototype(s)",
            isTextRequired: true)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void CreateOperationsFromTextService(Repository rep, string txt)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CreateOperationsFromText(rep, txt);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error Insert Function");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        private static void CreateOperationsFromText(Repository rep, string txt)
        {
            Diagram dia = rep.GetCurrentDiagram();
            Element el = Util.GetElementFromContextObject(rep);
            if (el == null) return;

            if (dia != null && dia.SelectedObjects.Count != 1)
            {
                dia = null;
            }

            if (dia != null) rep.SaveDiagram(dia.DiagramID);
            // delete comment
            txt = DeleteComment(txt);
            txt = DeleteCurleyBrackets(txt);

            txt = txt.Replace(";", " ");
            // delete macros
            txt = Regex.Replace(txt, @"^[\s]*#[^\n]*\n", "", RegexOptions.Multiline);

            string[] lTxt = Regex.Split(txt, @"\)[\s]*\r\n");
            for (int i = 0; i < lTxt.Length; i++)
            {
                txt = lTxt[i].Trim();
                if (txt.Equals("")) continue;
                if (!txt.EndsWith(")", StringComparison.Ordinal)) txt = txt + ")";

                CreateOperationFromText(rep, el, txt);
            }
            if (dia != null)
            {
                rep.ReloadDiagram(dia.DiagramID);
                dia.SelectedObjects.AddNew(el.ElementID.ToString(), el.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
            }

        }

        private static void CreateOperationFromText(Repository rep, Element el,
            string txt)
        {
            Method m = null;
            string functionName;
            string parameters = "";
            string functionType = "";
            int typeClassifier = 0;
            bool isStatic = false;
            // delete comment
            var leftover = DeleteComment(txt);


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
                MessageBox.Show(txt, @"No function definition");
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
                leftover = leftover.Replace("*", "").Trim();
            }
            leftover = leftover.Trim();
            regex = @"[a-zA-Z_0-9*]*$";
            match = Regex.Match(leftover, regex, RegexOptions.Multiline);
            if (match.Success)
            {
                functionType = match.Value + pointer;
                // get classifierID of type
                typeClassifier = Util.GetTypeFromName(rep, ref functionName, ref functionType);
                if (typeClassifier == 0 & functionType.Contains("*"))
                {
                    functionType = functionType.Remove(functionName.IndexOf("*", StringComparison.Ordinal), 1);
                    functionName = "*" + functionName;
                    typeClassifier = Util.GetTypeId(rep, functionType);
                    if (typeClassifier == 0 & functionType.Contains("*"))
                    {
                        functionType = functionType.Replace("*", "");
                        functionName = "*" + functionName;
                        typeClassifier = Util.GetTypeId(rep, functionType);
                    }
                }
            }
            // create function if not exists, else update function
            bool isNewFunctions = true;
            foreach (Method m1 in el.Methods)
            {
                if (m1.Name == functionName)
                {
                    isNewFunctions = false;
                    m = m1;
                    // delete all parameters
                    for (short i = (short) (m.Parameters.Count - 1); i >= 0; i--)
                    {
                        m.Parameters.Delete(i);
                        m.Parameters.Refresh();
                    }
                }
            }

            if (isNewFunctions)
            {
                m = (Method) el.Methods.AddNew(functionName, "");
                m.Pos = el.Methods.Count + 1;
                el.Methods.Refresh();
            }
            m.ReturnType = functionType;
            m.ClassifierID = typeClassifier.ToString();
            // static
            m.IsStatic = isStatic;
            m.Visibility = el.Type.Equals("Interface") ? "public" : "private";
            m.Update();


            el.Methods.Refresh();
            string[] lpar = parameters.Split(',');


            int pos = 1;
            foreach (string para in lpar)
            {
                string par = para.Trim();
                if (par == "void" | par == "") continue;
                bool isConst = false;
                if (par.Contains(@"const"))
                {
                    isConst = true;
                    par = par.Replace(@"const", "");
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
                par = Regex.Replace(par.Trim(), @"[\s]+", " ");
                string[] lparElements = par.Split(' ');
                if (lparElements.Length != 2)
                {
                    MessageBox.Show(txt, @"Can't evaluate parameters");
                    return;
                }
                string name = lparElements[lparElements.Length - 1];
                string type = lparElements[lparElements.Length - 2] + pointer;

                // get classifier ID
                var classifierId = Util.GetTypeFromName(rep, ref name, ref type);

                var elPar = (Parameter) m.Parameters.AddNew(name, "");
                m.Parameters.Refresh();
                elPar.IsConst = isConst;

                elPar.Type = type;
                elPar.Kind = "in";
                elPar.Position = pos;
                elPar.ClassifierID = classifierId.ToString();

                try
                {
                    elPar.Update();
                }
                catch (Exception e)
                {
                    MessageBox.Show($@"{e}\n\n {elPar.GetLastError()}", $@"Error creating parameter: {type} {name}");
                }
                pos = pos + 1;
            }
        }

        #region createTypeDefStructFromTextService

        [ServiceOperation("{6784026E-1B54-47CA-898F-A49EEB8A6ECB}",
            "Create/Update typedef for struct or enum from C-text for selected Class/Interface/Component",
            "Insert text with typedef\nSelect Class to generate it beneath class\nSelect typedef to update it",
            isTextRequired: true)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void CreateTypeDefStructFromTextService(Repository rep, string txt)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Diagram dia = rep.GetCurrentDiagram();
                if (dia == null) return;

                Package pkg = rep.GetPackageByID(dia.PackageID);
                if (dia.SelectedObjects.Count != 1) return;

                var el = Util.GetElementFromContextObject(rep);

                // delete comment
                txt = DeleteComment(txt);

                // delete macros
                txt = Regex.Replace(txt, @"^[\s]*#[^\n]*\n", "", RegexOptions.Multiline);

                MatchCollection matches = Regex.Matches(txt, @".*?}[\s]*[A-Za-z0-9_]*[\s]*;", RegexOptions.Singleline);
                foreach (Match match in matches)
                {
                    CreateTypeDefStructFromText(rep, dia, pkg, el, match.Value);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), @"Error insert Attributes");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        static void CreateTypeDefStructFromText(Repository rep, Diagram dia, Package pkg, Element el, string txt)
        {
            Element elTypedef = null;


            // delete comment
            txt = DeleteComment(txt);

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
            int targetId = Util.GetTypeId(rep, name);
            if (targetId != 0)
            {
                elTypedef = rep.GetElementByID(targetId);
                update = true;
                for (int i = elTypedef.Attributes.Count - 1; i > -1; i = i - 1)
                {
                    elTypedef.Attributes.DeleteAt((short) i, true);
                }
            }


            // create typedef
            if (update == false)
            {
                if (el != null)
                {
                    // create class below element
                    if ("Interface Class Component".Contains(el.Type))
                    {
                        elTypedef = (Element) el.Elements.AddNew(name, elType);
                        el.Elements.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(@"Can't create element below selected Element");
                    }
                }
                else // create class in package
                {
                    elTypedef = (Element) pkg.Elements.AddNew(name, elType);
                    pkg.Elements.Refresh();
                }
            }
            if (isStruct)
            {
                elTypedef.Stereotype = @"struct";
                EA.TaggedValue tag = TaggedValue.AddTaggedValue(elTypedef, "typedef");
                tag.Value = "true";
                tag.Update();
            }
            if (update == false)
            {
                elTypedef.Name = name;
                elTypedef.Update();
            }

            // add elements
            if (isStruct) CreateClassAttributesFromText(rep, elTypedef, txt);
            else CreateEnumerationAttributesFromText(rep, elTypedef, txt);

            if (update)
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
                var diaObj = (DiagramObject) dia.DiagramObjects.AddNew(position, "");
                dia.DiagramObjects.Refresh();
                diaObj.ElementID = elTypedef.ElementID;
                diaObj.Update();
            }
            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(elTypedef.ElementID.ToString(), elTypedef.ObjectType.ToString());
            dia.SelectedObjects.Refresh();
        }

        #region insertAttributeService

        [ServiceOperation("{BE4759E5-2E8D-454D-83F7-94AA2FF3D50A}",
            "Insert/Update Attributes in Class, Interface, Component", @"Insert text with variables, macros or enums",
            isTextRequired: true)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void InsertAttributeService(Repository rep, string txt)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CreateAttributesFromText(rep, txt);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), @"Error insert Attributes");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        public static void CreateAttributesFromText(Repository rep, string txt)
        {
            DiagramObject objSelected = null;
            Element el = Util.GetElementFromContextObject(rep);
            Diagram dia = rep.GetCurrentDiagram();
            if (!(dia == null && dia.SelectedObjects.Count > 0))
            {
                objSelected = (DiagramObject) dia.SelectedObjects.GetAt(0);
            }


            if (el == null)
            {
                MessageBox.Show(@"No Element selected, probably nothing or an attribute / operation");
                return;
            }
            if (el.Type.Equals("Class") | el.Type.Equals("Interface")) CreateClassAttributesFromText(rep, el, txt);
            if (el.Type.Equals("Enumeration")) CreateEnumerationAttributesFromText(rep, el, txt);

            if (objSelected != null)
            {
                el = rep.GetElementByID(objSelected.ElementID);
                dia.SelectedObjects.AddNew(el.ElementID.ToString(), el.ObjectType.ToString());
                dia.SelectedObjects.Refresh();
            }
        }

        static void CreateClassAttributesFromText(Repository rep, Element el, string txt)
        {
            // delete comment
            txt = DeleteComment(txt);

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
                // remove everything behind ";"
                var pos = sRaw.IndexOf(";", StringComparison.Ordinal);
                if (pos >= 0 & pos >= sRaw.Length) sRaw = sRaw.Substring(pos + 1);


                // remove everything behind "//"
                sRaw = Regex.Replace(sRaw, @"//.*", "");
                // remove everything behind "/*"
                sRaw = Regex.Replace(sRaw, @"/\*.*", "");
                sRaw = sRaw.Trim();
                if (sRaw == "") continue;
                sRaw = CallOperationAction.RemoveCasts(sRaw);


                if (sRaw.Equals("")) continue;
                if (sRaw.Contains("#") && sRaw.Contains("define"))
                {
                    CreateMacro(rep, el, sRaw, stereotype);
                    continue;
                }

                //-----------------------------------------------------------------------------
                // attributes
                // remove macros
                sRaw = Regex.Replace(sRaw, @"[\s]*#[^$]*", "");

                string name;
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
                    a = (EA.Attribute) el.Attributes.AddNew(name + defaultValue, "");
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

                isConst |= sRaw.Contains(@"const");
                isStatic |= sRaw.ToLower().Contains("static");


                string sCompact = sRaw.Replace("*", "");
                sCompact = sCompact.Replace(@"const", "");
                sCompact = sCompact.Replace(@"static ", "");
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
                    var type = match.Groups[1].Value.Trim();
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
                        a = (EA.Attribute) el.Attributes.AddNew(aName, "");
                        if (a.Name.Length > 255)
                        {
                            MessageBox.Show($@"{a.Name} has {a.Name.Length}", @"Name longer than 255");
                            continue;
                        }
                        a.Pos = el.Attributes.Count + 1;
                        el.Attributes.Refresh();
                    }

                    a.Type = type;
                    a.IsConst = isConst;
                    a.Default = defaultValue;
                    a.ClassifierID = Util.GetTypeId(rep, type);
                    a.Visibility = el.Type.Equals("Class") ? "Private" : "Public";
                    if (!collectionValue.Equals(""))
                    {
                        a.IsCollection = true;
                        a.Container = collectionValue;
                        if (collectionValue.Length > 50)
                        {
                            MessageBox.Show(
                                $@"Collection '{collectionValue}' has {collectionValue.Length}  characters.",
                                @"Break! Collection length need to be <=50 characters");
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
                        MessageBox.Show(e.ToString(), @"Error updating attribute");
                    }
                }
                else
                {
                    MessageBox.Show($@"{s}\n\n+{sCompact}", @"Couldn't understand attribute syntax");
                }
            }
        }

        #region createMacro
        /// <summary>
        /// Create Macro, a stereotype is used as a macro.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="s"></param>
        /// <param name="stereotype"></param>
        // ReSharper disable once UnusedParameter.Local
        static void CreateMacro(Repository rep, Element el, string s, string stereotype)
        {
            string name = "";
            string value = "";
            EA.Attribute a = null;
            bool isNewAttribute = true;

            // delete spaces between parameters
            s = Regex.Replace(s, @",[\s]+", ",");
            s = Regex.Replace(s, @"[\s]+,", ",");

            string regexDefine = @"#[\s]*define[\s]*([a-zA-Z0-9_(),]*)[\s]*(.*)";
            Match match = Regex.Match(s, regexDefine);
            if (match.Success)
            {
                name = match.Groups[1].Value.Trim();
                value = match.Groups[2].Value.Trim();
            }

            if (!name.Equals(""))
            {
                value = CallOperationAction.RemoveCasts(value);
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
                    a = (EA.Attribute) el.Attributes.AddNew(name, "");
                    a.Pos = el.Attributes.Count;
                    el.Attributes.Refresh();
                }
                a.Default = value;
                a.Visibility = el.Type.Equals("Interface") ? "public" : "private";

                a.IsConst = true;
                a.Stereotype = !stereotype.Equals("") ? stereotype : "define";
                a.ClassifierID = 0;
                a.Type = "";
                a.Update();
            }
            else
            {
                MessageBox.Show(s, @"Can't identify macro");
            }
        }

        #endregion

        // ReSharper disable once UnusedParameter.Local
        static void CreateEnumerationAttributesFromText(Repository rep, Element el, string txt)
        {
            // delete comment
            txt = DeleteComment(txt);


            // check for (with or without comma):
            // abc = 5 ,           or
            // abc  ,
            string regexEnum = @"([a-zA-Z_0-9]+)[\s]*(=[\s]*([a-zA-Z_0-9| ]+)|,|$)";
            Match match = Regex.Match(txt, regexEnum, RegexOptions.Multiline);
            int pos = 0;
            while (match.Success)
            {
                var a = (EA.Attribute) el.Attributes.AddNew(match.Groups[1].Value, "");
                // with/without default value
                if (match.Groups[2].Value != ",") a.Default = match.Groups[3].Value;
                a.Stereotype = "enum";
                a.Pos = pos;
                a.Update();
                el.Attributes.Refresh();
                pos = pos + 1;
                match = match.NextMatch();
            }
        }

        static void UpdateOperationTypeForPackage(Repository rep, Package pkg)
        {
            foreach (Element el1 in pkg.Elements)
            {
                foreach (Method m in el1.Methods)
                {
                    UpdateOperationType(rep, m);
                }
            }
            foreach (Package pkgSub in pkg.Packages)
            {
                UpdateOperationTypeForPackage(rep, pkgSub);
            }
        }

        // update the types of operations

        #region CreateActivityForOperation

        [ServiceOperation("{AC0111AB-10AE-4FC6-92DE-CD58F610C4E6}",
            "Update Activity Parameter from Operation, Class/Interface",
            "Select Package, Class/Interface or operation", isTextRequired: false)]
        public static void UpdateOperationTypes(Repository rep)
        {
            ObjectType oType = rep.GetContextItemType();
            switch (oType)
            {
                case ObjectType.otMethod:
                    UpdateOperationType(rep, (Method) rep.GetContextObject());
                    break;
                case ObjectType.otElement:
                    var el = (Element) rep.GetContextObject();
                    if (el.Type == "Activity")
                    {
                        Method m = Util.GetOperationFromBrehavior(rep, el);
                        if (m == null)
                        {
                            MessageBox.Show(@"Activity hasn't an operation");
                            return;
                        }
                        UpdateOperationType(rep, m);
                    }
                    else
                    {
                        foreach (Method m in el.Methods)
                        {
                            UpdateOperationType(rep, m);
                        }
                    }
                    break;

                case ObjectType.otPackage:
                    var pkg = (Package) rep.GetContextObject();
                    UpdateOperationTypeForPackage(rep, pkg);
                    break;
            }
        }

        #region updateOperationType

        /// <summary> 
        /// Update the types of the operation
        /// </summary>
        static void UpdateOperationType(Repository rep, Method m)
        {
            // update method type
            string methodName = m.Name;
            string methodType = m.ReturnType;
            if (methodType == "") methodType = "void";
            int methodClassifierId = 0;
            if (m.ClassifierID != "") methodClassifierId = Convert.ToInt32(m.ClassifierID);
            var typeChanged = UpdateTypeName(rep, ref methodClassifierId, ref methodName, ref methodType);
            if (typeChanged)
            {
                if (methodType == "")
                {
                    MessageBox.Show($@"Method {m.Name} Type '{m.ReturnType}' ",
                        @"Method type undefined");
                }
                else
                {
                    m.ClassifierID = methodClassifierId.ToString();
                    m.Name = methodName;
                    m.ReturnType = methodType;
                    m.Update();
                }
            }

            // update parameter
            // set parameter direction to "in"
            foreach (Parameter par in m.Parameters)
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
                int classifierId = 0;
                if (!par.ClassifierID.Equals("")) classifierId = Convert.ToInt32(par.ClassifierID);
                typeChanged = UpdateTypeName(rep, ref classifierId, ref parName, ref parType);
                if (typeChanged)
                {
                    if (parType == "")
                    {
                        MessageBox.Show($@"Method {m.Name} Parameter '{par.Name}: {par.Type}' ",
                            @"Parameter type undefined");
                    }
                    else
                    {
                        par.ClassifierID = classifierId.ToString();
                        par.Name = parName;
                        par.Type = parType;
                        parameterUpdated = true;
                    }
                }
                if (parameterUpdated) par.Update();
            }
        }

        #endregion

        private static bool UpdateTypeName(Repository rep, ref int classifierId, ref string parName, ref string parType)
        {
            // no classifier defined
            // check if type is correct
            Element el = null;
            if (!classifierId.Equals(0))
            {
                try
                {
                    el = rep.GetElementByID(classifierId);
                    if (el.Name != parType) el = null;
                }
                // empty catch, el = null
#pragma warning disable RECS0022
                catch //(Exception e)
                {
                    // ignored
                }
#pragma warning restore RECS0022
            }

            if (el == null)
            {
                // get the type
                // find type from type_name
                classifierId = Util.GetTypeId(rep, parType);
                // type not defined
                if (classifierId == 0)
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
                    classifierId = Util.GetTypeId(rep, parType);
                }

                if (classifierId != 0)
                {
                    return true;
                }
                parType = "";
                return true;
            }
            return false;
        }

        public static string GetAssemblyPath() => Path.GetDirectoryName(
            Assembly.GetAssembly(typeof(EaService)).CodeBase);

        // ReSharper disable once UnusedMethodReturnValue.Global
        public static bool SetNewXmlPath(Repository rep)
        {
            if (rep.GetContextItemType().Equals(ObjectType.otPackage))
            {
                var pkg = (Package) rep.GetContextObject();
                string guid = pkg.PackageGUID;

                var openFileDialogXml = new OpenFileDialog
                {
                    Filter = @"xml files (*.xml)|*.xml",
                    FileName = Util.GetVccFilePath(rep, pkg)
                };
                if (openFileDialogXml.ShowDialog() == DialogResult.OK)
                {
                    var path = openFileDialogXml.FileName;
                    string rootPath = Util.GetVccRootPath(rep, pkg);
                    // delete root path and an preceding '\'
                    string shortPath = path.Replace(rootPath, "");
                    if (shortPath.StartsWith(@"\", StringComparison.Ordinal)) shortPath = shortPath.Substring(1);

                    // write to repository
                    Util.SetXmlPath(rep, guid, shortPath);

                    // write to file
                    try
                    {
                        // set readonly attribute to false
                        System.IO.File.SetAttributes(path, FileAttributes.Normal);

                        string strFile = System.IO.File.ReadAllText(path);
                        string replace = @"value=[.0-9a-zA-Z_\\-]*\.xml";
                        string replacement = shortPath;
                        strFile = Regex.Replace(strFile, replace, replacement);
                        System.IO.File.WriteAllText(path, strFile);

                        // checkout + checkin to make the change permanent
                        pkg.VersionControlCheckout();
                        pkg.VersionControlCheckin("Re- organization *.xml files");
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString(), $@"Error writing '{path}'");
                    }

                    MessageBox.Show(path, @"Changed");
                }
            }
            return true;
        }

        #region VcReconcile

        [ServiceOperation("{EAC9246F-96FA-40E7-885A-A572E907AF86}", "Scan XMI and reconcile", "no selection required",
            isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void VcReconcile(Repository rep)
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
                MessageBox.Show($@"{e}\n\n", @"Error VC reconcile");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region checkOutService

        [ServiceOperation("{1BF01759-DD99-4552-8B68-75F19A3C593E}", "Check out", "Select Package", isTextRequired: false
        )]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void CheckOutService(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CheckOut(rep);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error Checkout");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        private static void CheckOut(Repository rep, Package pkg = null)
        {
            if (pkg == null) pkg = rep.GetTreeSelectedPackage();
            if (pkg == null) return;

            pkg = Util.GetFirstControlledPackage(rep, pkg);
            if (pkg == null) return;

            var svnHandle = new Svn(rep, pkg);
            string userNameLockedPackage = svnHandle.GetLockingUser();
            if (userNameLockedPackage != "")
            {
                MessageBox.Show($@"Package is checked out by '{userNameLockedPackage}'");
                return;
            }

            //
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                pkg.VersionControlCheckout("");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}\n\n{pkg.GetLastError()}", @"Error Checkout");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #region checkInService

        [ServiceOperation("{085C84D2-7B51-4783-8189-06E956411B94}", "Check in ",
            "Select package or something in package", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void CheckInService(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CheckIn(rep, pkg: null, withGetLatest: false, comment: "code changed");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error Checkin");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region checkInServiceWithUpdateKeyword

        [ServiceOperation("{C5BB52C6-F300-42AE-B4DC-DC97D57D8F7D}",
            "Check in with get latest (update VC keywords, if Tagged Values 'svnDate'/'svnRevision')",
            "Select package or something in package", false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void CheckInServiceWithUpdateKeyword(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CheckIn(rep, pkg: null, withGetLatest: true, comment: "code changed");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error Checkin");
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
        /// <param name="comment">A check in comment, default="0" = aks for checkin comment</param>
        static void CheckIn(Repository rep, Package pkg = null, bool withGetLatest = false, string comment = "0")
        {
            if (pkg == null) pkg = rep.GetTreeSelectedPackage();
            if (pkg == null) return;

            pkg = Util.GetFirstControlledPackage(rep, pkg);
            if (pkg == null) return;

            var svnHandle = new Svn(rep, pkg);
            string userNameLockedPackage = svnHandle.GetLockingUser();
            if (userNameLockedPackage == "")
            {
                MessageBox.Show(@"Package isn't checked out");
                return;
            }


            if (InputBox(@"Checkin comment", @"Checkin", ref comment) == DialogResult.OK)
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
                    MessageBox.Show($@"{e} \n\n {pkg.GetLastError()}", @"Error Checkin");
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
                Element el = rep.GetElementByGuid(pkg.PackageGUID);
                foreach (EA.TaggedValue t in el.TaggedValues)
                {
                    if (t.Name == "svnDoc" | t.Name == "svnRevision")
                    {
                        pkg.VersionControlResynchPkgStatus(false);
                        if (pkg.Flags.Contains("Checkout"))
                        {
                            MessageBox.Show($@"Flags={pkg.Flags}", @"Package Checked out, Break!");
                            return;
                        }
                        pkg.VersionControlGetLatest(true);
                        return;
                    }
                }
            }
        }

        #endregion

        static DialogResult InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = @"OK";
            buttonCancel.Text = @"Cancel";
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
            form.Controls.AddRange(new Control[] {label, textBox, buttonOk, buttonCancel});
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

        static bool CheckTaggedValuePackage(Package pkg)
        {
            bool workForPackage = false;
            foreach (Package pkg1 in pkg.Packages)
            {
                if (pkg1.Name.Equals("Architecture") | pkg1.Name.Equals("Behavior"))
                {
                    workForPackage = true;
                    break;
                }
            }
            return workForPackage;
        }

        static void SetDirectoryTaggedValueRecursive(Repository rep, Package pkg)
        {
            // remember Id, because of reloading package from xmi
            string pkgGuid = pkg.PackageGUID;
            if (CheckTaggedValuePackage(pkg)) SetDirectoryTaggedValues(rep, pkg);

            pkg = rep.GetPackageByGuid(pkgGuid);
            foreach (Package pkg1 in pkg.Packages)
            {
                SetDirectoryTaggedValueRecursive(rep, pkg1);
            }
        }

        public static void SetTaggedValueGui(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ObjectType oType = rep.GetContextItemType();
                if (!oType.Equals(ObjectType.otPackage)) return;
                var pkg = (Package) rep.GetContextObject();
                SetDirectoryTaggedValueRecursive(rep, pkg);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error set directory tagged values");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private static bool IsTaggedValuesComplete(Element el)
        {
            bool isRevision = false;
            bool isDate = false;
            foreach (EA.TaggedValue tag in el.TaggedValues)
            {
                isRevision |= tag.Name == "svnRevision";
                isDate |= tag.Name == "svnDate";
            }
            if (isRevision & isDate) return true;
            return false;
        }

        public static void SetDirectoryTaggedValues(Repository rep, Package pkg)
        {
            bool withCheckIn = false;
            string guid = pkg.PackageGUID;

            Element el = rep.GetElementByGuid(guid);
            if (IsTaggedValuesComplete(el)) return;
            if (pkg.IsVersionControlled)
            {
                int state = pkg.VersionControlGetStatus();
                if (state == 4)
                {
                    MessageBox.Show("", @"Package checked out by another user, break");
                    return;
                }
                if (state == 1) // checked in
                {
                    CheckOut(rep, pkg);
                    withCheckIn = true;
                }
            }
            pkg = rep.GetPackageByGuid(guid);
            SetSvnProperty(rep, pkg);

            // set tagged values
            el = rep.GetElementByGuid(guid);
            bool createSvnDate = true;
            bool createSvnRevision = true;
            foreach (EA.TaggedValue t in el.TaggedValues)
            {
                createSvnDate &= t.Name != "svnDate";
                createSvnRevision &= t.Name != "svnRevision";
            }
            EA.TaggedValue tag;
            if (createSvnDate)
            {
                tag = (EA.TaggedValue) el.TaggedValues.AddNew("svnDate", "");
                tag.Value = "$Date: $";
                el.TaggedValues.Refresh();
                tag.Update();
            }
            if (createSvnRevision)
            {
                tag = (EA.TaggedValue) el.TaggedValues.AddNew("svnRevision", "");
                tag.Value = "$Revision: $";
                el.TaggedValues.Refresh();
                tag.Update();
            }


            if (pkg.IsVersionControlled)
            {
                int state = pkg.VersionControlGetStatus();
                if (state == 2 & withCheckIn) // checked out to this user
                {
                    //EaService.checkIn(rep, pkg, "");
                    CheckIn(rep, pkg, withGetLatest: true, comment: @"svn tags added");
                }
            }
        }

        public static void SetSvnProperty(Repository rep, Package pkg)
        {
            // set SVN properties
            if (pkg.IsVersionControlled)
            {
                var svnHandle = new Svn(rep, pkg);
                svnHandle.SetProperty();
            }
        }

        public static void GotoSvnLog(Repository rep, Package pkg)
        {
            // set SVN properties
            if (pkg.IsVersionControlled)
            {
                var svnHandle = new Svn(rep, pkg);
                svnHandle.GotoLog();
            }
        }

        public static void GotoSvnBrowser(Repository rep, Package pkg)
        {
            // set SVN properties
            if (pkg.IsVersionControlled)
            {
                var svnHandle = new Svn(rep, pkg);
                svnHandle.GotoRepoBrowser();
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
        // ReSharper disable once UnusedMember.Global
        public static void InsertDiagramElementAndConnect(Repository rep, string type, string subType,
            string guardString = "")
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (dia.Type != "Activity") return;

            int count = dia.SelectedObjects.Count;
            if (count == 0) return;

            rep.SaveDiagram(dia.DiagramID);
            var oldCollection = new List<DiagramObject>();

            // get context element (last selected element)
            Element originalSrcEl = Util.GetElementFromContextObject(rep);
            if (originalSrcEl == null) return;
            int originalSrcId = originalSrcEl.ElementID;

            for (int i = count - 1; i > -1; i = i - 1)
            {
                oldCollection.Add((DiagramObject) dia.SelectedObjects.GetAt((short) i));
                // keep last selected element
                //if (i > 0) dia.SelectedObjects.DeleteAt((short)i, true);
            }

            Util.GetDiagramObjectById(rep, dia, originalSrcId);
            //EA.DiagramObject originalSrcObj = dia.GetDiagramObjectByID(originalSrcID, "");

            DiagramObject trgObj = CreateDiagramObjectFromContext(rep, "", type, subType, 0, 0, guardString,
                originalSrcEl);
            Element trgtEl = rep.GetElementByID(trgObj.ElementID);

            // if connection to more than one element make sure the new element is on the deepest position
            int offset = 50;
            if (guardString == "yes") offset = 0;
            int bottom = 1000;
            int diff = trgObj.top - trgObj.bottom;


            foreach (DiagramObject diaObj in oldCollection)
            {
                Element srcEl = rep.GetElementByID(diaObj.ElementID);
                // don't connect two times
                if (originalSrcId != diaObj.ElementID)
                {
                    var con = (Connector) srcEl.Connectors.AddNew("", "ControlFlow");
                    con.SupplierID = trgObj.ElementID;
                    if (type == "MergeNode" && guardString == "no" && srcEl.Type == "Decision")
                        con.TransitionGuard = "no";
                    con.Update();
                    srcEl.Connectors.Refresh();
                    dia.DiagramLinks.Refresh();
                    //trgtEl.Connectors.Refresh();

                    // set line style
                    string style = "LV";
                    if ((srcEl.Type == "Action" | srcEl.Type == "Activity") & guardString == "no") style = "LH";
                    var link = GetDiagramLinkForConnector(dia, con.ConnectorID);
                    if (link != null) Util.SetLineStyleForDiagramLink(style, link);
                }
                // set new high/bottom_Position
                var srcObj = Util.GetDiagramObjectById(rep, dia, srcEl.ElementID);
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
                    Element parEl = rep.GetElementByID(trgtEl.ParentID);
                    if (parEl.Type == "Activity")
                    {
                        DiagramObject parObj = Util.GetDiagramObjectById(rep, dia, parEl.ElementID);
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

        [ServiceOperation("{6946E63E-3237-4F45-B4D8-7EE0D6580FA5}", "Join nodes to the last selected node",
            "Only Activity Diagram", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void JoinDiagramObjectsToLastSelected(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count < 2) return;
            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            var trgEl = (Element) rep.GetContextObject();

            for (int i = 0; i < count; i = i + 1)
            {
                var srcObj = (DiagramObject) dia.SelectedObjects.GetAt((short) i);
                var srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                Connectors.Connector connector = GetConnectionDefault();

                var con = (Connector) srcEl.Connectors.AddNew("", connector.Type);
                con.SupplierID = trgEl.ElementID;
                con.Stereotype = connector.Stereotype;
                con.Update();
                srcEl.Connectors.Refresh();
                trgEl.Connectors.Refresh();
                dia.DiagramLinks.Refresh();
                // set line style
                DiagramLink link = GetDiagramLinkForConnector(dia, con.ConnectorID);
                if (link != null) Util.SetLineStyleForDiagramLink("LV", link);
            }

            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(trgEl.ElementID.ToString(), trgEl.ObjectType.ToString());
        }

        #endregion

        private static Connectors.Connector GetConnectionDefault() => new Connectors.Connector("DataFlow", "");

        #region splitDiagramObjectsToLastSelected

        [ServiceOperation("{521FCFEB-984B-43F0-A710-E97C29E4C8EE}",
            "Split last selected Diagram object from previous selected Diagram Objects",
            "Incoming and outgoing connections", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SplitDiagramObjectsToLastSelected(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count < 2) return;
            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            ObjectType objType = rep.GetContextItemType();
            if (!(objType.Equals(ObjectType.otElement))) return;
            var trgEl = (Element) rep.GetContextObject();

            for (int i = 0; i < count; i = i + 1)
            {
                var srcObj = (DiagramObject) dia.SelectedObjects.GetAt((short) i);
                var srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                SplitElementsByConnectorType(srcEl, trgEl, "ControlFlow");
            }

            rep.ReloadDiagram(dia.DiagramID);
            dia.SelectedObjects.AddNew(trgEl.ElementID.ToString(), trgEl.ObjectType.ToString());
        }

        #endregion

        #region splitAllDiagramObjectsToLastSelected

        [ServiceOperation("{CA29CB67-77EA-4BCC-B3B4-8893F6B0DAE2}",
            "Split last selected Diagram object from all connected Diagram Objects", "Incoming and outgoing connections",
            isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void SplitAllDiagramObjectsToLastSelected(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // target object/element
            ObjectType objType = rep.GetContextItemType();
            if (!(objType.Equals(ObjectType.otElement))) return;
            var trgEl = (Element) rep.GetContextObject();

            foreach (DiagramObject srcObj in dia.DiagramObjects)
            {
                var srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                SplitElementsByConnectorType(srcEl, trgEl);
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
        static void SplitElementsByConnectorType(Element srcEl, Element trgEl, string connectorType = "all",
            string direction = "all")
        {
            for (int i = srcEl.Connectors.Count - 1; i >= 0; i = i - 1)
            {
                var con = (Connector) srcEl.Connectors.GetAt((short) i);
                if (con.SupplierID == trgEl.ElementID &&
                    (con.Type == connectorType | connectorType == "all" | direction == "all" | direction == "in"))
                {
                    srcEl.Connectors.DeleteAt((short) i, true);
                }
                if (con.ClientID == trgEl.ElementID &&
                    (con.Type == connectorType | connectorType == "all" | direction == "all" | direction == "out"))
                {
                    srcEl.Connectors.DeleteAt((short) i, true);
                }
            }
        }

        #endregion

        // ReSharper disable once UnusedMember.Local
        private static void MakeNested(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            if (count < 2) return;

            rep.SaveDiagram(dia.DiagramID);

            // target object/element

            ObjectType objType = rep.GetContextItemType();
            if (!(objType.Equals(ObjectType.otElement))) return;

            var trgEl = (Element) rep.GetContextObject();
            if (!(trgEl.Type.Equals("Activity")))
            {
                MessageBox.Show($@"Target '{trgEl.Name}:{trgEl.Type}' isn't an Activity",
                    @" Only move below Activity is allowed");
                return;
            }
            for (int i = 0; i < count; i = i + 1)
            {
                var srcObj = (DiagramObject) dia.SelectedObjects.GetAt((short) i);
                var srcEl = rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;
                srcEl.ParentID = trgEl.ElementID;
                srcEl.Update();
            }
        }

        // ReSharper disable once UnusedMember.Local
        static void DeleteInvisibleUseRealizationDependencies(Repository rep)
        {
            Connector con;
            var dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (!rep.GetContextItemType().Equals(ObjectType.otElement)) return;

            // only one diagram object selected as source
            if (dia.SelectedObjects.Count != 1) return;

            rep.SaveDiagram(dia.DiagramID);
            var diaObjSource = (DiagramObject) dia.SelectedObjects.GetAt(0);
            var elSource = rep.GetElementByID(diaObjSource.ElementID);
            var elSourceId = elSource.ElementID;
            if (!("Interface Class".Contains(elSource.Type))) return;

            // list of all connectorIDs
            var lInternalId = new List<int>();
            foreach (DiagramLink link in dia.DiagramLinks)
            {
                con = rep.GetConnectorByID(link.ConnectorID);
                if (con.ClientID != elSourceId) continue;
                if (!("Usage Realisation".Contains(con.Type))) continue;
                lInternalId.Add(con.ConnectorID);
            }


            for (int i = elSource.Connectors.Count - 1; i >= 0; i = i - 1)
            {
                con = (Connector) elSource.Connectors.GetAt((short) i);
                var conType = con.Type;
                if ("Usage Realisation".Contains(conType))
                {
                    // check if target is..
                    var elTarget = rep.GetElementByID(con.SupplierID);
                    if (elTarget.Type == "Interface")
                    {
                        if (lInternalId.BinarySearch(con.ConnectorID) < 0)
                        {
                            elSource.Connectors.DeleteAt((short) i, true);
                        }
                    }
                }
            }
        }

        #region copyReleaseInfoOfModuleService

        [ServiceOperation("{1C78E1C0-AAC8-4284-8C25-2D776FF373BC}", "Copy release information to clipboard",
            "Select Component", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void CopyReleaseInfoOfModuleService(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CopyReleaseInfoOfModule(rep);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), @"Error generating ports for component");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        static void CopyReleaseInfoOfModule(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (!rep.GetContextItemType().Equals(ObjectType.otElement)) return;
            var elSource = (Element) rep.GetContextObject();
            if (elSource.Type != "Component") return;

            Util.GetDiagramObjectById(rep, dia, elSource.ElementID);
            //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");

            string txt = "";
            string nl = "";
            foreach (DiagramObject obj in dia.DiagramObjects)
            {
                var elTarget = rep.GetElementByID(obj.ElementID);
                if (!("Class Interface".Contains(elTarget.Type))) continue;
                txt = txt + nl + AddReleaseInformation(rep, elTarget);
                nl = "\r\n";
            }
            Clipboard.SetText(txt);
        }

        static string AddReleaseInformation(Repository rep, Element el)
        {
            string txt;
            string path = Util.GetGenFilePathElement(rep, el);
            if (path == "")
            {
                MessageBox.Show($@"No file defined in property for: '{el.Name}': {el.Type}");
                return "";
            }
            try
            {
                txt = System.IO.File.ReadAllText(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), @"Error Reading file '" + el.Name + @"':" + el.Type);
                return "";
            }
            string extension = ".c";
            if (el.Type == "Interface") extension = ".h";
            string name = el.Name + extension;
            if (name.Length > 58) name = name + "   ";
            else name = name.PadRight(60);
            return name + GetReleaseInformationFromText(txt);
        }

        private static string GetReleaseInformationFromText(string txt)
        {
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

        [ServiceOperation("{00602D5F-D581-4926-A31F-806F2D06691C}", "Generate ports for component", "Select Component",
            isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void GenerateComponentPortsService(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GenerateComponentPorts(rep);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e10)
            {
                MessageBox.Show(e10.ToString(), @"Error generating ports for component");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        static void GenerateComponentPorts(Repository rep)
        {
            int pos = 0;
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            if (!rep.GetContextItemType().Equals(ObjectType.otElement)) return;
            var elSource = (Element) rep.GetContextObject();
            if (elSource.Type != "Component") return;

            Util.GetDiagramObjectById(rep, dia, elSource.ElementID);
            //diaObjSource = dia.GetDiagramObjectByID(elSource.ElementID, "");

            rep.SaveDiagram(dia.DiagramID);
            foreach (DiagramObject obj in dia.DiagramObjects)
            {
                var elTarget = rep.GetElementByID(obj.ElementID);
                if (!("Class Interface".Contains(elTarget.Type))) continue;
                if (!(elTarget.Name.EndsWith("_i", StringComparison.Ordinal)))
                {
                    AddPortToComponent(elSource, elTarget);
                    pos = pos + 1;
                }

                if ("Interface Class".Contains(elTarget.Type))
                {
                    List<Element> lEl = GetIncludedHeaderFiles(rep, elTarget);
                    foreach (Element el in lEl)
                    {
                        if (el == null) continue;
                        if (!(el.Name.EndsWith("_i", StringComparison.Ordinal)))
                        {
                            AddPortToComponent(elSource, el);
                            pos = pos + 1;
                        }
                    }
                }
            }
            ShowEmbeddedElements(rep);
        }

        static List<Element> GetIncludedHeaderFiles(Repository rep, Element el)
        {
            var lEl = new List<Element>();
            string path = Util.GetGenFilePathElement(rep, el);
            if (path == "")
            {
                MessageBox.Show($@"No file defined in property for: '{el.Name}':{el.Type}");
                return lEl;
            }
            string text;
            try
            {
                text = System.IO.File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Clipboard.SetText(e.ToString());
                MessageBox.Show($@"{e}\n\nsee Clipboard!", $@"Error Reading file '{el.Name}':{el.Type}");
                return lEl;
            }
            lEl = GetInterfacesFromText(rep, null, text, createWarningNote: false);

            return lEl;
        }

        #region vCGetState

        [ServiceOperation("{597608A2-5C3F-4AE6-9B18-86C1B3C27382}", "Get and update VC state of selected package",
            "Select FullPackageElement", isTextRequired: false)]
        // ReSharper disable once InconsistentNaming
        // dynamical usage as configurable service by reflection
        // ReSharper disable once UnusedMember.Local
        static void VCGetState(Repository rep)
        {
            Package pkg = rep.GetTreeSelectedPackage();
            if (pkg != null)
            {
                if (pkg.IsControlled)
                {
                    string pkgState = Util.GetVCstate(rep, pkg, true);
                    DialogResult result =
                        MessageBox.Show($@"Package is {pkgState}\nPath={pkg.XMLPath}\nFlags={pkg.Flags}",
                            @"Update package state?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes) Util.UpdateVc(rep, pkg);
                }
            }
            else MessageBox.Show(@"No package selected");
        }

        #endregion

        #region updateVcStateOfSelectedPackageRecursiveService

        [ServiceOperation("{A521EB65-3F3C-4C5D-9B82-D12FFCEC71D4}", "Update VC-State of package(recursive)",
            "Select FullPackageElement or model", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        // dynamical usage as configurable service by reflection
        public static void UpdateVcStateOfSelectedPackageRecursiveService(Repository rep)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;


                Package pkg = rep.GetTreeSelectedPackage();
                UpdateVcStateRecursive(rep, pkg);
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
                MessageBox.Show(e11.ToString(), @"Error Insert Function");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region updateVcStateRecursive

        static void UpdateVcStateRecursive(Repository rep, Package pkg, bool recursive = true)
        {
            if (pkg.IsControlled) Util.UpdateVc(rep, pkg);
            if (recursive)
            {
                foreach (Package p in pkg.Packages)
                {
                    if (p.IsControlled) Util.UpdateVc(rep, p);
                    UpdateVcStateRecursive(rep, p);
                }
            }
        }

        #endregion

        #region getDiagramLinkForConnector

        static DiagramLink GetDiagramLinkForConnector(Diagram dia, int connectorId)
        {
            foreach (DiagramLink link in dia.DiagramLinks)
            {
                if (connectorId == link.ConnectorID) return link;
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
            isTextRequired: false)]
        public static void AddFavorite(Repository rep)
        {
            var f = new Favorite(rep, GetGuidfromSelectedItem(rep));
            f.Save();
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
            isTextRequired: false)]
        public static void RemoveFavorite(Repository rep)
        {
            var f = new Favorite(rep, GetGuidfromSelectedItem(rep));
            f.Delete();
        }

        #endregion

        #region Favorites

        /// <summary>
        /// Show Favorite:
        /// - Show Favorites in the search window
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{756710FA-A99E-40D3-B265-518DDF1014D1}", "Show Favorites",
            "Element, Package, Diagram, Attribute, Operation in EA Model Search",
            isTextRequired: false)]
        public static void Favorites(Repository rep)
        {
            var f = new Favorite(rep);
            f.Search();
        }

        #endregion

        #region getGuidfromSelectedItem

        private static string GetGuidfromSelectedItem(Repository rep)
        {
            ObjectType objectType = rep.GetContextItemType();
            string guid = "";
            switch (objectType)
            {
                case ObjectType.otAttribute:
                    var a = (EA.Attribute) rep.GetContextObject();
                    guid = a.AttributeGUID;
                    break;
                case ObjectType.otMethod:
                    var m = (Method) rep.GetContextObject();
                    guid = m.MethodGUID;
                    break;
                case ObjectType.otElement:
                    var el = (Element) rep.GetContextObject();
                    guid = el.ElementGUID;
                    break;
                case ObjectType.otDiagram:
                    var dia = (Diagram) rep.GetContextObject();
                    guid = dia.DiagramGUID;
                    break;
                case ObjectType.otPackage:
                    var pkg = (Package) rep.GetContextObject();
                    guid = pkg.PackageGUID;
                    break;
                default:
                    MessageBox.Show(@"Nothing useful selected");
                    break;
            }
            return guid;
        }

        #endregion

        #region moveEmbeddedLeftGUI

        /// <summary>
        /// Move the selected ports left
        /// - If left level is crossed it locates ports to top left corner.
        /// </summary>
        /// <param name="rep"></param>
        [ServiceOperation("{28188D09-7B40-4396-8FCF-90EA901CFE12}", "Embedded Elements left", "Select embedded elements",
            isTextRequired: false)]
        public static void MoveEmbeddedLeftGui(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (DiagramObject) dia.SelectedObjects.GetAt(0);
            Element port = rep.GetElementByID(objPort0.ElementID);
            if (!port.IsEmbeddedElement()) return;

            // get parent of embedded element
            Element el = rep.GetElementByID(port.ParentID);

            DiagramObject obj = Util.GetDiagramObjectById(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");


            // check if left limit element is crossed
            int leftLimit = obj.left - 0; // limit cross over left 
            bool isRightLimitCrossed = false;
            foreach (DiagramObject objPort in dia.SelectedObjects)
            {
                if (objPort.left < leftLimit)
                {
                    isRightLimitCrossed = true;
                    break;
                }
            }
            // move all to left upper corner of element
            int startValueTop = obj.top - 8;
            int startValueLeft = obj.left - 8;
            int pos = 0;
            foreach (DiagramObject objPort in dia.SelectedObjects)
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
        [ServiceOperation("{91998805-D1E6-4A3E-B9AA-8218B1C9F4AB}", "Embedded Elements right",
            "Select embedded elements", isTextRequired: false)]
        public static void MoveEmbeddedRightGui(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (DiagramObject) dia.SelectedObjects.GetAt(0);
            Element port = rep.GetElementByID(objPort0.ElementID);
            if (!port.IsEmbeddedElement()) return;

            // get parent of embedded element
            Element el = rep.GetElementByID(port.ParentID);

            DiagramObject obj = Util.GetDiagramObjectById(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");


            // check if right limit element is crossed
            int rightLimit = obj.right - 16; // limit cross over right 
            bool isRightLimitCrossed = false;
            foreach (DiagramObject objPort in dia.SelectedObjects)
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
            foreach (DiagramObject objPort in dia.SelectedObjects)
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
                    objPort.left = startValueLeft;
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
        [ServiceOperation("{1F5BA798-F9AC-4F80-8004-A8E8236AF629}", "Embedded Elements down", "Select embedded elements",
            isTextRequired: false)]
        public static void MoveEmbeddedDownGui(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (DiagramObject) dia.SelectedObjects.GetAt(0);
            Element port = rep.GetElementByID(objPort0.ElementID);
            if (!port.IsEmbeddedElement()) return;

            // get parent of embedded element
            Element el = rep.GetElementByID(port.ParentID);

            DiagramObject obj = Util.GetDiagramObjectById(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");

            // check if lower limit element is crossed
            int lowerLimit = obj.bottom + 12; // limit cross over upper 
            bool isLowerLimitCrossed = false;
            foreach (DiagramObject objPort in dia.SelectedObjects)
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
            foreach (DiagramObject objPort in dia.SelectedObjects)
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
        [ServiceOperation("{26F5F957-4CFD-4684-9417-305A1615460A}", "Embedded Elements up", "Select embedded elements",
            isTextRequired: false)]
        public static void MoveEmbeddedUpGui(Repository rep)
        {
            Diagram dia = rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            rep.SaveDiagram(dia.DiagramID);

            // check if port,..
            var objPort0 = (DiagramObject) dia.SelectedObjects.GetAt(0);
            Element port = rep.GetElementByID(objPort0.ElementID);
            if (!port.IsEmbeddedElement()) return;

            // get parent of embedded element
            Element el = rep.GetElementByID(port.ParentID);

            DiagramObject obj = Util.GetDiagramObjectById(rep, dia, el.ElementID);
            //EA.DiagramObject obj = dia.GetDiagramObjectByID(el.ElementID, "");


            // check if upper limit element is crossed
            int upLimit = obj.top - 10; // limit cross over upper 
            bool isUpperLimitCrossed = false;
            foreach (DiagramObject objPort in dia.SelectedObjects)
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
            foreach (DiagramObject objPort in dia.SelectedObjects)
            {
                if (!isUpperLimitCrossed)
                {
                    // move to top
                    objPort.top = objPort.top + 10;
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

        /// <summary>
        /// Move Feature (Attribute/Operation) up. EA automatic ordering has to be disabled in the configuration
        /// </summary>
        /// [ServiceOperation("{7DEB5894-1B07-4743-B97E-95C71FDC7614}", "Move Feature (Attribute/Operation) up", "Select Feature", isTextRequired: false)]
        public static void FeatureUp(EA.Repository rep)
        {
            switch (rep.GetContextItemType())
            {
                case EA.ObjectType.otAttribute:
                    EA.Attribute findAttribute = (EA.Attribute)rep.GetContextObject();
                    EA.Element el = rep.GetElementByID(findAttribute.ParentID);
                    int lfdNr = 1;
                    EA.Attribute lastAttribute = null;
                    foreach (EA.Attribute a in el.Attributes)
                    {
                        a.Pos = lfdNr;
                        a.Update();
                        if (a.AttributeID == findAttribute.AttributeID)
                        {
                            if (lastAttribute != null)
                            {
                                lastAttribute.Pos = lfdNr + 1;
                                lastAttribute.Update();
                            }
                        }
                        lastAttribute = a;
                        lfdNr += 1;
                    }
                    el.Attributes.Refresh();
                    el.Update();
                    rep.ShowInProjectView(findAttribute);
                    break;
                // handle methods
                case EA.ObjectType.otMethod:
                    EA.Method findMethod = (EA.Method)rep.GetContextObject();
                    EA.Element elMethods = rep.GetElementByID(findMethod.ParentID);
                    int lfdNrMethod = 1;
                    EA.Method lastMethod = null;
                    foreach (EA.Method m in elMethods.Methods)
                    {
                        m.Pos = lfdNrMethod;
                        m.Update();
                        if (m.MethodID == findMethod.MethodID)
                        {
                            if (lastMethod != null)
                            {
                                lastMethod.Pos = lfdNrMethod + 1;
                                lastMethod.Update();
                            }
                        }
                        lastMethod = m;
                        lfdNrMethod += 1;
                    }
                    elMethods.Methods.Refresh();
                    elMethods.Update();
                    rep.ShowInProjectView(findMethod);
                    break;
            }
        }

        /// <summary>
        /// Move feature (Attribute/Operation) down. EA automatic ordering has to be disabled in the configuration
        /// </summary>
        /// [ServiceOperation("{8A54BF0E-F901-4D74-A8C4-D66B5A2508AE}", "Move Attribute down", "Select attribute", isTextRequired: false)]
        public static void FeatureDown(EA.Repository rep)
        {
            switch (rep.GetContextItemType())
            {
                case EA.ObjectType.otAttribute:
                    EA.Attribute findAttribute = (EA.Attribute)rep.GetContextObject();
                    EA.Element el = rep.GetElementByID(findAttribute.ParentID);
                    int indexFind = -1;
                    int lfdNr = 1;
                    foreach (EA.Attribute a in el.Attributes)
                    {
                        if (a.AttributeID == findAttribute.AttributeID)
                        {
                            a.Pos = lfdNr + 1;
                            indexFind = lfdNr + 1;
                        }
                        else
                        {
                            if (indexFind == lfdNr) a.Pos = lfdNr - 1;
                            else a.Pos = lfdNr;
                        }
                        a.Update();
                        lfdNr += 1;
                    }
                    el.Attributes.Refresh();
                    el.Update();
                    rep.ShowInProjectView(findAttribute);
                    break;
                // Method
                case EA.ObjectType.otMethod:
                    EA.Method findMethod = (EA.Method)rep.GetContextObject();
                    EA.Element elMethod = rep.GetElementByID(findMethod.ParentID);
                    int indexFindMethod = -1;
                    int lfdNrMethod = 1;
                    foreach (EA.Method m in elMethod.Methods)
                    {
                        if (m.MethodID == findMethod.MethodID)
                        {
                            m.Pos = lfdNrMethod + 1;
                            indexFindMethod = lfdNrMethod + 1;
                        }
                        else
                        {
                            if (indexFindMethod == lfdNrMethod) m.Pos = lfdNrMethod - 1;
                            else m.Pos = lfdNrMethod;
                        }
                        m.Update();
                        lfdNrMethod += 1;
                    }
                    elMethod.Methods.Refresh();
                    elMethod.Update();
                    rep.ShowInProjectView(findMethod);
                    break;
            }
        }

        [ServiceOperation("{10845456-3A25-4357-9F54-C8010E5AC9E1}", "Search for + Copy to Clipboard selected item name",
            "Select Package, Element, Attribute, Operation. Run Quick Search with Item name.", isTextRequired: false)]

        // ReSharper disable once UnusedMember.Global
        public static void SearchForName(EA.Repository rep)
        {
            string name = GetNameFromContextItem(rep);
            // run search if name found
            if (name != "")
            {
                Clipboard.SetText(name);
                // run search
                rep.RunModelSearch("Quick Search", name, "", "");
            }

        }

        [ServiceOperation("{0B719197-81AF-4B7A-9E2E-2E2B96C101DB}", "Copy name of selected item to Clipboard",
            "Select Package, Element, Attribute, Operation.", isTextRequired: false)]
        // ReSharper disable once UnusedMember.Global
        public static void CopyContextNameToClipboard(EA.Repository rep)
        {
            string txt = GetNameFromContextItem(rep);
            Clipboard.SetText(txt);
        }

        /// <summary>
        /// Get name from context element
        /// If Action try to extract the operation name.
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        private static string GetNameFromContextItem(EA.Repository rep)
        {
            string name = "";
            switch (rep.GetContextItemType())
            {
                case ObjectType.otElement:
                    EA.Element el = (EA.Element)rep.GetContextObject();
                    name = el.Name;
                    // possible Action which contains a function
                    if (el.Type == "Action" && el.Name.EndsWith(")") && el.Name.Contains("("))
                    {
                        // xxxxx( , , ) // extract function name
                        Regex rx = new Regex(@"\s*(\w*)\s*\([^\)]*\)");
                        Match match = rx.Match(name);
                        if (match.Groups.Count == 2)
                        {
                            name = match.Groups[1].Value;
                        }
                    }
                    break;
                case ObjectType.otDiagram:
                    name = ((EA.Diagram)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otPackage:
                    name = ((EA.Package)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otAttribute:
                    name = ((EA.Attribute)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otMethod:
                    name = ((EA.Method)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otParameter:
                    name = ((EA.Parameter)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otDatatype:
                    name = ((EA.Datatype)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otConnector:
                    name = ((EA.Connector)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otIssue:
                    name = ((EA.Issue)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otTest:
                    name = ((EA.Test)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otTask:
                    name = ((EA.Task)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otScenario:
                    name = ((EA.Scenario)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otClient:
                    name = ((EA.Client)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otAuthor:
                    name = ((EA.Author)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otProjectResource:
                    name = ((EA.ProjectResource)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otRequirement:
                    name = ((EA.Requirement)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otRisk:
                    name = ((EA.Risk)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otEffort:
                    name = ((EA.Effort)rep.GetContextObject()).Name;
                    break;
                case ObjectType.otMetric:
                    name = ((EA.Metric)rep.GetContextObject()).Name;
                    break;
            }
            return name;
        }

        #region About

        /// <summary>
        /// Outputs the About window
        /// </summary>
        /// <param name="release"></param>
        /// <param name="configFilePath"></param>
        public static void About(string release, string configFilePath)
        {
            string installDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EaService)).CodeBase);
            string s1 = @"!!!Make live with EA easier and more efficient!!!

Helmut.Ortmann@t-online.de
Helmut.Ortmann@hoModeler.de
(+49) 172 / 51 79 16 7
Workshops, Training Coaching, Project Work
- Processes (SPICE / Functional Safety)
- Requirements
- Enterprise Architect
 -- UML / SysML, also integration Modelica
 -- Method- development and -implementation
 -- Addin
 -- Query & Script
 !!!Make live with EA easier and more efficient!!!
";


            string s2 =
                $"\nInstall:\t {installDir}\nConfig:\t {configFilePath}\n\n\nhoTools  {release} (AddinClass.dll AssemblyFileVersion)";


            MessageBox.Show(s1 + s2, @"hoTools");
        }

        #endregion

        public static void AboutVar1(string release, string configFilePath)
        {
            string installDir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EaService)).CodeBase);
            string s1 = @"!!!Make live with EA easier and more efficient!!!

Helmut.Ortmann@t-online.de
Helmut.Ortmann@hoModeler.de
(+49) 172 / 51 79 16 7
Workshops, Training Coaching, Project Work
- Processes (SPICE / Functional Safety)
- Requirements
- Enterprise Architect
 -- UML / SysML with Modelica integration
 -- Method- development and -implementation
 -- Addin
 -- Query & Script
 !!!Make live with EA easier and more efficient!!!
";

            string s2 =
                $"Install:\t {installDir}\nConfig:\t {configFilePath}\nhoTools  {release} (AddinClass.dll AssemblyFileVersion)";
            MessageBox.Show(s1 + s2, @"hoTools");
        }

        #endregion



    }
}