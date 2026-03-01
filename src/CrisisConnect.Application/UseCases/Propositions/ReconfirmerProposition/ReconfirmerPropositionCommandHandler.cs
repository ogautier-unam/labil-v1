using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;

public class ReconfirmerPropositionCommandHandler : ICommandHandler<ReconfirmerPropositionCommand>
{
    private readonly IPropositionRepository _repository;

    public ReconfirmerPropositionCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async ValueTask<Unit> Handle(ReconfirmerPropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Reconfirmer();
        await _repository.UpdateAsync(proposition, cancellationToken);
        return Unit.Value;
    }
}
