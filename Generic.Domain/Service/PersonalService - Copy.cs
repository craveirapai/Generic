using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tonnus.Domain.Enums;
using Tonnus.Domain.Repository;
using Tonnus.Infra.Utils;

namespace Tonnus.Domain.Service
{
    public class PersonalService
    {
        private PersonalRepository Repository { get; set; } = new PersonalRepository();

     
        public bool ExistsEmail(String email)
        {
            var userTemp = this.Repository.GetPersonalByEmail(email);

            if (userTemp == null)
                return false;

            return true;

        }

        public void Create(Personal personal, string base64Picture = null)
        {
            try
            {
                string hashedPassword = Infra.Utils.SecurityUtils.HashSHA1(personal.Password);
                personal.Password = hashedPassword;

                personal.RoleId = (int)RoleEnum.Personal;
                personal.PicturePath = Guid.NewGuid().ToString();
                personal.IsEnabled = true;

                this.Repository.Save(personal);

                if (!String.IsNullOrWhiteSpace(base64Picture))
                {
                    byte[] image = Convert.FromBase64String(base64Picture);
                    MemoryStream ms = new MemoryStream(image);
                    MemoryStream profileImg = Infra.Utils.ImageResizerUtils.ResizeImage(ms.ToArray(), 150, 150, "max");
                    Infra.Storage.AzureCloudStorage.SaveToStorage(Infra.Storage.AzureCloudImageDirectoryStorageEnum.PERSONAL, profileImg.ToArray(), Domain.Personal.PROFILE_PICTURE, personal.PicturePath);
                }

            }
            catch(System.Exception ex)
            {
                throw ex;
            }

        }

        public Personal GetPersonalByEmail(string email)
        {
            return this.Repository.GetPersonalByEmail(email);   
        }

        public void ResetPassword(Personal personal)
        {
            string clearPassword = Infra.Utils.SecurityUtils.MakePassword(6);
            personal.Password = Infra.Utils.SecurityUtils.HashSHA1(clearPassword);
        
            this.Repository.Update(personal);

            Infra.Utils.EmailUtils.SendEmail(personal.Email, String.Format(EmailUtils.ResetPasswordText, personal.Name, clearPassword), EmailUtils.ResetPasswordSubject);
            
        }
    }
}
