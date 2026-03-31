namespace MixxFit.API.Common.Extensions;

public static class UtilityExtensions
{
    public static string ToLowerFirstLetter(this string str) => str.Substring(0, 1).ToLower() + str.Substring(1);
    
    public static int? ToTotalSeconds(this int? minutes, int? seconds)
    {
        return minutes * 60 + seconds;
    }

    public static int? ToMinutesFromSeconds(this int? seconds)
    {
        return seconds / 60;
    }

    public static int? ToSecondsFromRemainderMinutes(this int? seconds)
    {
        return seconds % 60;
    }
}