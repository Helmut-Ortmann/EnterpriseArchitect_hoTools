
using System.Linq;
using System.Xml.Linq;
using System;

namespace EAAddinFramework.Utils
{
    public class EaScript
    {
        // Script t_script
        // ScriptID         ID, auto generated)
        // ScriptCategory   Group:  "3955A83E-9E54-4810-8053-FACC68CD4782"
        //                  Script: "605A62F7-BCD0-4845-A8D0-7DC45B4D2E3F"
        // ScriptName       GUID of Script or Group
        // ScriptAuthor     Group:  free
        //                  Script: GUID as Group the script belongs to (JOIN script with GROUP)
        // Notes:           <Script Name =".." Type="Internal" Language="VBScript"/>
        //                  <Group Type="Normal" Notes=""/>
        // Script:          The Script itself
        //                  The Group Name
        // Script Category of a Script
        const string SCRIPT_CATEGORY = "605A62F7-BCD0-4845-A8D0-7DC45B4D2E3F";

        public int ID => _id;
        public string Author { get { return _author; }
                               set { _author = value; } }
        public string GUID => _guid;

        int _id;
        string _guid;
        string _author;  // the group / author of the Script
        string _name;    // the name of the Script/Group
        string _type;  // VBScript, JavaScript, JScript
        string _language;
        Model _model;

        #region Constructor
        public EaScript(Model model, string name, string type, string language, string author)
        {
            init(model, name, type, language, author);
        }
        public EaScript(Model model, string name, string type, string language )
        {
            init(model, name, type, language, "");
        }

        void init(Model model, string name, string type, string language, string author)
        {
            _author = author;
            _name = name;
            _type = type;
            _language = language;
            _model = model;

        }
        #endregion
        public bool exists()
        {
            getInfo();
            return (_id > 0);
        }
        public void save()
        {
            string guid = "{" + Guid.NewGuid() + "}";
            string sql = "insert into t_script (ScriptCategory, ScriptAuthor, ScriptName, Notes, Script) " +
                    $" Values ("+
                               $"'{SCRIPT_CATEGORY}',"+
                               $"'{_author}'," +
                               $"'{guid}'," +
                               $"'<Script Name=\"{_name}\" Notes=\"\"/>'," +
                               $"'my Script code')";
            _model.executeSQL(sql);
            getInfo();
            return;

        }
        void getInfo()
        {
            // check if author (Group of Script) is the expected
            string sqlWhereScriptGroup = "";
            if (_author != null && _author != "" )
            {
                sqlWhereScriptGroup = " AND s.[Author] = '{_author}'";
            }

            string sql = "select s.[ScriptID] " +
                             "  from t_script s " +
                             "       inner join t_script grp on s.ScriptAuthor = grp.ScriptName " +
                             "  where s.Script like '%EA-Matic%' AND " +
                             $"  s.Notes like '<Script Name=\"{_name}\"*' " +
                             sqlWhereScriptGroup;
            // run query into XDocument to proceed with LinQ
            string xml = _model.SQLQueryNative(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get scriptID
            var scriptIDNode = x.Descendants("ScriptID").FirstOrDefault();
            if (scriptIDNode == null) return;
            _id = (int)scriptIDNode;

            // get scriptName
            //var scriptNameNode = x.Descendants("ScriptName").FirstOrDefault();
            //if (scriptNameNode == null) return;
            //_name = scriptNameNode.Value;
            return;

        }
    }
}
