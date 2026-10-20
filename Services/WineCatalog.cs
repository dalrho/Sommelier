using Sommelier.Models;

namespace Sommelier.Services;

public class WineCatalog
{
    private readonly List<Wine> _wines = new()
    {
        new Wine
        {
            Name = "Riesling Grand Cru",
            Type = WineType.White,
            Body = 2,
            Abv = "11%",
            Temp = "8–10°C",
            Score = 96,
            Notes = new List<string> { "White peach", "Lime zest", "Honeysuckle", "Flint" },
            Reason = "Its bright natural acidity cools fiery spices while delicate residual sweetness rounds out deep umami and rich sauces."
        },
        new Wine
        {
            Name = "Pinot Noir Reserve",
            Type = WineType.Red,
            Body = 1,
            Abv = "13.5%",
            Temp = "14–16°C",
            Score = 93,
            Notes = new List<string> { "Wild cherry", "Forest floor", "Black tea", "Soft spice" },
            Reason = "Silky tannins and earthy undertones echo savory umami and roasted poultry without overwhelming delicate flavors."
        },
        new Wine
        {
            Name = "Champagne Brut Grand Cru",
            Type = WineType.Sparkling,
            Body = 1,
            Abv = "12%",
            Temp = "6–8°C",
            Score = 91,
            Notes = new List<string> { "Green apple", "Brioche", "Chalk minerality", "Lemon curd" },
            Reason = "Laser-like acidity and fine effervescence act as a palate reset, cutting through fatty richness and salty crudo."
        },
        new Wine
        {
            Name = "Cabernet Sauvignon",
            Type = WineType.Red,
            Body = 3,
            Abv = "14.5%",
            Temp = "16–18°C",
            Score = 95,
            Notes = new List<string> { "Blackcurrant", "Cedarwood", "Graphite", "Dark cocoa" },
            Reason = "Substantial tannins bond with animal proteins and melted fat, cutting through rich cuts and smoky wood char."
        },
        new Wine
        {
            Name = "Sancerre Sauvignon Blanc",
            Type = WineType.White,
            Body = 1,
            Abv = "13%",
            Temp = "8–10°C",
            Score = 94,
            Notes = new List<string> { "Gooseberry", "Crushed slate", "Lemongrass", "Fresh thyme" },
            Reason = "Vibrant herbaceous notes and crisp flinty minerality harmonize with citrus marinades, herbal oils, and goat cheeses."
        },
        new Wine
        {
            Name = "Northern Rhône Syrah",
            Type = WineType.Red,
            Body = 3,
            Abv = "13.5%",
            Temp = "15–17°C",
            Score = 92,
            Notes = new List<string> { "Cracked black pepper", "Black olive", "Smoked bacon", "Violet" },
            Reason = "Pungent peppercorn aromatics and savory umami depth create an intoxicating counterpoint to barbecue, duck, and wild game."
        },
        new Wine
        {
            Name = "Sparkling Artisanal Verjus",
            Type = WineType.NonAlcoholic,
            Body = 1,
            Abv = "0.0%",
            Temp = "6–8°C",
            Score = 89,
            Notes = new List<string> { "Tart green grape", "Elderflower", "Yuzu peel", "Crisp verjus" },
            Reason = "Unfermented tart grape verjus and natural carbonation provide the mouthwatering palate-cleansing lift of champagne with zero alcohol."
        }
    };

    public IReadOnlyList<Wine> GetAllWines() => _wines.AsReadOnly();

    public Wine? GetWineByName(string name)
    {
        return _wines.FirstOrDefault(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase))
            ?? _wines.FirstOrDefault(w => w.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<Wine> GetRecommendedWines(IEnumerable<string> flavors)
    {
        var flavorList = flavors.ToList();
        if (flavorList.Count == 0)
        {
            return Array.Empty<Wine>();
        }

        var results = new List<Wine>();

        foreach (var wine in _wines)
        {
            var dynamicScore = CalculateCompatibilityScore(wine, flavorList);
            results.Add(wine with { Score = dynamicScore });
        }

        return results.OrderByDescending(w => w.Score).ToList();
    }

    public int CalculateCompatibilityScore(Wine wine, IEnumerable<string> flavors)
    {
        var flavorSet = new HashSet<string>(flavors, StringComparer.OrdinalIgnoreCase);
        if (flavorSet.Count == 0) return wine.Score;

        int score = wine.Score;

        if (wine.Name.Contains("Riesling"))
        {
            if (flavorSet.Contains("Spicy")) score += 4;
            if (flavorSet.Contains("Sweet")) score += 3;
            if (flavorSet.Contains("Umami")) score += 2;
            if (flavorSet.Contains("Citrus / Acidic")) score += 1;
        }
        else if (wine.Name.Contains("Pinot Noir"))
        {
            if (flavorSet.Contains("Umami")) score += 5;
            if (flavorSet.Contains("Herbal")) score += 3;
            if (flavorSet.Contains("Smoky")) score += 2;
            if (flavorSet.Contains("Rich / Fatty")) score += 1;
        }
        else if (wine.Name.Contains("Champagne"))
        {
            if (flavorSet.Contains("Salty")) score += 5;
            if (flavorSet.Contains("Rich / Fatty")) score += 4;
            if (flavorSet.Contains("Citrus / Acidic")) score += 3;
        }
        else if (wine.Name.Contains("Cabernet"))
        {
            if (flavorSet.Contains("Rich / Fatty")) score += 6;
            if (flavorSet.Contains("Smoky")) score += 4;
            if (flavorSet.Contains("Spicy")) score -= 5; // Heavy tannin clashes with extreme capsaicin
        }
        else if (wine.Name.Contains("Sancerre") || wine.Name.Contains("Sauvignon"))
        {
            if (flavorSet.Contains("Citrus / Acidic")) score += 5;
            if (flavorSet.Contains("Herbal")) score += 5;
            if (flavorSet.Contains("Salty")) score += 2;
        }
        else if (wine.Name.Contains("Syrah"))
        {
            if (flavorSet.Contains("Smoky")) score += 6;
            if (flavorSet.Contains("Umami")) score += 4;
            if (flavorSet.Contains("Herbal")) score += 2;
        }
        else if (wine.Name.Contains("Verjus"))
        {
            if (flavorSet.Contains("Citrus / Acidic")) score += 4;
            if (flavorSet.Contains("Salty")) score += 4;
            if (flavorSet.Contains("Spicy")) score += 3;
        }

        return Math.Clamp(score, 78, 99);
    }
}
