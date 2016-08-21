/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace EAAddinFramework.Utils
{
    /// <summary>
    /// Description of VBScriptLanuguage.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class VBScriptLanguage:ScriptLanguage
	{
        protected override string FunctionStart => "function ";
        protected override string ParameterListStart => "(";
        protected override string ParameterSeparator => ", ";
        protected override string ParameterListEnd => ")";
        protected override string BodyStart => string.Empty;
        protected override string BodyEnd => string.Empty;
        protected override string FunctionEnd => "end function";
        protected override string CommentLine => "'";

        private string SubStart => "sub ";
        private string SubEnd => "end sub";


        public override string Name => "VBScript";

    }
}
