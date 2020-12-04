using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using WebServicePedidos.DataAccess;
using WebServicePedidos.Models;

namespace WebServicePedidos.Controllers
{
    //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
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

        [HttpPut]
        [Route("Cadelga/ArticuloUDO")]
        public IHttpActionResult Put([FromBody] ArticuloUDO articulo)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new ArticuloUDORepository().Actualizar(articulo)));
            }
            catch (Exception ex)
            {
                mensaje = Request.CreateResponse(HttpStatusCode.InternalServerError);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(ex));
            }

            mensaje.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return ResponseMessage(mensaje);
        }

        [HttpDelete]
        [Route("Cadelga/ArticuloUDO")]
        public IHttpActionResult Delete([FromUri] string codigo)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new ArticuloUDORepository().Eliminar(codigo)));
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
