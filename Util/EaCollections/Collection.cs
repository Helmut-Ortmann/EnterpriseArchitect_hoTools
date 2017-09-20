using System;
using System.Linq;
using hoTools.Utils.Diagram;

namespace hoTools.Utils.EaCollections
{
    /// <summary>
    /// Handle EA collections for:
    /// - ordering
    /// </summary>
    public class EaCollection
    {
        protected readonly EA.Repository Rep;
        public EaCollection(EA.Repository rep)
        {
            Rep = rep;
        }

        public virtual bool OrderAlphabetic()
        {
            return true;
        }
    }

    public class EaCollectionDiagramObjects : EaCollection
    {
        readonly EaDiagram _eaDia;

        public EaCollectionDiagramObjects(EaDiagram eaDia) :base(eaDia.Rep)
        {
            _eaDia = eaDia;
            
        }
        /// <summary>
        /// Order diagram objects alphabetic like:
        /// - Classifier
        /// - Port, Parameter, Pin
        /// - Packages
        /// </summary>
        /// <returns></returns>
        public override bool OrderAlphabetic()
        {
            //EA.Element el = _rep.GetElementByID(diaObj.ElementID);
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID,el.Name, item.left, item.right, item.top, item.bottom, item}
                into temp
                orderby temp.Name
                select temp;
            var llist = list.ToList();
            // estimate the direction 

            bool isVertical =
                (Math.Abs(_eaDia.SelObjects[0].top - _eaDia.SelObjects[1].top)) >
                (Math.Abs(_eaDia.SelObjects[0].left - _eaDia.SelObjects[1].bottom)) 
                    ? true
                    : false;
            if (isVertical)
            {
                // vertical
                int topItem = llist.Max(t => t.top);
                int bottomItem = llist.Min(t => t.bottom);
                int sum = llist.Sum(t => t.top - t.bottom);
                int diff = (topItem - bottomItem - sum) / llist.Count;
                int top = topItem;
                foreach (var item in llist)
                {
                    int itemDif = item.item.top - item.item.bottom;
                    item.item.top = top;
                    item.item.bottom = item.item.top - itemDif;
                    top = item.item.bottom - diff;
                    item.item.Update();
                }
                Rep.ReloadDiagram(_eaDia.Dia.DiagramID);
                return true;
            }
            else
            {
                // horizontal
                int leftItem = llist.Max(t => t.left);
                int rightItem = llist.Min(t => t.right);
                int sum = llist.Sum(t => t.top - t.bottom);
                int diff = (leftItem - rightItem - sum) / llist.Count;
                int left = leftItem;
                foreach (var item in llist)
                {
                    int itemDif = item.item.left - item.item.right;
                    item.item.left = left;
                    item.item.right = item.item.left - itemDif;
                    left = item.item.right - diff;
                    item.item.Update();
                }
                Rep.ReloadDiagram(_eaDia.Dia.DiagramID);
                return true;

            }
        }

    }


    
}
