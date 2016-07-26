using System.Collections.Generic;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace
namespace GlobalHotkeys
{
    public class GlobalKeysConfig
    {
        public GlobalKeysConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4,
            string help)
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
                {"None", Keys.None},
                {"A", Keys.A},
                {"B", Keys.B},
                {"C", Keys.C},
                {"D", Keys.D},
                {"E", Keys.E},
                {"F", Keys.F},
                {"F1", Keys.F1},
                {"F2", Keys.F2},
                {"F3", Keys.F3},
                {"F4", Keys.F4},
                {"F5", Keys.F5},
                {"F6", Keys.F6},
                {"F7", Keys.F7},
                {"F8", Keys.F9},
                {"F9", Keys.F6},
                {"F10", Keys.F10},
                {"F11", Keys.F11},
                {"F12", Keys.F12},
                {"G", Keys.G},
                {"H", Keys.H},
                {"I", Keys.I},
                {"J", Keys.J},
                {"K", Keys.K},
                {"L", Keys.L},
                {"M", Keys.M},
                {"N", Keys.N},
                {"O", Keys.O},
                {"P", Keys.P},
                {"Q", Keys.Q},
                {"R", Keys.R},
                {"S", Keys.S},
                {"T", Keys.T},
                {"U", Keys.U},
                {"V", Keys.V},
                {"W", Keys.W},
                {"X", Keys.X},
                {"Y", Keys.Y},
                {"Z", Keys.Z},
                {"0", Keys.D0},
                {"1", Keys.D1},
                {"2", Keys.D2},
                {"3", Keys.D3},
                {"4", Keys.D4},
                {"5", Keys.D5},
                {"6", Keys.D6},
                {"7", Keys.D7},
                {"8", Keys.D8},
                {"9", Keys.D9},
                {"Left", Keys.Left},
                {"Right", Keys.Right},
                {"Up", Keys.Up},
                {"Down", Keys.Down},
                {"NumPad0", Keys.NumPad0},
                {"NumPad1", Keys.NumPad1},
                {"NumPad2", Keys.NumPad2},
                {"NumPad3", Keys.NumPad3},
                {"NumPad4", Keys.NumPad4},
                {"NumPad5", Keys.NumPad5},
                {"NumPad6", Keys.NumPad6},
                {"NumPad7", Keys.NumPad7},
                {"NumPad8", Keys.NumPad8},
                {"NumPad9", Keys.NumPad9}

            };

        public static Dictionary<string, Modifiers> GetModifiers() =>
            new Dictionary<string, Modifiers>
            {
                {"Alt", Modifiers.Alt},
                {"Ctrl", Modifiers.Ctrl},
                {"No", Modifiers.NoMod},
                {"Shift", Modifiers.Shift},
                {"Win", Modifiers.Win}
            };
    }
}
