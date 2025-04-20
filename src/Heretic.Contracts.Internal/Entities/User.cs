using JetBrains.Annotations;


namespace ErikTheCoder.Heretic.Contracts.Internal.Entities;


// ReSharper disable UnusedAutoPropertyAccessor.Global
[UsedImplicitly]
public record User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}