using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    class FindAndReplaceItemDiagram : FindAndReplaceItem
    {
        readonly EA.Diagram _dia;
        public  FindAndReplaceItemDiagram(EA.Repository rep, string GUID)  :base( rep, GUID)
        {
            this._dia = (EA.Diagram)rep.GetDiagramByGuid(GUID);
            this.load(rep);
        }
        #region load
        public override void load(EA.Repository rep)
        {
            _Name = _dia.Name;
            _Description = _dia.Notes;
            _Stereotype = _dia.StereotypeEx;
         
        }
        #endregion
        #region save
        public override void save(EA.Repository rep, FindAndReplaceItem.FieldType fieldType)
        {
            EA.Diagram dia = (EA.Diagram)rep.GetDiagramByGuid(GUID);
            if ((fieldType & FindAndReplaceItem.FieldType.Description) > 0)
            { dia.Notes = _Description; }
            if ((fieldType & FindAndReplaceItem.FieldType.Name) > 0)
            {   dia.Name = _Name; }
            if ((fieldType & FindAndReplaceItem.FieldType.Stereotype) > 0)
            { dia.StereotypeEx = _Stereotype; }
            _isUpdated = true;            
            dia.Update();
        }
        #endregion
        #region locate
        public override void locate(EA.Repository rep)
        {
            rep.ShowInProjectView(_dia);
        }
        #endregion
        public override string getType() => "Diagram";
        public override string getSubType() => _dia.Type;
    }
}
