using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Users;

public enum UserRole
{
    Administrator,
    Author,
    Tourist
}

public class User : Entity
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Email { get; private set; }   // Svaki nalog obuhvata i Email
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    public User() { }

    public User(string username, string password, string email, UserRole role, bool isActive)
    {
        Username = username;
        Password = password;
        Email = email;
        Role = role;
        IsActive = isActive;
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username)) throw new ArgumentException("Invalid Username");
        if (string.IsNullOrWhiteSpace(Password)) throw new ArgumentException("Invalid Password");
        if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentException("Invalid Email");
    }

    public string GetPrimaryRoleName()
    {
        return Role.ToString().ToLower();
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        if (Role == UserRole.Administrator)
            throw new InvalidOperationException("Cannot block an Administrator account.");
        IsActive = false;
    }

}
