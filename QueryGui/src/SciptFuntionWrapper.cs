using System;
using System.Windows.Forms;

using EAAddinFramework.Utils;

namespace hoTools.Query
{
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
        public Boolean Execute(object[] par) { 
            try
            {
                _function.Execute(par);
                return true;
            }
            catch (Exception e)
            {
                if (_isErrorOutput) MessageBox.Show(e.ToString(), $"Error run Script  '{_function.FullName}()'");
                return false;

            }
       }

    }
}
