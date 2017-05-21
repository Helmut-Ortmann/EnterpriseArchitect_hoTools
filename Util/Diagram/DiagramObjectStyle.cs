using System;
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


            foreach (var link in Property)
            {
                string name;
                string value;
                int iValue = 0;
                bool bValue = false;
                if (!GetNameValueFromStyle(link, out name, out value)) continue;



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
            _diaObj.Update();
        }
        /// <summary>
        /// Update styles
        /// </summary>
        public void UpdateStyles()
        {
            string oldStyle = (string)_diaObj.Style;
            oldStyle = base.UpdateStyles(oldStyle );
            _diaObj.Style = oldStyle;
            _diaObj.Update();

        }

        public bool IsToProcess()
        {
            if (Type.Length == 0) return true;
            bool isToProcessType = true;
            bool isToProcessStereotype = true;
            EA.Element el = Rep.GetElementByID(_diaObj.ElementID);
            foreach (var type in Type)
            {

                string name;
                string value;
                if (!GetNameValueFromStyle(type, out name, out value)) continue;
                switch (name)
                {
                    case "Types":
                        string nameType;
                        string valueTypes;
                        if (!GetNameValueFromStyle(type, out nameType, out valueTypes)) continue;
                        if (valueTypes.Trim() == "") continue;

                        // must be a supported Type value
                        isToProcessType = false;
                        foreach (var elType in valueTypes.Split(','))
                        {
                            if (elType == "") continue;
                            if (elType == el.Type)
                            {
                                isToProcessType = true;
                                break;
                            }
                        }

                        break;
                    case "Stereotypes":
                        string nameStereotype;
                        string valueStereotypes;
                        if (!GetNameValueFromStyle(type, out nameStereotype, out valueStereotypes)) continue;
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
                }

            }
            return isToProcessType && isToProcessStereotype;

        }


    }
}
