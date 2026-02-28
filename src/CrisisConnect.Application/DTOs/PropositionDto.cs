using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record PropositionDto(
    Guid Id,
    string Titre,
    string Description,
    StatutProposition Statut,
    Guid CreePar,
    DateTime CreeLe);
