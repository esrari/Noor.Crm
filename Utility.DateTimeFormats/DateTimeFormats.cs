using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Utility.DateTimeFormats
{
	public static class DateTimeFormats
	{

		public static string GetDateTimeString(this DateTime dt, DateOrTime type, int format,
			bool isPersian, string dateDelimeter = "/", string timeDelimeter = ":",
			string miliSecondDelimeter = ".", string betweenDateTimeDelimeter = " ")
		{
			return GetDateTimeString(type, format, dt, isPersian, dateDelimeter,
				timeDelimeter, miliSecondDelimeter, betweenDateTimeDelimeter);
		}


		/// <summary>
		/// ساخت رشته ای از تاریخ و زمان که در زمان تولید گزارش در سلول های انتخاب شده قرار می گیرند
		/// </summary>
		/// <param name="type">نوع که می تواند تاریخ، زمان، تاریخ و زمان، سال، ماه، روز، ساعت، دقیقه و ثانیه باشد</param>
		/// <param name="format">هر کدام از نوع های فوق چند فرمت نوشتن دارند مانند تعداد کاراکتر های سال که می تواند 2 رقمی و 4 رقمی باشد</param>
		/// <param name="dt">تاریخ میلادی برای درج</param>
		/// <param name="isPersian">آیا خروجی تاریخ شمسی باشد</param>
		/// <returns>رشته تولید شده</returns>
		public static string GetDateTimeString(DateOrTime type, int format, DateTime dt,
			bool isPersian, string dateDelimeter = "/", string timeDelimeter = ":",
			string miliSecondDelimeter = ".", string betweenDateTimeDelimeter = " ")
		{
			int year;
			int month;
			int day;

			// اگر شمسی خواسته شده باشد
			if (isPersian)
			{
				PersianCalendar pc = new PersianCalendar(); // بدست آوردن تاریخ شمسی
				try
				{
					year = pc.GetYear(dt);
					month = pc.GetMonth(dt);
					day = pc.GetDayOfMonth(dt);
				}
				catch (Exception ex)
				{
					return "";
				}
			}
			else
			{
				year = dt.Year;
				month = dt.Month;
				day = dt.Day;
			}

			// برای هر کدام از انواع فرمت نیز باید تعیین شود که هر کدام با یک سوییچ جداگانه انجام می شوند
			switch (type)
			{
				// سوییچ مربوط به تاریخ
				case DateOrTime.Date:
					switch ((DateFormat)format)
					{
						case DateFormat.yyyymmdd:
							return string.Format("{0}{3}{1}{3}{2}", year, month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'), dateDelimeter);
						case DateFormat.yymmdd:
							return string.Format("{0}{3}{1}{3}{2}", year % 100, month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'), dateDelimeter);
					}
					break;
				// سوییچ مربوط به زمان
				case DateOrTime.Time:
					switch ((TimeFormat)format)
					{
						case TimeFormat.HHmmss:
							return string.Format("{0}{3}{1}{3}{2}", dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'),
																	dt.Second.ToString().PadLeft(2, '0'), timeDelimeter);
						case TimeFormat.HHmm:
							return string.Format("{0}{2}{1}", dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'), timeDelimeter);

						case TimeFormat.HHmmsshh:
							return string.Format("{0}{4}{1}{4}{2}{5}{3}", dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'),
																		  dt.Second.ToString().PadLeft(2, '0'), dt.Millisecond.ToString().PadLeft(3, '0'),
																		  timeDelimeter, miliSecondDelimeter);
					}
					break;
				// سوییچ مربوط به تاریخ و زمان
				case DateOrTime.DateTime:
					switch ((DateTimeFormat)format)
					{
						case DateTimeFormat.yyyymmddHHmmss:
							return string.Format("{0}{6}{1}{6}{2}{7}{3}{8}{4}{8}{5}", year, month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'),
								dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'), dt.Second.ToString().PadLeft(2, '0'),
								dateDelimeter, betweenDateTimeDelimeter, timeDelimeter);
						case DateTimeFormat.yymmddHHmm:
							return string.Format("{0}{5}{1}{5}{2}{6}{3}{7}{4}", (year % 100).ToString(), month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'),
								dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'),
								dateDelimeter, betweenDateTimeDelimeter, timeDelimeter);
						case DateTimeFormat.yyyymmddHHmmsshh:
							return string.Format("{0}{7}{1}{7}{2}{8}{3}{9}{4}{9}{5}{10}{6}", year, month.ToString().PadLeft(2, '0'), day.ToString().PadLeft(2, '0'),
								dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'), dt.Second.ToString().PadLeft(2, '0'),
								(dt.Millisecond / 10).ToString().PadLeft(2, '0'), dateDelimeter, betweenDateTimeDelimeter, timeDelimeter, miliSecondDelimeter);
					}
					break;
				// سوییچ مربوط به سال
				case DateOrTime.Year:
					switch ((YearFormat)format)
					{
						case YearFormat.yyyy:
							return year.ToString();
						case YearFormat.yy:
							return (year % 100).ToString();
					}
					break;
				// ماه
				case DateOrTime.Month:
					return month.ToString().PadLeft(2, '0');
				// روز
				case DateOrTime.Day:
					return day.ToString().PadLeft(2, '0');
				// ساعت
				case DateOrTime.Hour:
					return dt.Hour.ToString().PadLeft(2, '0');
				// دقیقه
				case DateOrTime.Minute:
					return dt.Minute.ToString().PadLeft(2, '0');
				// ثانیه
				case DateOrTime.Second:
					return dt.Second.ToString().PadLeft(2, '0');
				// روز هفته
				case DateOrTime.WeekDay:
					// اگر فارسی نخواهد همان انگلیسی را تحویل می دهیم
					if (!isPersian)
						return dt.DayOfWeek.ToString();
					// ولی اگر فارس بخواهد باید ترجمه کنیم
					else
					{
						switch (dt.DayOfWeek)
						{
							case DayOfWeek.Friday:
								return "جمعه";
							case DayOfWeek.Monday:
								return "دوشنبه";
							case DayOfWeek.Saturday:
								return "شنبه";
							case DayOfWeek.Sunday:
								return "یکشنبه";
							case DayOfWeek.Thursday:
								return "پنج شنبه";
							case DayOfWeek.Tuesday:
								return "سه شنبه";
							case DayOfWeek.Wednesday:
								return "چهارشنبه";
						}
					}
					break;
			}

			return "";
		}


		/// <summary>
		/// ساخت رشته ای که نشان دهنده نوع تاریخ و زمان انتخاب شده برای یک سلول در گزارش است
		/// </summary>
		/// <param name="type">نوع که می تواند تاریخ، زمان، تاریخ و زمان، سال، ماه، روز، ساعت، دقیقه و ثانیه باشد</param>
		/// <param name="format">هر کدام از نوع های فوق چند فرمت نوشتن دارند مانند تعداد کاراکتر های سال که می تواند 2 رقمی و 4 رقمی باشد</param>
		/// <returns>رشته تولید شده</returns>
		public static string GetFormatString(DateOrTime type, int format)
		{
			// نوع و فرمت در کنار هم قرار می گیرند و رشته ساخته می شود
			// کلی تر از این نمی توان نوشت زیرا باید حتما چک شوند و برای انتخاب مطمئن رشته تولید شود
			// آورد ولی بهتر است اینگونه باشد defualt تا درصد خطا پایین بیاید. از روز به بعد را می شود در تگ
			switch (type)
			{
				// تاریخ
				case DateOrTime.Date:
					return string.Format("{0}:{1}", type.ToString(), ((DateFormat)format).ToString());
				// زمان
				case DateOrTime.Time:
					return string.Format("{0}:{1}", type.ToString(), ((TimeFormat)format).ToString());
				// تاریخ و زمان
				case DateOrTime.DateTime:
					return string.Format("{0}:{1}", type.ToString(), ((DateTimeFormat)format).ToString());
				// سال
				case DateOrTime.Year:
					return string.Format("{0}:{1}", type.ToString(), ((YearFormat)format).ToString());
				// ماه
				case DateOrTime.Month:
					return type.ToString();
				// روز
				case DateOrTime.Day:
					return type.ToString();
				// ساعت
				case DateOrTime.Hour:
					return type.ToString();
				// دقیقه
				case DateOrTime.Minute:
					return type.ToString();
				// ثانیه
				case DateOrTime.Second:
					return type.ToString();
				// روز هفته
				case DateOrTime.WeekDay:
					return type.ToString();
			}

			return "";
		}

		public static List<string> GetDateOrTimeFields()
		{
			List<string> fields = new List<string>();

			fields.Add("تاریخ");
			fields.Add("زمان");
			fields.Add("تاریخ و زمان");
			fields.Add("سال");
			fields.Add("ماه");
			fields.Add("روز");
			fields.Add("ساعت");
			fields.Add("دقیقه");
			fields.Add("ثانیه");
			fields.Add("روز هفته");

			return fields;
		}

		public static List<string> GetTypeFormatFields(DateOrTime type)
		{
			List<string> fields = new List<string>();

			switch (type)
			{
				case DateOrTime.Date:
					fields.Add("yyyymmdd");
					fields.Add("yymmdd");
					break;
				case DateOrTime.Time:
					fields.Add("HHmmss");
					fields.Add("HHmm");
					break;
				case DateOrTime.DateTime:
					fields.Add("yyyymmddHHmmss");
					fields.Add("yymmddHHmm");
					break;
				case DateOrTime.Year:
					fields.Add("yyyy");
					fields.Add("yy");
					break;
			}

			return fields;
		}
	}
}
