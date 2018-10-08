using Avt.Web.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Avt.Web.Backend.Data.Configuration
{
    public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasData(
                  new Vehicle() { Id = "YS2R4X20005399401", RegNumber = "ABC123", OwnerId = "Kalles Grustransporter AB" },
                  new Vehicle() { Id = "VLUR4X20009093588", RegNumber = "DEF456", OwnerId = "Kalles Grustransporter AB" },
                  new Vehicle() { Id = "VLUR4X20009048066", RegNumber = "GHI789", OwnerId = "Kalles Grustransporter AB" },
                  new Vehicle() { Id = "YS2R4X20005388011", RegNumber = "JKL012", OwnerId = "Johans Bulk AB" },
                  new Vehicle() { Id = "YS2R4X20005387949", RegNumber = "MNO345", OwnerId = "Johans Bulk AB" },
                  new Vehicle() { Id = "VLUR4X20009048065", RegNumber = "PQR678", OwnerId = "Haralds Värdetransporter AB" },
                  new Vehicle() { Id = "YS2R4X20005387055", RegNumber = "STU901", OwnerId = "Haralds Värdetransporter AB" }
                );

        }
    }
}
