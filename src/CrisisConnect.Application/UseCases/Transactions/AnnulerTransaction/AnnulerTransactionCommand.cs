using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;

public record AnnulerTransactionCommand(Guid TransactionId) : ICommand;
