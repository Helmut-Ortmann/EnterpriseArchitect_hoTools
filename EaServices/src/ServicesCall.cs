using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace hoTools.EaServices
{
    public class ServiceCall
    {
        bool _isTextRequired;

        public ServiceCall(MethodInfo method, string guid, string description, string help, bool isTextRequired)
        {
            Method = method;
            Description = description;
            Guid = guid;
            Help = help;
            _isTextRequired = isTextRequired;
        }



        public string Description { get; }

        public MethodInfo Method { get; }

        public string Help { get; }

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
    public class ServicesCallConfig
    {
        public ServicesCallConfig(int pos, string guid, string buttonText)
        {
            Guid = guid;
            Pos = pos;
            ButtonText = buttonText;
        }
        public string Invoke(EA.Repository rep, string text)
        {
            if (Method != null)
            {
                try {
                    // Invoke the method itself. The string returned by the method winds up in s
                    // substitute default parameter by Type.Missing
                    if (IsTextRequired)
                    {
                        // use Type.Missing for optional parameters
                        switch (Method.GetParameters().Length)
                        {
                            case 1:
                                Method.Invoke(null, new object[] { rep, text });
                                break;
                            case 2:
                                Method.Invoke(null, new[] { rep, text, Type.Missing });
                                break;
                            case 3:
                                Method.Invoke(null, new[] { rep, text, Type.Missing, Type.Missing });
                                break;
                            default:
                                Method.Invoke(null, new[] { rep, text, Type.Missing, Type.Missing, Type.Missing });
                                break;
                        }
                    }
                    else
                    {
                        // use Type.Missing for optional parameters
                        switch (Method.GetParameters().Length)
                        {
                            case 1:
                                Method.Invoke(null, new object[] { rep });
                                break;
                            case 2:
                                Method.Invoke(null, new[] { rep, Type.Missing });
                                break;
                            case 3:
                                Method.Invoke(null, new[] { rep, Type.Missing, Type.Missing });
                                break;
                            default:
                                Method.Invoke(null, new[] { rep, Type.Missing, Type.Missing, Type.Missing });
                                break;
                        }

                    }
                } catch (Exception e)
                {
                    MessageBox.Show($"{e}\n\nCan't invoke {Method.Name}  Return:'{Method.ReturnParameter}'  {Method}" , 
                        $"Error Invoking service '{Method.Name}'");
                    return null;
                }
            }
            return null;
        }
        public string Help { get; set; }

        public MethodInfo Method { get; set; }

        public string MethodName {
            get
            {
                if (Method == null) return "";
                else return Method.Name;
            }

        }
        public string Description { get; set; }

        public string HelpTextLong
        {
            get
            {
                if (MethodName == "") return "";
                return $"Service\t\t: {ButtonText} / {MethodName}()\nService Name\t: {Description}\nDescription\t: {Help}";
            }

        }
        public int Pos { get; }

        public bool IsTextRequired { get; set; }

        public string Guid { get; set; }

        public string ButtonText { get; set; }
    }
}
