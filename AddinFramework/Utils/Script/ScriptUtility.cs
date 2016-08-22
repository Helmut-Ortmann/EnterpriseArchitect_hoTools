using System;
using System.Windows.Forms;
using AddinFramework.Util.Script;
using EAAddinFramework.Utils;

namespace AddinFramework.Util
{
    public class ScriptUtility
    {
        /// <summary>
        /// Run function for EA item of arbitrary type<par></par>
        /// - If parameter count = 2 it calls the function with oType, oContext<par></par>
        /// - If parameter count = 3 it calls the function with oType, oContext, Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="function">Function</param>
        /// <param name="oType">EA Object type</param>
        /// <param name="oContext">EA Object</param>
        /// <returns></returns>
        public static bool RunScriptFunction(Model model, ScriptFunction function, EA.ObjectType oType, object oContext)
        {
            // run script according to parameter count
            switch (function.NumberOfParameters)
            {
                case 2:
                    object[] par2 = { oContext, oType };
                    new ScriptFuntionWrapper(function).Execute(par2);
                    return true;
                case 3:
                    object[] par3 = { oContext, oType, model };
                    return new ScriptFuntionWrapper(function).Execute(par3);
                default:
                    MessageBox.Show($"Script {function.FullName}  has {function.NumberOfParameters} parameter",
                        @"Script function parameter count not 2 or 3, Break!");
                    return false;
            }

        }
    }
    /// <summary>
    /// Wrapper for Script Functions:
    /// - Message box if error
    /// - Configure if you want to show message box
    /// 
    /// </summary>
    public class ScriptFuntionWrapper
    {
        readonly ScriptFunction _function;
        readonly bool _isErrorOutput = true;

        public ScriptFuntionWrapper(ScriptFunction function)
        {
            _function = function;
        }
        public ScriptFuntionWrapper(ScriptFunction function, bool isErrorOutput)
        {
            _function = function;
            _isErrorOutput = isErrorOutput;

        }
        /// <summary>
        /// Execute function with or without error MessageBox
        /// </summary>
        /// <param name="par"></param>
        /// <returns>true=ok;false=nok</returns>
        public Boolean Execute(object[] par)
        {
            try
            {
                _function.Execute(par);
                return true;
            }
            catch (Exception e)
            {
                if (_isErrorOutput) MessageBox.Show($"Have you updated the Scripts (File, Update Scripts)??\r\n\r\n{e.ToString()}", $"Error run Script  '{_function.FullName}()'");
                return false;

            }
        }

    }
}
