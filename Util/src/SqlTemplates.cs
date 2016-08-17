using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using EA;
using hoTools.Utils.Extension;


namespace hoTools.Utils.SQL
{
    public static class SqlTemplates
    {
        #region Template Dictionary SqlTemplare
        /// <summary>
        /// Dictionary of the available Templates. Note: If error like duplicate key the constructor breaks without exception.
        /// Not easy to find errors.
        /// </summary>
        static readonly Dictionary<SqlTemplateId, SqlTemplate> SqlTemplate = new Dictionary<SqlTemplateId, SqlTemplate>
        {

             {  SqlTemplateId.CurrentItemIdTemplate,
                new SqlTemplate("Current Item Id Template", // Name
                    "CurrentItemIdTemplate",                // String ID of Resource
                    "Template to select current selected item (Package, Diagram, Element, Attribute, Operation) by an ID",
                    isResource:true
                    ) },
              {  SqlTemplateId.CurrentItemGuidTemplate,
                new SqlTemplate("Current Item Id Template", // Name
                    "CurrentItemGuidTemplate",       // String ID of Resource
                    "Template to select current selected item (Package, Diagram, Element, Attribute, Operation) by a GUID",
                    isResource:true
                    ) },
              {  SqlTemplateId.ConveyedItemsFromConnectorTemplate,
                new SqlTemplate("Conveyed Items from selected Connector Template", // Name
                    "ConveyedItemsIdsTemplate",           // String ID of Resource
                    "Template to get Conveyed Items from the selected Connector like: 'ElementID IN ( #ConveyedItemsIDS# )",
                    isResource:true
                    ) },
              {  SqlTemplateId.ConnectorsFromElementTemplate,
                new SqlTemplate("Connectors from Element",    // Name
                    "ConnectorsFromElementTemplate",          // String ID of Resource
                    "Template to get connectors with Conveyed Items from the selected Element. Gets list of Elements which are source of connector. Right Click, Goto Diagram to get the Source element in the Diagram",
                    isResource:true
                    ) },

                {  SqlTemplateId.DiagramElementsIdsTemplate,
                new SqlTemplate("Diagram Elements IDS template",    // Name
                    "DiagramElementsIdsTemplate",          // String ID of Resource
                    "Template to get all Diagram Elements",
                    isResource:true
                    ) },

                {  SqlTemplateId.DiagramSelectedElementsIdsTemplate,
                new SqlTemplate("Diagram selected Elements IDS template",    // Name
                    "DiagramSelectedElementsIdsTemplate",          // String ID of Resource
                    "Template to get selected Diagram Elements",
                    isResource:true
                    ) },

             {  SqlTemplateId.DemoRunScriptTemplate,
                new SqlTemplate("Demo Query to run Scripts on results ",    // Name
                    "DemoRunScript",          // String ID of Resource
                    "Demo Query to run Scripts on results (needs exactly one GUID in result column) ",
                    isResource:true
                    ) },

             {  SqlTemplateId.DeletedTreeSelectedItems,
                new SqlTemplate("Demo Delete Tree selected Items ",    // Name
                    "DeleteTreeSelectedItemsTemplate",          // String ID of Resource
                    "Demo delete SQL to delete the selected Items in browser (Diagram, Element, Attribute, Operation). Be careful! This might cause damage! ",
                    isResource:true
                    ) },
             {  SqlTemplateId.UpdateItemTemplate,
                new SqlTemplate("Demo Update current selected Item ",    // Name
                    "UpdateCurrentSelectedItemTemplate",          // String ID of Resource
                    "Demo update SQL the selected Item (Diagram, Element, Attribute, Operation). Be careful! This might cause damage! ",
                    isResource:true
                    ) },
             {  SqlTemplateId.InsertItemInPackageTemplate,
                new SqlTemplate("Demo Insert into Package ",    // Name
                    "InsertElementIntoCurrentPackage",          // String ID of Resource
                    "Demo insert SQL in current selected package (Package of Diagram, Element, Attribute, Operation). Be careful! This might cause damage! ",
                    isResource:true
                    ) },



            {  SqlTemplateId.BranchTemplate,
                new SqlTemplate("Branch",@"
//
// Template #Branch#
//
select pkg.ea_guid AS CLASSGUID, 'Package' AS CLASSTYPE,pkg.Name AS Name,'Package' As Type
from t_package pkg
where pkg.package_ID in (#Branch#)
     
ORDER BY pkg.Name
            ", "Template Package recursive") },

             { SqlTemplateId.MacrosHelp,
                new SqlTemplate("Macros help",
                "//\r\n" +
                "// Help to available Macros\r\n" +
                "// - #Branch#                    Replaced by the package ID of the selected package and all nested package like '512, 31,613' \r\nTip: Select Package while inserting via Macro" +
                "// - #Branch={...guid....}#      Replaced by the package ID of the package according to GUID and all nested package like '512, 31,613' \r\n" +
                "// - #ConnectorID#              Selected Connector, Replaced by ConnectorID\r\n" +
                "// - #ConveyedItemIDS#           Selected Connector, Replaced by the Conveyed Items as comma separated list of ElementIDs like ' IN (#ConveyedItemIDS#)'\r\n" +
                "// - #CurrentElementGUID#        Alias for #CurrentItemGUID# (compatible to EA)\r\n" +
                "// - #CurrentElementID#          Alias for #CurrentItemID# (compatible to EA)\r\n" +
                "// - #CurrentItemGUID#           Replaced by the GUID of the selected item (Element, Diagram, Package, Attribute, Operation) \r\n" +
                "// - #CurrentItemID#             Replaced by the ID of the selected item (Element, Diagram, Package, Attribute, Operation)\r\n" +
                "// - #DiagramElements_IDS#         Diagram Elements of selected Diagram / current Diagram\r\n"+
                "// - #DiagramSelectedElements_IDS# Selected Diagram Elements of selected Diagram / current Diagram \r\n" +
                "// - #Diagram_ID                 Current Diagram or selected Diagram \r\n" +
                "// - #InBranch#                  like Branch (nested package recursive), but with SQL IN clause like 'package_ID IN (512, 31,613)'\r\n" +
                "// - #NewGuid#                   replace by a new GUID surrounded by brackets ('{...}'). This is useful for SQL 'insert' statements.\r\n" +
                "// - #Package#                   Replaced by the package ID of the containing package of selected Package like: 'package_ID in (#Branch)'\r\n" +
                "// - #PackageID#                 Replaced by the package ID of the containing package of selected Package like: 'package_ID in (#Branch)'\r\n"   +
                "// - #TreeSelectedGUIDS#       In Browser selected Elements as a list of comma separated GUIDS like 'IN (#TreeSelectedGUIDS#)'\r\n" +
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
             { SqlTemplateId.ElementTemplate,
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
              { SqlTemplateId.ElementTypeTemplate,
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

               { SqlTemplateId.DiagramTypeTemplate,
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

             { SqlTemplateId.DiagramTemplate,
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
             { SqlTemplateId.PackageTemplate,
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
             { SqlTemplateId.DiagramObjectTemplate,
                new SqlTemplate(@"Diagram Object Template",@"
//
// Diagram Object Template
//
select o.ea_guid AS CLASSGUID, o.object_type AS CLASSTYPE, o.name, o.object_type, do.Diagram_ID, do.Object_ID, 
do.RectTop, do.RectLeft, do.RectRight, do.RectBottom,
Sequence, ObjectStyle, Instance_ID
from t_diagramobjects do, t_object o
where do.object_id = o.object_ID     AND
      do.diagram_id = #Diagram_ID#
ORDER BY 3",
                    "TemplateText to search Diagram Object Packages") },


             { SqlTemplateId.AttributeTemplate,
                new SqlTemplate(@"Attribute TemplateText",
                     "//\r\n" +
                    "// Template Attribute\r\n" +
                    "//\r\n" +
                    "select o.ea_guid AS CLASSGUID, 'Attribute' AS CLASSTYPE,o.Name AS Name, * \r\n" +
                    "from t_attribute o\r\n" +
                    "where o.name like '%' ",
                    "Diagram objects of current or selected Diagram") },
             { SqlTemplateId.OperationTemplate,
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
            { SqlTemplateId.SearchTerm,
                new SqlTemplate("SEARCH_TERM",
                    "<Search Term>",
                    "<Search Term> is a string replaced at runtime by a variable string.\nExample: Name = '<Search Term>'") },
            { SqlTemplateId.ConnectorId,
                new SqlTemplate("CONNECTOR_ID",
                    "#ConnectorID#",
                    "Placeholder for the current selected connector ID\nExample: ConnectorID = #ConnectorID# ") },
             { SqlTemplateId.ConveyedItemIds,
                new SqlTemplate("CONVEDYED_ITEM_IDS",
                    "#ConveyedItemIDS#",
                    "Placeholder for the current conveyed item IDs as comma separated list\nExample: 'elementID in (#ConveyedItemIDS#)'") },
             { SqlTemplateId.TreeSelectedGuids,
                new SqlTemplate("TREE_SELECTED_GUIDS",
                    "#TreeSelectedGUIDS#",
                    "Placeholder for selected Browser Elements  as comma separated list of GUIDs\nExample: ea_GUID in (#TreeSelectedGUIDS#)") },
              { SqlTemplateId.PackageId,
                new SqlTemplate("PACKAGE_ID",
                    "#Package#",
                    "Placeholder for the package the selected Item (Package, Diagram, Element, Operation, Attribute) is contained, use as PackageID\nExample: pkg.Package_ID = #Package# ") },
            { SqlTemplateId.Package,
                new SqlTemplate("PACKAGE_ID",
                    "#PackageID#",
                    "Placeholder for the package the selected Item (Package, Diagram, Element, Operation, Attribute) is contained, use as PackageID\nExample: pkg.Package_ID = #PackageID# ") },
            { SqlTemplateId.BranchIds,
                new SqlTemplate("BRANCH_IDS",
                    "#Branch#",
                    "Placeholder for the current selected package (recursive), use as PackageID\nExample: pkg.Package_ID in (#Branch#)\nExpands to in '512,513,..' ")  },
             { SqlTemplateId.BranchIdsConstantPackage,
                new SqlTemplate("BRANCH_IDS_CONSTANT_PACKAGE",
                    "#Branch={....guid...}#",
                    @"Placeholder for the package (recursive) for the used guid, use as PackageID
Example: pkg.Package_ID in (#Branch={....guid...0#)\nExpands to in '512,513,..' 

Tip: Select Package while inserting in SQL via Insert Macro in Context Menu"
)  },

            { SqlTemplateId.InBranchIds,
                new SqlTemplate("IN_BRANCH_IDS",
                    "#InBranch#",
                    "Placeholder for sql in clause for the current selected package (recursive), use as PackageID\nExample: pkg.Package_ID  (#InBranch#)\nExpands to 'IN (512,513,..)' ") },
            { SqlTemplateId.NewGuid,
                new SqlTemplate("NewGuid",
                    "#NewGuid#",
                    "Placeholder for a new created GUID to use in SQL INSERT statement.") },
            { SqlTemplateId.CurrentItemId,
                new SqlTemplate("CURRENT_ITEM_ID",
                    "#CurrentItemID#","Placeholder for the current selected item ID (Element, Package, Diagram, Attribute, Operation, may be ambiguous, GUID is more secure), use as ID\nExample: obj.Object_ID = #CurrentItemID#; Alias: #CurrentElementID#") },
            { SqlTemplateId.CurrentItemGuid,
                new SqlTemplate( "CURRENT_ITEM_GUID",
                    "#CurrentItemGUID#",
                    "Placeholder for the current selected item GUID (Element, Package, Diagram, Attribute, Operation), use as GUID\nExample: obj.ea_GUID = #CurrentItemGUID#; Alias: #CurrentElementGUID# ") },

            { SqlTemplateId.DiagramSelectedElementsIds,
                new SqlTemplate( "DiagramSelectedElements_IDS",
                    "#DiagramSelectedElements_IDS#",
                    "Placeholder for the current selected items in the diagram, as ID\nExample: elementID in (#DiagramSelectedElements_IDS# )") },

            { SqlTemplateId.DiagramElementsIds,
                new SqlTemplate( "DiagramElements_IDS",
                    "#DiagramElements_IDS#",
                    "Placeholder for the diagram Elements in the selected diagram, as ID\nExample: elementID in (#DiagramElements_IDS#) ") },

             { SqlTemplateId.DiagramId,
                new SqlTemplate( "Diagram ID", // Name
                    "#Diagram_ID#",            // Macro
                    "Placeholder for the diagram ID of the current diagram, as ID\nExample: diagramID in (#Diagram_ID#) ") },

            { SqlTemplateId.Wc,
                new SqlTemplate("DB Wild Card", 
                    "#WC#", 
                    "Placeholder for the Database Wild Card (% or *)\r\n"+
                    "Example like 'MyClass#WC#'\r\n " +
                    "Remark: Just for compatibility with EA Searches.You may simple use the familiar wild cards '*' or '%'\r\n " +
                    "        hoTools take care of the correct usage for the current database\r\n "
                    ) },
            { SqlTemplateId.DesignId,
                new SqlTemplate("Design Time ID",
                    "ID",
                    @"Inserts the ID of the selected item at Design time. It also copies the ID to clipboard.
For: Package, Element, Diagram, Attribute, Operation" 
                    ) },
            { SqlTemplateId.DesignGuid,
                new SqlTemplate(@"Design Time GUID",
                    "GUID",
                    @"Inserts the GUID of the selected item at Design time. It also copies the GUID to clipboard.
For: Package, Element, Diagram, Attribute, Operation" 
                    ) },


            { SqlTemplateId.Now,
                new SqlTemplate("NOW date/time", 
                    "#NOW(not implemented)#", 
                    "Placeholder date/time, not yet implemented ") },
            { SqlTemplateId.Author,
                new SqlTemplate("Author",
                    "#Author(not implemented)#",
                    "Author, Takes the name from the 'Author' field in the 'Options' dialog 'General' page.") },
            { SqlTemplateId.DbMysql,
               new SqlTemplate("DB_MYSQL",
                    "#DB=MYSQL#                             #DB=MYSQL#",
                    "The SQL string for other DBs included by #DB=MYSQL#, #DB=MYSQL#     ...my db sql....#DB=MYSQL#") },
            { SqlTemplateId.DbJet,
                new SqlTemplate("DB_JET",
                    "#DB=JET#                             #DB=JET#",
                    "The SQL string for other DBs included by #DB=JET#, #DB=JET#     ...my db sql....#DB=JET#") },
            { SqlTemplateId.DbOracle,
                new SqlTemplate("DB_ORACLE",
                    "#DB=ORACLE#                             #DB=ORACLE#",
                    "The SQL string for other DBs included by #DB=ORACLE#, #DB=ORACLE#     ...my db sql....#DB=ORACLE#") },
            { SqlTemplateId.DbSqlsvr,
                new SqlTemplate("DB_SQLSVR",
                    "#DB=SQLSVR#                             #DB=SQLSVR#",
                    "The SQL string for other DBs included by #DB=SQLSVR#, #DB=SQLSVR#     ...my db sql....#DB=SQLSVR#") },
            { SqlTemplateId.DbAsa,
                new SqlTemplate("DB_ASA",
                    "#DB=ASA#                             #DB=ASA#",
                    "The SQL string for other DBs included by #DB=ASA#, #DB=ASA#     ...my db sql....#DB=ASA#") },
            { SqlTemplateId.DbPostgres,
                new SqlTemplate("DB_POSTGRES",
                    "#DB=POSTGRES#                             #DB=POSTGRES#",
                    "The SQL string for other DBs included by #DB=POSTGRES#, #DB=POSTGRES#     ...my db sql....#DB=POSTGRES#") },
            { SqlTemplateId.DbAccess2007,
                new SqlTemplate("DB_ACCESS2007",
                    "#DB=ACCESS2007#                             #DB=ACCESS2007#",
                    "The SQL string for other DBs included by #DB=ACCESS2007#, #DB=ACCESS2007#     ...my db sql....#DB=ACCESS2007#") },
             { SqlTemplateId.DbFirebird,
                new SqlTemplate("DB_FIREBIRD",
                    "#DB=FIREBIRD#                             #DB=FIREBIRD#",
                    "The SQL string for other DBs included by #DB=FIREBIRD#, #DB=FIREBIRD#     ...my db sql....#DB=FIREBIRD#") }


        };
        #endregion
        #region public Enum SQL_TEMPLATE_ID
        /// <summary>
        /// Available Templates
        /// </summary>
        public enum SqlTemplateId
        {
            DbOther,           // Macros for special DBs
            DbAccess2007,
            DbOracle,
            DbAsa,
            DbSqlsvr,
            DbMysql,
            DbPostgres,
            DbFirebird,
            DbJet,

            BranchTemplate,   // Branch Template
            ElementTemplate,
            ElementTypeTemplate,
            DiagramTemplate,
            DiagramTypeTemplate,
            PackageTemplate,
            DiagramObjectTemplate,
            AttributeTemplate,
            OperationTemplate,
            SearchTerm,

            // SQL modifying
            InsertItemInPackageTemplate,
            UpdateItemTemplate,
            DeletedTreeSelectedItems,
               
            //-------------------------
            // macros
            MacrosHelp,        // Help to macros
            ConnectorId,   // Get's the connector
            ConveyedItemIds, // Get's the conveyed Items of the connector as a comma separated ID list of elementIDs
            ConveyedItemsFromConnectorTemplate, // Template Conveyed Items of the selected connector
            ConnectorsFromElementTemplate, // Template to get the Connector with Conveyed Items from Element
            PackageId,      // The containing package of Package, Diagram, Element, Attribute, Operation
            Package,         // The containing package of Package, Diagram, Element, Attribute, Operation (compatible with EA)
            BranchIds,     // Package (nested, recursive) ids separated by ','  like '20,21,47,1'
            BranchIdsConstantPackage, // Package (nested, recursive) for the package defined by the GUID, ids separated by ','  like '20,21,47,1'
            InBranchIds,  // Package (nested, recursive), complete SQL in clause, ids separated by ','  like 'IN (20,21,47,1)', just a shortcut for #BRANCH_ID#
            CurrentItemId,     // ALIAS CURRENT_ELEMENT_ID exists (compatible to EA)
            CurrentItemIdTemplate,   // Template for usage of #CurrentItemId#
            CurrentItemGuidTemplate, // Template for usage of #CurrentItemGUID#
            CurrentItemGuid,  // ALIAS CURRENT_ELEMENT_GUID exists (compatible to EA)
            DiagramSelectedElementsIds,// Selected Diagram objects as a comma separated list of IDs
            DiagramElementsIds,        // Diagram objects (selected diagram) as a comma separated list of IDs
            DiagramElementsIdsTemplate,
            DiagramSelectedElementsIdsTemplate,
            DiagramId,                 // Diagram ID
            Author,
            TreeSelectedGuids, // get all the GUIDs of the selected items (otDiagram, otElement, otPackage, otAttribute, otMethod
            NewGuid,            // create a new GUIDs to use in insert statement
            Now,
            Wc,
            DesignId,                 // The ID at design time (editing query) of the selected item (Package, Element, Diagram, Operation, Attribute)
            DesignGuid,               // The GUID at design time (editing query) of the selected item (Package, Element, Diagram, Operation, Attribute) 
            DemoRunScriptTemplate
        }
        #endregion

        /// <summary>
        /// Get TemplateText
        /// - TemplateString
        /// - Name
        /// - ToolTip
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static SqlTemplate GetTemplate(SqlTemplateId templateId)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateId, out template))
            {
                return template;
            }
            else {
                MessageBox.Show($"ID={templateId}", @"Invalid templateID");
                return null;
            }

        }
        /// <summary>
        /// Get TemplateText (the template) or null according to templateID. 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string GetTemplateText(SqlTemplateId templateId)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateId, out template))
            {
                // get string from class or from resource
                if (template.IsResource)
                {
                    ResourceManager rm = new ResourceManager("hoTools.Utils.Resources.Strings", Assembly.GetExecutingAssembly());
                    return rm.GetString(template.TemplateText); 
                }  else return template.TemplateText;
            }
            else {
                MessageBox.Show($"ID={templateId}", @"Invalid templateID");
                return null;
            }
   
        }
        /// <summary>
        /// Get TemplateText Tool tip or null according to templateID
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string GetTooltip(SqlTemplateId templateId)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateId, out template))
            {
                return template.ToolTip;
            }
            else {
                MessageBox.Show($"ID={templateId}", @"Invalid templateID");
                return null;
            }

        }
        /// <summary>
        /// Get TemplateText name or null according to templateID
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string GetTemplateName(SqlTemplateId templateId)
        {
            SqlTemplate template;
            if (SqlTemplate.TryGetValue(templateId, out template))
            {
                return template.TemplateName;
            }
            else {
                MessageBox.Show($"ID={templateId}", @"Invalid templateID");
                return null;
            }

        }

        /// <summary>
        /// Replace Macro and 'Search Term' by value. Possible Macros are: Search Term, ID, GUID, Package ID, Branch ID,... 
        /// <para/>- convert all macros to lower case
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sqlString">The complete SQL string</param>
        /// <param name="searchTerm">The Search Term from the text entry field</param>
        /// <returns>"" if error occurred</returns>
        public static string ReplaceMacro(Repository rep, string sqlString, string searchTerm)
        {
            // delete Comments
            string sql = deleteC_Comments(sqlString);

            // <Search Term>
            sql = sql.Replace(GetTemplateText(SqlTemplateId.SearchTerm), searchTerm);

            sql = macroDiagram_ID(rep, sql);
            if (sql == "") return "";

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
            sql = MacroPackageId(rep, sql);
            if (sql == "") return "";

            // replace #ConnectorID#, #ConveyedItemIDS# 
            sql = MacroConnector(rep, sql);
            if (sql == "") return "";

            // replace #TreeSelectedGUIDS#
            sql = MacroTreeSelected(rep, sql);
            if (sql == "") return "";

            // replace #Branch#
            sql = MacroBranch(rep, sql);
            if (sql == "") return "";

            // replace #Branch={...guid....}# by a list of nested packages
            sql = MacroBranchConstant(rep, sql);
            if (sql == "") return "";


            // Replace #WC# (DB wile card)
            // Later '*' is changed to the wild card of the current DB
            string currentTemplate = GetTemplateText(SqlTemplateId.Wc);
            if (sql.Contains(currentTemplate))
            {
                sql = sql.Replace(currentTemplate, "*"); 
            }

            // replace #NewGuid" by a newly created GUID (global unique identifier
            string newGuidTemplate = GetTemplateText(SqlTemplateId.NewGuid);
            if (sql.Contains(newGuidTemplate))
            {
                sql = sql.Replace(newGuidTemplate, "{" + Guid.NewGuid() + "}");
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
        /// Replace macro #Diagram_ID# by Diagram_Id of the current diagram or selected Diagram.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns>sql string with replaced macro</returns>
        static string macroDiagram_ID(Repository rep, string sql)
        {
            // get template
            string template = GetTemplateText(SqlTemplateId.DiagramId);

            // template is used
            if (sql.Contains(template))
            {
                // get the diagram
                Diagram dia;
                if (rep.GetContextItemType() == ObjectType.otDiagram)
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
                    // replace by list of IDs
                    sql = sql.Replace(template, $"{dia.DiagramID}");
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
        static string macroDiagramElements_IDS(Repository rep, string sql) {
            // get template
            string template = GetTemplateText(SqlTemplateId.DiagramElementsIds);

            // template is used
            if (sql.Contains(template))
            {
                // get the diagram
                Diagram dia;
                if (rep.GetContextItemType() == ObjectType.otDiagram)
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
            string template = GetTemplateText(SqlTemplateId.DiagramSelectedElementsIds);

            // template is used
            if (sql.Contains(template))
            {
                // get the diagram
                Diagram dia = rep.GetContextItemType() == ObjectType.otDiagram ? rep.GetContextObject() : rep.GetCurrentDiagram();
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
            string template = GetTemplateText(SqlTemplateId.CurrentItemId);
            if (sql.Contains(template) | sql.Contains("#CurrentElementID#"))
            {
                ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    case ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.ElementID;
                        break;
                    case ObjectType.otDiagram:
                        Diagram dia = (Diagram)rep.GetContextObject();
                        id = dia.DiagramID;
                        break;
                    case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                    case ObjectType.otAttribute:
                        EA.Attribute attr = (EA.Attribute)rep.GetContextObject();
                        id = attr.AttributeID;
                        break;
                    case ObjectType.otMethod:
                        Method method = (Method)rep.GetContextObject();
                        id = method.MethodID;
                        break;
                }

                if (id > 0)
                {
                    sql = sql.Replace(template, $"{id}");
                    sql = sql.Replace("#CurrentElementID#", $"{id}");// Alias for EA compatibility
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, @"No item (Package, Element, Diagram, Operation, Attribute) selected!");
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
            string template = GetTemplateText(SqlTemplateId.CurrentItemGuid);
            if (sql.Contains(template) | sql.Contains("#CurrentElementGUID#")) 
            {
                ObjectType objectType = rep.GetContextItemType();
                string guid = "";
                switch (objectType)
                {
                    case ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        guid = el.ElementGUID;
                        break;
                    case ObjectType.otDiagram:
                        Diagram dia = (Diagram)rep.GetContextObject();
                        guid = dia.DiagramGUID;
                        break;
                    case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        guid = pkg.PackageGUID;
                        break;
                    case ObjectType.otAttribute:
                        EA.Attribute attr = (EA.Attribute)rep.GetContextObject();
                        guid = attr.AttributeGUID;
                        break;
                    case ObjectType.otMethod:
                        Method method = (Method)rep.GetContextObject();
                        guid = method.MethodGUID;
                        break;
                }

                if (guid != "")
                {
                    sql = sql.Replace(template, $"{guid}");
                    sql = sql.Replace("#CurrentElementGUID#", $"{guid}");// Alias for EA compatibility
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, @"No item (Package, Element, Diagram, Operation, Attribute) selected!");
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
        static string MacroPackageId(Repository rep, string sql)
        {
            // Package ID
            string currentPackageTemplate = GetTemplateText(SqlTemplateId.PackageId);
            if (sql.Contains(currentPackageTemplate))
            {
                // get Package id
                int id = GetParentPackageIdFromContextElement(rep);
                if (id > 0)
                {
                    sql = sql.Replace(currentPackageTemplate, $"{id}");
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, @"No element, diagram, package, attribute, operation selected!");
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
        static int GetParentPackageIdFromContextElement(Repository rep)
        {
            ObjectType objectType = rep.GetContextItemType();
            int id = 0;
            switch (objectType)
            {
                case ObjectType.otDiagram:
                    Diagram dia = (Diagram)rep.GetContextObject();
                    id = dia.PackageID;
                    break;
                case ObjectType.otElement:
                    EA.Element el = (EA.Element)rep.GetContextObject();
                    id = el.PackageID;
                    break;
                case ObjectType.otPackage:
                    EA.Package pkg = (EA.Package)rep.GetContextObject();
                    id = pkg.PackageID;
                    break;
                case ObjectType.otAttribute:
                    EA.Attribute attr = (EA.Attribute)rep.GetContextObject();
                    EA.Element elOfAttr = rep.GetElementByID(attr.ParentID);
                    id = elOfAttr.PackageID;
                    break;
                case ObjectType.otMethod:
                    Method meth = (Method)rep.GetContextObject();
                    EA.Element elOfMeth = rep.GetElementByID(meth.ParentID);
                    id = elOfMeth.PackageID;
                    break;
            }

            return id;
        }

        /// <summary>
        /// Connector macros: ConnectorID and ConveyedItemIDS
        /// Note: If Element connector 
        /// <para/>
        /// Replace macro #ConnectorID# by the ID of the selected connector.
        /// <para/>
        /// Replace macro #ConveyedItemIDS# by the IDs of conveyed Elements of the selected connector.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string MacroConnector(Repository rep, string sql)
        {
            //--------------------------------------------------------------------------------------------
            // CONNECTOR ID
            // CONVEYED_ITEM_ID
            string currentConnectorTemplate = GetTemplateText(SqlTemplateId.ConnectorId);
            string currentConveyedItemTemplate = GetTemplateText(SqlTemplateId.ConveyedItemIds);

            if ((sql.Contains(currentConnectorTemplate) | sql.Contains(currentConveyedItemTemplate)))
            {
                // connector
                if (rep.GetContextItemType() == ObjectType.otConnector)
                {
                    // connector ID
                    Connector con = rep.GetContextObject();
                    if (sql.Contains(currentConnectorTemplate))
                    {
                        
                        sql = sql.Replace(currentConnectorTemplate, $"{con.ConnectorID}");
                    }
                    // conveyed items are a comma separated list of elementIDs
                    if (sql.Contains(currentConveyedItemTemplate))
                    {

                        // to avoid syntax error, 0 will never fit any conveyed item
                        string conveyedItems = "0";

                        // first get "InformationFlows" which carries the conveyed items
                        if (con.MetaType == "Connector")
                        {
                            // get semicolon delimiter list of information flow guids
                            string sqlInformationFlows = "select x.description " +
                                                         "    from  t_xref x " +
                                                         $"    where x.client = '{con.ConnectorGUID}' ";

                            // get semicolon delimiter list of guids of all dependent connectors/information flows
                            List<string> lFlows = rep.GetStringsBySql(sqlInformationFlows);
                            foreach (string flowGuids in lFlows) { 
                                string[] lFlowGuid = flowGuids.Split(',');
                                foreach (string flowGuid in lFlowGuid)
                                {
                                    EA.Connector flow = rep.GetConnectorByGuid(flowGuid);
                                    foreach (EA.Element el in flow.ConveyedItems)
                                    {
                                        conveyedItems = $"{conveyedItems}, {el.ElementID}";
                                    }
                                }

                            }
                        }
                        else
                        {


                            foreach (EA.Element el in con.ConveyedItems)
                            {
                                conveyedItems = $"{conveyedItems}, {el.ElementID}";
                            }

                        }
                        sql = sql.Replace(currentConveyedItemTemplate, $"{conveyedItems}");
                    }
                } else
                // no connector selected
                {
                    MessageBox.Show(sql, @"No connector selected!");
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
        static string MacroTreeSelected(Repository rep, string sql)
        {
            //--------------------------------------------------------------------------------------------
            // Tree selected items
            // CONVEYED_ITEM_ID
            string template = GetTemplateText(SqlTemplateId.TreeSelectedGuids);


            if (sql.Contains(template))
            {
                // get the selected elements (Element)
                string guiDs = "";
                string comma = "";
                Collection col = rep.GetTreeSelectedElements();
                foreach (var el in col)
                {
                    var guid = ((EA.Element)el).ElementGUID;

                    // make list
                    if (guid != "")
                    {
                        guiDs = $"{guiDs}{comma}'{guid}'";
                        comma = ", ";
                    }
                }
                // at least one element selected
                if (guiDs != "")
                {
                    // replace by list of GUIDs
                    sql = sql.Replace(template, $"{guiDs}");
                }
                else// no element in Browser selected
                {
                    MessageBox.Show(sql, @"No elements in browser of type Element(Class, Activity,..) selected, Break!!!!");
                    sql = "";
                }
            }
            return sql;
        }

        /// <summary>
        /// Replace macro #Branch# and #InBranch# by IDs of selected packages, recursive nested. 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string MacroBranch(Repository rep, string sql) { 
        // Branch=comma separated Package IDs, Recursive:
        // Example for 3 Packages with their PackageID 7,29,128
        // 7,29,128
        //
        // Branch: complete SQL IN statement ' IN (comma separated Package IDs, Recursive):
        // IN (7,29,128)
            string currentBranchTemplate = GetTemplateText(SqlTemplateId.BranchIds);
            string currrentInBranchTemplate = GetTemplateText(SqlTemplateId.InBranchIds);

            
            if (sql.Contains(currentBranchTemplate) | sql.Contains(currrentInBranchTemplate))
            {
                ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    // use Package of diagram
                    case ObjectType.otDiagram:
                        Diagram dia = (Diagram)rep.GetContextObject();
                        id = dia.PackageID;
                        break;
                    // use Package of element
                    case ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.PackageID;
                        break;
                    case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                }
                // Context element available
                if (id > 0)
                {
                    // get package recursive
                    string branch = Package.GetBranch(rep, "", id);
                    sql = sql.Replace(currentBranchTemplate, branch);
                    sql = sql.Replace(currrentInBranchTemplate, branch);
                } else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, @"No element, diagram or package selected!");
                    sql =  "";
                }
            }
            return sql;
        }
        /// <summary>
        /// Replace macro '#Branch={...guid...}#' by ids of the package referenced by GUID. It selects all nested packages recursive. 
        /// <para />
        /// Usage: '#Branch={2BC1A31E-0F99-40CE-BE76-04E7DCEDDD87}#' is replaced by a comma separated list of the package and its recursive nested packages.
        /// <para /> 
        /// Result:    6,28,5,20147
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        static string MacroBranchConstant(Repository rep, string sql)
        {
            foreach (SqlTemplateId id in new SqlTemplateId[]
            {
                SqlTemplateId.BranchIds,
                SqlTemplateId.InBranchIds
            })
            {
                string branchPattern = GetTemplateText(id);
                branchPattern = branchPattern.Remove(branchPattern.Length - 1);
                branchPattern = branchPattern + @"=({[ABCDEF0-9-]*})#";
                Regex pattern = new Regex(branchPattern, RegexOptions.IgnoreCase);
                MatchCollection matches = pattern.Matches(sql);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        // get Package id
                        EA.Package pkg = rep.GetPackageByGuid(match.Groups[1].Value);
                        if (pkg == null)
                        {
                            MessageBox.Show($"Package for GUID '{match.Groups[1].Value}' not found",
                                @"GUID for #Branch={...}# not found, Break");
                            {
                                return "";
                            }
                        }
                        int pkgId = pkg.PackageID;
                        string branch = Package.GetBranch(rep, "", pkgId);
                        sql = sql.Replace(match.Groups[0].Value, branch);
                    }
                }
            }
            return sql;
        }

        /// <summary>
        /// Replace macro #Branch={..guid..}# and #InBranch={..guid..}# by IDs of selected packages, recursive nested. 
        /// It uses the package of the guid and not of the selected Package
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string MacroBranchConstantPackage(Repository rep, string sql)
        {
            // Branch=comma separated Package IDs, Recursive:
            // Example for 3 Packages with their PackageID 7,29,128
            // 7,29,128
            //
            // Branch: complete SQL IN statement ' IN (comma separated Package IDs, Recursive):
            // IN (7,29,128)
            string currentBranchTemplate = GetTemplateText(SqlTemplateId.BranchIds);
            string currrentInBranchTemplate = GetTemplateText(SqlTemplateId.InBranchIds);
            if (sql.Contains(currentBranchTemplate) | sql.Contains(currrentInBranchTemplate))
            {
                ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    // use Package of diagram
                    case ObjectType.otDiagram:
                        Diagram dia = (Diagram)rep.GetContextObject();
                        id = dia.PackageID;
                        break;
                    // use Package of element
                    case ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        id = el.PackageID;
                        break;
                    case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                }
                // Context element available
                if (id > 0)
                {
                    // get package recursive
                    string branch = Package.GetBranch(rep, "", id);
                    sql = sql.Replace(currentBranchTemplate, branch);
                    sql = sql.Replace(currrentInBranchTemplate, branch);
                }
                else
                // no diagram, element or package selected
                {
                    MessageBox.Show(sql, @"No element, diagram or package selected!");
                    sql = "";
                }
            }
            return sql;
        }
    }
}
