using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Mandats.CreerMandat;

public class CreerMandatCommandHandler : IRequestHandler<CreerMandatCommand, MandatDto>
{
    private readonly IMandatRepository _repository;
    private readonly AppMapper _mapper;

    public CreerMandatCommandHandler(IMandatRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<MandatDto> Handle(CreerMandatCommand request, CancellationToken cancellationToken)
    {
        var mandat = new Mandat(
            request.MandantId,
            request.MandataireId,
            request.Portee,
            request.Description,
            request.EstPublic,
            request.DateDebut,
            request.DateFin);

        await _repository.AddAsync(mandat, cancellationToken);
        return _mapper.ToDto(mandat);
    }
}
