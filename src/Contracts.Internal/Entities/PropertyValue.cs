namespace ErikTheCoder.Contracts.Internal.Entities;


public record PropertyValue
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public int PropertyId { get; set; }
    public bool? BoolValue { get; set; }
    public int? IntValue { get; set; }
    public long? LongValue { get; set; }
    public decimal? DecimalValue { get; set; }
    public DateTimeOffset? DateTimeValue { get; set; }
    public string ShortTextValue { get; set; }
    public string LongTextValue { get; set; }
}