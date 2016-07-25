using System;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    public class ServiceScript : Service
    {
        public ScriptFunction Function;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        public ServiceScript(ScriptFunction function) : base($"{function.Owner.Name}:{function.Name}", $"{function.Owner.Name}:{function.Name}", function.Owner.LanguageName)
        {
            Function = function;
        }

    }
}
