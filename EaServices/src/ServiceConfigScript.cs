using AddinFramework.Util;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    /// <summary>
    /// Class to define the configurable Script
    /// </summary>
    public class ServicesConfigScript : ServicesConfig
    {
        public ScriptFunction Function { get; }

        public string functionName
        {
            get { return Id; }
            set { Id = value; }
        }

        public override string HelpTextLong
        {
            get
            {
                if (Function == null) return "";
                return
                    $"{"Script",10}: {ButtonText} / {Function.Owner.Name}:{Function.Name}\n{Description}\n{Help}";
            }

        }

        public ServicesConfigScript(int pos, ScriptFunction function, string buttonText)
            : base(pos, $"{function.Owner.Name}:{function.Name}", buttonText)
        {
            Function = function;
        }

        public string Invoke(Model model)
        {
            if (Function != null)
            {
                EA.ObjectType objectType = model.Repository.GetContextItemType();
                object oContext = (object)model.Repository.GetContextObject();

                ScriptUtility.RunScriptFunction(model, Function, objectType, oContext);

            }
            return null;
        }





    }
}
