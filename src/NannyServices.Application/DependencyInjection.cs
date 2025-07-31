using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using NannyServices.Application.Common.Behaviors;

namespace NannyServices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}