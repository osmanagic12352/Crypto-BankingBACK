using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto_BankingREG.Migrations
{
    public partial class Migracija2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Transakcija",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_UserId",
                table: "Transakcija",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_AspNetUsers_UserId",
                table: "Transakcija",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transakcija_AspNetUsers_UserId",
                table: "Transakcija");

            migrationBuilder.DropIndex(
                name: "IX_Transakcija_UserId",
                table: "Transakcija");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transakcija");
        }
    }
}
