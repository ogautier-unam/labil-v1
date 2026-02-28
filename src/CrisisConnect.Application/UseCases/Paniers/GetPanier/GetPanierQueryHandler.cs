using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Paniers.GetPanier;

public class GetPanierQueryHandler : IRequestHandler<GetPanierQuery, PanierDto?>
{
    private readonly IPanierRepository _panierRepository;
    private readonly IMapper _mapper;

    public GetPanierQueryHandler(IPanierRepository panierRepository, IMapper mapper)
    {
        _panierRepository = panierRepository;
        _mapper = mapper;
    }

    public async Task<PanierDto?> Handle(GetPanierQuery request, CancellationToken cancellationToken)
    {
        var paniers = await _panierRepository.GetByProprietaireAsync(request.ProprietaireId, cancellationToken);
        var panier = paniers.FirstOrDefault(p => p.Statut == StatutPanier.Ouvert);
        return panier is null ? null : _mapper.Map<PanierDto>(panier);
    }
}
