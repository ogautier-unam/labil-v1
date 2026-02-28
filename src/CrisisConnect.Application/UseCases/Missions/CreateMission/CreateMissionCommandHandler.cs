using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using MediatR;

namespace CrisisConnect.Application.UseCases.Missions.CreateMission;

public class CreateMissionCommandHandler : IRequestHandler<CreateMissionCommand, MissionDto>
{
    private readonly IMissionRepository _repository;
    private readonly IMapper _mapper;

    public CreateMissionCommandHandler(IMissionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MissionDto> Handle(CreateMissionCommand request, CancellationToken cancellationToken)
    {
        PlageTemporelle? plage = null;
        if (request.DebutPlage.HasValue && request.FinPlage.HasValue)
            plage = new PlageTemporelle(request.DebutPlage.Value, request.FinPlage.Value);

        var mission = new Mission(request.Titre, request.Description, request.PropositionId, request.CreePar, request.NombreBenevoles, plage);
        await _repository.AddAsync(mission, cancellationToken);

        return _mapper.Map<MissionDto>(mission);
    }
}
