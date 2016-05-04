using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using EA;

using hoTools.Utils;


namespace hoTools.Utils.SQL
{
    public static class SqlTemplates
    {
        #region Template Dictionary SqlTemplare
        static Dictionary<SQL_TEMPLATE_ID, SqlTemplate> SqlTemplate = new Dictionary<SQL_TEMPLATE_ID, SqlTemplate>
        {
             { SQL_TEMPLATE_ID.ELEMENT_TEMPLATE,
                new SqlTemplate("Element Template",
                "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                "from t_object o\r\n" +
                "where o.object_type in \r\n"+
                "     (\r\n"+
                "      \"Class\",\"Component\"\r\n" +
                "      )\r\n" +
                "ORDER BY o.Name\r\n",
                     "Template to search for Elements") },
              { SQL_TEMPLATE_ID.ELEMENT_TYPE_TEMPLATE,
                new SqlTemplate("Element Type Template Text",
                "select o.object_type As Type, count(*) As Count \r\n" +
                "from t_object o\r\n" +
                "GROUP BY o.object_type\r\n" +
                "ORDER BY 1,2\r\n",
                     "Template to display all Element Types and their counts") },

               { SQL_TEMPLATE_ID.DIAGRAM_TYPE_TEMPLATE,
                new SqlTemplate("Diagram Type Template ",
                "select d.diagram_type As Type, count(*) As Count\r\n" +
                "from t_diagram d\r\n" +
                "GROUP BY d.diagram_type\r\n" +
                "ORDER BY 1,2\r\n",
                     "Template to display all Diagram Types and their counts") },

             { SQL_TEMPLATE_ID.DIAGRAM_TEMPLATE,
                new SqlTemplate("Diagram TemplateText",
                     "select d.ea_guid AS CLASSGUID, d.diagram_type AS CLASSTYPE,d.Name AS Name,d.diagram_type As Type, * \r\n" +
                     "from t_diagram d\r\n" +
                     "where d.diagram_type in \r\n" +
                      "    (\r\n" +
                      "      \"Activity\",\"Analysis\",\"Collaboration\",\"Component\",\"CompositeStructure\", \"Custom\",\"Deployment\",\"Logical\",\r\n"+
                     "      \"Object\",\"Package\",  \"Sequence\",\"Statechart\",\"Timing\", \"UMLDiagram\", \"Use Case\"\r\n"+
                     "    )\r\n" +
                     "ORDER BY d.Name\r\n",
                   "TemplateText to search for Diagrams") },
             { SQL_TEMPLATE_ID.PACKAGE_TEMPLATE,
                new SqlTemplate("Package TemplateText",
                           "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                           "from t_object o, t_package pkg \r\n" +
                           "where o.object_type = 'Package' AND \r\n" +
                           "      o.ea_guid = pkg.ea_guid \r\n" +
                           "ORDER BY o.Name\r\n",
                    "TemplateText to search for Packages") },
             { SQL_TEMPLATE_ID.DIAGRAM_OBJECT_TEMPLATE,
                new SqlTemplate(@"Diagram Object TemplateText", 
                    "<Search Term>",
                    "TemplateText to search Diagram Object Packages") },
             { SQL_TEMPLATE_ID.ATTRIBUTE_TEMPLATE,
                new SqlTemplate(@"Attribute TemplateText",
                    "select o.ea_guid AS CLASSGUID, 'Attribute' AS CLASSTYPE,o.Name AS Name, * \r\n" +
                    "from t_attribute o\r\n" +
                    "where o.name like '%' ",
                    "TemplateText to search for Attributes") },
             { SQL_TEMPLATE_ID.OPERATION_TEMPLATE,
                new SqlTemplate(@"OPERATION TemplateText",
                      "select o.ea_guid AS CLASSGUID, 'Operation' AS CLASSTYPE,o.Name AS Name, * \r\n" +
                      "from t_operation o\r\n" +
                      "where o.name like '%' ",
                    "TemplateText to search for Operations") },
             //--------------------------------------------------------------------------------
             // Insert Macros
            { SQL_TEMPLATE_ID.SEARCH_TERM,
                new SqlTemplate("SEARCH_TERM", 
                    "<Search Term>",
                    "<Search Term> is a string replaced at runtime by a variable string.\nExample: Name = '<Search Term>'") },
            { SQL_TEMPLATE_ID.PACKAGE_ID,
                new SqlTemplate("PACKAGE_ID", 
                    "#Package#",
                    "Placeholder for the current selected package, use as PackageID\nExample: pkg.Package_ID = #Package# ") },
            { SQL_TEMPLATE_ID.BRANCH_IDS,
                new SqlTemplate("BRANCH_IDS", 
                    "#Branch#",
                    "Placeholder for the current selected package (recursive), use as PackageID\nExample: pkg.Package_ID in (#Branch#)\nExpands to in (512,513,..) ")  },
            { SQL_TEMPLATE_ID.IN_BRANCH_IDS,
                new SqlTemplate("IN_BRANCH_IDS", 
                    "#InBranch#",
                    "Placeholder for sql in clause for the current selected package (recursive), use as PackageID\nExample: pkg.Package_ID  (#InBranch#)\nExpands to in (512,513,..) ") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_ID,
                new SqlTemplate("CURRENT_ITEM_ID", 
                    "#CurrentElementID#","Placeholder for the current selected item ID, use as ID\nExample: obj.Object_ID = #CurrentElementID# ") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_GUID,
                new SqlTemplate( "CURRENT_ITEM_GUID",
                    "#CurrentElementGUID#",
                    "Placeholder for the current selected item GUID, use as GUID\nExample: obj.ea_GUID = #CurrentElementGUID# ") },
            { SQL_TEMPLATE_ID.WC,
                new SqlTemplate("DB Wild Card", 
                    "#WC#", 
                    "Placeholder for the Database Wild Card (% or *)\nExample like 'MyClass#WC#' ") },
            { SQL_TEMPLATE_ID.NOW,
                new SqlTemplate("NOW date/time", 
                    "#NOW(not implemented)#", 
                    "Placeholder date/time, not yet implemented ") },
            { SQL_TEMPLATE_ID.AUTHOR,
                new SqlTemplate("Author",
                    "#Author(not implemented)#",
                    "Author, Takes the name from the 'Author' field in the 'Options' dialog 'General' page.") }
        };
        #endregion
        #region public Enum SQL_TEMPLATE_ID
        public enum SQL_TEMPLATE_ID
        {
            ELEMENT_TEMPLATE,
            ELEMENT_TYPE_TEMPLATE,
            DIAGRAM_TEMPLATE,
            DIAGRAM_TYPE_TEMPLATE,
            PACKAGE_TEMPLATE,
            DIAGRAM_OBJECT_TEMPLATE,
            ATTRIBUTE_TEMPLATE,
            OPERATION_TEMPLATE,
            SEARCH_TERM,
            PACKAGE_ID,
            BRANCH_IDS,
            IN_BRANCH_IDS,
            CURRENT_ITEM_ID,
            CURRENT_ITEM_GUID,
            AUTHOR,
            NOW,
            WC
        }
        #endregion

        /// <summary>
        /// Get TemplateText
        /// - TemplateString
        /// - Name
        /// - ToolTip
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static SqlTemplate getTemplate(SQL_TEMPLATE_ID templateID)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateID, out template))
            {
                return template;
            }
            else {
                MessageBox.Show("ID={templateID}", "Invalid templateID");
                return null;
            }

        }
        /// <summary>
        /// Get TemplateText or null according to templateID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static string getTemplateText(SQL_TEMPLATE_ID templateID)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateID, out template))
            {
                return template.TemplateText;
            }
            else {
                MessageBox.Show("ID={templateID}", "Invalid templateID");
                return null;
            }
   
        }
        /// <summary>
        /// Get TemplateText Tool tip or null according to templateID
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
        /// Get TemplateText name or null according to templateID
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


        /// <summary>
        /// Replace Macro by value. Possible Macros are: Search Term, ID, GUID, Package ID, Branch ID. 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sqlString">The complete SQL string</param>
        /// <param name="searchTerm">The Search Term from the text entry field</param>
        /// <returns></returns>
        public static string replaceMacro(Repository rep, string sqlString, string searchTerm)
        {
            // <Search Term>
            string sql = sqlString.Replace(getTemplateText(SQL_TEMPLATE_ID.SEARCH_TERM), searchTerm);


            // replace ID
            string currentIdTemplate = getTemplateText(SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
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
            string currentGuidTemplate = getTemplateText(SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
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
            // Package ID
            string currentPackageTemplate = getTemplateText(SQL_TEMPLATE_ID.PACKAGE_ID);
            if (sql.Contains(currentPackageTemplate))
            {
                EA.ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    case EA.ObjectType.otDiagram:
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        id = dia.PackageID;
                        break;
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.PackageID;
                        break;
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                }
                sql = sql.Replace(currentPackageTemplate, $"{id}");
            }
            // Branch=Package IDs, Recursive
            string currentBranchTemplate = getTemplateText(SQL_TEMPLATE_ID.BRANCH_IDS);
            if (sql.Contains(currentBranchTemplate))
            {
                EA.ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    case EA.ObjectType.otDiagram:
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        id = dia.PackageID;
                        break;
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.PackageID;
                        break;
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                }
                if (id > 0)
                {
                    string branch = Package.getBranch(rep, "", id);
                    sql = sql.Replace(currentBranchTemplate, branch);
                }
            }

            // delete Comments
            return deleteC_Comments(sql);
             
        }
        #region deleteC_Comments 
        /// <summary>
        /// Delete C-Comment
        /// </summary>
        /// <param name="sql">SQL to delete C-Comments in </param>
        /// <returns></returns>
        static string deleteC_Comments(string sql)
        {
            //s = Regex.Replace(s, @"/\*[^\n]*\*/", "", RegexOptions.Singleline);
            // ? for greedy behavior (find shortest matching string)
            string s = Regex.Replace(sql, @"/\*.*?\*/", "", RegexOptions.Singleline);
            // delete comments //....
            s = Regex.Replace(s, @"//[^\n]*\n", "\r\n");
            // delete comments /*....
            s = Regex.Replace(s, @"/\*[^\n]*\n", "\r\n");
            // delete empty lines
            s = Regex.Replace(s, "(\r\n){2,100}", "\r\n");
            return s;
        }
        #endregion
    }
}
