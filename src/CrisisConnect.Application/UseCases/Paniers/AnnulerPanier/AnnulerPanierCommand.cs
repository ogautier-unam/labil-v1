using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;

public record AnnulerPanierCommand(Guid PanierId) : IRequest;
