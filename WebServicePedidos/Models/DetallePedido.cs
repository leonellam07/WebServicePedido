using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServicePedidos.Models
{
    public class DetallePedido
    {
        public string CodArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }

    }
}