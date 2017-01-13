using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Generic.Admin.Web.Models;
using Generic.Domain.Enums;
using Generic.Domain.Service;
using WebMatrix.WebData;

namespace Generic.Admin.Web.Controllers
{
    public class AccountController : Controller
    {

        private UserService UserService { get; set; } = new UserService();

        public ActionResult Login()
        {
            ViewBag.ReturnUrl = Request.QueryString["returnUrl"];
            return View("Login");
        }

        [HttpPost]
        public ActionResult SignIn(LoginViewModel model)
        {

            string hashedPassword = Infra.Utils.SecurityUtils.HashSHA1(model.Password);

            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            var user = UserService.GetByEmailAndPassword(model.Email, hashedPassword);

            if (user != null)
            {
                Session["UserName"] = model.Email;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(30), false, String.Empty, FormsAuthentication.FormsCookiePath);
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(faCookie);

                if (Request.Form["returnUrl"] != null)
                {
                    return Redirect(Request.Form["returnUrl"]);
                }

                return RedirectToAction("Index", "User");
            }

            ModelState.AddModelError("", "Username ou senha inválidos");
            return View("Login");

        }

        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); 
            return RedirectToAction("Login", "Account");
        }
    }
}
