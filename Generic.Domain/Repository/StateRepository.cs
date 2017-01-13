using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Domain.BaseContext;

namespace Generic.Domain.Repository
{
    public class StateRepository : BaseRepository<State>
    {
        public System.Data.Entity.DbSet<Generic.Domain.User> User { get; set; }

        public System.Data.Entity.DbSet<Generic.Domain.City> Cities { get; set; }

        public System.Data.Entity.DbSet<Generic.Domain.Role> Roles { get; set; }
    }
}
