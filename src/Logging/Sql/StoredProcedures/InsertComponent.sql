create procedure InsertComponent
	@name nvarchar(100),
	@id int output
as
begin

	declare @insertedComponent table(Id int)

	begin try		

		insert into Components(Name)
			output inserted.Id into @insertedComponent
			values (@name)

	end try
	begin catch
	
		if ERROR_NUMBER() <> 2601 -- Ignore duplicate key insertion error (violating UX_Components_Name index) that may occur due to race condition.
		throw

		-- Retrieve Id of component that caused duplicate key insertion error.
		insert into @insertedComponent
			select c.Id from Components c
			where c.Name = @name
	end catch

	select @id = ic.Id from @insertedComponent ic
	return

end