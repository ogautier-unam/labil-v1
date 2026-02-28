using MediatR;

namespace CrisisConnect.Application.UseCases.Auth.Logout;

public record LogoutCommand(Guid PersonneId) : IRequest;
