using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.CreatePanier;

public record CreatePanierCommand(Guid ProprietaireId) : IRequest<PanierDto>;
