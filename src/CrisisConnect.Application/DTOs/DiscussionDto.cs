namespace CrisisConnect.Application.DTOs;

public record DiscussionDto(
    Guid Id,
    Guid TransactionId,
    string Visibilite,
    IReadOnlyList<MessageDto> Messages);
