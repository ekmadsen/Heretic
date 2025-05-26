namespace ErikTheCoder.Contracts.Dtos;


public record User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime PasswordChanged { get; set; }
    public bool Active { get; set; }
}