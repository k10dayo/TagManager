using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TagManager.Views.Converter
{
    public class NaviWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue)
            {
                //return isTrue ? "0.2*" : "0"; // IsTrueがTrueなら0.2*、Falseなら0
                return isTrue ? new GridLength(0.2, GridUnitType.Star) : new GridLength(0);
            }
            //return "0"; // デフォルト値
            return new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
