﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EA;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.Diagram
{
    /// <summary>
    /// The current diagram with the selected objects (first element=context=last selected element):
    /// <para/>Selected DiagramObjects if selected, all DiagramObjects if nothing selected. Check IsElementSelectedObjects. The first element is the last selected!
    /// <para/>Selected Connector
    /// </summary>
    public class EaDiagram
    {
        readonly Repository _rep;
        readonly EA.Diagram _dia;
        readonly List<EA.DiagramObject> _selectedObjects = new List<EA.DiagramObject>();
        readonly EA.DiagramObject _conTextDiagramObject;


        readonly Connector _selectedConnector;
        #region Constructor

        /// <summary>
        /// Get the current Diagram with it's selected objects and connectors.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="getAllDiagramObject">True if you want all diagram objects if nothing no diagram object is selected</param>
        public EaDiagram(Repository rep, bool  getAllDiagramObject= false)
        {
            _rep = rep;
            _dia = _rep.GetCurrentDiagram();
            IsSelectedObjects = false;
            if (_dia == null) return;
            if (_dia.SelectedObjects.Count == 0 && getAllDiagramObject)
            {
                // overall diagram objects
                foreach (EA.DiagramObject obj in _dia.DiagramObjects)
                {
                    _selectedObjects.Add(obj);
                    SelElements.Add(rep.GetElementByID(obj.ElementID));
                }

            }
            // If an context element exists than this is the last selected element
            if (_dia.SelectedObjects.Count > 0)
            {
                EA.ObjectType type = _rep.GetContextItemType();
                // only package and object makes sense, or no context element (than go for selected elements)
                if (type == EA.ObjectType.otElement ||
                    type == EA.ObjectType.otPackage ||
                    type == EA.ObjectType.otNone)
                {
                    // 1. store context element/ last selected element
                    EA.Element elContext = (EA.Element) rep.GetContextObject();
                    // no context element available, take first element
                    if (elContext == null)
                    {
                        EA.DiagramObject obj = (EA.DiagramObject)_dia.SelectedObjects.GetAt(0);
                        elContext = rep.GetElementByID(obj.ElementID);
                    }
                    _conTextDiagramObject = _dia.GetDiagramObjectByID(elContext.ElementID, "");

                    SelElements.Add(elContext);
                    _selectedObjects.Add(_conTextDiagramObject);
                    IsSelectedObjects = true;

                    // over all selected diagram objects
                    foreach (EA.DiagramObject obj in _dia.SelectedObjects)
                    {
                        // skip last selected element / context element
                        if (obj.ElementID == _conTextDiagramObject.ElementID) continue;

                        _selectedObjects.Add(obj);
                        SelElements.Add(rep.GetElementByID(obj.ElementID));
                    }
                }
            }
            _selectedConnector = _dia.SelectedConnector;


        }
        #endregion
        #region Properties
        public List<EA.DiagramObject> SelObjects =>_selectedObjects;
        // ReSharper disable once CollectionNeverQueried.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public List<EA.Element> SelElements { get; } = new List<EA.Element>();

        public bool IsSelectedObjects { get; }

        public EA.Diagram Dia => _dia;
        public int SelectedObjectsCount => _dia.SelectedObjects.Count;
        public EA.Connector SelectedConnector => _selectedConnector;

        #endregion

        #region ReloadSelectedObjectsAndConnector
        /// <summary>
        /// Reload previously stored selected diagram objects and diagram links
        /// </summary>
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
        /// <summary>
        /// Select diagram object. Save diagram and reload it refresh it.
        /// </summary>
        public void SelectDiagramObject()
        {
            Unselect();
            Save();
            _rep.ReloadDiagram(_dia.DiagramID);

            _dia.SelectedObjects.AddNew(_conTextDiagramObject.ElementID.ToString(), _conTextDiagramObject.ObjectType.ToString());
        }
        /// <summary>
        /// Select diagram object for the passed connector. Save diagram and reload / refresh it.
        /// </summary>
        public void SelectDiagramObject(EA.Connector con)
        {
            Unselect();
            Save();
            _rep.ReloadDiagram(_dia.DiagramID);


            EA.DiagramObject diaObj = _dia.GetDiagramObjectByID(con.SupplierID,"");
            _dia.SelectedObjects.AddNew(diaObj.ElementID.ToString(), diaObj.ObjectType.ToString());
        }
        /// <summary>
        /// Unselect all elements, connector.
        /// </summary>
        private void Unselect()
        {
            Dia.SelectedConnector = null;
            for (int i = _dia.SelectedObjects.Count - 1; i >= 0; i=i-1)
            {
                _dia.SelectedObjects.DeleteAt((short)i, true);
            }
            Dia.SelectedObjects.Refresh();

        }

        #region sortSelectedObjects
        // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once MemberCanBePrivate.Global
        public void Save()
        {
            _rep.SaveDiagram(_dia.DiagramID);
        }
        #endregion


        /// <summary>
        /// Set Diagram styles:
        /// HideQuals=1 HideQualifiers: 
        /// OpParams=2  Show full Operation Parameter
        /// ScalePI=1   Scale to fit page
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="par">par[0] contains the values as a semicolon separated list</param>
        // ReSharper disable once UnusedMember.Global
        public static void SetDiagramStyle(EA.Repository rep, EA.Diagram dia, string[] par)
        {
            string[] styleEx = par[0].Split(';');
            string diaStyle = dia.StyleEx;
            string diaExtendedStyle = dia.ExtendedStyle.Trim();
            // no distinguishing between StyleEx and ExtendedStayle, may cause of trouble
            if (dia.StyleEx == "") diaStyle = par[0]+";";
            if (dia.ExtendedStyle == "") diaExtendedStyle = par[0] + ";"; 

            Regex rx = new Regex(@"([^=]*)=.*");
            rep.SaveDiagram(dia.DiagramID);
            foreach (string style in styleEx)
            {
                Match match = rx.Match(style);
                string patternFind = $@"{match.Groups[1].Value}=[^;]*;";
                diaStyle = Regex.Replace(diaStyle, patternFind, $@"{style};");
                diaExtendedStyle = Regex.Replace(diaExtendedStyle, patternFind, $@"{style};");
            }
            dia.ExtendedStyle = diaExtendedStyle;
            dia.StyleEx = diaStyle;
            dia.Update();
            rep.ReloadDiagram(dia.DiagramID);

        }
       



    }
    public class DiagramObject
    {
        #region Constructor
        public DiagramObject(string name, int id)
        {
            Id = id;
            Name = name;
        }
        #endregion

        #region Properties
        public string Name { get; }

        public int Id { get; }

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
        #region Constructor
        public DiagramObjectSelected(EA.DiagramObject obj, int position, int left)
        {
            Position = position;
            Obj = obj;
            Left = left;
        }
        #endregion

        #region Properties
        public EA.DiagramObject Obj { get; }

        public int Position { get; }

        public int Left { get; }

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
