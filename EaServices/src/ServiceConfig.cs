using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hoTools.EaServices
{
    public class ServicesConfig
    {
        public int Pos { get; set; }
        public string Id { get; set; }
        public string ButtonText { get; set; }
        public string Description { get; set; }
        public string Help { get; set; }

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
