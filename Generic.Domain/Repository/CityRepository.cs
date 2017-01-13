using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Domain.BaseContext;

namespace Generic.Domain.Repository
{
    public class CityRepository : BaseRepository<City>
    {
        public IEnumerable<City> SearchCity(string q, int StateId)
        {
            return (from x in this.DbSet
                    where x.Name.StartsWith(q)
                    && x.StateId == StateId
                    select x);
        }
    }
}
