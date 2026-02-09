using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectAssigneeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignees_AspNetUsers_AssignedByUserId",
                table: "TaskAssignees");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignees_AssignedByUserId",
                table: "TaskAssignees");

            migrationBuilder.DropColumn(
                name: "AssignedByUserId",
                table: "TaskAssignees");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedBy",
                table: "TaskAssignees",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectAssignees",
                columns: table => new
                {
                    ProjectAssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnterById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnterBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAssignees", x => x.ProjectAssigneeId);
                    table.ForeignKey(
                        name: "FK_ProjectAssignees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAssignees_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignees_AssignedBy",
                table: "TaskAssignees",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssignees_ProjectId",
                table: "ProjectAssignees",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssignees_UserId",
                table: "ProjectAssignees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignees_AspNetUsers_AssignedBy",
                table: "TaskAssignees",
                column: "AssignedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignees_AspNetUsers_AssignedBy",
                table: "TaskAssignees");

            migrationBuilder.DropTable(
                name: "ProjectAssignees");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignees_AssignedBy",
                table: "TaskAssignees");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedBy",
                table: "TaskAssignees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedByUserId",
                table: "TaskAssignees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignees_AssignedByUserId",
                table: "TaskAssignees",
                column: "AssignedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignees_AspNetUsers_AssignedByUserId",
                table: "TaskAssignees",
                column: "AssignedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
