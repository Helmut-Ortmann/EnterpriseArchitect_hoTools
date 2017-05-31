using EA;
using System.Collections.Generic;

namespace hoTools.Utils
{
    public class ElementRecursive
    {
        Repository _rep;
        readonly List<int> _lElId;

        #region Constructor
        public ElementRecursive(Repository rep)
        {
            _rep = rep;
            _lElId = new List<int>();
        }
        #endregion

        public List<int> GetItemsRecursive(EA.Package pkg)
        {

            foreach (EA.Package pkgSub in pkg.Packages)
            {
                GetItemsRecursive(pkgSub);
            }
            foreach (EA.Element el in pkg.Elements)
            {
                GetItemEl(el);
            }
            return _lElId;
        }
        private void GetItemEl(EA.Element el)
        {
            _lElId.Add(el.ElementID);
            foreach (EA.Element elSub in el.Elements)
            {
                GetItemEl(elSub);
            }

        }

    }
    /// <summary>
    /// The scope of the change
    /// </summary>
    public enum ChangeScope
    {
        Item,    // only elements, diagrams of elements, no elements recursive
        Package, // Only content of package, elements recursive, diagrams, no packages recursive
        PackageRecursive

    }
    /// <summary>
    /// Recursive to execute functions for Package, Diagram, Element. You can pass an string[] to give parameters.
    /// ChangeScope:changeScope contains the Scope of processing:
    /// - Item
    /// - Package
    /// - PackageRecursive
    /// 
    /// <para>- setPackage(EA.Repository rep, EA.Package pkg, string[] s) </para>
    /// <para>- setElement(EA.Repository rep, EA.Element el, string[] s) </para>
    /// <para>- setDiagram(EA.Repository rep, EA.Diagram dia, string[] s)</para>
    /// </summary>
    public static class RecursivePackages
    {
        public delegate void SetPackage(Repository rep, EA.Package pkg, string[] parameterStrings);
        public delegate void SetElement(Repository rep, EA.Element el, string[] parameterStrings);
        public delegate void SetDiagram(Repository rep, EA.Diagram dia, string[] parameterStrings);

        public static void DoRecursivePkg(Repository rep, EA.Package pkg, SetPackage setPkg,
            SetElement setEl, SetDiagram setDia, string[] parameterStrings, ChangeScope changeScope)
        {
            
            // Change package
            setPkg?.Invoke(rep, pkg, parameterStrings);

            // only the package itself
            if (changeScope == ChangeScope.Item) return;

            // perform diagrams of package
            foreach (EA.Diagram dia in pkg.Diagrams)
            {
                if (dia != null) setDia?.Invoke(rep, dia, parameterStrings);
            }
            // run elements of package
            foreach (EA.Element el in pkg.Elements)
            {
                DoRecursiveEl(rep, el, setEl, setDia, parameterStrings, changeScope);
            }

            // run packages of package
            if (changeScope != ChangeScope.Item)
            {
                if (changeScope == ChangeScope.Package)
                {
                    // inside package only the items
                    changeScope = ChangeScope.Item;
                }
                foreach (EA.Package pkgTrgt in pkg.Packages)
                {
                    DoRecursivePkg(rep, pkgTrgt, setPkg, setEl, setDia, parameterStrings, changeScope);
                }
            }
            
        }

        /// <summary>
        /// Run the delegates for Element and Diagram for the current element.
        /// - changeScope.Item   Only Element itself and Diagrams of element, not beneath element
        /// - changeScope...     Element and all beneath Element
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="setEl"></param>
        /// <param name="setDia"></param>
        /// <param name="parameterStrings"></param>
        /// <param name="changeScope"></param>
        public static void DoRecursiveEl(Repository rep, EA.Element el, SetElement setEl, SetDiagram setDia,
            string[] parameterStrings, ChangeScope changeScope)
        {
              // perform change for element
                setEl?.Invoke(rep, el, parameterStrings);

                // perform changes for diagrams beneath element
                foreach (EA.Diagram dia in el.Diagrams)
                {
                    if (dia != null) setDia?.Invoke(rep, dia, parameterStrings);
                }
                // only the item itself, no recursion
                if (changeScope == ChangeScope.Item) return;

                //run all elements
                foreach (EA.Element elTrgt in el.Elements)
                {
                    DoRecursiveEl(rep, elTrgt, setEl, setDia, parameterStrings, changeScope);
                }
        }
    }
}
