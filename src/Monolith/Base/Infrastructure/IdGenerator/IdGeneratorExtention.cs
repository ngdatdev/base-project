using BaseApiReference.Abstractions.IdGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IdGenerator;

/// <summary>
/// IdGenerator extention.
/// </summary>
public static class IdGeneratorExtention
{
    public static void AddIdGenerator(this IServiceCollection services)
    {
        services.AddSingleton<IGeneratorIdHandler, SnowflakeIdGeneratorHandler>();
    }
}
