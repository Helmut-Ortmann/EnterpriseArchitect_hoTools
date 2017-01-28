using System;
using System.Collections.Generic;
using EA;

namespace hoTools.Utils
{
    /// <summary>
    /// The Current diagram with:
    /// <para/>Selected Objects
    /// <para/>Selected Connector
    /// </summary>
    public class EaDiagram
    {
        readonly Repository _rep;
        readonly EA.Diagram _dia;
        readonly List<EA.DiagramObject> _selectedObjects = new List<EA.DiagramObject>();
        readonly List<EA.Element> _selectedElements = new List<EA.Element>();
        readonly bool _isElementSelectedObjects = false;

        readonly Connector _selectedConnector;
        #region Constructor
        /// <summary>
        /// Get the current Diagram with it's selected objects and connectors.
        /// </summary>
        /// <param name="rep"></param>
        public EaDiagram(Repository rep)
        {
            _rep = rep;
            _dia = _rep.GetCurrentDiagram();
            if (_dia == null )   return;
            if (_dia.SelectedObjects.Count == 0)
            {
                foreach (EA.DiagramObject obj in _dia.DiagramObjects)
                {
                    _selectedObjects.Add(obj);
                    _selectedElements.Add(rep.GetElementByID(obj.ElementID));
                }
                

            }
            else
            {
                _isElementSelectedObjects = true;
                foreach (EA.DiagramObject obj in  _dia.SelectedObjects)
                {
                    _selectedObjects.Add(obj);
                    _selectedElements.Add(rep.GetElementByID(obj.ElementID));
                }
            }
            _selectedConnector = _dia.SelectedConnector;


        }
        #endregion
        #region Properties
        public List<EA.DiagramObject> SelObjects =>_selectedObjects;
        public List<EA.Element> SelElements => _selectedElements;
        public bool IsSelectedObjects => _isElementSelectedObjects;
        public Diagram Dia => _dia;
        public int SelectedObjectsCount => _dia.SelectedObjects.Count;
        public EA.Connector SelectedConnector => _selectedConnector;

        #endregion

        #region ReloadSelectedObjectsAndConnector
        /// <summary>
        /// Reload previously stored selected diagramobjects and diagramlinks
        /// </summary>
        public void ReloadSelectedObjectsAndConnector()
        {
            if (! IsSelectedObjects) return;
            Save();
            _rep.ReloadDiagram(_dia.DiagramID);
            if (_selectedConnector != null) _dia.SelectedConnector = _selectedConnector;
            foreach (EA.DiagramObject dObj in _selectedObjects)
            {
              _dia.SelectedObjects.AddNew(dObj.ElementID.ToString(), dObj.ObjectType.ToString());
            }
            _dia.SelectedObjects.Refresh();

        }
        #endregion
        #region sortSelectedObjects
        public void SortSelectedObjects()
        {
            // estimate sort criteria (left/right, top/bottom)
            bool isVerticalSorted = true;

            EA.DiagramObject obj1 = (EA.DiagramObject)_dia.SelectedObjects.GetAt(0);
            EA.DiagramObject obj2 = (EA.DiagramObject)_dia.SelectedObjects.GetAt(1);
            if (Math.Abs(obj1.left - obj2.left) > Math.Abs(obj1.top - obj2.top)) isVerticalSorted = false;

            // fill the diagram objects to sort by name / by position
            var lIdsByName = new List<DiagramObject>();
            var lObjByPosition = new List<DiagramObjectSelected>();
            foreach (EA.DiagramObject obj in _selectedObjects)
            {
                EA.Element el= _rep.GetElementByID(obj.ElementID);
                lIdsByName.Add(new DiagramObject(el.Name, el.ElementID) );
                int position = obj.left;
                if (isVerticalSorted) position = obj.top;
                lObjByPosition.Add(new DiagramObjectSelected(obj, position, obj.left));
            }
            
            // sort name list and position list
            lIdsByName.Sort(new DiagramObjectComparer());
            // sort diagram objects according to position and vertical / horizontal sorting
            if (isVerticalSorted) lObjByPosition.Sort(new DiagramObjectSelectedVerticalComparer());
            else lObjByPosition.Sort(new DiagramObjectSelectedHorizontalComparer());


            foreach (EA.DiagramObject obj in _dia.SelectedObjects)
            {
                // find position of element in sorted selected objects
                int pos = lIdsByName.FindIndex(x => x.Id == obj.ElementID);

                int length = obj.right - obj.left;
                int high = obj.top - obj.bottom;
                obj.left = lObjByPosition[pos].Left;
                obj.bottom = lObjByPosition[pos].Obj.bottom;
                obj.right = obj.left + length;
                obj.top = obj.bottom + high;
                obj.Update();
           }

        }
        #endregion

        #region Save
        public void Save()
        {
            _rep.SaveDiagram(_dia.DiagramID);
        }
        #endregion
    }
    public class DiagramObject
    {
        readonly string _name;
        readonly int _id;
        #region Constructor
        public DiagramObject(string name, int id)
        {
            _id = id;
            _name = name;
        }
        #endregion

        #region Properties
        public string Name => _name;
        public int Id => _id;
        #endregion
    }
    /// <summary>
    /// Sort diagramObject
    /// </summary>
    public class DiagramObjectComparer : IComparer<DiagramObject>
    {
        public int Compare(DiagramObject firstValue, DiagramObject secondValue)
        {
            if (firstValue == null && secondValue == null) return 0;
            if (firstValue == null) return 1;
            if (secondValue == null) return -1;
            return string.Compare(firstValue.Name, secondValue.Name, StringComparison.CurrentCulture);
        }
    }
    public class DiagramObjectSelected
    {
        readonly EA.DiagramObject _obj;
        readonly int _position;
        readonly int _left;
        #region Constructor
        public DiagramObjectSelected(EA.DiagramObject obj, int position, int left)
        {
            _position = position;
            _obj = obj;
            _left = left;
        }
        #endregion

        #region Properties
        public EA.DiagramObject Obj => _obj;
        public int Position => _position;
        public int Left => _left;
        #endregion
    }
    /// <summary>
    /// Sort diagramObjectSelectedLeftComparer
    /// </summary>
    public class DiagramObjectSelectedHorizontalComparer : IComparer<DiagramObjectSelected>
    {
        public int Compare(DiagramObjectSelected firstValue, DiagramObjectSelected secondValue)
        {
            if (firstValue == null && secondValue == null) return 0;
            if (firstValue == null) return 1;
            if (secondValue == null) return -1;
            return firstValue.Position.CompareTo(secondValue.Position);
        }
    }
    /// <summary>
    /// Sort diagramObjectSelectedLeftComparer
    /// </summary>
    public class DiagramObjectSelectedVerticalComparer : IComparer<DiagramObjectSelected>
    {
        public int Compare(DiagramObjectSelected firstValue, DiagramObjectSelected secondValue)
        {
            if (firstValue == null && secondValue == null) return 0;
            if (firstValue == null) return 1;
            if (firstValue.Position < secondValue?.Position) return 1;
            return -1;
         }
    }
}
