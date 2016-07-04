/*
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;

namespace EAAddinFramework.Utils
{
	/// <summary>
	/// Description of ScriptLanguage.
	/// </summary>
	public abstract class ScriptLanguage
	{

		public ScriptFunction AddFunction(Script script, MethodInfo operation)
		{
			//get the script code
			//get the function code
			string functionCode = Translate(operation);
			//add function code to script code
			script.AddCode(functionCode);
			//return the function
			return script.Functions.Find(x => x.Name == operation.Name);
		}
		public abstract string Name{get;}
		protected abstract string FunctionStart {get;}
		protected abstract string ParameterListStart {get;}
		protected abstract string ParameterSeparator {get;}
		protected abstract string ParameterListEnd{get;}
		protected abstract string BodyStart {get;}
		protected abstract string BodyEnd {get;}
		protected abstract string FunctionEnd {get;}
		protected abstract string CommentLine {get;}

		public string Translate(MethodInfo operation)
		{
			//start with e new line
			string code = Environment.NewLine;
			//keyword				
			code += FunctionStart;
			//name of the method
			code += operation.Name;
			//open parenthesis
			code += ParameterListStart;
			//parameters
			bool firstParameter = true;
			foreach (ParameterInfo parameter in operation.GetParameters()) 
			{
				//don't add the repository parameter as it is added directly to the scriptcontroller
				if (parameter.Name.ToLower() != "repository")
				{
					if (firstParameter)
					{
						firstParameter = false;
					}
					else
					{
						//add a comma and space starting from the second parameter
						code += ParameterSeparator;
					}
					//parameter name
					code += parameter.Name;
					
				}
			}
			//close parenthesis
			code += ParameterListEnd;
			//add newline
			code += Environment.NewLine;
			//begin of body
			code += BodyStart;
			//add newline if there was a bodyStart
			if (!string.IsNullOrEmpty(BodyStart))
			{
				code += Environment.NewLine;							
			}
			//add tab + comment
			code += "\t "+CommentLine+"Add code here";
			//add newline
			code += Environment.NewLine;
			//add end of body 
			code += BodyEnd;
			//add newline if there was a body end
			if (!string.IsNullOrEmpty(BodyEnd))
			{
				code += Environment.NewLine;
			}
			//add end keyword
			code += FunctionEnd;
			return code;							
		}

	}
}
