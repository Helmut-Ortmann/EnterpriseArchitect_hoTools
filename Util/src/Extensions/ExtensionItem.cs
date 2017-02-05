using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace hoTools.Utils.Extensions
{
    public class ExtensionItem
    {
        private string _fileName;
        private string _description;
        private string _helpText;
        private string _type;
        private string _assemblyVersion;
        private string _fileVersion;
        private string _signature;
        private List<string> _publicStaticMethods;
        private List<string> _publicMethods;


        public ExtensionItem(string fileName)
        {
            _fileName = fileName;
        }

        public string Name
        {
            get { return Path.GetFileName(_fileName); }

        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string HelpText
        {
            get { return _helpText; }
            set { _helpText = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }
            set { _assemblyVersion = value; }
        }
        public string FileVersion
        {
            get { return _fileVersion; }
            set { _fileVersion = value; }
        }
        public string Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public List<string> PublicStaticMethods
        {
            get { return _publicStaticMethods; }
        }

        public void AnalyzeAssembly()
        {
            Assembly ass = Assembly.ReflectionOnlyLoadFrom(_fileName);
            foreach (Type t in ass.GetTypes())
            {
                string name = t.Name;
                string fullyQualifiedName = t.FullName;
            }

            // get all public methods
            _publicStaticMethods  = (from type in ass.GetTypes()
                         from method in type.GetMethods(
                           BindingFlags.Public |
                           BindingFlags.Static)
                         select type.FullName + ":" + method.Name).Distinct().ToList();

            _publicMethods = (from type in ass.GetTypes()
                                    from method in type.GetMethods(
                                      BindingFlags.Public |
                                      BindingFlags.Instance )
                                    select method.ReturnType + "  "+ type.FullName + ":" + method.Name).Distinct().ToList();

            var m = from type in ass.GetTypes()
                     from method in type.GetMethods(
                       BindingFlags.Public |
                       BindingFlags.Instance)
                     select new { type.FullName, method.Name, method.ReturnType, method};
            string methodNames = "";
            foreach (var e in m)
            {
                string name = $@"{e.ReturnType}     {e.FullName}:{e.Name}";
                var method = e.method;
                // collect parameters
                string parameters = "";
                string delimeter = " ";
                foreach (var par in method.GetParameters())
                {
                    parameters = parameters + $@"{delimeter}{par.Name}:{par.ParameterType}";
                    delimeter = ",";
                }
                methodNames = methodNames + $@"{name} ({parameters}){Environment.NewLine}";
            }
            _publicMethods = new List<string>();
            _publicMethods.Add(methodNames);



        }

        public string ExtensionDetails()
        {
            string staticMethods = string.Join($@"{Environment.NewLine}", _publicStaticMethods.ToArray());
            string publicMethods = string.Join($@"{Environment.NewLine}", _publicMethods.ToArray());
            string info = $@"File:    {_fileName}
Static Methods:
{staticMethods}

Instance Methods:
{publicMethods}

";
            return info;

        }
    }
}
