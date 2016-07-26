
using AddinFramework.Util;
using EAAddinFramework.Utils;


// ReSharper disable once CheckNamespace
namespace GlobalHotkeys
{
    public class GlobalKeysConfigScript : GlobalKeysConfig
    {
        /// <summary>
        /// Constructor Global Key Definition for a Script Function. Id="Script:Function" Name. 
        /// <para />It contains the key, ScriptFunction as Object, Description and Help.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifier1"></param>
        /// <param name="modifier2"></param>
        /// <param name="modifier3"></param>
        /// <param name="modifier4"></param>
        /// <param name="help"></param>
        /// <param name="scriptFunction"></param>
        /// <param name="description"></param>
        public GlobalKeysConfigScript(string key, string modifier1, string modifier2, string modifier3, string modifier4, 
                ScriptFunction scriptFunction, string description, string help)
                : base($"{scriptFunction.Owner.Name}:{scriptFunction.Name}", key, modifier1, modifier2, modifier3, modifier4, description, help)
            {
            ScriptFunction = scriptFunction;
        }

        #region GetterSetter
        public ScriptFunction ScriptFunction { get; set; }

        public string Guid { get { return Id; } set { Id = value; } }

        #endregion

        #region Invoke
        /// <summary>
        /// Invokes the ScriptFunction with 2 or 3 Parameters.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Invoke(Model model)
        {
            if (ScriptFunction != null)
            {
                EA.ObjectType eaObjectType = model.Repository.GetContextItemType();
                object eaObject = model.Repository.GetContextObject();
                ScriptUtility.RunScriptFunction(model, ScriptFunction, eaObjectType, eaObject);
            }
            return null;
        }
        #endregion Invoke
    }
}
