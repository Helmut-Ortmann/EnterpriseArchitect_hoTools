
namespace AddinFramework.Util
{
    /// <summary>
    /// A Search Item to define a search
    /// </summary>
    public class SearchItem
    {
        public double Score { get; set; }
        public string Name { get; set; }


        public SearchItem(string name)
        {
            Score = 0.0;
            Name = name;

        }
    }
    /// <summary>
    /// An EA Search Item to define an EA Search
    /// </summary>
    public class EaSearchItem : SearchItem
    {
        public string MdgId { get; set; }

        public EaSearchItem(string id, string name): base(name)
        {
            MdgId = id;
        }
        
    }
    /// <summary>
    /// An SQL Search Item
    /// </summary>
    public class SqlSearchItem : SearchItem
    {
        public string LongName { get; set; }

        public SqlSearchItem(string name, string longName) : base(name)
        {
            LongName = longName;
        }

    }
}
