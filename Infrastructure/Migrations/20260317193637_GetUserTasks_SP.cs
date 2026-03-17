using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GetUserTasks_SP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
             CREATE PROCEDURE [dbo].[GetUserTasks]
                @UserId NVARCHAR(450)=NULL,
                @FromDate DATETIME =NULL,
                @ToDate DATETIME =NULL
            AS
            BEGIN
                SET NOCOUNT ON;
            
                IF @UserId IS NOT NULL
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
                        MAX(A.TaskAttachmentId) AS TaskAttachmentId,
                        MAX(A.FileName) AS FileName,
                        STRING_AGG(ISNULL(NULLIF(U.FullName, ''), U.UserName), ', ') AS UserName,
                        STRING_AGG(CAST(TA.TaskAssigneeId AS NVARCHAR(100)), ', ') AS TaskAssigneeIds,
                        STRING_AGG(CAST(TA.AssignedBy AS NVARCHAR(100)), ', ') AS AssignedBy,
                        MAX(TA.AssignedAt) AS AssignedAt,
                        T.Created
                    FROM 
                        Tasks T
                        INNER JOIN Modules M ON T.ModuleId = M.ModuleId
                        LEFT JOIN TaskAssignees TA ON T.TaskId = TA.TaskId AND TA.IsDeleted = 0
                        LEFT JOIN AspNetUsers U ON TA.AssignedBy = U.Id
                        LEFT JOIN TaskAttachments A ON T.TaskId = A.TaskId
                    WHERE 
                        T.IsActive = 1
                        AND (
                            @UserId IS NULL 
                            OR ',' + TA.AssignedBy + ',' LIKE '%,' + @UserId + ',%'
                        )
                        AND (
                            (@FromDate IS NULL OR CAST(T.Created AS DATE) >= CAST(@FromDate AS DATE))
                            AND (@ToDate IS NULL OR CAST(T.Created AS DATE) <= CAST(@ToDate AS DATE))
                        )
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
                END
                ELSE
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
                        A.TaskAttachmentId,
                        A.FileName,
                        CASE 
                            WHEN LTRIM(RTRIM(U.FullName)) IS NOT NULL AND LTRIM(RTRIM(U.FullName)) <> '' 
                            THEN U.FullName 
                            ELSE U.UserName 
                        END AS UserName,
                        TA.TaskAssigneeId AS TaskAssigneeIds,
                        TA.AssignedBy,
                        TA.AssignedAt,
                        T.Created
                    FROM 
                        Tasks T
                        INNER JOIN Modules M ON T.ModuleId = M.ModuleId
                        LEFT JOIN TaskAssignees TA ON T.TaskId = TA.TaskId AND TA.IsDeleted = 0
                        LEFT JOIN AspNetUsers U ON TA.AssignedBy = U.Id
                        LEFT JOIN TaskAttachments A ON T.TaskId = A.TaskId
                    WHERE 
                        T.IsActive = 1
                        AND (
                            (@FromDate IS NULL OR CAST(T.Created AS DATE) >= CAST(@FromDate AS DATE))
                            AND (@ToDate IS NULL OR CAST(T.Created AS DATE) <= CAST(@ToDate AS DATE))
                        );
                END
            END

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetUserTasks");
        }
    }
}
