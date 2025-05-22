using Microsoft.AspNetCore.Identity;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.Services.Identity;

namespace SuperFilm.Enerji.WebUI.Services
{
    public class SeedService
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope=serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EnerjiDbContext>();
            var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var logger=scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();
            try
            {
                logger.LogInformation("Ensuring the database is created");
                await context.Database.EnsureCreatedAsync();

                logger.LogInformation("Seeding Roles");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "SuperAdmin");
                await AddRoleAsync(roleManager, "User");
                await AddRoleAsync(roleManager, "Manager");

                logger.LogInformation("Seeding Admin User");
                var adminEmail = "admin@sanshine.com";
                if(await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new User
                    {
                        UserName = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed=true,
                        SecurityStamp=Guid.NewGuid().ToString()
                       
                    };
                    var result = await userManager.CreateAsync(adminUser,"Admin@123");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Assigning Admin role to the admin user");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogError("Failed to create admin user");
                    }
                }
                logger.LogInformation("Seeding SuperAdmin User");
                var superAdminEmail = "superadmin@sanshine.com";
                if(await userManager.FindByEmailAsync(superAdminEmail) == null)
                {
                    var superAdminUser = new User
                    {
                        UserName = superAdminEmail,
                        NormalizedUserName = superAdminEmail.ToUpper(),
                        Email = superAdminEmail,
                        NormalizedEmail = superAdminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin@123");
                    if(!result.Succeeded)
                    {
                        logger.LogInformation("Assigning SuperAdmin role to the superAdmin user");
                        await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    }
                    else
                    {
                        logger.LogError("Failed to the create SuperAdmin user");
                    }
                }
           
                



            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occured while sending the database");
            }
        }
        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if(!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if(!result.Succeeded)
                {
                    throw new Exception("Failed to create role");
                }
            }
        }
    }
}
