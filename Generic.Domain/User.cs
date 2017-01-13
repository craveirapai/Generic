using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Domain
{
    public class User
    {

      

        public virtual int Id { get; set; }

        public string PicturePath { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public virtual String Name { get; set; }

        public virtual String Email { get; set; }

        public virtual String Password { get; set; }

        public DateTime? DtRegister { get; set; }

      
        public bool IsEnabled { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        //public String GetProfilePicture()
        //{
        //    if (String.IsNullOrEmpty(this.PicturePath))
        //        return String.Empty;

        //    return String.Format("{0}/User/{1}/{2}", Infra.Storage.AzureCloudStorage.PathImage, this.PicturePath, Domain.User.PROFILE_PICTURE);
        //}

    }
}
