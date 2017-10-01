using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EA;

namespace hoTools.Utils.Extension
{
    /// <summary>
    /// Extension Methods for EA Repository
    /// </summary>
    public static class EaExtensionClass
    {
      
        /// <summary>
        /// Returns a list of connectors. The query has to select ea_guid (the only column) of the needed connectors.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">SQL which select ea_guid</param>
        /// <returns>List of connectors</returns>
        // ReSharper disable once UnusedMember.Global
        public static List<Connector> GetConnectorsBySql(this Repository rep, string sql)
        {

            var lCon = new List<Connector>();
            // run query into XDocument to proceed with LinQ
            string xml =rep.SQLQuery(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get ea_guid from descendants of 
            var node = from row in x.Descendants("Row").Descendants()
                         select row;

            foreach (var row in node)
            {
                Connector con = rep.GetConnectorByGuid(row.Value);
                lCon.Add(con);
            }
    
            return lCon;
        }
        /// <summary>
        /// Returns a list of strings of the query with one column.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">SQL which one column</param>
        /// <returns>List of strings</returns>
        public static List<string> GetStringsBySql(this Repository rep, string sql)
        {

            var lCon = new List<string>();
            // run query into XDocument to proceed with LinQ
            string xml = rep.SQLQuery(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get ea_guid from descendants of 
            var node = from row in x.Descendants("Row").Descendants()
                       select row;

            foreach (var row in node)
            {
                lCon.Add(row.Value);
            }

            return lCon;
        }

        /// <summary>
        /// Return true if Element is an Embedded Element Type
        /// - Port
        /// - Activity Parameter
        /// - Parameter
        /// - ExpansionNode
        /// - Pin
        /// </summary>
        /// <param name="el"></param>
        /// <param name="rep"></param>
        /// <param name="alsoEmbeddedInterfaces"></param>
        /// <returns>EA Version</returns>
        // ReSharper disable once UnusedMember.Global
        public static bool IsEmbeddedElement(this EA.Element el, Repository rep, bool alsoEmbeddedInterfaces=false)
        {
            if (el.ParentID == 0) return false;
            if (alsoEmbeddedInterfaces)
            {
                return el.Type == "Port" ||
                       el.Type == "ActivityParameter" ||
                       el.Type == "Parameter" ||
                       el.Type == "ExpansionNode" ||
                       el.Type == "ActionPin" ||
                       el.Type == "RequiredInterface" || 
                       el.Type == "ProvidedInterface";
            }
            else
            {
                if (el.Type == "ExpansionNode" && rep != null)
                {
                    EA.Element elAction = rep.GetElementByID(el.ParentID);
                    if (elAction.Type == "Action") return false;
                }
                return el.Type == "Port" ||
                       el.Type == "ActivityParameter" ||
                       el.Type == "Parameter" ||
                       el.Type == "ExpansionNode" ||
                       el.Type == "ActionPin";
            }
        }
        /// <summary>
        /// Get parent of an Embedded Element which isn't an embedded element. Returns null if not found
        /// </summary>
        /// <param name="el"></param>
        /// <param name="rep"></param>
        /// <returns></returns>

        public static EA.Element GetParentOfEmbedded(this EA.Element el, EA.Repository rep)
        {
            if (!el.IsEmbeddedElement(rep, true))
            {
                return el;
            }
            if (el.ParentID == 0) return null;
            EA.Element elParent = rep.GetElementByID(el.ParentID);
            if (elParent.IsEmbeddedElement(rep, true))
            {
                return elParent.GetParentOfEmbedded(rep);
            }
            return elParent;
            


        }
        /// <summary>
        /// Returns the edge an embedded element is bound to (left, right, top, bottom)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static EmbeddedPosition Edge(this EA.DiagramObject obj, EA.Repository rep)
        {
            EA.Element el = rep.GetElementByID(obj.ElementID);


            EA.Element elParent = el.GetParentOfEmbedded(rep);
            if (elParent == null) return  EmbeddedPosition.undefined;
            // for Required/Required Interface use the owning Port for the position
            if (el.Type == "ProvidedInterface" ||
                el.Type == "RequiredInterface")
            {
                if (el.ParentID == 0 ) return EmbeddedPosition.undefined;
                el = rep.GetElementByID(el.ParentID);
            }


                EA.Diagram dia = rep.GetDiagramByID(obj.DiagramID);
            EA.DiagramObject objParent = dia.GetDiagramObjectByID(elParent.ElementID, "");
            EA.DiagramObject objFirstEmbedded = dia.GetDiagramObjectByID(el.ElementID, "");
            if (objParent == null) return EmbeddedPosition.undefined;

            int horicontalCenter = objFirstEmbedded.left + (objFirstEmbedded.right - objFirstEmbedded.left) / 2;
            int verticalCenter = objFirstEmbedded.top - (objFirstEmbedded.top - objFirstEmbedded.bottom ) / 2;

            if (horicontalCenter < objParent.left + 10 && horicontalCenter > objParent.left -10 ) return EmbeddedPosition.left;
            if (horicontalCenter < objParent.right + 10 && horicontalCenter > objParent.right - 10) return EmbeddedPosition.right;


            if (verticalCenter < objParent.top + 10 && verticalCenter > objParent.top - 10) return EmbeddedPosition.top;
            if (verticalCenter < objParent.bottom + 10 && verticalCenter > objParent.bottom - 10) return EmbeddedPosition.bottom;
            return EmbeddedPosition.undefined;

        }

        /// <summary>
        /// Embedded position is an extension method to see on with edge it is located
        /// </summary>
        public enum EmbeddedPosition
        {
            left,
            right,
            top,
            bottom,
            undefined
        }
    }
}
