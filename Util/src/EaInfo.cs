using System;
using System.Collections.Generic;
using EA;

namespace VwTools_Utils.General
{
    public class EaInfo
    {
        private readonly EA.Repository _rep;
        // ReSharper disable once NotAccessedField.Local
        public List<string> FullText { get; set; }

        private const int NameLength = 35;
        private const int IdLength = 5;
        private const int TypeLength = 18;
        private const string Tab = "    ";
        public EaInfo(EA.Repository rep) {
            _rep = rep;
            FullText = new List<string>();
        }

        /// <summary>
        /// Write EA-Context information to Clipboard and EA output Tab "Debug"
        /// - GUID
        /// - ID
        /// - Type 
        /// </summary>
        /// <returns></returns>
        public string WriteInfoContext()
        {
            string name = "";
            string type = "";
            string strGuid = "";
            string description = "";
            int id = 0;
            PrepareOutput();
            var eaDiagram = new hoTools.Utils.Diagram.EaDiagram(_rep, true);
            if (eaDiagram.Dia != null)
            {
                DivideOutput("Diagram");
                // Diagram
                name = eaDiagram.Dia.Name;
                type = eaDiagram.Dia.Type;
                strGuid = eaDiagram.Dia.DiagramGUID;
                id = eaDiagram.Dia.DiagramID;
                description = "";
                OutputLine(name, type, strGuid, id, description);
                if (eaDiagram.SelObjects.Count > 0) DivideOutput("DiagramObjects");
                foreach (EA.DiagramObject dObj in eaDiagram.SelObjects)
                {
                    if (dObj.ObjectType == ObjectType.otDiagramObject) {
                        // DiagramObject + Element/Package
                        var el = _rep.GetElementByID(dObj.ElementID);
                        strGuid = el.ElementGUID;
                        type = el.Type;
                        name = el.Name;
                        id = el.ElementID;
                        description = "";

                        if (el.ClassifierID != 0)
                        {
                            var classifier = _rep.GetElementByID(el.ClassifierID);
                            description = $@"Classifier: {classifier.ElementID},{classifier.ElementGUID} {classifier.Name}:{classifier.ObjectType} ";
                        }
                        OutputLine(name, type, strGuid, id, description);
                    }
                    if (dObj.ObjectType == ObjectType.otPackage)
                    {
                        var pkg = _rep.GetPackageByID(dObj.ElementID);
                        strGuid = pkg.PackageGUID;
                        type = "Package";
                        name = pkg.Name;
                        id = pkg.PackageID;
                        description = "";
                        OutputLine(name, type, strGuid, id, description);
                    }
                }
                if (eaDiagram.SelLinks.Count > 0) DivideOutput("DiagramLinks"); 
                foreach (EA.DiagramLink link in eaDiagram.SelLinks)
                {
     
                    // DiagramLink + Connector
                    var con = _rep.GetConnectorByID(link.ConnectorID);
                    strGuid = con.ConnectorGUID;
                    type = con.Type;
                    name = con.Name;
                    id = con.ConnectorID;
                    description = "";
                    var sourceId = con.ClientID;
                    var targetId = con.SupplierID;
                    var sourceEl = _rep.GetElementByID(sourceId);
                    var targetEl = _rep.GetElementByID(targetId);
                    var diaLinkDescr = $@"Link: {link.InstanceID}/Hidden={link.IsHidden} order by supplier";
                    description = $@"Client={sourceId,-IdLength}/{sourceEl.Name,-NameLength}{Tab}{Tab}{Tab}Supplier={targetId,-IdLength}/{targetEl.Name,-NameLength}{Tab}{Tab}{Tab}{diaLinkDescr}";

                    OutputLine(name, type, strGuid, id, description);
                }
                EndOutput();
                return "";
            }




            if (_rep == null) return "";
           
           

            EA.ObjectType objType = _rep.GetContextItem(out object o);
            switch (objType)
            {
                case EA.ObjectType.otElement:
                    var el = (EA.Element)o;
                    strGuid = el.ElementGUID;
                    type = el.Type;
                    name = el.Name;
                    id = el.ElementID;

                    if (el.ClassifierID != 0)
                    {
                        var classifier = _rep.GetElementByID(el.ClassifierID);
                        description = $@"Classifier: {classifier.ElementID},{classifier.ElementGUID} {classifier.Name}:{classifier.ObjectType} ";
                    }

                    break;
                case EA.ObjectType.otPackage:
                    strGuid = ((EA.Package) o).PackageGUID;
                    type = "Package";
                    name = ((EA.Package) o).Name;
                    break;
                case EA.ObjectType.otDiagram:
                    strGuid = ((EA.Diagram) o).DiagramGUID;
                    type = $"Diagram {((EA.Diagram)o).Type}";
                    name = $"{((EA.Diagram)o).Name}";
                    id = ((EA.Diagram)o).DiagramID;
                    break;
                case EA.ObjectType.otAttribute:
                    strGuid = ((EA.Attribute) o).AttributeGUID;
                    name = $"{((EA.Attribute)o).Name}";
                    type = "Attribute";
                    id = ((EA.Attribute)o).AttributeID;
                    break;
                case EA.ObjectType.otMethod:
                    strGuid = ((EA.Method) o).MethodGUID;
                    name = $"{((EA.Method)o).Name}";
                    type = "Method";
                    id = ((EA.Method)o).MethodID;
                    break;

                case EA.ObjectType.otConnector:
                    var c = (EA.Connector)o;
                    strGuid = c.ConnectorGUID;
                    type = $"{c.Type}";
                    name = $"{c.Name}";
                    id = ((EA.Connector)o).ConnectorID;
                    var sourceId = c.ClientID;
                    var targetId = c.SupplierID;
                    var sourceEl = _rep.GetElementByID(sourceId);
                    var targetEl = _rep.GetElementByID(targetId);

                    var dia = _rep.GetCurrentDiagram();
                    var diaLinkDescr = "Link:";
                    if (dia != null)
                    {
                        foreach (var lt in dia.DiagramLinks)
                        {
                            var l = (DiagramLink)lt;
                            if (l.ConnectorID == id)
                            {
                                diaLinkDescr = $@"Link: {l.InstanceID,-IdLength}/Hidden={l.IsHidden}";
                                break;
                            }
                        }
                    }
                    description = $@"Client={sourceId,-IdLength}/{sourceEl.Name,-NameLength} Supplier={targetId,-IdLength}/{targetEl.Name,-NameLength}
DiagramId: {dia?.DiagramID}, {dia?.DiagramGUID} {diaLinkDescr}";


                    break;
                case EA.ObjectType.otModel:
                    strGuid = ((EA.Package) o).PackageGUID;
                    name = $"{((EA.Package)o).Name}";
                    type = "Model";
                    id = ((EA.Package)o).PackageID;
                    break;
                case EA.ObjectType.otParameter:
                    strGuid = ((EA.Parameter) o).ParameterGUID;
                    name = $"{((EA.Parameter)o).Name}";
                    type = "Parameter";
                    id = 0;
                    break;

            }

            string txt = "";
            if (String.IsNullOrWhiteSpace(strGuid)) Clipboard.ClipboardNoException.Clear();
            else
            {
                PrepareOutput();
                OutputLine(name, type, strGuid, id, description);
                EndOutput();
               
            }
            return txt;
        }

        private void OutputLine(string name, string type, string guid, int id, string description)
        {
            var txt = $"'{name,-NameLength}:{type,-TypeLength}{Tab}' {guid}/{id,-IdLength}{Tab}{Tab}{Tab}{description}";
            FullText.Add(txt);
            _rep.WriteOutput("Debug", txt, 0);
        }

        private void DivideOutput(string text)
        {
            _rep.WriteOutput("Debug", "", 0);
            _rep.WriteOutput("Debug", text, 0);
        }
        private void PrepareOutput()
        {
            _rep.CreateOutputTab("Debug");
            _rep.EnsureOutputVisible("Debug");
        }

        private void EndOutput()
        {
            VwTools_Utils.Clipboard.ClipboardNoException.SetText(String.Join($@"{Environment.NewLine}",FullText));
        }
    }
}
