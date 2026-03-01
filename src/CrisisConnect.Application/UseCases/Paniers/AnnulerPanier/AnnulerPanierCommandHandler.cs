using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;

public class AnnulerPanierCommandHandler : ICommandHandler<AnnulerPanierCommand>
{
    private readonly IPanierRepository _panierRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationService _notificationService;

    public AnnulerPanierCommandHandler(
        IPanierRepository panierRepository,
        INotificationRepository notificationRepository,
        INotificationService notificationService)
    {
        _panierRepository = panierRepository;
        _notificationRepository = notificationRepository;
        _notificationService = notificationService;
    }

    public async ValueTask<Unit> Handle(AnnulerPanierCommand request, CancellationToken cancellationToken)
    {
        var panier = await _panierRepository.GetByIdAsync(request.PanierId, cancellationToken)
            ?? throw new NotFoundException("Panier", request.PanierId);

        var offresLiberades = panier.Offres.ToList();
        panier.Annuler();
        await _panierRepository.UpdateAsync(panier, cancellationToken);

        // Notifier les propriétaires des offres remises disponibles
        var proprietairesNotifies = new HashSet<Guid>();
        foreach (var offre in offresLiberades)
        {
            if (proprietairesNotifies.Add(offre.CreePar))
            {
                var notification = new Notification(
                    offre.CreePar,
                    TypeNotification.PanierAnnuleOffreRemiseActive,
                    $"Votre offre « {offre.Titre} » est à nouveau disponible suite à l'annulation d'un panier.",
                    refEntiteId: offre.Id.ToString());
                await _notificationRepository.AddAsync(notification, cancellationToken);
                await _notificationService.EnvoyerAsync(
                    offre.CreePar, "Offre remise disponible", notification.Contenu, cancellationToken);
            }
        }

        return Unit.Value;
    }
}
