CREATE PROCEDURE getAssetReportByCategoryAndState
    @SortColumn NVARCHAR(50) = 'Category', 
    @SortDirection NVARCHAR(4) = 'ASC'    
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = '
        SELECT 
            c.Name AS Category,
            COUNT(a.Id) AS Total,
            SUM(CASE WHEN a.State = 0 THEN 1 ELSE 0 END) AS Assigned,
            SUM(CASE WHEN a.State = 1 THEN 1 ELSE 0 END) AS Available,
            SUM(CASE WHEN a.State = 2 THEN 1 ELSE 0 END) AS NotAvailable,
            SUM(CASE WHEN a.State = 3 THEN 1 ELSE 0 END) AS WaitingForRecycling,
            SUM(CASE WHEN a.State = 4 THEN 1 ELSE 0 END) AS Recycled
        FROM 
            Assets a
        INNER JOIN 
            Categories c ON a.CategoryId = c.Id
        GROUP BY 
            c.Name
        ORDER BY ' + @SortColumn + ' ' + @SortDirection;

    EXEC sp_executesql @SQL;
END
