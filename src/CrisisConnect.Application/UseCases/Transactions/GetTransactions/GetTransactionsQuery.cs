using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.GetTransactions;

public record GetTransactionsQuery() : IRequest<IReadOnlyList<TransactionDto>>;
