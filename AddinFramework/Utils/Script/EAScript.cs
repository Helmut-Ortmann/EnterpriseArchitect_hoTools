
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
        const string ScriptCategory = "605A62F7-BCD0-4845-A8D0-7DC45B4D2E3F";

        int _id;
        string _groupGuid;  // the group / author of the Script
        string _name;    // the name of the Script
        string _type;    // VBScript, JavaScript, JScript
        string _language;
        string _code; // the script itself
        Model _model;

        #region Constructor
        public EaScript(Model model, string name, string type, string language, string groupGuid, string code)
        {
            Init(model, name, type, language, groupGuid, code);
        }
        // ReSharper disable once UnusedMember.Global
        public EaScript(Model model, string name, string type, string language, string code)
        {
            Init(model, name, type, language, "", code);
        }

        private void Init(Model model, string name, string type, string language, string groupGuid, string code)
        {
            _groupGuid = groupGuid;
            _name = name;
            _type = type;
            _language = language;
            _model = model;
            _code = code;

        }
        #endregion

        private bool Exists()
        {
            GetInfo();
            return (_id > 0);
        }
        public void Save()
        {
            string notes = $" '<Script Name=\"{_name}\" Type=\"{ _type}\" Language=\"{_language}\" Notes=\"\"/>' ";
            if (Exists())
            {

                string sqlUpdate = "update t_script set " +
                                            $"script = '{_model.EscapeSqlString(_code)}' " + 
                                            $", notes = {notes} "  +                      
                                    " where ScriptID = " + _id;
                _model.ExecuteSql(sqlUpdate);
            }
            else
            {

                string guid = "{" + Guid.NewGuid() + "}";
                string sql = "insert into t_script (ScriptCategory, ScriptAuthor, ScriptName, Notes, Script) " +
                        @" Values (" +
                                   $"'{ScriptCategory}', " +
                                   $"'{_groupGuid}', " +
                                   $"'{guid}', " +
                                   $"{notes}, " +
                                   $"'{_model.EscapeSqlString(_code)}' )";
                _model.ExecuteSql(sql);
                // update script information
                GetInfo();
            }
        }
        /// <summary>
        /// getInfo from script (ID, groupName, groupGuid)
        /// <para/> It get's only one Script (The same ScriptName could be in more than one ScriptGroup)
        /// </summary>
        void GetInfo()
        {
            // check if author (Group of Script) is the expected
            string sqlWhereScriptGroup = "";
            if (!string.IsNullOrEmpty(_groupGuid) )
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
            string xml = _model.SqlQueryNative(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get scriptID
            var scriptIdNode = x.Descendants("ScriptID").FirstOrDefault();
            if (scriptIdNode == null) return;
            _id = (int)scriptIdNode;

            // get groupName
            var scriptGroupNameNode = x.Descendants("GroupName").FirstOrDefault();
            if (scriptGroupNameNode == null) return;

            // get groupName
            var scriptGroupGuidNode = x.Descendants("GroupGuid").FirstOrDefault();
            if (scriptGroupGuidNode == null) return;
            _groupGuid = scriptGroupGuidNode.Value;
        }
    }
}
