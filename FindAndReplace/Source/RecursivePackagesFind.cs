using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Find
{
    /// <summary>
    /// Find package recursive.
    /// 
    /// Static functions to:
    /// - Recursive Package
    /// - Recursive Element
    /// </summary>
    public class RecursivePackageFind
    {
        #region doRecursivePkg
        public static void doRecursivePkg(EA.Repository rep, EA.Package pkg, FindAndReplace fr)
        {
            // perform package
            if (fr.isPackageSearch)
            {
                fr.FindStringInItem(EA.ObjectType.otPackage, pkg.PackageGUID);
                if (fr.isTagSearch)
                {
                    // tagged values are beneath element with the same GUID
                    EA.Element el = rep.GetElementByGuid(pkg.PackageGUID);
                    if (el != null)   FindMatchingPackageTaggedValue(rep, pkg, fr);
                }

            }

            // perform diagrams of package
            if (fr.isDiagramSearch)
            {
                foreach (EA.Diagram dia in pkg.Diagrams)
                {
                    if (dia != null) fr.FindStringInItem(EA.ObjectType.otDiagram, dia.DiagramGUID);
                }
            }
            // run elements of package
            foreach (EA.Element el in pkg.Elements)
            {
                doRecursiveEl(rep, el, fr);
            }
            
            // run packages of package
            foreach (EA.Package pkgTrgt in pkg.Packages)
            {
                doRecursivePkg(rep, pkgTrgt, fr);
            }
            return;
        }
        #endregion
        #region doRecursiveEl
        public static void doRecursiveEl(EA.Repository rep, EA.Element el, FindAndReplace fr)
        {
            // perform element
            if (fr.isElementSearch)
            {
                fr.FindStringInItem(EA.ObjectType.otElement, el.ElementGUID);
                if (fr.isTagSearch)   FindMatchingElementTaggedValue(rep, el, fr);
            }

            if (fr.isAttributeSearch)
            {
                foreach (EA.Attribute a in el.Attributes)
                {
                    fr.FindStringInItem(EA.ObjectType.otAttribute, a.AttributeGUID);
                    if (fr.isTagSearch) FindMatchingAttributeTaggedValue(rep, a, fr);
                }
            }
            if (fr.isOperationSearch)
            {
                foreach (EA.Method m in el.Methods)
                {
                    fr.FindStringInItem(EA.ObjectType.otMethod, m.MethodGUID);
                    if (fr.isTagSearch) FindMatchingMethodTaggedValue(rep, m, fr);
                }
            }
            
            // perform diagrams of package
            if (fr.isDiagramSearch)
            {
                foreach (EA.Diagram dia in el.Diagrams)
                {
                    if (dia != null) fr.FindStringInItem(EA.ObjectType.otDiagram, dia.DiagramGUID);
                }
            }
            //run all elements
            foreach (EA.Element elTrgt in el.Elements)
            {
                doRecursiveEl(rep, elTrgt, fr);
            }
            
            return;
        }
        #endregion
        #region FindMatchingElementTaggedValue
        /// <summary>
        /// Find matching tagged values for element 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="fr"></param>
        private static void FindMatchingElementTaggedValue(EA.Repository rep, EA.Element el, FindAndReplace fr)
        {
            foreach (EA.TaggedValue tag in el.TaggedValues)
            {
                if ((fr.tagValueNames.Length == 0) || (fr.tagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.findCountForType(fr.regexPattern, tag.Value);
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.lastItem();
                        if ((frItem == null) || (frItem.GUID != el.ElementGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otElement, el.ElementGUID);
                            fr.l_items.Add(frItem);


                        }
                        var frItemEl = (FindAndReplaceItemElement)frItem;
                        frItemEl.l_itemTag.Add(new FindAndReplaceItemTagElement(tag));
                        frItemEl.CountChanges = frItemEl.CountChanges + count;
                        

                    }
                }

            }
        }
        #endregion
        #region FindMatchingPackageTaggedValue
        /// <summary>
        /// Find matching tagged values for element 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="fr"></param>
        private static void FindMatchingPackageTaggedValue(EA.Repository rep, EA.Package pkg, FindAndReplace fr)
        {
            EA.Element el = rep.GetElementByGuid(pkg.PackageGUID);
            foreach (EA.TaggedValue tag in el.TaggedValues)
            {
                if ((fr.tagValueNames.Length == 0) || (fr.tagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.findCountForType(fr.regexPattern, tag.Value);
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.lastItem();
                        if ((frItem == null) || (frItem.GUID != el.ElementGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otPackage, el.ElementGUID);
                            fr.l_items.Add(frItem);


                        }
                        var frItemPkg = (FindAndReplaceItemPackage)frItem;
                        frItemPkg.l_itemTag.Add(new FindAndReplaceItemTagPackage(tag));
                        frItemPkg.CountChanges = frItemPkg.CountChanges + count;


                    }
                }

            }
        }
        #endregion
        #region FindMatchingAttributeTaggedValue
        /// <summary>
        /// Find matching tagged values for element 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="fr"></param>
        private static void FindMatchingAttributeTaggedValue(EA.Repository rep, EA.Attribute a, FindAndReplace fr)
        {
            foreach (EA.AttributeTag tag in a.TaggedValues)
            {
                if ((fr.tagValueNames.Length == 0) || (fr.tagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.findCountForType(fr.regexPattern, tag.Value);
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.lastItem();
                        if ((frItem == null) || (frItem.GUID != a.AttributeGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otAttribute, a.AttributeGUID);
                            fr.l_items.Add(frItem);


                        }

                        var frItemAttr = (FindAndReplaceItemAttribute)frItem;
                        frItemAttr.l_itemTag.Add(new FindAndReplaceItemTagAttribute(tag));
                        frItemAttr.CountChanges = frItemAttr.CountChanges + count;

                    }
                }

            }
        }
        #endregion
        #region FindMatchingMethodTaggedValue
        /// <summary>
        /// Find matching tagged values for element 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="el"></param>
        /// <param name="fr"></param>
        private static void FindMatchingMethodTaggedValue(EA.Repository rep, EA.Method m, FindAndReplace fr)
        {
            foreach (EA.MethodTag tag in m.TaggedValues)
            {
                if ((fr.tagValueNames.Length == 0) || (fr.tagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.findCountForType(fr.regexPattern, tag.Value);
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.lastItem();
                        if ((frItem == null) || (frItem.GUID != m.MethodGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otMethod, m.MethodGUID);
                            fr.l_items.Add(frItem);


                        }

                        var frItemMeth = (FindAndReplaceItemMethod)frItem;
                        frItemMeth.l_itemTag.Add(new FindAndReplaceItemTagMethod(tag));
                        frItemMeth.CountChanges = frItemMeth.CountChanges + count;

                    }
                }

            }
        }
        #endregion
    }
}
