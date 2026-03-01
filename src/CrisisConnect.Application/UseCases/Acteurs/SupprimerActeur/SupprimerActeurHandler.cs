using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Acteurs.SupprimerActeur;

public class SupprimerActeurHandler : IRequestHandler<SupprimerActeurCommand, Unit>
{
    private readonly IPersonneRepository _personneRepository;

    public SupprimerActeurHandler(IPersonneRepository personneRepository)
    {
        _personneRepository = personneRepository;
    }

    public async ValueTask<Unit> Handle(SupprimerActeurCommand request, CancellationToken cancellationToken)
    {
        var personne = await _personneRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Acteur", request.Id);

        personne.Anonymiser();
        await _personneRepository.UpdateAsync(personne, cancellationToken);

        return Unit.Value;
    }
}
