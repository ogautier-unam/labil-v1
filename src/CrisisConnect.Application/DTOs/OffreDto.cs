using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record OffreDto(
    Guid Id,
    string Titre,
    string Description,
    StatutProposition Statut,
    Guid CreePar,
    DateTime CreeLe,
    bool LivraisonIncluse,
    List<Guid> DemandesCouplees);
