using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.GetDiscussion;

public record GetDiscussionQuery(Guid TransactionId) : IRequest<DiscussionDto>;
