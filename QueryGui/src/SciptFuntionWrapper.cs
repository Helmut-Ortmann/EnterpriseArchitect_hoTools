using System;
using System.Windows.Forms;

using EAAddinFramework.Utils;

namespace hoTools.Query
{
    /// <summary>
    /// Wrapper for Script Functions:
    /// - Messagebox if error
    /// - Configure if you want to show message box
    /// 
    /// </summary>
    public class ScriptFuntionWrapper
    {
        ScriptFunction _function = null;
        bool _isErrorOutput = true;

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
        public Boolean execute(object[] par) { 
            try
            {
                _function.execute(par);
                return true;
            }
            catch (Exception e)
            {
                if (_isErrorOutput) MessageBox.Show(e.ToString(), $"Error run Script  '{_function.fullName}()'");
                return false;

            }
       }

    }
}
