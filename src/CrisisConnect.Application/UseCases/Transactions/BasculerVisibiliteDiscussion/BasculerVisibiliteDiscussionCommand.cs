using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;

public record BasculerVisibiliteDiscussionCommand(
    Guid TransactionId,
    Visibilite NouvelleVisibilite)
    : ICommand;
