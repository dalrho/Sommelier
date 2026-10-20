using Sommelier.Models;

namespace Sommelier.Services;

public class WineCatalog
{
    private readonly List<Wine> _wines = new()
    {
        new Wine
        {
            Name = "Riesling",
            Type = "White",
            Body = 2,
            Abv = "11%",
            Temp = "8–10°C",
            Score = 96,
            Notes = new List<string> { "White peach", "Lime zest", "Honeysuckle" },
            Reason = "Its bright acidity cools spice while a touch of sweetness rounds out deep savory flavors."
        },
        new Wine
        {
            Name = "Pinot Noir",
            Type = "Red",
            Body = 1,
            Abv = "13.5%",
            Temp = "14–16°C",
            Score = 91,
            Notes = new List<string> { "Wild cherry", "Forest floor", "Soft spice" },
            Reason = "Silky tannins and earthy aromatics echo umami without overpowering the dish."
        },
        new Wine
        {
            Name = "Champagne Brut",
            Type = "Sparkling",
            Body = 1,
            Abv = "12%",
            Temp = "6–8°C",
            Score = 87,
            Notes = new List<string> { "Green apple", "Brioche", "Chalk" },
            Reason = "Fine bubbles and mineral acidity refresh the palate between richly flavored bites."
        }
    };

    public IReadOnlyList<Wine> GetAllWines() => _wines.AsReadOnly();

    public IReadOnlyList<Wine> GetRecommendedWines(IEnumerable<string> flavors)
    {
        var flavorList = flavors.ToList();
        if (flavorList.Count == 0)
        {
            return Array.Empty<Wine>();
        }

        // Return curated recommendations ordered by score
        return _wines.OrderByDescending(w => w.Score).ToList();
    }
}
