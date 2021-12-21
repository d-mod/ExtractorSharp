using System;
using System.Globalization;
using System.Windows.Data;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp {
    internal class ColorFormatsBoolConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var s = (ColorFormats)value;
            return s == (ColorFormats)int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var isChecked = (bool)value;
            if(!isChecked) {
                return null;
            }
            return (ColorFormats)int.Parse(parameter.ToString());
        }
    }
}
