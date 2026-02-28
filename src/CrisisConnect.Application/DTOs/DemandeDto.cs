using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record DemandeDto(
    Guid Id,
    string Titre,
    string Description,
    StatutProposition Statut,
    Guid CreePar,
    DateTime CreeLe,
    OperateurLogique OperateurLogique,
    NiveauUrgence Urgence,
    string? RegionSeverite);
