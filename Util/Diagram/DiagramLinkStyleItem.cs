using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hoTools.Utils.Diagram
{

    /// <summary>
    /// Item to specify the style of an EA DiagramObject
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiagramLinkStyleItem : DiagramGeneralStyleItem
    {
        [JsonConstructor]
        public DiagramLinkStyleItem(string name, string description, string type, string style, string property)
            :base(name, description, type, style, property)
        {
           
        }
    }
}
