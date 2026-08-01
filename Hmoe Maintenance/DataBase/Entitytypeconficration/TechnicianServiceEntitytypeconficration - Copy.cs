using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hmoe_Maintenance.DataBase.Entitytypeconficration
{
    public class TechnicianServiceEntitytypeconficration : IEntityTypeConfiguration<TechnicianService>
    {
        public void Configure(EntityTypeBuilder<TechnicianService> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.ServiceCategoryId, x.TechnicianProfileId })
                .IsUnique();
            builder.HasIndex(x=>x.TechnicianProfileId).IsUnique();
        }
    }
}
