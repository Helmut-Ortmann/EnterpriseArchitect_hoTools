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

namespace AddinFramework.Util.Script
{
	/// <summary>
	/// Description of ScriptFunction.
    /// A function in a Script
	/// </summary>
	public class ScriptFunction
	{
		public EAAddinFramework.Utils.Script Owner { get; }
        public string Name => Procedure.Name;
        public string FullName => Owner.Name + "." + Procedure.Name;
        public int NumberOfParameters => Procedure.NumArgs;
        Procedure Procedure { get; }
        public string Description { get; } 

        /// <summary>
        /// Constructor ScriptFunction
        /// </summary>
        /// <param name="owner">Script</param>
        /// <param name="procedure">Function</param>
        public ScriptFunction(EAAddinFramework.Utils.Script owner, Procedure procedure)
		{
			Owner = owner;
			Procedure = procedure;
		}

	    /// <summary>
	    /// Constructor ScriptFunction with description
	    /// </summary>
	    /// <param name="owner">Script</param>
	    /// <param name="procedure">Function</param>
	    /// <param name="description">Description</param>
	    public ScriptFunction(EAAddinFramework.Utils.Script owner, Procedure procedure, string description)
        {
            Owner = owner;
            Procedure = procedure;
            Description = description;
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
