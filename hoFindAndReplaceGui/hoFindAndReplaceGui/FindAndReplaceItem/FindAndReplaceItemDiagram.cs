namespace hoTools.Find
{
    class FindAndReplaceItemDiagram : FindAndReplaceItem
    {
        readonly EA.Diagram _dia;
        public  FindAndReplaceItemDiagram(EA.Repository rep, string guid)  :base( rep, guid)
        {
            _dia = (EA.Diagram)rep.GetDiagramByGuid(guid);
            Load(rep);
        }
        #region load
        public override void Load(EA.Repository rep)
        {
            Name = _dia.Name;
            Description = _dia.Notes;
            Stereotype = _dia.StereotypeEx;
         
        }
        #endregion
        #region save
        public override void Save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {
            EA.Diagram dia = (EA.Diagram)rep.GetDiagramByGuid(Guid);
            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { dia.Notes = Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   dia.Name = Name; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { dia.StereotypeEx = Stereotype; }
            IsUpdated = true;            
            dia.Update();
        }
        #endregion
        #region locate
        public override void Locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_dia);
        }
        #endregion
        public override string GetSearchType() => "Diagram";
        public override string GetSubType() => _dia.Type;
    }
}
