using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiV2.Models
{
    public class ItemModel
    {
        public int id { get; set; }
        public int LineNum { get; set; }
        public double Quantity { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        //public string Office { get; set; }
        //public int Stock { get; set; }
        public double OpenCreQty { get; set; } //cantidad pendiente
        //public double OpenInvQty { get; set; } //cantidad pendiente en inventario
        public double DelivrdQty { get; set; } //cantidad entregada
        //public int BaseLine { get; set; }
    }
}