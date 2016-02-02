using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils
{
    public class ElementRecursive
    {
        EA.Repository _rep = null;
        List<int> _l_el_id = null;

        #region Constructor
        public ElementRecursive(EA.Repository rep)
        {
            _rep = rep;
            _l_el_id = new List<int>();
        }
        #endregion

        public List<int> getItemsRecursive(EA.Package pkg) {
            
            foreach (EA.Package pkgSub in pkg.Packages)
            {
                getItemsRecursive(pkgSub);
            }
            foreach (EA.Element el in pkg.Elements)
            {
                getItemEl(el);
            }
            return _l_el_id;
        }
        private void getItemEl(EA.Element el) 
        {
            _l_el_id.Add(el.ElementID);
            foreach (EA.Element elSub in el.Elements)
            {
                getItemEl(elSub);
            }

        }
 
    }
    public class RecursivePackages
    {
        public delegate void setPackage(EA.Repository rep, EA.Package pkg, string[] s);
        public delegate void setElement(EA.Repository rep, EA.Element el, string[] s);
        public delegate void setDiagram(EA.Repository rep, EA.Diagram dia, string[] s);

        public static void doRecursivePkg(EA.Repository rep, EA.Package pkg, setPackage setPkg, setElement setEl, setDiagram setDia, string[] s)
        {
            // perform package
            if (setPkg != null) setPkg(rep, pkg, s);

            // perform diagrams of package
            foreach (EA.Diagram dia in pkg.Diagrams)
            {
                if (dia != null) setDia(rep, dia, s);
            }
            // run elements of package
            foreach (EA.Element el in pkg.Elements)
            {
                if (setEl != null) doRecursiveEl(rep, el, setEl, setDia, s);
            }
            
            // run packages of package
            foreach (EA.Package pkgTrgt in pkg.Packages)
            {
                doRecursivePkg(rep, pkgTrgt, setPkg, setEl, setDia, s);
            }
            return;
        }
        public static void doRecursiveEl(EA.Repository rep, EA.Element el, setElement setEl, setDiagram setDia, string[] s)
        {
            // performel
            setEl(rep, el, s);
            //run all elements
            foreach (EA.Element elTrgt in el.Elements)
            {
                doRecursiveEl(rep, elTrgt, setEl, setDia, s);
            }
            return;
        }
    }
}
