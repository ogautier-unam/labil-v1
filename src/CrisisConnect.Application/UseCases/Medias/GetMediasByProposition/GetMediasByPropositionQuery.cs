using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Medias.GetMediasByProposition;

public record GetMediasByPropositionQuery(Guid PropositionId) : IRequest<List<MediaDto>>;
