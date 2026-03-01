using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.GetSuggestionsByDemande;

public record GetSuggestionsByDemandeQuery(Guid DemandeId) : IRequest<IReadOnlyList<SuggestionAppariementDto>>;
