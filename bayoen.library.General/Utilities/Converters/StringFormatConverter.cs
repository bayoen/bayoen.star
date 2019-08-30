using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

//namespace bayoen.library.General.Utilities.Converters
//{
//    public class StringFormatConverter : IMultiValueConverter
//    {
//        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
//        {
//            string token = values[0] as string;

//            if (token == null) return string.Empty;
//            if (token == DependencyProperty.UnsetValue) return string.Empty;            

//            for (int seedIndex = 0; seedIndex < (values.Length - 1) / 2; seedIndex++)
//            {
//                token = token.Replace(values[2 * seedIndex] as string, values[2 * seedIndex + 1] as string);
//            }

//            return token;
//        }

//        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
