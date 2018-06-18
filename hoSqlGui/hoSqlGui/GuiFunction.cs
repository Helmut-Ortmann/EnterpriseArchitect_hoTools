using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml.Linq;
using AddinFramework.Util;
using AddinFramework.Util.Script;
using EAAddinFramework.Utils;

namespace hoTools.hoSqlGui
{
    /// <summary>
    /// GUI Layer for hoSqlGui
    /// </summary>
    public static class GuiFunction
    {
        /// <summary>
        /// Run selected script with 2/3 parameters for all rows of SQL. 
        /// If you choose isWithAsk=true
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <param name="function"></param>
        /// <param name="isWithAsk">Ask if execute, skip script execution or break altogether</param>
        /// <returns></returns>
        public static bool RunScriptWithAsk(Model model, string sql, ScriptFunction function, bool isWithAsk = false)
        {
            string scriptName = function.Owner.Name;
            string functionName = function.Name;
            int scriptParCount = function.NumberOfParameters;

            // Check parameter count of function
            if (scriptParCount < 2 || scriptParCount > 3)
            {
                MessageBox.Show($@"Function: '{scriptName}:{functionName} count of parameters={scriptParCount}", @"Count of parameters for function shall be 2 or 3 (object_type, Id, Model), Break!!!!");
                return false;
            }

            // get SQL
            string xml = model.SqlQueryWithException(sql);
            if (xml == null) return false;

            // Output the query in EA Search Window
            string target = model.MakeEaXmlOutput(xml);
            model.Repository.RunModelSearch("", "", "", target);

            // get rows / items to call function
            List<EaItem> eaItemList = model.MakeEaItemListFromQuery(XDocument.Parse(xml));
            int countCurrent = 0;
            int count = eaItemList.Count;
            foreach (EaItem item in eaItemList)
            {

                switch (scriptParCount)
                {
                    case 2:
                    case 3:
                        // run script
                        bool run = true;
                        if (isWithAsk)
                        {
                            // run the function with two or three parameters
                            DialogResult result = MessageBox.Show($@"Function '{functionName}', Item {countCurrent} of {count}", @"YES=Execute,No=Skip execution, Cancel=Break,", MessageBoxButtons.YesNoCancel);
                            if (result.Equals(DialogResult.No)) run = false;
                            if (result.Equals(DialogResult.Cancel)) return false;
                        }
                        if (run)  // run script
                        {
                            countCurrent += 1;
                            if (countCurrent%20 == 0)
                                    model.Repository.WriteOutput("Script", $"{functionName}: {countCurrent} of {count}", 0);
                            if (ScriptUtility.RunScriptFunction(model, function, item.EaObjectType, item.EaObject) == false) return false;
                        }
                        continue;
                    default:
                        MessageBox.Show($@"Script parameter count shall be 2 or 3, is {scriptParCount}", @"Invalid count of function parameters, Break!!!!");
                        break;

                }
            }
            return true;
        }

        
    }
}
