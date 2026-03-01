using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.CreateOffre;

public record CreateOffreCommand(
    string Titre,
    string Description,
    Guid CreePar,
    bool LivraisonIncluse = false,
    double? Latitude = null,
    double? Longitude = null) : IRequest<OffreDto>;
