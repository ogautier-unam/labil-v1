using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;

public class ArchiverPropositionCommandHandler : IRequestHandler<ArchiverPropositionCommand>
{
    private readonly IPropositionRepository _repository;

    public ArchiverPropositionCommandHandler(IPropositionRepository repository)
        => _repository = repository;

    public async Task Handle(ArchiverPropositionCommand request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.PropositionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.PropositionId);

        proposition.Archiver();
        await _repository.UpdateAsync(proposition, cancellationToken);
    }
}
