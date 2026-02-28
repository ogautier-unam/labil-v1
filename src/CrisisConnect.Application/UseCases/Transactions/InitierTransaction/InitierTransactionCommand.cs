using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.InitierTransaction;

public record InitierTransactionCommand(
    Guid PropositionId,
    Guid InitiateurId) : IRequest<TransactionDto>;
