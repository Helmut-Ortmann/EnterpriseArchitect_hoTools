

namespace Control.EaAddinShortcuts
{
    public class EaAddinButtons
    {
        public string  KeyText;
        public readonly int KeyPos;
        public string KeySearchTooltip;

        protected EaAddinButtons(int pos, string text, string toolTip)
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

        public bool IsEmpty()
        {
            if (KeyText.Trim() == "" || KeySearchName.Trim() == "") return true;
            return false;
        }
        public EaAddinShortcutSearch(int keyPos, string text, string searchName, string searchTerm, string toolTip)
            : base(keyPos, text, toolTip)
        {
            KeySearchName = searchName;
            KeySearchTerm = searchTerm;
        }
        public string HelpTextLong
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
