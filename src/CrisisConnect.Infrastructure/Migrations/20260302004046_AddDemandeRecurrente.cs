using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrisisConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDemandeRecurrente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "est_recurrente",
                table: "propositions",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "frequence_recurrence",
                table: "propositions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "est_recurrente",
                table: "propositions");

            migrationBuilder.DropColumn(
                name: "frequence_recurrence",
                table: "propositions");
        }
    }
}
