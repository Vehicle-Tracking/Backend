using Avt.Web.Backend.Data.Base;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Spec;

namespace Avt.Web.Backend.Data.Repositories
{
    public class OwnerRepository : Repository<Owner, string>, IOwnerRepository
    {
        public OwnerRepository()
        {
        }

        public OwnerRepository(IDataContext dataContext) : base(dataContext)
        {
        }
    }

    public interface IOwnerRepository : IRepository<Owner, string>
    {
    }
}