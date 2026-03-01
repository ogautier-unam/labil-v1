using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.GetPanier;

/// <summary>Retourne le panier ouvert du propriétaire, ou null s'il n'en a pas.</summary>
public record GetPanierQuery(Guid ProprietaireId) : IRequest<PanierDto?>;
