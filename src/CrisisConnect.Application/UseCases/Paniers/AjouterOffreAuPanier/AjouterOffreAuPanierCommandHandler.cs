using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;

public class AjouterOffreAuPanierCommandHandler : IRequestHandler<AjouterOffreAuPanierCommand, PanierDto>
{
    private readonly IPanierRepository _panierRepository;
    private readonly IOffreRepository _offreRepository;
    private readonly IMapper _mapper;

    public AjouterOffreAuPanierCommandHandler(
        IPanierRepository panierRepository,
        IOffreRepository offreRepository,
        IMapper mapper)
    {
        _panierRepository = panierRepository;
        _offreRepository = offreRepository;
        _mapper = mapper;
    }

    public async Task<PanierDto> Handle(AjouterOffreAuPanierCommand request, CancellationToken cancellationToken)
    {
        var panier = await _panierRepository.GetByIdAsync(request.PanierId, cancellationToken)
            ?? throw new NotFoundException("Panier", request.PanierId);

        var offre = await _offreRepository.GetByIdAsync(request.OffreId, cancellationToken)
            ?? throw new NotFoundException("Offre", request.OffreId);

        panier.AjouterOffre(offre);
        await _panierRepository.UpdateAsync(panier, cancellationToken);
        return _mapper.Map<PanierDto>(panier);
    }
}
