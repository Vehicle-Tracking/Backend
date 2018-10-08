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
    public class VehicleRepository : Repository<Vehicle, string>, IVehicleRepository
    {
        public VehicleRepository()
        {
        }

        public VehicleRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Vehicle> GetWithOwnerAsync(string vin)
        {
            var all = DbSet.AsQueryable().Any();
            return await this.DbSet.Include(t => t.Owner).FirstOrDefaultAsync(t => t.Id == vin);
        }
    }

    public interface IVehicleRepository : IRepository<Vehicle, string>
    {
        Task<Vehicle> GetWithOwnerAsync(string vin);
    }
}