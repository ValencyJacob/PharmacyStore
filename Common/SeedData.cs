using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public /*static*/ class SeedData
    {
        //public static async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    await SeedRoles(roleManager);
        //    await SeedUsers(userManager);
        //}

        //private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        //{
        //    if(await userManager.FindByEmailAsync("admin@mail.com") == null)
        //    {
        //        var user = new IdentityUser
        //        {
        //            UserName = "admin@mail.com",
        //            Email = "admin@mail.com"
        //        };

        //        var result = await userManager.CreateAsync(user, "111111Test$");

        //        if (result.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(user, StaticDetails.Role_Admin);
        //        }
        //    }

        //    if (await userManager.FindByEmailAsync("customer@mail.com") == null)
        //    {
        //        var user = new IdentityUser
        //        {
        //            UserName = "customer@mail.com",
        //            Email = "customer@mail.com"
        //        };

        //        var result = await userManager.CreateAsync(user, "111111Test$");

        //        if (result.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(user, StaticDetails.Role_Customer);
        //        }
        //    }
        //}

        //private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        //{
        //    if (!await roleManager.RoleExistsAsync(StaticDetails.Role_Admin))
        //    {
        //        var role = new IdentityRole
        //        {
        //            Name = StaticDetails.Role_Admin
        //        };

        //        await roleManager.CreateAsync(role);
        //    }

        //    if (!await roleManager.RoleExistsAsync(StaticDetails.Role_Customer))
        //    {
        //        var role = new IdentityRole
        //        {
        //            Name = StaticDetails.Role_Customer
        //        };

        //        await roleManager.CreateAsync(role);
        //    }
        //}
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }

            if (_context.Roles.Any(x => x.Name == StaticDetails.Role_Admin))
                return;

            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin))
                .GetAwaiter().GetResult();

            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer))
                .GetAwaiter().GetResult();

            _userManager.CreateAsync(new IdentityUser
            {
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                EmailConfirmed = true
            }, "111111Test$").GetAwaiter().GetResult();

            IdentityUser user = _context.Users.FirstOrDefault(x => x.Email == "admin@mail.com");

            _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
