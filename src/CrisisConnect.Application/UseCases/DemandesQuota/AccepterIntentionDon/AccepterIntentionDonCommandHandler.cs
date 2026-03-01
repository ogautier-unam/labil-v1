using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.AccepterIntentionDon;

public class AccepterIntentionDonCommandHandler : IRequestHandler<AccepterIntentionDonCommand, DemandeQuotaDto>
{
    private readonly IDemandeQuotaRepository _repository;

    public AccepterIntentionDonCommandHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeQuotaDto> Handle(AccepterIntentionDonCommand request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.DemandeQuotaId, cancellationToken)
            ?? throw new NotFoundException("DemandeQuota", request.DemandeQuotaId);

        demande.ValiderIntention(request.IntentionId);
        await _repository.UpdateAsync(demande, cancellationToken);

        return AppMapper.ToDto(demande);
    }
}
