using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;

public record ConfirmerTransactionCommand(Guid TransactionId) : IRequest;
