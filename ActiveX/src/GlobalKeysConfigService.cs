using System;
using System.Windows.Forms;
using System.Reflection;
using EAAddinFramework.Utils;

// ReSharper disable once CheckNamespace
namespace GlobalHotkeys
{

   

        public class GlobalKeysConfigService : GlobalKeysConfig
        {
        /// <summary>
        /// Create Key Config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifier1"></param>
        /// <param name="modifier2"></param>
        /// <param name="modifier3"></param>
        /// <param name="modifier4"></param>
        /// <param name="help"></param>
        /// <param name="method"></param>
        /// <param name="guid"></param>
        /// <param name="description"></param>
        /// <param name="isTextRequired"></param>
            public GlobalKeysConfigService(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
                MethodInfo method, string guid, string description, bool isTextRequired)
                : base(guid, key, modifier1, modifier2, modifier3, modifier4, description, help)
            {
                IsTextRequired = isTextRequired;
                Method = method;
            }

            #region GetterSetter
            public MethodInfo Method { get; set; }

            public bool IsTextRequired { get; set; }

            #endregion
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
                                    Method.Invoke(null, new object[] { model.Repository, text });
                                    break;
                                case 2:
                                    Method.Invoke(null, new[] { model.Repository, text, Type.Missing });
                                    break;
                                case 3:
                                    Method.Invoke(null, new[] { model.Repository, text, Type.Missing, Type.Missing });
                                    break;
                                default:
                                    Method.Invoke(null, new[] { model.Repository, text,  Type.Missing, Type.Missing, Type.Missing });
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
                                    Method.Invoke(null, new[] { model.Repository, Type.Missing });
                                    break;
                                case 3:
                                    Method.Invoke(null, new[] { model.Repository, Type.Missing, Type.Missing });
                                    break;
                                default:
                                    Method.Invoke(null, new[] { model.Repository, Type.Missing, Type.Missing, Type.Missing });
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
