using BaseApiReference.Abstractions.Excel;
using Infrastructure.CSV;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Excel;

/// <summary>
/// Excel services extention.
/// </summary>
internal static class ExcelServicesExtention
{
    public static void AddExcelServices(this IServiceCollection services)
    {
        services.AddSingleton<IExcelHandler, ExcelHandler>();
    }
}
