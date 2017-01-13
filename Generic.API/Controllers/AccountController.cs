using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Generic.API.ViewModel;
using Generic.Domain;
using Generic.Domain.Enums;
using Generic.Domain.Service;

namespace Generic.API.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private UserService UserService { get; set; } = new UserService();

        [Route("user/create")]
        [HttpPost]
        public IHttpActionResult CreateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (this.UserService.ExistsEmail(model.Email))
                return BadRequest("Email já cadastrado em nossa base de dados, por favor tente outro.");

            if (model.Name.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
                return BadRequest("Nome completo deve ter pelo menos 2 nomes");

            var User = Mapper.Map<UserViewModel, User>(model);

            this.UserService.Create(User);

            return Ok(new
            {
                User = Mapper.Map<User, UserViewModel>(User)
            });


        }

        [Route("forget")]
        [HttpPost]

        public IHttpActionResult ForgetPassword(ForgetPasswordViewModel forget)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = this.UserService.GetByEmail(forget.Email);

            if (user != null)
            {
                this.UserService.ResetPassword(user);
                return Ok();
            }

            return BadRequest("Não encontramos nenhum cadastro com este email");
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAccount()
        {
            var identity = (ClaimsIdentity)User.Identity;

            var id = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

            var role = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                
            if (role == RoleEnum.user.ToString())
            {
                var user = this.UserService.GetById(Convert.ToInt32(id));

                if (!user.IsEnabled)
                    return NotFound();

                var viewModel = Mapper.Map<User, UserViewModel>(user);
               //viewModel.PictureURL = user.GetProfilePicture();
                viewModel.Password = "";

                return Ok(new
                {
                    Result = viewModel,
                    AccountType = RoleEnum.user.ToString()
                });

            }
            else if (role == RoleEnum.Admin.ToString())
            {
               // var viewModel = Mapper.Map<Admin, adminViewModel>(user);

                //return Ok(new
                //{
                //    Result = viewModel,
                //    AccountType = RoleEnum.Admin.ToString()
                //});
            }

            return BadRequest("Erro ao processar a sua solicitação");
        }

      }
}
