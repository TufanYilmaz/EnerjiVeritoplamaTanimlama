using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class OpcNodes_add_Isletme_ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_IsletmeId",
            //    table: "OPC_NODES");

            //migrationBuilder.AlterColumn<int>(
            //    name: "IsletmeId",
            //    table: "OPC_NODES",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_IsletmeId",
            //    table: "OPC_NODES",
            //    column: "IsletmeId",
            //    principalTable: "ISLETME",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_IsletmeId",
            //    table: "OPC_NODES");

            //migrationBuilder.AlterColumn<int>(
            //    name: "IsletmeId",
            //    table: "OPC_NODES",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_IsletmeId",
            //    table: "OPC_NODES",
            //    column: "IsletmeId",
            //    principalTable: "ISLETME",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
