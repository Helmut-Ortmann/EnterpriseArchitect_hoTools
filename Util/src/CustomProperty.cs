using System.Text.RegularExpressions;
using System.Xml;
using EA;

// ReSharper disable once CheckNamespace
namespace hoTools.hoUtils
{
    public class CustomProperty
    {
        public Repository Rep { get; }

        public EA.Element El { get; }

        public string Guid { get; set; }

        public string Name { get; set; } = "";

        // only the type, used to construct the whole value (ValueEx)
        public string Type { get; set; } = "";

        // only the value, used to construct the whole value (ValueEx)
        public string Value { get; set; } = "";

        // the whole string with name, type  and value
        public string ValueEx { get; set; } = "";


        public CustomProperty(EA.Repository rep, EA.Element el)
        {
            Rep = rep;
            El = el;
        }

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Update(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
            return Update();
        }

        public bool Update()
        {
            // add ClassifierGUID to target action
            string updateStr = $@"
                        update t_xref 
                            set Description = '{GetValueEx()}'
                            where xrefid = '{Guid}'"; 
            Rep.Execute(updateStr);
            return true;
        }
        
        /// <summary>
        /// Get complete custom property according to name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValueEx(string name)
        {
            Name = name;
            return Get();
        }
        /// <summary>
        /// Get customerProperty of Name
        /// - Type
        /// - Kind
        /// - ValueEx
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            Guid = "";
            string query = $@"
select description As [DESCRIPTIONXX], XrefID AS [XREFIDXX] 
 from t_xref 
 where client = '{El.ElementGUID}' 
   AND Type = 'element property' 
   AND Description like '*@NAME={Name}@ENDNAME*'";

            //string query = @"select o.ea_guid AS EA_GUID
            //          from t_object o 
            //          where o.name = '" + elementName + "' AND " +
            //               "o.Object_Type = '" + elementType + "' ";

            string str = Rep.SQLQuery(query);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode descriptionNode = xmlDoc.SelectSingleNode("//DESCRIPTIONXX");
            if (descriptionNode != null)
            {
                ValueEx = descriptionNode.InnerText;

            }
            XmlNode guidNode = xmlDoc.SelectSingleNode("//XREFIDXX");
            if (guidNode != null)
            {
                Guid = guidNode.InnerText;

            }
            else
            {
                ValueEx = "";
            }

            Regex rx = new Regex(@"@NAME=([^@]*).*@TYPE=([^@]*).*@VALU=([^@]*)@");
            Match match = rx.Match(ValueEx);
            if (match.Success)
            {
                Name = match.Groups[1].Value;
                Type = match.Groups[2].Value;
                Value = match.Groups[3].Value;
            }
            else
            {
                Name = "";
                Type = "";
                Value = "";
            }
            return ValueEx;

        }
        public bool Create(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
            return Create();
        }

        public bool Create(string value)
        {
            Value = value;
            return Create();
        }
        public bool Create()
        {
            // set CallOperation
            // TYPE = ActionKind
            this.Guid = $@"{{{System.Guid.NewGuid()}}}";
            
            string insertIntoTXref = $@"
insert into t_xref 
       (XrefID,            Name,               Type,              Visibility, Namespace, Requirement, [Constraint], Behavior, Partition, Description, Client, Supplier, Link)
        VALUES('{this.Guid}', 'CustomProperties', 'element property','Public', '','','', '',0, 
           '{{{GetValueEx()}}}', '{El.ElementGUID}', null,'')";
            Rep.Execute(insertIntoTXref);

            return true;
        }
        /// <summary>
        /// Get value of the custom property
        /// </summary>
        /// <returns></returns>
        private string GetValueEx()
        {
            return GetValueEx(Name, Type, Value);
        }
        /// <summary>
        /// Get value of the custom property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetValueEx(string name, string type, string value)
        {
            return $@"@PROP=@NAME={name}@ENDNAME;@TYPE={type}@ENDTYPE;@VALU={value}@ENDVALU;@PRMT=@ENDPRMT;@ENDPROP;";
        }
    }
}
