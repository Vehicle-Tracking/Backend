using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Avt.Web.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Avt.Web.Backend.Data.Spec
{
    public interface IDataContext: IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
        DbSet<T> SetEntity<T>() where T : class;
        EntityEntry<IEntity<TKey>> SetEntityEntry<TKey>(IEntity<TKey> entity);

        ChangeTracker ContextChangeTracker { get; }

        DbSet<Vehicle> VehicleDbSet { get; set; }
        DbSet<VehicleStatusDetail> VehicleStatusDetailDbSet { get; set; }
        DbSet<VehicleAggregateOverview> VehicleAggregateOverviewDbSet { get; set; }
    }
}