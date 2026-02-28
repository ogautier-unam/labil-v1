using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;

public class ConfirmerPanierCommandHandler : IRequestHandler<ConfirmerPanierCommand>
{
    private readonly IPanierRepository _panierRepository;

    public ConfirmerPanierCommandHandler(IPanierRepository panierRepository)
    {
        _panierRepository = panierRepository;
    }

    public async Task Handle(ConfirmerPanierCommand request, CancellationToken cancellationToken)
    {
        var panier = await _panierRepository.GetByIdAsync(request.PanierId, cancellationToken)
            ?? throw new NotFoundException("Panier", request.PanierId);

        panier.Confirmer();
        await _panierRepository.UpdateAsync(panier, cancellationToken);
    }
}
