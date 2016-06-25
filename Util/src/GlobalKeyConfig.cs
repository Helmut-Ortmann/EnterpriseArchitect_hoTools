using System;
using System.Windows.Forms;
using GlobalHotkeys;
using System.Collections.Generic;
using System.Reflection;

namespace GlobalHotkeys
{

    public class GlobalKeysConfig
    {
        string _key;
        string _modifier1;
        string _modifier2;
        string _modifier3;
        string _modifier4;
        string _tooltip;
        
       public GlobalKeysConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help)
        {
            _key = key;
            _modifier1 = modifier1;
            _modifier2 = modifier2;
            _modifier3 = modifier3;
            _modifier4 = modifier4;
            _tooltip = help;

        }

       #region GetterSetter
       public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        public string Modifier1
        {
            get { return _modifier1; }
            set { _modifier1 = value; }
        }
        public string Modifier2
        {
            get { return _modifier2; }
            set { _modifier2 = value; }
        }
        public string Modifier3
        {
            get { return _modifier3; }
            set { _modifier3 = value; }
        }
        public string Modifier4
        {
            get { return _modifier4; }
            set { _modifier4 = value; }
        }
        public string Tooltip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }
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
            private MethodInfo _method;
            private string _guid = "";
            private string _description = "";
            private bool _isTextRequired;

            public GlobalKeysServiceConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
                string guid, string description, bool isTextRequired)
                : base(key, modifier1, modifier2, modifier3, modifier4, help)
            {
                _guid = guid;
                _description = description;
                _isTextRequired = isTextRequired;
            }

            #region GetterSetter
            public MethodInfo Method
            {
                get { return _method; }
                set { _method = value; }
            }
            public string Guid
            {
                get { return _guid; }
                set { _guid = value; }
            }
            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }
            
            public bool IsTextRequired
            {
                get { return _isTextRequired; }
                set { _isTextRequired = value; }
            }
            #endregion
            public string Invoke(EA.Repository rep, string text)
            {
                object s = null;
                if (_method != null)
                {
                    try
                    {
                        // Invoke the method itself. The string returned by the method winds up in s
                        // substitute default parameter by Type.Missing
                        if (_isTextRequired)
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
                                    _method.Invoke(null, new object[] { rep,text, Type.Missing, Type.Missing });
                                    break;
                                default:
                                    _method.Invoke(null, new object[] { rep,text,  Type.Missing, Type.Missing, Type.Missing });
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
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e + "\nCan't invoke " + _method.Name + "Return:'" + _method.ReturnParameter + "' " + _method, "Error Invoking service");
                        return (string)s;
                    }
                }
                return null;
            }
        }
        public class GlobalKeysSearchConfig : GlobalKeysConfig
        {
            private string _searchName = "";
            private string _searchTerm = "";

            public GlobalKeysSearchConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
                string searchName, string searchTerm)
                : base(key, modifier1, modifier2, modifier3, modifier4, help)
            {
                _searchName = searchName;
                _searchTerm = searchTerm;
            }

            #region GetterSetter
            
            public string SearchName
            {
                get { return _searchName; }
                set { _searchName = value; }
            }
            public string SearchTerm
            {
                get { return _searchTerm; }
                set { _searchTerm = value; }
            }
            public string Description
            {
                get { return _searchTerm; }
                set { _searchTerm = value; }
            }
            #endregion
        }
       

    }
}