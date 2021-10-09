using System;
using System.Globalization;

namespace _Framework
{
    public static class DateConvention
    {
        public static string ToPersianDate(this DateTime date)
        {
            var persianCalender = new PersianCalendar();
            return $"{persianCalender.GetYear(date)}/{persianCalender.GetMonth(date)}/{persianCalender.GetDayOfMonth(date)}";
        }

        public static string ToPersianTime(this DateTime date)
        {
            var persianCalender = new PersianCalendar();
            return $"{persianCalender.GetHour(date)}:{persianCalender.GetMinute(date)}";
        }

        public static string ToPersianDateTime(this DateTime date)
        {
            var persianCalender = new PersianCalendar();
            return $"{persianCalender.GetYear(date)}/{persianCalender.GetMonth(date)}/{persianCalender.GetDayOfMonth(date)}  {persianCalender.GetHour(date)}:{persianCalender.GetMinute(date)}";
        }
    }
}