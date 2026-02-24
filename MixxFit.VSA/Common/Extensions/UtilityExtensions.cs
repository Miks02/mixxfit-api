namespace MixxFit.VSA.Common.Extensions;

public static class UtilityExtensions
{
    public static string ToLowerFirstLetter(this string str) => str.Substring(0, 1).ToLower() + str.Substring(1);
    
    public static int? ToIntegerFromNullableSeconds(this TimeSpan? timeSpan)
    {
        if (timeSpan is null)
            return null;
        return timeSpan.Value.Seconds;
    }

    public static int? ToIntegerFromNullableMinutes(this TimeSpan? timeSpan)
    {
        if (timeSpan is null)
            return null;
        return (int)timeSpan.Value.TotalMinutes;
    }
    
    public static TimeSpan? ValidateMinutesAndSeconds(int? minutes, int? seconds)
    {
        if (minutes is null || seconds is null)
            return null;

        TimeSpan fromMinutes = TimeSpan.FromMinutes((double)minutes);
        TimeSpan fromSeconds = TimeSpan.FromSeconds((double)seconds);

        return fromMinutes + fromSeconds;
    }
}