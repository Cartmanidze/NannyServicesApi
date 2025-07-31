using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NannyServices.Domain.Repositories;
using NannyServices.Infrastructure.Data;
using NannyServices.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace NannyServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var env = configuration[WebHostDefaults.EnvironmentKey];

        var isTest = string.Equals(env, "Test", StringComparison.OrdinalIgnoreCase);
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (!isTest)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}