using Mediator;

namespace CrisisConnect.Application.UseCases.Auth.Logout;

public record LogoutCommand(Guid PersonneId) : ICommand;
