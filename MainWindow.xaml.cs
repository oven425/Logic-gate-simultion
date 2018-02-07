using System;
using System.Collections.Generic;
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

namespace WPF_LogicSimulation
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CQGateUI cc = null;
            QGate ggate = null;
            cc = new CQGateUI();
            cc.GateName = "AND";
            cc.Pin_in.Add(new CQPin());
            cc.Pin_in.Add(new CQPin());
            cc.Pin_out.Add(new CQPin());
            ggate = new QGate();
            ggate.Height = 50;
            ggate.Width = 80;
            ggate.DataContext = cc;
            ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            this.canvas.Children.Add(ggate);

            cc = new CQGateUI();
            cc.GateName = "AND";
            cc.Pin_in.Add(new CQPin());
            cc.Pin_in.Add(new CQPin());
            cc.Pin_out.Add(new CQPin());
            ggate = new QGate();
            ggate.Height = 50;
            ggate.Width = 80;
            ggate.OnPinMouseDwon += Ggate_OnPinMouseDwon;
            ggate.OnPinMouseUp += Ggate_OnPinMouseUp;
            ggate.DataContext = cc;
            this.canvas.Children.Add(ggate);
        }
        bool m_IsConnect;
        Line m_Line;
        private bool Ggate_OnPinMouseUp(CQPin pin, Point pt)
        {
            this.m_Line.X2 = pt.X;
            this.m_Line.Y2 = pt.Y;
            this.m_Line = null;
            this.m_IsConnect = false;
            return true;
        }

        private bool Ggate_OnPinMouseDwon(CQPin pin, Point pt)
        {
            this.m_Line = new Line();
            this.m_Line.Fill = Brushes.Gray;
            this.m_Line.X1 = this.m_Line.X2 = pt.X;
            this.m_Line.Y1 = this.m_Line.Y2 = pt.Y;
            this.m_Line.Stroke = Brushes.Gray;
            this.m_Line.StrokeThickness = 1;
            this.m_IsConnect = true;
            this.canvas.Children.Add(this.m_Line);
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
            }
            else if(this.m_IsConnect == true)
            {
                Point pt = e.GetPosition(this.canvas);
                this.m_Line.X2 = pt.X-1;
                this.m_Line.Y2 = pt.Y-1;
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
    }
}
