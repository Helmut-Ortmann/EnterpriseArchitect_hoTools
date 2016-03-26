using System.Collections.Generic;
using System.Windows.Forms;
using EA;


namespace hoTools.Utils.SQL
{
    public class SqlTemplates
    {
        #region Template Dictionary SqlTemplare
        static Dictionary<SQL_TEMPLATE_ID, SqlTemplate> SqlTemplate = new Dictionary<SQL_TEMPLATE_ID, SqlTemplate>
        {
             { SQL_TEMPLATE_ID.ELEMENT_TEMPLATE,
                new SqlTemplate(@"Element Template",
                "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                "from t_object o\r\n" +
                "where o.object_type in \r\n"+
                "     (\r\n"+
                "      \"Class\",\"Component\"\r\n" +
                "      )\r\n",
                     "Template to search for Elements") },
             { SQL_TEMPLATE_ID.DIAGRAM_TEMPLATE,
                new SqlTemplate(@"Diagram Template",
                     "select d.ea_guid AS CLASSGUID, d.diagram_type AS CLASSTYPE,d.Name AS Name,d.diagram_type As Type, * \r\n" +
                     "from t_diagram d\r\n" +
                     "where d.diagram_type in \r\n" +
                      "    (\r\n" +
                      "      \"Activity\",\"Analysis\",\"Collaboration\",\"Component\",\"CompositeStructure\", \"Custom\",\"Deployment\",\"Logical\",\r\n"+
                     "      \"Object\",\"Package\",  \"Sequence\",\"Statechart\",\"Timing\", \"UMLDiagram\", \"Use Case\"\r\n"+
                     "    )",
                    @"Template to search for Diagrams") },
             { SQL_TEMPLATE_ID.PACKAGE_TEMPLATE,
                new SqlTemplate("Package Template",
                           "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                           "from t_object o, t_package pkg \r\n" +
                           "where o.object_type = 'Package' AND \r\n" +
                           "      o.ea_guid = pkg.ea_guid",
                    @"Template to search for Packages") },
             { SQL_TEMPLATE_ID.DIAGRAM_OBJECT_TEMPLATE,
                new SqlTemplate(@"Diagram Object Template", 
                    "<Search Term>",
                    @"Template to search Diagram Object Packages") },
             { SQL_TEMPLATE_ID.ATTRIBUTE_TEMPLATE,
                new SqlTemplate(@"Attribute Template",
                    "select o.ea_guid AS CLASSGUID, 'Attribute' AS CLASSTYPE,o.Name AS Name, * \r\n" +
                    "from t_attribute o\r\n" +
                    "where o.name like '%' ",
                    "Template to search for Attributes") },
             { SQL_TEMPLATE_ID.OPERATION_TEMPLATE,
                new SqlTemplate(@"OPERATION Template",
                      "select o.ea_guid AS CLASSGUID, 'Operation' AS CLASSTYPE,o.Name AS Name, * \r\n" +
                      "from t_operation o\r\n" +
                      "where o.name like '%' ",
                    @"Template to search for Operations") },
            { SQL_TEMPLATE_ID.SEARCH_TERM,
                new SqlTemplate(@"SEARCH_TERM", "<Search Term>","<Search Term> is a filed replaced at runtime by a variable string.") },
            { SQL_TEMPLATE_ID.PACKAGE,
                new SqlTemplate(@"PACKAGE", @"#Package#",@"Placeholder for the current selected package, use as PackageID\n pkg.Package_ID = #Package# ") },
            { SQL_TEMPLATE_ID.BRANCH,
                new SqlTemplate(@"BRANCH", @"#Branch#",@"Placeholder for the current selected package (recursive), use as PackageID\n pkg.Package_ID in (#Branch# ") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_ID,
                new SqlTemplate(@"CURRENT_ITEM_ID", @"#CurrentElementID#",@"Placeholder for the current selected item ID, use as ID\n obj.Object_ID = #CurrentElementID# ") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_GUID,
                new SqlTemplate( "CURRENT_ITEM_GUID", "#CurrentElementGUID#",@"Placeholder for the current selected item GUID, use as GUID\n obj.ea_GUID = #CurrentElementGUID# ") },
            { SQL_TEMPLATE_ID.WC,
                new SqlTemplate(@"DB Wild Card", @"#WC#", @"Placeholder for the Database Wild Card (% or *)\n like 'MyClass#WC#' ") },
            { SQL_TEMPLATE_ID.NOW,
                new SqlTemplate(@"NOW date/time", @"#NOW(not implemented)#", @"Placeholder date/time, not yet implemented ") },
            { SQL_TEMPLATE_ID.AUTHOR,
                new SqlTemplate(@"Author",@"#Author(not implemented)#",@"Author, Takes the name from the 'Author' field in the 'Options' dialog 'General' page.") }
        };
        #endregion
        #region public Enum SQL_TEMPLATE_ID
        public enum SQL_TEMPLATE_ID
        {
            ELEMENT_TEMPLATE,
            DIAGRAM_TEMPLATE,
            PACKAGE_TEMPLATE,
            DIAGRAM_OBJECT_TEMPLATE,
            ATTRIBUTE_TEMPLATE,
            OPERATION_TEMPLATE,
            SEARCH_TERM,
            PACKAGE,
            BRANCH,
            CURRENT_ITEM_ID,
            CURRENT_ITEM_GUID,
            AUTHOR,
            NOW,
            WC
        }
        #endregion
        /// <summary>
        /// Get Template or null according to templateID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static string getTemplate(SQL_TEMPLATE_ID templateID)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateID, out template))
            {
                return template.Template;
            }
            else {
                MessageBox.Show("ID={templateID}", "Invalid templateID");
                return null;
            }
   
        }
        /// <summary>
        /// Get Template Tool tip or null according to templateID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static string getTooltip(SQL_TEMPLATE_ID templateID)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateID, out template))
            {
                return template.ToolTip;
            }
            else {
                MessageBox.Show("ID={templateID}", "Invalid templateID");
                return null;
            }

        }
        /// <summary>
        /// Get Template name or null according to templateID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static string getTemplateName(SQL_TEMPLATE_ID templateID)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateID, out template))
            {
                return template.TemplateName;
            }
            else {
                MessageBox.Show("ID={templateID}", "Invalid templateID");
                return null;
            }

        }



        public static string replaceSearchTerm(Repository rep, string toUpdate, string searchTerm)
        {
            string sql = toUpdate.Replace(getTemplate(SQL_TEMPLATE_ID.SEARCH_TERM), searchTerm);


            // replace ID
            string currentIdTemplate = getTemplate(SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            if (sql.Contains(currentIdTemplate))
            {
                EA.ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.ElementID;
                        break;
                    case EA.ObjectType.otDiagram:
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        id = dia.DiagramID;
                        break;
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                }
                sql = sql.Replace(currentIdTemplate, $"{id}");
            }
            // replace GUID
            // replace ID
            string currentGuidTemplate = getTemplate(SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            if (sql.Contains(currentGuidTemplate))
            {
                EA.ObjectType objectType = rep.GetContextItemType();
                string guid = "";
                switch (objectType)
                {
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        guid = el.ElementGUID;
                        break;
                    case EA.ObjectType.otDiagram:
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        guid = dia.DiagramGUID;
                        break;
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        guid = pkg.PackageGUID;
                        break;
                }
                sql = sql.Replace(currentGuidTemplate, guid);
            }


            return sql;
             
        }
    }
}
