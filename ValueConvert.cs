using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_LogicSimulation
{
    public class CQBoolVisibility : IValueConverter
    {
        public Visibility True { set; get; }
        public Visibility False { set; get; }
        public CQBoolVisibility()
        {
            this.True = Visibility.Visible;
            this.False = Visibility.Collapsed;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bb = (bool)value;
            return bb == true ? this.True : this.False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CQBool2Brush:IValueConverter
    {
        public Brush True { set; get; }
        public Brush False { set; get; }
        public CQBool2Brush()
        {
            this.True = Brushes.Green;
            this.False = Brushes.Red;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bb = (bool)value;
            return bb == true ? this.True : this.False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
