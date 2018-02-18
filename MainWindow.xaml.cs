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
            while(true)
            {
                try
                {
                    
                }
                catch(Exception ee)
                {
                    System.Diagnostics.Trace.WriteLine(ee.Message);
                    System.Diagnostics.Trace.WriteLine(ee.StackTrace);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Add_AND(0, 0, "");
            //this.Add_AND(0, 0, "");
            this.Add_AND(0, 0, "");
            this.Add_Input_Switch(0, 0, "");
            this.Add_Input_Switch(0, 0, "");
            this.Add_LED(0, 0, "");
        }
        bool m_IsConnect;
        Line m_Line;
        private bool Ggate_OnPinMouseUp(QGate sender, CQPin pin, Point pt)
        {
            //CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            //this.m_Line.X2 = pin.ConnectPoint.X;
            //this.m_Line.Y2 = pin.ConnectPoint.Y;
            //this.m_LineDatas[this.m_Line].End.GateID = gateui.ID;
            //this.m_LineDatas[this.m_Line].End.Index = pin.Index;
            //this.m_LineDatas[this.m_Line].End.Type = pin.Type;
            //this.m_LineDatas[this.m_Line].End.EndType = CQSaveFile_LinePoint.EndTypes.End;
            //this.m_IsConnect = false;
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

        void Add_Input_Switch(double x, double y, string id)
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
            //CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            //this.m_Line.X2 = pin.ConnectPoint.X;
            //this.m_Line.Y2 = pin.ConnectPoint.Y;
            //this.m_LineDatas[this.m_Line].End.GateID = gateui.ID;
            //this.m_LineDatas[this.m_Line].End.Index = pin.Index;
            //this.m_LineDatas[this.m_Line].End.Type = pin.Type;
            //this.m_LineDatas[this.m_Line].End.EndType = CQSaveFile_LinePoint.EndTypes.End;
            //this.m_IsConnect = false;
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

        void Add_LED(double x, double y, string id)
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
            //CQGateBaseUI gateui = sender.DataContext as CQGateBaseUI;
            //this.m_Line.X2 = pin.ConnectPoint.X;
            //this.m_Line.Y2 = pin.ConnectPoint.Y;
            //this.m_LineDatas[this.m_Line].End.GateID = gateui.ID;
            //this.m_LineDatas[this.m_Line].End.Index = pin.Index;
            //this.m_LineDatas[this.m_Line].End.Type = pin.Type;
            //this.m_LineDatas[this.m_Line].End.EndType = CQSaveFile_LinePoint.EndTypes.End;
            //this.m_IsConnect = false;
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

        void Add_AND(double x, double y, string id)
        {
            CQGateUI cc = null;
            QGate ggate = null;
            cc = new CQGateUI();
            cc.GateName = "AND";
            cc.Type = "AND";
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index=0 });
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index=1 });
            cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
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
            XmlSerializer xml = new XmlSerializer(typeof(CQSaveFile));
            using (FileStream fs = new FileStream("QQ.txt", FileMode.Create))
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
                this.canvas.Children.Clear();
                this.m_LineDatas.Clear();
                foreach(CQSaveFile_Gate gate in sv.Gates)
                {
                    switch(gate.Type)
                    {
                        case "AND":
                            {
                                this.Add_AND(gate.X, gate.Y, gate.ID);
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
                if(togglebutton.IsChecked == true)
                {
                    List<QInput_Switch> inputs;
                    List<QGate> gates;
                    List<QOutput_LED> outputs;
                    
                    this.GetGates(out inputs, out gates, out outputs);
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
                        CQSimulateData sud = new CQSimulateData() { GateData = inputs[i].DataContext as CQInput_SwitchUI };
                        
                        CQInput_SwitchUI input_ui = inputs[i].DataContext as CQInput_SwitchUI;
                        var v1 = this.m_LineDatas.Values.Where(x => x.Begin.GateID == input_ui.ID);
                        foreach(CQSaveFile_Line line in v1)
                        {
                            this.FineGateFromGateID(line.End.GateID, gates1);
                        }
                    }
                }
            }
        }

        void FineGateFromGateID(string id, List<FrameworkElement> gates)
        {
            foreach(FrameworkElement child in gates)
            {
                CQGateBaseUI ui = child.DataContext as CQGateBaseUI;
                if(ui != null)
                {

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
    }
}
