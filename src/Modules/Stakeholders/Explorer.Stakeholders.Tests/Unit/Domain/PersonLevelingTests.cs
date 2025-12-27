using Shouldly;
using Explorer.Stakeholders.Core.Domain.Users.Entities;

namespace Explorer.Stakeholders.Tests.Unit.Domain;

public class PersonLevelingTests
{
    [Fact]
    public void Person_starts_at_level_zero_with_zero_xp()
    {
        // Arrange & Act
        var person = new Person(1, "Test", "User", "test@example.com");

        // Assert
        person.Level.ShouldBe(0);
        person.ExperiencePoints.ShouldBe(0);
    }

    [Fact]
    public void AddExperience_increases_xp()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act
        person.AddExperience(50);

        // Assert
        person.ExperiencePoints.ShouldBe(50);
        person.Level.ShouldBe(0); // Not enough for level 1
    }

    [Fact]
    public void AddExperience_levels_up_at_100_xp()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act
        var leveledUp = person.AddExperience(100);

        // Assert
        leveledUp.ShouldBeTrue();
        person.Level.ShouldBe(1);
        person.ExperiencePoints.ShouldBe(100);
    }

    [Fact]
    public void AddExperience_accumulates_without_subtracting()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act
        person.AddExperience(100); // Level 1
        person.AddExperience(100);

        // Assert
        person.ExperiencePoints.ShouldBe(200);
        person.Level.ShouldBe(1); // Still level 1, needs 282 total for level 2
    }

    [Fact]
    public void AddExperience_reaches_level_2_at_282_xp()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act - Level formula: 100 * level^1.5
        // Level 2: 282 XP (100 * 2^1.5 ? 282.84)
        var leveledUp = person.AddExperience(282);

        // Assert
        leveledUp.ShouldBeTrue();
        person.Level.ShouldBe(2);
        person.ExperiencePoints.ShouldBe(282);
    }

    [Fact]
    public void AddExperience_can_skip_multiple_levels()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act - Give 1000 XP at once
        var leveledUp = person.AddExperience(1000);

        // Assert
        leveledUp.ShouldBeTrue();
        person.Level.ShouldBeGreaterThan(2);
        person.ExperiencePoints.ShouldBe(1000);
    }

    [Fact]
    public void AddExperience_fails_with_negative_xp()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act & Assert
        Should.Throw<ArgumentException>(() => person.AddExperience(-50));
    }

    [Fact]
    public void GetXPForNextLevel_returns_correct_amount_at_level_0()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act
        var xpNeeded = person.GetXPForNextLevel();

        // Assert - Need 100 XP to reach level 1
        xpNeeded.ShouldBe(100);
    }

    [Fact]
    public void GetXPForNextLevel_returns_remaining_xp_needed()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");
        person.AddExperience(50);

        // Act
        var xpNeeded = person.GetXPForNextLevel();

        // Assert - Need 50 more XP to reach level 1
        xpNeeded.ShouldBe(50);
    }

    [Fact]
    public void GetXPForNextLevel_at_level_1()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");
        person.AddExperience(100); // Reach level 1

        // Act
        var xpNeeded = person.GetXPForNextLevel();

        // Assert - Need 182 more XP to reach level 2 (282 total - 100 current)
        xpNeeded.ShouldBe(182);
    }

    [Fact]
    public void CanCreateChallenges_is_false_below_level_10()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");
        person.AddExperience(500); // Should be around level 3-4

        // Act & Assert
        person.CanCreateChallenges().ShouldBeFalse();
    }

    [Fact]
    public void CanCreateChallenges_is_true_at_level_10()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");
        
        // Calculate XP needed for level 10
        // This is cumulative: sum of XP for levels 1-10
        int totalXpForLevel10 = 0;
        for (int i = 1; i <= 10; i++)
        {
            totalXpForLevel10 += (int)(100 * Math.Pow(i, 1.5));
        }

        // Act
        person.AddExperience(totalXpForLevel10);

        // Assert
        person.Level.ShouldBeGreaterThanOrEqualTo(10);
        person.CanCreateChallenges().ShouldBeTrue();
    }

    [Fact]
    public void AddExperience_multiple_times_accumulates_correctly()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act - Add XP in smaller chunks
        person.AddExperience(50);
        person.AddExperience(30);
        person.AddExperience(20);

        // Assert
        person.ExperiencePoints.ShouldBe(100);
        person.Level.ShouldBe(1);
    }

    [Fact]
    public void Leveling_formula_is_exponential()
    {
        // Arrange
        var person = new Person(1, "Test", "User", "test@example.com");

        // Act - Reach level 1 and level 2
        person.AddExperience(100); // Level 1
        var xpForLevel2 = person.GetXPForNextLevel();
        
        person.AddExperience(xpForLevel2); // Level 2
        var xpForLevel3 = person.GetXPForNextLevel();

        // Assert - XP requirement should increase (exponential growth)
        xpForLevel3.ShouldBeGreaterThan(xpForLevel2);
    }
}
