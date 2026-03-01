using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;

namespace CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;

public class EnvoyerMessageCommandHandler : IRequestHandler<EnvoyerMessageCommand, MessageDto>
{
    private const string LangueSysteme = "fr";

    private readonly ITransactionRepository _transactionRepository;
    private readonly IServiceTraduction _serviceTraduction;
    private readonly AppMapper _mapper;

    public EnvoyerMessageCommandHandler(
        ITransactionRepository transactionRepository,
        IServiceTraduction serviceTraduction,
        AppMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _serviceTraduction = serviceTraduction;
        _mapper = mapper;
    }

    public async ValueTask<MessageDto> Handle(EnvoyerMessageCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transaction), request.TransactionId);

        // Traduction automatique si la langue de l'expéditeur diffère de la langue système
        var langueSource = string.IsNullOrWhiteSpace(request.Langue) ? LangueSysteme : request.Langue;
        string contenuFinal = request.Contenu;
        string? texteOriginal = null;
        bool traduit = false;

        if (!string.Equals(langueSource, LangueSysteme, StringComparison.OrdinalIgnoreCase))
        {
            texteOriginal = request.Contenu;
            contenuFinal = await _serviceTraduction.TraduireAsync(
                request.Contenu, langueSource, LangueSysteme, cancellationToken);
            traduit = true;
        }

        var message = transaction.Discussion.AjouterMessage(
            request.ExpediteurId, contenuFinal, LangueSysteme,
            issueTraductionAuto: traduit, texteOriginal: texteOriginal);

        await _transactionRepository.UpdateAsync(transaction, cancellationToken);

        return _mapper.ToDto(message);
    }
}
