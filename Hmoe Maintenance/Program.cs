
using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services;
using Hmoe_Maintenance.Services.Interfaces;
using Hmoe_Maintenance.SignalRWebAPI;
using Hmoe_Maintenance.Utilise;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.Tasks;

namespace Hmoe_Maintenance
{
    public class Program
    {
        public static async Task Main(string[] args)
         {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDBcontext>(option=>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );

            builder.Services.AddSignalR();

            Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedEmail = true;
            })
              .AddEntityFrameworkStores<AppDBcontext>()
              .AddDefaultTokenProviders();

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IServiceCCategory, ServiceCCategory>();
            builder.Services.AddScoped<ICompanyServiceservice, CompanyServiceservice>();
            builder.Services.AddScoped<ICompanyCoverageAreaService, CompanyCoverageAreaService>();
            builder.Services.AddScoped<ICompanyService, Services.CompanyService>();
            builder.Services.AddScoped<ICompanyControlService,  CompanyControlService>();
            builder.Services.AddScoped<ITechnicianProfileServices, TechnicianProfileServices>();
            builder.Services.AddScoped<ITechnicianerviceSesrvice, TechnicianerviceSesrvice>();
            builder.Services.AddScoped<ITechnicianControlService, TechnicianControlService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IAdminCompanyTechService, AdminCompanyTechService>();
            builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
            builder.Services.AddScoped<IProfileUserService, ProfileUserService>();
            builder.Services.AddScoped<ILockunlockUserService, LockunlockUserService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAdminTechnicianByCOMPService, AdminTechnicianByCOMPService>();
            builder.Services.AddScoped<ICompanyProfileAndDetailsService, CompanyProfileAndDetailsService>();
            builder.Services.AddScoped<IDBIntializer,DBIntializer>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
          

            builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:7066",
                    ValidAudience = "https://localhost:7066",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Nf7$Pq19!sD@84LmZ#xT2wQvR%k9Hp36"))
                };
            });

          

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBIntializer>();
            await service!.Intialize();

           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapHub<NotificationHub>("/notificationHub");
            app.MapControllers();

            app.Run();
        }
    }
}
