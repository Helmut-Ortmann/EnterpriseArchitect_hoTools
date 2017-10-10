using System;
using System.Linq;
using hoTools.Utils.Diagram;
using hoTools.Utils.Extension;

namespace hoTools.Utils.EaCollections
{
    /// <summary>
    /// Handle EA collections for:
    /// - ordering
    /// </summary>
    public class EaCollection
    {
        protected readonly EA.Repository Rep;

        protected EaCollection(EA.Repository rep)
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
        /// If it sorts for Ports, it ignores other element types.
        /// 
        /// Note: It doesn't sort for Required/Provided Interface
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
            var llist = list.ToList();  // run query
            // sort only Port, Pin, Parameter
            if (llist.Count(t => t.Type == "Port" || t.Type == "Pin" || t.Type == "Parameter") > 0)
            {
                llist = llist.Where(t => t.Type == "Port" || t.Type == "Pin" || t.Type == "Parameter").ToList();
            }
            // nothing to sort
            if (llist.Count < 2) return false;
            

            // estimate the direction 
            bool isVertical =
                (Math.Abs(llist[0].top - llist[1].top)) >
                (Math.Abs(llist[0].left - llist[1].left));
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
        /// Move left around the corner
        /// </summary>
        /// <returns></returns>
        public bool MoveLeft()
        {
            EA.Repository rep = _eaDia.Rep;
            EA.Diagram dia = _eaDia.Dia;
            // order all embedded elements, only ports, pins,..
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.el.IsEmbeddedElement(rep)
                orderby temp.left
                select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaParentObj = llist[0].item.GetParentOfEmbedded(rep, dia);
            int leftLimit = diaParentObj.left;

            // top alignment /  top bound
            int topLimit = diaParentObj.top - (llist.Count - 1) * (Distant2Element + 15);
            int direction = -1;
            // start bottom bound or bottom bound
            if (llist[0].top + 30 < diaParentObj.top)
            {
                // right alignment / right bound
                topLimit = diaParentObj.bottom + (llist.Count - 1) * (Distant2Element + 15);
                direction = 1;
            }


            // estimate the mode to shift 
            // isAccross: jump from right edge to left edge
           bool isAccross =
                (Math.Abs(llist[0].top - llist[1].top)) >
                (Math.Abs(llist[0].left - llist[1].left));
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
                    int top = topLimit;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.left = leftLimit - 7;
                        el.item.right = leftLimit + 8;
                        el.item.top = top - (pos * Distant2Element) * direction;
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
            EA.Repository rep = _eaDia.Rep;
            EA.Diagram dia = _eaDia.Dia;
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.el.IsEmbeddedElement(rep)
                orderby temp.right descending
                select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaParentObj = llist[0].item.GetParentOfEmbedded(rep, dia);
            int rightLimit = diaParentObj.right;

            // top alignment /  top bound
            int topLimit = diaParentObj.top - (llist.Count - 1) * (Distant2Element + 15);
            int direction = -1;
            // start bottom bound or bottom bound
            if (llist[0].top + 30 < diaParentObj.top)
            {
                // right alignment / right bound
                topLimit = diaParentObj.bottom + (llist.Count - 1) * (Distant2Element + 15);
                direction = 1;
            }


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(llist[0].top - llist[1].top)) >
                (Math.Abs(llist[0].left - llist[1].left));
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
                    int top = topLimit ;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.left = rightLimit - 7;
                        el.item.right = rightLimit + 8;
                        el.item.top = top - (pos * Distant2Element) * direction;
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
            EA.Repository rep = _eaDia.Rep;
            EA.Diagram dia = _eaDia.Dia;
            var list = from item in _eaDia.SelObjects
                let el = Rep.GetElementByID(item.ElementID)
                select new {item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item}
                into temp
                where temp.el.IsEmbeddedElement(rep)
                orderby temp.top descending
                select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaParentObj = llist[0].item.GetParentOfEmbedded(rep, dia);
            int topLimit = diaParentObj.top;

            // left alignment /  left bound
            int leftLimit = diaParentObj.left + (llist.Count - 1) * (Distant2Element + 15);
            int direction = -1;
            // start left bound or right bound
            if (llist[0].left - 30 > diaParentObj.left)
            {
                // right alignment / right bound
                leftLimit = diaParentObj.right - (llist.Count - 1) * (Distant2Element + 15);
                direction = 1;
            }


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(llist[0].top - llist[1].top)) <
                (Math.Abs(llist[0].left - llist[1].left));
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
                    int left = leftLimit;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.top = topLimit + 7;
                        el.item.bottom = topLimit - 8;
                        el.item.left = left + (pos * Distant2Element) * direction;
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
            EA.Repository rep = _eaDia.Rep;
            EA.Diagram dia = _eaDia.Dia;
            var list = from item in _eaDia.SelObjects
                       let el = Rep.GetElementByID(item.ElementID)
                       select new { item.ElementID, el.Name, el, el.Type, item.left, item.right, item.top, item.bottom, item }
                          into temp
                       where temp.el.IsEmbeddedElement(rep)
                       orderby temp.bottom  
                       select temp;
            var llist = list.ToList();

            if (llist.Count < 2) return false;
            // get parent element
            EA.DiagramObject diaParentObj = llist[0].item.GetParentOfEmbedded(rep, dia);
            int bottomLimit = diaParentObj.bottom;

            // left alignment /  left bound
            int leftLimit = diaParentObj.left + (llist.Count - 1) * (Distant2Element + 15);
            int direction = -1;
            // start left bound or right bound
            if (llist[0].left - 30 > diaParentObj.left)
            {
                // right alignment / right bound
                leftLimit = diaParentObj.right - (llist.Count - 1) * (Distant2Element + 15) ;
                direction = 1;
            }
           


            // estimate the mode to shift 
            // 
            bool isAccross =
                (Math.Abs(llist[0].bottom - llist[1].bottom)) <
                (Math.Abs(llist[0].left - llist[1].left));
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
                // shift up, bottom element found, line them on left/right edge 
                if (llist[0].bottom < bottomLimit + OffsetFirstElement)
                {   
                    // position top left, sequence down
                    int left = leftLimit ;
                    int pos = 0;
                    foreach (var el in llist)
                    {
                        el.item.bottom = bottomLimit - 7;
                        el.item.top = bottomLimit + 8;
                        el.item.left = left + (pos * Distant2Element)  * direction;
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
        }
    }



}
