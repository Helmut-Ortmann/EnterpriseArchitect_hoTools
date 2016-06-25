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
        const string SCRIPT_GROUP_CATEGORY = "3955A83E-9E54-4810-8053-FACC68CD4782";

        public string Name => _name;
        public int ID => _id;
        public string GUID => _guid;

        #region Constructor
        string _name;  // Group Name
        string _guid; // GUID of Script or Group
        int _id;      // Script ID
        string _note;
        Model _model;
        EaScriptGroupType _groupType;
        public EaScriptGroup(Model model, string name, EaScriptGroupType groupType)
        {
            init(model, name, groupType, "");
        }
        public EaScriptGroup(Model model, string name, EaScriptGroupType groupType, string note)
        {
            init(model, name, groupType, note);
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
            _name = groupName;
            _groupType = groupType;
            _note = note;
        }

        public bool exists()
        {
            getInfo();
            return (_id > 0);

            
        }

        void getInfo()
        {
            string sql = "select s.[ScriptID], s.[ScriptName] " +
                         " from t_script s " +
                         $" where s.Notes like '<Group Type=\"{getGroupType()}\"*'    AND " +
                         $"       s.Script = '{_name}'  ";
            // run query into XDocument to proceed with LinQ
            string xml = _model.SqlQueryNative(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get scriptID
            var scriptIDNode = x.Descendants("ScriptID").FirstOrDefault();
            if (scriptIDNode == null) return;
            _id = (int)scriptIDNode;

            // get scriptName
            var scriptNameNode = x.Descendants("ScriptName").FirstOrDefault();
            if (scriptNameNode == null) return;
            _guid = scriptNameNode.Value;
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
                    $" Values ('{SCRIPT_GROUP_CATEGORY}','{GUID}','<Group Type=\"{getGroupType()}\" Notes=\"\"/>','{_name}')";
            _model.ExecuteSql(sql);
            getInfo();
            return true;
        }
        string getGroupType()
        {
            return _groupType.ToString();
        }
        #endregion
    }
}
