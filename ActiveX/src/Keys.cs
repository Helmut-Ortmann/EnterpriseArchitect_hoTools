using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Control.EaAddinShortcuts
{
    public class EaAddinButtons
    {
        public string  keyText;
        public int keyPos;
        public string keySearchTooltip = "";
        public EaAddinButtons(int pos, string Text, string toolTip)
        {
            keyPos = pos;
            keyText = Text;
            keySearchTooltip = toolTip;
        }
    }
    public class EaAddinShortcutSearch : EaAddinButtons
    {
        public string keyType = "Search";
        public string keySearchName = "";
        public string keySearchTerm = "";
        public EaAddinShortcutSearch(int keyPos, string text, string searchName, string searchTerm, string toolTip)
            : base(keyPos, text, toolTip)
        {
            keySearchName = searchName;
            keySearchTerm = searchTerm;
        }
        public string HelpTextLog
        {
            get
            {
                if (keySearchName == "") return "";
                return ("Search\t\t:\t'" + keySearchName + "'" +
                      "\nSearchTerm\t:\t'" + keySearchTerm + "'" +
                      "\nDescription\t:\t" + keySearchTooltip);
            }
        }
    }
}
