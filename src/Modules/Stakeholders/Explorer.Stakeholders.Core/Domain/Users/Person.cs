using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Social;
using System.Net.Mail;

namespace Explorer.Stakeholders.Core.Domain.Users;

public class Person : Entity
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }

    public string? Biography { get; set; }
    public string? Motto { get; set; }
    public string? ProfileImagePath { get; set; }

    public ICollection<ProfileFollow> Following { get; private set; } = [];
    public ICollection<ProfileFollow> Followers { get; private set; } = [];

    private Person() { }

    public Person(long userId, string? name, string? surname, string email)
    {
        UserId = userId;
        Name = name ?? "";
        Surname = surname ?? "";
        Email = email;

        Validate();
    }

    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (!MailAddress.TryCreate(Email, out _)) throw new ArgumentException("Invalid Email");
    }
}