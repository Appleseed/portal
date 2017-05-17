UPDATE [dbo].[rb_GeneralModuleDefinitions] SET [Admin] = 1 WHERE [GeneralModDefID] in ('3e9629ae-dbea-4af7-b929-076359b929f0', --Admin - Evolutility Module Renderer
'7222060b-3fdb-466c-8ca8-6ac2c8328140', -- Admin - Evolutility Advanced Module Renderer
'8230d43a-7c14-4ed8-8429-6f0a60730c9d', -- Admin - Evolutility Module List
'c1ea4115-e7f2-4cbc-b1e7-dda46791493c' --Admin - Short Links
)
GO
UPDATE rb_Pages SET [AuthorizedRoles] = 'Admins;Builder;' WHERE [PageID] IN (110, 155);
GO