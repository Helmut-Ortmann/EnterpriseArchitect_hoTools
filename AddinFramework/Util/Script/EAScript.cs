using System;
using System.Linq;
using System.Xml.Linq;

namespace EAAddinFramework.Utils
{
    public class EaScript
    {
        // Script Category of a Script
        const string SCRIPT_GROUP_ID = "605A62F7-BCD0-4845-A8D0-7DC45B4D2E3F";

        string _author;
        string _scriptName;
        string _scriptType;
        string _language;

        #region Constructor
        public EaScript(string author, string scriptName, string scriptType, string language)
        {
            _author = author;
            _scriptName = scriptName;
            _scriptType = scriptType;
            _language = language;
        }
        #endregion
        public bool exists()
        {
            return false;
        }
        public void save()
        {
            
        }
    }
}
