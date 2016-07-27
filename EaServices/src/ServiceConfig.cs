using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hoTools.EaServices
{
    public class ServicesConfig
    {
        // Empty service
        public const string ServiceEmpty = "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}";
        public int Pos { get; set; }
        public string Id { get; set; }
        public string ButtonText { get; set; }
        public string Description { get; set; }
        public string Help { get; set; }

        public virtual string HelpTextLong { get;  }

        public ServicesConfig(int pos, string id, string buttonText)
        {
            Init(pos, id, buttonText, "", "");
        }
        public ServicesConfig(int pos, string id, string buttonText, string description, string help)
        {
            Init(pos, id, buttonText, description, help);
        }
        private void Init(int pos, string id, string buttonText, string description, string help)
        {
            Pos = pos;
            Id = id;
            ButtonText = buttonText;
            Description = description;
            Help = help;
        }
    }
}
