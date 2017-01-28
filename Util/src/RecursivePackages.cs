using System;
using System.Collections.Generic;
using EA;

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

        public List<int> GetItemsRecursive(EA.Package pkg) {
            
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
    /// The scope of the chage
    /// </summary>
    public enum ChangeScope
    {
        Item,
        FullPackageElement,
        PackageRecursive

    }
    /// <summary>
    /// Change Packages Author of the package according to changeScope.
    /// - parameterStrings[0] Author
    /// - parameterStrings[1] changeScope as string
    /// 
    /// </summary>
    public static class RecursivePackages
    {
        public delegate void SetPackage(Repository rep, EA.Package pkg, string[] parameterStrings);
        public delegate void SetElement(Repository rep, EA.Element el, string[] parameterStrings);
        public delegate void SetDiagram(Repository rep, Diagram dia, string[] parameterStrings);

        public static void DoRecursivePkg(Repository rep, EA.Package pkg, SetPackage setPkg, 
            SetElement setEl, SetDiagram setDia, string[] parameterStrings)
        {
            ChangeScope changeScope;
            if (Enum.TryParse(parameterStrings[1], out changeScope))
            {
                // Change package
                setPkg?.Invoke(rep, pkg, parameterStrings);
               
                // only the package itself
                if (changeScope == ChangeScope.Item) return;

                // perform diagrams of package
                foreach (Diagram dia in pkg.Diagrams)
                {
                    if (dia != null) setDia(rep, dia, parameterStrings);
                }
                // run elements of package
                foreach (EA.Element el in pkg.Elements)
                {
                    if (setEl != null) DoRecursiveEl(rep, el, setEl, setDia, parameterStrings);
                }

                // run packages of package
                if (changeScope == ChangeScope.PackageRecursive) { 
                    foreach (EA.Package pkgTrgt in pkg.Packages)
                    {

                        DoRecursivePkg(rep, pkgTrgt, setPkg, setEl, setDia, parameterStrings);
                    }
                }
            }
        }


        public static void DoRecursiveEl(Repository rep, EA.Element el, SetElement setEl, SetDiagram setDia,
            string[] parameterStrings)
        {
            ChangeScope changeScope;
            if (Enum.TryParse(parameterStrings[1], out changeScope)) { 
                // perform
                setEl(rep, el, parameterStrings);

                // only the item itself
                if (changeScope == ChangeScope.Item) return;

                //run all elements
                foreach (EA.Element elTrgt in el.Elements)
                {
                    DoRecursiveEl(rep, elTrgt, setEl, setDia, parameterStrings);
                }
        }
    }
    }
}
