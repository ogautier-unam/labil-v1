using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record PropositionAvecValidationDto(
    Guid Id,
    string Titre,
    string Description,
    StatutProposition Statut,
    Guid CreePar,
    DateTime CreeLe,
    string DescriptionValidation,
    StatutValidation StatutValidation,
    Guid? ValideurEntiteId);
