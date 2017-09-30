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

        public virtual bool SortAlphabetic()
        {
            return true;
        }
    }

    /// <summary>
    /// Handle Collection of Diagram objects
    /// </summary>

    public class EaCollectionDiagramObjects : EaCollection
    {
        readonly EaDiagram _eaDia;

        const int OffsetFirstElement = 20;
        const int Distant2Element = 25;
        private const int DistantToMove = 25;

        public EaCollectionDiagramObjects(EaDiagram eaDia) : base(eaDia.Rep)
        {
            _eaDia = eaDia;

        }

        /// <summary>
        /// Sort diagram objects alphabetic like:
        /// - Classifier
        /// - Port, Parameter, Pin
        /// - Packages
        /// </summary>
        /// <returns></returns>
        public override bool SortAlphabetic()
        {

            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.Type != "RequiredInterface" && temp.Type != "ProvidedInterface"
                orderby temp.Name
                select temp;
            var llist = list.ToList();

            // estimate the direction 
            bool isVertical =
                (Math.Abs(_eaDia.SelObjects[0].top - _eaDia.SelObjects[1].top)) >
                (Math.Abs(_eaDia.SelObjects[0].left - _eaDia.SelObjects[1].left))
                    ? true
                    : false;
            if (isVertical)
            {
                // vertical
                int topItem = llist.Max(t => t.top);
                int bottomItem = llist.Min(t => t.bottom);
                int sum = llist.Sum(t => t.top - t.bottom);
                int diff = (topItem - bottomItem - sum) / (llist.Count - 1);
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
                int leftItem = llist.Min(t => t.left);
                int rightItem = llist.Max(t => t.right);
                int sum = llist.Sum(t => t.right - t.left);
                int diff = (rightItem - leftItem - sum) / (llist.Count - 1);
                int left = leftItem;
                foreach (var item in llist)
                {
                    int itemDif = item.item.right - item.item.left;
                    item.item.left = left;
                    item.item.right = item.item.left + itemDif;
                    left = item.item.right + diff;
                    item.item.Update();
                }
                Rep.ReloadDiagram(_eaDia.Dia.DiagramID);
                return true;

            }
        }

        /// <summary>
        /// Move left
        /// </summary>
        /// <returns></returns>
        public bool MoveLeft()
        {
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.el.IsEmbeddedElement(_eaDia.Rep)
                orderby temp.left
                select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaObj = _eaDia.Dia.GetDiagramObjectByID(llist[0].el.ParentID, "");
            int leftLimit = diaObj.left;
            int topLimit = diaObj.top;


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(_eaDia.SelObjects[0].top - _eaDia.SelObjects[1].top)) >
                (Math.Abs(_eaDia.SelObjects[0].left - _eaDia.SelObjects[1].left))
                    ? true
                    : false;
            if (isAccross)
            {
                foreach (var el in llist)
                {
                    el.item.left = leftLimit - 7;
                    el.item.right = leftLimit + 8;
                    el.item.Update();
                }
                return true;
            }
            else
            {
                if (llist[0].left < leftLimit + OffsetFirstElement)
                {
                    // position top left, sequence down
                    int top = topLimit - OffsetFirstElement;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.left = leftLimit - 7;
                        el.item.right = leftLimit + 8;
                        el.item.top = top - (pos * Distant2Element);
                        el.item.bottom = el.item.top - 15;
                        el.item.Update();
                        pos = pos + 1;

                    }

                }
                else
                {

                    // shift to left
                    foreach (var el in llist)
                    {
                        el.item.left = el.item.left - DistantToMove;
                        el.item.right = el.item.right - DistantToMove;
                        el.item.Update();
                    }
                }
                return true;
            }

        }

        /// <summary>
        /// Move left
        /// </summary>
        /// <returns></returns>
        public bool MoveRight()
        {
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.el.IsEmbeddedElement(_eaDia.Rep)
                orderby temp.right descending
                select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaObj = _eaDia.Dia.GetDiagramObjectByID(llist[0].el.ParentID, "");
            int rightLimit = diaObj.right;
            int topLimit = diaObj.top;


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(_eaDia.SelObjects[0].top - _eaDia.SelObjects[1].top)) >
                (Math.Abs(_eaDia.SelObjects[0].left - _eaDia.SelObjects[1].left))
                    ? true
                    : false;
            if (isAccross)
            {
                foreach (var el in llist)
                {
                    el.item.left = rightLimit - 7;
                    el.item.right = rightLimit + 8;
                    el.item.Update();
                }
                return true;
            }
            else
            {
                if (llist[0].right > rightLimit - OffsetFirstElement)
                {
                    // position top left, sequence down
                    int top = topLimit - OffsetFirstElement;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.left = rightLimit - 7;
                        el.item.right = rightLimit + 8;
                        el.item.top = top - (pos * Distant2Element);
                        el.item.bottom = el.item.top - 15;
                        el.item.Update();
                        pos = pos + 1;

                    }

                }
                else
                {

                    // shift to right
                    foreach (var el in llist)
                    {
                        el.item.left = el.item.left + DistantToMove;
                        el.item.right = el.item.right + DistantToMove;
                        el.item.Update();
                    }
                }
                return true;
            }

        }

        /// <summary>
        /// Move left
        /// </summary>
        /// <returns></returns>
        public bool MoveUp()
        {
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.el.IsEmbeddedElement(_eaDia.Rep)
                orderby temp.top descending
                select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaObj = _eaDia.Dia.GetDiagramObjectByID(llist[0].el.ParentID, "");
            int topLimit = diaObj.top;
            int leftLimit = diaObj.left;


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(_eaDia.SelObjects[0].top - _eaDia.SelObjects[1].top)) <
                (Math.Abs(_eaDia.SelObjects[0].left - _eaDia.SelObjects[1].left))
                    ? true
                    : false;
            if (isAccross)
            {
                // jump to top left -> to right lined
                foreach (var el in llist)
                {
                    el.item.top = topLimit + 8;
                    el.item.bottom = topLimit - 7;
                    el.item.Update();
                }
                return true;
            }
            else
            {
                // shift up, top element found
                if (llist[0].top > topLimit - OffsetFirstElement)
                {
                    // position top left, sequence down
                    int left = leftLimit + OffsetFirstElement;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.top = topLimit + 7;
                        el.item.bottom = topLimit - 8;
                        el.item.left = left + (pos * Distant2Element);
                        el.item.right = el.item.top + 15;
                        el.item.Update();
                        pos = pos + 1;

                    }

                }
                else
                {

                    // shift to top
                    foreach (var el in llist)
                    {
                        el.item.top = el.item.top + DistantToMove;
                        el.item.bottom = el.item.top - DistantToMove;
                        el.item.Update();
                    }
                }
                return true;
            }

        }

        /// <summary>
        /// Move down
        /// </summary>
        /// <returns></returns>
        public bool MoveDown()
        {
            var list = from item in _eaDia.SelObjects
                       let el = Rep.GetElementByID(item.ElementID)
                       select new { item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item }
                          into temp
                       where temp.el.IsEmbeddedElement(_eaDia.Rep)
                       orderby temp.bottom 
                       select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaObj = _eaDia.Dia.GetDiagramObjectByID(llist[0].el.ParentID, "");
            int bottomLimit = diaObj.bottom;
            int leftLimit = diaObj.left;


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(_eaDia.SelObjects[0].bottom - _eaDia.SelObjects[1].bottom)) <
                (Math.Abs(_eaDia.SelObjects[0].left - _eaDia.SelObjects[1].left))
                    ? true
                    : false;
            if (isAccross)
            {
                // jump to bottom left -> to right lined
                foreach (var el in llist)
                {
                    el.item.top = bottomLimit + 8;
                    el.item.bottom = bottomLimit - 7;
                    el.item.Update();
                }
                return true;
            }
            else
            {
                // shift up, bottom element found, line them on top edge from left to right
                if (llist[0].bottom < bottomLimit - OffsetFirstElement)
                {
                    // position top left, sequence down
                    int left = leftLimit + OffsetFirstElement;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.bottom = bottomLimit - 7;
                        el.item.top = bottomLimit + 8;
                        el.item.left = left + (pos * Distant2Element);
                        el.item.right = el.item.left + 15;
                        el.item.Update();
                        pos = pos + 1;

                    }

                }
                else
                {

                    // shift one step to top
                    foreach (var el in llist)
                    {
                        el.item.top = el.item.top - DistantToMove;
                        el.item.bottom = el.item.bottom - DistantToMove;
                        el.item.Update();
                    }
                }
                return true;
            }
            return false;
        }
    }



}
