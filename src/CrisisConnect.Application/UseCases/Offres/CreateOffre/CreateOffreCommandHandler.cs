using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.CreateOffre;

public class CreateOffreCommandHandler : IRequestHandler<CreateOffreCommand, OffreDto>
{
    private readonly IOffreRepository _repository;
    private readonly AppMapper _mapper;

    public CreateOffreCommandHandler(IOffreRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<OffreDto> Handle(CreateOffreCommand request, CancellationToken cancellationToken)
    {
        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        var offre = new Offre(request.Titre, request.Description, request.CreePar, request.LivraisonIncluse, localisation);
        await _repository.AddAsync(offre, cancellationToken);

        return _mapper.ToDto(offre);
    }
}
