using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Missions.AssignBenevole;

public class AssignBenevoleCommandHandler : IRequestHandler<AssignBenevoleCommand, MatchingDto>
{
    private readonly IMissionRepository _missionRepository;
    private readonly IMatchingRepository _matchingRepository;
    private readonly IMapper _mapper;

    public AssignBenevoleCommandHandler(IMissionRepository missionRepository, IMatchingRepository matchingRepository, IMapper mapper)
    {
        _missionRepository = missionRepository;
        _matchingRepository = matchingRepository;
        _mapper = mapper;
    }

    public async Task<MatchingDto> Handle(AssignBenevoleCommand request, CancellationToken cancellationToken)
    {
        var mission = await _missionRepository.GetByIdAsync(request.MissionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Mission), request.MissionId);

        if (mission.Statut != Domain.Enums.StatutMission.Planifiee)
            throw new DomainException("Impossible d'assigner un bénévole à une mission non planifiée.");

        var matching = new Matching(request.MissionId, request.BenevoleId);
        await _matchingRepository.AddAsync(matching, cancellationToken);

        return _mapper.Map<MatchingDto>(matching);
    }
}
