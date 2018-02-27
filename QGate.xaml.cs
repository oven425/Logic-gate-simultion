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
using System.Windows.Controls.Primitives;

namespace WPF_LogicSimulation
{
    /// <summary>
    /// QGate.xaml 的互動邏輯
    /// </summary>
    public partial class QGate : UserControl
    {
        public delegate bool PinMouseDwonDelegate(QGate sender, CQPin pin, Point pt);
        public event PinMouseDwonDelegate OnPinMouseDwon;
        public delegate bool PinMouseUpDelegate(QGate sender, CQPin pin, Point pt);
        public event PinMouseUpDelegate OnPinMouseUp;
        public delegate bool GateMoveDelegate(QGate gate);
        public event GateMoveDelegate OnGateMove;
        
        public QGate()
        {
            InitializeComponent();
            
        }

        DependencyObject findElementInItemsControlItemAtIndex(ItemsControl itemsControl, int itemOfIndexToFind, string nameOfControlToFind)
        {
            if (itemOfIndexToFind >= itemsControl.Items.Count) return null;

            DependencyObject depObj = null;
            object o = itemsControl.Items[itemOfIndexToFind];
            if (o != null)
            {
                var item = itemsControl.ItemContainerGenerator.ContainerFromItem(o);
                if (item != null)
                {
                    //GridViewItem it = item as GridViewItem;
                    //var i = it.FindName(nameOfControlToFind);
                    depObj = getVisualTreeChild(item, nameOfControlToFind);
                    return depObj;
                }
            }
            return null;
        }

        DependencyObject getVisualTreeChild(DependencyObject obj, String name)
        {
            DependencyObject dependencyObject = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childrenCount; i++)
            {
                var oChild = VisualTreeHelper.GetChild(obj, i);
                var childElement = oChild as FrameworkElement;
                if (childElement != null)
                {
                    //Code to take care of Paragraph/Run
                    if (childElement is Rectangle || childElement is TextBlock)
                    {
                        dependencyObject = childElement.FindName(name) as DependencyObject;
                        if (dependencyObject != null)
                            return dependencyObject;
                    }

                    if (childElement.Name == name)
                    {
                        return childElement;
                    }
                }
                dependencyObject = getVisualTreeChild(oChild, name);
                if (dependencyObject != null)
                    return dependencyObject;
            }
            return dependencyObject;
        }

        public bool RefreshLocation()
        {
            bool result = true;
            //List<CQPin> pin_in = new List<CQPin>();
            //List<CQPin> pin_out = new List<CQPin>();
            for (int i=0; i<this.itemscontrol_in.Items.Count; i++)
            {
                ContentPresenter container = this.itemscontrol_in.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                container.ApplyTemplate();
                FrameworkElement oo = container.ContentTemplate.FindName("rectangle", container) as FrameworkElement;
                CQPin pin = oo.DataContext as CQPin;
                this.GetPinLocation(pin, oo);
                //pin_in.Add(pin);
            }
            for (int i = 0; i < this.itemscontrol_out.Items.Count; i++)
            {
                ContentPresenter container = this.itemscontrol_out.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                container.ApplyTemplate();
                FrameworkElement oo = container.ContentTemplate.FindName("rectangle", container) as FrameworkElement;
                CQPin pin = oo.DataContext as CQPin;
                this.GetPinLocation(pin, oo);
                //pin_out.Add(pin);
            }
            if(this.OnGateMove != null)
            {
                

                this.OnGateMove(this);
            }
            return result;
        }

        Point GetPinLocation(CQPin pin, FrameworkElement rectangle)
        {
            UIElement container = VisualTreeHelper.GetParent(this) as UIElement;
            Point relativeLocation = rectangle.TranslatePoint(new Point(0, 0), container);
            if (pin.Type == CQPin.Types.IN)
            {
                relativeLocation.X = relativeLocation.X;
            }
            else
            {
                relativeLocation.X = relativeLocation.X + rectangle.Width;
            }
            relativeLocation.Y = relativeLocation.Y + rectangle.Height / 2;
            pin.ConnectPoint = relativeLocation;
            return relativeLocation;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            CQPin pin = rectangle.DataContext as CQPin;
            this.GetPinLocation(pin, rectangle);
            if (this.OnPinMouseDwon != null)
            {
                this.OnPinMouseDwon(this, pin, new Point());
            }
            e.Handled = true;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement rectangle = sender as FrameworkElement;
            CQPin pin = rectangle.DataContext as CQPin;
            this.GetPinLocation(pin, rectangle);
            if (this.OnPinMouseUp != null)
            {
                this.OnPinMouseUp(this, pin, new Point());
            }
            e.Handled = true;
        }
    }

    public class CQGateBaseUI : INotifyPropertyChanged
    {
        public string Type { set; get; }
        public ObservableCollection<CQPin> Pin_in { set; get; }
        public ObservableCollection<CQPin> Pin_out { set; get; }
        public string GateName { set; get; }
        bool m_IsSimulate;
        string m_ID;
        bool m_IsSelected;
        public CQGateBaseUI()
        {
            this.m_IsSelected = false;
        }
        public string ID
        {
            set { this.m_ID = value; }
            get { if (string.IsNullOrEmpty(this.m_ID) == true) { this.m_ID = Guid.NewGuid().ToString(); } return this.m_ID; }
        }
        public bool IsSimulate { set { this.m_IsSimulate = value; this.Update("IsSimulate"); } get { return this.m_IsSimulate; } }
        public bool IsSelected { set { this.m_IsSelected = value; this.Update("IsSelected"); } get { return this.m_IsSelected; } }
        public virtual bool Process() { return true; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Update(string name) { if (this.PropertyChanged != null) { this.PropertyChanged(this, new PropertyChangedEventArgs(name)); } }
        public override string ToString()
        {
            return string.Format("Type:{0} ID:{1}"
                , this.Type
                , this.ID);
        }
    }

    public class CQGateUI : CQGateBaseUI
    {
        public CQGateUI()
        {
            this.Pin_in = new ObservableCollection<CQPin>();
            this.Pin_out = new ObservableCollection<CQPin>();
            this.Logic = new Dictionary<string, string>();
            
        }

        public void CreateNOT()
        {
            this.Logic.Add("0", "1");
            this.Logic.Add("1", "0");
        }

        public void CreateAND()
        {
            this.Logic.Add("00", "0");
            this.Logic.Add("01", "0");
            this.Logic.Add("10", "0");
            this.Logic.Add("11", "1");
        }

        public void CreateOR()
        {
            this.Logic.Add("00", "0");
            this.Logic.Add("01", "1");
            this.Logic.Add("10", "1");
            this.Logic.Add("11", "1");
        }

        public Dictionary<string, string> Logic { set; get; }

        public override bool Process()
        {
            string str_in = "";
            for(int i= 0; i<this.Pin_in.Count; i++)
            {
                str_in = str_in + (this.Pin_in[i].IsTrue == true ? "1" : "0");
            }
            string str_out = this.Logic[str_in];
            for(int i=0; i<str_out.Length; i++)
            {
                this.Pin_out[i].IsTrue = (str_out[i] == '1');
            }
            return true;
        }
    }

    public class CQPin:INotifyPropertyChanged
    {
        public enum Types
        {
            IN,
            OUT
        }
        public Types Type { set; get; }
        public int Index { set; get; }
        public Point ConnectPoint { set; get; }
        bool m_IsEnableSimulate;
        bool m_IsTrue;
        public bool IsTrue { set { this.m_IsTrue = value; this.Update("IsTrue"); } get { return this.m_IsTrue; } }
        public bool IsEnableSimulate { set { this.m_IsEnableSimulate = value; this.Update("IsEnableSimulate"); } get { return this.m_IsEnableSimulate; } }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Update(string name) { if (this.PropertyChanged != null) { this.PropertyChanged(this, new PropertyChangedEventArgs(name)); } }
        public override string ToString()
        {
            return string.Format("Index:{0} Index:{1} IsTrue:{2} ConnectPoint:{3}"
                , this.Type
                , this.Index
                , this.IsTrue
                , this.ConnectPoint);
        }
    }
}
