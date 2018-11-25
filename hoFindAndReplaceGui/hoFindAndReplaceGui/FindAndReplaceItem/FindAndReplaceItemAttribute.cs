using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemAttribute : FindAndReplaceItem
    {
        EA.Attribute _attr ;
        #region Constructor
        public  FindAndReplaceItemAttribute(EA.Repository rep, string GUID)  :base(rep, GUID)
        {
            this._attr = rep.GetAttributeByGuid(GUID);
            this.Load(rep);
        }
        #endregion
        #region Property
        public EA.Attribute Attr => _attr;
        #endregion
        #region load
        public override void Load(EA.Repository rep)
        {
            _attr = rep.GetAttributeByGuid(GUID);
            _Name = _attr.Name;
            _Description = _attr.Notes;
            _Stereotype = _attr.StereotypeEx;
         
        }
        #endregion
        #region save
        public override void Save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {

            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { _attr.Notes = _Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { _attr.StereotypeEx = _Stereotype; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   _attr.Name = _Name; }
            _isUpdated = true;
            _attr.Update();
        }
        #endregion
        #region locate
        public override void Locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_attr);
        }
        #endregion
        public override string GetSearchType() => "Attribute";
        public override string GetSubType() => "Attribute";
    }
}
