using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiV2.Models;
using WebApiV2.Views.ConexionSapView;

namespace WebApiV2.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class InvoiceController : ApiController
    {

        // GET: RespFacturaReserva

        //public static Company myCompany = null;
        /*public List<ListaFacturasModel> Get()
        {
            List<ListaFacturasModel> lista_f_reserva = new List<ListaFacturasModel>();

            try
            {
                myCompany = new Company
                {
                    Server = "SBO-SAP-SYN",
                    DbServerType = BoDataServerTypes.dst_MSSQL2016,
                    CompanyDB = "PRUEBAS_SYNERGY17",
                    UserName = "manager",
                    Password = "syn123..",
                    language = BoSuppLangs.ln_Spanish_La,
                    UseTrusted = false
                };


                int error = myCompany.Connect();
                if (error == 0)
                {

                    DateTime dt = DateTime.Now;
                    string dateFormatted = dt.ToString("yyyMMdd");
                    Recordset recordset = (Recordset)myCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    //recordset.DoQuery("SELECT top 10 T0.DocEntry, T0.DocNum,T0.DocDate,T0.CardCode,T0.CardName,T0.DocStatus,T1.LineNum,T1.ItemCode,T1.Quantity, " +
                    //"T1.WhsCode, T1.OpenCreQty, T1.LineStatus, T1.BaseLine, T1.OpenRtnQty, T1.BaseEntry, T1.BaseType FROM OINV T0 INNER JOIN INV1 T1" +
                    //"ON T1.DocEntry = T0.DocEntry WHERE T1.DocDate >= '20220420' AND T1.WhsCode = 'ALM-3ER' AND T1.OpenCreQty > 0");
                    recordset.DoQuery("SELECT DISTINCT T0.DocEntry, T0.DocNum,T0.DocDate,T0.CardCode,T0.CardName, T1.BaseEntry FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry WHERE T1.WhsCode ='ALM-3ER' AND T1.OpenCreQty > 0 AND T0.DocDate >='" + dateFormatted + "'");
                    while (!recordset.EoF)
                    {

                        lista_f_reserva.Add(new Models.ListaFacturasModel()
                        {
                            DocEntry = recordset.Fields.Item("DocEntry").Value.ToString(), //id doc
                            DocNum = recordset.Fields.Item("DocNum").Value.ToString(), //cod doc
                            DocDate = recordset.Fields.Item("DocDate").Value.ToString(), //fecha contab
                            BaseEntry = recordset.Fields.Item("BaseEntry").Value.ToString(), //fecha contab
                            CardCode = recordset.Fields.Item("CardCode").Value.ToString(), //cod clien
                            CardName = recordset.Fields.Item("CardName").Value.ToString(), //nom clien
                            //OcrCode2 = recordset.Fields.Item("OcrCode2").Value.ToString(), //nom clien
                            //OcrCode = recordset.Fields.Item("CardName").Value.ToString(), //nom clien
                            //Paid_sum = recordset.Fields.Item("PaidSum").Value.ToString(), //nom clien
                            //Doc_status = recordset.Fields.Item("DocStatus").Value.ToString(), //estado del doc

                            //Line_num = recordset.Fields.Item("LineNum").Value.ToString(), // num linea
                            //Item_code = recordset.Fields.Item("ItemCode").Value.ToString(), //cod item
                            //Quantity = recordset.Fields.Item("Quantity").Value.ToString(), //cantidad total
                            //OpenCreQty = recordset.Fields.Item("OpenCreQty").Value.ToString(), //cantidad pendiente
                            //OpenRtnQty = recordset.Fields.Item("OpenRtnQty").Value.ToString(), //cantidad entregada
                            //Line_status = recordset.Fields.Item("LineStatus").Value.ToString(), //estado de la linea
                            //Base_line = recordset.Fields.Item("BaseLine").Value.ToString(),
                            //Base_type = recordset.Fields.Item("BaseType").Value.ToString(),
                            //Base_entry = recordset.Fields.Item("BaseEntry").Value.ToString(),
                        });
                        recordset.MoveNext();

                    }
                }
            }
            catch (Exception ex)
            {
                Javascript.Alert("Error - " + ex + myCompany.GetLastErrorDescription().ToString());

            }

            return lista_f_reserva;



        }*/
        public List<InvoiceModel> Get()
        {
            List<InvoiceModel> lista_f_reserva = new List<InvoiceModel>();
            var resp = new ConexionSap();
            Company oCompany = resp.ConnectSap();

            if (oCompany.Connect() == 0)
            {
                DateTime dt = DateTime.Now;
                string dateFormatted = dt.ToString("yyyMMdd");
                Recordset recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                //recordset.DoQuery("SELECT DISTINCT T0.DocEntry, T0.DocNum,T0.DocDate,T0.CardCode,T0.CardName, T1.WhsCode, T1.BaseEntry FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry WHERE T1.WhsCode ='ALM-3ER' AND T1.OpenCreQty > 0 AND T0.isIns = 'Y' AND T0.DocDate >='" + dateFormatted + "'");
                recordset.DoQuery("SELECT DISTINCT T0.DocEntry, T2.SlpName, T3.U_NAME, T0.CardName, T0.NumAtCard, T1.WhsCode, T0.DocDate " +
                    "FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry " +
                    "INNER JOIN OSLP T2 ON T0.SlpCode=T2.SlpCode " +
                    "INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID " +
                    "WHERE T1.WhsCode ='ALM-3ER' AND T1.OpenCreQty > 0 AND t0.isIns = 'Y' and T0.DocDate=' " + dateFormatted + "'");
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
                        WarehouseCode= recordset.Fields.Item("WhsCode").Value, //agencia
                        DocDate = recordset.Fields.Item("DocDate").Value.ToString(), //fecha contab
                        //BaseEntry = recordset.Fields.Item("BaseEntry").Value, //fecha contab
                        ItemList=null
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
        public List<InvoiceModel> Get(string doc_entry)
        {
            List<InvoiceModel> f_reserva = new List<InvoiceModel>();
            List<ItemModel> Items = new List<ItemModel>();

            var resp = new ConexionSap();
            Company oCompany = resp.ConnectSap();

            if (oCompany.Connect() == 0)
            {
                //DateTime dt = DateTime.Now;
                //string dateFormatted = dt.ToString("yyyMMdd");
                Recordset recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //recordset.DoQuery("SELECT T0.DocEntry, T0.DocNum,T0.DocDate, T0.DocDueDate, T0.CardCode,T0.CardName,T0.DocStatus,T1.LineNum,T1.ItemCode,T1.Quantity, T1.WhsCode,T1.OpenCreQty,T1.LineStatus, T1.Dscription, T1.BaseLine,T1.OpenRtnQty, T1.BaseEntry, T1.BaseType FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry WHERE T1.DocDate >='" + dateFormatted + "' AND T1.WhsCode = 'ALM-3ER' AND T1.OpenCreQty > 0 AND T1.DocEntry = '" + doc_entry + "'");
                //recordset.DoQuery("SELECT T0.DocEntry, T0.DocNum,T0.DocDate, T0.CardCode,T0.CardName,T1.LineNum,T1.ItemCode,T1.Quantity, T1.WhsCode,T1.OpenCreQty, T1.OpenInvQty, T1.LineStatus, T1.Dscription, T1.BaseLine,T1.DelivrdQty, T1.BaseEntry, T1.BaseType FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry WHERE T1.WhsCode = 'ALM-3ER' AND T1.OpenCreQty > 0 AND T1.DocEntry = '" + doc_entry + "'");
                recordset.DoQuery("SELECT T0.DocEntry, T0.CardName, T2.SlpName, T0.NumAtCard, T1.WhsCode, T0.DocDate, T3.U_NAME, T1.LineNum,T1.ItemCode,T1.Quantity, T1.OpenCreQty, T1.DelivrdQty, T1.Dscription FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry INNER JOIN OSLP T2 ON T0.SlpCode=T2.SlpCode INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID WHERE T1.WhsCode = 'ALM-3ER' AND T1.OpenCreQty > 0 AND T1.DocEntry = '" + doc_entry + "'");

                f_reserva.Add(new InvoiceModel()
                {
                    id = recordset.Fields.Item("DocEntry").Value, //id doc
                    DocEntry = recordset.Fields.Item("DocEntry").Value, //id doc
                    CardName = recordset.Fields.Item("CardName").Value,
                    SlpName = recordset.Fields.Item("SlpName").Value, //nombre vendedor
                    NumAtCard = recordset.Fields.Item("NumAtCard").Value, //nro de factura
                    //DocNum = recordset.Fields.Item("DocNum").Value, //cod doc
                    U_Name = recordset.Fields.Item("U_Name").Value, //nombre usuario sap
                    DocDate = recordset.Fields.Item("DocDate").Value.ToString(),
                    WarehouseCode = recordset.Fields.Item("WhsCode").Value,
                    //BaseEntry = recordset.Fields.Item("BaseEntry").Value,
                    //CardCode = recordset.Fields.Item("CardCode").Value,
                    ItemList = Items
                });

                while (!recordset.EoF)
                {
                    Items.Add(new ItemModel()
                    {
                        id = recordset.Fields.Item("LineNum").Value, //numero de linea
                        Dscription = recordset.Fields.Item("Dscription").Value.ToString(), //nombre del item
                        OpenCreQty = recordset.Fields.Item("OpenCreQty").Value, //cantidad pendiente
                        //OpenInvQty = recordset.Fields.Item("OpenInvQty").Value, //cantidad pendiente en inventario
                        DelivrdQty = recordset.Fields.Item("DelivrdQty").Value, //cantidad entregada
                        Quantity = recordset.Fields.Item("Quantity").Value, //cantidad 
                        LineNum = recordset.Fields.Item("LineNum").Value, //numero de linea
                        ItemCode = recordset.Fields.Item("ItemCode").Value,
                    });
                    recordset.MoveNext();
                }
                //oCompany.Disconnect();
                return f_reserva;
            }
            else
            {
                Javascript.Alert("Error - " + oCompany.GetLastErrorDescription().ToString());
                return null;
            }
        }
    }
}