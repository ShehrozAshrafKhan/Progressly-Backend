using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklySummaryReport_SP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
             CREATE PROCEDURE [dbo].[WeeklySummaryReport]
                @UserId NVARCHAR(450) = NULL,
                @FromDate DATETIME = NULL,
                @ToDate DATETIME = NULL
            AS
            BEGIN
                SET NOCOUNT ON;
            
                SELECT 
                    DATEPART(YEAR, T.Created) AS [Year],
                    DATEPART(WEEK, T.Created) AS [WeekNumber],
                    MIN(CAST(T.Created AS DATE)) AS WeekStartDate,
                    MAX(CAST(T.Created AS DATE)) AS WeekEndDate,
                    COUNT(*) AS TotalTasks,
                    SUM(CASE WHEN T.Status = 'COMPLETED' THEN 1 ELSE 0 END) AS CompletedTasks,
                    SUM(CASE WHEN T.Status != 'COMPLETED' THEN 1 ELSE 0 END) AS PendingTasks,
                    SUM(CASE WHEN T.DueDate < GETDATE() AND T.Status != 'COMPLETED' THEN 1 ELSE 0 END) AS OverdueTasks
                FROM 
                    Tasks T
                    INNER JOIN TaskAssignees TA ON T.TaskId = TA.TaskId AND TA.IsDeleted = 0
                WHERE 
                    T.IsActive = 1
                    AND (
                        @UserId IS NULL 
                        OR ',' + TA.AssignedBy + ',' LIKE '%,' + @UserId + ',%' -- handles comma-separated IDs
                    )
                    AND (
                        @FromDate IS NULL OR CAST(T.Created AS DATE) >= CAST(@FromDate AS DATE)
                    )
                    AND (
                        @ToDate IS NULL OR CAST(T.Created AS DATE) <= CAST(@ToDate AS DATE)
                    )
                GROUP BY 
                    DATEPART(YEAR, T.Created),
                    DATEPART(WEEK, T.Created)
                ORDER BY 
                    [Year], [WeekNumber]
            END

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS WeeklySummaryReport");
        }
    }
}
