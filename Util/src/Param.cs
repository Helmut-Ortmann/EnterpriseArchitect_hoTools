using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EA;
using System.Xml;

namespace hoTools.Utils.Parameter
{
    //---------------------------------------------------------------------------------------
    // Param Allows to set properties for Activity Parameter
    //---------------------------------------------------------------------------------------
    // update properties for returnvalue
    //            Param par = new Param(rep, parTrgt);
    //            par.setParameterProperties("direction", "out");
    //            par.save();
    //            par = null;
    public class Param
    {
        EA.Repository _rep = null;
        EA.Element _parTrgt = null; // parameter
        string _xrefid = "";        // GUID of t_xref
        string _properties = "";    // the properties
        public Param(EA.Repository rep, EA.Element parTrgt) {
            _rep = rep;
            _parTrgt = parTrgt;

            // check if t_xref element is already present
            string query = @"SELECT XrefID As XREF_ID, description As DESCR
                            FROM  t_object  o inner JOIN t_xref x on (o.ea_guid = x.client)
                            where x.Name = 'CustomProperties' AND
                                  x.Type = 'element property' AND
                                  o.object_ID = " + _parTrgt.ElementID.ToString() ;
                        
                            
            string str = _rep.SQLQuery(query);
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(str);

            // get existing t_xref and remember GUID/XrefID
            XmlNode xrefGUID = XmlDoc.SelectSingleNode("//XREF_ID");
            if (xrefGUID != null)
            {
                _xrefid = xrefGUID.InnerText;// GUID of xref

                // get description
                XmlNode xrefDESC = XmlDoc.SelectSingleNode("//DESCR");
                string _properties = null;
                if (xrefDESC != null) _properties = xrefDESC.InnerText;
            }
        }
            //---------------------------------------------------------------------
            // setParameterProperties
            //---------------------------------------------------------------------
            // It's possible to call this function several times. It the accumulates the different properties
            //
            public bool setParameterProperties(string propertyName, string propertyValue) {
                if (propertyName == "direction")
                {
                    Regex rx = new Regex(@"@PROP=@NAME=direction@ENDNAME;@TYPE=ParameterDirectionKind@ENDTYPE;@VALU=[^@]+@ENDVALU;@PRMT=@ENDPRMT;@ENDPROP;");
                    Match regMatch = rx.Match(_properties);
                    while (regMatch.Success)
                    {
                        // delete old string
                        _properties = _properties.Replace(regMatch.Value, "");
                        regMatch = regMatch.NextMatch();
                        // add new string
                    }
                    _properties = _properties + "@PROP=@NAME=direction@ENDNAME;@TYPE=ParameterDirectionKind@ENDTYPE;@VALU=" + propertyValue + "@ENDVALU;@PRMT=@ENDPRMT;@ENDPROP;";
                }
                if (propertyName == "constant")
                {
                    Regex rx = new Regex(@"@PROP=@NAME=isStream@ENDNAME;@TYPE=Boolean@ENDTYPE;@VALU=[^@]+@ENDVALU;@PRMT=@ENDPRMT;@ENDPROP;");
                    Match regMatch = rx.Match(_properties);
                    while (regMatch.Success)
                    {
                        // delete old string
                        _properties = _properties.Replace(regMatch.Value, "");
                        regMatch = regMatch.NextMatch();
                        // add new string
                    }
                    _properties = _properties + "@PROP=@NAME=isStream@ENDNAME;@TYPE=Boolean@ENDTYPE;@VALU=" + propertyValue + "@ENDVALU;@PRMT=@ENDPRMT;@ENDPROP;";
                }
        
                return true;
            }
            //---------------------------------------------------------------------
            // Save() Save ParameterProperties to t_xref
            //---------------------------------------------------------------------
            //
            public bool save()
            {
                // create new entry in t_xref
                if (_xrefid == "")
                {
                    Guid g = Guid.NewGuid();
                    _xrefid = g.ToString();
                    string insertIntoT_xref = @"insert into t_xref 
                (XrefID,            Name,               Type,              Visibility, Namespace, Requirement, [Constraint], Behavior, Partition, Description, Client, Supplier, Link)
                VALUES('" + g + "', 'CustomProperties', 'element property','Public', '','','', '',0, '" + _properties + "', '" + _parTrgt.ElementGUID + "', null,'')";
                    _rep.Execute(insertIntoT_xref);
                }
                // update propertyValue
                string update = @"update t_xref set description = '" + _properties +
                                "' where XrefID = '" + _xrefid + "'";
                _rep.Execute(update);

                return true;
            }
    }
}
