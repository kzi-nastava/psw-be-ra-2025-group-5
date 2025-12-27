using Explorer.BuildingBlocks.Core.Domain;
using System.Net.Mail;
using System.Reflection.Emit;

namespace Explorer.Stakeholders.Core.Domain.Users.Entities;

public class Person : Entity
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }

    public string? Biography { get; set; }
    public string? Motto { get; set; }
    public string? ProfileImagePath { get; set; }
    
    public int Level { get; private set; }
    public int ExperiencePoints { get; private set; }
    
    public Person(long userId, string? name, string? surname, string email)
    {
        UserId = userId;
        Name = name ?? "";
        Surname = surname ?? "";
        Email = email;
        Level = 0;
        ExperiencePoints = 0;

        Validate();
    }

    public Person()
    {
        Level = 0;
        ExperiencePoints = 0;
    }

    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (!MailAddress.TryCreate(Email, out _)) throw new ArgumentException("Invalid Email");
    }

    public bool AddExperience(int xp)
    {
        if (xp < 0) throw new ArgumentException("Cannot add negative XP.");

        ExperiencePoints += xp;
        bool leveledUp = false;

        int totalXpNeeded = GetTotalXPForLevel(Level + 1);
        while (ExperiencePoints >= totalXpNeeded)
        {
            Level++;
            totalXpNeeded = GetTotalXPForLevel(Level + 1);
            leveledUp = true;
        }

        return leveledUp;
    }

    private int GetTotalXPForLevel(int level)
    {
        if (level <= 0) return 0;
        const int baseXP = 100;
        return (int)(baseXP * Math.Pow(level, 1.5));
    }

    public int GetXPForNextLevel()
    {
        int totalNeededForNext = GetTotalXPForLevel(Level + 1);
        int remaining = totalNeededForNext - ExperiencePoints;
        return remaining > 0 ? remaining : 0;
    }

    public bool CanCreateChallenges()
    {
        return Level >= 10;
    }
}