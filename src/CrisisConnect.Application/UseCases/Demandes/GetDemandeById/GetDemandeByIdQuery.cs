using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandeById;

public record GetDemandeByIdQuery(Guid Id) : IRequest<DemandeDto>;
