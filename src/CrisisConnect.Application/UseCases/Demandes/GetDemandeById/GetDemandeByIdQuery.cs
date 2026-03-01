using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandeById;

public record GetDemandeByIdQuery(Guid Id) : IRequest<DemandeDto>;
