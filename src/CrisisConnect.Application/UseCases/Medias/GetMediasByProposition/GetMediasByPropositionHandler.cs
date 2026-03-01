using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Medias.GetMediasByProposition;

public class GetMediasByPropositionHandler : IRequestHandler<GetMediasByPropositionQuery, List<MediaDto>>
{
    private readonly IMediaRepository _mediaRepository;

    public GetMediasByPropositionHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async ValueTask<List<MediaDto>> Handle(GetMediasByPropositionQuery request, CancellationToken cancellationToken)
    {
        var medias = await _mediaRepository.GetByPropositionIdAsync(request.PropositionId, cancellationToken);
        return AppMapper.ToDto(medias as List<Media> ?? medias.ToList());
    }
}
