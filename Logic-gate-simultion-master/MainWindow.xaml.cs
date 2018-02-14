using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WPF_LogicSimulation
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<Line, CQSaveFile_Line> m_LineDatas = new Dictionary<Line, CQSaveFile_Line>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Add_AND(0, 0, "");
            this.Add_AND(0, 0, "");
            this.Add_AND(0, 0, "");
        }
        bool m_IsConnect;
        Line m_Line;
        private bool Ggate_OnPinMouseUp(QGate sender, CQPin pin, Point pt)
        {
            this.m_Line.X2 = pin.ConnectPoint.X;
            this.m_Line.Y2 = pin.ConnectPoint.Y;
            this.m_LineDatas[this.m_Line].End.GateID = sender.ID;
            this.m_LineDatas[this.m_Line].End.Index = pin.Index;
            this.m_LineDatas[this.m_Line].End.Type = pin.Type;
            this.m_LineDatas[this.m_Line].End.EndType = CQSaveFile_LinePoint.EndTypes.End;
            //this.m_Line = null;
            this.m_IsConnect = false;
            return true;
        }

        private bool Ggate_OnPinMouseDwon(QGate sender, CQPin pin, Point pt)
        {
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
            this.m_LineDatas[this.m_Line].Begin.GateID = sender.ID;
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
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index=0 });
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN, Index=1 });
            cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            ggate = new QGate();
            ggate.Height = 50;
            ggate.Width = 80;
            ggate.ID = id;
            Canvas.SetLeft(ggate, x);
            Canvas.SetTop(ggate, y);
            ggate.DataContext = cc;
            ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            ggate.OnGateMove += Ggate_OnGateMove;
            this.canvas.Children.Add(ggate);
            
        }

        void ChangeLine(QGate gate)
        {
            CQGateUI gateui = gate.DataContext as CQGateUI;
            if (gateui != null)
            {
                var vv1 = this.m_LineDatas.Values.Where(x => x.Begin.GateID == gate.ID);
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
                vv1 = this.m_LineDatas.Values.Where(x => x.End.GateID == gate.ID);
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
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.Source is QGate)
            {
                this.m_DragGate = e.Source as QGate;
                this.m_IsDrag = true;
                this.m_DragOffset = e.GetPosition(this.m_DragGate);
                this.canvas.CaptureMouse();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(this.m_IsDrag == true)
            {
                Point pt = e.GetPosition(this.canvas);
                Canvas.SetLeft(this.m_DragGate, pt.X - this.m_DragOffset.X);
                Canvas.SetTop(this.m_DragGate, pt.Y - this.m_DragOffset.Y);
                this.m_DragGate.RefreshLocation();
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
            }
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            CQSaveFile sv = new CQSaveFile();
            foreach(FrameworkElement child in this.canvas.Children)
            {
                
                QGate gate = child as QGate;
                if(gate != null)
                {
                    CQSaveFile_Gate sg = new CQSaveFile_Gate();
                    sg.ID = gate.ID;
                    sg.X = Canvas.GetLeft(gate);
                    sg.Y= Canvas.GetTop(gate);
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
                    this.Add_AND(gate.X, gate.Y, gate.ID);
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
                foreach(var child in this.canvas.Children)
                {
                    if(child is QGate)
                    {
                        QGate gate = child as QGate;
                        gate.RefreshLocation();
                        this.ChangeLine(child as QGate);
                    }
                }
            }

        }
    }
}
