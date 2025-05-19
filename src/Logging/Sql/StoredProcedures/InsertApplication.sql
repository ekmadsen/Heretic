create procedure InsertApplication
	@name nvarchar(100),
	@id int output
as
begin

	declare @insertedApplication table(Id int)

	begin try		

		insert into Applications(Name)
			output inserted.Id into @insertedApplication
			values (@name)

	end try
	begin catch
	
		if ERROR_NUMBER() <> 2601 -- Ignore duplicate key insertion error (violating UX_Applications_Name index) that may occur due to race condition.
		throw

		-- Retrieve Id of application that caused duplicate key insertion error.
		insert into @insertedApplication
			select a.Id from Applications a
			where a.Name = @name
	end catch

	select @id = ia.Id from @insertedApplication ia
	return

end