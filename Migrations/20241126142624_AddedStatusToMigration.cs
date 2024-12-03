using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigaJira.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusToMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Statuses_statusId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_statusId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "statusId",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Tickets",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "statusId",
                table: "Tickets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_statusId",
                table: "Tickets",
                column: "statusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Statuses_statusId",
                table: "Tickets",
                column: "statusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }
    }
}
