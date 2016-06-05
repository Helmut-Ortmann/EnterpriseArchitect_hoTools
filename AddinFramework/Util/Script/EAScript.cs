
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
        public string GroupGuid { get { return _groupGuid; }
                               set { _groupGuid = value; } }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public string GUID => _guid;

        int _id;
        string _guid;    // the guid of the Script
        string _groupGuid;  // the group / author of the Script
        string _name;    // the name of the Script
        string _type;    // VBScript, JavaScript, JScript
        string _language;
        string _code; // the script itself
        string _groupName;
        Model _model;

        #region Constructor
        public EaScript(Model model, string name, string type, string language, string groupGuid, string code)
        {
            init(model, name, type, language, groupGuid, code);
        }
        public EaScript(Model model, string name, string type, string language, string code)
        {
            init(model, name, type, language, "", code);
        }

        void init(Model model, string name, string type, string language, string groupGuid, string code)
        {
            _groupGuid = groupGuid;
            _name = name;
            _type = type;
            _language = language;
            _model = model;
            _code = code;

        }
        #endregion
        public bool exists()
        {
            getInfo();
            return (_id > 0);
        }
        public void save()
        {
            string notes = $" '<Script Name=\"{_name}\" Type=\"{ _type}\" Language=\"{_language}\" Notes=\"\"/>' ";
            if (exists())
            {

                string SQLUpdate = "update t_script set " +
                                            $"script = '{_model.escapeSQLString(_code)}' " + 
                                            $", notes = {notes} "  +                      
                                    " where ScriptID = " + _id;
                _model.executeSQL(SQLUpdate);
            }
            else
            {

                string guid = "{" + Guid.NewGuid() + "}";
                string sql = "insert into t_script (ScriptCategory, ScriptAuthor, ScriptName, Notes, Script) " +
                        $" Values (" +
                                   $"'{SCRIPT_CATEGORY}', " +
                                   $"'{_groupGuid}', " +
                                   $"'{guid}', " +
                                   $"{notes}, " +
                                   $"'{_model.escapeSQLString(_code)}' )";
                _model.executeSQL(sql);
                // update script information
                getInfo();
            }
            return;

        }
        /// <summary>
        /// getInfo from script (ID, groupName, groupGuid)
        /// <para/> It get's only one Script (The same ScriptName could be in more than one ScriptGroup)
        /// </summary>
        void getInfo()
        {
            // check if author (Group of Script) is the expected
            string sqlWhereScriptGroup = "";
            if (_groupGuid != null && _groupGuid != "" )
            {
                sqlWhereScriptGroup = $" AND s.[ScriptAuthor] = '{_groupGuid}'";
            }

            string sql = "select s.[ScriptID], s.[ScriptName] as ScriptGuid, grp.[ScriptID] as GroupID, grp.[Script] as GroupName, grp.[ScriptName] as GroupGuid" +
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

            // get groupName
            var scriptGroupNameNode = x.Descendants("GroupName").FirstOrDefault();
            if (scriptGroupNameNode == null) return;
            _groupName = scriptGroupNameNode.Value;

            // get groupName
            var scriptGroupGuidNode = x.Descendants("GroupGuid").FirstOrDefault();
            if (scriptGroupGuidNode == null) return;
            _groupGuid = scriptGroupGuidNode.Value;
            return;

        }
    }
}
