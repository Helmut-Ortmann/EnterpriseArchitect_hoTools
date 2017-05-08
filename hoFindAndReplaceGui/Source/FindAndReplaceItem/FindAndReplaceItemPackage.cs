using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemPackage : FindAndReplaceItem
    {
        EA.Package _pkg;
        EA.Element _el ;
        #region Contructor
        /// <summary>
        /// Create a package element. Be aware that a package element also contains an element to support
        /// element specific things like tagged values.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="GUID"></param>
        public  FindAndReplaceItemPackage(EA.Repository rep, string GUID)  :base( rep, GUID)
        {
            this._el = rep.GetElementByGuid(GUID);
            this._pkg = rep.GetPackageByGuid(GUID);
            this.load(rep);
        }
        #endregion
        #region Property
        public EA.Element Element => _el;
        public EA.Package Package => _pkg;

        #endregion
        #region load
        public override void load(EA.Repository rep)
        {

            _Name = _pkg.Name;
            _Description = _pkg.Notes;
            _Stereotype = _pkg.StereotypeEx;


            // Model don't have an element
            if (_pkg.ParentID != 0)
            {
                EA.Element elPkg = rep.GetElementByGuid(GUID);
                _Stereotype = elPkg.StereotypeEx;
            }
        }
        #endregion
        #region save
        public override void save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {
            _pkg = rep.GetPackageByGuid(GUID);
            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { _pkg.Notes = _Description; }
            if ((fieldType & (FindAndReplaceItem.FieldType.Name | FindAndReplaceItem.FieldType.Stereotype) ) > 0)
            {
                // model don't have an element
                if (_pkg.ParentID != 0)
                {
                    EA.Element el = rep.GetElementByGuid(GUID);
                    el.StereotypeEx = _Stereotype;
                    el.Name = _Name;
                    el.Update();
                }
                _pkg.Name = _Name;
            }
            _isUpdated = true;
            _pkg.Update();
        }
        #endregion
        #region locate
        public override void locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_pkg);
        }
        #endregion
        public override string getType() => "Package";
        public override string getSubType() => "Package";
    }
}
