using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;

public class AnnulerTransactionCommandHandler : IRequestHandler<AnnulerTransactionCommand>
{
    private readonly ITransactionRepository _repository;
    private readonly IPropositionRepository _propositionRepository;

    public AnnulerTransactionCommandHandler(ITransactionRepository repository, IPropositionRepository propositionRepository)
    {
        _repository = repository;
        _propositionRepository = propositionRepository;
    }

    public async Task Handle(AnnulerTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        transaction.Annuler();
        await _repository.UpdateAsync(transaction, cancellationToken);

        // Libérer la proposition pour qu'elle soit de nouveau disponible
        var proposition = await _propositionRepository.GetByIdAsync(transaction.PropositionId, cancellationToken);
        if (proposition is not null && proposition.Statut == Domain.Enums.StatutProposition.EnTransaction)
        {
            proposition.LibererDeTransaction();
            await _propositionRepository.UpdateAsync(proposition, cancellationToken);
        }
    }
}
