using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avt.Web.Backend.Data.Base;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Spec;
using Microsoft.EntityFrameworkCore;

namespace Avt.Web.Backend.Data.Repositories
{
    public class VehicleStatusRepository : Repository<VehicleStatusDetail, int>, IVehicleStatusRepository
    {
        public VehicleStatusRepository() { }
        public VehicleStatusRepository(IDataContext dataContext) : base(dataContext) { }
        public async Task<IEnumerable<VehicleStatusDetail>> GetByVin(string vin)
        {
            return await this.DbSet.Where(t => t.VehicleId == vin).ToListAsync();
        }

        // crazy stuff :|
        public async Task<int> FindIdAsync()
        {
            return (await this.DbSet.AnyAsync()) ? ((await this.DbSet.MaxAsync(t => t.Id)) + 1) : 1;
        }

        public void ClearChangeTracker()
        {
            DbContext.ContextChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);
        }
    }

    public interface IVehicleStatusRepository : IRepository<VehicleStatusDetail, int>
    {
        Task<IEnumerable<VehicleStatusDetail>> GetByVin(string vin);
         Task<int> FindIdAsync();
         void ClearChangeTracker();
    }
}
