using System;
using System.Collections.Generic;

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
        public string Id { get; set; }
        /// <summary>
        /// Creates a new service with ID=Id or Name, description, help
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        public Service (string id, string description, string help)
        {
            Id = id;
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
