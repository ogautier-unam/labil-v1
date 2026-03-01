using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;

public class BasculerVisibiliteDiscussionCommandHandler
    : IRequestHandler<BasculerVisibiliteDiscussionCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public BasculerVisibiliteDiscussionCommandHandler(ITransactionRepository transactionRepository)
        => _transactionRepository = transactionRepository;

    public async Task Handle(
        BasculerVisibiliteDiscussionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        transaction.Discussion.BasculerVisibilite(request.NouvelleVisibilite);
        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
    }
}
