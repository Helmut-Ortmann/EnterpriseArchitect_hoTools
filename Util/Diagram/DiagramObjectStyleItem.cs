using Newtonsoft.Json;

namespace hoTools.Utils.Diagram
{
    /// <summary>
    /// Item to specify the style of an EA DiagramObject
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiagramObjectStyleItem:DiagramGeneralStyleItem
    {
        [JsonConstructor]
        public DiagramObjectStyleItem(string name, string description, string type, string style, string property)
            :base(name, description, type, style, property)
        {

        }
    }
}
