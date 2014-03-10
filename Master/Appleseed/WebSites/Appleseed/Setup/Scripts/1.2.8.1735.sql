---------------------
--1.2.8.1735.sql
---------------------

-- Fix for Appleseedportal-Bugs-791364: search module broken for case 'all'
UPDATE rb_generalmoduledefinitions SET ClassName='Appleseed.Content.Web.ModulesLinks' WHERE GeneralModDefID='{476CF1CC-8364-479D-9764-4B3ABD7FFABD}'
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1735','1.2.8.1735', CONVERT(datetime, '09/11/2003', 101))
GO
