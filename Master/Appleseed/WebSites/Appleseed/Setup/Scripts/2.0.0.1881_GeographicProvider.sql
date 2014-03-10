-- GeographicProvider-related database changes

-- RB_COUNTRIES: added AdministrativeDivisionNeutralName, renamed Country to NeutralName and dropped Code3 and Number columns
EXEC sp_rename 
    @objname = 'rb_Countries.Country', 
    @newname = 'NeutralName', 
    @objtype = 'COLUMN'

alter table rb_Countries add
  AdministrativeDivisionNeutralName nvarchar(50) not null constraint DFX_353B5B30_0165 default(N'')
go

alter table rb_Countries drop
   constraint DFX_353B5B30_0165,
  column Code3,
  column Number
go

UPDATE rb_Countries SET AdministrativeDivisionNeutralName='State'
UPDATE rb_Countries SET NeutralName = (SELECT Description FROM rb_Localize WHERE TextKey= 'COUNTRY_' + CountryID AND CultureCode='en')

-- RB_LOCALIZE: added PK(TextKey,CultureCode)

set ANSI_NULLS on
go

update rb_Localize set Description = N'' where Description is null
go

alter table rb_Localize alter column
  Description nvarchar(1500) not null
go

alter table rb_Localize add
  constraint PK_rb_Localize primary key(TextKey,CultureCode)
go


-- RB_STATES: renamed Description to NeutralName
EXEC sp_rename 
    @objname = 'rb_States.Description', 
    @newname = 'NeutralName', 
    @objtype = 'COLUMN'


-- Esperantus_CountryList: dropped

drop view Esperantus_CountryList
go


-- STORED PROCEDURES

EXEC dbo.sp_executesql @statement = N'alter PROCEDURE rb_GetCountries
(
	@SortBy int = 2 -- by default, order by CountryID
)
AS

IF (@SortBy = 0) OR (@SortBy = 3) -- no order
SELECT     rb_Countries.CountryID, rb_Countries.NeutralName, rb_Countries.AdministrativeDivisionNeutralName
FROM       rb_Countries 

ELSE IF @SortBy = 1 -- order by NeutralName
SELECT     rb_Countries.CountryID, rb_Countries.NeutralName, rb_Countries.AdministrativeDivisionNeutralName
FROM       rb_Countries 
ORDER BY   rb_Countries.NeutralName

ELSE IF @SortBy = 2 -- order by CountryID
SELECT     rb_Countries.CountryID, rb_Countries.NeutralName, rb_Countries.AdministrativeDivisionNeutralName
FROM       rb_Countries 
ORDER BY   rb_Countries.CountryID'
go

EXEC dbo.sp_executesql @statement = N'alter PROCEDURE rb_GetCountriesFiltered
(
	@Filter nvarchar(1000) = '''',
	@SortBy int = 2 -- by default, order by CountryID
)
AS

IF (@SortBy = 0) OR (@SortBy = 3) -- no order
SELECT     rb_Countries.CountryID, rb_Countries.NeutralName, rb_Countries.AdministrativeDivisionNeutralName
FROM       rb_Countries 
WHERE      (PATINDEX(''%'' + rb_Countries.CountryID + ''%'', @Filter) > 0)

ELSE IF @SortBy = 1 -- order by NeutralName
SELECT     rb_Countries.CountryID, rb_Countries.NeutralName, rb_Countries.AdministrativeDivisionNeutralName
FROM       rb_Countries 
WHERE      (PATINDEX(''%'' + rb_Countries.CountryID + ''%'', @Filter) > 0)
ORDER BY   rb_Countries.NeutralName

ELSE IF @SortBy = 2 -- order by CountryID
SELECT     rb_Countries.CountryID, rb_Countries.NeutralName, rb_Countries.AdministrativeDivisionNeutralName
FROM       rb_Countries 
WHERE      (PATINDEX(''%'' + rb_Countries.CountryID + ''%'', @Filter) > 0)
ORDER BY   rb_Countries.CountryID'
go

drop procedure rb_GetStates
go
