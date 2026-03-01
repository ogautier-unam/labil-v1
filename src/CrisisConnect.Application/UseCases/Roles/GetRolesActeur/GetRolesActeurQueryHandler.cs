using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Roles.GetRolesActeur;

public class GetRolesActeurQueryHandler : IRequestHandler<GetRolesActeurQuery, IReadOnlyList<AttributionRoleDto>>
{
    private readonly IAttributionRoleRepository _repository;
    private readonly AppMapper _mapper;

    public GetRolesActeurQueryHandler(IAttributionRoleRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<AttributionRoleDto>> Handle(GetRolesActeurQuery request, CancellationToken cancellationToken)
    {
        var roles = await _repository.GetByActeurAsync(request.ActeurId, cancellationToken);
        return _mapper.ToDto(roles);
    }
}
