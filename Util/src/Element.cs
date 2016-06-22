using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils
{
    public static class Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pkg"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        public static EA.Element createElement(EA.Repository rep, EA.Package pkg, string name, string type, string stereotype)
        {
            EA.Element el = null;

            el = CallOperationAction.GetElementFromName(rep, name, type);

            if (el == null)
            {
                el = (EA.Element)pkg.Elements.AddNew(name, type);
                pkg.Elements.Refresh();
            }
            if (stereotype != "")
            {
                if (el.Stereotype != stereotype)
                {
                    el.Stereotype = stereotype;
                    el.Update();
                }
            }
            return el;
        }
        public static EA.Element createPortWithInterface(EA.Element elSource, EA.Element elInterface, string ifType ="RequiredInterface")
        {
            EA.Element interf = null;
            EA.Element port = null;

            foreach (EA.Element p in elSource.EmbeddedElements)
            {
                if (p.Name == elInterface.Name) return null; //interface exists
            }
            // create a port
            port = (EA.Element)elSource.EmbeddedElements.AddNew(elInterface.Name, "Port");
            // add interface
            interf = (EA.Element)port.EmbeddedElements.AddNew(elInterface.Name, ifType);
            // set classifier
            interf.ClassfierID = elInterface.ElementID;
            interf.Update();
            return interf;
        }
    }
}
