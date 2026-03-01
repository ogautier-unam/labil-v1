using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.RefuserIntentionDon;

public class RefuserIntentionDonCommandHandler : IRequestHandler<RefuserIntentionDonCommand, DemandeQuotaDto>
{
    private readonly IDemandeQuotaRepository _repository;

    public RefuserIntentionDonCommandHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeQuotaDto> Handle(RefuserIntentionDonCommand request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.DemandeQuotaId, cancellationToken)
            ?? throw new NotFoundException("DemandeQuota", request.DemandeQuotaId);

        var intention = demande.Intentions.FirstOrDefault(i => i.Id == request.IntentionId)
            ?? throw new NotFoundException("IntentionDon", request.IntentionId);

        intention.Refuser();
        await _repository.UpdateAsync(demande, cancellationToken);

        return AppMapper.ToDto(demande);
    }
}
