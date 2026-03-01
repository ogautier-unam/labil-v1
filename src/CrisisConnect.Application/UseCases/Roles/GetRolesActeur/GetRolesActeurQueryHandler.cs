using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Roles.GetRolesActeur;

public class GetRolesActeurQueryHandler : IRequestHandler<GetRolesActeurQuery, IReadOnlyList<AttributionRoleDto>>
{
    private readonly IAttributionRoleRepository _repository;
    private readonly IMapper _mapper;

    public GetRolesActeurQueryHandler(IAttributionRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AttributionRoleDto>> Handle(GetRolesActeurQuery request, CancellationToken cancellationToken)
    {
        var roles = await _repository.GetByActeurAsync(request.ActeurId, cancellationToken);
        return _mapper.Map<IReadOnlyList<AttributionRoleDto>>(roles);
    }
}
