namespace ErikTheCoder.Data.Options;


public record DatabaseOptions
{
    public string Name { get; set; }
    public string Connection { get; set; }
    public bool LogQueries { get; set; }
}