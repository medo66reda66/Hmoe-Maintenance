using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hmoe_Maintenance.DataBase.Entitytypeconficration
{
    public class ReviewServiceEntitytypeconficration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.HasIndex(x => new { x.MaintenanceRequestId, x.CustomerId })
                .IsUnique();
        }
    }
}
