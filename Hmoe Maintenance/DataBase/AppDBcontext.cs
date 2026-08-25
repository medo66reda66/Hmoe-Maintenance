using Hmoe_Maintenance.DataBase.Entitytypeconficration;
using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.DataBase
{
    public class AppDBcontext : IdentityDbContext<ApplicationUser>
    {
        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = default!;
        public DbSet<CompanyService> CompanyServices { get; set; } = default!;
        public DbSet<CompanyCoverageArea> CompanyCoverageAreas { get; set; } = default!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<AdditionalCostRequest> AdditionalCostRequests { get; set; } = default!;
        public DbSet<AdditionalCostImage> AdditionalCostImages { get; set; } = default!;
        public DbSet<MaintenanceRequestStatusHistory> MaintenanceRequestStatusHistory { get; set; } = default!;
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; } = default!;
        public DbSet<MaintenanceRequestImage> MaintenanceRequestImages { get; set; } = default!;
        public DbSet<Payment> Payment { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Notification> Notification { get; set; } = default!;
        public DbSet<Complaint> Complaint { get; set; } = default!;
        public DbSet<TechnicianProfile> TechnicianProfiles { get; set; } = default!;
        public DbSet<TechnicianService> TechnicianServices { get; set; } = default!;
        public DbSet<CompanyCopy> companyCopies { get; set; } = default!;
        public DbSet<TechnicianProfileCopy> TechnicianProfileCopies { get; set; } = default!;
        public DbSet<ApplicationuserOtp> applicationuserOtps { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Database=HmoeMaintenance;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AdditionalCostImageEntitytypeconficration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
