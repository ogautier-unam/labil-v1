using MediatR;

namespace CrisisConnect.Application.UseCases.Roles.RevoquerRole;

public record RevoquerRoleCommand(Guid AttributionId) : IRequest;
