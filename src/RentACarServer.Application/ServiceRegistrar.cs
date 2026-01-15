using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentACarServer.Application.Behaviors;
using RentACarServer.Application.Services;
using TS.MediatR;

namespace RentACarServer.Application;

public static class ServiceRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PermissionService>(); 
        services.AddMediatR(cfr =>
        {
            cfr.RegisterServicesFromAssembly(typeof(ServiceRegistrar).Assembly);
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServiceRegistrar).Assembly);

        return services;
    }
}

