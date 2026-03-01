using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Roles.AttribuerRole;

public class AttribuerRoleCommandHandler : IRequestHandler<AttribuerRoleCommand, AttributionRoleDto>
{
    private readonly IAttributionRoleRepository _repository;
    private readonly IMapper _mapper;

    public AttribuerRoleCommandHandler(IAttributionRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AttributionRoleDto> Handle(AttribuerRoleCommand request, CancellationToken cancellationToken)
    {
        var attribution = new AttributionRole(
            request.ActeurId,
            request.TypeRole,
            request.DateDebut,
            request.DateFin,
            request.Reconductible,
            request.AccrediteeParId);

        await _repository.AddAsync(attribution, cancellationToken);
        return _mapper.Map<AttributionRoleDto>(attribution);
    }
}
