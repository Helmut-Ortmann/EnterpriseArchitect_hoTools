namespace hoTools.Find
{
    class FindAndReplaceItemMethod : FindAndReplaceItem
    {
        EA.Method _meth;
        #region Constructor
        public  FindAndReplaceItemMethod(EA.Repository rep, string guid)  :base(rep, guid)
        {
            _meth = rep.GetMethodByGuid(guid);
            Load(rep);
        }
        #endregion
        #region Property
        public EA.Method Meth => _meth;
        //override public List<FindAndReplaceItemTag> l_itemTag
        //{
        //    get { return _l_itemTag; }
        //}
        #endregion
        #region Load
        public override void Load(EA.Repository rep)
        {

            Name = _meth.Name;
            Description = _meth.Notes;
            Stereotype = _meth.StereotypeEx;
         
        }
        #endregion
        #region save
        public override void Save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {
            EA.Method meth = rep.GetMethodByGuid(Guid);
            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { meth.Notes = Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   meth.Name = Name; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { meth.StereotypeEx = Stereotype; }
            IsUpdated = true;
            meth.Update();
        }
        #endregion
        #region Locate
        public override void Locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_meth);
        }
        #endregion
        public override string GetSearchType() => "Method";
        public override string GetSubType() => "Method";
    }
}
