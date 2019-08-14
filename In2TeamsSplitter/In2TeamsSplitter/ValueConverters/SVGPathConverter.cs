using System;
using System.Globalization;
using Xamarin.Forms;

namespace In2TeamsSplitter.ValueConverters
{
    public class SVGPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetSVGName((int)(value ?? 0), int.Parse(parameter as string ?? "0"));

        string GetSVGName(int status, int parameter)
        {
            if (status >= parameter)
                return "resource://In2TeamsSplitter.Resources.starFilled.svg";
            else
                return "resource://In2TeamsSplitter.Resources.starEmpty.svg";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
