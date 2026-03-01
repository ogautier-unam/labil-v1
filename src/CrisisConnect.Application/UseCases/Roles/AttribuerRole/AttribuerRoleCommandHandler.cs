using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Roles.AttribuerRole;

public class AttribuerRoleCommandHandler : IRequestHandler<AttribuerRoleCommand, AttributionRoleDto>
{
    private readonly IAttributionRoleRepository _repository;
    private readonly AppMapper _mapper;

    public AttribuerRoleCommandHandler(IAttributionRoleRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<AttributionRoleDto> Handle(AttribuerRoleCommand request, CancellationToken cancellationToken)
    {
        var attribution = new AttributionRole(
            request.ActeurId,
            request.TypeRole,
            request.DateDebut,
            request.DateFin,
            request.Reconductible,
            request.AccrediteeParId);

        await _repository.AddAsync(attribution, cancellationToken);
        return _mapper.ToDto(attribution);
    }
}
