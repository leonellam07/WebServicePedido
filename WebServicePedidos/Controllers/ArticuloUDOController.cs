using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebServicePedidos.DataAccess;
using WebServicePedidos.Models;

namespace WebServicePedidos.Controllers
{
    public class ArticuloUDOController : ApiController
    {
        [HttpPost]
        [Route("Cadelga/ArticuloUDO")]
        public IHttpActionResult Post([FromBody] ArticuloUDO articulo)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new ArticuloUDORepository().Agregar(articulo)));
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
