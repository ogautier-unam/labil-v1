using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;

public class AnnulerPanierCommandHandler : ICommandHandler<AnnulerPanierCommand>
{
    private readonly IPanierRepository _panierRepository;

    public AnnulerPanierCommandHandler(IPanierRepository panierRepository)
    {
        _panierRepository = panierRepository;
    }

    public async ValueTask<Unit> Handle(AnnulerPanierCommand request, CancellationToken cancellationToken)
    {
        var panier = await _panierRepository.GetByIdAsync(request.PanierId, cancellationToken)
            ?? throw new NotFoundException("Panier", request.PanierId);

        panier.Annuler();
        await _panierRepository.UpdateAsync(panier, cancellationToken);
        return Unit.Value;
    }
}
