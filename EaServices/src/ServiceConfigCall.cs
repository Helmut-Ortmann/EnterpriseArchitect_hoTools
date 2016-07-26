using System;
using System.Reflection;
using System.Windows.Forms;
using EAAddinFramework.Utils;

namespace hoTools.EaServices
{
    /// <summary>
    /// Class to define the configurable services
    /// </summary>
    public class ServicesConfigCall : ServicesConfig
    {
        public bool IsTextRequired { get; }

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

        public ServicesConfigCall(int pos, string id, string buttonText) : base(pos, id, buttonText)
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
                                Method.Invoke(null, new[] {model.Repository, text, Type.Missing});
                                break;
                            case 3:
                                Method.Invoke(null, new[] {model.Repository, text, Type.Missing, Type.Missing});
                                break;
                            default:
                                Method.Invoke(null,
                                    new[] {model.Repository, text, Type.Missing, Type.Missing, Type.Missing});
                                break;
                        }
                    }
                    else
                    {
                        // use Type.Missing for optional parameters
                        switch (Method.GetParameters().Length)
                        {
                            case 1:
                                Method.Invoke(null, new object[] {model.Repository});
                                break;
                            case 2:
                                Method.Invoke(null, new[] {model.Repository, Type.Missing});
                                break;
                            case 3:
                                Method.Invoke(null, new[] {model.Repository, Type.Missing, Type.Missing});
                                break;
                            default:
                                Method.Invoke(null, new[] {model.Repository, Type.Missing, Type.Missing, Type.Missing});
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
    }
}
