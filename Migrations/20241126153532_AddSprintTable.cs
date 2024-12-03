using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigaJira.Migrations
{
    /// <inheritdoc />
    public partial class AddSprintTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_approverId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_assigneId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "ticketName",
                table: "Tickets",
                newName: "TicketName");

            migrationBuilder.RenameColumn(
                name: "ticketDescription",
                table: "Tickets",
                newName: "TicketDescription");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Tickets",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "assigneId",
                table: "Tickets",
                newName: "AssigneId");

            migrationBuilder.RenameColumn(
                name: "approverId",
                table: "Tickets",
                newName: "ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_assigneId",
                table: "Tickets",
                newName: "IX_Tickets_AssigneId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_approverId",
                table: "Tickets",
                newName: "IX_Tickets_ApproverId");

            migrationBuilder.AlterColumn<string>(
                name: "AssigneId",
                table: "Tickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApproverId",
                table: "Tickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SprintId",
                table: "Tickets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sprint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sprint_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SprintId",
                table: "Tickets",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Sprint_ProjectId",
                table: "Sprint",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_ApproverId",
                table: "Tickets",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssigneId",
                table: "Tickets",
                column: "AssigneId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Sprint_SprintId",
                table: "Tickets",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_ApproverId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssigneId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Sprint_SprintId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Sprint");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SprintId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketName",
                table: "Tickets",
                newName: "ticketName");

            migrationBuilder.RenameColumn(
                name: "TicketDescription",
                table: "Tickets",
                newName: "ticketDescription");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tickets",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "AssigneId",
                table: "Tickets",
                newName: "assigneId");

            migrationBuilder.RenameColumn(
                name: "ApproverId",
                table: "Tickets",
                newName: "approverId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_AssigneId",
                table: "Tickets",
                newName: "IX_Tickets_assigneId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_ApproverId",
                table: "Tickets",
                newName: "IX_Tickets_approverId");

            migrationBuilder.AlterColumn<string>(
                name: "assigneId",
                table: "Tickets",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "approverId",
                table: "Tickets",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_approverId",
                table: "Tickets",
                column: "approverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_assigneId",
                table: "Tickets",
                column: "assigneId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
