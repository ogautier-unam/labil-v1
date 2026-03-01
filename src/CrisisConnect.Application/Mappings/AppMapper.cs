using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CrisisConnect.Application.Mappings;

/// <summary>
/// Mapper source-generated par Riok.Mapperly (Apache-2.0).
/// EnumMappingStrategy.ByName : toutes les conversions enum → string utilisent .ToString().
/// </summary>
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class AppMapper
{
    // Propositions
    public partial PropositionDto ToDto(Proposition proposition);
    public partial OffreDto ToDto(Offre offre);
    public partial List<OffreDto> ToDto(List<Offre> offres);
    public partial DemandeDto ToDto(Demande demande);
    public partial List<DemandeDto> ToDto(List<Demande> demandes);

    // Transactions
    public partial TransactionDto ToDto(Transaction transaction);
    public partial List<TransactionDto> ToDto(List<Transaction> transactions);
    public partial DiscussionDto ToDto(Discussion discussion);
    public partial MessageDto ToDto(Message message);
    public partial List<MessageDto> ToDto(List<Message> messages);

    // Paniers
    public partial PanierDto ToDto(Panier panier);

    // Notifications
    public partial NotificationDto ToDto(Notification notification);
    public partial List<NotificationDto> ToDto(List<Notification> notifications);

    // Journal
    public partial EntreeJournalDto ToDto(EntreeJournal entree);
    public partial List<EntreeJournalDto> ToDto(List<EntreeJournal> entrees);

    // Suggestions
    public partial SuggestionAppariementDto ToDto(SuggestionAppariement suggestion);
    public partial List<SuggestionAppariementDto> ToDto(List<SuggestionAppariement> suggestions);

    // Config catastrophe
    public partial ConfigCatastropheDto ToDto(ConfigCatastrophe config);

    // Rôles & Mandats
    public partial AttributionRoleDto ToDto(AttributionRole role);
    public partial List<AttributionRoleDto> ToDto(List<AttributionRole> roles);
    public partial MandatDto ToDto(Mandat mandat);
    public partial List<MandatDto> ToDto(List<Mandat> mandats);

    // Taxonomie & Entités
    public partial CategorieTaxonomieDto ToDto(CategorieTaxonomie categorie);
    public partial List<CategorieTaxonomieDto> ToDto(List<CategorieTaxonomie> categories);
    public partial EntiteDto ToDto(Entite entite);
    public partial List<EntiteDto> ToDto(List<Entite> entites);

    // Méthodes d'identification — mapping manuel : TypeMethode = nom du type runtime (TPH)
    public MethodeIdentificationDto ToDto(MethodeIdentification src) =>
        new(src.Id, src.PersonneId, src.GetType().Name,
            src.NiveauFiabilite.ToString(), src.EstVerifiee, src.DateVerification);

    public List<MethodeIdentificationDto> ToDto(List<MethodeIdentification> list) =>
        list.ConvertAll(ToDto);

    // Surcharges IReadOnlyList<T> → délèguent vers List<T> pour compatibilité repositories
    public List<OffreDto> ToDto(IReadOnlyList<Offre> items) => ToDto(items as List<Offre> ?? items.ToList());
    public List<DemandeDto> ToDto(IReadOnlyList<Demande> items) => ToDto(items as List<Demande> ?? items.ToList());
    public List<TransactionDto> ToDto(IReadOnlyList<Transaction> items) => ToDto(items as List<Transaction> ?? items.ToList());
    public List<NotificationDto> ToDto(IReadOnlyList<Notification> items) => ToDto(items as List<Notification> ?? items.ToList());
    public List<EntreeJournalDto> ToDto(IReadOnlyList<EntreeJournal> items) => ToDto(items as List<EntreeJournal> ?? items.ToList());
    public List<SuggestionAppariementDto> ToDto(IReadOnlyList<SuggestionAppariement> items) => ToDto(items as List<SuggestionAppariement> ?? items.ToList());
    public List<AttributionRoleDto> ToDto(IReadOnlyList<AttributionRole> items) => ToDto(items as List<AttributionRole> ?? items.ToList());
    public List<MandatDto> ToDto(IReadOnlyList<Mandat> items) => ToDto(items as List<Mandat> ?? items.ToList());
    public List<CategorieTaxonomieDto> ToDto(IReadOnlyList<CategorieTaxonomie> items) => ToDto(items as List<CategorieTaxonomie> ?? items.ToList());
    public List<EntiteDto> ToDto(IReadOnlyList<Entite> items) => ToDto(items as List<Entite> ?? items.ToList());
    public List<MethodeIdentificationDto> ToDto(IReadOnlyList<MethodeIdentification> items) => ToDto(items as List<MethodeIdentification> ?? items.ToList());
    public List<PropositionDto> ToDto(IReadOnlyList<Proposition> items) => items.Select(ToDto).ToList();
}
