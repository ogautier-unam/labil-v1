using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Roles.GetRolesActeur;

public record GetRolesActeurQuery(Guid ActeurId) : IRequest<IReadOnlyList<AttributionRoleDto>>;
