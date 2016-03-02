using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace hoTools.Utils
{
    public class CallOperationAction
    {
        public static bool createCallAction(EA.Repository rep, EA.Element action, EA.Method method)
        {
            // add ClassifierGUID to target action
            string updateStr = @"update t_object set classifier_GUID = '" + method.MethodGUID +
                       "' where ea_guid = '" + action.ElementGUID + "' ";
            rep.Execute(updateStr);

            // set CallOperation
            string CallOperationProperty = "@PROP=@NAME=kind@ENDNAME;@TYPE=ActionKind@ENDTYPE;@VALU=CallOperation@ENDVALU;@PRMT=@ENDPRMT;@ENDPROP;";
            Guid g = Guid.NewGuid();
            string xrefid = "{" + g.ToString() + "}";
            string insertIntoT_xref = @"insert into t_xref 
                (XrefID,            Name,               Type,              Visibility, Namespace, Requirement, [Constraint], Behavior, Partition, Description, Client, Supplier, Link)
                VALUES('" + xrefid + "', 'CustomProperties', 'element property','Public', '','','', '',0, '" + CallOperationProperty + "', '" + action.ElementGUID + "', null,'')";
                rep.Execute(insertIntoT_xref);

            // Link Call Operation to operation
                g = Guid.NewGuid();
                xrefid = "{" + g.ToString() + "}";
                insertIntoT_xref = @"insert into t_xref 
                (XrefID,            Name,               Type,              Visibility, Namespace, Requirement, [Constraint], Behavior, Partition, Description, Client, Supplier, Link)
                VALUES('" + xrefid + "', 'MOFProps', 'element property','Public', '','','', 'target',0, '  null ', '" + method.MethodGUID + "', null,'')";
                //rep.Execute(insertIntoT_xref);
              
            return true;
        }

        public static string removeFirstParenthesisPairFromString(string s)
        {
            // delete first (
            // not for macros
            if (! s.Substring(0, 1).Equals("#"))
            {
                int i = s.IndexOf((string)@"(", 1, StringComparison.CurrentCulture);
                if (i >= 0)
                {
                    s = s.Remove(i, 1);
                    s = s.Substring(0, s.Length - 1);
                }
            }

            //s = Regex.Replace(s, @"(\()|(\))","");
            return s;
        }

        public static string addQuestionMark(string s)
        {
            if (! s.Contains("end")) {
            if (s.Substring(s.Length - 1, 1) == "?") return s;
            s = s + "?";
            }

            return s;
        }
        // Remove modul prefix from call string
        // modulePrefix_function(..) ==> function(..)
        public static string removeModuleNameFromCallString(string s)
        {
            var pattern = new Regex(@"([a-zA-Z][a-zA-Z_0-9]*_)[a-zA-Z_0-9]*\(");
            Match regMatch = pattern.Match(s);
            if (regMatch.Success | regMatch.Groups.Count > 1)
            {
                // delete 1 Group
                s = s.Replace(regMatch.Groups[1].Value, "");
            }
            return s;
        }
        public static string removeCasts(string s)
        {
            // remove casts
            if (!s.ToLower().Contains("sizeof("))
            {
                s = Regex.Replace(s, @"\([\s]*[a-zA-Z][a-zA-Z0-9_]+_t[\s]*[\*]*[\s]*[const]{0,5}[\s]*\)", ""); // delete casts
            }
            s = Regex.Replace(s, @"\([\s]*void[\s][\*]*\)", ""); // delete (void *) casts


            // delete 1223ul (type suffixe)
            //s = Regex.Replace(s, @"([0-9])([uU][lL]*)", "");
            var pattern = new Regex(@"[^a-zA-Z_]([0-9])([uU][lL]*)");
            Match regMatch = pattern.Match(s);
            while (regMatch.Success)
            {
                string old = regMatch.Groups[1].Value;
                s = s.Replace(regMatch.Groups[1].Value + regMatch.Groups[2].Value, old);
                regMatch = regMatch.NextMatch();
            }
            //-------------------------------------------------------
            // remove type suffix

            // delete 1223u/123l
            //s = Regex.Replace(s, @"([0-9])([fFuUlL])", "");
            pattern = new Regex(@"[^a-zA-Z_]([0-9])([fFuUlL])");
            regMatch = pattern.Match(s);
            while (regMatch.Success)
            {
                string old = regMatch.Groups[1].Value;
                s = s.Replace(regMatch.Groups[1].Value + regMatch.Groups[2].Value, old);
                regMatch = regMatch.NextMatch();
            }

            // delete 0x012u
            pattern = new Regex(@"[^a-zA-Z_](0x[0-9AECDF]+)([uU])");
            regMatch = pattern.Match(s);
            while (regMatch.Success)
            {
                string old = regMatch.Groups[1].Value;
                s = s.Replace(regMatch.Groups[1].Value + regMatch.Groups[2].Value, old);
                regMatch = regMatch.NextMatch();
            }
            return s;
        }
        
        public static string removeUnwantedStringsFromText(string s, bool deleteMultipleSpaces= true)
        {
            s = removeCasts(s);
            

            if (! s.Contains("while"))
            { 
                // replace double "((" by "("
                //s = Regex.Replace(s, @"\(\(", "(");
                // replace double "))" by ")"
                //s = Regex.Replace(s, @"\)\)", ")");
            }
            // delete everything behind";"
            // not for a for loop
            if (! (s.Contains("for ") || s.Contains("for(")))
                {
                    s = Regex.Replace(s, @";.*", "");
                    s = Regex.Replace(s, @"\n", "\r\n");
                }

            // delete everything behind"{"
            s = Regex.Replace(s, @"{.*", "");
            s = Regex.Replace(s, @"\n", "\r\n");

            if (deleteMultipleSpaces)
            {
                // delete multiple spaces
                s = Regex.Replace(s, @"  +", " ");
            }

            
           

            return s.Trim();
        }
        public static EA.Element getElementFromName(EA.Repository rep, string elementName, string elementType)
        {
            EA.Element el = null;
            string query = @"select o.ea_guid AS EA_GUID
                      from t_object o 
                      where o.name = '" + elementName + "' AND " +
                            "o.Object_Type = '" + elementType + "' ";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//EA_GUID");
            if (operationGUIDNode != null)
            {
                string GUID = operationGUIDNode.InnerText;
                el = rep.GetElementByGuid(GUID);
            }

            return el;
        }

        public static string getMethodNameFromCallString(string s)
        {
            var pattern = new Regex(@"[a-zA-Z_][a-zA-Z_0-9]+\s*\(");
            Match regMatch = pattern.Match(s);
            if (regMatch.Success)
            {
                // delete old string
                string s1 = regMatch.Value;
                return s1.Substring(0,s1.Length -1);
            }
            return "";
        }
        public static EA.Method getMethodFromMethodName(EA.Repository rep, string methodName)
        {
            EA.Method method = null;
            string query = @"select op.ea_guid AS EA_GUID
                      from t_operation op 
                      where op.name = '" + methodName + "' ";
            string str = rep.SQLQuery(query);
            var XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            XmlNode operationGUIDNode = XmlDoc.SelectSingleNode("//EA_GUID");
            if (operationGUIDNode != null)
            {
                string GUID = operationGUIDNode.InnerText;
                method = rep.GetMethodByGuid(GUID);
            }

            return method;
        }
    }
}
