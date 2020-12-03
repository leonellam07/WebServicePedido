using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServicePedidos.Models;

namespace WebServicePedidos.DataAccess
{
    public class ArticuloUDORepository
    {
        public ArticuloUDO Agregar(ArticuloUDO articuloUDO)
        {
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }

            GeneralService udo = ApplicationContext.Db.GetCompanyService().GetGeneralService("PAGO_DOC");
            GeneralData data = udo.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);


            data.SetProperty("Code", articuloUDO.Codigo);
            data.SetProperty("Name", articuloUDO.Nombre);
            data.SetProperty("U_Proveedor", articuloUDO.Proveedor);
            data.SetProperty("U_Precio", articuloUDO.Precio);

            GeneralDataCollection detalles = data.Child("PAGO_ART_DET");
            foreach(DetalleArticuloUDO detalle in articuloUDO.Detalles)
            {
                GeneralData nuevoDetalle = detalles.Add();
                nuevoDetalle.SetProperty("U_Almacen", detalle.Alamacen);
                nuevoDetalle.SetProperty("U_Existencia", detalle.Existencia);
            }


            GeneralDataParams parametros = udo.Add(data);
            ApplicationContext.Db.EndTransaction(BoWfTransOpt.wf_Commit);

            return articuloUDO;
        }
    }
}