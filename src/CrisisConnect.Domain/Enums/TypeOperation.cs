namespace CrisisConnect.Domain.Enums;

public enum TypeOperation
{
    // Comptes & Sessions
    Connexion,
    Deconnexion,
    CreationCompte,
    RetablissementConfiance,
    // Propositions
    DepotProposition,
    ModificationProposition,
    ClotureProposition,
    ArchivageProposition,
    RecyclageProposition,
    // Transactions & Matching
    DebutTransaction,
    ConfirmationTransaction,
    AnnulationTransaction,
    EnvoiMessage,
    BasculeVisibiliteDiscussion,
    // Paniers
    CreationPanier,
    ConfirmationPanier,
    AnnulationPanier,
    // Administration
    AttributionRole,
    RevocationRole,
    MandatCree,
    MandatRevoque,
    EntiteAjoutee,
    EntiteSupprimee,
    ModificationTaxonomie,
    ModificationConfigCatastrophe
}
