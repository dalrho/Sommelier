using System.Text.Json.Serialization;

namespace Sommelier.Models;

public record Wine
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("body")]
    public int Body { get; init; } = 1;

    [JsonPropertyName("abv")]
    public string Abv { get; init; } = string.Empty;

    [JsonPropertyName("temp")]
    public string Temp { get; init; } = string.Empty;

    [JsonPropertyName("score")]
    public int Score { get; init; } = 90;

    [JsonPropertyName("notes")]
    public List<string> Notes { get; init; } = new();

    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;

    [JsonIgnore]
    public string BodyDescription => Body switch
    {
        1 => "Light",
        2 => "Medium",
        3 => "Full",
        _ => "Medium"
    };

    [JsonIgnore]
    public string TypeCssClass => Type.ToLowerInvariant() switch
    {
        "red" => "red",
        "sparkling" => "sparkling",
        "non-alcoholic" => "non-alcoholic",
        _ => "white"
    };
}
