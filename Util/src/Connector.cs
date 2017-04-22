using System.Collections.Generic;
using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace hoTools.Connectors {
    public class Connector
    {
        public Connector(string type, string stereotype, string lineStyle="LV", bool isDefault=false, bool isEnabled = true)
        {
            Type = type;
            Stereotype = stereotype;
            IsDefault = isDefault;
            IsEnabled = isEnabled;
            LineStyle = lineStyle;
        }

        public string LineStyle { get; }

        public string Type { get;  }

        public string Stereotype { get;  }

        public bool IsDefault { get;  }

        public bool IsEnabled { get;  }
    }
    public class DiagramConnector : BindingList<Connector>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public DiagramConnector(string diagramType)
{
                DiagramType = diagramType;
            }
        public string DiagramType { get; }
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
    }
    public class ActivityConnectors : DiagramConnector
    {

        public ActivityConnectors()
            : base("Activity")
        {

        }
    }

}
