using Newtonsoft.Json;

namespace hoTools.Utils.Diagram
{
    /// <summary>
    /// Item to specify the style of an EA Diagram
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiagramStyleItem : DiagramGeneralStyleItem
    {

        public string Pdata { get;  }
        public string StyleEx { get; }
        /// <summary>
        /// Item to specify the style of an EA DiagramObject
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Global
        [JsonConstructor]
        public DiagramStyleItem(string name, string description, string type, string pdata, string styleEx, string property)
            : base(name, description, type, $"{pdata};{styleEx}", property)
        {
            Pdata = pdata;
            StyleEx = styleEx;
        }
    }
}
