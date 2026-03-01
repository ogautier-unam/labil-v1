using CrisisConnect.Domain.Enums;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;

public record BasculerVisibiliteDiscussionCommand(
    Guid TransactionId,
    Visibilite NouvelleVisibilite)
    : IRequest;
