using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.GetDemandeQuotaById;

public class GetDemandeQuotaByIdQueryHandler : IRequestHandler<GetDemandeQuotaByIdQuery, DemandeQuotaDto>
{
    private readonly IDemandeQuotaRepository _repository;

    public GetDemandeQuotaByIdQueryHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeQuotaDto> Handle(GetDemandeQuotaByIdQuery request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("DemandeQuota", request.Id);

        return AppMapper.ToDto(demande);
    }
}
