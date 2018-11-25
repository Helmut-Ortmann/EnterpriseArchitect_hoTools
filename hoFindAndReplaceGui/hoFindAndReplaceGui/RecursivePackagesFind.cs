using System.Linq;

namespace hoTools.Find
{
    /// <summary>
    /// Find package recursive.
    /// 
    /// Static functions to:
    /// - Recursive Package
    /// - Recursive Element
    /// </summary>
    public static class RecursivePackageFind
    {
        #region doRecursivePkg
        public static void DoRecursivePkg(EA.Repository rep, EA.Package pkg, FindAndReplace fr)
        {
            // perform package
            if (fr.IsPackageSearch)
            {
                fr.FindStringInItem(EA.ObjectType.otPackage, pkg.PackageGUID);
                if (fr.IsTagSearch)
                {
                    // tagged values are beneath element with the same Id
                    EA.Element el = rep.GetElementByGuid(pkg.PackageGUID);
                    if (el != null)   FindMatchingPackageTaggedValue(rep, pkg, fr);
                }

            }

            // perform diagrams of package
            if (fr.IsDiagramSearch)
            {
                foreach (EA.Diagram dia in pkg.Diagrams)
                {
                    if (dia != null) fr.FindStringInItem(EA.ObjectType.otDiagram, dia.DiagramGUID);
                }
            }
            // run elements of package
            foreach (EA.Element el in pkg.Elements)
            {
                DoRecursiveEl(rep, el, fr);
            }
            
            // run packages of package
            foreach (EA.Package pkgTrgt in pkg.Packages)
            {
                DoRecursivePkg(rep, pkgTrgt, fr);
            }
        }
        #endregion
        #region doRecursiveEl
        public static void DoRecursiveEl(EA.Repository rep, EA.Element el, FindAndReplace fr)
        {
            // perform element
            if (fr.IsElementSearch)
            {
                fr.FindStringInItem(EA.ObjectType.otElement, el.ElementGUID);
                if (fr.IsTagSearch)   FindMatchingElementTaggedValue(rep, el, fr);
            }

            if (fr.IsAttributeSearch)
            {
                foreach (EA.Attribute a in el.Attributes)
                {
                    fr.FindStringInItem(EA.ObjectType.otAttribute, a.AttributeGUID);
                    if (fr.IsTagSearch) FindMatchingAttributeTaggedValue(rep, a, fr);
                }
            }
            if (fr.IsOperationSearch)
            {
                foreach (EA.Method m in el.Methods)
                {
                    fr.FindStringInItem(EA.ObjectType.otMethod, m.MethodGUID);
                    if (fr.IsTagSearch) FindMatchingMethodTaggedValue(rep, m, fr);
                }
            }
            
            // perform diagrams of package
            if (fr.IsDiagramSearch)
            {
                foreach (EA.Diagram dia in el.Diagrams)
                {
                    if (dia != null) fr.FindStringInItem(EA.ObjectType.otDiagram, dia.DiagramGUID);
                }
            }
            //run all elements
            foreach (EA.Element elTrgt in el.Elements)
            {
                DoRecursiveEl(rep, elTrgt, fr);
            }
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
                if ((fr.TagValueNames.Length == 0) || (fr.TagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.FindCountForType(fr.RegexPattern, Utils.TaggedValue.GetTaggedValue(el, tag.Name));
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.LastItem();
                        if ((frItem == null) || (frItem.GUID != el.ElementGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otElement, el.ElementGUID);
                            fr.LItems.Add(frItem);


                        }
                        var frItemEl = (FindAndReplaceItemElement)frItem;
                        frItemEl.LItemTag.Add(new FindAndReplaceItemTagElement(tag));
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
        /// <param name="pkg"></param>
        /// <param name="fr"></param>
        private static void FindMatchingPackageTaggedValue(EA.Repository rep, EA.Package pkg, FindAndReplace fr)
        {
            EA.Element el = rep.GetElementByGuid(pkg.PackageGUID);
            foreach (EA.TaggedValue tag in el.TaggedValues)
            {
                if ((fr.TagValueNames.Length == 0) || (fr.TagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.FindCountForType(fr.RegexPattern, Utils.TaggedValue.GetTaggedValue(el, tag.Name));
                    if (count > 0)
                    {
                        // Create the searchers
                        FindAndReplaceItem frItem = fr.LastItem();
                        if ((frItem == null) || (frItem.GUID != el.ElementGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otPackage, el.ElementGUID);
                            fr.LItems.Add(frItem);


                        }
                        // Search
                        var frItemPkg = (FindAndReplaceItemPackage)frItem;
                        frItemPkg.LItemTag.Add(new FindAndReplaceItemTagPackage(tag));
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
        /// <param name="attr"></param>
        /// <param name="fr"></param>
        private static void FindMatchingAttributeTaggedValue(EA.Repository rep, EA.Attribute attr, FindAndReplace fr)
        {
            foreach (EA.AttributeTag tag in attr.TaggedValues)
            {
                if ((fr.TagValueNames.Length == 0) || (fr.TagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.FindCountForType(fr.RegexPattern, Utils.TaggedValue.GetTaggedValue(attr, tag.Name));
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.LastItem();
                        if ((frItem == null) || (frItem.GUID != attr.AttributeGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otAttribute, attr.AttributeGUID);
                            fr.LItems.Add(frItem);


                        }

                        var frItemAttr = (FindAndReplaceItemAttribute)frItem;
                        frItemAttr.LItemTag.Add(new FindAndReplaceItemTagAttribute(tag));
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
        /// <param name="method"></param>
        /// <param name="fr"></param>
        private static void FindMatchingMethodTaggedValue(EA.Repository rep, EA.Method method, FindAndReplace fr)
        {
            foreach (EA.MethodTag tag in method.TaggedValues)
            {
                if ((fr.TagValueNames.Length == 0) || (fr.TagValueNames.Contains(tag.Name)))
                {
                    int count = FindAndReplaceItem.FindCountForType(fr.RegexPattern, Utils.TaggedValue.GetTaggedValue(method, tag.Name));
                    if (count > 0)
                    {
                        FindAndReplaceItem frItem = fr.LastItem();
                        if ((frItem == null) || (frItem.GUID != method.MethodGUID))
                        {
                            frItem = FindAndReplaceItem.Factory(rep, EA.ObjectType.otMethod, method.MethodGUID);
                            fr.LItems.Add(frItem);


                        }

                        var frItemMeth = (FindAndReplaceItemMethod)frItem;
                        frItemMeth.LItemTag.Add(new FindAndReplaceItemTagMethod(tag));
                        frItemMeth.CountChanges = frItemMeth.CountChanges + count;

                    }
                }

            }
        }
        #endregion
    }
}
