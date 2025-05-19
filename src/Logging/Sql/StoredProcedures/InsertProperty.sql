create procedure InsertProperty
	@name nvarchar(100),
	@id int output
as
begin

	declare @insertedProperty table(Id int)

	begin try		

		insert into Properties(Name)
			output inserted.Id into @insertedProperty
			values (@name)

	end try
	begin catch
	
		if ERROR_NUMBER() <> 2601 -- Ignore duplicate key insertion error (violating UX_Properties_Name index) that may occur due to race condition.
		throw

		-- Retrieve Id of property that caused duplicate key insertion error.
		insert into @insertedProperty
			select p.Id from Properties p
			where p.Name = @name
	end catch

	select @id = ip.Id from @insertedProperty ip
	return

end