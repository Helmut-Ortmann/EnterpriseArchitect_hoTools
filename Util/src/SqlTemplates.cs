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
        /// <summary>
        /// Dictionary of the available Templates
        /// </summary>
        static Dictionary<SQL_TEMPLATE_ID, SqlTemplate> SqlTemplate = new Dictionary<SQL_TEMPLATE_ID, SqlTemplate>
        {
             { SQL_TEMPLATE_ID.MACROS_HELP,
                new SqlTemplate("Macros help",
                "//\r\n" +
                "// Help to available Macros\r\n" +
                "// - #Branch#                    Replaced by the package ID of the selected package and all nested package like '512, 31,613' \r\n" +
                "// - #CONNECTOR_ID#              Selected Connector, Replaced by ConnectorID\r\n" +
                "// - #CONVEYED_ITEM_IDS#         Selected Connector, Replaced by the Conveyed Items as comma separated list of ElementIDs\r\n" +
                "// - #CurrentElementGUID#        Alias for #CurrentItemGUID# (compatible to EA)\r\n" +
                "// - #CurrentElementID#          Alias for #CurrentItemID# (compatible to EA)\r\n" +
                "// - #CurrentItemGUID#           Replaced by the GUID of the selected item (Element, Diagram, Package, Attribute, Operation) \r\n" +
                "// - #CurrentItemID#             Replaced by the ID of the selected item (Element, Diagram, Package, Attribute, Operation)\r\n" +
                "// - #DiagramElements_IDS#         Diagram Elements of selected Diagram / current Diagram\r\n"+
                "// - #DiagramSelectedElements_IDS# Selected Diagram Elements of selected Diagram / current Diagram \r\n" +
                "// - #InBranch#                  like Branch (nested package recursive), but with SQL IN clause like 'IN (512, 31,613)'\r\n" +
                "// - #Package#                   Replaced by the package ID of the containing package of selected Package, Element, Diagram, Operation, Attribute\r\n" +
                "// - #TREE_SELECTED_GUIDS#       In Browser selected Elements as a list of comma separated GUIDS like 'IN (#TREE_SELECTED_GUIDS#)'\r\n" +
                "// - <Search Term>               Replaced by the string in the 'Search Term' entry field\r\n" +
                "// - #WC#  or *                  Wild card depending of the current DB. You may simple use '*'\r\n" +
                "// - #DB=ACCESS2007#             DB specif SQL for ACCESS2007\r\n"+
                "// - #DB=ASA#                    DB specif SQL for ASA\r\n"+
                "// - #DB=FIREBIRD#               DB specif SQL for FIREBIRD\r\n"+
                "// - #DB=JET#                    DB specif SQL for JET\r\n"+
                "// - #DB=MYSQL#                  DB specif SQL for My SQL\r\n"+
                "// - DB=ORACLE#                  DB specif SQL for Oracle\r\n"+
                "// - #DB=POSTGRES#               DB specif SQL for POSTGRES\r\n"+
                "// - #DB=SQLSVR#                 DB specif SQL for SQL Serve\r\n"+

                "//\r\n" +
                "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                "from t_object o\r\n" +
                "where o.name like '<Search Term>#WC#' AND "+
                "     o.object_type in \r\n"+
                "     (\r\n"+
                "      \"Class\",\"Component\"\r\n" +
                "      )\r\n" +
                "ORDER BY o.Name\r\n",
                     "Template available Macros") },
             { SQL_TEMPLATE_ID.ELEMENT_TEMPLATE,
                new SqlTemplate("Element Template",
                "//\r\n" +
                "// Template Element\r\n" +
                "//\r\n" +
                "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                "from t_object o\r\n" +
                "where o.name like '<Search Term>#WC#' AND "+
                "     o.object_type in \r\n"+
                "     (\r\n"+
                "      \"Class\",\"Component\"\r\n" +
                "      )\r\n" +
                "ORDER BY o.Name\r\n",
                     "Template to search for Elements") },
              { SQL_TEMPLATE_ID.ELEMENT_TYPE_TEMPLATE,
                new SqlTemplate("Element Type Template Text",
                "//\r\n" +
                "// Template Element types\r\n" +
                "// - Group by \r\n" +
                "// - Aggregate function: count(*) \r\n" +
                "//\r\n" +
                "//\r\n" +
                "select o.object_type As Type, count(*) As Count \r\n" +
                "from t_object o\r\n" +
                "GROUP BY o.object_type\r\n" +
                "ORDER BY 1,2\r\n",
                     "Template to display all Element Types and their counts") },

               { SQL_TEMPLATE_ID.DIAGRAM_TYPE_TEMPLATE,
                new SqlTemplate("Diagram Type Template ",
                "//\r\n" +
                "// Template Diagram types\r\n" +
                "// - Group by \r\n" +
                "// - Aggregate function: count(*) \r\n" +
                "//\r\n" +
                "//\r\n" +
                "select d.diagram_type As Type, count(*) As Count\r\n" +
                "from t_diagram d\r\n" +
                "GROUP BY d.diagram_type\r\n" +
                "ORDER BY 1,2\r\n",
                     "Template to display all Diagram Types and their counts") },

             { SQL_TEMPLATE_ID.DIAGRAM_TEMPLATE,
                new SqlTemplate("Diagram TemplateText",
                "//\r\n" +
                "// Template Diagram of type\r\n" +
                "//\r\n" +
                "//\r\n" +
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
                           "//\r\n" +
                            "// Template Package with\r\n" +
                            "// join between \r\n" +
                            "// - t_object\r\n" +
                            "// - t_package\r\n" +
                            "//\r\n" +
                            "//\r\n" +
                           "select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE,o.Name AS Name,o.object_type As Type, * \r\n" +
                           "from t_object o, t_package pkg \r\n" +
                           "where o.object_type = 'Package' AND \r\n" +
                           "      o.ea_guid = pkg.ea_guid \r\n" +
                           "ORDER BY o.Name\r\n",
                    "TemplateText to search for Packages") },
             { SQL_TEMPLATE_ID.DIAGRAM_OBJECT_TEMPLATE,
                new SqlTemplate(@"Diagram Object TemplateText",
                    "//\r\n" +
                    "// Template Diagram Object with\r\n" +
                    "//  \r\n" +
                    "<Search Term>",
                    "TemplateText to search Diagram Object Packages") },
             { SQL_TEMPLATE_ID.ATTRIBUTE_TEMPLATE,
                new SqlTemplate(@"Attribute TemplateText",
                     "//\r\n" +
                    "// Template Attribute\r\n" +
                    "//\r\n" +
                    "select o.ea_guid AS CLASSGUID, 'Attribute' AS CLASSTYPE,o.Name AS Name, * \r\n" +
                    "from t_attribute o\r\n" +
                    "where o.name like '%' ",
                    "TemplateText to search for Attributes") },
             { SQL_TEMPLATE_ID.OPERATION_TEMPLATE,
                new SqlTemplate(@"OPERATION TemplateText",
                     "//\r\n" +
                      "// Template Operation\r\n" +
                     "//\r\n" +
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
            { SQL_TEMPLATE_ID.CONNECTOR_ID,
                new SqlTemplate("CONNECTOR_ID",
                    "#CONNECTOR_ID#",
                    "Placeholder for the current selected connector ID\nExample: ConnectorID = #CONNECTOR_ID# ") },
             { SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS,
                new SqlTemplate("CONVEDYED_ITEM_IDS",
                    "#CONVEYED_ITEM_IDS#",
                    "Placeholder for the current conveyed item IDs as comma separated list\nExample: elementID in (#CONVEYED_ITEM_IDS#)") },
              { SQL_TEMPLATE_ID.TREE_SELECTED_GUIDS,
                new SqlTemplate("TREE_SELECTED_GUIDS",
                    "#TREE_SELECTED_GUIDS#",
                    "Placeholder for selected Browser Elements  as comma separated list of GUIDs\nExample: eaGUID in (#TREE_SELECTED_GUIDS#)") },
            { SQL_TEMPLATE_ID.PACKAGE_ID,
                new SqlTemplate("PACKAGE_ID",
                    "#Package#",
                    "Placeholder for the package the selected Item (Package, Diagram, Element, Operation, Attribute) is contained, use as PackageID\nExample: pkg.Package_ID = #Package# ") },
            { SQL_TEMPLATE_ID.BRANCH_IDS,
                new SqlTemplate("BRANCH_IDS",
                    "#Branch#",
                    "Placeholder for the current selected package (recursive), use as PackageID\nExample: pkg.Package_ID in (#Branch#)\nExpands to in '512,513,..' ")  },
            { SQL_TEMPLATE_ID.IN_BRANCH_IDS,
                new SqlTemplate("IN_BRANCH_IDS",
                    "#InBranch#",
                    "Placeholder for sql in clause for the current selected package (recursive), use as PackageID\nExample: pkg.Package_ID  (#InBranch#)\nExpands to 'IN (512,513,..)' ") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_ID,
                new SqlTemplate("CURRENT_ITEM_ID",
                    "#CurrentItemID#","Placeholder for the current selected item ID (Element, Package, Diagram, Attribute, Operation, may be ambiguous, GUID is more secure), use as ID\nExample: obj.Object_ID = #CurrentItemID#; Alias: #CurrentElementID#") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_GUID,
                new SqlTemplate( "CURRENT_ITEM_GUID",
                    "#CurrentItemGUID#",
                    "Placeholder for the current selected item GUID (Element, Package, Diagram, Attribute, Operation), use as GUID\nExample: obj.ea_GUID = #CurrentItemGUID#; Alias: #CurrentElementGUID# ") },

            { SQL_TEMPLATE_ID.DiagramSelectedElements_IDS,
                new SqlTemplate( "DiagramSelectedElements_IDS",
                    "#DiagramSelectedElements_IDS#",
                    "Placeholder for the current selected items in the diagram, as ID\nExample: elementID in (#DiagramSelectedElements_IDS# ") },

            { SQL_TEMPLATE_ID.DiagramElements_IDS,
                new SqlTemplate( "DiagramElements_IDS",
                    "#DiagramElements_IDS#",
                    "Placeholder for the diagram Elements in the selected diagram, as ID\nExample: elementID in (#DiagramElements_IDS# ") },







            { SQL_TEMPLATE_ID.WC,
                new SqlTemplate("DB Wild Card", 
                    "#WC#", 
                    "Placeholder for the Database Wild Card (% or *)\r\n"+
                    "Example like 'MyClass#WC#'\r\n " +
                    "Remark: Just for compatibility with EA Searches.You may simple use the familiar wild cards '*' or '%'\r\n " +
                    "        hoTools take care of the correct usage for the current database\r\n "
                    ) },
            { SQL_TEMPLATE_ID.NOW,
                new SqlTemplate("NOW date/time", 
                    "#NOW(not implemented)#", 
                    "Placeholder date/time, not yet implemented ") },
            { SQL_TEMPLATE_ID.AUTHOR,
                new SqlTemplate("Author",
                    "#Author(not implemented)#",
                    "Author, Takes the name from the 'Author' field in the 'Options' dialog 'General' page.") },
            { SQL_TEMPLATE_ID.DB_MYSQL,
               new SqlTemplate("DB_MYSQL",
                    "#DB=MYSQL#                             #DB=MYSQL#",
                    "The SQL string for other DBs included by #DB=MYSQL#, #DB=MYSQL#     ...my db sql....#DB=MYSQL#") },
            { SQL_TEMPLATE_ID.DB_JET,
                new SqlTemplate("DB_JET",
                    "#DB=JET#                             #DB=JET#",
                    "The SQL string for other DBs included by #DB=JET#, #DB=JET#     ...my db sql....#DB=JET#") },
            { SQL_TEMPLATE_ID.DB_ORACLE,
                new SqlTemplate("DB_ORACLE",
                    "#DB=ORACLE#                             #DB=ORACLE#",
                    "The SQL string for other DBs included by #DB=ORACLE#, #DB=ORACLE#     ...my db sql....#DB=ORACLE#") },
            { SQL_TEMPLATE_ID.DB_SQLSVR,
                new SqlTemplate("DB_SQLSVR",
                    "#DB=SQLSVR#                             #DB=SQLSVR#",
                    "The SQL string for other DBs included by #DB=SQLSVR#, #DB=SQLSVR#     ...my db sql....#DB=SQLSVR#") },
            { SQL_TEMPLATE_ID.DB_ASA,
                new SqlTemplate("DB_ASA",
                    "#DB=ASA#                             #DB=ASA#",
                    "The SQL string for other DBs included by #DB=ASA#, #DB=ASA#     ...my db sql....#DB=ASA#") },
            { SQL_TEMPLATE_ID.DB_POSTGRES,
                new SqlTemplate("DB_POSTGRES",
                    "#DB=POSTGRES#                             #DB=POSTGRES#",
                    "The SQL string for other DBs included by #DB=POSTGRES#, #DB=POSTGRES#     ...my db sql....#DB=POSTGRES#") },
            { SQL_TEMPLATE_ID.DB_ACCESS2007,
                new SqlTemplate("DB_ACCESS2007",
                    "#DB=ACCESS2007#                             #DB=ACCESS2007#",
                    "The SQL string for other DBs included by #DB=ACCESS2007#, #DB=ACCESS2007#     ...my db sql....#DB=ACCESS2007#") },
             { SQL_TEMPLATE_ID.DB_FIREBIRD,
                new SqlTemplate("DB_FIREBIRD",
                    "#DB=FIREBIRD#                             #DB=FIREBIRD#",
                    "The SQL string for other DBs included by #DB=FIREBIRD#, #DB=FIREBIRD#     ...my db sql....#DB=FIREBIRD#") }


        };
        #endregion
        #region public Enum SQL_TEMPLATE_ID
        /// <summary>
        /// Available Templates
        /// </summary>
        public enum SQL_TEMPLATE_ID
        {
            DB_OTHER,           // Macros for special DBs
            DB_ACCESS2007,
            DB_ORACLE,
            DB_ASA,
            DB_SQLSVR,
            DB_MYSQL,
            DB_POSTGRES,
            DB_FIREBIRD,
            DB_JET,

            ELEMENT_TEMPLATE,
            ELEMENT_TYPE_TEMPLATE,
            DIAGRAM_TEMPLATE,
            DIAGRAM_TYPE_TEMPLATE,
            PACKAGE_TEMPLATE,
            DIAGRAM_OBJECT_TEMPLATE,
            ATTRIBUTE_TEMPLATE,
            OPERATION_TEMPLATE,
            SEARCH_TERM,
            //-------------------------
            // macros
            MACROS_HELP,        // Help to macros
            CONNECTOR_ID,   // Get's the connector
            CONVEYED_ITEM_IDS, // Get's the conveyed Items of the connector as a comma separated ID list of elementIDs
            PACKAGE_ID,      // The containing package of Package, Diagram, Element, Attribute, Operation
            BRANCH_IDS,     // Package (nested, recursive) ids separated by ','  like '20,21,47,1'
            IN_BRANCH_IDS,  // Package (nested, recursive), complete SQL in clause, ids separated by ','  like 'IN (20,21,47,1)', just a shortcut for #BRANCH_ID#
            CURRENT_ITEM_ID,     // ALIAS CURRENT_ELEMENT_ID exists (compatible to EA)
            CURRENT_ITEM_GUID,  // ALIAS CURRENT_ELEMENT_GUID exists (compatible to EA)
            DiagramSelectedElements_IDS,// Selected Diagram objects as a comma separated list of IDs
            DiagramElements_IDS,        // Diagram objects (selected diagram) as a comma separated list of IDs
            AUTHOR,
            TREE_SELECTED_GUIDS, // get all the GUIDs of the selected items (otDiagram, otElement, otPackage, otAttribute, otMethod
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
                MessageBox.Show($"ID={templateID.ToString()}", "Invalid templateID");
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
        /// Replace Macro by value. Possible Macros are: Search Term, ID, GUID, Package ID, Branch ID,... 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sqlString">The complete SQL string</param>
        /// <param name="searchTerm">The Search Term from the text entry field</param>
        /// <returns>"" if error occurred</returns>
        public static string replaceMacro(Repository rep, string sqlString, string searchTerm)
        {
            // delete Comments
            string sql = deleteC_Comments(sqlString);

            // <Search Term>
            sql = sql.Replace(getTemplateText(SQL_TEMPLATE_ID.SEARCH_TERM), searchTerm);

            // replace #DiagramElements_IDS# by IDs of diagram
            sql = macroDiagramElements_IDS(rep, sql);
            if (sql == "") return "";

            // replace #DiagramSelectedElements_IDS# by IDs of diagram
            sql = macroDiagramSelectedElements_IDS(rep, sql);
            if (sql == "") return "";

            // replace #CurrentItemID# by ID of the selected element
            // Alias: #CurrentElementID#
            sql = macroItem_ID(rep, sql);
            if (sql == "") return "";

            // replace #CurrentItemGUID# by ID of the selected element
            // Alias: #CurrentElementGUID#
            sql = macroItem_GUID(rep, sql);
            if (sql == "") return "";

            // replace #Package# by ID of the package the selected item is contained (Element, Package, Diagram, Attribute, Operation)
            sql = macroPackageID(rep, sql);
            if (sql == "") return "";

            // replace #ConnectorID#, #ConveyedItemIDS# 
            sql = macroConnector(rep, sql);
            if (sql == "") return "";

            // replace #TreeSelectedGUIDS#
            sql = macroTreeSelected(rep, sql);
            if (sql == "") return "";

            // replace #TreeSelectedGUIDS#
            sql = macroBranch(rep, sql);
            if (sql == "") return "";


            // Replace #WC# (DB wile card)
            // Later '*' is changed to the wild card of the current DB
            string currentTemplate = getTemplateText(SQL_TEMPLATE_ID.WC);
            if (sql.Contains(currentTemplate))
            {
                sql = sql.Replace(currentTemplate, "*"); 
            }

            return sql;
            
             
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


        /// <summary>
        /// Replace macro #DiagramElements_IDS# by a comma separated list of all Element IDs in diagram.
        /// <para/>
        /// If no Element is in the diagram it return '0' for an empty list (not existing ID).
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found list of Diagram Element IDs</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroDiagramElements_IDS(Repository rep, string sql) {
            // get template
            string template = getTemplateText(SQL_TEMPLATE_ID.DiagramElements_IDS);

            // template is used
            if (sql.Contains(template))
            {
                // get the diagram
                EA.Diagram dia;
                if (rep.GetContextItemType() == EA.ObjectType.otDiagram)
                {
                    dia = rep.GetContextObject();
                } else
                {
                    dia = rep.GetCurrentDiagram();
                }
                // Diagram selected?
                if (dia == null)
                {
                    MessageBox.Show(sql, $"No Diagram  for '{template}' selected!");
                    sql = "";
                }
                else
                {
                    // make a list of comma separated IDs
                    string listId = "0";
                    foreach (var el in dia.DiagramObjects)
                    {
                        int id = ((EA.DiagramObject)el).ElementID;
                        listId = $"{listId},{id}";
                    }

                    // replace by list of IDs
                    sql = sql.Replace(template, $"{listId}");
                }
                
            }
            return sql;
        }
        /// <summary>
        /// Replace macro #DiagramElements_IDS# by a comma separated list of all Element IDs in diagram.
        /// <para/>
        /// If no Element is in the diagram it return '0' for an empty list (not existing ID).
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found list of Diagram Element IDs</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroDiagramSelectedElements_IDS(Repository rep, string sql)
        {
            // get template
            string template = getTemplateText(SQL_TEMPLATE_ID.DiagramSelectedElements_IDS);

            // template is used
            if (sql.Contains(template))
            {
                // get the diagram
                EA.Diagram dia;
                if (rep.GetContextItemType() == EA.ObjectType.otDiagram)
                {
                    dia = rep.GetContextObject();
                }
                else
                {
                    dia = rep.GetCurrentDiagram();
                }
                // Diagram selected?
                if (dia == null)
                {
                    MessageBox.Show(sql, $"No Diagram  for '{template}' selected!");
                    sql = "";
                }
                else
                {
                    // make a list of comma separated IDs
                    string listId = "0";
                    foreach (var el in dia.SelectedObjects)
                    {
                        int id = ((EA.DiagramObject)el).ElementID;
                        listId = $"{listId},{id}";

                    }

                    // replace by list of IDs
                    sql = sql.Replace(template, $"{listId}");
                }

            }
            return sql;
        }
        /// <summary>
        /// Replace macro #CURRENT_ITEM_ID# by the ID of the currently selected Item.
        /// Because ID is ambiguous better use #CurrentItemGuid#
        /// Alias: #CurrentElementID#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroItem_ID(Repository rep, string sql)
        {
            // replace ID
            string template = getTemplateText(SQL_TEMPLATE_ID.CURRENT_ITEM_ID);
            if (sql.Contains(template) | sql.Contains("#CurrentElementID#"))
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
                    case EA.ObjectType.otAttribute:
                        EA.Attribute attr = (EA.Attribute)rep.GetContextObject();
                        id = attr.AttributeID;
                        break;
                    case EA.ObjectType.otMethod:
                        EA.Method method = (EA.Method)rep.GetContextObject();
                        id = method.MethodID;
                        break;
                }

                if (id > 0)
                {
                    sql = sql.Replace(template, $"{id}");
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No item (Package, Element, Diagram, Operation, Attribute) selected!");
                    sql = "";
                }

            }
            return sql;
        }
        /// <summary>
        /// Replace macro #CurrentItemGUID# by the GUID of the currently selected Item.
        /// Alias: #CurrentElementGUID#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroItem_GUID(Repository rep, string sql)
        {
            // replace ID
            string template = getTemplateText(SQL_TEMPLATE_ID.CURRENT_ITEM_GUID);
            if (sql.Contains(template) | sql.Contains("#CurrentElementGUID#")) 
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
                    case EA.ObjectType.otAttribute:
                        EA.Attribute attr = (EA.Attribute)rep.GetContextObject();
                        guid = attr.AttributeGUID;
                        break;
                    case EA.ObjectType.otMethod:
                        EA.Method method = (EA.Method)rep.GetContextObject();
                        guid = method.MethodGUID;
                        break;
                }

                if (guid != "")
                {
                    sql = sql.Replace(template, $"{guid}");
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No item (Package, Element, Diagram, Operation, Attribute) selected!");
                    sql = "";
                }

            }
            return sql;
        }
        /// <summary>
        /// Replace macro #PackageID# of the selected Diagram, Element, Package, Attribute, Operation by the ID of the containing Package.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroPackageID(Repository rep, string sql)
        {
            // Package ID
            string currentPackageTemplate = getTemplateText(SQL_TEMPLATE_ID.PACKAGE_ID);
            if (sql.Contains(currentPackageTemplate))
            {
                // get Package id
                int id = getParentPackageIdFromContextElement(rep);
                if (id > 0)
                {
                    sql = sql.Replace(currentPackageTemplate, $"{id}");
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No element, diagram, package, attribute, operation selected!");
                    sql = "";
                }

            }
            return sql;
        }

        /// <summary>
        /// Get containing Package ID from context element of Package, Element, Diagram, Attribute, Operation
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        static int getParentPackageIdFromContextElement(Repository rep)
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
                    id = pkg.ParentID;
                    break;
                case EA.ObjectType.otAttribute:
                    EA.Attribute attr = (EA.Attribute)rep.GetContextObject();
                    EA.Element elOfAttr = rep.GetElementByID(attr.ParentID);
                    id = elOfAttr.PackageID;
                    break;
                case EA.ObjectType.otMethod:
                    EA.Method meth = (EA.Method)rep.GetContextObject();
                    EA.Element elOfMeth = rep.GetElementByID(meth.ParentID);
                    id = elOfMeth.PackageID;
                    break;
            }

            return id;
        }

        /// <summary>
        /// Connector macros: ConnectorID and ConveyedItemIDS
        /// <para/>
        /// Replace macro #ConnectorID# by the ID of the selected connector.
        /// <para/>
        /// Replace macro #ConveyedItemIDS# by the IDs of conveyed Elements of the selected connector.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroConnector(Repository rep, string sql)
        {
            //--------------------------------------------------------------------------------------------
            // CONNECTOR ID
            // CONVEYED_ITEM_ID
            string currentConnectorTemplate = getTemplateText(SQL_TEMPLATE_ID.CONNECTOR_ID);
            string currentConveyedItemTemplate = getTemplateText(SQL_TEMPLATE_ID.CONVEYED_ITEM_IDS);

            if ((sql.Contains(currentConnectorTemplate) | sql.Contains(currentConveyedItemTemplate)))
            {
                // connector
                if (rep.GetContextItemType() == EA.ObjectType.otConnector)
                {
                    // connector ID
                    EA.Connector con = rep.GetContextObject();
                    if (sql.Contains(currentConnectorTemplate))
                    {
                        
                        sql = sql.Replace(currentConnectorTemplate, $"{con.ConnectorID}");
                    }
                    // conveyed items are a comma separated list of elementIDs
                    if (sql.Contains(currentConveyedItemTemplate))
                    {
                        // to avoid syntax error
                        string s = "0";
                        foreach (EA.Element el in con.ConveyedItems)
                        {
                            s = $"{s}, {el.ElementID}";
                        }
                        sql = sql.Replace(currentConveyedItemTemplate, $"{s}");
                    }
                } else
                // no connector selected
                {
                    MessageBox.Show(sql, $"No connector selected!");
                    sql = "";
                }
            }
            return sql;
        }


        /// <summary>
        /// Replace macro #TreeSelectedGUIDS# by GUIDs of selected Items of Browser
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroTreeSelected(Repository rep, string sql)
        {
            //--------------------------------------------------------------------------------------------
            // Tree selected items
            // CONVEYED_ITEM_ID
            string template = getTemplateText(SQL_TEMPLATE_ID.TREE_SELECTED_GUIDS);


            if (sql.Contains(template))
            {
                // get the selected elements (Element)
                string GUIDs = "";  
                string GUID = "";
                string comma = "";
                EA.Collection col = rep.GetTreeSelectedElements();
                foreach (var el in col)
                {
                    GUID = ((EA.Element)el).ElementGUID;
                   
                    // make list
                    if (GUID != "")
                    {
                        GUIDs = $"{GUIDs}{comma}'{GUID}'";
                        comma = ", ";
                    }

                }
                // at least one element selected
                if (GUIDs != "")
                {
                    // replace by list of GUIDs
                    sql = sql.Replace(template, $"{GUIDs}");
                }
                else// no element in Browser selected
                {
                    MessageBox.Show(sql, $"No elements in browser of type Element(Class, Activity,..) selected!");
                    sql = "";
                }
            }
            return sql;
        }

        /// <summary>
        /// Replace macro #Branch# an #InBranch# by IDs of selected packages, recursive nested 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string macroBranch(Repository rep, string sql) { 
        // Branch=comma separated Package IDs, Recursive:
        // Example for 3 Packages with their PackageID 7,29,128
        // 7,29,128
        //
        // Branch: complete SQL IN statement ' IN (comma separated Package IDs, Recursive):
        // IN (7,29,128)
            string currentBranchTemplate = getTemplateText(SQL_TEMPLATE_ID.BRANCH_IDS);
            string currrentInBranchTemplate = getTemplateText(SQL_TEMPLATE_ID.IN_BRANCH_IDS);
            if (sql.Contains(currentBranchTemplate) | sql.Contains(currrentInBranchTemplate))
            {
                EA.ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    // use Package of diagram
                    case EA.ObjectType.otDiagram:
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        id = dia.PackageID;
                        break;
                    // use Package of element
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.PackageID;
                        break;
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                }
                // Context element available
                if (id > 0)
                {
                    // get package recursive
                    string branch = Package.getBranch(rep, "", id);
                    sql = sql.Replace(currentBranchTemplate, branch);
                    sql = sql.Replace(currrentInBranchTemplate, branch);
                } else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No element, diagram or package selected!");
                    sql =  "";
                }
            }
            return sql;
        }
    }
}
