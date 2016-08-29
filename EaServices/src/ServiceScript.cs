using System;
using AddinFramework.Util.Script;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    public class ServiceScript : Service
    {
        public ScriptFunction Function { get; }
        /// <summary>
        /// Create a ServiceScript with Reference to Function and Service Parameter:
        /// <para/>ID=ScriptName:FunctionName
        /// <para/>Description=ScriptName:FunctionName  (used to display the function)
        /// <para/>Help=Function.Description            (used to show help, not description!!)
        /// </summary>
        /// <param name="function"></param>
        public ServiceScript(ScriptFunction function) : base($"{function.Owner.Name}:{function.Name}", $"{function.Owner.Name}:{function.Name}", function.Description)
        {
            Function = function;
        }
        // public ServiceScript(ScriptFunction function) : base($"{function.Owner.Name}:{function.Name}", $"{function.Owner.Name}:{function.Name}", function.Owner.LanguageName)

    }
}
