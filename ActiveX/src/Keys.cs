

namespace Control.EaAddinShortcuts
{
    public class EaAddinButtons
    {
        public string  KeyText;
        public int KeyPos;
        public string KeySearchTooltip;
        public EaAddinButtons(int pos, string text, string toolTip)
        {
            KeyPos = pos;
            KeyText = text;
            KeySearchTooltip = toolTip;
        }
    }
    public class EaAddinShortcutSearch : EaAddinButtons
    {
        public string KeyType = "Search";
        public string KeySearchName;
        public string KeySearchTerm;
        public EaAddinShortcutSearch(int keyPos, string text, string searchName, string searchTerm, string toolTip)
            : base(keyPos, text, toolTip)
        {
            KeySearchName = searchName;
            KeySearchTerm = searchTerm;
        }
        public string HelpTextLog
        {
            get
            {
                if (KeySearchName == "") return "";
                return ("Search\t\t:\t'" + KeySearchName + "'" +
                      "\nSearchTerm\t:\t'" + KeySearchTerm + "'" +
                      "\nDescription\t:\t" + KeySearchTooltip);
            }
        }
    }
}
