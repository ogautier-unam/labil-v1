using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;

public record ConfirmerPanierCommand(Guid PanierId) : IRequest;
