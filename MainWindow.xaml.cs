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
        Dictionary<Line, CQSaveFile_Line> m_LineData = new Dictionary<Line, CQSaveFile_Line>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CQGateUI cc = null;
            //QGate ggate = null;
            //cc = new CQGateUI();
            //cc.GateName = "AND";
            //cc.Pin_in.Add(new CQPin() { Type= CQPin.Types.IN});
            //cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN });
            //cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            //ggate = new QGate();
            //ggate.Height = 50;
            //ggate.Width = 80;
            //ggate.DataContext = cc;
            //ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            //ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            //this.canvas.Children.Add(ggate);

            //cc = new CQGateUI();
            //cc.GateName = "AND";
            //cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN });
            //cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN });
            //cc.Pin_out.Add(new CQPin() { Type = CQPin.Types.OUT });
            //ggate = new QGate();
            //ggate.Height = 50;
            //ggate.Width = 80;
            //ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            //ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            //ggate.DataContext = cc;
            //this.canvas.Children.Add(ggate);
            this.Add_AND(0, 0, "");
            this.Add_AND(0, 0, "");
        }
        bool m_IsConnect;
        Line m_Line;
        private bool Ggate_OnPinMouseUp(QGate sender, CQPin pin, Point pt)
        {
            this.m_Line.X2 = pt.X;
            this.m_Line.Y2 = pt.Y;
            this.m_LineData[this.m_Line].End.GateID = sender.ID;
            this.m_LineData[this.m_Line].End.Index = pin.Index;
            this.m_LineData[this.m_Line].End.Type = pin.Type;
            this.m_Line = null;
            this.m_IsConnect = false;
            return true;
        }

        private bool Ggate_OnPinMouseDwon(QGate sender, CQPin pin, Point pt)
        {
            this.m_Line = new Line();
            this.m_Line.Fill = Brushes.Gray;
            this.m_Line.X1 = this.m_Line.X2 = pt.X;
            this.m_Line.Y1 = this.m_Line.Y2 = pt.Y;
            this.m_Line.Stroke = Brushes.Gray;
            this.m_Line.StrokeThickness = 1;
            this.m_IsConnect = true;
            if(this.m_LineData.ContainsKey(this.m_Line) == false)
            {
                this.m_LineData.Add(this.m_Line, new CQSaveFile_Line());
            }
            else
            {

            }
            this.m_LineData[this.m_Line].Begin.GateID = sender.ID;
            this.m_LineData[this.m_Line].Begin.Index = pin.Index;
            this.m_LineData[this.m_Line].Begin.Type = pin.Type;
            this.canvas.Children.Add(this.m_Line);
            return true;
        }

        void Add_AND(double x, double y, string id)
        {
            CQGateUI cc = null;
            QGate ggate = null;
            cc = new CQGateUI();
            cc.GateName = "AND";
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN });
            cc.Pin_in.Add(new CQPin() { Type = CQPin.Types.IN });
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
            this.canvas.Children.Add(ggate);
            
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
            sv.Lines.AddRange(this.m_LineData.Values);
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
                this.m_LineData.Clear();
                foreach(CQSaveFile_Gate gate in sv.Gates)
                {
                    this.Add_AND(gate.X, gate.Y, gate.ID);
                }
            }
        }
    }
}
