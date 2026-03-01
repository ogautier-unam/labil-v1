using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;

public class CreateConfigCatastropheCommandHandler : IRequestHandler<CreateConfigCatastropheCommand, ConfigCatastropheDto>
{
    private readonly IConfigCatastropheRepository _repository;
    private readonly AppMapper _mapper;

    public CreateConfigCatastropheCommandHandler(IConfigCatastropheRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<ConfigCatastropheDto> Handle(CreateConfigCatastropheCommand request, CancellationToken cancellationToken)
    {
        var config = new Domain.Entities.ConfigCatastrophe(
            request.Nom,
            request.Description,
            request.ZoneGeographique,
            request.EtatReferent,
            request.DelaiArchivageJours,
            request.DelaiRappelAvantArchivage);

        await _repository.AddAsync(config, cancellationToken);
        return _mapper.ToDto(config);
    }
}
