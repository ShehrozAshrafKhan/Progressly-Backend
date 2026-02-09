using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryByIdFieldInBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "TaskUpdates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "TaskUpdates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "TaskTags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "TaskTags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "TaskAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "TaskAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "TaskAssignees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "TaskAssignees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnterById",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "TaskUpdates");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskUpdates");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "TaskTags");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskTags");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "TaskAssignees");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TaskAssignees");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "EnterById",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ActivityLogs");
        }
    }
}
