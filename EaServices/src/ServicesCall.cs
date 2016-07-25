using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using AddinFramework.Util;
using EA;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    public class ServiceCall: Service
    {
        bool _isTextRequired;

        /// <summary>
        /// Definition of a Service of Type Call.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="guid"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        /// <param name="isTextRequired"></param>
        public ServiceCall(MethodInfo method, string guid, string description, string help, bool isTextRequired): base(guid, description, help)
        { 
            Method = method;
            _isTextRequired = isTextRequired;
        }
        /// <summary>
        /// Create an empty Service
        /// </summary>
        public ServiceCall() : base(ServicesConfigCall.ServiceCallEmpty, "-- no --", "no service selected")
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
            return Guid == ServicesConfigCall.ServiceCallEmpty;
        }


        public MethodInfo Method { get; }


        public string Guid { get; }
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
    /// Sort/Search ServicesCalls against column GUID. Use Interface IComparable.
    /// </summary>
    public class ServicesCallGuidComparer : IComparer<ServiceCall>
    {
        public int Compare(ServiceCall firstValue, ServiceCall secondValue)
        {
            if (firstValue == null && secondValue == null) return 0;
            if (firstValue == null) return 1;
            if (secondValue == null) return -1;
            return string.Compare(firstValue.Guid, secondValue.Guid, StringComparison.Ordinal);
        }
    }





   

    
}
