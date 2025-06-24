using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class EnerjiRequest_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnerjiRequestId",
                table: "EnerjiRequestAdvance",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnerjiRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnerjiRequest", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnerjiRequestAdvance_EnerjiRequestId",
                table: "EnerjiRequestAdvance",
                column: "EnerjiRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnerjiRequestAdvance_EnerjiRequest_EnerjiRequestId",
                table: "EnerjiRequestAdvance",
                column: "EnerjiRequestId",
                principalTable: "EnerjiRequest",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnerjiRequestAdvance_EnerjiRequest_EnerjiRequestId",
                table: "EnerjiRequestAdvance");

            migrationBuilder.DropTable(
                name: "EnerjiRequest");

            migrationBuilder.DropIndex(
                name: "IX_EnerjiRequestAdvance_EnerjiRequestId",
                table: "EnerjiRequestAdvance");

            migrationBuilder.DropColumn(
                name: "EnerjiRequestId",
                table: "EnerjiRequestAdvance");
        }
    }
}
