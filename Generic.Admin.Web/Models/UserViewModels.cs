using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Generic.Domain;
using Generic.Domain.Enums;

namespace Generic.Admin.Web.Models
{
    public class UserViewModels
    {
        private int profileId;

        public int Id { get; set; }
        public String Name { get; set; }
      
        public String Phone { get; set; }
        public String Email { get; set; }
        public virtual int ProfileId
        {
            get;
            set;
        }


        public override string ToString()
        {
            return String.Format("{0};{1};{2}\n", Name, Email, Phone);
        }

        public static User ToDomain(UserViewModels model)
        {
            var user = Mapper.Map<UserViewModels, User>(model);

            return user;
        }

        public static UserViewModels FromDomain(User user)
        {
            var result = Mapper.Map<User, UserViewModels>(user);

            return result;

        }


        public static List<UserViewModels> FromDomain(List<User> user)
        {
            List<UserViewModels> result = new List<UserViewModels>();

            foreach (User item in user)
            {
                result.Add(Mapper.Map<User, UserViewModels>(item));
            }      

            return result;

        }

    }
}