/* Install script, WhosLoggedOn module, [paul@paulyarrow.com], 16/07/2003 */


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetLoggedOnUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetLoggedOnUsers]
GO

CREATE PROCEDURE rb_GetLoggedOnUsers
(
    @PortalID    int,
    @MinutesToCheck int
)
AS
		SELECT DISTINCT rbm.UserHostAddress, rbu.Name,
                          (
                          SELECT TOP 1 ActivityType
                          FROM rb_Monitoring
                          WHERE ActivityTime >= DATEADD(n, - @MinutesToCheck, GETDATE()) 
                          AND UserHostAddress = rbm.UserHostAddress 
                          AND UserID = rbm.UserID 
                          AND rbm.PortalID = @PortalID
                          ORDER BY ActivityTime DESC) 
                          AS LastAction
                          FROM  rb_Monitoring rbm INNER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID
                          WHERE (rbm.ActivityTime >= DATEADD(n, - @MinutesToCheck, GETDATE())) 
                          AND (rbm.PortalID = @PortalID)

GO



IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetNumberOfActiveUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetNumberOfActiveUsers]
GO

CREATE PROCEDURE rb_GetNumberOfActiveUsers
(
    @PortalID    int,
    @MinutesToCheck int,
    @NoOfUsers int OUTPUT
)
AS

	SELECT @NoOfUsers =  COUNT(DISTINCT UserHostAddress)
	FROM rb_Monitoring
	WHERE ActivityTime >= DATEADD(n, -@MinutesToCheck, GETDATE())
	AND PortalID = @PortalID
GO
