using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebServicePedidos.DataAccess;
using WebServicePedidos.Helpers;

namespace WebServicePedidos.Controllers
{
    [AllowAnonymous]
    public class AutenticacionController : ApiController
    {
        [HttpPost]
        [Route("Cadelga/Autenticacion")]
        public IHttpActionResult Post([FromBody] Login login)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new AutenticacionRepository().Autenticar(login)));
            }
            catch (Exception ex)
            {
                mensaje = Request.CreateResponse(HttpStatusCode.InternalServerError);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(ex));
            }

            mensaje.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return ResponseMessage(mensaje);
        }
    }
}
