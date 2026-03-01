using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrisisConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOffreDemandesCouplees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offre_demandes_couplees",
                columns: table => new
                {
                    demandes_couplees_id = table.Column<Guid>(type: "uuid", nullable: false),
                    offre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offre_demandes_couplees", x => new { x.demandes_couplees_id, x.offre_id });
                    table.ForeignKey(
                        name: "fk_offre_demandes_couplees_propositions_demandes_couplees_id",
                        column: x => x.demandes_couplees_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_offre_demandes_couplees_propositions_offre_id",
                        column: x => x.offre_id,
                        principalTable: "propositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_offre_demandes_couplees_offre_id",
                table: "offre_demandes_couplees",
                column: "offre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offre_demandes_couplees");
        }
    }
}
