using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.CloreProposition;

public class ClorePropositionCommandHandler : IRequestHandler<ClorePropositionCommand>
{
    private readonly IPropositionRepository _repository;

    public ClorePropositionCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async Task Handle(ClorePropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Clore();
        await _repository.UpdateAsync(proposition, cancellationToken);
    }
}
