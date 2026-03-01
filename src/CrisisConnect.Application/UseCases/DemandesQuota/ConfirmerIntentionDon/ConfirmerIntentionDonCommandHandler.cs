using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.ConfirmerIntentionDon;

public class ConfirmerIntentionDonCommandHandler : IRequestHandler<ConfirmerIntentionDonCommand, DemandeQuotaDto>
{
    private readonly IDemandeQuotaRepository _repository;

    public ConfirmerIntentionDonCommandHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeQuotaDto> Handle(ConfirmerIntentionDonCommand request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.DemandeQuotaId, cancellationToken)
            ?? throw new NotFoundException("DemandeQuota", request.DemandeQuotaId);

        var intention = demande.Intentions.FirstOrDefault(i => i.Id == request.IntentionId)
            ?? throw new NotFoundException("IntentionDon", request.IntentionId);

        intention.Confirmer();
        await _repository.UpdateAsync(demande, cancellationToken);

        return AppMapper.ToDto(demande);
    }
}
