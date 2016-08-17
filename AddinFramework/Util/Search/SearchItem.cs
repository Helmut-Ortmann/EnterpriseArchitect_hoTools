
 // ReSharper disable once CheckNamespace

using System;
using System.IO;
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
        public string EARelease { get; set; }


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
        public SearchItem(double score, string name, string description, string category, bool favorite)
        {
            Score = score;
            Name = name;
            Description = description;
            Category = category;
            Favorite = favorite;
        }
        public SearchItem(double score, string name, string description, string category, bool favorite, string eaRelease)
        {
            Score = score;
            Name = name;
            Description = description;
            Category = category;
            Favorite = favorite;
            EARelease = eaRelease;
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

        [JsonConstructor]
        public SqlSearchItem(string longName):base(Path.GetFileName(longName))
        {
           Init(longName, "");
        }

        public SqlSearchItem(string name, string longName) : base(name)
        {
            Init(longName, "");
        }

        public SqlSearchItem(string name, string longName, string description ) : base(name)
        {
            Init(longName, description);
        }

        void Init(string longName, string description)
        {
            LongName = longName;
            Category = "SQL-File";
            Description = description;

        }

    }
}
