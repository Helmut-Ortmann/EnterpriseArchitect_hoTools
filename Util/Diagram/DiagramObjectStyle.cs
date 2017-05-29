using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EA;

namespace hoTools.Utils.Diagram
{
    public class DiagramObjectStyle : DiagramGeneralStyle
    {
        private readonly EA.DiagramObject _diaObj;
        public DiagramObjectStyle(EA.Repository rep, EA.DiagramObject diaObj, string type, string style, string property)
            : base(rep, type, style, property)
        {
            _diaObj = diaObj;
        }

        /// <summary>
        /// Set all properties
        /// </summary>
        public void SetProperties()
        {
            bool isFontName = false;
            bool isFontSize = false;
            bool isBold = false;
            bool isItalic = false;
            bool isUnderline = false;
            string fontName = "";
            int fontSize =0;
            bool bold = false;
            bool italic = false;
            bool underline = false;


            foreach (var property in Property)
            {
                string name;
                string value;
                int iValue;
                bool bValue = false;
                if (!GetNameValueFromString(property, out name, out value)) continue;



                switch (name)
                {
                    case "BackgroundColor":
                        int backgroundColor;
                        if (!ConvertInteger(name, value, out backgroundColor)) continue;
                        _diaObj.BackgroundColor = backgroundColor;
                        break;
                    case "BorderColor":
                        int borderColor;
                        if (!ConvertInteger(name, value, out borderColor)) continue;
                        _diaObj.BorderColor = borderColor;
                        break;
                    case "BorderLineWidth":
                        int borderLineWidth;
                        if (!ConvertInteger(name, value, out borderLineWidth)) continue;
                        _diaObj.BorderLineWidth = borderLineWidth;
                        break;
                    case "Bottom":
                        int bottom;
                        if (!ConvertInteger(name, value, out bottom)) continue;
                        _diaObj.bottom = bottom;
                        break;
                    case "ElementDisplayMode":
                        int elementDisplayMode;
                        if (!ConvertInteger(name, value, out elementDisplayMode)) continue;
                        _diaObj.ElementDisplayMode = (FeatureDisplayMode)elementDisplayMode;
                        break;
                    case "FeatureStereotypesToHide":
                        _diaObj.FeatureStereotypesToHide = value;
                        break;
                    case "FontBold":
                        if (!ConvertBool(name, value, out bold)) continue;
                        _diaObj.FontBold = bold;
                        isBold = true;
                        break;
                    case "FontColor":
                        if (!ConvertInteger(name, value, out iValue)) continue;
                        _diaObj.FontColor = iValue;
                        break;

                    case "FontItalic":
                        if (!ConvertBool(name, value, out italic)) continue;
                        _diaObj.FontItalic = italic;
                        isItalic = true;
                        break;
                    // currently not supported, instead:SetFontStyle
                    case "FontName":
                        fontName = value;
                        isFontName = true;
                        break;
                    case "FontSize":
                        if (!ConvertInteger(name, value, out fontSize)) continue;
                        isFontSize = true;
                        break;

                    case "FontUnderline":
                        if (!ConvertBool(name, value, out underline)) continue;
                        _diaObj.FontUnderline = underline;
                        isUnderline = true;
                        break;
                    case "IsSelectable":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.IsSelectable = bValue;
                        break;
                    case "LineColor":
                        _diaObj.SetStyleEx("LCol", value);
                        break;
                    case "LineWidth":
                        _diaObj.SetStyleEx("LWth", value);
                        break;
                    case "BCol":
                        _diaObj.SetStyleEx("BCol", value);
                        break;
                    case "BFol":
                        _diaObj.SetStyleEx("BFol", value);
                        break;
                    case "LCol":
                        _diaObj.SetStyleEx("LCol", value);
                        break;
                    case "LWth":
                        _diaObj.SetStyleEx("LWth", value);
                        break;
                    case "Sequence":
                        if (!ConvertInteger(name, value, out iValue)) continue;
                        _diaObj.Sequence = iValue;
                        break;
                    case "ShowComposedDiagram":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowComposedDiagram = bValue;
                        break;
                    case "ShowConstraints":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowConstraints = bValue;
                        break;
                    case "ShowFormattedNotes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowFormattedNotes = bValue;
                        break;
                    case "ShowFullyQualifiedTags":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowFullyQualifiedTags = bValue;
                        break;
                    case "ShowInheritedAttributes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowInheritedAttributes = bValue;
                        break;
                    case "ShowInheritedConstraints":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowInheritedConstraints = bValue;
                        break;
                    case "ShowInheritedOperations":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowInheritedOperations = bValue;
                        break;
                    case "ShowInheritedResponsibilities":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowInheritedResponsibilities = bValue;
                        break;
                    case "ShowInheritedTags":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowInheritedTags = bValue;
                        break;
                    case "ShowNotes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowNotes = bValue;
                        break;
                    case "ShowPortType":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowPortType = bValue;
                        break;
                    case "ShowPrivateAttributes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowPrivateAttributes = bValue;
                        break;
                    case "ShowPublicAttributes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowPublicAttributes = bValue;
                        break;
                    case "ShowProtectedAttributes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowProtectedAttributes = bValue;
                        break;
                    case "ShowPrivateOperations":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowPrivateOperations = bValue;
                        break;
                    case "ShowPublicOperations":
                        _diaObj.ShowPublicOperations = bValue;
                        break;
                    case "ShowProtectedOperations":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowProtectedOperations = bValue;
                        break;
                    case "ShowPackageOperations":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowPackageOperations = bValue;
                        break;
                    case "ShowPackageAttributes":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowPackageAttributes = bValue;
                        break;
                    case "ShowResponsibilities":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowResponsibilities = bValue;
                        break;
                    case "ShowRunstates":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowRunstates = bValue;
                        break;
                    case "ShowStructuredCompartments":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowStructuredCompartments = bValue;
                        break;
                    case "ShowTags":
                        if (!ConvertBool(name, value, out bValue)) continue;
                        _diaObj.ShowTags = bValue;
                        break;
                    case "TextAlign":
                        if (!ConvertInteger(name, value, out iValue)) continue;
                        _diaObj.TextAlign = (TextAlignment)iValue;
                        break;
                }
                if (isFontSize && isFontName && isBold && isItalic && isUnderline)
                    _diaObj.SetFontStyle(fontName, fontSize, bold, italic, underline);
            }
            Update();
        }

       

        /// <summary>
        /// Set completeness marker for object
        /// 
        /// </summary>
        public void SetCompleteNessMarker()
        {
            string completeDiagram = "CompleteDiagram=";
            foreach (var property in Property)
            {
                if (property.StartsWith(completeDiagram))
                {
                    // Completeness marker detected
                    string[] parameters = property.Substring(completeDiagram.Length).Split(':');
                    string propertyName = parameters[0];
                    bool isCompleteDiagram = IsCompleteDiagram();
                    bool isCompleteGlobal = IsCompleteGlobal();
                    string par;
                    if (isCompleteGlobal)
                    {
                        par = parameters[1];  // Complete
                    }
                    else
                    {
                        if (isCompleteDiagram) par = parameters[2]; // Complete in Diagram
                        else par = parameters[3];  // Incomplete in Diagram 
                    }
                    int iValue;
                    switch (propertyName)
                    {
                        case "BorderColor":
                            if (!ConvertInteger(propertyName, par, out iValue)) continue;
                            _diaObj.BorderColor = iValue;
                            break;
                        case "BackgroundColor":
                            if (!ConvertInteger(propertyName, par, out iValue)) continue;
                            _diaObj.BackgroundColor = iValue;
                            break;
                        case "BorderLineWidth":
                            if (!ConvertInteger(propertyName, par, out iValue)) continue;
                            _diaObj.BorderLineWidth = iValue;
                            break;
                        case "FontColor":
                            if (!ConvertInteger(propertyName, par, out iValue)) continue;
                            _diaObj.FontColor = iValue;
                            break;
                    }


                }
            }
            Update();
            return;
        }


        /// <summary>
        /// Set according ti EA Text Styles
        /// </summary>
        public void SetEaLayoutStyles()
        {
            if (EaLayoutStyle == null) return;
            foreach (var s in EaLayoutStyle)
            {
                string style = s.Trim();
                if (style == "") continue;
                string name;
                string value;
                if (!GetNameValueFromString(style, out name, out value)) continue;
                switch (name)
                {
                    // Backgroundcolor
                    case "Fill":
                        _diaObj.SetStyleEx("BCol", value);
                        break;
                    // Linewidth
                    case @"Line":
                        _diaObj.SetStyleEx("LWth", value);
                        break;
                        //Font color
                    case @"Font":
                        _diaObj.SetStyleEx(@"BFol", value);
                        break;
                    // Border or Lincolor
                    case "Border":
                        _diaObj.SetStyleEx("LCol", value);
                        break;
                    // fontname
                    case @"fontname":
                        _diaObj.SetStyleEx(@"font", value);
                        break;
                    // 
                    case @"fontsz":
                        _diaObj.SetStyleEx(@"fontsz", value);
                        break;
                    case "italic":
                        _diaObj.SetStyleEx("italic", value);
                        break;
                    case "bold":
                        _diaObj.SetStyleEx("bold", value);
                        break;
                    case @"ul":  // underline
                        _diaObj.SetStyleEx(@"ul", value);
                        break;
                }
            }
           Update();
        }
        /// <summary>
        /// Update styles
        /// </summary>
        public void UpdateStyles()
        {
            string oldStyle = (string)_diaObj.Style;
            oldStyle = base.UpdateStyles(oldStyle );
            _diaObj.Style = oldStyle;
            Update();

        }
        /// <summary>
        /// Update Diagram Object
        /// </summary>
        private void Update()
        {
            try
            {
                _diaObj.Update();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Style is possibly to long for DB field\r\n\r\e{e}", "Cant write Diagram Styles!");
            }
        }

        public bool IsToProcess()
        {
            if (Type.Length == 0) return true;
            bool isToProcessType = true;
            bool isToProcessStereotype = true;
            bool isIncompleteLinks = true;
            bool isCompleteLinks = true;
            EA.Element el = Rep.GetElementByID(_diaObj.ElementID);
            foreach (var type in Type)
            {
                if (String.IsNullOrWhiteSpace(type)) continue;
                string name;
                string value;
                if (!GetNameValueFromString(type, out name, out value)) continue;
                switch (name.Substring(0,4).ToLower())
                {
                    case "type":
                        string nameType;
                        string valueTypes;
                        if (!GetNameValueFromString(type, out nameType, out valueTypes)) continue;
                        if (valueTypes.Trim() == "") continue;

                        // must be a supported Type value
                        isToProcessType = false;
                        foreach (var t in valueTypes.Split(','))
                        {
                            string elType = t.Trim();
                            if (elType == "") continue;
                            if (elType.Equals(el.Type))
                            {
                                isToProcessType = true;
                                break;
                            }
                        }

                        break;
                    case @"ster":
                        string nameStereotype;
                        string valueStereotypes;
                        if (!GetNameValueFromString(type, out nameStereotype, out valueStereotypes)) continue;
                        if (valueStereotypes.Trim() == "") continue;

                        // must be a supported Stereotype value
                        isToProcessStereotype = false;
                        foreach (var objStereo in valueStereotypes.Split(','))
                        {
                            if (objStereo == "") continue;

                            // check if stereotype exists
                            if (Array.IndexOf(el.StereotypeEx.Split(','), objStereo) > -1)
                            {
                                isToProcessStereotype = true;
                                break;
                            }
                        }


                        break;
                    // IncompleLinks 'InCompleteLinks'
                    case @"inco":
                        isIncompleteLinks = IsCompleteDiagram() ^true;
                        break;
                    // compleLinks 'CompleteLinks'
                    case @"compl":
                        isCompleteLinks = IsCompleteDiagram();
                        break;
                }

            }
            return isToProcessType && isToProcessStereotype && isIncompleteLinks && isCompleteLinks;

        }
        /// <summary>
        /// Check if all links in diagram are visible (diagram local approach).
        /// </summary>
        /// <returns></returns>
        public bool IsCompleteDiagram()
        {
          
            EA.Diagram dia = Rep.GetDiagramByID(_diaObj.DiagramID);
           
            foreach (EA.DiagramLink l in dia.DiagramLinks)
            {
                if (l.IsHidden == true)
                {
                    var con = Rep.GetConnectorByID(l.ConnectorID);
                    var sourceObject = dia.GetDiagramObjectByID(con.SupplierID, "");
                    var targetObject = dia.GetDiagramObjectByID(con.ClientID, "");
                    if ( (sourceObject.ElementID == _diaObj.ElementID && sourceObject.ObjectType == _diaObj.ObjectType) ||
                         (targetObject.ElementID == _diaObj.ElementID && targetObject.ObjectType == _diaObj.ObjectType) )
                          return false;
                } 
            }
            return true;

        }
        /// <summary>
        /// Check if all connectors are visible in diagram (global approach). It uses the direction of UML/EA.
        /// </summary>
        /// <returns></returns>
        public bool IsCompleteGlobal()
        {

            EA.Diagram dia = Rep.GetDiagramByID(_diaObj.DiagramID);

            List<int> globalIds = new List<int>();
            globalIds.AddRange(from Connector c in Rep.GetElementByID(_diaObj.ElementID).Connectors select c.ConnectorID);
            // get all connectors of object
            //switch (_diaObj.ObjectType)
            //{
            //    case EA.ObjectType.otElement:

            //        globalIds.AddRange(from Connector c in Rep.GetElementByID(_diaObj.ElementID).Connectors select c.ConnectorID);
            //        break;

            //    case EA.ObjectType.otPackage:
            //        globalIds.AddRange(from Connector c in Rep.GetPackageByID(_diaObj.ElementID).Connectors select c.ConnectorID);
            //        break;
            //}

            List<int> diagramIds = new List<int>();
            foreach (EA.DiagramLink l in dia.DiagramLinks)
            {
                if (l.IsHidden == false) diagramIds.Add(l.ConnectorID);
            }

            foreach (int id in globalIds)
            {
                if (! diagramIds.Contains(id)) return false;
            }
            return true;

        }


    }
}
