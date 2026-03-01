using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.SoumettreIntentionDon;

public class SoumettreIntentionDonCommandHandler : IRequestHandler<SoumettreIntentionDonCommand, IntentionDonDto>
{
    private readonly IDemandeQuotaRepository _repository;

    public SoumettreIntentionDonCommandHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IntentionDonDto> Handle(SoumettreIntentionDonCommand request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.DemandeQuotaId, cancellationToken)
            ?? throw new NotFoundException("DemandeQuota", request.DemandeQuotaId);

        var intention = demande.AjouterIntention(
            request.ActeurId, request.Quantite, request.Unite, request.Description);

        await _repository.UpdateAsync(demande, cancellationToken);

        return AppMapper.ToDto(intention);
    }
}
