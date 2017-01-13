using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.Domain.Enums;
using Generic.Domain.Repository;
using Generic.Infra.Utils;

namespace Generic.Domain.Service
{
    public class UserService
    {
        private UserRepository Repository { get; set; } = new UserRepository();
    

        public bool ExistsEmail(String email)
        {
            var userTemp = this.Repository.GetByEmail(email);

            if (userTemp == null)
                return false;

            return true;

        }

        public void Create(User user, string base64Picture = null)
        {
            try
            {
                string hashedPassword = Infra.Utils.SecurityUtils.HashSHA1(user.Password);
                user.Password = hashedPassword;

                user.RoleId = (int)RoleEnum.user;
                user.IsEnabled = true;

                user.DtRegister = DateTime.Now;

                //if (!String.IsNullOrWhiteSpace(base64Picture))
                //{
                //    user.PicturePath = Guid.NewGuid().ToString();

                //    byte[] image = Convert.FromBase64String(base64Picture);
                //    MemoryStream ms = new MemoryStream(image);
                //    MemoryStream profileImg = Infra.Utils.ImageResizerUtils.ResizeImage(ms.ToArray(), 500, 500, "max");
                //    Infra.Storage.AzureCloudStorage.SaveToStorage(Infra.Storage.AzureCloudImageDirectoryStorageEnum.user, profileImg.ToArray(), Domain.User.PROFILE_PICTURE, user.PicturePath);
                //}

                this.Repository.Save(user);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        public User GetByEmailAndPassword(string email, string password)
        {
            var user = this.Repository.GetByEmailAndPassword(email, password);

            if (user == null)
                return null;

            if (!user.IsEnabled)
                return null;

            return user;

        }

        public User GetByIdAndPassword(int Id, string password)
        {
            string hashedPassword = Infra.Utils.SecurityUtils.HashSHA1(password);

            var user = this.Repository.GetByIdAndPassword(Id, hashedPassword);

            if (user == null)
                return null;

            return user;

        }

        public void Delete(User User)
        {
            this.Repository.Delete(User.Email);
        }

       

        public void ChangePassword(int id, string newPassword)
        {
            var user = this.Repository.GetById(id);
            string hashedPassword = Infra.Utils.SecurityUtils.HashSHA1(newPassword);

            user.Password = hashedPassword;

            this.Repository.Update(user);

        }

        public void UpdatePicture(User User, string base64Picture)
        {
            if (!String.IsNullOrWhiteSpace(base64Picture))
            {
                //User.PicturePath = Guid.NewGuid().ToString();
                //this.Repository.Update(User);

                //byte[] image = Convert.FromBase64String(base64Picture);
                //MemoryStream ms = new MemoryStream(image);
                //MemoryStream profileImg = Infra.Utils.ImageResizerUtils.ResizeImage(ms.ToArray(), 500, 500, "max");
                //Infra.Storage.AzureCloudStorage.SaveToStorage(Infra.Storage.AzureCloudImageDirectoryStorageEnum.user, profileImg.ToArray(), Domain.User.PROFILE_PICTURE, User.PicturePath);
            }
        }

        public void Update(User User)
        {
            this.Repository.Update(User);
        }

        public User GetById(int id)
        {
            return this.Repository.GetById(id);
        }

        public User GetByEmail(string email)
        {
            return this.Repository.GetByEmail(email);
        }

        public IEnumerable<User> GetAll()
        {
            return this.Repository.GetAll();
        }

        public void ResetPassword(User User)
        {
            string clearPassword = Infra.Utils.SecurityUtils.MakePassword(6);
            User.Password = Infra.Utils.SecurityUtils.HashSHA1(clearPassword);

            this.Repository.Update(User);

            Infra.Utils.EmailUtils.SendEmail(User.Email, String.Format(EmailUtils.ResetPasswordText, User.Name, clearPassword), EmailUtils.ResetPasswordSubject);
        }

        

        

        
    }
}
