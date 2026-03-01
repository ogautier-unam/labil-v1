using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.RecyclerProposition;

public class RecyclerPropositionCommandHandler : ICommandHandler<RecyclerPropositionCommand>
{
    private readonly IPropositionRepository _repository;

    public RecyclerPropositionCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async ValueTask<Unit> Handle(RecyclerPropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Recycler();
        await _repository.UpdateAsync(proposition, cancellationToken);
        return Unit.Value;
    }
}
