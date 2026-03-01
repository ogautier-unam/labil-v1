using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Roles.RevoquerRole;

public class RevoquerRoleCommandHandler : ICommandHandler<RevoquerRoleCommand>
{
    private readonly IAttributionRoleRepository _repository;

    public RevoquerRoleCommandHandler(IAttributionRoleRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<Unit> Handle(RevoquerRoleCommand request, CancellationToken cancellationToken)
    {
        var attribution = await _repository.GetByIdAsync(request.AttributionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.AttributionRole), request.AttributionId);

        attribution.Expirer();
        await _repository.UpdateAsync(attribution, cancellationToken);
        return Unit.Value;
    }
}
