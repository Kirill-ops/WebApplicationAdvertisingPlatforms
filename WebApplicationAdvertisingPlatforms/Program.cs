
using ClassLibraryServices;
using WebApplicationAdvertisingPlatforms.Middlewares;

namespace WebApplicationAdvertisingPlatforms;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSingleton<Storage>();
        builder.Services.AddSingleton<ImportFileService>();

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        // TODO 1:  Разрешены все CORS
        app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
