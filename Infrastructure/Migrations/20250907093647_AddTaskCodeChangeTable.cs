using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskCodeChangeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskCodeChanges",
                columns: table => new
                {
                    TaskCodeChangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_TaskCodeChanges", x => x.TaskCodeChangeId);
                    table.ForeignKey(
                        name: "FK_TaskCodeChanges_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCodeChanges_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskCodeChanges_TagId",
                table: "TaskCodeChanges",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCodeChanges_TaskId",
                table: "TaskCodeChanges",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskCodeChanges");
        }
    }
}
