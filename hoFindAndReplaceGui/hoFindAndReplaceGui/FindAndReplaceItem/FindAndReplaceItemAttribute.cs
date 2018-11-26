namespace hoTools.Find
{
    class FindAndReplaceItemAttribute : FindAndReplaceItem
    {
        EA.Attribute _attr ;
        #region Constructor
        public  FindAndReplaceItemAttribute(EA.Repository rep, string guid)  :base(rep, guid)
        {
            _attr = rep.GetAttributeByGuid(guid);
            Load(rep);
        }
        #endregion
        #region Property
        public EA.Attribute Attr => _attr;
        #endregion
        #region load
        public override void Load(EA.Repository rep)
        {
            _attr = rep.GetAttributeByGuid(Guid);
            Name = _attr.Name;
            Description = _attr.Notes;
            Stereotype = _attr.StereotypeEx;
         
        }
        #endregion
        #region Save
        public override void Save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {

            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { _attr.Notes = Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { _attr.StereotypeEx = Stereotype; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   _attr.Name = Name; }
            IsUpdated = true;
            _attr.Update();
        }
        #endregion
        #region Locate
        public override void Locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_attr);
        }
        #endregion
        public override string GetSearchType() => "Attribute";
        public override string GetSubType() => "Attribute";
    }
}
