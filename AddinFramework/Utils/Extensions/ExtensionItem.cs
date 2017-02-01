using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinFramework.Utils.Extensions
{
    public class ExtensionItem
    {
        private string _name;
        private string _longName;
        private string _signature;
        private string _description;
        private string _helpText;
        private string _type;
        private string _assemblyVersion;
        private string _fileVersion;
        private string _signiture;


        public ExtensionItem(string longName)
        {
            _longName = longName;
        }

        public string Name
        {
            get { return Path.GetFileName(_name); }

        }

        public string LongName
        {
            get { return _longName; }
            set { _longName = value; }
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
        public string Signiture
        {
            get { return _signiture; }
            set { _signiture = value; }
        }
    }
}
