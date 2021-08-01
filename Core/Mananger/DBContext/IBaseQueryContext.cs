using Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.Mananger.DBContext
{
    public interface IBaseQueryContext
    {
        public DbSet<User> Users { get; set; }
    }
}
