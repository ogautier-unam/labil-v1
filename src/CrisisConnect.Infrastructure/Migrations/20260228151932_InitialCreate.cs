using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrisisConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "acteurs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    mot_de_passe_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    est_actif = table.Column<bool>(type: "boolean", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifie_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    type_acteur = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    moyens_contact = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    url_page_presentation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    comment_faire_don = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    types_contributions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    est_active = table.Column<bool>(type: "boolean", nullable: true),
                    responsable_id = table.Column<Guid>(type: "uuid", nullable: true),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    adresse_rue = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    adresse_ville = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    adresse_code_postal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    adresse_pays = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    url_photo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    langue_preferee = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_acteurs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attributions_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    acteur_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_role = table.Column<string>(type: "text", nullable: false),
                    accreditee_par_id = table.Column<Guid>(type: "uuid", nullable: true),
                    date_debut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reconductible = table.Column<bool>(type: "boolean", nullable: false),
                    date_rappel = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rappel_envoye = table.Column<bool>(type: "boolean", nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attributions_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entrees_journal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_heure = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type_operation = table.Column<string>(type: "text", nullable: false),
                    acteur_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entite_cible_id = table.Column<Guid>(type: "uuid", nullable: true),
                    entite_cible_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    details = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    adresse_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entrees_journal", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mandats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mandant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mandataire_id = table.Column<Guid>(type: "uuid", nullable: false),
                    portee = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    est_public = table.Column<bool>(type: "boolean", nullable: false),
                    date_debut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mandats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    destinataire_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    contenu = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ref_entite_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    est_lue = table.Column<bool>(type: "boolean", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_envoi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paniers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    proprietaire_id = table.Column<Guid>(type: "uuid", nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_confirmation = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paniers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "propositions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    cree_par = table.Column<Guid>(type: "uuid", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifie_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_relance = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_archivage = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_cloture = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    type_proposition = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    operateur_logique = table.Column<string>(type: "text", nullable: true),
                    urgence = table.Column<string>(type: "text", nullable: true),
                    region_severite = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    livraison_incluse = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propositions", x => x.id);
                    table.ForeignKey(
                        name: "fk_propositions_propositions_parent_id",
                        column: x => x.parent_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    personne_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    est_revoque = table.Column<bool>(type: "boolean", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    initiateur_id = table.Column<Guid>(type: "uuid", nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_maj = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paniers_offres",
                columns: table => new
                {
                    offres_id = table.Column<Guid>(type: "uuid", nullable: false),
                    panier_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paniers_offres", x => new { x.offres_id, x.panier_id });
                    table.ForeignKey(
                        name: "fk_paniers_offres_paniers_panier_id",
                        column: x => x.panier_id,
                        principalTable: "paniers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_paniers_offres_propositions_offres_id",
                        column: x => x.offres_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "discussions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    visibilite = table.Column<string>(type: "text", nullable: false),
                    date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discussions", x => x.id);
                    table.ForeignKey(
                        name: "fk_discussions_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expediteur_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contenu = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    langue = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    issue_traduction_auto = table.Column<bool>(type: "boolean", nullable: false),
                    texte_original = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    date_envoi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_discussions_discussion_id",
                        column: x => x.discussion_id,
                        principalTable: "discussions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_acteurs_email",
                table: "acteurs",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_discussions_transaction_id",
                table: "discussions",
                column: "transaction_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_messages_discussion_id",
                table: "messages",
                column: "discussion_id");

            migrationBuilder.CreateIndex(
                name: "ix_paniers_offres_panier_id",
                table: "paniers_offres",
                column: "panier_id");

            migrationBuilder.CreateIndex(
                name: "ix_propositions_parent_id",
                table: "propositions",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "acteurs");

            migrationBuilder.DropTable(
                name: "attributions_roles");

            migrationBuilder.DropTable(
                name: "entrees_journal");

            migrationBuilder.DropTable(
                name: "mandats");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "paniers_offres");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "discussions");

            migrationBuilder.DropTable(
                name: "paniers");

            migrationBuilder.DropTable(
                name: "propositions");

            migrationBuilder.DropTable(
                name: "transactions");
        }
    }
}
