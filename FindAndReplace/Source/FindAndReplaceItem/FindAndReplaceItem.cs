using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hoTools.Find
{
    #region Description
    // Internal representation of an item to change
    #endregion
    public class FindAndReplaceItem
    {
        protected string _GUID;
        protected string _Name;
        protected string _Description;
        protected string _Stereotype; // from .Sterotype.Ex
        protected int _countChanges;
        protected bool _isUpdated;

        // tagged values
        private  List<FindAndReplaceItemTag> _l_itemTag = new List<FindAndReplaceItemTag>();

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
            string GUID,
            string Name,
            string Description,
            string Stereotype,
            int CountChanges)
        {
            _GUID = GUID;
            _Name = Name;
            _Description = Description;
            _Stereotype = Stereotype;
            _countChanges = CountChanges;
            _isUpdated = false;
        }
        public FindAndReplaceItem( EA.Repository rep, string GUID)
        {
            _GUID = GUID;
            _countChanges = 0;
            _isUpdated = false;

        }
        #endregion
        #region Properties
        public string GUID => _GUID;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Stereotype
        {
            get { return _Stereotype; }
            set { _Stereotype = value; }
        }

        public List<FindAndReplaceItemTag> l_itemTag => _l_itemTag;

        public int CountChanges
        {
            get { return _countChanges; }
            set { _countChanges = value; }
        }
       
        public bool isUpdated
        {
            get { return _isUpdated; }
            set { _isUpdated = value; }
        }
        #endregion
        #region findCount
        public int findCount(Regex regExPattern, FieldType fieldType)
        {
            int count = 0;
            if ( (fieldType & FieldType.Name) >0 )  count = count + findCountForType(regExPattern, _Name);
            if ( (fieldType & FieldType.Description) > 0)  count = count + findCountForType(regExPattern, _Description);
            if ((fieldType & FieldType.Stereotype) > 0) count = count + findCountForType(regExPattern, _Stereotype);
            return count;
        }
        #endregion 
        #region findCountForType
        public static int findCountForType(Regex regExPattern, string value)
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
            if (_Stereotype != "") stereotype = "<<" +_Stereotype + ">>"; 
            return String.Format("{4} '{0}:{1}'{2} {3} changes in item found ", getSubType(), _Name, stereotype,  _countChanges, isUpdated);

        }
        #endregion
        public virtual void locate(EA.Repository rep) {}
        public virtual void load(EA.Repository rep){}
        public virtual void save(EA.Repository rep, FieldType fieldType) {}
        public virtual string getType() => "unknown";
        public virtual string getSubType() => "unknown";


        #region static Factory
        public static FindAndReplaceItem Factory(EA.Repository rep, EA.ObjectType objectType, string GUID)
        {
            switch (objectType)
            {
                case EA.ObjectType.otPackage:
                    return new FindAndReplaceItemPackage(rep, GUID);

                case EA.ObjectType.otElement:
                    return new FindAndReplaceItemElement(rep, GUID);

                case EA.ObjectType.otDiagram:
                    return new FindAndReplaceItemDiagram(rep, GUID);

                case EA.ObjectType.otAttribute:
                    return new FindAndReplaceItemAttribute(rep, GUID);

                case EA.ObjectType.otMethod:
                    return new FindAndReplaceItemMethod(rep, GUID);

                default: return null;
            }
        }
        #endregion
    }
   
}
