using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace hoTools.Utils.Extensions
{
    public class ExtensionItem
    {
        private List<string> _publicMethods;


        public ExtensionItem(string fileName, string type, string description)
        {
            FileName = fileName;
            Type = type;
            Description = description;
        }

        public string Name => Path.GetFileName(FileName);

        public string FileName { get;  }

        public string Description { get;  }

        public string HelpText { get; set; }

        public string Type { get;}

        public string AssemblyVersion { get; set; }

        public string FileVersion { get; set; }

        public string Signature { get; set; }

        public List<string> PublicStaticMethods { get; private set; }

        public void AnalyzeAssembly()
        {
            Assembly ass = Assembly.LoadFrom(FileName);
            foreach (Type t in ass.GetTypes())
            {
                string name = t.Name;
                string fullyQualifiedName = t.FullName;
            }

            // get all public methods
            PublicStaticMethods  = (from type in ass.GetTypes()
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
            string staticMethods = string.Join($@"{Environment.NewLine}", PublicStaticMethods.ToArray());
            string publicMethods = string.Join($@"{Environment.NewLine}", _publicMethods.ToArray());
            string info = $@"File:    {FileName}
Static Methods:
{staticMethods}

Instance Methods:
{publicMethods}

";
            return info;

        }
    }
}
