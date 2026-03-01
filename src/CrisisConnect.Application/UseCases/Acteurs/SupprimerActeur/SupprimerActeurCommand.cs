using Mediator;

namespace CrisisConnect.Application.UseCases.Acteurs.SupprimerActeur;

/// <summary>Droit à l'oubli RGPD (NF-06) — anonymise les données personnelles de l'acteur.</summary>
public record SupprimerActeurCommand(Guid Id) : IRequest<Unit>;
