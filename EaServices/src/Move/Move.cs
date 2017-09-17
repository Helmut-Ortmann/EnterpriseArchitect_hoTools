using System;
using System.Windows.Forms;
using hoTools.Utils.ODBC;

namespace hoTools.EaServices.MOVE
{
    /// <summary>
    /// Move all connectors and appearances from source element to target element
    /// </summary>
    public static class Move
    {
        public static bool MoveClassifier(EA.Repository rep, EA.Diagram dia, EA.Element srcEl, EA.Element targetEl)
        {
            Odbc odbc = new Odbc(rep);
            // change each occurrence of source by target
            // - flow in t_xref
            // - connector in diagram links 
            // - diagram usage
            // - object has parent
            // - object has classifier
            // - attribute has classifier
            // - operation has classifier
            // - operation param has classifier

            int srcId = srcEl.ElementID;
            srcEl.Name = $"{srcEl.Name}_DeleteMe";
            srcEl.Update();

            int targetId = targetEl.ElementID;
            int targetPkgId = targetEl.PackageID;
            // change flow in t_xref  
            if (!Change_t_xref(odbc, srcEl.ElementGUID, targetEl.ElementGUID)) return false;
            // change connector of diagram flow
            if (!ChangeConnector(odbc, srcId, targetId)) return false;
            // change signal in diagram (source, target, generalization, dependency)
            if (!ChangeDiagramUsage(odbc, dia.DiagramID, srcId, targetId)) return false;
            // change if type is used as parent
            if (!ChangeObjectParent(odbc, srcId, targetId, targetPkgId)) return false;
            // change if type is used as classifier
            if (!ChangeObjectClassifier(odbc, srcId, targetId)) return false;
            // change type of attribute 
            if (!ChangeTypeAttribute(odbc, srcId, targetEl)) return false;
            // change type of operation 
            if (!ChangeTypeOperation(odbc, srcId, targetEl)) return false;
            // change type is operation parameter
            if (!ChangeTypeOperationParameter(odbc, srcId, targetEl)) return false;


            //Repository.GetProjectInterface().ReloadProject();
            string msg = "Delete source element '" + srcEl.Name + "'  " + srcEl.Type + "'  " + srcEl.Stereotype + " ?";
            if (MessageBox.Show(msg, "Proceed with delete?", MessageBoxButtons.YesNo)
                                   == DialogResult.No) return true;
            // update project to ensure that EA notice all changes

            if (!DeleteSource(rep, odbc, srcEl, targetEl)) return false;
            return true;
        }

        private static bool Change_t_xref(Odbc odbc, string srcGUID, string targetGUID)
        {   // Change GUID in description
            // Change GUID in client
            // for the following types of behavior
            // - 
            // search for all description fields in xref where the source GUID is used
            string sql = "SELECT x.description, x.xrefID " +
                  " FROM t_xref x" +
                  $" WHERE x.description like '{odbc.Wc}{srcGUID}{odbc.Wc}' ;";
            odbc.Rs.Open(sql, odbc.Cn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, 0);
            while (odbc.Rs.EOF == false)
            {
                // replace the source ID by the target ID
                string description = odbc.Rs.Fields[0].Value.ToString();
                description = description.Replace(srcGUID, targetGUID);
                // change source class to target of found elements
                sql = "Update t_xref" +
                    " set description = '" + description + "' " +
                    "  where xrefID = '" + odbc.Rs.Fields[1].Value.ToString() + "' " +
                    " ;";

                if (odbc.OdbcCmd(sql) == false)
                {
                    odbc.Rs.Close();
                    return false;
                }
                odbc.Rs.MoveNext();
            }
            odbc.Rs.Close();
            // replace the client ID by the target ID
            sql = "Update t_xref" +
                " set client = '" + targetGUID + "' " +
                "  where client = '" + srcGUID + "' " +
                " ;";

            if (odbc.OdbcCmd(sql) == false) return false;



            return true;
        }
        private static bool DeleteInvalidSignals(Odbc odbc)
        {
            // search for all description fields in xref where the source GUID is used
            string sql = "SELECT has.ea_GUID, x.xrefID, x.description " +
                  " FROM ho_ConHasItem has, t_connector con, t_xref x" +
                  " WHERE has.object_id = 0 AND " +
                  " has.connector_id = con.connector_id AND " +
                  " x.client = con.ea_GUID " +
                  " order by x.xrefID " +
                  ";";
            string xrefIdOld = "";
            string description = "";
            try
            { odbc.Rs.Close(); }
            catch (Exception e)
            {
            }
            odbc.Rs.Open(sql, odbc.Cn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, 0);
            while (odbc.Rs.EOF == false)
            {
                string srcGUID = odbc.Rs.Fields[0].Value.ToString().Trim();
                //check GUID is a GUID
                if (srcGUID.Length != 38 | srcGUID.Substring(0, 1) != "{")
                {
                    odbc.Rs.MoveNext();
                    continue;
                }
                // delete the source ID
                // check if new xref_id, elsewhere use old description
                if (xrefIdOld != odbc.Rs.Fields[1].Value.ToString())
                {
                    description = odbc.Rs.Fields[2].Value.ToString();
                    xrefIdOld = odbc.Rs.Fields[1].Value.ToString();
                }
                // stored procedure works only in the range to 7994 bytes
                if (description.Length <= 7990)
                {

                    description = description.Replace("," + srcGUID, "");
                    description = description.Replace(srcGUID + ",", "");
                    description = description.Replace(srcGUID, "");

                    // change source class to target of found elements if there are still signals on flow
                    // if no signals on flow delete t_xref entry
                    if (description == "")
                    {
                        sql = "delete from t_xref where xrefID = '" + odbc.Rs.Fields[1].Value.ToString().Trim() + "' " +
                        " ;";
                    }
                    else
                    {
                        sql = "Update t_xref" +
                            " set description = '" + description + "' " +
                            "  where xrefID = '" + odbc.Rs.Fields[1].Value.ToString().Trim() + "' " +
                            " ;";
                    }
                    if (odbc.OdbcCmd(sql) == false)
                    {
                        odbc.Rs.Close();
                        return false;
                    }
                }
                odbc.Rs.MoveNext();
            }
            odbc.Rs.Close();
            return true;
        }
        private static bool Delete_t_xref(Odbc odbc, string srcGUID)
        {   // delete GUID in description
            // search for all description fields in xref where the source GUID is used
            string sql = "SELECT x.description, x.xrefID " +
                  " FROM t_xref x" +
                  $" WHERE x.description like '{odbc.Wc}{srcGUID.Trim()}{ odbc.Wc}';";
            try
            { odbc.Rs.Close(); }
            catch (Exception e)
            {
            }
            odbc.Rs.Open(sql, odbc.Cn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, 0);
            while (odbc.Rs.EOF == false)
            {
                // delete the source ID
                string description = odbc.Rs.Fields[0].Value.ToString();
                description = description.Replace("," + srcGUID, "");
                description = description.Replace(srcGUID + ",", "");
                description = description.Replace(srcGUID, "");
                // change source class to target of found elements if there are still signals on flow
                // if no signals on flow delete t_xref entry
                if (description == "")
                {
                    sql = "delete from t_xref where xrefID = '" + odbc.Rs.Fields[1].Value.ToString() + "' " +
                    " ;";
                }
                else
                {
                    sql = "Update t_xref" +
                        " set description = '" + description + "' " +
                        "  where xrefID = '" + odbc.Rs.Fields[1].Value.ToString() + "' " +
                        " ;";
                }
                if (odbc.OdbcCmd(sql) == false)
                {
                    odbc.Rs.Close();
                    return false;
                }
                odbc.Rs.MoveNext();
            }
            odbc.Rs.Close();
            return true;
        }

        private static bool ChangeTypeAttribute(Odbc odbc, int srcId, EA.Element  target)
        {
            // change type of attribute to target
            string sql = $"update t_attribute set classifier = '{target.ElementID}', Type = '{target.Name}'" +
                $" where classifier = '{srcId}' ;";

            if (odbc.OdbcCmd(sql) == false) return false;

            return true;
        }

        /// <summary>
        /// Sets the parent id, package id to the new target
        /// </summary>
        /// <param name="odbc"></param>
        /// <param name="srcId"></param>
        /// <param name="targetId"></param>
        /// <param name="targetPkgId"></param>
        /// <returns></returns>
        private static bool ChangeObjectParent(Odbc odbc, int srcId, int targetId, int targetPkgId)
        {
            // change parent_id to target
            // change package_id to target package
            string sql = "update t_object set ParentID = " + targetId +
                    " , Package_id = " + targetPkgId +
                " where parentID = " + srcId + " ;";
            if (odbc.OdbcCmd(sql) == false) return false;
            return true;
        }
        private static bool ChangeObjectClassifier(Odbc odbc, int srcId, int targetId)
        {
            // change type of an object (classifier) to target
            string sql = $"update t_object set Classifier = {targetId} " +
                         $" where Classifier = {srcId} ;";

            if (odbc.OdbcCmd(sql) == false) return false;

            return true;
        }
        private static bool ChangeTypeOperation(Odbc odbc, int srcId, EA.Element target)
        {
            // change type to target
            string sql = $"update t_operation set Classifier = '{target.ElementID}', Type = '{target.Name}'" +
                $" where Classifier = '{srcId}' ;";

            if (odbc.OdbcCmd(sql) == false) return false;

            return true;
        }


        private static bool ChangeTypeOperationParameter(Odbc odbc, int srcId, EA.Element target)
        {
            // change type to target
            string sql = $"update t_operationparams set Classifier = '{target.ElementID}', Type = '{target.Name}'" +
                         $" where Classifier = '{srcId}' ;";

            if (odbc.OdbcCmd(sql) == false) return false;

            return true;
        }



        private static bool DeleteSource(EA.Repository rep, Odbc odbc, EA.Element srcEl, EA.Element targetEl)
        {
            EA.Package pkg = rep.GetPackageByID(srcEl.PackageID);
            short i = 0;
            int srcID = srcEl.ElementID;
            foreach (EA.Element el in pkg.Elements)
            {
                if (srcID == el.ElementID)
                {
                    pkg.Elements.DeleteAt(i, true);
                    return true;

                }
                i += 1;
            }
            return false;
        }

        /// <summary>
        /// Change occurrence in diagram of source to target. It doesn't change the current diagram.
        /// </summary>
        /// <param name="odbc"></param>
        /// <param name="diaId"></param>
        /// <param name="srcId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        private static bool ChangeDiagramUsage(Odbc odbc, int diaId, int srcId, int targetId)
        {
            // all source elements on diagrams are to exchange with target
            // as starting point and as target point
            string sql = $"Update t_diagramobjects set Object_ID = {targetId}" +
                $"  where object_id = {srcId} AND diagram_id <> {diaId} ;";

            if (odbc.OdbcCmd(sql) == false) return false;

            return true;
        }

        /// <summary>
        /// Change the connector from source to target
        /// - As source
        /// - As target
        /// </summary>
        /// <param name="odbc"></param>
        /// <param name="srcId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        private static bool ChangeConnector(Odbc odbc, int srcId, int targetId)
        {
            // Change the connection from slave to master in all connectors
            // as starting point and as target point
            // It solves:
            // - Inheritance
            // - Source and target of a connection
            // Note: An inheritance/generalization from source signal to target signal is not moved

            // object is target of connector
            string sql = "Update t_connector set end_Object_ID = " + targetId + " where EXISTS (" +
                " Select * " +
                " From  t_object o, t_connector c " +
                " Where  " +
                "        c.Connector_type <> 'Dependency' " +
                "  AND   c.End_Object_ID =  " + srcId +
                "  AND   o.object_id     = c.start_object_id " +
                "  AND   c.connector_ID  = t_connector.connector_id " +
                " );";
            if (odbc.OdbcCmd(sql) == false) return false;

            // object is source of connector
            sql = "Update t_connector set start_Object_ID = " + targetId + " where EXISTS (" +
                        " Select * " +
                        " From  t_object o, t_connector c " +
                        " Where  " +
                        "        c.Connector_type <> 'Dependency' " +
                        "  AND   c.Start_Object_ID =  " + srcId +
                        "  AND   o.object_id     = c.end_object_id " +
                        "  AND   c.connector_ID  = t_connector.connector_id " +
                        "  AND   c.Connector_Type <> 'Generalization' " +
                        " );";
            if (odbc.OdbcCmd(sql) == false) return false;

            return true;
        }
    }
}
