using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Generic.Domain.Service;

namespace Generic.API.Controllers
{
    [RoutePrefix("api/state")]
    public class StateController : ApiController
    {
        public StateService StateService = new StateService();

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetState()
        {
            return Ok(new
            {
                State = this.StateService.GetAll()
            });
        }

        [Route("{StateId}/city/search/{q}")]
        [HttpGet]
        public IHttpActionResult GetCity(string q, int stateId)
        {
            if (String.IsNullOrWhiteSpace(q))
                return BadRequest("Texto de busca não pode ser vazio");

            return Ok(new
            {
                Cities = this.StateService.SearchCity(q, stateId)
            });

        }


    }
}
