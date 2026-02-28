using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;

public class CreateConfigCatastropheCommandHandler : IRequestHandler<CreateConfigCatastropheCommand, ConfigCatastropheDto>
{
    private readonly IConfigCatastropheRepository _repository;
    private readonly IMapper _mapper;

    public CreateConfigCatastropheCommandHandler(IConfigCatastropheRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ConfigCatastropheDto> Handle(CreateConfigCatastropheCommand request, CancellationToken cancellationToken)
    {
        var config = new Domain.Entities.ConfigCatastrophe(
            request.Nom,
            request.Description,
            request.ZoneGeographique,
            request.EtatReferent,
            request.DelaiArchivageJours,
            request.DelaiRappelAvantArchivage);

        await _repository.AddAsync(config, cancellationToken);
        return _mapper.Map<ConfigCatastropheDto>(config);
    }
}
