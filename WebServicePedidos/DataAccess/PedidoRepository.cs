using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServicePedidos.Models;

namespace WebServicePedidos.DataAccess
{
    public class PedidoRepository
    {
        public Pedido BuscarPorId(int numDocumento)
        {
            Pedido nuevoPedido = new Pedido();
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }
 

            Recordset registros = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.BoRecordset);
            registros.DoQuery(string.Format("select * from OPOR T0  INNER JOIN POR1 T1 ON " +
                                                "T0.\"DocEntry\" = T1.\"DocEntry\" where T0.\"DocNum\" = '{0}'", numDocumento));

            if (registros.RecordCount == 0) { throw new Exception("No se encontraron registros"); }
           
            nuevoPedido.NumDocumento = registros.Fields.Item("DocEntry").Value;
            nuevoPedido.Fecha = registros.Fields.Item("DocDate").Value;
            nuevoPedido.CodProveedor = registros.Fields.Item("CardCode").Value;
            nuevoPedido.NombreProveedor = registros.Fields.Item("CardName").Value;
            nuevoPedido.Total = registros.Fields.Item("DocTotal").Value;
            nuevoPedido.Detalles = new List<DetallePedido>(); // Inicializar la lista


            while (!registros.EoF)  //Verificar que halla llegado hasta el ultimo registro.
            {
                DetallePedido detalle = new DetallePedido();
                detalle.CodArticulo = registros.Fields.Item("ItemCode").Value;
                detalle.NombreArticulo = registros.Fields.Item("Dscription").Value;
                detalle.Cantidad = registros.Fields.Item("Quantity").Value;
                detalle.Precio = registros.Fields.Item("Price").Value;

                registros.MoveNext();                   //Moverse al Siguiente registros
                nuevoPedido.Detalles.Add(detalle);      //Agregar a la lista
            }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(registros);
            GC.Collect();

            return nuevoPedido;
        }
    }
}