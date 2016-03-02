using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using hoTools.EaServices;
using hoTools.Utils;
using hoTools.Utils.SQL;
using hoTools.Utils.RUN;

namespace hoTools.EAServicesPort
{
    public class PortServices
    {
        private EA.Repository _rep = null;
        private int _count = 0; // a variable to count the amout of something
        private const string EMBEDDED_ELEMENT_TYPES ="Port Parameter Pin";
              
        

        #region Constructor
        public PortServices(EA.Repository rep)
        {
            _rep = rep;
            _count = 0;

            
        }
        #endregion
       
       

        #region copyPortsGUI
        public void copyPortsGUI()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                doCopyPortsGUI();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(String.Format("{0} ports copied!", _count));
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error copying ports");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doCopyPortsGUI
        public int doCopyPortsGUI()
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return 0;
            int count = dia.SelectedObjects.Count;
           
            if (count < 2) return 0;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject srcObj;
            EA.Element srcEl;
            var trgEl = (EA.Element)_rep.GetContextObject();
            if (trgEl.Type != "Class" && trgEl.Type != "Component")
            {
                MessageBox.Show("Target element has to be Class or Component", "");
                return 0;
            }
           
            // over all selected DiagramObjects
            for (int i = 0; i < count; i = i + 1)
            {
                srcObj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                srcEl = (EA.Element)_rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;

                if (srcEl.Type == "Port")
                {
                    // selected element was port
                    copyPort(srcEl, trgEl);
                    _count += 1;
                }
                else
                {   // selected element was "Port"
                    foreach (EA.Element p in srcEl.EmbeddedElements)
                    {
                        if (srcEl.ElementID == trgEl.ElementID) continue;
                        if (p.Type == "Port")
                        {
                            copyPort(p, trgEl);
                            _count += 1;
                        }
                    }
                }
               
            }
            return _count;
            //_rep.ReloadDiagram(dia.DiagramID);
            
   
        } 
        #endregion
        #region copyPort
        /// <summary>
        /// Copy port to target element
        /// </summary>
        /// <param name="srcPort"></param>
        /// <param name="trgEl"></param>
        private void copyPort(EA.Element srcPort, EA.Element trgEl)
        {
            bool isUpdate = false;
            EA.Element trgPort = null;
            if (srcPort.Type != "Port") return;
            // check if port already exits
            
            foreach (EA.Element p in trgEl.EmbeddedElements)
            {
                if (p.Name == srcPort.Name && p.Stereotype == srcPort.Stereotype)
                {
                    isUpdate = true;
                    trgPort = p;
                    break;
                }
            }

            if (isUpdate == false)
            {
                string name = srcPort.Name;
                string stereotype = srcPort.Stereotype;
                if (isUpdate == false)
                {
                    trgPort = (EA.Element)trgEl.EmbeddedElements.AddNew(name, "Port");
                    trgEl.EmbeddedElements.Refresh();
                }
                trgPort.Stereotype = stereotype;
                trgPort.Update();
            }

        }
        #endregion

        #region deletePortsGUI
        public void deletePortsGUI()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                doDeletePortsGUI();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(String.Format("{0} ports deleted!", _count));
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error deleting ports");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doDeletePortsGUI
        public int doDeletePortsGUI()
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return 0;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return 0;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject obj;
            EA.Element el;

            for (int i = 0; i < selCount; i++)
            {
                obj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                el = _rep.GetElementByID(obj.ElementID);
                
                if (el.Type == "Port")
                {
                    // selected element was port
                    delPort(el);
                    _count += 1;
                }
                else
                {   // selected element was "Element"
                    foreach (EA.Element p in el.EmbeddedElements)
                    {
                        delPort(p);
                        _count += 1; 
                    }
                }

            }

            return _count;
       } 
        #endregion
        #region delPort
        private void delPort(EA.Element port)
        {
            EA.Element el = _rep.GetElementByID(port.ParentID);
            for (int i = 0; i < el.EmbeddedElements.Count; i++)
            {
                var p1 = (EA.Element)el.EmbeddedElements.GetAt((short)i);
                if (p1.ElementID == port.ElementID)
                {
                    el.EmbeddedElements.Delete((short)i);
                    el.EmbeddedElements.Refresh();
                }
            }
        }
        #endregion delPort

        #region orderDiagramObjectsGUI
        public void orderDiagramObjectsGUI()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                doOrderDiagramObjectsGUI();
                Cursor.Current = Cursors.Default;
               
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error ordering diagram objects");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doOrderDiagramObjectsGUI
        public void doOrderDiagramObjectsGUI()
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;

            if (count < 2) return;
            _rep.SaveDiagram(dia.DiagramID);

            var eadia = new EADiagram(_rep);
            eadia.sortSelectedObjects();
            eadia.ReloadSelectedObjectsAndConnector();
 
            //_rep.ReloadDiagram(dia.DiagramID);


        }
        #endregion

        #region showPortsInDiagram
        /// <summary>
        /// Show Ports of selected elements in Diagram. The ports are on the right side of the element.
        /// If isOptimized=true:
        /// - Receiving Ports on the left side (Server, Receiver)
        /// - Sending Ports on the right side (Client, Sender)
        /// </summary>
        /// <param name="isOptimizePortLayout"></param>
        public void showPortsInDiagram(bool isOptimizePortLayout=false)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // get Diagram data
                var eaDia = new EADiagram(_rep);

                // hide all ports
                removePortFromDiagramGUI();
                // show all ports
                eaDia.ReloadSelectedObjectsAndConnector();// reload selection
                EaService.showEmbeddedElementsGUI(_rep, "Port", isOptimizePortLayout);
                // set selction

                eaDia.ReloadSelectedObjectsAndConnector();
                Cursor.Current = Cursors.Default;

            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error show ports on diagram");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion

        #region removePortFromDiagramGUI
        public void removePortFromDiagramGUI()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                this.doRemovePortFromDiagramGUI();
                Cursor.Current = Cursors.Default;

            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error removing ports from diagram");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doRemovePortFromDiagramGUI
        private void doRemovePortFromDiagramGUI()
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return;
            int selCount = dia.SelectedObjects.Count;
            if (selCount == 0) return;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject obj;
            EA.Element el;

            for (int i = selCount - 1; i >= 0; i -= 1)
            {
                obj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                el = _rep.GetElementByID(obj.ElementID);

                if (el.Type == "Port")
                {
                    // selected element was port
                    removePortFromDiagram(dia, el);
                }
                else
                {   // selected element was "Element"
                    foreach (EA.Element port in el.EmbeddedElements)
                    {
                        if (port.Type == "Port")
                        {
                            removePortFromDiagram(dia, port);
                        }
                    }

                }

            }
            _rep.ReloadDiagram(dia.DiagramID);
        }
        #endregion  
        #region removePortFromDiagram
        private void removePortFromDiagram(EA.Diagram dia, EA.Element port)
        {
            for (int i= dia.DiagramObjects.Count-1; i>=0;i-=1)
            {
                var obj = (EA.DiagramObject)dia.DiagramObjects.GetAt((short) i);
                if (obj.ElementID == port.ElementID)
                {
                    dia.DiagramObjects.Delete((short)i);
                    dia.DiagramObjects.Refresh();
                    break;
                } 
            }
           
        }
        #endregion


        #region changeLabelGUI
        [Flags]
        public enum LabelStyle
        {
            IS_HIDDEN = 1,
            IS_SHOWN = 2,
            POSITION_LEFT = 4,
            POSITION_RIGHT = 8,
            POSITION_PLUS = 16,
            POSITION_MINUS = 32
        }
        public void changeLabelGUI(LabelStyle style)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                doChangeLabelGUI(style);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error label hide/show");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doChangeLabelGUI
        public void doChangeLabelGUI(LabelStyle style)
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.Element el;

            // for each selected element
            foreach (EA.DiagramObject obj in dia.SelectedObjects)
            {
                el = _rep.GetElementByID(obj.ElementID);
                if (EMBEDDED_ELEMENT_TYPES.Contains(el.Type) )
                {
                    EA.DiagramObject portObj = Util.getDiagramObjectByID(_rep, dia, el.ElementID);
                    //EA.DiagramObject portObj = dia.GetDiagramObjectByID(el.ElementID, "");
                    doChangeLabelStyle(portObj, style);
                }
                else
                {   // all element like Class, Component,..
                    foreach (EA.Element p in el.EmbeddedElements)
                    {
                        if (! (EMBEDDED_ELEMENT_TYPES.Contains(p.Type))) continue;
                        EA.DiagramObject portObj = Util.getDiagramObjectByID(_rep, dia, p.ElementID);
                        if (portObj != null) {
                            //EA.DiagramObject portObj = dia.GetDiagramObjectByID(p.ElementID, "");
                            // HDN=1;  Label hidden
                            // HDN=0;  Label visible
                            doChangeLabelStyle(portObj, style);
                        }
                    }
                }
            }
        }
        #endregion

        #region doChangeLabelStyle
        private static void doChangeLabelStyle(EA.DiagramObject portObj, LabelStyle style)
        {
            switch (style)
            {
                case LabelStyle.IS_HIDDEN:
                    changeLabel(portObj, @"HDN=0", "HDN=1");
                    break;

                case LabelStyle.IS_SHOWN:
                    changeLabel(portObj, @"HDN=1", "HDN=0");
                    break;

                case LabelStyle.POSITION_LEFT:
                    changeLabel(portObj, @"OX=[\+\-0-9]*", @"OX=-200");
                    break;

                case LabelStyle.POSITION_RIGHT:
                    changeLabel(portObj, @"OX=[\+\-0-9]*", @"OX=24");
                    break;

                case LabelStyle.POSITION_MINUS:
                    // get old x position
                    Match match = Regex.Match(portObj.Style, @"OX=([\+\-0-9]*)");
                    if (match.Success)
                    {
                        int xPos = Convert.ToInt32(match.Groups[1].Value) - 15;
                        changeLabel(portObj, @"OX=[\+\-0-9]*", String.Format("OX={0}", xPos));
                    }
                    break;

                case LabelStyle.POSITION_PLUS:
                    // get old x position
                    match = Regex.Match(portObj.Style, @"OX=([\+\-0-9]*)");
                    if (match.Success)
                    {
                        int xPos = Convert.ToInt32(match.Groups[1].Value) + 15;
                        changeLabel(portObj, @"OX=[\+\-0-9]*", String.Format("OX={0}", xPos)  );
                    }
                    break;
            }

            //string style = (string)portObj.Style;
            //if (isHidden) style = style.Replace("HDN=0", "HDN=1");
            //else style = style.Replace("HDN=1", "HDN=0");
            //portObj.Style = style;
        }
        #endregion
        #region setLabel
        private static void changeLabel(EA.DiagramObject portObj, string from, string to)
        {
            var style = (string)portObj.Style;

            Match match = Regex.Match(style, from);
            if (match.Success)
            {
                style = style.Replace(match.Value, to);
                portObj.Style = style;
                portObj.Update();
            }
        }
        #endregion




        #region deletePortsMarkedPorts
        public void deletePortsMarkedPorts()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                doDeleteMarkedPortsGUI();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(String.Format("{0} Ports / CharacteristicData / CharacteristicCurve deleted", _count));
            }

            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error deleting Ports / CharacteristicData / CharacteristicCurve");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doDeleteMarkedPortsGUI
        public void doDeleteMarkedPortsGUI()
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject obj;
            EA.Element el;

            for (int i = 0; i < count; i = i + 1)
            {
                obj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                el = _rep.GetElementByID(obj.ElementID);
                for (int i1 = el.EmbeddedElements.Count -1; i1 >= 0; i1 = i1-1)
                {
                    var p = (EA.Element)el.EmbeddedElements.GetAt((short)i1);
                    if (p.Type == "Port" ||
                        p.Type == "Part" && p.Stereotype == "CharacteristicCurve" ||
                        p.Type == "Part" && p.Stereotype == "CharacteristicData" )
                    {
                        string name = p.Name;
                        if (name.Contains("_DeleteMe"))
                        {
                            _count += 1;
                            el.EmbeddedElements.Delete((short)i1);
                        }
                    }
                }
            }
        }
        #endregion

        #region connectPortsInsideGUID
        public void connectPortsInsideGUI()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                var eaDia = new EADiagram(_rep);
                doConnectPortsInsideGUI();

                eaDia.ReloadSelectedObjectsAndConnector();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(String.Format("{0} ports connected!", _count));
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error generating ports");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }

        #endregion
        #region doConnectPortsInsideGUI
        /// <summary>
        /// Connect all ports of all selected Class / Components to each other.
        /// </summary>
        /// <returns></returns>
        public int doConnectPortsInsideGUI()
        {
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return 0;
            int count = dia.SelectedObjects.Count;

            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            EA.DiagramObject srcObj;
            EA.Element srcEl;


            // all selected objects
            for (int iSrc = 0; iSrc < count ; iSrc += 1)
            {
                srcObj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)iSrc);
                srcEl = _rep.GetElementByID(srcObj.ElementID);

                ConnectPortsOf2Classes(srcEl, srcEl);
              
            }
            _rep.ReloadDiagram(dia.DiagramID);
            return _count;



        }
        #endregion



        #region connectPortsGUID
        public void connectPortsGUI()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                var eaDia = new EADiagram(_rep);
                doConnectPortGUI();

                eaDia.ReloadSelectedObjectsAndConnector();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(String.Format("{0} ports connected!", _count));
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), "Error generating ports");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doConnectPortsGUI
        /// <summary>
        /// Connect all ports of:
        /// - all selected Class / Components to each other
        /// - recursive selected packages
        /// </summary>
        /// <returns></returns>
        public void doConnectPortGUI () 
        {
            var l_el_id = new List<int>();
            EA.Package pkg = _rep.GetTreeSelectedPackage();
            if (pkg == null) return;
            
            // overall selected elements 
            EA.Diagram dia = _rep.GetCurrentDiagram();
            if (dia != null && dia.SelectedObjects.Count > 0)
            {
                foreach (EA.DiagramObject obj in dia.SelectedObjects)
                {
                    l_el_id.Add(obj.ElementID);
                }

            }
            else
            {
                var rec = new ElementRecursive(_rep);
                l_el_id = rec.getItemsRecursive(pkg);
            }

            // between all components
            for (int i1 = 0; i1 < l_el_id.Count -1; i1 +=1)
            {
                int srcId = l_el_id[i1];
                _rep.ShowInProjectView(_rep.GetElementByID(i1) );
                EA.Element srcEl = _rep.GetElementByID(srcId);
                for (int i2 = i1+1; i2 < l_el_id.Count; i2 += 1)
                {
                    int trgtId = l_el_id[i2];
                    if (srcId == trgtId) continue;
                    EA.Element trgtEl = _rep.GetElementByID(trgtId);

                    ConnectPortsOf2Classes(srcEl, trgtEl);
                }
            }
            _rep.ReloadDiagram(dia.DiagramID);
            return ;
    
                
        }
        #endregion
        #region ConnectPortsOf2Classes
        public void ConnectPortsOf2Classes(EA.Element srcEl, EA.Element trgtEl)
        {
            foreach (EA.Element srcPort in srcEl.EmbeddedElements)
            {
                foreach (EA.Element trgtPort in trgtEl.EmbeddedElements)
                {
                    // don't connect to itself
                    if (srcPort.Name == trgtPort.Name && srcPort.ElementID != trgtPort.ElementID)
                    {
                        if (srcPort.Stereotype != trgtEl.Stereotype)
                        {
                            // only connect:
                            // sender to receiver
                            // client to server
                            // check if connection already exists
                            if (srcPort.Stereotype == "Sender")
                                if (trgtPort.Stereotype != "Receiver") continue;
                            if (srcPort.Stereotype == "Receiver")
                                if (trgtPort.Stereotype != "Sender") continue;
                            if (srcPort.Stereotype == "Client")
                                if (trgtPort.Stereotype != "Server") continue;
                            if (srcPort.Stereotype == "Server")
                                if (trgtPort.Stereotype != "Client") continue;

                            var sql = new UtilSql(_rep);
                            if (sql.isConnectionAvailable(srcPort, trgtPort) == false)
                            {
                                // direction of connector
                                if (srcPort.Stereotype == "Sender" | srcPort.Stereotype == "Client")
                                {
                                    var con = (EA.Connector)srcPort.Connectors.AddNew("", "Connector");
                                    srcPort.Connectors.Refresh();
                                    con.SupplierID = trgtPort.ElementID;
                                    con.Update();
                                    _count += 1;
                                }
                                else
                                {
                                    var con = (EA.Connector)trgtPort.Connectors.AddNew("", "Connector");
                                    trgtPort.Connectors.Refresh();
                                    con.SupplierID = srcPort.ElementID;
                                    con.Update();
                                    _count += 1;
                                }
                            }
                        }
                    }
                }
            }
            return;

        }
        #endregion
     

        #region setConnectionDirectionUnspecifiedGUI
        public void setConnectionDirectionUnspecifiedGUI()
        {
            var eaDia = new EADiagram(_rep);
            EA.Diagram dia = eaDia.Dia;
            if (eaDia.Dia == null) return;
            if (eaDia.SelectedObjectsCount == 0) return;
            eaDia.Save();

            // target object/element
            EA.DiagramObject obj;
            EA.Element el;

            for (int i = 0; i < eaDia.SelectedObjectsCount; i++)
            {
                obj = (EA.DiagramObject)dia.SelectedObjects.GetAt((short)i);
                el = _rep.GetElementByID(obj.ElementID);

                if (el.Type == "Port")
                {
                    // selected element was port
                    setElementConnectorDirectionUnspecified(el);
                }
                else
                {   // selected element was "Element"
                    foreach (EA.Element port in el.EmbeddedElements)
                    {
                        setElementConnectorDirectionUnspecified(port);
                    }

                }

            }
            eaDia.ReloadSelectedObjectsAndConnector();
        }
        #endregion  
        #region setElementConnectorDirectionUnspecified
        private void setElementConnectorDirectionUnspecified(EA.Element el) {
            foreach (EA.Connector con in el.Connectors)
            {
                if (con.Type == "Connector")
                {
                    con.Direction = "Unspecified";
                    con.Update();
                }
            }
        }
        #endregion


       
    }
}

      