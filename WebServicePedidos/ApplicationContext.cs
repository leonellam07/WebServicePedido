
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServicePedidos
{
    public class ApplicationContext
    {
        private static Company SBOCompany = null;
        private static readonly object padlock = new object();

        private static string DistributationSQL { get; set; }
        private static string Server { get; set; }
        private static string CompanyDB { get; set; }
        private static string UserName { get; set; }
        private static string Password { get; set; }
        private static string LicenseServer { get; set; }

        private ApplicationContext()
        {

        }

        public static void Open()
        {
            if (SBOCompany != null) { if (SBOCompany.Connected) return; }

            DistributationSQL = System.Configuration.ConfigurationManager.AppSettings["DistributationSQL"];
            Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            CompanyDB = System.Configuration.ConfigurationManager.AppSettings["Database"];
            UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            LicenseServer = System.Configuration.ConfigurationManager.AppSettings["LicenseServer"];

            if (string.IsNullOrEmpty(DistributationSQL)) { throw new Exception("Se necesita la distribucion de la base de datos MSSQL|HANA"); }
            if (string.IsNullOrEmpty(Server)) { throw new Exception("Se necesitael host de la base de datos MSSQL|HANA"); }
            if (string.IsNullOrEmpty(CompanyDB)) { throw new Exception("Se necesita la base de datos MSSQL|HANA"); }
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password)) { throw new Exception("Se necesita usuario y/o contrase;a"); }

            SBOCompany = new Company();
            SBOCompany.LicenseServer = (!string.IsNullOrEmpty(LicenseServer)) ? LicenseServer : null;
            SBOCompany.CompanyDB = CompanyDB;
            SBOCompany.Server = Server;
            SBOCompany.UserName = UserName;
            SBOCompany.Password = Password;

            switch (DistributationSQL)
            {
                case "HANA": SBOCompany.DbServerType = BoDataServerTypes.dst_HANADB; break;
                case "MSSQL2016": SBOCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016; break;
            }

            if (SBOCompany.Connect() != 0) { throw new Exception(SBOError); }

        }

        public static string SBOError { get { return string.Format("Error {0}: {1}", SBOCompany.GetLastErrorCode(), SBOCompany.GetLastErrorDescription()); } }

        public static Company Db
        {
            get
            {
                lock (padlock)
                {
                    Open();
                    return SBOCompany;
                }
            }
        }

        public static bool DisconnectService()
        {
            if (SBOCompany != null)
            {
                if (SBOCompany.Connected)
                {
                    SBOCompany.Disconnect();
                }
                else { return false; }
                SBOCompany = null;
            }
            return true;
        }

    }
}