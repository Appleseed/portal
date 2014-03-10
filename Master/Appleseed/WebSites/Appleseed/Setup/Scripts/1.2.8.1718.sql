-------------------
--1.2.8.1718.sql
-------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Esperantus_CountryList]') AND OBJECTPROPERTY(id, N'IsView') = 1)
drop view [Esperantus_CountryList]
GO

CREATE VIEW Esperantus_CountryList
AS
SELECT     rb_Countries.CountryID AS Name, NULL AS Parent,
                          (SELECT Description
                            FROM          rb_Localize
                            WHERE      TextKey = 'COUNTRY_' + rb_Countries.CountryID AND CultureCode ='en') AS EnglishName, rb_Countries.CountryID AS twoLetterISOCountryName, 
                      rb_Countries.Code3 AS threeLetterISOCountryName, rb_Countries.Number AS CountryCode, cast(1 as bit)  AS Inhabited, COUNT(rb_States.StateID) 
                      AS ChildCount
FROM         rb_Countries LEFT OUTER JOIN
                      rb_States ON rb_Countries.CountryID = rb_States.CountryID
GROUP BY rb_Countries.CountryID, rb_Countries.Country, rb_Countries.Code3, rb_Countries.Number
UNION
SELECT     rb_Countries.CountryID + '-' + upper(Left(rb_States.Description, 3)) AS Name, rb_Countries.CountryID AS Parent, rb_States.Description AS EnglishName, 
                      rb_Countries.CountryID AS twoLetterISOCountryName, NULL AS threeLetterISOCountryName, rb_States.StateID AS CountryCode, cast(1 as bit) AS Inhabited,
                       0 AS ChildCount
FROM         rb_Countries RIGHT OUTER JOIN
                      rb_States ON rb_Countries.CountryID = rb_States.CountryID
GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1718','1.2.8.1718', CONVERT(datetime, '05/24/2003', 101))
GO
