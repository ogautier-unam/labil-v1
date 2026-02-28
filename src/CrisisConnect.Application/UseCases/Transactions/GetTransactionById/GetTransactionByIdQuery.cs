using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto>;
