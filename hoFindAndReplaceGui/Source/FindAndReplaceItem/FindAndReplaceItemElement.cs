using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemElement : FindAndReplaceItem
    {
        readonly EA.Element _el;
        #region Constructor
        public  FindAndReplaceItemElement(EA.Repository rep, string GUID)  :base(rep, GUID)
        {
            this._el = rep.GetElementByGuid(GUID);
            this.load(rep);
        }
        #endregion
        #region Property
        public EA.Element Element => this._el;

        #endregion
        #region load
        public override void load(EA.Repository rep)
        {
            _Name = _el.Name;
            _Description = _el.Notes;
            _Stereotype = _el.StereotypeEx;
         
        }
        #endregion
        #region ReplaceItem
        #endregion
        #region save
        public override void save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {
            
            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { _el.Notes = _Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { _el.StereotypeEx = _Stereotype; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   _el.Name = _Name; }
            _isUpdated = true;
            _el.Update();
        }
        #endregion
        #region locate
        public override void locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_el);
        }
        #endregion
        public override string getType() => "Element";
        public override string getSubType() => _el.Type;
    }
}
