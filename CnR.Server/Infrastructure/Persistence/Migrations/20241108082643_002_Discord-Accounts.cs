using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnR.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class _002_DiscordAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordAccount_Accounts_AccountId",
                table: "DiscordAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordAccount",
                table: "DiscordAccount");

            migrationBuilder.RenameTable(
                name: "DiscordAccount",
                newName: "DiscordAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_DiscordAccount_DiscordId",
                table: "DiscordAccounts",
                newName: "IX_DiscordAccounts_DiscordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordAccounts",
                table: "DiscordAccounts",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordAccounts_Accounts_AccountId",
                table: "DiscordAccounts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordAccounts_Accounts_AccountId",
                table: "DiscordAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordAccounts",
                table: "DiscordAccounts");

            migrationBuilder.RenameTable(
                name: "DiscordAccounts",
                newName: "DiscordAccount");

            migrationBuilder.RenameIndex(
                name: "IX_DiscordAccounts_DiscordId",
                table: "DiscordAccount",
                newName: "IX_DiscordAccount_DiscordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordAccount",
                table: "DiscordAccount",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordAccount_Accounts_AccountId",
                table: "DiscordAccount",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
