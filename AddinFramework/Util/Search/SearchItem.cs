
 // ReSharper disable once CheckNamespace

using System;
using Newtonsoft.Json;

namespace AddinFramework.Util
{
    /// <summary>
    /// A Search Item to define a Search like EA Model Search or SQL. 
    /// </summary>
    public class SearchItem
    {
        public double Score { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Favorite { get; set; }


        [JsonConstructor]
        public SearchItem(string name)
        {
            Name = name;
        }
        public SearchItem(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public SearchItem(string name, string description, string category)
        {
            Name = name;
            Description = description;
            Category = category;
        }
        public SearchItem(string name, string description, string category, bool favorite)
        {
            Name = name;
            Description = description;
            Category = category;
            Favorite = favorite;
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
    /// An SQL Search Item with name, longName.
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
