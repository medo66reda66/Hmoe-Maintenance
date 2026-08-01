using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hmoe_Maintenance.DataBase.Entitytypeconficration
{
    public class CompanyServiceEntitytypeconficration : IEntityTypeConfiguration<CompanyService>
    {
        public void Configure(EntityTypeBuilder<CompanyService> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.ServiceCategoryId, x.CompanyId })
                .IsUnique();
            
        }
    }
}
