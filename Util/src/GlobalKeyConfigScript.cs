using System;
using EAAddinFramework.Utils;


namespace GlobalHotkeys
{
    public class GlobalKeyConfigScript : GlobalKeysConfig
    {

        public GlobalKeyConfigScript(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
                string id, string description, bool isTextRequired)
                : base(key, modifier1, modifier2, modifier3, modifier4, help)
            {
            Id = id;
            Description = description;
            IsTextRequired = isTextRequired;
        }

        #region GetterSetter
        public ScriptFunction Method { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public bool IsTextRequired { get; set; }

        #endregion
        public string Invoke(EA.Repository rep, string text)
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
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e}\nCan't invoke { Method.Name } Return:'{ Method.ReturnParameter}' { Method}", @"Error Invoking service");
                    return null;
                }
            }
            return null;
        }
    }
}
