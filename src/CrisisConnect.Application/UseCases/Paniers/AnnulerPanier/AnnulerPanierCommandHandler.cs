using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;

public class AnnulerPanierCommandHandler : IRequestHandler<AnnulerPanierCommand>
{
    private readonly IPanierRepository _panierRepository;

    public AnnulerPanierCommandHandler(IPanierRepository panierRepository)
    {
        _panierRepository = panierRepository;
    }

    public async Task Handle(AnnulerPanierCommand request, CancellationToken cancellationToken)
    {
        var panier = await _panierRepository.GetByIdAsync(request.PanierId, cancellationToken)
            ?? throw new NotFoundException("Panier", request.PanierId);

        panier.Annuler();
        await _panierRepository.UpdateAsync(panier, cancellationToken);
    }
}
