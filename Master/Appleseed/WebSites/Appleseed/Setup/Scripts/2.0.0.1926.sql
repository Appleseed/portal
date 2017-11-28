
IF NOT EXISTS(SELECT * FROM aspnet_Permission WHERE PermissionID = 11)
BEGIN
	Insert into aspnet_Permission VALUES (11, 'File Add/Edit/Rename','Allow user to upload new files/edit files using File Manager')
END
IF NOT EXISTS(SELECT * FROM aspnet_Permission WHERE PermissionID = 12)
BEGIN
	Insert into aspnet_Permission VALUES (12, 'File Delete','Allow user to delete file content using File Manager')
END