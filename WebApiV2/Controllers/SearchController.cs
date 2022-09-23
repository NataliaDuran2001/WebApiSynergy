using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiV2.Models;
using WebApiV2.Views.ConexionSapView;

namespace WebApiV2.Controllers
{
    public class SearchController : ApiController
    {
        // GET: Client
        [Authorize]
        [EnableCors("*", "*", "*")]
        public List<InvoiceModel> Get(string nameClient, string docDate)
        {
            List<InvoiceModel> lista_f_reserva = new List<InvoiceModel>();

            var resp = new ConexionSap();
            Company oCompany = resp.ConnectSap();

            if (oCompany.Connect() == 0)
            {
                Recordset recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //nameClient es dado
                if ((nameClient != "" && nameClient != null) && (docDate == "" || docDate == null))
                {
                    recordset.DoQuery("SELECT DISTINCT T0.DocEntry, T0.DocNum, T2.SlpName, T3.U_NAME, T0.CardName, T0.NumAtCard, T1.WhsCode, T0.DocDate " +
                    "FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry " +
                    "INNER JOIN OSLP T2 ON T0.SlpCode = T2.SlpCode " +
                    "INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID " +
                    "WHERE T1.WhsCode ='ALM-3ER' " +
                    "AND T1.OpenCreQty > 0 " +
                    "AND T0.isIns = 'Y' and T0.CardName like '%" + nameClient + "%'");
                }
                //docDate es dado
                if ((docDate != "" && docDate != null) && (nameClient == "" || nameClient == null))
                {
                    recordset.DoQuery("SELECT DISTINCT T0.DocEntry, T0.DocNum, T2.SlpName, T3.U_NAME, T0.CardName, T0.NumAtCard, T1.WhsCode, T0.DocDate " +
                    "FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry " +
                    "INNER JOIN OSLP T2 ON T0.SlpCode = T2.SlpCode " +
                    "INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID " +
                    "WHERE T1.WhsCode = 'ALM-3ER'" +
                    "AND T1.OpenCreQty > 0 " +
                    "AND T0.isIns = 'Y' AND T0.DocDate >= '" + docDate + "'");
                }
                //ambos parametros son dados
                if ((nameClient != "" && nameClient != null) && (docDate != "" & docDate != null))
                {
                    recordset.DoQuery("SELECT DISTINCT T0.DocEntry, T0.DocNum, T2.SlpName, T3.U_NAME, T0.CardName, T0.NumAtCard, T1.WhsCode, T0.DocDate " +
                    "FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry " +
                    "INNER JOIN OSLP T2 ON T0.SlpCode = T2.SlpCode " +
                    "INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID " +
                    "WHERE T1.WhsCode = 'ALM-3ER' " +
                    "AND T1.OpenCreQty > 0 " +
                    "AND T0.isIns = 'Y' AND T0.CardName like '%" + nameClient + "%' AND T0.DocDate >= '" + docDate + "'");
                }

                while (!recordset.EoF)
                {

                    lista_f_reserva.Add(new Models.InvoiceModel()
                    {
                        id = recordset.Fields.Item("DocEntry").Value, //id doc
                        DocEntry = recordset.Fields.Item("DocEntry").Value, //id doc
                        SlpName = recordset.Fields.Item("SlpName").Value, //nombre vendedor
                        //DocNum = recordset.Fields.Item("DocNum").Value, //cod doc
                        U_Name = recordset.Fields.Item("U_Name").Value, //nombre usuario sap
                        //CardCode = recordset.Fields.Item("CardCode").Value, //cod clien
                        CardName = recordset.Fields.Item("CardName").Value, //nom clien
                        NumAtCard = recordset.Fields.Item("NumAtCard").Value, //nro de factura
                        WarehouseCode = recordset.Fields.Item("WhsCode").Value, //agencia
                        DocDate = recordset.Fields.Item("DocDate").Value.ToString(), //fecha contab
                        //BaseEntry = recordset.Fields.Item("BaseEntry").Value, //fecha contab
                        ItemList = null
                    });
                    recordset.MoveNext();
                }
                //disconnect
                //oCompany.Disconnect();
                return lista_f_reserva;
            }
            else
            {
                Javascript.Alert("Error - " + oCompany.GetLastErrorDescription().ToString());
                return null;
            }
        }
    }
}