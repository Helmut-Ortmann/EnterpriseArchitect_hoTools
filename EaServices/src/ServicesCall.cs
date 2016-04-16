using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace hoTools.EaServices
{
    public class ServiceCall
    {
        private MethodInfo _method;
        private string _GUID = "";
        private string _Description = "";
        private string _Help = "";
        private bool _IsTextRequired = false;

        public ServiceCall(MethodInfo Method, string GUID, string Description, string Help, bool IsTextRequired)
        {
            _method = Method;
            _Description = Description;
            _GUID = GUID;
            _Help = Help;
            _IsTextRequired = IsTextRequired;
        }



        public String Description => this._Description;
        public MethodInfo Method => this._method;
        public string Help => this._Help;
        public String GUID => this._GUID;
    }
    /// <summary>
    /// Sort ServicesCalls against column Description. Use Interface IComparable.
    /// </summary>
    /// <param name="x">first value of Description to compare</param>
    /// <param name="y">second value of Description to compare</param>
    public class ServicesCallDescriptionComparer : IComparer<ServiceCall>
    {
        public int Compare(ServiceCall x, ServiceCall y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return string.Compare(x.Description, y.Description, StringComparison.Ordinal);
        } 
    }
    /// <summary>
    /// Sort/Search ServicesCalls against column GUID. Use Interface IComparable.
    /// </summary>
    /// <param name="x">first value of Description to compare</param>
    /// <param name="y">second value of Description to compare</param>
    public class ServicesCallGUIDComparer : IComparer<ServiceCall>
    {
        public int Compare(ServiceCall x, ServiceCall y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return string.Compare(x.GUID, y.GUID, StringComparison.Ordinal);
        }
    }
    /// <summary>
    /// Class to define the configurable services
    /// </summary>
    /// <param name="GUID">GUID of service</param>
    public class ServicesCallConfig
    {
        private MethodInfo _method = null;
        private string _GUID;
        private int _pos;
        private string _buttonText;
        private string _description;
        private string _help;
        private bool _IsTextRequired;
        public ServicesCallConfig(int Pos, string GUID, string ButtonText)
        {
            _GUID = GUID;
            _pos = Pos;
            _buttonText = ButtonText;
        }
        public string Invoke(EA.Repository rep, string text)
        {
            object s = null;
            if (_method != null)
            {
                try {
                    // Invoke the method itself. The string returned by the method winds up in s
                    // substitute default parameter by Type.Missing
                    if (_IsTextRequired)
                    {
                        // use Type.Missing for optional parameters
                        switch (_method.GetParameters().Length)
                        {
                            case 1:
                                _method.Invoke(null, new object[] { rep, text });
                                break;
                            case 2:
                                _method.Invoke(null, new object[] { rep, text, Type.Missing });
                                break;
                            case 3:
                                _method.Invoke(null, new object[] { rep, text, Type.Missing, Type.Missing });
                                break;
                            default:
                                _method.Invoke(null, new object[] { rep, text, Type.Missing, Type.Missing, Type.Missing });
                                break;
                        }
                    }
                    else
                    {
                        // use Type.Missing for optional parameters
                        switch (_method.GetParameters().Length)
                        {
                            case 1:
                                _method.Invoke(null, new object[] { rep });
                                break;
                            case 2:
                                _method.Invoke(null, new object[] { rep, Type.Missing });
                                break;
                            case 3:
                                _method.Invoke(null, new object[] { rep, Type.Missing, Type.Missing });
                                break;
                            default:
                                _method.Invoke(null, new object[] { rep, Type.Missing, Type.Missing, Type.Missing });
                                break;
                        }

                    }
                } catch (Exception e)
                {
                    MessageBox.Show(e.ToString() +  "\nCan't invoke " + _method.Name + "Return:'"+ _method.ReturnParameter + "' "+_method.ToString(),"Error Invoking service");
                    return (string)s;
                }
            }
            return null;
        }
        public string Help
        {
            get
            {
                return this._help;

            }
            set
            {
                this._help = value;

            }
        }
        public MethodInfo Method
        {
            get
            {
                return this._method;

            }
            set
            {
                this._method = value;

            }
        }
        public string MethodName {
            get
            {
                if (this._method == null) return "";
                else return this._method.Name;
            }

        }
        public string Description
        {
            get
            {
                return this._description; 
            }
            set
            {
                this._description = value;
            }

        }
        public string HelpTextLong
        {
            get
            {
                if (MethodName == "") return "";
                return "MethodName\t:\t"+ MethodName + "()\nDescription1\t:\t" + Description + "\nDescription2\t:\t" + this._help;
            }

        }
        public int Pos => this._pos;
        public bool IsTextRequired
         {
             get
             {
                 return this._IsTextRequired;

             }
             set
             {
                 this._IsTextRequired = value;

             }
         }
         public string GUID
         {
             get
             {
                 return this._GUID;

             }
             set
             {
                this._GUID = value;

             }
         }
         public string ButtonText
         {
             get
             {
                 return this._buttonText;

             }
              set
             {
                 this._buttonText = value;

             }
         }
    }
}
