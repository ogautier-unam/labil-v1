using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Journal.GetEntreesJournal;

public record GetEntreesJournalQuery(Guid ActeurId) : IRequest<IReadOnlyList<EntreeJournalDto>>;
