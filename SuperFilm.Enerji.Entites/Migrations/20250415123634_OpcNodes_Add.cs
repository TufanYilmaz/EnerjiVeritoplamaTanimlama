using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class OpcNodes_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "IsletmeId",
            //    table: "OPC_NODES",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_OPC_NODES_IsletmeId",
            //    table: "OPC_NODES",
            //    column: "IsletmeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_IsletmeId",
            //    table: "OPC_NODES",
            //    column: "IsletmeId",
            //    principalTable: "ISLETME",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_IsletmeId",
            //    table: "OPC_NODES");

            //migrationBuilder.DropIndex(
            //    name: "IX_OPC_NODES_IsletmeId",
            //    table: "OPC_NODES");

            //migrationBuilder.DropColumn(
            //    name: "IsletmeId",
            //    table: "OPC_NODES");
        }
    }
}
