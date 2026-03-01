using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;

public class ConfirmerTransactionCommandHandler : ICommandHandler<ConfirmerTransactionCommand>
{
    private readonly ITransactionRepository _repository;
    private readonly IPropositionRepository _propositionRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;

    public ConfirmerTransactionCommandHandler(
        ITransactionRepository repository,
        IPropositionRepository propositionRepository,
        INotificationRepository notificationRepository,
        INotificationService notificationService)
    {
        _repository = repository;
        _propositionRepository = propositionRepository;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
    }

    public async ValueTask<Unit> Handle(ConfirmerTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        transaction.Confirmer();
        await _repository.UpdateAsync(transaction, cancellationToken);

        // Clore la proposition associée
        var proposition = await _propositionRepository.GetByIdAsync(transaction.PropositionId, cancellationToken);
        if (proposition is not null)
        {
            proposition.Clore();
            await _propositionRepository.UpdateAsync(proposition, cancellationToken);

            // Notifier les deux parties
            var txId = transaction.Id.ToString();
            var contenu = $"La transaction sur « {proposition.Titre} » a été confirmée.";

            var notifInitiateur = new Notification(
                transaction.InitiateurId, TypeNotification.TransactionConfirmee, contenu, refEntiteId: txId);
            var notifProprietaire = new Notification(
                proposition.CreePar, TypeNotification.TransactionConfirmee, contenu, refEntiteId: txId);

            await _notificationRepository.AddAsync(notifInitiateur, cancellationToken);
            await _notificationRepository.AddAsync(notifProprietaire, cancellationToken);
            await _notificationService.EnvoyerAsync(
                transaction.InitiateurId, "Transaction confirmée", contenu, cancellationToken);
            await _notificationService.EnvoyerAsync(
                proposition.CreePar, "Transaction confirmée", contenu, cancellationToken);
        }

        return Unit.Value;
    }
}
