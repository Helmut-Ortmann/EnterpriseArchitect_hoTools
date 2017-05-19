namespace hoTools.Utils.Diagram
{
    /// <summary>
    /// General item to specify the style of an EA stylable item. See the subtypes. 
    /// </summary>
    public class DiagramGeneralStyleItem
    {
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }
        public string Style { get; }
        public string Property { get; }

        protected DiagramGeneralStyleItem(string name, string description, string type,string style, string property)
        {
            Name = name;
            Description = description;
            Type = type;
            Style = style;
            Property = property;
        }
    }
}
