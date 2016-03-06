/*
 * Created by SharpDevelop.
 * User: Geert
 * Date: 7/10/2014
 * Time: 19:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MSScriptControl;


namespace EAAddinFramework.Utils
{
	/// <summary>
	/// Description of ScriptFunction.
    /// A function in a Script
	/// </summary>
	public class ScriptFunction
	{
		private Script owner {get;set;}
        public string name => procedure.Name;
        public string fullName => this.owner.name + "." + this.procedure.Name;
        public int numberOfParameters => procedure.NumArgs;
        Procedure procedure { get; set; }

        /// <summary>
        /// Constructor ScriptFunction
        /// </summary>
        /// <param name="owner">Script</param>
        /// <param name="procedure">Funtion</param>
        public ScriptFunction(Script owner, Procedure procedure)
		{
			this.owner = owner;
			this.procedure = procedure;
		}
		/// <summary>
		/// execute this function
		/// </summary>
		/// <param name="parameters">the parameters needed by this function</param>
		/// <returns>whatever gets returned by the the actual script function</returns>
		public object execute(object[] parameters)
		{
			if (this.procedure.NumArgs == parameters.Length)
			{
				return this.owner.executeFunction(this.name, parameters);
			}
			else if (this.procedure.NumArgs == 0)
			{
				return this.owner.executeFunction(this.name);
			}
			else
			{
				throw new ArgumentException ("wrong number of arguments. Script has "+this.procedure.NumArgs+" argument where the call has " + parameters.Length + " parameters");
			}
		}
	}
}
