using Explorer.Stakeholders.API.Public.Badges;
using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Microsoft.Extensions.Logging;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Badges;

public class DailyBadgeCheckService : IDailyBadgeCheckService
{
    private readonly IPersonRepository _personRepository;
    private readonly IUserStatisticsService _userStatisticsService;
    private readonly IBadgeService _badgeService;
    private readonly IUserBadgeService _userBadgeService;
    private readonly ILogger<DailyBadgeCheckService> _logger;

    public DailyBadgeCheckService(
        IPersonRepository personRepository,
        IUserStatisticsService userStatisticsService,
        IBadgeService badgeService,
        IUserBadgeService userBadgeService,
        ILogger<DailyBadgeCheckService> logger)
    {
        _personRepository = personRepository;
        _userStatisticsService = userStatisticsService;
        _badgeService = badgeService;
        _userBadgeService = userBadgeService;
        _logger = logger;
    }

    public void CheckAndAwardVeteranBadges()
    {
        try
        {
            _logger.LogInformation("Starting daily Veteran badge check at {Time}", DateTime.UtcNow);

            var allPeople = _personRepository.GetAll();

            foreach (var person in allPeople)
            {
                try
                {
                    var accountAgeDays = (int)(DateTime.UtcNow - person.CreatedAt).TotalDays;
                    
                    // Ažuriraj statistiku
                    var stats = _userStatisticsService.GetByUserId(person.UserId);
                    if (stats.AccountAgeDays != accountAgeDays)
                    {
                        _userStatisticsService.UpdateAccountAgeDays(person.UserId, accountAgeDays);
                    }

                    // Proveri bedževe
                    var badges = _badgeService.GetByType((int)BadgeType.AccountAge);
                    foreach (var badge in badges)
                    {
                        if (accountAgeDays >= badge.RequiredValue && !_userBadgeService.HasBadge(person.UserId, badge.Id))
                        {
                            _userBadgeService.AwardBadge(person.UserId, badge.Id);
                            _logger.LogInformation(
                                "Awarded Veteran badge '{BadgeName}' to user {UserId} (Account age: {Days} days)", 
                                badge.Name, 
                                person.UserId, 
                                accountAgeDays);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking Veteran badges for user {UserId}", person.UserId);
                }
            }

            _logger.LogInformation("Completed daily Veteran badge check at {Time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error during daily Veteran badge check");
        }
    }
}
