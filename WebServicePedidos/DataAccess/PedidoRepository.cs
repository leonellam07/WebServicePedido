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
                                                "T0.\"DocEntry\" = T1.\"DocEntry\" where T0.\"DocEntry\" = '{0}'", numDocumento));

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

        public Pedido AgregarPedido(Pedido pedido)
        {
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }

            Documents nuevoPedido = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            nuevoPedido.CardCode = pedido.CodProveedor;
            nuevoPedido.DocDueDate = DateTime.Now;

            foreach (DetallePedido detalle in pedido.Detalles)
            {
                nuevoPedido.Lines.ItemCode = detalle.CodArticulo;
                nuevoPedido.Lines.Quantity = detalle.Cantidad;
                nuevoPedido.Lines.Add();
            }

            int resultado = nuevoPedido.Add();
            if (resultado != 0) { throw new Exception(ApplicationContext.SBOError); }
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(BoWfTransOpt.wf_Commit); }

            string llave = string.Empty;
            ApplicationContext.Db.GetNewObjectCode(out llave);

            return BuscarPorId(Convert.ToInt32(llave));
        }

        public Pedido ActualizarPedido(Pedido pedido)
        {
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }

            Documents actualizarPedido = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            actualizarPedido.GetByKey(pedido.NumDocumento);

            if (!string.IsNullOrEmpty(pedido.CodProveedor)) { actualizarPedido.CardCode = pedido.CodProveedor; }
            int row = 0;
            foreach(DetallePedido detalle in pedido.Detalles)
            {
                actualizarPedido.Lines.SetCurrentLine(row);
                if (!string.IsNullOrEmpty(detalle.CodArticulo)) { actualizarPedido.Lines.ItemCode = detalle.CodArticulo; }
                if (detalle.Cantidad > 0) { actualizarPedido.Lines.Quantity = detalle.Cantidad; }
                row++;
            }

            int resultado = actualizarPedido.Update();
            if (resultado != 0) { throw new Exception(ApplicationContext.SBOError); }
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(BoWfTransOpt.wf_Commit); }
            
            return BuscarPorId(pedido.NumDocumento);
        }
    
        public Pedido CancelarPedido(int numDocumento) {
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }

            Documents actualizarPedido = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            actualizarPedido.GetByKey(numDocumento);
           
            int resultado = actualizarPedido.Cancel();
            if (resultado != 0) { throw new Exception(ApplicationContext.SBOError); }
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(BoWfTransOpt.wf_Commit); }

            return BuscarPorId(numDocumento);
        }
    }
}