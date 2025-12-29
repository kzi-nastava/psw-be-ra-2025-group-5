using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Public.AppRatings;
using Explorer.Stakeholders.API.Public.Clubs;
using Explorer.Stakeholders.API.Public.Diaries;
using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.API.Public.Positions;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.AppRatings;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Diaries;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Notifications;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Positions;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TourProblems;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Mappers;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Core.UseCases.ClubMembership;
using Explorer.Stakeholders.Core.UseCases.Administration.Social;
using Explorer.Stakeholders.Core.UseCases.Administration.Users;
using Explorer.Stakeholders.Core.UseCases.Reporting;
using Explorer.Stakeholders.Core.UseCases.Statistics;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.AppRatings;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.Clubs;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.Notifications;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.Positions;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.TourProblems;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Stakeholders.Infrastructure
{
    public static class StakeholdersStartup
    {
        public static IServiceCollection ConfigureStakeholdersModule(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(StakeholderProfile).Assembly);
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenGenerator, JwtGenerator>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserInfoService, UserService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IAppRatingService, AppRatingService>();
            services.AddScoped<ITourProblemService, TourProblemService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ITouristStatisticsService, TouristStatisticsService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDiaryService, DiaryService>();
            services.AddScoped<IClubInviteService, ClubInviteService>();
            services.AddScoped<IClubJoinRequestService, ClubJoinRequestService>();
            services.AddScoped<IPaymentNotificationService, NotificationService>();

        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonDbRepository>();
            services.AddScoped<IUserRepository, UserDbRepository>();
            services.AddScoped<IClubRepository, ClubDbRepository>();
            services.AddScoped<IAppRatingRepository, AppRatingDbRepository>();
            services.AddScoped<ITourProblemRepository, TourProblemDbRepository>();
            services.AddScoped<IPositionRepository, PositionDbRepository>();
            services.AddScoped<INotificationRepository, NotificationDbRepository>();
            services.AddScoped<IDiaryRepository, DiaryRepository>();
            services.AddScoped<IClubInviteRepository, ClubInviteDbRepository>();
            services.AddScoped<IClubJoinRequestRepository, ClubJoinRequestDbRepository>();


            var dataSourceBuilder = new NpgsqlDataSourceBuilder(DbConnectionStringBuilder.Build("stakeholders"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<StakeholdersContext>(opt =>
                opt.UseNpgsql(dataSource,
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
        }
    }
}
