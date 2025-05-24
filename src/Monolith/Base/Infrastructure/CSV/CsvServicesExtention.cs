using BaseApiReference.Abstractions.CSV;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CSV;

/// <summary>
/// CSV Collection Extention
/// </summary>
internal static class CsvServicesExtention
{
    public static void AddCSVServices(this IServiceCollection services)
    {
        services.AddSingleton<ICsvHandler, CsvHandler>();
    }
}
