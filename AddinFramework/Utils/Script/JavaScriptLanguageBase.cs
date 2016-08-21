/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace EAAddinFramework.Utils
{
	/// <summary>
	/// Description of JavaScriptLanguageBase.
	/// </summary>
	public abstract class JavaScriptLanguageBase : ScriptLanguage
	{
        protected override string FunctionStart => "function ";
        protected override string ParameterListStart => "(";
        protected override string ParameterSeparator => ", ";
        protected override string ParameterListEnd => ")";
        protected override string BodyStart => "{";
        protected override string BodyEnd => "}";
        protected override string FunctionEnd => string.Empty;
        protected override string CommentLine => "//";

    }
}
