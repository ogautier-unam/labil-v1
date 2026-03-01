using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.CreatePanier;

public class CreatePanierCommandHandler : IRequestHandler<CreatePanierCommand, PanierDto>
{
    private readonly IPanierRepository _panierRepository;
    private readonly AppMapper _mapper;

    public CreatePanierCommandHandler(IPanierRepository panierRepository, AppMapper mapper)
    {
        _panierRepository = panierRepository;
        _mapper = mapper;
    }

    public async ValueTask<PanierDto> Handle(CreatePanierCommand request, CancellationToken cancellationToken)
    {
        var existants = await _panierRepository.GetByProprietaireAsync(request.ProprietaireId, cancellationToken);
        if (existants.Any(p => p.Statut == StatutPanier.Ouvert))
            throw new DomainException("Un panier ouvert existe déjà pour cet acteur.");

        var panier = new Panier(request.ProprietaireId);
        await _panierRepository.AddAsync(panier, cancellationToken);
        return _mapper.ToDto(panier);
    }
}
