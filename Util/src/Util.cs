using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using EA;
using hoTools.Utils.svnUtil;

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
                MessageBox.Show($"{ex.Message}\n\nFile:'{filePath}'", $"Can't open file {Path.GetFileName(filePath)}");
            }
        }
        #endregion
        public static string objectTypeToString(EA.ObjectType objectType)
        {
            switch (objectType)
            {
                case EA.ObjectType.otPackage:
                    return "Package";
                case EA.ObjectType.otElement:
                    return "Element";
                case EA.ObjectType.otDiagram:
                    return "Diagram";
                case EA.ObjectType.otMethod:
                    return "Operation";
                case EA.ObjectType.otAttribute:
                    return "Attribute";
                default:
                    return "unknown object type";
            } 
        }
       


        public static EA.Element getElementFromContextObject(EA.Repository rep)  {
            EA.Element el = null;
            EA.ObjectType objectType = rep.GetContextItemType();
            switch (objectType)
            {
                case EA.ObjectType.otAttribute:
                    var a = (EA.Attribute)rep.GetContextObject();
                    el = rep.GetElementByID(a.ParentID);
                    break;
                case EA.ObjectType.otMethod:
                    var m = (EA.Method)rep.GetContextObject();
                    el = rep.GetElementByID(m.ParentID);
                    break;
                case EA.ObjectType.otElement:
                    el = (EA.Element)rep.GetContextObject();
                    break;
                case EA.ObjectType.otNone:
                    EA.Diagram dia = rep.GetCurrentDiagram();
                    if (dia != null)
                    {
                        if (dia.SelectedObjects.Count == 1)
                        {
                            var objSelected = (EA.DiagramObject)dia.SelectedObjects.GetAt(0);
                            el = rep.GetElementByID(objSelected.ElementID);
                        }
                    }
                    break;
                default:
                    MessageBox.Show("No Interface, Class or Component selected");
                    break;
             }
            return el;
        }
        public static void startApp(string app, string par)
        {
            var p = new Process();
            p.StartInfo.FileName = app;
            p.StartInfo.Arguments = par;
            try
            {
                p.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(p.StartInfo.FileName + " " +
                                p.StartInfo.Arguments + "\n\n" +
                                "Have you set the %path% environment variable?\n\n" + e,
                                "Can't show controlled package");
            }
        }
        public static string getWildCard(Repository rep)
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
            else { return "%"; }
                
        }
        public static void setSequenceNumber(EA.Repository rep, EA.Diagram dia, 
            EA.DiagramObject obj, string sequence )
        {
            string s = "";
            if (!(obj == null)) s = " AND instance_id = " + obj.InstanceID;

            string updateStr = @"update t_DiagramObjects set sequence = " + sequence +
                       " where diagram_id = " + dia.DiagramID + 
                       " AND instance_id = " + obj.InstanceID; 

            rep.Execute(updateStr);
            return;

        }

        public static void addSequenceNumber (EA.Repository rep, EA.Diagram dia) {
             
            string updateStr = @"update t_DiagramObjects set sequence = sequence + 1 "+
                       " where diagram_id = " + dia.DiagramID; 

            rep.Execute(updateStr);
            return;

        }
        public static int getHighestSequenceNoFromDiagram(EA.Repository rep, EA.Diagram dia)
        {
            int sequenceNumber = 0;
            string query = @"select sequence from t_diagramobjects do " +
                            "  where do.Diagram_ID = "+ dia.DiagramID +
                            "  order by 1 desc";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//SEQUENCE_NUMBER");
            if (operationGUIDNode != null)
            {
                sequenceNumber = Convert.ToInt32(operationGUIDNode.InnerText);
            }
            return sequenceNumber;
        }

        // replace of API-function getDiagramObjectByID which isn't available in EA 9.
        public static EA.DiagramObject getDiagramObjectByID(EA.Repository rep, EA.Diagram dia, int ElementID)
        {
            if (rep.LibraryVersion > 999)
            { 
                return dia.GetDiagramObjectByID(ElementID,"");
                //return null;
            } else
            {
                foreach (EA.DiagramObject obj in dia.DiagramObjects)
                {
                    if (obj.ElementID == ElementID)
                    {
                        return obj;
                    }
                }
                return null;
            }
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
        public static void setLineStyleForDiagramLink(string lineStyle, EA.DiagramLink link)
        {
            #pragma warning disable RECS0012
            lineStyle = lineStyle + "  "; 
            if (lineStyle.Substring(0, 2).ToUpper() == "NO") return;
            if (lineStyle.Substring(0,2) == "TH") lineStyle = "H ";
            if (lineStyle.Substring(0,2) == "TV") lineStyle = "V ";
            if (lineStyle.Substring(0,1) == "D") link.Style = "Mode=1;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;";
            else if (lineStyle.Substring(0,1) == "A") link.Style = "Mode=2;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;";
            else if (lineStyle.Substring(0,1) == "C") link.Style = "Mode=3;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;";
            else if (lineStyle.Substring(0,1) == "B") link.Style = "Mode=8;EOID=61B36ED5;SOID=08967F1E;Color=-1;LWidth=0;";
            else
            {
            link.Style = "Mode=3;EOID=A36C0F5C;SOID=3ECFB522;Color=-1;LWidth=0;TREE=" +
                                    lineStyle.Trim() + ";";

            }
            link.Update();
            #pragma warning restore RECS0012
        }


        //--------------------------------------------------------------------------------------------------------------
        // SetLineStyleDiagramObjectsAndConnectors  Set line style for diagram objects and connectors
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
        public static void SetLineStyleDiagramObjectsAndConnectors(EA.Repository rep, EA.Diagram d, string lineStyle)
         {
             EA.Collection selectedObjects = d.SelectedObjects;
             EA.Connector selectedConnector = d.SelectedConnector;
             // store current diagram
             rep.SaveDiagram(d.DiagramID);
             foreach (EA.DiagramLink link in d.DiagramLinks)
             {
                 if (link.IsHidden == false)
                 {

                     // check if connecter is connected with diagram object
                     EA.Connector c = rep.GetConnectorByID(link.ConnectorID);
                     foreach (EA.DiagramObject dObject in d.SelectedObjects)
                     {
                         if (c.ClientID == dObject.ElementID | c.SupplierID == dObject.ElementID)
                         {

                             setLineStyleForDiagramLink(lineStyle, link);
                         }
                     }
                     if (selectedConnector != null)
                     {
                         if (c.ConnectorID == selectedConnector.ConnectorID)
                         {
                             setLineStyleForDiagramLink(lineStyle, link);
                             continue;
                         }
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


        public static void SetLineStyleDiagram(EA.Repository rep, EA.Diagram d, string lineStyle)
        {
            // store current diagram
            rep.SaveDiagram(d.DiagramID);
            // all links
            foreach (EA.DiagramLink link in d.DiagramLinks)
            {
                if (link.IsHidden == false)
                {
                    setLineStyleForDiagramLink(lineStyle, link);
                }

            }
            rep.ReloadDiagram(d.DiagramID);
        }

        
        public static void ChangeClassNameToSynonyms(EA.Repository rep, EA.Element el)
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
        public static void ChangePackageClassNameToSynonyms(EA.Repository rep, EA.Package pkg)
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


        public static bool updateClass(EA.Repository rep, EA.Element el)
        {

            foreach (EA.Attribute a in el.Attributes)
            {
                Util.updateAttribute(rep, a);
            }
            foreach (EA.Method m in el.Methods)
            {
                Util.updateMethod(rep, m);
            }

            // over all nested classes
            foreach (EA.Element e in el.Elements)
            {
                updateClass(rep, e);
            }
            return true;
        }

        public static bool updatePackage(EA.Repository rep, EA.Package pkg)
        {
            foreach (EA.Element el in pkg.Elements)
            {
                updateClass(rep, el);
            }
            foreach (EA.Package pkg1 in pkg.Packages)
            {
                updatePackage(rep, pkg1);
            }
            
            return true;
        }



        public static bool updateAttribute(EA.Repository rep, EA.Attribute a)
        {
            // no classifier defined
            if (a.ClassifierID == 0)
            {
                // find type from type_name
                int id = Util.getTypeID(rep, a.Type);
                if (id > 0)
                {
                    a.ClassifierID = id;
                    bool error = a.Update();
                    if (!error)
                    {
                        MessageBox.Show("Error write Attribute", a.GetLastError());
                        // Error occured
                        return false;
                    }
                }
            }
            return true;
        }

        // Update Method Types
        public static bool updateMethod(EA.Repository rep, EA.Method m)
        {

            int id;

            // over all parameters
            foreach (EA.Parameter par in m.Parameters)
            {
                if ((par.ClassifierID == "") || (par.ClassifierID == "0"))
                {
                    // find type from type_name
                    id = Util.getTypeID(rep, par.Type);
                    if (id > 0)
                    {
                        par.ClassifierID = id.ToString();
                        bool error = par.Update();
                        if (!error)
                        {
                            MessageBox.Show("Error write Parameter", m.GetLastError());
                            return false;
                            // Error occured
                        }
                    }


                }

            }
            // no classifier defined
            if ((m.ClassifierID == "") || (m.ClassifierID == "0"))
            {
                // find type from type_name
                id = Util.getTypeID(rep, m.ReturnType);
                if (id > 0)
                {
                    m.ClassifierID = id.ToString();
                    bool error = m.Update();
                    if (!error)
                    {
                        MessageBox.Show("Error write Method", m.GetLastError());
                        return false;
                        // Error occured
                    }
                }
            }
            return true;
        }



        // Find type for name
        // 1. Search for name (if type contains a '*' search for type with '*' and for type without '*'
        // 2. Search for Synonyms
        public static int getTypeID(EA.Repository rep, string name)
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
                                        where Object_Type in ('Class','PrimitiveType','DataType','Enumeration') AND name = '" + name + "' ";
                string str = rep.SQLQuery(query);
                var XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(str);

                XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//OBJECT_ID");
                if (operationGUIDNode != null)
                {
                    intReturn = Convert.ToInt32(operationGUIDNode.InnerText);
                }
            }

            
            return intReturn;
        }

        public static int getTypeFromName(EA.Repository rep, ref string name, ref string type) {
            int id = 0;
            id = Util.getTypeID(rep, type);
            if (id == 0 & type.Contains("*"))
            {
                type = type.Remove(type.IndexOf("*", StringComparison.CurrentCulture), 1);
                name = "*" + name;
                id = Util.getTypeID(rep, type);
                if (id == 0 & type.Contains("*"))
                {
                    type = type.Replace("*", "");
                    name = "*" + name;
                    id = Util.getTypeID(rep, type);
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
        public static EA.Element  getParameterFromActivity(EA.Repository rep, EA.Parameter par, EA.Element act, Boolean isReturn = false)
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
                             " AND  o2.Alias like '"+ aliasName + getWildCard(rep) + "'";
            string str = rep.SQLQuery(query); 
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//CLASSIFIER_GUID");
            if (operationGUIDNode != null)
            {
                string GUID = operationGUIDNode.InnerText;
                parTrgt = rep.GetElementByGuid(GUID);
            }
            return parTrgt;
        }

        // Find the calling operation from a Call Operation Action
        public static EA.Method getOperationFromAction(EA.Repository rep, EA.Element action)
        {
            EA.Method method = null;
            string query = @"select o.Classifier_guid AS CLASSIFIER_GUID
                      from t_object o 
                      where o.Object_ID = " + action.ElementID;
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//CLASSIFIER_GUID");
            if (operationGUIDNode != null)
            {
                string GUID = operationGUIDNode.InnerText;
                method = rep.GetMethodByGuid(GUID);
            }
            return method;
        }
        // Find the calling operation from a Call Operation Action
        public static string getParameterType(EA.Repository rep, string actionPinGuid)
        {
            string query = @"SELECT par.type AS OPTYPE 
			    from t_object o  inner join t_operationparams par on (o.classifier_guid = par.ea_guid)
                where o.ea_guid = '" + actionPinGuid + "' ";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode typeGUIDNode = XmlDoc.SelectSingleNode("//OPTYPE");
            if (typeGUIDNode != null)
            {
                return typeGUIDNode.InnerText;
                
            }
            return "";
        }


        // Find the calling operation from a Call Operation Action
        public static EA.Method getOperationFromCallAction(EA.Repository rep, EA.Element obj)
        {
            string wildCard = getWildCard(rep);
            string query = @"SELECT op.ea_guid AS OPERATION from (t_object o inner join t_operation op on (o.classifier_guid = op.ea_guid))
               inner join t_xref x on (x.client = o.ea_guid)
			   where x.name = 'CustomProperties' and
			             x.description like '"+ wildCard + "CallOperation" + wildCard + 
                         "' and o.object_id = " + obj.ElementID;
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//OPERATION");
            string GUID = "";
            if (operationGUIDNode != null)
            {
                GUID = operationGUIDNode.InnerText;
                return rep.GetMethodByGuid(GUID);
            }
            return null;
        }
        // Find the calling operation from a Call Operation Action
        public static string getClassifierGUID(EA.Repository rep,string GUID)
        {
            string query = @"select o.Classifier_guid AS CLASSIFIER_GUID
                      from t_object o 
                      where o.EA_GUID = '" + GUID + "'";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//CLASSIFIER_GUID");
            GUID = "";
            if (operationGUIDNode != null)
            {
                GUID = operationGUIDNode.InnerText;
            }
            return GUID;
        }


        // Gets the trigger associated with the connector / element
        public static string getTrigger(EA.Repository rep, string GUID)
        {
            string query = @"select x.Description AS TRIGGER_GUID
                      from t_xref x 
                      where x.Client = '" + GUID + "'    AND behavior = 'trigger' ";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//TRIGGER_GUID");
            GUID = "";
            if (operationGUIDNode != null)
            {
                GUID = operationGUIDNode.InnerText;
            }
            return GUID;
        }
        // Gets the signal associated with the element
        public static string getSignal(EA.Repository rep, string GUID)
        {
            string query = @"select x.Description AS SIGNAL_GUID
                      from t_xref x 
                      where x.Client = '" + GUID + "'    AND behavior = 'event' ";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//SIGNAL_GUID");
            GUID = "";
            if (operationGUIDNode != null)
            {
                GUID = operationGUIDNode.InnerText;
            }
            return GUID;
        }
        // Gets the composite element for a diagram GUID
        public static string getElementFromCompositeDiagram(EA.Repository rep, string diagramGUID)
        {
            string query = @"select o.ea_guid AS COMPOSITE_GUID
                      from t_xref x INNER JOIN t_object o on (x.client = o.ea_guid and type = 'element property')
                      where x.supplier = '" + diagramGUID + "'    ";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//COMPOSITE_GUID");
            diagramGUID = "";
            if (operationGUIDNode != null)
            {
                diagramGUID = operationGUIDNode.InnerText;
            }
            return diagramGUID;
        }
        // set "ShowBeh=1; in operation field StyleEx

        public static Boolean setShowBehaviorInDiagram(EA.Repository rep, EA.Method m)
        {
            string updateStr = @"update t_operation set StyleEx = 'ShowBeh=1;'"  +
                       " where operationID = " + m.MethodID;
            rep.Execute(updateStr);
            return true;
        }
        
        public static Boolean setFrameLinksToDiagram(EA.Repository rep, EA.Element frm, EA.Diagram dia)
        {
            string updateStr = @"update t_object set pdata1 = "+ dia.DiagramID + 
                       " where object_ID = " + frm.ElementID;
            rep.Execute(updateStr);
            return true;
        }
        public static Boolean setActivityCompositeDiagram(EA.Repository rep, EA.Element el, string s)
        {
            string updateStr = @"update t_object set pdata1 = '"+ s +"', ntype = 8 " +
                       " where object_ID = " + el.ElementID;
            rep.Execute(updateStr);
            return true;
        }
        public static Boolean setElementPDATA1(EA.Repository rep, EA.Element el, string s)
        {
            string updateStr = @"update t_object set pdata1 = '" + s + "' " +
                       " where object_ID = " + el.ElementID;
            rep.Execute(updateStr);
            return true;
        }
        public static Boolean setConnectorGuard(EA.Repository rep, int connectorID, string connectorGuard)
        {

            string updateStr = @"update t_connector set pdata2 = '" + connectorGuard + "' " +
           " where Connector_Id = " + connectorID;
            rep.Execute(updateStr);


            return true;
        }
        
        public static Boolean setDiagramHasAttchaedLink(EA.Repository rep, EA.Element el)
        {
            setElementPDATA1(rep,el,"Diagram Note"); 
            return true;
        }
        public static Boolean setVCFlags (EA.Repository rep, EA.Package pkg, string flags)
        {
            string updateStr = @"update t_package set packageflags = '" + flags +"' " +
                       " where package_ID = " + pkg.PackageID;
            rep.Execute(updateStr);
            return true;
        }

        public static Boolean setElementHasAttchaedLink(EA.Repository rep, EA.Element el, EA.Element elNote)
        {
            string updateStr = @"update t_object set pdata1 = 'Element Note', pdata2 = '" + el.ElementID  + "', pdata4='Yes' " +
           " where object_ID = " + elNote.ElementID ;
            rep.Execute(updateStr);


            return true;
        }

        public static Boolean setBehaviorForOperation(EA.Repository rep, EA.Method op, EA.Element act)
        {
            
            string updateStr = @"update t_operation set behaviour = '" + act.ElementGUID + "' " +
           " where operationID = " + op.MethodID;
            rep.Execute(updateStr);


            return true;
        }

        public static string getDiagramObjectLabel(EA.Repository rep, int objectID, int diagramID, int instanceID)
        {
            string attributeName = "OBJECT_STYLE";
            string query = @"select ObjectStyle AS " + attributeName + 
                    @" from t_diagramobjects
                      where Object_ID = " + objectID + @" AND 
                            Diagram_ID = " + diagramID + @" AND 
                            Instance_ID = " + instanceID ;

            return getSingleSQLValue(rep, query, attributeName);
        }

        private static string getSingleSQLValue(EA.Repository rep, string query, string attributeName)
        {
            string s = "";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode node = XmlDoc.SelectSingleNode("//"+attributeName);
            if (node != null)
            {
                s = node.InnerText;
            }
            return s;
        }

        public static Boolean setDiagramObjectLabel(EA.Repository rep, int objectID, int diagramID, int instanceID, string s)
        {

            string updateStr = @"update t_diagramObjects set ObjectStyle = '" + s + "' " +
           " where Object_ID = " + objectID + " AND "+
                " Diagram_ID = " + diagramID + " AND " +
                " Instance_ID = " + instanceID ;
               
            rep.Execute(updateStr);


            return true;
        }

        // Find the operation from Activity / State Machine
        // it excludes operations in state machines
        public static EA.Method getOperationFromBrehavior(EA.Repository rep, EA.Element el)
        {
            EA.Method method = null;
            string query = "";
            string conString = getConnectionString(rep); // due to shortcuts
            if (conString.Contains("DBType=3"))
            {   // Oracle DB
                query = 
                    @"select op.ea_guid AS EA_GUID
                      from t_operation op 
                      where Cast(op.Behaviour As Varchar2(38)) = '" + el.ElementGUID + "' "+
                            " AND (Type is Null or Type not in ('do','entry','exit'))";
            }
            if (conString.Contains("DBType=1"))
                // SQL Server
            {    query = 
                      @"select op.ea_guid AS EA_GUID
                        from t_operation op 
                        where Substring(op.Behaviour,1,38) = '" + el.ElementGUID + "'" +
                       " AND (Type is Null or Type not in ('do','entry','exit'))";

            }

            if (conString.Contains(".eap"))
                // SQL Server
            {     query = 
                      @"select op.ea_guid AS EA_GUID
                        from t_operation op 
                        where op.Behaviour = '" + el.ElementGUID + "'" +
                       " AND ( Type is Null or Type not in ('do','entry','exit'))";

            }
            if ((! conString.Contains("DBType=1")) &&  // SQL Server, DBType=0 MySQL
               (!  conString.Contains("DBType=3")) &&  // Oracle
               (!  conString.Contains(".eap")))// Access
            {
                query =
                  @"select op.ea_guid AS EA_GUID
                        from t_operation op 
                        where op.Behaviour = '" + el.ElementGUID + "'" +
                       " AND (Type is Null or Type not in ('do','entry','exit'))";

            }

            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//EA_GUID");
            if (operationGUIDNode != null)
            {
                string GUID = operationGUIDNode.InnerText;
                method = rep.GetMethodByGuid(GUID);
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



        public static EA.Method getOperationFromConnector(EA.Repository rep, EA.Connector con)
        {
            EA.Method method = null;
            string query = "";
            if (getConnectionString(rep).Contains("DBType=3"))
                //pdat3: 'Activity','Sequence', (..)
            {   // Oracle DB
                query =
                    @"select description AS EA_GUID
                      from t_xref x 
                      where Cast(x.client As Varchar2(38)) = '" + con.ConnectorGUID + "'" +
                                                                " AND Behavior = 'effect' ";
            }
            if (getConnectionString(rep).Contains("DBType=1"))
            {   // SQL Server

                query =
                      @"select description AS EA_GUID
                        from t_xref x 
                        where Substring(x.client,1,38) = " + "'" + con.ConnectorGUID + "'" +
                           " AND Behavior = 'effect' "                   
                                          ;
            }
            if (getConnectionString(rep).Contains(".eap"))
            {

                query =
                      @"select description AS EA_GUID
                        from t_xref x 
                        where client = " + "'" + con.ConnectorGUID + "'" +
                           " AND Behavior = 'effect' "
                                          ;
            }
            if ((! getConnectionString(rep).Contains("DBType=1")) &&  // SQL Server, DBType=0 MySQL
                (! getConnectionString(rep).Contains("DBType=3")) &&  // Oracle
                (! getConnectionString(rep).Contains(".eap")))// Access
            {
                query =
                @"select description AS EA_GUID
                        from t_xref x 
                        where client = " + "'" + con.ConnectorGUID + "'" +
                             " AND Behavior = 'effect' "
                                            ;

            }


            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            //string type = "";
            //XmlNode pdat3Node = XmlDoc.SelectSingleNode("//PDAT3");
            //if (pdat3Node != null)
            //{
            //    type = pdat3Node.InnerText;
                
            //}
            //if ( type.EndsWith(")")) // Operation
            //{ 
            string GUID = null;
                XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//EA_GUID");
                if (operationGUIDNode != null)
                {
                    GUID = operationGUIDNode.InnerText;
                    method = rep.GetMethodByGuid(GUID);
                }
                if (method == null)
                {

                     if (GUID != null) OpenBehaviorForElement(rep, rep.GetElementByGuid(GUID));
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
        public static string updateVC(EA.Repository rep, EA.Package pkg)
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


                var svnHandle = new svn(rep, pkg);
                userNameLockedPackage = svnHandle.getLockingUser();
                svnHandle = null;
                if (userNameLockedPackage != "") flags = flags + "CheckedOutTo=" + userNameLockedPackage + ";";
                try
                {
                    setVCFlags(rep, pkg, flags);
                    rep.ShowInProjectView(pkg);
                }
                catch (Exception e)
                {
                    string s = e.Message + " ;" + pkg.GetLastError();
                    s = s + "!";
                }


            }
            return userNameLockedPackage;
         }

        //------------------------------------------------------------------------------------------
        // resetVCRecursive   If package is controlles: Reset packageflags field of package, work for all packages recursive 
        //------------------------------------------------------------------------------------------
        // packageflags:  Recurse=0;VCCFG=unchanged;
        public static void resetVCRecursive(EA.Repository rep, EA.Package pkg)
        {
            resetVC(rep, pkg);
            foreach (EA.Package p in pkg.Packages)
            {
                resetVC(rep, pkg);
            }
        }
        //------------------------------------------------------------------------------------------
        // resetVC   If package is controlles: Reset packageflags field of package 
        //------------------------------------------------------------------------------------------
        // packageflags:  Recurse=0;VCCFG=unchanged;
        public static void resetVC(EA.Repository rep, EA.Package pkg)
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
                    setVCFlags(rep, pkg, flags);
                }
                catch (Exception e)
                {
                    string s = e.Message + " ;" + pkg.GetLastError();
                    s = s + "!";
                }


            }
            // recursive package
            //foreach (EA.Package pkg1 in pkg.Packages)
            //{
            //    updateVC(rep, pkg1);
            //}
        }
        public static string getVCstate(EA.Repository rep, EA.Package pkg, Boolean isLong) {
                        string[] checkedOutStatusLong = { "Uncontrolled",
                                                      "Checked in",
                                                      "Checked out to this user",
                                                      "Read only version",
                                                      "Checked out to another user",
                                                      "Offline checked in",
                                                      "Offline checked out by user",
                                                      "Offline checked out by other user", 
                                                      "Deleted" };
                        string[] checkedOutStatusShort = { "Uncontrolled",
                                                      "Checked in",
                                                      "Checked out",
                                                      "Read only",
                                                      "Checked out",
                                                      "Offline checked in",
                                                      "Offline checked out",
                                                      "Offline checked out", 
                                                      "Deleted" };

                        try
                        {
                            string s = ""; 
                             var svnHandle = new svn(rep, pkg);
                             s = svnHandle.getLockingUser();
                            svnHandle = null;
                            if (s != "") s = "CheckedOutTo=" + s ;
                            else s = "Checked in";
                            return s;
                            //state = pkg.VersionControlGetStatus();
                        }
                        catch (Exception e)
                        {
                            if (isLong) return "VC State Error: " + e.Message;
                            else return "State Error";
                        }

        }
        #pragma warning disable RECS0154 // Parameter is never used
        public static string getGenFilePath(EA.Repository rep, EA.Element el)
        #pragma warning restore RECS0154 // Parameter is never used
        {
            string path = el.Genfile;
            string localPathVar = "";
            
            // check if a local path is defined
            Match m = Regex.Match(path, @"%[^%]*");
            if (m.Success)
            {
                localPathVar = m.Value.Substring(1) ;
                // get path for localDir
                Environment.CurrentDirectory = Environment.GetEnvironmentVariable("appdata");
                string s1 = @"Sparx Systems\EA\paths.txt";
                TextReader tr = new StreamReader(s1);
                string line = "";
                Match regMatch;
                var pattern = new Regex(@"(type=" + el.Gentype + ";id=" + localPathVar + @").+(path=[^;]+)");
                while ((line = tr.ReadLine()) != null)
                {

                    regMatch = pattern.Match(line);
                    if (regMatch.Success)
                    {
                        path = path.Replace("%"+ localPathVar + "%" ,"");
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
        public static string getVccRootPath(EA.Repository rep, EA.Package pkg)
        {
            string rootPath = "";
            var pattern = new Regex(@"VCCFG=[^;]+");
            Match regMatch = pattern.Match(pkg.Flags);
            string uniqueId = "";
            if (regMatch.Success)
            {
                // get VCCFG
                uniqueId = regMatch.Value.Substring(6);
                // get path for UiqueId
                Environment.CurrentDirectory = Environment.GetEnvironmentVariable("appdata");
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
            else
            {
                rep.WriteOutput("Debug", "VCCFG=... not found:" + pkg.Name, 0);
                return "";
            }

        }

        public static string getVccFilePath(EA.Repository rep, EA.Package pkg)
        {
            string path = "";
            string rootPath = getVccRootPath(rep, pkg);
            path = rootPath + @"\" + pkg.XMLPath;
            return path;
        }
       
        public static Boolean getLatest(EA.Repository rep, EA.Package pkg, Boolean recursive, ref int count, int level, ref int errorCount)
        {
            if (pkg.IsControlled)
            {
                level = level + 1;
                // check if checked out

                string path = Util.getVccFilePath(rep, pkg);
                string fText = "";
                //rep.WriteOutput("Debug", "Path:" + pkg.Name + path, 0);
                var sLevel = new string(' ', level * 2);
                rep.WriteOutput("Debug", sLevel + (count+1).ToString(",0") + " Work for:" + path, 0);
                if (path != "")
                {
                    count = count + 1;
                    rep.ShowInProjectView(pkg);
                    // delete a potential write protection
                    try
                    {
                        var fileInfo = new FileInfo(path);
                        var attributes = (FileAttributes)(fileInfo.Attributes - FileAttributes.ReadOnly);
                        System.IO.File.SetAttributes(fileInfo.FullName, attributes);
                        System.IO.File.Delete(path);
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
                        string pkgGUID = pkg.PackageGUID;
                        pkg.VersionControlGetLatest(true);
                        pkg = rep.GetPackageByGuid(pkgGUID);
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
                    getLatest(rep, pkgNested, true, ref count, level, ref errorCount);

                }
            }
            return true;

        }
        public static string getConnectionString(EA.Repository rep) {
            string s = rep.ConnectionString;
            if (s.Contains("DBType="))
            {
                return s;
            }
            else
            {
                var f = new FileInfo(s);
                if (f.Length > 1025)
                {
                    return s;
                }
                else
                {
                    return System.IO.File.ReadAllText(s);
                }
            }
            

        }
        public static void OpenBehaviorForElement(EA.Repository Repository, EA.Element el)
        {
            // find the diagram
            if (el.Diagrams.Count > 0)
            {
                // get the diagram
                var dia = (EA.Diagram)el.Diagrams.GetAt(0);
                // open diagram
                Repository.OpenDiagram(dia.DiagramID);
            }
            // no diagram found, select element
            Repository.ShowInProjectView(el);
        }
        public static Boolean setXmlPath(EA.Repository rep, string guid, string path)
        {

            string updateStr = @"update t_package set XMLPath = '" + path +
           "' where ea_guid = '" + guid + "' ";
                
            rep.Execute(updateStr);


            return true;
        }
        public static void SetReadOnlyAttribute(string fullName, bool readOnly)
        {
            var filePath = new FileInfo(fullName);
            FileAttributes attribute;
            if (readOnly)
                attribute = filePath.Attributes | FileAttributes.ReadOnly;
            else
                attribute = (FileAttributes)(filePath.Attributes - FileAttributes.ReadOnly);

            System.IO.File.SetAttributes(filePath.FullName, attribute);
        }

        #region visualizePortForDiagramobject
        public static void visualizePortForDiagramobject(int pos, EA.Diagram dia, EA.DiagramObject diaObjSource, EA.Element port, 
            EA.Element interf, string portBoundTo = "right")
        {
            // check if port already exists
            foreach (EA.DiagramObject diaObjPort in dia.DiagramObjects)
            {
                if (diaObjPort.ElementID == port.ElementID) return;
            }

            // visualize ports
            int length = 12;
            int leftPort;
            int rightPort;
            // calculate target position of port
            if (portBoundTo == "right" || portBoundTo == "")
            {
                leftPort = diaObjSource.right - length / 2;
                rightPort = leftPort + length;
            }
            else
            {
                leftPort = diaObjSource.left - length / 2;
                rightPort = leftPort + length;

            }

            int top = diaObjSource.top;
                       

            int topPort = top - 35 - pos * 20 ;
            int bottomPort = topPort - length;

            // diagram object can't host port (not tall enough)
            // make diagram object taller to host all ports
            if (bottomPort <= diaObjSource.bottom)
            {
                diaObjSource.bottom = diaObjSource.bottom - 30; 
                diaObjSource.Update();
            }

            string position = "l=" + leftPort + ";r=" + rightPort + ";t=" + topPort + ";b=" + bottomPort + ";";
            var diaObjectPort = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
            if (port.Type.Equals("Port"))
            {
                // not showing label
                //diaObject.Style = "LBL=CX=97:CY=13:OX=45:OY=0:HDN=1:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
                diaObjectPort.Style = "LBL=CX=200:CY=12:OX=23:OY=1:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            }
            else
            {

                // not showing label
                diaObjectPort.Style = "LBL=CX=97:CY=13:OX=39:OY=3:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            }
            diaObjectPort.ElementID = port.ElementID;


            diaObjectPort.Update();

            if (interf == null) return;

            // visualize interface
            var diaObject2 = (EA.DiagramObject)dia.DiagramObjects.AddNew(position, "");
            dia.Update();
            diaObject2.Style = "LBL=CX=69:CY=13:OX=45:OY=0:HDN=0:BLD=0:ITA=0:UND=0:CLR=-1:ALN=0:ALT=0:ROT=0;";
            diaObject2.ElementID = interf.ElementID;
            diaObject2.Update();

        }
        #endregion
        public static EA.DiagramLink getDiagramLinkFromConnector(EA.Diagram dia, int connectorID)
        {
            foreach (EA.DiagramLink link in dia.DiagramLinks)
            {
                if (link.ConnectorID == connectorID)
                {
                    return link;
                }
            }
            return null;
        }


       
        // Find the operation from Activity / State Machine
        // it excludes operations in state machines
        public static EA.Package getModelDocumentFromPackage(EA.Repository rep, EA.Package pkg)
        {
            EA.Package pkg1 = null;
            string query = "";
            string repositoryType = "JET";// rep.RepositoryType();

            // get object_ID of package
            query = @"select pkg.ea_GUID AS EA_GUID " +
                   @" from (((t_object o  INNER JOIN t_attribute a on (o.object_ID = a.Object_ID AND a.type = 'Package')) " +
                   @"     INNER JOIN t_package pkg on (pkg.Package_ID = o.Package_ID)) " +
                   @"		  INNER JOIN t_object o1 on (cstr(o1.object_id) = a.classifier)) " +
                   @" where o1.ea_guid = '" + pkg.PackageGUID + "' ";
            
						 
            if (repositoryType== "JET")
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
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//EA_GUID");

            if (operationGUIDNode != null)
            {
                string GUID = operationGUIDNode.InnerText;
                pkg1 = rep.GetPackageByGuid(GUID);
            }
            return pkg1;
        }
        public static EA.Package getFirstControlledPackage(EA.Repository rep, EA.Package pkg)
        {
            int pkgID = 0;
            if (pkg.IsControlled) return pkg;
            pkgID = pkg.ParentID;
            if (pkgID == 0) return null;
            pkg = getFirstControlledPackage(rep, rep.GetPackageByID(pkgID));
            return pkg;

        }
    }
}
