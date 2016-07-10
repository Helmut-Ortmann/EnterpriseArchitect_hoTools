using System.Collections.Generic;


namespace hoTools.Utils
{
    public class ElementRecursive
    {
        EA.Repository _rep;
        readonly List<int> _lElId;

        #region Constructor
        public ElementRecursive(EA.Repository rep)
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
        public delegate void SetPackage(EA.Repository rep, EA.Package pkg, string[] s);
        public delegate void SetElement(EA.Repository rep, EA.Element el, string[] s);
        public delegate void SetDiagram(EA.Repository rep, EA.Diagram dia, string[] s);

        public static void DoRecursivePkg(EA.Repository rep, EA.Package pkg, SetPackage setPkg, SetElement setEl, SetDiagram setDia, string[] s)
        {
            // perform package
            setPkg?.Invoke(rep, pkg, s);

            // perform diagrams of package
            foreach (EA.Diagram dia in pkg.Diagrams)
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
        public static void DoRecursiveEl(EA.Repository rep, EA.Element el, SetElement setEl, SetDiagram setDia, string[] s)
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
