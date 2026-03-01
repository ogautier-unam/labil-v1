using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Journal.GetEntreesJournal;

public record GetEntreesJournalQuery(Guid ActeurId) : IRequest<IReadOnlyList<EntreeJournalDto>>;
