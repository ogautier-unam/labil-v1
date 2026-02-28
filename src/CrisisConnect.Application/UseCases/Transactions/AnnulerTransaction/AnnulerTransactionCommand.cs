using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;

public record AnnulerTransactionCommand(Guid TransactionId) : IRequest;
