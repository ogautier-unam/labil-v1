using CrisisConnect.Application.Common.Interfaces;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CrisisConnect.Application.Common.Behaviours;

/// <summary>
/// Pipeline behavior qui persiste une EntreeJournal pour chaque commande exécutée avec succès.
/// Les Query ne sont pas auditées (leurs noms se terminent par "Query").
/// </summary>
public class AuditBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IMessage
{
    private readonly ICurrentUserService _currentUser;
    private readonly IEntreeJournalRepository _journalRepository;
    private readonly ILogger<AuditBehaviour<TRequest, TResponse>> _logger;

    private static readonly Dictionary<string, TypeOperation> _commandMap = new()
    {
        ["LoginCommand"]                    = TypeOperation.Connexion,
        ["LogoutCommand"]                   = TypeOperation.Deconnexion,
        ["RegisterActeurCommand"]           = TypeOperation.CreationCompte,
        ["CreateOffreCommand"]              = TypeOperation.DepotProposition,
        ["CreateDemandeCommand"]            = TypeOperation.DepotProposition,
        ["InitierTransactionCommand"]       = TypeOperation.DebutTransaction,
        ["ConfirmerTransactionCommand"]     = TypeOperation.ConfirmationTransaction,
        ["AnnulerTransactionCommand"]       = TypeOperation.AnnulationTransaction,
        ["CreatePanierCommand"]             = TypeOperation.CreationPanier,
        ["ConfirmerPanierCommand"]          = TypeOperation.ConfirmationPanier,
        ["AnnulerPanierCommand"]            = TypeOperation.AnnulationPanier,
        ["AjouterOffreAuPanierCommand"]     = TypeOperation.ModificationProposition,
        ["CreateConfigCatastropheCommand"]  = TypeOperation.ModificationConfigCatastrophe,
        ["MarkNotificationAsReadCommand"]    = TypeOperation.RetablissementConfiance,
        ["RefreshTokenCommand"]              = TypeOperation.RetablissementConfiance,
        ["AcknowledgeSuggestionCommand"]         = TypeOperation.AcquittementSuggestion,
        ["GenererSuggestionsCommand"]             = TypeOperation.GenerationSuggestion,
        ["ArchiverPropositionCommand"]            = TypeOperation.ArchivageProposition,
        ["MarquerEnAttenteRelanceCommand"]        = TypeOperation.ModificationProposition,
        ["ReconfirmerPropositionCommand"]         = TypeOperation.ModificationProposition,
        ["RecyclerPropositionCommand"]            = TypeOperation.RecyclageProposition,
        ["ClorePropositionCommand"]                   = TypeOperation.ClotureProposition,
        ["EnvoyerMessageCommand"]                     = TypeOperation.EnvoiMessage,
        ["BasculerVisibiliteDiscussionCommand"]       = TypeOperation.BasculeVisibiliteDiscussion,
        ["UpdateConfigCatastropheCommand"]            = TypeOperation.ModificationConfigCatastrophe,
        ["AttribuerRoleCommand"]                      = TypeOperation.AttributionRole,
        ["RevoquerRoleCommand"]                       = TypeOperation.RevocationRole,
        ["CreerMandatCommand"]                        = TypeOperation.MandatCree,
        ["RevoquerMandatCommand"]                     = TypeOperation.MandatRevoque,
        ["CreateCategorieCommand"]                    = TypeOperation.ModificationTaxonomie,
        ["DesactiverCategorieCommand"]                = TypeOperation.ModificationTaxonomie,
        ["CreateEntiteCommand"]                       = TypeOperation.EntiteAjoutee,
        ["DesactiverEntiteCommand"]                   = TypeOperation.EntiteSupprimee,
        ["VerifierMethodeCommand"]                    = TypeOperation.RetablissementConfiance,
    };

    public AuditBehaviour(
        ICurrentUserService currentUser,
        IEntreeJournalRepository journalRepository,
        ILogger<AuditBehaviour<TRequest, TResponse>> logger)
    {
        _currentUser = currentUser;
        _journalRepository = journalRepository;
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        // N'audite que les commandes (pas les queries)
        if (requestName.EndsWith("Query", StringComparison.Ordinal))
            return await next(message, cancellationToken);

        var response = await next(message, cancellationToken);

        // Après succès : persiste l'entrée d'audit
        try
        {
            var acteurId = _currentUser.UserId ?? Guid.Empty;
            var typeOperation = _commandMap.TryGetValue(requestName, out var op)
                ? op
                : TypeOperation.ModificationProposition; // fallback

            var entree = new EntreeJournal(acteurId, typeOperation, details: requestName);
            await _journalRepository.AddAsync(entree, cancellationToken);
        }
        catch (Exception ex)
        {
            // L'audit ne doit jamais bloquer la commande principale
            _logger.LogWarning(ex, "Impossible d'écrire l'entrée journal pour {Request}", requestName);
        }

        return response;
    }
}
