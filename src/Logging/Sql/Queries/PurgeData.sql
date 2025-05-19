delete from PropertyValues
dbcc checkident ('PropertyValues', reseed, 0)

delete from Messages
dbcc checkident ('Messages', reseed, 0)

delete from Properties
dbcc checkident ('Properties', reseed, 0)

delete from Components
dbcc checkident ('Components', reseed, 0)

delete from Applications
dbcc checkident ('Applications', reseed, 0)