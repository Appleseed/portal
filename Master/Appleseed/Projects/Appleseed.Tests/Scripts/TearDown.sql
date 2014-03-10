-- CAUTION

-- Drops all user views in the current database.

WHILE EXISTS(SELECT [name] FROM sys.views WHERE [type] = 'V')
BEGIN
DECLARE @view_name varchar(500)
DECLARE view_cursor CURSOR FOR SELECT [name] FROM sys.views WHERE [type] = 'V'
OPEN view_cursor
FETCH NEXT FROM view_cursor INTO @view_name
WHILE @@FETCH_STATUS = 0
BEGIN
BEGIN TRY
EXEC ('DROP VIEW [' + @view_name + ']')
PRINT 'Dropped view ' + @view_name
END TRY
BEGIN CATCH END CATCH
FETCH NEXT FROM view_cursor INTO @view_name
END
CLOSE view_cursor
DEALLOCATE view_cursor
END

-- Drops all user tables in the current database.

WHILE EXISTS(SELECT [name] FROM sys.tables WHERE [type] = 'U')
BEGIN
DECLARE @table_name varchar(500)
DECLARE table_cursor CURSOR FOR SELECT [name] FROM sys.tables WHERE [type] = 'U'
OPEN table_cursor
FETCH NEXT FROM table_cursor INTO @table_name
WHILE @@FETCH_STATUS = 0
BEGIN
BEGIN TRY
EXEC ('DROP TABLE [' + @table_name + ']')
PRINT 'Dropped Table ' + @table_name
END TRY
BEGIN CATCH END CATCH
FETCH NEXT FROM table_cursor INTO @table_name
END
CLOSE table_cursor
DEALLOCATE table_cursor
END

-- Drops all user stored procs in the current database.

WHILE EXISTS(SELECT [name] FROM sys.procedures WHERE [type] = 'P')
BEGIN
DECLARE @proc_name varchar(500)
DECLARE proc_cursor CURSOR FOR SELECT [name] FROM sys.procedures WHERE [type] = 'P'
OPEN proc_cursor
FETCH NEXT FROM proc_cursor INTO @proc_name
WHILE @@FETCH_STATUS = 0
BEGIN
BEGIN TRY
EXEC ('DROP PROCEDURE [' + @proc_name + ']')
PRINT 'Dropped Procedure ' + @proc_name
END TRY
BEGIN CATCH END CATCH
FETCH NEXT FROM proc_cursor INTO @proc_name
END
CLOSE proc_cursor
DEALLOCATE proc_cursor
END
