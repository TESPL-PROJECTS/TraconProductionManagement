IF EXISTS(select 1 from sys.views where name='VW_DSProductUpdateGrid' and type='v')
DROP VIEW VW_DSProductUpdateGrid
GO



CREATE VIEW [dbo].[VW_DSProductUpdateGrid]
AS
SELECT        P.ProductID, P.BuyerOrderNumberName, P.Processname, P.Productname, P.SPCDate, GETDATE() AS TodayDate, F_2.FinishedDays, P.ProductQty, P.FinishedQty, P.Unit, 
                         CASE WHEN DATEDIFF(day, GETDATE(), SPCDate) > 0 THEN DATEDIFF(day, GETDATE(), SPCDate) ELSE 0 END AS [RemainingDays], 
                         CASE WHEN FinishedQty = 0 THEN 0 ELSE ((CAST(FinishedQty AS DECIMAL(18,2)) / CAST(ProductQty AS DECIMAL(18,2))) * 100.00) END AS Done
FROM            dbo.ProductUpdateGrid AS P LEFT OUTER JOIN
                             (SELECT        COUNT(F_1.ProductID) * 7 AS FinishedDays, P.ProductID
                               FROM            dbo.ProductUpdateGrid AS P LEFT OUTER JOIN
                                                             (SELECT        COUNT(F.PlannerID) AS CountPlannerID, F.PlannerID, MAX(PL.ProductID) AS ProductID
                                                               FROM            dbo.FinishingUpdateGrid AS F LEFT OUTER JOIN
                                                                                         dbo.PlannerUpdateGrid AS PL ON PL.PlannerID = F.PlannerID
                                                               GROUP BY F.PlannerID, PL.ProductID) AS F_1 ON F_1.ProductID = P.ProductID
                               GROUP BY F_1.ProductID, P.ProductID) AS F_2 ON F_2.ProductID = P.ProductID

GO



IF EXISTS(select 1 from sys.views where name='VW_ProdcutGroupGantt' and type='v')
DROP VIEW VW_ProdcutGroupGantt
GO



CREATE VIEW [dbo].[VW_ProdcutGroupGantt]
AS
Select ROW_NUMBER() OVER (ORDER BY P.BuyerOrderNumberName, P.Processname, P.Productname ASC) AS ID,  P.BuyerOrderNumberName,P.Processname,
P.Productname,P.StartDate,P.SPCDate,P.FinishedDays,
CASE WHEN DATEDIFF(d, DATEDIFF(d, P.StartDate, P.SPCDate),P.FinishedDays) > 0 THEN 
	DATEDIFF(d, DATEDIFF(d, P.StartDate, P.SPCDate),P.FinishedDays)
ELSE 0 END AS DelayedDays, 
P.Done,
CASE WHEN DATEDIFF(d, StartDate, SPCDate) > 0 THEN 
DATEDIFF(d, StartDate, SPCDate)
ELSE  0 END AS NoDays 
FROM 
(SELECT P.ProductID, P.BuyerOrderNumberName, P.Processname, P.Productname,P.StartDate, P.SPCDate, GETDATE() AS TodayDate, F_2.FinishedDays, P.ProductQty, P.FinishedQty, P.Unit, 
                         CASE WHEN DATEDIFF(day, SPCDate, GETDATE()) > 0 THEN DATEDIFF(day, SPCDate, GETDATE()) ELSE 0 END AS [RemainingDays], 
                         CASE WHEN FinishedQty = 0 THEN 0 ELSE ((CAST(FinishedQty AS DECIMAL(18,2)) / CAST(ProductQty AS DECIMAL(18,2))) * 100.00) END AS Done						
FROM dbo.ProductUpdateGrid AS P LEFT OUTER JOIN
                             (SELECT        COUNT(F_1.ProductID) * 7 AS FinishedDays, P.ProductID
                               FROM            dbo.ProductUpdateGrid AS P LEFT OUTER JOIN
                                                             (SELECT        COUNT(F.PlannerID) AS CountPlannerID, F.PlannerID, MAX(PL.ProductID) AS ProductID
                                                               FROM            dbo.FinishingUpdateGrid AS F LEFT OUTER JOIN
                                                                                         dbo.PlannerUpdateGrid AS PL ON PL.PlannerID = F.PlannerID
                                                               GROUP BY F.PlannerID, PL.ProductID) AS F_1 ON F_1.ProductID = P.ProductID
                               GROUP BY F_1.ProductID, P.ProductID) AS F_2 ON F_2.ProductID = P.ProductID) as P


GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'ProductUpdateGrid' AND [COLUMN_NAME] = 'SKUStartDate')
BEGIN
  ALTER TABLE [ProductUpdateGrid] ADD [SKUStartDate] DateTime NULL
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'ProductUpdateGrid' AND [COLUMN_NAME] = 'SKUEndDate')
BEGIN
  ALTER TABLE [ProductUpdateGrid] ADD [SKUEndDate] DateTime NULL
END
GO


IF EXISTS(select 1 from sys.views where name='VW_DSProductUpdateGrid' and type='v')
DROP VIEW VW_DSProductUpdateGrid
GO


CREATE VIEW [dbo].[VW_DSProductUpdateGrid]
AS
SELECT        P.ProductID, P.BuyerOrderNumberName, P.Processname, P.Productname, P.SPCDate, GETDATE() AS TodayDate,  CASE WHEN DATEDIFF(day , P.SKUStartDate , P.SKUEndDate) > 0 THEN DATEDIFF(day , P.SKUStartDate , P.SKUEndDate) ELSE 0 END AS FinishedDays, P.ProductQty, 
                         P.FinishedQty, P.Unit, CASE WHEN DATEDIFF(day, SPCDate, GETDATE()) > 0 THEN DATEDIFF(day, SPCDate, GETDATE()) ELSE 0 END AS RemainingDays, 
                         CASE WHEN FinishedQty = 0 THEN 0 ELSE ((CAST(FinishedQty AS DECIMAL(18, 2)) / CAST(ProductQty AS DECIMAL(18, 2))) * 100.00) END AS Done
FROM            dbo.ProductUpdateGrid AS P 

GO

IF EXISTS(select 1 from sys.views where name='VW_ProdcutGroupGantt' and type='v')
DROP VIEW VW_ProdcutGroupGantt
GO

CREATE VIEW [dbo].[VW_ProdcutGroupGantt]
AS
	SELECT        ROW_NUMBER() OVER (ORDER BY P.BuyerOrderNumberName, P.Processname, P.Productname ASC) AS ID, P.BuyerOrderNumberName, P.Processname, 
	P.Productname, P.StartDate, P.SPCDate, 
	CASE WHEN DATEDIFF(d, P.SPCDate, P.SKUEndDate) > 0 
	THEN DATEDIFF(d, P.SPCDate, P.SKUEndDate) ELSE 0 END AS DelayedDays, P.Done, 
	CASE WHEN DATEDIFF(d, StartDate, SPCDate) > 0 
	THEN DATEDIFF(d, StartDate, SPCDate) 
	ELSE 0 END AS NoDays, P.SKUStartDate, P.SKUEndDate
	FROM            (SELECT P.ProductID, P.BuyerOrderNumberName, P.Processname, P.Productname, P.StartDate, P.SPCDate, GETDATE() AS TodayDate, 
							CASE WHEN DATEDIFF(day , P.SKUStartDate , P.SKUEndDate) > 0 THEN DATEDIFF(day , P.SKUStartDate , P.SKUEndDate) ELSE 0 END AS FinishedDays, 
							P.ProductQty, P.FinishedQty, P.Unit, CASE WHEN DATEDIFF(day, SPCDate, GETDATE()) > 0 THEN DATEDIFF(day, SPCDate, GETDATE()) 
							ELSE 0 END AS [RemainingDays], CASE WHEN FinishedQty = 0 THEN 0 ELSE ((CAST(FinishedQty AS DECIMAL(18, 2)) / CAST(ProductQty AS DECIMAL(18, 2))) * 100.00) END AS Done, P.SKUStartDate, P.SKUEndDate
					 FROM dbo.ProductUpdateGrid AS P ) AS P
GO