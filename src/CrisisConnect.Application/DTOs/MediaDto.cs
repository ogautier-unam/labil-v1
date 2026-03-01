using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record MediaDto(
    Guid Id,
    Guid PropositionId,
    string Url,
    TypeMedia Type,
    DateTime DateAjout);
