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
            foreach (DetalleArticuloUDO detalle in articuloUDO.Detalles)
            {
                GeneralData nuevoDetalle = detalles.Add();
                nuevoDetalle.SetProperty("U_Almacen", detalle.Alamacen);
                nuevoDetalle.SetProperty("U_Existencia", detalle.Existencia);
            }


            GeneralDataParams parametros = udo.Add(data);
            ApplicationContext.Db.EndTransaction(BoWfTransOpt.wf_Commit);

            return articuloUDO;
        }

        public ArticuloUDO Actualizar(ArticuloUDO articuloUDO)
        {
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }
            GeneralService udo = ApplicationContext.Db.GetCompanyService().GetGeneralService("PAGO_DOC");
            GeneralDataParams parametros = udo.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
            parametros.SetProperty("Code", articuloUDO.Codigo);
            GeneralData data = udo.GetByParams(parametros);


            if (!string.IsNullOrEmpty(articuloUDO.Nombre)) { data.SetProperty("Name", articuloUDO.Nombre); }
            if (!string.IsNullOrEmpty(articuloUDO.Proveedor)) { data.SetProperty("U_Proveedor", articuloUDO.Proveedor); }
            if (articuloUDO.Precio > 0) { data.SetProperty("U_Precio", articuloUDO.Precio); }

            GeneralDataCollection detalles = data.Child("PAGO_ART_DET");
            int row = 0;
            foreach (DetalleArticuloUDO detalle in articuloUDO.Detalles)
            {
                if (detalle.Eliminar)
                {
                    detalles.Remove(row);
                }

                if (detalle.Modificar)
                {
                    GeneralData modDetalle = detalles.Item(row);
                    modDetalle.SetProperty("U_Almacen", detalle.Alamacen);
                    modDetalle.SetProperty("U_Existencia", detalle.Existencia);
                }

                if (detalle.Agregar)
                {
                    GeneralData modDetalle = detalles.Add();
                    modDetalle.SetProperty("U_Almacen", detalle.Alamacen);
                    modDetalle.SetProperty("U_Existencia", detalle.Existencia);
                }
                row++;
            }

            udo.Update(data);

            return articuloUDO;
        }
    
        public string Eliminar(string Codigo)
        {
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }
            GeneralService udo = ApplicationContext.Db.GetCompanyService().GetGeneralService("PAGO_DOC");
            GeneralDataParams parametros = udo.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
            parametros.SetProperty("Code", Codigo);

            udo.Delete(parametros);
            return "Eliminado";
        }
    }
}