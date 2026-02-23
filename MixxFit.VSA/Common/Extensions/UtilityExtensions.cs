namespace MixxFit.VSA.Common.Extensions;

public static class UtilityExtensions
{
    public static string ToLowerFirstLetter(this string str) => str.Substring(0, 1).ToLower() + str.Substring(1);
}