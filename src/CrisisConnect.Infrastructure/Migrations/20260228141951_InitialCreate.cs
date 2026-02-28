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
                name: "personnes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    adresse_rue = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    adresse_ville = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    adresse_code_postal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    adresse_pays = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    mot_de_passe_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    cree_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifie_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personnes", x => x.id);
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
                    modifie_le = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_propositions", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "personnes");

            migrationBuilder.DropTable(
                name: "propositions");
        }
    }
}
