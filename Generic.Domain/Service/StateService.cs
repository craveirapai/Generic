using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Domain.Repository;

namespace Generic.Domain.Service
{
    public class StateService
    {
        StateRepository Repository { get; set; } = new StateRepository();
        CityRepository CityRepository { get; set; } = new CityRepository();
    
        public IEnumerable<State> GetAll()
        {
            return this.Repository.GetAll();
        }
        
        public IEnumerable<City> SearchCity(string q, int StateId)
        {
            return this.CityRepository.SearchCity(q, StateId);
        }
    }
}
