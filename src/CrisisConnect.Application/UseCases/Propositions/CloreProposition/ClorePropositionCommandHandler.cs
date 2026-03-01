using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.CloreProposition;

public class ClorePropositionCommandHandler : ICommandHandler<ClorePropositionCommand>
{
    private readonly IPropositionRepository _propositionRepository;
    private readonly IDemandeRepository _demandeRepository;

    public ClorePropositionCommandHandler(IPropositionRepository propositionRepository, IDemandeRepository demandeRepository)
    {
        _propositionRepository = propositionRepository;
        _demandeRepository = demandeRepository;
    }

    public async ValueTask<Unit> Handle(ClorePropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _propositionRepository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Clore();
        await _propositionRepository.UpdateAsync(proposition, cancellationToken);

        // Logique OU ascendante : si la demande clôturée appartient à un parent OU, fermer les alternatives sœurs
        if (proposition is Demande demande && demande.ParentId.HasValue)
        {
            var parent = await _demandeRepository.GetByIdAsync(demande.ParentId.Value, cancellationToken);
            if (parent is not null)
            {
                parent.ClorerAlternativesOu(demande.Id);
                await _demandeRepository.UpdateAsync(parent, cancellationToken);
            }
        }

        return Unit.Value;
    }
}
