using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;

public record AjouterOffreAuPanierCommand(Guid PanierId, Guid OffreId) : IRequest<PanierDto>;
