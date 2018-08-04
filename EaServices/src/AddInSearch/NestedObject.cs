using System.Collections.Generic;

namespace hoTools.EaServices.AddInSearch
{
    public class NestedObject
    {
        public int ParentId;
        public int TPos;
        public string Name;
        public string Alias;
        public string Notes;
        public List<Tv> Tv;
        public string Guid;
        public string ObjectType;
        public NestedObject(int parentId, int tPos, string name, string alias, string notes, string guid, string objectType,  List<Tv> tv)
        {
            ParentId = parentId;
            TPos = tPos;
            Name = name;
            Alias = alias;
            Notes = notes;
            Guid = guid;
            ObjectType = objectType;
            Tv = tv;

        }
    }
}
