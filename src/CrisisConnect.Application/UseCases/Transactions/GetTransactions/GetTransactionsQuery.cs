using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.GetTransactions;

public record GetTransactionsQuery() : IRequest<IReadOnlyList<TransactionDto>>;
