using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Generic.API.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email é obrigatário")]
        [EmailAddress(ErrorMessage = "Email não está em um formato correto")]
        public string Email { get; set; }
    }
}