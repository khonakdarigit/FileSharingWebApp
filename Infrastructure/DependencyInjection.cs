using Domain.Repositories.Common;
using Domain.Repositories;
using Infrastructure.Repositories.Common;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        //public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var connectionString = configuration.GetConnectionString("DefaultConnection")
        //                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        //    services.AddDbContext<ApplicationDbContext>(options =>
        //        options.UseSqlServer(connectionString));

        //    // ثبت Identity
        //    services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //            .AddEntityFrameworkStores<ApplicationDbContext>();

        //    services.AddDatabaseDeveloperPageExceptionFilter();

        //    return services;
        //}


        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
