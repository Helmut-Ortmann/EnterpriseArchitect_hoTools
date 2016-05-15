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
                "// - #CurrentElementGUID#        Replaced by the GUID of the selected item (Element, Diagram, Package, Attribute, Operation \r\n" +
                "// - #CurrentElementID#          Replaced by the ID of the selected item (Element, Diagram, Package, Attribute, Operation\r\n" +
                "// - #InBranch#                  like Branch (nested package recursive), but with SQL IN clause like 'IN (512, 31,613)'\r\n" +
                "// - #Package#                   Replaced by the package ID of the selected package\r\n" +
                "// - <Search Term>               Replaced by the string in the 'Search Term' entry field\r\n" +
                "// -  #WC#                       \r\n" +

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
            { SQL_TEMPLATE_ID.PACKAGE_ID,
                new SqlTemplate("PACKAGE_ID", 
                    "#Package#",
                    "Placeholder for the current selected package, use as PackageID\nExample: pkg.Package_ID = #Package# ") },
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
                    "#CurrentElementID#","Placeholder for the current selected item ID, use as ID\nExample: obj.Object_ID = #CurrentElementID# ") },
            { SQL_TEMPLATE_ID.CURRENT_ITEM_GUID,
                new SqlTemplate( "CURRENT_ITEM_GUID",
                    "#CurrentElementGUID#",
                    "Placeholder for the current selected item GUID, use as GUID\nExample: obj.ea_GUID = #CurrentElementGUID# ") },
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
            MACROS_HELP,        // Help to macros
            ELEMENT_TEMPLATE,
            ELEMENT_TYPE_TEMPLATE,
            DIAGRAM_TEMPLATE,
            DIAGRAM_TYPE_TEMPLATE,
            PACKAGE_TEMPLATE,
            DIAGRAM_OBJECT_TEMPLATE,
            ATTRIBUTE_TEMPLATE,
            OPERATION_TEMPLATE,
            SEARCH_TERM,
            CONNECTOR_ID,   // Get's the connector
            CONVEYED_ITEM_IDS, // Get's the conveyed Items of the connector as a comma separated ID list of elementIDs
            PACKAGE_ID,
            BRANCH_IDS,     // Package (nested, recursive) ids separated by ','  like '20,21,47,1'
            IN_BRANCH_IDS,  // Package (nested, recursive), complete SQL in clause, ids separated by ','  like 'IN (20,21,47,1)', just a shortcut for #BRANCH_ID#
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
        /// <returns></returns>
        public static string replaceMacro(Repository rep, string sqlString, string searchTerm)
        {
            // delete Comments
            string sql = deleteC_Comments(sqlString);

            // <Search Term>
            sql = sql.Replace(getTemplateText(SQL_TEMPLATE_ID.SEARCH_TERM), searchTerm);


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
                
                if (id > 0)
                {
                    sql = sql.Replace(currentIdTemplate, $"{id}");
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No element, diagram or package selected!");
                }

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
                if (guid != "")
                {
                    sql = sql.Replace(currentGuidTemplate, guid);
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No element, diagram or package selected!");
                }
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
                if (id >  0)
                {
                    sql = sql.Replace(currentPackageTemplate, $"{id}");
                } else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No element, diagram or package selected!");
                }

            }
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
                        string s = "";
                        string del = "";
                        foreach (EA.Element el in con.ConveyedItems)
                        {
                            s = s + del + el.ElementID;
                            del = ", ";
                        }
                        sql = sql.Replace(currentConveyedItemTemplate, $"{s}");
                    }
                } else
                // no connector selected
                {
                    MessageBox.Show(sql, $"No connector selected!");
                }
            }
               


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
                    string branch = Package.getBranch(rep, "", id);
                    sql = sql.Replace(currentBranchTemplate, branch);
                    sql = sql.Replace(currrentInBranchTemplate, branch);
                } else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, $"No element, diagram or package selected!");
                }
            }
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
    }
}
