Create Table dbo.rb_userPagePermission
(
	Id int identity PRIMARY KEY,
	PageId int,
	UserId uniqueidentifier,
	Permission int
)
GO

