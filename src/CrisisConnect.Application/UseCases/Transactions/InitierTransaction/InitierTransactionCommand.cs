using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.InitierTransaction;

public record InitierTransactionCommand(
    Guid PropositionId,
    Guid InitiateurId) : IRequest<TransactionDto>;
