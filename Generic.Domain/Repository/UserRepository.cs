using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Domain.BaseContext;

namespace Generic.Domain.Repository
{
    public class UserRepository :  BaseRepository<User>
    {
        public User GetByEmail(string email)
        {
            return this.FindOne(x => x.Email == email && x.IsEnabled == true );
        }

        public User GetByIdAndPassword(int Id, string password)
        {
            return this.FindOne(x => x.Password == password && x.Id == Id);
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            return this.FindOne(x => x.Email == email && x.Password == password);
        }

        public void Delete( string email)
        {

            var sqlQuery = String.Format("Delete FROM [User] where Email = '{0}';", email);

            this.Database.ExecuteSqlCommand(sqlQuery);
        }


    }
}
