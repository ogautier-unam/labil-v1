using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.UpdateOffre;

public class UpdateOffreCommandHandler : IRequestHandler<UpdateOffreCommand, OffreDto>
{
    private readonly IOffreRepository _repository;

    public UpdateOffreCommandHandler(IOffreRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<OffreDto> Handle(UpdateOffreCommand request, CancellationToken cancellationToken)
    {
        var offre = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Offre), request.Id);

        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        offre.Modifier(request.Titre, request.Description, request.LivraisonIncluse, localisation);
        await _repository.UpdateAsync(offre, cancellationToken);

        return AppMapper.ToDto(offre);
    }
}
