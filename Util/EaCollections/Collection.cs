using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using hoTools.Utils.Diagram;

namespace hoTools.Utils.EaCollections
{
    public class EaCollection
    {
        protected EA.Repository _rep;
        public EaCollection(EA.Repository rep)
        {
            _rep = rep;
        }

        public virtual bool OrderAlphabetic()
        {
            return true;
        }
    }

    public class EaCollectionDiagramObjects : EaCollection
    {
        EaDiagram _eaDia;
        List<EaCollectionDiagramObjectItem> _item = new List<EaCollectionDiagramObjectItem>();

        public EaCollectionDiagramObjects(EaDiagram eaDia) :base(eaDia.Rep)
        {
            _eaDia = eaDia;
            
        }
        public override bool OrderAlphabetic()
        {
            //EA.Element el = _rep.GetElementByID(diaObj.ElementID);
            var list = from item in _item
                let el = _rep.GetElementByID(item.Id)
                select new {item.Id,el.Name, item.Left, item.Right, item.Top, item.Bottom, item}
                into temp
                orderby temp.Name
                select temp;
            var llist = list.ToList();
            int topItem = llist.Max(t => t.Top);
            int bottomItem = llist.Min(t => t.Bottom);
            int sum = llist.Sum(t => t.Top - t.Bottom);
            int diff = (topItem- bottomItem - sum) / llist.Count;
            int top = topItem;
            foreach (var item in llist)
            {
                int itemDif = item.item.Top - item.item.Bottom;
                item.item.Top = top - diff;
                item.item.Bottom = item.item.Top - itemDif;
                top = item.item.Bottom - diff;
            }
            _rep.ReloadDiagram(_eaDia.Dia.DiagramID);
            return true;
        }

    }


    /// <summary>
    /// Collection item
    /// </summary>
    public class EaCollectionItem
    {
        int _left;
        int _right;
        int _top;
        int _bottom;
        int _id;
        string _name;

        public int Left { get => _left; set => _left = value; }
        public int Right { get => _right; set => _right = value; }
        public int Top { get => _top; set => _top = value; }
        public int Bottom { get => _bottom; set => _bottom = value; }
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public EaCollectionItem(int id, string name, int left, int right, int top, int bottom)
        {
            _id = id;
            _name = name;
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
        }
    }

    public class EaCollectionDiagramObjectItem : EaCollectionItem
    {
        public EaCollectionDiagramObjectItem(int id, string name, int left, int right, int top, int bottom) : base(id, name, left,
            right, top, bottom)
        {
            
        }

    }
}
