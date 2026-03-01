using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record EntreeJournalDto(
    Guid Id,
    Guid ActeurId,
    TypeOperation TypeOperation,
    DateTime DateHeure,
    string? Details);
