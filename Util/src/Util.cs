using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using EA;
using hoTools.Utils.Extension;
using hoTools.Utils.svnUtil;
using Attribute = EA.Attribute;
using File = System.IO.File;

namespace hoTools.Utils
{
    public static class Util
    {

        #region Start File

        /// <summary>
        /// Start file
        /// </summary>
        /// <param name="filePath"></param>
        public static void StartFile(string filePath)
        {
            try
            {
                // start file with the program defined in Windows for this file
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"{ex.Message}:{Environment.NewLine}'{filePath}'", $@"Can't open file {Path.GetFileName(filePath)}");
            }
        }

        #endregion

        public static string ObjectTypeToString(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.otPackage:
                    return "Package";
                case ObjectType.otElement:
                    return "Element";
                case ObjectType.otDiagram:
                    return "Diagram";
                case ObjectType.otMethod:
                    return "Operation";
                case ObjectType.otAttribute:
                    return "Attribute";
                default:
                    return "unknown object type";
            }
        }


        /// <summary>
        /// Get element from Context element. Possible inputs are: Attribute, Operation, Element, Package
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static EA.Element GetElementFromContextObject(Repository rep)
        {
            EA.Element el = null;
            ObjectType objectType = rep.GetContextItemType();
            switch (objectType)
            {
                case ObjectType.otAttribute:
                    var a = (Attribute) rep.GetContextObject();
                    el = rep.GetElementByID(a.ParentID);
                    break;
                case ObjectType.otMethod:
                    var m = (Method) rep.GetContextObject();
                    el = rep.GetElementByID(m.ParentID);
                    break;
                case ObjectType.otElement:
                    el = (EA.Element) rep.GetContextObject();
                    break;
                case ObjectType.otPackage:
                    EA.Package pkg = (EA.Package)rep.GetContextObject();
                    el = rep.GetElementByGuid(pkg.PackageGUID);
                    break;
                case ObjectType.otNone:
                    EA.Diagram dia = rep.GetCurrentDiagram();
                    if (dia?.SelectedObjects.Count == 1)
                    {
                        var objSelected = (EA.DiagramObject) dia.SelectedObjects.GetAt(0);
                        el = rep.GetElementByID(objSelected.ElementID);
                    }
                    break;
                default:
                    MessageBox.Show(@"No Element, Attribute, Operation, Package selected");
                    break;
            }
            return el;
        }
        /// <summary>
        /// Open Directory of a directory or a file with Explorer or Totalcommander.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isTotalCommander"></param>
        public static void ShowFolder(string path, bool isTotalCommander=false)
        {
            path = Path.GetDirectoryName(path);

            if (isTotalCommander)
                StartApp(@"totalcmd.exe", "/o " + path);
            else
                StartApp(@"Explorer.exe", "/e, " + path);
        }

        
        /// <summary>
        /// Start Application with parameters
        /// </summary>
        /// <param name="app"></param>
        /// <param name="par"></param>
        public static void StartApp(string app, string par)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = app,
                    Arguments = par
                }
            };
            try
            {
                p.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(p.StartInfo.FileName + @" " +
                                p.StartInfo.Arguments + @"\n\n" +
                                @"Have you set the %path% environment variable?\n\n" + e,
                    @"Can't show controlled package");
            }
        }

        private static string GetWildCard(Repository rep)
        {
            string cnString = rep.ConnectionString.ToUpper();

            if (cnString.EndsWith(".EAP", StringComparison.CurrentCulture))
            {
                var f = new FileInfo(cnString);
                if (f.Length > 20000) return "*";
                TextReader tr = new StreamReader(cnString);
                string shortcut = tr.ReadLine().ToUpper();
                tr.Close();
                if (shortcut.Contains(".EAP")) return "*";
                if (shortcut.Contains("DBTYPE=")) return "%";
                return "";

            }
            return "%";
        }

        public static void SetSequenceNumber(Repository rep, EA.Diagram dia,
            EA.DiagramObject obj, string sequence)
        {
            if (obj != null)
            {

                string updateStr = @"update t_DiagramObjects set sequence = " + sequence +
                                   " where diagram_id = " + dia.DiagramID +
                                   " AND instance_id = " + obj.InstanceID;

                rep.Execute(updateStr);
            }
        }

        public static void AddSequenceNumber(Repository rep, EA.Diagram dia)
        {

            string updateStr = @"update t_DiagramObjects set sequence = sequence + 1 " +
                               " where diagram_id = " + dia.DiagramID;

            rep.Execute(updateStr);
        }

        public static int GetHighestSequenceNoFromDiagram(Repository rep, EA.Diagram dia)
        {
            int sequenceNumber = 0;
            string query = @"select sequence from t_diagramobjects do " +
                           "  where do.Diagram_ID = " + dia.DiagramID +
                           "  order by 1 desc";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//SEQUENCE_NUMBER");
            if (operationGuidNode != null)
            {
                sequenceNumber = Convert.ToInt32(operationGuidNode.InnerText);
            }
            return sequenceNumber;
        }

        // replace of API-function getDiagramObjectByID which isn't available in EA 9.
        public static EA.DiagramObject GetDiagramObjectById(Repository rep, EA.Diagram dia, int elementId)
        {
            if (rep.LibraryVersion > 999)
            {
                return dia.GetDiagramObjectByID(elementId, "");
                //return null;
            }
            foreach (EA.DiagramObject obj in dia.DiagramObjects)
            {
                if (obj.ElementID == elementId)
                {
                    return obj;
                }
            }
            return null;
        }


        //--------------------------------------------------------------------------------------------------------------
        // setLineStyleForLink  Set line style for a digram link
        //--------------------------------------------------------------------------------------------------------------
        // linestyle
        // LH = "Line Style: Lateral Horizontal";
        // LV = "Line Style: Lateral Vertical";
        // TH  = "Line Style: Tree Horizontal";
        // TV = "Line Style: Tree Vertical";
        // OS = "Line Style: Orthogonal Square";
        // OR =              Orthogonal Round
        // A =               Automatic
        // D =               Direct
        // C =               Customer
        // B =               Bezier
        // NO=               make nothing
        public static void SetLineStyleForDiagramLink(string lineStyle, DiagramLink link)
        {
#pragma warning disable RECS0012
            lineStyle = lineStyle + "  ";
            if (lineStyle.Substring(0, 2).ToUpper() == "NO") return;
            if (lineStyle.Substring(0, 2) == "TH") lineStyle = "H ";
            if (lineStyle.Substring(0, 2) == "TV") lineStyle = "V ";
            if (lineStyle.Substring(0, 1) == "D") link.Style = "Mode=1;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;";
            else if (lineStyle.Substring(0, 1) == "A") link.Style = "Mode=2;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;";
            else if (lineStyle.Substring(0, 1) == "C") link.Style = "Mode=3;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;";
            else if (lineStyle.Substring(0, 1) == "B") link.Style = "Mode=8;EOID=61B36ED5;SOID=08967F1E;Color=-1;LWidth=0;";
            else
            {
                link.Style = "Mode=3;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;TREE=" +
                             lineStyle.Trim() + ";";

            }
            link.Update();
#pragma warning restore RECS0012
        }


        //--------------------------------------------------------------------------------------------------------------
        // SetLineStyleDiagramObjectsAndConnectors  Set line style for selected connector and all connectors of selected diagram objects
        //--------------------------------------------------------------------------------------------------------------
        // linestyle
        // LH = "Line Style: Lateral Horizontal";
        // LV = "Line Style: Lateral Vertical";
        // TH  = "Line Style: Tree Horizontal";
        // TV = "Line Style: Tree Vertical";
        // OS = "Line Style: Orthogonal Square";
        // OR =              Orthogonal Round
        // A =               Automatic
        // D =               Direct
        // C =               Customer 
        // B =               Bezier
        // R =               Reverse direction of connector
        public static void SetLineStyleDiagramObjectsAndConnectors(Repository rep, EA.Diagram d, string lineStyle)
        {
            Collection selectedObjects = d.SelectedObjects;
            Connector selectedConnector = d.SelectedConnector;
            // store current diagram
            rep.SaveDiagram(d.DiagramID);
            foreach (DiagramLink link in d.DiagramLinks)
            {
                if (link.IsHidden == false)
                {

                    // check if connector is connected with diagram object
                    Connector c = rep.GetConnectorByID(link.ConnectorID);
                    foreach (EA.DiagramObject dObject in d.SelectedObjects)
                    {
                        if (c.ClientID == dObject.ElementID | c.SupplierID == dObject.ElementID)
                        {
                            // Line style or direction of connector
                            if (lineStyle.Substring(0, 1) == "R")
                                ReverseConnectorDirection(rep, rep.GetConnectorByID(link.ConnectorID));
                            else
                                SetLineStyleForDiagramLink(lineStyle, link);
                        }
                    }
                    if (c.ConnectorID == selectedConnector?.ConnectorID)
                    {
                        if (lineStyle.Substring(0, 1) == "R")
                            ReverseConnectorDirection(rep, rep.GetConnectorByID(link.ConnectorID));
                        else
                            SetLineStyleForDiagramLink(lineStyle, link);
                    }
                }
            }
            rep.ReloadDiagram(d.DiagramID);
            if (selectedConnector != null) d.SelectedConnector = selectedConnector;
            foreach (EA.DiagramObject dObject in selectedObjects)
            {
                //d.SelectedObjects.AddNew(el.ElementID.ToString(), el.Type);
                d.SelectedObjects.AddNew(dObject.ElementID.ToString(), dObject.ObjectType.ToString());
            }
            //d.Update();
            d.SelectedObjects.Refresh();
        }

        //--------------------------------------------------------------------------------------------------------------
        // SetLineStyleDiagram  Set line style for a diagram (all visible connectors)
        //--------------------------------------------------------------------------------------------------------------
        // linestyle
        // LH = "Line Style: Lateral Horizontal";
        // LV = "Line Style: Lateral Vertical";
        // TH  = "Line Style: Tree Horizontal";
        // TV = "Line Style: Tree Vertical";
        // OS = "Line Style: Orthogonal Square";
        // OR =              Orthogonal Round
        // A =               Automatic
        // D =               Direct
        // C =               Customer  
        // R =  Reverse direction of connector     


        public static void SetLineStyleDiagram(Repository rep, EA.Diagram d, string lineStyle)
        {
            // store current diagram
            rep.SaveDiagram(d.DiagramID);
            // all links
            foreach (DiagramLink link in d.DiagramLinks)
            {
                if (link.IsHidden == false)
                {
                    // Line style or direction of connector
                    if (lineStyle.Substring(0, 1) == "R")
                        ReverseConnectorDirection(rep, rep.GetConnectorByID(link.ConnectorID));
                    else
                        SetLineStyleForDiagramLink(lineStyle, link);
                }

            }
            rep.ReloadDiagram(d.DiagramID);
        }


        private static void ChangeClassNameToSynonyms(Repository rep, EA.Element el)
        {
            if (el.Type.Equals("Class"))
            {
                // check if property 'Syynonym' exists
                foreach (EA.TaggedValue tag in el.TaggedValues)
                {
                    if (tag.Name == "typeSynonyms")
                    {
                        if (tag.Value != el.Name)
                        {
                            el.Name = tag.Value;
                            el.Update();
                            break;
                        }
                    }
                }
                foreach (EA.Element elNested in el.Elements)
                {
                    ChangeClassNameToSynonyms(rep, elNested);
                }

            }
        }

        public static void ChangePackageClassNameToSynonyms(Repository rep, EA.Package pkg)
        {
            // All elements in package
            foreach (EA.Element el in pkg.Elements)
            {
                if (el.Type.Equals("Class"))
                {
                    // class nested
                    ChangeClassNameToSynonyms(rep, el);
                }
            }
            // all packages in packages
            foreach (EA.Package pkgNested in pkg.Packages)
            {
                // package nested
                ChangePackageClassNameToSynonyms(rep, pkgNested);
            }
        }


        public static bool UpdateClass(Repository rep, EA.Element el)
        {

            foreach (Attribute a in el.Attributes)
            {
                UpdateAttribute(rep, a);
            }
            foreach (Method m in el.Methods)
            {
                UpdateMethod(rep, m);
            }

            // over all nested classes
            foreach (EA.Element e in el.Elements)
            {
                UpdateClass(rep, e);
            }
            return true;
        }

        public static bool UpdatePackage(Repository rep, EA.Package pkg)
        {
            foreach (EA.Element el in pkg.Elements)
            {
                UpdateClass(rep, el);
            }
            foreach (EA.Package pkg1 in pkg.Packages)
            {
                UpdatePackage(rep, pkg1);
            }

            return true;
        }



        public static bool UpdateAttribute(Repository rep, Attribute a)
        {
            // no classifier defined
            if (a.ClassifierID == 0)
            {
                // find type from type_name
                int id = GetTypeId(rep, a.Type);
                if (id > 0)
                {
                    a.ClassifierID = id;
                    bool error = a.Update();
                    if (!error)
                    {
                        MessageBox.Show(@"Error write Attribute", a.GetLastError());
                        return false;
                    }
                }
            }
            return true;
        }

        // Update Method Types
        public static bool UpdateMethod(Repository rep, Method m)
        {

            int id;

            // over all parameters
            foreach (EA.Parameter par in m.Parameters)
            {
                if ((par.ClassifierID == "") || (par.ClassifierID == "0"))
                {
                    // find type from type_name
                    id = GetTypeId(rep, par.Type);
                    if (id > 0)
                    {
                        par.ClassifierID = id.ToString();
                        bool error = par.Update();
                        if (!error)
                        {
                            MessageBox.Show(@"Error write Parameter", m.GetLastError());
                            return false;

                        }
                    }


                }

            }
            // no classifier defined
            if ((m.ClassifierID == "") || (m.ClassifierID == "0"))
            {
                // find type from type_name
                id = GetTypeId(rep, m.ReturnType);
                if (id > 0)
                {
                    m.ClassifierID = id.ToString();
                    bool error = m.Update();
                    if (!error)
                    {
                        MessageBox.Show(@"Error write Method", m.GetLastError());
                        return false;
                    }
                }
            }
            return true;
        }



        // Find type for name
        // 1. Search for name (if type contains a '*' search for type with '*' and for type without '*'
        // 2. Search for Synonyms
        public static int GetTypeId(Repository rep, string name)
        {
            int intReturn = 0;
            //Boolean isPointer = false;
            //if (name.Contains("*")) isPointer = true;
            //
            // delete an '*' at the end of the type name

            // remove a 'const ' from start of string
            // remove a 'volatile ' from start of string
            name = name.Replace("const", "");
            name = name.Replace("volatile", "");
            //name = name.Replace("*", "");
            name = name.Trim();

//            if (isPointer) {
//                string queryIsPointer = @"SELECT o.object_id As OBJECT_ID
//                            FROM  t_object  o
//                            INNER  JOIN  t_objectproperties  p ON  o.object_id  =  p.object_id
//                            where property = 'typeSynonyms' AND
//                                  Object_Type in ('Class','PrimitiveType','DataType','Enumeration')  AND
//                                  p.value = '" + name + "*' " +
//                            @" UNION
//                               Select o.object_id
//                               From t_object o
//                                        where Object_Type in ('Class','PrimitiveType','DataType','Enumeration') AND name = '" + name + "*' ";
//                string strIsPointer = rep.SQLQuery(queryIsPointer);
//                XmlDocument XmlDocIsPointer = new XmlDocument();
//                XmlDocIsPointer.LoadXml(strIsPointer);

//                XmlNode operationGUIDNodeIsPointer = XmlDocIsPointer.SelectSingleNode("//OBJECT_ID");
//                if (operationGUIDNodeIsPointer != null)
//                {
//                    intReturn = Convert.ToInt32(operationGUIDNodeIsPointer.InnerText);
//                }     
//            }

            if (intReturn == 0)
            {
                //if (name.Equals("void") || name.Equals("void*")) return 0;
                string query = @"SELECT o.object_id As OBJECT_ID
                            FROM  t_object  o
                            INNER  JOIN  t_objectproperties  p ON  o.object_id  =  p.object_id
                            where property = 'typeSynonyms' AND
                                  Object_Type in ('Class','PrimitiveType','DataType','Enumeration')  AND
                                  p.value = '" + name + "' " +
                               @" UNION
                               Select o.object_id
                               From t_object o
                                        where Object_Type in ('Class','PrimitiveType','DataType','Enumeration') AND name = '" +
                               name + "' ";
                string str = rep.SQLQuery(query);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(str);

                XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//OBJECT_ID");
                if (operationGuidNode != null)
                {
                    intReturn = Convert.ToInt32(operationGuidNode.InnerText);
                }
            }


            return intReturn;
        }

        public static int GetTypeFromName(Repository rep, ref string name, ref string type)
        {
            var id = GetTypeId(rep, type);
            if (id == 0 & type.Contains("*"))
            {
                type = type.Remove(type.IndexOf("*", StringComparison.CurrentCulture), 1);
                name = "*" + name;
                id = GetTypeId(rep, type);
                if (id == 0 & type.Contains("*"))
                {
                    type = type.Replace("*", "");
                    name = "*" + name;
                    id = GetTypeId(rep, type);
                }
            }


            return id;

        }

        //------------------------------------------------------------------------------------------------------------------------------------
        // Find the Parameter of a Activity
        //------------------------------------------------------------------------------------------------------------------------------------
        // par Parameter of Operation (only if isReturn = false)
        // act Activity
        // Parameter wird aufgrund des Alias-Namens gefunden
        //
        // 
        public static EA.Element GetParameterFromActivity(Repository rep, EA.Parameter par, EA.Element act,
            bool isReturn = false)
        {

            string aliasName;
            if (isReturn)
            {
                aliasName = "return:";
            }
            else
            {
                aliasName = "par_" + par.Position;
            }

            EA.Element parTrgt = null;
            string query = @"select o2.ea_guid AS CLASSIFIER_GUID
                      from t_object o1 INNER JOIN t_object o2 on ( o2.parentID = o1.object_id)
                      where o1.Object_ID = " + act.ElementID +
                           " AND  o2.Alias like '" + aliasName + GetWildCard(rep) + "'";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//CLASSIFIER_GUID");
            if (operationGuidNode != null)
            {
                string guid = operationGuidNode.InnerText;
                parTrgt = rep.GetElementByGuid(guid);
            }
            return parTrgt;
        }

        // Find the calling operation from a Call Operation Action
        public static Method GetOperationFromAction(Repository rep, EA.Element action)
        {
            Method method = null;
            string query = @"select o.Classifier_guid AS CLASSIFIER_GUID
                      from t_object o 
                      where o.Object_ID = " + action.ElementID;
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//CLASSIFIER_GUID");
            if (operationGuidNode != null)
            {
                string guid = operationGuidNode.InnerText;
                method = rep.GetMethodByGuid(guid);
            }
            return method;
        }

        // Find the calling operation from a Call Operation Action
        public static string GetParameterType(Repository rep, string actionPinGuid)
        {
            string query = @"SELECT par.type AS OPTYPE 
			    from t_object o  inner join t_operationparams par on (o.classifier_guid = par.ea_guid)
                where o.ea_guid = '" + actionPinGuid + "' ";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode typeGuidNode = xmlDoc.SelectSingleNode("//OPTYPE");
            if (typeGuidNode != null)
            {
                return typeGuidNode.InnerText;

            }
            return "";
        }


        // Find the calling operation from a Call Operation Action
        public static Method GetOperationFromCallAction(Repository rep, EA.Element obj)
        {
            string wildCard = GetWildCard(rep);
            string query = @"SELECT op.ea_guid AS OPERATION from (t_object o inner join t_operation op on (o.classifier_guid = op.ea_guid))
               inner join t_xref x on (x.client = o.ea_guid)
			   where x.name = 'CustomProperties' and
			             x.description like '" + wildCard + "CallOperation" + wildCard +
                           "' and o.object_id = " + obj.ElementID;
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//OPERATION");
            if (operationGuidNode != null)
            {
                var guid = operationGuidNode.InnerText;
                return rep.GetMethodByGuid(guid);
            }
            return null;
        }

        // Find the calling operation from a Call Operation Action
        public static string GetClassifierGuid(Repository rep, string guid)
        {
            string query = @"select o.Classifier_guid AS CLASSIFIER_GUID
                      from t_object o 
                      where o.EA_GUID = '" + guid + "'";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//CLASSIFIER_GUID");
            guid = "";
            if (operationGuidNode != null)
            {
                guid = operationGuidNode.InnerText;
            }
            return guid;
        }


        // Gets the trigger associated with the connector / element
        public static string GetTrigger(Repository rep, string guid)
        {
            string query = @"select x.Description AS TRIGGER_GUID
                      from t_xref x 
                      where x.Client = '" + guid + "'    AND behavior = 'trigger' ";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//TRIGGER_GUID");
            guid = "";
            if (operationGuidNode != null)
            {
                guid = operationGuidNode.InnerText;
            }
            return guid;
        }

        // Gets the signal associated with the element
        public static string GetSignal(Repository rep, string guid)
        {
            string query = @"select x.Description AS SIGNAL_GUID
                      from t_xref x 
                      where x.Client = '" + guid + "'    AND behavior = 'event' ";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//SIGNAL_GUID");
            guid = "";
            if (operationGuidNode != null)
            {
                guid = operationGuidNode.InnerText;
            }
            return guid;
        }

        // Gets the composite element for a diagram GUID
        public static string GetElementFromCompositeDiagram(Repository rep, string diagramGuid)
        {
            string query = @"select o.ea_guid AS COMPOSITE_GUID
                      from t_xref x INNER JOIN t_object o on (x.client = o.ea_guid and type = 'element property')
                      where x.supplier = '" + diagramGuid + "'    ";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//COMPOSITE_GUID");
            diagramGuid = "";
            if (operationGuidNode != null)
            {
                diagramGuid = operationGuidNode.InnerText;
            }
            return diagramGuid;
        }

        // set "ShowBeh=1; in operation field StyleEx

        public static bool SetShowBehaviorInDiagram(Repository rep, Method m)
        {
            string updateStr = @"update t_operation set StyleEx = 'ShowBeh=1;'" +
                               " where operationID = " + m.MethodID;
            rep.Execute(updateStr);
            return true;
        }

        public static bool SetFrameLinksToDiagram(Repository rep, EA.Element frm, EA.Diagram dia)
        {
            string updateStr = @"update t_object set pdata1 = " + dia.DiagramID +
                               " where object_ID = " + frm.ElementID;
            rep.Execute(updateStr);
            return true;
        }

        public static bool SetActivityCompositeDiagram(Repository rep, EA.Element el, string s)
        {
            string updateStr = @"update t_object set pdata1 = '" + s + "', ntype = 8 " +
                               " where object_ID = " + el.ElementID;
            rep.Execute(updateStr);
            return true;
        }

        public static bool SetElementPdata1(Repository rep, EA.Element el, string s)
        {
            string updateStr = @"update t_object set pdata1 = '" + s + "' " +
                               " where object_ID = " + el.ElementID;
            rep.Execute(updateStr);
            return true;
        }
        public static bool SetElementPdata3(Repository rep, EA.Element el, string s)
        {
            string updateStr = @"update t_object set pdata3 = '" + s + "' " +
                               " where object_ID = " + el.ElementID;
            rep.Execute(updateStr);
            return true;
        }

        public static bool SetConnectorGuard(Repository rep, int connectorId, string connectorGuard)
        {

            string updateStr = @"update t_connector set pdata2 = '" + connectorGuard + "' " +
                               " where Connector_Id = " + connectorId;
            rep.Execute(updateStr);


            return true;
        }
        /// <summary>
        /// PDATA1:
        /// - "Diagram Note"
        /// - "
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        public static bool SetDiagramHasAttchaedLink(Repository rep, EA.Element el)
        {
            SetElementPdata1(rep, el, "Diagram Note");
            return true;
        }

        private static bool SetVcFlags(Repository rep, EA.Package pkg, string flags)
        {
            string updateStr = @"update t_package set packageflags = '" + flags + "' " +
                               " where package_ID = " + pkg.PackageID;
            rep.Execute(updateStr);
            return true;
        }
        
        public static bool SetElementHasAttachedConnectorLink(Repository rep, Connector con, EA.Element elNote, string connectorLinkType= "Link Notes")
        {

            return SetElementLink(rep, elNote.ElementID, connectorLinkType, 0, "",  $@"idref1={con.ConnectorID}",1);
        }
        /// <summary>
        /// Attach a Model element to another Model element
        /// pdata1= 'Element Note'  (what to attach to)
        ///         'Link Notes'    (link to connector)
        /// pdata2= ElementID to attach to if element
        /// pdata4= 'Yes' if link to element
        ///         idref1=ID connector if link to connector
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="elNote"></param>
        /// <returns></returns>
        public static bool SetElementHasAttachedElementLink(Repository rep, EA.Element el, EA.Element elNote)
        {
            return SetElementLink(rep, elNote.ElementID, "Element Note", el.ElementID, "", "Yes", 0);
        }
        /// Set Element Link to:
        /// - Object
        /// - Diagram
        /// - Connector
        /// Attach a Model element to another Model item (Element, Connector, Diagram)
        /// pdata1= Attach Note to feature of (aka connectorLinkType)
        ///         'Diagram Note' Notes of the Diagram 
        ///         'Element Note' Notes of the Element
        ///         'Link Notes'   Notes of a Connector
        ///         'Attribute'    Notes of the Attribute
        ///         'Operation'    Notes of the Operation
        /// pdata2= ElementID to attach to if object
        /// pdata3= Feature name if feature, else blank
        /// pdata4= 'Yes' if link to object
        ///         'idref1=connectorID;' connector if link to connector
        /// ntype   0 Standard
        ///         1 connect to connector according to 'idref1=connectorID;'
        public static bool SetElementLink(Repository rep, int elId, string pdata1, int pdata2, string pdata3, string pdata4, int ntype)
        {
            if (String.IsNullOrWhiteSpace(pdata1)) pdata1 = " ";
            // ID of the object (Element, Connector, Attribute, Operation,..)
            string pdata2Value = "";
            if (pdata2 > 0)
                pdata2Value = $@", pdata2 = { pdata2}";
            // Feature name
            string pdata3Value = "";
            if (pdata3 != "")
                pdata3Value = $@", pdata3 = '{pdata3}'";

            string updateStr = $@"update t_object set pdata1 = '{pdata1}' {pdata2Value} {pdata3Value}, pdata4='{pdata4}', NTYPE={ntype}" +
                               $@" where object_ID = {elId} ";
            rep.Execute(updateStr);
            return true;
        }

        public static bool SetBehaviorForOperation(Repository rep, Method op, EA.Element act)
        {

            string updateStr = @"update t_operation set behaviour = '" + act.ElementGUID + "' " +
                               " where operationID = " + op.MethodID;
            rep.Execute(updateStr);


            return true;
        }

        public static string GetDiagramObjectLabel(Repository rep, int objectId, int diagramId, int instanceId)
        {
            string attributeName = "OBJECT_STYLE";
            string query = @"select ObjectStyle AS " + attributeName +
                           @" from t_diagramobjects
                      where Object_ID = " + objectId + @" AND 
                            Diagram_ID = " + diagramId + @" AND 
                            Instance_ID = " + instanceId;

            return GetSingleSqlValue(rep, query, attributeName);
        }

        private static string GetSingleSqlValue(Repository rep, string query, string attributeName)
        {
            string s = "";
            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode node = xmlDoc.SelectSingleNode("//" + attributeName);
            if (node != null)
            {
                s = node.InnerText;
            }
            return s;
        }

        public static bool SetDiagramObjectLabel(Repository rep, int objectId, int diagramId, int instanceId,
            string s)
        {

            string updateStr = @"update t_diagramObjects set ObjectStyle = '" + s + "' " +
                               " where Object_ID = " + objectId + " AND " +
                               " Diagram_ID = " + diagramId + " AND " +
                               " Instance_ID = " + instanceId;

            rep.Execute(updateStr);


            return true;
        }

        // Find the operation from Activity / State Machine
        // it excludes operations in state machines
        public static Method GetOperationFromBrehavior(Repository rep, EA.Element el)
        {
            Method method = null;
            string query = "";
            string conString = GetConnectionString(rep); // due to shortcuts
            if (conString.Contains("DBType=3"))
            {
                // Oracle DB
                query =
                    @"select op.ea_guid AS EA_GUID
                      from t_operation op 
                      where Cast(op.Behaviour As Varchar2(38)) = '" + el.ElementGUID + "' " +
                    " AND (Type is Null or Type not in ('do','entry','exit'))";
            }
            if (conString.Contains("DBType=1"))
                // SQL Server
            {
                query =
                    @"select op.ea_guid AS EA_GUID
                        from t_operation op 
                        where Substring(op.Behaviour,1,38) = '" + el.ElementGUID + "'" +
                    " AND (Type is Null or Type not in ('do','entry','exit'))";

            }

            if (conString.Contains(".eap"))
                // SQL Server
            {
                query =
                    @"select op.ea_guid AS EA_GUID
                        from t_operation op 
                        where op.Behaviour = '" + el.ElementGUID + "'" +
                    " AND ( Type is Null or Type not in ('do','entry','exit'))";

            }
            if ((!conString.Contains("DBType=1")) && // SQL Server, DBType=0 MySQL
                (!conString.Contains("DBType=3")) && // Oracle
                (!conString.Contains(".eap"))) // Access
            {
                query =
                    @"select op.ea_guid AS EA_GUID
                        from t_operation op 
                        where op.Behaviour = '" + el.ElementGUID + "'" +
                    " AND (Type is Null or Type not in ('do','entry','exit'))";

            }

            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//EA_GUID");
            if (operationGuidNode != null)
            {
                string guid = operationGuidNode.InnerText;
                method = rep.GetMethodByGuid(guid);
            }
            return method;
        }

//        // read PDATA1
//        public static EA.Element getPDATA(EA.Repository rep, int ID)
//        {
//            EA.Element el = null;
//            string query = "";
//            query =
//                    @"select pdata1 AS PDATA1
//                      from t_object o 
//                      where Cast(op.Behaviour As Varchar2(38)) = '" + el.ElementGUID + "'";

//            if (rep.ConnectionString.Contains("DBType=3"))
//            {   // Oracle DB
//                query =
//                    @"select op.ea_guid AS EA_GUID
//                      from t_operation op 
//                      where Cast(op.Behaviour As Varchar2(38)) = '" + el.ElementGUID + "'";
//            }
//            if (rep.ConnectionString.Contains("DBType=1"))
//            // SQL Server
//            {
//                query =
//                     @"select op.ea_guid AS EA_GUID
//                        from t_operation op 
//                        where Substring(op.Behaviour,1,38) = '" + el.ElementGUID + "'";

//            }

//            if (rep.ConnectionString.Contains(".eap"))
//            // SQL Server
//            {
//                query =
//                    @"select op.ea_guid AS EA_GUID
//                        from t_operation op 
//                        where op.Behaviour = '" + el.ElementGUID + "'";

//            }
//            if ((!rep.ConnectionString.Contains("DBType=1")) &&  // SQL Server, DBType=0 MySQL
//               (!rep.ConnectionString.Contains("DBType=3")) &&  // Oracle
//               (!rep.ConnectionString.Contains(".eap")))// Access
//            {
//                query =
//                  @"select op.ea_guid AS EA_GUID
//                        from t_operation op 
//                        where op.Behaviour = '" + el.ElementGUID + "'";

//            }

//            string str = rep.SQLQuery(query);
//            XmlDocument XmlDoc = new XmlDocument();
//            XmlDoc.LoadXml(str);

//            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//EA_GUID");
//            if (operationGUIDNode != null)
//            {
//                string GUID = operationGUIDNode.InnerText;
//                method = rep.GetMethodByGuid(GUID);
//            }
//            return method;
//        }



        public static Method GetOperationFromConnector(Repository rep, Connector con)
        {
            Method method = null;
            string query = "";
            if (GetConnectionString(rep).Contains("DBType=3"))
                //pdat3: 'Activity','Sequence', (..)
            {
                // Oracle DB
                query =
                    @"select description AS EA_GUID
                      from t_xref x 
                      where Cast(x.client As Varchar2(38)) = '" + con.ConnectorGUID + "'" +
                    " AND Behavior = 'effect' ";
            }
            if (GetConnectionString(rep).Contains("DBType=1"))
            {
                // SQL Server

                query =
                    @"select description AS EA_GUID
                        from t_xref x 
                        where Substring(x.client,1,38) = " + "'" + con.ConnectorGUID + "'" +
                    " AND Behavior = 'effect' "
                    ;
            }
            if (GetConnectionString(rep).Contains(".eap"))
            {

                query =
                    @"select description AS EA_GUID
                        from t_xref x 
                        where client = " + "'" + con.ConnectorGUID + "'" +
                    " AND Behavior = 'effect' "
                    ;
            }
            if ((!GetConnectionString(rep).Contains("DBType=1")) && // SQL Server, DBType=0 MySQL
                (!GetConnectionString(rep).Contains("DBType=3")) && // Oracle
                (!GetConnectionString(rep).Contains(".eap"))) // Access
            {
                query =
                    @"select description AS EA_GUID
                        from t_xref x 
                        where client = " + "'" + con.ConnectorGUID + "'" +
                    " AND Behavior = 'effect' "
                    ;

            }


            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            //string type = "";
            //XmlNode pdat3Node = XmlDoc.SelectSingleNode("//PDAT3");
            //if (pdat3Node != null)
            //{
            //    type = pdat3Node.InnerText;

            //}
            //if ( type.EndsWith(")")) // Operation
            //{ 
            string guid = null;
            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//EA_GUID");
            if (operationGuidNode != null)
            {
                guid = operationGuidNode.InnerText;
                method = rep.GetMethodByGuid(guid);
            }
            if (method == null)
            {

                if (guid != null) OpenBehaviorForElement(rep, rep.GetElementByGuid(guid));
            }
            //}

            return method;
        }

        /// <summary>
        /// Update VC (Version Control state of a controlled package:
        /// - Returns user name of user who have checked out the package
        /// - Updates the package flags
        /// </summary>
        /// <param name="rep">Repository</param>
        /// <param name="pkg">Package to check</param>
        public static string UpdateVc(Repository rep, EA.Package pkg)
        {
            string userNameLockedPackage = "";
            if (pkg.IsVersionControlled)
            {
                // find                  VC=...;
                // replace by:           VC=currentState();
                string flags = pkg.Flags;

                // remove check out flags
                flags = Regex.Replace(flags, @"VC=[^;]*;", "");
                flags = Regex.Replace(flags, @"CheckedOutTo=[^;]*;", "");


                var svnHandle = new Svn(rep, pkg);
                userNameLockedPackage = svnHandle.GetLockingUser();
                if (userNameLockedPackage != "") flags = flags + "CheckedOutTo=" + userNameLockedPackage + ";";
                try
                {
                    SetVcFlags(rep, pkg, flags);
                    rep.ShowInProjectView(pkg);
                }
                catch (Exception e)
                {
                    string s = e.Message + " ;" + pkg.GetLastError();
                    s = s + "!";
                    MessageBox.Show(s, @"Error UpdateVC state");
                }


            }
            return userNameLockedPackage;
        }

        //------------------------------------------------------------------------------------------
        // resetVCRecursive   If package is controlled: Reset package flags field of package, work for all packages recursive 
        //------------------------------------------------------------------------------------------
        // package flags:  Recurse=0;VCCFG=unchanged;
        public static void ResetVcRecursive(Repository rep, EA.Package pkg)
        {
            ResetVc(rep, pkg);
            foreach (EA.Package p in pkg.Packages)
            {
                ResetVc(rep, p);
            }
        }

        //------------------------------------------------------------------------------------------
        // resetVC   If package is controlled: Reset package flags field of package 
        //------------------------------------------------------------------------------------------
        // package flags:  Recurse=0;VCCFG=unchanged;
        private static void ResetVc(Repository rep, EA.Package pkg)
        {
            if (pkg.IsVersionControlled)
            {
                // find                  VC=...;
                string flags = pkg.Flags;
                var pattern = new Regex(@"VCCFG=[^;]+;");
                Match regMatch = pattern.Match(flags);
                if (regMatch.Success)
                {
                    // delete old string
                    flags = @"Recurse=0;" + regMatch.Value;
                }
                else
                {
                    return;
                }
                // write flags
                try
                {
                    SetVcFlags(rep, pkg, flags);
                }
                catch (Exception e)
                {
                    string s = e.Message + " ;" + pkg.GetLastError();
                    s = s + "!";
                    MessageBox.Show(s, @"Error Reset VC");
                }


            }
            // recursive package
            //foreach (EA.Package pkg1 in pkg.Packages)
            //{
            //    updateVC(rep, pkg1);
            //}
        }

        public static string GetVCstate(Repository rep, EA.Package pkg, bool isLong)
        {
            

            try
            {
                var svnHandle = new Svn(rep, pkg);
                var s = svnHandle.GetLockingUser();
                if (s != "") s = "CheckedOutTo=" + s;
                else s = "Checked in";
                return s;
            }
            catch (Exception e)
            {
                if (isLong) return "VC State Error: " + e.Message;
                return "State Error";
            }

        }
        /// <summary>
        /// Get file path for a string. It regards the local file path definition in %APPDATA%. To do so it needs the GenType (C,C++,..)
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="genType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFilePath(Repository rep, string genType, string path)
        {
            // check if a local path is defined
            Match m = Regex.Match(path, @"%[^%]*");
            if (m.Success)
            {
                var localPathVar = m.Value.Substring(1);
                // get path for localDir
                Environment.CurrentDirectory = Environment.GetEnvironmentVariable("appdata");
                string s1 = @"Sparx Systems\EA\paths.txt";
                TextReader tr = new StreamReader(s1);
                string line = "";
                Regex pattern = new Regex(@"(type=" + genType + ";id=" + localPathVar + @").+(path=[^;]+)");
                while ((line = tr.ReadLine()) != null)
                {
                    var regMatch = pattern.Match(line);
                    if (regMatch.Success)
                    {
                        path = path.Replace("%" + localPathVar + "%", "");
                        path = regMatch.Groups[2] + @"\" + path;
                        path = path.Substring(5);
                        path = path.Replace(@"\\", @"\");
                        break;
                    }
                }
                tr.Close();
            }
            return path;


        }

#pragma warning disable RECS0154 // Parameter is never used

        /// <summary>
        /// Get file path for an implementation file which uses code generation. It transforms the path into the local path.
        /// Note: A file might have one or no implementation language.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        public static string GetGenFilePathElement(Repository rep, EA.Element el)
#pragma warning restore RECS0154 // Parameter is never used
        {
            return GetFilePath(rep, el.Gentype, el.Genfile);
        }
        /// <summary>
        /// 1. Get the ID from pkg.Flags
        /// 2. Get root path from id in "%APPDATA%\Sparx System\EA\paths.tyt"
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pkg"></param>
        /// <returns></returns>
        public static string GetVccRootPath(Repository rep, EA.Package pkg)
        {
            string rootPath = "";
            var pattern = new Regex(@"VCCFG=[^;]+");
            Match regMatch = pattern.Match(pkg.Flags);
            if (regMatch.Success)
            {
                // get VCCFG
                var uniqueId = regMatch.Value.Substring(6);
                // get path for UiqueId
                Environment.CurrentDirectory = Environment.GetEnvironmentVariable(@"appdata");
                string s1 = @"Sparx Systems\EA\paths.txt";
                TextReader tr = new StreamReader(s1);
                string line = "";
                pattern = new Regex(@"(id=" + uniqueId + @").+(path=[^;]+)");
                while ((line = tr.ReadLine()) != null)
                {

                    regMatch = pattern.Match(line);
                    if (regMatch.Success)
                    {
                        rootPath = regMatch.Groups[2].Value;
                        rootPath = rootPath.Substring(5);
                        break;
                    }
                }
                tr.Close();
                if (rootPath == "")
                {
                    rep.WriteOutput("Debug", "VCCFG=... not found in" + s1 + " " + pkg.Name, 0);
                }
                return rootPath;
            }
            rep.WriteOutput("Debug", "VCCFG=... not found:" + pkg.Name, 0);
            return "";
        }

        public static string GetVccFilePath(Repository rep, EA.Package pkg)
        {
            string rootPath = GetVccRootPath(rep, pkg);
            var path = rootPath + @"\" + pkg.XMLPath;
            return path;
        }
        /// <summary>
        /// Get latest for all checked in packages. It is possible to use it recursive
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pkg"></param>
        /// <param name="recursive"></param>
        /// <param name="count"></param>
        /// <param name="level"></param>
        /// <param name="errorCount"></param>
        /// <returns></returns>
        public static bool GetLatest(Repository rep, EA.Package pkg, bool recursive, ref int count, int level,
            ref int errorCount)
        {
            var pkgState = (VC.Vc.EnumCheckOutStatus) pkg.VersionControlGetStatus();
            if (pkg.IsControlled && pkgState == VC.Vc.EnumCheckOutStatus.CsCheckedIn)
            {
                level = level + 1;
                // check if checked out

                string path = GetVccFilePath(rep, pkg);
                string fText;
                //rep.WriteOutput("Debug", "Path:" + pkg.Name + path, 0);
                var sLevel = new string(' ', level*2);
                rep.WriteOutput("Debug", sLevel + (count + 1).ToString(",0") + " Work for:" + path, 0);
                if (path != "")
                {
                    count = count + 1;
                    rep.ShowInProjectView(pkg);
                    // delete a potential write protection
                    try
                    {
                        var fileInfo = new FileInfo(path);
                        fileInfo.IsReadOnly = false;

                        // delete old file to get the new one
                        File.Delete(path);
                    }
                    catch (FileNotFoundException e)
                    {
                        fText = path + " " + e.Message;
                        rep.WriteOutput("Debug", fText, 0);
                        errorCount = errorCount + 1;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        fText = path + " " + e.Message;
                        rep.WriteOutput("Debug", fText, 0);
                        errorCount = errorCount + 1;
                    }
                    // get latest
                    try
                    {
                        // to make sure pkg is the correct reference
                        // new load of pkg after GetLatest
                        string pkgGuid = pkg.PackageGUID;
                        pkg.VersionControlGetLatest(true);
                        pkg = rep.GetPackageByGuid(pkgGuid);
                        count = count + 1;
                    }
                    catch
                    {
                        fText = path + " " + pkg.GetLastError();
                        rep.WriteOutput("Debug", fText, 0);
                        errorCount = errorCount + 1;
                    }

                }
                else
                {
                    fText = pkg.XMLPath + " invalid path";
                    rep.WriteOutput("Debug", fText, 0);
                    errorCount = errorCount + 1;

                }
            }

            //rep.WriteOutput("Debug", "Recursive:" +recursive.ToString(), 0);
            if (recursive)
            {
                //rep.WriteOutput("Debug","Recursive count:" + pkg.Packages.Count.ToString(), 0);
                // over all contained packages
                foreach (EA.Package pkgNested in pkg.Packages)
                {
                    //rep.WriteOutput("Debug", "Recursive:"+ pkgNested.Name, 0);
                    GetLatest(rep, pkgNested, true, ref count, level, ref errorCount);

                }
            }
            return true;

        }

        private static string GetConnectionString(Repository rep)
        {
            string s = rep.ConnectionString;
            if (s.Contains("DBType="))
            {
                return s;
            }
            var f = new FileInfo(s);
            if (f.Length > 1025)
            {
                return s;
            }
            return File.ReadAllText(s);
        }

        public static void OpenBehaviorForElement(Repository repository, EA.Element el)
        {
            // find the diagram
            if (el.Diagrams.Count > 0)
            {
                // get the diagram
                var dia = (EA.Diagram) el.Diagrams.GetAt(0);
                // open diagram
                repository.OpenDiagram(dia.DiagramID);
            }
            // no diagram found, select element
            repository.ShowInProjectView(el);
        }

        public static bool SetXmlPath(Repository rep, string guid, string path)
        {

            string updateStr = @"update t_package set XMLPath = '" + path +
                               "' where ea_guid = '" + guid + "' ";

            rep.Execute(updateStr);


            return true;
        }

        public static void SetReadOnlyAttribute(string fullName, bool readOnly)
        {
            var fileInfo = new FileInfo(fullName);
            fileInfo.IsReadOnly = readOnly;
            
        }

        #region visualizePortForDiagramobject

        /// <summary>
        /// Visualize port with or without interface (required/provided) for diagram object
        /// return: true = port was newly shown
        ///         false= part was already shown
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pos"></param>
        /// <param name="dia"></param>
        /// <param name="diaObjSource"></param>
        /// <param name="port"></param>
        /// <param name="portInterface"></param>
        /// <param name="portBoundTo"></param>
        public static bool VisualizePortForDiagramobject(EA.Repository rep, int pos, EA.Diagram dia, EA.DiagramObject diaObjSource,
            EA.Element port,
            EA.Element portInterface, 
            string portBoundTo = "right")
        {
            // check if port already exists
            foreach (EA.DiagramObject diaObjPort in dia.DiagramObjects)
            {
                if (diaObjPort.ElementID == port.ElementID) return false;
            }


            // visualize ports
            string type = rep.GetElementByID(diaObjSource.ElementID).Type;
            int portPosLength = 20;
            int portPosDistance = 35;
            int portPosStart = 35;
            int portPosOffset = 15;
            switch (type) 
            {
                case "Port":
                    portPosOffset = 10;
                    portPosDistance = 25;
                    portPosLength = 20;
                    break;
                case "Pin":
                    portPosOffset = 0;
                    portPosDistance = 25;
                    portPosLength = 20;
                    break;
                case "Parameter":
                    portPosLength = 30;
                    portPosDistance = 35;
                    portPosStart = 35;
                    portPosOffset = 10;
                    break;
            }
           
            int leftPort;
            int rightPort;
            // calculate target position of port
            if (portBoundTo == "right" || portBoundTo == "")
            {
                leftPort = diaObjSource.right - portPosLength/2 + portPosOffset;
                rightPort = leftPort + portPosLength;
            }
            else
            {
                leftPort = diaObjSource.left - portPosLength/2 + portPosOffset;
                rightPort = leftPort + portPosLength;

            }

            int top = diaObjSource.top;


            int topPort = top - portPosStart - pos* portPosDistance; 
            int bottomPort = topPort - portPosLength;

            // diagram object can't host port (not tall enough)
            // make diagram object taller to host all ports
            if (bottomPort <= diaObjSource.bottom)
            {
                diaObjSource.bottom = diaObjSource.bottom - 30;
                diaObjSource.Update();
            }

            //string position = $"l={leftPort};r={rightPort};t={Math.Abs(topPort)};b={Math.Abs(bottomPort)};";
            string position = $"l={leftPort};r={rightPort};t={topPort};b={bottomPort};";
            var diaObjectPort = (EA.DiagramObject) dia.DiagramObjects.AddNew(position, "");
            if (port.Type.Equals("Port"))
            {
                string hdn = "HDN=0:"; // Port without interface, visualize name, label
                string ox = "OX=23:";  // Port without interface, visualize name, label
                if (port.EmbeddedElements.Count > 0 || portInterface != null)
                {  // port with interface
                    ox = "OX=60:"; // more to the right side
                    hdn = "HDN=1:";// without label
                }
                diaObjectPort.Style = $@"LBL=CX=200:CY=12:{ox}OY=0:{hdn}BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            }
            else
            {

                // Parameter: Not showing label
                diaObjectPort.Style = "LBL=CX=97:CY=13:OX=39:OY=0:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            }
            diaObjectPort.ElementID = port.ElementID;
            diaObjectPort.Update();
            dia.DiagramObjects.Refresh();// first update element than refresh collection 

            //----------------------------------------------------------------------------
            // Show of port: Embedded Interface/Port
            if (portInterface == null) return true;

            // visualize interface
            position = $"l={leftPort-15};r={rightPort-15};t={topPort};b={bottomPort};";
            EA.DiagramObject diaObjectPortInterface =
                (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");

            // diaObject2.Style = "LBL=CX=69:CY=13:OX=45:OY=0:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            // HDN=0 Label visible
            // HDN=1 Label invisible
            // PType=1: Type Shown
            // CX = nn; Name Position
            // OX = nn; Label Position, -nn = Left, +nn = Right
            diaObjectPortInterface.Style = "LBL=CX=69:CY=13:OX=45:OY=0:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            diaObjectPortInterface.ElementID = portInterface.ElementID;
            diaObjectPortInterface.Update();
            dia.DiagramObjects.Refresh(); // first update element than refresh collection 
            return true;

        }

        #endregion

        public static DiagramLink GetDiagramLinkFromConnector(EA.Diagram dia, int connectorId)
        {
            foreach (DiagramLink link in dia.DiagramLinks)
            {
                if (link.ConnectorID == connectorId)
                {
                    return link;
                }
            }
            return null;
        }



        // Find the operation from Activity / State Machine
        // it excludes operations in state machines
        public static EA.Package GetModelDocumentFromPackage(Repository rep, EA.Package pkg)
        {
            EA.Package pkg1 = null;
            string repositoryType = "JET"; // rep.RepositoryType();

            // get object_ID of package
            var query = @"select pkg.ea_GUID AS EA_GUID " +
                        @" from (((t_object o  INNER JOIN t_attribute a on (o.object_ID = a.Object_ID AND a.type = 'Package')) " +
                        @"     INNER JOIN t_package pkg on (pkg.Package_ID = o.Package_ID)) " +
                        @"		  INNER JOIN t_object o1 on (cstr(o1.object_id) = a.classifier)) " +
                        @" where o1.ea_guid = '" + pkg.PackageGUID + "' ";


            if (repositoryType == "JET")
            {
                query = @"select pkg.ea_GUID AS EA_GUID " +
                        @" from (((t_object o  INNER JOIN t_attribute a on (o.object_ID = a.Object_ID AND a.type = 'Package')) " +
                        @"     INNER JOIN t_package pkg on (pkg.Package_ID = o.Package_ID)) " +
                        @"		  INNER JOIN t_object o1 on (cstr(o1.object_id) = a.classifier)) " +
                        @" where o1.ea_guid = '" + pkg.PackageGUID + "' ";
            }
            if (repositoryType == "SQLSVR")
                // SQL Server
            {
                query = @"select pkg.ea_GUID AS EA_GUID " +
                        @" from (((t_object o  INNER JOIN t_attribute a on (o.object_ID = a.Object_ID AND a.type = 'Package')) " +
                        @"     INNER JOIN t_package pkg on (pkg.Package_ID = o.Package_ID)) " +
                        @"		  INNER JOIN t_object o1 on o1.object_id = Cast(a.classifier As Int)) " +
                        @" where o1.ea_guid = '" + pkg.PackageGUID + "' ";

            }



            string str = rep.SQLQuery(query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode operationGuidNode = xmlDoc.SelectSingleNode("//EA_GUID");

            if (operationGuidNode != null)
            {
                string guid = operationGuidNode.InnerText;
                pkg1 = rep.GetPackageByGuid(guid);
            }
            return pkg1;
        }

        public static EA.Package GetFirstControlledPackage(Repository rep, EA.Package pkg)
        {
            if (pkg.IsControlled) return pkg;
            var pkgId = pkg.ParentID;
            if (pkgId == 0) return null;
            pkg = GetFirstControlledPackage(rep, rep.GetPackageByID(pkgId));
            return pkg;

        }

        /// <summary>
        /// Reverse direction of connector
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="con"></param>
        private static void ReverseConnectorDirection(Repository rep, Connector con)
        {
            // reverse connector direction
            SetConnector(rep, con.ConnectorGUID, con.SupplierID, con.ClientID);
            // handle connectors
            // A Connector may have n information flows realized
            if (con.MetaType == "Connector")
            {
                string sql = $"select Description from t_xref where Client = '{con.ConnectorGUID}'";
                var list = rep.GetStringsBySql(sql);
                foreach (var description in list)
                {
                    // {GUID},{GUID},..
                    foreach (var guid in description.Split(','))
                    {
                       SetConnector(rep, guid,  con.SupplierID, con.ClientID);
                       
                    }
                }

            }

        }

        /// <summary>
        /// Set connector start and end point
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="guid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private static void SetConnector(Repository rep, string guid, int start, int end)
        {
            string sql =
                               "update t_connector set " +
                               $" Start_Object_Id = {start}, End_Object_Id = {end} " +
                               $" where ea_guid = '{guid}'";
            rep.Execute(sql);

        }
        /// <summary>
        /// Delete file with error handling
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool TryToDeleteFile(string fileName)
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
