using System.Linq;
using System.Xml.Linq;



namespace EAAddinFramework.Utils
{
    public class EaScriptGroup
    {
        public enum EaScriptGroupType
        {
            Normal,
            Projtbrowser,
            Contextelement,
            Modelsearch
        }
        // Script Category of a Script
        const string ScriptGroupCategory = "3955A83E-9E54-4810-8053-FACC68CD4782";

        public string Name => _name;
        public int Id => _id;
        public string Guid => _guid;

        #region Constructor
        string _name;  // Group Name
        string _guid; // GUID of Script or Group
        int _id;      // Script ID
        Model _model;
        EaScriptGroupType _groupType;
        public EaScriptGroup(Model model, string name, EaScriptGroupType groupType)
        {
            Init(model, name, groupType);
        }


        /// <summary>
        /// Initialize ScriptGroup
        /// </summary>
        /// <param name="model"></param>
        /// <param name="groupName"></param>
        /// <param name="groupType"></param>
        private void Init(Model model, string groupName, EaScriptGroupType groupType)
        {
            _model = model;
            _name = groupName;
            _groupType = groupType;
        }

        public bool Exists()
        {
            GetInfo();
            return _id > 0;

            
        }

        void GetInfo()
        {
            string sql = "select s.[ScriptID], s.[ScriptName] " +
                         " from t_script s " +
                         $" where s.Notes like '<Group Type=\"{GetGroupType()}\"*'    AND " +
                         $"       s.Script = '{_name}'  ";
            // run query into XDocument to proceed with LinQ
            string xml = _model.SqlQueryNative(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get scriptID
            var scriptIdNode = x.Descendants("ScriptID").FirstOrDefault();
            if (scriptIdNode == null) return;
            _id = (int)scriptIdNode;

            // get scriptName
            var scriptNameNode = x.Descendants("ScriptName").FirstOrDefault();
            if (scriptNameNode == null) return;
            _guid = scriptNameNode.Value;
        }

        /// <summary>
        /// Save EaScriptGroup.
        /// </summary>
        /// <returns>true = successful</returns>
        public void Save()
        {
            var guid = "{" + System.Guid.NewGuid() + "}";
            string sql = "insert into t_script (ScriptCategory, ScriptName,Notes, Script) " +
                    $" Values ('{ScriptGroupCategory}','{guid}','<Group Type=\"{GetGroupType()}\" Notes=\"\"/>','{_name}')";
            _model.ExecuteSql(sql);
            GetInfo();
        }
        string GetGroupType()
        {
            return _groupType.ToString();
        }
        #endregion
    }
}
