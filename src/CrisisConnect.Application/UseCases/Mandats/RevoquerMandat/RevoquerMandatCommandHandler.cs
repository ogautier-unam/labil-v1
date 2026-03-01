using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;

public class RevoquerMandatCommandHandler : IRequestHandler<RevoquerMandatCommand>
{
    private readonly IMandatRepository _repository;

    public RevoquerMandatCommandHandler(IMandatRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RevoquerMandatCommand request, CancellationToken cancellationToken)
    {
        var mandat = await _repository.GetByIdAsync(request.MandatId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Mandat), request.MandatId);

        mandat.Revoquer();
        await _repository.UpdateAsync(mandat, cancellationToken);
    }
}
