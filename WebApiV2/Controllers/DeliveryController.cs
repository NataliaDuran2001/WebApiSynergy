using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApiV2.Models;
using WebApiV2.Views.ConexionSapView;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Web.Http.Cors;

namespace WebApiV2.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class DeliveryController : ApiController
    {
        // GET: Entrega
        //public Company myCompany = null;

        //public string Post(int base_doc_docEntry, int lineNum, double cant_entrega)
        
        public List<DeliveryModel> Get()
        {
            List<DeliveryModel> lista_deliveries = new List<DeliveryModel>();
            var resp = new ConexionSap();
            Company oCompany = resp.ConnectSap();

            if (oCompany.Connect() == 0)
            {
                DateTime dt = DateTime.Now;
                string dateFormatted = dt.ToString("yyyMMdd");
                Recordset recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //recordset.DoQuery("select DISTINCT T0.DocNum, T0.DocEntry,T0.CardCode,T0.CardName,T0.DocDate, T1.WhsCode, T0.Comments FROM ODLN T0 INNER JOIN DLN1 T1 ON T1.DocEntry = T0.DocEntry WHERE T1.WhsCode = 'ALM-3ER' AND T0.DocDate = '" + dateFormatted + "'");
                recordset.DoQuery("select DISTINCT T0.DocEntry, T0.CardName, T2.SlpName, T3.U_NAME, T0.NumAtCard, T0.Comments, " +
                    "T1.WhsCode FROM ODLN T0  " +
                    "INNER JOIN DLN1 T1 ON T1.DocEntry = T0.DocEntry " +
                    "INNER JOIN OSLP T2 ON T0.SlpCode=T2.SlpCode " +
                    "INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID WHERE T1.WhsCode ='ALM-3ER' AND T0.DocDate='" + dateFormatted + "'");
                while (!recordset.EoF)
                {   

                    lista_deliveries.Add(new Models.DeliveryModel()
                    {
                        id = recordset.Fields.Item("DocEntry").Value, //id doc
                        //DocNum = recordset.Fields.Item("DocNum").Value, //id doc
                        DocEntry = recordset.Fields.Item("DocEntry").Value, //id doc
                        //CardCode = recordset.Fields.Item("CardCode").Value, //cod clien
                        CardName = recordset.Fields.Item("CardName").Value, //nom clien
                        SlpName = recordset.Fields.Item("SlpName").Value, //vendedor
                        NumAtCard = recordset.Fields.Item("NumAtCard").Value, //nro de factura
                        U_Name = recordset.Fields.Item("U_NAME").Value, //usuario sap
                        //DocDate = recordset.Fields.Item("DocDate").Value.ToString(), //fecha contab
                        WarehouseCode = recordset.Fields.Item("WhsCode").Value, //fecha contab
                        Comments= recordset.Fields.Item("Comments").Value, //fecha contab

                    });
                    recordset.MoveNext();
                }
                //disconnect
                return lista_deliveries;
            }
            else
            {
                Javascript.Alert("Error - " + oCompany.GetLastErrorDescription().ToString());
                return null;
            }
        }

        public List<DeliveryModel> Get(string doc_entry)
        {
            List<DeliveryModel> f_reserva = new List<DeliveryModel>();
            List<ItemModel> Items = new List<ItemModel>();

            var resp = new ConexionSap();
            Company oCompany = resp.ConnectSap();

            if (oCompany.Connect() == 0)
            {
                Recordset recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //recordset.DoQuery("SELECT T0.DocEntry, T0.DocNum,T0.DocDate, T0.DocDueDate, T0.CardCode,T0.CardName,T0.DocStatus,T1.LineNum,T1.ItemCode,T1.Quantity, T1.WhsCode,T1.OpenCreQty,T1.LineStatus, T1.Dscription, T1.BaseLine,T1.OpenRtnQty, T1.BaseEntry, T1.BaseType FROM OINV T0 INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry WHERE T1.DocDate >='" + dateFormatted + "' AND T1.WhsCode = 'ALM-3ER' AND T1.OpenCreQty > 0 AND T1.DocEntry = '" + doc_entry + "'");
                //recordset.DoQuery("select T0.DocNum, T0.DocEntry, T0.DocDate, T0.Comments, T1.WhsCode, T1.LineNum, t1.ItemCode, t1.Dscription, t1.Quantity, T0.CardCode,T0.CardName FROM ODLN T0 INNER JOIN DLN1 T1 ON T1.DocEntry = T0.DocEntry WHERE t0.DocEntry = '" + doc_entry + "'");
                //recordset.DoQuery("select T0.DocNum, T0.DocEntry, T0.DocDate, T0.Comments, T1.WhsCode, T1.LineNum, t1.ItemCode, t1.Dscription, t1.Quantity, T0.CardCode,T0.CardName FROM ODLN T0 INNER JOIN DLN1 T1 ON T1.DocEntry = T0.DocEntry WHERE t0.DocEntry = '" + doc_entry + "'");
                recordset.DoQuery("select T0.DocEntry, T1.WhsCode,T2.SlpName, T3.U_NAME, T0.NumAtCard, T0.Comments, T0.CardName, T1.ItemCode, T1.LineNum,  T1.Dscription, T1.Quantity FROM ODLN T0 INNER JOIN DLN1 T1 ON T1.DocEntry = T0.DocEntry INNER JOIN OSLP T2 ON T0.SlpCode=T2.SlpCode INNER JOIN OUSR T3 ON T0.UserSign = T3.USERID WHERE T0.DocEntry= '" + doc_entry + "'");
                f_reserva.Add(new DeliveryModel()
                {
                    id = recordset.Fields.Item("DocEntry").Value, //id doc
                    DocEntry = recordset.Fields.Item("DocEntry").Value, //id doc
                    WarehouseCode = recordset.Fields.Item("WhsCode").Value,
                    SlpName = recordset.Fields.Item("SlpName").Value, //vendedor
                    U_Name = recordset.Fields.Item("U_NAME").Value, //usuario sap
                    NumAtCard = recordset.Fields.Item("NumAtCard").Value, //nro de factura
                    Comments = recordset.Fields.Item("Comments").Value,
                    //DocNum = recordset.Fields.Item("DocNum").Value, //id doc
                    //CardCode = recordset.Fields.Item("CardCode").Value,
                    CardName = recordset.Fields.Item("CardName").Value,
                    //DocDate = recordset.Fields.Item("DocDate").Value.ToString(),

                    ItemList = Items

                });

                while (!recordset.EoF)
                {

                    Items.Add(new ItemModel()
                    {
                        ItemCode = recordset.Fields.Item("ItemCode").Value,
                        LineNum = recordset.Fields.Item("LineNum").Value, //numero de linea
                        Dscription = recordset.Fields.Item("Dscription").Value, //nombre del item
                        id = recordset.Fields.Item("LineNum").Value, //numero de linea
                        Quantity = recordset.Fields.Item("Quantity").Value, //cantidad 
                        //OpenCreQty = recordset.Fields.Item("OpenCreQty").Value, //cantidad pendiente
                        //OpenInvQty = recordset.Fields.Item("OpenInvQty").Value, //cantidad pendiente en inventario
                        //DelivrdQty = recordset.Fields.Item("DelivrdQty").Value, //cantidad entregada
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

        public List<ItemModel> Items { get; set; }

        [ResponseType(typeof(ItemModel))]
        public IHttpActionResult PostEntrega(DeliveryModel entregaList)
        { //conexion

            var resp = new ConexionSap();
            Company oCompany = resp.ConnectSap();
            IHttpActionResult response = null;

            if (oCompany.Connect() == 0)
            {
                Documents base_doc = oCompany.GetBusinessObject(BoObjectTypes.oInvoices); //factura de reserva
                Documents new_doc = oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes); //nota de entrega

                int base_doc_entry = entregaList.DocEntry;

                if (base_doc.GetByKey(base_doc_entry) == true)
                {
                    //copia nombre del cliente, fecha 
                    new_doc.CardCode = base_doc.CardCode;
                    new_doc.DocDate = DateTime.Now;
                    new_doc.DocDueDate = DateTime.Now;

                    //si no hay items, se entrega todo
                    /*if (entregaList.ItemList == null)
                    {
                        //int total_doc_Line_itemList = 0;
                        int total_doc_Line = base_doc.Lines.Count;


                        int x;
                        for (x = 0; x < total_doc_Line; x++)
                        {
                            base_doc.Lines.SetCurrentLine(x);

                            new_doc.Lines.ItemCode = base_doc.Lines.ItemCode;
                            new_doc.Lines.WarehouseCode = base_doc.Lines.WarehouseCode;
                            new_doc.Lines.Quantity = Convert.ToDouble(base_doc.Lines.RemainingOpenQuantity);
                            new_doc.Lines.BaseType = 13;
                            int docentry = base_doc.Lines.DocEntry;
                            new_doc.Lines.BaseEntry = base_doc.Lines.DocEntry;
                            new_doc.Lines.BaseLine = base_doc.Lines.LineNum;
                            new_doc.Lines.Add();
                        }

                    }*/
                    if (entregaList.ItemList != null)
                    {
                        //JArray o = new JArray { entregaList.ItemList.ToString() };
                        //int cant = o.Count;

                        //total_doc_Line_itemList = cant;
                        //int cant = entregaList.ItemList.Count;
                        //for (x = 1; x < cant; x++)
                        //{}

                        foreach (var dt in entregaList.ItemList)
                        {
                            new_doc.Lines.ItemCode = dt.ItemCode;
                            new_doc.Lines.BaseLine = dt.LineNum;
                            new_doc.Lines.Quantity = dt.Quantity;
                            new_doc.Lines.BaseEntry = entregaList.DocEntry; //base_doc_entry
                            new_doc.Lines.WarehouseCode = entregaList.WarehouseCode;
                            new_doc.Lines.BaseType = 13;
                            new_doc.Lines.Add();
                        }
                    }

                    new_doc.Comments = "Entrega por medio de la API " + DateTime.Now;

                    int aux = new_doc.Add();
                    if (aux == 0)
                    {
                        //Javascript.Alert("Entrega creada: " + oCompany.GetNewObjectKey());
                        //string cmd = "Select T0.DocNum From ODLN T0 WHERE  T0.DocEntry= '" + Convert.ToString(myCompany.GetNewObjectKey()) + "'";
                        //myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                        response= CreatedAtRoute("DefaultApi", new { id = entregaList.DocEntry }, entregaList.ItemList);
                        
                    }
                    else
                    {
                        //Javascript.Alert("Entrega error: " + oCompany.GetLastErrorDescription());
                        //myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                        response= BadRequest("Entrega error: " + oCompany.GetLastErrorDescription());
                    }
                }
                base_doc = null;
                new_doc = null;
            }
            //oCompany.Disconnect();

            //return CreatedAtRoute("DefaultApi", new { id = entregaList.DocEntry }, entregaList.ItemList);
            //oCompany.Disconnect();
            return response;
            
        }
        /*public bool Post( string req)
        {
            bool bandera = false;
            try
            {
                //conexion
                myCompany = new Company
                {
                    Server = "SBO-SAP-SYN",
                    DbServerType = BoDataServerTypes.dst_MSSQL2016,
                    CompanyDB = "PRUEBAS_SYNERGY17",
                    UserName = "manager",
                    Password = "syn123..",
                    language = BoSuppLangs.ln_Spanish_La
                };

                int error = myCompany.Connect();

                if (error == 0)
                {
                    int total_doc_Line = 0;
                    Documents base_doc = myCompany.GetBusinessObject(BoObjectTypes.oInvoices);

                    Documents new_doc = myCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                    string resp = req;
                    ItemModel itemList = JsonSerializer.Deserialize<ItemModel>(resp);
                    int base_doc_docEntry = itemList.base_doc_entry;

                    if (base_doc.GetByKey(base_doc_docEntry) == true)
                    {
                        //copia la cabecera
                        new_doc.CardCode = base_doc.CardCode;
                        new_doc.DocDate = DateTime.Now;
                        new_doc.DocDueDate = DateTime.Now;
                        //totalBase_doc_Line = base_doc.Lines.Count;
                        total_doc_Line = itemList.Item_code.Length;

                        int x = 0;

                        //entregar todo

                        for (x = 0; x < total_doc_Line; x++)
                        {
                            //base_doc.Lines.SetCurrentLine(x);

                            new_doc.Lines.WarehouseCode = itemList.WarehouseCode;
                            new_doc.Lines.BaseType = 13;
                            //new_doc.Lines.BaseEntry = base_doc.DocEntry;
                            new_doc.Lines.BaseLine = itemList.BaseLine;
                            new_doc.Lines.ItemCode = itemList.Item_code;
                            //new_doc.Lines.Quantity = Convert.ToDouble(base_doc.Lines.RemainingOpenQuantity);
                            new_doc.Lines.Quantity = itemList.Quantity;

                            //if (base_doc.Lines.LineStatus == SAPbobsCOM.BoStatus.bost_Close)
                            //{
                            //}
                            //else
                            //{
                            //    //new_doc.Lines.WarehouseCode = base_doc.Lines.WarehouseCode;
                            //    //new_doc.Lines.BaseType = 13;
                            //    //new_doc.Lines.BaseEntry = base_doc.DocEntry;
                            //    //new_doc.Lines.BaseLine = base_doc.Lines.LineNum;
                            //    //new_doc.Lines.ItemCode = base_doc.Lines.ItemCode;
                            //    //new_doc.Lines.Quantity = Convert.ToDouble(base_doc.Lines.RemainingOpenQuantity);
                                

                            //    for (int y = 0; y<= base_doc.Lines.ItemCode.Length; y++)
                            //    {
                                    
                                    
                            //        /*items.Add(new Models.ItemModel()
                            //        {
                            //            Item_code = recordset.Fields.Item("ItemCode").Value,
                            //            Item_name = recordset.Fields.Item("ItemName").Value,

                            //            new_doc.Lines.ItemCode = req => itemCode;
                            //        new_doc.Lines.Quantity = Convert.ToDouble(base_doc.Lines.RemainingOpenQuantity);
                            //        }
                                        
                            //    }
                            //}
                                new_doc.Lines.Add();

                        }

                        new_doc.Comments = "Factura de reserva entregada en su totalidad por medio de la API " + DateTime.Now;



                        int aux = new_doc.Add();
                        if (aux == 0)
                        {
                            //Javascript.Alert("Entrega creada: " + myCompany.GetNewObjectKey());
                            Javascript.Alert("Entrega creada: " + myCompany.GetNewObjectKey());
                            bandera = true;
                            //string cmd = "Select T0.DocNum From ODLN T0 WHERE  T0.DocEntry= '" + Convert.ToString(myCompany.GetNewObjectKey()) + "'";

                            //myCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            Javascript.Alert("Entrega error: " + myCompany.GetLastErrorDescription());
                            //myCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                    }
                    base_doc = null;
                    new_doc = null;
                }
                myCompany.Disconnect();
            }
            catch (Exception ex)
            {
                Javascript.Alert("Error -" + ex);
            }
            return bandera;

        }*/
    }
}