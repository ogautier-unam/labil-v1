namespace CrisisConnect.Web.Models;

public record DiscussionData(
    Guid Id,
    Guid TransactionId,
    string Visibilite,
    IReadOnlyList<MessageModel> Messages);
