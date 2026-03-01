using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Medias.AttacherMedia;

public record AttacherMediaCommand(
    Guid PropositionId,
    string Url,
    TypeMedia Type) : IRequest<MediaDto>;
