using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigaJira.Migrations
{
    /// <inheritdoc />
    public partial class AddSprintToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Sprints_SprintId",
                table: "Tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "SprintId1",
                table: "Tickets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SprintId1",
                table: "Tickets",
                column: "SprintId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Sprints_SprintId",
                table: "Tickets",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Sprints_SprintId1",
                table: "Tickets",
                column: "SprintId1",
                principalTable: "Sprints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Sprints_SprintId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Sprints_SprintId1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SprintId1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SprintId1",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Sprints_SprintId",
                table: "Tickets",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id");
        }
    }
}
