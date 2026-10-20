namespace Sommelier.Models;

public enum WineBody
{
    Light = 1,
    Medium = 2,
    Full = 3
}

public static class WineBodyExtensions
{
    public static string ToDisplayString(this WineBody body) => body switch
    {
        WineBody.Light => "Light",
        WineBody.Medium => "Medium",
        WineBody.Full => "Full",
        _ => string.Empty
    };

    public static string ToDisplayString(this int body) => body switch
    {
        1 => "Light",
        2 => "Medium",
        3 => "Full",
        _ => string.Empty
    };
}
