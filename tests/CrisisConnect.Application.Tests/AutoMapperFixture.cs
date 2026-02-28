using AutoMapper;
using CrisisConnect.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace CrisisConnect.Application.Tests;

/// <summary>
/// Fournit une instance IMapper configurée de la même manière que dans le vrai projet
/// (AddAutoMapper v16 via ServiceCollection).
/// </summary>
public static class AutoMapperFixture
{
    public static IMapper Créer()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }
}
