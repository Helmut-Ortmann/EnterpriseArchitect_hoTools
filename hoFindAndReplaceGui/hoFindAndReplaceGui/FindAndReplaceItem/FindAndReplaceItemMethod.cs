using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemMethod : FindAndReplaceItem
    {
        EA.Method _meth;
        #region Constructor
        public  FindAndReplaceItemMethod(EA.Repository rep, string GUID)  :base(rep, GUID)
        {
            this._meth = rep.GetMethodByGuid(GUID);
            this.load(rep);
        }
        #endregion
        #region Property
        public EA.Method Meth => _meth;
        //override public List<FindAndReplaceItemTag> l_itemTag
        //{
        //    get { return _l_itemTag; }
        //}
        #endregion
        #region load
        public override void load(EA.Repository rep)
        {

            _Name = _meth.Name;
            _Description = _meth.Notes;
            _Stereotype = _meth.StereotypeEx;
         
        }
        #endregion
        #region save
        public override void save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {
            EA.Method meth = rep.GetMethodByGuid(GUID);
            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { meth.Notes = _Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   meth.Name = _Name; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { meth.StereotypeEx = _Stereotype; }
            _isUpdated = true;
            meth.Update();
        }
        #endregion
        #region locate
        public override void locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_meth);
        }
        #endregion
        public override string getType() => "Method";
        public override string getSubType() => "Method";
    }
}
