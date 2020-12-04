using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServicePedidos.Models
{
    public class DetalleArticuloUDO
    {
        public string Alamacen { get; set; }
        public int Existencia { get; set; }
        public bool Eliminar { get; set; }
        public bool Modificar { get; set; }
        public bool Agregar { get; set; }
    }
}