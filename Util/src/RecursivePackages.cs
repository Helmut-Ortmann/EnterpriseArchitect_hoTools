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
    public static class RecursivePackages
    {
        public delegate void SetPackage(Repository rep, EA.Package pkg, string[] s);
        public delegate void SetElement(Repository rep, EA.Element el, string[] s);
        public delegate void SetDiagram(Repository rep, Diagram dia, string[] s);

        public static void DoRecursivePkg(Repository rep, EA.Package pkg, SetPackage setPkg, SetElement setEl, SetDiagram setDia, string[] s)
        {
            // perform package
            setPkg?.Invoke(rep, pkg, s);

            // perform diagrams of package
            foreach (Diagram dia in pkg.Diagrams)
            {
                if (dia != null) setDia(rep, dia, s);
            }
            // run elements of package
            foreach (EA.Element el in pkg.Elements)
            {
                if (setEl != null) DoRecursiveEl(rep, el, setEl, setDia, s);
            }
            
            // run packages of package
            foreach (EA.Package pkgTrgt in pkg.Packages)
            {
                DoRecursivePkg(rep, pkgTrgt, setPkg, setEl, setDia, s);
            }
        }
        public static void DoRecursiveEl(Repository rep, EA.Element el, SetElement setEl, SetDiagram setDia, string[] s)
        {
            // perform
            setEl(rep, el, s);
            //run all elements
            foreach (EA.Element elTrgt in el.Elements)
            {
                DoRecursiveEl(rep, elTrgt, setEl, setDia, s);
            }
        }
    }
}
