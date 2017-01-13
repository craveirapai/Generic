using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Domain.Enums;
using Generic.Domain.BaseContext;

namespace Generic.Domain.Repository
{
    public class RoleRepository : BaseRepository<Role>
    {

        public Role GetProfileByName(RoleEnum pName)
        {
            return (from x in this.DbSet
                    where x.Name == pName.ToString()
                    select x).FirstOrDefault();
        }
      
    }
}
