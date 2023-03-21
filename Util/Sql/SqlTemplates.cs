using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EA;
using hoLinqToSql.LinqUtils;
using hoTools.Utils.Extension;
using hoTools.Utils.SQL;
using Attribute = EA.Attribute;

namespace hoTools.Utils.Sql
{
    /// <summary>
    /// Macros to add useful features to SQL:
    /// - DB independent (SQLite, Access2007, JET, MySQL)
    /// - EA macros
    /// - Additional features
    /// </summary>
    public class SqlTemplates
    {
        private readonly Repository _rep;
        private readonly string _sqlString;
        private string _repType;
        //
        // Show the Regex result for a Column
        // The regex must not contain '#'
        // #Regex#source-column#target-column#regexp#
        private readonly Regex _regShowColumn = new Regex(@"#(Regex|RegexShowColumn)#([^#]*)#([^#]*)#([^#]*)#");

        // The regex must not contain '#'
        // #Regex#[OR|AND|NOT]#source-column#regexp#
        private readonly Regex _regFilterRows = new Regex(@"#RegexFilterRows#([^#]*)#([^#]*)#([^#]*)#");
        //
        /// <summary>
        /// Show Column content as a result of a RegExp
        /// #Regex#source column#target column#regex#
        /// </summary>
        private readonly List<RegExShowColumn> _regexShowColumns = new List<RegExShowColumn>();

        //
        /// <summary>
        /// Filter Rows where it's columns matches a RegExp
        /// condition: [AND|OR|NOT]
        /// #RegexFilterRows#condition#column#regex#
        /// </summary>
        private readonly List<RegExFilterRows> _regexFilterRows = new List<RegExFilterRows>();
        /// <summary>
        /// The SQL-String to output.
        /// It doesn't contain the regex definitions
        /// </summary>
        public string SqlString => _sqlString;
        /// <summary>
        /// IsAdvanced
        /// SQL with Regex
        /// </summary>
        public bool IsAdvanced => _regexShowColumns.Any() || _regexFilterRows.Any();

        /// <summary>
        /// Used for advanced SQL
        /// - Use regex to set column content
        /// - Use regex to filter rows based on column content
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sqlString"></param>
        public SqlTemplates(EA.Repository rep, string sqlString)
        {
            _rep = rep;

            _sqlString = sqlString;
            _repType = RepType(_rep);
            try
            {
                // Regular expression support
                // #Regex#source column#target column#regex#
                MatchCollection mc = _regShowColumn.Matches(sqlString);
                foreach (Match match in mc)
                {
                    _regexShowColumns.Add(new RegExShowColumn(match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
                    // remove ReEx macros
                    sqlString = sqlString.Replace(match.Value, "");
                }

                // Find matching Rows
                // # RegexFilter#source column#target column#regex#
                mc = _regFilterRows.Matches(sqlString);
                foreach (Match match in mc)
                {
                    _regexFilterRows.Add(new RegExFilterRows(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));
                    // remove ReEx macros
                    sqlString = sqlString.Replace(match.Value, "");
                }

                // removed sql, converted to _regexShowColumns
                _sqlString = sqlString;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"SQL: {_sqlString}

{e}
",@"Can't apply Regex");
                throw;
            }

           


        }
        /// <summary>
        /// Perform regex
        /// - set column content
        /// - filter rows
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>xml to output via EA</returns>
        public string PerformRegExpression(string xml)
        {
            try
            {
                var dt = Xml.MakeDataTableFromSqlXml(xml);

                if (_regexShowColumns.Count > 0)
                {
                    foreach (var item in _regexShowColumns)
                    {
                        var regex = item.GetRegEx();

                        // Get index of srcIndex
                        var srcIndex = dt.Columns.IndexOf(item.SrcColumn);
                        if (srcIndex == -1) continue;              // Source Column not available

                        // Create target Column if not available
                        if (dt.Columns.IndexOf(item.TrgColumn) < 0) dt.Columns.Add(item.TrgColumn, typeof(String));

                        // over all found rows
                        foreach (DataRow row in dt.Rows)
                        {
                            var srcValue = row[srcIndex].ToString();
                            var trgValue = "";

                            // extract value
                            Match m = regex.Match(srcValue);
                            if (m.Success)
                            {
                                trgValue = m.Groups[1].Value;
                            }
                            // Update cell
                            row[item.TrgColumn] = trgValue;

                        }
                    }
                }
                // Filter rows
                // The condition associated with a regex are sequential performed (false condition1 regexFilterRow1 condition2 regexFilterRow2 condition3 regexFilterRow3....)
                // There are no priorities like AND before OR or so
                if (_regexFilterRows.Count > 0)
                {
                    // over all found rows, allow deleting rows
                    dt.AcceptChanges();

                    foreach (DataRow row in dt.Rows)
                    {
                        // over all columns
                        bool showRow = true;
                        foreach (var item in _regexFilterRows)
                        {
                            var regex = item.GetRegEx();
                            var index = dt.Columns.IndexOf(item.Column);
                            if (index == -1) continue;              // Column not available
                            var value = row[index].ToString();

                            // extract value
                            Match m = regex.Match(value);
                            // fund first match
                            if (m.Success)
                            {
                                if (item.Condition == "AND") showRow = showRow & true;
                                if (item.Condition == "OR") showRow = showRow | true;

                            }
                            else
                            {
                                if (item.Condition == "AND") showRow = false;
                                if (item.Condition == "OR") showRow = showRow | false;

                            }
                        }
                        if (showRow == false) row.Delete();


                    }
                    
                }
                // return EA xml to output with EA.
                return dt.ToEaXml().ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"SQL: {_sqlString}

{e}", @"Error perform Regex on Column");
                return "";
            }
           

        }
        #region Template Dictionary SqlTemplare
        /// <summary>
        /// Dictionary of the available Templates. Note: If error like duplicate key the constructor breaks without exception.
        /// Not easy to find errors.
        /// </summary>
        static readonly Dictionary<SqlTemplateId, SqlTemplate> SqlTemplate = new Dictionary<SqlTemplateId, SqlTemplate>
        {
            // Added
            {  SqlTemplateId.Concat,
                new SqlTemplate("Concat Template", // Name
                    "ConcatTemplate",                // String ID of Resource
                    "#Concat <value1>, <value2>, ...#  Provides a method of concatenating two or more SQL terms into one string, independent of the database type.",
                    isResource:false
                ) },
            {  SqlTemplateId.If,
                new SqlTemplate("If Template", // Name
                    "IfTemplate",                // String ID of Resource
                    "#If condition, trueValue, falseValue#  Checks whether a condition is met, and returns one value if TRUE of another on if it is FALSE.",
                    isResource:false
                ) },
            {  SqlTemplateId.Left,
                new SqlTemplate("Left Template", // Name
                    "LeftTemplate",                // String ID of Resource
                    "#Left string, numberOfCharacters#  The Left() function extracts a number of characters from a string (starting from left)",
                    isResource:false
                ) },
            {  SqlTemplateId.Right,
                new SqlTemplate("Right Template", // Name
                    "RightTemplate",                // String ID of Resource
                    "#Right string, numberOfCharacters#  The Right() function extracts a number of characters from a string (starting from right)",
                    isResource:false
                ) },
            {  SqlTemplateId.SubString,
                new SqlTemplate("Substring Template", // Name
                    "SubStringTemplate",                // String ID of Resource
                    "#SubString stack, start, length#  The Substring() function extracts some characters from a string (starting at any position, rel 1).",
                    isResource:false
                ) },
            {  SqlTemplateId.InStr,
                new SqlTemplate("InStr Template", // Name
                    "InStringTemplate",                // String ID of Resource
                    "#InStr start, stack, needle#  The InStr() function Search for needle in stack, and return position (start rel 1)",
                    isResource:false
                ) },
            {  SqlTemplateId.ToLower,
                new SqlTemplate("ToLower Template", // Name
                    "ToLowerTemplate",                // String ID of Resource
                    "#ToLower string#  Converts to lower",
                    isResource:false
                ) },
            {  SqlTemplateId.Length,
                new SqlTemplate("Length Template", // Name
                    "ToLengthTemplate",                // String ID of Resource
                    "#Length string#  Gives the length of a string",
                    isResource:false
                ) },




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
            
            {  SqlTemplateId.ShowSearchItems,
            new SqlTemplate("Insert Icons to navigate ",    // Name
                "ShowSearchItemsTemplate",          // String ID of Resource
                "Insert 'ea_guid As CLASSGUID. object_type As CLASSTYPE, 't_connector' As CLASSTABLE' to show the icons in the EA Search Window (CLASSTABLE is only used for connectors) ",
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
                "// - #CondBranchStatement#       ' AND pkg.Package_ID in (...) ' If a package is selected in Browser: Make a SQL statement with ... comma separated list of Package_Ids of the selected package (recursive) \r\n" +
                "// - #CondBranchStatement operation1, column, operation#  ' operation1, column, operation2 (...) ' If a package is selected in Browser: Make a SQL statement with ' ... comma separated list of Package_Ids of the selected package (recursive) \r\n" +
                "//   Example: #CondBranchStatement AND, pkg.Package_Id, in# ' AND pkg.Package_id in (....) ' ... Package_Ids of the selected package recursive " +
                "// - #Concat= <value1>, <value2>,..# Provides a method of concatenating two or more SQL terms into one string, independent of the database type." +
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
                "// - #DB=SL3#                    DB specif SQL for SQLITE\r\n"+
                "// - #DB=SQLITE#                 DB specif SQL for SQLITE\r\n"+

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

            { SqlTemplateId.CondBranchStatement,
                new SqlTemplate("IN_BRANCH_IDS",
                    "#CondBranchStatement#",
                    "If a Package is in the browser selected it adds: 'AND pkg.package_id in (.....) ' where ... are the beneath package ids.\nExpands to ' AND pkg.package_id IN (512,513,..)' ") },
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
                    "The SQL string for other DBs included by #DB=FIREBIRD#, #DB=FIREBIRD#     ...my db sql....#DB=FIREBIRD#") },
             { SqlTemplateId.DbSl3,
                 new SqlTemplate("DB_SL3",
                     "#DB=SL3#                             #DB=SL3#",
                     "The SQL string for other DBs included by #DB=SL3#, #DB=SL3#     ...my db sql....#DB=SL3#") },
             { SqlTemplateId.DbSqLite,
                 new SqlTemplate("DB_SQLITE",
                     "#DB=SQLITE#                             #DB=SQLITE#",
                     "The SQL string for other DBs included by #DB=SQLITE#, #DB=SQLITE#     ...my db sql....#DB=SQLITE#") },
             { SqlTemplateId.DbOther,
                 new SqlTemplate("DB_Other",
                     "#DB=Other#                             #DB=Other#",
                     "The SQL string for other DBs (EA16)") }


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
            DbSl3,
            DbSqLite,

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

            ShowSearchItems,  // insert ea_guid

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
            CondBranchStatement, // If a package is selected: ' AND pkg.Package_id in (.....) '
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
            DemoRunScriptTemplate,

            // new VW Tools
            Concat,                   // #Concat <value1>, <value2>, ...#
            // Provides a method of concatenating two or more SQL strings/terms into one string, independent of the database type.
            // See EA Create Search Definitions
            Left,                       // #Left string, countCharacters#
            // Extracts countCharacters from the string (left)
            // See EA Create Search Definitions
            Right,                      // #Right string, countCharacters#
            // Extracts countCharacters from the string (right)
            // See EA Create Search Definitions
            SubString,                  // #SubString string, start, length#
            // Start: rel 1
            // Provides a method of extracting characters from a string.
            // See EA Create Search Definitions
            InStr,                      // #InStr start, stack, needle#
            // Start: rel 1
            // Provides a method of searching a needle in a hay-stack.
            // See EA Create Search Definitions
            If,                         // #If condition, trueValue, falseValue#
            // Provides a method of concatenating two or more SQL terms into one string, independent of the database type.
            // See EA Create Search Definitions
            ToLower,                     // #ToLower string#
            // Coverts to lower case
            Length                      // #Length string#
                                        // Gives the length of a string
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
            if (SqlTemplate.TryGetValue(templateId, out SqlTemplate template))
            {
                return template;
            }
            MessageBox.Show($@"ID={templateId}", @"Invalid templateID");
            return null;
        }
        /// <summary>
        /// Get TemplateText (the template) or null according to templateID. 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string GetTemplateText(SqlTemplateId templateId)
        {
            if (SqlTemplate.TryGetValue(templateId, out SqlTemplate template))
            {
                // get string from class or from resource
                if (template.IsResource)
                {
                    ResourceManager rm = new ResourceManager("hoTools.Utils.Resources.Strings", Assembly.GetExecutingAssembly());
                    return rm.GetString(template.TemplateText);
                }
                return template.TemplateText;
            }
            MessageBox.Show($@"ID={templateId}", @"Invalid templateID");
            return null;
        }
        /// <summary>
        /// Get TemplateText Tool tip or null according to templateID
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string GetTooltip(SqlTemplateId templateId)
        {
            if (SqlTemplate.TryGetValue(templateId, out SqlTemplate template))
            {
                return template.ToolTip;
            }
            MessageBox.Show($@"ID={templateId}", @"Invalid templateID");
            return null;
        }
        /// <summary>
        /// Get TemplateText name or null according to templateID
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static string GetTemplateName(SqlTemplateId templateId)
        {
            if (SqlTemplate.TryGetValue(templateId, out SqlTemplate template))
            {
                return template.TemplateName;
            }
            MessageBox.Show($@"ID={templateId}", @"Invalid templateID");
            return null;
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

 // replace DB specific code
            sql = FormatSqlDbSpecific(rep, sql);


            // replace DB specific code
            sql = FormatSqlDbSpecific(rep, sql);


            // Find needle in hay-stack
            // #Instr start, stack, needle#
            // start rel 1
            sql = macroInstr_ID(rep, sql);

            // if #If condition, trueValue, falseValue#
            sql = macroIf_ID(rep, sql);

 // macro Format a number with left padding and thousand separator
            // macro Format a number with left padding and thousand separator
            // #Format numberString, length, stringPaddingBefore#
            sql = MacroFormat(rep, sql);

            // macro LPad
            // #Lpad numberString, length, stringPaddingBefore#
            sql = MacroLPad(rep, sql);

            // macro RPad
            // #RPad numberString, length, stringPaddingAfter#
            sql = MacroRPad(rep, sql);

            // if #ToString str#
            sql = macroToString(rep, sql);

             // if #ToBool str#
            sql = macroToBool(rep, sql);
            // if #Left string count#
            sql = macroLeft_ID(rep, sql);

            // #Right string, count#
            sql = macroRight_ID(rep, sql);

   // #Length string#
            sql = macroLength_ID(rep, sql);
            // #SubString string, start, length#
            // start rel 1
            sql = macroSubstring_ID(rep, sql);

            // #ToLower string#
            sql = macroToLower_ID(rep, sql);

   // #ToUpper string#
            sql = macroToUpper_ID(rep, sql);
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

            // replace #Branch={...guid....}# by a list of nested packages
            sql = MacroBranchConstantPackage(rep, sql);
            if (sql == "") return "";

          // replace #Branch={...guid....}# by a list of nested packages
            sql = MacroCondBranchStatement(rep, sql);
            if (sql == "") return "";

            


            // Concatenate strings
            // #Concat str1, str2,..#
            sql = macroConcat_ID(rep, sql);
            // Replace #WC# (DB wile card)
            // Later '*' is changed to the wild card of the current DB
    // see: ReplaceSqlWildCards
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
            s = Regex.Replace(s, @"//.*$", "", RegexOptions.Multiline);// ^$ for each line
            // delete comments /*....
            s = Regex.Replace(s, @"/\*[^\n]*\n", "\r\n");
            // delete empty lines
            s = Regex.Replace(s, "(\r\n){2,200}", "\r\n");
            return s;
        }
        #endregion
        /// <summary>
        /// macro Concatenated
        /// #Concat str1, str2,..#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroConcat_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#Concat([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {
                //var lColumns = match.Groups[1].Value.Replace(" ", "").Split(',');
                var lColumns = match.Groups[1].Value.Split(',');

                string content;
  switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        content = String.Join("&",lColumns); // & supports null values
                        break;
                    case "MYSQL":
                        // Null values:
                        // IFNULL(value, '')
                        var t = lColumns.Select(x => $@"IFNULL({x},'')");
                        content = String.Join(",", t);
                        break;
                    case "SQLITE":
                    case "SL3":
                        var t1 = lColumns.Select(x => $@"COALESCE({x},'')");
                        content = String.Join("+", t1);
                        //content = String.Join("||", lColumns); // || supports null values
                        break;
                    default:
                        content = String.Join(",", lColumns);
                        break;
                }

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = content;
                        break;
                    case "MYSQL":
                        replacement = $@"Concat({content})";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = content;
                        break;
                    default:
                        replacement = $@"Concat({content})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }
           
            return sql;
        }
        /// <summary>
        /// macro ToString
        /// #ToInt string#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        // ReSharper disable once InconsistentNaming
        static string macroToString(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#ToString\s+([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Format({match.Groups[1].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"{match.Groups[1].Value.Trim()}";
                        break;
                    default:
                        replacement = $"{match.Groups[1].Value.Trim()}";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }
        /// <summary>
        /// macro ToBool
        /// #ToBool string#
        /// Returns the string 'TRUE', or 'FALSE'
        /// MySQL: 0=FALSE, #0=TRUE
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns>'true','false'</returns>

        // ReSharper disable once InconsistentNaming
static string macroToBool(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#ToBool\s+([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        // native SQL
                        replacement = $"{match.Groups[1]}";
                        break;
                    case "MYSQL":
                        replacement = $"if({match.Groups[1].Value.Trim()} = 0,'FALSE','TRUE')";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"iif({match.Groups[1].Value.Trim()} = 0,'FALSE','TRUE')";
                        break;
                    default:
                        replacement = match.Groups[1].Value.Trim().ToLower();
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }


        /// <summary>
        /// macro Left
        /// #Left string, position#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroLeft_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#Left\s+([^,]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Left({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Left({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()})";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"Substr({match.Groups[1].Value.Trim()},1,{match.Groups[2].Value.Trim()})";
                        break;
                    default:
                        replacement = $"Substr({match.Groups[1].Value.Trim()},1,{match.Groups[2].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }
        /// <summary>
        /// macro ToLower
        /// #ToLower string#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroToLower_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#ToLower\s+([^#]*)#", RegexOptions.IgnoreCase);
            {
                while (match.Success)
                {

                    string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                            replacement = $"LCase({match.Groups[1].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Lower({match.Groups[1].Value.Trim()})";
                        break;
    case "SQLITE":
                    case "SL3":
                        replacement = $"Lower({match.Groups[1].Value.Trim()})";
                            break;
                    default:
                        replacement = $"LCase({match.Groups[1].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
                }
            }

            return sql;
        }

        /// <summary>
        /// macro ToLower
        /// #ToLower string#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroToUpper_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#ToUpper\s+([^#]*)#", RegexOptions.IgnoreCase);
            {
                while (match.Success)
                {

                    string replacement;
                    switch (RepType(rep))
                    {
                        case "JET":
                        case "ACCESS":
                            replacement = $"UCase({match.Groups[1].Value.Trim()})";
                            break;
                        case "MYSQL":
                            replacement = $"Upper({match.Groups[1].Value.Trim()})";
                            break;
                        case "SQLITE":
                        case "SL3":
                            replacement = $"Upper({match.Groups[1].Value.Trim()})";
                            break;
                        default:
                            replacement = $"UCase({match.Groups[1].Value.Trim()})";
                            break;
                    }

                    sql = sql.Replace(match.Groups[0].Value, replacement);
                    match = match.NextMatch();
                }
            }
            return sql;

        }


        /// <summary>
        /// macro Right
        /// #Right string, position#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroRight_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#Right\s+([^,]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Right({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Right({match.Groups[1].Value.Trim()}, {match.Groups[2].Value.Trim()})";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"Substr({match.Groups[1].Value.Trim()}, -{match.Groups[2].Value.Trim()})";
                        break;

                    default:
                        replacement = $"Right({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }
        
        static string macroLength_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#Length\s+([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Len({match.Groups[1].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Length({match.Groups[1].Value.Trim()})";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"Length({match.Groups[1].Value.Trim()})";
                        break;

                    default:
                        replacement = $"Length({match.Groups[1].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }
        /// <summary>
        /// macro Substring
        /// #Substring string, start, length#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroSubstring_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#Substring\s+([^,]*),\s*([^#]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Mid({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Substr({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    default:
                        replacement = $"Substr({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }
        /// <summary>
        /// macro if
        /// #If condition, trueValue, falseValue#
        /// Checks whether a condition is met, and returns one value if TRUE of another on if it is FALSE.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroIf_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#If\s+([^,]*),\s*([^#]*),\s*([^#]*)#",RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"iif({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"if({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"iif({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    default:
                        replacement = $"if({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }
        /// <summary>
 /// macro Format a number with left padding and thousand separator
        /// #Format numberString, length, stringPaddingBefore#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string MacroFormat(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#Format\s+([^,]*),\s*([^#]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                var stringPaddingBefore = string.Concat(Enumerable.Repeat(match.Groups[3].Value.Trim(), 20));
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                         replacement = $"Right (\"{stringPaddingBefore}\" & Format({match.Groups[1].Value.Trim()}, \"#,###\"),{match.Groups[2].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Right (Concat(\"{stringPaddingBefore}\", Format({match.Groups[1].Value.Trim()}, 0)), {match.Groups[2].Value.Trim()} )";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"Substring( \"{stringPaddingBefore}\" || printf (\"%,d\",{match.Groups[1].Value.Trim()}),-{match.Groups[2].Value.Trim()}, {match.Groups[2].Value.Trim()})";
                        break;
                    default:
                        replacement = $"Right (\"{stringPaddingBefore}\" & Format({match.Groups[1].Value.Trim()}, \"#,###\"),{match.Groups[2].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }


            return sql;
        }
        /// <summary>
        /// macro LPad Pad a string with leading string/character
        /// #LPad string, length, stringPaddingBefore#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string MacroLPad(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#LPad\s+([^,]*),\s*([^#]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                var stringPaddingBefore = string.Concat(Enumerable.Repeat(match.Groups[3].Value.Trim(), 20));
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Right (\"{stringPaddingBefore}\" & {match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Right (Concat(\"{stringPaddingBefore}\", {match.Groups[1].Value.Trim()}), {match.Groups[2].Value.Trim()} )";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"Substring( \"{stringPaddingBefore}\" || {match.Groups[1].Value.Trim()},-{match.Groups[2].Value.Trim()}, {match.Groups[2].Value.Trim()})";
                        break;
                    default:
                        replacement = $"Right (\"{stringPaddingBefore}\" & {match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }


            return sql;
        }
        /// <summary>
        /// macro RPad Pad a string with trailing string/character
        /// #RPad string, length, stringPaddingAfter#
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string MacroRPad(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#RPad\s+([^,]*),\s*([^#]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                var stringPaddingAfter = string.Concat(Enumerable.Repeat(match.Groups[3].Value.Trim(), 20));
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        replacement = $"Left ({match.Groups[1].Value.Trim()} & \"{stringPaddingAfter}\",{match.Groups[2].Value.Trim()})";
                        break;
                    case "MYSQL":
                        replacement = $"Left (Concat( {match.Groups[1].Value.Trim()}, \"{stringPaddingAfter}\"), {match.Groups[2].Value.Trim()} )";
                        break;
                    case "SQLITE":
                    case "SL3":
                        replacement = $"Substring(  {match.Groups[1].Value.Trim()} || \"{stringPaddingAfter}\",1, {match.Groups[2].Value.Trim()})";
                        break;
                    default:
                        replacement = $"Left ({match.Groups[1].Value.Trim()} & \"{stringPaddingAfter}\",{match.Groups[2].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }


            return sql;
        }
        /// <summary>
        /// macro #InStr start, stack, needle#
        /// InStr: Searches for needle in stack and gives the position
        ///
        /// SQLITE StartPosition is always 1 (position rel. 1)
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql"></param>
        /// <returns></returns>

        static string macroInstr_ID(Repository rep, string sql)
        {
            Match match = Regex.Match(sql, @"#InStr\s+([^,]*),\s*([^#]*),\s*([^#]*)#", RegexOptions.IgnoreCase);
            while (match.Success)
            {

                string replacement;
                switch (RepType(rep))
                {
                    case "JET":
                    case "ACCESS":
                        // Instr(position,stack, needle)
                        replacement = $"InStr({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    case "MYSQL":
                        // locate(needle, stack, needle)
                        replacement = $"locate({match.Groups[3].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[1].Value.Trim()})";
                        break;
                    case "SQLITE":
                    case "SL3":
                        // Instr(stack, needle)
 replacement = $"InStr({match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                    default:
                        replacement = $"InStr({match.Groups[1].Value.Trim()},{match.Groups[2].Value.Trim()},{match.Groups[3].Value.Trim()})";
                        break;
                }
                sql = sql.Replace(match.Groups[0].Value, replacement);
                match = match.NextMatch();
            }

            return sql;
        }

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
                EA.Diagram dia;
                if (rep.GetContextItemType() == ObjectType.otDiagram)
                {
                    dia = (EA.Diagram)rep.GetContextObject();
                }
                else
                {
                    dia = rep.GetCurrentDiagram();
                }
                // Diagram selected?
                if (dia == null)
                {
                    // replace by empty list of IDs
                    sql = sql.Replace(template, $@" 0 ");
                }
                else
                {
                    // replace by list of IDs
                    sql = sql.Replace(template, $@"{dia.DiagramID}");
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
                EA.Diagram dia;
                if (rep.GetContextItemType() == ObjectType.otDiagram)
                {
                    dia = (EA.Diagram)rep.GetContextObject();
                } else
                {
                    dia = rep.GetCurrentDiagram();
                }
                // Diagram selected?
                if (dia == null)
                {
                    // replace by empty list of IDs
                    sql = sql.Replace(template, $" 0 ");
                }
                else
                {
                    // make a list of comma separated IDs
                    string listId = "0";
                    foreach (var el in dia.DiagramObjects)
                    {
                        int id = ((EA.DiagramObject)el).ElementID;
                        listId = $@"{listId},{id}";
                    }

                    // replace by list of IDs
                    sql = sql.Replace(template, $@"{listId}");
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
                EA.Diagram dia = rep.GetContextItemType() == ObjectType.otDiagram ? (EA.Diagram)rep.GetContextObject() : rep.GetCurrentDiagram();
                // Diagram selected?
                if (dia == null)
                {
                    // Replace by empty list of IDs
                    sql = sql.Replace(template, $@" 0 ");
                }
                else
                {
                    // make a list of comma separated IDs
                    string listId = "0";
                    foreach (var el in dia.SelectedObjects)
                    {
                        int id = ((EA.DiagramObject)el).ElementID;
                        listId = $@"{listId},{id}";

                    }

                    // replace by list of IDs
                    sql = sql.Replace(template, $@"{listId}");
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
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        id = dia.DiagramID;
                        break;
                    case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        id = pkg.PackageID;
                        break;
                    case ObjectType.otAttribute:
                        Attribute attr = (Attribute)rep.GetContextObject();
                        id = attr.AttributeID;
                        break;
                    case ObjectType.otMethod:
                        Method method = (Method)rep.GetContextObject();
                        id = method.MethodID;
                        break;
   default:
                        id = 0;
                        break;
                }

                if (id > 0)
                {
                    sql = sql.Replace(template, $@"{id}");
                    sql = sql.Replace("#CurrentElementID#", $@"{id}");// Alias for EA compatibility
                }
                else
                // no diagram, element or package selected
                {
                    // replace by empty list of IDs
                    sql = sql.Replace(template, $@" 0 ");
                    sql = sql.Replace("#CurrentElementID#", $@" 0 ");// Alias for EA compatibility
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
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                        guid = dia.DiagramGUID;
                        break;
                    case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        guid = pkg.PackageGUID;
                        break;
                    case ObjectType.otAttribute:
                        Attribute attr = (Attribute)rep.GetContextObject();
                        guid = attr.AttributeGUID;
                        break;
                    case ObjectType.otMethod:
                        Method method = (Method)rep.GetContextObject();
                        guid = method.MethodGUID;
                        break;
                }

                if (guid != "")
                {
                    sql = sql.Replace(template, $@"{guid}");
                    sql = sql.Replace("#CurrentElementGUID#", $"{guid}");// Alias for EA compatibility
                }
                else
                // no diagram, element or package selected
                {
                    // replace by empty list of IDs
                    sql = sql.Replace(template, $@" ' ' ");

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
                    sql = sql.Replace(currentPackageTemplate, $@"{id}");
                }
                else
                // no diagram, element or package selected
                {
                    // replace by empty list of IDs
                    sql = sql.Replace(currentPackageTemplate, $@" 0 ");
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
                    EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
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
                    Attribute attr = (Attribute)rep.GetContextObject();
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
                    Connector con = (EA.Connector)rep.GetContextObject();
                    if (sql.Contains(currentConnectorTemplate))
                    {
                        
                        sql = sql.Replace(currentConnectorTemplate, $@"{con.ConnectorID}");
                    }
                    // conveyed items are a comma separated list of elementIDs
                    if (sql.Contains(currentConveyedItemTemplate))
                    {

                        // to avoid syntax error, 0 will never fit any conveyed item
                        string conveyedItems = "0";

                        // first get "InformationFlows" which carries the conveyed items
                        if (con.MetaType == "Connector" || con.MetaType == "Sequence")
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
                                    Connector flow = rep.GetConnectorByGuid(flowGuid);
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
                    // Replace by empty list

                    if ( sql.Contains(currentConveyedItemTemplate)) sql = sql.Replace(currentConveyedItemTemplate, " 0 ");

                    if (sql.Contains(currentConnectorTemplate) ) sql = sql.Replace(currentConnectorTemplate, " 0 ");
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
                        // Replace by empty list of IDs
                        sql = sql.Replace(template, " ' ' ");
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
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
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
                    // Replace by empty list of IDs
                    sql = sql.Replace(currentBranchTemplate, " 0 ");
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

            // The macros to support
            foreach (SqlTemplateId id in new[]
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
                            sql = sql.Replace(match.Groups[0].Value, " ");
                            
                                return sql;

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
        /// Replace macro #CondBranchStatement operator, column# by a sql statement checking the recursive IDs of the selected Browser Packages, recursive nested. 
        /// It uses the package the Package selected in Browser tree or removes the macro all together.
        /// ... stands for the comma separated Package ids
        ///
        /// Example:
        /// - #CondBranchStatement#                         ==> 'AND pkg.Package_id in (...) '
        /// - #CondBranchStatement AND, pkg.Package_Id, in# ==> 'AND pkg.Package_Id in (...) '
        ///
        /// Optional parameters:
        /// - Operator1  (e.g. 'AND')
        /// - Column     (e.g. 't.package_id')
        /// - Operator2  (e.g. 'in')
        /// Result: ' operator1 column operator 2 (...) ' 
        /// 
        /// If no optional parameter is used:
        /// ' AND pkg.Package_id in (....) '
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">The sql string to replace the macro by the found ID</param>
        /// <returns>sql string with replaced macro</returns>
        static string MacroCondBranchStatement(Repository rep, string sql)
        {
            // Branch=comma separated Package IDs, Recursive:
            // Example for 3 Packages with their PackageID 7,29,128
            // 7,29,128
            //
            // Branch: complete SQL IN statement ' AND package_id in (comma separated Package IDs, Recursive):
            // IN (7,29,128)
            string currentBranchTemplate = GetTemplateText(SqlTemplateId.CondBranchStatement);
            if (sql.Contains(currentBranchTemplate))
            {
                ObjectType objectTypeContext = rep.GetContextItem(out _);
                ObjectType objectType = rep.GetTreeSelectedItem(out object obj);
                int id = 0;
                if (objectTypeContext != ObjectType.otPackage) return sql.Trim();
                switch (objectType)
                {
                   case ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)obj;
                        id = pkg.PackageID;
                        break;
                }
                // Context element available
                if (id > 0)
                {
                    // get package recursive
                    string branch = GetBranch(rep, "", id);
                    // find #CondBranchStatement...# with optional parameter
                    var pattern = $@"{currentBranchTemplate.Remove(currentBranchTemplate.Length - 1)}([^#]*)#";
                    Match match = Regex.Match(sql,pattern , RegexOptions.IgnoreCase);
                    while (match.Success)
                    {
                        var lColumns = match.Groups[1].Value.Split(',');
                        if (lColumns.Length == 3)
                            sql = sql.Replace(match.Groups[0].Value, $@" {lColumns[0]} {lColumns[1]} {lColumns[2]} ({branch}) ");
                        else
                        {
                            sql = sql.Replace(match.Groups[0].Value, $@" AND pkg.package_id in ({branch}) ");
                        }
                        match = match.NextMatch();
                    }

                }
                else
                // no diagram, element or package selected
                {
                    // replace by empty statement
                    sql = sql.Replace(currentBranchTemplate, $@" ");
                }
            }
            return sql.Trim();
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
            string currentBranchTemplate = GetTemplateText(SqlTemplateId.BranchIdsConstantPackage);
            if (sql.Contains(currentBranchTemplate) )
            {
                ObjectType objectType = rep.GetContextItemType();
                int id = 0;
                switch (objectType)
                {
                    // use Package of diagram
                    case ObjectType.otDiagram:
                        EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
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
                    string branch = GetBranch(rep, "", id);
                    sql = sql.Replace(currentBranchTemplate, branch);
                   
                }
                else
                // no diagram, element or package selected
                {
                    // replace by empty list of IDs
                    sql = sql.Replace(currentBranchTemplate, $@" 0 ");
                }
            }
            return sql;
        }
       /// <summary>
        /// Get list of package ids as comma separated list
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="branch"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetBranch(Repository rep, string branch, int id)
        {
            if (id > 0)
            {
                // add current package id
                if (branch == "") branch = id.ToString();
                else branch = branch + ", " + id;

                EA.Package pkg = rep.GetPackageByID(id);
                foreach (EA.Package p in pkg.Packages)
                {
                    int pkgId = p.PackageID;
                    branch = GetBranch(rep, branch, pkgId);
                }


            }
            return branch;
        }
        /// <summary>
        /// Repository type for macros
        ///
        /// Rational:
        /// Adapt special Repository Types like:
        /// - ACCESS
        /// - ACCESS207
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        private static string RepType(Repository rep)
        {
            string repType = rep.RepositoryType().ToUpper();
            if (repType.StartsWith("ACCESS")) return "ACCESS";
            return repType;
        }
     /// <summary>
        /// Format DB specific by removing unnecessary DB specific string parts.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        //#DB=Asa#                DB specif SQL for Asa
        //#DB=FIREBIRD#           DB specif SQL for FIREBIRD
        //#DB=JET#                DB specif SQL for JET
        //#DB=MySql#              DB specif SQL for My SQL
        //#DB=ACCESS2007#         DB specif SQL for ACCESS2007
        //#DB=ORACLE#             DB specif SQL for Oracle
        //#DB=POSTGRES#           DB specif SQL for POSTGRES
        //#DB=SqlSvr#             DB specif SQL for SQL Server
        //#DB=SQLite#             DB specif SQL for SQLite
        private static string FormatSqlDbSpecific(Repository rep, string sql)
        {
            // available DBs
            // Key: Repository.RepositoryType() 'Access2007' transformed to 'Access'
            var dbs = new Dictionary<string, string>()
            {
                { "ACCESS", "#DB=ACCESS2007#" }, // documentation 
                { "Asa", "#DB=Asa#" },
                { "Firebird", "#DB=FIREBIRD#" },
                { "JET", "#DB=JET#" },
                { "MYSQL", "#DB=MySql#" },
                { "Oracle", "#DB=ORACLE#" },
                { "Postgres", "#DB=POSTGRES#" },
                { "SqlSvr", "#DB=SqlSvr#" },
                { "Other", "#DB=Other#" },
                //{ "SQLite", "#DB=SQLITE#" },
                { "SL3", "#DB=SQLITE#" }

            };
            var dbType = dbs.Where(x=>x.Key == RepType(rep)).Select(x=>x.Value).FirstOrDefault();
            if (String.IsNullOrEmpty(dbType))
            {
                MessageBox.Show($@"DB not supported in SQL, only 'ACCESS', 'MYSQL', 'JET', 'SQLite', 'SL3'!
{sql}",$@"DB {RepType(rep)} not supported");
                return sql;
            }
            string s = sql;
            foreach (var curDb in dbs)
            {
                if (curDb.Key.ToLower() != RepType(rep).ToLower())
                {   // delete not used DBs
                    string delete = $"{curDb.Value}.*?{curDb.Value}";
                    s = Regex.Replace(s, delete, "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                }

            }
            // delete remaining DB identifying string
            s = Regex.Replace(s, @"#DB=(Asa|FIREBIRD|JET|MySql|ORACLE|ACCESS2007|POSTGRES|SqlSvr|SQLITE)#", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            // delete multiple empty lines
            for (int i = 0; i < 4; i++)
            {
                s = Regex.Replace(s, "\r\n\r\n", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
            return s;
        }
    }
    /// <summary>
    /// The RegEx show column definition
    /// </summary>
    public class RegExShowColumn
    {
        public RegExShowColumn(string srcColumn, string trgColumn, string regExString)
        {
            SrcColumn = srcColumn;
            TrgColumn = trgColumn;
            RegExString = regExString;

        }
        /// <summary>
        /// Get the regex
        /// </summary>
        /// <returns></returns>
        public Regex GetRegEx()
        {
            try
            {
                var regex = new Regex(RegExString);
                return regex;

            }
            catch (Exception e)
            {
                MessageBox.Show($@"String: {RegExString}

{e}", @"Error Regex");
                return null;
            } 
            
        }
        public string SrcColumn;
        public string TrgColumn;
        public string RegExString;
    }
    /// <summary>
    /// The RegEx to filter rows due to column content
    /// </summary>
    public class RegExFilterRows
    {
        public RegExFilterRows(string condition, string column, string regExString)
        {
            Condition = condition;
            Column = column;
            RegExString = regExString;

        }
        /// <summary>
        /// Get the regex
        /// </summary>
        /// <returns></returns>
        public Regex GetRegEx()
        {
            try
            {
                var regex = new Regex(RegExString);
                return regex;

            }
            catch (Exception e)
            {
                MessageBox.Show($@"String: {RegExString}

{e}", @"Error Regex");
                return null;
            }

        }
        public string Condition;
        public string Column;
        public string RegExString;
    }
}