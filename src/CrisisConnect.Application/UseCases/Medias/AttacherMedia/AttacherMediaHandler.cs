using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Medias.AttacherMedia;

public class AttacherMediaHandler : IRequestHandler<AttacherMediaCommand, MediaDto>
{
    private readonly IPropositionRepository _propositionRepository;
    private readonly IMediaRepository _mediaRepository;

    public AttacherMediaHandler(IPropositionRepository propositionRepository, IMediaRepository mediaRepository)
    {
        _propositionRepository = propositionRepository;
        _mediaRepository = mediaRepository;
    }

    public async ValueTask<MediaDto> Handle(AttacherMediaCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _propositionRepository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        var media = new Media(proposition.Id, request.Url, request.Type);
        await _mediaRepository.AddAsync(media, cancellationToken);
        return AppMapper.ToDto(media);
    }
}
