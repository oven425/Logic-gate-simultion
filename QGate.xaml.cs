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
        public string ID
        {
            set { this.m_ID = value; }
            get
            {
                if(string.IsNullOrEmpty(this.m_ID) == true)
                {
                    this.m_ID = Guid.NewGuid().ToString();
                }
                return this.m_ID;
            }
        }
        string m_ID;
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
            for(int i=0; i<this.itemscontrol_in.Items.Count; i++)
            {
                UIElement el = (UIElement)this.itemscontrol_in.ItemContainerGenerator.ContainerFromIndex(i);
            }

            return result;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            CQPin ping = rectangle.DataContext as CQPin;
            UIElement container = VisualTreeHelper.GetParent(this) as UIElement;
            Point relativeLocation = rectangle.TranslatePoint(new Point(0, 0), container);
            //relativeLocation.X = relativeLocation.X + rectangle.Width;
            if (ping.Type == CQPin.Types.IN)
            {
                relativeLocation.X = relativeLocation.X;
            }
            else
            {
                relativeLocation.X = relativeLocation.X+ rectangle.Width;
            }
            relativeLocation.Y = relativeLocation.Y + rectangle.Height / 2;
            if (this.OnPinMouseDwon != null)
            {
                this.OnPinMouseDwon(this, ping, relativeLocation);
            }
            e.Handled = true;
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement rectangle = sender as FrameworkElement;
            CQPin ping = rectangle.DataContext as CQPin;

            UIElement container = VisualTreeHelper.GetParent(this) as UIElement;
            Point relativeLocation = rectangle.TranslatePoint(new Point(0, 0), container);
            if(ping.Type == CQPin.Types.IN)
            {
                relativeLocation.X = relativeLocation.X;
            }
            else
            {
                relativeLocation.X = relativeLocation.X + rectangle.Width;
            }
            
            relativeLocation.Y = relativeLocation.Y + rectangle.Height / 2;
            if (this.OnPinMouseUp != null)
            {
                this.OnPinMouseUp(this, ping, relativeLocation);
            }
            e.Handled = true;
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
