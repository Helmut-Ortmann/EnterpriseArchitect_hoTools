using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hoTools.Find
{
    /// <summary>
    /// Item found and might be changed.
    /// </summary>
    public class FindAndReplaceItem
    {
        private readonly string _guid;
        private string _name;
        private string _description;
        private string _stereotype; // from .Sterotype.Ex
        private int _countChanges;
        private bool _isUpdated;

        // tagged values
        private readonly List<FindAndReplaceItemTag> _lItemTag = new List<FindAndReplaceItemTag>();

        #region FieldType
        // field type which allows multiple values
        [Flags]
        public enum FieldType
        {
            None = 0,
            Name = 1,
            Description = 2,
            Tag = 4,
            Stereotype = 8
        }
        #endregion
        #region Constructor
        public FindAndReplaceItem(
            string guid,
            string name,
            string description,
            string stereotype,
            int countChanges)
        {
            _guid = guid;
            _name = name;
            _description = description;
            _stereotype = stereotype;
            _countChanges = countChanges;
            _isUpdated = false;
        }
        public FindAndReplaceItem( EA.Repository rep, string guid)
        {
            _guid = guid;
            _countChanges = 0;
            _isUpdated = false;

        }
        #endregion
        #region Properties
        public string Guid => _guid;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public string Stereotype
        {
            get => _stereotype;
            set => _stereotype = value;
        }

        public List<FindAndReplaceItemTag> LItemTag => _lItemTag;

        public int CountChanges
        {
            get => _countChanges;
            set => _countChanges = value;
        }
       
        public bool IsUpdated
        {
            get => _isUpdated;
            set => _isUpdated = value;
        }
        #endregion
        #region findCount
        public int FindCount(Regex regExPattern, FieldType fieldType)
        {
            int count = 0;
            if ( (fieldType & FieldType.Name) >0 )  count = count + FindCountForType(regExPattern, _name);
            if ( (fieldType & FieldType.Description) > 0)  count = count + FindCountForType(regExPattern, _description);
            if ((fieldType & FieldType.Stereotype) > 0) count = count + FindCountForType(regExPattern, _stereotype);
            return count;
        }
        #endregion 
        #region findCountForType
        public static int FindCountForType(Regex regExPattern, string value)
        {
            int count = 0;
            Match match = regExPattern.Match(value);
            while (match.Success)
            {
                count = count + 1;
                match = match.NextMatch();
            }
            return count;
        }
        #endregion
        #region ToString
        public override string ToString()
        {
            string isUpdated = "";
            if (_isUpdated) isUpdated = "*";

            string stereotype = "";
            if (_stereotype != "") stereotype = "<<" +_stereotype + ">>"; 
            return String.Format("{4} '{0}:{1}'{2} {3} changes in item found ", GetSubType(), _name, stereotype,  _countChanges, isUpdated);

        }
        #endregion
        public virtual void Locate(EA.Repository rep) {}
        public virtual void Load(EA.Repository rep){}
        public virtual void Save(EA.Repository rep, FieldType fieldType) {}
        public virtual string GetSearchType() => "unknown";
        public virtual string GetSubType() => "unknown";


        #region static Factory
        public static FindAndReplaceItem Factory(EA.Repository rep, EA.ObjectType objectType, string guid)
        {
            switch (objectType)
            {
                case EA.ObjectType.otPackage:
                    return new FindAndReplaceItemPackage(rep, guid);

                case EA.ObjectType.otElement:
                    return new FindAndReplaceItemElement(rep, guid);

                case EA.ObjectType.otDiagram:
                    return new FindAndReplaceItemDiagram(rep, guid);

                case EA.ObjectType.otAttribute:
                    return new FindAndReplaceItemAttribute(rep, guid);

                case EA.ObjectType.otMethod:
                    return new FindAndReplaceItemMethod(rep, guid);

                default: return null;
            }
        }
        #endregion
    }
   
}
