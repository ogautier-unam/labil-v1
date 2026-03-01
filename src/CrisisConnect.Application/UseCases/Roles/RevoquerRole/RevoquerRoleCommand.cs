using Mediator;

namespace CrisisConnect.Application.UseCases.Roles.RevoquerRole;

public record RevoquerRoleCommand(Guid AttributionId) : ICommand;
