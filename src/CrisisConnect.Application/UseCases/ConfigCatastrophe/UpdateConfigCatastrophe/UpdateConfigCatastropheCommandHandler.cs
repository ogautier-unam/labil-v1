using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.UpdateConfigCatastrophe;

public class UpdateConfigCatastropheCommandHandler : IRequestHandler<UpdateConfigCatastropheCommand, ConfigCatastropheDto>
{
    private readonly IConfigCatastropheRepository _repository;
    private readonly AppMapper _mapper;

    public UpdateConfigCatastropheCommandHandler(IConfigCatastropheRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<ConfigCatastropheDto> Handle(UpdateConfigCatastropheCommand request, CancellationToken cancellationToken)
    {
        var config = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ConfigCatastrophe), request.Id);

        config.MettreAJourParametres(request.DelaiArchivageJours, request.DelaiRappelAvantArchivage);

        if (request.EstActive)
            config.Activer();
        else
            config.Desactiver();

        await _repository.UpdateAsync(config, cancellationToken);
        return _mapper.ToDto(config);
    }
}
