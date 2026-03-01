using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.InitierTransaction;

public class InitierTransactionCommandHandler : IRequestHandler<InitierTransactionCommand, TransactionDto>
{
    private readonly IPropositionRepository _propositionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;
    private readonly AppMapper _mapper;

    public InitierTransactionCommandHandler(
        IPropositionRepository propositionRepository,
        ITransactionRepository transactionRepository,
        INotificationRepository notificationRepository,
        INotificationService notificationService,
        AppMapper mapper)
    {
        _propositionRepository = propositionRepository;
        _transactionRepository = transactionRepository;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async ValueTask<TransactionDto> Handle(InitierTransactionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _propositionRepository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.MarquerEnTransaction();

        var transaction = new Transaction(request.PropositionId, request.InitiateurId);
        await _transactionRepository.AddAsync(transaction, cancellationToken);

        // Notifier le propriétaire de la proposition qu'une transaction a été initiée
        var notification = new Notification(
            proposition.CreePar,
            TypeNotification.TransactionInitiee,
            $"Une transaction a été initiée sur votre proposition « {proposition.Titre} ».",
            refEntiteId: transaction.Id.ToString());
        await _notificationRepository.AddAsync(notification, cancellationToken);
        await _notificationService.EnvoyerAsync(
            proposition.CreePar, "Transaction initiée", notification.Contenu, cancellationToken);

        return _mapper.ToDto(transaction);
    }
}
