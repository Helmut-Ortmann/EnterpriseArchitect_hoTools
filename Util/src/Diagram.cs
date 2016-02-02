using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils
{
    public class EADiagram
    {
        private EA.Repository _rep = null;
        private EA.Diagram _dia = null;
        private List<EA.DiagramObject> _selectedObjects = new List<EA.DiagramObject>();

        private EA.Connector _selectedConnector = null;
        #region Constructor
        public EADiagram(EA.Repository rep)
        {
            _rep = rep;
            _dia = _rep.GetCurrentDiagram();
            if (_dia == null) return;
            foreach (EA.DiagramObject obj in  _dia.SelectedObjects)
            {
                _selectedObjects.Add(obj);
            }
            _selectedConnector = _dia.SelectedConnector;

        }
        #endregion
        #region Properties
        public EA.Diagram Dia => _dia;
        public int SelectedObjectsCount => _dia.SelectedObjects.Count;

        #endregion

        #region ReloadSelectedObjectsAndConnector
        public void ReloadSelectedObjectsAndConnector()
        {
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
        public void sortSelectedObjects()
        {
            // estiminate sort criteria (left/right, top/bottom)
            bool isVerticalSorted = true;
            EA.DiagramObject obj1 = _dia.SelectedObjects.GetAt(0);
            EA.DiagramObject obj2 = _dia.SelectedObjects.GetAt(1);
            if (Math.Abs(obj1.left - obj2.left) > Math.Abs(obj1.top - obj2.top)) isVerticalSorted = false;

            // fill the diagram objects to sort by name / by position
            List<DiagramObject> l_ids_by_name = new List<DiagramObject>();
            List<DiagramObjectSelected> l_obj_by_position = new List<DiagramObjectSelected>();
            foreach (EA.DiagramObject obj in _selectedObjects)
            {
                EA.Element el= _rep.GetElementByID(obj.ElementID);
                l_ids_by_name.Add(new DiagramObject(el.Name, el.ElementID) );
                int position = obj.left;
                if (isVerticalSorted) position = obj.top;
                l_obj_by_position.Add(new DiagramObjectSelected(obj, position, obj.left));
            }
            
            // sort name list and position list
            l_ids_by_name.Sort(new DiagramObjectComparer());
            // sort diagram objects according to position and vertical/horizental sorting
            if (isVerticalSorted) l_obj_by_position.Sort(new DiagramObjectSelectedVerticalComparer());
            else l_obj_by_position.Sort(new DiagramObjectSelectedHorizontalComparer());


            foreach (EA.DiagramObject obj in _dia.SelectedObjects)
            {
                // find position of element in sorted selected objects
                int pos = l_ids_by_name.FindIndex(x => x.Id == obj.ElementID);

                int length = obj.right - obj.left;
                int high = obj.top - obj.bottom;
                obj.left = l_obj_by_position[pos].Left;
                obj.bottom = l_obj_by_position[pos].Obj.bottom;
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
        string _name = "";
        int _id = 0;
        #region Constructor
        public DiagramObject(string Name, int Id)
        {
            _id = Id;
            _name = Name;
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
    /// <param name="x">first value of Name to compare</param>
    /// <param name="y">second value of Name to compare</param>
    public class DiagramObjectComparer : IComparer<DiagramObject>
    {
        public int Compare(DiagramObject x, DiagramObject y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return x.Name.CompareTo(y.Name);
        }
    }
    public class DiagramObjectSelected
    {
        EA.DiagramObject _obj = null;
        int _position = 0;
        int _left;
        #region Constructor
        public DiagramObjectSelected(EA.DiagramObject Obj, int position, int Left)
        {
            _position = position;
            _obj = Obj;
            _left = Left;
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
    /// <param name="x">first value of Name to compare</param>
    /// <param name="y">second value of Name to compare</param>
    public class DiagramObjectSelectedHorizontalComparer : IComparer<DiagramObjectSelected>
    {
        public int Compare(DiagramObjectSelected x, DiagramObjectSelected y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return x.Position.CompareTo(y.Position);
        }
    }
    /// <summary>
    /// Sort diagramObjectSelectedLeftComparer
    /// </summary>
    /// <param name="x">first value of Name to compare</param>
    /// <param name="y">second value of Name to compare</param>
    public class DiagramObjectSelectedVerticalComparer : IComparer<DiagramObjectSelected>
    {
        public int Compare(DiagramObjectSelected x, DiagramObjectSelected y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            if (x.Position < y.Position) return 1;
            return -1;
         }
    }
}
