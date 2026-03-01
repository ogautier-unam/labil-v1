using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Mandats.CreerMandat;

public class CreerMandatCommandHandler : IRequestHandler<CreerMandatCommand, MandatDto>
{
    private readonly IMandatRepository _repository;
    private readonly IMapper _mapper;

    public CreerMandatCommandHandler(IMandatRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MandatDto> Handle(CreerMandatCommand request, CancellationToken cancellationToken)
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
        return _mapper.Map<MandatDto>(mandat);
    }
}
