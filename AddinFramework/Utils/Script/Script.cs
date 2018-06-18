/*
 * Created by SharpDevelop.
 * User: Geert
 * Date: 7/10/2014
 * Time: 19:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using AddinFramework.Util.Script;
using Microsoft.Win32;
using MSScriptControl;
using hoTools.Utils;
//using EAWrappers = TSF.UmlToolingFramework.Wrappers.EA;


// ReSharper disable once CheckNamespace
namespace EAAddinFramework.Utils
{
    /// <summary>
    /// Description of Script.
    /// </summary>
    public class Script
	{
		static readonly string _scriptLanguageIndicator = "Language=\"";
		static readonly string _scriptNameIndicator = "Script Name=\"";
        static int _scriptHash;
        private static List<Script> _allScripts = new List<Script>();
        static Dictionary<string, string> _includableScripts;
        private static Dictionary<string,string> _staticIncludableScripts ;
        static readonly List<Script> StaticEaMaticScripts = new List<Script>();
        private static Dictionary<string,string> _modelIncludableScripts = new Dictionary<string, string>();
        static bool _reloadModelIncludableScripts;
        private Model _model;
        readonly string _scriptId;
        public string Code { get; private set; }
		public string ErrorMessage {get;set;}
        ScriptControl _scriptController;
        public List<ScriptFunction> Functions {get;set;}
        ScriptLanguage _language;
        public string Name{get;set;}
		public string GroupName {get;set;}
        public string LanguageName => _scriptController.Language;
        public string DisplayName => Name + " - " + _scriptController.Language;
        public bool IsStatic { get; }
        static Script()
		{
			LoadStaticIncludableScripts();
		}
		/// <summary>
		/// A dictionary with all the includable script.
		/// The key is the complete !INC statement, the value is the code
		/// </summary>
		static Dictionary<string,string> IncludableScripts 
		{
			get
			{
				if (_staticIncludableScripts == null)
				{
					LoadStaticIncludableScripts();
				}
				//if _includableScript is null then it has been made empty because the model scripts changed
				if (_reloadModelIncludableScripts)
				{
					//start with the static includeable scripts
					_includableScripts = new Dictionary<string, string>(_staticIncludableScripts);
					//then add the model scripts
					foreach (KeyValuePair<string, string> script in _modelIncludableScripts) 
					{
						_includableScripts.Add(script.Key, script.Value);
					}
					//turn off flag to reload scripts
					_reloadModelIncludableScripts = false;					
				}
				return _includableScripts;					
			}
			set
			{
				_includableScripts = value;
			}

		}
		/// <summary>
		/// creates a new script
		/// </summary>
		/// <param name="scriptId">the id of the script</param>
		/// <param name="scriptName">the name of the script</param>
		/// <param name="groupName">the name of the script group</param>
		/// <param name="code">the code</param>
		/// <param name="language">the language the code is written in</param>
		/// <param name="model">the model this script resides in</param>
		public Script(string scriptId,string scriptName,string groupName, string code, string language, Model model):this(scriptId, scriptName, groupName, code, language, false)
		{
			_model = model;
		}

        /// <summary>
        /// creates a new script
        /// </summary>
        /// <param name="scriptId">the id of the script</param>
        /// <param name="scriptName">the name of the script</param>
        /// <param name="groupName">the name of the scriptgroup</param>
        /// <param name="code">the code</param>
        /// <param name="language">the language the code is written in</param>
        /// <param name="isStatic"></param>
        public Script(string scriptId,string scriptName,string groupName, string code, string language, bool isStatic)
		{
			_scriptId = scriptId;
			Name = scriptName;
			GroupName = groupName;
			Functions = new List<ScriptFunction>();
			Code = code;
			SetLanguage(language);
			IsStatic = isStatic;
		}
        /// <summary>
        /// reload the code into the controller to refresh the functions
        /// </summary>
        private void ReloadCode()
        {
            //remove all functions
            Functions.Clear();
            //create new script controller
            _scriptController = new ScriptControl {Language = _language.Name};
            // Objects available in Script
            _scriptController.AddObject("Repository", _model.Repository);
            _scriptController.AddObject("EAModel", _model);
            //Add the actual code. This must be done in a try/catch because a syntax error in the script will result in an exception from AddCode
            try
            {
                //first add the included code
                string includedCode = IncludeScripts(Code);

                //then add the included code to the script controller
                _scriptController.AddCode(includedCode);
                // set the functions
                foreach (Procedure procedure in _scriptController.Procedures)
                {
                    string description = GetDescription(includedCode, procedure);
                    Functions.Add(new ScriptFunction(this, procedure, description));
                }
            }
            catch (Exception e)
            {
                //the addCode didn't work, probably because of a syntax error, or unsupported syntax in the code
                var iscriptControl = _scriptController as IScriptControl;
                ErrorMessage = e.Message + " ERROR : " + iscriptControl.Error.Description + " | Line of error: " + iscriptControl.Error.Line + " | Code error: " + iscriptControl.Error.Text;
                // use only the error message in the Script object
                //MessageBox.Show("Error in loading code for script " + this.name + ": " + this.errorMessage, "Syntax error in Script");
            }
        }
        /// <summary>
        /// Get Description for Procedure. The function is identified by (independent of language):
        /// <para/>function name
        /// <para/>Sub name
        /// </summary>
        /// <param name="includeCode"></param>
        /// <param name="procedure"></param>
        /// <returns></returns>
	    private string GetDescription(string includeCode, Procedure procedure)
	    {
	        string comment = _language.CommentLine;
            string pattern = $"(([ \t]*{comment} \\S[^\n]*\n)+)+" +  // Comment line, 1 Blank + Description of Function / procedure
                    $"(([ \t]*\r\n)*|([ \t]*{comment}[\\S][^\n]*\n)*)*" + // Ignore empty lines, Comment lines without 1 Blank
                    $"[ \t]*((public[ \t]*)*(default[ \t]*)*(private[ \t]*)*function|sub)\\s+{procedure.Name}";

            Match m = Regex.Match(includeCode, pattern, RegexOptions.Multiline);
	        if (m.Success)
	        {
                // remove line feeds
                Char[] lineFeed = { '\r', '\n' };
                string s = m.Groups[1].Value.Trim(lineFeed);
                // remove Comment at start
	            pattern = $"^[ \t]*{comment} ";
	            s = Regex.Replace(s, pattern,"", RegexOptions.Multiline);
	            return s;

	        }

	        return "";
	    }
        /// <summary>
        /// loads all static includable scripts. 
        /// These scripts are stored outside the model and can not be changed by the user
        /// </summary>
        static void LoadStaticIncludableScripts()
		{
			_staticIncludableScripts = new Dictionary<string, string>();
			//local scripts
			LoadLocalScripts();
			//MDG scripts in the program folder
			LoadLocalMdgScripts();
			// MDG scripts in other locations
			LoadOtherMdgScripts();
			//store the staticIncludeable scripts in a separate dictionary
			_includableScripts = new Dictionary<string, string>(_staticIncludableScripts);

		}

        /// <summary>
        /// The local scripts are located in the "ea program files"\scripts (so usually C:\Program Files (x86)\Sparx Systems\EA\Scripts or C:\Program Files\Sparx Systems\EA\Scripts)
        /// The contents of the local scripts is loaded into the includableScripts
        /// </summary>
        static void LoadLocalScripts()
        {
            string scriptsDirectory = Path.GetDirectoryName(Model.ApplicationFullPath) + "\\Scripts";
            string[] scriptFiles = Directory.GetFiles(scriptsDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string scriptfile in scriptFiles)
            {
                string scriptcode = Util.ReadAllText(scriptfile);
                string scriptName = Path.GetFileNameWithoutExtension(scriptfile);
                string scriptLanguage = GetLanguageFromPath(scriptfile);
                _staticIncludableScripts.Add("!INC Local Scripts." + scriptName, scriptcode);
                //also check if the script needs to be loaded as static EA-Matic script
                LoadStaticEaMaticScript(scriptName, "Local Scripts", scriptcode, scriptLanguage);
            }
        }
        private static string GetLanguageFromPath (string path)
		{
			string extension = Path.GetExtension(path);
			string foundLanguage = "VBScript";
			if (extension.Equals(@"vbs",StringComparison.InvariantCultureIgnoreCase))
		    {
		    	foundLanguage =  "VBScript";
			} 
			else if (extension.Equals(@"js",StringComparison.InvariantCultureIgnoreCase))
			{
				if (path.Contains(@"Jscript"))
				{
					foundLanguage =  @"Jscript";
				}
				else
				{
					foundLanguage =  "JavaScript";
				}
			}
			return foundLanguage;
		}

        /// <summary>
        /// loads the MDG scripts from the locations added from MDG Technologies|Advanced. 
        /// these locations are stored as a comma separated string in the registry
        /// a location can either be a directory, or an URL
        /// </summary>
        static void LoadOtherMdgScripts()
        {
            //read the registry key to find the locations
            var pathList = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Sparx Systems\EA400\EA\OPTIONS", "MDGTechnology PathList", null) as string;
            if (pathList != null)
            {
                string[] mdgPaths = pathList.Split(',');
                foreach (string mdgPath in mdgPaths)
                {
                    if (mdgPath.Trim() == "") continue;
                    //figure out it we have a folder path or an URL
                    if (mdgPath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //URL
                        LoadMdgScriptsFromUrl(mdgPath);
                    }
                    else
                    {
                        //directory
                        LoadMdgScriptsFromFolder(mdgPath);
                    }
                }
            }

        }
        /// <summary>
        /// load the MDG scripts from the MDG file located at the given URL
        /// </summary>
        /// <param name="url">the URL pointing to the MDG file</param>
        static void LoadMdgScriptsFromUrl(string url)
		{
			try
			{
				LoadMdgScripts(new WebClient().DownloadString(url));
			}
			catch (Exception e)
			{
                MessageBox.Show($@"URL='{url}' skipped (see: Extensions, MDGTechnology,Advanced).{Environment.NewLine}{e.Message}", 
                    @"Error in load *.xml MDGScripts from url! ");
            }
		}
		/// <summary>
		/// get the MDG files in the local MDGtechnologies folder.
        /// In EA .\EA\MDGTechnologies\...
		/// </summary>
		static void LoadLocalMdgScripts()
		{
			string mdgDirectory = Path.GetDirectoryName(Model.ApplicationFullPath) + "\\MDGTechnologies";
			LoadMdgScriptsFromFolder(mdgDirectory);
		}
		/// <summary>
		/// load the scripts from the MDG files in the given directory
		/// </summary>
		/// <param name="folderPath">the path to the directory</param>
		static void LoadMdgScriptsFromFolder(string folderPath)
		{
		    if (! Directory.Exists(folderPath)) { 
		        MessageBox.Show($@"Folder '{folderPath}' doesn't exists, skipped loading MdgScripts from folder", @"Can't read from folder!");
		        return;
		    }
		    string[] mdgFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.TopDirectoryOnly);
            try
			{
				
				foreach(string mdgFile in mdgFiles)
				{
					LoadMdgScripts(Util.ReadAllText(mdgFile));
				}
			}
			catch (Exception e)
			{
                MessageBox.Show($@"Folder '{folderPath}' skipped{Environment.NewLine}{e.Message}",
                    @"Error in load *.xml MDGScripts from file! " );
                
			}
		}
        /// <summary>
        /// loads the scripts described in the MDG file into the includable scripts
        /// </summary>
        /// <param name="mdgXmlContent">the string content of the MDG file</param>
        private static void LoadMdgScripts(string mdgXmlContent)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(mdgXmlContent);
				//first get the name of the MDG
				var documentationElement = xmlDoc.SelectSingleNode("//Documentation") as XmlElement;
				if (documentationElement != null)
				{
					string mdgName = documentationElement.GetAttribute("id");
					//then get the scripts
					XmlNodeList scriptNodes = xmlDoc.SelectNodes("//Script");
					foreach (XmlNode scriptNode in scriptNodes) 
					{
						var scriptElement = (XmlElement)scriptNode;
						//get the name of the script
						string scriptName = scriptElement.GetAttribute("name");
						//get the language of the script
						string scriptLanguage = scriptElement.GetAttribute("language");
						//get the script itself
						XmlNode contentNode = scriptElement.SelectSingleNode("Content");
						if (contentNode != null)
						{
							//the script itself is base64 encoded in the content tag
							string scriptcontent = System.Text.Encoding.Unicode.GetString( Convert.FromBase64String(contentNode.InnerText));
                            string key = "!INC " + mdgName + "." + scriptName;
                            // key exists
                            if (! _staticIncludableScripts.ContainsKey(key))
                            {
                                _staticIncludableScripts.Add(key, scriptcontent);
                                //also check if the script needs to be loaded as static EA-Matic script
                                LoadStaticEaMaticScript(scriptName, mdgName, scriptcontent, scriptLanguage);
                            }
						
						}
					}
				}
			}
			catch (Exception e)
			{
                MessageBox.Show(@"", @"Error in loadMDGScripts: " + e.Message );
			}
		}
        static void LoadStaticEaMaticScript(string scriptName, string groupName, string scriptCode, string language)
        {
            if (scriptCode.Contains("EA-Matic"))
            {
                var script = new Script(groupName + "." + scriptName, scriptName, groupName, scriptCode, language, true);
                StaticEaMaticScripts.Add(script);
            }
        }

        /// <summary>
        /// replaces the !INC  statements with the actual code of the local script.
        /// The local scripts are located in the "ea program files"\scripts (so usually C:\Program Files (x86)\Sparx Systems\EA\Scripts or C:\Program Files\Sparx Systems\EA\Scripts)
        /// 
        /// </summary>
        /// <param name="code">the code containing the include parameters</param>
        /// <param name="parentIncludeStatement"></param>
        /// <returns>the code including the included code</returns>
        private string IncludeScripts(string code,string parentIncludeStatement = null)
		{
			string includedCode = code;
			//find all lines starting with !INC
			foreach (string includeString in GetIncludes(code)) 
			{
				if (includeString != parentIncludeStatement) //prevent eternal loop
				{
					//then replace with the contents of the included script
					includedCode = includedCode.Replace(includeString,IncludeScripts(GetIncludedcode(includeString),includeString));
				}
			}
			
			return includedCode;
			
		}
		/// <summary>
		/// gets the code to be included based on the include string. !INC statements
		/// </summary>
		/// <param name="includeString">the include statement</param>
		/// <returns>the code to be included</returns>
		private string GetIncludedcode(string includeString)
		{
			string includedCode = string.Empty;
			if (IncludableScripts.ContainsKey(includeString))
			{
				includedCode = IncludableScripts[includeString];
			}
			return includedCode;
		}
        /// <summary>
        /// finds each instance of "!INC" and returns the whole line
        /// </summary>
        /// <param name="code">the code to search</param>
        /// <returns>the contents of each line starting with "!INC"</returns>
        List<string> GetIncludes(string code)
        {
            var includes = new List<string>();
            using (StringReader reader = new StringReader(code))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("!INC", StringComparison.Ordinal))
                    {
                        includes.Add(line);
                    }
                }
            }
            return includes;
        }

        private void SetLanguage(string language)
		{
			switch (language) {
				case "VBScript":
					_language = new VBScriptLanguage();
					break;
				case "JScript":
					_language = new JScriptLanguage();
					break;
				case "JavaScript":
					_language = new JavaScriptLanguage();
					break;
			}
		}
		/// <summary>
		/// Gets all suitable Scripts defined in the model.Suitable Scripts have:
		/// <para /> 2 or three parameters
		/// <para /> Tag: EA-Matic 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static List<Script> GetEaMaticScripts(Model model)
		{			
			if (model != null)
			{
			 XmlDocument xmlScripts = model.SqlQuery(@"select s.ScriptID, s.Notes, s.Script,ps.Script as SCRIPTGROUP, ps.Notes as GROUPNOTES from t_script s
													   inner join t_script ps on s.ScriptAuthor = ps.ScriptName
													   where s.Script like '%EA-Matic%'");
			 //check the hash before continuing
			 int newHash = xmlScripts.InnerXml.GetHashCode();
			 //only create the scripts of the hash is different
			 //otherwise we returned the cached scripts
			 if (newHash != _scriptHash)
			 {
			  //set the new hash code
			  _scriptHash = newHash;
			  //reset scripts
		 	  _allScripts = new List<Script>();
		 	  
		 	  //set flag to reload scripts in includableScripts
		 	  _reloadModelIncludableScripts = true;
		 	  _modelIncludableScripts = new Dictionary<string, string>();
		 	  
		 	  XmlNodeList scriptNodes = xmlScripts.SelectNodes("//Row");
              foreach (XmlNode scriptNode in scriptNodes)
              {
              	//get the <notes> node. If it contains "Group Type=" then it is a group. Else we need to find "Language=" 
              	XmlNode notesNode = scriptNode.SelectSingleNode(model.FormatXPath("Notes"));
              	if (notesNode.InnerText.Contains(_scriptLanguageIndicator))
          	    {
          	    	//we have an actual script.
          	    	//the name of the script
          	    	string scriptName = GetValueByName(notesNode.InnerText, _scriptNameIndicator);
					//now figure out the language
					string language = GetValueByName(notesNode.InnerText, _scriptLanguageIndicator);
					//get the ID
					XmlNode idNode = scriptNode.SelectSingleNode(model.FormatXPath("ScriptID"));
					string scriptId = idNode.InnerText;
					//get the group
					XmlNode groupNode = scriptNode.SelectSingleNode(model.FormatXPath("SCRIPTGROUP"));
					string groupName = groupNode.InnerText;
					//then get the code
					XmlNode codeNode = scriptNode.SelectSingleNode(model.FormatXPath("Script"));	
					if (codeNode != null && language != string.Empty)
					{
						//if the script is still empty EA returns NULL
						string scriptCode = codeNode.InnerText;
						if (scriptCode.Equals("NULL",StringComparison.InvariantCultureIgnoreCase))
						{
							scriptCode = string.Empty;
						}
						var script = new Script(scriptId,scriptName,groupName,scriptCode, language,model); 
						//and create the script if both code and language are found
						_allScripts.Add(script);
						//also add the script to the include dictionary
						_modelIncludableScripts.Add("!INC "+ script.GroupName + "." + script.Name,script.Code);
					}
          	    }
              }
              //Add the static EA-Matic scripts to allScripts
              foreach (Script staticScript in StaticEaMaticScripts)
              {
              	//add the model to the static script first
              	staticScript._model = model;
              	//then add the static scrip to all scripts
              	_allScripts.Add(staticScript);
			  }              
			  //load the code of the scripts (because a script can include another script we can only load the code after all scripts have been created)
			  foreach (Script script in _allScripts) 
			  {
			  	script.ReloadCode();
			  }			  
			 }
			}
			return _allScripts;
		}
        /// <summary>
        /// gets the value from the content of the notes.
        /// The value can be found after "name="
        /// </summary>
        /// <param name="notesContent">the contents of the notes node</param>
        /// <param name="name">the name of the tag</param>
        /// <returns>the value string</returns>
        static string GetValueByName(string notesContent, string name)
        {
            string returnValue = string.Empty;
            if (notesContent.Contains(name))
            {
                int startName = notesContent.IndexOf(name, StringComparison.Ordinal) + name.Length;
                int endName = notesContent.IndexOf("\"", startName, StringComparison.Ordinal);
                if (endName > startName)
                {
                    returnValue = notesContent.Substring(startName, endName - startName);
                }

            }
            return returnValue;

        }

        /// <summary>
        /// executes the function with the given name
        /// </summary>
        /// <param name="functionName">name of the function to execute</param>
        /// <param name="parameters">the parameters needed by this function</param>
        /// <returns>whatever (if anything) the function returns</returns>
        internal object ExecuteFunction(string functionName, object[] parameters)
        {
            //_scriptController.AddObject("rep", _model.Repository);
            //_scriptController.AddObject("EAContextObject",parameters[0]);
            //_scriptController.AddObject("EAObjectType", parameters[1]);
            return _scriptController.Run(functionName, parameters);
        }
        
        /// <summary>
        /// executes the function with the given name
        /// </summary>
        /// <param name="functionName">name of the function to execute</param>
        /// <returns>whatever (if anything) the function returns</returns>
        internal object ExecuteFunction(string functionName) 
            => _scriptController.Run(functionName, new object[0]);
        /// <summary>
        /// add a function with based on the given operation
        /// </summary>
        /// <param name="operation">the operation to base this function on</param>
        /// <returns>the new function</returns>
        public ScriptFunction AddFunction(MethodInfo operation)
		{
			//translate the method info into code
			string functionCode = _language.Translate(operation);
			//add the code to the script
			AddCode(functionCode);
			//reload the script code
			ReloadCode();
			//return the new function
			return Functions.Find(x => x.Name == operation.Name);
		}
		
		/// <summary>
		/// adds the given code to the end of the script
		/// </summary>
		/// <param name="functionCode">the code to be added</param>
		public void AddCode(string functionCode)
		{
			Code += functionCode;
			string sqlUpdate = "update t_script set script = '"+ _model.EscapeSqlString(Code) +"' where ScriptID = " + _scriptId ;
			_model.ExecuteSql(sqlUpdate);
		}

	}
}
