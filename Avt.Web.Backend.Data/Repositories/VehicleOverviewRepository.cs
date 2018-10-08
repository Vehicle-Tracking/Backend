using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avt.Web.Backend.Data.Base;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Spec;
using Avt.Web.Backend.Data.Types;

namespace Avt.Web.Backend.Data.Repositories
{
    public class VehicleOverviewRepository : Repository<VehicleAggregateOverview, string>, IVehicleOverviewRepository
    {
        public VehicleOverviewRepository() { }
        public VehicleOverviewRepository(IDataContext dataContext) : base(dataContext) { }

        public async Task UpdateOverviewAsync(Vehicle vehicle, VehicleStatusDetail vehicleStatus)
        {
            var currentStatus = await this.DbSet.FindAsync(vehicle.Id);
            if (currentStatus != null)
            {
                currentStatus.Total += 1;
                currentStatus.LastSync = vehicleStatus.SyncDate;
                currentStatus.LastStatus = vehicleStatus.Status;

                if (vehicleStatus.Status == VehicleStatus.Connected)
                    currentStatus.ConnectedStatusCount++;
                else
                    currentStatus.DisconnectedStatusCount++;
            }
            else
            {
                await this.DbSet.AddAsync(new VehicleAggregateOverview()
                {
                    Id = vehicle.Id,
                    RegNumber = vehicle.RegNumber,
                    OwnerName = vehicle.OwnerId,
                    ConnectedStatusCount = vehicleStatus.Status == VehicleStatus.Connected ? 1 : 0,
                    DisconnectedStatusCount = vehicleStatus.Status == VehicleStatus.Disconnected ? 1 : 0,
                    Total = 1,
                    LastSync = vehicleStatus.SyncDate,
                    LastStatus = vehicleStatus.Status
                });
            }

            await this.DbContext.SaveChangesAsync();
        }
    }

    public interface IVehicleOverviewRepository : IRepository<VehicleAggregateOverview, string>
    {
        Task UpdateOverviewAsync(Vehicle vehicle, VehicleStatusDetail vehicleStatus);
    }
}
