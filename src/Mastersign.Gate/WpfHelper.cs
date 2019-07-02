using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Mastersign.Gate
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public System.Windows.Visibility Unvisible { get; set; }

        public BoolToVisibilityConverter()
        {
            Unvisible = System.Windows.Visibility.Hidden;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() != typeof(bool)) return value;
            var boolValue = (bool)value;
            if (Invert) boolValue = !boolValue;
            return boolValue
                ? System.Windows.Visibility.Visible
                : Unvisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.GetType() != typeof(System.Windows.Visibility)) return value;
            var boolValue = ((System.Windows.Visibility)value) == System.Windows.Visibility.Visible;
            if (Invert) boolValue = !boolValue;
            return boolValue;
        }
    }

    public class NegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;
            if (value.GetType() != typeof(bool)) return value;
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;
            if (value.GetType() != typeof(bool)) return value;
            return !(bool)value;
        }
    }
}
