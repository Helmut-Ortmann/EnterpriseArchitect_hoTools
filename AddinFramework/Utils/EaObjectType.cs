using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;

namespace EAAddinFramework.Utils
{
    public static class EaObjectType
    {
        // Dictionary of SQL Types
        // only use unambiguous types
        // !!!!Package may be a Package as well as a Diagram!!!!!!
        static readonly Dictionary<string, EA.ObjectType> EaObjectTypes = new Dictionary<string, EA.ObjectType>
        {
            { "Action",EA.ObjectType.otElement},
            { "ActionPin",EA.ObjectType.otElement},
            { "ActivityParameter",EA.ObjectType.otElement},
            { "ActivityPartition",EA.ObjectType.otElement},
            { "Actor",EA.ObjectType.otElement},
            { "Boundary",EA.ObjectType.otElement},
            { "CentralBufferNode",EA.ObjectType.otElement},

            { "Change",EA.ObjectType.otElement},
            { "Class",EA.ObjectType.otElement},
            { "Collaboration",EA.ObjectType.otElement},
            { "CollaborationOccurence",EA.ObjectType.otElement},
            //{ "Component",EA.ObjectType.otElement},
            { "Decision",EA.ObjectType.otElement},
            { "DiagramFrame",EA.ObjectType.otElement},

            { "Attribute",EA.ObjectType.otAttribute},
            { "Operation",EA.ObjectType.otMethod},
            { "Analysis",EA.ObjectType.otDiagram},
            { "CompositeStruncture",EA.ObjectType.otDiagram},
            { "Custom",EA.ObjectType.otDiagram},
            { "Deployment",EA.ObjectType.otDiagram},
            { "InteractionOverview",EA.ObjectType.otDiagram},
            { "InteractionLogical",EA.ObjectType.otDiagram},
            { "Statechart",EA.ObjectType.otDiagram},
            { "Timing",EA.ObjectType.otDiagram},
            { "Use Case",EA.ObjectType.otDiagram}

          };


        /// <summary>
        /// Extension Method EA.Repository to get:
        /// - EA Object
        /// - EA.ObjectType
        /// from Object Type in Table and GUID
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sqlObjectType"></param>
        /// <param name="guid"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object GetEaObject(this EA.Repository rep, string  sqlObjectType, string guid, out EA.ObjectType objectType)
        {

            objectType = EA.ObjectType.otNone;

            // eaObjectType found in dictionary
            if ( EaObjectTypes.TryGetValue(sqlObjectType, out EA.ObjectType eaObjectType) )
            {
                switch (eaObjectType)
                {
                    case EA.ObjectType.otElement:
                        objectType = eaObjectType;
                        return rep.GetElementByGuid(guid);
                    case EA.ObjectType.otDiagram:
                        objectType = eaObjectType;
                        return (object)rep.GetDiagramByGuid(guid);
                    case EA.ObjectType.otPackage:
                        objectType = eaObjectType;
                        return rep.GetPackageByGuid(guid);
                    case EA.ObjectType.otAttribute:
                        objectType = eaObjectType;
                        return rep.GetAttributeByGuid(guid);
                    case EA.ObjectType.otMethod:
                        objectType = eaObjectType;
                        return rep.GetMethodByGuid(guid);
                    case EA.ObjectType.otConnector:
                        objectType = eaObjectType;
                        return rep.GetConnectorByGuid(guid);
                    default:
                        break;
                }

            } else {
                // by SQL
                string where = $"where ea_guid = '{ guid}'";
                string sql = $"select 'OBJECT'  as object_type from t_object  {where}      UNION " +
                             $"select 'DIAGRAM'                from t_diagram {where}            ";
                XElement x = XElement.Parse(rep.SQLQuery(sql));
                var oType = (from t in x.Descendants("object_type")
                             select t).FirstOrDefault();
                if (oType == null )
                {
                    MessageBox.Show($"GUID:'{guid}'", @"GUID not found, Break!!!!");
                    return null;
                }
                string type = oType.Value;
                switch (type)
                {
                    case "OBJECT":
                        objectType = EA.ObjectType.otElement;
                        return rep.GetElementByGuid(guid);
                    case "DIAGRAM":
                        objectType = EA.ObjectType.otDiagram;
                        return rep.GetDiagramByGuid(guid);
                    default:
                        MessageBox.Show($"GUID searched in object, diagram:'{guid}'", @"GUID not found in Repository, Break!!!!");
                        return null;

                }
            }
            return null;
           
        }

    }
}
