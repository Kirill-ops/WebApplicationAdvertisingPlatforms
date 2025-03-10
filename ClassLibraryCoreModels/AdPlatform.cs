using System.Xml.Linq;

namespace ClassLibraryCoreModels;

/// <summary>
/// Рекламная площадка
/// </summary>
public class AdPlatform
{
    /// <summary>
    /// ID рекламной площадки
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название рекламной площадки
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Локации, в которых действует рекламная площадка
    /// </summary>
    public IReadOnlyList<string> Locations { get; init; }

    public AdPlatform(string name, IReadOnlyList<string> locations)
    {
        Id = Guid.NewGuid();
        Name = name;
        Locations = locations;
    }

    public AdPlatform(Guid id, string name, IReadOnlyList<string> locations)
    {
        Id = id;
        Name = name;
        Locations = locations;
    }

    public AdPlatform(AdPlatform platform)
    {
        Id = platform.Id;
        Name = platform.Name;
        Locations = platform.Locations;
    }
}
