using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Generic.Domain.Enums;

namespace Generic.API.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é Obrigatório")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Email não pode ser vazio")]
        [EmailAddress(ErrorMessage = "Email não está em um formato correto")]
        public String Email { get; set; }


        [Required(ErrorMessage = "Senha não pode ser vazio")]
        [MinLength(6, ErrorMessage = "A senha não pode ter menos do que 6 caracteres.")]
        public String Password { get; set; }

        public String City { get; set; }

        public String State { get; set; }

    }
}