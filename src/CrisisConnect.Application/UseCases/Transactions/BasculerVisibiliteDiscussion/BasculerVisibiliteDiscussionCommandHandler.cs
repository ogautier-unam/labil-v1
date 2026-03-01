using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;

public class BasculerVisibiliteDiscussionCommandHandler
    : ICommandHandler<BasculerVisibiliteDiscussionCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public BasculerVisibiliteDiscussionCommandHandler(ITransactionRepository transactionRepository)
        => _transactionRepository = transactionRepository;

    public async ValueTask<Unit> Handle(
        BasculerVisibiliteDiscussionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        transaction.Discussion.BasculerVisibilite(request.NouvelleVisibilite);
        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
        return Unit.Value;
    }
}
