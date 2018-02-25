using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.ComponentModel;
using WPF_LogicSimulation.UIData;

namespace WPF_LogicSimulation
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<Line, CQSaveFile_Line> m_LineDatas = new Dictionary<Line, CQSaveFile_Line>();
        BackgroundWorker m_Thread_Sim;
        public MainWindow()
        {
            InitializeComponent();
            this.m_Thread_Sim = new BackgroundWorker();
            this.m_Thread_Sim.DoWork += M_Thread_Sim_DoWork;
            
        }

        private void M_Thread_Sim_DoWork(object sender, DoWorkEventArgs e)
        {
            while(this.m_IsStartSimulate == true)
            {
                try
                {
                    for(int i= 0; i<this.m_Simulate.Count; i++)
                    {
                        List<CQSimulateData> temp_parent = new List<CQSimulateData>() { this.m_Simulate[i] };
                        //List<CQSimulateData> temp = this.m_Simulate[i].Nexts;
                        while(true)
                        {
                            foreach(CQSimulateData parent in temp_parent)
                            {
                                foreach (var sud in parent.Nexts)
                                {
                                    sud.Key.GateData.Pin_in[sud.Value.Destination].IsTrue = parent.GateData.Pin_out[sud.Value.Source].IsTrue;
                                    sud.Key.GateData.Process();
                                }
                            }

                            temp_parent = temp_parent.SelectMany(x => x.Nexts.Keys).ToList();
                            //temp_parent = temp;
                            if (temp_parent.Count == 0)
                            {
                                break;
                            }
                        }
                        
                    }
                    System.Threading.Thread.Sleep(100);
                }
                catch(Exception ee)
                {
                    System.Diagnostics.Trace.WriteLine(ee.Message);
                    System.Diagnostics.Trace.WriteLine(ee.StackTrace);
                }
            }
        }

        CQMainUI m_MainUI;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.m_MainUI == null)
            {
                this.DataContext = this.m_MainUI = new CQMainUI();
                //string[] str_files = Directory.GetFiles(System.Environment.CurrentDirectory, "*.txt");
                //XmlSerializer xml = new XmlSerializer(typeof(CQSaveFile));
                //using (FileStream fs = File.Open(str_files[0], FileMode.Open))
                //{
                //    CQSaveFile sv = (CQSaveFile)xml.Deserialize(fs);
                //    sv.Snapshot = new BitmapImage();
                //    sv.Snapshot.BeginInit();
                //    sv.Snapshot.StreamSource = new MemoryStream(sv.SnapshotRaw);
                //    sv.Snapshot.EndInit();
                //    this.m_MainUI.SaveFiles.Add(sv);
                //}
            }
            
                
        }
        bool m_IsConnect;
        Line m_Line;
        private bool Ggate_OnPinMouseUp(QGate sender, CQPin pin, Point pt)
        {
            this.ConnectEnd(sender, pin, pt);
            return true;
        }

        private bool Ggate_OnPinMouseDwon(QGate sender, CQPin pin, Point pt)
        {
            CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            this.m_Line = new Line();
            this.m_Line.Fill = Brushes.Gray;
            this.m_Line.X1 = this.m_Line.X2 = pin.ConnectPoint.X;
            this.m_Line.Y1 = this.m_Line.Y2 = pin.ConnectPoint.Y;
            this.m_Line.Stroke = Brushes.Gray;
            this.m_Line.StrokeThickness = 1;
            this.m_IsConnect = true;
            CQSaveFile_Line save_line = new CQSaveFile_Line() { Line = this.m_Line };
            if (this.m_LineDatas.ContainsKey(save_line.Line) == false)
            {
                this.m_LineDatas.Add(save_line.Line, save_line);
            }
            else
            {

            }
            this.m_LineDatas[this.m_Line].Begin.GateID = gateui.ID;
            this.m_LineDatas[this.m_Line].Begin.Index = pin.Index;
            this.m_LineDatas[this.m_Line].Begin.Type = pin.Type;
            
            this.m_LineDatas[this.m_Line].Begin.EndType = CQSaveFile_LinePoint.EndTypes.Start;
            this.canvas.Children.Add(this.m_Line);
            return true;
        }

        void Add_Input_Switch(double x=0, double y=0, string id="")
        {
            CQInput_SwitchUI cc = null;
            QInput_Switch input_switch = null;
            cc = new CQInput_SwitchUI();
            cc.GateName = "Switch";
            cc.Type = "Switch";
            cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            input_switch = new QInput_Switch();
            input_switch.Height = 50;
            input_switch.Width = 80;
            cc.ID = id;
            Canvas.SetLeft(input_switch, x);
            Canvas.SetTop(input_switch, y);
            input_switch.DataContext = cc;
            input_switch.OnPinMouseDwon += Input_switch_OnPinMouseDwon;
            input_switch.OnPinMouseUp += Input_switch_OnPinMouseUp;
            input_switch.OnGateMove += Input_switch_OnGateMove;
            this.canvas.Children.Add(input_switch);

        }

        private bool Input_switch_OnGateMove(QInput_Switch gate)
        {
            this.ChangeLine(gate);
            return true;
        }

        private bool Input_switch_OnPinMouseUp(QInput_Switch sender, CQPin pin, Point pt)
        {
            this.ConnectEnd(sender, pin, pt);
            return true;
        }

        private bool Input_switch_OnPinMouseDwon(QInput_Switch sender, CQPin pin, Point pt)
        {
            CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            this.m_Line = new Line();
            this.m_Line.Fill = Brushes.Gray;
            this.m_Line.X1 = this.m_Line.X2 = pin.ConnectPoint.X;
            this.m_Line.Y1 = this.m_Line.Y2 = pin.ConnectPoint.Y;
            this.m_Line.Stroke = Brushes.Gray;
            this.m_Line.StrokeThickness = 1;
            this.m_IsConnect = true;
            CQSaveFile_Line save_line = new CQSaveFile_Line() { Line = this.m_Line };
            if (this.m_LineDatas.ContainsKey(save_line.Line) == false)
            {
                this.m_LineDatas.Add(save_line.Line, save_line);
            }
            else
            {

            }
            this.m_LineDatas[this.m_Line].Begin.GateID = gateui.ID;
            this.m_LineDatas[this.m_Line].Begin.Index = pin.Index;
            this.m_LineDatas[this.m_Line].Begin.Type = pin.Type;

            this.m_LineDatas[this.m_Line].Begin.EndType = CQSaveFile_LinePoint.EndTypes.Start;
            this.canvas.Children.Add(this.m_Line);
            return true;
        }

        void Add_LED(double x=0, double y=0, string id="")
        {
            CQOutput_LedUI cc = null;
            QOutput_LED output_led = null;
            cc = new CQOutput_LedUI();
            cc.GateName = "LED";
            cc.Type = "LED";
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index = 0 });
            output_led = new QOutput_LED();
            output_led.Height = 50;
            output_led.Width = 80;
            cc.ID = id;
            Canvas.SetLeft(output_led, x);
            Canvas.SetTop(output_led, y);
            output_led.DataContext = cc;
            output_led.OnPinMouseDwon += Output_led_OnPinMouseDwon;
            output_led.OnPinMouseUp += Output_led_OnPinMouseUp;
            output_led.OnGateMove += Output_led_OnGateMove;
            this.canvas.Children.Add(output_led);
        }

        private bool Output_led_OnGateMove(QOutput_LED gate)
        {
            this.ChangeLine(gate);
            return true;
        }

        private bool Output_led_OnPinMouseUp(QOutput_LED sender, CQPin pin, Point pt)
        {
            this.ConnectEnd(sender, pin, pt);
            return true;
        }

        void ConnectEnd(FrameworkElement sender, CQPin pin, Point pt)
        {
            CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            this.m_Line.X2 = pin.ConnectPoint.X;
            this.m_Line.Y2 = pin.ConnectPoint.Y;
            this.m_LineDatas[this.m_Line].End.GateID = gateui.ID;
            this.m_LineDatas[this.m_Line].End.Index = pin.Index;
            this.m_LineDatas[this.m_Line].End.Type = pin.Type;
            this.m_LineDatas[this.m_Line].End.EndType = CQSaveFile_LinePoint.EndTypes.End;
            if(this.m_LineDatas[this.m_Line].Begin.Type == CQPin.Types.IN)
            {
                CQSaveFile_LinePoint pp1 = new CQSaveFile_LinePoint(this.m_LineDatas[this.m_Line].End);
                CQSaveFile_LinePoint pp2 = new CQSaveFile_LinePoint(this.m_LineDatas[this.m_Line].Begin);
                this.m_LineDatas[this.m_Line].Begin = pp1;
                this.m_LineDatas[this.m_Line].End = pp2;
            }
            this.m_IsConnect = false;
        }

        private bool Output_led_OnPinMouseDwon(QOutput_LED sender, CQPin pin, Point pt)
        {
            CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            this.m_Line = new Line();
            this.m_Line.Fill = Brushes.Gray;
            this.m_Line.X1 = this.m_Line.X2 = pin.ConnectPoint.X;
            this.m_Line.Y1 = this.m_Line.Y2 = pin.ConnectPoint.Y;
            this.m_Line.Stroke = Brushes.Gray;
            this.m_Line.StrokeThickness = 1;
            this.m_IsConnect = true;
            CQSaveFile_Line save_line = new CQSaveFile_Line() { Line = this.m_Line };
            if (this.m_LineDatas.ContainsKey(save_line.Line) == false)
            {
                this.m_LineDatas.Add(save_line.Line, save_line);
            }
            else
            {

            }
            this.m_LineDatas[this.m_Line].Begin.GateID = gateui.ID;
            this.m_LineDatas[this.m_Line].Begin.Index = pin.Index;
            this.m_LineDatas[this.m_Line].Begin.Type = pin.Type;

            this.m_LineDatas[this.m_Line].Begin.EndType = CQSaveFile_LinePoint.EndTypes.Start;
            this.canvas.Children.Add(this.m_Line);
            return true;
        }

        void Add_NOT(double x=0, double y=0, string id="")
        {
            CQGateUI cc = null;
            QGate ggate = null;
            cc = new CQGateUI();
            cc.GateName = "NOT";
            cc.Type = "NOT";
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index = 0 });
            cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            cc.CreateNOT();
            ggate = new QGate();
            ggate.Height = 50;
            ggate.Width = 80;
            cc.ID = id;
            Canvas.SetLeft(ggate, x);
            Canvas.SetTop(ggate, y);
            ggate.DataContext = cc;
            ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            ggate.OnGateMove += Ggate_OnGateMove;
            this.canvas.Children.Add(ggate);
        }

        void Add_OR(double x = 0, double y = 0, string id = "")
        {
            CQGateUI cc = null;
            QGate ggate = null;
            cc = new CQGateUI();
            cc.GateName = "OR";
            cc.Type = "OR";
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index = 0 });
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index = 1 });
            cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            cc.CreateOR();
            ggate = new QGate();
            ggate.Height = 50;
            ggate.Width = 80;
            cc.ID = id;
            Canvas.SetLeft(ggate, x);
            Canvas.SetTop(ggate, y);
            ggate.DataContext = cc;
            ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            ggate.OnGateMove += Ggate_OnGateMove;
            this.canvas.Children.Add(ggate);
        }

        void Add_AND(double x=0, double y=0, string id="")
        {
            CQGateUI cc = null;
            QGate ggate = null;
            cc = new CQGateUI();
            cc.GateName = "AND";
            cc.Type = "AND";
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index=0 });
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index=1 });
            cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            cc.CreateAND();
            ggate = new QGate();
            ggate.Height = 50;
            ggate.Width = 80;
            cc.ID = id;
            Canvas.SetLeft(ggate, x);
            Canvas.SetTop(ggate, y);
            ggate.DataContext = cc;
            ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            ggate.OnGateMove += Ggate_OnGateMove;
            this.canvas.Children.Add(ggate);
        }

        void ChangeLine(FrameworkElement gate)
        {
            CQGateBaseUI gateui = gate.DataContext as CQGateBaseUI;
            if (gateui != null)
            {
                var vv1 = this.m_LineDatas.Values.Where(x => x.Begin.GateID == gateui.ID);
                if (vv1 != null)
                {
                    foreach(var vv in vv1)
                    {
                        switch (vv.Begin.EndType)
                        {
                            case CQSaveFile_LinePoint.EndTypes.Start:
                                {
                                    switch (vv.Begin.Type)
                                    {
                                        case CQPin.Types.IN:
                                            {
                                                vv.Line.X1 = gateui.Pin_in[vv.Begin.Index].ConnectPoint.X;
                                                vv.Line.Y1 = gateui.Pin_in[vv.Begin.Index].ConnectPoint.Y;
                                            }
                                            break;
                                        case CQPin.Types.OUT:
                                            {
                                                vv.Line.X1 = gateui.Pin_out[vv.Begin.Index].ConnectPoint.X;
                                                vv.Line.Y1 = gateui.Pin_out[vv.Begin.Index].ConnectPoint.Y;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case CQSaveFile_LinePoint.EndTypes.End:
                                {
                                    switch (vv.Begin.Type)
                                    {
                                        case CQPin.Types.IN:
                                            {
                                                vv.Line.X2 = gateui.Pin_in[vv.Begin.Index].ConnectPoint.X;
                                                vv.Line.Y2 = gateui.Pin_in[vv.Begin.Index].ConnectPoint.Y;
                                            }
                                            break;
                                        case CQPin.Types.OUT:
                                            {
                                                vv.Line.X2 = gateui.Pin_out[vv.Begin.Index].ConnectPoint.X;
                                                vv.Line.Y2 = gateui.Pin_out[vv.Begin.Index].ConnectPoint.Y;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
                vv1 = this.m_LineDatas.Values.Where(x => x.End.GateID == gateui.ID);
                if (vv1 != null)
                {
                    foreach(var vv in vv1)
                    {
                        switch (vv.End.EndType)
                        {
                            case CQSaveFile_LinePoint.EndTypes.Start:
                                {
                                    switch (vv.End.Type)
                                    {
                                        case CQPin.Types.IN:
                                            {
                                                vv.Line.X1 = gateui.Pin_in[vv.End.Index].ConnectPoint.X;
                                                vv.Line.Y1 = gateui.Pin_in[vv.End.Index].ConnectPoint.Y;
                                            }
                                            break;
                                        case CQPin.Types.OUT:
                                            {
                                                vv.Line.X1 = gateui.Pin_out[vv.End.Index].ConnectPoint.X;
                                                vv.Line.Y1 = gateui.Pin_out[vv.End.Index].ConnectPoint.Y;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case CQSaveFile_LinePoint.EndTypes.End:
                                {
                                    switch (vv.End.Type)
                                    {
                                        case CQPin.Types.IN:
                                            {
                                                vv.Line.X2 = gateui.Pin_in[vv.End.Index].ConnectPoint.X;
                                                vv.Line.Y2 = gateui.Pin_in[vv.End.Index].ConnectPoint.Y;
                                            }
                                            break;
                                        case CQPin.Types.OUT:
                                            {
                                                vv.Line.X2 = gateui.Pin_out[vv.End.Index].ConnectPoint.X;
                                                vv.Line.Y2 = gateui.Pin_out[vv.End.Index].ConnectPoint.Y;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    
                }

            }
        }

        private bool Ggate_OnGateMove(QGate gate)
        {
            this.ChangeLine(gate);
            return true;
        }

        bool m_IsDrag = false;
        QGate m_DragGate;
        Point m_DragOffset;
        QInput_Switch m_DragInputSwitch;
        QOutput_LED m_DragOutputLED;
        Point m_SelectPt;
        bool m_IsSelect;
        Rectangle m_SelectRect;
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.Source is QGate)
            {
                this.m_DragGate = e.Source as QGate;
                this.m_IsDrag = true;
                this.m_DragOffset = e.GetPosition(this.m_DragGate);
                this.canvas.CaptureMouse();
            }
            else if(e.Source is QInput_Switch)
            {
                this.m_DragInputSwitch = e.Source as QInput_Switch;
                this.m_IsDrag = true;
                this.m_DragOffset = e.GetPosition(this.m_DragInputSwitch);
                this.canvas.CaptureMouse();
            }
            else if(e.Source is QOutput_LED)
            {
                this.m_DragOutputLED = e.Source as QOutput_LED;
                this.m_IsDrag = true;
                this.m_DragOffset = e.GetPosition(this.m_DragOutputLED);
                this.canvas.CaptureMouse();
            }
            else
            {
                this.m_SelectPt = e.GetPosition(this.canvas);
                this.m_SelectRect = new Rectangle();
                //rectangle.Width = 100;
                //rectangle.Height = 100;
                this.m_SelectRect.Stroke = Brushes.Black;
                this.m_SelectRect.StrokeDashArray.Add(6);
                this.m_SelectRect.StrokeDashCap = PenLineCap.Square;
                this.canvas.Children.Add(this.m_SelectRect);
                Canvas.SetLeft(this.m_SelectRect, this.m_SelectPt.X);
                Canvas.SetTop(this.m_SelectRect, this.m_SelectPt.Y);
                this.m_IsSelect = true;
                this.canvas.CaptureMouse();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(this.m_IsDrag == true)
            {
                Point pt = e.GetPosition(this.canvas);
                if(this.m_DragGate != null)
                {
                    Canvas.SetLeft(this.m_DragGate, pt.X - this.m_DragOffset.X);
                    Canvas.SetTop(this.m_DragGate, pt.Y - this.m_DragOffset.Y);
                    this.m_DragGate.RefreshLocation();
                }
                else if(this.m_DragInputSwitch != null)
                {
                    Canvas.SetLeft(this.m_DragInputSwitch, pt.X - this.m_DragOffset.X);
                    Canvas.SetTop(this.m_DragInputSwitch, pt.Y - this.m_DragOffset.Y);
                    this.m_DragInputSwitch.RefreshLocation();
                }
                else if(this.m_DragOutputLED != null)
                {
                    Canvas.SetLeft(this.m_DragOutputLED, pt.X - this.m_DragOffset.X);
                    Canvas.SetTop(this.m_DragOutputLED, pt.Y - this.m_DragOffset.Y);
                    this.m_DragOutputLED.RefreshLocation();
                }
            }
            else if(this.m_IsConnect == true)
            {
                Point pt = e.GetPosition(this.canvas);
                if(pt.X < this.m_Line.X2)
                {
                    this.m_Line.X2 = pt.X + 1;
                }
                else
                {
                    this.m_Line.X2 = pt.X - 1;
                }
                if(pt.Y< this.m_Line.Y2)
                {
                    this.m_Line.Y2 = pt.Y + 1;
                }
                else
                {
                    this.m_Line.Y2 = pt.Y - 1;
                }
                
            }
            else if(this.m_IsSelect == true)
            {
                Point pt = e.GetPosition(this.canvas);
                if(pt.X < this.m_SelectPt.X)
                {
                    Canvas.SetLeft(this.m_SelectRect, pt.X);
                }
                if (pt.Y < this.m_SelectPt.Y)
                {
                    Canvas.SetTop(this.m_SelectRect, pt.Y);
                }
                this.m_SelectRect.Width = Math.Abs(pt.X - this.m_SelectPt.X);
                this.m_SelectRect.Height = Math.Abs(pt.Y - this.m_SelectPt.Y);
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.m_IsDrag == true)
            {
                this.m_IsDrag = false;
                this.canvas.ReleaseMouseCapture();
                this.m_DragGate = null;
                this.m_DragInputSwitch = null;
            }
            else if(this.m_IsSelect == true)
            {
                this.m_IsSelect = false;
                this.canvas.Children.Remove(this.m_SelectRect);
                this.m_SelectRect = null;
                this.canvas.ReleaseMouseCapture();
            }
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            CQSaveFile sv = new CQSaveFile();
            foreach(FrameworkElement child in this.canvas.Children)
            {                
                CQGateBaseUI gateui = child.DataContext as CQGateBaseUI;
                if (gateui != null)
                {
                    CQSaveFile_Gate sg = new CQSaveFile_Gate();
                    sg.ID = gateui.ID;
                    sg.X = Canvas.GetLeft(child);
                    sg.Y = Canvas.GetTop(child);
                    sg.Type = gateui.Type;
                    sv.Gates.Add(sg);
                } 

            }
            sv.Lines.AddRange(this.m_LineDatas.Values);
            MemoryStream snapshot = this.Snapshot();
            sv.SnapshotRaw = snapshot.ToArray();
            XmlSerializer xml = new XmlSerializer(typeof(CQSaveFile));
            string filename = this.textbox_savename.Text + ".txt";
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                xml.Serialize(fs, sv);
            }
        }

        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            this.togglebutton_simulation.IsChecked = false;
            CQSaveFile sv = null;
            if (File.Exists("QQ.txt") == true)
            {
                XmlSerializer xml = new XmlSerializer(typeof(CQSaveFile));
                using (FileStream fs = new FileStream("QQ.txt", FileMode.Open))
                {
                    sv = (CQSaveFile)xml.Deserialize(fs);
                }
            }
            if(sv != null)
            {
                this.ClearGate();
                foreach(CQSaveFile_Gate gate in sv.Gates)
                {
                    switch(gate.Type)
                    {
                        case "AND":
                            {
                                this.Add_AND(gate.X, gate.Y, gate.ID);
                            }
                            break;
                        case "NOT":
                            {
                                this.Add_NOT(gate.X, gate.Y, gate.ID);
                            }
                            break;
                        case "OR":
                            {
                                this.Add_OR(gate.X, gate.Y, gate.ID);
                            }
                            break;
                        case "Switch":
                            {
                                this.Add_Input_Switch(gate.X, gate.Y, gate.ID);
                            }
                            break;
                        case "LED":
                            {
                                this.Add_LED(gate.X, gate.Y, gate.ID);
                            }
                            break;
                    }
                   
                }
                foreach(CQSaveFile_Line line in sv.Lines)
                {
                    line.Line = new Line();
                    line.Line.Fill = Brushes.Gray;
                    //this.m_Line.X1 = this.m_Line.X2 = pin.ConnectPoint.X;
                    //this.m_Line.Y1 = this.m_Line.Y2 = pin.ConnectPoint.Y;
                    line.Line.Stroke = Brushes.Gray;
                    line.Line.StrokeThickness = 1;
                    this.m_LineDatas.Add(line.Line, line);
                    this.canvas.Children.Add(line.Line);
                }
                this.canvas.UpdateLayout();
                foreach(FrameworkElement child in this.canvas.Children)
                {
                    QGate gate = child as QGate;
                    if (gate != null)
                    {
                        gate.RefreshLocation();
                    }
                    QInput_Switch input_switch = child as QInput_Switch;
                    if (input_switch != null)
                    {
                        input_switch.RefreshLocation();
                    }
                    QOutput_LED output_led = child as QOutput_LED;
                    if(output_led != null)
                    {
                        output_led.RefreshLocation();
                    }
                    this.ChangeLine(child);
                }
            }

        }

        public List<CQSimulateData> m_Simulate = new List<CQSimulateData>();

        private void togglebutton_simulation_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton togglebutton = sender as ToggleButton;
            if(togglebutton != null)
            {
                List<QInput_Switch> inputs;
                List<QGate> gates;
                List<QOutput_LED> outputs;
                this.GetGates(out inputs, out gates, out outputs);
                if (togglebutton.IsChecked == true)
                {
                    List<FrameworkElement> gates1 = new List<FrameworkElement>();
                    for(int i=0; i<gates.Count; i++)
                    {
                        gates1.Add(gates[i]);
                    }
                    for (int i = 0; i < outputs.Count; i++)
                    {
                        gates1.Add(outputs[i]);
                    }
                    for (int i=0; i<inputs.Count; i++)
                    {
                        int col = 1;
                        CQSimulateData sud = new CQSimulateData() { GateData = inputs[i].DataContext as CQInput_SwitchUI };
                        sud.GateData.IsSimulate = true;
                        
                        Dictionary< CQSimulateData, CQSimlateEndData> temp_suds = this.FindNexts(sud.GateData.ID, col, gates1);
                        CQSimulateData find_sud = null;
                        if(temp_suds.Count > 0)
                        {
                            this.FindGate(temp_suds.ElementAt(0).Key, this.m_Simulate, out find_sud);
                        }
                        
                        if(find_sud == null)
                        {
                            foreach(var vvv1 in temp_suds)
                            {
                                sud.Nexts.Add(vvv1.Key, vvv1.Value);
                            }
                            //sud.Nexts.AddRange(temp_suds);
                            while (true)
                            {
                                List<CQSimulateData> temp_suds_find = temp_suds.Keys.ToList();

                                col = col + 1;
                                bool isend = true;
                                foreach (CQSimulateData simd in temp_suds_find)
                                {
                                    temp_suds = this.FindNexts(simd.GateData.ID, col, gates1);
                                    if (temp_suds.Count > 0)
                                    {
                                        isend = false;
                                        if (i > 0)
                                        {

                                        }
                                        foreach (var vvv1 in temp_suds)
                                        {
                                            simd.Nexts.Add(vvv1.Key, vvv1.Value);
                                        }
                                        //simd.Nexts.AddRange(temp_suds);
                                    }
                                }
                                if (isend == true)
                                {
                                    this.m_Simulate.Add(sud);

                                    break;
                                }

                            }
                        }
                        else
                        {
                            //find_sud.PinIndex = temp_suds[0].PinIndex;
                            //sud.Nexts.Add(find_sud);
                            foreach (var vvv1 in temp_suds)
                            {
                                sud.Nexts.Add(vvv1.Key, vvv1.Value);
                            }
                            this.m_Simulate.Add(sud);
                            //break;
                        }
                        

                    }
                }
                else
                {
                    this.m_IsStartSimulate = false;
                    foreach(var vv in gates)
                    {
                        CQGateBaseUI gateui = vv.DataContext as CQGateBaseUI;
                        gateui.IsSimulate = false;
                    }
                    foreach (var vv in inputs)
                    {
                        CQGateBaseUI gateui = vv.DataContext as CQGateBaseUI;
                        gateui.IsSimulate = false;
                    }
                    foreach (var vv in outputs)
                    {
                        CQGateBaseUI gateui = vv.DataContext as CQGateBaseUI;
                        gateui.IsSimulate = false;
                    }
                    this.m_Simulate.Clear();
                }
            }
            if(this.m_Simulate.Count > 0)
            {
                if(this.m_Thread_Sim.IsBusy == false)
                {
                    this.m_IsStartSimulate = true;
                    this.m_Thread_Sim.RunWorkerAsync();
                }
            }
        }

        bool FindGate(CQSimulateData sd, List<CQSimulateData> src, out CQSimulateData dst)
        {
            dst = null;
            bool result = false;
            List<CQSimulateData> temp;
            for (int i = 0; i < src.Count; i++)
            {

                temp = src[i].Nexts.Keys.ToList();
                bool isend = false;
                var hr1 = temp.FirstOrDefault(x => x.GateData.ID == sd.GateData.ID);
                if (hr1 != null)
                {
                    dst = hr1;
                    isend = true;
                    break;
                }
                else
                {
                    while (true)
                    {
                        if (temp.All(x => x.Nexts.Count == 0) == true)
                        {
                            isend = true;
                        }
                        else
                        {
                            var hr = temp.SelectMany(x => x.Nexts).FirstOrDefault(x => x.Key.GateData.ID == sd.GateData.ID);
                            if (hr.Key != null)
                            {
                                dst = hr.Key;
                                isend = true;
                                break;
                            }
                            else
                            {
                                temp = temp.Select(x => x.Nexts.Keys.ToList()).ElementAt(0);
                            }
                        }

                        if (isend == true)
                        {
                            break;
                        }
                    }
                }
                
            }
            return result;
        }
        bool m_IsStartSimulate = false;
        Dictionary<CQSimulateData, CQSimlateEndData> FindNexts(string id, int col, List<FrameworkElement> gates)
        {
            Dictionary<CQSimulateData, CQSimlateEndData> suds = new Dictionary<CQSimulateData, CQSimlateEndData>();
            var v1 = this.m_LineDatas.Values.Where(x => x.Begin.GateID == id);
            CQSimulateData sud1;
            foreach (CQSaveFile_Line line in v1)
            {
                CQGateBaseUI gateui;
                FrameworkElement gate;
                this.FineGateFromGateID(line.End.GateID, gates, out gate, out gateui);
                if (gateui != null)
                {
                    sud1 = new CQSimulateData();
                    sud1.Col = col++;
                    //sud1.PinIndex = line.End.Index;
                    sud1.GateData = gateui;
                    sud1.GateData.IsSimulate = true;
                    suds.Add(sud1, new CQSimlateEndData(line.Begin.Index, line.End.Index));
                }

            }
            return suds;
        }

        void FineGateFromGateID(string id, List<FrameworkElement> gates, out FrameworkElement gate, out CQGateBaseUI gateui)
        {
            gate = null;
            gateui = null;
            foreach(FrameworkElement child in gates)
            {
                CQGateBaseUI ui = child.DataContext as CQGateBaseUI;
                if(ui.ID == id)
                {
                    gate = child;
                    gateui = ui;
                    break;
                }
            }
        }


        void GetGates(out List<QInput_Switch> inputs, out List<QGate> gates, out List<QOutput_LED> outputs)
        {
            inputs = new List<QInput_Switch>();
            gates = new List<QGate>();
            outputs = new List<QOutput_LED>();
            foreach(var child in this.canvas.Children)
            {
                if(child is QInput_Switch)
                {
                    inputs.Add(child as QInput_Switch);
                }
                else if(child is QOutput_LED)
                {
                    outputs.Add(child as QOutput_LED);
                }
                else if(child is QGate)
                {
                    gates.Add(child as QGate);
                }
            }
        }

        private void button_addgate_Click(object sender, RoutedEventArgs e)
        {
            string gatetype = this.combobox_gate.SelectionBoxItem as string;
            switch(gatetype.ToUpperInvariant())
            {
                case "NOT":
                    {
                        this.Add_NOT();
                    }
                    break;
                case "AND":
                    {
                        this.Add_AND();
                    }
                    break;
                case "OR":
                    {
                        this.Add_OR();
                    }
                    break;
                case "SWITCH":
                    {
                        this.Add_Input_Switch();
                    }
                    break;
                case "LED":
                    {
                        this.Add_LED();
                    }
                    break;
            }
        }

        void ClearGate()
        {
            this.canvas.Children.Clear();
            this.m_LineDatas.Clear();
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            this.ClearGate();
        }

        MemoryStream Snapshot()
        {
            MemoryStream mm = new MemoryStream();
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)this.canvas.ActualWidth, (int)this.canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(this.canvas);

            PngBitmapEncoder image = new PngBitmapEncoder();
            image.Frames.Add(BitmapFrame.Create(bitmap));
            image.Save(mm);
            return mm;
        }
    }
}
