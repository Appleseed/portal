-------------------
--1.4.0.1780.sql
-- Updates rb_users table to include the user_last_visit and user_current_visit fields for determining the user's last visit date
-- Recreates the rb_GetSingleUser procedure to return all fields from the rb_users table, so any additional fields can be returned when this procedure is called
-- Creates the rb_UpdateLastVisit to update the specified user's last visit date, only updates once during a day
-------------------
if exists (select * from sysobjects where id = object_id(N'[rb_GetSingleUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetSingleUser]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_UpdateLastVisit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_UpdateLastVisit]
GO

-- Add these columns to the users table, if they do not exist 
if exists (select * from sysobjects where id = object_id(N'[rb_Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) begin
	if not exists (select * from syscolumns C inner join sysobjects O on O.id = C.id where O.id = object_id(N'[rb_Users]') and OBJECTPROPERTY(O.id, N'IsUserTable') = 1 and C.name = 'user_last_visit') 
		ALTER TABLE rb_Users ADD user_last_visit datetime NOT NULL DEFAULT(getdate())
	if not exists (select * from syscolumns C inner join sysobjects O on O.id = C.id where O.id = object_id(N'[rb_Users]') and OBJECTPROPERTY(O.id, N'IsUserTable') = 1 and C.name = 'user_current_visit') 
		ALTER TABLE rb_Users ADD user_current_visit datetime NOT NULL DEFAULT(getdate())
end
GO

CREATE   PROCEDURE rb_GetSingleUser
(
    @Email nvarchar(100),
    @PortalID int,
	@IDLang	nchar(2) = 'IT'
)
AS
SELECT
	rb_Users.*,
	
	(SELECT TOP 1 rb_Localize.Description
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     ((rb_Localize.CultureCode = @IDLang) OR
                      (rb_Cultures.NeutralCode = @IDLang)) AND (rb_Countries.CountryID = rb_Users.CountryID))
	
	
	 AS Country
					  
FROM 
	rb_Users LEFT OUTER JOIN
	rb_States ON rb_Users.StateID = rb_States.StateID
	
WHERE
(rb_Users.Email = @Email) AND (rb_Users.PortalID = @PortalID)
GO

CREATE       procedure rb_UpdateLastVisit (@Email varchar(100), @PortalID int)


AS
BEGIN
   DECLARE
	@CurDate as datetime,
	@LastCurDate as datetime,
	@LastVisitDate as datetime

   SET NOCOUNT ON
   --Call this procedure BEFORE you call procedures to obtain data based on user_last_visit date field 
   --Get current dates stored in DB	
   SELECT @LastVisitDate = user_last_visit, @LastCurDate = user_current_visit
   FROM rb_users
   WHERE Email = @Email AND PortalID = @PortalID
   
   IF @@ROWCOUNT = 1
   BEGIN
     SET @CurDate = getdate()
     --Check to see if current visit is at least a day old
     IF DATEADD(hour, 12, @LastCurDate) < (@CurDate)
     BEGIN
       IF @LastCurDate <> @LastVisitDate
       BEGIN
         --if current visit date and last visit date are different 
         --then set current visit date and Last Visit date to current date
         --This allows for the user to view the pages based on last visit date,
         --for 1 day after they first visit the site before it sets a new last
         --visit date
			UPDATE rb_users 
			SET 
	 			user_current_visit = @CurDate,
            	user_last_visit = @LastCurDate
			WHERE Email = @Email AND PortalID = @PortalID
       END
       ELSE
       BEGIN
         --else only set current visit date to current date
         UPDATE rb_users
         SET 
			user_current_visit = @CurDate    
         WHERE Email = @Email AND PortalID = @PortalID
       END
     END
   END
END

GO