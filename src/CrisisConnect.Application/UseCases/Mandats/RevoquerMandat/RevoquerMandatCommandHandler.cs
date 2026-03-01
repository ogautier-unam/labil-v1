using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;

public class RevoquerMandatCommandHandler : ICommandHandler<RevoquerMandatCommand>
{
    private readonly IMandatRepository _repository;

    public RevoquerMandatCommandHandler(IMandatRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<Unit> Handle(RevoquerMandatCommand request, CancellationToken cancellationToken)
    {
        var mandat = await _repository.GetByIdAsync(request.MandatId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Mandat), request.MandatId);

        mandat.Revoquer();
        await _repository.UpdateAsync(mandat, cancellationToken);
        return Unit.Value;
    }
}
