using Microsoft.Extensions.DependencyInjection;
using NannyServices.Application.Services;

namespace NannyServices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CustomerService>();
        services.AddScoped<ProductService>();
        services.AddScoped<OrderService>();

        return services;
    }
}