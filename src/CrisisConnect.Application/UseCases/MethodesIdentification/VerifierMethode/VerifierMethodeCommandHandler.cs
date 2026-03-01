using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.MethodesIdentification.VerifierMethode;

public class VerifierMethodeCommandHandler : ICommandHandler<VerifierMethodeCommand>
{
    private readonly IMethodeIdentificationRepository _repository;

    public VerifierMethodeCommandHandler(IMethodeIdentificationRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<Unit> Handle(VerifierMethodeCommand request, CancellationToken cancellationToken)
    {
        var methode = await _repository.GetByIdAsync(request.MethodeId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.MethodeIdentification), request.MethodeId);

        methode.MarquerVerifiee();
        await _repository.UpdateAsync(methode, cancellationToken);
        return Unit.Value;
    }
}
