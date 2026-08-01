using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hmoe_Maintenance.DataBase.Entitytypeconficration
{
    public class AdditionalCostImageEntitytypeconficration : IEntityTypeConfiguration<AdditionalCostImage>
    {
        public void Configure(EntityTypeBuilder<AdditionalCostImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.AdditionalCostRequestId, x.ImageUrl })
                .IsUnique();
        }
    }
}
