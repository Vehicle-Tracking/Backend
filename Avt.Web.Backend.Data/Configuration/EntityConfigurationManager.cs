using System;
using Avt.Web.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Avt.Web.Backend.Data.Configuration
{
    public class EntityConfigurationManager
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);
         //   modelBuilder.HasSequence<VehicleStatusDetail>("Id");

            modelBuilder
                .ApplyConfiguration(new VehicleStatusDetailConfig())
                .ApplyConfiguration(new OwnerConfig())
                .ApplyConfiguration(new VehicleConfig());
        }
    }
}
