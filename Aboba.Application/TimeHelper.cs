namespace Aboba.Application;

public class TimeHelper
{
    public static DateTime GetCzechLocalTime(DateTime utcDateTime)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
    }
}