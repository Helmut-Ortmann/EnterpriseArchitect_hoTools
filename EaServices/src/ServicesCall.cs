using System;
using System.Collections.Generic;
using System.Reflection;

namespace hoTools.EaServices
{
    public class ServiceCall: Service
    {
        bool _isTextRequired;

        /// <summary>
        /// Definition of a Service of Type Call.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        /// <param name="isTextRequired"></param>
        public ServiceCall(MethodInfo method, string id, string description, string help, bool isTextRequired): base(id, description, help)
        { 
            Method = method;
            _isTextRequired = isTextRequired;
        }
        /// <summary>
        /// Create an empty Service
        /// </summary>
        public ServiceCall() : base(ServicesConfig.ServiceEmpty, "-- no --", "no service selected")
        {
            Method = null;
            _isTextRequired = false;
        }

        /// <summary>
        /// Check whether the Service is an empty service
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Id == ServicesConfig.ServiceEmpty;
        }


        public MethodInfo Method { get; }


        //public string Id { get; }
    }
    /// <summary>
    /// Sort ServicesCalls against column Description. Use Interface IComparable.
    /// </summary>
    public class ServicesCallDescriptionComparer : IComparer<ServiceCall>
    {
        public int Compare(ServiceCall firstValue, ServiceCall secondValue)
        {
            if (firstValue == null && secondValue == null) return 0;
            if (firstValue == null) return 1;
            if (secondValue == null) return -1;
            return string.Compare(firstValue.Description, secondValue.Description, StringComparison.Ordinal);
        } 
    }
    /// <summary>
    /// Sort/Search ServicesCalls against column Id. Use Interface IComparable.
    /// </summary>
    public class ServicesCallGuidComparer : IComparer<ServiceCall>
    {
        public int Compare(ServiceCall firstValue, ServiceCall secondValue)
        {
            if (firstValue == null && secondValue == null) return 0;
            if (firstValue == null) return 1;
            if (secondValue == null) return -1;
            return string.Compare(firstValue.Id, secondValue.Id, StringComparison.Ordinal);
        }
    }





   

    
}
