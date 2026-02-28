using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record TransactionDto(
    Guid Id,
    Guid PropositionId,
    Guid InitiateurId,
    StatutTransaction Statut,
    DateTime DateCreation);
