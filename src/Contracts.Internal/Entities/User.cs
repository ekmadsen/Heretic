namespace ErikTheCoder.Contracts.Internal.Entities;


public record User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public int PasswordManagerId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime PasswordChanged { get; set; }
    public bool Active { get; set; }
}