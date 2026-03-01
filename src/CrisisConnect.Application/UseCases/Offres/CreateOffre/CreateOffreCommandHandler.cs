using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.CreateOffre;

public class CreateOffreCommandHandler : IRequestHandler<CreateOffreCommand, OffreDto>
{
    private readonly IOffreRepository _repository;
    private readonly IDemandeRepository _demandeRepository;

    public CreateOffreCommandHandler(IOffreRepository repository, IDemandeRepository demandeRepository)
    {
        _repository = repository;
        _demandeRepository = demandeRepository;
    }

    public async ValueTask<OffreDto> Handle(CreateOffreCommand request, CancellationToken cancellationToken)
    {
        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        var offre = new Offre(request.Titre, request.Description, request.CreePar, request.LivraisonIncluse, localisation);

        foreach (var demandeId in request.DemandeIds ?? [])
        {
            var demande = await _demandeRepository.GetByIdAsync(demandeId, cancellationToken);
            if (demande is not null)
                offre.CouplerDemande(demande);
        }

        await _repository.AddAsync(offre, cancellationToken);

        return AppMapper.ToDto(offre);
    }
}
