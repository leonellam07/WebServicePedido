using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServicePedidos.Helpers;
using WebServicePedidos.Middlewares;

namespace WebServicePedidos.DataAccess
{
    public class AutenticacionRepository
    {
        public string Autenticar(Login login)
        {
            if(login == null) { throw new Exception("Faltan Credenciales"); }
            if (ApplicationContext.Db.InTransaction) { ApplicationContext.Db.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack); }
       

            if(ApplicationContext.Db.AuthenticateUser(login.Username, login.Password) != SAPbobsCOM.AuthenticateUserResultsEnum.aturUsernamePasswordMatch)
            {
                throw new Exception("Credenciales incorrectas");
            }

            return TokenGenerator.GenerateTokenJWT(login);
        }
    }
}