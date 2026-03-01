using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;

public class MarquerEnAttenteRelanceCommandHandler : ICommandHandler<MarquerEnAttenteRelanceCommand>
{
    private readonly IPropositionRepository _repository;

    public MarquerEnAttenteRelanceCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async ValueTask<Unit> Handle(MarquerEnAttenteRelanceCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.MarquerEnAttenteRelance();
        await _repository.UpdateAsync(proposition, cancellationToken);
        return Unit.Value;
    }
}
