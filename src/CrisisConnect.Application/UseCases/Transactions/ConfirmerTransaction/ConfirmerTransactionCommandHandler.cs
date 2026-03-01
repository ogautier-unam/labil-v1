using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;

public class ConfirmerTransactionCommandHandler : ICommandHandler<ConfirmerTransactionCommand>
{
    private readonly ITransactionRepository _repository;
    private readonly IPropositionRepository _propositionRepository;

    public ConfirmerTransactionCommandHandler(ITransactionRepository repository, IPropositionRepository propositionRepository)
    {
        _repository = repository;
        _propositionRepository = propositionRepository;
    }

    public async ValueTask<Unit> Handle(ConfirmerTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        transaction.Confirmer();
        await _repository.UpdateAsync(transaction, cancellationToken);

        // Clore la proposition associ�e
        var proposition = await _propositionRepository.GetByIdAsync(transaction.PropositionId, cancellationToken);
        if (proposition is not null)
        {
            proposition.Clore();
            await _propositionRepository.UpdateAsync(proposition, cancellationToken);
        }
        return Unit.Value;
    }
}
