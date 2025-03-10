using ClassLibraryCoreModels;
using ClassLibraryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAdvertisingPlatforms.Controllers;

[Route("platforms")]
[ApiController]
[AllowAnonymous]
public class PlatformsController(
    Storage storage,
    ImportFileService importFileService)
    : Controller
{

    [HttpPost("")]
    public async Task UploadingFromFile([FromForm] IFormFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (fileExtension != ".txt")
            throw new ArgumentException("Поддерживаются только файлы с расширением .txt.");

        storage.AdPlatforms = await importFileService.GetPlatformsFromStream(file.OpenReadStream());
    }

    [HttpGet("")]
    public IReadOnlyList<string> Get([FromQuery] string location) // для простоты будем брать локацию из параметров запроса
    {
        var locationTrim = location.Trim();
        var result = storage.AdPlatforms
               .Where(ad => ad.Locations.Any(
                   l => IsValidLocations(l, locationTrim)))
               .Select(ad => ad.Name)
               .ToList();

        return result;
    }

    [HttpGet("all")]
    public IReadOnlyList<string> Get() 
    {
        var result = storage.AdPlatforms
               .Select(ad => ad.Name)
               .ToList();

        return result;
    }

    private bool IsValidLocations(string AdPlatformLocation, string location)
    {
        if (location.StartsWith(AdPlatformLocation) && location.Length >= AdPlatformLocation.Length
            || (AdPlatformLocation.StartsWith(location) && AdPlatformLocation.Length <= location.Length))
            return true;

        return false;
    }
}
