using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemPackage : FindAndReplaceItem
    {
        private readonly string _guid;
        EA.Package _pkg;
        EA.Element _el ;
        #region Contructor
        /// <summary>
        /// Create a package element. Be aware that a package element also contains an element to support
        /// element specific things like tagged values.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="guid"></param>
        public  FindAndReplaceItemPackage(EA.Repository rep, string guid)  :base( rep, guid)
        {
            _guid = guid;
            this._el = rep.GetElementByGuid(guid);
            this._pkg = rep.GetPackageByGuid(guid);
            this.Load(rep);
        }
        #endregion
        #region Property
        public EA.Element Element => _el;
        public EA.Package Package => _pkg;

        #endregion
        #region Load
        /// <summary>
        /// Load 
        /// </summary>
        /// <param name="rep"></param>
        public sealed override void Load(EA.Repository rep)
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
        #region Save
        public override void Save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
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
        #region Locate
        public override void Locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_pkg);
        }
        #endregion
        public override string GetSearchType() => "Package";
        public override string GetSubType() => "Package";
    }
}
