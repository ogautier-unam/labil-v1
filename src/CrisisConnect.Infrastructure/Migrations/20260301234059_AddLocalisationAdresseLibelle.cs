using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrisisConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalisationAdresseLibelle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "adresse_libelle",
                table: "propositions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adresse_libelle",
                table: "propositions");
        }
    }
}
