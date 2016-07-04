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
		public Script Owner { get; }
        public string Name => Procedure.Name;
        public string FullName => Owner.Name + "." + Procedure.Name;
        public int NumberOfParameters => Procedure.NumArgs;
        Procedure Procedure { get; }

        /// <summary>
        /// Constructor ScriptFunction
        /// </summary>
        /// <param name="owner">Script</param>
        /// <param name="procedure">Function</param>
        public ScriptFunction(Script owner, Procedure procedure)
		{
			Owner = owner;
			Procedure = procedure;
		}
		/// <summary>
		/// execute this function
		/// </summary>
		/// <param name="parameters">the parameters needed by this function</param>
		/// <returns>whatever gets returned by the actual script function</returns>
		public object Execute(object[] parameters)
		{
			if (Procedure.NumArgs == parameters.Length)
			{
				return Owner.ExecuteFunction(Name, parameters);
			}
			else if (Procedure.NumArgs == 0)
			{
				return Owner.ExecuteFunction(Name);
			}
			else
			{
				throw new ArgumentException ("wrong number of arguments. Script has "+Procedure.NumArgs+" argument where the call has " + parameters.Length + " parameters");
			}
		}
	}
}
