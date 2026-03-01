using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Entites.CreateEntite;

public record CreateEntiteCommand(
    string Email,
    string MotDePasse,
    string Nom,
    string Description,
    string MoyensContact,
    Guid ResponsableId) : IRequest<EntiteDto>;
