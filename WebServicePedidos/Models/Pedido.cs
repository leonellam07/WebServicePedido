using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServicePedidos.Models
{
    public class Pedido
    {
        public int NumDocumento { get; set; }
        public DateTime Fecha { get; set; }
        public string CodProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public double Total { get; set; }
        public List<DetallePedido> Detalles { get; set; }
    }
}