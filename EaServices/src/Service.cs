using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EA;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    public class Service
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string Help { get; set; }
        public string GUID { get; set; }
        public Service (string guid, string description, string help)
        {
            GUID = guid;
            Description = description;
            Help = help;
        }
    }
    public class ServiceScript : Service
    {
        private ScriptFunction _function;

        public ServiceScript(ScriptFunction function):base($"{function.Owner.Name}:{function.Name}", $"{function.Owner.Name}:{function.Name}", function.Owner.LanguageName)
        {
            _function = function;
        }
    
    }
}
