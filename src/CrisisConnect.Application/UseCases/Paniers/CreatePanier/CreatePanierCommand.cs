using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.CreatePanier;

public record CreatePanierCommand(Guid ProprietaireId) : IRequest<PanierDto>;
