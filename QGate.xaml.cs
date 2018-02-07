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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WPF_LogicSimulation
{
    /// <summary>
    /// QGate.xaml 的互動邏輯
    /// </summary>
    public partial class QGate : UserControl
    {
        public delegate bool PinMouseDwonDelegate(CQPin pin, Point pt);
        public event PinMouseDwonDelegate OnPinMouseDwon;
        public delegate bool PinMouseUpDelegate(CQPin pin, Point pt);
        public event PinMouseUpDelegate OnPinMouseUp;
        public QGate()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            CQPin ping = rectangle.DataContext as CQPin;
            UIElement container = VisualTreeHelper.GetParent(this) as UIElement;
            Point relativeLocation = rectangle.TranslatePoint(new Point(0, 0), container);
            relativeLocation.X = relativeLocation.X + rectangle.Width;
            relativeLocation.Y = relativeLocation.Y + rectangle.Height / 2;
            if (this.OnPinMouseDwon != null)
            {
                this.OnPinMouseDwon(ping, relativeLocation);
            }
            e.Handled = true;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            CQPin ping = rectangle.DataContext as CQPin;
            UIElement container = VisualTreeHelper.GetParent(this) as UIElement;
            Point relativeLocation = rectangle.TranslatePoint(new Point(0, 0), container);
            relativeLocation.X = relativeLocation.X + rectangle.Width / 2;
            relativeLocation.Y = relativeLocation.Y + rectangle.Height / 2;
            if (this.OnPinMouseUp != null)
            {
                this.OnPinMouseUp(ping, relativeLocation);
            }
            e.Handled = true;
        }

        private void Rectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }

    public class CQGateUI : INotifyPropertyChanged
    {
        public ObservableCollection<CQPin> Pin_in { set; get; }
        public ObservableCollection<CQPin> Pin_out { set; get; }
        public string GateName { set; get; }
        public CQGateUI()
        {
            this.Pin_in = new ObservableCollection<CQPin>();
            this.Pin_out = new ObservableCollection<CQPin>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void Update(string name)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class CQPin
    {
        public enum Types
        {
            IN,
            OUT
        }
        public Types Type { set; get; }
        public int Index { set; get; }
    }
}
