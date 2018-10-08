using Avt.Web.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Avt.Web.Backend.Data.Configuration
{
    public class OwnerConfig : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Owner> builder)
        {
            builder.HasData(
                new Owner()
                {
                    Id = "Kalles Grustransporter AB",
                    Address = "Cementvägen 8, 111 11 Södertälje"
                },
                new Owner()
                {
                    Id = "Johans Bulk AB",
                    Address = "Balkvägen 12, 222 22 Stockholm"
                },
                new Owner()
                {
                    Id = "Haralds Värdetransporter AB",
                    Address = "Budgetvägen 1, 333 33 Uppsala"
                });

        }
    }
}
