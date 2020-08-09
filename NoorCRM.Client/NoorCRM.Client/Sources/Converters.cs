using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Utility.DateTimeFormats;
using Xamarin.Forms;

namespace NoorCRM.Client.Sources
{
    public class DoubleToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            if (value is double)
            {
                double v = (double)value;
                if (v > 0) return true;
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemSelectedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            if (value is string && string.IsNullOrWhiteSpace((string)value)) return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
                    int index = (priceStr.Length - insertedCommaCount) - (++insertedCommaCount * 3);
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
            if (value != null && value is DateTime)
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

    public class DateTimeConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is DateTime)
            {
                string date = ((DateTime)value).GetDateTimeString(DateOrTime.Date,
                    (int)DateFormat.yymmdd, isPersian: true);
                string time = ((DateTime)value).GetDateTimeString(DateOrTime.Time,
                    (int)TimeFormat.HHmm, isPersian: true);
                string day = ((DateTime)value).GetDateTimeString(DateOrTime.WeekDay,
                    0, isPersian: true);
                return $"{day} - {time} - {date}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is DateTime)
            {
                string date = ((DateTime)value).GetDateTimeString(DateOrTime.Date,
                    (int)DateFormat.yyyymmdd, isPersian: true);
                string day = ((DateTime)value).GetDateTimeString(DateOrTime.WeekDay,
                    0, isPersian: true);
                return $"{day} - {date}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateConverterNoWeekday : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is DateTime)
            {
                return ((DateTime)value).GetDateTimeString(DateOrTime.Date,
                    (int)DateFormat.yyyymmdd, isPersian: true);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is DateTime)
            {
                string time = ((DateTime)value).GetDateTimeString(DateOrTime.Time,
                    (int)TimeFormat.HHmm, isPersian: true);
                return $"{time}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TodayTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is DateTime)
            {
                TimeSpan dif = ((DateTime)value).Date - (DateTime.Today);
                if (dif.TotalDays == 1)
                    return $"فردا";
                if (dif.TotalDays > 0)
                    return $"{dif.TotalDays} روز بعد";
                if (dif.TotalDays == 0)
                {
                    string time = ((DateTime)value).GetDateTimeString(DateOrTime.Time,
                        (int)TimeFormat.HHmm, isPersian: true);
                    return $"{time}";
                }
                if (dif.TotalDays == 1)
                    return $"دیروز";
                if (dif.TotalDays < 0)
                    return $"{-dif.TotalDays} روز قبل";
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
