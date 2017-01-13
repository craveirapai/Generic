using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Generic.Admin.Web.Models;
using Generic.Domain;
using Generic.Domain.Enums;
using WebMatrix.WebData;

namespace Generic.Admin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserViewModels, User>();
                cfg.CreateMap<User, UserViewModels>();
            });

        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(authCookie.Name, "Forms"), new[] { RoleEnum.Admin.ToString() });
            }
            
        }
    }
}
