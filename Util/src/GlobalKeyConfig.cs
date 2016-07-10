using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace GlobalHotkeys
{

    public class GlobalKeysConfig
    {
        private GlobalKeysConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help)
        {
            Key = key;
            Modifier1 = modifier1;
            Modifier2 = modifier2;
            Modifier3 = modifier3;
            Modifier4 = modifier4;
            Tooltip = help;

        }

       #region GetterSetter
       public string Key { get; set; }

        public string Modifier1 { get; set; }

        public string Modifier2 { get; set; }

        public string Modifier3 { get; set; }

        public string Modifier4 { get; set; }

        public string Tooltip { get; set; }

        #endregion

        public static Dictionary<string, Keys> GetKeys() => 
            new Dictionary<string, Keys>
            {
              {"None",Keys.None },
              {"A",Keys.A}, {"B",Keys.B}, {"C",Keys.C}, {"D",Keys.D}, {"E",Keys.E}, {"F",Keys.F},
              {"F1",Keys.F1},{"F2",Keys.F2}, {"F3",Keys.F3},{"F4",Keys.F4}, {"F5",Keys.F5},{"F6",Keys.F6}, {"F7",Keys.F7},{"F8",Keys.F9},{"F9",Keys.F6}, {"F10",Keys.F10},{"F11",Keys.F11},{"F12",Keys.F12},
              {"G",Keys.G}, {"H",Keys.H}, {"I",Keys.I}, {"J",Keys.J}, {"K",Keys.K}, {"L",Keys.L},
              {"M",Keys.M}, {"N",Keys.N}, {"O",Keys.O}, {"P",Keys.P}, {"Q",Keys.Q}, {"R",Keys.R},
               {"S",Keys.S}, {"T",Keys.T}, {"U",Keys.U}, {"V",Keys.V}, {"W",Keys.W}, {"X",Keys.X}, {"Y",Keys.Y}, {"Z",Keys.Z},
              {"0",Keys.D0}, {"1",Keys.D1}, {"2",Keys.D2}, {"3",Keys.D3}, {"4",Keys.D4}, {"5",Keys.D5}, {"6",Keys.D6}, {"7",Keys.D7}, {"8",Keys.D8}, {"9",Keys.D9},
              {"Left",Keys.Left}, {"Right",Keys.Right},{"Up",Keys.Up},{"Down",Keys.Down},
              {"NumPad0",Keys.NumPad0}, {"NumPad1",Keys.NumPad1}, {"NumPad2",Keys.NumPad2}, {"NumPad3",Keys.NumPad3},
              {"NumPad4",Keys.NumPad4}, {"NumPad5",Keys.NumPad5}, {"NumPad6",Keys.NumPad6}, {"NumPad7",Keys.NumPad7},
              {"NumPad8",Keys.NumPad8}, {"NumPad9",Keys.NumPad9}

            };

        public static Dictionary<string, Modifiers> GetModifiers() => 
            new Dictionary<string, Modifiers>
            { {"Alt",Modifiers.Alt},
              {"Ctrl",Modifiers.Ctrl},
              {"No",Modifiers.NoMod},
              {"Shift",Modifiers.Shift},
              {"Win",Modifiers.Win}
            };

        public class GlobalKeysServiceConfig : GlobalKeysConfig
        {
            public GlobalKeysServiceConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
                string guid, string description, bool isTextRequired)
                : base(key, modifier1, modifier2, modifier3, modifier4, help)
            {
                Guid = guid;
                Description = description;
                IsTextRequired = isTextRequired;
            }

            #region GetterSetter
            public MethodInfo Method { get; set; }

            public string Guid { get; set; }

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
                                    Method.Invoke(null, new[] { rep,text, Type.Missing, Type.Missing });
                                    break;
                                default:
                                    Method.Invoke(null, new[] { rep,text,  Type.Missing, Type.Missing, Type.Missing });
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
        public class GlobalKeysSearchConfig : GlobalKeysConfig
        {
            public GlobalKeysSearchConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
                string searchName, string searchTerm)
                : base(key, modifier1, modifier2, modifier3, modifier4, help)
            {
                SearchName = searchName;
                SearchTerm = searchTerm;
            }

            #region GetterSetter
            
            public string SearchName { get; set; }

            public string SearchTerm { get; set; }

            public string Description
            {
                get { return SearchTerm; }
                set { SearchTerm = value; }
            }
            #endregion
        }
       

    }
}