-- Remove Slider manager module, not required any more
DELETE FROM [rb_GeneralModuleDefinitions] WHERE [GeneralModDefID] in ('1010d514-73b2-4e46-b6a9-2c0182d22865')
Go
IF  EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[rb_SliderImages]') AND type in (N'U'))
BEGIN
drop table rb_SliderImages
END
GO
IF  EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[rb_Sliders]') AND type in (N'U'))
BEGIN
	drop table rb_Sliders
END
GO
Create Table rb_Sliders
(
	Id int primary key identity,
	ModuleId int,
	ClientQuote varchar(2000),
	ClientFirstName varchar(100),
	ClientLastName varchar(100),
	BackgroudImageUrl varchar(1000),
	BackgroudColor varchar(100)
)
GO
Alter table dbo.rb_Sliders
Add ClientWorkPosition varchar(200)
GO