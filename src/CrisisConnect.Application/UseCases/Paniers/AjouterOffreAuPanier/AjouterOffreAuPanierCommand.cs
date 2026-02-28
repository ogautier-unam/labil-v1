using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;

public record AjouterOffreAuPanierCommand(Guid PanierId, Guid OffreId) : IRequest<PanierDto>;
