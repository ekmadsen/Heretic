namespace ErikTheCoder.Logging.Entities;


// ReSharper disable UnusedAutoPropertyAccessor.Global
public record Message
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public byte LogLevel { get; set; }
    public int ApplicationId { get; set; }
    public int ComponentId { get; set; }
    public Guid? CorrelationId { get; set; }
    public string Category { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; }
    public string Text { get; set; }
}