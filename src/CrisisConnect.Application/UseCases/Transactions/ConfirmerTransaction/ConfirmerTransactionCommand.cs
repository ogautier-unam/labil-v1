using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;

public record ConfirmerTransactionCommand(Guid TransactionId) : ICommand;
