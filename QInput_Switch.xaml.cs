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
    /// QInput_Switch.xaml 的互動邏輯
    /// </summary>
    public partial class QInput_Switch : UserControl
    {
        public delegate bool PinMouseDwonDelegate(QInput_Switch sender, CQPin pin, Point pt);
        public event PinMouseDwonDelegate OnPinMouseDwon;
        public delegate bool PinMouseUpDelegate(QInput_Switch sender, CQPin pin, Point pt);
        public event PinMouseUpDelegate OnPinMouseUp;
        public delegate bool GateMoveDelegate(QInput_Switch gate);
        public event GateMoveDelegate OnGateMove;
        public QInput_Switch()
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

            for (int i = 0; i < this.itemscontrol_out.Items.Count; i++)
            {
                ContentPresenter container = this.itemscontrol_out.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                container.ApplyTemplate();
                FrameworkElement oo = container.ContentTemplate.FindName("rectangle", container) as FrameworkElement;
                CQPin pin = oo.DataContext as CQPin;
                this.GetPinLocation(pin, oo);
                //pin_out.Add(pin);
            }
            if (this.OnGateMove != null)
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

        private void toggglebutton_switch_Click(object sender, RoutedEventArgs e)
        {
            this.textblock_state.Text = this.toggglebutton_switch.IsChecked == true ? "On" : "False";
        }

        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

    public class CQInput_SwitchUI :CQGateBaseUI
    {
        public CQInput_SwitchUI()
        {
            this.Pin_out = new ObservableCollection<CQPin>();
        }
    }
}
