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
        public ServiceCall() : base(ServicesCallConfig.ServiceCallEmpty, "-- no --", "no service selected")
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
            return Guid == ServicesCallConfig.ServiceCallEmpty;
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





   

    /// <summary>
    /// Class to define the configurable services
    /// </summary>
    public class ServicesCallConfig : ServicesConfig
    {
        public const string ServiceCallEmpty = "{B93C105E-64BC-4D9C-B92F-3DDF0C9150E6}";
        public bool IsTextRequired { get;  }

        public string Guid
        {
            get { return Id; }
            set { Id = value; }
        }

        public MethodInfo Method { get; set; }

        public string MethodName
        {
            get
            {
                if (Method == null) return "";
                return Method.Name;
            }
        }

        public string HelpTextLong
        {
            get
            {
                if (MethodName == "") return "";
                return
                    $"Service\t\t: {ButtonText} / {MethodName}()\nService Name\t: {Description}\nDescription\t: {Help}";
            }

        }

        public ServicesCallConfig(int pos, string guid, string buttonText) : base(pos, guid, buttonText)
        {

        }

        public string Invoke(Model model, string text)
        {
            if (Method != null)
            {
                try
                {
                    // Invoke the method itself. The string returned by the method winds up in s
                    // substitute default parameter by Type.Missing
                    if (IsTextRequired)
                    {
                        // use Type.Missing for optional parameters
                        switch (Method.GetParameters().Length)
                        {
                            case 1:
                                Method.Invoke(null, new object[] {model.Repository, text});
                                break;
                            case 2:
                                Method.Invoke(null, new[] { model.Repository, text, Type.Missing});
                                break;
                            case 3:
                                Method.Invoke(null, new[] { model.Repository, text, Type.Missing, Type.Missing});
                                break;
                            default:
                                Method.Invoke(null, new[] { model.Repository, text, Type.Missing, Type.Missing, Type.Missing});
                                break;
                        }
                    }
                    else
                    {
                        // use Type.Missing for optional parameters
                        switch (Method.GetParameters().Length)
                        {
                            case 1:
                                Method.Invoke(null, new object[] { model.Repository });
                                break;
                            case 2:
                                Method.Invoke(null, new[] { model.Repository, Type.Missing});
                                break;
                            case 3:
                                Method.Invoke(null, new[] { model.Repository, Type.Missing, Type.Missing});
                                break;
                            default:
                                Method.Invoke(null, new[] { model.Repository, Type.Missing, Type.Missing, Type.Missing});
                                break;
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e}\n\nCan't invoke {Method.Name}  Return:'{Method.ReturnParameter}'  {Method}",
                        $"Error Invoking service '{Method.Name}'");
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Class to define the configurable services
        /// </summary>
        public class ServicesScriptConfig : ServicesConfig
        {
            public ScriptFunction Function;

            public string functionName
            {
                get { return Id; }
                set { Id = value; }
            }

            public string HelpTextLong
            {
                get
                {
                    if (Function == null) return "";
                    return
                        $"{"Script",8}: {ButtonText} / {Function.Owner.Name}:{Function.Name}\n {Description}\nHelp\t: {Help}";
                }

            }

            public ServicesScriptConfig(int pos, ScriptFunction function, string buttonText)
                : base(pos, $"{function.Owner.Name}:{function.Name}", buttonText)
            {

            }

            public string Invoke(Model model)
            {
                if (Function != null)
                {
                    EA.ObjectType objectType = model.Repository.GetContextItemType();
                    object oContext = (object)model.Repository.GetContextObject();

                    ScriptUtility.RunScriptFunction(model, Function, objectType, oContext);

                }
                return null;
            }





        }
    }
}
