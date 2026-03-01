using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;

public class ReconfirmerPropositionCommandHandler : IRequestHandler<ReconfirmerPropositionCommand>
{
    private readonly IPropositionRepository _repository;

    public ReconfirmerPropositionCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async Task Handle(ReconfirmerPropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Reconfirmer();
        await _repository.UpdateAsync(proposition, cancellationToken);
    }
}
