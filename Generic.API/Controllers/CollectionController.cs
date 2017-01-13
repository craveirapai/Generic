using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Generic.Domain.Enums;

namespace Generic.API.Controllers
{
    [RoutePrefix("api/collection")]
    public class CollectionController : ApiController
    {
        [Route("Exemplo")]
        [HttpGet]
        public IHttpActionResult GetCollection()
        {
            return Ok(new
            {
                Type = new[]
                {
                    new { Name = "1", Value = CollectionnEnum.Um},
                    new { Name = "2", Value = CollectionnEnum.dois},
                }

                
            });
        }
    }
}
