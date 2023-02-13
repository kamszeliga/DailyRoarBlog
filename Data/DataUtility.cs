using DailyRoarBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DailyRoarBlog.Data
{
    public static class DataUtility
    {
        private const string _adminRole = "Admin";
        private const string _moderatorRole = "Moderator";
        private const string _adminEmail = "dailyroaradmin@mailinator.com";
        private const string _moderatorEmail = "dailyroarmod@mailinator.com";

        public static DateTime GetPostGresDate(DateTime dateTime) 
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
        public static string GetConnectionString(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);

        }
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            // Service: An instance of RoleManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

            //Service: An instance of Confirutation Service
            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();

            // Service: An instance of UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BlogUser>>();

            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Migration: this is the programic equivilant to update database
            await dbContextSvc.Database.MigrateAsync();

            //Seed Roles
            await SeedRolesAsync(roleManagerSvc);

            //Seed Users
            await SeedUsersAsync(dbContextSvc, configurationSvc, userManagerSvc);

            //Seed Demo User
            //await SeedDemoUserAsync(userManagerSvc);

        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager) 
        {   
            //Seed Admin User
            // ! = NOT
            if (!await roleManager.RoleExistsAsync(_adminRole)) 
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole));
            }
            if (!await roleManager.RoleExistsAsync(_moderatorRole)) 
            {
                await roleManager.CreateAsync(new IdentityRole(_moderatorRole));
            }
        }

        private static async Task SeedUsersAsync(ApplicationDbContext context, IConfiguration config, UserManager<BlogUser> userManager) 
        { 
            if (!context.Users.Any(u=>u.Email == _adminEmail))
            {
                BlogUser adminUser = new()
                {
                    Email = _adminEmail,
                    UserName = _adminEmail,
                    FirstName = "Kam",
                    LastName = "Szeliga",
                    EmailConfirmed= true,
                };

                await userManager.CreateAsync(adminUser, config["AdminPwd"] ?? Environment.GetEnvironmentVariable("AdminPwd")!);
                await userManager.AddToRoleAsync(adminUser, _adminRole);
            }
            if (!context.Users.Any(u=>u.Email == _moderatorEmail))
            {
                BlogUser moderatorUser = new()
                {
                    Email = _moderatorEmail,
                    UserName = _moderatorEmail,
                    FirstName = "Blog",
                    LastName = "Moderator",
                    EmailConfirmed= true,
                };

                await userManager.CreateAsync(moderatorUser, config["ModeratorPwd"] ?? Environment.GetEnvironmentVariable("ModeratorPwd")!);
                await userManager.AddToRoleAsync(moderatorUser, _moderatorRole);
            }


        }

    }
}
