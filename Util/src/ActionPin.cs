using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using hoTools.Utils;
using hoTools.Utils.Appls;
using hoTools.Utils.Parameter;

namespace hoTools.Utils.ActionPins
{
    //--------------------------------------------------------------------------------------------
    // Update Action Pins from Operation
    //--------------------------------------------------------------------------------------------

    public class ActionPin
    {

        //---------------------------------------------------------------------------------------------
        // updateActionParameter(EA.Repository rep, EA.Element actionPin)
        //---------------------------------------------------------------------------------------------
        public static bool updateActionPinParameter(EA.Repository rep, EA.Element action)
        {
            foreach (EA.Element actionPin in action.EmbeddedElements)
            {
                // pin target for the return type of the action
                if (actionPin.Name == "target")
                {
                    //// return type
                    //Int32 parTypeID = Util.getTypeID(rep, m.ReturnType);
                    //if (parTypeID != 0)
                    //{
                    //    //pin.Name = par.
                    //    pin.ClassfierID = parTypeID;
                    //    EA.Element el = rep.GetElementByID(parTypeID);
                    //    pin.Update(); // do it before update table
                    //    Util.setElementPDATA1(rep, pin, el.ElementGUID);// PDATA1 setzen

                    //}
                }
                else
                {
                    // get type of synchronized parameter
                    // if parameter isn't synchronized it will not work
                    string type = Util.getParameterType(rep, actionPin.ElementGUID);
                    if (type == "")
                    {
                        string txt = "No type is available for action:'" + action.Name + "'";
                        rep.WriteOutput("ifm_addin", txt, 0);
                    }
                    else
                    {
                        Int32 parTypeID = Util.getTypeID(rep, type);
                        if (parTypeID != 0)
                        {
                            //pin.Name = par.
                            EA.Element el = rep.GetElementByID(parTypeID);
                            Util.setElementPDATA1(rep, actionPin, el.ElementGUID);// PDATA1 setzen
                        }
                    }
                }
            }

            return true;
        }

        //public static bool updateActionPin(EA.Repository rep, EA.Element el) {
        //    // get classifier (operation)
        //    EA.Method m = Util.getOperationFromCallAction(rep, el);

        //    // update action pins
        //    if (m != null)
        //    {
        //        foreach (EA.Parameter par in m.Parameters)
        //        {
        //            updateActionPinParameter(rep, m, el, par);
        //        }
        //    }
        //    return true;
        //}

        public static bool updateActionPinForElement(EA.Repository rep, EA.Element el1)
        {
            if (el1.Type == "Action")
            {
                updateActionPinParameter(rep, el1);
                return true;
            }
            if (el1.Type == "Class" | el1.Type == "Interface")
            {
                return true;
            }
            foreach (EA.Element el in el1.Elements)
            {   // update parameter
                if (el.Type == "Action")
                {
                    updateActionPinParameter(rep, el);

                }
                if (el.Type == "Activity")
                {
                    updateActionPinForElement(rep, el);
                }
            }

            return true;
        }
        //----------------------------------------------------------------------------
        // updateActionPinForPackage(EA.Repository rep, EA.Package pkg)
        //----------------------------------------------------------------------------

        public static bool updateActionPinForPackage(EA.Repository rep, EA.Package pkg)
        {
            foreach (EA.Element el in pkg.Elements)
            {   // update parameter
                if (el.Type == "Action")
                {
                    updateActionPinParameter(rep, el);
                    rep.RefreshModelView(pkg.PackageID); // reload package
                }
                if (el.Type == "Activity")
                {
                    foreach (EA.Element elSub in el.Elements)
                    {
                        updateActionPinForElement(rep, elSub);
                    }
                }
            }
            foreach (EA.Package pkgSub in pkg.Packages)
            {
                // update all packages
                updateActionPinForPackage(rep, pkgSub);
            }
            return true;

        }
    }
}
