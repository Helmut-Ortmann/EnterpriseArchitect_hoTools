using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EA;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    /// <summary>
    /// Generalization of services like ServiceCall and ServiceScript.
    /// </summary>
    public class Service
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string Help { get; set; }
        public string GUID { get; set; }
        /// <summary>
        /// Creates a new service with ID=GUID or Name, description, help
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        public Service (string guid, string description, string help)
        {
            GUID = guid;
            Description = description;
            Help = help;
        }
        /// <summary>
        /// Sort ServicesCalls against column Description. Use Interface IComparable.
        /// </summary>
        public class ServicesDescriptionComparer : IComparer<Service>
        {
            public int Compare(Service firstValue, Service secondValue)
            {
                if (firstValue == null && secondValue == null) return 0;
                if (firstValue == null) return 1;
                if (secondValue == null) return -1;
                return string.Compare(firstValue.Description, secondValue.Description, StringComparison.Ordinal);
            }
        }
        
    }
    
}
