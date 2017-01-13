using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generic.Admin.Web.Models;
using Generic.Domain;
using Generic.Domain.Repository;
using Generic.Domain.Service;

namespace Generic.Admin.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserService UserService { get; set; } = new UserService();

        public ActionResult Index()
        {
            IEnumerable<User> result = UserService.GetAll().OrderBy(u => u.Name).ToList();

            IList<UserViewModels> Lista = UserViewModels.FromDomain(result.ToList<User>());

            return View(Lista);
        }



        public ActionResult ExportToExcel()
        {

            IEnumerable<User> result = UserService.GetAll();

            IList<UserViewModels> Lista = UserViewModels.FromDomain(result.ToList<User>());


            GridView gv = new GridView();
            gv.DataSource = Lista;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=usuario.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View(Lista);
        }


        [HttpDelete]
        public ActionResult Delete(string email)
        {
            User result = UserService.GetByEmail(email);

            if (result != null)
            {
                UserService.Delete(result);
            }
            return Json("OK");
        }
        

    }
}
