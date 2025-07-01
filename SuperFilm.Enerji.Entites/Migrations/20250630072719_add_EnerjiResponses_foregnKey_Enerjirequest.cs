using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class add_EnerjiResponses_foregnKey_Enerjirequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnerjiRequestId",
                table: "EnerjiResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EnerjiResponses_EnerjiRequestId",
                table: "EnerjiResponses",
                column: "EnerjiRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnerjiResponses_EnerjiRequest_EnerjiRequestId",
                table: "EnerjiResponses",
                column: "EnerjiRequestId",
                principalTable: "EnerjiRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnerjiResponses_EnerjiRequest_EnerjiRequestId",
                table: "EnerjiResponses");

            migrationBuilder.DropIndex(
                name: "IX_EnerjiResponses_EnerjiRequestId",
                table: "EnerjiResponses");

            migrationBuilder.DropColumn(
                name: "EnerjiRequestId",
                table: "EnerjiResponses");
        }
    }
}
