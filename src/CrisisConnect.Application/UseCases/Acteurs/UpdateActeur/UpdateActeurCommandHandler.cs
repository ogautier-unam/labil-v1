using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Acteurs.UpdateActeur;

public class UpdateActeurCommandHandler : IRequestHandler<UpdateActeurCommand, PersonneDto>
{
    private readonly IPersonneRepository _repository;

    public UpdateActeurCommandHandler(IPersonneRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<PersonneDto> Handle(UpdateActeurCommand request, CancellationToken cancellationToken)
    {
        var personne = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Personne), request.Id);

        Adresse? adresse = null;
        if (!string.IsNullOrWhiteSpace(request.AdresseRue)
            && !string.IsNullOrWhiteSpace(request.AdresseVille)
            && !string.IsNullOrWhiteSpace(request.AdresseCodePostal))
        {
            adresse = new Adresse(request.AdresseRue, request.AdresseVille,
                request.AdresseCodePostal, request.AdressePays ?? "France");
        }

        personne.ModifierProfil(request.Prenom, request.Nom, request.Telephone,
            request.UrlPhoto, request.LanguePreferee, request.MoyensContact, adresse);

        await _repository.UpdateAsync(personne, cancellationToken);

        return AppMapper.ToDto(personne);
    }
}
