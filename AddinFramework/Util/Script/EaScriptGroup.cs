using System;
using System.Linq;
using System.Xml.Linq;



namespace EAAddinFramework.Utils
{
    public class EaScriptGroup
    {
        public enum EaScriptGroupType
        {
            NORMAL,
            PROJBROWSER,
            CONTEXTELEMENT,
            MODELSEARCH
        }
        // Script Category of a Script
        const string SCRIPT_GROUP_ID = "3955A83E-9E54-4810-8053-FACC68CD4782";

        #region Constructor
        string _groupName;  // Group Name
        string _scriptName; // GUID of Script or Group
        int _scriptId;      // Script ID
        string _note;
        Model _model;
        EaScriptGroupType _groupType;
        public EaScriptGroup(Model model, string groupName, EaScriptGroupType groupType)
        {
            init(model, groupName, groupType, "");
        }
        public EaScriptGroup(Model model, string groupName, EaScriptGroupType groupType, string note)
        {
            init(model, groupName, groupType, note);
        }
    /// <summary>
    /// Initialize ScriptGroup
    /// </summary>
    /// <param name="model"></param>
    /// <param name="groupName"></param>
    /// <param name="groupType"></param>
    /// <param name="note"></param>
        void init(Model model, string groupName, EaScriptGroupType groupType, string note)
        {
            _model = model;
            _groupName = groupName;
            _groupType = groupType;
            _note = note;
        }

        public bool exists()
        {
            getScriptInfo();
            return (_scriptId > 0);

            
        }

        void getScriptInfo()
        {
            string sql = "select s.[ScriptID], s.[ScriptName] " +
                         " from t_script s " +
                         $" where s.Notes like '<Group Type=\"{getGroupType()}\"*'    AND " +
                         $"       s.Script = '{_groupName}'  ";
            // run query into XDocument to proceed with LinQ
            string xml = _model.SQLQueryNative(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get scriptID
            var scriptIDNode = x.Descendants("ScriptID").FirstOrDefault();
            if (scriptIDNode == null) return;
            _scriptId = (int)scriptIDNode;

            // get scriptName
            var scriptNameNode = x.Descendants("ScriptName").FirstOrDefault();
            if (scriptNameNode == null) return;
            _scriptName = scriptNameNode.Value;
            return; 

        }

        /// <summary>
        /// Save EaScriptGroup.
        /// </summary>
        /// <returns>true = successful</returns>
        public bool save()
        {
            var GUID = "{" + Guid.NewGuid() + "}";
            string sql = "insert into t_script (ScriptCategory, ScriptName,Notes, Script) " +
                    $" Values ('{SCRIPT_GROUP_ID}','{GUID}','<Group Type=\"{getGroupType()}\" Notes=\"\"/>','{_groupName}')";
            _model.executeSQL(sql);
            getScriptInfo();
            return true;
        }
        string getGroupType()
        {
            return _groupType.ToString();
        }
        #endregion
    }
}
