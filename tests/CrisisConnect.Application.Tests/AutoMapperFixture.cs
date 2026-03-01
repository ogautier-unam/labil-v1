using CrisisConnect.Application.Mappings;

namespace CrisisConnect.Application.Tests;

/// <summary>
/// Fournit une instance AppMapper (Riok.Mapperly) pour les tests unitaires.
/// </summary>
public static class AutoMapperFixture
{
    public static AppMapper Créer() => new AppMapper();
}
