using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EA;
using hoTools.Utils;
using hoTools.Utils.Configuration;
using hoTools.Utils.Diagram;
using hoTools.Utils.SQL;
using hoTools.Utils.Extension;
using DiagramObject = EA.DiagramObject;
using Element = EA.Element;
using Package = EA.Package;
using hoTools.Utils.Json;
using Newtonsoft.Json.Linq;

namespace hoTools.EAServicesPort
{
    public class PortServices
    {
        private readonly Repository _rep;
        int _count; // a variable to count the amount of something

        private List<PortAlignmentItem> _portAlignmentItems; 

        #region Constructor
        /// <summary>
        /// Port services like: Label, Connect, Copy, Delete,..
        /// </summary>
        /// <param name="rep"></param>
        public PortServices(Repository rep)
        {
            _rep = rep;
            _count = 0;


            // global configuration parameters independent from EA-Instance and used by services
            var globalCfg = HoToolsGlobalCfg.Instance;

            // use 'Deserializing Partial JSON Fragments'
            JObject search;
            try
            {
                // Read JSON
                string text = System.IO.File.ReadAllText(globalCfg.JasonFilePath);
                search = JObject.Parse(text);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Can't read '{globalCfg.JasonFilePath}'

{e}", "Can't import 'PortAlignment' settings from Settings.json. ");
                return;
            }

            _portAlignmentItems = (List<PortAlignmentItem>)JasonHelper.GetConfigurationStyleItems<PortAlignmentItem>(search, "PortAlignment", ignoreError:true);

            // nothing imported, use default values
            if (_portAlignmentItems == null)
            {
                _portAlignmentItems = new List<PortAlignmentItem>();
                _portAlignmentItems.Add(new PortAlignmentItem("Port",  
                    "-80","0","0","0",    
                    "20", "0", "0", "0",   
                    "0", "-90", "0", "1",
                    "0", "50", "0", "1"
                    ));
                _portAlignmentItems.Add(new PortAlignmentItem("Interface",
                    "-80", "0", "0", "0",
                    "50", "0", "0", "0",
                    "0", "-80", "0", "1",
                    "0", "70", "0", "1"
                ));
                _portAlignmentItems.Add(new PortAlignmentItem("Port1",
                    "25", "0", "0", "0",
                    "-140", "0", "0", "0",
                    "0", "30", "0", "1",
                    "0", "-105", "0", "1"
                ));
                _portAlignmentItems.Add(new PortAlignmentItem("Interface1",
                    "60", "0", "0", "0",
                    "-100", "0", "0", "0",
                    "0", "65", "0", "1",
                    "0", "-120", "0", "1"
                ));
            }


        }
        #endregion
       
       

        #region copyPortsGUI
        /// <summary>
        /// Copy Ports
        /// </summary>
        public void CopyPortsGui()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DoCopyPortsGui();
                Cursor.Current = Cursors.Default;
                MessageBox.Show($@"{_count} ports copied!");
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error copying ports");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doCopyPortsGUI
        public int DoCopyPortsGui()
        {
            Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return 0;
            int count = dia.SelectedObjects.Count;
           
            if (count < 2) return 0;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element
            var trgEl = (Element)_rep.GetContextObject();
            if (trgEl.Type != "Class" && trgEl.Type != "Component")
            {
                MessageBox.Show($"Target Element is '{trgEl.Type}'",@"Target element for copy Port has to be Class or Component");
                return 0;
            }
           
            // over all selected DiagramObjects
            for (int i = 0; i < count; i = i + 1)
            {
                var srcObj = (DiagramObject)dia.SelectedObjects.GetAt((short)i);
                var srcEl = _rep.GetElementByID(srcObj.ElementID);
                if (srcEl.ElementID == trgEl.ElementID) continue;

                if (srcEl.Type == "Port")
                {
                    // selected element was port
                    CopyPort(_rep, srcEl, trgEl);
                    _count += 1;
                }
                else
                {   // selected element was "Port"
                    foreach (Element p in srcEl.EmbeddedElements)
                    {
                        if (srcEl.ElementID == trgEl.ElementID) continue;
                        if (p.Type == "Port")
                        {
                            CopyPort(_rep, p, trgEl);
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
        /// Copy port to target element. It port exists it don't copy it. The Ports are locked against changes.
        /// Note: hoTools don't copy tagged values
        /// </summary>
        /// <param name="srcPort"></param>
        /// <param name="trgEl"></param>
        public static void CopyPort(EA.Repository rep, Element srcPort, Element trgEl)
        {
            bool isUpdated = false;
            if (srcPort.Type != "Port") return;
            // check if port already exits
            
            foreach (Element trgtPort in trgEl.EmbeddedElements)
            {
                // the target port already exists in source (Target Port PDATA3 contains ea_guid of source port the port is dependent from)
                if (srcPort.ElementGUID == trgtPort.MiscData[2])
                {
                   isUpdated = true;
                    
                    break;
                }
            }
            // Source port isn't available in target Part
            if (isUpdated == false)
            {
                // Create new Port and set the properties according to source port
                var newPort = (Element) trgEl.EmbeddedElements.AddNew(srcPort.Name, "Port");
                trgEl.EmbeddedElements.Refresh();
                newPort.Stereotype = srcPort.Stereotype;
                newPort.Notes = srcPort.Notes;
                newPort.PropertyType = srcPort.PropertyType;
                newPort.Update();
                if (trgEl.Type == "Property" || trgEl.Type == "Part")
                {
                    // Link new Port to the source Port GUID (only if property/part which inherits from it's block)
                    Util.SetElementPdata3(rep, newPort, srcPort.ElementGUID);
                }
                newPort.Refresh();
                //newPort.Locked = true;
            }
            
        }
        #endregion

        
        
        #region orderDiagramObjectsGUI
        public void OrderDiagramObjectsGui()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DoOrderDiagramObjectsGui();
                Cursor.Current = Cursors.Default;
               
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error ordering diagram objects");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doOrderDiagramObjectsGUI
        public void DoOrderDiagramObjectsGui()
        {
            Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;

            if (count < 2) return;
            _rep.SaveDiagram(dia.DiagramID);

            var eadia = new EaDiagram(_rep);
            eadia.SortSelectedObjects();
            eadia.ReloadSelectedObjectsAndConnector();
 
            //_rep.ReloadDiagram(dia.DiagramID);


        }
        #endregion

       

        
        
        
        #region changeLabelGUI
        [Flags]
        public enum LabelStyle
        {
            IsHidden = 1,        // HDN=1; Hide Label/Name
            IsShown = 2,         // HDN=0; Show Label/Name
            PositionLeft = 4,
            PositionRight = 8,
            PositionPlus = 16,    // Label position right
            PositionMinus = 32,   // Label position left
            IsTypeHidden = 64, // PType=0;
            IsTypeShown = 128,  // PType=1;
            IsPortResizable = 256, // PortResizable=1;
            IsNotPortResizable = 512, // PortResizable=0;
            RotateLabel = 1024,     // Rotate Label
            AlignLabel1 = 2048,  // Align Label to default settings 1
            PositionUpPlus = 4096, // Label position up
            PositionDownPlus = 9128, // Label position down
            AlignLabel2 = 18356       // Align Label 2 (default set 2)
        }

        /// <summary>
        /// Change the type of nodes:
        /// - All nodes
        /// - Selected nodes
        /// - Pass the LabelStyle attribute you want to change (see type LabelStyle)
        ///   hoTools updates: DiagramObjects.Styles 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="embeddedCheckSub">true: If port check also Required- and Provided Interface</param>
        public void ChangeLabelGui(EA.Repository rep,  LabelStyle style, bool embeddedCheckSub=false)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DoChangeLabelGui(rep, style, embeddedCheckSub);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error label hide/show");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doChangeLabelGUI

        /// <summary>
        /// Worker for change the type of nodes:
        /// - All nodes
        /// - Selected nodes
        /// - Pass the LabelStyle attribute you want to change (see type LabelStyle)
        ///   hoTools updates: DiagramObjects.Styles 
        /// </summary>
        /// <param name="style"></param>
        /// <param name="embeddedCheckSub"></param>
        private void DoChangeLabelGui(EA.Repository rep, LabelStyle style, bool embeddedCheckSub = false)
        {
            EaDiagram eaDia = new EaDiagram(rep);
            Diagram dia =eaDia.Dia;
            if (dia == null) return;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element

            // for each selected element
            foreach (DiagramObject obj in eaDia.SelObjects)
            {
                var el = _rep.GetElementByID(obj.ElementID);
                if (el.IsEmbeddedElement(rep, true))
                {
                    
                    DiagramObject portObj = dia.GetDiagramObjectByID(el.ElementID, "");
                    //EA.DiagramObject portObj = dia.GetDiagramObjectByID(el.ElementID, "");
                    DoChangeLabelStyle(el, portObj, style);
                }
                else
                {   // all element like Class, Component,..
                    foreach (Element p in el.EmbeddedElements)
                    {
                        if (p.IsEmbeddedElement(rep, true) )
                        {
                            DiagramObject portObj = dia.GetDiagramObjectByID(p.ElementID, "");
                            if (portObj != null)
                            {
                                //EA.DiagramObject portObj = dia.GetDiagramObjectByID(p.ElementID, "");
                                // HDN=1;  Label hidden
                                // HDN=0;  Label visible
                                DoChangeLabelStyle(p, portObj, style);
                                if (p.Type == "Port" && embeddedCheckSub)
                                {
                                    // Check if embedded interface
                                    foreach (EA.Element p1 in p.EmbeddedElements)
                                    {
                                        DiagramObject portObj1 = dia.GetDiagramObjectByID(p1.ElementID, "");
                                        if (portObj1 != null)
                                        {
                                            DoChangeLabelStyle(p, portObj, style);
                                        }


                                    }
                                }
                            }
                        }
                    }
                }
            }
            _rep.ReloadDiagram(dia.DiagramID);
            eaDia.ReloadSelectedObjectsAndConnector();

        }
        #endregion

        #region doChangeLabelStyle
        /// <summary>
        /// Set the style of an diagram object. The style is code with enum LabelStyle.
        /// -HDN Hide Label
        /// - PType: Output type
        /// - OX=nn; Label Position, -nn=Left, +nn = Right
        /// </summary>
        /// <param name="portObj"></param>
        /// <param name="style"></param>
        public void DoChangeLabelStyle(EA.Element el, EA.DiagramObject portObj, LabelStyle style)
        {
            Match match;
            switch (style)
            {
                case LabelStyle.IsHidden:
                    ChangeDiagramObjectStyle(portObj, @"HDN=0", "HDN=1");
                    break;

                case LabelStyle.IsShown:
                    ChangeDiagramObjectStyle(portObj, @"HDN=1", "HDN=0");
                    break;
                case LabelStyle.IsPortResizable:
                    ChangeDiagramObjectStyle(portObj, @"PortResizable=0", "PortResizable=1");
                    break;
                case LabelStyle.IsNotPortResizable:
                    ChangeDiagramObjectStyle(portObj, @"PortResizable=1", "PortResizable=0");
                    break;
                case LabelStyle.IsTypeHidden:
                    ChangeDiagramObjectStyle(portObj, @"PType=1", "PType=0");
                    break;

                case LabelStyle.IsTypeShown:
                    ChangeDiagramObjectStyle(portObj, @"PType=0", "PType=1");
                    break;

                case LabelStyle.PositionLeft:
                    ChangeDiagramObjectStyle(portObj, @"OX=[\+\-0-9]*", @"OX=-200");
                    break;

                case LabelStyle.PositionRight:
                    ChangeDiagramObjectStyle(portObj, @"OX=[\+\-0-9]*", @"OX=24");
                    break;

                case LabelStyle.PositionDownPlus:
                    MoveLabel(portObj, "OY", 15);
                    break;
                case LabelStyle.PositionUpPlus:
                    MoveLabel(portObj, "OY", -15);
                    break;

                case LabelStyle.PositionMinus:
                    // get old x position
                    MoveLabel(portObj, "OX", -15);
                   
                    break;

                // add offset to position
                case LabelStyle.PositionPlus:
                    MoveLabel(portObj, "OX", +15);
                    break;


                // Round robin Rotation settings (no, clockwise, anti clockwise)
                case LabelStyle.RotateLabel:
                    // get old rotation
                    string newRotationValue = "";
                    match = Regex.Match((string)portObj.Style, @"ROT=(0|-1|1)");
                    if (match.Success)
                    {
                        string oldRotationValue = match.Value;
                        switch (oldRotationValue)
                        {
                            case "ROT=0":
                                newRotationValue = "ROT=1";
                                break;
                            case "ROT=-1":
                                newRotationValue = "ROT=0";

                                break;
                            case "ROT=1":
                                newRotationValue = "ROT=-1";
                                break;

                        }
                        if (newRotationValue != "") ChangeDiagramObjectStyle(portObj, oldRotationValue, newRotationValue);
                    }
                    break;

                // Align Label to default position
                case LabelStyle.AlignLabel1:
                    var edge = portObj.Edge(_rep);
                    if (el.Type.Contains("Interface"))
                    {
                        // Required / Provided Interface
                        PortAlignmentItem settingsInterface = _portAlignmentItems.Find(x => x.Type == "Interface");
                        AlignLabel(portObj, edge, settingsInterface);
                    }
                    else
                    {   // Ports, Pins,..
                        PortAlignmentItem settingsPort = _portAlignmentItems.Find(x => x.Type == "Port");
                        AlignLabel(portObj, edge, settingsPort);
                        
                    }
                    break;
                // Align Label to default position 2
                case LabelStyle.AlignLabel2:
                    edge = portObj.Edge(_rep);
                    if (el.Type.Contains("Interface"))
                    {
                        // Required / Provided Interface
                        PortAlignmentItem settingsInterface = _portAlignmentItems.Find(x => x.Type == "Interface1");
                        AlignLabel(portObj, edge, settingsInterface);
                    }
                    else
                    {   // Ports, Pins,..
                        PortAlignmentItem settingsPort = _portAlignmentItems.Find(x => x.Type == "Port1");
                        AlignLabel(portObj, edge, settingsPort);

                    }
                    break;
            }

            //string style = (string)portObj.Style;
            //if (isHidden) style = style.Replace("HDN=0", "HDN=1");
            //else style = style.Replace("HDN=1", "HDN=0");
            //portObj.Style = style;
        }
        /// <summary>
        /// Align Label
        /// </summary>
        /// <param name="portObj"></param>
        /// <param name="edge"></param>
        /// <param name="settingsInterface"></param>
        private void AlignLabel(DiagramObject portObj, EaExtensionClass.EmbeddedPosition edge, PortAlignmentItem settingsInterface)
        {
            switch (edge)
            {
                case EaExtensionClass.EmbeddedPosition.Top:
                    ChangeDiagramObjectLabel(portObj, x: settingsInterface.XTop, y: settingsInterface.YTop,
                        rotation: settingsInterface.RotationTop);
                    break;
                case EaExtensionClass.EmbeddedPosition.Bottom:
                    ChangeDiagramObjectLabel(portObj, x: settingsInterface.XBottom, y: settingsInterface.YBottom,
                        rotation: settingsInterface.RotationBottom);
                    break;

                case EaExtensionClass.EmbeddedPosition.Right:
                    ChangeDiagramObjectLabel(portObj, x: settingsInterface.XRight, y: settingsInterface.YRight,
                        rotation: settingsInterface.RotationRight);
                    break;
                case EaExtensionClass.EmbeddedPosition.Left:
                    ChangeDiagramObjectLabel(portObj, x: settingsInterface.XLeft, y: settingsInterface.YLeft,
                        rotation: settingsInterface.RotationLeft);
                    break;
            }
        }

        #endregion
        /// <summary>
        /// Change Diagram Object Label (x, y, rotation)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rotation">0=none, 1=clockwise, -1 anti clockwise</param>
        private void ChangeDiagramObjectLabel(EA.DiagramObject obj, string x, string y, string rotation)
        {
           // make one line (works for horizontal and vertical)
           ChangeDiagramObjectStyle(obj, @"CX=[\+\-0-9]*", $@"CX=100");
           ChangeDiagramObjectStyle(obj, @"CY=[\+\-0-9]*", $@"CY=13");
           
            ChangeDiagramObjectStyle(obj, @"OX=[\+\-0-9]*", $@"OX={x}");
            ChangeDiagramObjectStyle(obj, @"OY=[\+\-0-9]*", $@"OY={y}");
            ChangeDiagramObjectStyle(obj, "ROT=(0|-1|1)", $"ROT={rotation}");
        }

        /// <summary>
        /// MoveLabel amount Pixels according to EA coordinate system. Give the type as 'OX' (horizontal) or 'OY' (vertical)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">'OX' or 'OY'</param>
        /// <param name="amountPixels"></param>
        private void MoveLabel(EA.DiagramObject obj, string type, int  amountPixels)
        {
            Match match = Regex.Match((string)obj.Style, $@"{type}=([\+\-0-9]*)");
            if (match.Success)
            {
                int xPos = Convert.ToInt32(match.Groups[1].Value) + amountPixels;
                ChangeDiagramObjectStyle(obj, $@"{type}=[\+\-0-9]*", $@"{type}={xPos}");
            }

        }
        
        #region ChangeDiagramObjectStyle
        /// <summary>
        /// Change Style of a DiagramObject. 
        /// </summary>
        /// <param name="portObj"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private static void ChangeDiagramObjectStyle(DiagramObject portObj, string from, string to)
        {
            var style = (string)portObj.Style;

            Match match = Regex.Match(style, from);

            if (match.Success)
            {
                style = style.Replace(match.Value, to);
            }
            else // Empty style, just add new style
            {
                style = style.Replace($"{to};", "");// delete possible new value
                style = style + $" {to};";// insert new value
            }
            // update style
            portObj.Style = style;
            portObj.Update();
        }
        #endregion
        #region GetDiagramObjectStyle

        /// <summary>
        /// Get Style of a DiagramObject. 
        /// </summary>
        /// <param name="portObj"></param>
        /// <param name="styleName"></param>
        private static string GetDiagramObjectStyle(DiagramObject portObj, string styleName)
        {
            var style = (string)portObj.Style;

            Match match = Regex.Match(style, $"{styleName}=[;]*;");

            if (match.Success)
            {
                return match.Value;
            }
            return "";
        }
        #endregion




        #region deletePortsMarkedPorts
        public void DeletePortsMarkedPorts()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DoDeleteMarkedPortsGui();
                Cursor.Current = Cursors.Default;
                MessageBox.Show($@"{_count} Ports / CharacteristicData / CharacteristicCurve deleted");
            }

            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error deleting Ports / CharacteristicData / CharacteristicCurve");
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
        }
        #endregion
        #region doDeleteMarkedPortsGUI
        public void DoDeleteMarkedPortsGui()
        {
            Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return;
            int count = dia.SelectedObjects.Count;
            _rep.SaveDiagram(dia.DiagramID);

            // target object/element

            for (int i = 0; i < count; i = i + 1)
            {
                var obj = (DiagramObject)dia.SelectedObjects.GetAt((short)i);
                var el = _rep.GetElementByID(obj.ElementID);
                for (int i1 = el.EmbeddedElements.Count -1; i1 >= 0; i1 = i1-1)
                {
                    var p = (Element)el.EmbeddedElements.GetAt((short)i1);
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
        public void ConnectPortsInsideGui()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                var eaDia = new EaDiagram(_rep);
                DoConnectPortsInsideGui();

                eaDia.ReloadSelectedObjectsAndConnector();
                Cursor.Current = Cursors.Default;
                MessageBox.Show($@"{_count} ports connected!");
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error generating ports");
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
        public int DoConnectPortsInsideGui()
        {
            Diagram dia = _rep.GetCurrentDiagram();
            if (dia == null) return 0;
            int count = dia.SelectedObjects.Count;

            _rep.SaveDiagram(dia.DiagramID);

            // target object/element


            // all selected objects
            for (int iSrc = 0; iSrc < count ; iSrc += 1)
            {
                var srcObj = (DiagramObject)dia.SelectedObjects.GetAt((short)iSrc);
                var srcEl = _rep.GetElementByID(srcObj.ElementID);

                ConnectPortsOf2Classes(srcEl, srcEl);
              
            }
            _rep.ReloadDiagram(dia.DiagramID);
            return _count;



        }
        #endregion



        #region connectPortsGUID
        /// <summary>
        /// Connect Ports between selected Elements from GUI with Mouse Wait
        /// It connects Ports of different elements with the same name with a not directed connector
        /// </summary>
        public void ConnectPortsGui()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                var eaDia = new EaDiagram(_rep);
                DoConnectPortGui();

                eaDia.ReloadSelectedObjectsAndConnector();
                Cursor.Current = Cursors.Default;
                MessageBox.Show($@"{_count} ports connected!");
            }
            catch (Exception e11)
            {
                MessageBox.Show(e11.ToString(), @"Error generating ports");
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
        public void DoConnectPortGui () 
        {
            var lElId = new List<int>();
            Package pkg = _rep.GetTreeSelectedPackage();
            if (pkg == null) return;
            
            // overall selected elements 
            Diagram dia = _rep.GetCurrentDiagram();
            if (dia != null && dia.SelectedObjects.Count > 0)
            {
                foreach (DiagramObject obj in dia.SelectedObjects)
                {
                    lElId.Add(obj.ElementID);
                }

            }
            else
            {
                var rec = new ElementRecursive(_rep);
                lElId = rec.GetItemsRecursive(pkg);
            }

            // between all components / classes
            for (int i1 = 0; i1 < lElId.Count -1; i1 +=1)
            {
                // source id
                int srcId = lElId[i1];
                _rep.ShowInProjectView(_rep.GetElementByID(srcId) );
                Element srcEl = _rep.GetElementByID(srcId);
                for (int i2 = i1+1; i2 < lElId.Count; i2 += 1)
                {
                    int trgtId = lElId[i2];
                    if (srcId == trgtId) continue;
                    Element trgtEl = _rep.GetElementByID(trgtId);

                    ConnectPortsOf2Classes(srcEl, trgtEl);
                }
            }
            _rep.ReloadDiagram(dia.DiagramID);
        }
        #endregion
        #region ConnectPortsOf2Classes
        public void ConnectPortsOf2Classes(Element srcEl, Element trgtEl)
        {
            foreach (Element srcPort in srcEl.EmbeddedElements)
            {
                foreach (Element trgtPort in trgtEl.EmbeddedElements)
                {
                    // don't connect to itself
                    if (srcPort.Name == trgtPort.Name && srcPort.ElementID != trgtPort.ElementID)
                    {
                        //if (srcPort.Stereotype != trgtEl.Stereotype)
                        //{
                        //    // only connect:
                        //    // sender to receiver
                        //    // client to server
                        //    // check if connection already exists
                        //    if (srcPort.Stereotype == "Sender")
                        //        if (trgtPort.Stereotype != "Receiver") continue;
                        //    if (srcPort.Stereotype == "Receiver")
                        //        if (trgtPort.Stereotype != "Sender") continue;
                        //    if (srcPort.Stereotype == "Client")
                        //        if (trgtPort.Stereotype != "Server") continue;
                        //    if (srcPort.Stereotype == "Server")
                        //        if (trgtPort.Stereotype != "Client") continue;

                            var sql = new UtilSql(_rep);
                            if (sql.IsConnectionAvailable(srcPort, trgtPort) == false)
                            {
                                // direction of connector
                                if (srcPort.Stereotype == "Sender" | srcPort.Stereotype == "Client")
                                {
                                    var con = (Connector)srcPort.Connectors.AddNew("", "Connector");
                                    srcPort.Connectors.Refresh();
                                    con.SupplierID = trgtPort.ElementID;
                                    con.Update();
                                    _count += 1;
                                }
                                else
                                {
                                    var con = (Connector)trgtPort.Connectors.AddNew("", "Connector");
                                    trgtPort.Connectors.Refresh();
                                    con.SupplierID = srcPort.ElementID;
                                    con.Update();
                                    _count += 1;
                                }
                            }
                        //}
                    }
                }
            }
        }
        #endregion
     

        #region setConnectionDirectionUnspecifiedGUI
        public void SetConnectionDirectionUnspecifiedGui()
        {
            var eaDia = new EaDiagram(_rep);
            Diagram dia = eaDia.Dia;
            if (eaDia.Dia == null) return;
            if (eaDia.SelectedObjectsCount == 0) return;
            eaDia.Save();

            // target object/element

            for (int i = 0; i < eaDia.SelectedObjectsCount; i++)
            {
                var obj = (DiagramObject)dia.SelectedObjects.GetAt((short)i);
                var el = _rep.GetElementByID(obj.ElementID);

                if (el.Type == "Port")
                {
                    // selected element was port
                    SetElementConnectorDirectionUnspecified(el);
                }
                else
                {   // selected element was "Element"
                    foreach (Element port in el.EmbeddedElements)
                    {
                        SetElementConnectorDirectionUnspecified(port);
                    }

                }

            }
            eaDia.ReloadSelectedObjectsAndConnector();
        }
        #endregion  
        #region setElementConnectorDirectionUnspecified
        private void SetElementConnectorDirectionUnspecified(Element el) {
            foreach (Connector con in el.Connectors)
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

      