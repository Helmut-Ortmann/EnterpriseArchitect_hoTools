using System;
using AddinFramework.Util;
using AddinFramework.Util.Script;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    /// <summary>
    /// Class to define the configurable Script
    /// </summary>
    public class ServicesConfigScript : ServicesConfig
    {
        public ScriptFunction Function { get; set; }

        public string FunctionName
        {
            get { return Id; }
            set
            {
                Id = value; 
            }
        }

        public override string HelpTextLong
        {
            get
            {
                if (Function == null) return "";
                return
                    $"{"Script",-10}: '{ButtonText}' / {Function.Owner.Name}:{Function.Name}{Environment.NewLine}{Description}{Environment.NewLine}{Help}";
            }

        }
        public ServicesConfigScript(int pos, string id, string buttonText)
            : base(pos, id, buttonText)
        {
    
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
                object[] par = { oContext, objectType,  };
                //Function.Execute(par);

                ScriptUtility.RunScriptFunction(model, Function,objectType, oContext);

            }
            return null;
        }





    }
}
