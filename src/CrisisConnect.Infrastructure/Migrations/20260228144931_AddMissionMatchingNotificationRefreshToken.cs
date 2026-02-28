using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrisisConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionMatchingNotificationRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "missions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false),
                    proposition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cree_par = table.Column<Guid>(type: "uuid", nullable: false),
                    plage_debut = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    plage_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    nombre_benevoles = table.Column<int>(type: "integer", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifie_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_missions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    destinataire_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sujet = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    contenu = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    est_lue = table.Column<bool>(type: "boolean", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
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
                name: "matchings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    benevole_id = table.Column<Guid>(type: "uuid", nullable: false),
                    statut = table.Column<string>(type: "text", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifie_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_matchings", x => x.id);
                    table.ForeignKey(
                        name: "fk_matchings_missions_mission_id",
                        column: x => x.mission_id,
                        principalTable: "missions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_matchings_mission_id",
                table: "matchings",
                column: "mission_id");

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
                name: "matchings");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "missions");
        }
    }
}
