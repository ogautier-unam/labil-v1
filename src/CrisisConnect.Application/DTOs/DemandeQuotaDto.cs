using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record DemandeQuotaDto(
    Guid Id,
    string Titre,
    string Description,
    StatutProposition Statut,
    Guid CreePar,
    DateTime CreeLe,
    int CapaciteMax,
    string UniteCapacite,
    int CapaciteUtilisee,
    string? AdresseDepot,
    DateTime? DateLimit,
    List<IntentionDonDto> Intentions);
