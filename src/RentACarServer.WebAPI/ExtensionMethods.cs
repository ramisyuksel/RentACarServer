using GenericRepository;
using RentACarServer.Application.Services;
using RentACarServer.Domain.Users;
using RentACarServer.Domain.Users.ValueObjects;

namespace RentACarServer.WebAPI;

public static class ExtensionMethods
{
    public static async Task CreateFirstUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userRepository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (!(await userRepository.AnyAsync(p => p.UserName.Value == "admin")))
        {
            FirstName firstName = new("Admin");
            LastName lastName = new("User");
            Email email = new("admin@admin.com");
            UserName userName = new("admin");
            Password password = new("1");

            User user = new(
                firstName,
                lastName,
                email,
                userName,
                password);

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public static async Task CleanRemovedPermissionsFromRoleAsync(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var srv = scoped.ServiceProvider;
        var service = srv.GetRequiredService<PermissionCleanerService>();
        await service.CleanRemovedPermissionsFromRolesAsync();
    }
}