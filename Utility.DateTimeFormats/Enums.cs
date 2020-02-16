using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.DateTimeFormats
{
    public enum DateOrTime
    {
        Date = 1,
        Time = 2,
        DateTime = 3,
        Year = 4,
        Month = 5,
        Day = 6,
        Hour = 7,
        Minute = 8,
        Second = 9,
		WeekDay = 10
    }

    public enum DateFormat
    {
        yyyymmdd = 1,
        yymmdd = 2
    }

    public enum TimeFormat
    {
        HHmmss = 1,
        HHmm = 2,
		HHmmsshh = 3
    }

    public enum DateTimeFormat
    {
        yyyymmddHHmmss = 1,
        yymmddHHmm = 2,
		yyyymmddHHmmsshh = 3
    }

    public enum YearFormat
    {
        yyyy = 1,
        yy = 2
    }
}
