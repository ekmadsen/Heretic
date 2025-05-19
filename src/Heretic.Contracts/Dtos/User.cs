namespace ErikTheCoder.Heretic.Contracts.Dtos;


// ReSharper disable UnusedAutoPropertyAccessor.Global
public record User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}