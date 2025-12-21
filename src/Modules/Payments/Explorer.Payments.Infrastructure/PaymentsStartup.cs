using Explorer.Payments.Core.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Payments.Infrastructure.Database;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure
{
    public static class PaymentsStartup
    {
        public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
        {
            // Registers all profiles since it works on the assembly
            services.AddAutoMapper(typeof(PaymentsProfile).Assembly);
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            //services.AddScoped<IShoppingCartService, ShoppingCartService>();
            //services.AddScoped<ITourPurchaseTokenService, TourPurchaseTokenService>();
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            //services.AddScoped<IShoppingCartRepository, ShoppingCartDbRepository>();
            //services.AddScoped<ITourPurchaseTokenRepository, TourPurchaseTokenDbRepository>();

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(DbConnectionStringBuilder.Build("payments"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            services.AddDbContext<PaymentsContext>(opt =>
                opt.UseNpgsql(dataSource,
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "payments")));
        }
    }
}
