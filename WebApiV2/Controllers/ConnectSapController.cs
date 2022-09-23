using SAPbobsCOM;
using System;
using System.Web.Http;
using WebApiV2.Views.ConexionSapView;

namespace WebApiV2.Controllers
{
    public class ConnectSapController : ApiController
    {
        // GET: ConexionSap

        //public static Company myCompany = null;
        public Company myCompany;

        public string GetConexion()
        {
            bool Connection = false;
            try
            {

                myCompany = new Company
                {
                    Server = "SBO-SAP-SYN",
                    CompanyDB = "PRUEBAS_SYNERGY17",
                    UserName = "manager",
                    Password = "syn123..",
                    DbServerType = BoDataServerTypes.dst_MSSQL2016,
                    language = BoSuppLangs.ln_Spanish_La
                };


                int error = myCompany.Connect();
                if (error == 0)
                {
                    Connection = true;
                }
                else
                {
                    Javascript.Alert("Error - " + myCompany.GetLastErrorDescription().ToString());
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "Response:  " + Connection.ToString() + myCompany.GetLastErrorDescription().ToString();
        }
    }
}