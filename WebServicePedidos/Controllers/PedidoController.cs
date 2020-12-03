using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebServicePedidos.DataAccess;
using WebServicePedidos.Models;

namespace WebServicePedidos.Controllers
{
    public class PedidoController : ApiController
    {
        [HttpGet]
        [Route("Cadelga/Pedidos")]
        public IHttpActionResult Get([FromUri] int NumDocumento)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new PedidoRepository().BuscarPorId(NumDocumento)));
            }
            catch (Exception ex)
            {
                mensaje = Request.CreateResponse(HttpStatusCode.InternalServerError);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(ex));
            }

            mensaje.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return ResponseMessage(mensaje);
        }

        [HttpPost]
        [Route("Cadelga/Pedidos")]
        public IHttpActionResult Post([FromBody] Pedido pedido)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new PedidoRepository().AgregarPedido(pedido)));
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
        [Route("Cadelga/Pedidos")]
        public IHttpActionResult Put([FromBody] Pedido pedido)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new PedidoRepository().ActualizarPedido(pedido)));
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
        [Route("Cadelga/Pedidos")]
        public IHttpActionResult Delete([FromUri] int NumDocumento)
        {
            HttpResponseMessage mensaje = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                mensaje = Request.CreateResponse(HttpStatusCode.OK);
                mensaje.Content = new StringContent(JsonConvert.SerializeObject(new PedidoRepository().CancelarPedido(NumDocumento)));
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
