using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;

public class ArchiverPropositionCommandHandler : ICommandHandler<ArchiverPropositionCommand>
{
    private readonly IPropositionRepository _repository;

    public ArchiverPropositionCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async ValueTask<Unit> Handle(ArchiverPropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Archiver();
        await _repository.UpdateAsync(proposition, cancellationToken);
        return Unit.Value;
    }
}
