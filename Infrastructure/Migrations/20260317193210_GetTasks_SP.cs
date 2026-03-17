using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GetTasks_SP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[GetTasks]
            AS
            BEGIN
                SELECT 
                    T.TaskId,
                    T.Title,
                    T.TaskNo,
                    T.Description,
                    T.Status,
                    T.Priority,
                    T.EstimatedHours,
                    T.IsActive,
                    T.IsCompletedRequest,
                    T.DueDate,
                    T.ModuleId,
                    M.ModuleName,
            		T.Created,
                    MAX(A.TaskAttachmentId) AS TaskAttachmentId,
                    MAX(A.FileName) AS FileName,
                    STRING_AGG(ISNULL(NULLIF(U.FullName, ''), U.UserName), ', ') AS UserName, -- 👈 Combine usernames
                    STRING_AGG(CAST(TA.TaskAssigneeId AS NVARCHAR(100)), ', ') AS TaskAssigneeIds,
                    MAX(TA.AssignedBy) AS AssignedBy,
                    MAX(TA.AssignedAt) AS AssignedAt
                FROM 
                    Tasks T
                    INNER JOIN Modules M ON T.ModuleId = M.ModuleId
                    INNER JOIN TaskAssignees TA ON T.TaskId = TA.TaskId
                    INNER JOIN AspNetUsers U ON TA.AssignedBy = U.Id
                    LEFT JOIN TaskAttachments A ON T.TaskId = A.TaskId
            		WHERE
                    TA.IsDeleted=0
                GROUP BY 
                    T.TaskId,
                    T.Title,
                    T.TaskNo,
                    T.Description,
                    T.Status,
                    T.Priority,
                    T.EstimatedHours,
                    T.IsActive,
                    T.IsCompletedRequest,
                    T.DueDate,
                    T.ModuleId,
                    M.ModuleName,
            		T.Created
                ORDER BY 
                    T.Created DESC
            END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetTasks");
        }
    }
}
