using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace hoTools.Connectors {
    public class Connector
    {
        string _type = "";
        string _stereotype = "";
        bool _isDefault;
        bool _isEnabled = true;
        int _pos;
        string _lineStyle = "LV";
       

        public Connector(string type, string stereotype, string lineStyle="LV", bool isDefault=false, bool isEnabled = true)
        {
            _type = type;
            _stereotype = stereotype;
            _isDefault = isDefault;
            _isEnabled = isEnabled;
            _lineStyle = lineStyle;
        }
        static public List<string> GetLineStyle() => new List<String> {
        "LV","LH","TV","TH","B","OS","OR","D"};

        public string LineStyle
        {
            get { return _lineStyle; }
            set { _lineStyle = value; }
        }
        
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
         public int Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public string Stereotype
        {
            get { return _stereotype; }
            set { _stereotype = value; }
        }
        public bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }
    }
    public class DiagramConnector : BindingList<Connector>
    {
        private string _diagramType = "";

        public DiagramConnector(string diagramType)
{
                _diagramType = diagramType;
            }
        public string DiagramType
        {
            get { return _diagramType; }
            set { _diagramType = value; }
        }
       
    }
    public class LogicalConnectors : DiagramConnector
    {

        public LogicalConnectors() : base("Logical") 
        {
           
        }
        public List<string> GetConnectorTypes() => new List<string> {
                "Abstraction", "Aggregation", "Assembly", "Association", "Collaboration", "Connector","ControlFlow", "Delegate","Deployment", "ERLink",
                "Extension", "InformationFlow",  "Instantiation", "InterruptFlow", "Manifast", "Nesting", "NoteLink", "ObjectFlow", "Package", "Realisation",
                "Sequence", "StateFlow", "Substitution", "Usage", "UseCase"
            };
        public List<String> GetStandardStereotypes() => new List<String> {
                "trace", "trace1", "trace2"
            };

    }
    public class ActivityConnectors : DiagramConnector
    {

        public ActivityConnectors()
            : base("Activity")
        {

        }
        public List<String> GetConnectorTypes() => new List<String> {
                "DataFlow", "ControlFlow"
            };
        public List<String> GetStandardStereotypes() => new List<String>
        {

        };

    }

}
