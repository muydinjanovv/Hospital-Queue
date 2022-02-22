using hospital.Entity;
using Microsoft.AspNetCore.Identity;


public class Seed : BackgroundService
{
    private UserManager<QueueUser> _userM;
    private RoleManager<IdentityRole> _roleM;
    private readonly IServiceProvider _provider;

    public Seed(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<QueueUser>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new []{ "superadmin", "teacher", "student" };
        foreach(var role in roles)
        {
            if(!await _roleM.RoleExistsAsync(role))
            {
                await _roleM.CreateAsync(new IdentityRole(role));
            }
        }

        if((await _userM.FindByEmailAsync("superadmin@ilmhub.uz")) == null)
        {
            var user = new QueueUser()
            {
                Email = "superadmin@ilmhub.uz",
                PhoneNumber = "998950136172",
                UserName = "superadmin",
                Fullname = "Super Admin",
                Birthdate = DateTimeOffset.Now
            };

            var result = await _userM.CreateAsync(user, "123456");
            if(result.Succeeded)
            {
                var newUser = await _userM.FindByEmailAsync("superadmin@ilmhub.uz");
                await _userM.AddToRoleAsync(newUser, "superadmin");
            }
        }

    }
}