using ClassLibraryCoreModels;

namespace ClassLibraryServices;

/// <summary>
/// Временное хранилище
/// </summary>
public class Storage
{
    /// <summary>
    /// Список рекламных площадок
    /// </summary>
    public List<AdPlatform> AdPlatforms { get; set; } = [];
}
