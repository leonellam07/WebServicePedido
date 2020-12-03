using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServicePedidos.Models
{
    public class ArticuloUDO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Proveedor { get; set; }
        public double Precio { get; set; }
        public List<DetalleArticuloUDO> Detalles { get; set; }

    }
}