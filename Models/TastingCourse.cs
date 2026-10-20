namespace Sommelier.Models;

public record TastingCourse
{
    public int CourseNumber { get; init; }
    public string DishName { get; init; } = string.Empty;
    public Wine Wine { get; init; } = new();
    public string PairingNote { get; init; } = string.Empty;
    public List<string> Flavors { get; init; } = new();
}
