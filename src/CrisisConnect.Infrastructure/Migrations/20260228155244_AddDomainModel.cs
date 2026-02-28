using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrisisConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type_proposition",
                table: "propositions",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "adresse_depot",
                table: "propositions",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "capacite_max",
                table: "propositions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "capacite_utilisee",
                table: "propositions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "categorie_id",
                table: "propositions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_limit",
                table: "propositions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description_mission",
                table: "propositions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description_validation",
                table: "propositions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "nombre_ressources_requises",
                table: "propositions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "statut_validation",
                table: "propositions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unite_capacite",
                table: "propositions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "url_catalogue",
                table: "propositions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "valideur_entite_id",
                table: "propositions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "config_catastrophes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    zone_geographique = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    etat_referent = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    delai_archivage_jours = table.Column<int>(type: "integer", nullable: false),
                    delai_rappel_avant_archivage = table.Column<int>(type: "integer", nullable: false),
                    est_active = table.Column<bool>(type: "boolean", nullable: false),
                    date_debut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    langues_actives = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    modes_identification_actifs = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_config_catastrophes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intentions_don",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    demande_quota_id = table.Column<Guid>(type: "uuid", nullable: false),
                    acteur_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantite = table.Column<int>(type: "integer", nullable: false),
                    unite = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    date_intention = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_intentions_don", x => x.id);
                    table.ForeignKey(
                        name: "fk_intentions_don_propositions_demande_quota_id",
                        column: x => x.demande_quota_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lignes_catalogue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    demande_sur_catalogue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    designation = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    quantite = table.Column<int>(type: "integer", nullable: false),
                    prix_unitaire = table.Column<double>(type: "double precision", nullable: false),
                    url_produit = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    statut = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lignes_catalogue", x => x.id);
                    table.ForeignKey(
                        name: "fk_lignes_catalogue_propositions_demande_sur_catalogue_id",
                        column: x => x.demande_sur_catalogue_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    date_ajout = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_medias", x => x.id);
                    table.ForeignKey(
                        name: "fk_medias_propositions_proposition_id",
                        column: x => x.proposition_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "methodes_identification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    personne_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_verification = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    est_verifiee = table.Column<bool>(type: "boolean", nullable: false),
                    niveau_fiabilite = table.Column<string>(type: "text", nullable: false),
                    type_methode = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    numero_carte = table.Column<string>(type: "text", nullable: true),
                    pays_emetteur = table.Column<string>(type: "text", nullable: true),
                    garant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    login = table.Column<string>(type: "text", nullable: true),
                    mot_de_passe_hash = table.Column<string>(type: "text", nullable: true),
                    derniere_connexion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    nombre_parrains_requis = table.Column<int>(type: "integer", nullable: true),
                    parrains_ids = table.Column<string>(type: "text", nullable: true),
                    reference_virement = table.Column<string>(type: "text", nullable: true),
                    montant_vire_centimes = table.Column<int>(type: "integer", nullable: true),
                    type_facture = table.Column<int>(type: "integer", nullable: true),
                    date_facture = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    url_photo = table.Column<string>(type: "text", nullable: true),
                    inclu_personne = table.Column<bool>(type: "boolean", nullable: true),
                    mode_verification = table.Column<int>(type: "integer", nullable: true),
                    numero_telephone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_methodes_identification", x => x.id);
                    table.ForeignKey(
                        name: "fk_methodes_identification_personnes_personne_id",
                        column: x => x.personne_id,
                        principalTable: "acteurs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "suggestions_appariement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    offre_id = table.Column<Guid>(type: "uuid", nullable: false),
                    demande_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_generation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score_correspondance = table.Column<double>(type: "double precision", nullable: false),
                    raisonnement = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    est_acknowledged = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suggestions_appariement", x => x.id);
                    table.ForeignKey(
                        name: "fk_suggestions_appariement_propositions_demande_id",
                        column: x => x.demande_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_suggestions_appariement_propositions_offre_id",
                        column: x => x.offre_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories_taxonomie",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nom_json = table.Column<string>(type: "jsonb", nullable: false),
                    description_json = table.Column<string>(type: "jsonb", nullable: false),
                    est_active = table.Column<bool>(type: "boolean", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    config_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories_taxonomie", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_taxonomie_categories_taxonomie_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories_taxonomie",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_categories_taxonomie_configs_catastrophe_config_id",
                        column: x => x.config_id,
                        principalTable: "config_catastrophes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_taxonomie_code_config_id",
                table: "categories_taxonomie",
                columns: new[] { "code", "config_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_taxonomie_config_id",
                table: "categories_taxonomie",
                column: "config_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_taxonomie_parent_id",
                table: "categories_taxonomie",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_intentions_don_demande_quota_id",
                table: "intentions_don",
                column: "demande_quota_id");

            migrationBuilder.CreateIndex(
                name: "ix_lignes_catalogue_demande_sur_catalogue_id",
                table: "lignes_catalogue",
                column: "demande_sur_catalogue_id");

            migrationBuilder.CreateIndex(
                name: "ix_medias_proposition_id",
                table: "medias",
                column: "proposition_id");

            migrationBuilder.CreateIndex(
                name: "ix_methodes_identification_personne_id",
                table: "methodes_identification",
                column: "personne_id");

            migrationBuilder.CreateIndex(
                name: "ix_suggestions_appariement_demande_id",
                table: "suggestions_appariement",
                column: "demande_id");

            migrationBuilder.CreateIndex(
                name: "ix_suggestions_appariement_offre_id",
                table: "suggestions_appariement",
                column: "offre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories_taxonomie");

            migrationBuilder.DropTable(
                name: "intentions_don");

            migrationBuilder.DropTable(
                name: "lignes_catalogue");

            migrationBuilder.DropTable(
                name: "medias");

            migrationBuilder.DropTable(
                name: "methodes_identification");

            migrationBuilder.DropTable(
                name: "suggestions_appariement");

            migrationBuilder.DropTable(
                name: "config_catastrophes");

            migrationBuilder.DropColumn(
                name: "adresse_depot",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "capacite_max",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "capacite_utilisee",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "categorie_id",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "date_limit",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "description_mission",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "description_validation",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "nombre_ressources_requises",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "statut_validation",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "unite_capacite",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "url_catalogue",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "valideur_entite_id",
                table: "propositions");

            migrationBuilder.AlterColumn<string>(
                name: "type_proposition",
                table: "propositions",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(34)",
                oldMaxLength: 34);
        }
    }
}
