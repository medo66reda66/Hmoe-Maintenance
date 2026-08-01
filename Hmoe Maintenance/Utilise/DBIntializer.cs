using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.IO;
using System.Threading.Tasks;

namespace Hmoe_Maintenance.Utilise
{
    public class DBIntializer : IDBIntializer
    {
        private readonly AppDBcontext _Context;
        private readonly ILogger<DBIntializer> _logger;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public DBIntializer(AppDBcontext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<DBIntializer> logger)
        {
            _Context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
            _logger = logger;
        }

        public async Task Intialize()
        {
            try
            {
                if (_Context.Database.GetPendingMigrations().Any())
                {
                    _Context.Database.Migrate();
                }
                if(!_RoleManager.Roles.Any())
                {
                    _RoleManager.CreateAsync(new(DS.ADMIN_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.TECHNICAL_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.COMPANYOWNER_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.CLIENT_ROLE)).GetAwaiter().GetResult();

                   var result = await _UserManager.CreateAsync(new ApplicationUser
                    {
                        UserName="Admin",
                        Email="Admin123@Hoda.com",
                        FullName="Mahmoud reda zahra",
                        PhoneNumber="01120811023",
                        EmailConfirmed = true,
                    },"admin123#");

                    if (!result.Succeeded) 
                    {
                        foreach (var error in result.Errors)
                            Console.WriteLine(error.Description);
                    }

                    var user = await _UserManager.FindByEmailAsync("Admin123@Hoda.com");

                    var address = new Address
                    { 
                        ApplicationUserId = user.Id,
                        Title = "mmmmmmmm",
                        Governorate = "cairo",
                        City = " Nasr",
                        Area = "vvvvvv",
                        Street = "blablabla",
                        BuildingNumber = "00"
                    };
                    await _Context.AddAsync(address);
                    _Context.SaveChanges();

                   await _UserManager.AddToRoleAsync(user!,DS.ADMIN_ROLE);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Erorr{ex.Message}");
            }
        }


    }
}
