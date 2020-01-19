using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Utility.DateTimeFormats;
using Xamarin.Forms;

namespace NoorCRM.Client.Sources
{
    public class PriceCommaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                string priceStr = value.ToString();
                int insertedCommaCount = 0;
                while (true)
                {
                    int index = (priceStr.Length -insertedCommaCount) - (++insertedCommaCount * 3);
                    if (index >= 1)
                        priceStr = priceStr.Insert(index, ",");
                    else
                        break;
                }
                return priceStr;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                string date = ((DateTime)value).GetDateTimeString(DateOrTime.DateTime,
                    (int)DateTimeFormat.yyyymmddHHmmss, isPersian: true);
                return (date);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
