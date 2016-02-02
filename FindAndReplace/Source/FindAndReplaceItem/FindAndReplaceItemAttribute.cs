using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemAttribute : FindAndReplaceItem
    {
        private EA.Attribute _attr = null;
        #region Constructor
        public  FindAndReplaceItemAttribute(EA.Repository rep, string GUID)  :base(rep, GUID)
        {
            this._attr = rep.GetAttributeByGuid(GUID);
            this.load(rep);
        }
        #endregion
        #region Property
        public EA.Attribute Attr => _attr;
        #endregion
        #region load
        public override void load(EA.Repository rep)
        {
            _attr = rep.GetAttributeByGuid(GUID);
            _Name = _attr.Name;
            _Description = _attr.Notes;
            _Stereotype = _attr.StereotypeEx;
         
        }
        #endregion
        #region save
        public override void save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
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
        public override void locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_attr);
        }
        #endregion
        public override string getType() => "Attribute";
        public override string getSubType() => "Attribute";
    }
}
