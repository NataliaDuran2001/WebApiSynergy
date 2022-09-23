using SAPbobsCOM;
using System;
using WebApiV2.Views.ConexionSapView;

namespace WebApiV2
{
    public class ConexionSap
    {
        //public Company myCompany = null;

        public Company ConnectSap()
        {
            try
            {
                Company myCompany = new Company
                {
                    Server = "SBO-SAP-SYN",
                    CompanyDB = "PRUEBAS_SYNERGY17",
                    UserName = "manager",
                    Password = "syn123..",
                    DbServerType = BoDataServerTypes.dst_MSSQL2016,
                    language = BoSuppLangs.ln_Spanish_La

                };
                //int con = myCompany.Connect();
                //con = 1 conectado
                //con <> 1 no conectado
                /*if (myCompany.Connect() != 1)
                {
                    Javascript.Alert("La conexión con SAP no funciona");
                }*/
                return myCompany;

            }
            catch (Exception ex)
            {
                return (Company)ex;
            }



        }
        /*public void Disconnection()
        {
            if (myCompany.Connected == true)
            {
                myCompany.Disconnect();
            }
        }*/

    }
}

/*public static Company myCompany = null;

public static bool Open()
{
    bool Respuesta = false;
    try
    {
        myCompany = new Company();
        myCompany.Server = "SBO-SAP-SYN";
        myCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016;
        myCompany.CompanyDB = "PRUEBAS_SYNERGY17";
        myCompany.UserName = "manager";
        myCompany.Password = "syn123..";
        myCompany.language = BoSuppLangs.ln_Spanish_La;
        int error = myCompany.Connect();

        if (error == 0)
        {
            Respuesta = true;
        }
        else
        {
            Javascript.Alert("Error - " + myCompany.GetLastErrorDescription().ToString());
        }
    }
    catch (Exception ex)
    {
        Javascript.Alert("Error - " + ex);
    }
    return Respuesta;
}*/