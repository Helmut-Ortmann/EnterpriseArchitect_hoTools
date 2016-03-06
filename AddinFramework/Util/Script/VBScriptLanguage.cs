/*
 * Created by SharpDevelop.
 * User: wij
 * Date: 22/11/2014
 * Time: 7:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace EAAddinFramework.Utils
{
    /// <summary>
    /// Description of VBScriptLanuguage.
    /// </summary>
    public class VBScriptLanguage:ScriptLanguage
	{
		public VBScriptLanguage()
		{
		}
        protected override string functionStart => "function ";
        protected override string parameterListStart => "(";
        protected override string parameterSeparator => ", ";
        protected override string parameterListEnd => ")";
        protected override string bodyStart => string.Empty;
        protected override string bodyEnd => string.Empty;
        protected override string functionEnd => "end function";
        protected override string commentLine => "'";

        string subStart => "sub ";
        string subEnd => "end sub";


        public override string name => "VBScript";

    }
}
