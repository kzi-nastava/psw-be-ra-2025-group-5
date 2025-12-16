using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Mappers;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Core.UseCases.Reporting;
using Explorer.Stakeholders.Core.UseCases.Statistics;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories;
using Explorer.Tours.API.Internal.Statistics;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IAppRatingService, AppRatingService>();
            services.AddScoped<ITourProblemService, TourProblemService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ITouristStatisticsService, TouristStatisticsService>();
            services.AddScoped<ITourStatisticsDbRepository, TourStatisticsDbRepository>();

        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonDbRepository>();
            services.AddScoped<IUserRepository, UserDbRepository>();
            services.AddScoped<IClubRepository, ClubDbRepository>();
            services.AddScoped<IAppRatingRepository, AppRatingDbRepository>();
            services.AddScoped<ITourProblemRepository, TourProblemDbRepository>();
            services.AddScoped<IPositionRepository, PositionDbRepository>();

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(DbConnectionStringBuilder.Build("stakeholders"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<StakeholdersContext>(opt =>
                opt.UseNpgsql(dataSource,
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
        }
    }
}
