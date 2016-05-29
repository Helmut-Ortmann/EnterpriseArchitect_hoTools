using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using System.Linq;


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
        string _name;
        int _scriptId;
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

        void init(Model model, string name, EaScriptGroupType groupType, string note)
        {
            _model = model;
            _name = name;
            _groupType = groupType;
            _note = note;
        }
        public bool exists()
        {
            if (_scriptId > 0) return true;
            string sql = "select s.[ScriptID] " +
                         " from t_script s " +
                         " where s.Notes like '<Group Type=\"{getGroupType()}\"*'    AND "   +
                         "       s.Script == '{_name}' ";
            // run query into XDocument to proceed with LinQ
            var x = new XDocument(_model.SQLQuery(sql));
            // get script
            string scriptId = x.Elements("ScriptID").FirstOrDefault().Value;

            return false;
        }
        public bool save()
        {
            return false;
        }
        string getGroupType()
        {
            return _groupType.ToString();
        }
        #endregion
    }
}
