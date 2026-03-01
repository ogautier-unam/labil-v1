using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.UpdateOffre;

public record UpdateOffreCommand(
    Guid Id,
    string Titre,
    string Description,
    bool LivraisonIncluse = false,
    double? Latitude = null,
    double? Longitude = null) : IRequest<OffreDto>;
