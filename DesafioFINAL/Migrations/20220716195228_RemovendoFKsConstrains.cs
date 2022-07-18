using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioFINAL.Migrations
{
    public partial class RemovendoFKsConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos");

            migrationBuilder.AlterColumn<int>(
                name: "IdProduto",
                table: "LogProdutos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos",
                column: "IdProduto",
                principalTable: "Produtos",
                principalColumn: "IdProduto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos");

            migrationBuilder.AlterColumn<int>(
                name: "IdProduto",
                table: "LogProdutos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos",
                column: "IdProduto",
                principalTable: "Produtos",
                principalColumn: "IdProduto",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
