using System.Windows.Forms;
using EA;
using hoTools.Utils;
using hoTools.Utils.Diagram;


// ReSharper disable once CheckNamespace
namespace hoTools.EaServices
{

    public static partial class EaService
    {
        /// <summary>
        /// Bulk change Diagram Styles 1 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{7D3B03FD-399B-4D39-9F54-5E7CB2CDBBBF}", "Bulk change Diagram to 'Style 1'",
            "Select Package, Element, Diagram (see Settings.Json, 1. entry)", isTextRequired: false)]
        public static void DiagramStyle1(EA.Repository rep)
        {
            DiagramStyleWrapper(rep, 0);
        }
        /// <summary>
        /// Bulk change Diagram Styles 2 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{FD79F3ED-1345-4CF6-AB43-9EF34571CA52}", "Bulk change Diagram to 'Style 2'",
            "Select Package, Element, Diagram (see Settings.Json, 2. entry)", isTextRequired: false)]
        public static void DiagramStyle2(EA.Repository rep)
        {
            DiagramStyleWrapper(rep, 1);
        }
        /// <summary>
        /// Wrapper to change Diagram style
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pos"></param>
        private static void DiagramStyleWrapper(Repository rep, int pos)
        {
            if (DiagramStyle.DiagramStyleItems == null && DiagramStyle.DiagramStyleItems.Count <= pos)
            {
                MessageBox.Show("", "No Diagram style in 'Settings.json' found");
                return;
            }
            // [0] StyleEx
            // [1] PDATA
            // [2] Properties
            // [3] Diagram types
            string[] styleEx = new string[4];
            styleEx[0] = DiagramStyle.DiagramStyleItems[pos].StyleEx;
            styleEx[1] = DiagramStyle.DiagramStyleItems[pos].Pdata;
            styleEx[2] = DiagramStyle.DiagramStyleItems[pos].Property;
            styleEx[3] = DiagramStyle.DiagramStyleItems[pos].Type;
            ChangeDiagramStyle(rep, styleEx, ChangeScope.PackageRecursive);
        }
        /// <summary>
        /// Bulk change DiagramLink Styles 0 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{B1227872-4968-439A-A926-7FE70F022F09}", "Change DiagramLinks to 'Style 1'",
            "Select Diagram, Diagram Objects or Diagram link (see Settings.Json, 1. entry)", isTextRequired: false)]
        public static void DiagramLinkStyle1(EA.Repository rep)
        {
            DiagramLinkStyleWrapper(rep, 0, ChangeScope.Package);
        }



        /// <summary>
        /// Bulk change DiagramObject Styles 1 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{FF7B370C-8D64-4919-9121-3571AA433B7C}", "Change DiagramLinks to 'Style 2'",
            "Select Diagram, Diagram Objects or Diagram link (see Settings.Json, 2. entry)", isTextRequired: false)]
        public static void DiagramLinkStyle2(EA.Repository rep)
        {
            DiagramLinkStyleWrapper(rep, 1, ChangeScope.Package);
        }
        /// <summary>
        /// Bulk change DiagramObject Styles 2 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{6AA996AD-640D-4615-886F-EC29062B2695}", "Change DiagramLinks to 'Style 3'",
            "Select Diagram, Diagram Objects or Diagram link (see Settings.Json, 3. entry)", isTextRequired: false)]
        public static void DiagramLinkStyle3(EA.Repository rep)
        {
            DiagramLinkStyleWrapper(rep, 2, ChangeScope.Package);
        }
        /// <summary>
        /// Bulk change DiagramObject Styles 3 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{1D838C4B-634C-490B-8CFE-2CC418133984}", "Change DiagramLinks to 'Style 4'",
            "Select Diagram, Diagram Objects or Diagram link (see Settings.Json, 4. entry)", isTextRequired: false)]
        public static void DiagramLinkStyle4(EA.Repository rep)
        {
            DiagramLinkStyleWrapper(rep, 3, ChangeScope.Package);
        }

        //-----------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Bulk change DiagramObject Styles 0 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{7FBCA44B-5DEA-4BFA-8948-EAFD17D60D83}", "Change DiagramObjects to 'Style 1'",
            "Select Diagram or Diagram Objects (see Settings.Json, 1. entry)", isTextRequired: false)]
        public static void DiagramObjectStyle1(EA.Repository rep)
        {
            DiagramObjectStyleWrapper(rep, 0, ChangeScope.Package);
        }

        /// <summary>
        /// Bulk change DiagramObject Styles 1 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{94F8855F-22BB-4BD7-BCEE-FC8D64E63B83}", "Change DiagramObjects to 'Style 2'",
            "Select Diagram or Diagram Objects (see Settings.Json, 2. entry)", isTextRequired: false)]
        public static void DiagramObjectStyle2(EA.Repository rep)
        {
            DiagramObjectStyleWrapper(rep, 1, ChangeScope.Package);
        }
        /// <summary>
        /// Bulk change DiagramObject Styles 3 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{98B0661C-DEB0-453D-BD30-3FB900228B38}", "Change DiagramObjects to 'Style 3'",
            "Select Diagram or Diagram Objects (see Settings.Json, 3. entry)", isTextRequired: false)]
        public static void DiagramObjectStyle3(EA.Repository rep)
        {
            DiagramObjectStyleWrapper(rep, 2, ChangeScope.Package);
        }
        /// <summary>
        /// Bulk change DiagramObject Styles 4 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{BFD60FFD-2244-4320-8AB2-57858DA6817F}", "Change DiagramObjects to 'Style 4'",
            "Select Diagram or Diagram Objects (see Settings.Json, 4. entry)", isTextRequired: false)]
        public static void DiagramObjectStyle4(EA.Repository rep)
        {
            DiagramObjectStyleWrapper(rep, 3, ChangeScope.Package);
        }
        /// <summary>
        /// Bulk change DiagramObject Styles 5 according to Settings.Json 
        /// </summary>
        [ServiceOperation("{DD52FE95-51DB-4619-AF5E-39EE7AA2CF4A}", "Change DiagramObjects to 'Style 5'",
            "Select Diagram or Diagram Objects (see Settings.Json, 5. entry)", isTextRequired: false)]
        public static void DiagramObjectStyle5(EA.Repository rep)
        {
            DiagramStyleWrapper(rep, 4);
        }


        /// <summary>
        /// Wrapper to change DiagramObject style
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pos"></param>
        /// <param name="changeScope"></param>
        private static void DiagramObjectStyleWrapper(Repository rep, int pos, ChangeScope changeScope)
        {
            if (DiagramStyle.DiagramStyleItems == null && DiagramStyle.DiagramObjectStyleItems.Count <= pos)
            {
                MessageBox.Show("", "No DiagramObject style in 'Settings.json' found");
                return;
            }
            string style = $@"{DiagramStyle.DiagramObjectStyleItems[pos].Style}".Trim();
            string property = $@"{DiagramStyle.DiagramObjectStyleItems[pos].Property}".Trim();
            string type = $@"{DiagramStyle.DiagramObjectStyleItems[pos].Type}".Trim();
            DiagramObjectStyleWrapper(rep, type, style, property, changeScope);
        }

        /// <summary>
        /// Wrapper to change DiagramObject style
        /// - Selected Diagramobjects
        /// - Package (Diagrams and their DiagramObjects in package and below Elements)
        /// - Element (Diagrams and their DiagramObjects below Elements)
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="type"></param>
        /// <param name="style"></param>
        /// <param name="property"></param>
        /// <param name="changeScope"></param>
        public static void DiagramObjectStyleWrapper(Repository rep, string type, string style, string property, ChangeScope changeScope)
        {
            EaDiagram eaDia = new EaDiagram(rep, getAllDiagramObject: true);
            if (eaDia.Dia != null)
            {
                rep.SaveDiagram(eaDia.Dia.DiagramID);
                foreach (var diaObj in eaDia.SelObjects)
                {
                    var objectStyle = new DiagramObjectStyle(rep, diaObj, type, style, property);
                    if (objectStyle.IsToProcess())
                    {
                        objectStyle.UpdateStyles();
                        objectStyle.SetProperties();
                        objectStyle.SetEaLayoutStyles();
                        objectStyle.SetCompleteNessMarker();
                    }
                }
                eaDia.ReloadSelectedObjectsAndConnector(saveDiagram: false);

            }
            else
            {
                var liParameter = new string[4];
                liParameter[0] = type;
                liParameter[1] = style;
                liParameter[2] = property;

                switch (rep.GetContextItemType())
                {
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        RecursivePackages.DoRecursivePkg(rep, pkg, null, null,
                            SetDiagramObjectStyle,
                            liParameter,
                            changeScope);
                        break;
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        RecursivePackages.DoRecursiveEl(rep, el, null,
                            SetDiagramObjectStyle,
                            liParameter,
                            changeScope);
                        break;

                }
            }

        }

        /// <summary>
        /// Wrapper to change DiagramLink style
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="pos"></param>
        /// <param name="changeScope"></param>
        // ReSharper disable once MemberCanBePrivate.Global
        public static void DiagramLinkStyleWrapper(Repository rep, int pos, ChangeScope changeScope)
        {
            if (DiagramStyle.DiagramStyleItems == null && DiagramStyle.DiagramLinkStyleItems.Count <= pos)
            {
                MessageBox.Show("", "No DiagramLink style in 'Settings.json' found");
                return;
            }
            string type = $@"{DiagramStyle.DiagramLinkStyleItems[pos].Type}".Trim();
            string style = $@"{DiagramStyle.DiagramLinkStyleItems[pos].Style}".Trim();
            string property = $@"{DiagramStyle.DiagramLinkStyleItems[pos].Property}".Trim();
            DiagramLinkStyleWrapper(rep, type, style, property, changeScope);

        }

        /// <summary>
        /// Wrapper to change DiagramLink style
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="type"></param>
        /// <param name="style"></param>
        /// <param name="property"></param>
        /// <param name="changeScope"></param>
        public static void DiagramLinkStyleWrapper(Repository rep, string type, string style, string property, ChangeScope changeScope)
        {
            EaDiagram eaDia = new EaDiagram(rep, getAllDiagramObject: false);

            // Handle selected diagram and its selected items (connector/objects)
            if (eaDia.Dia != null)
            {
                rep.SaveDiagram(eaDia.Dia.DiagramID);
                // over all links
                foreach (var link in eaDia.GetSelectedLinks())
                {
                    var linkStyle = new DiagramLinkStyle(rep, link, type, style, property);
                    if (linkStyle.IsToProcess())
                    {
                        linkStyle.UpdateStyles();
                        linkStyle.SetProperties();
                        linkStyle.SetEaLayoutStyles();
                    }

                }
                eaDia.ReloadSelectedObjectsAndConnector(saveDiagram: false);
            }
            else
            {
                var liParameter = new string[4];
                liParameter[0] = type;
                liParameter[1] = style;
                liParameter[2] = property;

                switch (rep.GetContextItemType())
                {
                    case EA.ObjectType.otPackage:
                        EA.Package pkg = (EA.Package)rep.GetContextObject();
                        RecursivePackages.DoRecursivePkg(rep, pkg, null, null,
                            SetDiagramLinkStyle,
                            liParameter,
                            changeScope);
                        break;
                    case EA.ObjectType.otElement:
                        EA.Element el = (EA.Element)rep.GetContextObject();
                        RecursivePackages.DoRecursiveEl(rep, el, null,
                            SetDiagramLinkStyle,
                            liParameter,
                            changeScope);
                        break;

                }
            }
        }
        /// <summary>
        /// Update all DiagramLinks of diagram
        /// - liParameter[0] = type;
        /// - liParameter[1] = style;</summary>
        /// - liParameter[2] = property;
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="liParameter"></param>
        private static void SetDiagramLinkStyle(EA.Repository rep, EA.Diagram dia, string[] liParameter)
        {
            rep.SaveDiagram(dia.DiagramID);

            string types = liParameter[0];
            string styles = liParameter[1];
            string properties = liParameter[2];
            foreach (EA.DiagramLink link in dia.DiagramLinks)
            {
                var linkStyle = new DiagramLinkStyle(rep, link, types, styles, properties);
                if (linkStyle.IsToProcess())
                {
                    linkStyle.UpdateStyles();
                    linkStyle.SetProperties();
                    linkStyle.SetEaLayoutStyles();
                }
            }
            rep.ReloadDiagram(dia.DiagramID);

        }
        /// <summary>
        /// Update all DiagramObjects of diagram
        /// - liParameter[0] = type;
        /// - liParameter[1] = style;
        /// - liParameter[2] = property;
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="liParameter"></param>
        private static void SetDiagramObjectStyle(EA.Repository rep, EA.Diagram dia, string[] liParameter)
        {
            rep.SaveDiagram(dia.DiagramID);

            string types = liParameter[0];
            string styles = liParameter[1];
            string properties = liParameter[2];
            foreach (EA.DiagramObject obj in dia.DiagramObjects)
            {
                var objStyle = new DiagramObjectStyle(rep, obj, types, styles, properties);
                if (objStyle.IsToProcess())
                {
                    objStyle.UpdateStyles();
                    objStyle.SetProperties();
                    objStyle.SetEaLayoutStyles();
                    objStyle.SetCompleteNessMarker();
                }
            }
            rep.ReloadDiagram(dia.DiagramID);

        }




        /// <summary>
        /// Bulk Change Diagram Style according to:
        /// liParameter[0]  Styles/StyleEx
        /// liParameter[1]  PDATA/ExtendedStyle
        /// liParameter[2]  Properties
        /// liParameter[3]  Diagram types as comma, semicolon separated list
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="liParameter"></param>
        /// <param name="changeScope"></param>
        public static void ChangeDiagramStyle(EA.Repository rep, string[] liParameter, ChangeScope changeScope = ChangeScope.PackageRecursive)
        {
            switch (rep.GetContextItemType())
            {
                case EA.ObjectType.otDiagram:
                    EA.Diagram dia = (EA.Diagram)rep.GetContextObject();
                    SetDiagramStyle(rep, dia, liParameter);
                    break;
                case EA.ObjectType.otPackage:
                    EA.Package pkg = (EA.Package)rep.GetContextObject();
                    RecursivePackages.DoRecursivePkg(rep, pkg, null, null, SetDiagramStyle,
                        liParameter,
                        changeScope);
                    break;
                case EA.ObjectType.otElement:
                    EA.Element el = (EA.Element)rep.GetContextObject();
                    RecursivePackages.DoRecursiveEl(rep, el, null, SetDiagramStyle, liParameter,
                        changeScope);
                    break;
            }
        }


        /// <summary>
        /// Set Diagram styles in PDATA and StyleEx. 
        /// 
        /// HideQuals=1 HideQualifiers: 
        /// OpParams=2  Show full Operation Parameter
        /// ScalePI=1   Scale to fit page
        /// Theme=:119  Set the diagram theme and the used features of the theme (here 119, see StyleEx of t_diagram)
        /// 
        /// par[0] contains the values as a semicolon/comma separated Style
        /// par[1] contains the values as a semicolon/comma separated PDATA
        /// par[2] contains the values as a semicolon/comma separated properties
        /// par[3] contains the possible diagram types
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dia"></param>
        /// <param name="par">par[] </param>
        private static void SetDiagramStyle(Repository rep, EA.Diagram dia, string[] par)
        {
            EaDiagram eaDia = new EaDiagram(rep, getAllDiagramObject: false);
            if (eaDia.Dia == null) return;
            rep.SaveDiagram(eaDia.Dia.DiagramID);

            string styles = par[0].Replace(",", ";");
            string pdatas = par[1].Replace(",", ";");
            string properties = par[2].Replace(",", ";");
            string types = par[3];



            var diagramStyle = new DiagramStyle(rep, dia, types, styles, pdatas, properties);
            if (diagramStyle.IsToProcess())
            {
                diagramStyle.UpdateStyles();
                diagramStyle.SetProperties(withSql: false);
                diagramStyle.SetProperties(withSql: true);
            }

            eaDia.ReloadSelectedObjectsAndConnector(saveDiagram: false);


        }
    }
}
