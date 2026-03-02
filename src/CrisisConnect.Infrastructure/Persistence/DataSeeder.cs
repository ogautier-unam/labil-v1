using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CrisisConnect.Infrastructure.Persistence;

/// <summary>
/// Données de test cohérentes couvrant tous les use cases.
/// Appliqué automatiquement au démarrage si la base est vide.
/// Comptes : alice/bob/carol/david @test.com — mot de passe : Test1234!
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db, ILogger logger, CancellationToken ct = default)
    {
        if (await db.Personnes.AnyAsync(ct))
        {
            logger.LogInformation("DataSeeder: données déjà présentes, skip.");
            return;
        }

        logger.LogInformation("DataSeeder: base vide — création des données de test…");

        // ── 1. Acteurs ──────────────────────────────────────────────────────────
        var pwd = BCrypt.Net.BCrypt.HashPassword("Test1234!");

        var alice = new Personne("alice@test.com", pwd, "Citoyen",       "Alice", "Dupont");
        var bob   = new Personne("bob@test.com",   pwd, "Benevole",      "Bob",   "Martin");
        var carol = new Personne("carol@test.com", pwd, "Coordinateur",  "Carol", "Lemaire");
        var david = new Personne("david@test.com", pwd, "Responsable",   "David", "Roux");

        db.Personnes.AddRange(alice, bob, carol, david);
        await db.SaveChangesAsync(ct);

        // ── 2. Config catastrophe ───────────────────────────────────────────────
        var config = new ConfigCatastrophe(
            "Inondations Région Sud 2026",
            "Crues importantes suite aux pluies exceptionnelles de mars 2026. Plusieurs communes en vigilance rouge.",
            "Provence-Alpes-Côte d'Azur",
            "Vigilance rouge — ORSEC activé",
            delaiArchivageJours: 30,
            delaiRappelAvantArchivage: 7);

        db.ConfigsCatastrophe.Add(config);
        await db.SaveChangesAsync(ct);

        // ── 3. Taxonomie ────────────────────────────────────────────────────────
        var catLogement     = new CategorieTaxonomie("LOGEMENT",     """{"fr":"Logement","en":"Housing"}""",              config.Id);
        var catSoins        = new CategorieTaxonomie("SOINS",        """{"fr":"Soins","en":"Healthcare"}""",              config.Id);
        var catAlimentation = new CategorieTaxonomie("ALIMENTATION", """{"fr":"Alimentation","en":"Food"}""",             config.Id);
        var catTransport    = new CategorieTaxonomie("TRANSPORT",    """{"fr":"Transport","en":"Transport"}""",           config.Id);
        var catFournitures  = new CategorieTaxonomie("FOURNITURES",  """{"fr":"Fournitures","en":"Supplies"}""",          config.Id);
        var catEvacuation   = new CategorieTaxonomie("EVACUATION",   """{"fr":"Évacuation","en":"Evacuation"}""",         config.Id);
        var catInfos        = new CategorieTaxonomie("INFORMATION",  """{"fr":"Information","en":"Information"}""",       config.Id);
        var catFinancier    = new CategorieTaxonomie("AIDE_FIN",     """{"fr":"Aide financière","en":"Financial aid"}""", config.Id);
        var catLogistique   = new CategorieTaxonomie("LOGISTIQUE",   """{"fr":"Aide logistique","en":"Logistics"}""",     config.Id);

        db.CategoriesTaxonomie.AddRange(
            catLogement, catSoins, catAlimentation, catTransport, catFournitures,
            catEvacuation, catInfos, catFinancier, catLogistique);
        await db.SaveChangesAsync(ct);

        // Sous-catégorie
        var catHebergement = new CategorieTaxonomie(
            "HEBERGEMENT", """{"fr":"Hébergement d'urgence","en":"Emergency shelter"}""",
            config.Id, parentId: catLogement.Id);
        db.CategoriesTaxonomie.Add(catHebergement);
        await db.SaveChangesAsync(ct);

        // ── 4. Attributions de rôle ─────────────────────────────────────────────
        var roleCarol = new AttributionRole(carol.Id, TypeRole.AdminCatastrophe, DateTime.UtcNow,
            reconductible: true, accrediteeParId: david.Id);
        var roleDavid = new AttributionRole(david.Id, TypeRole.AdminSysteme, DateTime.UtcNow,
            reconductible: true);

        db.AttributionsRoles.AddRange(roleCarol, roleDavid);
        await db.SaveChangesAsync(ct);

        // ── 5. Offres ────────────────────────────────────────────────────────────
        var offre1 = new Offre(
            "Eau potable en bouteilles",
            "Distribution de palettes d'eau minérale (1,5 L). Livraison possible dans un rayon de 20 km.",
            carol.Id, livraisonIncluse: true);

        var offre2 = new Offre(
            "Repas chauds midi et soir",
            "Service de cuisine communautaire : 50 repas/j disponibles. Menu équilibré, régimes alimentaires respectés.",
            bob.Id, livraisonIncluse: false);

        var offre3 = new Offre(
            "Transport de personnes sinistrées",
            "Minibus 9 places disponible pour évacuation ou navette médicale. Chauffeur bénévole disponible 7j/7.",
            carol.Id, livraisonIncluse: false);

        var offre4 = new Offre(
            "Kit médicaments d'urgence",
            "Trousses de premiers soins (antalgiques, pansements, antiseptiques). 20 kits disponibles.",
            bob.Id);

        var offre5 = new Offre(
            "Hébergement 10 places",
            "Appartement T4 et garage aménagé, literie fournie. Disponible immédiatement pour sinistrés.",
            bob.Id);

        var offre6 = new Offre(
            "Vêtements chauds adultes",
            "Sacs de vêtements triés et lavés, toutes tailles adultes.",
            bob.Id);

        // Transitions de statut cohérentes avec les transactions ci-dessous
        offre4.MarquerEnAttenteRelance();   // Active → EnAttenteRelance
        offre6.Archiver();                  // Active → Archivée

        // Tx2 (Confirmée) : offre2 → EnTransaction → Cloturée
        offre2.MarquerEnTransaction();
        var tx2 = new Transaction(offre2.Id, alice.Id);
        tx2.Confirmer();
        offre2.Clore();

        // Tx1 (EnCours) : offre3 → EnTransaction
        offre3.MarquerEnTransaction();
        var tx1 = new Transaction(offre3.Id, alice.Id);

        // Tx3 (Annulée) : offre5 → EnTransaction → Active (libérée)
        offre5.MarquerEnTransaction();
        var tx3 = new Transaction(offre5.Id, alice.Id);
        tx3.Annuler();
        offre5.LibererDeTransaction();

        db.Propositions.AddRange(offre1, offre2, offre3, offre4, offre5, offre6);
        db.Transactions.AddRange(tx1, tx2, tx3);
        await db.SaveChangesAsync(ct);

        // ── 6. Demandes ──────────────────────────────────────────────────────────
        var demande1 = new Demande(
            "Besoin urgent de nourriture",
            "Famille de 5 personnes sans accès à la cuisine depuis 3 jours. Enfants en bas âge.",
            alice.Id, urgence: NiveauUrgence.Critique, regionSeverite: "Marseille 13");

        var demande2 = new Demande(
            "Hébergement famille (4 personnes)",
            "Logement inondé — cherche hébergement temporaire pour 2 adultes et 2 enfants (6 et 9 ans).",
            alice.Id, urgence: NiveauUrgence.Eleve);

        var demande3 = new Demande(
            "Transport médical d'urgence",
            "Patient dialysé, 3 séances/semaine à l'hôpital La Timone. Transport indispensable.",
            alice.Id, urgence: NiveauUrgence.Critique);

        var demande4 = new Demande(
            "Médicaments chroniques hebdomadaires",
            "Ordonnance renouvelable mensuelle, besoin de récupération médicaments chaque semaine en pharmacie.",
            alice.Id, urgence: NiveauUrgence.Moyen);

        var demande5 = new Demande(
            "Bénévoles pour logistique entrepôt",
            "Besoin de 3 bénévoles pour trier et distribuer les dons reçus au centre de collecte.",
            carol.Id, urgence: NiveauUrgence.Moyen);

        demande4.ConfigurerRecurrence(true, "Hebdomadaire");

        db.Propositions.AddRange(demande1, demande2, demande3, demande4, demande5);
        await db.SaveChangesAsync(ct);

        // ── 7. Messages dans la discussion tx1 ───────────────────────────────────
        // Insertion directe via DbSet : évite un DbUpdateConcurrencyException EF Core
        // causé par un état de change-tracker incohérent après plusieurs SaveChangesAsync enchaînés.
        db.Messages.AddRange(
            new Message(tx1.Discussion.Id, alice.Id,
                "Bonjour, je suis disponible dès ce soir pour le transport. Où est le point de rendez-vous ?"),
            new Message(tx1.Discussion.Id, carol.Id,
                "Parfait ! Rendez-vous au point de collecte Place du Général-de-Gaulle à 18h. Je serai en minibus blanc.")
        );
        await db.SaveChangesAsync(ct);

        // ── 8. Panier ────────────────────────────────────────────────────────────
        var panier = new Panier(alice.Id);
        panier.AjouterOffre(offre1);
        db.Paniers.Add(panier);
        await db.SaveChangesAsync(ct);

        // ── 9. Notifications ─────────────────────────────────────────────────────
        var notif1 = new Notification(alice.Id, TypeNotification.TransactionInitiee,
            "Une transaction est en cours pour l'offre « Transport de personnes sinistrées ».",
            tx1.Id.ToString());

        var notif2 = new Notification(bob.Id, TypeNotification.RelancePropositionAvantArchivage,
            "Votre offre « Kit médicaments d'urgence » arrive bientôt à expiration. Pensez à la reconfirmer.",
            offre4.Id.ToString());

        var notif3 = new Notification(carol.Id, TypeNotification.SuggestionAppariementDisponible,
            "Nouvelle suggestion d'appariement : « Eau potable » ↔ « Besoin urgent de nourriture ».");
        notif3.MarquerCommeLue();
        notif3.MarquerEnvoyee();

        db.Notifications.AddRange(notif1, notif2, notif3);
        await db.SaveChangesAsync(ct);

        // ── 10. Suggestion d'appariement ─────────────────────────────────────────
        var suggestion = new SuggestionAppariement(
            offre1.Id, demande1.Id,
            0.82,
            "Offre avec livraison incluse (eau potable) ↔ demande alimentaire critique en proximité géographique estimée. Score Jaccard élevé sur les mots-clés.");
        db.SuggestionsAppariement.Add(suggestion);
        await db.SaveChangesAsync(ct);

        logger.LogInformation(
            "DataSeeder: seed terminé — 4 acteurs, 1 config, 10 catégories, 2 rôles, " +
            "6 offres, 5 demandes, 3 transactions, 1 panier, 3 notifications, 1 suggestion.");
    }
}
