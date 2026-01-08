using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Equipments;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Facilities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Monuments;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Core.UseCases.Tours;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database.Repositories.Equipments;
using Explorer.Tours.Infrastructure.Database.Repositories.Facilities;
using Explorer.Tours.Infrastructure.Database.Repositories.Monuments;
using Explorer.Tours.Infrastructure.Database.Repositories.Preferences;
using Explorer.Tours.Infrastructure.Database.Repositories.Statistics;
using Explorer.Tours.Infrastructure.Database.Repositories.Tours;
using Explorer.Tours.Infrastructure.Database.Repositories.Tours.Executions;
using Explorer.Tours.API.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<ITouristPreferencesService, TouristPreferencesService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<ITourSharedService, TourService>();
        services.AddScoped<IMonumentService, MonumentService>();
        services.AddScoped<ITouristEquipmentService, TouristEquipmentService>();
        services.AddScoped<IFacilityService, FacilityService>();
        services.AddScoped<ITourExecutionService, TourExecutionService>();
        services.AddScoped<ITourStatisticsService, TourStatisticsService>();
        services.AddScoped<ITourAnalyticsService, TourStatisticsService>();
        services.AddScoped<ITourManualService, TourManualService>(); 
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IEquipmentRepository, EquipmentDbRepository>();
        services.AddScoped<ITouristPreferencesRepository, TouristPreferencesDbRepository>();
        services.AddScoped<ITourRepository, TourDbRepository>();
        services.AddScoped<IMonumentRepository, MonumentDbRepository>();
        services.AddScoped<ITouristEquipmentRepository, TouristEquipmentDbRepository>();
        services.AddScoped<IFacilityRepository, FacilityDbRepository>();
        services.AddScoped<ITourExecutionRepository, TourExecutionDbRepository>();
        services.AddScoped<ITourStatisticsDbRepository, TourStatisticsDbRepository>();
        services.AddScoped<ITourAnalyticsService, TourStatisticsService>();
        services.AddScoped<ITourManualRepository, TourManualDbRepository>();

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(DbConnectionStringBuilder.Build("tours"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(dataSource,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
    }
}
