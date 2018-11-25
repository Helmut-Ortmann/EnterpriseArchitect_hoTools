namespace hoTools.Find
{
    class FindAndReplaceItemElement : FindAndReplaceItem
    {
        readonly EA.Element _el;
        #region Constructor
        public  FindAndReplaceItemElement(EA.Repository rep, string guid)  :base(rep, guid)
        {
            this._el = rep.GetElementByGuid(guid);
            this.Load(rep);
        }
        #endregion
        #region Property
        public EA.Element Element => this._el;

        #endregion
        #region Load
        public override void Load(EA.Repository rep)
        {
            _Name = _el.Name;
            _Description = _el.Notes;
            _Stereotype = _el.StereotypeEx;
         
        }
        #endregion
        #region ReplaceItem
        #endregion
        #region Save
        public override void Save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
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
        public override void Locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_el);
        }
        #endregion
        public override string GetSearchType() => "Element";
        public override string GetSubType() => _el.Type;
    }
}
