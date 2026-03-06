using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Options;
using Scrutor;

namespace RentACarServer.Infrastructure;

public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtSetupOptions>();
        
        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();

        services.Configure<MailSettingOptions>(configuration.GetSection("MailSettings"));
        
        var mailSettings = configuration
            .GetSection("MailSettings")
            .Get<MailSettingOptions>() ?? new MailSettingOptions();
        

        if (string.IsNullOrEmpty(mailSettings.UserId))
        {
            services.AddFluentEmail(mailSettings.Email)
                .AddSmtpSender(
                    mailSettings.Smtp,
                    mailSettings.Port);
        }

        else
        {
            services.AddFluentEmail(mailSettings.Email)
                .AddSmtpSender(
                    mailSettings.Smtp,
                    mailSettings.Port,
                    mailSettings.UserId,
                    mailSettings.Password);
        }
        services.AddHttpContextAccessor();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("SqlServer")
                                   ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");

            options.UseSqlServer(connectionString);
        });


        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services.Scan(action => action
            .FromAssemblies(typeof(ServiceRegistrar).Assembly)
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        
        return services;
    }
}