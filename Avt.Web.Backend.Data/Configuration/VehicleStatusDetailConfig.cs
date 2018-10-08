using Avt.Web.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Avt.Web.Backend.Data.Configuration
{
    public class VehicleStatusDetailConfig : IEntityTypeConfiguration<VehicleStatusDetail>
    {
        public void Configure(EntityTypeBuilder<VehicleStatusDetail> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
        }
    }
}