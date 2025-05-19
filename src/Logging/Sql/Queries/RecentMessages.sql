select
	m.Id, m.Timestamp, m.CorrelationId, a.Name as Application, c.Name as Component, m.Category,
	case
		when m.LogLevel = 0 then 'Trace'
		when m.LogLevel = 1 then 'Debug'
		when m.LogLevel = 2 then 'Information'
		when m.LogLevel = 3 then 'Warning'
		when m.LogLevel = 4 then 'Error'
		when m.LogLevel = 5 then 'Critical'
		else 'None'
	end as LogLevel,
	m.EventId, m.EventName, m.Text
from Messages m
inner join Components c on m.ComponentId = c.Id
inner join Applications a on m.ApplicationId = a.Id
where m.Timestamp >= dateadd(hour, -1, sysutcdatetime())
order by m.Timestamp desc