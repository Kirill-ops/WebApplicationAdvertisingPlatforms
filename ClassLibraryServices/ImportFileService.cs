using ClassLibraryCoreModels;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
namespace ClassLibraryServices;

/// <summary>
/// Сервис для парсинга файла с рекламными площадками
/// Файл должен иметь содержание такое же, как в описании тестового задания
/// </summary>
public class ImportFileService
{
    /// <summary>
    /// Парсинга файла с рекламными площадками
    /// </summary>
    /// <param name="fileStream">Поток для чтения файла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список <see cref="AdPlatform"/></returns>
    public async Task<List<AdPlatform>> GetPlatformsFromStream(Stream fileStream, CancellationToken cancellationToken = default)
    {
        using StreamReader reader = new StreamReader(fileStream);

        var fileContent = await reader.ReadToEndAsync(cancellationToken);
        if (fileContent is null || fileContent.Trim() == string.Empty)
            return [];

        var adPlatforms = new List<AdPlatform>();

        var fileContentStrings = fileContent.Split(['\n', '\r']);

        foreach(var fileContentString in fileContentStrings)
        {
            var platformAdnLocaltions = fileContentString.Split(':');

            // Если очередная строка не имеет вид "text:text", то пропускаем её (
            // Или можно кинуть исключение и сообщить, что в файле есть ошибка

            if (platformAdnLocaltions.Length != 2)
                continue;

            var namePlatform = platformAdnLocaltions[0];

            // небольшая проверка, что локация соответствует шаблону '/текст/текст/.../текст',
            // где 'текст' состоит из английских символов и цифр
            // Если не соответствует, то просто не добавляется
            // Или можно кинуть исключение и сообщить, что в файле есть ошибка
            var regex = new Regex(@"^\/[a-zA-Z0-9]+(\/[a-zA-Z0-9]+)*$");
            var locations = platformAdnLocaltions[1].Split(',').Where(x => regex.IsMatch(x)).ToList();

            adPlatforms.Add(new(namePlatform, locations));

        }

        return adPlatforms.OrderBy(x => x.Locations.Count).ToList();
    }
}
