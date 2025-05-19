declare @id int
exec InsertProperty 'Email', @id output
select @id