using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml.Linq;
using EAAddinFramework.Utils;
using hoTools.Utils.SQL;



namespace hoTools.Query
{
    /// <summary>
    /// GUI Layer for QueryGUI
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
            string scriptName = function.owner.name;
            string functionName = function.name;
            int scriptParCount = function.numberOfParameters;

            // Check parameter count of function
            if (scriptParCount < 2 || scriptParCount > 3)
            {
                MessageBox.Show($"Function: '{scriptName}:{functionName} count of parameters={scriptParCount}", "Count of parameters for function shall be 2 or 3 (object_type, GUID, Model), Break!!!!");
                return false;
            }

            // get SQL
            string xml = model.SqlQueryWithException(sql);
            if (xml == null) return false;

            // get rows / items to call function
            List<EaItem> eaItemList = model.MakeEaItemListFromQuery(XDocument.Parse(xml));
            int countCurrent = 0;
            int count = eaItemList.Count;
            foreach (EaItem item in eaItemList)
            {

                // get selected element and type
                EA.ObjectType oType = model.Repository.GetContextItemType();
                object oContext = model.Repository.GetContextObject();


                switch (scriptParCount)
                {
                    case 2:
                    case 3:
                        // run script
                        bool run = true;
                        if (isWithAsk)
                        {
                            // run the function with two or three parameters
                            DialogResult result = MessageBox.Show($"Function '{functionName}', Item {countCurrent} of {count}", "YES=Execute,No=Skip execution, Cancel=Break,", MessageBoxButtons.YesNoCancel);
                            if (result.Equals(DialogResult.No)) run = false;
                            if (result.Equals(DialogResult.Cancel)) return false;
                        }
                        if (run)  // run script
                        {
                            countCurrent += 1;
                            if (countCurrent%20 == 0)
                                    model.Repository.WriteOutput("Script", $"{functionName}: {countCurrent} of {count}", 0);
                            if (RunScriptFunction(model, function, item.EaObjectType, item.EaObject) == false) return false;
                        }
                        continue;
                    default:
                        MessageBox.Show($"Script parameter count shall be 2 or 3, is {scriptParCount}", "Invalid count of function parameters, Break!!!!");
                        break;

                }
            }
            return true;
        }
        /// <summary>
        /// Run function for EA item of arbitrary type<par></par>
        /// - If parameter count = 2 it calls the function with oType, oContext<par></par>
        /// - If parameter count = 3 it calls the function with oType, oContext, Model
        /// </summary>
        /// <param name="function">Function</param>
        /// <param name="oType">EA Object type</param>
        /// <param name="oContext">EA Object</param>
        /// <returns></returns>
        public static bool RunScriptFunction(Model model, ScriptFunction function, EA.ObjectType oType, object oContext)
        {
            // run script according to parameter count
            switch (function.numberOfParameters)
            {
                case 2:
                    object[] par2 = { oContext, oType };
                    return new ScriptFuntionWrapper(function).execute(par2);
                case 3:
                    object[] par3 = { oContext, oType, model };
                    return new ScriptFuntionWrapper(function).execute(par3);
                default:
                    MessageBox.Show($"Script {function.fullName}  has {function.numberOfParameters} parameter",
                        "Script function parameter count not 2 or 3, Break!");
                    return false;
            }

        }
                       
    }
}
