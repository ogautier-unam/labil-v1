using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.GetDiscussion;

public record GetDiscussionQuery(Guid TransactionId) : IRequest<DiscussionDto>;
